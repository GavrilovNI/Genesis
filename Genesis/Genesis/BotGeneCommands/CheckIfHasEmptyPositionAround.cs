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
        public override bool IsFinal => false;
        public override int Code => 43;

        public override void Apply(Bot bot)
        {
            bool hasEmptyPosition = bot.Map!.GetEmptyPositionAroundPosition(bot.Position) != null;

            int commandMoveDelta;
            if (hasEmptyPosition)
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 2).Code;
            }
            else
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 1).Code;
            }
            bot.MoveCommand(commandMoveDelta);
        }
    }
}
