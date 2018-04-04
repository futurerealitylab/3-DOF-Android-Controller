# 2018/04/03
## GyroScope Controller
* Build Scene/GyroScope_Rotation.unity into any Targeted Android phones
  * High FPS performance
  * the toggle in the left down corner is for open/close gyro, and the button in the right down corner is for recenter the targeted object
  * Calling `Input.gyro.rotationRate` instead of `Input.gyro.attitude`, in which will boost the application because of some idoit issues from Unity to Android
* For Zhenyi if next steps is needed
  * Recommanded to broadcast the `Vector3 rotationRate`, and calculate the `Rotation` inside the client server, by using `Transform.rotate()` methods.
  * Recenter will be easy to directly recent the target object, instead of operate some Quaternion, which will always have some issue in the math side
  * 
## Next Steps
* 2D drawing traces array, rendering in screen first 
