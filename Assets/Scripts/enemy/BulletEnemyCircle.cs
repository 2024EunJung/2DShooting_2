using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyCircle : MonoBehaviour
{
    public float speed;      // ���� �̵� �ӵ�
    public GameObject bulletPrefab; // �߻��� �Ѿ� ������
    public float fireRate = 1.0f;   // �Ѿ� �߻� �ֱ� (�� ����)
    private Transform tr;
    public Transform player;        // �÷��̾��� Transform
    public int health = 5;          // ���� ü�� (5�� ����)
    public float easeTime = 2.0f;   // Ease Out ȿ���� ���� �ð�

    private bool hasStopped = false; // �̵��� �Ϸ�Ǿ����� Ȯ��
    private float moveStartTime;    // �̵� ���� �ð��� ����� ����
    private SpriteRenderer spriteRenderer;
    public Sprite originalSprite;   // ���� ��������Ʈ ����
    public Sprite flashSprite;      // �Ͼ�� ������ �� ����� ��������Ʈ
    public GameObject[] Item;
    private float angleOffset = 0f; // ������ ȸ����Ű�� ���� ����

    void Start()
    {
        tr = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� ��ġ ã��
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ����
        originalSprite = spriteRenderer.sprite; // ���� ��������Ʈ ����

        // speed ���� 5���� 10 ���̷� �����ϰ� ����
        speed = Random.Range(5f, 10f);

        moveStartTime = Time.time; // �̵� ���� �ð� ���
        StartCoroutine(DestroySelf());     // ���� �ð��� ������ �ı�
        InvokeRepeating("FireBullet", 1.0f, fireRate); // �ֱ������� �Ѿ� �߻�
    }

    void Update()
    {
        // Ease Out �̵� ���� (ó������ ������, �ð��� �����鼭 ���� ������)
        if (!hasStopped)
        {
            // t�� �̵� ���ۺ��� ������� ����� �ð��� ���
            float t = Mathf.Clamp01((Time.time - moveStartTime) / easeTime); // �̵� ���ۺ��� ��������� �ð� ����
            float easeOutSpeed = speed * Mathf.Pow(1 - t, 2); // Ease Out ȿ��: �ð� ����� ���� �ӵ��� ���� ����

            tr.Translate(Vector2.down * easeOutSpeed * Time.deltaTime); // �Ʒ��� �̵�

            if (t >= 1) // �̵��� �Ϸ�Ǹ� �������� �߻�
            {
                hasStopped = true; // �̵� �Ϸ�
            }
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10.0f); // 10�� �� �ı�
        Destroy(this.gameObject);
    }

    void FireBullet()
    {
        if (hasStopped) // �̵��� ���� �� �Ѿ� �߻�
        {
            // ���� ��η� �߻��� �Ѿ��� �� (8��)
            int numBullets = 8;
            float angleStep = 360.0f / numBullets; // �� �Ѿ� ���� ���� ����

            for (int i = 0; i < numBullets; i++)
            {
                // ������ ����Ͽ� �Ѿ� �߻� ������ ����
                float angle = angleOffset + (i * angleStep); // ������ ��� ������ ���� angleOffset�� �߰�
                Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

                // ���� ��ġ���� �߻�
                GameObject bullet = Instantiate(bulletPrefab, tr.position, bulletRotation);

                // ���� ��η� �Ѿ� �߻�
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bulletRotation * Vector2.up * 4, ForceMode2D.Impulse); // �������� �߻�
            }

            // �߻� �� ������ ȸ����Ŵ (���� �߻縦 ����)
            angleOffset += 10f; // ������ 10���� �������Ѽ� �߻� ������ �ٲ۴�.
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
                GameManager.Instance.AddXp(50);
                int randomValue = Random.Range(0, Item.Length);
                Instantiate(Item[randomValue], transform.position, Quaternion.identity);

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
