using System;
using UnityEngine;

public interface IIngredientProcessor
{
    void ProcessIngredient();
    void SetCutTable(CutTable cutTable);
    int CurrentProgress {  get; }
    int RequiredProgress {  get; }

    event Action<int, int> OnProgressChanged;
}
