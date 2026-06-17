using UnityEngine;

/// <summary>
/// Base weapon system for handling attacks, cooldowns, and damage.
/// </summary>
public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRange = 100f;

    [Header("Fire Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask hitLayer;

    private float lastAttackTime = -1f;
    private bool canAttack => Time.time - lastAttackTime >= attackCooldown;

    private void Start()
    {
        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    public bool Attack()
    {
        if (!canAttack) return false;

        lastAttackTime = Time.time;
        PerformAttack();
        return true;
    }

    private void PerformAttack()
    {
        // Raycast from fire point
        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, attackRange, hitLayer))
        {
            // Deal damage to hit object
            Health healthComponent = hit.collider.GetComponent<Health>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
                Debug.Log($"Hit {hit.collider.name} for {damage} damage!");
            }
        }
    }

    public float GetAttackCooldownPercent()
    {
        return Mathf.Clamp01(1f - (Time.time - lastAttackTime) / attackCooldown);
    }

    public bool CanAttack() => canAttack;
    public void SetDamage(float newDamage) => damage = newDamage;
}
