using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class MonsterCtrl : MonoBehaviour
{
    public int e_Type; // 0 = ��� 1 = ���� 2 = �� 3 = ������
    private PlayerMove _playerMove;
    private GameObject player; // �÷��̾�
    private Animator M_ani; // ���� �ִϸ��̼�
    private int hp; // ü��
    private float e_Speed; // �⺻ �ӵ�
    private float attackRange; // ���� �Ÿ�
    private float attackSpeed; // ���� �ӵ�
    private float pushPower = 0f; // �������� ��

    private DecalProjector projector;

    private bool isDash = false;
    private bool isCooldown = false;
    private Rigidbody rb;

    public NavMeshAgent positionAgent;
    private float maxium = float.MinValue;
    private Transform target;
    private bool hasReachedT = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player"); // �÷��̾� ã��
        _playerMove = player.GetComponent<PlayerMove>();
        M_ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        positionAgent = GetComponent<NavMeshAgent>();

        switch (e_Type)
        {
            case 0:
                e_Speed = 3.5f;
                hp = 1;
                pushPower = 3.0f;
                break;

            case 1:
                e_Speed = 4.0f;
                hp = 3;
                attackRange = 6.0f;
                attackSpeed = 7.0f;
                break;

            case 2:
                e_Speed = 3.0f;
                attackRange = 7.0f;
                attackSpeed = 9.0f;
                break;

            case 3:
                pushPower = 3.0f;
                break;
        }
    }

    void Update()
    {
        if (_playerMove.Is_On_corutine && Vector3.Distance(transform.position, _playerMove.transform.position) < _playerMove.skRange)
        {
            switch (e_Type)
            {
                case 0:
                    transform.Translate(transform.forward * e_Speed * Time.deltaTime, Space.Self);
                    break;

                case 1:
                    if (hp > 1)
                    {
                        // ���� ���� �ִϸ��̼�
                    }
                    else
                    {
                        // �ȳ����輼��! ������! ����
                    }
                    break;

                case 2:
                    return;

                case 3:
                    return;
            }
        }
        else
        {
            Vector3 moveDir = (player.transform.position - transform.position).normalized; // ���� ����
            float Distance = Vector3.Distance(transform.position, player.transform.position); // ���Ϳ� �÷��̾���� �Ÿ�
            switch (e_Type)
            {
                case 0: // ��� (�÷��̾ �Ѿƴٴ�)
                    transform.Translate(moveDir * e_Speed * Time.deltaTime, Space.World);
                    transform.LookAt(player.transform.position);
                    break;

                case 1: // ���� (�÷��̾�� ����)
                    moveDir.y = 0f; // y�� ����
                    if (Distance <= attackRange && !isDash && !isCooldown)
                    {
                        isDash = true;
                        StartCoroutine(w_Dash(moveDir));
                    }
                    else if (!isDash)
                    {
                        M_ani.SetBool("w_Walk", true);
                        transform.Translate(moveDir * e_Speed * Time.deltaTime, Space.World);

                        if (moveDir != Vector3.zero)
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                            transform.rotation = targetRotation;
                        }
                    }
                    break;

                case 2: // ��
                    moveDir.y = 0f; // y�� ����
                    if (Distance <= attackRange && !isDash && !isCooldown)
                    {
                        isDash = true;
                        StartCoroutine(g_Dash(moveDir));

                    }
                    else if (!isDash)
                    {
                        M_ani.SetBool("g_Walk", true);
                        transform.Translate(moveDir * e_Speed * Time.deltaTime, Space.World);

                        if (moveDir != Vector3.zero)
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                            transform.rotation = targetRotation;
                        }
                    }
                    break;

                case 3: // �̷� ������
                    if (positionAgent.remainingDistance <= positionAgent.stoppingDistance)
                    // remainingDistance: ���� ��ġ���� ��ǥ �������� ���� �ִ� �Ÿ�
                    // stoppingDistance: ��ǥ ������ �������� �� �����ؾ� �ϴ� �Ÿ�
                    {
                        hasReachedT = true; // ������ ��ġ�� ����
                    }

                    if (hasReachedT || target == null) // hasReachedT == true �̰ų� target == null �� ��
                    {
                        hasReachedT = false;
                        FindNewPosition(); // ���ο� ��ġ ã��
                    }

                    break;

            }
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player") // �÷��̾�� ���� ���� ��
        {
            player.GetComponent<Rigidbody>().AddForce(transform.forward * pushPower, ForceMode.Impulse); // �÷��̾� �о��
        }
    }

    void FindNewPosition()
    {
        GameObject[] sameTag = GameObject.FindGameObjectsWithTag(this.tag);

        maxium = float.MinValue; // �ּҰ��� ������ �ִ밪 ã��

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

        positionAgent.SetDestination(target.position); // Ÿ�� ��ġ�� ����
    }

    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(3.0f);
        rb.velocity = Vector3.zero;
        isDash = false; // ���� ����
        isCooldown = true; // ��ٿ� ����

        yield return new WaitForSeconds(1.0f); // ��ٿ� �ð�
        isCooldown = false; // ��ٿ� ����
    }

    IEnumerator w_Dash(Vector3 moveDir) // ���� ����
    {
        M_ani.SetBool("w_Walk", false);
        projector.fadeFactor = 1.0f; // ���� ���� ǥ��
        yield return new WaitForSeconds(0.5f);

        projector.fadeFactor = 0f; // ���� ���� ǥ�� x

        M_ani.SetBool("w_Attack", true);
        rb.AddForce(moveDir * attackSpeed, ForceMode.Impulse); // AddForce()�� ����
        transform.rotation = Quaternion.LookRotation(transform.forward);
        M_ani.SetBool("w_Attack", false);

        yield return StartCoroutine(StopDash()); // StopDash() ����
    }

    IEnumerator g_Dash(Vector3 moveDir) // �� ����
    {
        M_ani.SetBool("g_Walk", false);
        projector.fadeFactor = 1.0f; // ���� ���� ǥ��
        yield return new WaitForSeconds(3f);
        projector.fadeFactor = 0f; // ���� ���� ǥ�� x

        M_ani.SetBool("g_Attack", true);
        rb.AddForce(moveDir * attackSpeed, ForceMode.Impulse); // AddForce()�� ����
        transform.rotation = Quaternion.LookRotation(transform.forward);
        M_ani.SetBool("g_Attack", false);

        yield return StartCoroutine(StopDash()); // StopDash() ����
    }
}


