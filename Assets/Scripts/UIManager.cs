using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject titleSet;
    public GameObject dialogue;
    public Text dialogueName;
    public Text dialogueText;
    public GameObject Interact;
    public GameObject lifeGroup;
    public Image[] lifes;
    public GameObject EndingGroup;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void interact(string name) // 물체와 상호작용
    {
        switch (name)
        {
            case "ScareCrow":
                TalkData[] scareDialog1 = Reader.Read("scarecrow.txt");
                StartCoroutine(ShowDialogue(scareDialog1, 1));
                break;
            case "Bell":
                GameManager.instance.crowBattle = false;
                SceneManager.LoadScene("MainRoad_ScareCrow2");
                instance.lifeGroup.SetActive(false);

                EndingGroup.SetActive(true);
                // TalkData[] scareDialog2 = Reader.Read("scarecrow2.txt");
                // StartCoroutine(ShowDialogue(scareDialog2));
                break;
        }
    }

    public IEnumerator ShowDialogue(TalkData[] dialogues, int index = 0)
    {
        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogue.SetActive(true);
            dialogueName.text = dialogues[i].CharacterName;
            dialogueText.text = dialogues[i].Dialogue;
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }
        // 대사 끝나고 난 뒤
        switch(index)
        {
            case 0:
                dialogue.SetActive(false);
                break;
            case 1:
                dialogue.SetActive(false);
                SceneManager.LoadScene("Battle_Crow");
                GameManager.instance.crowBattle = true;
                lifeGroup.SetActive(true);
                break;
        }

        yield return null;
    }

    public void GameStart()
    {
        if (DataManager.instance.isExist())
        {
            SceneManager.LoadScene("MainRoad_Scarecrow");
            titleSet.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene("FirstCutScene");
            titleSet.SetActive(false);
        }
    }

    public void GameExit()
    {
        Application.Quit();
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
