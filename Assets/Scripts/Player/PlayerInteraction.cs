using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InputActionReference grab;
    [SerializeField] private InputActionReference action;
    [SerializeField] private float interactDistance = 0.75f;

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

        Debug.DrawLine(transform.position + Vector3.up * -0.5f, transform.position + Vector3.up * -0.5f + transform.forward * interactDistance, Color.green);
    }

    private void Grab()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * -0.5f;

        if (Physics.Raycast(rayOrigin, transform.forward, out  RaycastHit hit, interactDistance))
        {
            Debug.Log($"He golpeado: {hit.collider.name}");
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(inventory);
            }
        }
        else
        {
            Debug.Log("No hay nada delante");
        }
    }

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
}
