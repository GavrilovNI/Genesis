using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class CheckIfHasEmptyPositionAround : BotGeneCommand
    {
        public override void Apply(Bot bot)
        {
            bool hasEmptyPosition = bot.Map!.GetEmptyPositionAroundPosition(bot.Position) != null;

            int commandMoveDelta;
            if (hasEmptyPosition)
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 1).GetCode();
            }
            else
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 2).GetCode();
            }
            bot.MoveCommand(commandMoveDelta);
        }

        public override int GetCode()
        {
            return 43;
        }
    }
}
