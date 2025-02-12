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
    public int walkLength = 10;
    [SerializeField]
    public bool startRandomlyEachIteration = true;
    [SerializeField]
    private GameObject player = null;

    public void RunProceduralGeneration()
    {
        Debug.Log("Planet Name: " + PlanetData.Instance.planetName);
        Debug.Log("Planet Description: " + PlanetData.Instance.planetDesc);

        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tileMapVisualiser.ClearTileMap();
        tileMapVisualiser.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualiser);

        player.SetActive(true);
        Vector3 playerPosition = new Vector3(startPosition.x, startPosition.y, player.transform.position.z);
        if (floorPositions.Contains(startPosition))
            player.transform.position = playerPosition;
        // else
        // {
        //     Vector2Int closestFloor = floorPositions.OrderBy(p => Vector2Int.Distance(p, startPosition)).First();
        //     player.transform.position = new Vector3(closestFloor.x, closestFloor.y, player.transform.position.z);
        // }
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
