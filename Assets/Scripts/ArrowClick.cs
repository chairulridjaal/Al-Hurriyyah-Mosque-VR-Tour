using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowClick : MonoBehaviour
{
    public string targetLocationId;
    public PanoramaManager manager;

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
        Debug.Log($"✅ Arrow selected → Loading: {targetLocationId}");
        manager.LoadLocation(targetLocationId);
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelected);
        }
    }
}
