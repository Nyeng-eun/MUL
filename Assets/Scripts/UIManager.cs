using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button GameStart;
    [SerializeField] private Button GameSetting;
    [SerializeField] private Button GameExit;

    public void OnButtonEnter(RectTransform tr)
    {
        tr.localScale = Vector3.Lerp(tr.localScale, Vector3.one * 1.2f, 2 * Time.smoothDeltaTime);
    }

    public void OnButtonExit(RectTransform tr)
    {
        tr.localScale = Vector3.Lerp(tr.localScale, Vector3.one, 2 * Time.smoothDeltaTime);
    }
}
