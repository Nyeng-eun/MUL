using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MonsterCtrl : MonoBehaviour
{
    public int e_Type; // 몬스터 속성
    private GameObject player; // 플레이어
    private Animator M_ani; // 몬스터 애니메이션
    private float e_Speed = 3.0f; // 기본 속도
    private float attackRange = 6.0f; // 돌진 거리
    private float attackSpeed = 7.0f; // 돌진 속도
    private float pushPower = 5.0f; // 밀쳐지는 힘

    private DecalProjector projector;

    private bool isDash = false;
    private bool isCooldown = false;
    public bool isDelay = false;
    private Rigidbody rb;

    void Awake()
    {
        player = GameObject.FindWithTag("Player"); // 플레이어 찾기
        M_ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        projector = GetComponent<DecalProjector>();

    }

    void Update()
    {
        Vector3 moveDir = (player.transform.position - transform.position).normalized; // 방향 설정
        float Distance = Vector3.Distance(transform.position, player.transform.position); // 몬스터와 플레이어와의 거리

        Vector3 targetPosition = transform.position;
        targetPosition.y = player.transform.position.y;  // Y축 고정

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

        yield return new WaitForSeconds(3.0f); // 쿨다운 시간
        isCooldown = false; // 쿨다운 종료
    }

    IEnumerator Dash(Vector3 moveDir)
    {
        M_ani.SetBool("w_idle", true);
        projector.fadeFactor = 1.0f; // 공격 방향 표시
        yield return new WaitForSeconds(0.5f);
        M_ani.SetBool("w_idle", false);
        projector.fadeFactor = 0f; // 공격 방향 표시 x

        M_ani.SetBool("w_Attack", true);
        rb.AddForce(moveDir * attackSpeed, ForceMode.Impulse); // AddForce()로 돌진
        transform.rotation = Quaternion.LookRotation(moveDir);

        yield return StartCoroutine(StopDash()); // StopDash() 시작
    }
}
