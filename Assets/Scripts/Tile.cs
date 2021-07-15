using UnityEngine;

/// <summary>
/// This is the structure matrix where the player can move next.
/// Also hold information whether a obstacle is placed there or not.
/// </summary>
public class Tile
{
    public Vector3 Vec = new Vector3(0, 0, 0);
    private TileType tileTyp;

    public TileType TileTyp { get => tileTyp; set => tileTyp = value; }

    public Tile()
    {
        ////By default there is no obstacle on the tile
        this.TileTyp = TileType.FREE;
    }

    public bool IsFree()
    {
        return this.TileTyp == TileType.FREE;
    }

    /// <summary>
    /// should return postion.
    /// </summary>
    /// <returns> return the current position in Vector</returns>
    public Vector3 GetPosition()
    {
        return this.Vec;
    }

    public TileType GetTileType()
    {
        return this.TileTyp;
    }
}
