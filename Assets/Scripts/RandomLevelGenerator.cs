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

    void Start()
    {
        RunProceduralGeneration();
    }

    public void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tileMapVisualiser.ClearTileMap();
        tileMapVisualiser.PaintFloorTiles(floorPositions);

        HashSet<Vector2Int> wallPositions = WallGenerator.CreateWalls(floorPositions, tileMapVisualiser);

        tileMapVisualiser.PlaceOrderedTileSetsRectangular(floorPositions);
        tileMapVisualiser.PaintStoneGroundTiles(floorPositions);
        tileMapVisualiser.PlaceFlowerTiles(floorPositions);

        Vector3 playerPosition = new Vector3(startPosition.x, startPosition.y, PlayerController.Instance.transform.position.z);

        Vector2Int[] directions = {
            new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1),
            new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1)
        };

        HashSet<Vector2Int> safePositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            bool isSafe = true;
            foreach (var direction in directions)
            {
                Vector2Int adjacentPosition = position + direction;
                if (wallPositions.Contains(adjacentPosition))
                {
                    isSafe = false;
                    break;
                }
            }

            if (isSafe)
                safePositions.Add(position);
        }

        if (!safePositions.Contains(startPosition))
            startPosition = safePositions.ElementAt(UnityEngine.Random.Range(0, safePositions.Count));

        PlayerController.Instance.transform.position = playerPosition;
        PlayerController.Instance.gameObject.SetActive(true);
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
