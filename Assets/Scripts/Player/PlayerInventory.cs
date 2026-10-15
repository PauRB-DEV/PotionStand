using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform handPosition;

    private Ingredient heldIngredient;
    private Bottle heldBottle;

    public Ingredient HeldIngredient => heldIngredient;
    public bool HasItem => heldIngredient != null || heldBottle != null;
    public Bottle HeldBottle => heldBottle;

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

    public bool PickUp(Bottle bottle)
    {
        if (HasItem || bottle == null)
        {
            return false;
        }

        heldBottle = bottle;

        bottle.transform.SetParent(handPosition);
        bottle.transform.localPosition = Vector3.zero;
        bottle.transform.localRotation = Quaternion.identity;

        Collider itemCollider = bottle.GetComponent<Collider>();

        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }

        return true;
    }

    public Ingredient DropIngredient()
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

    public Bottle DropBottle()
    {
        if (heldBottle == null)
        {
            return null;
        }

        Bottle bottle = heldBottle;
        heldBottle = null;

        bottle.transform.SetParent(null);

        Collider itemCollider = bottle.GetComponent<Collider>();

        if (itemCollider != null)
        {
            itemCollider.enabled = true;
        }

        return bottle;

    }
}
