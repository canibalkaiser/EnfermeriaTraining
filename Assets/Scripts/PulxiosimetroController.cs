using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulxiosimetroController : MonoBehaviour
{
    [Header("Required Components")]
    public GameObject pulxiosimetroInTheBody;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Pulsioximetro"))
        {
            Destroy(other.gameObject.transform.root.gameObject);
            pulxiosimetroInTheBody.SetActive(true);
            Destroy(this.gameObject);
        }
        else print(other.gameObject.name);
    }
}