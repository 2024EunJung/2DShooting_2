using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreate : MonoBehaviour
{
    public GameObject[] prefabEnemy;
    public Vector2 limitMin;
    public Vector2 limitMax;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CreateEnemy()
    {
        while (true)
        {
            float r = Random.Range(limitMin.x, limitMax.x);
            // 첫 번쨰 인자와 두 번째 인자 사이에서 랜덤한 값을 돌려주는 함수
            Vector2 creatingPoint = new Vector2(r, limitMin.y);

            Instantiate(prefabEnemy[Random.Range(0, prefabEnemy.Length)], creatingPoint, Quaternion.identity );

            yield return new WaitForSeconds(1f/GameManager.Instance.level);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(limitMin, limitMax);
    }
}
