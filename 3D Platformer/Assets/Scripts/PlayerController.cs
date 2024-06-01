using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1;

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(inputHorizontal, 0.0f, inputVertical);

        if(movement.magnitude > 1)
        {
            movement.Normalize();
        }

        transform.Translate(movement * walkSpeed * Time.deltaTime, Space.World);
    }
}
