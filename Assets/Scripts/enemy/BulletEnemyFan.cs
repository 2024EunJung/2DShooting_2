using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BulletEnemyFan : MonoBehaviour
{
    public float speed = 5.0f;      // ���� �̵� �ӵ�
    public GameObject bulletPrefab; // �߻��� �Ѿ� ������
    public float fireRate = 2.0f;   // �Ѿ� �߻� �ֱ� (�� ����)
    private Transform tr;
    public Transform player;        // �÷��̾��� Transform
    public int health = 5;          // ���� ü�� (5�� ����)

    private SpriteRenderer spriteRenderer;
    public Sprite originalSprite;   // ���� ��������Ʈ ����
    public Sprite flashSprite;      // �Ͼ�� ������ �� ����� ��������Ʈ

    void Start()
    {
        tr = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� ��ġ ã��
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ����
        originalSprite = spriteRenderer.sprite; // ���� ��������Ʈ ����

        StartCoroutine(DestroySelf());     // ���� �ð��� ������ �ı�
        InvokeRepeating("FireBullet", 1.0f, fireRate); // �ֱ������� �Ѿ� �߻�
    }

    void Update()
    {
        tr.Translate(Vector2.down * speed * Time.deltaTime); // �Ʒ��� �̵�
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10.0f); // 10�� �� �ı�
        Destroy(this.gameObject);
    }

    void FireBullet()
    {
        // �÷��̾ ���� ���� ���
        Vector2 direction = (player.position - tr.position).normalized;

        // ��ä�÷� �߻��� ���� ����
        float spreadAngle = 10.0f; // ��ä���� �ݰ� (�¿�� �߻��� ������ ����)
        int numBullets = 5;        // �߻��� �Ѿ��� �� (5��)

        for (int i = 0; i < numBullets; i++)
        {
            // �� �Ѿ��� ������ ���
            float angleOffset = (i - (numBullets / 2)) * spreadAngle; // ��ä�÷� �й�� ����
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angleOffset); // ȸ���� ����

            // ���� ��ġ���� �߻�
            GameObject bullet = Instantiate(bulletPrefab, tr.position, bulletRotation);

            // �Ѿ� �߻� ���� ���� �� ���� ���Ͽ� �߻�
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bulletRotation * direction * 4, ForceMode2D.Impulse); // ��ä�÷� �߻�
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet")) // �÷��̾��� �Ѿ˿� ������
        {
            // ü�� ����
            health--;

            // �Ͼ�� �����̴� ȿ��
            StartCoroutine(FlashWhite());

            if (health <= 0) // ü���� 0�� �Ǹ� �� �ı�
            {
                GameManager.Instance.AddScore(10);
                Destroy(gameObject); // �� �ı�
            }

            Destroy(collision.gameObject); // �÷��̾��� �Ѿ� �ı�
        }
    }

    // �Ͼ�� �����̴� ��� ���� (��������Ʈ �̹��� ����)
    IEnumerator FlashWhite()
    {
        spriteRenderer.sprite = flashSprite; // �Ͼ�� ��������Ʈ�� ����
        yield return new WaitForSeconds(0.1f); // ��� ���
        spriteRenderer.sprite = originalSprite; // ���� ��������Ʈ�� ����
    }
}
