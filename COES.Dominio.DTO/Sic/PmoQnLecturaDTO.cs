using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_QN_LECTURA
    /// </summary>
    public class PmoQnLecturaDTO : EntityBase
    {
        public int Qnlectcodi { get; set; }
        public string Qnlectnomb { get; set; }
    }
}
