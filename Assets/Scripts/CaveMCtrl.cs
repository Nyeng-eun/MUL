using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CaveMCtrl : MonoBehaviour
{
    public PlayerMove c_playerMove;
    private NavMeshAgent positionAgent; // 네비게이션
    private Transform target; // 가야하는 위치
    private GameObject c_Player; // 플레이어
    public GameObject stun_Particle; // 사자 스킬 사용 시 몬스터 스턴 파티클 효과
    private Animator C_ani;

    private float pushPower = 5.0f; // 플레이어를 밀어내는 힘
    private float maxium = float.MinValue; // 최대값 찾기

    private bool hasReachedT = false; // 위치 도달 유무
    public bool isStunned = false; // 스턴 유무


    // Start is called before the first frame update
    void Start()
    {
        c_Player = GameObject.FindWithTag("Player");
        positionAgent = GetComponent<NavMeshAgent>();
        C_ani = GetComponent<Animator>();
        c_playerMove = c_Player.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {

        C_ani.SetBool("c_Walk", !isStunned);

        if (isStunned) // 스턴 시
        {
            positionAgent.isStopped = true; // NavMeshAgent 멈춤
        }
        else
        {
            positionAgent.isStopped = false; // NavMeshAgent 다시 동작
        }

        if (c_playerMove.Is_LionSK_corutine && Vector3.Distance(transform.position, c_playerMove.transform.position) < c_playerMove.L_skRange)
        {
            if (c_playerMove.Is_LionSK) // 사자 스킬을 쓸 시 그 즉시에만 동작
            {
                StartCoroutine(lion_Stun());
            }
        }

        if (positionAgent.remainingDistance <= positionAgent.stoppingDistance)
        {
            hasReachedT = true; // 가야할 위치에 도달
        }

        if (hasReachedT || target == null) // 새로운 위치 찾는 조건
        {
            hasReachedT = false; // 초기화
            FindNewPosition(); // 새로운 위치 찾기
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player") // 플레이어에게 돌진 성공 시
        {
            c_Player.GetComponent<Rigidbody>().AddForce(transform.forward * pushPower, ForceMode.Impulse); // 플레이어 밀어내기
        }
    }

    void FindNewPosition()
    {
        if (!isStunned)
        {
            GameObject[] sameTag = GameObject.FindGameObjectsWithTag(this.tag); // 자기자신과 같은 tag 찾아내기

            maxium = float.MinValue;

            foreach (GameObject position in sameTag)
            {
                if (position.GetInstanceID() == gameObject.GetInstanceID()) // 자기자신 배제
                {
                    continue;
                }
                float distance = Vector3.Distance(transform.position, position.transform.position); // 자기자신과 position의 거리

                if (distance > maxium)
                {
                    maxium = distance;
                    target = position.transform;
                }
            }
            positionAgent.SetDestination(target.position); // position 찾아가기
        }
    }

    IEnumerator lion_Stun()
    {
        isStunned = true;
        GameObject stunObj = Instantiate(stun_Particle); // 파티클 게임오브젝트 생성

        Vector3 pos = this.transform.position;
        stunObj.transform.position = pos; // 몬스터 위치에 파티클 생성
        yield return new WaitForSeconds(3.0f);

        Destroy(stunObj); // 파티클 삭제
        isStunned = false;
    }
}
