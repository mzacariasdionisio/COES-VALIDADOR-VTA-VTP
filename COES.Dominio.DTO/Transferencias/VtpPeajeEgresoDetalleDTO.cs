using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{

    /// <summary>
    /// Clase que mapea la tabla VTP_PEAJE_EGRESO_DETALLE
    /// </summary>
    public class VtpPeajeEgresoDetalleDTO : EntityBase
    {
        public int Pegrdcodi { get; set; }
        public int Pegrcodi { get; set; }
        public int? Emprcodi { get; set; }   
        public int? Barrcodi { get; set; }
        public string Pegrdtipousuario { get; set; }
        public string Pegrdlicitacion { get; set; }
        public decimal? Pegrdpotecalculada { get; set; }
        public decimal? Pegrdpotedeclarada { get; set; }
        public string Pegrdcalidad { get; set; }
        public decimal? Pegrdpreciopote { get; set; }
        public decimal? Pegrdpoteegreso { get; set; }
        public decimal? Pegrdpeajeunitario { get; set; }
        public int? Barrcodifco { get; set; }
        public decimal? Pegrdpoteactiva { get; set; }
        public decimal? Pegrdpotereactiva { get; set; }
        public string Pegrdusucreacion { get; set; }
        public DateTime Pegrdfeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb { get; set; }
        public string Barrnombre { get; set; }
        public string Barrnombrefco { get; set; }
        public string Coregecodvtp { get; set; }
        public int? Coregecodi { get; set; }
        public decimal? Pegrdpotecoincidente { get; set; }
        public decimal? Pegrdfacperdida { get; set; }
        public string Codcncodivtp { get; set; }
        public int TipConCondi { get; set; }
        public string TipConNombre { get; set; }
    }
}
