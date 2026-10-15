using UnityEngine;

public class BottleSource : MonoBehaviour, IInteractable
{
    [SerializeField] private Bottle bottlePrefab;

    public void Interact(PlayerInventory inventory)
    {
        if (inventory == null || inventory.HasItem)
        {
            return;
        }

        Bottle bottle = Instantiate(bottlePrefab, transform.position + Vector3.up, Quaternion.identity);

        if (!inventory.PickUp(bottle))
        {
            Destroy(bottle.gameObject);
        }
    }
}
