using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DirectionLink
{
    public string direction; // "front", "back", etc.
    public string targetLocationId;
}

[System.Serializable]
public class PanoramaLocation
{
    public string locationId;
    public Material panoramaMaterial;
    public List<DirectionLink> directions;
    public List<InfoPoint> infoPoints;
    public List<QuizPointData> quizPoints;
    public List<ShalatAnimation> shalatAnimations;
}

[System.Serializable]
public class InfoPoint
{
    public Vector3 position;
    public string title;
    public string message;
}

[System.Serializable]
public class ShalatAnimation
{
    public Vector3 position;
}

[System.Serializable]
public class QuizPointData
{
    public Vector3 position;
    public string question;
    public List<string> answers;
    public int correctAnswerIndex;
}
