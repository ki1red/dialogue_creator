using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsCreator
{
    [Serializable]
    public class Vector4
    {
        public double X1;
        public double X2;
        public double Y1;
        public double Y2;

        public Vector4(double X1, double X2, double Y1, double Y2)
        {
            this.X1 = X1;
            this.X2 = X2;
            this.Y1 = Y1;
            this.Y2 = Y2;
        }

        public Vector4()
        {
            this.X1 = -1;
            this.X2 = -1;
            this.Y1 = -1;
            this.Y2 = -1;
        }
    }

    [Serializable]
    public class LinkDataDialogPackageSerialize
    {
        public int OutIdDialogView;
        public int OutIdOptionView;
        public int OutIdBindingView;

        public int InIdDialogView;
        public int InIdOptionView;
        public int InIdBindingView;

        public Vector4[] LinesCoords;

        //public LinkDataDialogPackageSerialize(int outIdDialogView, int outIdBindingView, int inIdDialogView, int inIdBindingView, Vector4[] linesCoords)
        //{
        //    OutIdDialogView = outIdDialogView;
        //    OutIdOptionView = -1;
        //    OutIdBindingView = outIdBindingView;
        //    InIdDialogView = inIdDialogView;
        //    InIdOptionView = -1;
        //    InIdBindingView = inIdBindingView;
        //    LinesCoords = linesCoords;
        //}

        public LinkDataDialogPackageSerialize(int OutIdDialogView, int OutIdOptionView, int OutIdBindingView, int InIdDialogView, int InIdOptionView, int InIdBindingView, Vector4[] LinesCoords)
        {
            this.OutIdDialogView = OutIdDialogView;
            this.OutIdOptionView = OutIdOptionView;
            this.OutIdBindingView = OutIdBindingView;
            this.InIdDialogView = InIdDialogView;
            this.InIdOptionView = InIdOptionView;
            this.InIdBindingView = InIdBindingView;
            this.LinesCoords = LinesCoords;
        }

        public LinkDataDialogPackageSerialize()
        {
            this.OutIdDialogView = -1;
            this.OutIdOptionView = -1;
            this.OutIdBindingView = -1;
            this.InIdDialogView = -1;
            this.InIdOptionView = -1;
            this.InIdBindingView = -1;
            this.LinesCoords = new Vector4[0];
        }
    }
}
