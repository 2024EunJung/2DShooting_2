using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveForward : MonoBehaviour
{
    public float speed = 5.0f; // �Ѿ� �ӵ�
    private Vector2 direction;  // �Ѿ��� �̵��� ����


    void Start()
    {
        Destroy(gameObject, 5f);

    }

    // �Ѿ��� �̵� ������ ����
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

}
