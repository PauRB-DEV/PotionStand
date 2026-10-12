using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform itemPosition;

    private Ingredient storedIngredient;

    public void Interact()
    {
        PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();

        if (inventory == null)
        {
            return;
        }

        if (inventory.HasItem && storedIngredient == null)
        {
            storedIngredient = inventory.Drop();

            storedIngredient.transform.position = itemPosition.position;
            storedIngredient.transform.rotation = Quaternion.identity;
            storedIngredient.transform.SetParent(itemPosition);
        }
        else if (!inventory.HasItem && storedIngredient != null)
        {
            Ingredient ingredient = storedIngredient;

            if (inventory.PickUp(ingredient))
            {
                storedIngredient = null;
            }
        }
    }
}
