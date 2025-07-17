using COES.Storage.App.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Models
{
    public class SearchModel
    {
        public string Texto { get; set; }
        public string Extension { get; set; }
        public List<FileData> ListaResultado { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
    }
}