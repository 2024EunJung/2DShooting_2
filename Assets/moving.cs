using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    public float lowerLimit = -5f;  // y축 하한선
    public float speed = 2f;       // 등속 이동 속도
    public float resetY = 0f;      // 복귀 위치 (y축)

    private Vector3 initialPosition; // 초기 위치 저장

    void Start()
    {
        // 초기 위치를 저장
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // 현재 위치 가져오기
        Vector3 currentPosition = transform.localPosition;

        // 등속으로 내려감
        currentPosition.y -= speed * Time.deltaTime;

        // 특정 값 이하로 내려가면 원래 위치로 복귀
        if (currentPosition.y <= lowerLimit)
        {
            currentPosition.y = resetY;
        }

        // 위치 갱신
        transform.localPosition = currentPosition;
    }
}
