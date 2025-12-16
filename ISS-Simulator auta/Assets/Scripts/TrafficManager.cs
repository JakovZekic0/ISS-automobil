using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [SerializeField] Transform[] lanes;
    [SerializeField] GameObject[] trafficvehicles;
      [SerializeField] CarController carController;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TrafficSpawner());
    }
    IEnumerator TrafficSpawner()
    {
         yield return new WaitForSeconds(3f);
        
        while (true)
        {
            if (carController.Carspeed() > 20f)
            {
                Spawntrafficvehicle();
            }
            yield return new WaitForSeconds(3f); 
           
        }
    }
    void Spawntrafficvehicle()
    {
        int randomlaneindex = Random.Range(0,lanes.Length);
        int trafficvehiclesindex = Random.Range(0,trafficvehicles.Length);
        Instantiate(trafficvehicles[trafficvehiclesindex],lanes[randomlaneindex].position,Quaternion.identity);
    }
    
}
