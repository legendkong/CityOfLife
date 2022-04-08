using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject human;
    [SerializeField] private GameObject protest;
    [SerializeField] private GameObject car;
    [SerializeField] private Transform[] HumanWaypointsA;
    [SerializeField] private Transform[] HumanWaypointsB;
    [SerializeField] private Transform[] CarWaypointsA;
    [SerializeField] private Transform[] CarWaypointsB;
    private int start_idx;
    private float x;
    private float y;
    private float z;

    // Start is called before the first frame update

    public void spawnHuman()
    {
        Patroller p = human.GetComponent<Patroller>();
        p.moveSpots = HumanWaypointsA;
        p.moveSpotsB = HumanWaypointsB;
        // 80% chance of spawning Route A, 20% chance of route B
        if (Random.Range(0, 10) < 8)
        {
            p.pattern = 1;
            start_idx = Random.Range(0, HumanWaypointsA.Length);
            p.moveSpotsIndex = start_idx;
            x = HumanWaypointsA[start_idx].transform.position.x;
            y = HumanWaypointsA[start_idx].transform.position.y;
            z = HumanWaypointsA[start_idx].transform.position.z;
        }
        else
        {
            p.pattern = 2;
            start_idx = Random.Range(0, HumanWaypointsB.Length);
            p.moveSpotsIndex = start_idx;
            x = HumanWaypointsB[start_idx].transform.position.x;
            y = HumanWaypointsB[start_idx].transform.position.y;
            z = HumanWaypointsB[start_idx].transform.position.z;
        }
        Instantiate(human, new Vector3(x, y, z), Quaternion.identity);
        human.tag = "Movable";
    }

    public void spawnProtest()
    {
        ProtestPatrol p = protest.GetComponent<ProtestPatrol>();
        p.moveSpots = HumanWaypointsA;
        p.moveSpotsB = HumanWaypointsB;
        // 80% chance of spawning Route A, 20% chance of route B
        if (Random.Range(0, 10) < 8)
        {
            p.pattern = 1;
            start_idx = Random.Range(0, HumanWaypointsA.Length);
            p.moveSpotsIndex = start_idx;
            x = HumanWaypointsA[start_idx].transform.position.x;
            y = HumanWaypointsA[start_idx].transform.position.y;
            z = HumanWaypointsA[start_idx].transform.position.z;
        }
        else
        {
            p.pattern = 2;
            start_idx = Random.Range(0, HumanWaypointsB.Length);
            p.moveSpotsIndex = start_idx;
            x = HumanWaypointsB[start_idx].transform.position.x;
            y = HumanWaypointsB[start_idx].transform.position.y;
            z = HumanWaypointsB[start_idx].transform.position.z;
        }
        Instantiate(protest, new Vector3(x, y, z), Quaternion.identity);
        protest.tag = "Movable";
    }
    public void spawnCar()
    {
        CarPatroller p = car.GetComponent<CarPatroller>();
        p.moveSpots = CarWaypointsA;
        p.moveSpotsB = CarWaypointsB;
        // 50% chance of spawning Route A, 50% chance of route B
        if (Random.Range(0, 10) < 5)
        {
            p.pattern = 1;
            start_idx = Random.Range(0, CarWaypointsA.Length);
            p.moveSpotsIndex = start_idx;
            x = CarWaypointsA[start_idx].transform.position.x;
            y = CarWaypointsA[start_idx].transform.position.y;
            z = CarWaypointsA[start_idx].transform.position.z;
        }
        else
        {
            p.pattern = 2;
            start_idx = Random.Range(0, CarWaypointsB.Length);
            p.moveSpotsIndex = start_idx;
            x = CarWaypointsB[start_idx].transform.position.x;
            y = CarWaypointsB[start_idx].transform.position.y;
            z = CarWaypointsB[start_idx].transform.position.z;
        }
        Instantiate(car, new Vector3(x,y,z), Quaternion.identity);
        car.tag = "Movable";
    }

    public void ClearAllMovables()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Movable"))
        {
            Destroy(obj);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            spawnHuman();
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            spawnProtest();
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            spawnCar();
        }
    }
}
