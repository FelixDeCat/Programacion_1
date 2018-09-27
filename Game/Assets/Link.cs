using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Link
{
    public static Entity MoveLink(this Entity ent, Move m)
    {
        ent.moves.Add(m);
        return ent;
    }
}
