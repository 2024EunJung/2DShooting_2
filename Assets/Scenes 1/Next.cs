using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButton : MonoBehaviour
{
    public GameObject player; // 플레이어 오브젝트
    public Sprite newPlayerImage; // 플레이어에게 적용할 이미지
    public string nextSceneName; // 이동할 씬 이름

    // 버튼 클릭 시 호출되는 함수
    public void OnButtonClick()
    {
        // 플레이어 이미지 변경
        if (player != null && newPlayerImage != null)
        {
            SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = newPlayerImage;
            }
        }

        // 다음 씬으로 이동
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
