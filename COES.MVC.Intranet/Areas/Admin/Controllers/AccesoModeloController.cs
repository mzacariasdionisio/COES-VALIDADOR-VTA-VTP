using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class AccesoModeloController : BaseController
    {
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        TramiteVirtualAppServicio servicioCorreo = new TramiteVirtualAppServicio();

        public ActionResult Index()
        {
            AccesoModeloModel model = new AccesoModeloModel();
            model.ListaEmpresas = (new EmpresaAppServicio()).GetEmpresaSistemaPorTipo("-1").Where(x=>x.Emprcoes != "S").ToList();
            model.ListaModulos = this.seguridad.ListarModulos().Where(x=>x.ModCodi == 30).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener el listado de correos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Correos(int idEmpresa, int idModulo)
        {
            AccesoModeloModel model = new AccesoModeloModel();
            model.ListaEmpresaCorreo = this.servicioCorreo.ObtenerCorreosPorEmpresa(idEmpresa, idModulo);
            model.IdEmpresa = idEmpresa;
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ListaAcceso(int idEmpresa, int idModulo)
        {
            AccesoModeloModel model = new AccesoModeloModel();
            model.ListaAccesos = (new GeneralAppServicio()).ListadoAccesos(idEmpresa, idModulo);
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult Acceso(int idEmpresaCorreo, int idEmpresa, int idModulo)
        {
            AccesoModeloModel model = new AccesoModeloModel();
            model.IdEmpresaCorreo = idEmpresaCorreo;
            model.IdEmpresa = idEmpresa;
            model.IdModulo = idModulo;
            model.ActivoDesde = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ActivoHasta = DateTime.Now.AddDays(7).ToString(Constantes.FormatoFecha);
            model.NroVeces = 5;
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult DetalleCorreo(int idCorreo, int idEmpresa, int idModulo)
        {
            AccesoModeloModel model = new AccesoModeloModel();
            if (idCorreo != 0)
            {
                model.EmpresaCorreo = this.servicioCorreo.ObtenerEmpresaCorreo(idCorreo);
            }
            else
            {
                model.EmpresaCorreo = new Dominio.DTO.Sic.SiEmpresaCorreoDTO
                {
                    Emprcodi = idEmpresa,
                    Empcorestado = Constantes.EstadoActivo,
                    Empcorindnotic = Constantes.SI
                };
            }
            model.IdModulo = idModulo;
            return PartialView(model);
        }


        /// <summary>
        /// Permite eliminar los datos del correo
        /// </summary>
        /// <param name="idEmpresaCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarCorreo(int idEmpresaCorreo)
        {
            try
            {
                (new GeneralAppServicio()).EliminarAccesosModPlanPorContacto(idEmpresaCorreo);
                this.servicioCorreo.EliminarCuentaCorreo(idEmpresaCorreo);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult EliminarAcceso(int id)
        {
            try
            {
                (new GeneralAppServicio()).EliminarAccesoModplan(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult GrabarAcceso(AccesoModeloModel model)
        {
            try
            {
                FwAccesoModeloDTO entity = new FwAccesoModeloDTO
                {
                    Acmodestado = Constantes.EstadoActivo,
                    Acmodfeccreacion = DateTime.Now,
                    Acmodfecinicio = DateTime.ParseExact(model.ActivoDesde, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                    Acmodfin = DateTime.ParseExact(model.ActivoHasta, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                    Acmodcodi = model.IdModulo,
                    Acmodveces = model.NroVeces,
                    Emprcodi = model.IdEmpresa,
                    Modcodi = model.IdModulo,
                    Empcorcodi = model.IdEmpresaCorreo,
                    Acmodusucreacion = base.UserName
                };

                (new GeneralAppServicio()).GrabarAccesoModelo(entity);


                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// permite grabar los datos de la nueva cuenta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEmpresaCorreo"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarCorreo(AccesoModeloModel model)
        {
            try
            {
                SiEmpresaCorreoDTO entity = new SiEmpresaCorreoDTO
                {
                    Empcorcargo = model.CargoCuenta,
                    Empcorcodi = model.CodigoCuenta,
                    Empcoremail = model.CorreoCuenta,
                    Empcorestado = model.EstadoCuenta,
                    Empcormovil = model.MovilCuenta,
                    Empcortelefono = model.TelefonoCuenta,
                    Empcornomb = model.NombreCuenta,
                    Emprcodi = model.EmpresaCuenta,
                    Empcorindnotic = model.IncluirNotificacion,
                    Modcodi = model.IdModulo,
                    Lastuser = User.Identity.Name
                };

                this.servicioCorreo.GrabarContacto(entity);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
