using UnityEngine;

public class Trash : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interactuando con la basura");

        PlayerInventory inventory =
            FindFirstObjectByType<PlayerInventory>();

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

        Ingredient ingredient = inventory.Drop();

        if (ingredient == null)
        {
            Debug.LogError("Drop() ha devuelto null");
            return;
        }

        Debug.Log("Destruyendo: " + ingredient.name);

        Destroy(ingredient.gameObject);
    }
}
