using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Storage.App.Base.Core;

namespace COES.MVC.Publico.Areas.Publicaciones.Models
{
    /// <summary>
    /// Model para el reporte de pruebas aleatorias
    /// </summary>
    public class EstadisticasAnualesModel
    {
        public int anio { get; set; }
        public int infografia { get; set; }

        public int ejecutiva { get; set; }
        public List<FileData> ListadoExcel { get; set; }
    }
}