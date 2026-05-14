using UnityEngine;
using UnityEngine.UI;

public class PlayerUIItem : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    
    public void Select()
    {
        _toggle.isOn = true;
    }
}
