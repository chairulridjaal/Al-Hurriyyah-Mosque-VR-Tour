using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    private ICloseablePanel closeable;

    public void Setup(ICloseablePanel closeRef)
    {
        closeable = closeRef;
        GetComponent<Button>().onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        closeable?.ClosePanel();
    }
}
