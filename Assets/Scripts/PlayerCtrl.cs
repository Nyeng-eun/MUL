using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private CharacterController _controller; // ĳ���� ��Ʈ�ѷ�
    private Animator _animator; // �ִϸ�����

    private float rotSpeed = 250f; // ȸ�� �ӵ�
    private float moveSpeed = 5f;  // �̵� �ӵ�
    private float jumpHeight = 1f; // ���� ����
    private Vector3 moveDir = Vector3.zero; // �̵� ����

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

            if (Input.GetKey(KeyCode.LeftShift) && v > 0.9f && h < 0.1f) // �޸��� (Shift + only W Ű)
            {
                v = 2f;
            }

            if (Input.GetKeyDown(KeyCode.Space)) // ���� (Space Ű)
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
            moveDir.y += Physics.gravity.y * Time.deltaTime; // ���ӵ� * �ð� = �ӵ�
        }
        _controller.Move(moveDir * Time.deltaTime); // �ӵ� * �ð� = �Ÿ�
    }
}
