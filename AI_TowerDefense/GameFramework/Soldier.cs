﻿namespace GameFramework
{
    /*
     * Automatically called by the game play. Forbidden to use as part of
     * the assignment.
     * 
     * A Soldier is an active, movable unit. Soldiers are cheap, but cause low damage and can take low damage.
     * Soldiers score, when they reach the opposite side.
     */
    public class Soldier : Unit
    {
        public Soldier(Player player, PlayerLane lane, int x) : base(player, lane)
        {
            speed = 1;
            health = 6;
            damageCaused = 1;
            range = 2;
            type = "S";
            cost = 2;
            posX = x;
            posY = 0;
        }
    }
}