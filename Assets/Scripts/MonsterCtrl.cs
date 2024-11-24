using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MonsterCtrl : MonoBehaviour
{
    public int e_Type; // ���� �Ӽ�
    private GameObject player; // �÷��̾�
    private Animator M_ani; // ���� �ִϸ��̼�
    private float e_Speed = 3.0f; // �⺻ �ӵ�
    private float attackRange = 6.0f; // ���� �Ÿ�
    private float attackSpeed = 7.0f; // ���� �ӵ�
    private float pushPower = 5.0f; // �������� ��

    private DecalProjector projector;

    private bool isDash = false;
    private bool isCooldown = false;
    public bool isDelay = false;
    private Rigidbody rb;

    void Awake()
    {
        player = GameObject.FindWithTag("Player"); // �÷��̾� ã��
        M_ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        projector = GetComponent<DecalProjector>();

    }

    void Update()
    {
        Vector3 moveDir = (player.transform.position - transform.position).normalized; // ���� ����
        float Distance = Vector3.Distance(transform.position, player.transform.position); // ���Ϳ� �÷��̾���� �Ÿ�

        Vector3 targetPosition = transform.position;
        targetPosition.y = player.transform.position.y;  // Y�� ����

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
                    StartCoroutine(Dash(moveDir));

                }
                else if (!isDash)
                {
                    transform.Translate(moveDir * e_Speed * Time.deltaTime, Space.World);

                    if (moveDir != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                        transform.rotation = targetRotation;
                    }
                    M_ani.SetBool("w_Attack", false);
                }
                break;
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player") // �÷��̾�� ���� ���� ��
        {
            player.GetComponent<Rigidbody>().AddForce(transform.forward * pushPower, ForceMode.Impulse); // �÷��̾� �о��
        }
    }
    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(1.0f);
        rb.velocity = Vector3.zero;
        isDash = false; // ���� ����
        isCooldown = true; // ��ٿ� ����

        yield return new WaitForSeconds(3.0f); // ��ٿ� �ð�
        isCooldown = false; // ��ٿ� ����
    }

    IEnumerator Dash(Vector3 moveDir)
    {
        M_ani.SetBool("w_idle", true);
        projector.fadeFactor = 1.0f; // ���� ���� ǥ��
        yield return new WaitForSeconds(0.5f);
        M_ani.SetBool("w_idle", false);
        projector.fadeFactor = 0f; // ���� ���� ǥ�� x

        M_ani.SetBool("w_Attack", true);
        rb.AddForce(moveDir * attackSpeed, ForceMode.Impulse); // AddForce()�� ����
        transform.rotation = Quaternion.LookRotation(moveDir);

        yield return StartCoroutine(StopDash()); // StopDash() ����
    }
}
