using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool Is_On_corutine = false; // 코루틴 실행여부 변수 선언 (공격)
    private bool isGround; // 땅에 닿았는지 확인하는 변수 (점프)

    private Animator _animator; // 애니메이터 변수 선언
    private Rigidbody rb; // 물리엔진을 사용하기 위한 변수 (점프)
    private GameObject scanObject = null; // 상호작용 오브젝트

    private RaycastHit hit; // RaycastHit (충돌체크) 선언

    public int life = 4; // 생명 4개 (보스전때는 6개정도)
    private float rayLength = 3f; // Ray 길이 설정
    public float jumpForce = 5f; // 점프 힘 설정
    public float rotSpeed = 300f; // 회전속도
    public float moveSpeed = 10.0f; // 이동속도
    public float skRange = 5; // 스킬 범위 변수

    void Awake()
    {
        _animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
    }

    void Update()
    {
        if (Input.anyKey) // 입력이 들어왔을 때
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime);
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
            }

            else if (Input.GetKeyDown(KeyCode.Space) && isGround) // 점프 (Space 키)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                // 점프, ForceMode.Impulse : 순간적인 힘을 가함
                // rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange); -> 점프, ForceMode.VelocityChange : 순간적인 속도 변화를 가함
            }

            else if (Input.GetKeyDown(KeyCode.E)) // 상호 작용 (E 키)
            {
                Debug.DrawRay(transform.position, transform.forward * rayLength, Color.green);
                // Debug.DrawRay(시작점, 방향 * 길이, 색상) : 레이를 그리는 함수

                if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, LayerMask.GetMask("Interact")))
                    // Raycast(시작점, 방향, out hit, 길이, LayerMask.GetMask("Interact")) : 레이를 쏘는 함수
                    // LayerMask.GetMask("Interact") : Interact 레이어에만 충돌하도록 설정
                    scanObject = hit.collider.gameObject; // 충돌한 오브젝트를 scanObject로 할당
                else
                    scanObject = null; // scanObject을 null로 초기화

                if (scanObject) // 상호작용 오브젝트가 존재한다면
                    GameManager.instance.Interact(); // GameManager 스크립트의 Interact 함수 실행
            }

            else if (Input.GetKeyDown(KeyCode.Q) && !Is_On_corutine) // 공격 (Q 키)
            {
                StartCoroutine(Attack()); // 코루틴 실행, 시간 딜레이 후 공격, 공격 중에는 다시 공격할 수 없음
            }

            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h); 
            // 이동 방향 벡터 moveDir 계산
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);

            _animator.SetFloat("p_V", v);
            _animator.SetFloat("p_H", h);
            // mixamo에서 좌우 움직임 애니메이션, 앞뒤 움직임 애니메이션 찾기
            // animation Blend Tree
        }
    }

    void OnCollisionEnter(Collision coll)
    // OnCollisionEnter : 충돌이 시작되었을 때, OnCollisionStay : 충돌 중일 때, OnCollisionExit : 충돌이 끝났을 때
    // Collider가 충돌했을 때 호출되는 함수, 충돌한 상대방의 Collider가 인자로 전달됨
    {
        if (coll.gameObject.CompareTag("Monster")) // 모든 몬스터 (잡몹, 칼리다, 마녀)
        {
            life--; // 생명 1 감소
        }

        else if (coll.gameObject.CompareTag("Meteor")) // 메테오
        {
            life -= 2; // 생명 2 감소
        }

        if (coll.gameObject.CompareTag("Ground")) // 땅에 닿았을 때
        {
            isGround = true; // 땅에 닿음 (true), 점프 가능
            _animator.SetBool("Jump", isGround); 
            // isGround 값에 따라 점프 애니메이션 실행
            // 점프 애니메이션 실행 X
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
    }

    IEnumerator Attack() // 공격 코루틴 (시간 딜레이 후 공격)
    {
        Is_On_corutine = true; // 코루틴 실행 중, 공격 중
        yield return new WaitForSeconds(5f); // 5초 대기
        Is_On_corutine = false; // 코루틴 종료, 공격 종료
    }
}
