using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class IntegranteController : BaseController
    {
        /// <summary>
        /// Instancia de la clase de servicio correspondiente
        /// </summary>
        TramiteVirtualAppServicio servicio = new TramiteVirtualAppServicio();

        /// <summary>
        /// Código de módulo asociado
        /// </summary>
        public int IdModulo = 34;

        /// <summary>
        /// Muestra la pantalla inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            IntegranteModel model = new IntegranteModel();

            return View(model);
        }

        [HttpPost]
        public PartialViewResult Listado(string tipoAgente, int tipoEmpresa, string indicador, string ruc, string razonSocial, int nroPagina)
        {
            if (string.IsNullOrEmpty(ruc)) ruc = "-1";
            if (string.IsNullOrEmpty(razonSocial)) razonSocial = "-1";
            IntegranteModel model = new IntegranteModel();
            model.ListadoEmpresa = this.servicio.ObtenerEmpresasPortalTramite(tipoAgente, tipoEmpresa, indicador, ruc, razonSocial, nroPagina, Constantes.PageSize);
            return PartialView(model);
        }


        /// <summary>
        /// Muesta el paginado
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="ruc"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="estado"></param>
        /// <param name="empresaSein"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string tipoAgente, int tipoEmpresa, string indicador, string ruc, string razonSocial)
        {
            Paginacion model = new Paginacion();


            if (string.IsNullOrEmpty(ruc)) ruc = "-1";
            if (string.IsNullOrEmpty(razonSocial)) razonSocial = "-1";
            int nroRegistros = servicio.ObtenerNroRegistrosBusquedaTramite(tipoAgente, tipoEmpresa, indicador, ruc, razonSocial);
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.Paginado(model);
        }


        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string tipoAgente, int tipoEmpresa)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin;
                string file = ConstantesAdmin.ReporteContactoEmpresa;
                List<SiEmpresaCorreoDTO> listado = this.servicio.ObtenerPesonasContactoExportacion(tipoAgente, tipoEmpresa);
                ReporteHelper.GenerarReporteContactoPorEmpresa(listado, path, file);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin + ConstantesAdmin.ReporteContactoEmpresa;
            return File(fullPath, Constantes.AppExcel, ConstantesAdmin.ReporteContactoEmpresa);
        }


        public ActionResult Detalle(int id, string ind)
        {
            IntegranteModel model = new IntegranteModel();          
            model.IdEmpresa = id;
            model.Integrante = ind;
            return View(model);
        }

        [HttpPost]
        public PartialViewResult Representantes(int id, string ind)
        {
            IntegranteModel model = new IntegranteModel();
            model.ListaRepresentantes = this.servicio.ObtenerRepresentantesLegales(id);
            model.IdEmpresa = id;
            model.Integrante = ind;
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult DetalleRepresentante(int id, int idEmpr, string ind)
        {
            IntegranteModel model = new IntegranteModel();

            if(id > 0)
            {
                model.Representante = this.servicio.ObtenerRepresentanteLegal(id);
                model.IdEmpresa = idEmpr;
                model.Integrante = ind;
            }
            else
            {
                model.Representante = new Dominio.DTO.Sic.SiRepresentanteDTO
                {
                    Emprcodi = idEmpr,
                    Rpteindnotic = Constantes.SI
                };
                model.IdEmpresa = idEmpr;
                model.Integrante = ind;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos del representante
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarRepresentante(SiRepresentanteDTO representante)
        {
            try
            {
                this.servicio.ActualizarRepresentante(representante);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener el listado de correos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Correos(int idEmpresa)
        {
            IntegranteModel model = new IntegranteModel();
            model.ListaEmpresaCorreo = this.servicio.ObtenerCorreosPorEmpresa(idEmpresa, IdModulo);
            model.IdEmpresa = idEmpresa;
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult DetalleCorreo(int idCorreo, int idEmpresa)
        {
            IntegranteModel model = new IntegranteModel();
            if (idCorreo != 0)
            {
                model.EmpresaCorreo = this.servicio.ObtenerEmpresaCorreo(idCorreo);
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
            return PartialView(model);
        }
                    

        /// <summary>
        /// Permite eliminar los datos del correo
        /// </summary>
        /// <param name="idEmpresaCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarCorreo(int idEmpresaCorreo, int idEmpresa)
        {
            try
            {
                this.servicio.EliminarCuentaCorreoTramite(idEmpresaCorreo, idEmpresa, IdModulo);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar los datos del correo
        /// </summary>
        /// <param name="idRpt"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarRepresentante(int idRpt)
        {
            try
            {
                this.servicio.EliminarRepresentante(idRpt, User.Identity.Name);
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
        public JsonResult GrabarCorreo(IntegranteModel model)
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
                    Modcodi = this.IdModulo,
                    Lastuser = User.Identity.Name
                };

                this.servicio.GrabarContacto(entity);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite crear las credenciales
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CrearCredenciales(int idEmpresa)
        {
            try
            {
                int result = this.servicio.GenerarCredencialIntegrante(idEmpresa, IdModulo);
                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }

        //[HttpPost]
        //public JsonResult GenerarMasivo(string empresas, string tipo)
        //{
        //    try
        //    {
        //        this.servicio.GenerarCredencialIntegrante(empresas, tipo, 0);
        //        return Json(1);
        //    }
        //    catch
        //    {
        //        return Json(-1);
        //    }
        //}
    }
}
