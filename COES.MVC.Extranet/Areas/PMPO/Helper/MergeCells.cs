using System;
using System.Runtime.Serialization;

namespace COES.MVC.Extranet.Areas.PMPO.Helper
{
    [DataContract]
    public class MergeCells
    {
        public MergeCells() { }

        public MergeCells(int rowIndex, int columnIndex, int rowSpan, int colSpan)
        {
            this.RowIndex = rowIndex;
            this.ColumnIndex = columnIndex;
            this.RowSpan = rowSpan;
            this.ColumnSpan = colSpan;
        }

        [DataMember(Name = "rowspan", Order = 0)]
        public int RowSpan { get; set; }

        [DataMember(Name = "colspan", Order = 1)]
        public int ColumnSpan { get; set; }

        [DataMember(Name = "row", Order = 2)]
        public int RowIndex { get; set; }

        [DataMember(Name = "col", Order = 3)]
        public int ColumnIndex { get; set; }
    }
}