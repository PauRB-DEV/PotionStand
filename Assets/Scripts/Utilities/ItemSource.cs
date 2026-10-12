using UnityEngine;

public class ItemSource : MonoBehaviour, IInteractable
{
    [SerializeField] private Ingredient ingredientPrefab;

    public void Interact()
    {
        PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();

        if (inventory == null || inventory.HasItem)
        {
            return;
        }

        Ingredient ingredient = Instantiate(ingredientPrefab, transform.position + Vector3.up, Quaternion.identity);

        if (!inventory.PickUp(ingredient))
        {
            Destroy(ingredient.gameObject);
        }
    }
}
