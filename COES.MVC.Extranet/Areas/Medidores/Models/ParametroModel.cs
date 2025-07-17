using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Medidores.Models
{
    public class ParametroModel
    {
        public ParametroRangoSolar RangoSolar { get; set; }
        public List<ParametroRangoSolar> ListaRangoSolar { get; set; }

        public ParametroHPPotenciaActiva HPPotenciaActiva { get; set; }
        public List<ParametroHPPotenciaActiva> ListaHPPotenciaActiva { get; set; }

        public ParametroRangoPotenciaInductiva RangoPotenciaInductiva { get; set; }
        public List<ParametroRangoPotenciaInductiva> ListaRangoPotenciaInductiva { get; set; }

        public ParametroRangoPeriodoHP RangoPeriodoHP { get; set; }
        public List<ParametroRangoPeriodoHP> ListaRangoPeriodoHP { get; set; }
    }

    public class ParametroRangoSolar
    {
        public DateTime Fecha { get; set; }
        public string FechaFormato { get; set; }
        public int SiParvcodiHoraInicio { get; set; }
        public int SiParvcodiHoraFin { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public int HInicio { get; set; }
        public int HFin { get; set; }
        public string Estado { get; set; }
        public string EstadoValor { get; set; }
        public DateTime Siparvfechainicial { get; set; }
        public DateTime Siparvfechafinal { get; set; }
        public string Siparvusucreacion { get; set; }
        public string Siparvfeccreacion { get; set; }
        public string Siparvusumodificacion { get; set; }
        public string Siparvfecmodificacion { get; set; }
    }

    public class ParametroHPPotenciaActiva
    {
        public DateTime Fecha { get; set; }
        public string FechaFormato { get; set; }
        public int SiParvcodiHoraMinima { get; set; }
        public int SiParvcodiHoraMedia { get; set; }
        public int SiParvcodiHoraMaxima { get; set; }
        public string HoraMinima { get; set; }
        public string HoraMedia { get; set; }
        public string HoraMaxima { get; set; }
        public int HMaxFinMinima { get; set; }
        public int HMaxFinMedia { get; set; }
        public int HMaxFinMaxima { get; set; }
        public string Estado { get; set; }
        public string EstadoValor { get; set; }
        public string Siparvusucreacion { get; set; }
        public string Siparvfeccreacion { get; set; }
        public string Siparvusumodificacion { get; set; }
        public string Siparvfecmodificacion { get; set; }
    }
    public class ParametroRangoPotenciaInductiva
    {
        public DateTime Fecha { get; set; }
        public string FechaFormato { get; set; }
        public int SiParvcodiH1Ini { get; set; }
        public int SiParvcodiH1Fin { get; set; }
        public int SiParvcodiH2Ini { get; set; }
        public int SiParvcodiH2Fin { get; set; }
        public string H1Ini { get; set; }
        public string H1Fin { get; set; }
        public string H2Ini { get; set; }
        public string H2Fin { get; set; }

        public string Estado { get; set; }
        public string EstadoValor { get; set; }
        public string Siparvusucreacion { get; set; }
        public string Siparvfeccreacion { get; set; }
        public string Siparvusumodificacion { get; set; }
        public string Siparvfecmodificacion { get; set; }
    }

    public class ParametroRangoPeriodoHP
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int SiParvcodi { get; set; }
        public string FechaFormatoInicio { get; set; }
        public string FechaFormatoFin { get; set; }
        public string Estado { get; set; }
        public string EstadoValor { get; set; }
        public string Siparvusucreacion { get; set; }
        public string Siparvfeccreacion { get; set; }
        public string Siparvusumodificacion { get; set; }
        public string Siparvfecmodificacion { get; set; }
    }
}