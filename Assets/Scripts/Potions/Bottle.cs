using UnityEngine;
using UnityEngine.Rendering;

public class Bottle : MonoBehaviour, IInteractable
{
    [Header("Liquido")]
    [SerializeField] private MeshRenderer liquidRenderer;
    [SerializeField] private Material emptyMaterial;

    [Header("Materiales de las pociones")]
    [SerializeField] private Material healMaterial;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private Material resistanceMaterial;
    [SerializeField] private Material staminaMaterial;
    [SerializeField] private Material magicMaterial;
    [SerializeField] private Material poisonMaterial;
    [SerializeField] private Material beautyMaterial;
    [SerializeField] private Material amnesiaMaterial;
    [SerializeField] private Material fireMaterial;
    [SerializeField] private Material invisibilityMaterial;

    private PotionTypes potionType;
    private bool isEmpty;

    public bool IsEmpty => isEmpty;
    public PotionTypes PotionType => potionType;

    private void Awake()
    {
        isEmpty = true;
        liquidRenderer.material = emptyMaterial;
    }

    public void Interact(PlayerInventory inventory)
    {
        if (inventory == null) return;

        if (inventory.PickUp(this))
        {
            Debug.Log("Botella recogida");
        }
    }

    public bool Fill(PotionTypes potion)
    {
        if (!isEmpty)
        {
            Debug.Log("La botella ya contiene una poción");
            return false;
        }

        Material potionMaterial = GetPotionMaterial(potion);

        if (potionMaterial == null)
        {
            Debug.LogError("No hay material asignado para: " + potion);
            return false;
        }

        potionType = potion;
        liquidRenderer.material = potionMaterial;
        isEmpty = false;

        Debug.Log("Botella llena de: " + potionType);
        return true;
    }

    private Material GetPotionMaterial(PotionTypes potion)
    {
        switch (potion)
        {
            case PotionTypes.HealPotion:
                return healMaterial;

            case PotionTypes.DamagePotion:
                return damageMaterial;

            case PotionTypes.ResistancePotion:
                return resistanceMaterial;

            case PotionTypes.StaminaPotion:
                return staminaMaterial;

            case PotionTypes.MagicPotion:
                return magicMaterial;

            case PotionTypes.PoisonPotion:
                return poisonMaterial;

            case PotionTypes.BeautyPotion:
                return beautyMaterial;

            case PotionTypes.AmnesiaPotion:
                return amnesiaMaterial;

            case PotionTypes.FirePotion:
                return fireMaterial;

            case PotionTypes.InvisibilityPotion:
                return invisibilityMaterial;

            default:
                return null;
        }
    }
}
