using UnityEngine;

public class VehicleHorn : MonoBehaviour
{
    public AudioSource hornSource;
    public Transform playerCar;
    public float honkDistance = 10f;
    public float honkCooldown = 5f;

    private float lastHonkTime = -10f; // moze potrubiti odmah na pocetku

    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerCar.position); // izracunava udaljenost izmedju sporednih vozila i playera u metrima

        if (distance < honkDistance && Time.time - lastHonkTime > honkCooldown) // manja udaljenost od zadane i prosao je cooldown
        {
            // 50% sanse da potrubi kada je blizu
            if (Random.value > 0.5f)
            {
                hornSource.PlayOneShot(hornSource.clip, 5.0f); // glasniji zvuk jer se nije dobro cuo
                lastHonkTime = Time.time; // sad je potrubio
            }
        }
    }
}
