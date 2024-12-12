using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Image dialogue;
    public Text dialogueName;
    public Text dialogueText;
    public GameObject lifeGroup;
    public Image[] lifes;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowDialogue(string name, string text)
    {
        dialogueName.text = name;
        dialogueText.text = text;
    }

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

    public void LifeUpdate(int maxLife, int life, bool maxUpdate)
    {
        for (int i = 0; i < lifes.Length; i++)
        {
            if (maxUpdate)
            {
                if (i < maxLife)
                {
                    lifes[i].enabled = true;
                }
                else
                {
                    lifes[i].enabled = false;
                }
            }

            if (i < life)
            {
                lifes[i].color = Color.white;
            }
            else
            {
                lifes[i].color = Color.black;
            }
        }
    }
}
