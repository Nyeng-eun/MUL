using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform targerTr; // �÷��̾� Transform ����
    public float dist = 8.0f; // ī�޶� �� �÷��̾� ���� �Ÿ�
    public float height = 5.0f; //  ī�޶�� �÷��̾� ���� ����
    public float damping = 5.0f; // ���� �ӵ�
    private Transform tr; // Main Camera �ڽ��� Transform ����

    void Start()
    {
        tr = GetComponent<Transform>(); // Main Camera �� Transform ������Ʈ �Ҵ�
    }

    void LateUpdate() // ��� Update �Լ��� ���� �� �� �۵�
    {
        tr.position = Vector3.Lerp // �������� ��� (������ �ӵ��� �̵�, ȸ���� �� ���)
            (tr.position
            // ī�޶� ������ġ - Main Camera ��ġ
            , targerTr.position - (targerTr.forward * dist) + (Vector3.up * height)
            // ī�޶� ���� ��ġ - �÷��̾� ��ġ (�÷��̾� ��, ����3, �Ÿ�8 ��ġ)
            , Time.deltaTime * damping);
        // ���� �ð� (trace ���� ���� ���� ������ ������ �� ����)
        tr.LookAt(targerTr.position); // ī�޶� �÷��̾ �ٶ󺸵��� ���� (ȸ��)
    }
}
