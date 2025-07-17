using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_CODIGO_CONSOLIDADO
    /// </summary>
    public class VtpCodigoConsolidadoDTO : EntityBase
    {
        public int Codcncodi { get; set; }
        public int Emprcodi { get; set; }
        public string Empresa { get; set; }
        public int Clicodi { get; set; }
        public string Cliente { get; set; }
        public int Barrcodi { get; set; }
        public string Barra { get; set; }
        public string Codcncodivtp { get; set; }
        public int Codcnpotegre { get; set; }
        public string TipUsuNombre { get; set; }
        public string TipConNombre { get; set; }
    }
}
