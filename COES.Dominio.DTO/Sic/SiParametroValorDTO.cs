using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_PARAMETRO_VALOR
    /// </summary>
    public partial class SiParametroValorDTO : EntityBase
    {
        public int Siparvcodi { get; set; } 
        public int? Siparcodi { get; set; } 
        public DateTime? Siparvfechainicial { get; set; } 
        public DateTime? Siparvfechafinal { get; set; } 
        public decimal? Siparvvalor { get; set; } 
        public string Siparvnota { get; set; } 
        public string Siparveliminado { get; set; } 
        public string Siparvusucreacion { get; set; } 
        public DateTime? Siparvfeccreacion { get; set; } 
        public string Siparvusumodificacion { get; set; } 
        public DateTime? Siparvfecmodificacion { get; set; } 
        public string Siparabrev { get; set; }
    }

    public partial class SiParametroValorDTO
    {
        //PR15 - Máxima demanda
        public int HMaxFinMinima { get; set; }
        public int HMaxFinMedia { get; set; }
        public int HMaxFinMaxima { get; set; }

        public int HInicio { get; set; }
        public int HFin { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }

        public int HRango1Ini { get; set; }
        public int HRango1Fin { get; set; }
        public int HRango2Ini { get; set; }
        public int HRango2Fin { get; set; }

        public int HFueraHoraPuntaFin { get; set; }
        public int HDespuesHoraPuntaFin { get; set; }

        //Indisponibilidades
        public int HIniHP { get; set; }
        public int HFinHP { get; set; }
        public int SegIniHP { get; set; }
        public int SegFinHP { get; set; }
        public int TotalHorasHP { get; set; }

        //Monitoreo MME
        public decimal? HHITendenciaUno { get; set; }
        public decimal? HHITendenciaCero { get; set; }
        public string HHITendenciaUnoColor { get; set; }
        public string HHITendenciaCeroColor { get; set; }

        public int IOPEsPivotal { get; set; }
        public int IOPNoPivotal { get; set; }
        public string IOPEsPivotalColor { get; set; }
        public string IOPNoPivotalColor { get; set; }
    }
}
