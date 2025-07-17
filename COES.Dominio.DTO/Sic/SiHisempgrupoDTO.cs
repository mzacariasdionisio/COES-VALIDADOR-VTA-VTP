using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_HISEMPGRUPO
    /// </summary>
    public class SiHisempgrupoDTO : EntityBase
    {
        public int Grupocodiold { get; set; }
        public int Hempgrcodi { get; set; }
        public int Grupocodi { get; set; }
        public int Emprcodi { get; set; }
        public int Migracodi { get; set; }
        public DateTime Hempgrfecha { get; set; }
        public string Hempgrestado { get; set; }
        public int Hempgrdeleted { get; set; }

        public bool EstadoRecorrido { get; set; }
        public int Grupocodiactual { get; set; }
        public string Gruponomb { get; set; }
    }
}
