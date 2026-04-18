using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rb;
    Vector2 moveInput;

    [Header("Dash Settings")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    bool isDashing;
    float dashTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        Vector3 mouse=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir=(Vector2)mouse-rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) *Mathf.Rad2Deg; //Atan2 Calcula la ArcoTangente en radianes, multiplicarlo por Rad2Deg lo deja en grados.
        rb.rotation = angle;

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > dashTimer && moveInput != Vector2.zero)
        {
            StartCoroutine(PerformDash());
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
            rb.linearVelocity = dashDir * dashForce;
            yield return null;
        }

        isDashing = false;
    }



    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.MovePosition(rb.position + moveInput.normalized * speed * Time.fixedDeltaTime);
        }
        
    }
}
