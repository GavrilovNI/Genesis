using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class Separate : BotGeneCommand
    {
        public override bool IsFinal => true;

        public override void Apply(Bot bot)
        {
            bot.Separate();
            bot.MoveCommand(1);
        }

        public override int GetCode()
        {
            return 41;
        }
    }
}
