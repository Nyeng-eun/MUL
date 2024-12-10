using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utils
{
    public static void interact(string name) // 물체와 상호작용
    {
        switch (name)
        {
            case "Scarecrow":
                UIManager.Instance.ShowDialogue("허수아비", "저기요..! 저 좀 도와주세요! 까마귀들이 괴롭히고 있어요.");
                SceneManager.LoadScene("Battle_Crow");
                break;
            case "Bell":
                SceneManager.LoadScene("MainRoad_ScareCrow2");
                break;
        }
    }
}
