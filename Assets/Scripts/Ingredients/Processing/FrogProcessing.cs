using System;
using UnityEngine;

public class FrogProcessing : MonoBehaviour, IIngredientProcessor
{
    [SerializeField] private Ingredient processedPrefab;
    [SerializeField] private int actionsRequired = 4;

    private int actionsDone = 0;
    private bool isProcessed = false;
    private CutTable cutTable;

    public void ProcessIngredient()
    {
        if (isProcessed)
        {
            return;
        }

        actionsDone++;

        OnProgressChanged?.Invoke(actionsDone, actionsRequired);

        Debug.Log("Procesando: " + actionsDone + "/" + actionsRequired);

        if (actionsDone >= actionsRequired)
        {
            isProcessed = true;

            if (cutTable != null)
            {
                Debug.Log("Tabla encontrada, sustituyendo ingrediente.");
                cutTable.ReplaceIngredient(processedPrefab);
            }
            else
            {
                Debug.Log("No se ha encontrado ninguna Tabla de cortar");
            }
        }
    }

    public void SetCutTable(CutTable table)
    {
        cutTable = table;
    }

    public int CurrentProgress => actionsDone;

    public int RequiredProgress => actionsRequired;

    public event Action<int, int> OnProgressChanged;
}
