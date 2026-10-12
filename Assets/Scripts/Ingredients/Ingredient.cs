using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private IngredientType ingredientType;

    public IngredientType IngredientType => ingredientType;
}
