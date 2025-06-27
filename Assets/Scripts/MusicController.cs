using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource; // Drag your AudioSource here in the Inspector

    [Range(0f, 1f)]
    public float volume = 1f; // Control this in Inspector or script

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            audioSource.Play(); // Start playing the song
        }
    }

    void Update()
    {
        // Dynamically update volume from script/Inspector
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }

        // Optional: Press up/down arrow to change volume
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            volume = Mathf.Clamp01(volume + 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            volume = Mathf.Clamp01(volume - 0.1f);
        }
    }
}
