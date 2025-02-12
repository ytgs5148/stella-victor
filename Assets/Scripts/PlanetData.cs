using UnityEngine;

public class PlanetData : MonoBehaviour
{
    public static PlanetData Instance { get; private set; } = null;
    public string planetName;
    public string planetDesc;
    public string planetLevel;
    public string planetType;
    public string planetXP;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
