using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomLevelGenerator : MonoBehaviour
{
    [SerializeField]
    protected TileMapVisualiser tileMapVisualiser = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;
    [SerializeField]
    private int iterations = 10;
    [SerializeField]
    public int walkLength = 100;
    [SerializeField]
    public bool startRandomlyEachIteration = true;
    [SerializeField]
    public int safeEnemyRadius = 5;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private int enemyCount = 5;

    void Start()
    {
        RunProceduralGeneration();
    }

    public void RunProceduralGeneration()
    {
        Debug.Log("Running procedural generation");
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tileMapVisualiser.ClearTileMap();
        tileMapVisualiser.PaintFloorTiles(floorPositions);

        HashSet<Vector2Int> wallPositions = WallGenerator.CreateWalls(floorPositions, tileMapVisualiser);

        tileMapVisualiser.PlaceOrderedTileSetsRectangular(floorPositions);
        tileMapVisualiser.PaintStoneGroundTiles(floorPositions);
        tileMapVisualiser.PlaceFlowerTiles(floorPositions);

        Vector3 playerPosition = new Vector3(startPosition.x, startPosition.y, PlayerController.Instance.transform.position.z);

        HashSet<Vector2Int> safePositions = new HashSet<Vector2Int>(floorPositions);
        Debug.Log("Safe positions count: " + safePositions.Count);
        Debug.Log("Wall positions count: " + wallPositions.Count);

        foreach (var wallPosition in wallPositions)
        {
            safePositions.Remove(wallPosition);
            safePositions.Remove(wallPosition + Vector2Int.up);
            safePositions.Remove(wallPosition + Vector2Int.down);
            safePositions.Remove(wallPosition + Vector2Int.left);
            safePositions.Remove(wallPosition + Vector2Int.right);
            safePositions.Remove(wallPosition + Vector2Int.up + Vector2Int.left);
            safePositions.Remove(wallPosition + Vector2Int.up + Vector2Int.right);
            safePositions.Remove(wallPosition + Vector2Int.down + Vector2Int.left);
            safePositions.Remove(wallPosition + Vector2Int.down + Vector2Int.right);
        }

        Debug.Log("Safe positions count final: " + safePositions.Count);

        if (safePositions.Count == 0)
        {
            Debug.LogError("No safe positions found, retrying generation");
            RunProceduralGeneration();
            return;
        }

        foreach (var safePosition in safePositions)
        {
            Debug.Log("Safe position: " + safePosition);
        }

        if (!safePositions.Contains(new Vector2Int((int)playerPosition.x, (int)playerPosition.y)))
        {
            Debug.Log("Player position not in safe positions, changing player position");
            Vector2Int randomSafePosition = safePositions.ElementAt(Random.Range(0, safePositions.Count));
            playerPosition = new Vector3(randomSafePosition.x, randomSafePosition.y, playerPosition.z);
        }

        Debug.Log("Player position: " + playerPosition);
        PlayerController.Instance.transform.position = playerPosition;

        GenerateEnemies(playerPosition, safePositions, safeEnemyRadius, enemyPrefab, enemyCount);
    }

    private void GenerateEnemies(Vector3 playerPosition, HashSet<Vector2Int> floorPositions, int safeEnemyRadius, GameObject enemyPrefab, int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Debug.Log("Generating enemy " + i);
            Vector2Int enemyPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));

            while (Vector2.Distance(enemyPosition, startPosition) < safeEnemyRadius)
                enemyPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));

            Vector3 enemyPosition3D = new Vector3(enemyPosition.x, enemyPosition.y, playerPosition.z);
            Instantiate(enemyPrefab, enemyPosition3D, Quaternion.identity);
        }
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, walkLength);
            floorPositions.UnionWith(path);

            if (startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }
}
