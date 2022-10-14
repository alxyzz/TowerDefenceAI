﻿namespace GameFramework
{

    /*
     * Automatically called by the game play. Forbidden to use as part of
     * the assignment.
     * 
     * A Tower is anon-movable unit. Towers are expensive, but crucial to prevent the
     * opponents soldiers to get through. Strategic placement of towers is crucial to success.
     * 
     */
    public class Tower : Unit
    {
        public static readonly int COSTS = 2;

        /*
         * Can be used to get the costs of the tower to buy next
         */
        public static int GetNextTowerCosts(PlayerLane lane)
        {
            return Tower.COSTS + lane.TowerCount();
        }

        public Tower(Player player, PlayerLane lane, int x, int y) : base(player, lane)
        {
            speed = 0;
            health = 9;
            damageCaused = 2;
            range = 2;
            type = "T";
            cost = COSTS;
            posX = x;
            posY = y;
        }
    }
}