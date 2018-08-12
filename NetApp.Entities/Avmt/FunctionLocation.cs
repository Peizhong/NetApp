using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetApp.Entities.Avmt
{
    [Table("dm_function_location")]
    public class FunctionLocation : AccountBase
    {
        [Column("fl_name")]
        public string FlName { get; set; }

        [Column("full_path")]
        public string FullPath { get; set; }

        [Column("fl_type")]
        public int FlType { get; set; }

        [Column("running_state")]
        public int RunningState { get; set; }

        [Column("fl_code")]
        public string FlCode { get; set; }

        [Column("sort_no")]
        public double SortNo { get; set; }
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