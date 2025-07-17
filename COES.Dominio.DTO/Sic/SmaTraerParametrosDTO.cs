using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_CONFIGURACION
    /// </summary>
    public class SmaTraerParametrosDTO : EntityBase
    {
        public bool TParamOfertaDefecto { get; set; }
        public decimal TParamPrecioMaximo { get; set; }
        public decimal TParamPrecioMinimo { get; set; }
        public decimal TParamPrecioDefecto { get; set; }

        public decimal TParamPotenciaMinimaMan { get; set; }
    }
}
