using System;

/// <summary>
/// Command mention which direction the rover will rotate or move.
/// </summary>
public enum Command
{
    GO,
    ROTATE_LEFT,
    ROTATE_RIGHT
}

/// <summary>
/// TileType mention whether a new tile is free or there is any obstacles there or any goal object there.
/// </summary>
public enum TileType
{
    FREE,
    GOAL,
    OBSTACLE
}

/// <summary>
/// The direction. North means upside of the screen.
/// </summary>
public enum Orientation
{
    EAST = 0,
    NORTH = 1,
    WEST = 2,
    SOUTH = 3
}