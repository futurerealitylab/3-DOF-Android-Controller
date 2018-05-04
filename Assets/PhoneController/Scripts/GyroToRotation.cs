using UnityEngine;

public class GyroToRotation: MonoBehaviour
{
    public GameObject rotationIndicater;
    public Vector3 rotationRate = new Vector3(0, 0, 0);
    public UnityEngine.Vector3 RotationRate
    {
        get { return rotationRate; }
        set { rotationRate = value; }
    }
    string frameTouchMessage = "";
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
            rotationIndicater.transform.Rotate(RotationRate);
        }
        frameTouchMessage = touchInFrames();
        //Debug.Log(frameTouchMessage);
    }

    protected void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 20;
        GUILayout.Label("rotationRate: " + Input.gyro.rotationRate);
        GUILayout.Label("currentOrientation: " + rotationIndicater.transform.rotation.eulerAngles);
        GUILayout.Label("touchInFrame: " + frameTouchMessage);
     }

    public void enableGyro(bool isOn)
    {
        Input.gyro.enabled = isOn;
    }

    public void resetGyro()
    {
        rotationIndicater.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public string touchInFrames()
    {
        string touchMessage = "";
        if (Input.touchCount > 0)
        {
            int fingerCount = 0;
            foreach (Touch touch in Input.touches)
            {
                
                touchMessage += "Index of touch in frame: ";
                touchMessage += fingerCount;
                touchMessage += ", position: ";
                touchMessage += touch.position;
                touchMessage += ", statues: ";
                touchMessage += touch.phase;
                touchMessage += (float)Screen.width;
                touchMessage += (float)Screen.height;
                touchMessage += "\n";
                fingerCount++;
            }
        }
        return touchMessage;
    }
}