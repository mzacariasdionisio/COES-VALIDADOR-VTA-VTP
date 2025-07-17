using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class RelacionController : BaseController
    {
        EquipamientoAppServicio servicio = new EquipamientoAppServicio();
        IEODAppServicio servIeod = new IEODAppServicio();
        EventoAppServicio servEvento = new EventoAppServicio();

        readonly List<EstadoModel> lsEstados = new List<EstadoModel>();
        private List<DatoComboBox> listadoSINO = new List<DatoComboBox>();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(AreaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public RelacionController()
        {
            lsEstados.Add(new EstadoModel { EstadoCodigo = "A", EstadoDescripcion = "Activo" });
            lsEstados.Add(new EstadoModel { EstadoCodigo = "B", EstadoDescripcion = "Baja" });
            lsEstados.Add(new EstadoModel { EstadoCodigo = "P", EstadoDescripcion = "Pendiente" });
            lsEstados.Add(new EstadoModel { EstadoCodigo = "X", EstadoDescripcion = "Anulado" });
            listadoSINO.Add(new DatoComboBox { Descripcion = "SI", Valor = "S" });
            listadoSINO.Add(new DatoComboBox { Descripcion = "NO", Valor = "N" });
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("RelacionController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("RelacionController", ex);
                throw;
            }
        }
        //
        // GET: /Equipamiento/Relacion/

        public ActionResult IndexTipoRel()
        {
            TipoRelModel modelo = new TipoRelModel();
            modelo.idTipoRel = 0;
            modelo.ListaEstados = lsEstados;
            bool AccesoNuevo = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            modelo.EnableNuevo = AccesoNuevo ? "" : "disabled=disabled";
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";
            return View(modelo);
        }
        [ActionName("IndexTipoRel"), HttpPost]
        public ActionResult IndexPostTipoRel(TipoRelModel datosVentana)
        {
            datosVentana.ListaEstados = lsEstados;
            return View(datosVentana);
        }
        [HttpPost]
        public PartialViewResult ListaTipoRel(TipoRelModel datosVentana)
        {
            datosVentana.IndicadorPagina = false;
            
            string sEstado = datosVentana.EstadoCodigo ?? " ";
            var NombreTiporel = string.IsNullOrEmpty(datosVentana.NombreTiporel) ? "" : datosVentana.NombreTiporel.ToUpperInvariant();
            int nroRegistros = servicio.CantidadTipoRelxFiltro(NombreTiporel, sEstado);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                datosVentana.NroPaginas = nroPaginas;
                datosVentana.NroMostrar = Constantes.NroPageShow;
                datosVentana.IndicadorPagina = true;
            }

            var lsResultado = servicio.ListadoTipoRelxFiltroPaginado(NombreTiporel, sEstado, datosVentana.NroPagina, Constantes.PageSizeEvento);
            foreach (var oTipoRel in lsResultado)
            {
                oTipoRel.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oTipoRel.TiporelEstado);
                oTipoRel.TiporelUsuarioCreacion = EquipamientoHelper.EstiloEstado(oTipoRel.TiporelEstado);
            }
            datosVentana.ListaTiporel = lsResultado;

            return PartialView(datosVentana);
        }

        public PartialViewResult DetalleTipoRel(int idTiporel)
        {
            var oTipoRel= servicio.GetByIdEqTiporel(idTiporel);
            var oTipoRelModel = new TipoRelDetalleModel()
            {
                TiporelEstado = oTipoRel.TiporelEstado,
                EstadoDesc = EquipamientoHelper.EstadoDescripcion(oTipoRel.TiporelEstado),
                TiporelFechaCreacion = (DateTime.Equals(oTipoRel.TiporelFechaCreacion, null) ? "" : oTipoRel.TiporelFechaCreacion.Value.ToString("dd/MM/yyyy")),
                TiporelUsuarioCreacion = oTipoRel.TiporelUsuarioCreacion,
                TiporelFechaUpdate = (DateTime.Equals(oTipoRel.TiporelFechaUpdate, null) ? "" : oTipoRel.TiporelFechaUpdate.Value.ToString("dd/MM/yyyy")),
                TiporelUsuarioUpdate = oTipoRel.TiporelUsuarioUpdate,
                Tiporelcodi = oTipoRel.Tiporelcodi,
                Tiporelnomb = oTipoRel.Tiporelnomb
            };
            return PartialView(oTipoRelModel);
        }
        public PartialViewResult EditarTipoRel(int idTiporel)
        {
            var oTipoRel = servicio.GetByIdEqTiporel(idTiporel);
            var oTipoRelModel = new TipoRelDetalleModel()
            {
                TiporelEstado = oTipoRel.TiporelEstado,
                EstadoDesc = EquipamientoHelper.EstadoDescripcion(oTipoRel.TiporelEstado),
                TiporelFechaCreacion = (DateTime.Equals(oTipoRel.TiporelFechaCreacion, null) ? "" : oTipoRel.TiporelFechaCreacion.Value.ToString("dd/MM/yyyy")),
                TiporelUsuarioCreacion = oTipoRel.TiporelUsuarioCreacion,
                TiporelFechaUpdate = (DateTime.Equals(oTipoRel.TiporelFechaUpdate, null) ? "" : oTipoRel.TiporelFechaUpdate.Value.ToString("dd/MM/yyyy")),
                TiporelUsuarioUpdate = oTipoRel.TiporelUsuarioUpdate,
                Tiporelcodi = oTipoRel.Tiporelcodi,
                Tiporelnomb = oTipoRel.Tiporelnomb,
                lsEstado = lsEstados
            };
            return PartialView(oTipoRelModel);
        }

        public JsonResult ActualizarTipoRel(int iTipoRelCodi,string sNombreTipoRel,string sEstado)
        {
            try
            {
                var oTipoRel = servicio.GetByIdEqTiporel(iTipoRelCodi);
                oTipoRel.TiporelEstado = sEstado;
                oTipoRel.Tiporelnomb = sNombreTipoRel;
                oTipoRel.TiporelUsuarioUpdate = User.Identity.Name;
                servicio.UpdateEqTiporel(oTipoRel);
                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        public PartialViewResult NuevoTipoRel()
        {
            var oTipoRelModel = new TipoRelDetalleModel()
            {
                lsEstado = lsEstados
            };
            return PartialView(oTipoRelModel);
        }

        public JsonResult GuardarTipoRel(string sNombreTipoRel, string sEstado)
        {
            try
            {
                var oTipoRelNuevo = new EqTiporelDTO
                {
                    TiporelEstado = sEstado,
                    Tiporelnomb = sNombreTipoRel,
                    TiporelUsuarioCreacion = User.Identity.Name
                };
                servicio.SaveEqTiporel(oTipoRelNuevo);
                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        public ViewResult RelacionEquiposTipoRel(int idTipoRel)
        {
            var oTipoRel = servicio.GetByIdEqTiporel(idTipoRel);
            
            var oModeloRelEquipos = new FamRelTipoRelIndexModel
            {
                Tiporelcodi = oTipoRel.Tiporelcodi,
                Tiporelnomb = oTipoRel.Tiporelnomb,
                lsEstado = lsEstados
            };
            return View(oModeloRelEquipos);
        }

        public PartialViewResult ListaFamRelTipoRel(int idTipoRel,string sEstado)
        {
            var lsFamRel = servicio.ListarFamRelPorTipoRelEstado(idTipoRel, sEstado);
            List<FamRelModel> lsFamRelModel = new List<FamRelModel>();
            foreach (var oFamrel in lsFamRel)
            {
                var oFamRelModel = new FamRelModel();

                oFamRelModel.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oFamrel.Famrelestado);
                oFamRelModel.Famcodi1 = oFamrel.Famcodi1;
                oFamRelModel.Famcodi2 = oFamrel.Famcodi2;
                oFamRelModel.Famnomb1 = oFamrel.Famnomb1;
                oFamRelModel.Famnomb2 = oFamrel.Famnomb2;
                oFamRelModel.Famnumconec = oFamrel.Famnumconec;
                oFamRelModel.Famreltension = EquipamientoHelper.SiNoDescripcion(oFamrel.Famreltension);
                oFamRelModel.Famrelusuariocreacion = EquipamientoHelper.EstiloEstado(oFamrel.Famrelestado);

                lsFamRelModel.Add(oFamRelModel);
            }
            bool AccesoNuevo = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
           
            var oModeloRelEquipos = new FamRelTipoRelIndexModel
            {
                lsFamRel = lsFamRelModel,
                EnableNuevo = AccesoNuevo ? "" : "disabled=disabled",
                EnableEditar = AccesoEditar ? "" : "disableClick"
            };
            return PartialView(oModeloRelEquipos);
        }

        public PartialViewResult NuevoFamRel(int? iTipoRel)
        {
            var oModeloDetalle = new FamRelModel
            {
                LsTipoEquipo = servicio.ListEqFamilias().OrderBy(t => t.Famnomb).ToList(),
                LsTension = listadoSINO,
                Tiporelcodi = iTipoRel ?? 0,
            };
            return PartialView(oModeloDetalle);
        }

        public PartialViewResult DetalleFamRel(int iTipoRel, int iFamcodi1, int iFamcodi2)
        {
            var oFamRel = servicio.GetByIdEqFamrel(iTipoRel, iFamcodi1, iFamcodi2);
            var oModeloDetalle = new FamRelModel
            {
                Famnomb1 = oFamRel.Famnomb1,
                Famnomb2 = oFamRel.Famnomb2,
                Famnumconec = oFamRel.Famnumconec,
                Famrelfechacreacion = (DateTime.Equals(oFamRel.Famrelfechacreacion, null) ? "" : oFamRel.Famrelfechacreacion.Value.ToString("dd/MM/yyyy")),
                Famrelusuariocreacion = oFamRel.Famrelusuariocreacion,
                Famrelfechaupdate = (DateTime.Equals(oFamRel.Famrelfechaupdate, null) ? "" : oFamRel.Famrelfechaupdate.Value.ToString("dd/MM/yyyy")),
                Famrelusuarioupdate = oFamRel.Famrelusuarioupdate,
                Famreltension = EquipamientoHelper.SiNoDescripcion(oFamRel.Famreltension),
                Famrelestado = EquipamientoHelper.EstiloEstado(oFamRel.Famrelestado)
            };
            return PartialView(oModeloDetalle);
        }
        public PartialViewResult EditarFamRel(int iTipoRel, int iFamcodi1, int iFamcodi2)
        {
            var oFamRel = servicio.GetByIdEqFamrel(iTipoRel, iFamcodi1, iFamcodi2);
            var oModeloDetalle = new FamRelModel
            {
                Famnomb1 = oFamRel.Famnomb1,
                Famnomb2 = oFamRel.Famnomb2,
                Famnumconec = oFamRel.Famnumconec,
                Famreltension = oFamRel.Famreltension,
                Famrelestado = oFamRel.Famrelestado,
                LsTipoEquipo = servicio.ListEqFamilias().OrderBy(t=>t.Famnomb).ToList(),
                LsTension = listadoSINO,
                Famcodi1 = oFamRel.Famcodi1,
                Famcodi2 = oFamRel.Famcodi2,
                lsEstado = lsEstados
            };
            return PartialView(oModeloDetalle);
        }

        [HttpPost]
        public JsonResult GuardarNuevoFamRel(int iTipoRel, int iFamcodi1, int iFamcodi2, int? iNumCon,string sTension)
        {
            try
            {
                EqFamrelDTO oFamRel = new EqFamrelDTO
                {
                    Tiporelcodi = iTipoRel,
                    Famcodi1 = iFamcodi1,
                    Famcodi2 = iFamcodi2,
                    Famnumconec = iNumCon,
                    Famrelestado = "A",
                    Famreltension = sTension,
                    Famrelusuariocreacion = User.Identity.Name
                };
                servicio.SaveEqFamrel(oFamRel);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error("GuardarNuevoFamRel", ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult ActualizarFamRel(int iTipoRel, int iFamcodi1old, int iFamcodi2old, int iFamcodi1, int iFamcodi2, int? iNumCon, string sTension,string sEstado)
        {
            try
            {
                var oFamRelOriginal = servicio.GetByIdEqFamrel(iTipoRel, iFamcodi1old, iFamcodi2old);
                oFamRelOriginal.Famcodi1 = iFamcodi1;
                oFamRelOriginal.Famcodi2 = iFamcodi2;
                oFamRelOriginal.Famnumconec = iNumCon;
                oFamRelOriginal.Famrelestado = sEstado;
                oFamRelOriginal.Famreltension = sTension;
                oFamRelOriginal.Famrelusuarioupdate = User.Identity.Name;
                oFamRelOriginal.Famcodi1old = iFamcodi1old;
                oFamRelOriginal.Famcodi2old = iFamcodi2old;

                servicio.UpdateEqFamrel(oFamRelOriginal);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error("ActualizarFamRel", ex);
                return Json(-1);
            }
        }

        #region Relación de equipos EQ_EQUIREL

        public ActionResult IndexEquiposRel(int idTipoRel)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            var model = new EquipoRelModel();

            EqTiporelDTO oTipoRel = servicio.GetByIdEqTiporel(idTipoRel);
            List<EqFamrelDTO> lsFamRel = servicio.ListarFamRelPorTipoRelEstado(idTipoRel, " ");

            model.NombreTiporel = oTipoRel.Tiporelnomb;
            model.Tiporelcodi = oTipoRel.Tiporelcodi;
            model.ListaFamiliaRel = lsFamRel;

            return View(model);
        }

        /// <summary>
        /// Metodo para generar reporte html de Relacion de equipos
        /// </summary>
        /// <param name="idTipoRel"></param>
        /// <param name="famcodi1"></param>
        /// <param name="famcodi2"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEquipoRel(int idTipoRel, int famcodi1, int famcodi2)
        {
            var model = new EquipoRelModel();

            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");

                model.Resultado = servicio.GenerarReporteEquipoRel(idTipoRel, famcodi1, famcodi2, url);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Registrar/editar 
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarRelacionEquipo(string strJsonData)
        {
            var model = new EquipoRelModel();
            try
            {
                base.ValidarSesionJsonResult();
                //if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                model = serialize.Deserialize<EquipoRelModel>(strJsonData);

                //Validaciones
                if (model.Equicodi1 <= 0 || model.Equicodi2 <= 0)
                {
                    throw new Exception("Debe seleccionar equipo.");
                }

                if (model.Famcodi1 <= 0 || model.Famcodi2 <= 0)
                {
                    throw new Exception("Debe seleccionar Tipo de equipo.");
                }

                //Guardar
                EqEquirelDTO reg = new EqEquirelDTO();
                reg.Tiporelcodi = model.Tiporelcodi;
                reg.Equicodi1 = model.Equicodi1;
                reg.Equicodi2 = model.Equicodi2;
                reg.Famcodi1 = model.Famcodi1;
                reg.Famcodi2 = model.Famcodi2;
                reg.Equirelusumodificacion = User.Identity.Name;

                this.servicio.RegistrarRelacionEquipo(reg);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Dar de baja circuito
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarRelacionEquipo(int tiporelcodi, int equicodi1, int equicodi2)
        {
            var model = new EquipoRelModel();
            try
            {
                base.ValidarSesionJsonResult();
                //if (!base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                this.servicio.DeleteEqEquiRelDTO(equicodi1, tiporelcodi, equicodi2, User.Identity.Name);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }


        #region Búsqueda de Equipos

        /// <summary>
        /// View Busqueda equipos
        /// </summary>
        /// <returns></returns>
        public PartialViewResult BusquedaEquipo(int famcodi1, int famcodi2, string filtroFamilia = "0")
        {
            var model = new EquipoRelModel();
            model.ListaEmpresas = this.servIeod.ListarEmpresasxTipoEquipos(famcodi1+ ","+ famcodi2);
            model.ListaFamilia = this.servIeod.ListarFamilia().Where(x=>x.Famcodi == famcodi1 || x.Famcodi == famcodi2).ToList();
            model.FiltroFamilia = filtroFamilia;

            return PartialView(model);
        }

        /// <summary>
        /// Muestra el resultado de la busqueda
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="idArea"></param>
        /// <param name="filtro"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoResultado(int idEmpresa, int idFamilia, string filtro, int nroPagina, int? idArea = 0, string filtroFamilia = "0", int? tipoFormulario = 0)
        {
            var model = new EquipoRelModel();
            model.TipoFormulario = tipoFormulario.GetValueOrDefault(0);

            List<EqEquipoDTO> listaEquipo = new List<EqEquipoDTO>();
            var listaEq = this.servEvento.BuscarEquipoEvento(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro, nroPagina, Constantes.NroPageShow);

            foreach (var reg in listaEq)
            {
                EqEquipoDTO eq = new EqEquipoDTO();
                eq.Emprnomb = reg.EMPRENOMB;
                eq.Areanomb = reg.AREANOMB;
                eq.Equicodi = reg.EQUICODI;
                eq.Equinomb = reg.EQUIABREV != null ? reg.EQUIABREV.Trim() : string.Empty;
                eq.Equiabrev = reg.EQUIABREV;
                eq.Famabrev = reg.FAMABREV;
                eq.Emprcodi = reg.EMPRCODI;
                eq.Areacodi = reg.AREACODI;
                eq.Famcodi = reg.FAMCODI;

                listaEquipo.Add(eq);
            }

            model.ListaEquipo = listaEquipo;
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="idArea"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoPaginado(int idEmpresa, int idFamilia, string filtro, int? idArea = 0, string filtroFamilia = "0")
        {
            var model = new EquipoRelModel();
            model.IndicadorPagina = false;
            int nroRegistros = this.servEvento.ObtenerNroFilasBusquedaEquipo(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.NroPageShow;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Muestra las areas de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoArea(int idEmpresa, int idFamilia, string filtroFamilia = "0")
        {
            var model = new EquipoRelModel();
            model.ListaArea = this.servEvento.ObtenerAreaPorEmpresa(idEmpresa, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia)).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Funcion para obtener las familias permitidas. 
        /// </summary>
        /// <param name="idFamilia"></param>
        /// <param name="filtroFamilia"> -1: filtrar todas las familias, 0: filtrar solo para lineas de tranmision </param>
        /// <returns></returns>
        private string ListarFamiliaByFiltro(int idFamilia, string filtroFamilia = "0")
        {
            if (idFamilia == 0)
            {
                return filtroFamilia;
            }

            return idFamilia.ToString();
        }

        #endregion

        #endregion
    }
}
