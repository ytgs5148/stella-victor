using System.Collections.Generic;
using UnityEngine;

public class AllMarkersDataHolder : MonoBehaviour
{
    public static AllMarkersDataHolder Instance { get; private set; } = null;

    [System.Serializable]
    public class MarkerData
    {
        public string planetName;
        public string description;
        public string element;
        public int difficultyLevel;
        public string objectiveType;
        public Vector3 position;
        public string markerColor;
    }

    public List<MarkerData> allMarkersData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
