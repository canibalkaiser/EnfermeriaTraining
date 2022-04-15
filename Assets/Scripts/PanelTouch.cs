using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTouch : MonoBehaviour
{
    private bool cooldown;
    private GameObject partEnterOfTheHand;

    public UnityEngine.Events.UnityEvent EnterEvent;
    public UnityEngine.Events.UnityEvent ExitEvent;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand") && partEnterOfTheHand == null && !cooldown && other.gameObject.name == "hands_coll:b_r_index3" || other.gameObject.name == "hands_coll:b_l_index3")
        {
            EnterEvent.Invoke();
            partEnterOfTheHand = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand") && partEnterOfTheHand != null && other.gameObject.name == "hands_coll:b_r_index3" || other.gameObject.name == "hands_coll:b_l_index3")
        {
            cooldown = true;
            ExitEvent.Invoke();
            partEnterOfTheHand = null;
            Invoke(nameof(ResetCooldown), 0.05f);
        }

    }

    private void ResetCooldown()
    {
        cooldown = false;
    }
}
