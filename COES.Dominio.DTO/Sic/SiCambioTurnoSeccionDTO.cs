using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_CAMBIO_TURNO_SECCION
    /// </summary>
    public class SiCambioTurnoSeccionDTO : EntityBase
    {
        public int Cambioturnocodi { get; set; } 
        public int? Nroseccion { get; set; } 
        public string Descomentario { get; set; } 
        public int Seccioncodi { get; set; }
        public List<SiCambioTurnoSubseccionDTO> ListItems { get; set; }
    }
}
