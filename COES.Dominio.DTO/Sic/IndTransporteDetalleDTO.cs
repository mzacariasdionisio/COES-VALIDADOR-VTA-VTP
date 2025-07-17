using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class IndTransporteDetalleDTO
    {
        public int Tnsdetcodi { get; set; }
        public int Cpctnscodi { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnombalter { get; set; }
        public DateTime Tnsdetfecha { get; set; }
        public decimal Tnsdetcntadquirida { get; set; }
        public decimal Tnsdetprctransferencia { get; set; }
        public string Tnsdetptosuministro { get; set; }
        public int TnsdetCompraventa { get; set; }
        public string Tnsdetusucreacion { get; set; }
        public DateTime Tnsdetfeccreacion { get; set; }
        public string Tnsdetusumodificacion { get; set; }
        public DateTime Tnsdetfecmodificacion { get; set; }
        
        //Adicional
        public string Emprnomb { get; set; }
        public string Tnsdetdescripcion { get; set; }

        //Campos para el reporte de cumplimiento
        public string FechaCumplimiento { get; set; }
        public string EmpresaCompra { get; set; }
        public decimal CantidadCompra { get; set; }
        public string EmpresaVenta { get; set; }
        public decimal CantidadVenta { get; set; }
    }
}
