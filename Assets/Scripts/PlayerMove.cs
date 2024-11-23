using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float speed;

    Transform tr;
    Vector2 mousePosition;

    public Vector2 limitPoint1; //왼쪽하단
    public Vector2 limitPoint2; //오른쪽상단


    public GameObject bulletPrefab; // 발사할 총알 프리팹

    public int numBullets = 3;      // 한 번에 발사할 총알의 개수 (부채꼴로 3발)
    public float angleSpread = 30f; // 부채꼴 각도 범위 (좌우 각도 차이)

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        StartCoroutine(FireBullet());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //카메라가 비추고있는 화면내의 좌표값을 사용할 수 있게 해줌

            if(mousePosition.x < limitPoint1.x)
            {
                mousePosition = new Vector2(limitPoint1.x, mousePosition.y);
            }
            if (mousePosition.y < limitPoint1.y)
            {
                mousePosition = new Vector2(mousePosition.x, limitPoint1.y);
            }
            if (mousePosition.x > limitPoint2.x)
            {
                mousePosition = new Vector2(limitPoint2.x, mousePosition.y);
            }
            if (mousePosition.y > limitPoint2.y)
            {
                mousePosition = new Vector2(mousePosition.x, limitPoint2.y);
            }

            tr.position = Vector2.MoveTowards(tr.position, mousePosition, Time.deltaTime * speed);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(limitPoint1, new Vector2(limitPoint2.x, limitPoint1.y));
        Gizmos.DrawLine(limitPoint1, new Vector2(limitPoint1.x, limitPoint2.y));
        Gizmos.DrawLine(new Vector2(limitPoint1.x, limitPoint2.y), limitPoint2);
        Gizmos.DrawLine(new Vector2(limitPoint2.x, limitPoint1.y), limitPoint2);

    }
    IEnumerator FireBullet() // 코루틴 함수로 일정 시간동안 대기
    {
        while (true)
        {
            if (numBullets == 1)
            {
                // 하나의 총알은 그냥 정중앙으로 발사
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse); // 위쪽 방향으로 발사
            }
            else
            {
                float angleStep = angleSpread / (numBullets - 1); // 각 발사 총알 간의 각도 차이

                for (int i = 0; i < numBullets; i++)
                {
                    // 각도를 계산하여 총알 발사 방향을 설정
                    float angle = -angleSpread / 2 + i * angleStep; // 부채꼴 범위 내에서 각도 계산

                    // 총알을 발사할 방향을 계산 (회전 적용)
                    Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

                    // 플레이어의 위치에서 총알을 발사
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, bulletRotation);

                    // 총알의 Rigidbody2D에 힘을 추가하여 발사
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.AddForce(bulletRotation * Vector2.up * 5f, ForceMode2D.Impulse); // 위쪽 방향으로 발사
                }
            }
            
            yield return new WaitForSeconds(0.1f); // 0.3초마다 내용 반복
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
