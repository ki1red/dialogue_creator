using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DialogsCreator
{
    internal class TestBindAndUnbindOBJ2 : LinkedObject
    {
        public override void Bounds(LinkedObject linkObject)
        {
            MessageBox.Show("You bounds obj");
        }

        public override void UnBounds(LinkedObject linkObject)
        {
            MessageBox.Show("You unbounds obj");
        }
    }
}
