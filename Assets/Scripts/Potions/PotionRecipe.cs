using System;
using UnityEngine;

[Serializable]
public class PotionRecipe
{
    [Header("Tipo de receta")]
    public bool isSpecialRecipe;

    [Header("Cliente")]
    public ClientTypes clientType;

    [Header("Pocion resultante")]
    public PotionTypes potionType;

    [Header("Ingredientes necesarios")]
    public IngredientType[] ingredients;
}
