using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class OrderedTileSet
{
    [Tooltip("The width (a) of the area (in grid cells) for this tile set.")]
    public int width;
    
    [Tooltip("The height (b) of the area (in grid cells) for this tile set.")]
    public int height;
    
    [Tooltip("Tiles to place in a clockwise order. The number of tiles must equal width*height.")]
    public TileBase[] tiles;

    [Tooltip("The minimum number of sets to place in the level.")]
    public int minSetsToPlace;

    [Tooltip("The maximum number of sets to place in the level.")]
    public int maxSetsToPlace;

    [Tooltip("Will the tile set appear below player")]
    public bool belowPlayer;
}