using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseraController : MonoBehaviour
{
    [Header("Required Components")]
    public GameObject pulseraFalsa;
    public SkinnedMeshRenderer pulseraFalsaSkinned;

    [Header("Configurable Variables")]
    public float animationSpeed;

    [Header("InGame Variables")]
    public float animationSlicerNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Pulsera"))
        {
            Destroy(other.gameObject);
            pulseraFalsa.SetActive(true);
            Invoke(nameof(Animation), animationSpeed);
        }
    }

    private void Animation()
    {
        pulseraFalsaSkinned.SetBlendShapeWeight(0, animationSlicerNumber);
        animationSlicerNumber++;
        if(animationSlicerNumber < 110) Invoke(nameof(Animation), animationSpeed);

    }
}
