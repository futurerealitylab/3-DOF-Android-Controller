# 2018/04/03
## GyroScope Controller
* Build Scene/GyroScope_Rotation.unity into any Targeted Android phones
  * Should get the gyro fast enough
  * the toggle in the left down corner is for open/close gyro, and the button in the right down corner is for recenter the targeted object
  * methods for the case is by calling `Input.gyro.rotationRate` instead of `Input.gyro.attitude`, in which will boost the speed because of some idoit issues from Unity to Android
* For Zhenyi to broadcast
  * Recommanded to broadcast the `Vector3 rotationRate`, and calculate the `Rotation` inside the client server, by using `Transform.rotate()` methods, recenter will be easy since all we need to do is to recent the target object, instead of operate some Quaternion, which will always have some issue in the math side
