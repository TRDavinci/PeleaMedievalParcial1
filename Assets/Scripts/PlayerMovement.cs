using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Dash")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

   
    [Networked] private Vector2 MoveInput { get; set; }
    [Networked] private float Rotation { get; set; }

    
    [Networked] private bool isDashing { get; set; }
    [Networked] private float dashTimer { get; set; }
    [Networked] private float dashCooldownTimer { get; set; }

    public override void Spawned()
    {
        
    }

    void Update()
    {
        if (!HasInputAuthority) return;

        
        MoveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (Vector2)mouse - (Vector2)transform.position;

        Rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        
        if (Input.GetKeyDown(KeyCode.Space) &&
            MoveInput != Vector2.zero &&
            !isDashing &&
            dashCooldownTimer <= 0f)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        Vector3 move = new Vector3(MoveInput.x, MoveInput.y, 0).normalized;

        // DASH
        if (isDashing)
        {
            transform.position += move * dashSpeed * Runner.DeltaTime;

            dashTimer -= Runner.DeltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
        else
        {
            // Movimiento normal
            transform.position += move * speed * Runner.DeltaTime;
        }

        // Cooldown del dash
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Runner.DeltaTime;
        }

        // Rotación del player
        transform.rotation = Quaternion.Euler(0, 0, Rotation);
    }
}