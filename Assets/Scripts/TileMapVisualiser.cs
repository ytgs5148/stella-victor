using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class TileMapVisualiser : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap, cosmeticBelowTilemap, cosmeticAboveTilemap;
    [SerializeField]
    private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull, wallInnerCornerDownLeft, wallInnerCornerDownRight, wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    [SerializeField]
    private TileBase[] stoneGroundTiles;
    [SerializeField]
    private TileBase[] flowerTiles;
    [SerializeField]
    private float flowerProbability = 0.1f;
    [SerializeField]
    private float stoneTileProbability = 0.5f;
    [SerializeField]
    private OrderedTileSet[] orderedTileSets;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tileMap, TileBase tileBase)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tileMap, tileBase, position);
        }
    }

    private void PaintSingleTile(Tilemap tileMap, TileBase tileBase, Vector2Int position)
    {
        var tilePosition = tileMap.WorldToCell((Vector3Int)position);
        tileMap.SetTile(tilePosition, tileBase);
    }

    public void ClearTileMap()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallByteHelper.wallTop.Contains(typeAsInt))
            tile = wallTop;
        else if (WallByteHelper.wallSideLeft.Contains(typeAsInt))
            tile = wallSideLeft;
        else if (WallByteHelper.wallSideRight.Contains(typeAsInt))
            tile = wallSideRight;
        else if (WallByteHelper.wallBottm.Contains(typeAsInt))
            tile = wallBottom;
        else if (WallByteHelper.wallFull.Contains(typeAsInt))
            tile = wallFull;

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, position);
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeASInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallByteHelper.wallInnerCornerDownLeft.Contains(typeASInt))
            tile = wallInnerCornerDownLeft;
        else if (WallByteHelper.wallInnerCornerDownRight.Contains(typeASInt))
            tile = wallInnerCornerDownRight;
        else if (WallByteHelper.wallDiagonalCornerDownLeft.Contains(typeASInt))
            tile = wallDiagonalCornerDownLeft;
        else if (WallByteHelper.wallDiagonalCornerDownRight.Contains(typeASInt))
            tile = wallDiagonalCornerDownRight;
        else if (WallByteHelper.wallDiagonalCornerUpRight.Contains(typeASInt))
            tile = wallDiagonalCornerUpRight;
        else if (WallByteHelper.wallDiagonalCornerUpLeft.Contains(typeASInt))
            tile = wallDiagonalCornerUpLeft;
        else if (WallByteHelper.wallFullEightDirections.Contains(typeASInt))
            tile = wallFull;
        else if (WallByteHelper.wallBottmEightDirections.Contains(typeASInt))
            tile = wallBottom;

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, position);
    }

    public void PaintStoneGroundTiles(IEnumerable<Vector2Int> floorPositions)
    {
        foreach (var pos in floorPositions)
        {
            if (UnityEngine.Random.value < stoneTileProbability)
            {
                TileBase tile = stoneGroundTiles[UnityEngine.Random.Range(0, stoneGroundTiles.Length)];
                PaintSingleTile(floorTilemap, tile, pos);
            }
        }
    }

    public void PlaceFlowerTiles(IEnumerable<Vector2Int> floorPositions)
    {
        foreach (var pos in floorPositions)
        {
            if (UnityEngine.Random.value < flowerProbability)
            {
                TileBase flowerTile = flowerTiles[UnityEngine.Random.Range(0, flowerTiles.Length)];
                PaintSingleTile(floorTilemap, flowerTile, pos);
            }
        }
    }

    public void PlaceOrderedTileSetsRectangular(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> floorList = new List<Vector2Int>(floorPositions);
        HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();

        foreach (var orderedSet in orderedTileSets)
        {
            int setsToPlace = UnityEngine.Random.Range(orderedSet.minSetsToPlace, orderedSet.maxSetsToPlace + 1);

            for (int setCount = 0; setCount < setsToPlace; setCount++)
            {
                bool placed = false;
                int attempts = 0;

                while (!placed && attempts < 100)
                {
                    attempts++;
                    Vector2Int candidate = floorList[UnityEngine.Random.Range(0, floorList.Count)];

                    bool canPlace = true;
                    List<Vector2Int> positionsToCheck = new List<Vector2Int>();

                    for (int x = 0; x < orderedSet.width; x++)
                    {
                        for (int y = 0; y < orderedSet.height; y++)
                        {
                            Vector2Int pos = candidate + new Vector2Int(x, y);
                            if (!floorPositions.Contains(pos) || occupiedPositions.Contains(pos))
                            {
                                canPlace = false;
                                break;
                            }
                            positionsToCheck.Add(pos);
                        }
                        if (!canPlace) break;
                    }

                    if (canPlace)
                    {
                        List<Vector2Int> spiralOffsets = GetClockwiseSpiralPositions(orderedSet.width, orderedSet.height);

                        int expectedCount = orderedSet.width * orderedSet.height;
                        if (spiralOffsets.Count != expectedCount || spiralOffsets.Count != orderedSet.tiles.Length)
                        {
                            Debug.LogError("OrderedTileSet configuration mismatch: Expected " + expectedCount + " positions, but got " + spiralOffsets.Count);
                            return;
                        }

                        for (int i = 0; i < spiralOffsets.Count; i++)
                        {
                            Vector2Int tilePos = candidate + spiralOffsets[i];

                            if (orderedSet.isChest)
                                PlayerData.Instance.chestPosition = tilePos;

                            if (orderedSet.belowPlayer == true)
                                PaintSingleTile(cosmeticBelowTilemap, orderedSet.tiles[i], tilePos);
                            else
                                PaintSingleTile(cosmeticAboveTilemap, orderedSet.tiles[i], tilePos);
                            occupiedPositions.Add(tilePos);
                        }

                        if (orderedSet.applyCustomLighting && orderedSet.lightPrefab != null)
                        {
                            Tilemap targetTilemap = (orderedSet.belowPlayer == true) ? cosmeticBelowTilemap : cosmeticAboveTilemap;

                            Vector3 bottomLeftWorld = targetTilemap.CellToWorld((Vector3Int)candidate);

                            Vector3 propWorldSize = new Vector3(
                                orderedSet.width * targetTilemap.cellSize.x,
                                orderedSet.height * targetTilemap.cellSize.y,
                                0f);

                            Vector3 centerWorldPos = bottomLeftWorld + propWorldSize * 0.5f;
    
                            Instantiate(orderedSet.lightPrefab, centerWorldPos, Quaternion.identity, transform);
                        }

                        placed = true;
                    }
                }
            }
        }
    }

    private List<Vector2Int> GetClockwiseSpiralPositions(int width, int height)
    {
        List<Vector2Int> spiral = new List<Vector2Int>();

        int left = 0, right = width - 1;
        int bottom = 0, top = height - 1;

        while (left <= right && bottom <= top)
        {
            for (int x = left; x <= right; x++)
                spiral.Add(new Vector2Int(x, top));

            for (int y = top - 1; y >= bottom; y--)
                spiral.Add(new Vector2Int(right, y));

            if (bottom != top)
            {
                for (int x = right - 1; x >= left; x--)
                    spiral.Add(new Vector2Int(x, bottom));
            }

            if (left != right)
            {
                for (int y = bottom + 1; y <= top - 1; y++)
                    spiral.Add(new Vector2Int(left, y));
            }

            left++;
            right--;
            bottom++;
            top--;
        }

        return spiral;
    }
}