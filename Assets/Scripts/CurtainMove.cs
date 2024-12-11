using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurtainMove : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < 4f)
        {
            transform.Translate(Vector3.up * 0.75f * Time.deltaTime, Space.World);
        }
        else if (transform.position.y < 10f)
        {
            transform.Translate(Vector3.up * 0.75f * Time.deltaTime, Space.World);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(-15f, 0f, 0f), 0.3f * Time.deltaTime);
        }
        else
        {
            SceneManager.LoadScene("MainRoad_Scarecrow");
        }
    }
}
