using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public Rigidbody2D rb;

    private InputController _inputController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        Vector2 movement = new Vector2(moveX, moveY);

        movement = movement.normalized;

        if (rb != null)
        {
            rb.linearVelocity = movement * Speed;
        }
        else
        {
            transform.Translate(movement * Speed * Time.deltaTime);
        }
    }

    public void Initialize(InputController inputController)
    {
        _inputController.OnMoveInput += Move;
    }
    private void Move(Vector2 inputVector)
    {
        
    }
}
