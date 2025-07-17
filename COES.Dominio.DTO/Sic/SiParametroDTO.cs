using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_PARAMETRO
    /// </summary>
    public class SiParametroDTO : EntityBase
    {
        public int Siparcodi { get; set; } 
        public string Siparabrev { get; set; } 
        public string Sipardescripcion { get; set; } 
        public string Siparusucreacion { get; set; } 
        public DateTime? Siparfeccreacion { get; set; } 
        public string Siparusumodificacion { get; set; } 
        public DateTime? Siparfecmodificacion { get; set; } 
    }

    public class ParametroRangoSolar
    {
        public DateTime Fecha { get; set; }
        public string FechaFormato { get; set; }
        public string FechaFormatoLetra { get; set; }
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
        public string ClaseFila { get; set; }
        public bool Editable { get; set; }
    }

    public class ParametroHPPotenciaActiva
    {
        public DateTime Fecha { get; set; }
        public string FechaFormato { get; set; }
        public string FechaFormatoLetra { get; set; }
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
        public bool EsVigente { get; set; }
        public string VigenteDesc { get; set; }
        public DateTime Siparvfechainicial { get; set; }
        public DateTime Siparvfechafinal { get; set; }
        public string Siparvusucreacion { get; set; }
        public string Siparvfeccreacion { get; set; }
        public string Siparvusumodificacion { get; set; }
        public string Siparvfecmodificacion { get; set; }
        public string ClaseFila { get; set; }
        public bool Editable { get; set; }
    }

    public class ParametroRangoPotenciaInductiva
    {
        public DateTime Fecha { get; set; }
        public string FechaFormato { get; set; }
        public string FechaFormatoLetra { get; set; }
        public int SiParvcodiH1Ini { get; set; }
        public int SiParvcodiH1Fin { get; set; }
        public int SiParvcodiH2Ini { get; set; }
        public int SiParvcodiH2Fin { get; set; }
        public string H1Ini { get; set; }
        public string H1Fin { get; set; }
        public string H2Ini { get; set; }
        public string H2Fin { get; set; }

        public int HRango1Ini { get; set; }
        public int HRango1Fin { get; set; }
        public int HRango2Ini { get; set; }
        public int HRango2Fin { get; set; }

        public string Estado { get; set; }
        public string EstadoValor { get; set; }
        public DateTime Siparvfechainicial { get; set; }
        public DateTime Siparvfechafinal { get; set; }
        public string Siparvusucreacion { get; set; }
        public string Siparvfeccreacion { get; set; }
        public string Siparvusumodificacion { get; set; }
        public string Siparvfecmodificacion { get; set; }
        public string ClaseFila { get; set; }
        public bool Editable { get; set; }
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
        public string Normativa { get; set; }
        public string ClaseFila { get; set; }
        public bool Editable { get; set; }
    }

    public class EstadoParametro
    {
        public string EstadoCodigo { get; set; }
        public string EstadoCodigo2 { get; set; }
        public string EstadoDescripcion { get; set; }
    }

    public class ParametroMagnitudRPF
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int SiParvcodi { get; set; }
        public string FechaFormatoInicio { get; set; }
        public string FechaFormatoFin { get; set; }
        public string Periodo { get; set; }
        public string PeriodoDesc { get; set; }
        public decimal Magnitud { get; set; }
        public string MagnitudTexto { get; set; }

        public string Estado { get; set; }
        public string EstadoValor { get; set; }
        public DateTime Siparvfechainicial { get; set; }
        public DateTime Siparvfechafinal { get; set; }
        public string Siparvusucreacion { get; set; }
        public string Siparvfeccreacion { get; set; }
        public string Siparvusumodificacion { get; set; }
        public string Siparvfecmodificacion { get; set; }
    }

    public class ParametroTendenciaHHI
    {
        public DateTime Fecha { get; set; }
        public string FechaFormato { get; set; }
        public int SiParvcodi { get; set; }
        public DateTime Siparvfechainicial { get; set; }
        public DateTime Siparvfechafinal { get; set; }

        public string Estado { get; set; }
        public string EstadoValor { get; set; }

        public int SiParvcodiTendenciaUno { get; set; }
        public int SiParvcodiTendenciaCero { get; set; }
        public decimal HHITendenciaUno { get; set; }
        public decimal HHITendenciaCero { get; set; }

        public string Siparvusucreacion { get; set; }
        public string Siparvfeccreacion { get; set; }
        public string Siparvusumodificacion { get; set; }
        public string Siparvfecmodificacion { get; set; }

        public string ClaseFila { get; set; }
        public bool Editable { get; set; }
    }
}
