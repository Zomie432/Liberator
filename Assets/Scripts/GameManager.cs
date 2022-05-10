using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerMotor playerMoveScript;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Player playerScript;
    [HideInInspector]
    public GameObject mainCamera;
    [HideInInspector]
    public Vector3 playerAimVector;
    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public CharacterController playerCharacterController;
    [HideInInspector]
    public bool playerIsGrounded;
    [HideInInspector]
    public GameObject pause;
    [HideInInspector]
    public GameObject hostageSecured;

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
        playerCharacterController = player.GetComponent<CharacterController>();
        pause = GameObject.FindGameObjectWithTag("PauseMenu");
        pause.SetActive(false);
        hostageSecured = GameObject.FindGameObjectWithTag("HostageSecuredScreen");
        hostageSecured.SetActive(false);

        if (player == null)
        {
            Debug.LogError("Player class cannot be found, does not exist");

            playerMoveScript = player.GetComponent<PlayerMotor>();
        }

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if(mainCamera == null)
        {
            Debug.LogError("MainCamera not found in scene");
        }
    }

    private void Update()
    {
        //player's position and rotation in the world
        playerTransform = player.transform;

        //getting where the player is looking which includes the rotation up and down of the main camera
        playerAimVector = mainCamera.transform.forward;

        //implemented to use for footstep audio so it doesn't play if the player is in the air, can be used or anything
        playerIsGrounded = playerCharacterController.isGrounded;
    }
}
