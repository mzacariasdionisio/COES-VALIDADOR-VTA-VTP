using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_REPORTE_DET
    /// </summary>
    public partial class IndReporteDetDTO : EntityBase, ICloneable
    {
        public int Idetcodi { get; set; }
        public int Itotcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equipadre { get; set; }
        public int? Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public string Idetopcom { get; set; }
        public int Idetincremental { get; set; }
        public int? Idetdia { get; set; }
        public string Idettipoindisp { get; set; }
        public string Idettieneexc { get; set; }
        public DateTime? Idethoraini { get; set; }
        public DateTime? Idethorafin { get; set; }
        public int? Idetmin { get; set; }
        public decimal? Idetmw { get; set; }
        public decimal? Idetpr { get; set; }
        public decimal? Idetminparcial { get; set; }
        public decimal? Idetminifparcial { get; set; }
        public decimal? Idetminipparcial { get; set; }
        public int? Idettienedisp { get; set; }
        public decimal? Idetfactork { get; set; }
        public decimal? Idetpe { get; set; }
        public decimal? Idetpa { get; set; }
        public decimal? Idetminif { get; set; }
        public decimal? Idetminip { get; set; }
        public decimal? Idetnumho { get; set; }
        public int? Idetnumarranq { get; set; }
        public DateTime? Idetfechainifort7d { get; set; }
        public DateTime? Idetfechafinfort7d { get; set; }
        public DateTime? Idetfechainiprog7d { get; set; }
        public DateTime? Idetfechafinprog7d { get; set; }
        public string Idetdescadic { get; set; }
        public string Idetjustf { get; set; }
        public int? Idetcodiold { get; set; }
        public string Idettipocambio { get; set; }

        //Se agrega nuevo campo -Assetec (RAC)
        public int? Idetconsval { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class IndReporteDetDTO
    {
        public string Unidadnomb { get; set; }
        public string IdethorainiDesc { get; set; }
        public string IdethorafinDesc { get; set; }
        public string CeldaDescripcion { get; set; }
        public string CeldaFormato { get; set; }
        public int NumEje { get; set; }
        public string IdetpeDesc { get; set; }
        public string IdetpaDesc { get; set; }
        public decimal? PrPrevista { get; set; }
        public List<PrGrupoDTO> ListaMcc { get; set; } = new List<PrGrupoDTO>();

        public string Tab { get; set; }
        public string Tipocambio { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaDesc { get; set; }
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Indisponibilidad { get; set; }

        public decimal? NumHorasIp { get; set; }
        public decimal? NumHorasIf { get; set; }

        public string IdetnumhoDesc { get; set; }
        public string IdetnumarranqDesc { get; set; }
        public string NumHorasIpDesc { get; set; }
        public string NumHorasIfDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public int? Idettienedispold { get; set; }
        public string RangoHoras { get; set; }
        public string RangoHorasOld { get; set; }

        public List<int> ListaEquicodi { get; set; } = new List<int>();
        public string ListaEquicodiStr { get; set; } = "";
    }

}
