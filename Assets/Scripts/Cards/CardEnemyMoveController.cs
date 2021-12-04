using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEnemyMoveController : MonoBehaviour
{
    GameObject enemySlots;

    public void EndInitialAnimation()
    {
        this.transform.GetChild(0).transform.SetParent(this.transform.parent.Find("EnemyZone"));
        Destroy(this.gameObject);
    }

    public void ReverseCardEnemy()
    {
        transform.GetChild(0).transform.GetChild(7).gameObject.SetActive(false);
    }

    public void EndPlayCardAnimation()
    {
        enemySlots = transform.parent.Find("EnemySlots").gameObject;
        transform.GetChild(0).transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
        Destroy(enemySlots.transform.GetChild(transform.parent.GetComponent<BattleStats>().enemySlotIndex).gameObject);
        transform.GetChild(0).SetParent(enemySlots.transform, true);
        enemySlots.transform.GetChild(enemySlots.transform.childCount - 1).SetSiblingIndex(transform.parent.GetComponent<BattleStats>().enemySlotIndex);
        transform.parent.GetComponent<BattleStats>().enemySlotIndex++;
        Destroy(transform.gameObject);
        Time.timeScale = 1f;
    }
}
