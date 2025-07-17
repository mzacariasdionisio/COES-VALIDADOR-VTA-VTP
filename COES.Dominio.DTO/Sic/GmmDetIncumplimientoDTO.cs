using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public partial class GmmDetIncumplimientoDTO : EntityBase
    {
        /// <summary>
        /// Clase que mapea la tabla GMM_DETINCUMPLIMIENTO
        /// </summary>
        public int INCUCODI { get; set; }
        public string TINFCODI { get; set; }
        public DateTime? DINCFECRECEPCION { get; set; }
        public string DINCARCHIVO { get; set; }
        public int DINCCODI { get; set; }
        public string DINCESTADO { get; set; }

    }

    public partial class GmmDetIncumplimientoDTO : EntityBase
    {
        //Lista de documentos
        public int Detdinccodi { get; set; }
        public string DetTipInforme { get; set; }
        public DateTime? DetIncFecEntregaDet { get; set; }
        public string DetIncArchivoDet { get; set; }
    }
}
