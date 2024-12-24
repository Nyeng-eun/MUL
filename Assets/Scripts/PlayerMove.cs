using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public bool is_Sk = false;
    public bool Is_On_corutine = false; // 코루틴 실행여부 변수 선언 (공격 딜레이 추가하기위해)
    private bool isGround; // 땅에 닿았는지 확인하는 변수 (닌자맛 쿠키 전용 스킬인 2단 점프 방지용)
    private bool is_Lion_Start = false; // 사자 스킬 발동 조건 확인 변수 (발판 밟았는지 확인하기위해)
    public bool Is_LionSK_corutine = false; // 사자후 스킬 딜레이용 코루틴 실행여부 변수 (사자후 스킬 연달아 사용하는걸 방지하기 위한 딜레이)
    public bool Is_LionSK = false; // R키 누른 직후에만 스킬 발동
    public bool _Unbeatable = false; // 데미지를 받을 시 무적 상태

    private Animator _animator; // 애니메이터 변수 선언
    private Rigidbody rb; // 물리엔진을 사용하기 위한 변수 (점프)
    private GameObject scanObject = null; // 상호작용 오브젝트
    public GameObject p_Dam; // 사자 스킬 사용 시 몬스터 스턴 파티클 효과

    private RaycastHit hit; // RaycastHit (충돌체크) 선언

    public int maxLife = 4;
    public int life = 4; // 생명 4개 (보스전때는 6개정도)
    private float rayLength = 5f; // Ray 길이 설정
    public float jumpForce = 5f; // 점프 힘 설정
    public float rotSpeed = 250f; // 회전속도
    public float moveSpeed = 3.5f; // 이동속도
    public float skRange = 5; // 스킬 범위 변수
    public float L_skRange = 5; // 사자 스킬 범위 변수

    void Awake()
    {
        _animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
    }

    void Update()
    {
        // if (Input.anyKey) // 입력이 들어왔을 때
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime);
            transform.rotation = (transform.rotation.y > 360f) ? Quaternion.identity : transform.rotation;
            // 마우스 회전으로 플레이어 회전
            // Vector3.up : y축 기준, rotSpeed : 회전 속도, Time.deltaTime : 시간에 따른 부드러운 회전

            float h = Input.GetAxis("Horizontal"); // 수평 입력값
            float v = Input.GetAxis("Vertical"); // 수직 입력값

            h *= Mathf.Sqrt(1f - Mathf.Pow(v, 2) / 2f);
            v *= Mathf.Sqrt(1f - Mathf.Pow(h, 2) / 2f);
            // 대각선 이동 속도 보정
            // 수평 입력값과 수직 입력값을 이용하여 대각선 이동 속도 보정, 대각선 이동 시 속도가 빨라지는 것을 방지

            if (Input.GetKey(KeyCode.LeftShift) && v > 0.9f && h < 0.1f) // 달리기 (Shift + W 키(수직 입력값))
            {
                v = 2f;
                Debug.Log("달리기 성공");
            }

            else if (Input.GetKeyDown(KeyCode.Space) && isGround) // 점프 (Space 키)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                // 점프, ForceMode.Impulse : 순간적인 힘을 가함
                // rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange); -> 점프, ForceMode.VelocityChange : 순간적인 속도 변화를 가함
                Debug.Log("점프 성공");
            }
            
            else if (Input.GetKeyDown(KeyCode.E)) // 상호 작용 (E 키)
            {
                Debug.DrawRay(transform.position, transform.forward * rayLength, Color.green);
                // Debug.DrawRay(시작점, 방향 * 길이, 색상) : 레이를 그리는 함수
                Debug.Log("상호작용 E키 눌림");

                if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, rayLength, LayerMask.GetMask("Interact")))
                // Raycast(시작점, 방향, out hit, 길이, LayerMask.GetMask("Interact")) : 레이를 쏘는 함수
                // LayerMask.GetMask("Interact") : Interact 레이어에만 충돌하도록 설정
                {
                    scanObject = hit.collider.gameObject; // 충돌한 오브젝트를 scanObject로 할당
                    Debug.Log("Interact 오브젝트 충돌 확인");
                }
                else
                {
                    scanObject = null; // scanObject을 null로 초기화
                    Debug.Log("Interact 오브젝트 없음, null 초기화");
                }

                if (scanObject) // 상호작용 오브젝트가 존재한다면
                {
                    Utils.interact(scanObject.name); // interact 함수 실행
                    Debug.Log("GameManager 상호작용 함수 실행 완료");
                }
            }

            else if (Input.GetKeyDown(KeyCode.Q) && !Is_On_corutine) // 공격 (Q 키)
            {
                StartCoroutine(Attack()); // 코루틴 실행, 시간 딜레이 후 공격, 공격 중에는 다시 공격할 수 없음
            }

            else if (Input.GetKeyDown(KeyCode.R) && is_Lion_Start) // 사자 스킬 (R키 -> 특정 씬에서만 사용 가능)
            {
                StartCoroutine(LionPower()); // 코루틴 실행, 시간 딜레이 후 스킬 시전, 스킬 사용 중에는 다시 공격할 수 없음
            }

            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h); 
            // 이동 방향 벡터 moveDir 계산
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);

            _animator.SetFloat("v", v);
            _animator.SetFloat("h", h);
            // mixamo에서 좌우 움직임 애니메이션, 앞뒤 움직임 애니메이션 찾기
            // animation Blend Tree\
            Debug.DrawRay(transform.position + Vector3.up, transform.forward * rayLength, Color.green);
        }
    }

    void OnCollisionEnter(Collision coll)
    // OnCollisionEnter : 충돌이 시작되었을 때, OnCollisionStay : 충돌 중일 때, OnCollisionExit : 충돌이 끝났을 때
    // Collider가 충돌했을 때 호출되는 함수, 충돌한 상대방의 Collider가 인자로 전달됨
    {
        if (coll.gameObject.CompareTag("Monster")) // 모든 몬스터 (잡몹, 골렘)
        {
            if(_Unbeatable == false)
            {
                life--; // 생명 1 감소
                UIManager.instance.LifeUpdate(maxLife, life, false);
                _animator.SetBool("Attacked", true);
                Debug.Log("몬스터와 충돌, 생명 1 감소 {life}");
                StartCoroutine(p_DamStun());
            }
        }

        else if (coll.gameObject.CompareTag("Position1") || coll.gameObject.CompareTag("Position2") || coll.gameObject.CompareTag("Position3") || coll.gameObject.CompareTag("Position4") || coll.gameObject.CompareTag("Position5") || coll.gameObject.CompareTag("Position6") || coll.gameObject.CompareTag("Position7")) // 미로 몬스터
        {
            if (_Unbeatable == false)
            {
                life--; // 생명 1 감소
                _animator.SetBool("Attacked", true);
                Debug.Log("몬스터와 충돌, 생명 1 감소 {life}");
                StartCoroutine(p_DamStun());
            }
        }

        else if (coll.gameObject.CompareTag("Meteor")) // 메테오
        {
            if (_Unbeatable == false)
            {
                life -= 2; // 생명 2 감소
                _animator.SetBool("Attacked", true);
                Debug.Log("몬스터와 충돌, 생명 2 감소 {life}");
                StartCoroutine(p_DamStun());
            }
        }

        if (coll.gameObject.CompareTag("Ground")) // 땅에 닿았을 때
        {
            isGround = true; // 땅에 닿음 (true), 점프 가능
            _animator.SetBool("isJumped", isGround); 
            // isGround 값에 따라 점프 애니메이션 실행
            // 점프 애니메이션 실행 X
        }

        if (coll.gameObject.CompareTag("LionSkill")) // 사자 스킬 발동 구간
        {
            is_Lion_Start = true; // 사자 스킬 발동 조건 확인 변수 true
        }

        if (coll.gameObject.name == "TpToTinwoodman")
        {
            SceneManager.LoadScene("MainRoad_TinWoodman");
        }
        if (coll.gameObject.name == "TpToFuelPlant")
        {
            SceneManager.LoadScene("FuelPlant");
        }
    }

    void OnCollisionExit(Collision collision) // 충돌이 끝났을 때
    {
        if (collision.gameObject.CompareTag("Ground")) // 충돌한 상대 태그가 Ground라면, 땅에서 떨어진 상태
        {
            isGround = false; // 땅에서 떨어짐 (false), 점프 불가능
            _animator.SetBool("Jump", isGround); 
            // isGround 값에 따라 점프 애니메이션 실행
            // 점프 애니메이션 실행
        }

        if (collision.gameObject.CompareTag("LionSkill")) // 사자 스킬 발동 구간을 빠져나왔을 때
        {
            is_Lion_Start = false; // 사자 스킬 발동 조건 확인 변수 false
        }
    }

    IEnumerator Attack() // 공격 코루틴 (시간 딜레이 후 공격)
    {
        is_Sk = true;
        Is_On_corutine = true; // 코루틴 실행 중, 공격 중
        yield return new WaitForSeconds(0.1f); // 0.1초 대기

        is_Sk = false;
        Debug.Log("공격 코루틴 실행중");
        yield return new WaitForSeconds(5f); // 5초 대기
        Is_On_corutine = false; // 코루틴 종료, 공격 종료
    }

    IEnumerator LionPower() // 사자후 딜레이 추가용 코루틴 함수
    {
        Is_LionSK = true;
        Is_LionSK_corutine = true; // 코루틴 실행 중, 스킬 시전 중
        yield return new WaitForSeconds(0.1f); // 0.1초 대기

        Is_LionSK = false;
        Debug.Log("사자 스킬 코루틴 실행중");
        yield return new WaitForSeconds(5f); // 5초 대기

        Is_LionSK_corutine = false; // 코루틴 종료, 스킬 종료
    }

    public void AttackedEnd()
    {
        _animator.SetBool("Attacked", false);
    }

    IEnumerator p_DamStun()
    {
        _Unbeatable = true;
        GameObject p_DamObj = Instantiate(p_Dam); // 파티클 게임오브젝트 생성

        float timer = 0f;
        while (timer < 4.0f)
        {
            timer += Time.deltaTime;

            if (p_DamObj != null)
            {
                // 플레이어 위치를 가져와 Y축 값을 수정
                Vector3 DamPosition = this.transform.position;
                DamPosition.y += 0.6f;

                
                p_DamObj.transform.position = DamPosition; // 파티클 위치 업데이트
            }

            yield return null; // 다음 프레임까지 대기
        }

        Destroy(p_DamObj); // 파티클 삭제
        _Unbeatable = false;
    }
}
