using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreate : MonoBehaviour
{
    public GameObject[] prefabEnemy;
    public Vector2 limitMin;
    public Vector2 limitMax;
    private Coroutine Create;

    // Start is called before the first frame update
    void Start()
    {
        Create = StartCoroutine(CreateEnemy());
    }

    IEnumerator CreateEnemy()
    {
        while (true)
        {
            float r = Random.Range(limitMin.x, limitMax.x);
            Vector2 creatingPoint = new Vector2(r, limitMin.y);

            Instantiate(prefabEnemy[Random.Range(0, prefabEnemy.Length)], creatingPoint, Quaternion.identity);

            yield return new WaitForSeconds(1f / GameManager.Instance.level);
        }
    }

    public void StopCreatingEnemies()
    {
        if (Create != null)
        {
            StopCoroutine(Create);
            Create = null;
        }
    }

    public void ResumeCreatingEnemies()
    {
        if (Create == null)
        {
            Create = StartCoroutine(CreateEnemy());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(limitMin, limitMax);
    }
}
