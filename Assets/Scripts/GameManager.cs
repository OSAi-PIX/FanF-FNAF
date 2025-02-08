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
    public AudioSource glitchSound;
    public GameObject mainAnimatronic;
    public GameObject leftAnimatronic;
    public GameObject rightAnimatronic;
    public int currentNight = 1;
    public Text nightText;
    public GameObject loreCutscene;
    private bool powerOutTriggered = false;
    private bool flickerTriggered = false;
    private bool nightCompleted = false;
    private float nightDuration = 360f; // 6 minutes per night
    private float nightTimer;

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

    void Start()
    {
        nightTimer = nightDuration;
        UpdateNightUI();
    }

    void Update()
    {
        if (!isGameOver && !nightCompleted)
        {
            ReducePower(powerDrainRate * Time.deltaTime);
            UpdatePowerUI();
            nightTimer -= Time.deltaTime;
            
            if (nightTimer <= 0)
            {
                CompleteNight();
            }
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
            if (glitchSound != null)
            {
                glitchSound.Play();
            }
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
        Invoke("TriggerJumpscare", 8f);
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

    void CompleteNight()
    {
        nightCompleted = true;
        currentNight++;
        if (currentNight > 6)
        {
            SceneManager.LoadScene("VictoryScene");
        }
        else
        {
            StartCoroutine(PlayLoreCutscene());
        }
    }

    IEnumerator PlayLoreCutscene()
    {
        loreCutscene.SetActive(true);
        yield return new WaitForSeconds(10f); // Adjust cutscene duration
        loreCutscene.SetActive(false);
        ResetNight();
    }

    void ResetNight()
    {
        nightCompleted = false;
        powerLevel = 100f;
        nightTimer = nightDuration;
        SceneManager.LoadScene("Night" + currentNight);
        UpdateNightUI();
    }

    void UpdateNightUI()
    {
        if (nightText != null)
            nightText.text = "Night " + currentNight;
    }
}
