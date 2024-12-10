using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowCtrl : MonoBehaviour
{
    private Animator _animator; // �ִϸ����� ���� ����
    public float moveSpeed = 5f; // �̵��ӵ�

    void Awake()
    {
        _animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
    }

    void Update()
    {
        float v;
        if (Input.GetKey(KeyCode.C)) // �޸��� (Shift + W Ű(���� �Է°�))
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
