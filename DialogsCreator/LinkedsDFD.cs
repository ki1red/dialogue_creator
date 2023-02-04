﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsCreator
{
    [Serializable]
    public class Vector4
    {
        public double X1 { get; private set; }
        public double X2 { get; private set; }
        public double Y1 { get; private set; }
        public double Y2 { get; private set; }

        public Vector4(double x1, double x2, double y1, double y2)
        {
            this.X1 = x1;
            this.X2 = x2;
            this.Y1 = y1;
            this.Y2 = y2;
        }
    }

    [Serializable]
    public class LinkDataDialogPackageSerialize
    {
        public int OutIdDialogView { get; private set; }
        public int OutIdOptionView { get; private set; }
        public int OutIdBindingView { get; private set; }

        public int InIdDialogView { get; private set; }
        public int InIdOptionView { get; private set; }
        public int InIdBindingView { get; private set; }

        public Vector4[] LinesCoords { get; private set; }

        public LinkDataDialogPackageSerialize(int outIdDialogView, int outIdBindingView, int inIdDialogView, int inIdBindingView, Vector4[] linesCoords)
        {
            OutIdDialogView = outIdDialogView;
            OutIdOptionView = -1;
            OutIdBindingView = outIdBindingView;
            InIdDialogView = inIdDialogView;
            InIdOptionView = -1;
            InIdBindingView = inIdBindingView;
            LinesCoords = linesCoords;
        }

        public LinkDataDialogPackageSerialize(int outIdDialogView, int outIdOptionView, int outIdBindingView, int inIdDialogView, int inIdOptionView, int inIdBindingView, Vector4[] linesCoords)
        {
            OutIdDialogView = outIdDialogView;
            OutIdOptionView = outIdOptionView;
            OutIdBindingView = outIdBindingView;
            InIdDialogView = inIdDialogView;
            InIdOptionView = inIdOptionView;
            InIdBindingView = inIdBindingView;
            LinesCoords = linesCoords;
        }
    }
}
