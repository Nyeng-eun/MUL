using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour // ���� �Ŵ���, ������ �������� ����� ����
{
    public static GameManager instance = null; // ��� ��ũ��Ʈ���� ���� �����ϵ��� ���� ����(static)�� ����
    [SerializeField] public PlayerMove _playerCtrl = null;
    [SerializeField] public bool isFirst = false;

    public Transform[] spawnPoints;//�� �迭 �� ���� ��ġ �迭 ���� ����
    public GameObject b_crow; // ���� ���

    public float maxSpawnDelay;
    public float couSpawnDelay;  //�� ���� ������ ���� ����

    public bool isCrowattacked = false; // ���� ��� ��ȯ

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

    private void Update()
    {
        couSpawnDelay += Time.deltaTime; //���� �帣�� �ִ� �ð�

        if (isCrowattacked)
        {
            if (couSpawnDelay > maxSpawnDelay)
            {
                SpawnCrow();
                maxSpawnDelay = Random.Range(0.5f, 3f); //������ ���� ���� ���� ���� ��ȯ (float, int)
                couSpawnDelay = 0; //�� ���� �� ������ ���� 0���� �ʱ�ȭ
            }
        }
    }

    void SpawnCrow() // ���� ��� ��ȯ �Լ�
    {
        int ranPoint = Random.Range(0, 3); // ���� ��ȯ
        Instantiate(b_crow, spawnPoints[ranPoint].position, Quaternion.identity);
    }
}
