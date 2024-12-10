using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour // ���� �Ŵ���, ������ �������� ����� ����
{
    public static GameManager instance = null; // ��� ��ũ��Ʈ���� ���� �����ϵ��� ���� ����(static)�� ����
    [SerializeField] public PlayerMove _playerCtrl = null;
    [SerializeField] public bool isFirst = false;

    void Awake()
    {
        // �̱��� ����(�ϳ��� Ŭ������ �����ϵ��� �ߺ� ����)
        if (instance != null) Destroy(gameObject); // instance�� �̹� ������ ������Ʈ �ı�

        instance = this; // instance�� �ڽ����� ����
        DontDestroyOnLoad(gameObject); // �� ��ȯ�� �������� �ʵ��� ����
    }

    void Start()
    {
        _playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>(); // �÷��̾� ã��, �÷��̾� ������Ʈ�� ã�� player�� ����
        
        if (DataManager.instance != null) // DataManager �ν��Ͻ��� �����Ѵٸ�
        {
            DataManager.instance.LoadCheckpoint(_playerCtrl.gameObject);
            // DataManager�� LoadCheckpoint �Լ� ȣ��, �÷��̾� ��ġ ����
            // �÷��̾� ��ġ�� ���� -> ������ �����ߴٰ� �ٽ� �������� �� �÷��̾��� ��ġ�� �����صξ��ٰ� �ҷ����� ����
        }
    }

    private void Interact() // ��ȣ�ۿ� �Լ�
    {

    }
}
