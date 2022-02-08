using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class Photosynthesis : BotGeneCommand
    {
        public override bool IsFinal => true;
        public override int Code => 25;

        public override void Apply(Bot bot)
        {
            bot.Photosynthesis();
            bot.MoveCommand(1);
        }
    }
}
