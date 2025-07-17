using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Servicios.Aplicacion.DPODemanda.Helper
{
    class ComunDpo
    {
    }

    public class DpoMedicion1440
    {
        public int Ptomedicodi { get; set; }
        public DateTime Medifecha { get; set; }
        public int Tipoinfo { get; set; }
        public decimal[] Medicion { get; set; }
        public decimal?[] MedicionNull { get; set; }
        public int Ieod { get; set; }
    }

    public class DpoMedDiaTipicoModel
    {
        public List<DpoMedicion1440> datosLunes { get; set; }
        public List<DpoMedicion1440> datosSabado { get; set; }
        public List<DpoMedicion1440> datosDomingo { get; set; }
        public List<DpoMedicion1440> datosOtros { get; set; }
    }

    public class DpoFiltro
    {
        public string filtroCodigo { get; set; }
        public string filtroNombre { get; set; }
    }

    public class DpoGrillaDatos96
    {
        public string Fecha { get; set; }
        public string Punto { get; set; }
        public string NombrePunto { get; set; }
        public string NombreTransformador { get; set; }
        public string NombreBarra { get; set; }
        public decimal? Total { get; set; }
        public decimal? H1 { get; set; }
        public decimal? H2 { get; set; }
        public decimal? H3 { get; set; }
        public decimal? H4 { get; set; }
        public decimal? H5 { get; set; }
        public decimal? H6 { get; set; }
        public decimal? H7 { get; set; }
        public decimal? H8 { get; set; }
        public decimal? H9 { get; set; }
        public decimal? H10 { get; set; }
        public decimal? H11 { get; set; }
        public decimal? H12 { get; set; }
        public decimal? H13 { get; set; }
        public decimal? H14 { get; set; }
        public decimal? H15 { get; set; }
        public decimal? H16 { get; set; }
        public decimal? H17 { get; set; }
        public decimal? H18 { get; set; }
        public decimal? H19 { get; set; }
        public decimal? H20 { get; set; }
        public decimal? H21 { get; set; }
        public decimal? H22 { get; set; }
        public decimal? H23 { get; set; }
        public decimal? H24 { get; set; }
        public decimal? H25 { get; set; }
        public decimal? H26 { get; set; }
        public decimal? H27 { get; set; }
        public decimal? H28 { get; set; }
        public decimal? H29 { get; set; }
        public decimal? H30 { get; set; }
        public decimal? H31 { get; set; }
        public decimal? H32 { get; set; }
        public decimal? H33 { get; set; }
        public decimal? H34 { get; set; }
        public decimal? H35 { get; set; }
        public decimal? H36 { get; set; }
        public decimal? H37 { get; set; }
        public decimal? H38 { get; set; }
        public decimal? H39 { get; set; }
        public decimal? H40 { get; set; }
        public decimal? H41 { get; set; }
        public decimal? H42 { get; set; }
        public decimal? H43 { get; set; }
        public decimal? H44 { get; set; }
        public decimal? H45 { get; set; }
        public decimal? H46 { get; set; }
        public decimal? H47 { get; set; }
        public decimal? H48 { get; set; }
        public decimal? H49 { get; set; }
        public decimal? H50 { get; set; }
        public decimal? H51 { get; set; }
        public decimal? H52 { get; set; }
        public decimal? H53 { get; set; }
        public decimal? H54 { get; set; }
        public decimal? H55 { get; set; }
        public decimal? H56 { get; set; }
        public decimal? H57 { get; set; }
        public decimal? H58 { get; set; }
        public decimal? H59 { get; set; }
        public decimal? H60 { get; set; }
        public decimal? H61 { get; set; }
        public decimal? H62 { get; set; }
        public decimal? H63 { get; set; }
        public decimal? H64 { get; set; }
        public decimal? H65 { get; set; }
        public decimal? H66 { get; set; }
        public decimal? H67 { get; set; }
        public decimal? H68 { get; set; }
        public decimal? H69 { get; set; }
        public decimal? H70 { get; set; }
        public decimal? H71 { get; set; }
        public decimal? H72 { get; set; }
        public decimal? H73 { get; set; }
        public decimal? H74 { get; set; }
        public decimal? H75 { get; set; }
        public decimal? H76 { get; set; }
        public decimal? H77 { get; set; }
        public decimal? H78 { get; set; }
        public decimal? H79 { get; set; }
        public decimal? H80 { get; set; }
        public decimal? H81 { get; set; }
        public decimal? H82 { get; set; }
        public decimal? H83 { get; set; }
        public decimal? H84 { get; set; }
        public decimal? H85 { get; set; }
        public decimal? H86 { get; set; }
        public decimal? H87 { get; set; }
        public decimal? H88 { get; set; }
        public decimal? H89 { get; set; }
        public decimal? H90 { get; set; }
        public decimal? H91 { get; set; }
        public decimal? H92 { get; set; }
        public decimal? H93 { get; set; }
        public decimal? H94 { get; set; }
        public decimal? H95 { get; set; }
        public decimal? H96 { get; set; }
    }

    public class DpoConsultaPrefiltrado
    {
        public int id { get; set; }
        public int dias { get; set; }
        public List<string> diasHora { get; set; }
        public string name { get; set; }
        public decimal[] data { get; set; }
        public List<int> diasMostrar { get; set; }
    }

    public class DpoBarraDiferencia
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public decimal diferencia { get; set; }
    }

    public class DpoActualizacionUL
    {
        public int id { get; set; }
        public List<int> intervalos { get; set; }
    }

    public class DpoMedCompFormula
    {
        public int idRelacion { get; set; }
        public int idCompVegetativo { get; set; }
        public int idRelBarra { get; set; }
        public int idRelFormula { get; set; }
        public string idFuente { get; set; }
        public int idCompFormula { get; set; }
        public decimal constante { get; set; }
    }

    public class DpoAreademanda
    {
        public int? Splfrmarea { get; set; }
        public string Splfrmareanombre { get; set; }
    }

    public class DpoFormulaCorregidaFormmato
    {
        public string Serie { get; set; }
        public List<decimal> Valores { get; set; }
        public List<string> Fechas { get; set; }
    }

    public class DpoModProcesoVeg
    {
        public List<string> ItvPromF2F3PerfMin { get; set; }
        public List<DpoMedicionTotal> PromHistoricoFiltroF2 { get; set; }
        public List<DpoMedicionTotal> PromHistoricoFiltroF3 { get; set; }
        public List<DpoMedicionTotal> PromHistoricoPerfilesMin { get; set; }
        public List<DpoMedicionTotal> PromHistoricoFiltroF4 { get; set; }
        public List<DpoMedicionTotal> PromHistoricoFiltroF5 { get; set; }

        public List<DpoMedicionTotal> HoyPerfilesMin { get; set; }
        public List<DpoMedicionTotal> HoyFiltroF2 { get; set; }
        public List<DpoMedicionTotal> HoyFiltroF6 { get; set; }
        public List<DpoMedicionTotal> HoyFiltroF5 { get; set; }
        public List<DpoMedicionTotal> HoyPronosticoMinVegSCO { get; set; }
        public List<DpoMedicionTotal> HoyPronosticoMinVegSPR { get; set; }
        public List<DpoMedicion96DTO> HoyPronostico30MinVegSCO { get; set; }
        public List<DpoMedicion96DTO> HoyPronostico30MinVegSPR { get; set; }
    }

    public class DpoModRelacionTna
    {
        public List<DpoMedCompFormula> ForInputCDispatch { get; set; }
        public List<DpoMedCompFormula> ForInputEstimadorTna { get; set; }
        public List<DpoMedCompFormula> ForVegCDispatch { get; set; }
        public List<DpoMedCompFormula> ForVegEstimadorTna { get; set; }
        public List<PrnRelacionTnaDTO> Relaciones { get; set; }
    }

    public class DpoMedicionTotal
    {
        public int Ptomedicodi { get; set; }
        public string Ptomedidesc { get; set; }
        public DateTime Medifecha { get; set; }
        public List<decimal> Medicion { get; set; }
        public List<decimal?> MedicionNull { get; set; }
    }

    public class DemVegCol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaHora { get; set; }
        public List<decimal?> Valores { get; set; }
        public int SubId { get; set; }
        public int TipoInfo { get; set; }
        public decimal? PromRango { get; set; }
    }

    public class DemVegFrm
    {
        public int Id { get; set; }
        public decimal Factor { get; set; }
        public char Tipo { get; set; } 
    }
}
