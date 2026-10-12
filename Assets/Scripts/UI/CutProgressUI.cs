using System;
using UnityEngine;
using UnityEngine.UI;
public class CutProgressUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Slider progressBar;

    private IIngredientProcessor currentProcessor;

    private void Awake()
    {
        HideProgress();
    }

    public void HideProgress()
    {
        if (currentProcessor != null)
        {
            currentProcessor.OnProgressChanged -= UpdateProgress;
            currentProcessor = null;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void ShowProgress(IIngredientProcessor processor)
    {
        if (processor == null)
        {
            HideProgress();
            return;
        }

        if (currentProcessor != null)
        {
            currentProcessor.OnProgressChanged -= UpdateProgress;
        }

        currentProcessor = processor;
        currentProcessor.OnProgressChanged += UpdateProgress;

        UpdateProgress(currentProcessor.CurrentProgress, currentProcessor.RequiredProgress);

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }


    private void UpdateProgress(int current, int required)
    {
        progressBar.minValue = 0;
        progressBar.maxValue = required;
        progressBar.value = current;
    }
}
