using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TurnAI : MonoBehaviour
{
    public GameObject enemyZone;
    public Canvas mainCanvas;
    public GameObject cardConvoyEnemyPrefab;
    GameObject card;
    char enemyIndex;

    public void PlayEnemyCard()
    {
        GameObject cardConvoyEnemy;
        enemyIndex = transform.GetComponent<DrawCards>().activeEnemy.enemy.name[0];
        if (transform.GetComponent<BattleStats>().battleKind == "aim")
        {
            if(enemyIndex == 'D')
            {
                DeambuladorAI("aim");
            }
        }
        
        cardConvoyEnemy = Instantiate(cardConvoyEnemyPrefab, enemyZone.GetComponent<Transform>().position, Quaternion.identity);
        card.transform.SetParent(cardConvoyEnemy.transform, true);
        cardConvoyEnemy.transform.SetParent(mainCanvas.transform, false);
        Time.timeScale = 0f;


        Stack messageText = new Stack();
        messageText.Push(card.GetComponent<GameObjectCardData>().level);
        messageText.Push(card.GetComponent<GameObjectCardData>().ID);

        SendMessage("CardAction", messageText);
    }

    void DeambuladorAI(string battleKind)
    {
        if (battleKind == "aim")
        {
            for (int i = 0; i < enemyZone.transform.childCount; i++)
            {
                if (transform.GetComponent<BattleStats>().enemySlots.transform.childCount - transform.GetComponent<BattleStats>().fullEnemySlots > 2)
                {

                    if (enemyZone.transform.GetChild(i).GetComponent<GameObjectCardData>().ID == "D0008")
                    {
                        card = enemyZone.transform.GetChild(i).gameObject;
                        break;
                    }
                    else if (enemyZone.transform.GetChild(i).GetComponent<GameObjectCardData>().ID == "D0003")
                    {
                        card = enemyZone.transform.GetChild(i).gameObject;
                        break;
                    }
                    else if (enemyZone.transform.GetChild(i).GetComponent<GameObjectCardData>().ID == "D0002")
                    {
                        card = enemyZone.transform.GetChild(i).gameObject;
                        break;
                    }
                    else if (enemyZone.transform.GetChild(i).GetComponent<GameObjectCardData>().ID == "D0007")
                    {
                        card = enemyZone.transform.GetChild(i).gameObject;
                        break;
                    }
                    else if (enemyZone.transform.GetChild(i).GetComponent<GameObjectCardData>().ID == "D0004")
                    {
                        card = enemyZone.transform.GetChild(i).gameObject;
                        break;
                    }
                    else if (enemyZone.transform.GetChild(i).GetComponent<GameObjectCardData>().ID == "D0005")
                    {
                        card = enemyZone.transform.GetChild(i).gameObject;
                        break;
                    }
                }
                //else
                //{
                //    card = enemyZone.transform.GetChild(0).gameObject;
                //}
            }
        }
    }
}


