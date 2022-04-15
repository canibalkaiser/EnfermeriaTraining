using UnityEngine;
using System.Collections.Generic;
using Obi;
using System.Collections;

/**
 * Sample component that makes a collider "grab" any particle it touches (regardless of which Actor it belongs to).
 */

public class ObiContactGrabber : MonoBehaviour
{
    public bool testOneUse;
    public bool grabAtTheStart;
    public bool release;
    public ObiSolver[] solvers = { };
    public GameObject prefabGrapa;

    public ObiSolver solverP;
    public int indexP;
    public bool thisIsAP;
    public ObiContactGrabber preScript;

    public int grapperCount;

    public bool sphereBlack;
    public ObiRope rope;

    public bool grabbed
    {
        get { return grabbedActors.Count > 0; }
    }

    /**
     * Helper class that stores the index of a particle in the solver, its position in the grabber's local space, and its inverse mass previous to being grabbed.
     * This makes it easy to tell if a particle has been grabbed, update its position while grabbing, and restore its mass after being released.
     */
    public class GrabbedParticle : IEqualityComparer<GrabbedParticle>
    {
        public int index;
        public float invMass;
        public Vector3 localPosition;
        public ObiSolver solver;

        public GrabbedParticle(ObiSolver solver, int index, float invMass)
        {
            this.solver = solver;
            this.index = index;
            this.invMass = invMass;
        }

        public bool Equals(GrabbedParticle x, GrabbedParticle y)
        {
            return x.index == y.index;
        }

        public int GetHashCode(GrabbedParticle obj)
        {
            return index;
        }
    }

    public Dictionary<ObiSolver, ObiSolver.ObiCollisionEventArgs> collisionEvents = new Dictionary<ObiSolver, ObiSolver.ObiCollisionEventArgs>();                                 /**< store the current collision event*/
    public ObiCollider localCollider;                                                           /**< the collider on this gameObject.*/
    public HashSet<GrabbedParticle> grabbedParticles = new HashSet<GrabbedParticle>();          /**< set to store all currently grabbed particles.*/
    public HashSet<ObiActor> grabbedActors = new HashSet<ObiActor>();                           /**< set of softbodies grabbed during this step.*/

    public void Awake()
    {
        localCollider = GetComponent<ObiCollider>();
    }

    public void OnEnable()
    {
        if (solvers != null)
            foreach (ObiSolver solver in solvers)
                solver.OnCollision += Solver_OnCollision;
    }

    public void OnDisable()
    {
        if (solvers != null)
            foreach (ObiSolver solver in solvers)
                solver.OnCollision -= Solver_OnCollision;
    }

    public void Solver_OnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
    {
        collisionEvents[(ObiSolver)sender] = e;
    }

    public void UpdateParticleProperties()
    {
        // Update rest shape matching of all grabbed softbodies:
        foreach (ObiActor actor in grabbedActors)
        {
            actor.UpdateParticleProperties();
        }
    }

    /**
     * Creates and stores a GrabbedParticle from the particle at the given index.
     * Returns true if we sucessfully grabbed a particle, false if the particle was already grabbed.
     */
    public bool GrabParticle(ObiSolver solver, int index)
    {
        GrabbedParticle p = new GrabbedParticle(solver, index, solver.invMasses[index]);

        // in case this particle has not been grabbed yet:
        if (!grabbedParticles.Contains(p))
        {
            Matrix4x4 solver2Grabber = transform.worldToLocalMatrix * solver.transform.localToWorldMatrix;

            // record the particle's position relative to the grabber, and store it.
            p.localPosition = solver2Grabber.MultiplyPoint3x4(solver.positions[index]);
            grabbedParticles.Add(p);

            // Set inv mass and velocity to zero:
            solver.invMasses[index] = 0;
            solver.velocities[index] = Vector4.zero;
            grapperCount++;
            if (prefabGrapa && grapperCount == 2)
            {
                GameObject pre = Instantiate(prefabGrapa, new Vector3(0, 0, 0), Quaternion.identity);
                pre.transform.eulerAngles = new Vector3(0, 87.75101f, 0);
                preScript = pre.GetComponent<ObiContactGrabber>();
                preScript.solverP = solver;
                preScript.indexP = index;
                Invoke(nameof(ResetGrapperCount), 1);

            }
            //if (sphereBlack) rope.tearingEnabled = true;

            return true;
        }
        return false;
    }

    private void ResetGrapperCount()
    {
        grapperCount = 0;
    }


    /**
     * Grabs all particles currently touching the grabber.
     */
    public void Grab()
    {
        Release();

        var world = ObiColliderWorld.GetInstance();

        if (solvers != null && collisionEvents != null)
        {
            foreach (ObiSolver solver in solvers)
            {
                ObiSolver.ObiCollisionEventArgs collisionEvent;
                if (collisionEvents.TryGetValue(solver, out collisionEvent))
                {
                    foreach (Oni.Contact contact in collisionEvent.contacts)
                    {
                        // this one is an actual collision:
                        if (contact.distance < 0.01f)
                        {
                            var contactCollider = world.colliderHandles[contact.bodyB].owner;
                            int particleIndex = solver.simplices[contact.bodyA];

                            // if the current contact references our collider, proceed to grab the particle.
                            if (contactCollider == localCollider)
                            {
                                // try to grab the particle, if not already grabbed.
                                if (GrabParticle(solver, particleIndex))
                                    grabbedActors.Add(solver.particleToActor[particleIndex].actor);
                            }

                        }
                    }
                }
            }
        }

        UpdateParticleProperties();
    }

    /**
     * Releases all currently grabbed particles. This boils down to simply resetting their invMass.
     */
    public void Release()
    {
        // Restore the inverse mass of all grabbed particles, so dynamics affect them.
        foreach (GrabbedParticle p in grabbedParticles)
            p.solver.invMasses[p.index] = p.invMass;

        UpdateParticleProperties();
        grabbedActors.Clear();
        grabbedParticles.Clear();

        if (preScript) preScript.thisIsAP = true;
    }

    /**
     * Updates the position of the grabbed particles.
     */
    public void FixedUpdate()
    {
        foreach (GrabbedParticle p in grabbedParticles)
        {
            Matrix4x4 grabber2Solver = p.solver.transform.worldToLocalMatrix * transform.localToWorldMatrix;
            p.solver.positions[p.index] = grabber2Solver.MultiplyPoint3x4(p.localPosition);
        }

        if (testOneUse)
        {
            testOneUse = false;
            Grab();
        }

        if (release)
        {
            release = false;
            Release();
        }

        if (thisIsAP) this.gameObject.transform.position = solverP.positions[indexP];
    }

    public void Start()
    {
        if (grabAtTheStart)
        {
            Invoke(nameof(GrabAtTheStartTimer), 0.01f);
        }
    }

    public void GrabAtTheStartTimer()
    {
        Grab();
    }
}
