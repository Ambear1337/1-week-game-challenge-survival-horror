using UnityEngine;

namespace Enemy
{
    public class EnemyStats : MonoBehaviour
    {
        [SerializeField] private int health = 100;
        [SerializeField] private int maxHealth = 100;

        public void GetDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                Death();
            }
        }

        private void Death()
        {
            Debug.Log("Enemy died.");
            Destroy(gameObject);
        }
    }
}
