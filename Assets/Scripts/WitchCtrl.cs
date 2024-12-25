using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchCtrl : MonoBehaviour
{
    public GameManager gameManager; // 임시 게임 매니저

    public float hp = 30; // 체력
    public float b_maxHp = 30; // 최대 체력
    private int b_Type; // 페이즈
    private float b_Speed = 5.0f; // 마녀 이동 속도

    public bool isDestroyed = false; // 처치 유무
    public bool Unbeatable = false; // 마지막 페이즈 넘어가기 전 무적 시간
    public bool isAttacked = false; // 공격 중
    private Animator B_ani;

    public GameObject meteo_Particle;
    Vector3 originalPosition = new Vector3(0, 0.1f, 50.0f); // 마녀 바닥에 있을 시
    Vector3 targetPosition = new Vector3(0, 8.0f, 50.0f); // 마녀 공중에 있을 시

    // Start is called before the first frame update
    void Start()
    {
        B_ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed) return; // 처치
        if (Unbeatable) return; // 무적 시간

        switch (b_Type)
        {
            case 0: // 첫 페이즈
                // GameManager.instance.isCrowattacked = true;
                gameManager.isCrowattacked = true; // 임시
                if (hp <= 0)
                {
                    StartCoroutine(phase_Change());
                }
                break;

            case 1: // 마지막 페이즈
                if (hp <= 0)
                {
                    DestroyB();
                    return;
                }

                else
                {
                    if (!isAttacked)
                    {
                        StartCoroutine(last_Phase());
                    }
                }

                break;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Monster") // 까마귀에게 부딪힐 시
        {
            hp--;
            Destroy(coll.gameObject); // 부딪힌 까마귀 삭제
        }
    }
    void DestroyB() // 마녀 처치
    {
        if (isDestroyed) return; // 중복 파괴 방지

        isDestroyed = true;
        // GameManager.instance.isCrowattacked = false;
        gameManager.isCrowattacked = false; // 임시

        B_ani.SetTrigger("witch_Die");

        StopAllCoroutines(); // 진행 중인 코루틴 중단

        DestroyAllMeteors(); // 모든 메테오 파티클 제거

        DestroyAllMonsters(); // 모든 몬스터 제거
    }

    void DestroyAllMeteors()
    {
        GameObject[] meteos = GameObject.FindGameObjectsWithTag("Meteor"); // "Meteor" 태그를 가진 오브젝트 찾기

        foreach (GameObject meteo in meteos)
        {
            Destroy(meteo); // 메테오 삭제
        }
    }

    void DestroyAllMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster"); // "Monster" 태그를 가진 오브젝트 찾기

        foreach (GameObject monster in monsters)
        {
            Destroy(monster); // 몬스터 삭제
        }
    }

    IEnumerator phase_Change()
    {
        Unbeatable = true;
        // GameManager.instance.isCrowattacked = false;
        gameManager.isCrowattacked = false; // 임시
        hp = 30;

        B_ani.SetBool("witch_Fly", true);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) // 목표 도달까지 기다리기
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * b_Speed); // 마녀 위치 이동
            yield return null; // 다음 프레임까지 대기
        }

        yield return new WaitForSeconds(2.0f);

        b_Type = 1; // 마지막 페이즈
        Unbeatable = false;
        B_ani.SetBool("witch_Fly", false);
    }

    IEnumerator last_Phase() // 마지막 페이즈 공격 패턴
    {
        B_ani.SetBool("meteo_Attack", true);

        isAttacked = true; // 공격 시작
        int meteoAttack = 5;

        for (int i = 0; i < meteoAttack; i++)
        {
            int meteoCount = 10; // 생성할 메테오 개수

            for (int m = 0; m < meteoCount; m++)
            {
                GameObject meteoObj = Instantiate(meteo_Particle); // 메테오 생성

                // 랜덤 위치 지정
                float m_X = Random.Range(-35.0f, 35.0f);
                float m_Z = Random.Range(-4.0f, 60.0f);

                meteoObj.transform.position = new Vector3(m_X, 0.1f, m_Z);

                // 메테오 파티클 지속 시간 설정 후 파괴
                ParticleSystem m_Ps = meteoObj.GetComponent<ParticleSystem>();
                float duration = m_Ps.main.duration;

                Destroy(meteoObj, duration);

                yield return new WaitForSeconds(0.3f); // 약간의 딜레이를 줘서 메테오가 순차적으로 떨어지도록 설정
            }
            yield return new WaitForSeconds(2.0f);
        }
        B_ani.SetBool("meteo_Attack", false);
        B_ani.SetBool("witch_Fly", true);
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f) // 바닥으로 마녀 내려옴
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, Time.deltaTime * b_Speed); // 마녀 위치 이동
            yield return null; // 다음 프레임까지 대기
        }
        B_ani.SetBool("witch_Fly", false);

        // GameManager.instance.isCrowattacked = true;
        gameManager.isCrowattacked = true; // 임시
        yield return new WaitForSeconds(10.0f);

        // GameManager.instance.isCrowattacked = false;
        gameManager.isCrowattacked = false; // 임시
        B_ani.SetBool("witch_Fly", true);
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) // 위로 마녀 올라감
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * b_Speed); // 마녀 위치 이동
            yield return null; // 다음 프레임까지 대기
        }

        yield return new WaitForSeconds(2.0f);

        isAttacked = false; // 공격 끝
        B_ani.SetBool("witch_Fly", false);
    }
}
