using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public interface IRover
{
    Position GetPosition();

    Position GetNextPosition(Command command);

    void UpdatePosition(Position position);

    int Rotate(Command com);
}

public class Rover1 : IRover
{
    private int direction = (int)Orientation.NORTH;
    private Position ct = new Position(7, 7);

    public Position GetNextPosition(Command command)
    {
        Position ct = new Position(this.ct.Row, this.ct.Col);

        if (this.direction == (int)Orientation.NORTH)
        {
            ct.Row--;
        }
        else if (this.direction == (int)Orientation.SOUTH)
        {
            ct.Row++;
        }
        else if (this.direction == (int)Orientation.EAST)
        {
            ct.Col--;
        }
        else if (this.direction == (int)Orientation.WEST)
        {
            ct.Col++;
        }

        return ct;
    }

    public Position GetPosition()
    {
        return this.ct;
    }

    public int Rotate(Command com)
    {
        if (com == Command.ROTATE_RIGHT)
        {
            this.direction++;
        }
        else if (com == Command.ROTATE_LEFT)
        {
            this.direction--;
        }

        if (this.direction < 0)
        {
            this.direction = 3;
        }

        this.direction = this.direction % 4;
        return this.direction;
    }

    public void UpdatePosition(Position position)
    {
        this.ct = position;
    }
}