using System.Collections;
using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;



public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    NetworkRigidbody2D _rb;
    Vector2 moveInput;

    [Header("Dash Settings")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    [Networked] public bool isDashing { get; set; }
    float dashTimer;

    public override void Spawned()
    {
        //if (!HasStateAuthority) return;
        _rb = GetComponent<NetworkRigidbody2D>();
    }
    private void Update()
    {
        if (!HasStateAuthority) return;
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        Vector3 mouse=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir=(Vector2)mouse-(Vector2)transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) *Mathf.Rad2Deg; //Atan2 Calcula la ArcoTangente en radianes, multiplicarlo por Rad2Deg lo deja en grados.
        _rb.Rigidbody.rotation = angle;

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > dashTimer && moveInput != Vector2.zero)
        {
            Dash();
        }       
    }
    IEnumerator PerformDash()
    {
        isDashing = true;
        dashTimer = Time.time + dashCooldown;


        Vector2 dashDir = moveInput.normalized;

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            _rb.Rigidbody.linearVelocity = dashDir * dashForce;
            yield return null;
        }
        _rb.Rigidbody.linearVelocity = Vector2.zero;
        isDashing = false;
    }

    public void Dash()
    {
        StartCoroutine(PerformDash());
    }

    public void Movement()
    {
        Vector2 nextPos = _rb.Rigidbody.position + moveInput.normalized * speed * Runner.DeltaTime;
        _rb.Rigidbody.MovePosition(nextPos);
    }

    public override void FixedUpdateNetwork()
    {
        if (!isDashing)
        {
            Movement();
        }
        
    }
}
