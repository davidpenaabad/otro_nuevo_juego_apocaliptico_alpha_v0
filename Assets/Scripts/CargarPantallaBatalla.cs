using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CargarPantallaBatalla : MonoBehaviour
{
    public void LoadBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
