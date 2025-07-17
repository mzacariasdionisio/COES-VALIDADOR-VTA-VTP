using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.ConsumoCombustible
{
    public class ConstantesConsumoCombustible
    {
        public const string EstadoGenerado = "G";
        public const string EstadoValidado = "V";
        public const int PlantcodiTransgresion = 106;

        public const string HorizonteDiario = "D";
        public const string HorizonteMensual = "M";

        public static readonly List<int> ListaCalificacionAlertaHO = new List<int>() { ConstantesSubcausaEvento.SubcausaPorPruebas, ConstantesSubcausaEvento.SubcausaPorSeguridad, ConstantesSubcausaEvento.SubcausaPorTension };

        public const string ColorAlertaHO = "#FFFF00";
        public const string ColorFaltaValorDatosCOES = "#FCE4D6";
        public const string ColorFaltaValorConsumo = "#FFA500";
        public const string ColorTieneTransgresion = "#F07896";

        public const string ColorTextoPorRsf = "#0000FF";
        public const string ColorTextoMinimaCarga = "#C65911";

        public const string SeparadorObs = "##";

        public const int FlagCombustibleArranque = 2;

        //Cambios Pendientes
        public const int TipoCambioNuevo = 1;
        public const int TipoCambioModificar = 2;
        public const int TipoCambioEliminar = 3;

        public const string FolderRaizVCOM = "Intranet/Aplicativo_VCOM/";

        //Rendimiento Bagazo
        public const int PropiedadRendBagazo = 2227;
    }

    public class ReporteVCOM
    {
        public List<CccVersionDTO> ListaVersionDiaria { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public List<CccReporteDTO> ListaUnidad { get; set; }
        public List<CccReporteDTO> ListaDetalleDiario { get; set; }
        public List<ResultadoValidacionAplicativo> ListaMsj { get; set; }
    }

    public class ReporteConsumoCombustible
    {
        public List<EqEquipoDTO> ListaUnidad { get; set; }
        public List<EqEquipoDTO> ListaEquiposTermicos { get; set; }
        public List<MeMedicion48DTO> Lista48xGen { get; set; }
        public List<ConsumoHorarioCombustible> ListaCurva { get; set; }
        public List<ResultadoValidacionAplicativo> ListaMsj { get; set; }
        public List<DetalleHoraOperacionModo> ListaDetalleHO { get; set; }
        public List<CeldaMWxModo> ListaCeldaDetalle { get; set; }
        public List<CeldaMWxModo> ListaCeldaDetalle2 { get; set; }
    }

    public class CeldaMWxModo
    {
        public DateTime Fecha { get; set; }
        public EqEquipoDTO UnidadTermico { get; set; }
        public int Equipadre { get; set; }
        public int Grupocodimodo { get; set; }
        public ConsumoHorarioCombustible Curva { get; set; }
        public PrGrupoDTO Modo { get; set; }
        public DetalleHoraOperacionModo DetalleHO { get; set; }
        public GraficoWeb Grafico { get; set; }
        public bool TieneAlertaHo { get; set; }

        public int[] ListaMinuto { get; set; }
        public decimal?[] ListaMW { get; set; }
        public decimal?[] ListaConsumo { get; set; }
        public decimal?[] ListaMWHo { get; set; }
        public string[] ListaMensaje { get; set; }
        public int[] ListaNumGen { get; set; }
        public int[] ListaCalifHo { get; set; }
        public string[] ListaCalifHoDesc { get; set; }
        public string[] ListaSubcausadesc { get; set; }

        public CeldaMWxModo()
        {
            ListaMinuto= new int[48];
            ListaMW = new decimal?[48];
            ListaConsumo = new decimal?[48];
            ListaMWHo = new decimal?[48];
            ListaMensaje = new string[48];
            ListaNumGen = new int[48];
            ListaCalifHo = new int[48];
            ListaCalifHoDesc = new string[48];
            ListaSubcausadesc = new string[48];
            Modo = new PrGrupoDTO();
            Curva = new ConsumoHorarioCombustible();
            DetalleHO = new DetalleHoraOperacionModo();
            Grafico = new GraficoWeb();
        }
    }

    public class DetalleHoraOperacionModo
    {
        public int Equicodi { get; set; }
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public List<EveHoraoperacionDTO> ListaHO { get; set; }
        public List<string> ListaHODesc { get; set; }
        public string Comentario { get; set; }

        public DetalleHoraOperacionModo()
        {
            ListaHO = new List<EveHoraoperacionDTO>();
            ListaHODesc = new List<string>();
        }
    }

    public class LeyendaCCC
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public string Color { get; set; }
        public string Descripcion { get; set; }
        public bool TieneTransgresion { get; set; }
    }
}
