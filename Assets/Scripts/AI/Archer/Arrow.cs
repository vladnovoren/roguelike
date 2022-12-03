using UnityEngine;
using Lifetime;

namespace AI.Archer
{
    public class Arrow : MonoBehaviour
    {
        private void Awake()
        {
            _enemyHealth = enemy.GetComponent<Health>();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.position += transform.forward * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            var otherGameObject = other.gameObject;
            if (otherGameObject.GetComponent<PlayerTag>() != null)
            {
                otherGameObject.GetComponent<Health>().TakeArchDamage(_damage);
            }
            Destroy(gameObject);
        }

        [SerializeField] private GameObject enemy;
        private Health _enemyHealth;

        private float _damage = 30.0f;
    }
}
