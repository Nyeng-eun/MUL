using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public string checkpointID;
    // 체크포인트 고유 ID
    // 인스펙터 창에서 체크포인트의 ID를 설정가능
    // 체크포인트의 ID를 통해 어떤 체크포인트인지 구분

    private void OnTriggerEnter(Collider other)
    // 트리거 충돌이 시작되었을 때 호출되는 함수
    // other : 충돌한 상대방쪽 콜라이더
    {
        if (other.CompareTag("Player"))
        // 충돌한 상대방의 태그가 "Player"라면
        // CompareTag : 태그를 비교하는 함수
        {
            PlayerMove player = other.GetComponent<PlayerMove>(); // 플레이어 스크립트 가져오기
            if (player != null) // 플레이어 스크립트가 존재한다면
            {
                if (DataManager.instance != null) // DataManager 인스턴스가 존재한다면
                {
                    DataManager.instance.SaveCheckpoint(transform.position, player.life, checkpointID);
                    // DataManager의 SaveCheckpoint 함수 호출
                    // 플레이어의 위치, 목숨, 체크포인트 ID를 저장
                    Debug.Log("체크포인트 {checkpointID} 저장"); // {checkpointID} : checkpointID 값
                }
                else // DataManager 인스턴스가 존재하지 않는다면
                {
                    Debug.LogError("DataManager 인스턴스 없음");
                }
            }
            else // 플레이어 스크립트가 존재하지 않는다면
            {
                Debug.LogError("PlayerMove 스크립트 없음");
            }
        }
    }
}
