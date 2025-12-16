using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameobjectdestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.transform.parent.gameObject);
    }
}
