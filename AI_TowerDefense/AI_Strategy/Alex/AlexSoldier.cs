using GameFramework;
using System.Collections.Generic;

namespace AI_Strategy
{
    /*
     * This class derives from Soldier and provides a new move method. Your assignment should
     * do the same - but with your own movement strategy.
     */
    public class AlexSoldier : Soldier
    {

        //say we have a weak spot with a coverage of 3 towers for 5 tiles.
        //15 shots of 2hp each. a soldier has 6 HP. so we need 5 soldiers to pass thru. a column of 6 units can pass thru that and still reach the enemy base.
        //but obviously they'll have a ton of towers...
        // i need to find the optimal height/width for a platoon of soldiers, because surely there's a better way than just filling the row horizontally like some weird napoleon warfare firing line lol
        //except lategame
       
        
        //movement wise, it's probably wiser to just let them move as normal. since the strategy lies in their placement. and lategame the game is mostly about pumping out as many soldiers as possible and having a good tower structure anyways
        //plus the fact that towers make the most gold...

        public override void Move()
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
    }
}
