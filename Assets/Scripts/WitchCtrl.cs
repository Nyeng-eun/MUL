using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchCtrl : MonoBehaviour
{
    public GameManager gameManager; // �ӽ� ���� �Ŵ���

    public float hp = 30; // ü��
    public float b_maxHp = 30; // �ִ� ü��
    private int b_Type; // ������
    private float b_Speed = 5.0f; // ���� �̵� �ӵ�

    public bool isDestroyed = false; // óġ ����
    public bool Unbeatable = false; // ������ ������ �Ѿ�� �� ���� �ð�
    public bool isAttacked = false; // ���� ��
    private Animator B_ani;

    public GameObject meteo_Particle;
    Vector3 originalPosition = new Vector3(0, 0.1f, 50.0f); // ���� �ٴڿ� ���� ��
    Vector3 targetPosition = new Vector3(0, 8.0f, 50.0f); // ���� ���߿� ���� ��

    // Start is called before the first frame update
    void Start()
    {
        B_ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed) return; // óġ
        if (Unbeatable) return; // ���� �ð�

        switch (b_Type)
        {
            case 0: // ù ������
                // GameManager.instance.isCrowattacked = true;
                gameManager.isCrowattacked = true; // �ӽ�
                if (hp <= 0)
                {
                    StartCoroutine(phase_Change());
                }
                break;

            case 1: // ������ ������
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
        if (coll.gameObject.tag == "Monster") // ��Ϳ��� �ε��� ��
        {
            hp--;
            Destroy(coll.gameObject); // �ε��� ��� ����
        }
    }
    void DestroyB() // ���� óġ
    {
        if (isDestroyed) return; // �ߺ� �ı� ����

        isDestroyed = true;
        // GameManager.instance.isCrowattacked = false;
        gameManager.isCrowattacked = false; // �ӽ�

        B_ani.SetTrigger("witch_Die");

        StopAllCoroutines(); // ���� ���� �ڷ�ƾ �ߴ�

        DestroyAllMeteors(); // ��� ���׿� ��ƼŬ ����

        DestroyAllMonsters(); // ��� ���� ����
    }

    void DestroyAllMeteors()
    {
        GameObject[] meteos = GameObject.FindGameObjectsWithTag("Meteor"); // "Meteor" �±׸� ���� ������Ʈ ã��

        foreach (GameObject meteo in meteos)
        {
            Destroy(meteo); // ���׿� ����
        }
    }

    void DestroyAllMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster"); // "Monster" �±׸� ���� ������Ʈ ã��

        foreach (GameObject monster in monsters)
        {
            Destroy(monster); // ���� ����
        }
    }

    IEnumerator phase_Change()
    {
        Unbeatable = true;
        // GameManager.instance.isCrowattacked = false;
        gameManager.isCrowattacked = false; // �ӽ�
        hp = 30;

        B_ani.SetBool("witch_Fly", true);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) // ��ǥ ���ޱ��� ��ٸ���
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * b_Speed); // ���� ��ġ �̵�
            yield return null; // ���� �����ӱ��� ���
        }

        yield return new WaitForSeconds(2.0f);

        b_Type = 1; // ������ ������
        Unbeatable = false;
        B_ani.SetBool("witch_Fly", false);
    }

    IEnumerator last_Phase() // ������ ������ ���� ����
    {
        B_ani.SetBool("meteo_Attack", true);

        isAttacked = true; // ���� ����
        int meteoAttack = 5;

        for (int i = 0; i < meteoAttack; i++)
        {
            int meteoCount = 10; // ������ ���׿� ����

            for (int m = 0; m < meteoCount; m++)
            {
                GameObject meteoObj = Instantiate(meteo_Particle); // ���׿� ����

                // ���� ��ġ ����
                float m_X = Random.Range(-35.0f, 35.0f);
                float m_Z = Random.Range(-4.0f, 60.0f);

                meteoObj.transform.position = new Vector3(m_X, 0.1f, m_Z);

                // ���׿� ��ƼŬ ���� �ð� ���� �� �ı�
                ParticleSystem m_Ps = meteoObj.GetComponent<ParticleSystem>();
                float duration = m_Ps.main.duration;

                Destroy(meteoObj, duration);

                yield return new WaitForSeconds(0.3f); // �ణ�� �����̸� �༭ ���׿��� ���������� ���������� ����
            }
            yield return new WaitForSeconds(2.0f);
        }
        B_ani.SetBool("meteo_Attack", false);
        B_ani.SetBool("witch_Fly", true);
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f) // �ٴ����� ���� ������
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, Time.deltaTime * b_Speed); // ���� ��ġ �̵�
            yield return null; // ���� �����ӱ��� ���
        }
        B_ani.SetBool("witch_Fly", false);

        // GameManager.instance.isCrowattacked = true;
        gameManager.isCrowattacked = true; // �ӽ�
        yield return new WaitForSeconds(10.0f);

        // GameManager.instance.isCrowattacked = false;
        gameManager.isCrowattacked = false; // �ӽ�
        B_ani.SetBool("witch_Fly", true);
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) // ���� ���� �ö�
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * b_Speed); // ���� ��ġ �̵�
            yield return null; // ���� �����ӱ��� ���
        }

        yield return new WaitForSeconds(2.0f);

        isAttacked = false; // ���� ��
        B_ani.SetBool("witch_Fly", false);
    }
}
