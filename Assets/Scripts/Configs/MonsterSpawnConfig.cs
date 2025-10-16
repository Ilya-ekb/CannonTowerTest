using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "MonsterSpawnConfig", menuName = "Configs/Monster Spawn Config")]
    public class MonsterSpawnConfig : ScriptableObject
    {
        public float spawnInterval = 5f;
        public float monsterSpeed = 3f;
        public int monsterHp = 20; 
    }
}