using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrainSpawner : Spawner
{
    [SerializeField] private List<LinePath> _paths;
    [SerializeField] private float _movementSpeed;

    protected override void Awake()
    {
        _prefabs = GameManager.Instance.PedestrianPrefabs;
        base.Awake();
    }

    private void Start()
    {
        // Spawn initial wave of pedestrain
        for (int i = 0; i < _maxConcurrent; ++i) {
            StartCoroutine(_SpawnPedestrainAfterDelay());
        }
    }

    private IEnumerator _SpawnPedestrainAfterDelay()
    {
        // Wait for a random duration before spawning
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 10f));

        // Get spawn details
        LinePath path = _paths[UnityEngine.Random.Range(0, _paths.Count)];
        Tuple<SpawnPoint, SpawnPoint> points = path.GetStartAndEndPoint();
        SpawnPoint startPoint = points.Item1;
        SpawnPoint endPoint = points.Item2;
        Vector3 spawnOffset = startPoint.startDirection.x == 0 ? new Vector3(UnityEngine.Random.Range(-path.variationOffset, path.variationOffset), 0f, 0f) : new Vector3(0f, 0f, UnityEngine.Random.Range(-path.variationOffset, path.variationOffset));
        GameObject spawn = SpawnFromPool(startPoint.point.transform.position + spawnOffset, Quaternion.LookRotation(startPoint.startDirection, Vector3.up));

        // Move spawned pedestrain
        StartCoroutine(_PedestrainMovement(spawn, endPoint.point.transform.position + spawnOffset));
    }

    private IEnumerator _PedestrainMovement(GameObject pedestrain, Vector3 destination)
    {
        Vector3 startPos = pedestrain.transform.position;

        // Random movement speed
        yield return StartCoroutine(pedestrain.GetComponent<Character>().Move(startPos, destination, UnityEngine.Random.Range(0f, 2f) <= 0.3f ? 2f : 1.03f));

        // Recycle
        pedestrain.transform.GetChild(0).localRotation = Quaternion.identity; // Reset local rotation due to animation root motion
        pedestrain.SetActive(false);

        // Spawn next pedestrain
        StartCoroutine(_SpawnPedestrainAfterDelay());
    }
}
