using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{
    public int e_Type;
    private GameObject player; // 플레이어
    private int hp; // 몬스터 체력
    private Animator M_ani; // 몬스터 애니메이션
    private float e_Speed; // 기본 속도
    private float attackRange = 6.0f; // 돌진 거리
    private float attackSpeed = 7.0f; // 돌진 속도
    private float pushPower = 5.0f; // 밀쳐지는 힘

    private PlayerMove _playerCtrl;

    private bool isDash = false;
    private bool isCooldown = false;
    private Rigidbody rb;

    void Awake()
    {
        player = GameObject.FindWithTag("Player"); // 플레이어 찾기
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
                        // 늑대 스턴 애니메이션
                    }
                    else
                    {
                        // 안녕히계세요! 여러분! 시전
                    }
                    break;
            }
        }
        else
        {
            Vector3 moveDir = (player.transform.position - transform.position).normalized; // 방향 설정
            float Distance = Vector3.Distance(transform.position, player.transform.position); // 몬스터와 플레이어와의 거리
            switch (e_Type)
            {
                case 0: // 까마귀 (플레이어를 쫓아다님)
                    transform.Translate(moveDir * e_Speed * Time.deltaTime, Space.World);
                    transform.LookAt(player.transform.position);
                    break;

                case 1: // 늑대 (플레이어에게 돌진)
                    moveDir.y = 0f; // y축 고정
                    if (Distance <= attackRange && !isDash && !isCooldown)
                    {
                        isDash = true;
                        M_ani.SetBool("w_Attack", true); // 돌진할 때 애니메이션
                        rb.AddForce(moveDir * attackSpeed, ForceMode.Impulse); // AddForce()로 돌진
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
        if (coll.gameObject.tag == "Player") // 플레이어에게 돌진 성공 시
        {
            player.GetComponent<Rigidbody>().AddForce(transform.forward * pushPower, ForceMode.Impulse); // 플레이어 밀어내기
        }
    }
    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(1.0f);
        rb.velocity = Vector3.zero;
        isDash = false; // 돌진 가능
        isCooldown = true; // 쿨다운 시작

        yield return new WaitForSeconds(1.0f); // 쿨다운 시간
        isCooldown = false; // 쿨다운 종료
    }
}
