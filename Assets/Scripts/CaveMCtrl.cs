using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CaveMCtrl : MonoBehaviour
{
    public NavMeshAgent positionAgent;
    private Transform target;
    private bool hasReachedT = false;
    private float maxium = float.MinValue;
    private GameObject c_Player;
    private Animator C_ani;

    private float pushPower = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        c_Player = GameObject.FindWithTag("Player");
        positionAgent = GetComponent<NavMeshAgent>();
        C_ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        GameObject[] sameTag = GameObject.FindGameObjectsWithTag(this.tag);

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
