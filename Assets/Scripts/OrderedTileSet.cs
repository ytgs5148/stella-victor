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
    
    [Tooltip("The chance (0-100%) that this ordered tile set will be placed when a valid area is found.")]
    [Range(0,100)]
    public int placementChance;
    
    [Tooltip("Tiles to place in a clockwise order. The number of tiles must equal width*height.")]
    public TileBase[] tiles;
}