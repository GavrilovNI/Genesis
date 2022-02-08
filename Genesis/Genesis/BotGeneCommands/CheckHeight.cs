using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class CheckHeight : BotGeneCommand
    {
        public override bool IsFinal => false;

        public override void Apply(Bot bot)
        {
            float compareWith = 1f * bot.GetCommand(bot.CurrentCommand + 1).GetCode() * bot.Map!.Size.Y / bot.GeneSize;

            Vector2Int botPosition = bot.Position;

            int commandMoveDelta;
            if(botPosition.Y < compareWith)
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
            return 37;
        }
    }
}
