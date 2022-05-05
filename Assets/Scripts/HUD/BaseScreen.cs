using UnityEngine;

[System.Serializable]
public class BaseScreen : MonoBehaviour
{
    public string screenName;

    private void Start() // Add This Screen to HUBManagers list of screens
    {
        ScreenManager.Instance.AddScreen(this);
    }

    private void OnDestroy() // remove screen from HUBManager
    {
        ScreenManager.Instance.RemoveScreen(this);
    }

    /*
     * Shows the current screen by setting it active true
     */
    public virtual void Show() { gameObject.SetActive(true); }

    /*
     * Hides the current screen by setting it active false
     */
    public virtual void Hide() { gameObject.SetActive(false); }
}
