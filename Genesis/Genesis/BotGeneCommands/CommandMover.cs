using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class CommandMover : BotGeneCommand
    {
        public override bool IsFinal => false;
        public override int Code => _delta;

        private int _delta;

        public CommandMover(int delta)
        {
            if(delta < 0)
                throw new ArgumentOutOfRangeException(nameof(delta));

            _delta = delta;
        }

        public override void Apply(Bot bot)
        {
            bot.MoveCommand(_delta);
        }
    }
}
