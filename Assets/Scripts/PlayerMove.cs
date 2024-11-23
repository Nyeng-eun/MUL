using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool Is_On_corutine = false; // �ڷ�ƾ ���࿩�� ���� ���� (����)
    private bool isGround; // ���� ��Ҵ��� Ȯ���ϴ� ���� (����)

    private Animator _animator; // �ִϸ����� ���� ����
    private Rigidbody rb; // ���������� ����ϱ� ���� ���� (����)
    private GameObject scanObject = null; // ��ȣ�ۿ� ������Ʈ

    private RaycastHit hit; // RaycastHit (�浹üũ) ����

    public int life = 4; // ���� 4�� (���������� 6������)
    private float rayLength = 3f; // Ray ���� ����
    public float jumpForce = 5f; // ���� �� ����
    public float rotSpeed = 300f; // ȸ���ӵ�
    public float moveSpeed = 10.0f; // �̵��ӵ�
    public float skRange = 5; // ��ų ���� ����

    void Awake()
    {
        _animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
    }

    void Update()
    {
        if (Input.anyKey) // �Է��� ������ ��
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime);
            // ���콺 ȸ������ �÷��̾� ȸ��
            // Vector3.up : y�� ����, rotSpeed : ȸ�� �ӵ�, Time.deltaTime : �ð��� ���� �ε巯�� ȸ��

            float h = Input.GetAxis("Horizontal"); // ���� �Է°�
            float v = Input.GetAxis("Vertical"); // ���� �Է°�

            h *= Mathf.Sqrt(1f - Mathf.Pow(v, 2) / 2f);
            v *= Mathf.Sqrt(1f - Mathf.Pow(h, 2) / 2f);
            // �밢�� �̵� �ӵ� ����
            // ���� �Է°��� ���� �Է°��� �̿��Ͽ� �밢�� �̵� �ӵ� ����, �밢�� �̵� �� �ӵ��� �������� ���� ����

            if (Input.GetKey(KeyCode.LeftShift) && v > 0.9f && h < 0.1f) // �޸��� (Shift + W Ű(���� �Է°�))
            {
                v = 2f;
            }

            else if (Input.GetKeyDown(KeyCode.Space) && isGround) // ���� (Space Ű)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                // ����, ForceMode.Impulse : �������� ���� ����
                // rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange); -> ����, ForceMode.VelocityChange : �������� �ӵ� ��ȭ�� ����
            }

            else if (Input.GetKeyDown(KeyCode.E)) // ��ȣ �ۿ� (E Ű)
            {
                Debug.DrawRay(transform.position, transform.forward * rayLength, Color.green);
                // Debug.DrawRay(������, ���� * ����, ����) : ���̸� �׸��� �Լ�

                if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, LayerMask.GetMask("Interact")))
                    // Raycast(������, ����, out hit, ����, LayerMask.GetMask("Interact")) : ���̸� ��� �Լ�
                    // LayerMask.GetMask("Interact") : Interact ���̾�� �浹�ϵ��� ����
                    scanObject = hit.collider.gameObject; // �浹�� ������Ʈ�� scanObject�� �Ҵ�
                else
                    scanObject = null; // scanObject�� null�� �ʱ�ȭ

                if (scanObject) // ��ȣ�ۿ� ������Ʈ�� �����Ѵٸ�
                    GameManager.instance.Interact(); // GameManager ��ũ��Ʈ�� Interact �Լ� ����
            }

            else if (Input.GetKeyDown(KeyCode.Q) && !Is_On_corutine) // ���� (Q Ű)
            {
                StartCoroutine(Attack()); // �ڷ�ƾ ����, �ð� ������ �� ����, ���� �߿��� �ٽ� ������ �� ����
            }

            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h); 
            // �̵� ���� ���� moveDir ���
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);

            _animator.SetFloat("p_V", v);
            _animator.SetFloat("p_H", h);
            // mixamo���� �¿� ������ �ִϸ��̼�, �յ� ������ �ִϸ��̼� ã��
            // animation Blend Tree
        }
    }

    void OnCollisionEnter(Collision coll)
    // OnCollisionEnter : �浹�� ���۵Ǿ��� ��, OnCollisionStay : �浹 ���� ��, OnCollisionExit : �浹�� ������ ��
    // Collider�� �浹���� �� ȣ��Ǵ� �Լ�, �浹�� ������ Collider�� ���ڷ� ���޵�
    {
        if (coll.gameObject.CompareTag("Monster")) // ��� ���� (���, Į����, ����)
        {
            life--; // ���� 1 ����
        }

        else if (coll.gameObject.CompareTag("Meteor")) // ���׿�
        {
            life -= 2; // ���� 2 ����
        }

        if (coll.gameObject.CompareTag("Ground")) // ���� ����� ��
        {
            isGround = true; // ���� ���� (true), ���� ����
            _animator.SetBool("Jump", isGround); 
            // isGround ���� ���� ���� �ִϸ��̼� ����
            // ���� �ִϸ��̼� ���� X
        }
    }

    void OnCollisionExit(Collision collision) // �浹�� ������ ��
    {
        if (collision.gameObject.CompareTag("Ground")) // �浹�� ��� �±װ� Ground���, ������ ������ ����
        {
            isGround = false; // ������ ������ (false), ���� �Ұ���
            _animator.SetBool("Jump", isGround); 
            // isGround ���� ���� ���� �ִϸ��̼� ����
            // ���� �ִϸ��̼� ����
        }
    }

    IEnumerator Attack() // ���� �ڷ�ƾ (�ð� ������ �� ����)
    {
        Is_On_corutine = true; // �ڷ�ƾ ���� ��, ���� ��
        yield return new WaitForSeconds(5f); // 5�� ���
        Is_On_corutine = false; // �ڷ�ƾ ����, ���� ����
    }
}
