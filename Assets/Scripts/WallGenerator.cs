using System;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static HashSet<Vector2Int> CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualiser tileMapVisualiser)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionList);
        
        CreateBasicWalls(tileMapVisualiser, basicWallPositions, floorPositions);
        CreateCornerWalls(tileMapVisualiser, cornerWallPositions, floorPositions);

        basicWallPositions.UnionWith(cornerWallPositions);
        return basicWallPositions;
    }

    private static void CreateCornerWalls(TileMapVisualiser tileMapVisualiser, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighbourBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionList)
            {
                var neighbourPosition = position + direction;
                
                if (floorPositions.Contains(neighbourPosition))
                    neighbourBinaryType += "1";
                else
                    neighbourBinaryType += "0";
            }
            tileMapVisualiser.PaintSingleCornerWall(position, neighbourBinaryType);
        }
    }

    private static void CreateBasicWalls(TileMapVisualiser tileMapVisualiser, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in basicWallPositions)
        {
            string neighbourBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirectionList)
            {
                var neighbourPosition = position + direction;
                
                if (floorPositions.Contains(neighbourPosition))
                    neighbourBinaryType += "1";
                else
                    neighbourBinaryType += "0";
            }
            tileMapVisualiser.PaintSingleBasicWall(position, neighbourBinaryType);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbour = position + direction;

                if (!floorPositions.Contains(neighbour))
                    wallPositions.Add(neighbour);
            }
        }

        return wallPositions;
    }
}
