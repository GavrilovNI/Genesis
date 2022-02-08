using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class CheckEnergy : BotGeneCommand
    {
        public override void Apply(Bot bot)
        {
            float compareWith = 1f * bot.GetCommand(bot.CurrentCommand + 1).GetCode() * bot.MaxEnergy / bot.GeneSize;

            int commandMoveDelta;
            if (bot.Energy < compareWith)
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 2).GetCode();
            }
            else
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 3).GetCode();
            }
            bot.MoveCommand(commandMoveDelta);
        }

        public override int GetCode()
        {
            return 38;
        }
    }
}
