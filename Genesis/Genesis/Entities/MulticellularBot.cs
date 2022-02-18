using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Entities
{
    public class MulticellularBot : Bot
    {
        private MulticellularBot? _prev;
        private MulticellularBot? _next;

        private MulticellularBot Head => _prev?.Head ?? this;
        public bool IsHead => _prev == null;
        public bool IsTail => _next == null;
        public bool IsBody => IsHead == false && IsTail == false;

        public override bool IsMulticellular => (IsHead && IsTail) == false;

        public override int Energy
        {
            get
            {
                if (IsHead)
                    return base.Energy;
                else
                    return Head.Energy;
            }
            set
            {
                if (IsHead)
                    base.Energy = value;
                else
                    Head.Energy = value;
            }
        }


        public override int Minerals
        {
            get
            {
                if (IsHead)
                    return base.Minerals;
                else
                    return Head.Minerals;
            }
            set
            {
                if (IsHead)
                    base.Minerals = value;
                else
                    Head.Minerals = value;
            }
        }


        public MulticellularBot(Map map, Bot bot) : base(map, bot.Energy)
        {
            Minerals = bot.Minerals;
        }
        public MulticellularBot(Map map) : base(map, 1)
        {
            Minerals = 0;
        }

        private void MakeUnicellular()
        {
            if (IsAlive == false)
                return;

            var position = Position;
            Bot bot = new Bot(this);
            Map map = RemoveFromMap()!;
            map.AddEntity(position, bot);
        }

        public override void Kill()
        {
            if (IsAlive == false)
                return;

            MulticellularBot head = Head;
            
            if(_prev != null)
            {
                _prev._next = null;
            }

            if(_next != null)
            {
                _next._prev = null;
                if(IsHead)
                {
                    _next.Energy = Energy;
                    _next.Minerals = Minerals;
                }
                else
                {
                    _next.Energy = head.Energy / 2;
                    _next.Minerals = head.Minerals / 2;

                    head.Energy -= _next.Energy;
                    head.Minerals -= _next.Minerals;
                }
            }


            if (_prev != null && _prev.IsMulticellular == false)
                _prev.MakeUnicellular();
            if (_next != null && _next.IsMulticellular == false)
                _next.MakeUnicellular();

            _prev = _next = null;

            base.Kill();
        }

        public void Connect(MulticellularBot other)
        {
            if (_next == null)
            {
                _next = other;
                _next._prev = this;
            }
            else if (_prev == null)
            {
                _prev = other;
                _prev._next = this;
                _prev.Energy = Energy;
                _prev.Minerals = Minerals;
                Energy = 1;
                Minerals = 0;
            }
            else
            {
                throw new InvalidOperationException("Both sides are connected!");
            }
        }

        public override Bot? SeparateConnected(ref Bot thisBot)
        {
            thisBot = this;
            if (IsAlive == false)
                return null;

            if (IsBody)
                return SeparateForcedNotConnected();

            Energy -= ENERGY_TO_SEPARATE;
            if (Energy <= 0)
            {
                Kill();
                return null;
            }

            Map map = Map!;

            Vector2Int? position = map.GetEmptyPositionAroundPosition(Position);
            if (position == null)
            {
                Kill();
                return null;
            }

            MulticellularBot newBot = new MulticellularBot(map);
            newBot.MutateWithChance();

            map.AddEntity(position.Value, newBot);

            Connect(newBot);

            return newBot;
        }
    }
}
