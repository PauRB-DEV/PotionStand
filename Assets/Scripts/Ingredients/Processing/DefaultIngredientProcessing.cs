using UnityEngine;

public class DefaultIngredientProcessing : MonoBehaviour
{
    [SerializeField] private Ingredient processedPrefab;
    [SerializeField] private int actionsRequired = 4;

    private int actionsDone = 0;

    public bool Process()
    {
        actionsDone++;

        Debug.Log("Procesado: " + actionsDone + "/" + actionsRequired);

        return actionsDone >= actionsRequired;
    }

    public Ingredient GetProcessedPrefab()
    {
        return processedPrefab;
    }
}
