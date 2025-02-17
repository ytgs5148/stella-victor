using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject markerPrefab;
    public int numberOfMarkers = 10;
    public float safeMargin = 50f;
    public float minDistanceBetweenMarkers = 100f;

    private RectTransform canvasRectTransform;

    void Start()
    {
        canvasRectTransform = GetComponent<RectTransform>();

        List<Vector2> markerPositions = new List<Vector2>();

        if (AllMarkersDataHolder.Instance.allMarkersData != null && AllMarkersDataHolder.Instance.allMarkersData.Count > 0)
        {
            foreach (var markerData in AllMarkersDataHolder.Instance.allMarkersData)
            {
                GameObject marker = Instantiate(markerPrefab, transform);
                RectTransform markerRect = marker.GetComponent<RectTransform>();
                markerRect.anchoredPosition = markerData.position;

                MarkerManager markerController = marker.GetComponent<MarkerManager>();
                if (markerController != null)
                {
                    markerController.planetName = markerData.planetName;
                    markerController.planetDescription = markerData.description;
                    markerController.planetElement = markerData.element;
                    markerController.planetDifficultyLevel = markerData.difficultyLevel;
                    markerController.planetObjectiveType = markerData.objectiveType == "Eliminate Planet" ? 0 : 1;

                    if (PlayerData.Instance.planetsExplored.Contains(markerData.planetName))
                    {
                        Image markerImage = marker.GetComponent<Image>();
                        if (markerImage != null)
                        {
                            Color customColor;
                            ColorUtility.TryParseHtmlString("#604F4F", out customColor);
                            markerImage.color = customColor;
                            markerData.markerColor = "#604F4F";
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No markers data found. Generating random markers.");
            for (int i = 0; i < numberOfMarkers; i++)
            {
                Vector2 candidatePosition;
                if (TryGetRandomValidPosition(markerPositions, out candidatePosition))
                {
                    markerPositions.Add(candidatePosition);

                    GameObject marker = Instantiate(markerPrefab, transform);

                    string randomPlanetName = Planets.planetNames[Random.Range(0, Planets.planetNames.Length)];
                    string randomDescription = Planets.planetDescriptions[Random.Range(0, Planets.planetDescriptions.Length)];
                    string randomElement = Planets.elementTypes[Random.Range(0, Planets.elementTypes.Length)];
                    int randomDifficultyLevel = Random.Range(1, 5);
                    int randomObjectiveType = Random.Range(0, 2);

                    AllMarkersDataHolder.Instance.allMarkersData.Add(new AllMarkersDataHolder.MarkerData
                    {
                        planetName = randomPlanetName,
                        description = randomDescription,
                        element = randomElement,
                        difficultyLevel = randomDifficultyLevel,
                        objectiveType = randomObjectiveType == 0 ? "Eliminate Planet" : "Save Weaponry",
                        position = candidatePosition,
                        markerColor = "#FFFFFF"
                    });

                    RectTransform markerRect = marker.GetComponent<RectTransform>();
                    markerRect.anchoredPosition = candidatePosition;

                    MarkerManager markerController = marker.GetComponent<MarkerManager>();
                    if (markerController != null)
                    {
                        markerController.planetName = randomPlanetName;
                        markerController.planetDescription = randomDescription;
                        markerController.planetElement = randomElement;
                        markerController.planetDifficultyLevel = randomDifficultyLevel;
                        markerController.planetObjectiveType = randomObjectiveType;
                    }
                }
                else
                {
                    Debug.LogWarning("Could not find a valid position for marker #" + i);
                }
            }
        }
    }

    bool TryGetRandomValidPosition(List<Vector2> existingPositions, out Vector2 pos)
    {
        int attempts = 0;
        const int maxAttempts = 100;

        Rect rect = canvasRectTransform.rect;
        float minX = rect.xMin + safeMargin;
        float maxX = rect.xMax - safeMargin;
        float minY = rect.yMin + safeMargin;
        float maxY = rect.yMax - safeMargin;

        while (attempts < maxAttempts)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            pos = new Vector2(randomX, randomY);

            bool valid = true;
            foreach (Vector2 existing in existingPositions)
            {
                if (Vector2.Distance(existing, pos) < minDistanceBetweenMarkers)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
                return true;

            attempts++;
        }

        pos = Vector2.zero;
        return false;
    }
}
