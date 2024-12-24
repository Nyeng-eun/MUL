using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class MonsterCtrl : MonoBehaviour
{
    private PlayerMove _playerMove;
    private GameObject player; // 플레이어
    public GameObject w_StunP; // 늑대 스턴
    private Animator M_ani; // 몬스터 애니메이션
    private Rigidbody rb;

    public int e_Type; // 0 = 까마귀 1 = 늑대 2 = 골렘
    public float hp; // 체력
    public float maxHp; // 최대 체력
    public float e_Speed; // 기본 속도
    private float attackRange; // 돌진 거리
    private float attackSpeed; // 돌진 속도

    private DecalProjector projector;

    private bool isDash = false;
    private bool isCooldown = false;
    private bool w_isStunned = false;

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
                hp = 1.0f;
                maxHp = 1.0f;
                break;

            case 1:
                e_Speed = 4.0f;
                hp = 3.0f;
                maxHp = 3.0f;
                attackRange = 6.0f;
                attackSpeed = 12.0f;
                break;

            case 2:
                e_Speed = 6.0f;
                attackRange = 28.0f;
                attackSpeed = 25.0f;
                break;
        }
    }

    void Update()
    {
        if (w_isStunned)
        {
            M_ani.Play("Wolf_Idle"); // 바로 Wolf_Idle 재생
            rb.velocity = Vector3.zero; // 스턴 당하자마자 멈춤
            return;
        }

        if (_playerMove.is_Sk && Vector3.Distance(transform.position, _playerMove.transform.position) < _playerMove.skRange)
        {
            switch (e_Type)
            {
                case 0:
                    if (hp >= 1)
                    {
                        hp--;
                    }
                    break;

                case 1:
                    if (hp > 1)
                    {
                        hp--;
                        StartCoroutine(wolf_Stun());
                    }
                    else
                    {
                        Destroy(gameObject);
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
                    if(hp >= 1)
                    {
                        transform.Translate(moveDir * e_Speed * Time.deltaTime, Space.World);
                        transform.LookAt(player.transform.position + Vector3.up);
                    }
                    else
                        transform.Translate(transform.forward * e_Speed * Time.deltaTime, Space.World);
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
            switch (e_Type)
            {
                case 0:
                    Destroy(gameObject);
                    break;

                case 1:
                    return;

                case 2:
                    return;

            }
        }
    }

    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(1.5f);
        rb.velocity = Vector3.zero;
        isDash = false; // 돌진 가능
        isCooldown = true; // 쿨다운 시작

        yield return new WaitForSeconds(1.0f); // 쿨다운 시간
        isCooldown = false; // 쿨다운 종료
    }

    IEnumerator w_Dash(Vector3 moveDir) // 늑대 돌진
    {
        M_ani.SetBool("w_Walk", false);

        if (moveDir != Vector3.zero)
        {
            // 스턴 후 moveDir 재정의
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = targetRotation;
        }

        projector.fadeFactor = 1.0f; // 공격 방향 표시
        yield return new WaitForSeconds(0.5f);

        projector.fadeFactor = 0f; // 공격 방향 표시 x

        M_ani.SetTrigger("w_Attack");

        rb.AddForce(moveDir * attackSpeed, ForceMode.Impulse); // AddForce()로 돌진
        transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return StartCoroutine(StopDash()); // StopDash() 시작
        M_ani.ResetTrigger("w_Attack");
    }

    IEnumerator g_Dash(Vector3 moveDir) // 골렘 돌진
    {
        M_ani.SetBool("g_Walk", false);
        projector.fadeFactor = 1.0f; // 공격 방향 표시
        yield return new WaitForSeconds(1.0f);
        projector.fadeFactor = 0f; // 공격 방향 표시 x

        M_ani.SetTrigger("g_Attack");
        rb.AddForce(moveDir * attackSpeed, ForceMode.Impulse); // AddForce()로 돌진
        transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return StartCoroutine(StopDash()); // StopDash() 시작
        M_ani.ResetTrigger("g_Attack");
    }

    IEnumerator wolf_Stun()
    {
        w_isStunned = true;
        GameObject w_stunObj = Instantiate(w_StunP); // 파티클 게임오브젝트 생성

        Vector3 pos = this.transform.position;
        w_stunObj.transform.position = pos; // 몬스터 위치에 파티클 생성
        yield return new WaitForSeconds(3.0f);

        Destroy(w_stunObj); // 파티클 삭제
        w_isStunned = false;
    }
}


