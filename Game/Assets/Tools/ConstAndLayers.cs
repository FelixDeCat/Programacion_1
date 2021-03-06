﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class CommonState
{
    public const string IDLE = "Idle";

    public const string MOVING = "Moving";
    public const string JUMPING = "Jumping";
    public const string CROUCH = "Jumping";
    public const string DIE = "Die";

    public const string ONSIGHT = "OnSight";
    public const string PURSUIRT = "Pursuit";
    public const string SEARCHING = "OutOfSight";
    public const string ATTACKING = "Attacking";

    public const string FREEZE = "Freeze";
}

public static class PHYSICAL_INPUT
{
    public const string HORIZONTAL = "Horizontal";
    public const string VERTICAL = "Vertical";
    public const string JUMP = "Jump";
    public const string FIRE1 = "Fire1";
    public const string CROUCH = "Crouch";
}

public class Layers
{
    public static int PLAYER = 13;
    public static int ENEMY = 10;
    public static int WORLD = 11;
    public static int PLAYER_BULLET = 12;
    public static int ENEMY_BULLET = 14;
}

