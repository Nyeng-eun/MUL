using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public string checkpointID;
    // üũ����Ʈ ���� ID
    // �ν����� â���� üũ����Ʈ�� ID�� ��������
    // üũ����Ʈ�� ID�� ���� � üũ����Ʈ���� ����

    private void OnTriggerEnter(Collider other)
    // Ʈ���� �浹�� ���۵Ǿ��� �� ȣ��Ǵ� �Լ�
    // other : �浹�� ������ �ݶ��̴�
    {
        if (other.CompareTag("Player"))
        // �浹�� ������ �±װ� "Player"���
        // CompareTag : �±׸� ���ϴ� �Լ�
        {
            PlayerMove player = other.GetComponent<PlayerMove>(); // �÷��̾� ��ũ��Ʈ ��������
            if (player != null) // �÷��̾� ��ũ��Ʈ�� �����Ѵٸ�
            {
                if (DataManager.instance != null) // DataManager �ν��Ͻ��� �����Ѵٸ�
                {
                    DataManager.instance.SaveCheckpoint(transform.position, player.life, checkpointID);
                    // DataManager�� SaveCheckpoint �Լ� ȣ��
                    // �÷��̾��� ��ġ, ���, üũ����Ʈ ID�� ����
                    Debug.Log("üũ����Ʈ {checkpointID} ����"); // {checkpointID} : checkpointID ��
                }
                else // DataManager �ν��Ͻ��� �������� �ʴ´ٸ�
                {
                    Debug.LogError("DataManager �ν��Ͻ� ����");
                }
            }
            else // �÷��̾� ��ũ��Ʈ�� �������� �ʴ´ٸ�
            {
                Debug.LogError("PlayerMove ��ũ��Ʈ ����");
            }
        }
    }
}
