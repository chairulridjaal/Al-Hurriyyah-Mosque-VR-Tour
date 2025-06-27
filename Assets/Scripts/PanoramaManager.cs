using System.Collections.Generic;
using UnityEngine;

public class PanoramaManager : MonoBehaviour
{
    [Header("Core References")]
    public GameObject panoramaSphere;
    public GameObject arrowPrefab;
    public GameObject arrowParent;
    public GameObject infoPrefab;
    public GameObject quizPrefab;
    public GameObject shalatPrefab;

    [Header("Optional UI for Info Points and Quizzes")]
    public GameObject infoPanelUIPrefab;   
    public GameObject QuizPanelPrefab; 
    public Transform panelSpawnTransform;  
    public Transform quizPanelSpawnTransform; 

    public List<PanoramaLocation> locations;
    public string startLocationId;

    private PanoramaLocation currentLocation;

    void Start()
    {
        LoadLocation(startLocationId);
    }

    public void LoadLocation(string locationId)
    {
        Debug.Log("🔄 LoadLocation called: " + locationId);

        // Clear all children from arrowParent
        foreach (Transform child in arrowParent.transform)
        {
            Debug.Log("❌ Destroying child: " + child.name);
            Destroy(child.gameObject);
        }

        // Find matching panorama location
        currentLocation = locations.Find(loc => loc.locationId == locationId);
        if (currentLocation == null)
        {
            Debug.LogError("❌ Location not found: " + locationId);
            return;
        }

        // Update panorama material
        panoramaSphere.GetComponent<Renderer>().material = currentLocation.panoramaMaterial;

        // Spawn arrows
        foreach (DirectionLink link in currentLocation.directions)
        {
            CreateArrow(link.direction, link.targetLocationId);
        }

        // Spawn info points
        foreach (var info in currentLocation.infoPoints)
        {
            CreateInfoPoint(info.position, info.message, info.title);
        }

        foreach (var quiz in currentLocation.quizPoints)
        {
            CreateQuizPoint(quiz.position, quiz.question, quiz.answers, quiz.correctAnswerIndex);
        }

        // Spawn shalat animations
        foreach (var shalat in currentLocation.shalatAnimations)
        {
            CreateShalatAnimations(shalat.position);
        }
    }

    private void CreateArrow(string direction, string targetLocationId)
    {
        Vector3 localPos = Vector3.zero;
        Vector3 localRot = Vector3.zero;

        float arrowDistance = 1f;

        switch (direction.ToLower())
        {
            case "front":
                localPos = Vector3.forward * arrowDistance;
                localRot = new Vector3(90, 90, 0);
                break;
            case "back":
                localPos = Vector3.back * arrowDistance;
                localRot = new Vector3(90, -90, 0);
                break;
            case "left":
                localPos = Vector3.left * arrowDistance;
                localRot = new Vector3(90, 90, 90);
                break;
            case "right":
                localPos = Vector3.right * arrowDistance;
                localRot = new Vector3(90, 90, -90);
                break;
            case "front-right":
                localPos = (Vector3.forward + Vector3.right).normalized * arrowDistance;
                localRot = new Vector3(90, 90, -45);
                break;
            case "front-left":
                localPos = (Vector3.forward + Vector3.left).normalized * arrowDistance;
                localRot = new Vector3(90, 90, 45);
                break;
            case "back-right":
                localPos = (Vector3.back + Vector3.right).normalized * arrowDistance;
                localRot = new Vector3(90, 90, -135);
                break;
            case "back-left":
                localPos = (Vector3.back + Vector3.left).normalized * arrowDistance;
                localRot = new Vector3(90, 90, 135);
                break;
            default:
                Debug.LogWarning($"⚠️ Unknown direction '{direction}'");
                break;
        }

        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.SetParent(arrowParent.transform, false);
        arrow.transform.localPosition = localPos;
        arrow.transform.localRotation = Quaternion.Euler(localRot);

        Debug.Log($"✅ Arrow created: {arrow.name} → Parent: {arrow.transform.parent?.name}");

        ArrowClick arrowClick = arrow.GetComponent<ArrowClick>();
        if (arrowClick != null)
        {
            arrowClick.targetLocationId = targetLocationId;
            arrowClick.manager = this;
        }
    }

    private void CreateInfoPoint(Vector3 position, string message, string title)
    {
        Vector3 offset = position.normalized * 0.9f;

        GameObject infoPoint = Instantiate(infoPrefab);
        infoPoint.transform.SetParent(arrowParent.transform, false);
        infoPoint.transform.localScale = Vector3.one * 0.3f;
        infoPoint.transform.localPosition = offset;
        infoPoint.transform.localRotation = Quaternion.identity;

        InfoClick infoClick = infoPoint.GetComponent<InfoClick>();
        if (infoClick != null)
        {
            infoClick.title = title;
            infoClick.message = message;
            infoClick.infoPanelPrefab = infoPanelUIPrefab;
            infoClick.arrowParent = arrowParent.transform; 
        }

        Debug.Log($"📍 Info point created at {offset} with message: {message} → Parent: {infoPoint.transform.parent?.name}");
    }

    private void CreateQuizPoint(Vector3 position, string question, List<string> answers, int correctAnswerIndex)
    {
        Vector3 offset = position.normalized * 0.9f;

        GameObject quizPoint = Instantiate(quizPrefab);
        quizPoint.transform.SetParent(arrowParent.transform, false);
        quizPoint.transform.localScale = Vector3.one * 0.3f;
        quizPoint.transform.localPosition = offset;
        quizPoint.transform.localRotation = Quaternion.identity;

        QuizClick quizClick = quizPoint.GetComponent<QuizClick>();
        if (quizClick != null)
        {
            quizClick.question = question;
            quizClick.answers = answers;
            quizClick.correctAnswerIndex = correctAnswerIndex; 
            quizClick.QuizPanelPrefab = QuizPanelPrefab;
            Debug.Log($"QuizPanelPrefab: {QuizPanelPrefab?.name}");
            quizClick.arrowParent = arrowParent.transform; 
        }

        Debug.Log($"❓ Quiz point created at {offset} with question: {question} → Parent: {quizPoint.transform.parent?.name}");
    }

    private void CreateShalatAnimations(Vector3 position)
    {
        GameObject shalatPoint = Instantiate(shalatPrefab);
        shalatPoint.transform.SetParent(arrowParent.transform, false);
        shalatPoint.transform.localScale = Vector3.one * 0.3f;
        shalatPoint.transform.localPosition = position;
        shalatPoint.transform.localRotation = Quaternion.Euler(0f, -120.374f, 0f);

        Animator animator = shalatPoint.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play(0);
        }
        else
        {
            Debug.LogWarning("No Animator component found on shalatPrefab!");
        }

        Debug.Log($"Shalat animation created at {position} → Parent: {shalatPoint.transform.parent?.name}");
    }
}
