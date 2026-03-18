using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public Rigidbody2D rb;
    
    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackRadius = 0.8f;
    public float attackDamage = 10f;
    public float attackCooldown = 0.5f;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    
    [Header("Attack Animation")]
    public float attackDuration = 0.3f;
    public float attackAnticipation = 0.1f;
    
    private InputController _inputController;
    private float lastAttackTime;
    private bool isAttacking = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
        }
        
        if (attackPoint == null)
        {
            GameObject attackPointObj = new GameObject("AttackPoint");
            attackPointObj.transform.parent = transform;
            attackPoint = attackPointObj.transform;
        }
    }

    void Update()
    {
        if (!isAttacking)
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
        
        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }
    
    System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (animator != null)
        {
            animator.SetTrigger("AttackAnticipation");
        }
        
        yield return new WaitForSeconds(attackAnticipation);

        UpdateAttackPointPosition();

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);

            Debug.Log("Ударил врага: " + enemy.name);
        }

        yield return new WaitForSeconds(attackDuration - attackAnticipation);
        
        if (animator != null)
        {
            animator.SetTrigger("AttackRecovery");
        }
        
        isAttacking = false;
    }
    
    void UpdateAttackPointPosition()
    {
        float direction = transform.rotation.eulerAngles.z;

        Vector3 offset = new Vector3(0, -attackRange/2, 0);

        attackPoint.localPosition = Quaternion.Euler(0, 0, direction) * offset;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(attackPoint.position, attackPoint.position + Vector3.down * 0.5f);
        }
    }
    
    public void Initialize(InputController inputController)
    {
        _inputController = inputController;
        if (_inputController != null)
        {
            _inputController.OnMoveInput += Move;
        }
    }
    
    private void Move(Vector2 inputVector)
    {
    }
}