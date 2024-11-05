using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float speed;

    Transform tr;
    Vector2 mousePosition;

    public Vector2 limitPoint1; //�����ϴ�
    public Vector2 limitPoint2; //�����ʻ��

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
            //ī�޶� ���߰��ִ� ȭ�鳻�� ��ǥ���� ����� �� �ְ� ����

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
    IEnumerator FireBullet() // �ڷ�ƾ �Լ��� ���� �ð����� ���
    {
        while (true)
        {
            //���� ���߿� ���� ������Ʈ�� ������ �� ���
            //������ ���� ������Ʈ, ������ ��ġ, ���� �� ����
            Instantiate(prefabBullet, tr.position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f); // 0.3�ʸ��� ���� �ݺ�
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("GameOver");
    }
}
