﻿using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class Eat : RelationCommand
    {
        public const int ENERGY_TO_EAT = 4;

        public Eat(bool relative) : base(relative)
        {
        }

        public override void Apply(Bot bot, EntityType entityType, Vector2Int lookPosition)
        {
            bot.Energy -= ENERGY_TO_EAT;
            if (entityType == EntityType.Organic)
            {
                Organic organic = (Organic)bot.Map!.GetEntity(lookPosition);
                bot.AddEnergy(organic.GetEnergy());
                organic.Kill();
            }
            else if(entityType == EntityType.Bot)
            {
                Bot other = (Bot)bot.Map!.GetEntity(lookPosition);
                bot.Fight(other);
            }
        }

        public override int GetCode()
        {
            return Relative ? 30 : 31;
        }
    }
}
