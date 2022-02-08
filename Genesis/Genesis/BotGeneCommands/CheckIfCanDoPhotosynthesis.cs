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
        public override void Apply(Bot bot)
        {
            bool canDoPhotosynthesis = bot.Map!.GetSunEnergy(bot.Position) > 0;

            int commandMoveDelta;
            if (canDoPhotosynthesis)
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
            return 44;
        }
    }
}
