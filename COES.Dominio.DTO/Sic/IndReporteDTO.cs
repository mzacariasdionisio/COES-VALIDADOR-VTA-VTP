using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_REPORTE
    /// </summary>
    public partial class IndReporteDTO : EntityBase, ICloneable
    {
        public int Irptcodi { get; set; }
        public int Icuacodi { get; set; }
        public int Irecacodi { get; set; }
        public string Irptestado { get; set; }
        public string Irpttipo { get; set; }
        public string Irpttiempo { get; set; }
        public string Irptmedicionorigen { get; set; }
        public int Irptnumversion { get; set; }
        public int Irptesfinal { get; set; }
        public int? Irptreporteold { get; set; }
        public string Irptusucreacion { get; set; }
        public DateTime Irptfeccreacion { get; set; }
        public string Irptusumodificacion { get; set; }
        public DateTime? Irptfecmodificacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class IndReporteDTO : EntityBase
    {
        public string IrpttiempoDesc { get; set; }
        public string IrptmedicionorigenDesc { get; set; }
        public string IrptesfinalDesc { get; set; }
        public string IrptesfinalColorFondo { get; set; }
        public string IrptesfinalColorLetra { get; set; }
        public string IrptfeccreacionDesc { get; set; }
        public string IrptfecmodificacionDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public string Iperinombre { get; set; }
        public string Irecanombre { get; set; }
        public DateTime Irecafechaini { get; set; }
        public DateTime Irecafechafin { get; set; }
        public string Irecainforme { get; set; }
        public string IrecafechainiDesc { get; set; }
        public string IrecafechafinDesc { get; set; }

        public List<IndReporteTotalDTO> ListaIndReporteTotal { get; set; }

    }
}
