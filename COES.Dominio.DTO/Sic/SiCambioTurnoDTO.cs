using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_CAMBIO_TURNO
    /// </summary>
    public class SiCambioTurnoDTO : EntityBase
    {
        public int? Coordinadorresp { get; set; } 
        public int? Turno { get; set; } 
        public DateTime? Fecturno { get; set; } 
        public string Coordinadorrecibe { get; set; }
        public string Especialistarecibe { get; set; }
        public string Analistarecibe { get; set; } 
        public int? Cambioturnocodi { get; set; } 
        public string Emsoperativo { get; set; } 
        public string Emsobservaciones { get; set; } 
        public string Horaentregaturno { get; set; }
        public string CasoSinReserva { get; set; }
        public List<SiCambioTurnoSeccionDTO> ListaSeccion { get; set; }
        public int Percodi { get; set; }
        public string Pernomb { get; set; }
    }
}
