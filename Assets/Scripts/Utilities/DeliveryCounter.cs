using UnityEngine;

public class DeliveryCounter : MonoBehaviour, IInteractable
{
    [SerializeField] private OrderManager orderManager;

    public void Interact(PlayerInventory inventory)
    {
        if (inventory == null) return;

        Bottle bottle = inventory.HeldBottle;

        if (bottle == null) return;

        if (bottle.IsEmpty) return;

        CustomerOrder matchingOrder = null;

        foreach (CustomerOrder order in orderManager.ActiveOrders)
        {
            if (order.potionType == bottle.PotionType)
            {
                matchingOrder = order;
                break;
            }
        }

        if (matchingOrder == null) return;

        orderManager.DeliverOrder(matchingOrder);

        Bottle deliveredBottle = inventory.DropBottle();

        if (deliveredBottle != null)
        {
            Destroy(deliveredBottle.gameObject);
        }

        Debug.Log("Pedido entregado correctamente!");
    }
}
