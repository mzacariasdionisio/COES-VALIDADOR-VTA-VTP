using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_CODIGO_RETIRO_GENERADO
    /// </summary>
    public class CodigoGeneradoDTO
    {
        public int  Coregecodi { get; set; }
        public int Coresdcodi { get; set; }
        public string Coregeestado { get; set; }
        public string Coregeusuregistro { get; set; }
        public DateTime Coregefecharegistro { get; set; }
        public string Coregecodigovtp { get; set; }
        public int Coresocodi { get; set; }
        public string BarrNombSum { get; set; }

    }
}
