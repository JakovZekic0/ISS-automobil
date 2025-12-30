using UnityEngine;

public class TrafficSound : MonoBehaviour
{
    public AudioSource trafficSource;
    public Rigidbody carRb; // sporedno vozilo

    void Update()
    {
        float speed = carRb.velocity.magnitude; // brzina sporednog vozila u m/s bez obzira na smjer
        float t = Mathf.Clamp01(speed / 30f); // osiguraj da je t maksimalno 1 pri vecim brzinama od 30 m/s
        trafficSource.volume = Mathf.Lerp(0.4f, 1.0f, t); // jacina zvuka ovisno o brzini
    }
}
