using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.IEOD;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    public class EquiposSEINController : BaseController
    {
        //
        // GET: /IEOD/EquiposSEIN/
        IEODAppServicio logic = new IEODAppServicio();
        GeneralAppServicio logicGeneral = new GeneralAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EquiposSEINController));
        private static string NameController = "EquiposSEINController";
        private static List<EstadoModel> ListaEstadoSistemaA = new List<EstadoModel>();

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

        public EquiposSEINController()
        {
            ListaEstadoSistemaA = new List<EstadoModel>();
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "0", EstadoDescripcion = "NO" });
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "SÍ" });
        }

        /// <summary>
        /// Index de inicio de controller Ampliacion
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            EquiposSEINModel model = new EquiposSEINModel();

            int idModulo = ConstantesIEOD.IdModulo;
            int idOrigen = ConstantesIEOD.IdOrigenIEOD;

            //model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();

            model.ListaFamilia = logic.ListarFamilia();
            model.ListaTipoEmpresa =
                 this.logic.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).OrderBy(t => t.Tipoemprdesc).ToList();

            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            List<int> listaFormatCodi = new List<int>();
            List<int> listaFormatPeriodo = new List<int>();
            /** foreach (var reg in model.ListaFormato)
             {
                 listaFormatCodi.Add(reg.Formatcodi);
                 listaFormatPeriodo.Add((int)reg.Formatperiodo);
             }*/
            model.Anho = DateTime.Now.Year.ToString();
            model.StrFormatCodi = String.Join(",", listaFormatCodi);
            model.StrFormatPeriodo = String.Join(",", listaFormatPeriodo);

            return View(model);
        }

        /// <summary>
        /// Obtiene las empresas segun tipo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarEmpresas(int idTipoempresa)
        {
            EquiposSEINModel model = new EquiposSEINModel();
            model.ListaEmpresas = logic.GetListaCriteria(Convert.ToString(idTipoempresa));
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
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();
            int idCausa = ConstantesIEOD.IdCausaEvento;
            string SubcausaCmg = ConstantesIEOD.SubcausaCmgINT;
            List<EqAreaDTO> temp = new List<EqAreaDTO>();
            EqAreaDTO tmp = new EqAreaDTO();

            List<EveSubcausaeventoDTO> subtemp = new List<EveSubcausaeventoDTO>();
            EveSubcausaeventoDTO subtmp = new EveSubcausaeventoDTO();


            model.ListaFamilia = logic.ListarFamiliaXEmp((int)model.ListaEmpresas[0].Emprcodi);
            model.ListaEquipo = logic.ObtenerEquiposPorFamilia(0, 0);

            subtmp.Subcausacodi = 0;
            subtmp.Subcausadesc = "--SELECCIONE--";
            subtemp.Add(subtmp);

            model.ListaMotivo = subtemp;
            model.ListaMotivo.AddRange(logic.ListarSubCausas(idCausa, SubcausaCmg));




            tmp.Areacodi = 0;
            tmp.Areanomb = "--SELECCIONE--";
            temp.Add(tmp);

            model.ListaAreas = temp;

            model.ListaAreas.AddRange(logic.ObtenerAreasXEmpresa((int)model.ListaEmpresas[0].Emprcodi));

            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaPlazo = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.HoraPlazo = DateTime.Now.Hour * 2 + 1;
            model.AnhoMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            /* model.ListaSemanas = HelperIEOD.GetListaSemana(DateTime.Now.Year);*/
            model.NroSemana = EPDate.f_numerosemana(DateTime.Now);
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
            SelectList list = new SelectList(entitys, EntidadPropiedad.FamCodi, EntidadPropiedad.FamNomb);

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
            SelectList list = new SelectList(entitys, EntidadPropiedad.EquiCodi, EntidadPropiedad.EquiNomb);

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
            SelectList list = new SelectList(entitys, EntidadPropiedad.Areacodi, EntidadPropiedad.AreaNomb);

            return Json(list);
        }

        /// <summary>
        /// Lista de Semana por Año
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        /*[HttpPost]
        public JsonResult CargarSemanas(string idAnho)
        {
            BusquedaModel model = new BusquedaModel();
            List<TipoInformacion> entitys = HelperIEOD.GetListaSemana(Int32.Parse(idAnho));
            SelectList list = new SelectList(entitys, "IdTipoInfo", "NombreTipoInfo");
            return Json(list);
        }*/

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
            model.ListaEquipoSEIN = logic.ListarDetalleEquiposSEIN(sEmpresa, nroPagina, Constantes.PageSize, sFamilia,
                 fecha, orden);

            /* bool flagEditar = Tools.VerificarAcceso(base.IdOpcion, base.UserName, Acciones.Editar);
             model.OpcionEditar = flagEditar;
             model.OpcionEspecial = base.VerificarAccesoAccion(Acciones.Adicional, base.UserName);*/

            return PartialView(model);
        }

        /// <summary>
        /// Graba el registro en la base de datos
        /// </summary>
        [HttpPost]
        public JsonResult GrabarEquipo(int idempresa, int idfamilia, int idequipo, int idmotivo, string ifecha, int idUbicacion)
        {
            DateTime f_ = DateTime.Now;
            int resultado = -1;
            string date = "";
            string dateA = "";

            try
            {
                EveEventoEquipoDTO obj = new EveEventoEquipoDTO();
                obj.Eeqcodi = 0;
                obj.Emprcodi = idempresa;
                obj.Equicodi = idequipo;
                obj.Subcausacodi = idmotivo;
                obj.Eeqestado = 1;

                //date = Convert.ToString(DateTime.Now);
                //date = date.Substring(10, (date.Length - 10));
                //dateA = ifecha + date;
                f_ = DateTime.ParseExact(ifecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                obj.Eeqfechaini = new DateTime(f_.Year, f_.Month, f_.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);//Convert.ToDateTime(ifecha + date);

                int codigo = this.logic.SaveEveEventoEquipo(obj);

                int resul = AprobarProceso(codigo, idempresa, idfamilia, idequipo, idmotivo, dateA, idUbicacion, 2);

                resultado = 1;
            }
            catch (Exception ex)
            {
                resultado = -1;
            }

            return Json(resultado);
        }

        /// <summary>
        /// Aprueba los procesos y almacena en la base de datos
        /// </summary>
        public int AprobarProceso(int codigo, int idempresa, int idfamilia, int idequipo, int idmotivo, string ifecha, int idUbicacion, int estado)
        {
            int resultado = -1;
            string date = "";
            string motivoabrev = "";
            List<EveSubcausaeventoDTO> subcausa;

            try
            {
                EveEventoEquipoDTO obj = new EveEventoEquipoDTO();

                subcausa = logic.ListarSubCausasXid(idmotivo);

                obj.Eeqcodi = codigo;
                obj.Eeqestado = 2; /* 1:Pendiente, 2: Atentido, 3: Anulado*/

                obj.Subcausadesc = subcausa[0].Subcausaabrev;
                motivoabrev = subcausa[0].Subcausaabrev;

                /*this.logic.AproEveEventoEquipo(obj);*/

                string usuario = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                this.logic.AprobEveEventoEquipo(codigo, idempresa, idfamilia, idequipo, estado, idmotivo, motivoabrev, ifecha, idUbicacion, usuario);



                resultado = 1;
            }
            catch (Exception ex)
            {

                resultado = -1;
            }

            return resultado;
        }

        /// <summary>
        /// Index de inicio de controller Ampliacion
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidarSolicitudes()
        {
            EquiposSEINModel model = new EquiposSEINModel();

            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();

            model.ListaFamilia = logic.ListarFamilia();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);

            model.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);
            model.Anho = DateTime.Now.Year.ToString();

            if (Session["Fecha_Ini"] != null && Session["Fecha_Fin"] != null)
            {
                model.Fecha = (string)Session["Fecha_Ini"];
                model.FechaFin = (string)Session["Fecha_Fin"];
                Session["Fecha_Ini"] = null;
                Session["Fecha_Fin"] = null;
            }


            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial para mostrar listado de los Equipos en SEIN a aprobar
        /// </summary>
        /// <param name="empresas"></param>       
        /// <param name="tipoEquipo"></param>
        /// <param name="nroPagina"></param>
        /// <param name="idsFamilia"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaVal(string sEmpresa, string fecha, string fechaFin, string sFamilia, int nroPagina, string orden)
        {
            Session["Fecha_Ini"] = fecha;
            Session["Fecha_Fin"] = fechaFin;
            EquiposSEINModel model = new EquiposSEINModel();
            model.ListaEquipoSEIN = logic.ListarPendientesEquiposSEIN(sEmpresa, nroPagina, Constantes.PageSize, sFamilia,
                 fecha, fechaFin, orden);


            return PartialView(model);
        }
    }
}