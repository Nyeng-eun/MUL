using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void GameStart()
    {
        if (DataManager.instance.isExist())
        {
            SceneManager.LoadScene("MainRoad_Scarecrow");
        }
        else
        {
            SceneManager.LoadScene("FirstCutScene");
        }
    }
}
