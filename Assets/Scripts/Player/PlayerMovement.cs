using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Dash")]
    [SerializeField] private float dashpower = 25f; // Aumentado para que se note el impulso con fuerza
    [SerializeField] private float dashtime = 0.2f;  // Duración del dash
    [SerializeField] private float dashCooldown = 0.8f; // Tiempo de espera antes de poder hacer otro dash

    [Header("Inputs")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference Dash;

    private Rigidbody rb;
    private PlayerInventory inventory;
    private Vector2 moveDirection;
    private Vector3 dashDirection;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private bool doingDash = false;

    private void Awake()
    {
        // Obtenemos el Rigidbody del jugador para moverlo respetando físicas y paredes
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<PlayerInventory>();
    }

    private void OnEnable()
    {
        // Activamos las acciones para que el nuevo Input System detecte las teclas
        if (move != null && move.action != null) move.action.Enable();
        if (Dash != null && Dash.action != null) Dash.action.Enable();
    }

    private void OnDisable()
    {
        // Desactivamos las acciones al desactivar el objeto para ahorrar recursos (USAR EN MENU-INGAME)
        if (move != null && move.action != null) move.action.Disable();
        if (Dash != null && Dash.action != null) Dash.action.Disable();
    }

    private void Update()
    {
        OnMove();

        // Si el cooldown está activo, reducimos el temporizador cada segundo
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Comprobamos si se ha pulsado el Dash (vía InputAction, Teclado Space/Shift o Mando)
        bool isDashPressed = false;

        if (Dash != null && Dash.action != null && (Dash.action.triggered || Dash.action.WasPressedThisFrame()))
        {
            isDashPressed = true;
        }
        else if (Keyboard.current != null && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.leftShiftKey.wasPressedThisFrame))
        {
            isDashPressed = true;
        }
        else if (Gamepad.current != null && (Gamepad.current.buttonEast.wasPressedThisFrame || Gamepad.current.rightShoulder.wasPressedThisFrame))
        {
            isDashPressed = true;
        }

        if (isDashPressed && !doingDash && dashCooldownTimer <= 0f)
        {
            ActivateDash();
        }
    }

    // Llamado automáticamente por PlayerInput si tiene NotificationBehavior en SendMessage
    public void OnDash(InputValue value)
    {
        if (value != null && value.isPressed && !doingDash && dashCooldownTimer <= 0f)
        {
            ActivateDash();
        }
    }

    private void FixedUpdate()
    {
        if (doingDash)
        {
            // Movemos al jugador en la dirección del dash con fuerza
            Vector3 dashMovement = dashDirection * dashpower * Time.fixedDeltaTime;

            if (rb != null)
                rb.MovePosition(rb.position + dashMovement);
            else
                transform.position += dashMovement;

            // Restamos tiempo al dash hasta que se termine
            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0f)
            {
                doingDash = false;
            }

            return;
        }

        // Movimiento normal
        Vector3 movement = new Vector3(moveDirection.x, 0f, moveDirection.y);
        Vector3 deltaMovement = movement * moveSpeed * Time.fixedDeltaTime;

        if (rb != null)
            rb.MovePosition(rb.position + deltaMovement);
        else
            transform.position += deltaMovement;

        // Rotación suave hacia donde nos estamos moviendo
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnMove()
    {
        if (move != null && move.action != null)
        {
            moveDirection = move.action.ReadValue<Vector2>();
        }
        else if (Keyboard.current != null)
        {
            // Respaldo directo por si move no estuviera asignado
            float x = 0f;
            float y = 0f;
            if (Keyboard.current.wKey.isPressed) y += 1f;
            if (Keyboard.current.sKey.isPressed) y -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            moveDirection = new Vector2(x, y).normalized;
        }
    }

    private void ActivateDash()
    {
        doingDash = true;

        // Aseguramos que dashtime nunca sea 0 aunque en el Inspector de Unity se haya quedado en 0
        if (dashtime <= 0f)
        {
            dashtime = 0.2f;
        }
        dashTimer = dashtime;
        dashCooldownTimer = dashCooldown;

        // 1. Leemos la dirección de entrada actual (WASD o Stick)
        Vector3 inputDirection = new Vector3(moveDirection.x, 0f, moveDirection.y);

        // 2. Si te estás moviendo hacia alguna dirección, dasheamos hacia ahí; si estás quieto, hacia el frente
        if (inputDirection.sqrMagnitude > 0.01f)
        {
            dashDirection = inputDirection.normalized;
        }
        else
        {
            dashDirection = transform.forward;
        }

        // 3. Giramos el personaje inmediatamente hacia la dirección del dash
        if (dashDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dashDirection);
            transform.rotation = targetRotation;
            if (rb != null)
            {
                rb.rotation = targetRotation;
            }
        }

        Debug.Log($"<color=cyan>[DASH ACTIVADO]</color> Dirección: {dashDirection} | Potencia: {dashpower} | Duración: {dashTimer}s");

        // 30% de probabilidad de que el item se caiga de las manos al dashear
        TryDropItemOnDash();
    }

    private void TryDropItemOnDash()
    {
        if (inventory == null || !inventory.HasItem) return;

        bool itemDrops = Random.value < 0.3f;
        if (!itemDrops) return;

        // Colocamos el item a la misma altura que el origen del raycast para que se pueda recoger
        Vector3 raycastHeight = new Vector3(0f, -0.5f, 0f);
        Vector3 dropPosition = transform.position + raycastHeight + transform.forward * 0.5f;

        if (inventory.HeldIngredient != null)
        {
            Ingredient ingredient = inventory.DropIngredient();
            ingredient.transform.position = dropPosition;
            Debug.Log($"<color=orange>[DASH] ¡Se te ha caído: {ingredient.IngredientType}!</color>");
        }
        else if (inventory.HeldBottle != null)
        {
            Bottle bottle = inventory.DropBottle();
            bottle.transform.position = dropPosition;
            Debug.Log("<color=orange>[DASH] ¡Se te ha caído la botella!</color>");
        }
    }
}
