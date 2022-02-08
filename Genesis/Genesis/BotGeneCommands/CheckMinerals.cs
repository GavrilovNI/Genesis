using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class CheckMinerals : BotGeneCommand
    {
        public override bool IsFinal => false;
        public override int Code => 39;

        public override void Apply(Bot bot)
        {
            float compareWith = 1f * bot.GetCommand(bot.CurrentCommand + 1).Code * bot.MaxMinerals / bot.GeneSize;

            int commandMoveDelta;
            if (bot.Minerals < compareWith)
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 2).Code;
            }
            else
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 3).Code;
            }
            bot.MoveCommand(commandMoveDelta);
        }

    }
}
