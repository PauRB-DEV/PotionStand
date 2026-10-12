using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    private List<CustomerOrder> pendingOrders = new List<CustomerOrder>();
    private List<CustomerOrder> readyOrders = new List<CustomerOrder>();

    public List<CustomerOrder> PendingOrders => pendingOrders;
    public List<CustomerOrder> ReadyOrders => readyOrders;
    
    public void AddOrder(CustomerOrder order)
    {
        pendingOrders.Add(order);

        Debug.Log("Nueva comanda: " + order.clientType + " pide " +  order.potionType + ". Paciencia: " + order.currentPatience + " segundos.");
        Debug.Log("Pedidos pendientes: " + pendingOrders.Count);
    }

    public void CompleteOrder(CustomerOrder order)
    {
        if (pendingOrders.Remove(order))
        {
            readyOrders.Add(order);
            Debug.Log("Pedido listo. Pedidos listso: " + readyOrders.Count);
        }
    }

    private void Update()
    {
        foreach (CustomerOrder order in pendingOrders)
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
