using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    Dictionary<string, BaseScreen> m_Screens = new Dictionary<string, BaseScreen>();

    private static ScreenManager m_ScreenManager;
    public static ScreenManager Instance
    {
        get
        {
            return m_ScreenManager;
        }

        private set
        {
            m_ScreenManager = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple ScreenManagers! Destroying the newest one: " + this.name);
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void AddScreen(BaseScreen screen)
    {
        if (!m_Screens.ContainsKey(screen.screenName))
        {
            m_Screens.Add(screen.screenName, screen);
            screen.Hide();
        }
        else
        {
            Debug.LogWarning("Trying to add another screen that has aleady been added, screen name: " + screen.screenName);
        }
    }

    public void RemoveScreen(BaseScreen screen)
    {
        if(screen != null)
        {
            m_Screens.Remove(screen.screenName);
        }
    }

    public void ShowScreen(string screenName)
    {
        BaseScreen screen;
        if(m_Screens.TryGetValue(screenName, out screen))
        {
            Debug.Log("Showing screen - > " + screen.screenName);
            screen.Show();
        }
    }

    public void HideScreen(string screenName)
    {
        BaseScreen screen;
        if (m_Screens.TryGetValue(screenName, out screen))
        {
            screen.Hide();
        }
    }
}
