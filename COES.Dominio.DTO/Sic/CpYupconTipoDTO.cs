using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_YUPCON_TIPO
    /// </summary>
    public class CpYupconTipoDTO : EntityBase
    {
        public int Tyupcodi { get; set; }
        public string Tyupnombre { get; set; }
    }
}
