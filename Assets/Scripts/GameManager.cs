using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerMotor playerMoveScript;

    public GameObject player;
    public Player playerScript;
    public Transform playerTransform;
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {

        if (Instance != null)
        {
            Debug.LogError("Multiple GameManagers! Destroying the newest one: " + this.name);
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player class cannot be found, does not exist");

            playerMoveScript = player.GetComponent<PlayerMotor>();
        }

    }

    private void Update()
    {
        playerTransform = player.transform;
    }
}
