using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class SolicitudController : BaseController
    {
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Muestra la pantalla inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SolicitudModel model = new SolicitudModel();
            model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => x.EMPRCODI > 1).OrderBy(x => x.EMPRNOMB).ToList();
            return View(model);
        }

        /// <summary>
        /// Permite mostrar la vista de nuevas solicitudes
        /// </summary>
        /// <returns></returns>
        public ActionResult Detalle(int id)
        {
            SolicitudModel model = new SolicitudModel();

            SolicitudExtDTO solicitud = this.seguridad.ObtenerSolicitudExtranet(id);
            EmpresaDTO empresa = this.seguridad.ObtenerEmpresa((int)solicitud.EmprCodi);
            model.EmpresaNombre = empresa.EMPRNOMB;
            model.IdSolicitud = id;
            model.TipoSolicitud = solicitud.SolicTipo;

            return View(model);
        }

        /// <summary>
        /// Lista las solicitudes por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Grilla(int? idEmpresa, string estado)
        {
            SolicitudModel model = new SolicitudModel();
            if(idEmpresa == null)idEmpresa = 0;
            model.ListaSolicitud = this.seguridad.ListarSolicituExtranet((int)idEmpresa, estado).ToList();
            return PartialView(model);
        }


        /// <summary>
        /// Permite obtener el header de la grilla
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        protected string ObtenerHeader(string cadena)
        {
            string resultado = string.Empty;
            for (int i = 0; i < cadena.Length; i++)
            {
                resultado = resultado + (cadena[i]).ToString().ToUpper();
                if (i != cadena.Length - 1) { resultado = resultado + "<br />"; }
            }

            return resultado;
        }

        /// <summary>
        /// Permite mostrar la grilla de visualizacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EdicionExcel(int idSolicitud)
        {
            ExcelModel model = new ExcelModel();
            List<ModuloDTO> listModulo = this.seguridad.ListarModulos().ToList();
            List<int> listIdModulos = listModulo.OrderBy(x => x.ModNombre).Select(x => (int)x.ModCodi).ToList();
           
            model.Headers = listModulo.OrderBy(x => x.ModNombre).Select(x => this.ObtenerHeader(x.ModNombre)).ToArray();
            model.Widths = listModulo.Select(x => 40).ToArray();

            SolicitudExtDTO solicitud = this.seguridad.ObtenerDetalleSolicitudExtranet(idSolicitud);

            List<SolicitudDetalleDTO> listUsuarios = solicitud.ListaUsuarios.ToList();

            string[][] data = new string[2 + listUsuarios.Count][];


            data[0] = new string[3 + listModulo.Count];
            data[1] = new string[3 + listModulo.Count];


            data[0][0] = "Datos del Uuuario";
            data[0][3] = "Módulos solicitados";

            data[1][0] = "Nombres";
            data[1][1] = "Correo";
            data[1][2] = "Teléfono";

            int k = 0;
            foreach (string item in model.Headers)
            {
                data[1][k + 3] = item;
                k++;
            }

            int i = 2;
            foreach (SolicitudDetalleDTO item in listUsuarios)
            {
                data[i] = new string[3 + listModulo.Count];
                data[i][0] = item.UserName;
                data[i][1] = item.UserEmail;
                data[i][2] = item.UserTlf;

                List<int> idModuloSelected = item.ListaModulos.Select(x => (int)x.ModCodi).ToList();

                for (int j = 0; j < listIdModulos.Count; j++)
                {
                    if (idModuloSelected.Contains(listIdModulos[j]))
                    {
                        data[i][3 + j] = "X";
                    }
                }
                i++;
            }

            model.Data = data;
            object[] configuracion = new object[model.Headers.Length];

            int index = 0;
            foreach (string columna in model.Headers)
            {
                configuracion[index] = new
                {
                    type = "text",
                    readOnly = false
                };

                index++;
            }

            model.Columnas = configuracion;

            return Json(model);
        }


        /// <summary>
        /// Permite listar los usuarios para ser dados de baja
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Usuarios(int idSolicitud)
        {
            RepresentanteModel model = new RepresentanteModel();
            model.ListaUsuarios = this.seguridad.ListarUsuarioSolicitudBaja(idSolicitud).ToList();           

            return PartialView(model);
        }

        /// <summary>
        /// Permite anular la solicitud
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AnularSolicitud(int idSolicitud)
        {
            try
            {
                this.seguridad.AprobarSolicitudExtranet(idSolicitud, base.UserName, ConstantesAdmin.EstadoAnulado);
                this.seguridad.EnviarNotificacionSolicitudExtranet(idSolicitud, ConstantesAdmin.EstadoAnulado);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite dar por aprobada la solicitud
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AprobarSolicitud(int idSolicitud)
        {
            try
            {
                this.seguridad.AprobarSolicitudExtranet(idSolicitud, base.UserName, ConstantesAdmin.EstadoAprobado);
                this.seguridad.EnviarNotificacionSolicitudExtranet(idSolicitud, ConstantesAdmin.EstadoAprobado);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
