using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour // 게임 매니저, 게임의 전반적인 기능을 관리
{
    public static GameManager instance = null; // 모든 스크립트에서 접근 가능하도록 정적 변수(static)로 선언
    
    [SerializeField] private GameObject[] spawnPoints;//적 배열 및 생성 위치 배열 변수 선언
    [SerializeField] private int e_num; // 마녀 까마귀
    [SerializeField] private GameObject[] e_types; // 적 종류 배열
    [HideInInspector] public PlayerMove _playerCtrl = null;

    private float maxSpawnDelay = 3f;
    public float curSpawnDelay = 0f;  //적 생성 딜레이 변수 선언
    public bool crowBattle = false;
    public bool isCrowattacked = false; // 마녀 까마귀 소환
    public bool w_isFirstSpawn = true; // 마녀 까마귀 첫 번째 소환
    public bool ba_isFirstSpawn = true; // 까마귀 배틀씬 첫 번째 소환

    void Awake()
    {
        // 싱글턴 패턴(하나의 클래스만 존재하도록 중복 방지)
        if (instance != null) Destroy(gameObject); // instance가 이미 있으면 오브젝트 파괴

        instance = this; // instance를 자신으로 지정
        DontDestroyOnLoad(gameObject); // 씬 전환시 없어지지 않도록 설정
    }

    void Start()
    {
        if (DataManager.instance != null) // DataManager 인스턴스가 존재한다면
        {
            DataManager.instance.LoadCheckpoint(_playerCtrl.gameObject);
            // DataManager의 LoadCheckpoint 함수 호출, 플레이어 위치 복원
            // 플레이어 위치를 복원 -> 게임을 종료했다가 다시 시작했을 때 플레이어의 위치를 저장해두었다가 불러오기 위해
        }
    }

    void Update()
    {
        if (_playerCtrl == null)
        {
            _playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>(); // 플레이어 찾기, 플레이어 오브젝트를 찾아 player에 저장
        }

        if (crowBattle) {
            if (ba_isFirstSpawn)
            {
                maxSpawnDelay = 0.5f;
                ba_isFirstSpawn = false; // // 다음부턴 maxSpawnDelay = 0.5f; 부분이 작동하지 x
            }
            e_num = 0;
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            curSpawnDelay += Time.deltaTime; //지금 흐르고 있는 시간
            if (curSpawnDelay > maxSpawnDelay)
            {
                SpawnEnemy(e_types[e_num]);
                maxSpawnDelay = Random.Range(1f, 2f); //정해진 범위 내의 랜덤 숫자 반환 (float, int)
                curSpawnDelay = 0f; //적 생성 후 딜레이 변수 0으로 초기화
            }
        }
        else if (isCrowattacked) {

            if (w_isFirstSpawn)
            {
                maxSpawnDelay = 0.5f;
                w_isFirstSpawn = false; // 다음부턴 maxSpawnDelay = 0.5f; 부분이 작동하지 x
            }
            e_num = 0;
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            isCrowattacked = false;
            curSpawnDelay += Time.deltaTime; //지금 흐르고 있는 시간
            if (curSpawnDelay > maxSpawnDelay)
            {
                SpawnEnemy(e_types[e_num]);
                maxSpawnDelay = Random.Range(2f, 4f); //정해진 범위 내의 랜덤 숫자 반환 (float, int)
                curSpawnDelay = 0f; //적 생성 후 딜레이 변수 0으로 초기화
            }
        }
    }

    private void SpawnEnemy(GameObject enemy) // 적 소환 함수
    {
        int randPoint = Random.Range(0, spawnPoints.Length); // 랜덤 소환
        Instantiate(enemy, spawnPoints[randPoint].transform.position, spawnPoints[randPoint].transform.rotation);
    }
}

