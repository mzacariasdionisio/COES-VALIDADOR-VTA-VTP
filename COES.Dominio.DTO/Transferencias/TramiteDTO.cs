using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_TRAMITE
    /// </summary>
    public class TramiteDTO
    {
        public System.Int32 TramCodi { get; set; }
        public System.String UsuaCoesCodi { get; set; }
        public System.String UsuaSeinCodi { get; set; }
        public System.Int32? EmprCodi { get; set; }
        public System.Int32? PeriCodi  { get; set; }
        public System.Int32 TipoTramcodi { get; set; }
        public System.String TramCorrInf { get; set; }
        public System.String TramDescripcion { get; set; }
        public System.String TramRespuesta { get; set; }
        public System.DateTime TramFecReg { get; set; }
        public System.DateTime? TramFecRes { get; set; }
        public System.DateTime? TramFecIns { get; set; }
        public System.DateTime? TramFecAct { get; set; }
        public System.Int32 TramVersion { get; set; }
        public System.String TipoTramNombre { get; set; }
        public System.String EmprNombre { get; set; }
        public bool bGrabar { get; set; }

    }
}
