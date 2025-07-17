using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.IEOD.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.IEOD.Controllers
{
    public class EquiposSEINController : FormatoController
    {
        //
        // GET: /IEOD/EquiposSEIN/
        private IEODAppServicio logic = new IEODAppServicio();
        private GeneralAppServicio servGeneral = new GeneralAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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
        /// Index de inicio de controller
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //if (!base.IsValidSesion) return base.RedirectToLogin();
            //if (this.IdModulo == null) return base.RedirectToHomeDefault();

            EquiposSEINModel model = new EquiposSEINModel();
            model.ListaEmpresas = this.ListarEmpresaSEIN(Acciones.AccesoEmpresa, User.Identity.Name);
            model.ListaFamilia = logic.ListarFamilia();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial para mostrar listado de los Equipos en SEIN
        /// </summary>
        /// <param name="empresas"></param>       
        /// <param name="tipoEquipo"></param>
        /// <param name="nroPagina"></param>
        /// <param name="idsFamilia"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string sEmpresa, string fecha, string sFamilia, int nroPagina, string orden)
        {
            EquiposSEINModel model = new EquiposSEINModel();
            model.ListaEquipoSEIN = logic.ListarDetalleEquiposSEIN(sEmpresa, nroPagina, Constantes.PageSize, sFamilia, fecha, orden);
            model.OpcionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            return PartialView(model);
        }

        /// <summary>
        /// Genera la vista para registrar datos de Equipos recien incorporados al SEIN, repotenciados y/o reubicados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoCentral"></param>
        /// <param name="idCentral"></param>
        /// <param name="opcion"></param> opcion 0: modificar, 1: nueva
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        public PartialViewResult IngresoEquipoSEIN()
        {
            BusquedaModel model = new BusquedaModel();
            model.ListaEmpresas = this.ListarEmpresaSEIN(Acciones.AccesoEmpresa, User.Identity.Name);

            int idCausa = ConstantesIEOD.IdCausaEvento;
            string SubcausaCmg = ConstantesIEOD.SubcausaCmgEXT;
            model.ListaMotivo = logic.ListarSubCausas(idCausa, SubcausaCmg).Where(x => x.SubcausaCmg == "EXT").ToList();

            model.ListaFamilia = logic.ListarFamiliaXEmp((int)model.ListaEmpresas[0].Emprcodi);
            model.ListaEquipo = logic.ObtenerEquiposPorFamilia(0, 0);
            model.ListaAreas = logic.ObtenerAreasXEmpresa((int)model.ListaEmpresas[0].Emprcodi);

            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);

            return PartialView(model);
        }

        /// <summary>
        /// devuelve listado de equipo para filtro equipo
        /// </summary>
        /// <param name="idEmpresa"></param>             
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFamilias(int idEmpresa)
        {
            List<EqFamiliaDTO> entitys = new List<EqFamiliaDTO>();
            entitys = logic.ListarFamiliaXEmp(idEmpresa);
            SelectList list = new SelectList(entitys, "FamCodi", "FamNomb");

            return Json(list);
        }

        /// <summary>
        /// devuelve listado de equipo para filtro equipo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEquipos(int idEmpresa, int idFamilia)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            entitys = this.logic.ObtenerEquiposPorFamilia(idEmpresa, idFamilia);
            SelectList list = new SelectList(entitys, "EquiCodi", "EquiNomb");

            return Json(list);
        }

        /// <summary>
        /// devuelve listado de areas para filtro empresa
        /// </summary>
        /// <param name="idEmpresa"></param>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarAreas(int idEmpresa)
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            entitys = this.logic.ObtenerAreasXEmpresa(idEmpresa);
            SelectList list = new SelectList(entitys, "Areacodi", "AreaNomb");

            return Json(list);
        }

        /// <summary>
        /// Graba el registro en la base de datos
        /// </summary>
        [HttpPost]
        public JsonResult GrabarEquipo(int idempresa, int idfamilia, int idequipo, int idmotivo, string ifecha)
        {
            EquiposSEINModel model = new EquiposSEINModel();

            try
            {
                base.ValidarSesionJsonResult();

                EveEventoEquipoDTO obj = new EveEventoEquipoDTO();
                obj.Emprcodi = idempresa;
                obj.Equicodi = idequipo;
                obj.Subcausacodi = idmotivo;
                obj.Eeqestado = 1;
                obj.Eeqfechaini = DateTime.ParseExact(ifecha, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);

                this.logic.SaveEveEventoEquipo(obj);

                model.Resultado = obj.Eeqfechaini.ToString(ConstantesAppServicio.FormatoFecha);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Listar empresas del usuario
        /// </summary>
        /// <returns></returns>
        private List<SiEmpresaDTO> ListarEmpresaSEIN(int acceso, string usuario)
        {

            List<SiEmpresaDTO> listaEmpresas = new List<SiEmpresaDTO>();
            bool accesoEmpresa = base.VerificarAccesoAccion(acceso, usuario);
            List<SiEmpresaDTO> empresas = this.servGeneral.ObtenerEmpresasCOES();
            if (accesoEmpresa)
            {
                if (empresas.Count > 0)
                    listaEmpresas = empresas;
                else
                {
                    listaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                }
            }
            else
            {
                var emprUsuario = base.ListaEmpresas.Where(x => empresas.Any(y => x.EMPRCODI == y.Emprcodi)).
                    Select(x => new SiEmpresaDTO()
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB
                    });
                if (emprUsuario.Count() > 0)
                {
                    listaEmpresas = emprUsuario.ToList();

                }
                else
                {
                    listaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                }
            }

            return listaEmpresas.OrderBy(x => x.Emprnomb).ToList();
        }

    }
}
