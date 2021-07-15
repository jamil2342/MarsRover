using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IWorld
{
    bool IsPositionFree(Position pos);

    bool IsGoalReached(Position pos);
}