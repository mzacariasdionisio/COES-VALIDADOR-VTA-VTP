using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTReporteHistoricoController : BaseController
    {
        readonly MigracionesAppServicio servParam = new MigracionesAppServicio();

        private Int16 FUENTE_FICHA_TECNICA = 2;

        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();
        IEODAppServicio servIeod = new IEODAppServicio();
        private EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        private DespachoAppServicio appDespacho = new DespachoAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        #endregion

        #region Configurar agrupación de parámetros

        public ActionResult IndexAgrupacionParametro()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FichaTecnicaModel model = new FichaTecnicaModel();

            model.ListaFamilia = this.servIeod.ListarFamilia();
            model.ListaCategoria = this.servFictec.ListarCategoriaGrupoXCatecodi(ConstantesMigraciones.CatecodiParametroFiltro);

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

            FichaTecnicaModel model = new FichaTecnicaModel();

            model.ListaAgrupacion = this.servParam.GetByCriteriaPrAgrupacions(agrupfuente, ConstantesAppServicio.ParametroDefecto);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ListaEqPropiedad(int? famcodi)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (famcodi.GetValueOrDefault(0) <= -2)
                {
                    throw new Exception("Debe seleccionar un Tipo de Equipo.");
                }

                model.ListaPropiedad = this.servFictec.ListEqPropiedadByFamcodi(famcodi.Value, false);

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
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                string catecodis = catecodi > 0 ? catecodi.ToString() : ConstantesMigraciones.CatecodiParametroFiltro;

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

        /// <summary>
        /// Registrar/editar agrupacion
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgrupacionGuardar(int id, string nombre, List<int> listaSelec, List<int> listaOrigen)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
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
                    throw new Exception("Debe selecionar al menos un Concepto de Grupo o Propiedad de equipo.");
                }

                List<int> listaConcepcodi = new List<int>(), listaPropcodi = new List<int>();

                for (int indice = 0; indice < listaOrigen.Count; indice++)
                {
                    int codigo = listaSelec[indice];

                    if (listaOrigen[indice] == 2) listaPropcodi.Add(codigo);
                    else listaConcepcodi.Add(codigo);
                }

                //validacion duplicado
                listaConcepcodi = listaConcepcodi.Distinct().ToList();
                listaPropcodi = listaPropcodi.Distinct().ToList();

                DateTime fecha = DateTime.Now;
                string usuario = base.UserName;

                //Guardar
                if (id > 0)
                {
                    var listaAgrup = this.servParam.GetByCriteriaPrAgrupacions(FUENTE_FICHA_TECNICA, ConstantesAppServicio.ParametroDefecto).Where(x => x.Agrupestado != ConstantesAppServicio.Baja && x.Agrupcodi != id && x.Agrupnombre.ToUpper().Trim() == nombre).ToList();
                    if (listaAgrup.Count > 0)
                    {
                        throw new Exception("El nombre de la Agrupación ya existe.");
                    }

                    PrAgrupacionDTO reg = this.servParam.GetByIdPrAgrupacion(id);
                    reg.Agrupnombre = nombre;
                    reg.Agrupfecmodificacion = fecha;
                    reg.Agrupusumodificacion = usuario;
                    reg.Agrupfuente = FUENTE_FICHA_TECNICA;

                    this.servParam.UpdatePrAgrupacion(reg);
                }
                else
                {
                    var listaAgrup = this.servParam.GetByCriteriaPrAgrupacions(FUENTE_FICHA_TECNICA, ConstantesAppServicio.ParametroDefecto).Where(x => x.Agrupestado != ConstantesAppServicio.Baja && x.Agrupnombre.ToUpper().Trim() == nombre.ToUpper().Trim()).ToList();
                    if (listaAgrup.Count > 0)
                    {
                        throw new Exception("El nombre de la Agrupación ya existe.");
                    }

                    PrAgrupacionDTO reg = new PrAgrupacionDTO();
                    reg.Agrupnombre = nombre;
                    reg.Agrupfeccreacion = fecha;
                    reg.Agrupusucreacion = usuario;
                    reg.Agrupestado = ConstantesAppServicio.Activo;
                    reg.Agrupfuente = FUENTE_FICHA_TECNICA;

                    id = this.servParam.SavePrAgrupacion(reg);
                }

                this.servParam.GuardarAgrupacion(id, listaConcepcodi, listaPropcodi, usuario, fecha);

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
            FichaTecnicaModel model = new FichaTecnicaModel();
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
        /// Obtener objeto modelo
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public JsonResult AgrupacionObjeto(int id)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
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

        #endregion

        #region Reportes Ficha Técnica

        public ActionResult IndexHistoricoFT()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FichaTecnicaModel model = new FichaTecnicaModel();

            List<FtFichaTecnicaDTO> listaFichaMaestra = this.servFictec.ListarFichaMaestra(ConstantesAppServicio.ParametroDefecto)
                                                        .OrderByDescending(x => x.Ftecprincipal).ThenBy(x => x.Ftecambiente).ThenBy(x => x.Ftecestado).ToList();

            foreach (var obj in listaFichaMaestra)
            {
                if (obj.Ftecprincipal == 1 && obj.Ftecambiente == ConstantesFichaTecnica.FichaMaestraPortal) obj.Ftecnombre += " (OFICIAL)";
            }

            model.ListaFichaMaestra = listaFichaMaestra;

            model.ListaFichaTecnicaSelec = this.servFictec.ListarAllFichaTecnicaByMaestra(model.ListaFichaMaestra[0].Fteccodi);

            model.ListaEmpresa = this.servFictec.ListarEmpresaByFichaTecnica(model.ListaFichaTecnicaSelec[0].Fteqcodi);

            model.ListaEtapas = servFictec.ListFtExtEtapas();

            DateTime hoy = DateTime.Now;
            model.FechaInicio = (hoy.AddYears(-1)).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = (hoy).ToString(ConstantesAppServicio.FormatoFecha);

            // verificar si tiene permisos administrador de ficha
            model.TienePermisoAdmin = this.TieneRolAdministradorFicha() ? 1 : 0;

            return View(model);
        }

        /// <summary>
        /// Listar Empresas por Ficha Tecnica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaFichaTecnicaXMaestra(int idFTmaestra)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                model.ListaFichaTecnicaSelec = this.servFictec.ListarAllFichaTecnicaByMaestra(idFTmaestra);
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
        /// Listar Empresas por Ficha Tecnica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaEmpresaXFichaTecnica(int idFT)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                var fichaTecnica = servFictec.GetFichaTecnica(idFT);
                model.Origen = fichaTecnica.Origen;
                model.TipoElementoId = model.Origen == ConstantesFichaTecnica.OrigenTipoEquipo ? fichaTecnica.Famcodi.Value : -2;

                model.ListaEmpresa = this.servFictec.ListarEmpresaByFichaTecnicaTTIE(idFT);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Lista de Elementos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ElementosListado(int idEtapa, string idFicha, string iEmpresa, string tipocodis,
                                            bool historico, bool flagFormulaEnValor, string fecha, string fechafin, string iEstado)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                DateTime fechaVigIni = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaVigFin = DateTime.ParseExact(fechafin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaElemento = this.servFictec.ListarElementoReporteHistoricoFT(idEtapa, idFicha, iEmpresa, tipocodis,
                                                                    historico, flagFormulaEnValor, fechaVigIni, fechaVigFin, iEstado, false, false).ListaElementoWeb;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// Exportación de Excel Masivo por Ficha
        /// </summary>
        /// <param name="sFechaIni"></param>
        /// <param name="sFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarMasivoFichaTecnica(int idEtapa, string idFicha, string iEmpresa, string tipocodis,
                                                bool historico, bool flagFormulaEnValor, string fecha, string fechafin, string iEstado)
        {
            string ruta = string.Empty, nombre = string.Empty;
            string[] datos = new string[2];
            try
            {

                DateTime fechaVigIni = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaVigFin = DateTime.ParseExact(fechafin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                this.servFictec.GenerarReporteHistoricoFTExcel(idEtapa, idFicha, iEmpresa, tipocodis,
                                                                    historico, flagFormulaEnValor, fechaVigIni, fechaVigFin, iEstado, pathLogo, out ruta, out nombre);

                datos[0] = ruta;
                datos[1] = nombre;

                var jsonResult = Json(datos);
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        #endregion

        #region Reportes Por Parámetros

        public ActionResult IndexHistoricoParametro()
        {
            FichaTecnicaModel model = new FichaTecnicaModel();

            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            model.TienePermisoAdmin = this.TieneRolAdministradorFicha() ? 1 : 0;

            DateTime hoy = DateTime.Now;
            model.FechaInicio = (hoy.AddYears(-1)).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = (hoy).ToString(ConstantesAppServicio.FormatoFecha);

            // verificar si tiene permisos administrador de ficha
            model.ListaEtapas = servFictec.ListFtExtEtapas();
            model.ListaAgrupacion = this.servParam.GetByCriteriaPrAgrupacions(FUENTE_FICHA_TECNICA, ConstantesAppServicio.Activo);
            model.ListaFamilia = new List<EqFamiliaDTO>() { appEquipamiento.GetByIdEqFamilia(0) };
            model.ListaFamilia.AddRange(appEquipamiento.ListEqFamilias().OrderBy(x => x.Famnomb).ToList());
            foreach (var item in model.ListaFamilia)
            {
                item.Famnomb += " (" + item.Famabrev + ")";
            }
            model.ListaCategoria = appDespacho.ListarCategoriaGrupo().OrderBy(x => x.Catenomb).ToList();
            foreach (var item in model.ListaCategoria)
            {
                item.Catenomb += " (" + item.Cateabrev + ")";
            }

            return View(model);
        }

        /// <summary>
        /// Lista de Elementos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarFiltroHistParamXEtapaYTipoempresa(int tipoEmpresaFT, int idEtapa)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                servFictec.ListarFiltroReporteParametroHist(tipoEmpresaFT, idEtapa, out List<SiEmpresaDTO> listaEmp);
                model.ListaEmpresa = new List<SiEmpresaDTO>();
                if (listaEmp.Find(x => x.Emprcodi == 0) == null)
                    model.ListaEmpresa = new List<SiEmpresaDTO>() { new SiEmpresaDTO() { Emprnomb = "( TODOS )" } };
                model.ListaEmpresa.AddRange(listaEmp);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// Lista de Elementos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarFiltroHistParamXAgrupacion(int agrupcodi, string famcodis, string catecodis)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                servFictec.ListarConceptoYPropiedadXAgrupacion(agrupcodi, famcodis, catecodis, out List<EqPropiedadDTO> listaProp, out List<PrConceptoDTO> listaCnp);
                model.ListaPropiedad = listaProp;
                model.ListaConcepto = listaCnp;

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        [HttpPost]
        public JsonResult ListarDatoReporteHistoricoParametro(int tipoEmpresaFT, int idEtapa, string idEmpresa, string famcodis, string catecodis,
                                                            string propcodis, string concepcodis, string tipocodis,
                                                            bool historico, bool flagFormulaEnValor, string fecha, string fechafin, string estado)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                DateTime fechaVigIni = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaVigFin = DateTime.ParseExact(fechafin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                FTReporteExcel objRpt = servFictec.ListarReporteDatoHistoricoParam(tipoEmpresaFT, idEtapa, idEmpresa, famcodis, catecodis, propcodis, concepcodis, tipocodis,
                                                                historico, flagFormulaEnValor, fechaVigIni, fechaVigFin, estado, true, false, false);

                if (objRpt.ListaDataCompleto.Count <= 1000)
                {
                    model.ListaDataRptHist = objRpt.ListaDataCompleto;
                    model.Resultado = "1";
                }
                else
                {
                    model.Resultado = "0";
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// Exportación de Excel Masivo por Ficha
        /// </summary>
        /// <param name="sFechaIni"></param>
        /// <param name="sFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarReporteHistoricoParametro(int tipoEmpresaFT, int idEtapa, string idEmpresa, string famcodis, string catecodis,
                                                            string propcodis, string concepcodis, string tipocodis,
                                                            bool historico, bool flagFormulaEnValor, string fecha, string fechafin, string estado)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                DateTime fechaVigIni = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaVigFin = DateTime.ParseExact(fechafin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                this.servFictec.GenerarExcelReporteParametroHist(tipoEmpresaFT, idEtapa, idEmpresa, famcodis, catecodis, propcodis, concepcodis, tipocodis,
                                                            historico, flagFormulaEnValor, fechaVigIni, fechaVigFin, estado,
                                                            pathLogo, out string ruta, out string nombre);
                model.Resultado = nombre;
                model.Resultado2 = ruta;
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

        #region Utilidad

        /// <summary>
        /// Permite descargar el archivo 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcel()
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

    }
}