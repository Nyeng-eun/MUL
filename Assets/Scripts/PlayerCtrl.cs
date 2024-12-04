using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private CharacterController _controller; // 캐릭터 컨트롤러
    private Animator _animator; // 애니메이터

    private float rotSpeed = 250f; // 회전 속도
    private float moveSpeed = 5f;  // 이동 속도
    private float jumpHeight = 1f; // 점프 높이
    private Vector3 moveDir = Vector3.zero; // 이동 방향

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotSpeed * Time.smoothDeltaTime);

        _animator.SetBool("isJumped", _controller.isGrounded);
        if (_controller.isGrounded)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            h *= Mathf.Sqrt(1f - Mathf.Pow(v, 2) / 2f);
            v *= Mathf.Sqrt(1f - Mathf.Pow(h, 2) / 2f);

            if (Input.GetKey(KeyCode.LeftShift) && v > 0.9f && h < 0.1f) // 달리기 (Shift + only W 키)
            {
                v = 2f;
            }

            if (Input.GetKeyDown(KeyCode.Space)) // 점프 (Space 키)
            {
                moveDir.y = jumpHeight;
            }
            else
            {
                moveDir.y = 0f;
            }

            moveDir.x = h;
            moveDir.z = v;
            moveDir = _controller.transform.TransformDirection(moveDir);
            moveDir *= moveSpeed;

            _animator.SetFloat("v", v);
            _animator.SetFloat("h", h);
        }
        else
        {
            moveDir.y += Physics.gravity.y * Time.deltaTime; // 가속도 * 시간 = 속도
        }
        _controller.Move(moveDir * Time.deltaTime); // 속도 * 시간 = 거리
    }
}
