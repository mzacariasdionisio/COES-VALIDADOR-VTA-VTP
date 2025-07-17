using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_ESTADOENVIO
    /// </summary>
    public class MeEstadoenvioDTO : EntityBase
    {
        public int Estenvcodi { get; set; } 
        public string Estenvnombre { get; set; } 
        public string Estenvabrev { get; set; } 
    }
}
