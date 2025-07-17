using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class CodigoRetiroRelacionDTO
    {
        public int RetrelCodi { get; set; }
        public int Emprcodivtea { get; set; }
        public int Emprcodivtp { get; set; }
        public decimal Retrelvari { get; set; }
        public string Retelestado { get; set; }
        public string Retrelusucreacion { get; set; }
        public string Retrelfeccreacion { get; set; }
        public string Retrelusumodificacion { get; set; }
        public string Retrelfecmodificacion { get; set; }

        public string EmpresaVTEA { get; set; }
        public string ClienteVTEA { get; set; }
        public string Barrtrans { get; set; }
        public string EmpresaVTP { get; set; }
        public string ClienteVTP { get; set; }
        public string Barrsum { get; set; }

        public decimal getMaxPercent()
        {
            return Math.Round(Retrelvari + (Retrelvari / 100),2);
        }
        public decimal PorcentajeVariacionCalculado { get; set; }
        public decimal EnergiaVtea { get; set; }
        public decimal PotenciaVtp { get; set; }
        public decimal DiferenciaVteaVtp { get; set; }

        public List<CodigoRetiroRelacionDetalleDTO> ListarRelacion { get; set; }
   
    }
}
