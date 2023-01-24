using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsCreator
{
    public class Roots
    {
        public enum root
        {
            admin = 0,
            scenarist = 1,
            translator = 2
        }

        public root r { get; private set; }
        public Roots() { }

        public void SetRoot(string _root)
        {
            switch (_root)
            {
                case "Администратор":
                    r = root.admin;
                    break;
                case "Сценарист":
                    r = root.scenarist;
                    break;
                case "Переводчик":
                    r = root.translator;
                    break;
                default:
                    throw new Exception("Отсутствует такой тип прав");
            }
        }
    }
}
