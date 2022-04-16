using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.ControllerInput;

public class LavaboController : MonoBehaviour
{
    [Header("Required Components")]
    public SkinnedMeshRenderer meshRightHand;
    public SkinnedMeshRenderer meshLeftHand;
    public HVRTrackedController rightVelocity;
    public HVRTrackedController leftVelocity;
    public Animator animatorHands;
    public BoxCollider thisCollider;
    public GameObject water;

    [Header("Configurable Variables")]
    public float velocityRequired;

    [Header("InGame Variables")]
    public int hands;

    private void Start()
    {
        animatorHands.speed = 0;
        //animatorHands.SetBool("AnimationCanRun", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.EndsWith("b_r_middle1"))
        {
            hands++;
            if(hands == 2)
            {
                meshRightHand.enabled = false;
                meshLeftHand.enabled = false;
                CheckVelocity();
                animatorHands.speed = 1;
                animatorHands.gameObject.SetActive(true);
                thisCollider.size = new Vector3(2, 2, 2);
                water.SetActive(true);
            }
        }
        else if(other.gameObject.name.EndsWith("b_l_middle1"))
        {
            hands++;
            if (hands == 2)
            {
                meshRightHand.enabled = false;
                meshLeftHand.enabled = false;
                CheckVelocity();
                animatorHands.speed = 1;
                animatorHands.gameObject.SetActive(true);
                thisCollider.size = new Vector3(2, 2, 2);
                water.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.EndsWith("b_r_middle1") || other.gameObject.name.EndsWith("b_l_middle1"))
        {
            if(hands == 2)
            {
                meshRightHand.enabled = true;
                meshLeftHand.enabled = true;
                animatorHands.speed = 0;
                animatorHands.gameObject.SetActive(false);
                thisCollider.size = new Vector3(1, 1, 1);
                water.SetActive(false);
            }
            hands--;
            CancelInvoke(nameof(CheckVelocity));
        }
    }

    private void CheckVelocity()
    {
        if(rightVelocity.VelocityMagnitude > velocityRequired && leftVelocity.VelocityMagnitude > velocityRequired)
        {
            meshRightHand.enabled = false;
            meshLeftHand.enabled = false;
            animatorHands.speed = 1;
            animatorHands.gameObject.SetActive(true);
            thisCollider.size = new Vector3(2, 2, 2);
            water.SetActive(true);
        }
        else
        {
            meshRightHand.enabled = true;
            meshLeftHand.enabled = true;
            animatorHands.speed = 0;
            animatorHands.gameObject.SetActive(false);
            thisCollider.size = new Vector3(1, 1, 1);
            water.SetActive(false);
        }

        if(hands == 2)Invoke(nameof(CheckVelocity), 0.1f);
    } 
}
