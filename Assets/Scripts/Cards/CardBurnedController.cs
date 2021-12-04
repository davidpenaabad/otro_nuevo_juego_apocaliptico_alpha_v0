using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBurnedController : MonoBehaviour
{
    public void EndAnimationBurnedCard()
    {
        transform.GetChild(0).transform.SetParent(transform.parent.Find("DiscardZone").transform.GetChild(0).transform);
        transform.parent.GetComponent<EffectsController>().burnedCard = true;
        Destroy(this.gameObject);
    }
}
