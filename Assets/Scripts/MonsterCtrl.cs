using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{
    public int e_Type;
    private GameObject player; // �÷��̾�
    private int hp; // ���� ü��
    private Animator M_ani; // ���� �ִϸ��̼�
    private float e_Speed; // �⺻ �ӵ�
    private float attackRange = 6.0f; // ���� �Ÿ�
    private float attackSpeed = 7.0f; // ���� �ӵ�
    private float pushPower = 5.0f; // �������� ��

    private PlayerMove _playerCtrl;

    private bool isDash = false;
    private bool isCooldown = false;
    private Rigidbody rb;

    void Awake()
    {
        player = GameObject.FindWithTag("Player"); // �÷��̾� ã��
        _playerCtrl = player.GetComponent<PlayerMove>();
        M_ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        switch (e_Type)
        {
            case 0:
                e_Speed = 3.0f;
                hp = 1;
                break;

            case 1:
                e_Speed = 4.0f;
                hp = 3;
                break;
        }
    }

    void Update()
    {
        if (_playerCtrl.Is_On_corutine && Vector3.Distance(transform.position, _playerCtrl.transform.position) < _playerCtrl.skRange)
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
                        M_ani.SetBool("w_Attack", true); // ������ �� �ִϸ��̼�
                        rb.AddForce(moveDir * attackSpeed, ForceMode.Impulse); // AddForce()�� ����
                        transform.rotation = Quaternion.LookRotation(transform.forward);

                        StartCoroutine(StopDash());
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

        yield return new WaitForSeconds(1.0f); // ��ٿ� �ð�
        isCooldown = false; // ��ٿ� ����
    }
}
