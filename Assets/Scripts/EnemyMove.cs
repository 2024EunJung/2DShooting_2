using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Transform tr;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        StartCoroutine(DestroySelf());
    }

    // Update is called once per frame
    void Update()
    {
        tr.Translate(Vector2.down * speed * Time.deltaTime);
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet")) // 플레이어의 총알에 맞으면
        {
            GameObject.Find("GameManager").GetComponent<Score>().score += 10;
            Destroy(gameObject);        // 적 파괴
            Destroy(collision.gameObject); // 플레이어의 총알 파괴
        }
    }
}
