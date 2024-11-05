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

    public GameObject prefabBullet;

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
            //게임 도중에 게임 오브젝트를 생성할 때 사용
            //생성할 게임 오브젝트, 생성할 위치, 생성 시 각도
            Instantiate(prefabBullet, tr.position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f); // 0.3초마다 내용 반복
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("GameOver");
    }
}
