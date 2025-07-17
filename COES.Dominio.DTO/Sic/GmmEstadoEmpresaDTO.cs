using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla GMM_ESTADOEMPRESA
    /// </summary>
    public class GmmEstadoEmpresaDTO : EntityBase
    {
        public int ESTCODI { get; set; }
        public DateTime? ESTFECREGISTRO { get; set; }
        public string ESTESTADO { get; set; }
        public string ESTUSUEDICION { get; set; }
		public int EMPGCODI { get; set; }
    }
}
