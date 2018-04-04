using UnityEngine;

public class GyroToRotation: MonoBehaviour
{
    public GameObject rotationIndicater;
    Vector3 rotationRate = new Vector3(0, 0, 0);
 
    private void Start()
    {
        resetGyro();
    }

    protected void Update()
    {
        if (Input.gyro.enabled)
        {
            rotationRate = Input.gyro.rotationRate;
            float rx = -rotationRate.x;
            float ry = -rotationRate.z;
            float rz = -rotationRate.y;
            rotationRate.x = rx;
            rotationRate.y = ry;
            rotationRate.z = rz;
            rotationIndicater.transform.Rotate(rotationRate);
        }
    }

    protected void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;
       GUILayout.Label("rotationRate: " + Input.gyro.rotationRate);
        GUILayout.Label("currentOrientation: " + rotationIndicater.transform.rotation.eulerAngles);

     }

    public void enableGyro(bool isOn)
    {
        Input.gyro.enabled = isOn;
    }

    public void resetGyro()
    {
        rotationIndicater.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}