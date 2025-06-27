using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class NarratedEvent
{
    public float triggerTime;             // in seconds
    public string panoramaLocationId;     // to load in PanoramaManager
}

public class NarratedModeController : MonoBehaviour
{
    [Header("Narration Audio")]
    public AudioSource narrationAudio;

    [Header("Panorama")]
    public PanoramaManager panoramaManager;

    [Header("Narrated Events")]
    public List<NarratedEvent> events = new List<NarratedEvent>();

    [Header("UI Buttons")]
    public Button pauseResumeButton;
    public TMP_Text pauseButtonText;

    private int currentEventIndex = 0;
    private bool isPlaying = false;

    void Start()
    {
        Debug.Log("🔄 NarratedModeController Start");
        events.Sort((a, b) => a.triggerTime.CompareTo(b.triggerTime));

        if (pauseResumeButton != null)
            pauseResumeButton.onClick.AddListener(TogglePlayPause);

        StartNarration();
    }

    void Update()
    {
        if (!isPlaying || narrationAudio == null || currentEventIndex >= events.Count)
            return;
        if (narrationAudio.time >= events[currentEventIndex].triggerTime)
        {
            Debug.Log($"🎯 Triggering event {currentEventIndex} at {narrationAudio.time}");
            TriggerEvent(events[currentEventIndex]);
            currentEventIndex++;
        }
    }

    void StartNarration()
    {
        Debug.Log("🎙 StartNarration called");

        isPlaying = true;
        currentEventIndex = 0;

        if (narrationAudio != null)
        {
            narrationAudio.time = 0f;
            narrationAudio.Play();
        }

        UpdatePauseButtonText();
    }

    void TogglePlayPause()
    {
        if (narrationAudio == null) return;

        isPlaying = !isPlaying;

        if (isPlaying)
            narrationAudio.Play();
        else
            narrationAudio.Pause();

        UpdatePauseButtonText();
    }

    void UpdatePauseButtonText()
    {
        if (pauseButtonText != null)
        {
            pauseButtonText.text = isPlaying ? "Pause" : "Resume";
        }
    }

    void TriggerEvent(NarratedEvent evt)
    {
        if (panoramaManager != null && !string.IsNullOrEmpty(evt.panoramaLocationId))
        {
            panoramaManager.LoadLocation(evt.panoramaLocationId);
        }
    }
}
