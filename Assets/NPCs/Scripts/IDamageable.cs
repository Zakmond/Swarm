public interface IDamageable
{
    void OnHit(float damage);
    void UpdateNPC(float health, float speed, float attackDistance, float attackDamage);
}