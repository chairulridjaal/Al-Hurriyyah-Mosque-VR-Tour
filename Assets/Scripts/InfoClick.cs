using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class InfoClick : MonoBehaviour, ICloseablePanel
{
    [Header("Info Settings")]
    public string message = "This is an information point.";
    public string title = "Information";

    [Header("UI Panel")]
    public GameObject infoPanelPrefab;
    private GameObject spawnedPanel;
    public Transform arrowParent;   

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnSelected);
        }
        else
        {
            Debug.LogError("❌ XRSimpleInteractable missing on: " + gameObject.name);
        }
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        if (spawnedPanel != null) return;

        // Hide this info point visually
        gameObject.SetActive(false);

        // Spawn panel in same position
        spawnedPanel = Instantiate(infoPanelPrefab, transform.position, Quaternion.identity);

        // Make it face the camera
        Vector3 toCamera = Camera.main.transform.position - transform.position;
        spawnedPanel.transform.rotation = Quaternion.LookRotation(toCamera);
        spawnedPanel.transform.Rotate(0, 180f, 0); // flip if needed

        if (arrowParent != null)
        spawnedPanel.transform.SetParent(arrowParent, false);

        // Set text
        TMP_Text titleText = spawnedPanel.transform.Find("Background/title")?.GetComponentInChildren<TMP_Text>();
        if (titleText != null)
        {
            titleText.text = title;
        }

        // Set text
        TextMeshProUGUI text = spawnedPanel.transform.Find("Background/text")?.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = message;
        }

        // Hook close button
        CloseButton close = spawnedPanel.GetComponentInChildren<CloseButton>();
        if (close != null)
        {
            close.Setup(this);
        }
    }

    public void ClosePanel()
    {
        if (spawnedPanel != null)
        {
            Destroy(spawnedPanel);
            spawnedPanel = null;
        }

        gameObject.SetActive(true); // Show info point again
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelected);
        }
    }
}
