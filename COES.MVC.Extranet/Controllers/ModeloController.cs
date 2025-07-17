using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Planificacion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Controllers
{
    public class ModeloController : BaseController
    {
        string pathModelo = @"\\coes.org.pe\archivosapp\webapp\";

        public string Folder
        {
            get { return Session["Folder"].ToString(); }
            set { Session["Folder"] = value; }
        }

        public string NombreAplicativo
        {
            get { return Session["NombreAplicativo"].ToString(); }
            set { Session["NombreAplicativo"] = value; }
        }

        public int IdModelo
        {
            get { return int.Parse(Session["IdModelo"].ToString()); }
            set { Session["IdModelo"] = value; }
        }


        /// <summary>
        /// Instancia de la clase de servicio1
        /// </summary>
        ModplanAppServicio servicio = new ModplanAppServicio();

        // GET: Planificacion
        public ActionResult Index(string id)
        {
            PlanificacionModel model = new PlanificacionModel();

            if (id == "xgsddd")
            {
                this.Folder = @"Modelos\Yupana\";
                this.IdModelo = 2;
                model.NombreAplicativo = "Yupana";
                this.NombreAplicativo = "Yupana";
                model.Indicador = Constantes.SI;
                model.IdAplicativo = this.IdModelo;
            }
            else if (id == "sdwfacc")
            {
                this.Folder = @"Modelos\Arpay\";
                this.IdModelo = 3;
                model.NombreAplicativo = "Arpay";
                this.NombreAplicativo = "Arpay";
                model.Indicador = Constantes.SI;
                model.IdAplicativo = this.IdModelo;
            }
            else if (id == "frfagr")
            {
                this.Folder = @"Modelos\Quipu\";
                this.IdModelo = 4;
                model.NombreAplicativo = "Quipu";
                this.NombreAplicativo = "Quipu";
                model.Indicador = Constantes.SI;
                model.IdAplicativo = this.IdModelo;
            }
            else if (id == "qewrdfwer")
            {
                this.Folder = @"Modelos\Kumpliy\";
                this.IdModelo = 5;
                model.NombreAplicativo = "KUMPLIY";
                this.NombreAplicativo = "KUMPLIY";
                model.Indicador = Constantes.SI;
                model.IdAplicativo = this.IdModelo;
            }
            else
            {
                model.Indicador = Constantes.NO;
            }

            return View(model);
        }

        /// <summary>
        /// Inicio de la página
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado()
        {
            PlanificacionModel model = new PlanificacionModel();
            model.Listado = this.servicio.ListWbVersionModplans(this.IdModelo);
            return PartialView(model);
        }

        /// <summary>
        /// Permite visualizar el detalle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Detalle(int id)
        {
            PlanificacionModel model = new PlanificacionModel();
            model.Codigo = id;
            string url = "VERSION_" + id + @"\";
            WbVersionModplanDTO entity = this.servicio.ObtenerDetalleVersionAdicional(id, pathModelo, this.Folder + url);
            WbVersionModplanDTO entityPadre = this.servicio.GetByIdWbVersionModplan((int)entity.Vermplpadre);
            model.ListadoModelo = entity.ListadoArchivos.Where(x => x.Arcmplindtc == 1.ToString()).ToList();
            model.ListadoManual = entity.ListadoArchivos.Where(x => x.Arcmplindtc == 2.ToString()).ToList();
            model.NombreVersion = entity.Vermpldesc;
            model.NombrePlan = entityPadre.Vermpldesc;
            model.NombreAplicativo = this.NombreAplicativo;
           

            return PartialView(model);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivo(int idVersion, string indicador)
        {
            string url;
            if (indicador == "86") {
                url = AppDomain.CurrentDomain.BaseDirectory+ "Documentos"+ @"\";
            } else {
                url = "VERSION_" + idVersion + @"\";
            }
            string extension = string.Empty;
            string nombre = string.Empty;
            Stream stream = this.servicio.ObtenerArchivoAdicional(idVersion, indicador, pathModelo, this.Folder + url, out extension, out nombre);

            WbRegistroModplanDTO entity = new WbRegistroModplanDTO()
            {
                Vermplcodi = idVersion,
                Regmodusuario = base.UserName,
                Regmodfecha = DateTime.Now,
                Emprcodi = base.EmpresaId,
                Regmodtipo = this.IdModelo,
                Arcmplcodi= int.Parse(indicador)
            };

            this.servicio.SaveRegistroModPlan(entity);

            if (indicador == "86")
            {
                return File(url + nombre, extension, nombre);
            }
            else
            {
                return File(stream, extension, nombre);
            }
        }


    }
}