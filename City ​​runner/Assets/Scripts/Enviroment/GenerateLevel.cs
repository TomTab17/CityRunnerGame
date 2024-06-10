using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRunner : MonoBehaviour
{
    public GameObject[] scenarioPrefabs;
    public GameObject firstPrefab;
    public Transform player;
    public float scenarioSpawnOffset = 20f;
    public float destroyDistance = 20f;
    private int lastScenarioIndex = -1;

    private List<GameObject> spawnedScenarios = new List<GameObject>();
    private Vector3 spawnPosition = new Vector3(8.9f, 3.2f, 0);

    void Start()
    {
        GameObject spawnedScenario = Instantiate(firstPrefab, spawnPosition, Quaternion.identity);
        spawnedScenario.SetActive(true);
        spawnedScenarios.Add(spawnedScenario);
        spawnPosition.z += scenarioSpawnOffset;

        for (int i = 0; i < 3; i++)
        {
            SpawnScenario();
        }
    }

    void Update()
    {

        if (spawnedScenarios.Count > 0 && player.position.z - spawnedScenarios[0].transform.position.z > destroyDistance)
        {
            DestroyScenario();
            SpawnScenario();

        }
    }

    void SpawnScenario()
    {
        int scenarioIndex;
        do
        {
            scenarioIndex = Random.Range(0, scenarioPrefabs.Length);
        }
        while (scenarioIndex == lastScenarioIndex);

        lastScenarioIndex = scenarioIndex;

        GameObject spawnedScenario = Instantiate(scenarioPrefabs[scenarioIndex], spawnPosition, Quaternion.identity);
        spawnedScenario.SetActive(true);
        spawnedScenarios.Add(spawnedScenario);

        spawnPosition.z += scenarioSpawnOffset;
    }

    

    void DestroyScenario()
    {
        GameObject scenarioToRemove = spawnedScenarios[0];
        spawnedScenarios.RemoveAt(0);
        Destroy(scenarioToRemove);
    }
}
