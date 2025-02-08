using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float powerLevel = 100f;
    public bool isGameOver = false;

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
        if (powerLevel <= 0 && !isGameOver)
        {
            TriggerGameOver();
        }
    }

    public void ReducePower(float amount)
    {
        powerLevel = Mathf.Max(0, powerLevel - amount);
    }

    void TriggerGameOver()
    {
        isGameOver = true;
        SceneManager.LoadScene("GameOver");
    }
}
