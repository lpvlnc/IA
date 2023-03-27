using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public enum CharacterType { NPC, Player };

    public class Character
    {
        public CharacterType Type { get; set; }

        public bool Active { get; set; }

        public bool Alive { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public int Stamina { get; set; }
    }
}
