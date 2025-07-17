using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_PEAJE_EGRESO_MINFO
    /// </summary>
    public class VtpPeajeEgresoMinfoDTO : EntityBase
    {
        public int Pegrmicodi { get; set; }
        public int Pegrcodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int? Genemprcodi { get; set; }
        public int Pegrdcodi { get; set; }
        public int? Cliemprcodi { get; set; }       
        public int? Barrcodi { get; set; }
        public string Pegrmitipousuario { get; set; }
        public string Pegrmilicitacion { get; set; }
        public decimal? Pegrmipotecalculada { get; set; }
        public decimal? Pegrmipotedeclarada { get; set; }
        public string Pegrmicalidad { get; set; }
        public decimal? Pegrmipreciopote { get; set; }
        public decimal? Pegrmipoteegreso { get; set; }
        public decimal? Pegrmipeajeunitario { get; set; }
        public int? Barrcodifco { get; set; }
        public decimal? Pegrmipoteactiva { get; set; }
        public decimal? Pegrmipotereactiva { get; set; }
        public string Pegrmiusucreacion { get; set; }
        public DateTime Pegrmifeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Genemprnombre { get; set; }
        public string Cliemprnombre { get; set; }
        public string Barrnombre { get; set; }
        public string Barrnombrefco { get; set; }

        //MAPEAR NUEVOS CAMPOS 25/01/2021
        public decimal? Pegrmipotecoincidente { get; set; }
        public decimal? Pegrmifacperdida { get; set; }
        public string Coregecodvtp { get; set; }
        public int? Coregecodi { get; set; }

        // SE MANTIENEN POR EL UNION DE LA VISTA
        public decimal? Pegrdpotecoincidente { get; set; }
        public decimal? Pegrdfacperdida { get; set; }

        //código de retiro generado para registrar en vtp_peaje_egreso_detalle
        public string Codcncodivtp { get; set; }
        public int? Tipconcodi { get; set; }
        public string Tipconnombre { get; set; }
    }
}
