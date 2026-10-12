using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private int maxActiveOrders = 5;
    [SerializeField] private OrderHUD orderHUD;

    private List<CustomerOrder> activeOrders = new List<CustomerOrder>();
    private List<CustomerOrder> readyOrders = new List<CustomerOrder>();

    public List<CustomerOrder> ActiveOrders => activeOrders;
    public List<CustomerOrder> ReadyOrders => readyOrders;
    
    public bool HasSpace()
    {
        return activeOrders.Count < maxActiveOrders;
    }

    public bool AddOrder(CustomerOrder order)
    {
        if (!HasSpace())
        {
            Debug.Log("Ya tienes el máximo de encargos.");
            return false;
        }

        activeOrders.Add(order);
        Debug.Log("Nueva comanda: " + order.clientType + " pide " + order.potionType);
        orderHUD.RefreshHUD();
        return true;
    }

    public void MarkOrderReady(CustomerOrder order)
    {
        if (activeOrders.Contains(order) && !readyOrders.Contains(order))
        {
            readyOrders.Add(order);
            Debug.Log("Pocion preparada: " + order.potionType);
        }
    }

    public void DeliverOrder(CustomerOrder order)
    {
        if (activeOrders.Remove(order))
        {
            readyOrders.Remove(order);
            Debug.Log("Pedido entregado. Encargos activos: " + activeOrders.Count);
            orderHUD.RefreshHUD();
        }
    }

    private void Update()
    {
        foreach (CustomerOrder order in activeOrders)
        {
            if (order.currentPatience > 0)
            {
                order.currentPatience -= Time.deltaTime;

                if (order.currentPatience < 0)
                {
                    order.currentPatience = 0;
                    
                    Debug.Log("La paciencia del cliente se ha agotado");
                }
            }
        }
    }
}
