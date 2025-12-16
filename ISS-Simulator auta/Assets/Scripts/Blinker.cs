using System.Collections;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    public Light blinkerLight;
    public float blinkInterval = 0.5f;

    private bool isBlinking = false;
    private Coroutine blinkCoroutine;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!isBlinking)
            {
                blinkCoroutine = StartCoroutine(Blink());
                isBlinking = true;
            }
            else
            {
                StopCoroutine(blinkCoroutine);
                blinkerLight.enabled = false;
                isBlinking = false;
            }
        }
    }

    IEnumerator Blink()
    {
        while (true)
        {
            blinkerLight.enabled = !blinkerLight.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
