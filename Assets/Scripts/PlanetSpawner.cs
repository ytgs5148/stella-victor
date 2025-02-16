using UnityEngine;
using System.Collections.Generic;

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

        for (int i = 0; i < numberOfMarkers; i++)
        {
            Vector2 candidatePosition;
            if (TryGetRandomValidPosition(markerPositions, out candidatePosition))
            {
                markerPositions.Add(candidatePosition);

                GameObject marker = Instantiate(markerPrefab, transform);

                RectTransform markerRect = marker.GetComponent<RectTransform>();
                markerRect.anchoredPosition = candidatePosition;

                MarkerManager markerController = marker.GetComponent<MarkerManager>();
                if (markerController != null)
                {
                    markerController.planetName = Planets.planetNames[Random.Range(0, Planets.planetNames.Length)];
                    markerController.planetDescription = Planets.planetDescriptions[Random.Range(0, Planets.planetDescriptions.Length)];
                    markerController.planetElement = Planets.elementTypes[Random.Range(0, Planets.elementTypes.Length)];
                    markerController.planetDifficultyLevel = Random.Range(1, 5);
                    markerController.planetObjectiveType = Random.Range(0, 2);
                }
            }
            else
            {
                Debug.LogWarning("Could not find a valid position for marker #" + i);
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
