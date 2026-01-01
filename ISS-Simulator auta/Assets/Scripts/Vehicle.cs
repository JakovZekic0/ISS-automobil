using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] float speed=5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    // KOMENTIRANO DA SE AUTI NE ZABIJAJU U ZGRADE U WORLD SCENI
    //void Update()
    //{
    //    transform.Translate(0f,0f,speed*Time.deltaTime);
    //}
}
