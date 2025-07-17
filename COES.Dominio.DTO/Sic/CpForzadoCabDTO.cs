using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_FORZADO_CAB
    /// </summary>
    public class CpForzadoCabDTO : EntityBase
    {
        public int Cpfzcodi { get; set; }
        public int Topcodi { get; set; }
        public DateTime Cpfzfecha { get; set; }
        public int Cpfzbloquehorario { get; set; }
        public string Cpfzusuregistro { get; set; }
        public DateTime Cpfzfecregistro { get; set; }

        public string CpfzfecregistroDesc { get; set; }
    }
}
