using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIGameElement : MonoBehaviour
{
    private UIGameInfoType uiGameInfoType;
    public TextMeshProUGUI value;

    public void Init(UIGameInfoType _gameInfoType, string _value)
    {
        uiGameInfoType = _gameInfoType;
        switch(uiGameInfoType)
        {
            case UIGameInfoType.Damage:
                value.color = Color.red;
                break;
            case UIGameInfoType.Heal:
                value.color = Color.blue;
                break;
            default:
                break;
        }
        value.text = _value;
    }
}
