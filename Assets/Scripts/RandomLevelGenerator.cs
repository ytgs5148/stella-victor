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
        tileMapVisualiser.PaintStoneGroundTiles(floorPositions);
        tileMapVisualiser.PlaceFlowerTiles(floorPositions);
        tileMapVisualiser.PlaceOrderedTileSetsRectangular(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualiser);

        PlayerController.Instance.gameObject.SetActive(true);
        Vector3 playerPosition = new Vector3(startPosition.x, startPosition.y, PlayerController.Instance.transform.position.z);
        if (floorPositions.Contains(startPosition))
            PlayerController.Instance.transform.position = playerPosition;
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
