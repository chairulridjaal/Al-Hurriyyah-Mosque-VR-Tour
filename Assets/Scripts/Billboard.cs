using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        // Make the object face the camera
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // Optional: lock to Y axis only
    }
}
