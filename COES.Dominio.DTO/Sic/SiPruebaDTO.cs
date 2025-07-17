using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_PRUEBA
    /// </summary>
    public class SiPruebaDTO : EntityBase
    {
        public string Pruebacodi { get; set; } 
        public string Pruebanomb { get; set; } 
        public string Pruebaest { get; set; } 
    }
}
