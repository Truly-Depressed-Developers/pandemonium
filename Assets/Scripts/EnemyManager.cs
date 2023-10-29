using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager> {
    [SerializeField] private Ghost ghostPrefab;

    public void SpawnGhost(Vector2 pos) {
        Instantiate(ghostPrefab, pos, Quaternion.identity, transform);
    }
}
