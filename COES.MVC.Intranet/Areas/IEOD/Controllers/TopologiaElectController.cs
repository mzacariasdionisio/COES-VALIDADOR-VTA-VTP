using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    [ValidarSesion]
    public class TopologiaElectController : BaseController
    {
        IEODAppServicio servicio = new IEODAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Genera View para Index de Topologia
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            TopologiaElectModel model = new TopologiaElectModel();

            model.ListaEmpresas = servicio.ListarEmpresasxTipoEquipos(ConstantesTopologiaElect.TipodeEquipos);

            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial para mostrar listado de  ptos de medición
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="tipoOrigenLectura"></param>
        /// <param name="tipoPtomedicodi"></param>
        /// <param name="tipoEquipo"></param>
        /// <param name="nroPagina"></param>
        /// <param name="idsFamilia"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string empresas, string tipoEquipoPadre)
        {
            TopologiaElectModel model = new TopologiaElectModel();
            if (tipoEquipoPadre == null)
            {
                tipoEquipoPadre = "-1";
            }
            model.ListaTipoEquipos = servicio.ListarTopologiaEquiposPadres(empresas, tipoEquipoPadre, ConstantesTopologiaElect.TiporelcodiTopologia);
            return PartialView(model);
        }

        /// <summary>
        /// Genera la lista de tipos de Equipos padres en el Index
        /// </summary>
        /// <param name="idsAgente"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoEquiposPadresIndex(string empresas)
        {
            TopologiaElectModel model = new TopologiaElectModel();
            List<EqEquipoDTO> Lista = new List<EqEquipoDTO>();
            Lista = servicio.ListarCentralesXEmpresaGener2(empresas, ConstantesTopologiaElect.TipodeEquipos);

            List<EqEquipoDTO> listaTCentral = Lista.GroupBy(x => new { x.Famcodi, x.Famnomb })
                                .Select(y => new EqEquipoDTO()
                                {
                                    Famcodi = y.Key.Famcodi,
                                    Famnomb = y.Key.Famnomb
                                }
                                ).OrderBy(x => x.Famnomb).ToList();
            model.ListaTipoEquipos = listaTCentral;
            return PartialView(model);
        }

        /// <summary>
        /// Genera la lista de tipos de Equipos padres
        /// </summary>
        /// <param name="idsAgente"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoEquipoPadrePopup(string empresa, int idTipoEquipo, string activaFiltro, int idEquipoPadre)
        {
            TopologiaElectModel model = new TopologiaElectModel();
            model.IdTipoEquipo = idTipoEquipo;
            model.ActFiltroEquipoPadre = activaFiltro;
            model.IdEquipopadre = idEquipoPadre;
            List<EqEquipoDTO> Lista = new List<EqEquipoDTO>();
            Lista = servicio.ListarCentralesXEmpresaGener(empresa, ConstantesTopologiaElect.TipodeEquipos);

            List<EqEquipoDTO> listaTCentral = Lista.GroupBy(x => new { x.Famcodi, x.Famnomb })
                                .Select(y => new EqEquipoDTO()
                                {
                                    Famcodi = y.Key.Famcodi,
                                    Famnomb = y.Key.Famnomb
                                }
                                ).OrderBy(x => x.Famnomb).ToList();
            model.ListaTipoEquipos = listaTCentral;
            return PartialView(model);
        }

        /// <summary>
        /// Genera la lista de tipos de Equipos hijos
        /// </summary>
        /// <param name="idsAgente"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoEquipoHijoPopup(string empresa)
        {
            TopologiaElectModel model = new TopologiaElectModel();
            List<EqEquipoDTO> Lista = new List<EqEquipoDTO>();
            Lista = servicio.ListarCentralesXEmpresaGener(empresa, ConstantesTopologiaElect.TipodeEquipos);

            List<EqEquipoDTO> listaTCentral = Lista.GroupBy(x => new { x.Famcodi, x.Famnomb })
                                .Select(y => new EqEquipoDTO()
                                {
                                    Famcodi = y.Key.Famcodi,
                                    Famnomb = y.Key.Famnomb
                                }
                                ).OrderBy(x => x.Famnomb).ToList();
            model.ListaTipoEquipos = listaTCentral;
            return PartialView(model);
        }

        ////<summary>
        ////Genera la lista de equipos padres
        ////</summary>
        ////<param name="idsAgente"></param>
        ////<returns></returns>
        public PartialViewResult CargarEquiposPadresPopup(int idEmpresa, int idTipoPadre, int idEquipoPadre, string actFiltro)
        {
            TopologiaElectModel model = new TopologiaElectModel();
            model.IdEquipopadre = idEquipoPadre;
            model.ActFiltroEquipoPadre = actFiltro;
            List<EqEquipoDTO> Lista = new List<EqEquipoDTO>();
            Lista = this.servicio.ObtenerEquiposPorFamilia(idEmpresa, idTipoPadre);
            model.ListaEquiposPadres = Lista;
            return PartialView(model);
        }

        /// <summary>
        /// Genera la lista de equipos hijos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoHijo"></param>
        /// <returns></returns>
        public PartialViewResult CargarEquiposHijosPopup(int idEmpresa, int idTipoHijo)
        {
            TopologiaElectModel model = new TopologiaElectModel();
            List<EqEquipoDTO> Lista = new List<EqEquipoDTO>();
            Lista = this.servicio.ObtenerEquiposPorFamilia(idEmpresa, idTipoHijo);
            model.ListaEquipos = Lista;
            return PartialView(model);
        }

        /// <summary>
        /// Devuelve Vista Parcial para registrar datos del nuevo equipo asigando para de la topología
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewAgregarEquipos(int idEmpresa, int idEquipo, int tipoEquipo)
        {
            TopologiaElectModel model = new TopologiaElectModel();
            ////Carga valores iniciales si es modo editar
            model.IdEmpresa = idEmpresa;
            model.IdEquipopadre = idEquipo;
            model.IdTipoEquipo = tipoEquipo;
            model.ActFiltroEquipoPadre = (idEmpresa > 0) ? "disabled" : "";
            model.ListaEquiposTopologia = servicio.ListarDetalleEquiTopologia(idEquipo, ConstantesTopologiaElect.TiporelcodiTopologia);

            string hfStrEquiposTopologia = "";
            foreach (var item in model.ListaEquiposTopologia)
            {
                hfStrEquiposTopologia += item.Empresatopologia + "," + item.Equipotopologia + "," + item.Equicodi1 + "," + item.Equicodi2 + "," + item.Equirelagrup + "," + item.EquirelfecmodificacionDesc + "," + item.Equirelusumodificacion + "," + item.Equirelexcep + "#";
            }
            model.StrListaDatos = hfStrEquiposTopologia;

            model.ListaEmpresas = servicio.ListarEmpresasxTipoEquipos(ConstantesTopologiaElect.TipodeEquipos);

            model.ListaFamilia = servicio.ListarFamilia();
            model.ListaEquipos = this.servicio.ObtenerEquiposPorFamilia(-1, -1);
            model.ListaTipoExcepcion = IEODAppServicio.ObtenerListaTipoExcepcion();


            int Maxvaloragrupacion = 0;
            //var listaAux = servicio.ListarDetalleEquiTopologia(idEquipo, ConstantesTopologiaElect.TiporelcodiTopologia);
            if (model.ListaEquiposTopologia.Count > 0)
            {
                Maxvaloragrupacion = model.ListaEquiposTopologia.Max(x => x.Equirelagrup);
            }


            List<TipoDatoTopologia> entitys = new List<TipoDatoTopologia>();
            for (int i = 1; i <= Maxvaloragrupacion + 1; i++)
            {
                TipoDatoTopologia elemento = new TipoDatoTopologia() { Codi = i, DetName = i.ToString() };
                entitys.Add(elemento);
            }
            model.ListaAgrupacion = entitys;
            model.IdTipoExcepcion = 0;
            model.IdAgrupacion = 0;

            return PartialView(model);
        }

        /// <summary>
        /// Agregar Punto de Medicion en BD
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(List<EqEquirelDTO> data)
        {
            TopologiaElectModel model = new TopologiaElectModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                EqEquirelDTO ObjEquipo = new EqEquirelDTO();

                foreach (var obj in data)
                {
                    if (obj.OpCrud == 1) // nuevo equipo
                    {
                        ObjEquipo.Equicodi1 = obj.Equicodi1;
                        ObjEquipo.Tiporelcodi = ConstantesTopologiaElect.TiporelcodiTopologia; //topologia
                        ObjEquipo.Equicodi2 = obj.Equicodi2;
                        ObjEquipo.Equirelagrup = obj.Equirelagrup;
                        ObjEquipo.Equirelfecmodificacion = DateTime.Now;
                        ObjEquipo.Equirelusumodificacion = User.Identity.Name;
                        ObjEquipo.Equirelexcep = obj.Equirelexcep;

                        servicio.SaveEqEquiRelDTO(ObjEquipo);
                    }

                    if (obj.OpCrud == -1) // Eliminar
                    {
                        servicio.DeleteEqEquiRelDTO(obj.Equicodi1, obj.Tiporelcodi, obj.Equicodi2, User.Identity.Name);
                    }
                }
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

    }
}
