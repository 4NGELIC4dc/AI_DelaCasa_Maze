using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryHandler : MonoBehaviour
{
    public void RetryGame()
    {
        // Destroy BGM manually before reload so it restarts fresh
        GameObject bgm = GameObject.Find("BGMPlayer");
        if (bgm != null)
            Destroy(bgm);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
