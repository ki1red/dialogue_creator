using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DialogsCreator
{
    internal class TestBindAndUnbinOBJ : LinkedObject
    {
        public override void Bounds(LinkedObject linkObject)
        {
            MessageBox.Show("You link obj" );
        }

        public override void UnBounds(LinkedObject linkObject)
        {
            MessageBox.Show("You unlink obj");
        }
    }
}
