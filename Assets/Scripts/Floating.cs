using UnityEngine;

public class Floating : MonoBehaviour
{
    public float amplitude = 0.05f;
    public float frequency = 0.6f;  

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + Vector3.up * y;
    }
}
