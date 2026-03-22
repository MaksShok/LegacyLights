using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/Weapon")]
public class WeaponConfig : ScriptableObject
{
    public float attackRange = 1.5f;
    public float attackRadius = 0.8f;
    public float attackDamage = 10f;
    public float attackCooldown = 0.5f;
    public float attackDuration = 0.3f;
    public float attackAnticipation = 0.1f;
}
