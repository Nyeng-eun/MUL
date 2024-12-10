using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform targerTr; // �÷��̾� Transform ����
    public Vector3 lookat;
    public float dist; // ī�޶� �� �÷��̾� ���� �Ÿ�
    public float height; //  ī�޶�� �÷��̾� ���� ����
    public float damping; // ���� �ӵ�

    void Start()
    {
        damping = 5f;
    }

    void LateUpdate() // ��� Update �Լ��� ���� �� �� �۵�
    {
        transform.position = Vector3.Slerp // �������� ��� (������ �ӵ��� �̵�, ȸ���� �� ���)
            (transform.position
            // ī�޶� ������ġ - Main Camera ��ġ
            , targerTr.position - (targerTr.forward * dist) + (Vector3.up * height)
            // ī�޶� ���� ��ġ - �÷��̾� ��ġ (�÷��̾� ��, ����3, �Ÿ�8 ��ġ)
            , Time.deltaTime * damping);
        // ���� �ð� (trace ���� ���� ���� ������ ������ �� ����)
        transform.LookAt(targerTr.position + lookat); // ī�޶� �÷��̾ �ٶ󺸵��� ���� (ȸ��)
    }
}
