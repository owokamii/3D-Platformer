using UnityEngine;

public class PlayerInputCheck : MonoBehaviour
{
    private Player player;

    [SerializeField] private float idleSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintSpeed;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        // check if player presses left right, or forward backward
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if(Input.GetKey(KeyCode.LeftShift))
        {
            player.SetSprintingStatus(true, sprintSpeed);
        }
        else
        {
            player.SetSprintingStatus(false, 0);
        }

        ///////////////////////////////////////////////////////////

        if (movement.magnitude > 0)
        {
            player.SetMovingStatus(true, moveSpeed);
        }
        else
        {
            player.SetMovingStatus(false, 0);
        }

        player.MovePlayer(movement);
    }


}
