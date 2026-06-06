using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Level
{
    public class GroundSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform player;
        [SerializeField] private GameObject groundPrefab;

        [Header("Settings")]
        [SerializeField] private int tilesCount = 8;
        [SerializeField] private float tileLength = 20f;
        [SerializeField] private float spawnDistance = 60f;

        private readonly Queue<GameObject> _tiles = new();

        private float _nextSpawnZ;

        private void Start()
        {
            for (int i = 0; i < tilesCount; i++)
            {
                SpawnTile();
            }
        }

        private void Update()
        {
            if (player.position.z + spawnDistance > _nextSpawnZ)
            {
                SpawnTile();

                GameObject oldTile = _tiles.Dequeue();

                oldTile.transform.position =
                    new Vector3(0, 0, _nextSpawnZ - tileLength);

                _tiles.Enqueue(oldTile);
            }
        }

        private void SpawnTile()
        {
            GameObject tile;

            if (_tiles.Count < tilesCount)
            {
                tile = Instantiate(
                    groundPrefab,
                    new Vector3(0, 0, _nextSpawnZ),
                    Quaternion.identity);

                _tiles.Enqueue(tile);
            }

            _nextSpawnZ += tileLength;
        }
    }
}