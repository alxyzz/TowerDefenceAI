using GameFramework;
using System;
using System.Collections.Generic;

namespace AI_Strategy
{
    /*
     * very simple example strategy based on random placement of units.
     */
    public class AlexSoldierPlacementStrategy : AbstractStrategy
    {
        


        private static AlexSoldierPlacementStrategy _instance;
        private static readonly object _lock = new object();
        public static AlexSoldierPlacementStrategy GetInstance(Player player)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AlexSoldierPlacementStrategy(player);
                    }
                }
            }
            return _instance;
        }
        private int messageCounter = 1;

        private static List<Soldier> worms = new(); //list of squads. a solo tower can fire on a solo soldier 3 times to kill it- but a soldier needs 5 turns to pass through a tower's range. 
        //this means that a tower needs to be matched with 2 intact soldiers to pass thru. then we should also find the weakest possible spot

        List<Cell> columnPositions = new();//to fill out with troop ranks. if all full, 

        private static Random random = new Random();

        public AlexSoldierPlacementStrategy(Player player) : base(player)
        {

        }

        /*
         * example strategy for deploying Towers based on random placement and budget.
         * Your one should be better!
         */

        public bool  AddSoldierToWorm(AlexInfantryWave me)
        {
            if (worms.Count < 8)
            {
                worms.Add(me);
                return true;
            }
            else return false;
            
        }


        //places towers  as evenly distributed as possible, and should always have about 2 spaces below them to maximize firing area
        //also, no towers should be placed on the outermost 2 tiles unless there's no space anywhere else
        public override void DeployTowers()
        {
            if (player.Gold > 8)
            {
                //Boolean positioned = false;
                int count = 0;
                while (/*!positioned && */count < 6)
                {
                    count++;
                    int x = random.Next(PlayerLane.WIDTH +2, PlayerLane.WIDTH-2);
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
            //if gold is over 30 - spawn 6 soldiers as a squad



            //DebugLoger.Log(Tower.GetNextTowerCosts(defendLane));
            DebugLogger.Log("#" + messageCounter + " Deployed Soldier!");
            messageCounter++;

            while (messageCounter is > 5 and <= 15)
            {
                DebugLogger.Log("#" + messageCounter + " " + random.Next(1000), true);
                //DebugLoger.Log("#" + messageCounter + ": " + random.Next(1000));
                messageCounter++;

                System.Threading.Thread.Sleep(50);
            }


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

                        if (player.Gold < 30)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                player.TryBuySoldier<AlexInfantryWave>(x);
                            }

                        }
                        else
                        { player.TryBuySoldier<AlexInfantryWave>(x); }

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
