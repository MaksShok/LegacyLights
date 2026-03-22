using System;
using _main.ServiceLoc;
using Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;

    [SerializeField]
    private Rigidbody2D _rb;

    private InputController _inputController;

    private void Start()
    {
        _inputController = ServiceLocator.Current.Get<InputController>();

        _rb = GetComponent<Rigidbody2D>();

        if (_rb != null)
        {
            _rb.gravityScale = 0f;
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = _inputController.MoveInput();
        Move(moveInput);
        RotateTowardsMouse();
    }

    private void Move(Vector2 inputVector)
    {
        if (_rb == null) return;

        Vector2 velocity = inputVector.normalized * Speed;
        _rb.linearVelocity = velocity;
    }

    private void RotateTowardsMouse()
    {
        if (_rb == null) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPos - (Vector2)_rb.position;

        if (direction.x < 0)
        {
            _rb.rotation = 180f;
        }
        else
        {
            _rb.rotation = 0f;
        }
    }
}
