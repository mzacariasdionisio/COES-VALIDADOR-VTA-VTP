using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Migraciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Migraciones.Controllers
{
    public class ParametroController : BaseController
    {
        readonly MigracionesAppServicio servParam = new MigracionesAppServicio();
        readonly DespachoAppServicio servDespacho = new DespachoAppServicio();
        readonly FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();
        readonly SeguridadServicioClient servSeguridad = new SeguridadServicioClient();
        readonly EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        readonly GeneralAppServicio appGeneral = new GeneralAppServicio();

        private Int16 FUENTE_PARAMETROS = 1;

        #region Declaracion de variables

        private readonly int iTamanioPagina = 20;
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ParametroController));
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
        /// 
        /// </summary>
        /// <returns></returns>       
        public ActionResult Index(int? repcodi, int? grupocodi, int? agrupcodi, string fechaConsulta)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ParametroModel model = new ParametroModel();

            if (repcodi != null)
            {
                PrRepcvDTO oRepCv = servDespacho.GetByIdPrRepcv(repcodi.Value);
                if (oRepCv != null)
                {
                    model.RepCV = oRepCv;
                    fechaConsulta = oRepCv.Repfecha.ToString(Constantes.FormatoFecha);
                    switch (oRepCv.Reptipo.ToUpper())
                    {
                        case "D": oRepCv.ReptipoDesc = "Diario"; break;
                        case "S": oRepCv.ReptipoDesc = "Semanal"; break;

                    }
                }
            }
            model.IdAgrupacion = agrupcodi ?? 0;
            model.Fecha = fechaConsulta;
            model.Grupocodi = grupocodi ?? 0;

            model.ListaEmpresa = this.servFictec.ListarEmpresasxCategoria(this.GetCatecodiMop());


            model.ListaGrupo = this.servDespacho.ListPrGrupos();

            string catecodis = ConstantesMigraciones.CatecodiParametroFiltro;
            if (this.IdArea == ConstantesMigraciones.AreacodiDTI) catecodis += ",14";

            model.ListaCategoria = this.servFictec.ListarCategoriaGrupoXCatecodi(catecodis);

            model.ListaUbicacion = appEquipamiento.ListaTodasAreasActivas().OrderBy(x => x.Areaabrev).ThenBy(x => x.Areanomb).ToList();
            model.ListaFenerg = servParam.ListSiFuenteenergias();
            model.ListaEmpresaForm = this.appGeneral.ListadoComboEmpresasPorTipo(-2).Where(t => t.Emprestado.Trim() != "E").ToList();
            

            //eliminar los archivos temporales en reporte
            servParam.EliminarArchivosReporte();

            return View(model);
        }

        #region PrGrupo

        /// <summary>
        /// Lista de modos de operacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult GrupoListado(int emprcodi, string catecodi, string nombre, string estado, int nroPagina, int esReservaFria, int esNodoEnergetico)
        {
            ParametroModel model = new ParametroModel();
            model.AccesoEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.TienePermisoImportar = base.VerificarAccesoAccion(Acciones.Importar, base.UserName);// verificar rol de administrador

            catecodi = catecodi == ConstantesMigraciones.CatecodiAll ? this.GetCatecodiMop() : catecodi;
            model.ListaGrupo = this.servFictec.ListarGrupoPaginado(emprcodi, catecodi, nombre, estado, nroPagina, iTamanioPagina, DateTime.Today, esReservaFria, esNodoEnergetico);

            if (this.IdArea == ConstantesMigraciones.AreacodiDTI || this.IdArea == ConstantesMigraciones.AreacodiSGI)
            {
                var listaCatecodi = new List<int>() { 2, 6, 4, 3, 5 };
                foreach (var reg in model.ListaGrupo)
                {
                    reg.EsEditableOsinergcodi = listaCatecodi.Contains(reg.Catecodi);
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// total de elemento
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListGroupByEmprcodiAndCatecodi(int emprcodi, int catecodi)
        {
            var jsonResult = Json(this.servDespacho.ListPrGruposByEmprcodiAndCatecodi(emprcodi, catecodi));
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// total de elemento
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="catecodi"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult GrupoPaginado(int emprcodi, string catecodi, string nombre, string estado, int esReservaFria, int esNodoEnergetico)
        {
            ParametroModel model = new ParametroModel();
            model.IndicadorPagina = false;

            catecodi = catecodi == ConstantesMigraciones.CatecodiAll ? this.GetCatecodiMop() : catecodi;
            int nroRegistros = this.servFictec.TotalGrupo(emprcodi, catecodi, nombre, estado, DateTime.Now, esReservaFria, esNodoEnergetico);

            if (nroRegistros > iTamanioPagina)
            {
                int pageSize = iTamanioPagina;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = 10;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ListaEquipoTermoelectrico()
        {
            ParametroModel model = new ParametroModel();
            try
            {
                model.ListaEquipo = servParam.ListarEquipoTermoelectrico();
                model.ListaRelacionEquipo = servParam.ListPrGrupoeqs().Where(x => x.Geqactivo == 1).ToList();
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ObtenerPrGrupo(int grupocodi)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.Grupo = servParam.GetByIdPrGrupo(grupocodi);
                model.ListaRelacionEquipo = servParam.GetByCriteriaPrGrupoeqs(grupocodi).Where(x => x.Geqactivo == 1).ToList();

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult RegistrarPrGrupo(string stringJson, string equicodis)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                base.ValidarSesionJsonResult();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                PrGrupoDTO objGrupoWeb = stringJson != null ? serializer.Deserialize<PrGrupoDTO>(stringJson) : new PrGrupoDTO();

                List<int> listaEquicodis = new List<int>();
                if (!string.IsNullOrEmpty(equicodis)) listaEquicodis = equicodis.Split(',').Select(x => int.Parse(x)).ToList();

                //guardar en bd
                servParam.GuardarPrGrupo(objGrupoWeb, base.UserName, listaEquicodis);
                model.Resultado = "1";
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

        #endregion

        #region Parametros Grupos/Mop

        /// <summary>
        /// Parametros generales
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ParametrosGenerales(string fechaConsulta)
        {
            ParametroModel model = new ParametroModel();

            model.AccesoEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccesoNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);

            DateTime fecha = new DateTime();
            if (!string.IsNullOrEmpty(fechaConsulta))
            {
                fecha = DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                model.Fecha = fechaConsulta;
            }
            else
            {
                fecha = DateTime.Now;
                model.Fecha = fecha.ToString(Constantes.FormatoFecha);
            }
            model.FechaFull = fechaConsulta;

            return PartialView(model);
        }

        /// <summary>
        /// Parametros generales Tabla 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ParametrosGeneralesData(string fechaConsulta)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.AccesoEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
                model.AccesoNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);

                DateTime fecha = DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                var listaData = this.servDespacho.ParametrosGeneralesPorFecha(fecha);
                var usuario = this.servSeguridad.ObtenerUsuarioPorLogin(base.UserName);
                model.ListaParametros = new List<PrGrupodatDTO>();
                if (usuario != null)
                {
                    List<int> listaconcepcodiAll = this.servParam.ListarConcepcodiRegistrados();
                    List<int> listaconcepcodiUserActivo = this.servParam.ListarConcepcodiByUsuario(usuario.UserCode.ToString()).Where(x => x.Aconusactivo == ConstantesMigraciones.Activo).Select(x => x.Concepcodi).ToList();
                    List<int> listaconcepcodiUserInactivo = this.servParam.ListarConcepcodiByUsuario(usuario.UserCode.ToString()).Where(x => x.Aconusactivo == ConstantesMigraciones.Inactivo).Select(x => x.Concepcodi).ToList();

                    List<int> listaconcepcodiInactivo = listaconcepcodiAll.Where(x => !listaconcepcodiUserActivo.Contains(x)).ToList();

                    model.ListaParametros = listaData.Where(x => !listaconcepcodiInactivo.Contains(x.Concepcodi)).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return PartialView(model);
        }

        /// <summary>
        /// View Detalle de modo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult GrupoMopDetalle(int grupocodi, int? repcodi, int? idAgrup, string fechaConsulta)
        {
            ParametroModel model = new ParametroModel();
            model.AccesoEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.TienePermisoImportar = base.VerificarAccesoAccion(Acciones.Importar, base.UserName);// verificar rol

            DateTime fecha = new DateTime();
            if (repcodi != null)
            {
                PrRepcvDTO oRepCv = servDespacho.GetByIdPrRepcv(repcodi.Value);
                fecha = oRepCv.Repfecha;
                model.Fecha = fecha.ToString(Constantes.FormatoFecha);
            }
            else
            {
                fecha = !string.IsNullOrEmpty(fechaConsulta) ? DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now.Date;
                model.Fecha = fecha.ToString(Constantes.FormatoFecha);
            }

            model.IdAgrupacion = idAgrup ?? 0;
            model.ListaAgrupacion = this.servParam.GetByCriteriaPrAgrupacions(FUENTE_PARAMETROS, ConstantesAppServicio.Activo);
            model.Grupo = this.servFictec.GetByIdPrGrupo(grupocodi);
            model.ListaUnidad = this.servParam.ListaUnidadByGrupoYFecha(grupocodi, fecha);

            List<int> catecodis = ConstantesMigraciones.CatecodiParametroFiltro.Split(',').Select(int.Parse).ToList();

            model.HabilitaCargaMasiva = catecodis.Contains(model.Grupo.Catecodi) ? true : false;

            return PartialView(model);
        }

        /// <summary>
        /// Listar la data por modo
        /// </summary>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupoMopData(int grupocodi, string strfecha, string unidad, int idAgrup, string filtroFicha)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.AccesoEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
                model.AccesoNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);

                DateTime fecha = DateTime.ParseExact(strfecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                var usuario = this.servSeguridad.ObtenerUsuarioPorLogin(base.UserName);
                int usercode = usuario != null ? usuario.UserCode : 0;

                model.ListaGrupodat = servParam.ListarGrupodatGrupoOModoXFiltro(grupocodi, fecha, unidad, idAgrup, filtroFicha, usercode);

                //CONCEPPROPEQ = 1, flag de unidades especiales

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Listar historico de grupocodi y concepto
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarHistorico(int grupocodi, int concepcodi, int? equicodi = -1)
        {
            ParametroModel model = new ParametroModel();
            model.AccesoEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccesoNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);

            model.EquicodiFiltro = equicodi.Value;
            if (model.EquicodiFiltro > 0)
            {
                model.ListaGrupoEquipoVal = this.servParam.ListarGrupoEquipoValHistoricoValores(concepcodi, equicodi.Value, grupocodi);
            }
            else
            {
                model.ListaGrupodat = this.servParam.ListarGrupodatHistoricoValores(concepcodi, grupocodi);
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Registrar/editar grupodat
        /// </summary>
        /// <param name="tipoAccion"></param>
        /// <param name="grupocodi"></param>
        /// <param name="concepcodi"></param>
        /// <param name="strfechaDat"></param>
        /// <param name="formuladat"></param>
        /// <param name="deleted"></param>
        /// <param name="tipoParametro">S: Parametros Generales, N: grupomop</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupodatGuardar(int tipoAccion, int grupocodi, int concepcodi, string strfechaDat, string formuladat, int deleted, string tipoParametro, int gdatcheckcero, string gdatcomentario, string gdatsustento, int? repcodi)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaDat = DateTime.ParseExact(strfechaDat, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);

                if (repcodi != null)
                {
                    PrRepcvDTO oRepCv = servDespacho.GetByIdPrRepcv(repcodi.Value);
                    DateTime fecha = oRepCv.Repfecha.Date;

                    if (fechaDat.Date != fecha)
                    {
                        model.Resultado = "-1";
                        model.Mensaje = " La Fecha de Vigencia seleccionada no es válida";

                        return Json(model);
                    }
                }

                //Validaciones
                if (ConstantesAppServicio.NO == tipoParametro && grupocodi <= 0)
                {
                    throw new ArgumentException("Debe seleccionar un modo de operación.");
                }

                if (concepcodi <= 0)
                {
                    throw new ArgumentException("Debe seleccionar un parámetro.");
                }

                if (deleted != 0)
                {
                    throw new ArgumentException("El registro ya ha sido eliminado, no puede modificarse.");
                }

                //Guardar
                PrGrupodatDTO reg = new PrGrupodatDTO();
                reg.Grupocodi = grupocodi;
                reg.Formuladat = formuladat;
                reg.Gdatcheckcero = gdatcheckcero;
                reg.Gdatcomentario = gdatcomentario;
                reg.Gdatsustento = gdatsustento;
                reg.Concepcodi = concepcodi;
                reg.Fechadat = fechaDat;
                reg.Lastuser = base.UserName;
                reg.Fechaact = DateTime.Now;
                reg.Deleted = 0;
                this.servDespacho.ActualizarGrupodat(false, reg);

                model.Resultado = "1";
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
        /// Eliminar grupodat
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupodatEliminar(int grupocodi, int concepcodi, string strfechaDat)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaDat = DateTime.ParseExact(strfechaDat, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);

                PrGrupodatDTO reg = new PrGrupodatDTO();
                reg.Grupocodi = grupocodi;
                reg.Concepcodi = concepcodi;
                reg.Fechadat = fechaDat;
                reg.Lastuser = base.UserName;
                reg.Fechaact = DateTime.Now;
                reg.Deleted = 0;
                this.servDespacho.ActualizarGrupodat(true, reg);

                model.Resultado = "1";
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
        /// Registrar/editar GrupoEquipoVal
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupoEquipoValGuardar(int tipoAccion, int grupocodi, int equicodi, int concepcodi, string strfechaDat, string formuladat, int deleted, int gdatcheckcero, string gdatcomentario, string gdatsustento, int? repcodi)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaDat = DateTime.ParseExact(strfechaDat, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);

                if (repcodi != null)
                {
                    PrRepcvDTO oRepCv = servDespacho.GetByIdPrRepcv(repcodi.Value);
                    DateTime fecha = oRepCv.Repfecha.Date;

                    if (fechaDat.Date != fecha)
                    {
                        model.Resultado = "-1";
                        model.Mensaje = " La Fecha de Vigencia seleccionada no es válida";

                        return Json(model);
                    }
                }

                //Validaciones
                if (grupocodi <= 0)
                {
                    throw new ArgumentException("Debe seleccionar un modo de operación.");
                }
                if (equicodi <= 0)
                {
                    throw new ArgumentException("Debe seleccionar un equipo.");
                }

                if (concepcodi <= 0)
                {
                    throw new ArgumentException("Debe seleccionar un parámetro.");
                }

                if (deleted != 0)
                {
                    throw new ArgumentException("El registro ya ha sido eliminado, no puede modificarse.");
                }

                //Guardar
                if (tipoAccion == ConstantesMigraciones.AccionNuevo)
                {
                    PrGrupoEquipoValDTO reg = new PrGrupoEquipoValDTO();
                    reg.Grupocodi = grupocodi;
                    reg.Equicodi = equicodi;
                    reg.Greqvaformuladat = formuladat;
                    reg.Greqvacheckcero = gdatcheckcero;
                    reg.Greqvacomentario = gdatcomentario;
                    reg.Greqvasustento = gdatsustento;
                    reg.Concepcodi = concepcodi;
                    reg.Greqvafechadat = fechaDat;
                    reg.Greqvausucreacion = base.UserName;
                    reg.Greqvafeccreacion = DateTime.Now;
                    reg.Greqvadeleted = ConstantesMigraciones.GrupodatActivo;

                    this.servParam.SavePrGrupoEquipoVal(reg);
                }
                if (tipoAccion == ConstantesMigraciones.AccionEditar)
                {
                    PrGrupoEquipoValDTO reg = this.servParam.GetByIdPrGrupoEquipoVal(grupocodi, concepcodi, equicodi, fechaDat, ConstantesMigraciones.GrupodatActivo);
                    if (reg == null)
                    {
                        throw new ArgumentException("El registro no existe, no puede modificarse.");
                    }
                    reg.Greqvaformuladat = formuladat;
                    reg.Greqvacheckcero = gdatcheckcero;
                    reg.Greqvacomentario = gdatcomentario;
                    reg.Greqvasustento = gdatsustento;
                    reg.Greqvausumodificacion = base.UserName;
                    reg.Greqvafecmodificacion = DateTime.Now;

                    this.servParam.UpdatePrGrupoEquipoVal(reg);
                }

                model.Resultado = "1";
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
        /// Eliminar GrupoEquipoVal
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupoEquipoValEliminar(int grupocodi, int equicodi, int concepcodi, string strfechaDat, int deleted)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaDat = DateTime.ParseExact(strfechaDat, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);

                PrGrupoEquipoValDTO reg = this.servParam.GetByIdPrGrupoEquipoVal(grupocodi, concepcodi, equicodi, fechaDat, ConstantesMigraciones.GrupodatActivo);

                if (reg == null)
                {
                    throw new ArgumentException("El registro no existe o ha sido eliminada.");
                }

                reg.Greqvausumodificacion = base.UserName;
                reg.Greqvafecmodificacion = DateTime.Now;
                reg.Greqvadeleted2 = ConstantesMigraciones.GrupodatInactivo;

                this.servParam.UpdatePrGrupoEquipoVal(reg);

                model.Resultado = "1";
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

        #region FICHA TÉCNICA: EXPORTAR / IMPORTAR

        [HttpPost]
        public JsonResult ExportarReporteGrupo(int grupocodi, string strfecha, string unidad, int idAgrup, string filtroFicha, int? repcodi)
        {
            ParametroModel model = new ParametroModel();

            try
            {
                base.ValidarSesionJsonResult();
                var grupo = this.servFictec.GetByIdPrGrupo(grupocodi);

                #region PARAMETROS DATA

                DateTime fecha = DateTime.ParseExact(strfecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                var usuario = this.servSeguridad.ObtenerUsuarioPorLogin(base.UserName);
                int usercode = usuario != null ? usuario.UserCode : 0;

                model.ListaGrupodat = servParam.ListarGrupodatGrupoOModoXFiltro(grupocodi, fecha, unidad, idAgrup, filtroFicha, usercode);

                #endregion

                string fileName = ConstantesFichaTecnica.NombrePlantillaExcelParametros;
                string pathOrigen = ConstantesFichaTecnica.FolderRaizFichaTecnica + ConstantesFichaTecnica.Plantilla;
                string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                servParam.GenerarExcelPlantillaParametros(model.ListaGrupodat, grupo, pathDestino, fileName, repcodi);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
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
        /// Permite descargar el archivo Excel
        /// </summary>
        /// <param name="file">Nombre del archivo</param>
        /// <returns>Archivo</returns>
        public virtual FileResult AbrirArchivo(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes + file;

            string app = ConstantesFichaTecnica.AppExcel;

            return File(path, app, sFecha + "_" + file);
        }

        /// <summary>
        /// Carga del modulo de importación de parámetros de grupo
        /// </summary>
        /// <returns></returns>
        public ActionResult ParametrosMopMasivoImportacion(int? repcodi)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            ParametroModel model = new ParametroModel();
            model.Repcodi = repcodi ?? 0;

            return View(model);
        }

        [HttpPost]
        public ActionResult UploadParametro(string sFecha)
        {
            try
            {
                base.ValidarSesionUsuario();
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string sNombreArchivo = sFecha + "_" + file.FileName;

                    if (FileServer.VerificarExistenciaFile(null, sNombreArchivo, path))
                    {
                        //FileServer.DeleteBlob(sNombreArchivo, path + ConstantesEquipamientoAppServicio.Reportes);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult MostrarArchivoImportacion(string sFecha, string sFileName)
        {
            base.ValidarSesionUsuario();

            ParametroModel model = new ParametroModel();

            string fileName = sFecha + "_" + sFileName;
            model.ListaDocumentos = FileServer.ListarArhivos(null, AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes);
            var documento = new FileData();

            foreach (var item in model.ListaDocumentos)
            {
                if (String.Equals(item.FileName, fileName))
                {
                    model.Documento = new FileData();
                    model.Documento = item;
                    break;
                }
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ImportarParametrosMasivoExcel(string fileName)
        {
            ParametroModel model = new ParametroModel();
            model.ListaGrupoDatCorrectos = new List<PrGrupodatDTO>();
            model.ListaGrupoDatErrores = new List<PrGrupodatDTO>();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                // Ruta de los archivos EXCEL leidos
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;

                // Validar datos de Excel y realiza la importacion de los registros de este archivo           
                servParam.ValidarParametrosMasivoAImportar(path, fileName, base.UserName,
                                                       out List<PrGrupodatDTO> lstRegparametrosCorrectos,
                                                       out List<PrGrupodatDTO> lstRegParametrosErroneos,
                                                       out List<PrGrupodatDTO> listaNuevo,
                                                       out List<PrGrupodatDTO> listaModificado);

                model.ListaGrupoDatCorrectos = lstRegparametrosCorrectos;
                model.ListaGrupoDatErrores = lstRegParametrosErroneos;

                //validación si existen errores
                if (lstRegParametrosErroneos.Any())
                {
                    string filenameCSV = servParam.GenerarArchivoLogParametrosErroneosCSV(path, lstRegParametrosErroneos);
                    model.FileName = filenameCSV;

                    throw new Exception("¡No se guardó la información! Existen datos o registros que no permiten cargar el archivo completo.");
                }

                //validación si no tiene datos
                if (lstRegparametrosCorrectos.Count() == 0)
                {
                    throw new Exception("Por favor ingrese un documento con registros nuevos y/o actualizados.");
                }

                //Ejecución de la grabación de parámetros de un archivo Excel
                servParam.CargaMasivaParametrosMop(listaNuevo, listaModificado, base.UserName);

                model.Mensaje = "¡La Información se grabó correctamente!";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        /// <summary>
        /// Abrir archivo log de errores en formato CSV 
        /// </summary>
        /// <param name="file">Nombre del archivo</param>               
        /// <returns>Cadena del nombre del archivo</returns>
        public virtual ActionResult AbrirArchivoCSV(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes + file;

            string app = ConstantesFichaTecnica.AppCSV;

            // lo guarda el CSV en la carpeta de descarga
            return File(path, app, file);
        }

        /// <summary>
        /// Metodo para eliminar los archivos de parámetros importar
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <returns>Entero</returns>
        [HttpPost]
        public int EliminarArchivosImportacionNuevo(string nombreArchivo)
        {
            base.ValidarSesionUsuario();

            string nombreFile = string.Empty;

            ParametroModel modelArchivos = new ParametroModel();
            modelArchivos.ListaDocumentos = FileServer.ListarArhivos(null, AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes);
            foreach (var item in modelArchivos.ListaDocumentos)
            {
                string subString = item.FileName;
                if (subString == nombreArchivo)
                {
                    nombreFile = item.FileName;
                    break;
                }
            }

            if (FileServer.VerificarExistenciaFile(null, nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes))
            {
                FileServer.DeleteBlob(nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes);
            }

            return -1;
        }

        #endregion

        /// <summary>
        /// Descargar archivo de sustento de ficha técnica
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public JsonResult DescargarArchivoSustento()
        //{
        //    ParametroModel model = new ParametroModel();
        //    try
        //    {
        //        base.ValidarSesionJsonResult();

        //        string user = (new SeguridadServicio.SeguridadServicioClient()).EncriptarUsuario(this.UserName);

        //        model.Resultado = user;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(NameController, ex);
        //        model.Resultado = "-1";
        //        model.Mensaje = ex.Message;
        //        model.Detalle = ex.StackTrace;
        //    }

        //    return Json(model);
        //}

        #endregion

        #region Agrupación de Concepto

        /// <summary>
        /// Configuracion de los parametros (Agrupamientos y Visualizacion)
        /// </summary>
        /// <returns></returns>
        public ActionResult Agrupacion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ParametroModel model = new ParametroModel();
            model.ListaCategoria = this.servFictec.ListarCategoriaGrupoXCatecodi(this.GetCatecodiParametro());

            return View(model);
        }

        /// <summary>
        /// Lista de agrupacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AgrupacionLista(int agrupfuente)
        {
            if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

            ParametroModel model = new ParametroModel();

            model.ListaAgrupacion = this.servParam.GetByCriteriaPrAgrupacions(agrupfuente, ConstantesAppServicio.ParametroDefecto);

            return PartialView(model);
        }

        /// <summary>
        /// Obtener objeto modelo
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public JsonResult AgrupacionObjeto(int id)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                if (id > 0)
                {
                    model.Agrupacion = this.servParam.GetByIdPrAgrupacion(id);
                    model.ListaAgrupacionConcepto = this.servParam.GetByCriteriaPrAgrupacionConceptos(ConstantesMigraciones.Activo, id);
                }
                else
                {
                    model.Agrupacion = new PrAgrupacionDTO();
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
        /// Registrar/editar agrupacion
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgrupacionGuardar(int id, string nombre, List<int> listaSelec)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                //Validaciones
                if (nombre == null || nombre.Trim() == string.Empty)
                {
                    throw new Exception("Debe ingresar nombre.");
                }
                nombre = nombre.Trim();

                if (listaSelec == null || listaSelec.Count == 0)
                {
                    throw new Exception("Debe selecionar al menos un Concepto de Grupo");
                }

                var groupConcepto = listaSelec.GroupBy(x => x)
                     .Select(x => new { Concepcodi = x.Key, Count = x.Count() }).Where(x => x.Count > 1).ToList();

                if (groupConcepto.Count > 0)
                {
                    throw new Exception("No debe existir Concepto de Grupo repetidos dentro de una Agrupación");
                }

                DateTime fecha = DateTime.Now;
                string usuario = base.UserName;

                //Guardar
                if (id > 0)
                {
                    var listaAgrup = this.servParam.GetByCriteriaPrAgrupacions(FUENTE_PARAMETROS, ConstantesAppServicio.ParametroDefecto).Where(x => x.Agrupestado != ConstantesAppServicio.Baja && x.Agrupcodi != id && x.Agrupnombre.ToUpper().Trim() == nombre).ToList();
                    if (listaAgrup.Count > 0)
                    {
                        throw new Exception("El nombre de la Agrupación ya existe.");
                    }

                    PrAgrupacionDTO reg = this.servParam.GetByIdPrAgrupacion(id);
                    reg.Agrupnombre = nombre;
                    reg.Agrupfecmodificacion = fecha;
                    reg.Agrupusumodificacion = usuario;

                    this.servParam.UpdatePrAgrupacion(reg);
                }
                else
                {
                    var listaAgrup = this.servParam.GetByCriteriaPrAgrupacions(FUENTE_PARAMETROS, ConstantesAppServicio.ParametroDefecto).Where(x => x.Agrupestado != ConstantesAppServicio.Baja && x.Agrupnombre.ToUpper().Trim() == nombre.ToUpper().Trim()).ToList();
                    if (listaAgrup.Count > 0)
                    {
                        throw new Exception("El nombre de la Agrupación ya existe.");
                    }

                    PrAgrupacionDTO reg = new PrAgrupacionDTO();
                    reg.Agrupnombre = nombre;
                    reg.Agrupfeccreacion = fecha;
                    reg.Agrupusucreacion = usuario;
                    reg.Agrupestado = ConstantesAppServicio.Activo;
                    reg.Agrupfuente = FUENTE_PARAMETROS;
                    id = this.servParam.SavePrAgrupacion(reg);
                }

                this.servParam.GuardarAgrupacion(id, listaSelec, usuario, fecha);

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
        /// Eliminar agrupacion
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgrupacionEliminar(int id)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                PrAgrupacionDTO reg = this.servParam.GetByIdPrAgrupacion(id);

                if (reg == null)
                {
                    throw new Exception("La Agrupación no existe o ha sido eliminada.");
                }

                reg.Agrupfecmodificacion = DateTime.Now;
                reg.Agrupusumodificacion = base.UserName;
                this.servParam.DeletePrAgrupacion(reg);

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
        /// Listar los conceptos de una categoria de grupo
        /// </summary>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaPrConcepto(int catecodi)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                string catecodis = catecodi > 0 ? catecodi.ToString() : this.GetCatecodiParametro();

                model.ListaConcepto = this.servFictec.ListPrConceptoByCatecodi(catecodis, false);

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

        #endregion

        #region Visualizacion de Concepto

        /// <summary>
        /// visualizacion
        /// </summary>
        /// <returns></returns>
        public ActionResult Visualizacion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            List<int> areasCoes = ConstantesMigraciones.AreacoesParaVisualizacion.Split(',').Select(x => int.Parse(x)).ToList();

            ParametroModel model = new ParametroModel();
            model.ListaCategoria = this.servFictec.ListarCategoriaGrupoXCatecodi(ConstantesMigraciones.CatecodiParametroVisualizacion);
            model.ListaArea = this.servParam.ListarAreaByListacodi(areasCoes);
            model.ListaUsuario = this.ListarUsuarioFromServicio(areasCoes, model.ListaArea);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            StringBuilder json = new StringBuilder();
            serializer.Serialize(model.ListaUsuario, json);
            model.ListaUsuarioJson = json.ToString();

            StringBuilder json2 = new StringBuilder();
            serializer.Serialize(model.ListaArea, json2);
            model.ListaAreaJson = json2.ToString();

            return View(model);
        }

        /// <summary>
        /// Listar conceptos
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult VisualizacionConcepto(int catecodi)
        {
            ParametroModel model = new ParametroModel();
            model.AccesoEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccesoNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);

            string catecodis = catecodi > 0 ? catecodi.ToString() : ConstantesMigraciones.CatecodiParametroVisualizacion;

            model.ListaConcepto = this.servParam.ListConceptoYConfiguracionVisualizacion(catecodis);

            return PartialView(model);
        }

        /// <summary>
        /// Listar los usuarios x area del concepto
        /// </summary>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaConfiguracionXConcepto(int concepcodi)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                model.ListaAreaConcepto = this.servParam.GetByCriteriaPrAreaConceptos(concepcodi, ConstantesMigraciones.Activo.ToString());
                model.ListaUsuariocodi = this.servParam.GetByCriteriaPrAreaConcepUsers(concepcodi, ConstantesMigraciones.Activo.ToString(), ConstantesMigraciones.Activo.ToString()).Select(x => x.Usercode).ToList();

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
        /// Registrar/editar grupodat
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VisualizacionConceptoGuardar(int concepcodi, List<int> listaUsuarios)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                //Validaciones
                if (concepcodi <= 0)
                {
                    throw new Exception("Debe seleccionar un parámetro.");
                }

                listaUsuarios = listaUsuarios == null ? new List<int>() : listaUsuarios;

                List<int> areasCoes = ConstantesMigraciones.AreacoesParaVisualizacion.Split(',').Select(x => int.Parse(x)).ToList();
                var listaArea = this.servParam.ListarAreaByListacodi(areasCoes);
                var listaUsuario = this.ListarUsuarioFromServicio(areasCoes, listaArea);

                this.servParam.GuardarConfiguracionAreaConceptoUser(concepcodi, listaUsuarios, base.UserName, listaUsuario);

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

        #endregion

        /// <summary>
        /// Listar usuarios del servicio
        /// </summary>
        /// <param name="areasCoes"></param>
        /// <param name="listaArea"></param>
        /// <returns></returns>
        private List<UsuarioParametro> ListarUsuarioFromServicio(List<int> areasCoes, List<FwAreaDTO> listaArea)
        {
            List<UsuarioParametro> listaUsuario = new List<UsuarioParametro>();
            var arrayUsuario = this.servSeguridad.ListarUsuarios();
            if (arrayUsuario != null)
            {

                listaUsuario = arrayUsuario.Select(x => this.ConvertirUsuarioServicio(x, listaArea)).ToList();
                listaUsuario = listaUsuario.Where(x => areasCoes.Contains(x.AreaCode)).ToList();
            }

            return listaUsuario.OrderBy(x => x.AreaName).ThenBy(x => x.UsernName).ToList();
        }

        /// <summary>
        /// Convertir objetos de Usuario
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listaArea"></param>
        /// <returns></returns>
        private UsuarioParametro ConvertirUsuarioServicio(UserDTO user, List<FwAreaDTO> listaArea)
        {
            UsuarioParametro u = new UsuarioParametro();
            u.UserCode = user.UserCode;
            u.UsernName = user.UsernName;
            u.AreaCode = user.AreaCode.GetValueOrDefault(0);

            var area = listaArea.Find(x => x.Areacode == u.AreaCode);
            u.AreaName = area != null ? area.Areaname : string.Empty;
            u.AreaAbrev = area != null ? area.Areaabrev : string.Empty;

            return u;
        }

        private string GetCatecodiMop()
        {
            string catecodis = ConstantesMigraciones.CatecodiParametroFiltro;
            if (this.IdArea == ConstantesMigraciones.AreacodiDTI) catecodis += ",14";
            return catecodis;
        }

        private string GetCatecodiParametro()
        {
            string catecodis = ConstantesMigraciones.CatecodiParametro;
            if (this.IdArea == ConstantesMigraciones.AreacodiDTI) catecodis += ",14";
            return catecodis;
        }

        #region Reporte de Control de Cambios

        /// <summary>
        /// Reporte de Control de Cambios
        /// </summary>
        /// <returns></returns>
        public ActionResult ReporteControlCambios()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ParametroModel model = new ParametroModel();
            DateTime fecha = DateTime.Now.Date;

            model.Fecha = fecha.ToString(Constantes.FormatoFecha);
            model.ListaAgrupacion = this.servParam.GetByCriteriaPrAgrupacions(FUENTE_PARAMETROS, ConstantesAppServicio.Activo);
            model.ListaCategoria = this.servFictec.ListarCategoriaGrupoXCatecodi(ConstantesMigraciones.CatecodiControlCambio);

            return View(model);
        }

        /// <summary>
        /// Genera  View de Listado de Reporte
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="sIdTipoOperacion"></param>
        /// <param name="sFechaIni"></param>
        /// <param name="sFechaFin"></param>
        /// <param name="idTipoCentral"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarReporteControlCambios(string strFecha, int idAgrup, int catecodi)
        {
            if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

            ParametroModel model = new ParametroModel();

            DateTime fecha = DateTime.ParseExact(strFecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            model.Resultado = this.servParam.ListarReporteControlCambiosHtml(fecha, idAgrup, catecodi);

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        /// <summary>
        /// Exportar reporte
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarReporteControlCambios(string strFecha)
        {
            string ruta = string.Empty;
            string[] datos = new string[2];
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                DateTime fecha = DateTime.ParseExact(strFecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                ruta = this.servParam.GenerarFileExcelReporteControlCambio(fecha);
                datos[0] = ruta;
                datos[1] = ConstantesMigraciones.RptControlCambios;

                var jsonResult = Json(datos);
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        /// <summary>
        /// Permite descargar el archivo del reporte
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcelReporte()
        {
            string strArchivoTemporal = Request["archivo"];
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        #endregion

        #region Ficha tecnica 2023

        /// <summary>
        /// Devuelve la informacion de relaciones de proyectos
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosAsignacion(int grupocodi)
        {
            //IndexEquipoModel model = new IndexEquipoModel();
            ParametroModel model = new ParametroModel();
            

            try
            {
                string usuario = base.UserName;
                //usuario = "JPEREZ2";
                base.ValidarSesionJsonResult();

                model.Grupo = servParam.GetDatosGrupo(grupocodi);
                model.ListaProyectosGrupo = servParam.ObtenerListadoProyectosPorGrupo(grupocodi);
                //model.ListadoEmpresasCopropietarias = appEquipamiento.ObtenerListadoEmpresasCopropietarias(equicodi);

                //FichaTecnicaAppServicio servFT = new FichaTecnicaAppServicio();
                //model.ListaEmpresas = servFT.ListarEmpresasActivas();

                model.Resultado = "1";
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
        /// Devuelve todos los proyectos activos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerProyectosExistentes()
        {
            ParametroModel model = new ParametroModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListadoProyectos = appEquipamiento.ListarProyectosExistentes();
                model.Resultado = "1";
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
        /// Obtiene los datos del proyecto seleccionado
        /// </summary>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosProySel(int ftprycodi)
        {
            ParametroModel model = new ParametroModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.Proyecto = appEquipamiento.ObtenerDatosProyecto(ftprycodi);
                model.Resultado = "1";
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
        /// Devuelve los datos de una relacion Equipo-Proyecto
        /// </summary>
        /// <param name="ftreqpcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosPEAuditoria(int ftreqpcodi)
        {
            ParametroModel model = new ParametroModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.RelEquipoProyecto = appEquipamiento.ObtenerDatosRelEquipoProyecto(ftreqpcodi);
                model.Resultado = "1";
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
        /// Guarda los datos de la asignacion
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="strCambiosPE"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarInfoAsignacion(int grupocodi, string strCambiosPE)
        {
            ParametroModel model = new ParametroModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                servParam.GuardarDatosAsignacionFT(grupocodi, strCambiosPE, base.UserName);
                appEquipamiento.ActualizarCambiosEnAsignacionDeProyectos(strCambiosPE, base.UserName);
                model.Resultado = "1";
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

        #endregion
    }
}
