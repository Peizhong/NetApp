using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Avmt
{
    public class FunctionLocation : AccountBase
    {
        public string FlName => Name;
        public int FlType => ObjectType;
        public int RunningState => State;

        public string FlCode { get; set; }
        public int SortNo { get; set; }
    }

    public class LineFunctionLocation : FunctionLocation
    {
        public string PhaseSequence { get; set; }
        public double OverheadLineLength { get; set; }
        public string ConnCodeLoad { get; set; }
    }

    public class FeederFunctionLocation : FunctionLocation
    {
        public int LineType { get; set; }
        public double OverheadLineLength { get; set; }
        public double CableLineLength { get; set; }
    }

    public class SubstationFunctionLocation : FunctionLocation
    {
        public int OutputCount { get; set; }
        public int InputCount { get; set; }
    }
}