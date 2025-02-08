using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameControls : MonoBehaviour
{
    private float holdTime = 2.0f;
    private float timer8 = 0f;
    private bool isMinigameActive = false;
    public Button exitButton;
    public Animator screenAnimator;
    
    void Start()
    {
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitMinigame);
        }
    }
    
    void Update()
    {
        if (isMinigameActive) return;
        
        // Check if keys 1 and 2 are held simultaneously
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha2))
        {
            TriggerMinigame(1);
        }

        // Check if key 8 is held for 2 seconds
        if (Input.GetKey(KeyCode.Alpha8))
        {
            timer8 += Time.deltaTime;
            if (timer8 >= holdTime)
            {
                TriggerMinigame(2);
                timer8 = 0f;
            }
        }
        else
        {
            timer8 = 0f;
        }
        
        // Check if keys 2, 0, 1, and 6 are held simultaneously
        if (Input.GetKey(KeyCode.Alpha2) && Input.GetKey(KeyCode.Alpha0) && Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha6))
        {
            TriggerMinigame(3);
        }

        // Check if ESC is pressed to exit minigame
        if (Input.GetKeyDown(KeyCode.Escape) && isMinigameActive)
        {
            ExitMinigame();
        }
    }

    void TriggerMinigame(int minigameNumber)
    {
        Debug.Log("Minigame " + minigameNumber + " triggered!");
        isMinigameActive = true;
        Time.timeScale = 0f; // Pause main game
        if (screenAnimator != null)
        {
            screenAnimator.SetTrigger("Close"); // Retro screen shutdown effect
        }
        Invoke("LoadMinigame", 1.0f); // Delay for animation
    }

    void LoadMinigame()
    {
        SceneManager.LoadScene("Minigame" + minigameNumber, LoadSceneMode.Additive);
    }

    public void ExitMinigame()
    {
        Debug.Log("Exiting Minigame");
        if (screenAnimator != null)
        {
            screenAnimator.SetTrigger("Open"); // Retro screen boot-up effect
        }
        Invoke("UnloadMinigame", 1.0f); // Delay for animation
    }

    void UnloadMinigame()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f; // Resume main game
        isMinigameActive = false;
    }
}
