using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_RECALCULO
    /// </summary>
    public class RecalculoDTO
    {
        public System.Int32 RecaCodi { get; set; }
        public System.Int32 RecaPeriCodi { get; set; }
        public System.DateTime RecaFechaValorizacion { get; set; }
        public System.DateTime RecaFechaLimite { get; set; }
        public System.String RecaHoraLimite { get; set; }
        public System.DateTime RecaFechaObservacion { get; set; }
        public System.String RecaEstado { get; set; }
        public System.String RecaNombre { get; set; }
        public System.String RecaNroInforme { get; set; }
        public System.String RecaDescripcion { get; set; }
        public System.String RecaMasInfo { get; set; }
        public System.String RecaUserName { get; set; }
        public System.DateTime RecaFecIns { get; set; }
        public System.DateTime RecaFecAct { get; set; }
        public System.Int32 PeriCodiDestino { get; set; }
        public System.String PeriNombre { get; set; }
        public System.String RecaCuadro1 { get; set; }
        public System.String RecaCuadro2 { get; set; }
        public System.String RecaNota2 { get; set; }
        public System.String RecaCuadro3 { get; set; }
        public System.String RecaCuadro4 { get; set; }
        public System.String RecaCuadro5 { get; set; }

        public System.String PeriNombreDestino { get; set; }        // PrimasRER.2023
    }
}
