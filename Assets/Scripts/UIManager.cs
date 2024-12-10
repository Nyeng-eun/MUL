using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text dialogueName;
    public Text dialogueText;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
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
}
