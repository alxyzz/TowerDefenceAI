using GameFramework;
namespace AI_Strategy
{
    /*
     * This class derives from Soldier and is meant to ANNIHILATE the enemy.
     */
    public class MultifunctionalSoldier : Soldier
    {
        /*
         * This move method is a mere copy of the base movement method.
         */

        enum soldierState
        {
            Hunter,
            AnnihilationWorm,
            Return
        }

        soldierState state = MultifunctionalSoldier.soldierState.Return;

        //get cells on map and find a path that leads through the zone of as few towers as possible. then it's simply a matter of time
        public override void Move()
        {
            switch (state)
            {
                case soldierState.Hunter:
                    HunterAct();
                    break;
                case soldierState.AnnihilationWorm:
                    WormAct();
                    break;
                case soldierState.Return:
                    break;
                default:
                    break;
            }




            void HunterAct()
            {
                if (speed > 0 && posY < PlayerLane.HEIGHT)
                {
                    int x = posX;
                    int y = posY;
                    for (int i = speed; i > 0; i--)
                    {
                        if (MoveTo(x, y + i)) return;
                        if (MoveTo(x + i, y + i)) return;
                        if (MoveTo(x - i, y + i)) return;
                        if (MoveTo(x + i, y)) return;
                        if (MoveTo(x - i, y)) return;
                        if (MoveTo(x, y - i)) return;
                        if (MoveTo(x - i, y - i)) return;
                        if (MoveTo(x + i, y - i)) return;
                    }
                }
            }


            void WormAct(){




            }


            void ReturnAct(){



            }

          
        }
    }
}
