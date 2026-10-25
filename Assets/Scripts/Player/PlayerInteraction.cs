using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InputActionReference grab;
    [SerializeField] private InputActionReference action;
    [SerializeField] private InputActionReference drop;
    [SerializeField] private float interactDistance = 0.75f;
    [SerializeField] private float dropDistance = 0.6f;   // Siempre menor que interactDistance

    private PlayerInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        if (grab.action.WasPressedThisFrame())
        {
            Grab();
        }
        else if (action.action.WasPressedThisFrame())
        {
            Interact();
        }
        else if (drop.action.WasPressedThisFrame())
        {
            Drop();
        }

        Debug.DrawLine(transform.position + Vector3.up * -0.5f, transform.position + Vector3.up * -0.5f + transform.forward * interactDistance, Color.green);
    }

    // Recoger un item del suelo o interactuar con algo (mesas, calderos, etc.)
    // Usa SphereCast para tener más tolerancia al apuntar hacia el suelo

    private void Grab()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * -0.5f;
        float sphereRadius = 0.2f;

        if (Physics.SphereCast(rayOrigin, sphereRadius, transform.forward, out RaycastHit hit, interactDistance))
        {
            Debug.Log($"He golpeado: {hit.collider.name}");

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable == null)
            {
                interactable = hit.collider.GetComponentInParent<IInteractable>();
                Debug.Log("Collider golpeado: " + hit.collider.name);

                Debug.Log("Ingredient: " +
                    (hit.collider.GetComponentInParent<Ingredient>() != null));

                Debug.Log("Bottle: " +
                    (hit.collider.GetComponentInParent<Bottle>() != null));

                Debug.Log("IInteractable: " +
                    (hit.collider.GetComponentInParent<IInteractable>() != null));
            }

            if (interactable != null)
            {
                interactable.Interact(inventory);
            }
            else
            {
                Debug.Log("El objeto no tiene IInteractable");
            }
        }
        else
        {
            Debug.Log("No hay nada delante");
        }
    }


    // Usar el item que tienes en la mano sobre algo (usar maquinas, etc.)
    private void Interact()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * -0.5f;

        if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit hit, interactDistance))
        {
            Debug.Log($"He golpeado: {hit.collider.name}");
            if (hit.collider.TryGetComponent(out IUsable usable))
            {
                usable.Use();
            }
        }
        else
        {
            Debug.Log("No hay nada delante");
        }
    }

    // Tirar el item que llevas en la mano al suelo delante del jugador
    private void Drop()
    {
        if (!inventory.HasItem)
        {
            Debug.Log("No llevas nada encima");
            return;
        }

        // Colocamos el item a la misma altura que el origen del raycast para que se pueda recoger
        Vector3 raycastHeight = new Vector3(0f, -0.5f, 0f);
        Vector3 dropPosition = transform.position + raycastHeight + transform.forward * dropDistance;

        if (inventory.HeldIngredient != null)
        {
            Ingredient ingredient = inventory.DropIngredient();
            ingredient.transform.position = dropPosition;
            Debug.Log($"Has tirado: {ingredient.IngredientType}");
        }
        else if (inventory.HeldBottle != null)
        {
            Bottle bottle = inventory.DropBottle();
            bottle.transform.position = dropPosition;
            Debug.Log("Has tirado la botella");
        }
    }
}
