using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Medidores.Models
{
    /// <summary>
    /// Modelo de stock de combustibles
    /// </summary>
    public class StockCombustibleModel
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MeMedicion1DTO> ListaCombustible { get; set; }
        public List<MeMedicion1DTO> ListaConsulta { get; set; }

    }
}