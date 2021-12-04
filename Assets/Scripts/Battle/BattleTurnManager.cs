using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleTurnManager : MonoBehaviour
{
    string battleKind;
    public bool playerTurn;
    public GameObject playerZone, enemyZone, deckPlayerZone;
    public bool ready = false;
    public bool endProcess = true;


    // Start is called before the first frame update
    void Start()
    {
        battleKind = GetComponent<BattleStats>().battleKind;

        if(battleKind == "aim")
        {
            playerTurn = true;
        }
        else
        {
            playerTurn = false;
        }
    }

    private void Update()
    {
        if(enemyZone.transform.childCount == transform.GetComponent<DrawCards>().activeEnemy.enemy.hand)
        {
            ready = true;
        }

        if (playerTurn == true)
        {

            for (int i = 0; i < playerZone.transform.childCount; i++)
            {
                playerZone.transform.GetChild(i).GetComponent<EventTrigger>().enabled = true;
                playerZone.transform.GetChild(i).GetComponent<CardInteraction>().isActive = true;
            }
        }
        else if(!playerTurn)
        {
            for (int i = 0; i < playerZone.transform.childCount; i++)
            {
                playerZone.transform.GetChild(i).GetComponent<EventTrigger>().enabled = false;
                playerZone.transform.GetChild(i).GetComponent<CardInteraction>().isActive = false;
            }
            if (ready)
            {
                transform.GetComponent<TurnAI>().PlayEnemyCard();
            }
        }
    }

    // Update is called once per frame

    public void ChangeTurn()
    {
        if (playerTurn == true)
        {
            playerTurn = false;
        }
        else if(playerTurn == false)
        {
            playerTurn = true;
        }
    }

    IEnumerator WaitingProcess()
    {
        yield return new WaitForEndOfFrame();
    }
}
