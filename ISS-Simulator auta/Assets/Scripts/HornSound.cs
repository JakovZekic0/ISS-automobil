using UnityEngine;

public class HornSound : MonoBehaviour
{
    public AudioSource hornSource;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            hornSource.PlayOneShot(hornSource.clip); // reproducira se jednom bez prekida prethodnog
    }
}
