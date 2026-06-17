using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages health for player or enemies.
/// Handles damage, healing, and death events.
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private bool isDead = false;

    [Header("Damage Settings")]
    [SerializeField] private float damageFlashDuration = 0.2f;

    // Events
    public UnityEvent<float> OnDamaged = new UnityEvent<float>();
    public UnityEvent<float> OnHealed = new UnityEvent<float>();
    public UnityEvent OnDeath = new UnityEvent();

    private Renderer[] renderers;
    private float damageFlashTimer = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        // Update damage flash timer
        if (damageFlashTimer > 0)
        {
            damageFlashTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        damageFlashTimer = damageFlashDuration;

        OnDamaged.Invoke(damageAmount);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        if (isDead) return;

        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        OnHealed.Invoke(healAmount);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath.Invoke();

        // Disable physics and disable object
        GetComponent<Rigidbody>().isKinematic = true;
        this.enabled = false;
    }

    public float GetHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public float GetHealthPercent() => currentHealth / maxHealth;
    public bool IsDead() => isDead;
}
