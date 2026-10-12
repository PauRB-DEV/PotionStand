using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform handPosition;

    private Ingredient heldIngredient;

    public bool HasItem => heldIngredient != null;
    public Ingredient HeldIngredient => heldIngredient;

    public bool PickUp(Ingredient ingredient)
    {
        if (HasItem)
        {
            return false;
        }

        heldIngredient = ingredient;

        ingredient.transform.SetParent(handPosition);
        ingredient.transform.localPosition = Vector3.zero;
        ingredient.transform.localRotation = Quaternion.identity;

        Collider itemCollider = ingredient.GetComponent<Collider>();

        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }
        return true;
    }

    public Ingredient Drop()
    {
        if (!HasItem)
        {
            return null;
        }

        Ingredient ingredient = heldIngredient;
        heldIngredient = null;

        ingredient.transform.SetParent(null);

        Collider itemCollider = ingredient.GetComponent<Collider>();

        if (itemCollider != null)
        {
            itemCollider.enabled = true;
        }

        return ingredient;
    }
}
