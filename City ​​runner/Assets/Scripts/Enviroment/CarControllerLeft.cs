using System.Collections;
using UnityEngine;

public class CarControllerLeft : MonoBehaviour
{
    public GameObject[] carPrefabs; 
    public Transform spawnPoint; 
    public float spawnIntervalMin = 1f; 
    public float spawnIntervalMax = 5f; 
    public float moveTime = 10f; 
    public float carSpeed = 1f; 

    void Start()
    {
        StartCoroutine(SpawnCars());
    }

    IEnumerator SpawnCars()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));

            
            GameObject carPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

            
            GameObject car = Instantiate(carPrefab, spawnPoint.position, Quaternion.Euler(0, 180, 0));

            car.SetActive(true);

            
            StartCoroutine(MoveAndDestroyCar(car));
        }
    }

    IEnumerator MoveAndDestroyCar(GameObject car)
    {
        float startTime = Time.time;

        while (Time.time - startTime < moveTime)
        {
            car.transform.Translate(Vector3.forward * carSpeed * Time.deltaTime);
            yield return null;
        }

        
        Destroy(car);
    }
}
