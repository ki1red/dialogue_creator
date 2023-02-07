using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsCreator
{
    public enum TypeUser
    {
        none = 0,
        painter = 1,
        scenarist = 2,
        translator = 3
    }

    public class Roots 
    {
        public TypeUser typeUser { get; private set; }
        public Roots() { typeUser = TypeUser.none; }

        public bool SetRoot(string root)
        {
            this.typeUser = ToTypeUser(root);

            if (typeUser == TypeUser.none)
                return false;
            else
                return true;
        }

        private TypeUser ToTypeUser(string typeUser)
        {
            switch (typeUser)
            {
                case "painter":
                    return TypeUser.painter;
                case "scenarist":
                    return TypeUser.scenarist;
                case "translator":
                    return TypeUser.translator;
                default:
                    return TypeUser.none;
            }
        }
    }
}
