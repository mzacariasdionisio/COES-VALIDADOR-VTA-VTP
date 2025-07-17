using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.MercadoMayorista.Models
{
    public class BrowserModel
    {
        public string BaseDirectory { get; set; }
        public List<FileData> DocumentList { get; set; }
        public List<BreadCrumb> BreadList { get; set; }
        public string Origen { get; set; }
    }
}