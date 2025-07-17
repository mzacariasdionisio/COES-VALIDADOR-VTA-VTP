using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_ESTUDIO_ESTADO
    /// </summary>
    public class EpoEstudioEstadoDTO : EntityBase
    {
        public int Estacodi { get; set; } 
        public string Estadescripcion { get; set; } 
    }
}
