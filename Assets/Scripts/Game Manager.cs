using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player m_PlayerScript;
    public Player Player
    {
        get
        {
            return m_PlayerScript;
        }

        private set
        {
            m_PlayerScript = value;
        }
    }

    private static GameManager m_Instance;
    public static GameManager Instance
    { 
        get
        {
            return m_Instance;
        }

        private set
        {
            m_Instance = value;
        }
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Multiple GameManagers! Destroying the newest one: " + this.name);
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        m_PlayerScript = FindObjectOfType<Player>();

        if(Player == null)
        {
            Debug.LogError("Player class cannot be found, does not exist");
        }
    }
}
