using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.StockCombustibles.Models
{
    public class StockCombustiblesModel : FormatoModel
    {
        public Boolean EnabledStockInicio { get; set; }
        public List<decimal> ListaStockInicio { get; set; }
        public Boolean IsExcelWeb { get; set; }
        public string FechaNext { get; set; }
    }
}