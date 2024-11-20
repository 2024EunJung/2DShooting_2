using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float speed = 2.0f;      // ���� �̵� �ӵ�
    public GameObject bulletPrefab; // �߻��� �Ѿ� ������
    public float fireRate = 1.5f;   // �Ѿ� �߻� �ֱ� (�� ����)
    private Transform tr;
    public Transform player;        // �÷��̾��� Transform
    public int health = 3;          // ���� ü�� (3���� ����)

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
        // �÷��̾ ���ϴ� ���� ���
        Vector2 direction = (player.position - tr.position).normalized;

        // ���� ��ġ���� �÷��̾ ���� �Ѿ� ����
        GameObject bullet = Instantiate(bulletPrefab, tr.position, Quaternion.identity);

        // �Ѿ� �߻� ���� ���� �� ���� ���Ͽ� �߻�
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 4, ForceMode2D.Impulse); // �Ѿ� �߻�
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
