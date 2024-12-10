using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform targerTr; // 플레이어 Transform 변수
    public Vector3 lookat;
    public float dist; // 카메라 와 플레이어 간의 거리
    public float height; //  카메라와 플레이어 간의 높이
    public float damping; // 반응 속도

    void Start()
    {
        damping = 5f;
    }

    void LateUpdate() // 모든 Update 함수가 실행 된 후 작동
    {
        transform.position = Vector3.Slerp // 선형보간 사용 (균일한 속도로 이동, 회전할 때 사용)
            (transform.position
            // 카메라 시작위치 - Main Camera 위치
            , targerTr.position - (targerTr.forward * dist) + (Vector3.up * height)
            // 카메라 종료 위치 - 플레이어 위치 (플레이어 뒤, 높이3, 거리8 위치)
            , Time.deltaTime * damping);
        // 보간 시간 (trace 값에 따라 추적 감도를 조정할 수 있음)
        transform.LookAt(targerTr.position + lookat); // 카메라를 플레이어를 바라보도록 고정 (회전)
    }
}
