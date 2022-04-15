using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazaleteController : MonoBehaviour
{
    [Header("Required Components")]
    public GameObject brazaleteFalsa;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Brazalete"))
        {
            Destroy(other.gameObject);
            brazaleteFalsa.SetActive(true);
        }
        else print(other.gameObject.name);
    }

}
