// GameManager.cs - Manages game state, night progression, and power level
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float powerLevel = 100f;
    public bool isGameOver = false;
    public float powerDrainRate = 0.1f; // Baseline power drain per second
    public Text powerText;
    public Image powerBar;
    public GameObject blackoutScreen;
    public Light[] lights;
    public AudioSource powerOutSound;
    private bool powerOutTriggered = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            ReducePower(powerDrainRate * Time.deltaTime);
            UpdatePowerUI();
        }
        
        if (powerLevel <= 0 && !isGameOver && !powerOutTriggered)
        {
            TriggerPowerShutdown();
        }
    }

    public void ReducePower(float amount)
    {
        powerLevel = Mathf.Max(0, powerLevel - amount);
    }

    public void TriggerPowerShutdown()
    {
        powerOutTriggered = true;
        blackoutScreen.SetActive(true);
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
        if (powerOutSound != null)
        {
            powerOutSound.Play();
        }
        Invoke("TriggerGameOver", 5f); // Delay game over for dramatic effect
    }

    public void TriggerGameOver()
    {
        isGameOver = true;
        SceneManager.LoadScene("GameOver");
    }

    void UpdatePowerUI()
    {
        if (powerText != null)
            powerText.text = "Power: " + Mathf.RoundToInt(powerLevel) + "%";

        if (powerBar != null)
            powerBar.fillAmount = powerLevel / 100f;
    }
}
