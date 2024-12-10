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
        if (isDestroyed) return; // óġ
        if (Unbeatable) return; // ���� �ð�

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
        if (coll.gameObject.tag == "Monster") // ��Ϳ��� ���� ��
        {
            hp--;
            Destroy(coll.gameObject);
        }
    }
    void DestroyB()
    {
        if (isDestroyed) return; // �ߺ� �ı� ����

        isDestroyed = true;
        Destroy(gameObject);
    }

    IEnumerator phase_Change()
    {
        Unbeatable = true;
        hp = 30;
        //B_ani.SetBool("���߿� ���ִ� ��.. ���߿� �ٲ� �̴ϴ�. �Ƚ��Ͻʼ�.", true);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) // ��ǥ ���ޱ��� ��ٸ��� // ���� x,z �� ��ġ�� �ٲ� ����
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * b_Speed); // ���� ��ġ �̵�
            yield return null; // ���� �����ӱ��� ���
        }

        yield return new WaitForSeconds(2.0f);

        b_Type = 1; // ������ ������
        Unbeatable = false;
    }

    IEnumerator last_Phase() // ������ ������ ���� ����
    {
        isAttacked = true;
        int meteoAttack = 5;

        for (int i = 0; i < meteoAttack; i++)
        {
            int meteoCount = 10; // ������ ���׿� ����

            for (int m = 0; m < meteoCount; m++)
            {
                GameObject meteoObj = Instantiate(meteo_Particle); // ���׿� ����

                // ���� ��ġ ����
                float m_X = Random.Range(-35.0f, 35.0f);
                float m_Z = Random.Range(-35.0f, 35.0f);

                meteoObj.transform.position = new Vector3(m_X, 0.1f, m_Z);

                // ���׿� ��ƼŬ ���� �ð� ���� �� �ı�
                ParticleSystem m_Ps = meteoObj.GetComponent<ParticleSystem>();
                float duration = m_Ps.main.duration;

                Destroy(meteoObj, duration);

                yield return new WaitForSeconds(0.3f); // �ణ�� �����̸� �༭ ���׿��� ���������� ���������� ����
            }
            yield return new WaitForSeconds(2.0f);
        }

        while (Vector3.Distance(transform.position, originalPosition) > 0.01f) // �ٴ����� ���� ������
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, Time.deltaTime * b_Speed); // ���� ��ġ �̵�
            yield return null; // ���� �����ӱ��� ���
        }
        //B_ani.SetBool("���߿� ���ִ� ��.. ���߿� �ٲ� �̴ϴ�. �Ƚ��Ͻʼ�.", false);

        // ��� ���� ���� �κ�

        //B_ani.SetBool("���߿� ���ִ� ��.. ���߿� �ٲ� �̴ϴ�. �Ƚ��Ͻʼ�.", true);
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) // ���� ���� �ö�
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * b_Speed); // ���� ��ġ �̵�
            yield return null; // ���� �����ӱ��� ���
        }

        yield return new WaitForSeconds(2.0f);

        isAttacked = false;
    }
}
