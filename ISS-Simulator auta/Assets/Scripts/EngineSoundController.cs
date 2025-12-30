using UnityEngine;

public class EngineSoundController : MonoBehaviour
{
    public AudioSource idle;
    public AudioSource low;
    public AudioSource med;
    public AudioSource high;

    public Rigidbody carRb;

    public float maxSpeed = 160f;

    void Start()
    { // pokreni sve zvukove
        idle.Play();
        low.Play();
        med.Play();
        high.Play();
    }

    void Update()
    {
        float speed = carRb.velocity.magnitude * 3.6f; // pretvori u km/h
        float t = Mathf.Clamp01(speed / maxSpeed); // ako je brzina veca od maksimalne, onda ce t biti 1

        // osiguraj da su svi zvukovi iskljuceni
        idle.volume = 0;
        low.volume = 0;
        med.volume = 0;
        high.volume = 0;

        if (t < 0.25f) // postepeno pojacavanje zvuka za sporu voznju i smanjenje idle zvuka
        {
            idle.volume = Mathf.Lerp(1f, 0f, t / 0.25f); // dakle idle zvuk se smanjuje od 1 do 0, a trenutna jacina zvuka ovisi o t
            low.volume = Mathf.Lerp(0f, 1f, t / 0.25f);
        }
        else if (t < 0.5f)
        {
            low.volume = Mathf.Lerp(1f, 0f, (t - 0.25f) / 0.25f); // moramo oduzeti 0.25f od t jer se t krece od 0.25f do 0.5f
            med.volume = Mathf.Lerp(0f, 1f, (t - 0.25f) / 0.25f);
        }
        else
        {
            med.volume = Mathf.Lerp(1f, 0f, (t - 0.5f) / 0.5f);
            high.volume = Mathf.Lerp(0f, 1f, (t - 0.5f) / 0.5f);
        }

        // visina tona se povecava s brzinom
        float pitch = Mathf.Lerp(0.9f, 1.5f, t);
        idle.pitch = pitch;
        low.pitch = pitch;
        med.pitch = pitch;
        high.pitch = pitch;
    }
}
