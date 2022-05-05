using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerMotor playerMoveScript;

    public GameObject pauseMenu;
    public ButtonFunctionality btnFuncScript;

    private GameObject player;
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Instance != null)
        {
            Debug.LogError("Multiple GameManagers! Destroying the newest one: " + this.name);
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        player = GameObject.FindGameObjectWithTag("Player");

        if(player == null)
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
