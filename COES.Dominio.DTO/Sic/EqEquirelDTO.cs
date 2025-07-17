using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_EQUIREL
    /// </summary>
    public class EqEquirelDTO : EntityBase
    {
        public int Equicodi1 { get; set; }
        public int Tiporelcodi { get; set; }
        public int Equicodi2 { get; set; }
        public int Equirelagrup { get; set; }
        public DateTime? Equirelfecmodificacion { get; set; }
        public string Equirelusumodificacion { get; set; }
        public int Equirelexcep { get; set; }
        public string Emprnomb { get; set; }
        public int Emprcodi { get; set; }
        public string Equinomb { get; set; }
        public string Empresatopologia { get; set; }
        public string Equipotopologia { get; set; }
        public string Famnomb { get; set; }

        public string EquirelfecmodificacionDesc { get; set; }
        public int OpCrud { get; set; }

        public int Famcodi1 { get; set; }
        public int Famcodi2 { get; set; }
        public string Famnomb1 { get; set; }
        public string Famnomb2 { get; set; }
        public string Equinomb1 { get; set; }
        public string Equinomb2 { get; set; }
        public string Emprnomb1 { get; set; }
        public string Emprnomb2 { get; set; }
    }

}
