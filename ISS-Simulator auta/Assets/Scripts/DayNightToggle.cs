using UnityEngine;

public class DayNightToggle : MonoBehaviour
{
    public Light sun;

    public Color dayAmbient = Color.white;
    public Color nightAmbient = Color.gray;

    public float dayIntensity = 1f;
    public float nightIntensity = 0.1f;

    private bool isNight = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleDayNight();
        }
    }

    void ToggleDayNight()
    {
        isNight = !isNight;

        if (isNight)
        {
            // 🌙 NIGHT
            sun.intensity = nightIntensity;
            RenderSettings.ambientLight = nightAmbient;

            // tamno plavo nebo
            Camera.main.backgroundColor = new Color(0.02f, 0.02f, 0.1f);
        }
        else
        {
            // ☀️ DAY
            sun.intensity = dayIntensity;
            RenderSettings.ambientLight = dayAmbient;

            // svijetlo plavo nebo
            Camera.main.backgroundColor = new Color(0.53f, 0.81f, 0.92f);
        }
    }
}
