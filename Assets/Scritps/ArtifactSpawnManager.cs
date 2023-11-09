using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental;
using UnityEditor.Search;
using UnityEngine;

[Serializable]
public struct SpecialArtifactSpawn
{
    public ArtifactController artifact;
    public List<Transform> spawnPoints;

}

public class ArtifactSpawnManager : MonoBehaviour
{
    [Header("Special Artifacts")]
    [Tooltip("The script will spawn the special items first.")]
    [SerializeField]
    private List<SpecialArtifactSpawn> specialArtifacts;

    [Header("Simple Artifacts")]
    [SerializeField]
    private List<ArtifactController> simpleArtifacts;
    [SerializeField]
    private List<Transform> simpleSpawnPoints;

    private void Awake()
    {
#if UNITY_EDITOR
        CheckData();
#endif

        SpawnArtifacts();
    }

    private void SpawnArtifacts()
    {
        var usedSpawnPoints = new HashSet<Transform>();
        foreach (var specialArtifact in specialArtifacts)
            SpawnArtifact(specialArtifact.artifact, specialArtifact.spawnPoints, usedSpawnPoints);

        simpleSpawnPoints.Shuffle(simpleSpawnPoints.Count * 2);
        SpawnArtifact(new Queue<ArtifactController>(simpleArtifacts), new Queue<Transform>(simpleSpawnPoints), usedSpawnPoints);
    }

#if UNITY_EDITOR
    private void CheckData()
    {
        var spawnPoints = new HashSet<Transform>(simpleSpawnPoints);
        foreach (var specialArtifact in specialArtifacts)
        {
            foreach(var spawnPoint in specialArtifact.spawnPoints)
                spawnPoints.Add(spawnPoint);
        }

        if (specialArtifacts.Count + simpleArtifacts.Count > spawnPoints.Count)
            throw new Exception("There aren't enough spawn points");
    }
#endif

    private void SpawnArtifact(ArtifactController artifact, List<Transform> spawnPoints, HashSet<Transform> usedSpawnPoints)
    {
        while(true)
        {
            var index = UnityEngine.Random.Range(0, spawnPoints.Count);
            if (!usedSpawnPoints.Contains(spawnPoints[index]))
            {
                usedSpawnPoints.Add(spawnPoints[index]);
                Instantiate(artifact, spawnPoints[index].position, Quaternion.identity, transform);
                return;
            }
        }
    }

    private void SpawnArtifact(Queue<ArtifactController> artifacts, Queue<Transform> spawnPoints, HashSet<Transform> usedSpawnPoints)
    {
        while (spawnPoints.Count > 0 && artifacts.Count > 0)
        {
            Transform spawnPoint = spawnPoints.Dequeue();
            if (usedSpawnPoints.Contains(spawnPoint))
                continue;

            var artifact = artifacts.Dequeue();
            Instantiate(artifact, spawnPoint.position, Quaternion.identity, transform);
        }
    }
}
