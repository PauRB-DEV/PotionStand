using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class OrderTicketUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text clientNameText;
    [SerializeField] private Image potionIcon;

    public void SetOrder(CustomerOrder order, Sprite icon)
    {
        canvasGroup.alpha = 1f;

        clientNameText.text = order.clientType.ToString();
        potionIcon.sprite = icon;
        potionIcon.enabled = icon != null;
    }

    public void Clear()
    {
        canvasGroup.alpha = 0f;
    }
}
