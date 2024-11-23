using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // 파일 입출력을 위한 네임스페이스

public class PlayerData // 플레이어 데이터 클래스 (제이슨으로 만들어줄 클래스)
{
    public int life; // 플레이어의 목숨
    public Vector3 position; // 플레이어의 위치
    public string checkpointID; // 체크포인트 ID 추가
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance; 
    // 싱글톤 패턴을 위한 변수, DataManager를 다른 스크립트에서 사용하기 위해 static으로 선언

    PlayerData nowPlayer = new PlayerData(); // 현재 플레이어 데이터

    string path; // 저장 경로
    string fileName = "save"; // 저장할 파일 이름

    private void Awake()
    {
        if (instance == null) // instance가 비어있으면
        {
            instance = this; // instance에 자신을 넣어줌
        }
        else if (instance != this) // instance가 비어있지 않고 자신이 아니면
        {
            Destroy(instance.gameObject); // instance를 파괴
        }
        DontDestroyOnLoad(this.gameObject); // 씬이 바뀌어도 파괴되지 않게 함

        path = Application.persistentDataPath + "/";
        // 저장 경로
        // Application.persistentDataPath : 해당 플랫폼에서 사용할 수 있는 영구적인 데이터 저장 경로를 반환함
    }

    void Start()
    {
        Load(); // 세이브 데이터 로드
        GameObject player = GameObject.FindWithTag("Player"); // 플레이어 찾기
        // LoadCheckpoint(player); // 플레이어 위치 복원 -> GameManager에서 관리하도록 변경
    }

    public void Save()
    {
        string data = JsonUtility.ToJson(nowPlayer); // 플레이어 데이터를 json 형식으로 변환 (문자열로 변환)
        File.WriteAllText(path + fileName, data);
        // 파일에 저장
        // File.WriteAllText : 파일에 텍스트를 씀 (파일이 없으면 생성(path)하고, 파일이 있으면 덮어씀(path + fileName))
    }

    public void Load()
    {
        if (File.Exists(path + fileName))
        // 파일이 존재하면
        // File.Exists : 파일이 존재하는지 확인
        {
            string data = File.ReadAllText(path + fileName); // 파일에서 텍스트를 읽음
            nowPlayer = JsonUtility.FromJson<PlayerData>(data);
            // json 형식의 텍스트를 플레이어 데이터로 변환
            // JsonUtility.FromJson : json 형식의 텍스트를 객체로 변환
        }
        else // 파일이 없으면 (처음시작)
        {
            Debug.LogWarning("세이브 파일이 없습니다.");
            nowPlayer.life = 4; // 기본 목숨 설정 (4개로)
            nowPlayer.position = Vector3.zero; // 시작 위치 설정 (아직 시작위치를 몰라서 zero로 지정함)
        }
    }

    public void SaveCheckpoint(Vector3 checkpointPosition, int playerLife, string checkpointID)
    // 체크포인트 저장 함수
    // 체크포인트 위치와 플레이어 목숨, 체크포인트 ID를 받아서 저장함
    {
        nowPlayer.position = checkpointPosition; // 플레이어 위치 업데이트
        nowPlayer.life = playerLife; // 플레이어 목숨 업데이트
        nowPlayer.checkpointID = checkpointID; // 체크포인트 ID 저장
        Save(); // 플레이어 데이터 저장
        Debug.Log("체크포인트 {checkpointID} 저장 완료"); 
        // 체크포인트 저장됨, {checkpointID} : checkpointID 값 출력
    }

    public void LoadCheckpoint(GameObject player)
    // 체크포인트 로드 함수
    // 플레이어 게임 오브젝트를 받아서 위치를 복원함
    {
        if (player != null) // 플레이어가 존재하면
        {
            player.transform.position = nowPlayer.position; // 플레이어 위치 복원
            PlayerMove playerScript = player.GetComponent<PlayerMove>(); // 플레이어 스크립트 가져오기

            if (playerScript != null) // 플레이어 스크립트가 존재하면
            {
                playerScript.life = nowPlayer.life; // 플레이어 목숨 복원
            }
            Debug.Log("체크포인트 {nowPlayer.checkpointID} 로드 완료");
        }
    }
}
