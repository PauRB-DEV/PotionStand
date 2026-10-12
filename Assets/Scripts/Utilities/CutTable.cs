using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class CutTable : MonoBehaviour, IInteractable, IUsable
{
    [Header("Configuracion")]
    [SerializeField] private Transform itemPostion;
    [SerializeField] private List<IngredientType> acceptedIngredients;
    [SerializeField] private CutProgressUI progressUI;

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
            Ingredient ingredient = GetHeldIngredient(inventory);

            if (ingredient == null || !acceptedIngredients.Contains(ingredient.IngredientType))
            {
                return;
            }
            else
            {
                progressUI.HideProgress();
            }

            storedIngredient = inventory.Drop();

            storedIngredient.transform.SetParent(itemPostion);
            storedIngredient.transform.localPosition = Vector3.zero;
            storedIngredient.transform.rotation = Quaternion.identity;
            if (storedIngredient.TryGetComponent(out IIngredientProcessor processor))
            {
                processor.SetCutTable(this);
                progressUI.ShowProgress(processor);
            }
            else
            {
                progressUI.HideProgress();
            }
        }
        else if (!inventory.HasItem && storedIngredient != null)
        {
            if (inventory.PickUp(storedIngredient))
            {
                storedIngredient = null;
                progressUI.HideProgress();
            }
        }
    }

    public void Use()
    {
        if (storedIngredient == null)
        {
            return;
        }

        if (storedIngredient.TryGetComponent(out IIngredientProcessor processor))
        {
            processor.SetCutTable(this);
            processor.ProcessIngredient();
        }
        else
        {
            Debug.Log("Este ingrediente no tiene proceso");
            progressUI.HideProgress();
        }
    }

    private Ingredient GetHeldIngredient(PlayerInventory inventory)
    {
        return inventory.HeldIngredient;
    }

    public void ReplaceIngredient(Ingredient processedPrefab)
    {        
        if (storedIngredient == null || processedPrefab == null)
        {
            return;
        }
        
        progressUI.HideProgress();

        Debug.Log("Ingrediente original: " + storedIngredient.name);
        Debug.Log("Prefab procesado: " + processedPrefab.name);

        Destroy(storedIngredient.gameObject);


        storedIngredient = Instantiate(processedPrefab, itemPostion.position, itemPostion.rotation, itemPostion);

        storedIngredient.transform.localPosition = Vector3.zero;
        storedIngredient.transform.localRotation = Quaternion.identity;

        Debug.Log("Ingrediente nuevo en la tabla: " + storedIngredient.name);
    }
}
