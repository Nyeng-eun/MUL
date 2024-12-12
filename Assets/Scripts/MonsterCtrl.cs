using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class MonsterCtrl : MonoBehaviour
{
    public int e_Type; // 0 = 까마귀 1 = 늑대 2 = 골렘
    private PlayerMove _playerMove;
    private GameObject player; // 플레이어
    private Animator M_ani; // 몬스터 애니메이션
    private int hp; // 체력
    private float e_Speed; // 기본 속도
    private float attackRange; // 돌진 거리
    private float attackSpeed; // 돌진 속도
    private float pushPower; // 밀쳐지는 힘

    private DecalProjector projector;

    private bool isDash = false;
    private bool isCooldown = false;
    private Rigidbody rb;

    void Awake()
    {
        player = GameObject.FindWithTag("Player"); // 플레이어 찾기
        _playerMove = player.GetComponent<PlayerMove>();
        M_ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        projector = GetComponent<DecalProjector>();

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
                attackSpeed = 12.0f;
                pushPower = 2.0f;
                break;

            case 2:
                e_Speed = 6.0f;
                attackRange = 28.0f;
                attackSpeed = 25.0f;
                pushPower = 2.0f;
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
                        // 늑대 스턴 애니메이션
                    }
                    else
                    {
                        // 안녕히계세요! 여러분! 시전
                    }
                    break;

                case 2:
                    return;
            }
        }
        else
        {
            Vector3 moveDir = (player.transform.position + Vector3.up - transform.position).normalized; // 방향 설정
            float Distance = Vector3.Distance(transform.position, player.transform.position + Vector3.up); // 몬스터와 플레이어와의 거리
            switch (e_Type)
            {
                case 0: // 까마귀 (플레이어를 쫓아다님)
                    transform.Translate(moveDir * e_Speed * Time.deltaTime, Space.World);
                    transform.LookAt(player.transform.position + Vector3.up);
                    break;

                case 1: // 늑대 (플레이어에게 돌진)
                    moveDir.y = 0f; // y축 고정
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

                case 2: // 골렘
                    moveDir.y = 0f; // y축 고정
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
            }
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player") // 플레이어에게 돌진 성공 시
        {
            Destroy(gameObject);
        }
    }

    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(3.0f);
        rb.velocity = Vector3.zero;
        isDash = false; // 돌진 가능
        isCooldown = true; // 쿨다운 시작

        yield return new WaitForSeconds(1.0f); // 쿨다운 시간
        isCooldown = false; // 쿨다운 종료
    }

    IEnumerator w_Dash(Vector3 moveDir) // 늑대 돌진
    {
        M_ani.SetBool("w_Walk", false);
        projector.fadeFactor = 1.0f; // 공격 방향 표시
        yield return new WaitForSeconds(0.5f);

        projector.fadeFactor = 0f; // 공격 방향 표시 x

        M_ani.SetBool("w_Attack", true);
        rb.AddForce(moveDir * attackSpeed, ForceMode.Impulse); // AddForce()로 돌진
        transform.rotation = Quaternion.LookRotation(transform.forward);
        M_ani.SetBool("w_Attack", false);

        yield return StartCoroutine(StopDash()); // StopDash() 시작
    }

    IEnumerator g_Dash(Vector3 moveDir) // 골렘 돌진
    {
        M_ani.SetBool("g_Walk", false);
        projector.fadeFactor = 1.0f; // 공격 방향 표시
        yield return new WaitForSeconds(1.0f);
        projector.fadeFactor = 0f; // 공격 방향 표시 x

        M_ani.SetBool("g_Attack", true);
        rb.AddForce(moveDir * attackSpeed, ForceMode.Impulse); // AddForce()로 돌진
        transform.rotation = Quaternion.LookRotation(transform.forward);
        M_ani.SetBool("g_Attack", false);

        yield return StartCoroutine(StopDash()); // StopDash() 시작
    }
}


