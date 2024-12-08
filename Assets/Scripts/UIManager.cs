using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button GameStart;
    [SerializeField] private Button GameSetting;
    [SerializeField] private Button GameExit;
    private bool isEnter = false;
    private RectTransform rectTr = null;

    void Update()
    {
        if (isEnter && rectTr)
        {
            rectTr.localScale = Vector3.Lerp(rectTr.localScale, new Vector3(1.5f, 1.5f, 1f), Time.smoothDeltaTime);
        }
        else if (rectTr)
        {
            rectTr.localScale = Vector3.Lerp(rectTr.localScale, Vector3.one, Time.smoothDeltaTime);
            if (rectTr.localScale == Vector3.one) rectTr = null;
        }
    }

    public void OnButtonEnter(RectTransform tr)
    {
        isEnter = true;
        rectTr = tr;
    }

    public void OnButtonExit(RectTransform tr)
    {
        isEnter = false;
    }
}
