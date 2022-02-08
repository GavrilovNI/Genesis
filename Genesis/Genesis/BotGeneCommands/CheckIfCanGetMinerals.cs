using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class CheckIfCanGetMinerals : BotGeneCommand
    {
        public override bool IsFinal => false;
        public override int Code => 45;

        public override void Apply(Bot bot)
        {
            bool canGetMinerals = bot.Map!.GetMinerals(bot.Position) > 0;

            int commandMoveDelta;
            if (canGetMinerals)
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
