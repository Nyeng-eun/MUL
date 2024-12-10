using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchCtrl : MonoBehaviour
{
    public int hp = 30;
    private int b_Type;
    private float b_Speed = 5.0f;

    private bool isDestroyed = false;
    public bool Unbeatable = false;
    public bool isAttacked = false;
    //private Animator B_ani;

    public GameObject meteo_Particle;
    Vector3 originalPosition = new Vector3(0, 0.1f, 0);
    Vector3 targetPosition = new Vector3(0, 8.0f, 0);

    // Start is called before the first frame update
    void Start()
    {
        //B_ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed) return; // 처치
        if (Unbeatable) return; // 무적 시간

        switch (b_Type)
        {
            case 0:
                if (hp <= 0)
                {
                    StartCoroutine(phase_Change());
                }
                break;

            case 1:
                if (hp <= 0)
                {
                    DestroyB();
                    return;
                }

                if (!isAttacked)
                {
                    StartCoroutine(last_Phase());
                }
                break;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Monster") // 까마귀에게 맞을 시
        {
            hp--;
            Destroy(coll.gameObject);
        }
    }
    void DestroyB()
    {
        if (isDestroyed) return; // 중복 파괴 방지

        isDestroyed = true;
        Destroy(gameObject);
    }

    IEnumerator phase_Change()
    {
        Unbeatable = true;
        hp = 30;
        //B_ani.SetBool("공중에 떠있는 거.. 나중에 바꿀 겁니다. 안심하십쇼.", true);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) // 목표 도달까지 기다리기 // 마녀 x,z 축 위치도 바꿀 예정
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * b_Speed); // 마녀 위치 이동
            yield return null; // 다음 프레임까지 대기
        }

        yield return new WaitForSeconds(2.0f);

        b_Type = 1; // 마지막 페이즈
        Unbeatable = false;
    }

    IEnumerator last_Phase() // 마지막 페이즈 공격 패턴
    {
        isAttacked = true;
        int meteoAttack = 5;

        for (int i = 0; i < meteoAttack; i++)
        {
            int meteoCount = 10; // 생성할 메테오 개수

            for (int m = 0; m < meteoCount; m++)
            {
                GameObject meteoObj = Instantiate(meteo_Particle); // 메테오 생성

                // 랜덤 위치 지정
                float m_X = Random.Range(-35.0f, 35.0f);
                float m_Z = Random.Range(-35.0f, 35.0f);

                meteoObj.transform.position = new Vector3(m_X, 0.1f, m_Z);

                // 메테오 파티클 지속 시간 설정 후 파괴
                ParticleSystem m_Ps = meteoObj.GetComponent<ParticleSystem>();
                float duration = m_Ps.main.duration;

                Destroy(meteoObj, duration);

                yield return new WaitForSeconds(0.3f); // 약간의 딜레이를 줘서 메테오가 순차적으로 떨어지도록 설정
            }
            yield return new WaitForSeconds(2.0f);
        }

        while (Vector3.Distance(transform.position, originalPosition) > 0.01f) // 바닥으로 마녀 내려옴
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, Time.deltaTime * b_Speed); // 마녀 위치 이동
            yield return null; // 다음 프레임까지 대기
        }
        //B_ani.SetBool("공중에 떠있는 거.. 나중에 바꿀 겁니다. 안심하십쇼.", false);

        // 까마귀 공격 넣을 부분

        //B_ani.SetBool("공중에 떠있는 거.. 나중에 바꿀 겁니다. 안심하십쇼.", true);
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) // 위로 마녀 올라감
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * b_Speed); // 마녀 위치 이동
            yield return null; // 다음 프레임까지 대기
        }

        yield return new WaitForSeconds(2.0f);

        isAttacked = false;
    }
}
