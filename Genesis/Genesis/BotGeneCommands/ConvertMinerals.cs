using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class ConvertMinerals : BotGeneCommand
    {
        public override bool IsFinal => true;
        public override int Code => 47;

        public override void Apply(Bot bot)
        {
            bot.ConvertMinerals();
            bot.MoveCommand(1);
        }
    }
}
