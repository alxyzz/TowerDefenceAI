using GameFramework;
using System.Collections.Generic;

namespace AI_Strategy
{
    /*
     * This class derives from Soldier and provides a new move method. Your assignment should
     * do the same - but with your own movement strategy.
     */
    public class AlexInfantryWave : Soldier
    {

        //say we have a weak spot with a coverage of 3 towers for 5 tiles.
        //15 shots of 2hp each. a soldier has 6 HP. so we need 5 soldiers to pass thru. a column of 6 units can pass thru that and still reach the enemy base.
        //but obviously they'll have a ton of towers...
        // i need to find the optimal height/width for a platoon of soldiers, because surely there's a better way than just filling the row horizontally like some weird napoleon warfare firing line lol
        //movement wise, it's probably better to just let them move as normal. since the strategy lies in their placement

        //enum mindstate
        //{
        //    hunter, //hunts the closest tower as a group and runs if it's low on health (to avoid enemy getting gold)
        //    worm, //agreggate as a column and move through the weakest spot
        //    stash, //inactive - deny the enemy their hard-earned gold. 
        //    reset//process situation and see what to do
        //}


        //mindstate mind = mindstate.reset; //default to reset

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
