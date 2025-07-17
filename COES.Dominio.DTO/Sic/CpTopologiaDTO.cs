using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    public partial class CpTopologiaDTO : EntityBase
    {
        public int Topcodi { get; set; }
        public string Topnombre { get; set; }
        public System.DateTime Topfecha { get; set; }
        public int Topinicio { get; set; }
        public short Tophorizonte { get; set; }
        public string Lastuser { get; set; }
        public Nullable<System.DateTime> Lastdate { get; set; }
        public short Tophora { get; set; }
        public short Toptipo { get; set; }
        public int Topdiasproc { get; set; }

        public int Topfinal { get; set; }
        public string Topuserfinal { get; set; }
        public DateTime Topfechafinal { get; set; }
        public string Topusersicoes { get; set; }
        public DateTime Topfechasicoes { get; set; }
        public int Topiniciohora { get; set; }
        public int Topfinhora { get; set; }

        //Indica si el escaenar se crea en blanco o a partir de Topologia Base
        public bool Escenblanco { get; set; }
        public string TopFechaStr { get; set; }
        public int Topsinrsf { get; set; }
        public int Topcodisinrsf { get; set; }
        // Duplicar
        public int Desfase { get; set; }

        public bool Toppadre { get; set; }
        public int HoraReprograma { get; set; }

        public string Topuserdespacho { get; set; }
        public DateTime? Topfechadespacho { get; set; }

        public string Color { get; set; }
    }

    public partial class CpTopologiaDTO
    {
        public bool EsOficial { get; set; }
        public bool EsUltimoPreliminar { get; set; }

        #region YupanaContinuo
        public bool EsReprograma { get; set; }
        public int OrdenReprograma { get; set; }
        public int Fverscodi { get; set; }
        public int Avercodi { get; set; }
        public string Identificador { get; set; }
        #endregion
    }

    public class ReporteResumenYupana
    {
        public int TipoInformacion { get; set; }
        public string NombreHoja { get; set; }
        public string HojaTotal { get; set; }
    }
}
