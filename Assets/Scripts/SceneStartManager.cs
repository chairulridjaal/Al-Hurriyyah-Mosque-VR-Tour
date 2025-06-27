using UnityEngine;

public class SceneStartManager : MonoBehaviour
{
    public Transform xrRigRoot; // Your "XR Rig" GameObject
    public Transform cameraOffset; // The child named "Camera Offset"
    public GameObject xrDeviceSimulator;

    void Start()
    {
        if (xrRigRoot != null)
        {
            xrRigRoot.position = Vector3.zero;
            xrRigRoot.rotation = Quaternion.identity;
        }

        if (cameraOffset != null)
        {
            cameraOffset.localPosition = Vector3.zero;
            cameraOffset.localRotation = Quaternion.identity;
        }

        if (xrDeviceSimulator != null)
        {
            xrDeviceSimulator.SetActive(false);
        }

        Debug.Log("✅ XR Rig and Camera Offset reset to (0, 0, 0)");
    }
}
