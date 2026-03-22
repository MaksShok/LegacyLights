using System;
using System.Collections;
using _main.ServiceLoc;
using Player;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Config")]
    public WeaponConfig weaponConfig;
    
    [Header("References")]
    public Transform attackPoint;
    public LayerMask enemyLayers;

    private InputController _inputController;
    private float _lastAttackTime;
    private bool _isAttacking;
    private Animator _animator;

    public float attackRange => weaponConfig != null ? weaponConfig.attackRange : 1.5f;
    public float attackRadius => weaponConfig != null ? weaponConfig.attackRadius : 0.8f;
    public float attackDamage => weaponConfig != null ? weaponConfig.attackDamage : 10f;
    public float attackCooldown => weaponConfig != null ? weaponConfig.attackCooldown : 0.5f;
    public float attackDuration => weaponConfig != null ? weaponConfig.attackDuration : 0.3f;
    public float attackAnticipation => weaponConfig != null ? weaponConfig.attackAnticipation : 0.1f;

    private void Start()
    {
        _inputController = ServiceLocator.Current.Get<InputController>();

        if (attackPoint == null)
        {
            GameObject attackPointObj = new GameObject("AttackPoint");
            attackPointObj.transform.parent = transform;
            attackPoint = attackPointObj.transform;
        }
    }

    private void Update()
    {
        if (_inputController.CheckAttackInput())
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (Time.time > _lastAttackTime + attackCooldown && !_isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        _isAttacking = true;
        _lastAttackTime = Time.time;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (_animator != null)
        {
            _animator.SetTrigger("AttackAnticipation");
        }

        yield return new WaitForSeconds(attackAnticipation);

        UpdateAttackPointPosition();
        PerformHit();

        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(attackDuration - attackAnticipation);

        if (_animator != null)
        {
            _animator.SetTrigger("AttackRecovery");
        }

        _isAttacking = false;
    }

    private void UpdateAttackPointPosition()
    {
        float direction = transform.rotation.eulerAngles.z;
        Vector3 offset = new Vector3(0, -attackRange / 2, 0);
        attackPoint.localPosition = Quaternion.Euler(0, 0, direction) * offset;
    }

    private void PerformHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Ударил врага: " + enemy.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(attackPoint.position, attackPoint.position + Vector3.down * 0.5f);
        }
    }
}
