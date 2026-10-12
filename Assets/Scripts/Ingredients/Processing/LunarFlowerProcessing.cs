using System;
using UnityEngine;

public class LunarFlowerProcessing : MonoBehaviour, IIngredientProcessor
{
    [SerializeField] private int actionsRequired = 4;

    private int actionsDone = 0;
    private bool isProcessed = false;
    private CutTable cutTable;

    public void ProcessIngredient()
    {
        Debug.Log("Iniciando minijuego para extraer los pétalos lunares");
    }

    public void SetCutTable(CutTable table)
    {
        cutTable = table;
    }
    public int CurrentProgress => actionsDone;

    public int RequiredProgress => actionsRequired;

    public event Action<int, int> OnProgressChanged;
}
