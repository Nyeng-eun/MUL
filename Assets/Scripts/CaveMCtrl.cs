using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CaveMCtrl : MonoBehaviour
{
    public PlayerMove c_playerMove;
    private NavMeshAgent positionAgent; // �׺���̼�
    private Transform target; // �����ϴ� ��ġ
    private GameObject c_Player; // �÷��̾�
    public GameObject stun_Particle; // ���� ��ų ��� �� ���� ���� ��ƼŬ ȿ��
    private Animator C_ani;

    private float pushPower = 5.0f; // �÷��̾ �о�� ��
    private float maxium = float.MinValue; // �ִ밪 ã��

    private bool hasReachedT = false; // ��ġ ���� ����
    public bool isStunned = false; // ���� ����


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

        if (isStunned) // ���� ��
        {
            positionAgent.isStopped = true; // NavMeshAgent ����
        }
        else
        {
            positionAgent.isStopped = false; // NavMeshAgent �ٽ� ����
        }

        if (c_playerMove.Is_LionSK_corutine && Vector3.Distance(transform.position, c_playerMove.transform.position) < c_playerMove.L_skRange)
        {
            if (c_playerMove.Is_LionSK) // ���� ��ų�� �� �� �� ��ÿ��� ����
            {
                StartCoroutine(lion_Stun());
            }
        }

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
        if (!isStunned)
        {
            GameObject[] sameTag = GameObject.FindGameObjectsWithTag(this.tag); // �ڱ��ڽŰ� ���� tag ã�Ƴ���

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

    IEnumerator lion_Stun()
    {
        isStunned = true;
        GameObject stunObj = Instantiate(stun_Particle); // ��ƼŬ ���ӿ�����Ʈ ����

        Vector3 pos = this.transform.position;
        stunObj.transform.position = pos; // ���� ��ġ�� ��ƼŬ ����
        yield return new WaitForSeconds(3.0f);

        Destroy(stunObj); // ��ƼŬ ����
        isStunned = false;
    }
}
