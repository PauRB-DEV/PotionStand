using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class OrderCounter : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform[] queuePositions;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private OrderManager orderManager;

    private ClientOrder waitingClient;

    private List<ClientOrder> clientQueue = new List<ClientOrder>();

    public void RegisterClient(ClientOrder client)
    {
        clientQueue.Add(client);
        UpdateQueue();

        Debug.Log("Clientes en cola: " + clientQueue.Count);
    }

    public bool IsFirstClient(ClientOrder client)
    {
        return clientQueue.Count > 0 && clientQueue[0] == client;
    }

    public void RemoveClient(ClientOrder client)
    {
        if (clientQueue.Remove(client))
        {
            UpdateQueue();
            Debug.Log("Clientes en cola: " + clientQueue.Count);
        }
    }

    private void UpdateQueue()
    {
        for (int i = 0; i < clientQueue.Count; i++)
        {
            if (i < queuePositions.Length)
            {
                clientQueue[i].MoveToQueuePosition(queuePositions[i].position);
            }
        }
    }

    public void Interact()
    {
        if (clientQueue.Count == 0)
        {
            Debug.Log("No hay clientes en la cola");
            return;
        }

        ClientOrder client = clientQueue[0];

        if (!client.ReachedCounter)
        {
            Debug.Log("El primer cliente todavia no ha llegado.");
            return;
        }

        if (orderManager == null)
        {
            Debug.Log("Falta asignar el OrderManager.");
            return;
        }

        CustomerOrder order = client.CreateOrder();

        orderManager.AddOrder(order);

        clientQueue.RemoveAt(0);
        client.Leave(GetExitPosition());

        UpdateQueue();

        Debug.Log("Pedido registrado. El cliente se marcha.");
    }

    public Vector3 GetExitPosition()
    {
        return exitPoint.position;
    }
}
