using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayerMoveController : MonoBehaviour
{

    public void ReverseCard() {
        this.transform.GetChild(0).transform.GetChild(7).gameObject.SetActive(false);
    }

    public void EndAnimation() {
        this.transform.GetChild(0).transform.SetParent(this.transform.parent.Find("PlayerZone"));
        int child = this.transform.parent.Find("PlayerZone").childCount;
        this.transform.parent.Find("PlayerZone").GetChild(child-1).SetAsFirstSibling();
        Destroy(this.gameObject);
    }
}


