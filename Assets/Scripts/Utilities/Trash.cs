using UnityEngine;

public class Trash : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInventory inventory)
    {
        Debug.Log("Interactuando con la basura");

        if (inventory == null)
        {
            Debug.LogError("No se ha encontrado PlayerInventory");
            return;
        }

        if (!inventory.HasItem)
        {
            Debug.Log("No llevas ningún objeto en las manos");
            return;
        }

        if (inventory.HeldIngredient != null)
        {
            Ingredient ingredient = inventory.DropIngredient();

            if (ingredient != null)
            {
                Debug.Log("Destruyendo ingrediente: " + ingredient.name);
                Destroy(ingredient.gameObject);
            }
        }
        // Tirar botella
        else if (inventory.HeldBottle != null)
        {
            Bottle bottle = inventory.DropBottle();

            if (bottle != null)
            {
                Debug.Log("Destruyendo botella: " + bottle.name);
                Destroy(bottle.gameObject);
            }
        }
    }
}
