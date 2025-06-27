using UnityEngine;

public class BillboardInfo : MonoBehaviour
{
    void Update()
    {
        // Make the object face the camera
        Vector3 direction = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // Keep Y-axis only if needed
    }
}
