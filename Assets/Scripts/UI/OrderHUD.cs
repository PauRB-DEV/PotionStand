using System;
using UnityEngine;

[Serializable]
public class PotionIconEntry
{
    public PotionTypes potionType;
    public Sprite icon;
}

public class OrderHUD: MonoBehaviour
{
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private OrderTicketUI[] ticketSlots;
    [SerializeField] private PotionIconEntry[] potionIcons;

    private void Start()
    {
        RefreshHUD();
    }

    public void RefreshHUD()
    {
        if (orderManager == null)
        {
            return;
        }

        for (int i = 0; i < ticketSlots.Length; i++)
        {
            if (i < orderManager.ActiveOrders.Count)
            {
                CustomerOrder order = orderManager.ActiveOrders[i];
                ticketSlots[i].SetOrder(order, GetPotionIcon(order.potionType));
            }
            else
            {
                ticketSlots[i].Clear();
            }
        }
    }

    private Sprite GetPotionIcon(PotionTypes potionType)
    {
        foreach (PotionIconEntry entry in potionIcons)
        {
            if (entry.potionType == potionType)
            {
                return entry.icon;
            }
        }

        return null;
    }
}
