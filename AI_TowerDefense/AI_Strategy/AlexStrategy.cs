using GameFramework;
using System;
using System.Collections.Generic;

namespace AI_Strategy
{
    /*
     * very simple example strategy based on random placement of units.
     */
    public class AlexStrategy : AbstractStrategy
    {
        //reserve - leaves a sum of gold in store for the Final Attack.
        //clump placement - a number of soldiers together are worth more than said soldiers alone - we should always try to send bursts out. this both preempts the enemy strategy only checking if gold is over a sum then placing a singular tower, and also tries to overwhelm the enemy towers
        //if both lanes were a single one, linked- i'd honestly just pass on towers and leave sentries around- they're cheaper for the price


        private static AlexStrategy _instance;
        public static AlexStrategy GetInstance(Player player)
        {
            return _instance;
        }

        private static List<Soldier> _Sentries; //list of squads. a solo tower can fire on a solo soldier 3 times to kill it- but a soldier needs 5 turns to pass through a tower's range. 
        //this means that a tower needs to be matched with 2 intact soldiers to pass thru

        private static Random random = new Random();

        public AlexStrategy(Player player) : base(player)
        {
        }

        /*
         * example strategy for deploying Towers based on random placement and budget.
         * Your one should be better!
         */


        //places ~~towers~~ SENTRIES as evenly distributed as possible, and should always have about 2 spaces below them to maximize firing area
        //also, no towers should be placed on the outermost 2 tiles unless there's no space anywhere else
        public override void DeployTowers()
        {
            if (player.Gold > 8)
            {
                //Boolean positioned = false;
                int count = 0;
                while (/*!positioned && */count < 20)
                {
                    count++;
                    int x = random.Next(PlayerLane.WIDTH);
                    int y = random.Next(PlayerLane.HEIGHT - 1) + 1; // has to leave soldier deploy lane empty
                    if (player.HomeLane.GetCellAt(x, y).Unit == null)
                    {
                        //positioned = true;
                        player.TryBuyTower<Tower>(x, y);
                    }
                }
            }

        }

        /*
         * example strategy for deploying Soldiers based on random placement and budget.
         * Yours should be better!
         */
        public override void DeploySoldiers()
        {
            int round = 0;
            while (player.Gold > 5 && round < 5)
            {
                round++;
                bool positioned = false;
                int count = 0;
                while (!positioned && count < 10)
                {
                    count++;
                    int x = random.Next(PlayerLane.WIDTH);
                    int y = 0;
                    if (player.EnemyLane.GetCellAt(x, y).Unit == null)
                    {
                        positioned = true;
                        player.TryBuySoldier<AnnihilationWorm>(x);
                    }
                }
            }
        }

        /*
         * called by the game play environment. The order in which the array is returned here is
         * the order in which soldiers will plan and perform their movement.
         *
         * The default implementation does not change the order. Do better!
         */
        public override List<Soldier> SortedSoldierArray(List<Soldier> unsortedList)
        {
            return unsortedList;
        }

        /*
         * called by the game play environment. The order in which the array is returned here is
         * the order in which towers will plan and perform their action.
         *
         * The default implementation does not change the order. Do better!
         */
        public override List<Tower> SortedTowerArray(List<Tower> unsortedList)

        {
            return unsortedList;
        }

    }
}
