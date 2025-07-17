using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Mediciones.Models
{
    public class IndexCalidadProductoModel
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Intervalo { get; set; }

        public List<EquipoGps> Equipos { get; set; }
        public Int32 GpsCodi { get; set; }
    }

    public class EquipoGps
    {
        public string GpsNombre { get; set; }
        public Int32 GpsCodi { get; set; }
    }

    public class CuadroResumenModel
    {
        public List<Variacion> lsCuadroFrecFrecuenciaInstantaneas { get; set; }
        public List<DatosIntervalo> lsCuadroIntervalos { get; set; }
    }

    public class Variacion
    {
        public string Indicador { get; set; }
        public string Fecha { get; set; }
        public string Gps { get; set; }
        public string Rango { get; set; }
        public string FrecMinValor { get; set; }
        public string FrecMinHora { get; set; }
        public string FrecMaxValor { get; set; }
        public string FrecMaxHora { get; set; }
        public string TransgHora { get; set; }
        public string TransgValor { get; set; }
        public int TransgAcum { get; set; }
        
    }
    public struct sVarSostenida
    {
        public DateTime Fechahora;
        public double Sumnum;
        public double Sumdesv;
        public double Valor;
    }
    public struct sFrecuencia
    {
        public DateTime Fechahora;
        public double Frecuencia;
    }

    public class DatosIntervalo
    {
        public string Fecha { get; set; }
        public string Gps { get; set; }
        public string NombreFIla { get; set; }
        public string Intervalo1 { get; set; }
        public string Intervalo2 { get; set; }
        public string Intervalo3 { get; set; }
        public string Intervalo4 { get; set; }
        public string Intervalo5 { get; set; }
        public string Intervalo6 { get; set; }
        public string Intervalo7 { get; set; }
        public string Intervalo8 { get; set; }
        public string Intervalo9 { get; set; }
    }
}