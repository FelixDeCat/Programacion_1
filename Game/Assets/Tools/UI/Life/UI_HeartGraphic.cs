using UnityEngine.UI;
using UnityEngine;

public class UI_HeartGraphic : MonoBehaviour
{
    [SerializeField] Image heart;
    public void SetImage(Sprite _sprite) { heart.sprite = _sprite; }
}
