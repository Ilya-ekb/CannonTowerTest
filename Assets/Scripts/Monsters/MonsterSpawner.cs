using Configs;
using UnityEngine;

namespace Monsters
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private Monster prefab;
        [SerializeField] private MonsterSpawnConfig config;
        [SerializeField] private Transform monsterTarget;
        private float timer;

        private void Start()
        {
            timer = config.spawnInterval;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= config.spawnInterval)
            {
                timer = 0f;
                Vector3 pos = transform.position;

                var t = Instantiate(prefab, pos, Quaternion.identity);
                t.name = $"Target_{Time.frameCount}";
                t.GetComponent<Monster>().Setup(config, monsterTarget);
            }
        }
    }
}