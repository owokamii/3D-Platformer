using UnityEngine;

public interface IPlayerInputCheckable
{
    Rigidbody RB { get; set; }
    bool IsMoving { get; set; }
    bool IsSprinting { get; set; }
    void MovePlayer(Vector3 direction);
    void SetMovingStatus(bool isMoving, float moveSpeed);
    void SetSprintingStatus(bool isSprinting, float sprintSpeed);
}
