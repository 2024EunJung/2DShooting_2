using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    Material mat;
    float cureentYoffset = 0f; //현재 Y축 오프셋(원래 위치에서 얼마나 떨어져 있는지에 대한 값)
    float speed = 0.08f;
    
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        cureentYoffset += speed * Time.deltaTime; // FPS 보정하기 위해 사용
        mat.mainTextureOffset = new Vector2(0, cureentYoffset);
    }
}
