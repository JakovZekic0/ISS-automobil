using UnityEngine;

public class FogToggle : MonoBehaviour
{
    private bool fogOn = true;

    void Start()
    {
        RenderSettings.fog = fogOn; // Postavi početnu vrednost (fog uključen)
    }

    void Update()
    {
        // Ako pritisneš taster "U" na tastaturi, menjaće se stanje foga
        if (Input.GetKeyDown(KeyCode.U))
        {
            fogOn = !fogOn;
            RenderSettings.fog = fogOn; // Pali/gasi fog
        }
    }
}
