using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScreen : BaseScreen
{    
    public void OnShowOptions()
    {
        ScreenManager.Instance.ShowScreen(screenName);
    }
}
