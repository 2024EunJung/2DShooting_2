using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveForward : MonoBehaviour
{
    public float speed = 5.0f; // 총알 속도
    private Vector2 direction;  // 총알이 이동할 방향

    void Start()
    {
        StartCoroutine(DestroySelf()); // 일정 시간이 지나면 총알 파괴
    }

    void Update()
    {
        // 총알이 지정된 방향으로 이동
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // 총알의 이동 방향을 설정
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(5.0f); // 5초 후 파괴
        Destroy(this.gameObject);
    }
}
