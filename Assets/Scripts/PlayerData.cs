using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    public int xp;
    public WeaponInfo[] weaponInfo;
    public int[] planetsExplored = new int[0];
    public float endingBar = 0f;
    public int kills = 0;
    public Vector2Int chestPosition = Vector2Int.zero;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
