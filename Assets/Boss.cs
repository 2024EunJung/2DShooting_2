using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public AudioClip hit; // 맞았을 때 재생할 소리 클립
    public AudioSource hited; // 소리를 재생할 AudioSource

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 물체의 태그가 "PlayerBullet"인지 확인
        if (other != null && other.CompareTag("PlayerBullet"))
        {
            // 보스의 체력 감소
            GameManager.Instance.bossHp -= 1;

            // 소리 재생
            if (hited != null && hit != null)
            {
                hited.PlayOneShot(hit); // 지정된 AudioClip 재생
            }

            // 총알 제거
            Destroy(other.gameObject, 4f);
        }
    }
}
