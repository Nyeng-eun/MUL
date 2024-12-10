using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowCtrl : MonoBehaviour
{
    private Animator _animator; // 애니메이터 변수 선언
    public float moveSpeed = 5f; // 이동속도

    void Awake()
    {
        _animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
    }

    void Update()
    {
        float v;
        if (Input.GetKey(KeyCode.C)) // 달리기 (Shift + W 키(수직 입력값))
        {
            v = 1f;
            _animator.SetFloat("V", v, 0.1f, Time.deltaTime);
            Vector3 moveDir = Vector3.forward * _animator.GetFloat("V");
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            v = 0f;
        }
    }
}
