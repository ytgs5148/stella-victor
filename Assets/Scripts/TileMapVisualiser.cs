using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualiser : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;
    [SerializeField]
    private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull, wallInnerCornerDownLeft, wallInnerCornerDownRight, wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    [SerializeField]
    private TileBase[] stoneGroundTiles;
    [SerializeField]
    private TileBase[] flowerTiles;
    [SerializeField]
    private float flowerProbability = 0.1f;
    [SerializeField]
    private float stoneTileProbability = 0.5f; // New probability field for stone tiles
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

    public void ClearTileMap() {
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
            if (UnityEngine.Random.value < stoneTileProbability) // Use the stone tile probability
            {
                TileBase tile = stoneGroundTiles[UnityEngine.Random.Range(0, stoneGroundTiles.Length)];
                PaintSingleTile(floorTilemap, tile, pos);
            }
        }
    }

    /// <summary>
    /// Randomly places flower tiles on the given floor positions.
    /// Uses a probability check so not every cell gets a flower.
    /// </summary>
    public void PlaceFlowerTiles(IEnumerable<Vector2Int> floorPositions)
    {
        foreach (var pos in floorPositions)
        {
            if (UnityEngine.Random.value < flowerProbability)
            {
                // You might want to ensure that the position isn’t near a wall.
                TileBase flowerTile = flowerTiles[UnityEngine.Random.Range(0, flowerTiles.Length)];
                PaintSingleTile(floorTilemap, flowerTile, pos);
            }
        }
    }

    public void PlaceOrderedTileSetsRectangular(HashSet<Vector2Int> floorPositions)
    {
        // Convert the floorPositions HashSet to a list for random access.
        List<Vector2Int> floorList = new List<Vector2Int>(floorPositions);
        
        foreach (var orderedSet in orderedTileSets)
        {
            bool placed = false;
            int attempts = 0;
            // Try up to 100 times to find a valid candidate.
            while (!placed && attempts < 100)
            {
                attempts++;
                // Pick a candidate cell randomly.
                Vector2Int candidate = floorList[UnityEngine.Random.Range(0, floorList.Count)];
                
                // Verify that a rectangle starting at candidate with size (width x height) is completely on floor.
                bool canPlace = true;
                for (int x = 0; x < orderedSet.width; x++)
                {
                    for (int y = 0; y < orderedSet.height; y++)
                    {
                        if (!floorPositions.Contains(candidate + new Vector2Int(x, y)))
                        {
                            canPlace = false;
                            break;
                        }
                    }
                    if (!canPlace) break;
                }
                
                if (canPlace)
                {
                    // Roll the chance (placementChance is in percentage).
                    if (UnityEngine.Random.Range(0, 100) < orderedSet.placementChance)
                    {
                        // Generate the list of offsets in clockwise (spiral) order.
                        List<Vector2Int> spiralOffsets = GetClockwiseSpiralPositions(orderedSet.width, orderedSet.height);
                        
                        // Check that the number of spiral positions matches the number of tiles.
                        int expectedCount = orderedSet.width * orderedSet.height;
                        if (spiralOffsets.Count != expectedCount || spiralOffsets.Count != orderedSet.tiles.Length)
                        {
                            Debug.LogError("OrderedTileSet configuration mismatch: Expected " + expectedCount + " positions, but got " + spiralOffsets.Count);
                            return;
                        }
                        
                        // Place each tile at the candidate offset plus the spiral offset.
                        for (int i = 0; i < spiralOffsets.Count; i++)
                        {
                            Vector2Int tilePos = candidate + spiralOffsets[i];
                            PaintSingleTile(floorTilemap, orderedSet.tiles[i], tilePos);
                        }
                        placed = true;
                    }
                    // If the chance roll failed, then exit (or you might try a new candidate).
                    else
                    {
                        placed = true;  // Do nothing in this case.
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
            // Top row: left to right.
            for (int x = left; x <= right; x++)
                spiral.Add(new Vector2Int(x, top));
            
            // Right column: top-1 to bottom.
            for (int y = top - 1; y >= bottom; y--)
                spiral.Add(new Vector2Int(right, y));
            
            // Bottom row: right-1 to left (if we are not on the same row).
            if (bottom != top)
            {
                for (int x = right - 1; x >= left; x--)
                    spiral.Add(new Vector2Int(x, bottom));
            }
            
            // Left column: bottom+1 to top-1 (if we are not on the same column).
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
