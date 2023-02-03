using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsCreator
{
    public abstract class LinkedObject
    {
        public abstract void Bounds(LinkedObject outlinkObject, LinkedObject inLinkObject);

        public abstract void UnBounds(LinkedObject linkObject);
    }
}
