using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.DemandaBarras.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.DemandaBarras;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DemandaBarras.Controllers
{
    public class NotificacionController : BaseController
    {
        /// <summary>
        /// Instancia de la clase de servicio
        /// </summary>
        NotificacionAppServicio servicio = new NotificacionAppServicio();

        /// <summary>
        /// Página de inicio
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            NotificacionModel model = new NotificacionModel();
            model.ListaTipoEmpresa = this.servicio.ListarTipoEmpresas();
            model.ListaEmpresa = this.servicio.ListarEmpresas(string.Empty);
            model.FechaLog = DateTime.Now.ToString(Constantes.FormatoFecha);
            
            return View(model);
        }

        /// <summary>
        /// Permite listar las cuentas asociadas a las empresas
        /// </summary>
        /// <param name="tipoEmpresa"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaCuentas(int tipoEmpresa, int empresa)
        {
            NotificacionModel model = new NotificacionModel();
            model.ListaCuentas = this.servicio.ListarCuentaEmpresa(tipoEmpresa, empresa);
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar las empresas por tipo de empresa
        /// </summary>
        /// <param name="tipoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresas(int tipoEmpresa)
        {
            string idTipoEmpresa = tipoEmpresa.ToString();
            if (tipoEmpresa == -1) idTipoEmpresa = string.Empty;
            List<SiEmpresaDTO> entitys = this.servicio.ListarEmpresas(idTipoEmpresa);
            SelectList list = new SelectList(entitys, EntidadPropiedad.Emprcodi, EntidadPropiedad.Emprnomb);

            return Json(list);
        }

        /// <summary>
        /// Permite mostrar la pantalla de edicion de cuentas
        /// </summary>
        /// <param name="idCuenta"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AddCuenta(int idCuenta)
        {
            NotificacionModel model = new NotificacionModel();
            model.ListaEmpresa = this.servicio.ListarEmpresas(string.Empty);

            if (idCuenta == 0)
            {
                model.EntidadCuenta = new SiEmpresaCorreoDTO();
                model.EntidadCuenta.Emprcodi = -1;
            }
            else 
            {
                model.EntidadCuenta = this.servicio.ObtenerEntidadCuenta(idCuenta);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos de la cuenta adicional
        /// </summary>
        /// <param name="idCuenta"></param>
        /// <param name="emprcodi"></param>
        /// <param name="nombre"></param>
        /// <param name="email"></param>
        /// <param name="descripcion"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarCuenta(int idCuenta, int emprcodi, string nombre, string email, string descripcion, string estado)
        {
            try
            {
                SiEmpresaCorreoDTO entity = new SiEmpresaCorreoDTO
                {
                    Empcorcodi = idCuenta,
                    Emprcodi = emprcodi,
                    Empcornomb = nombre,
                    Empcoremail = email,
                    Empcordesc = descripcion,
                    Empcorestado = estado,
                    Lastuser = User.Identity.Name
                };

                int resultado = this.servicio.GrabarCuenta(entity);
                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar una cuenta
        /// </summary>
        /// <param name="idCuenta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarCuenta(int idCuenta) 
        {
            try
            {
                this.servicio.EliminarCuenta(idCuenta);
                return Json(1);
            }
            catch 
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener los datos de la plantilla
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPlantilla(int idPlantilla)
        {
            try 
            {
                CorreoAppServicio correo = new CorreoAppServicio();
                return Json(correo.GetByIdSiPlantillacorreo(idPlantilla));
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite actualizar la plantilla
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarPlantilla(NotificacionModel model)
        {
            try
            {
                CorreoAppServicio correo = new CorreoAppServicio();

                SiPlantillacorreoDTO plantilla = new SiPlantillacorreoDTO
                {
                    Plantcodi = model.IdPlantilla,
                    Plantasunto = model.AsuntoCorreo,
                    Plantcontenido = model.ContenidoCorreo
                };

                correo.ActualizarPlantilla(plantilla);

                return Json(1);
            }
            catch 
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite consultar los logs de correos enviados
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ConsultarLog(string fecha)
        {
            NotificacionModel model = new NotificacionModel();
            DateTime fechaConsulta = DateTime.Now;

            if (!string.IsNullOrEmpty(fecha)) {
                fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            model.ListaCorreos = this.servicio.ListarLogCorreo(fechaConsulta);

            return PartialView(model);
        }

        /// <summary>
        /// Permite visualizar el contenido del correo enviado
        /// </summary>
        /// <param name="idCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerContenido(int idCorreo)
        {
            try
            {
                SiCorreoDTO entity = (new CorreoAppServicio()).GetByIdSiCorreo(idCorreo);
                return Json(entity.Corrcontenido);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite mostrar el listado de configuracion de envios
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaEmpresas()
        {
            NotificacionModel model = new NotificacionModel();
            model.ListaEmpresaConfiguracion = this.servicio.ConfigurarEmpresasNotificacion();
            model.EstadoProceso = this.servicio.ObtenerEstadoProceso();

            return PartialView(model);
        }

        /// <summary>
        ///  Permite actualizar el indicador de notificacion
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarIndNotificacion(int emprcodi, string indicador)
        {
            try
            {
                if (indicador == Constantes.SI)
                    indicador = Constantes.NO;
                else
                    indicador = Constantes.SI;

                this.servicio.EstablecerNotificacion(emprcodi, indicador, base.UserName);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }      
        }

        /// <summary>
        /// Permite grabar la configuración de empresas
        /// </summary>
        /// <param name="empresas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarConfiguracionEmpresas(string empresas)
        {
            try
            {
                string modulos = (!string.IsNullOrEmpty(empresas)) ?
                           empresas.Substring(0, empresas.Length - 1) : string.Empty;
                List<int> idEmpresas = modulos.Split(',').Select(int.Parse).ToList();
                this.servicio.ActualizarNotificacionEmpresas(idEmpresas, base.UserName);

                return Json(1);
            }
            catch 
            {
                return Json(-1);    
            }
        }

        /// <summary>
        /// Permite configurar el proceso de configuracion
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConfigurarProceso(string estado)
        {
            try
            {
                this.servicio.ConfigurarProceso(estado);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
