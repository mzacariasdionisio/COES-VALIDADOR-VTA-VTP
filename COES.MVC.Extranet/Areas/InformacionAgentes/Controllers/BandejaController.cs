using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.InformacionAgentes.Models;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.InformacionAgentes;
using log4net;


namespace COES.MVC.Extranet.Areas.InformacionAgentes.Controllers
{
    public class BandejaController : Controller
    {
        //
        // GET: /InformacionAgentes/Bandeja/
        private const string sPolicy = "{\"expiration\": \"XFECHAXT00:00:00Z\"," + "\"conditions\": [" + "{\"bucket\": \"XBUCKETX\"}," + "[\"starts-with\", \"$key\", \"\"]," + "{\"acl\": \"public-read\"}," + "[\"starts-with\", \"$Content-Type\", \"\"]," + "[\"content-length-range\", 0, 5368709120]" + "]" + "}";
        private InformacionAgentesAppServicio appInformacionAgentes = new InformacionAgentesAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(BandejaController));
        public ActionResult Index()
        {
            var model = new FileUploadViewModel
            {
                ListaEmpresa = (new SeguridadServicio.SeguridadServicioClient()).ObtenerEmpresasPorUsuario(User.Identity.Name).ToList(),
                AWSAccessKey = this.AwsAccessKeyID,
                Acl = "public-read",
                Base64Policy = this.Policy,
                Bucket = ConfigurationManager.AppSettings["BucketSgiDemanda"],
                Signature = GetS3Signature(myPolicy),
                FormAction = string.Format("https://{0}.s3.amazonaws.com/", ConfigurationManager.AppSettings["BucketSgiDemanda"]),
            };
            return View(model);
        }

        public BandejaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("BandejaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("BandejaController", ex);
                throw;
            }
        }
        #region FuncionesAWS-JS
        public string AwsAccessKeyID
        {
            get
            {
                return ConfigurationManager.AppSettings["AwsAccessKey"];
            }
        }
        private string AwsSecretKey
        {
            get
            {
                return ConfigurationManager.AppSettings["AwsSecret"];
            }
        }
        private string myPolicy
        {
            get
            {
                return sPolicy.Replace("XFECHAX", DateTime.Today.AddDays(1).ToString("yyy-MM-dd"))
                    .Replace("XBUCKETX", ConfigurationManager.AppSettings["BucketSgiDemanda"]);
            }
        }
        public string Policy
        {
            get
            {
                return Convert.ToBase64String(Encoding.ASCII.GetBytes((myPolicy)));
            }
        }
        public string Signature
        {
            get
            {
                return GetS3Signature(myPolicy);
            }
        }
        public string GetS3Signature(string policyStr)
        {
            string b64Policy = Convert.ToBase64String(Encoding.ASCII.GetBytes(policyStr));

            byte[] b64Key = Encoding.ASCII.GetBytes(AwsSecretKey);
            HMACSHA1 hmacSha1 = new HMACSHA1(b64Key);

            return Convert.ToBase64String(hmacSha1.ComputeHash(Encoding.ASCII.GetBytes(b64Policy)));
        }
        #endregion

        [HttpPost]
        public PartialViewResult Paginado(FileUploadViewModel model)
        {
            int nroRegistros = appInformacionAgentes.TotalListarArchivosPorEmpresa(model.iEmpresa);
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ListadoArchivos(FileUploadViewModel model)
        {
           var resultados = appInformacionAgentes.ListarArchivosPorEmpresa(model.iEmpresa, model.NroPagina, Constantes.PageSize);
           foreach (var item in resultados)
            {
                string link = string.Empty;
                if (string.IsNullOrEmpty(item.Archiruta) || string.IsNullOrWhiteSpace(item.Archiruta))
                {
                    item.Archiruta= link; 
                    continue;
                }
                if (item.Archiruta.ToUpperInvariant().Trim().StartsWith("HTTP"))
                {
                    link = "<a href='" + item.Archiruta + "' target='_blank'>Ver Archivo</a>";
                    link = link.Replace("'", "\"");
                    item.Archiruta = link;
                }
            }
            
            model.ListaResultados = resultados;
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GuardarDatosArchivo(int iEmpresa,string sNombreArchivo)
        {
            try
            {
                var nuevoArchivo = new InfArchivoAgenteDTO
                {
                    Archinomb = sNombreArchivo,
                    Emprcodi = iEmpresa,
                    Lastuser = User.Identity.Name,
                    Archiruta = @"https://s3.amazonaws.com/" + ConfigurationManager.AppSettings["BucketSgiDemanda"] + @"/" + HttpUtility.UrlEncode(sNombreArchivo)
                };
                appInformacionAgentes.SaveInfArchivoAgente(nuevoArchivo);
            }
            catch (Exception)
            {
                return Json(-1);
            }
            return Json(1);
        }

        [HttpPost]
        public JsonResult ExisteArchivo(string sNombreArchivo)
        {
            var iCantidad = appInformacionAgentes.CantidadArchivosPorNombre(sNombreArchivo.Trim());
            return Json(iCantidad.ToString());
        }
    }
}
