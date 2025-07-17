using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TiempoReal.Models
{
    public class CargaArchxmlSp7Model
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }

        public List<TrCargaarchxmlSp7DTO> ListaCargaArchivos { get; set; }
    }
}