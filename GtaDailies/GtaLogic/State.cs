using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaLogic
{
    public sealed class State
    {
        public static State Load()
        {
            var state = new State();
            return state;
        }

        public void Save()
        {

        }

        private State()
        {
        }

    }
}
