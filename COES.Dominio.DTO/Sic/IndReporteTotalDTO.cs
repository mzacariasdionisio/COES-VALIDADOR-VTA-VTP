using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_REPORTE_TOTAL
    /// </summary>
    public partial class IndReporteTotalDTO : EntityBase, ICloneable
    {
        public int Itotcodi { get; set; }
        public int Irptcodi { get; set; }
        public int Famcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equipadre { get; set; }
        public int? Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public string Itotunidadnomb { get; set; }
        public string Itotopcom { get; set; }
        public int Itotincremental { get; set; }
        public decimal? Itotminip { get; set; }
        public decimal? Itotminif { get; set; }
        public decimal? Itotminipparcial { get; set; }
        public decimal? Itotminifparcial { get; set; }
        public decimal? Itotpe { get; set; }
        public decimal? Itotfactork { get; set; }
        public decimal? Itotfactorif { get; set; }
        public decimal? Itotfactoripm { get; set; }
        public decimal? Itotfactoripa { get; set; }
        public string Itotcr { get; set; }
        public string Itotindmas15d { get; set; }
        public int? Itotinddiasxmes { get; set; }
        public decimal? Itotfactorpresm { get; set; }
        public decimal? Itotnumho { get; set; }
        public int? Itotnumarranq { get; set; }
        public string Itotdescadic { get; set; }
        public string Itotjustf { get; set; }
        public int? Itotcodiold { get; set; }
        public string Itottipocambio { get; set; }

        //INICIO: IND.PR25.2022
        public decimal? Itotpcm3 { get; set; }
        public decimal? Itot1ltvalor { get; set; }
        public string Itot1ltunidad { get; set; }
        public decimal? Itotfgte { get; set; }
        public decimal? Itotfrc { get; set; }

        public int? Itotconsval { get; set; }
        //FIN: IND.PR25.2022

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class IndReporteTotalDTO
    {
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Gruponomb { get; set; }
        public bool EsUnaUnidadXCentral { get; set; }
        public string Grupotipocogen { get; set; }
        public int Fenergcodi { get; set; }

        public decimal? ItotpeC2 { get; set; }

        public string Itotindmas15dDesc { get; set; }

        public string Itotdescadicold { get; set; }
        public decimal? Itotfactorkold { get; set; }
        public decimal? Itotpaold { get; set; }
        public string Tipocambio { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public string ItotminipDesc { get; set; }
        public string ItotminifDesc { get; set; }
        public string ItotminipparcialDesc { get; set; }
        public string ItotminifparcialDesc { get; set; }
        public string ItotpeDesc { get; set; }
        public string ItotpaDesc { get; set; }
        public string ItotfactorkDesc { get; set; }
        public bool PintarRojoFactorK { get; set; }
        public List<string> ListaDesIndispMayorA1Dia { get; set; }

        public bool TieneCentralConjunto { get; set; }

        public DateTime? Irecafechaini { get; set; }
        public DateTime? Irecafechafin { get; set; }
        public int Periodo { get; set; }
        public decimal? NumHorasIp { get; set; }
        public decimal? NumHorasIf { get; set; }

        public string Crdesc { get; set; }
        public decimal? FactorTeoricoProg { get; set; }
        public decimal? FactorTeoricoFort { get; set; }
        public string TipoCentral { get; set; }
        public string TipoCombustible { get; set; }

        public DateTime? Equifechiniopcom { get; set; }
        public DateTime? Equifechfinopcom { get; set; }
        public bool TieneIngresoOpCom { get; set; }

        public bool TieneFIF { get; set; }
        public bool TieneFIPM { get; set; }
        public bool TieneFIPA { get; set; }

        //INICIO: IND.PR25.2022
        public List<IndReporteFCDTO> ListIndReporteFC { get; set; }
        public List<IndReporteInsumosDTO> ListIndReporteInsumos { get; set; }
        public List<IndReporteCalculosDTO> ListIndReporteCalculos { get; set; }
        //FIN: IND.PR25.2022
    }
}
