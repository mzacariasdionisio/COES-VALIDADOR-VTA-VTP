using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_PERIODO
    /// </summary>
    public class PeriodoDTO
    {
        public System.Int32 PeriCodi { get; set; }
        public System.String PeriNombre { get; set; }
        public System.Int32 AnioCodi { get; set; }
        public System.Int32 MesCodi { get; set; }
        public System.String RecaNombre { get; set; }
        public System.DateTime PeriFechaValorizacion { get; set; }
        public System.DateTime PeriFechaLimite { get; set; }
        public System.String PeriHoraLimite { get; set; }
        public System.DateTime PeriFechaObservacion { get; set; }
        public System.String PeriEstado { get; set; }
        public System.String PeriUserName { get; set; }
        public System.DateTime PeriFecIns { get; set; }
        public System.DateTime PerifecAct { get; set; }
        public System.Int32 PeriAnioMes { get; set; }

        // Inicio de Agregados - Sistema de Compensaciones
        public String PeriDescripcion { get; set; }
        public decimal PeriTipoCambio { get; set; }
        public Int32 PeriVerCmg { get; set; }
        public DateTime Fechaini { get; set; }
        public DateTime Fechafin { get; set; }
        public String PecaNombre { get; set; }
        public int PecaVersionComp { get; set; }
        public int PecaVersionVTEA { get; set; }
        public String PecaEstadoRegistro { get; set; }
        public String RecaNombreComp { get; set; }
        public String PecaDscEstado { get; set; }
        public String PeriInforme { get; set; }
        public Int32 PeriFormNuevo { get; set; }
        // Fin de Agregados - Sistema de Compensaciones
    }
}
