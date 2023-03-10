using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxLives = 3;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float invincibilityTime = 3f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private int currentLives;
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentLives = maxLives;
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;

        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        rb.MovePosition(transform.position + movement);

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer > invincibilityTime)
            {
                isInvincible = false;
                invincibilityTimer = 0f;
            }
        }
    }

    void Update()
    {
        if (isGrounded && Input.GetButtonDown("Jump") && !isInvincible)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
     
        if ( collision.gameObject.layer == LayerMask.NameToLayer("Ground")
          )
        {
            isGrounded = true;

            currentLives--;
            Debug.Log("Lives: " + currentLives);
            if (currentLives == 0)
            {
                ResetPlayer();
            }
            else
            {
                isInvincible = true;
            }
        }
    }

    void ResetPlayer()
    {
        currentLives = maxLives;
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        isInvincible = true;
    }
}