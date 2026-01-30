using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovementYs : MonoBehaviour
{
    bool hasDied = false;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float deathAnimationDuration = 1.5f;

    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Vector3 initialScale;

    [Header("Animation Parameters")]
    [SerializeField] string runBoolParam = "isRuning";
    [SerializeField] string jumpBoolParam = "isJamping";

    [Header("Ground Check")]
    [Tooltip("Transform used as the origin for ground checks")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        initialScale = transform.localScale;
        
        // Reset the bridge destroyed state at the start of each level
        bridgeDestroyed.isDead = false;
    }

    void Update()
    {
        UpdateAnimation();
        Die();
    }

    void FixedUpdate()
    {
        Run();
    }

    void LateUpdate()
    {
        FlipSprite();
    }

    // INPUT SYSTEM METHODS (CASE SENSITIVE)
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (hasDied) return;
        // only jump if grounded (simple check); allow the animator to play jump state immediately
        if (value.isPressed /* && isGrounded */)
        {
            myRigidBody.linearVelocity = new Vector2(myRigidBody.linearVelocity.x, jumpSpeed);
            if (myAnimator != null)
            {
                myAnimator.SetBool(jumpBoolParam, true);
            }
        }
    }

    void Run()
    {
        if (hasDied) return;
        myRigidBody.linearVelocity = new Vector2(moveInput.x * moveSpeed, myRigidBody.linearVelocity.y);
    }

    void UpdateAnimation()
    {
        if (myAnimator == null) return;

        // update grounded state (use groundCheck when provided, otherwise fall back to vertical velocity)
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
        }
        else
        {
            isGrounded = Mathf.Abs(myRigidBody.linearVelocity.y) < 0.01f;
        }

        bool isMoving = Mathf.Abs(myRigidBody.linearVelocity.x) > 0.1f;
        myAnimator.SetBool(runBoolParam, isMoving);

        bool isJumping = !isGrounded || Mathf.Abs(myRigidBody.linearVelocity.y) > 0.1f;
        myAnimator.SetBool(jumpBoolParam, isJumping);
    }

    void FlipSprite()
    {
        if (Mathf.Abs(myRigidBody.linearVelocity.x) > 0.1f)
        {
            Vector3 newScale = initialScale;
            newScale.x = Mathf.Sign(myRigidBody.linearVelocity.x) * Mathf.Abs(initialScale.x);
            transform.localScale = newScale;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

void Die()
{
    if (bridgeDestroyed.isDead && !hasDied)
    {
        hasDied = true;
        myAnimator.SetTrigger("Dying");

        // Stop movement
        myRigidBody.linearVelocity = Vector2.zero;
        enabled = false; // disables movement script

        // Start coroutine to wait for death animation and reload scene
        StartCoroutine(WaitForDeathAnimationAndReload());
    }
}

IEnumerator WaitForDeathAnimationAndReload()
{
    // Wait for the death animation to complete
    yield return new WaitForSeconds(deathAnimationDuration);
    
    // Reset coins for the new level
    CoinPickUp.coins = 0;
    
    // Reload the scene
    SceneManager.LoadScene("level0");
}

}
