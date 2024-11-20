using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveForward : MonoBehaviour
{
    public float speed = 5.0f; // �Ѿ� �ӵ�
    private Vector2 direction;  // �Ѿ��� �̵��� ����

    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5f);

    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;

    }

    // �Ѿ��� �̵� ������ ����
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

}
