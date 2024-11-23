using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour // ���� �Ŵ���, ������ �������� ����� ����
{
    public static GameManager instance;
    // �̱��� ������ ���� ����, GameManager�� �ٸ� ��ũ��Ʈ���� ����ϱ� ���� static���� ����

    private void Awake()
    {
        // �̱��� ���� ����, �ߺ� ����
        if (instance == null) // instance�� ���������
        {
            instance = this; // instance�� �ڽ��� �־���
        }
        else if (instance != this) // instance�� ������� �ʰ� �ڽ��� �ƴϸ�
        {
            Destroy(instance.gameObject); // instance�� �ı�
        }
    }
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player"); // �÷��̾� ã��, �÷��̾� ������Ʈ�� ã�� player�� ����
        
        if (DataManager.instance != null) // DataManager �ν��Ͻ��� �����Ѵٸ�
        {
            DataManager.instance.LoadCheckpoint(player);
            // DataManager�� LoadCheckpoint �Լ� ȣ��, �÷��̾� ��ġ ����
            // �÷��̾� ��ġ�� ���� -> ������ �����ߴٰ� �ٽ� �������� �� �÷��̾��� ��ġ�� �����صξ��ٰ� �ҷ����� ����
        }
    }

    void Update()
    {
        
    }

    public void Interact() // ��ȣ�ۿ� �Լ�, ��ȣ�ۿ� ������Ʈ�� ��ȣ�ۿ��� ��
    {
    }
}
