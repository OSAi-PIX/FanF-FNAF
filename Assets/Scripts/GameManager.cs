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
    public GameObject glitchScreen;
    public Light[] lights;
    public AudioSource powerOutSound;
    public AudioSource animatronicSong;
    public GameObject mainAnimatronic;
    public GameObject leftAnimatronic;
    public GameObject rightAnimatronic;
    private bool powerOutTriggered = false;
    private bool flickerTriggered = false;

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
        
        if (powerLevel <= 2 && !flickerTriggered)
        {
            FlickerLights();
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

    void FlickerLights()
    {
        flickerTriggered = true;
        StartCoroutine(FlickerEffect());
    }

    IEnumerator FlickerEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            foreach (Light light in lights)
            {
                light.enabled = !light.enabled;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void TriggerPowerShutdown()
    {
        powerOutTriggered = true;
        StartCoroutine(GlitchSequence());
    }

    IEnumerator GlitchSequence()
    {
        float glitchTime = 2.7f;
        float interval = 0.3f;
        int glitchCount = (int)(glitchTime / interval);
        
        for (int i = 0; i < glitchCount; i++)
        {
            glitchScreen.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
            glitchScreen.SetActive(false);
            yield return new WaitForSeconds(interval);
        }
        
        blackoutScreen.SetActive(true);
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
        if (powerOutSound != null)
        {
            powerOutSound.Play();
        }
        if (animatronicSong != null)
        {
            animatronicSong.Play();
        }
        leftAnimatronic.SetActive(true);
        rightAnimatronic.SetActive(true);
        Invoke("TriggerJumpscare", 8f); // Animatronics sing before jumpscare
    }

    public void TriggerJumpscare()
    {
        mainAnimatronic.SetActive(true);
        Invoke("TriggerGameOver", 2f);
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
