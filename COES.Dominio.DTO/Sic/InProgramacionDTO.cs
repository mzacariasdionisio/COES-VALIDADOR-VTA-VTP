using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_PROGRAMACION
    /// </summary>
    public partial class InProgramacionDTO : EntityBase
    {
        public int Progrcodi { get; set; }
        public int Evenclasecodi { get; set; }

        public string Progrnomb { get; set; }
        public string Prograbrev { get; set; }

        public DateTime Progrfechaini { get; set; }
        public DateTime Progrfechafin { get; set; }

        public int Progrversion { get; set; }
        public int Progrsololectura { get; set; }
        public DateTime Progrfechalim { get; set; }

        public string Progrusucreacion { get; set; }
        public DateTime? Progrfeccreacion { get; set; }
        public string Progrusuaprob { get; set; }
        public DateTime? Progrfecaprob { get; set; }

        public int? Progresaprobadorev { get; set; }
        public DateTime? Progrmaxfecreversion { get; set; }
        public string Progrusuhabrev { get; set; }
        public DateTime? Progrfechabrev { get; set; }
        public string Progrusuaprobrev { get; set; }
        public DateTime? Progrfecaprobrev { get; set; }
    }

    public partial class InProgramacionDTO
    {
        public DateTime FechaInicial00 { get; set; }
        public DateTime FechaFinal24 { get; set; }
        public string ProgrfechainiDesc { get; set; }
        public string ProgrfechafinDesc { get; set; }

        public int TotalRegistro { get; set; }
        public int TotalRevertidos { get; set; }
        public string Evenclasedesc { get; set; }

        public bool EsPlanAprobado { get; set; }
        public bool EsCerradoIntranet { get; set; }
        public bool EsCerradoExtranet { get; set; }
        public int EstadoIntranet { get; set; }
        public string EstadoIntranetDesc { get; set; }
        public int EstadoExtranet { get; set; }
        public string EstadoExtranetDesc { get; set; }
        public bool PermiteReversion { get; set; }
        public bool EsPlanRevertido { get; set; }
        public bool TieneRegistrosxReversion { get; set; }

        public string ProgrnombYPlazo { get; set; }
        public string Nomprogramacion { get; set; }
        public string ProgrnombYPlazoCruzado { get; set; }

        public string CarpetaProgDefault { get; set; }

        public List<InParametroPlazoDTO> Ampliaciones { get; set; }

    }
}
