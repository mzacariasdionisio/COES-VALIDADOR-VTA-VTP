using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class ExportExcelModel
    {
        public List<ExportExcelDTO> ListaExportExcel { get; set; }
        public ExportExcelDTO Entidad { get; set; }
       
    }
}