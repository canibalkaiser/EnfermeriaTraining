using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloves : MonoBehaviour
{
    [Header("Required Components")]
    public Material gloveMaterial;
    public Renderer handR;
    public Renderer handL;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.StartsWith("coll_hands"))
        {
            handR.material = gloveMaterial;
            handL.material = gloveMaterial;
            Destroy(this.gameObject);
        }
    }

}
