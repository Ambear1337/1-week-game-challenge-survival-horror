using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyStats : MonoBehaviour
    {
        [SerializeField] private EnemyStateManager manager;
        [SerializeField] private int health = 100;

        private void Awake()
        {
            manager = GetComponent<EnemyStateManager>();
        }

        public void GetDamage(int damage)
        {
            manager.SwitchState(manager.idleState);
            
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                Death();
            }
        }

        private void Death()
        {
            manager.animator.enabled = false;
            manager.enabled = false;
        }
    }
}
