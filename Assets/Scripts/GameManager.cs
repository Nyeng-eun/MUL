using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour // 게임 매니저, 게임의 전반적인 기능을 관리
{
    public static GameManager instance;
    // 싱글톤 패턴을 위한 변수, GameManager를 다른 스크립트에서 사용하기 위해 static으로 선언

    private void Awake()
    {
        // 싱글턴 패턴 구현, 중복 방지
        if (instance == null) // instance가 비어있으면
        {
            instance = this; // instance에 자신을 넣어줌
        }
        else if (instance != this) // instance가 비어있지 않고 자신이 아니면
        {
            Destroy(instance.gameObject); // instance를 파괴
        }
    }
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player"); // 플레이어 찾기, 플레이어 오브젝트를 찾아 player에 저장
        
        if (DataManager.instance != null) // DataManager 인스턴스가 존재한다면
        {
            DataManager.instance.LoadCheckpoint(player);
            // DataManager의 LoadCheckpoint 함수 호출, 플레이어 위치 복원
            // 플레이어 위치를 복원 -> 게임을 종료했다가 다시 시작했을 때 플레이어의 위치를 저장해두었다가 불러오기 위해
        }
    }

    void Update()
    {
        
    }

    public void Interact() // 상호작용 함수, 상호작용 오브젝트와 상호작용할 때
    {
    }
}
