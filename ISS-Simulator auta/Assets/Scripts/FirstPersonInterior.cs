using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class FirstPersonInterior : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image SpeedometerNeedle;
    [SerializeField] private RawImage SteeringWheel;
    [SerializeField] private CarController carController;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI drivingMode;


    float steeringWheelCurrentAngle;
    float needleCurrentAngle;
    float carSpeed;
    void Start()
    {
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (canvasGroup.alpha == 0) canvasGroup.alpha = 1;
            else canvasGroup.alpha = 0;
        }

        if (carController.useDifferentialMotion)
        {
            drivingMode.text = "Model: Dynamic";
        }
        else
        {
            drivingMode.text = "Model: Simple";
        }

        //steering wheel
        float steerInput = Input.GetAxis("Horizontal"); // -1 .. 1
        float targetAngle = -steerInput * 90;
        steeringWheelCurrentAngle = Mathf.Lerp(steeringWheelCurrentAngle, targetAngle, Time.deltaTime * 8f);
        SteeringWheel.transform.localRotation = Quaternion.Euler(0f, 0f, steeringWheelCurrentAngle);

        //speed info
        carSpeed = Mathf.Floor(carController.Carspeed());
        text.text = $"{(int)carSpeed} Kmh";

        //speedometer needle
        float needleAngle = (carSpeed / 120) * -270f;
        needleCurrentAngle = Mathf.Lerp(needleCurrentAngle, needleAngle, Time.deltaTime * 8f);
        SpeedometerNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, needleCurrentAngle);


    }
}
