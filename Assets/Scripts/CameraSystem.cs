// GameManager.cs - Manages game state, night progression, and power level
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

// AnimatronicAI.cs - Controls animatronic movement and attack behavior
using UnityEngine;

public class AnimatronicAI : MonoBehaviour
{
    public Transform[] waypoints; // Path the animatronic follows
    public float moveSpeed = 2f;
    public float attackThreshold = 0.5f; // Distance to trigger attack
    private int currentWaypoint = 0;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MoveToNextWaypoint();
    }

    void Update()
    {
        MoveAlongPath();
        CheckAttackCondition();
    }

    void MoveAlongPath()
    {
        if (waypoints.Length == 0) return;

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void CheckAttackCondition()
    {
        if (Vector3.Distance(transform.position, player.position) < attackThreshold)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Animatronic attacks!");
        GameManager.Instance.TriggerGameOver();
    }
}

// CameraSystem.cs - Handles security camera switching
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public GameObject[] cameras;
    private int currentCameraIndex = 0;

    void Start()
    {
        SwitchCamera(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchCamera((currentCameraIndex - 1 + cameras.Length) % cameras.Length);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchCamera((currentCameraIndex + 1) % cameras.Length);
        }
    }

    void SwitchCamera(int newIndex)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(i == newIndex);
        }
        currentCameraIndex = newIndex;
    }
}
