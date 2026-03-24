using _main.ServiceLoc;
using InputControl;
using UnityEngine;
using WeaponModule;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField] 
    private SimpleWeapon _weapon;

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

    private void Update()
    {
        if (_inputController.CheckAttackInput())
        {
            _weapon.Attack();
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = _inputController.MoveInput();
        Move(moveInput);
    }

    private void Move(Vector2 inputVector)
    {
        if (_rb == null) return;

        Vector2 velocity = inputVector.normalized * Speed;
        _rb.linearVelocity = velocity;
    }
}
