using UnityEngine;
using UnityEngine.UI;
public class UIGameElement : MonoBehaviour
{
    public Text value;

    public void Init(string _value)
    {
        value.text = _value;
    }
}
