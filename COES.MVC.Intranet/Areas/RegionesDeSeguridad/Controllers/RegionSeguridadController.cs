using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.RegionesDeSeguridad.Helper;
using COES.MVC.Intranet.Areas.RegionesDeSeguridad.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.RegionesDeSeguridad;
using COES.Servicios.Aplicacion.Yupana;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.RegionesDeSeguridad.Controllers
{
    public class RegionSeguridadController : BaseController
    {
        readonly RegionesDeSeguridadAppServicio regSegServicio = new RegionesDeSeguridadAppServicio();
        readonly YupanaAppServicio yupanaServicio = new YupanaAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(RegionSeguridadController));

        #region Declaración de variables
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }
        public RegionSeguridadController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Almacena solo en nombre del archivo
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session[DatosSesion.SesionFileName] != null) ?
                    Session[DatosSesion.SesionFileName].ToString() : null;
            }
            set { Session[DatosSesion.SesionFileName] = value; }
        }

        #endregion

        public ActionResult Index()
        {
            RegionSeguridadModel model = new RegionSeguridadModel();
            return View(model);
        }

        /// <summary>
        /// Listado de Regiones
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult RegionListado(string estado)
        {
            RegionSeguridadModel model = new RegionSeguridadModel();
            model.ListaRegion = regSegServicio.GetByCriteriaSegRegions(estado);
            return PartialView(model);
        }

        /// <summary>
        /// Obtener Objeto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult RegionObjeto(int id)
        {
            RegionSeguridadModel model = new RegionSeguridadModel();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                if (id > 0)
                {
                    model.SegRegion = regSegServicio.GetByIdSegRegion(id);
                }
                else
                {
                    model.SegRegion = new SegRegionDTO();
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite almacenar los datos del periodo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegionGuardar(int id, string nombre)
        {
            RegionSeguridadModel model = new RegionSeguridadModel();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);
                DateTime fecha = DateTime.Now;
                string usuario = base.UserName;
                //Guardar
                if (id > 0)
                {
                    var listaRegion = regSegServicio.GetByCriteriaSegRegions("T").Where(x => x.Regestado != ConstantesAppServicio.Baja && x.Regcodi != id && x.Regnombre.ToUpper().Trim() == nombre).ToList();
                    if (listaRegion.Count > 0)
                    {
                        throw new Exception("El nombre de la Región ya existe.");
                    }

                    SegRegionDTO reg = regSegServicio.GetByIdSegRegion(id);
                    reg.Regnombre = nombre;
                    reg.Regfecmodificacion = fecha;
                    reg.Regusumodificacion = usuario;

                    regSegServicio.UpdateSegRegion(reg);
                }
                else
                {
                    var listaRegion = regSegServicio.GetByCriteriaSegRegions("T").Where(x => x.Regestado != ConstantesAppServicio.Baja && x.Regnombre.ToUpper().Trim() == nombre).ToList();
                    if (listaRegion.Count > 0)
                    {
                        throw new Exception("El nombre de la Región ya existe.");
                    }

                    SegRegionDTO reg = new SegRegionDTO();
                    reg.Regnombre = nombre;
                    reg.Regfeccreacion = fecha;
                    reg.Regfecmodificacion = fecha;
                    reg.Regusumodificacion = usuario;
                    reg.Regusucreacion = usuario;
                    reg.Regestado = ConstantesAppServicio.Activo;

                    id = regSegServicio.SaveSegRegion(reg);
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }
            return Json(model);
        }

        /// <summary>
        /// Permite eliminar una region de seguridad
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="idLinea"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegionEliminar(int idRegion, string estado)
        {
            try
            {
                this.regSegServicio.DeleteSegRegion(idRegion, estado, base.UserName);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Obtener las restricciones de la región
        /// </summary>
        /// <param name="regcodi"></param>
        /// <returns></returns>
        public ActionResult RestriccionesIndex(int regcodi, int idtipo)
        {
            RegionSeguridadModel model = new RegionSeguridadModel();
            model.SegRegion = regSegServicio.GetByIdSegRegion(regcodi);
            model.Tipo = idtipo;
            switch (idtipo)
            {
                case 0:
                    model.TipoNombre = "Mínima";
                    break;
                case 1:
                    model.TipoNombre = "Media";
                    break;
                case 2:
                    model.TipoNombre = "Máxima";
                    break;
            }
            return View(model);
        }

        [HttpPost]
        public PartialViewResult RestriccionList(int regcodi, int idtipo, string estado)
        {
            RestriccionModel model = new RestriccionModel();
            model.ListaRestricciones = regSegServicio.GetByCriteriaSegCoordenadas(regcodi, idtipo, estado);
            return PartialView(model);
        }


        [HttpPost]
        public PartialViewResult RestriccionEdit(int segcocodi, int regcodi)
        {
            RestriccionModel model = new RestriccionModel();
            model.ListaZona = regSegServicio.ListSegZonas();

            if (segcocodi == 0)
            {
                model.Restriccion = new SegCoordenadaDTO();
                model.Restriccion.Regcodi = regcodi;
                //model.EntidadCoordenada.Estado = ConstantesCortoPlazo.EstadoActivo;
            }
            else
            {
                model.Restriccion = this.regSegServicio.GetByIdSegCoordenada(segcocodi);
                //model.ListaZona = this.regSegServicio.ListarEquipoLineaPorEmpresa(model.Entidad.Emprcodi, (int)model.Entidad.Famcodi);
            }

            return PartialView(model);
        }


        [HttpPost]
        public JsonResult RestriccionSave(RestriccionModel model)
        {
            try
            {
                SegCoordenadaDTO entity = new SegCoordenadaDTO();
                entity.Regcodi = model.Regcodi;
                entity.Segcoflujo1 = model.Segcoflujo1;
                entity.Segcogener1 = model.Segcogener1;
                entity.Segcoflujo2 = model.Segcoflujo2;
                entity.Segcogener2 = model.Segcogener2;
                entity.Segcotipo = model.Segcotipo;
                entity.Zoncodi = model.Zoncodi;
                entity.Segcocodi = model.Segcocodi;
                entity.Segcousucreacion = base.UserName;
                entity.Segcousumodificacion = base.UserName;
                entity.Segcofeccreacion = DateTime.Now;
                entity.Segcofecmodificacion = DateTime.Now;
                entity.Segcoestado = ConstantesAppServicio.Activo;

                int resultado = regSegServicio.GrabarCoordenadaRegionSeguridad(entity, base.UserName);
                //Actualizar Region de Congestion

                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult RestriccionDelete(int segcocodi, string estado)
        {
            try
            {

                this.regSegServicio.EliminarSegCoordenada(segcocodi, estado, base.UserName);
                return Json(1);
            }
            catch
            {
                return Json(1);
            }
        }

        /// <summary>
        /// Permite mostrar la pantalla de enlaces del conjunto
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EnlaceRegionSeguridadIndex(int idRegion, int idtipo)
        {
            RegionSeguridadModel model = new RegionSeguridadModel();
            model.Codigo = idRegion;
            model.Tipo = idtipo;
            return PartialView(model);
        }

        /// <summary>
        /// Permite listar los lineas de un grupo
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EnlaceRegionSeguridadList(int idRegion, int idtipo)
        {
            RegionSeguridadModel model = new RegionSeguridadModel();
            model.ListaDetalle = this.regSegServicio.GetByCriteriaSegRegionequipos(idRegion, idtipo);
            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener el listado de equipos
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerEquiposRegionSeguridad(int tipo)
        {
            try
            {
                return Json(this.regSegServicio.ObtenerEquiposConjunto(tipo));
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar los datos del enlace
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="idLinea"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnlaceRegionSeguridadSave(int idRegion, int idEquipo, int tipoRegion, int tipo)
        {
            try
            {
                if (tipo != 4)
                {
                    SegRegionequipoDTO entity = new SegRegionequipoDTO
                    {
                        Regcodi = idRegion,
                        Equicodi = idEquipo,
                        Segcotipo = tipoRegion,
                        Regeusucreacion = base.UserName,
                        Regefeccreacion = DateTime.Now
                    };

                    int resultado = this.regSegServicio.GrabarEquipoRegionSeguridad(entity, tipo);

                    return Json(resultado);

                }
                else
                {
                    SegRegiongrupoDTO entity = new SegRegiongrupoDTO
                    {
                        Regcodi = idRegion,
                        Grupocodi = idEquipo,
                        Segcotipo = tipoRegion,
                        Reggusucreacion = base.UserName,
                        Reggfeccreacion = DateTime.Now
                    };

                    int resultado = this.regSegServicio.GrabarGrupoRegionSeguridad(entity);// this.regSegServicio.SaveSegRegiongrupo(entity);

                    return Json(resultado);
                }
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar un enlace de un grupo
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="idLinea"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnlaceRegionSeguridadDelete(int idRegion, int idEquipo, int tipoRegion, int tipo)
        {
            try
            {
                if (tipo == 4)
                {

                    //this.regSegServicio.DeleteSegRegiongrupo(idRegion, idEquipo, tipoRegion);
                    this.regSegServicio.EliminarGrupoRegionSeguridad(idRegion, idEquipo, tipoRegion);
                }
                else
                {
                    this.regSegServicio.EliminarEquipoRegionSeguridad(idRegion, idEquipo, tipoRegion);
                    //this.regSegServicio.DeleteSegRegionequipo(idRegion, idEquipo, tipoRegion);
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Recibe formato cargado interface web
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegionesDeSeguridad.FolderUpload;
                    this.FileName = ruta + fileRandom + "." + ConstantesRegionesDeSeguridad.ExtensionFileUploadRegionSeguridad; 
                    file.SaveAs(this.FileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LeerFileUpExcel()
        {
            List<CpRecursoDTO> listaRecurso = yupanaServicio.ListarRecursoPorTopologia(0);
            string recursos = string.Join(",", listaRecurso.Where(x => x.Catcodi == 3 && x.Recurcodisicoes > 0).Select(x => x.Recurcodisicoes));
            List<PrGrupoDTO> listaUnidades = regSegServicio.ListaUnidades(recursos);
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegionesDeSeguridad.FolderUpload;
            var data = RegionDeSeguridadHelper.LeerExcelFile2(ruta,this.FileName, listaRecurso, listaUnidades, base.UserName);
            regSegServicio.CargaFormatoRegiones(data.ListaRegion, data.ListaCoordenada, data.ListaEquipo, data.ListaGrupo, data.ListaCmRegion, data.ListaCmRegionDetalle);
            return Json(1);
        }
    }
}
