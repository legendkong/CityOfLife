using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] clouds;

    [SerializeField]
    private Transform[] spawnPoints;

    private float spawnInterval;

    public float _cloudSpawnSpeed {get; set;}

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    private void OnEnable()
    {
        Invoke("AttemptSpawn", spawnInterval);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    void SpawnCloud()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPos = spawnPoints[i];
            GameObject cloud = Instantiate(clouds[Random.Range(0, clouds.Length)]);
            float startY = Random.Range(spawnPos.position.y - 0.7f, spawnPos.position.y + 0.7f);
            float startX = Random.Range(spawnPos.position.x - 0.7f, spawnPos.position.x + 0.7f);
            cloud.transform.position = new Vector3(startX, startY, startPos.z);
        }
    }

    public void Enable(bool b)
    {
        gameObject.SetActive(b);
    }

    void AttemptSpawn()
    {
        if (gameObject.activeSelf)
        {
            SpawnCloud();
            spawnInterval = Random.Range(2.2f - _cloudSpawnSpeed, 4.2f - _cloudSpawnSpeed);
            if (spawnInterval <= 0)
            {
                spawnInterval = 0.2f;
            }
            Invoke("AttemptSpawn", spawnInterval);
        }
    }

}
