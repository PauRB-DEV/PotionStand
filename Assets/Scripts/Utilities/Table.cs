using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform itemPosition;

    private Ingredient storedIngredient;
    private Bottle storedBottle;

    public void Interact(PlayerInventory inventory)
    {
        if (inventory == null)
        {
            return;
        }

        if (inventory.HasItem && storedIngredient == null && storedBottle == null)
        {
            if (inventory.HeldIngredient != null)
            {
                storedIngredient = inventory.DropIngredient();

                storedIngredient.transform.position = itemPosition.position;
                storedIngredient.transform.rotation = Quaternion.identity;
                storedIngredient.transform.SetParent(itemPosition);
            }
            else if (inventory.HeldBottle != null)
            {
                storedBottle = inventory.DropBottle();

                storedBottle.transform.position = itemPosition.position;
                storedBottle.transform.rotation = Quaternion.identity;
                storedBottle.transform.SetParent(itemPosition);
            }
        }
        // Recoger un ingrediente de la mesa
        else if (!inventory.HasItem && storedIngredient != null)
        {
            if (inventory.PickUp(storedIngredient))
            {
                storedIngredient = null;
            }
        }
        // Recoger una botella de la mesa
        else if (!inventory.HasItem && storedBottle != null)
        {
            if (inventory.PickUp(storedBottle))
            {
                storedBottle = null;
            }
        }
    }
}
