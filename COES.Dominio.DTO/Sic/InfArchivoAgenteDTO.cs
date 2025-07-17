using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla INF_ARCHIVO_AGENTE
    /// </summary>
    public class InfArchivoAgenteDTO : EntityBase
    {
        public int Archicodi { get; set; } 
        public string Archinomb { get; set; } 
        public int Emprcodi { get; set; } 
        public string Archiruta { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime Lastdate { get; set; } 

        //Datos de otras tablas
        public string Emprnomb { get; set; } 
    }
}
