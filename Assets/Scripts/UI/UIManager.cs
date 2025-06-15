using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager: Singleton<UIManager>
{
    [Header("UI Elements")]
    public Canvas UIcanvas;
    public TMP_Text turnText;

    //Set turn text
    public void SetTurnText(int turnCounter)
    {
        turnText.text = string.Format("Turn:{0}",turnCounter);
    }
}
