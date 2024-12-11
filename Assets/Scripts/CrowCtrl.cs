using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowCtrl : MonoBehaviour
{
    private Animator _animator; // 애니메이터 변수 선언
    public float moveSpeed = 5f; // 이동속도
    public float rotSpeed = 1000f; // 회전속도
    private int isJumping = 0;

    void Awake()
    {
        _animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
    }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse ScrollWheel") * rotSpeed * Time.deltaTime, 0);
        
        if (Input.GetKey(KeyCode.Space))
        {
            _animator.SetBool("isJumped", true);
        }
        else
        {
            _animator.SetBool("isJumped", false);
        }

        if (isJumping == 1)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void SetJump(int j)
    {
        isJumping = j;
    }
}