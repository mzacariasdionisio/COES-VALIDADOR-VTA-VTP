using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.ServicioRPF;

namespace COES.MVC.Intranet.Areas.ServicioRPFNuevo.Models
{
    public class AnalisisModel
    {
        public string FechaProceso { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }

        public List<ServicioRpfDTO> ListaNoIncluida { get; set; }
        public List<ServicioRpfDTO> ListaIncluida { get; set; }
        public List<RegistrorpfDTO> ListaDatoExcluido { get; set; }
        public List<ServicioRpfDTO> ListaPotencia { get; set; }
        public List<ServicioRpfDTO> ListaGrafico { get; set; }
        public List<ServicioRpfDTO> ListaReporte { get; set; }

        public List<ServicioRpfDTO> ListaNoCargaron { get; set; }
        public List<ServicioRpfDTO> ListaNoEncontrados { get; set; }
        public List<ServicioRpfDTO> ListaOK { get; set; }
        public List<ServicioRpfDTO> ListIncorrecto { get; set; }


        public string IndicadorEvaluacion { get; set; }
    }

    public class AnalisisFallaModel
    {
        public string FechaProceso { get; set; }
        public List<ServicioRpfDTO> ListaUnidades { get; set; }
        public List<ServicioRpfDTO> ListaPotencia { get; set; }
        public List<ServicioRpfDTO> ListaReporte { get; set; }

        public string PotenciaDesconectada { get; set; }
        public string ReservaPrimaria { get; set; }
        public string IndicadorVerificacion { get; set; }
        public string IndicadorExistenciaPotencia { get; set; }
        public string IndicadorExistenciaRPF { get; set; }
        public string IndicadorExistenciaDatos { get; set; }

        public bool ValidacionPotencia { get; set; }
        public bool ValidacionFrecuencia { get; set; }
        public bool ValidacionGeneral { get; set; }
        public List<decimal> ListaFrecuencias { get; set; }
    }

    public class ConfiguracionRpfModel
    {
        public List<ParametroRpfDTO> ListaParametro { get; set; }
        public ValoresRPF Valores { get; set; }
        public string FechaVigencia { get; set; }
        public List<ParametroDetRpfDTO> ListaHistorico { get; set; }
        public decimal? ValorActual { get; set; }

    }
}