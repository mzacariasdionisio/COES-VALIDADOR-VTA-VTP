using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_CAMBIO_TURNO_SUBSECCION
    /// </summary>
    public class SiCambioTurnoSubseccionDTO : EntityBase
    {
        public int Subseccioncodi { get; set; }
        public string Sumhorainicio { get; set; } 
        public string Sumreposicion { get; set; } 
        public string Sumconsideraciones { get; set; } 
        public string Regopecentral { get; set; } 
        public string Regcentralsubestacion { get; set; } 
        public string Regcentralhorafin { get; set; } 
        public string Reglineas { get; set; } 
        public string Reglineasubestacion { get; set; } 
        public string Reglineahorafin { get; set; } 
        public string Gesequipo { get; set; } 
        public string Gesaceptado { get; set; } 
        public string Gesdetalle { get; set; } 
        public string Eveequipo { get; set; } 
        public string Everesumen { get; set; }
        public string Evehorainicio { get; set; }
        public string Evereposicion { get; set; }
        public string Infequipo { get; set; } 
        public string Infplazo { get; set; } 
        public string Infestado { get; set; }         
        public int? Seccioncodi { get; set; } 
        public int Subseccionnumber { get; set; } 
        public string Despcentromarginal { get; set; } 
        public string Despursautomatica { get; set; } 
        public decimal? Despmagautomatica { get; set; } 
        public string Despursmanual { get; set; } 
        public decimal? Despmagmanual { get; set; } 
        public string Despcentralaislado { get; set; } 
        public decimal? Despmagaislado { get; set; }
        public string Desptiporeprog { get; set; }
        public string Desparchivoatr { get; set; }
        public string Despreprogramas { get; set; } 
        public string Desphorareprog { get; set; } 
        public string Despmotivorepro { get; set; } 
        public string Desppremisasreprog { get; set; } 
        public string Manequipo { get; set; } 
        public string Mantipo { get; set; } 
        public string Manhoraconex { get; set; } 
        public string Manconsideraciones { get; set; } 
        public string Sumsubestacion { get; set; } 
        public string Summotivocorte { get; set; }
        public int Subcausacodi { get; set; }
        public string Ursnomb { get; set; }
        public decimal Ursvalor { get; set; }



        public string Pafecha { get; set; }
        public string Pasorteo { get; set; }
        public string Paresultado { get; set; }
        public string Pagenerador { get; set; }
        public string Paprueba { get; set; }
    }
}
