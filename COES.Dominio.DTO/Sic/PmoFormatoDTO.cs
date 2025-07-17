using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public partial class PmoFormatoDTO : EntityBase
    {
        public int PmFTabCodi { get; set; }
        public string PmFTabNombreTabla { get; set; }
        public string PmFTabDescripcionTabla { get; set; }
        public string PmFTabGrupoArchivo { get; set; }
        public string PmFTabTipoArchivo { get; set; }
        public string PmFTabNombArchivo { get; set; }
        public string PmFTabQueryCab { get; set; }
        public string PmFTabQueryDet { get; set; }
        public string PmFTabQueryCount { get; set; }
        public decimal? PmFTabOrden { get; set; }
        public string PmFTabEstRegistro { get; set; }
        public string PmFTabUsuCreacion { get; set; }
        public DateTime? PmFTabFecCreacion { get; set; }
        public string PmFTabUsuModificacion { get; set; }
        public DateTime? PmFTabFecModificacion { get; set; }

        #region SIOSEIN1
        public int Ptomedicodi { get; set; }
        public string Equinomb { get; set; }
        public string Ptomedidesc { get; set; }
        public int? Equicodi { get; set; }
        public string Osinergcodi { get; set; }
        public string Osicodi { get; set; }
        #endregion
    }

    public partial class PmoFormatoDTO
    {
        public string IndexWeb { get; set; }
        public string TipoFormato { get; set; }
    }
}
