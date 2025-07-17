using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RI_REVISION
    /// </summary>
    public class RiRevisionDTO : EntityBase
    {
        public int Revicodi { get; set; }
        
        public int Reviiteracion { get; set; }
        public string Reviestado { get; set; }
        public string Reviestadoregistro { get; set; }        
        
        public int? Emprcodi { get; set; }
        public int? Etrvcodi { get; set; }

        public string Reviusucreacion { get; set; }
        public DateTime? Revifeccreacion { get; set; }

        public string Reviusumodificacion { get; set; }
        public DateTime? Revifecmodificacion { get; set; }

        public int? Reviusurevision { get; set; }
        public DateTime? Revifecrevision { get; set; }

        public string Revinotificado { get; set; }
        public DateTime? Revifecnotificado { get; set; }

        public string Revifinalizado { get; set; }
        public DateTime? Revifecfinalizado { get; set; }

        public string Reviterminado { get; set; }
        public DateTime? Revifecterminado { get; set; }

        public string Revienviado { get; set; }
        public DateTime? Revifecenviado { get; set; }

        //Campos listado 
        public string Tipoemprdesc { get; set; }
        public string Emprrazsocial { get; set; }
        public string Emprnombrecomercial { get; set; }
        public string Emprsigla { get; set; }
        //public int Horas { get; set; }
        //public string Estsgi { get; set; }
        //public string Estdjr { get; set; }

        public List<RiDetalleRevisionDTO> Detalle { get; set; }
    }
}
