using UnityEngine;

public class Ingredient : MonoBehaviour, IInteractable
{
    [SerializeField] private IngredientType ingredientType;

    public IngredientType IngredientType => ingredientType;

    // Recoger ingrediente del suelo
    public void Interact(PlayerInventory inventory)
    {
        if (inventory == null) return;

        if (inventory.PickUp(this))
        {
            Debug.Log($"Ingrediente recogido: {ingredientType}");
        }
    }
}
