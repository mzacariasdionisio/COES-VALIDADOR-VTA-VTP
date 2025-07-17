using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AF_HORA_COORD
    /// </summary>
    public class AfHoraCoordDTO : EntityBase
    {
        public DateTime? Afhofecmodificacion { get; set; }
        public string Afhousumodificacion { get; set; }
        public DateTime? Afhofeccreacion { get; set; }
        public string Afhousucreacion { get; set; }
        public DateTime? Afhofecha { get; set; }
        public int Emprcodi { get; set; }
        public int Afhocodi { get; set; }
        public int Afecodi { get; set; }
        public int Fdatcodi { get; set; }
        public string Afhmotivo { get; set; }
        //
        public string Codigoosinergmin { get; set; }
        public string Afhofechadescripcion { get; set; }
        public string AfEmprenomb { get; set; }
        public string Emprnombr { get; set; }
        public string Intsumsubestacion { get; set; }
        public int Intsumcodi { get; set; }
        public string EmpresaSuministradora { get; set; }
        public int Eracmfcodi { get; set; }
        public string Evencodi { get; set; }
        public string Evenini { get; set; }



    }
}
