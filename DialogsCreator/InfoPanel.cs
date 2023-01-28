using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsCreator
{
    public class InfoPanel
    {
        public SelectionObject selectionObject { get; private set; }
        public InfoPanel() { }

        public void Show(SelectionObject selectionObject) { this.selectionObject = selectionObject; }
    }
}
