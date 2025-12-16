using UnityEngine;

public class HeadlightsToggle : MonoBehaviour
{
    public Light[] headlights;

    private bool lightsOn = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            lightsOn = !lightsOn;

            foreach (Light light in headlights)
            {
                light.enabled = lightsOn;
            }
        }
    }
}
