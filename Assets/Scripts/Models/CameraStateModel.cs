using System.Diagnostics;
using Unity.Collections;

[System.Serializable] // Ensure that class is not MonoBehaviour or ScriptableObject (just Plain Old C# Object)
public class CameraStateModel
{
    public bool IsDataAvailable { get; private set; }

    public delegate void CameraStateChangedHandler();
    public event CameraStateChangedHandler OnCameraStateChanged;

    public void UpdateCameraState(bool isDataAvailable)
    {
        //UnityEngine.Debug.Log($"Camera data available: {isDataAvailable}");
        if (IsDataAvailable != isDataAvailable)
        {
            IsDataAvailable = isDataAvailable;
            UnityEngine.Debug.Log($"Camera state changed:" + IsDataAvailable);
            OnCameraStateChanged?.Invoke();
        }
    }
}
