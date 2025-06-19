using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    public Animator robotAnimator;
    public Animator chestAnimator;
    public GameObject winImage;
    public GameObject retryButton;

    private bool hasOpened = false;

    void Start()
    {
        if (chestAnimator != null)
        {
            chestAnimator.Play("chest_idle", 0, 0f);
            chestAnimator.ResetTrigger("Open"); // Clear any leftover trigger
        }

        winImage?.SetActive(false);
        retryButton?.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasOpened && other.CompareTag("Player"))
        {
            hasOpened = true;
            chestAnimator.SetTrigger("Open");
            winImage?.SetActive(true);
            retryButton?.SetActive(true);

            if (robotAnimator != null)
            {
                robotAnimator.enabled = false; // Freeze the animation
            }
        }
    }
}
