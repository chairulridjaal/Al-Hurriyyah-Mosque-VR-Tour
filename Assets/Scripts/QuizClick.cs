using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuizClick : MonoBehaviour, ICloseablePanel
{
    [Header("Quiz Settings")]
    public string question = "This is a quiz question.";
    public List<string> answers;
    public int correctAnswerIndex = 0; 

    [Header("Quiz Panel")]
    public GameObject QuizPanelPrefab;
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
        spawnedPanel = Instantiate(QuizPanelPrefab, transform.position, Quaternion.identity);

        // Make it face the camera
        Vector3 toCamera = Camera.main.transform.position - transform.position;
        spawnedPanel.transform.rotation = Quaternion.LookRotation(toCamera);
        spawnedPanel.transform.Rotate(0, 180f, 0);

        if (arrowParent != null)
        spawnedPanel.transform.SetParent(arrowParent, false);

        TMP_Text questionText = spawnedPanel.transform.Find("Background/QuestionText")?.GetComponent<TMP_Text>();
        if (questionText != null) questionText.text = question;

        TMP_Dropdown dropdown = spawnedPanel.transform.Find("AnswerDropdown")?.GetComponent<TMP_Dropdown>();
        if (dropdown != null)
        {
            dropdown.ClearOptions();

            List<string> optionsWithPlaceholder = new List<string> { " Pilih Jawaban " };
            optionsWithPlaceholder.AddRange(answers);
            dropdown.AddOptions(optionsWithPlaceholder);

            dropdown.value = 0;
        }

        Button submitButton = spawnedPanel.transform.Find("SubmitButton")?.GetComponent<Button>();
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(() =>
            {
                int selectedIndex = dropdown.value;
                questionText.gameObject.SetActive(false);
                dropdown.gameObject.SetActive(false);
                submitButton.gameObject.SetActive(false);
                HandleAnswerClick(selectedIndex);
            });
        }

        CloseButton close = spawnedPanel.GetComponentInChildren<CloseButton>();
        if (close != null)
        {
            close.Setup(this);
        }
    }

    void HandleAnswerClick(int index)
    {
        if (index == 0)
        {
            Debug.LogWarning("⚠️ No answer selected.");
            return;
        }

        int answerIndex = index - 1; // Adjust because of the placeholder

        bool isCorrect = answerIndex == correctAnswerIndex;

        Debug.Log(isCorrect
            ? $"✅ Correct Answer: {answers[answerIndex]}"
            : $"❌ Wrong Answer: {answers[answerIndex]}");

        TMP_Text feedbackText = spawnedPanel.transform.Find("Background/FeedbackText")?.GetComponent<TMP_Text>();
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(true);
            feedbackText.fontSize = 24;
            feedbackText.alignment = TextAlignmentOptions.Center;
            feedbackText.enableWordWrapping = false;
            feedbackText.overflowMode = TextOverflowModes.Ellipsis;
            feedbackText.text = isCorrect ? "Yap, benar!" : "Masih salah :(";
            feedbackText.color = isCorrect ? Color.green : Color.red;
        }

        TMP_Dropdown dropdown = spawnedPanel.transform.Find("AnswerDropdown")?.GetComponent<TMP_Dropdown>();
        if (dropdown != null) dropdown.interactable = false;

        Button submitButton = spawnedPanel.transform.Find("SubmitButton")?.GetComponent<Button>();
        if (submitButton != null) submitButton.interactable = false;

        if (isCorrect)
        {
            Invoke(nameof(ClosePanel), 2f);
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
