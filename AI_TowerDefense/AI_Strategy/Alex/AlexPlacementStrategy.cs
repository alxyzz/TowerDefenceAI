using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AI_Strategy
{

    public class AlexPlacementStrategy : AbstractStrategy
    {
        private static AlexPlacementStrategy _instance;
        private static readonly object _lock = new object();
        public static AlexPlacementStrategy GetInstance(Player player)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AlexPlacementStrategy(player);
                    }
                }
            }
            return _instance;
        }
        private int round = 0;
        bool deploymentDir = true;

        //list of "tanks". a solo tower can fire on a solo soldier 3 times to kill it- but a soldier needs 5 turns to pass through a tower's range. 
        //this means that a tower needs to be matched with 2 intact soldiers to pass thru. phase two sends tanks, but later on we just send all the conscripts at once. cannon fodder

        private static Random random = new Random();

        public AlexPlacementStrategy(Player player) : base(player)
        {
        }

        /*
         * example strategy for deploying Towers based on random placement and budget.
         * Your one should be better!
         */

        //places as many towers as possible
        //also, no towers should be placed on the outermost 2 tiles unless there's no space anywhere else- actually, more towers fit on a line if we do that, so...
        //also considering that towers are our moneymakers there's literally no reason to send any soldiers unless we know for sure that they'll also kill a tower, or actually reach the end of the enemy's lane preferably
        //center squares have more firing range so they're also prioritized 

        public override void DeployTowers()
        {
            List<int> validLanes = new List<int>() { 2, 4, 1, 3, 5, 0, 6 };
            int startRow = (int)(PlayerLane.HEIGHT * 0.75);
            int bottomRows = 2;

            if (player.Gold < 8)
                return;

            for (int y = startRow; y >= bottomRows; y--)
            {
                if (y % 2 == 0)
                {
                    foreach (int lane in validLanes)
                        PlaceTower(lane, y);

                    for (int lane = 0; lane < PlayerLane.WIDTH; lane++)
                    {
                        if (!validLanes.Contains(lane))
                            PlaceTower(lane, y);
                    }
                }
                else //diagonals
                {
                   
                    foreach (int lane in validLanes)
                    {
                        int diagonalLane = lane + 1; 
                        if (diagonalLane < PlayerLane.WIDTH)
                            PlaceTower(diagonalLane, y);
                    }
                }
            }

            void PlaceTower(int lane, int row)
            {
                if (lane < 0 || lane >= PlayerLane.WIDTH || row < 0 || row >= PlayerLane.HEIGHT)
                    return;

                if (CellHasFreeADjacents(lane, row))
                    HandleErrors(player.TryBuyTower<Tower>(lane, row), $"x={lane}, y={row}");
            }

            bool CellHasFreeADjacents(int x, int y)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    int neighborX = x + dx;
                    if (neighborX < 0 || neighborX >= PlayerLane.WIDTH)
                        continue;

                    int neighborY = y;
                    if (player.HomeLane.GetCellAt(neighborX, neighborY).Unit != null)
                        return false;
                }
                return true;
            }

            bool HandleErrors(Player.TowerPlacementResult res, string coords)
            {
                switch (res)
                {
                    case Player.TowerPlacementResult.Success:
                        return true;

                    default:
                        // DebugLogger.Log(coords + " - " + res.ToString());
                        return false;
                }
            }
        }
        /*
         * example strategy for deploying Soldiers based on random placement and budget.
         * Yours should be better!
         */

        public override void DeploySoldiers()
        {
            if (player.Gold < 5)
            {
                return;
            }

            round++;

            if (round < 10)
            {
                ScoutAttack(8); 
            }
            else if (round >= 10 && round < 500)
            {
                SquadAttack(6);
            }
            else
            {
                FullAttack(); 
            }

            void ScoutAttack(int groupSize)
            {
                int leftEdge = 0;
                int rightEdge = PlayerLane.WIDTH - 1;

                while (player.Gold >= groupSize * 5)
                {
                    //we prefer the edges as they have less spaces to hit- just like in chess, where a knight is the most powerful in the middle of the board, so is a tower here. so naturally we hit where they're weak. atleast in the beginning before the Flood Waves Begin....
                    //though this doesn't really happen because there's not enough gold that early on, towers are far more worth it, and a partial investment is far worse than no soldier investment- it just feeds funds to the enemy
                    if (player.EnemyLane.GetCellAt(leftEdge, 0).Unit == null)
                    {
                        for (int i = 0; i < groupSize; i++)
                        {
                            player.TryBuySoldier<AlexSoldier>(leftEdge);
                        }
                    }

                    if (player.EnemyLane.GetCellAt(rightEdge, 0).Unit == null)
                    {
                        for (int i = 0; i < groupSize; i++)
                        {
                            player.TryBuySoldier<AlexSoldier>(rightEdge);
                        }
                    }
                }
            }

            void SquadAttack(int groupSize)
            {
                //if by chance we have enough money for soldier investment in between round 10 and 100, we'll send them out as squads. better than stragglers but you just can't compete with the all-out row flood attack once you have enough cashflow for that

                int middleLane = PlayerLane.WIDTH / 2;

                while (player.Gold >= groupSize * 5)
                {
                    for (int offset = -1; offset <= 1; offset++)
                    {
                        int lane = middleLane + offset;

                        if (lane >= 0 && lane < PlayerLane.WIDTH && player.EnemyLane.GetCellAt(lane, 0).Unit == null)
                        {
                            for (int i = 0; i < groupSize; i++)
                            {
                                player.TryBuySoldier<AlexSoldier>(lane);
                            }
                        }
                    }
                }
            }

            void FullAttack()
            {
                if (deploymentDir) //we do a tricky here. if the enemy has some sort of "place towers where there are more enemies" features, alternating the placement direction should mess with them a bit. though its more of a numbers game and just maximizing towers per lane count is better...
                {
                    for (int x = 0; x < PlayerLane.WIDTH; x++) //left to right
                    {
                        while (player.Gold >= 5 && player.EnemyLane.GetCellAt(x, 0).Unit == null)
                        {
                            player.TryBuySoldier<AlexSoldier>(x);
                        }
                    }
                }
                else
                {
                    for (int x = PlayerLane.WIDTH - 1; x >= 0; x--) // right to left
                    {
                        while (player.Gold >= 5 && player.EnemyLane.GetCellAt(x, 0).Unit == null)
                        {
                            player.TryBuySoldier<AlexSoldier>(x);
                        }
                    }
                }
                deploymentDir = !deploymentDir;
            }

        }

        public override List<Soldier> SortedSoldierArray(List<Soldier> unsortedList)
        {

            //pawns closer to the enemy ranks move first. So they also shoot first
            return unsortedList.OrderByDescending(soldier => soldier.PosY).ToList();
        }

        // The order in which the array is returned here is the order in which towers will plan and perform their action.
        public override List<Tower> SortedTowerArray(List<Tower> unsortedList)

        {


            //as above, the ones closer to the enemy (higher Y level) attack first. this only applies if my strategy is the first player though.
            return unsortedList.OrderByDescending(tower => tower.PosY).ToList();
        }

    }
}
