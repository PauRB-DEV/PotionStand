using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Cauldron : MonoBehaviour, IInteractable, IUsable
{
    [Header("Configuracion")]
    [SerializeField] private Transform ingredientPosition;
    [SerializeField] private float cookingTime = 5f;

    [Header("Recetas")]
    [SerializeField] private PotionRecipe[] recipes;

    private List<IngredientType> ingredients = new List<IngredientType>();

    private bool isCooking;
    private bool isCooked;

    private PotionRecipe currentRecipe;

    public void Interact(PlayerInventory inventory)
    {
        if (inventory == null || !inventory.HasItem)
            return;

        if (isCooking)
        {
            Debug.Log("El caldero está cocinando.");
            return;
        }

        // CASO 1: el jugador lleva una botella.
        if (inventory.HeldBottle != null)
        {
            if (isCooked && currentRecipe != null)
            {
                bool filled = inventory.HeldBottle.Fill(currentRecipe.potionType);

                if (filled)
                {
                    Debug.Log("Botella llenada con " + currentRecipe.potionType);

                    ingredients.Clear();
                    currentRecipe = null;
                    isCooked = false;
                }
            }
            else
            {
                Debug.Log("El caldero no tiene ninguna poción preparada");
            }

            return;
        }

        // CASO 2: el jugador lleva un ingrediente.
        Ingredient ingredient = inventory.HeldIngredient;

        if (ingredient == null)
            return;

        if (isCooked)
        {
            Debug.Log("Primero debes embotellar la poción.");
            return;
        }

        Ingredient droppedIngredient = inventory.DropIngredient();

        if (droppedIngredient == null)
            return;

        IngredientType ingredientType =
            droppedIngredient.IngredientType;

        ingredients.Add(ingredientType);

        Destroy(droppedIngredient.gameObject);

        Debug.Log("Ingrediente añadido al caldero: " + ingredientType);

    }

    public void Use()
    {
        if (isCooking)
        {
            Debug.Log("El caldero ya esta cocinando.");
            return;
        }

        if (isCooked)
        {
            Debug.Log("El caldero ya ha terminado de cocinar.");
            return;
        }

        if (ingredients.Count == 0)
        {
            Debug.Log("No hay ingredientes en el caldero.");
            return;
        }

        StartCoroutine(Cook());
    }

    private IEnumerator Cook()
    {
        isCooking = true;

        Debug.Log("Empieza la coccion!");

        yield return new WaitForSeconds(cookingTime);

        isCooking = false;

        CheckRecipe();
    }

    private void CheckRecipe()
    {
        currentRecipe = null;

        // Buscar una receta especial que coincida.
        foreach (PotionRecipe recipe in recipes)
        {
            if (!recipe.isSpecialRecipe)
                continue;

            if (MatchesIngredients(recipe))
            {
                currentRecipe = recipe;
                break;
            }
        }

        // Si no hay receta especial, buscar una general.
        if (currentRecipe == null)
        {
            foreach (PotionRecipe recipe in recipes)
            {
                if (recipe.isSpecialRecipe)
                    continue;

                if (MatchesIngredients(recipe))
                {
                    currentRecipe = recipe;
                    break;
                }
            }
        }

        // La combinación no corresponde a ninguna receta.
        if (currentRecipe == null)
        {
            Debug.Log("¡La combinación de ingredientes no es una receta válida!");
            return;
        }

        // La poción existe: cocción completada.
        isCooked = true;

        Debug.Log("¡Poción preparada correctamente!");
        Debug.Log("Poción: " + currentRecipe.potionType);
        Debug.Log("Receta especial: " + currentRecipe.isSpecialRecipe);

    }

    private bool MatchesIngredients(PotionRecipe recipe)
    {
        if (recipe.ingredients.Length != ingredients.Count)
            return false;

        List<IngredientType> remainingIngredients =
            new List<IngredientType>(ingredients);

        foreach (IngredientType requiredIngredient in recipe.ingredients)
        {
            if (!remainingIngredients.Remove(requiredIngredient))
                return false;
        }

        return remainingIngredients.Count == 0;

    }
}
