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
            hasReachedT = true; // ������ ��ġ�� ����
        }

        if (hasReachedT || target == null) // ���ο� ��ġ ã�� ����
        {
            hasReachedT = false; // �ʱ�ȭ
            FindNewPosition(); // ���ο� ��ġ ã��
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player") // �÷��̾�� ���� ���� ��
        {
            c_Player.GetComponent<Rigidbody>().AddForce(transform.forward * pushPower, ForceMode.Impulse); // �÷��̾� �о��
        }
    }

    void FindNewPosition()
    {
        GameObject[] sameTag = GameObject.FindGameObjectsWithTag(this.tag);

        maxium = float.MinValue;

        foreach (GameObject position in sameTag)
        {
            if (position.GetInstanceID() == gameObject.GetInstanceID()) // �ڱ��ڽ� ����
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, position.transform.position); // �ڱ��ڽŰ� position�� �Ÿ�

            if (distance > maxium)
            {
                maxium = distance;
                target = position.transform;
            }
        }

        positionAgent.SetDestination(target.position); // position ã�ư���
    }
}
