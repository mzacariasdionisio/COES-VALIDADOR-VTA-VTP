using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_EQUIPOMIGRAR
    /// </summary>
    public class SiEquipomigrarDTO : EntityBase
    {
        public int Equmigcodi { get; set; } 
        public int Migempcodi { get; set; } 
        public int? Equicodimigra { get; set; } 
        public int? Equicodibajanuevo { get; set; }
        public string Equmigusucreacion { get; set; }
        public DateTime? Equmigfeccreacion { get; set; }
        public string Equmigusumodificacion { get; set; }
        public DateTime? Equmigfecmodificacion { get; set; }

        public EqEquipoDTO objeto_equipo { get; set; }
        public List<EqEquipoDTO> lista_equipos { get; set; }
    }
}
