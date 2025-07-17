using COES.Dominio.DTO.Scada;
using COES.MVC.Intranet.Areas.Coordinacion.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.RegistroObservacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Coordinacion.Controllers
{
    public class RegistroCorreoController : BaseController
    {
        /// <summary>
        /// Instancia de la clase de servicio
        /// </summary>
        RegistroObservacionAppServicio servicio = new RegistroObservacionAppServicio();

        /// <summary>
        /// Página de inicio
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RegistroCorreoModel model = new RegistroCorreoModel();
            model.ListaEmpresas = this.servicio.ObtenerEmpresasScada();            
            return View(model);
        }

        /// <summary>
        /// Permite listar las cuentas asociadas a las empresas
        /// </summary>
        /// <param name="tipoEmpresa"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaCuentas(int empresa)
        {
            RegistroCorreoModel model = new RegistroCorreoModel();
            model.ListaCuentas = this.servicio.GetByCriteriaTrObservacionCorreos(empresa);
            return PartialView(model);
        }


        /// <summary>
        /// Permite mostrar la pantalla de edicion de cuentas
        /// </summary>
        /// <param name="idCuenta"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AddCuenta(int idCuenta)
        {
            RegistroCorreoModel model = new RegistroCorreoModel();
            model.ListaEmpresas = this.servicio.ObtenerEmpresasScada();       

            if (idCuenta == 0)
            {
                model.Entidad = new TrObservacionCorreoDTO();
                model.Entidad.Emprcodi = -1;
            }
            else
            {
                model.Entidad = this.servicio.GetByIdTrObservacionCorreo(idCuenta);
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
        public JsonResult GrabarCuenta(int idCuenta, int emprcodi, string nombre, string email, string estado)
        {
            try
            {
                TrObservacionCorreoDTO entity = new TrObservacionCorreoDTO
                {
                    Obscorcodi = idCuenta,
                    Emprcodi = emprcodi,
                    Obscornombre = nombre,
                    Obscoremail = email,
                    Obscorestado = estado,
                    Obscorusumodificacion = base.UserName,
                    Obscorfecmodificacion = DateTime.Now
                };

                int resultado = this.servicio.SaveTrObservacionCorreo(entity);
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
                this.servicio.DeleteTrObservacionCorreo(idCuenta);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
