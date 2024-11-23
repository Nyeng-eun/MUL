using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // ���� ������� ���� ���ӽ����̽�

public class PlayerData // �÷��̾� ������ Ŭ���� (���̽����� ������� Ŭ����)
{
    public int life; // �÷��̾��� ���
    public Vector3 position; // �÷��̾��� ��ġ
    public string checkpointID; // üũ����Ʈ ID �߰�
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance; 
    // �̱��� ������ ���� ����, DataManager�� �ٸ� ��ũ��Ʈ���� ����ϱ� ���� static���� ����

    PlayerData nowPlayer = new PlayerData(); // ���� �÷��̾� ������

    string path; // ���� ���
    string fileName = "save"; // ������ ���� �̸�

    private void Awake()
    {
        if (instance == null) // instance�� ���������
        {
            instance = this; // instance�� �ڽ��� �־���
        }
        else if (instance != this) // instance�� ������� �ʰ� �ڽ��� �ƴϸ�
        {
            Destroy(instance.gameObject); // instance�� �ı�
        }
        DontDestroyOnLoad(this.gameObject); // ���� �ٲ� �ı����� �ʰ� ��

        path = Application.persistentDataPath + "/";
        // ���� ���
        // Application.persistentDataPath : �ش� �÷������� ����� �� �ִ� �������� ������ ���� ��θ� ��ȯ��
    }

    void Start()
    {
        Load(); // ���̺� ������ �ε�
        GameObject player = GameObject.FindWithTag("Player"); // �÷��̾� ã��
        // LoadCheckpoint(player); // �÷��̾� ��ġ ���� -> GameManager���� �����ϵ��� ����
    }

    public void Save()
    {
        string data = JsonUtility.ToJson(nowPlayer); // �÷��̾� �����͸� json �������� ��ȯ (���ڿ��� ��ȯ)
        File.WriteAllText(path + fileName, data);
        // ���Ͽ� ����
        // File.WriteAllText : ���Ͽ� �ؽ�Ʈ�� �� (������ ������ ����(path)�ϰ�, ������ ������ ���(path + fileName))
    }

    public void Load()
    {
        if (File.Exists(path + fileName))
        // ������ �����ϸ�
        // File.Exists : ������ �����ϴ��� Ȯ��
        {
            string data = File.ReadAllText(path + fileName); // ���Ͽ��� �ؽ�Ʈ�� ����
            nowPlayer = JsonUtility.FromJson<PlayerData>(data);
            // json ������ �ؽ�Ʈ�� �÷��̾� �����ͷ� ��ȯ
            // JsonUtility.FromJson : json ������ �ؽ�Ʈ�� ��ü�� ��ȯ
        }
        else // ������ ������ (ó������)
        {
            Debug.LogWarning("���̺� ������ �����ϴ�.");
            nowPlayer.life = 4; // �⺻ ��� ���� (4����)
            nowPlayer.position = Vector3.zero; // ���� ��ġ ���� (���� ������ġ�� ���� zero�� ������)
        }
    }

    public void SaveCheckpoint(Vector3 checkpointPosition, int playerLife, string checkpointID)
    // üũ����Ʈ ���� �Լ�
    // üũ����Ʈ ��ġ�� �÷��̾� ���, üũ����Ʈ ID�� �޾Ƽ� ������
    {
        nowPlayer.position = checkpointPosition; // �÷��̾� ��ġ ������Ʈ
        nowPlayer.life = playerLife; // �÷��̾� ��� ������Ʈ
        nowPlayer.checkpointID = checkpointID; // üũ����Ʈ ID ����
        Save(); // �÷��̾� ������ ����
        Debug.Log("üũ����Ʈ {checkpointID} ���� �Ϸ�"); 
        // üũ����Ʈ �����, {checkpointID} : checkpointID �� ���
    }

    public void LoadCheckpoint(GameObject player)
    // üũ����Ʈ �ε� �Լ�
    // �÷��̾� ���� ������Ʈ�� �޾Ƽ� ��ġ�� ������
    {
        if (player != null) // �÷��̾ �����ϸ�
        {
            player.transform.position = nowPlayer.position; // �÷��̾� ��ġ ����
            PlayerMove playerScript = player.GetComponent<PlayerMove>(); // �÷��̾� ��ũ��Ʈ ��������

            if (playerScript != null) // �÷��̾� ��ũ��Ʈ�� �����ϸ�
            {
                playerScript.life = nowPlayer.life; // �÷��̾� ��� ����
            }
            Debug.Log("üũ����Ʈ {nowPlayer.checkpointID} �ε� �Ϸ�");
        }
    }
}
