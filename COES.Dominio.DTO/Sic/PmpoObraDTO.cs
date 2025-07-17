using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMPO_OBRA
    /// </summary>
    public class PmpoObraDTO : EntityBase
    {
        public int Obracodi { get; set; }
        public int ObraDetcodi { get; set; }
        public int TObracodi { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public DateTime? Obrafechaplanificada { get; set; }
        public string  Obradescripcion { get; set; }
        public string Obrausucreacion { get; set; }
        public DateTime Obrafeccreacion { get; set; }
        public string Obrausumodificacion { get; set; }
        public DateTime Obrafecmodificacion { get; set; }
        public string TObradescripcion { get; set; }

        public int ObraFlagFormat { get; set; }

        public string GrupoNomb { get; set; }
        public string BarraNomb { get; set; }
        public string EquiNomb { get; set; }


        //Detalle
        public List<PmpoObraDetalleDTO> ObraDet { get; set; }

    }
}
