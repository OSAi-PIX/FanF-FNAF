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

    public void TriggerGameOver()
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
        GameManager.Instance.TriggerJumpscare();
    }

    public void ScareAway()
    {
        if (Random.value <= 0.46f) // 46% chance to scare away animatronic
        {
            Debug.Log("Animatronic scared away!");
            currentWaypoint = 0; // Reset animatronic to starting position
        }
    }
}

// FlashlightController.cs - Controls flashlight functionality
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public KeyCode flashKey = KeyCode.Space;
    public AnimatronicAI animatronic;

    void Update()
    {
        if (Input.GetKeyDown(flashKey))
        {
            animatronic.ScareAway();
        }
    }
}

// DoorController.cs - Controls door opening and closing
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public KeyCode closeDoorKey = KeyCode.D;
    public GameObject door;
    private bool isClosed = false;

    void Update()
    {
        if (Input.GetKeyDown(closeDoorKey))
        {
            ToggleDoor();
        }
    }

    void ToggleDoor()
    {
        isClosed = !isClosed;
        door.SetActive(isClosed);
        Debug.Log(isClosed ? "Door closed!" : "Door opened!");
    }
}
