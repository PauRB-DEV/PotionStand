using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private InputActionReference move;

    private Vector2 moveDirection;

    private void Update()
    {
        OnMove();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveDirection.x, 0f, moveDirection.y);
        transform.position += movement * moveSpeed * Time.deltaTime;
        if (movement != Vector3.zero)
        {
            Vector3 lookDirection;

            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                // Izquierda o derecha
                lookDirection = moveDirection.x > 0 ? Vector3.right : Vector3.left;
            }
            else
            {
                // Arriba o abajo
                lookDirection = moveDirection.y > 0 ? Vector3.forward : Vector3.back;
            }

            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnMove()
    {
        moveDirection = move.action.ReadValue<Vector2>();
    }
}
