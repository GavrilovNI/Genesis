using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class SeparateConnected : BotGeneCommand
    {
        public override bool IsFinal => true;
        public override int Code => 40;

        public override void Apply(Bot bot)
        {
            bot.SeparateConnected(ref bot);
            bot.MoveCommand(1);
        }
    }
}
