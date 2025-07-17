using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_CORREO_ARCHIVO
    /// </summary>
    public class SiCorreoArchivoDTO : EntityBase
    {
        public int Earchcodi { get; set; } 
        public int? Corrcodi { get; set; } 
        public int? Earchtipo { get; set; } 
        public string Earchnombreoriginal { get; set; } 
        public string Earchnombrefisico { get; set; } 
        public int? Earchorden { get; set; } 
        public int? Earchestado { get; set; } 
    }
}
