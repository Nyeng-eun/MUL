using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour // 게임 매니저, 게임의 전반적인 기능을 관리
{
    public static GameManager instance = null; // 모든 스크립트에서 접근 가능하도록 정적 변수(static)로 선언
    [SerializeField] public PlayerMove _playerCtrl = null;
    [SerializeField] public bool isFirst = false;

    public Transform[] spawnPoints;//적 배열 및 생성 위치 배열 변수 선언
    public GameObject b_crow; // 마녀 까마귀

    public float maxSpawnDelay;
    public float couSpawnDelay;  //적 생성 딜레이 변수 선언

    public bool isCrowattacked = false; // 마녀 까마귀 소환

    void Awake()
    {
        // 싱글턴 패턴(하나의 클래스만 존재하도록 중복 방지)
        if (instance != null) Destroy(gameObject); // instance가 이미 있으면 오브젝트 파괴

        instance = this; // instance를 자신으로 지정
        DontDestroyOnLoad(gameObject); // 씬 전환시 없어지지 않도록 설정
    }

    void Start()
    {
        _playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>(); // 플레이어 찾기, 플레이어 오브젝트를 찾아 player에 저장
        
        if (DataManager.instance != null) // DataManager 인스턴스가 존재한다면
        {
            DataManager.instance.LoadCheckpoint(_playerCtrl.gameObject);
            // DataManager의 LoadCheckpoint 함수 호출, 플레이어 위치 복원
            // 플레이어 위치를 복원 -> 게임을 종료했다가 다시 시작했을 때 플레이어의 위치를 저장해두었다가 불러오기 위해
        }
    }

    private void Interact() // 상호작용 함수
    {

    }

    private void Update()
    {
        couSpawnDelay += Time.deltaTime; //지금 흐르고 있는 시간

        if (isCrowattacked)
        {
            if (couSpawnDelay > maxSpawnDelay)
            {
                SpawnCrow();
                maxSpawnDelay = Random.Range(0.5f, 3f); //정해진 범위 내의 랜덤 숫자 반환 (float, int)
                couSpawnDelay = 0; //적 생성 후 딜레이 변수 0으로 초기화
            }
        }
    }

    void SpawnCrow() // 마녀 까마귀 소환 함수
    {
        int ranPoint = Random.Range(0, 3); // 랜덤 소환
        Instantiate(b_crow, spawnPoints[ranPoint].position, Quaternion.identity);
    }
}
