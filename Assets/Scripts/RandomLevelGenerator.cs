using System;
using System.Collections;
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
        // Debug.Log("Planet Name: " + PlanetData.Instance.planetName);
        // Debug.Log("Planet Description: " + PlanetData.Instance.planetDesc);
        // Debug.Log("Planet Level: " + PlanetData.Instance.planetLevel);
        // Debug.Log("Planet Elemental Type: " + PlanetData.Instance.planetType);
        // Debug.Log("Planet Win XP: " + PlanetData.Instance.planetXP);

        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tileMapVisualiser.ClearTileMap();
        tileMapVisualiser.PaintFloorTiles(floorPositions);

        HashSet<Vector2Int> wallPositions = WallGenerator.CreateWalls(floorPositions, tileMapVisualiser);

        tileMapVisualiser.PlaceOrderedTileSetsRectangular(floorPositions);
        tileMapVisualiser.PaintStoneGroundTiles(floorPositions);
        tileMapVisualiser.PlaceFlowerTiles(floorPositions);

        PlayerController.Instance.gameObject.SetActive(true);
        Vector3 playerPosition = new Vector3(startPosition.x, startPosition.y, PlayerController.Instance.transform.position.z);

        Debug.Log("Player start position: " + startPosition);
        Debug.Log("Player start position is inside a wall: " + wallPositions.Contains(startPosition));
        Debug.Log("Wall positions:");
        foreach (var wallPosition in wallPositions)
        {
            Debug.Log($"Wall position: {wallPosition.x}, {wallPosition.y}");
        }

        Vector2Int[] directions = {
            new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1),
            new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1)
        };

        while (IsSurroundedByWalls(startPosition, wallPositions))
        {
            Debug.Log("Player start position is surrounded by walls, moving to a random floor position.");
            startPosition = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count));
        }

        PlayerController.Instance.transform.position = playerPosition;

        bool IsSurroundedByWalls(Vector2Int position, HashSet<Vector2Int> walls)
        {
            bool hasAtLeastOneWall = false;

            foreach (var direction in directions)
            {
                Vector2Int adjacentPosition = position + direction;
                Debug.Log("Adjacent position: " + adjacentPosition);
                if (walls.Contains(adjacentPosition))
                {
                    hasAtLeastOneWall = true;
                    break;
                }
            }

            return hasAtLeastOneWall;
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
