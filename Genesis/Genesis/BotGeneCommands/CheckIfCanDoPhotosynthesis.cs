using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class CheckIfCanDoPhotosynthesis : BotGeneCommand
    {
        public override bool IsFinal => false;
        public override int Code => 44;

        public override void Apply(Bot bot)
        {
            bool canDoPhotosynthesis = bot.Map!.GetSunEnergy(bot.Position) > 0;

            int commandMoveDelta;
            if (canDoPhotosynthesis)
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 1).Code;
            }
            else
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 2).Code;
            }
            bot.MoveCommand(commandMoveDelta);
        }
    }
}
