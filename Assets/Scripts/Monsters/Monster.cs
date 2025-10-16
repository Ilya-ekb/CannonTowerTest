using Configs;
using UnityEngine;

namespace Monsters
{
    [RequireComponent(typeof(Rigidbody))]
    public class Monster : MonoBehaviour
    {
        public Vector3 Velocity => velocity;
        
        private Vector3 velocity = Vector3.zero;
        private Vector3 targetPosition;
        private Rigidbody rb;
        private int health;
        
        public void Setup(MonsterSpawnConfig config, Transform target)
        {
            health = config.monsterHp;
            targetPosition = target.position;
            this.velocity = (target.position - transform.position).normalized * config.monsterSpeed;
        }

        public void OnHit(Projectile projectile)
        {
            health -= projectile.Damage;
            if (health <= 0f)
            {
                Debug.Log($"{name} destroyed by {projectile.name}");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log($"{name} hit! Remaining HP: {health}");
            }
        }
        
        public static Monster GetClosest(Vector3 origin, float radius)
        {
            Monster[] all = FindObjectsByType<Monster>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            Monster best = null;
            float bestDist = radius;

            foreach (var t in all)
            {
                float d = Vector3.Distance(origin, t.transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    best = t;
                }
            }

            return best;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        
        
        private void FixedUpdate()
        {
            var newPosition = rb.position + velocity * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
            if(MathUtils.ApproximatelyEqual(transform.position.magnitude, targetPosition.magnitude))
                Destroy(gameObject);
        }

    }

}