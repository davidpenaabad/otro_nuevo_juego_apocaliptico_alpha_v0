using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class BattleStats : MonoBehaviour
{
    Character activeCharacter;
    Enemy activeEnemy;
    public string battleKind;

    public Image playerBar, enemyBar;
    public Sprite aimBarPlayerSprite, defenseBarPlayerSprite, aimBarEnemySprite, defenseBarEnemySprite;
    public Text scorePlayer, scoreEnemy;

    public GameObject deckPlayerZone, deckEnemyZone;
    public GameObject slotPrefab;
    public GameObject playerSlots, enemySlots;
    public GameObject discardPanel;
    public GameObject playerZone;

    public int emptyPlayerSlots, emptyEnemySlots;
    public int fullPlayerSlots, fullEnemySlots;

    double value;
    int modifierPlayer = 1;
    int modifierEnemy = 1;
    public bool discardActive = false;
    public int discardAmount = 0;

    public int enemySlotIndex;
    bool endEnemyTurn = false;

    int burnedPlayerCardCount, burnedEnemyCardCount;
    public Text burnedDeckPlayerLabel, burnedDeckEnemyLabel;


    void Awake()
    {
        activeCharacter = GetComponent<DrawCards>().activeCharacter.character;
        activeEnemy = GetComponent<DrawCards>().activeEnemy.enemy;
        emptyPlayerSlots = activeCharacter.hand;
        emptyEnemySlots = activeEnemy.hand;

        burnedPlayerCardCount = transform.GetComponent<DrawCards>().activeCharacter.burnedDeck.cards.Count;
        burnedDeckPlayerLabel.text = burnedPlayerCardCount.ToString();
        burnedEnemyCardCount = transform.GetComponent<DrawCards>().activeEnemy.burnedDeck.cards.Count;
        burnedDeckEnemyLabel.text = burnedEnemyCardCount.ToString();

        battleKind = "aim";

        if (battleKind == "aim")
        {
            emptyPlayerSlots = activeCharacter.aimBaseSlots;
            emptyEnemySlots = activeEnemy.defenseBaseSlots;
            playerBar.sprite = aimBarPlayerSprite;
            enemyBar.sprite = defenseBarEnemySprite;
            StartCoroutine(SetScore(activeCharacter.aimBasePercentage, scorePlayer, playerBar));
            StartCoroutine(SetScore(activeEnemy.defenseBasePercentage, scoreEnemy, enemyBar));
            StartCoroutine(ShowSlots(playerSlots, activeCharacter.aimBaseSlots));
            StartCoroutine(ShowSlots(enemySlots, activeEnemy.defenseBaseSlots));
        }
        else if(battleKind == "defense")
        {
            emptyPlayerSlots = activeCharacter.defenseBaseSlots;
            emptyEnemySlots = activeEnemy.aimBaseSlots;
            playerBar.sprite = defenseBarPlayerSprite;
            enemyBar.sprite = aimBarEnemySprite;
            StartCoroutine(SetScore(activeCharacter.defenseBasePercentage, scorePlayer, playerBar));
            StartCoroutine(SetScore(activeEnemy.aimBasePercentage, scoreEnemy, enemyBar));
            StartCoroutine(ShowSlots(playerSlots, activeCharacter.defenseBaseSlots));
            StartCoroutine(ShowSlots(enemySlots, activeEnemy.aimBaseSlots));
        }

        fullPlayerSlots = 0;
        fullEnemySlots = 0;
        enemySlotIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator CardAction(Stack messageText)
    {
        string ID = messageText.Pop().ToString();
        int level = (int)messageText.Pop();
        char index = ID[0];

        #region EFECTO DE LAS CARTAS DE HARRY
        if (index.Equals('H'))
        {
            switch (ID)
            {
                case "H0001":
                    string tempID = "";
                    int tempLevel = 1;
                    Stack tempMessageText = new Stack();

                    for (int i = 0; i < playerSlots.transform.childCount; i++)
                    {
                        try
                        {
                            if (playerSlots.transform.GetChild(i) != null && playerSlots.transform.GetChild(i).GetComponent<GameObjectCardData>() != null && playerSlots.transform.GetChild(i).GetComponent<GameObjectCardData>().ID == ID)
                            {
                                if (playerSlots.transform.GetChild(i - 1) != null && (i - 1) >= 0)
                                {
                                    tempID = playerSlots.transform.GetChild(i - 1).GetComponent<GameObjectCardData>().ID;
                                    tempLevel = playerSlots.transform.GetChild(i - 1).GetComponent<GameObjectCardData>().level;
                                    tempMessageText.Push(tempLevel);
                                    tempMessageText.Push(tempID);
                                }
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }

                    if (tempID != null && tempMessageText.Count > 0)
                    {
                        StartCoroutine(CardAction(tempMessageText)); //ESTBLECER ESTAMENTO IF SI HARRY USA ARMA CORTA
                    }
                    break;

                case "H0002":
                    ModifyScore(activeCharacter.aimBasePercentage, 10, scorePlayer, playerBar, modifierPlayer, "player");
                    break;

                case "H0003":
                    ModifyScore(activeCharacter.aimBasePercentage, 5, scorePlayer, playerBar, modifierPlayer, "player");
                    break;

                case "H0004":
                    ResetParameters("player");
                    modifierPlayer = 2;
                    break;

                case "H0005":
                    ResetParameters("player");
                    activeCharacter.damageDices.Add("d4");//IMPLEMENTAR AÑADIR DOS DADOS EN CASO DE QUE HARRY EMPLEE UN ARMA CORTA
                    break;

                case "H0006":
                    string cardGramatical = "carta";
                    discardActive = true;

                    if (level > 1)
                    {
                        cardGramatical += "s";
                    }

                    string message = string.Format("Haz Click en <b>{0}</b> {1} de tu mano para descartar.", level, cardGramatical);
                    discardPanel.transform.GetChild(1).GetComponent<Text>().text = message;

                    discardPanel.gameObject.SetActive(true);
                    playerSlots.GetComponent<Collider2D>().enabled = false;


                    while (discardAmount < level)
                    {
                        yield return null;
                    }

                    discardPanel.gameObject.SetActive(false);

                    for (int i = 0; i < level * 2; i++)
                    {
                        StartCoroutine(transform.GetComponent<DrawCards>().FlipCard(deckPlayerZone.transform.GetChild(0).gameObject, "player"));
                        yield return new WaitForSeconds(0.085f * level + 1);
                    }

                    ModifyScore(activeCharacter.aimBasePercentage, 2, scorePlayer, playerBar, modifierPlayer, "player");

                    playerSlots.GetComponent<Collider2D>().enabled = true;

                    discardAmount = 0;
                    discardActive = false;

                    RefreshBurnedLabel("player");

                    break;

                case "H0007":
                    if (emptyEnemySlots > 0)
                    {
                        Destroy(enemySlots.transform.GetChild(enemySlots.transform.childCount - 1).gameObject);
                        emptyEnemySlots--;
                    }

                    break;

                case "H0008":
                    ModifyScore(activeCharacter.defenseBasePercentage, 5, scorePlayer, playerBar, modifierPlayer, "player");

                    break;

                case "H0009":
                    int effectTimes = 2 * modifierPlayer;
                    for (int i = 0; i < effectTimes; i++)
                    {
                        activeCharacter.effects.Add("Resilencia");
                    }

                    WriteCharacter(activeCharacter, GetComponent<MainData>().fileCharacterPath);

                    ResetParameters("player");

                    break;

                case "H0010":
                    bool modifyParameters = false;
                    if (battleKind == "aim" && activeCharacter.aimBaseSlots > 0)
                    {
                        activeCharacter.aimBaseSlots -= 1;
                        activeCharacter.effects.Add("Cansado");
                        modifyParameters = true;
                    }
                    else if (battleKind == "defense" && activeCharacter.defenseBaseSlots > 0)
                    {
                        activeCharacter.defenseBaseSlots -= 1;
                        activeCharacter.effects.Add("Herido");
                        modifyParameters = true;
                    }
                    else if (battleKind == "harvest" && activeCharacter.harvestBaseSlots > 0)
                    {
                        activeCharacter.harvestBaseSlots -= 1;
                        activeCharacter.effects.Add("Sin suerte");
                        modifyParameters = true;
                    }

                    if (modifyParameters)
                    {
                        WriteCharacter(activeCharacter, GetComponent<MainData>().fileCharacterPath);
                    }

                    SceneManager.LoadScene("BattleScene");

                    break;

                default: break;
            }
            #endregion
            emptyPlayerSlots--;
            fullPlayerSlots++;
            for (int i = playerZone.transform.childCount; i < activeCharacter.hand; i++)
            {
                StartCoroutine(transform.GetComponent<DrawCards>().FlipCard(deckPlayerZone.transform.GetChild(0).gameObject, "player"));
            }

            while (playerZone.transform.childCount != activeCharacter.hand)
            {
                yield return new WaitForSeconds(1f);
            }
        }
        #region EFECTO DE LAS CARTAS DEL DEAMBULADOR
        else if (index.Equals('D'))
        {
            switch (ID)
            {
                case "D0001":
                    ModifyScore(activeEnemy.aimBasePercentage, 5, scoreEnemy, enemyBar, modifierEnemy, "enemy");
                    break;

                case "D0002":
                    activeEnemy.effects.Add("Duro");
                    WriteEnemy(activeEnemy, transform.GetComponent<MainData>().fileEnemyPath);
                    break;

                case "D0003":
                    discardActive = true;

                    int consumeCard = Random.Range(0, playerZone.transform.childCount - 1);
                    playerZone.transform.GetChild(consumeCard).GetComponent<CardInteraction>().OnClick();
                    Time.timeScale = 0f;
                    RefreshBurnedLabel("player");
                    discardActive = false;
                    break;

                case "D0004":
                    if (battleKind == "aim")
                    {
                        ModifyScore(activeEnemy.aimBasePercentage, 5, scoreEnemy, enemyBar, modifierEnemy, "enemy");
                        ModifyScore(activeCharacter.defenseBasePercentage, -5, scorePlayer, playerBar, modifierEnemy, "enemy");
                    }
                    else
                    {
                        ModifyScore(activeCharacter.aimBasePercentage, -5, scorePlayer, playerBar, modifierEnemy, "enemy");
                    }

                    break;

                case "D0005":
                    string tempPath = transform.GetComponent<MainData>().fileScenePath;
                    transform.GetComponent<MainData>().activeScene.noise = transform.GetComponent<MainData>().activeScene.noise + modifierEnemy;
                    string tempJSONString = JsonUtility.ToJson(transform.GetComponent<MainData>().activeScene);
                    File.WriteAllText(tempPath, tempJSONString);

                    ResetParameters("enemy");

                    break;

                case "D0006":
                    ModifyScore(activeEnemy.aimBasePercentage, 3, scoreEnemy, enemyBar, modifierEnemy, "enemy");
                    if (emptyPlayerSlots > 0)
                    {
                        Destroy(playerSlots.transform.GetChild(playerSlots.transform.childCount - 1).gameObject);
                        emptyPlayerSlots--;
                    }

                    break;

                case "D0007":
                    ModifyScore(activeEnemy.defenseBasePercentage, 5, scoreEnemy, enemyBar, modifierEnemy, "enemy");
                    
                    break;

                case "D0008":
                    discardActive = true;

                    for (int i = 1; i <= 2; i++)
                    {
                        GameObject tempCardObject = deckPlayerZone.transform.GetChild(deckPlayerZone.transform.childCount - i).gameObject;
                        tempCardObject.GetComponent<CardInteraction>().isActive = true;
                        tempCardObject.GetComponent<CardInteraction>().OnClick();
                        Time.timeScale = 0f;
                        RefreshBurnedLabel("player");
                    }

                    discardActive = false;

                    break;

                case "D0009":
                    double tempCardModify = activeCharacter.defenseBasePercentage - activeEnemy.aimBasePercentage;

                    if (tempCardModify < 0)
                    {
                        tempCardModify *= -1;
                    }
                    else if (tempCardModify == 0)
                    {
                        tempCardModify = 1;
                    }

                    ModifyScore(activeEnemy.aimBasePercentage, tempCardModify, scoreEnemy, enemyBar, modifierEnemy, "enemy");
                    break;

                case "D0010":
                    int burnedCardsCount;

                    if(burnedPlayerCardCount > 0)
                    {
                        burnedCardsCount = burnedPlayerCardCount;
                    }
                    else
                    {
                        burnedCardsCount = 1;
                    }

                    if(battleKind == "aim")
                    {
                        ModifyScore(activeEnemy.defenseBasePercentage, burnedCardsCount, scoreEnemy, enemyBar, modifierEnemy, "enemy");
                    }
                    else if(battleKind == "defense")
                    {
                        ModifyScore(activeEnemy.aimBasePercentage, burnedCardsCount, scoreEnemy, enemyBar, modifierEnemy, "enemy");
                    }
                    break;

                default: break;
            }
            #endregion
            endEnemyTurn = true;
            emptyEnemySlots--;
            fullEnemySlots++;
        }

        transform.GetComponent<BattleTurnManager>().ChangeTurn();
        if (endEnemyTurn)
        {
            StartCoroutine(transform.GetComponent<DrawCards>().FlipCard(deckEnemyZone.transform.GetChild(0).gameObject, "enemy"));
            endEnemyTurn = false;
        }
    }

    public void SetBar(double fillAmount, Image bar) {
            bar.GetComponent<Image>().fillAmount = (float)fillAmount/100;
    }

    IEnumerator ShowSlots(GameObject slotsZone, int slotNumber) {
        for (int i = 0; i < slotNumber; i++)
        {
            string slotNumberText = (i + 1).ToString();
            slotPrefab = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
            slotPrefab.transform.GetChild(1).GetComponent<Text>().text = "Slot " + slotNumberText;
            slotPrefab.transform.SetParent(slotsZone.transform, false);
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator SetScore(double scoreIncrement, Text score, Image bar)
    {
        double initialScore = double.Parse(score.text);
        double scoreTemp = double.Parse(score.text);
        double incrementValue;

        if (initialScore != 0)
        {
            incrementValue = 0.025;
        }
        else
        {
            incrementValue = 1;
        }

        while (scoreTemp < (initialScore + scoreIncrement))
        {
            scoreTemp += incrementValue;
            score.text = scoreTemp.ToString();
            SetBar(scoreTemp, bar);
            yield return new WaitForSeconds(0f);
        }

        scoreTemp = System.Math.Round((float)scoreTemp, 2);

        if(initialScore != 0)
        {
            if(scoreIncrement > 10)
            {
                scoreTemp -= 0.03;
            }
            else
            {
                scoreTemp -= 0.02;
            }
            
        }
        score.text = scoreTemp.ToString();
    }

    void WriteCharacter(Character character, string path)
    {
        CharacterList tempCharacterList = new CharacterList();
        tempCharacterList.characters = new List<Character>();
        tempCharacterList.characters.Add(character);

        string tempJSONString = JsonUtility.ToJson(tempCharacterList);

        File.WriteAllText(path, tempJSONString);
    }

    void WriteEnemy(Enemy enemy, string path)
    {
        EnemyList tempEnemyList = new EnemyList();
        tempEnemyList.enemies = new List<Enemy>();
        tempEnemyList.enemies.Add(enemy);

        string tempJSONString = JsonUtility.ToJson(tempEnemyList);

        File.WriteAllText(path, tempJSONString);
    }

    void ModifyScore(double characterModify, double cardModify, Text score, Image bar, int modifier, string who)
    {
        value = (characterModify * cardModify / 100.00) * modifier;
        StartCoroutine(SetScore(value, score, bar));
        ResetParameters(who);
    }


    void ResetParameters(string who)
    {
        value = 0;
        if(who == "player")
        {
            modifierPlayer = 1;
        }
        else if(who == "enemy")
        {
            modifierEnemy = 1;
        }
        
    }

    void RefreshBurnedLabel(string who)
    {
        if(who == "player")
        {
            burnedPlayerCardCount++;
            burnedDeckPlayerLabel.text = burnedPlayerCardCount.ToString();
        }
        else if(who == "enemy")
        {
            burnedEnemyCardCount++;
            burnedDeckEnemyLabel.text = burnedEnemyCardCount.ToString();
        }
    }
}
