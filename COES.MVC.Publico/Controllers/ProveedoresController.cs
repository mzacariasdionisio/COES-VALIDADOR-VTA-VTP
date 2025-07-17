using COES.Servicios.Aplicacion.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.MVC.Publico.Models;
using System.Web.Mvc;
using COES.MVC.Publico.Helper;
using System.Globalization;

namespace COES.MVC.Publico.Controllers
{
    public class ProveedoresController : Controller
    {
        /// <summary>
        /// Instancia del servicio asociado
        /// </summary>
        ProveedoresAppServicio servicio = new ProveedoresAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Carga el listado de proveedor
        /// </summary>
        /// <returns></returns>
        public ActionResult Listado()
        {
            ProveedorModel model = new ProveedorModel();            
            model.ListaTipoProveedor = servicio.GetByCriteriaTipo();
            return View(model);
        }

        /// <summary>
        /// Permite listar los proveedores
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="tipo"></param>
        /// <param name="fechaD"></param>
        /// <param name="fechaH"></param>
        /// <returns></returns>
        public PartialViewResult List(string nombre, string tipo, string fechaDesde, string fechaHasta)
        {
            ProveedorModel model = new ProveedorModel();

            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            if (!string.IsNullOrEmpty(fechaDesde)) 
                fechaInicio = DateTime.ParseExact(fechaDesde, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(fechaHasta))
                fechaFin = DateTime.ParseExact(fechaHasta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            model.ListaProveedor = servicio.GetByCriteriaWbProveedors(nombre, tipo, fechaInicio, fechaFin);            

            return PartialView(model);
        }

        /// <summary>
        /// Reglas de contratacion
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcesoContratacion()
        {
            return View();
        }

        /// <summary>
        /// Pagina de concurso de adquisicion
        /// </summary>
        /// <returns></returns>
        public ActionResult ConcursoAdquisicion()
        {
            return View();
        }

        /// <summary>
        /// Reglas de inscripcion
        /// </summary>
        /// <returns></returns>
        public ActionResult RequisitosInscripcion()
        {
            return View();
        }

        /// <summary>
        /// Formulario de inscripcion
        /// </summary>
        /// <returns></returns>
        public ActionResult FormularioInscripcion()
        {
            return View();
        }

        /// <summary>
        /// Concurso de Fideicomiso
        /// </summary>
        /// <returns></returns>
        public ActionResult ConcursoFideicomiso()
        {
            return View();
        }

        public ActionResult RegistroProveedores()
        {
            return View();
        }
    }
}
