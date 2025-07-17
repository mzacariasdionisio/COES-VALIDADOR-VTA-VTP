using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PMPO.Helper;
using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using COES.Servicios.Aplicacion.PMPO.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class GeneracionArchivosDATController : BaseController
    {
        private GeneracionArchivosSDDPAppServicio servPmpodat = new GeneracionArchivosSDDPAppServicio();
        private ProgramacionAppServicio servPmpo = new ProgramacionAppServicio();

        #region Declaración de variables

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index(int? periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            //creación de periodos automáticos
            servPmpo.CrearIndPeriodoAutomatico();

            SDDPModel model = new SDDPModel();
            model.RutaDescarga = servPmpodat.GetDirectorioDat();

            model.ListaAnio = servPmpo.ListarAnio();
            if (periodo > 0)
            {
                var regPeriodo = servPmpo.GetByIdPmoPeriodo(periodo.GetValueOrDefault(0));

                model.Anho = regPeriodo.Pmperifecinimes.Year;
                model.ListaMes = servPmpo.ListarMesxAnio(model.Anho);
                model.Periodo = periodo.Value;
            }
            else
            {
                model.Anho = model.ListaAnio[0].Entero1.Value;
                if (model.ListaAnio.Count > 0) model.ListaMes = servPmpo.ListarMesxAnio(model.Anho);

                if (model.ListaMes.Any())
                {
                    DateTime mesDefecto = servPmpo.GetPeriodoActual();
                    var regMes = model.ListaMes.Find(x => x.PmPeriFechaPeriodo == mesDefecto);
                    if (regMes != null) model.Periodo = regMes.PmPeriCodi;
                }
            }

            return View(model);
        }

        /// <summary>
        /// Listar meses por año
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaMesxAnio(int anio)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                model.ListaMes = servPmpo.ListarMesxAnio(anio);
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
        /// Listar archivos por horizonte (semanal, mensual)
        /// </summary>
        /// <param name="pmpericodi"></param>
        /// <param name="horizonte"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaFormatoXMes(int pmpericodi, string horizonte)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                model.ListaPmpoformato = servPmpodat.ListarFormatoXMes(pmpericodi, horizonte);
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
        /// Generacion de data para los archivos .dat
        /// </summary>
        public JsonResult Procesar(string[] archivos, int periodo, string horizonte, string tipoReporteMantto)
        {
            base.ValidarSesionJsonResult();

            if (horizonte == "S")
            {
                foreach (var dat in archivos)
                {
                    switch (dat)
                    {
                        case "pmhisepe.dat":
                            servPmpodat.ProcesarDatNew(periodo, "pmhisepe.dat", tipoReporteMantto);//NET 20190225 - Procesamiento de archivos DAT
                            break;
                        case "pmtrsepe.dat":
                            servPmpodat.ProcesarDatNew(periodo, "pmtrsepe.dat", tipoReporteMantto);//NET 20190227 - Procesamiento de archivos DAT
                            break;
                        case "dbus.dat":
                            servPmpodat.ProcesarDat(periodo, "dbus.dat", base.UserName);
                            break;
                        case "dbf005pe.dat":
                            servPmpodat.ProcesarDatNew(periodo, "dbf005.dat", base.UserName);//NET 20190304 - Generar data Proyeccion en Demanda Barras SDDP
                            break;
                        case "cgndpe.dat":
                            servPmpodat.ProcesarDat(periodo, "cgndpe.dat", base.UserName);
                            break;
                        case "mgndpe.dat":
                            servPmpodat.ProcesarDat(periodo, "mgndpe.dat", base.UserName);
                            break;
                        case "gndse05pe.dat":
                            servPmpodat.ProcesarDatNew(periodo, "gndse05.dat", base.UserName);//NET 20190364 - Generar data RER
                            break;
                    }
                }
            }
            else
            {
                foreach (var dat in archivos)
                {
                    switch (dat)
                    {
                        case "pmhimepe.dat":
                            servPmpodat.ProcesarDatMensual(periodo, "pmhimepe.dat", tipoReporteMantto);
                            break;
                        case "pmtrmepe.dat":
                            servPmpodat.ProcesarDatMensual(periodo, "pmtrmepe.dat", tipoReporteMantto);
                            break;
                    }
                }

            }

            //cambiar a procesado el mes
            var regPeriodo = servPmpo.GetByIdPmoPeriodo(periodo);
            servPmpo.ActualizarProcesado(regPeriodo.Pmperifecinimes.Year, regPeriodo.Pmperifecinimes.Month);

            //return View();
            return Json("OK");
        }

        /// <summary>
        /// Generacion de archivos .dat
        /// </summary>
        public async Task<ActionResult> GenerarDat(string[] archivos, int periodo, string carpeta, string horizonte)
        {
            base.ValidarSesionJsonResult();

            string directorioPersonalizado = servPmpo.GetEstructuraRuta(carpeta) + "\\";
            if (string.IsNullOrEmpty(carpeta)) //si el usuario no escoge carpeta entonces crear carpeta temporal en servidor intranet y descargarlo
            {
                directorioPersonalizado = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload + ConstantesPMPO.FolderDat;
                FileServer.DeleteFolderAlter("", directorioPersonalizado);
                FileServer.CreateFolder("", ConstantesPMPO.FolderDat, AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload);
            }

            bool[] result = null;
            if (horizonte == "S")
            {
                result = await servPmpodat.GenerarArchivosSemanal(archivos, periodo, directorioPersonalizado);
            }
            else
            {
                result = await servPmpodat.GenerarArchivosMensual(archivos, periodo, directorioPersonalizado);
            }

            string nameFile = "";
            if (string.IsNullOrEmpty(carpeta)) //si el usuario no escoge carpeta entonces crear carpeta temporal en servidor intranet y descargarlo
            {
                var regPeriodo = servPmpo.GetByIdPmoPeriodo(periodo);
                var nombreZip = string.Format("GeneracionDat_{0}.zip", regPeriodo.Pmperifecinimes.ToString(ConstantesAppServicio.FormatoMesAnio));

                string rutaLocal = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload;
                var rutaZip = rutaLocal + nombreZip;

                if (FileServer.VerificarExistenciaFile("", nombreZip, rutaLocal)) FileServer.DeleteBlob(nombreZip, rutaLocal);

                FileServer.CreateZipFromDirectory("", rutaZip, directorioPersonalizado);

                nameFile = nombreZip;
            }

            var completos = 0;
            var error = 0;
            if (result != null)
            {
                completos = result.Where(x => x.Equals(true)).Count();
                error = result.Where(x => x.Equals(false)).Count();
            }
            else
            {
                error = 100;
                completos = 0;
            }


            return Json(new
            {
                completos,
                error,
                nameFile
            });

        }

        #region Insumos (correlaciones de equipos)

        #region Correlaciones de Equipos

        /// <summary>
        /// Vista de Correlaciones
        /// </summary>
        public ActionResult Correlaciones()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            SDDPModel model = new SDDPModel();
            model.ListaTipoSDDP = servPmpo.ListPmoSddpTipos().Where(x => x.Tsddpcodi == ConstantesPMPO.TsddpPlantaHidraulica || x.Tsddpcodi == ConstantesPMPO.TsddpPlantaTermica).ToList();
            model.ListaFamilia = servPmpodat.ListTipoEquipo();
            model.ListaEmpresa = servPmpodat.ListEmpresaCorrelacion();
            model.Fecha = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);
            model.Agrupcodi = ConstantesPMPO.AgrupcodiPmpo;
            model.ListaModoOp = servPmpodat.ListaModoOperacion();

            return View(model);
        }

        [HttpPost]
        public JsonResult ListaCorrelaciones(int emprcodi, int tsppcodi, int famcodi, string tipoReporteMantto)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                model.Correlaciones = servPmpodat.ListCorrelaciones(emprcodi, tsppcodi, famcodi, tipoReporteMantto
                                , out List<EqEquipoDTO> listaEquiposTermicos, out List<PrGrupoDTO> listaGrupoModo, out List<EqEquipoDTO> listaUnidadesTermo);
                model.TieneAlerta = model.Correlaciones.Count(x => x.TieneAlerta) > 0;
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
        /// Vista parcial de generadores para combobox
        /// </summary>
        public JsonResult ListaCodigoSddp(int tsppcodi)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                model.ListaCodigoSDDP = servPmpo.GetByCriteriaPmoSddpCodigos(tsppcodi.ToString());
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
        /// Vista parcial de Equipos para combobox
        /// </summary>
        public JsonResult ListaEquipo(int famcodi)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                model.ListaEquipo = servPmpodat.ListEquipo(famcodi);
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
        /// Grabar nueva correlacion
        /// </summary>
        public JsonResult GuardarCorrelacion(CorrelacionModel correlacion)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                base.ValidarSesionJsonResult();

                var entity = new PmoConfIndispEquipoDTO
                {
                    Sddpcodi = correlacion.Sddpcodi,
                    EquiCodi = correlacion.EquiCodi,
                    PmCindPorcentaje = correlacion.Porcentaje,
                    PmCindConJuntoEqp = correlacion.PmCindConJuntoEqp,
                    PmCindRelInversa = correlacion.PmCindRelInversa,
                    Grupocodimodo = correlacion.Grupocodimodo,
                };

                if (correlacion.Actualizar)
                {
                    entity.PmCindCodi = correlacion.PmCindCodi;
                    entity.PmCindUsuModificacion = base.UserName;
                    entity.PmCindFecModificacion = DateTime.Now;
                    servPmpodat.UpdateCorrelacion(entity);
                }
                else
                {
                    entity.PmCindEstRegistro = "A";
                    entity.PmCindUsuCreacion = base.UserName;
                    entity.PmCindFecCreacion = DateTime.Now;
                    servPmpodat.SaveCorrelacion(entity);
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
        /// Eliminar correlacion
        /// </summary>
        public JsonResult EliminarCorrelacion(int pmCindCodi)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                base.ValidarSesionJsonResult();

                servPmpodat.EliminarCorrelacion(pmCindCodi, base.UserName);
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
        /// Vista parcial de Equipos para combobox
        /// </summary>
        public JsonResult ObtenerCorrelacion(int codigo)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                model.Correlacion = servPmpodat.GetByIdPmoConfIndispEquipo(codigo);
                model.ListaCodigoSDDP = servPmpo.GetByCriteriaPmoSddpCodigos(model.Correlacion.Tsddpcodi.ToString());
                model.ListaEquipo = servPmpodat.ListEquipo(model.Correlacion.FamCodi);
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

        #endregion

        public JsonResult GenerarRepDemandaPorBloque(int periodo)
        {
            try
            {
                string file = "";
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload;
                file = servPmpodat.GenerarRepDemandaPorBloque(periodo, ruta);

                return Json(file, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GenerarRepGrupoRelaso(string strGrrdefcodi)
        {
            try
            {
                string file = "";
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload;
                file = servPmpodat.GenerarRepGrupoRelaso(strGrrdefcodi, ruta);

                return Json(file, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Mantenimiento semanal y mensual (1. Hidráulico, 2. Térmico)

        /// <summary>
        ///Vista Hidraulica Pmhisepe.dat
        /// </summary>
        public ActionResult Disponibilidad(int? periodo, string tipoFormato)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (periodo.GetValueOrDefault(0) <= 0 || string.IsNullOrEmpty(tipoFormato)) return base.RedirectToHomeDefault();

            SDDPModel model = new SDDPModel();
            model.Periodo = periodo.Value;
            model.PmoPeriodo = servPmpodat.GetRangoPeriodoManttos(model.Periodo, tipoFormato);
            model.TipoFormato = tipoFormato;
            model.Titulo = servPmpodat.TituloDisponibilidad(tipoFormato) + " - " + (model.PmoPeriodo.PmPeriNombre + " " + model.PmoPeriodo.Pmperifecinimes.Year);

            return View(model);
        }

        /// <summary>
        /// Handsontable Pmhisepe.dat
        /// </summary>
        public JsonResult ListarDisponibilidad(int periodo, string tipoFormato)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                servPmpodat.ArmarHandsonDisponibilidad(tipoFormato, periodo, out string[][] data, out object[] columnas, out string[][] descripcionHandson);
                model.Data = data;
                model.Columnas = columnas;

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
        /// Grabado de Handsontable Pmhisepe.dat
        /// </summary>
        public JsonResult GrabarDataDisponibilidad(int periodo, string tipoFormato, string[][] datos)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                base.ValidarSesionJsonResult();

                List<PmoDatPmhiTrDTO> listaUpdate = new List<PmoDatPmhiTrDTO>();
                for (var x = 2; x < datos.Length; x++)
                {
                    PmoDatPmhiTrDTO entity = new PmoDatPmhiTrDTO();
                    entity.PmPmhtCodi = Convert.ToInt32(datos[x][0]);
                    #region Mapeo Entidades
                    entity.PmPmhtDisp01 = string.IsNullOrEmpty(datos[x][5]) ? 0 : Convert.ToDecimal(datos[x][5]);
                    entity.PmPmhtDisp02 = string.IsNullOrEmpty(datos[x][6]) ? 0 : Convert.ToDecimal(datos[x][6]);
                    entity.PmPmhtDisp03 = string.IsNullOrEmpty(datos[x][7]) ? 0 : Convert.ToDecimal(datos[x][7]);
                    entity.PmPmhtDisp04 = string.IsNullOrEmpty(datos[x][8]) ? 0 : Convert.ToDecimal(datos[x][8]);
                    entity.PmPmhtDisp05 = string.IsNullOrEmpty(datos[x][9]) ? 0 : Convert.ToDecimal(datos[x][9]);
                    entity.PmPmhtDisp06 = string.IsNullOrEmpty(datos[x][10]) ? 0 : Convert.ToDecimal(datos[x][10]);
                    entity.PmPmhtDisp07 = string.IsNullOrEmpty(datos[x][11]) ? 0 : Convert.ToDecimal(datos[x][11]);
                    entity.PmPmhtDisp08 = string.IsNullOrEmpty(datos[x][12]) ? 0 : Convert.ToDecimal(datos[x][12]);
                    entity.PmPmhtDisp09 = string.IsNullOrEmpty(datos[x][13]) ? 0 : Convert.ToDecimal(datos[x][13]);
                    entity.PmPmhtDisp10 = string.IsNullOrEmpty(datos[x][14]) ? 0 : Convert.ToDecimal(datos[x][14]);
                    entity.PmPmhtDisp11 = string.IsNullOrEmpty(datos[x][15]) ? 0 : Convert.ToDecimal(datos[x][15]);
                    entity.PmPmhtDisp12 = string.IsNullOrEmpty(datos[x][16]) ? 0 : Convert.ToDecimal(datos[x][16]);

                    if (tipoFormato == ConstantesPMPO.FormatoTermicoSemanal || tipoFormato == ConstantesPMPO.FormatoHidraulicoSemanal)
                    {
                        entity.PmPmhtDisp13 = string.IsNullOrEmpty(datos[x][17]) ? 0 : Convert.ToDecimal(datos[x][17]);
                        entity.PmPmhtDisp14 = string.IsNullOrEmpty(datos[x][18]) ? 0 : Convert.ToDecimal(datos[x][18]);
                        entity.PmPmhtDisp15 = string.IsNullOrEmpty(datos[x][19]) ? 0 : Convert.ToDecimal(datos[x][19]);
                        entity.PmPmhtDisp16 = string.IsNullOrEmpty(datos[x][20]) ? 0 : Convert.ToDecimal(datos[x][20]);
                        entity.PmPmhtDisp17 = string.IsNullOrEmpty(datos[x][21]) ? 0 : Convert.ToDecimal(datos[x][21]);
                        entity.PmPmhtDisp18 = string.IsNullOrEmpty(datos[x][22]) ? 0 : Convert.ToDecimal(datos[x][22]);
                        entity.PmPmhtDisp19 = string.IsNullOrEmpty(datos[x][23]) ? 0 : Convert.ToDecimal(datos[x][23]);
                        entity.PmPmhtDisp20 = string.IsNullOrEmpty(datos[x][24]) ? 0 : Convert.ToDecimal(datos[x][24]);
                        entity.PmPmhtDisp21 = string.IsNullOrEmpty(datos[x][25]) ? 0 : Convert.ToDecimal(datos[x][25]);
                        entity.PmPmhtDisp22 = string.IsNullOrEmpty(datos[x][26]) ? 0 : Convert.ToDecimal(datos[x][26]);
                        entity.PmPmhtDisp23 = string.IsNullOrEmpty(datos[x][27]) ? 0 : Convert.ToDecimal(datos[x][27]);
                        entity.PmPmhtDisp24 = string.IsNullOrEmpty(datos[x][28]) ? 0 : Convert.ToDecimal(datos[x][28]);
                        entity.PmPmhtDisp25 = string.IsNullOrEmpty(datos[x][29]) ? 0 : Convert.ToDecimal(datos[x][29]);
                        entity.PmPmhtDisp26 = string.IsNullOrEmpty(datos[x][30]) ? 0 : Convert.ToDecimal(datos[x][30]);
                        entity.PmPmhtDisp27 = string.IsNullOrEmpty(datos[x][31]) ? 0 : Convert.ToDecimal(datos[x][31]);
                        entity.PmPmhtDisp28 = string.IsNullOrEmpty(datos[x][32]) ? 0 : Convert.ToDecimal(datos[x][32]);
                        entity.PmPmhtDisp29 = string.IsNullOrEmpty(datos[x][33]) ? 0 : Convert.ToDecimal(datos[x][33]);
                        entity.PmPmhtDisp30 = string.IsNullOrEmpty(datos[x][34]) ? 0 : Convert.ToDecimal(datos[x][34]);
                        entity.PmPmhtDisp31 = string.IsNullOrEmpty(datos[x][35]) ? 0 : Convert.ToDecimal(datos[x][35]);
                        entity.PmPmhtDisp32 = string.IsNullOrEmpty(datos[x][36]) ? 0 : Convert.ToDecimal(datos[x][36]);
                        entity.PmPmhtDisp33 = string.IsNullOrEmpty(datos[x][37]) ? 0 : Convert.ToDecimal(datos[x][37]);
                        entity.PmPmhtDisp34 = string.IsNullOrEmpty(datos[x][38]) ? 0 : Convert.ToDecimal(datos[x][38]);
                        entity.PmPmhtDisp35 = string.IsNullOrEmpty(datos[x][39]) ? 0 : Convert.ToDecimal(datos[x][39]);
                        entity.PmPmhtDisp36 = string.IsNullOrEmpty(datos[x][40]) ? 0 : Convert.ToDecimal(datos[x][40]);
                        entity.PmPmhtDisp37 = string.IsNullOrEmpty(datos[x][41]) ? 0 : Convert.ToDecimal(datos[x][41]);
                        entity.PmPmhtDisp38 = string.IsNullOrEmpty(datos[x][42]) ? 0 : Convert.ToDecimal(datos[x][42]);
                        entity.PmPmhtDisp39 = string.IsNullOrEmpty(datos[x][43]) ? 0 : Convert.ToDecimal(datos[x][43]);
                        entity.PmPmhtDisp40 = string.IsNullOrEmpty(datos[x][44]) ? 0 : Convert.ToDecimal(datos[x][44]);
                        entity.PmPmhtDisp41 = string.IsNullOrEmpty(datos[x][45]) ? 0 : Convert.ToDecimal(datos[x][45]);
                        entity.PmPmhtDisp42 = string.IsNullOrEmpty(datos[x][46]) ? 0 : Convert.ToDecimal(datos[x][46]);
                        entity.PmPmhtDisp43 = string.IsNullOrEmpty(datos[x][47]) ? 0 : Convert.ToDecimal(datos[x][47]);
                        entity.PmPmhtDisp44 = string.IsNullOrEmpty(datos[x][48]) ? 0 : Convert.ToDecimal(datos[x][48]);
                        entity.PmPmhtDisp45 = string.IsNullOrEmpty(datos[x][49]) ? 0 : Convert.ToDecimal(datos[x][49]);
                        entity.PmPmhtDisp46 = string.IsNullOrEmpty(datos[x][50]) ? 0 : Convert.ToDecimal(datos[x][50]);
                        entity.PmPmhtDisp47 = string.IsNullOrEmpty(datos[x][51]) ? 0 : Convert.ToDecimal(datos[x][51]);
                        entity.PmPmhtDisp48 = string.IsNullOrEmpty(datos[x][52]) ? 0 : Convert.ToDecimal(datos[x][52]);
                        entity.PmPmhtDisp49 = string.IsNullOrEmpty(datos[x][53]) ? 0 : Convert.ToDecimal(datos[x][53]);
                        entity.PmPmhtDisp50 = string.IsNullOrEmpty(datos[x][54]) ? 0 : Convert.ToDecimal(datos[x][54]);
                        entity.PmPmhtDisp51 = string.IsNullOrEmpty(datos[x][55]) ? 0 : Convert.ToDecimal(datos[x][55]);
                        entity.PmPmhtDisp52 = string.IsNullOrEmpty(datos[x][56]) ? 0 : Convert.ToDecimal(datos[x][56]);
                    }

                    #endregion mapeo

                    listaUpdate.Add(entity);
                }

                //guardar
                servPmpodat.ActualizarListaDataDisponibilidad(listaUpdate);
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
        /// Importar data Pmhisepe.dat
        /// </summary>
        public JsonResult ImportarDataDisponibilidad()
        {
            SDDPModel model = new SDDPModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    var NombreArchivo = file.FileName;
                    var lista = servPmpodat.ImportarDataDisponibilidad(Request.InputStream);
                    servPmpodat.ArmarHandsonImportarDisponibilidad(lista, out string[][] data, out object[] columnas, out string[][] descripcionHandson);
                    model.Data = data;
                    model.Columnas = columnas;
                }

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
        /// Exportado de Handsontable Pmhisepe.dat
        /// </summary>
        public ActionResult ExportarDataDisponibilidad(int periodo, string tipoFormato)
        {
            var file = servPmpodat.ExportarDataDisponibilidad(periodo, tipoFormato, out string nombreArchivo);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
        }

        #region Manttos

        public JsonResult ExportarMantenimientos(int periodo, string tipo, string tipoReporteMantto)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload;

                model.Archivo = servPmpodat.GenerarReporteMantenimientos(periodo, tipo, tipoReporteMantto, ruta);
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
        /// Descarga los mantenimientos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(int formato, string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            return File(path, app, file);
        }

        #endregion

        #endregion

        #region Demanda (3. Definición de Barras, 4. Demanda en barras)

        /// <summary>
        /// Vista Dbus.dat
        /// </summary>
        public ActionResult Dbus()
        {
            List<PmoDatDbusDTO> list = servPmpodat.ListDbus();
            var barras = list.Select(p => p.GrupoNomb).Distinct().ToList();
            ViewBag.NroBarras = barras.Count();
            return View(list);
        }

        /// <summary>
        /// Vista Dbf.dat
        /// </summary>
        public ActionResult Dbf(string periodo)
        {
            ViewBag.periodo = periodo;
            return View();
        }

        /// <summary>
        /// Exportar Dbf.dat
        /// </summary>
        public ActionResult ExportarDataDbf(string periodo, string nombarra)
        {
            try
            {
                string nombreArchivo = DateTime.Now.ToString("ddMMyyyyhhmmss") + "_Dbf.xlsx";
                var file = servPmpodat.ExportarDataDbf(Convert.ToInt32(periodo), nombarra);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        /// <summary>
        /// Carga de data Dbf.dat
        /// </summary>
        public ActionResult ListDbf(string periodo, string nombarra)
        {
            string[][] data = null;
            object[] columnas = null;

            List<PmoDatDbfDTO> lista = servPmpodat.ListDbf(Convert.ToInt32(periodo), nombarra);
            var grupos = servPmpodat.ListGrupoDbf(13);
            servPmpodat.ArmarHandsonDbf(lista, grupos, out data, out columnas);
            return Json(new
            {
                data,
                columnas
            });
        }

        public ActionResult GrabarDbf(string periodo, string[][] datos)
        {
            //Log.Info("Prueba");
            base.ValidarSesionJsonResult();
            var largo = datos.Length;
            PmoDatDbfDTO entity = null;

            int filasafectadas = 0;
            int filasNoGuardadas = 0;
            //var filas = pmo.DeleteDbf(Convert.ToInt32(periodo));
            for (var x = 1; x < largo; x++)
            {
                entity = new PmoDatDbfDTO();
                entity.PeriCodi = Convert.ToInt32(periodo);

                // NET 20190307 - Modificado para corregir la funcionalidad "Guardar Data DBF" - Inicio
                /*
                var compuesto = datos[x][0];
                if (!ValidarData.ValidarCompuestoDbf(compuesto)) {
                    filasNoGuardadas++;
                    continue;
                }
                var grupo = pmo.GetGrupoCodiDbf(Convert.ToInt32(compuesto.Split('-')[0]));
                entity.GrupoCodi = grupo.Grupocodi;
                if (grupo.Grupocodi == 0)
                {
                    filasNoGuardadas++;
                    continue;
                }
                  
                 * */

                if (datos[x][8] == null || datos[x][8].Trim() == "" || !ValidarData.ValidarEnteroDbf(datos[x][8]))
                {
                    filasNoGuardadas++;
                    continue;
                }
                entity.PmDbf5Codi = Convert.ToInt32(datos[x][8]);

                if (datos[x][7] == null || datos[x][7].Trim() == "" || !ValidarData.ValidarEnteroDbf(datos[x][7]))
                {
                    filasNoGuardadas++;
                    continue;
                }
                entity.GrupoCodi = Convert.ToInt32(datos[x][7]);




                entity.PmDbf5LCod = Convert.ToString(datos[x][1]);

                if (!ValidarData.ValidarFechaDbf(datos[x][2]))
                {
                    filasNoGuardadas++;
                    continue;
                }
                //entity.PmDbf5FecIni = Convert.ToDateTime(datos[x][2]);
                DateTime dt;
                DateTime.TryParseExact(datos[x][2], "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                entity.PmDbf5FecIni = dt;

                // NET 20190307 - Modificado para corregir la funcionalidad "Guardar Data DBF" - Fin

                if (!ValidarData.ValidarEnteroDbf(datos[x][3]))
                {
                    filasNoGuardadas++;
                    continue;
                }
                entity.PmBloqCodi = Convert.ToInt32(datos[x][3]);

                if (!ValidarData.ValidarDecimalDbf(datos[x][4]))
                {
                    filasNoGuardadas++;
                    continue;
                }
                entity.PmDbf5Carga = Convert.ToDecimal(datos[x][4]);

                if (!ValidarData.ValidarEnteroDbf(datos[x][5]))
                {
                    filasNoGuardadas++;
                    continue;
                }
                entity.PmDbf5ICCO = Convert.ToInt32(datos[x][5]);

                //filasafectadas += pmo.SaveDbf(entity);// NET 20190307 - Modificado para corregir la funcionalidad "Guardar Data DBF"
                filasafectadas += servPmpodat.UpdateDbf(entity);// NET 20190307 - Modificado para corregir la funcionalidad "Guardar Data DBF"  

            }

            return Json(new
            {
                filasafectadas,
                filasNoGuardadas
            });
        }

        public ActionResult ImportarDataDbf()
        {
            base.ValidarSesionJsonResult();
            try
            {
                string[][] data = null;
                object[] columnas = null;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    var NombreArchivo = file.FileName;

                    var lista = servPmpodat.ImportarDataDbf(Request.InputStream);
                    var grupos = servPmpodat.ListGrupoDbf(13);
                    servPmpodat.ArmarHandsonDbf(lista, grupos, out data, out columnas);
                }

                return Json(new
                {
                    data,
                    columnas
                });
            }
            catch (Exception e)
            {
                //throw new Exception(e.Message);
                //return Json(e.Message);
                return Json(new { success = false }, e.Message);
            }
        }

        #endregion

        #region Unidades tipo renovable (5. Configuración, 6. Modificación, 7. Escenario)

        /// <summary>
        /// Vista Cgndpe.dat
        /// </summary>
        public ActionResult Cgndpe()
        {
            List<PmoDatCgndDTO> listCgndpe = servPmpodat.ListCgnd();
            List<PrGrupoDTO> listBarra = servPmpodat.ListBarra();

            var entity = new CgndModel
            {
                ListBarra = listBarra,
                ListCgndpe = listCgndpe
            };
            return View(entity);
        }

        /// <summary>
        /// Obtener registro Cgndpe.dat
        /// </summary>
        public ActionResult GetCgndpe(string PmCgndCodi)
        {
            base.ValidarSesionJsonResult();
            PmoDatCgndDTO cgndpe = servPmpodat.GetByIdCgnd(Convert.ToInt32(PmCgndCodi));

            return Json(cgndpe);
        }

        /// <summary>
        /// Actualizar registro Cgndpe.dat
        /// </summary>
        public ActionResult UpdateCgndpe(CgndModel cgnd)
        {
            base.ValidarSesionJsonResult();
            var entity = new PmoDatCgndDTO
            {
                PmCgndCodi = cgnd.PmCgndCodi,
                GrupoCodi = cgnd.GrupoCodi,
                PmCgndTipoPlanta = cgnd.PmCgndTipoPlanta,
                PmCgndNroUnidades = cgnd.PmCgndNroUnidades,
                PmCgndPotInstalada = cgnd.PmCgndPotInstalada,
                PmCgndFactorOpe = cgnd.PmCgndFactorOpe,
                PmCgndProbFalla = cgnd.PmCgndProbFalla,
                PmCgndCorteOFalla = cgnd.PmCgndCorteOFalla
            };
            int cambio = servPmpodat.UpdateCgnd(entity);
            return Json(cambio);
        }

        /// <summary>
        /// Vista Mgndpe.dat
        /// </summary>
        public ActionResult Mgndpe()
        {
            List<PmoDatMgndDTO> listMgnd = servPmpodat.ListMgnd();
            List<PrGrupoDTO> listBarra = servPmpodat.ListBarraMgnd();

            var entity = new MgndDModel
            {
                ListBarra = listBarra,
                ListMgnd = listMgnd
            };
            return View(entity);
        }

        /// <summary>
        /// Obtener registro Mgndpe.dat
        /// </summary>
        public ActionResult GetMgndpe(string PmMgndCodi)
        {
            base.ValidarSesionJsonResult();
            PmoDatMgndDTO cgndpe = servPmpodat.GetByIdMgnd(Convert.ToInt32(PmMgndCodi));
            return Json(cgndpe);
        }

        /// <summary>
        /// Actualizar registro Mgndpe.dat
        /// </summary>
        public ActionResult UpdateMgndpe(MgndDModel cgnd)
        {
            base.ValidarSesionJsonResult();

            //20190317 - NET: Corrección
            DateTime dt;
            DateTime.TryParseExact(cgnd.PmMgndFechastr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            cgnd.PmMgndFecha = dt;


            var entity = new PmoDatMgndDTO
            {
                PmMgndCodi = cgnd.PmMgndCodi,
                GrupoCodi = cgnd.GrupoCodi,
                CodBarra = cgnd.CodBarra,//20190317 - NET: Corrección
                PmMgndFecha = cgnd.PmMgndFecha,
                PmMgndTipoPlanta = cgnd.PmMgndTipoPlanta,
                PmMgndNroUnidades = cgnd.PmMgndNroUnidades,
                PmMgndPotInstalada = cgnd.PmMgndPotInstalada,
                PmMgndFactorOpe = cgnd.PmMgndFactorOpe,
                PmMgndProbFalla = cgnd.PmMgndProbFalla,
                PmMgndCorteOFalla = cgnd.PmMgndCorteOFalla
            };

            int cambio = servPmpodat.UpdateMgnd(entity);
            return Json(cambio);
        }

        /// <summary>
        /// Vista Gndse.dat
        /// </summary>
        public ActionResult Gndse(string periodo)
        {
            var model = new GndseModel();
            //ViewBag.periodo = periodo;
            model.periodo = Convert.ToInt32(periodo);
            model.ListCentrales = servPmpodat.ListCentralesGndseByPeriodo(Convert.ToInt32(periodo)); ;
            return View(model);
        }

        public ActionResult ListGndse(string periodo, string central)
        {
            string[][] data = null;
            object[] columnas = null;

            List<PmoDatGndseDTO> lista = servPmpodat.ListGndse(Convert.ToInt32(periodo), central);
            //var grupos = pmo.ListGrupoDbf(13);
            servPmpodat.ArmarHandsonGnde(lista, out data, out columnas);
            return Json(new
            {
                data,
                columnas
            });
        }

        public ActionResult ExportarDataGndse(string periodo, string central)
        {
            try
            {
                string nombreArchivo = DateTime.Now.ToString("ddMMyyyyhhmmss") + "_Gndse.xlsx";
                var file = servPmpodat.ExportarDataGnde(Convert.ToInt32(periodo), central);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        public JsonResult GenerarReporteExcelTotalGndse(int periodo)
        {
            try
            {
                string file = "";
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload;
                file = servPmpodat.GenerarReporteExcelTotalGndse(periodo, ruta);

                return Json(file, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GrabarGnde(string periodo, string[][] datos, string central)
        {
            //Log.Info("Prueba");
            base.ValidarSesionJsonResult();
            var largo = datos.Length;
            PmoDatGndseDTO entity = null;

            int filasafectadas = 0;
            int filasNoGuardadas = 0;
            var filas = servPmpodat.DeleteGnde(Convert.ToInt32(periodo), central);
            for (var x = 1; x < largo; x++)
            {
                entity = new PmoDatGndseDTO();
                entity.PmPeriCodi = Convert.ToInt32(periodo);
                entity.PmGnd5Codi = Convert.ToInt32(datos[x][0]);
                entity.GrupoCodi = Convert.ToInt32(datos[x][1]);
                entity.PmGnd5STG = datos[x][4].ToString();
                entity.PmGnd5SCN = datos[x][5].ToString();
                entity.PmBloqCodi = Convert.ToInt32(datos[x][6]);

                if (!datos[x][7].Trim().Equals(""))
                    entity.PmGnd5PU = Convert.ToDecimal(datos[x][7]);

                filasafectadas += servPmpodat.SaveGnde(entity);

            }

            return Json(new
            {
                filasafectadas,
                filasNoGuardadas
            });
        }

        public ActionResult ImportarDataGndse()
        {
            base.ValidarSesionJsonResult();
            try
            {
                string[][] data = null;
                object[] columnas = null;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    var NombreArchivo = file.FileName;
                    var lista = servPmpodat.ImportarDataGnde(Request.InputStream);
                    servPmpodat.ArmarHandsonGnde(lista, out data, out columnas);
                }

                return Json(new
                {
                    data,
                    columnas
                });
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        public JsonResult ExportarCGND()
        {
            try
            {
                string file = "";
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload;
                file = servPmpodat.GenerarReporteExportarCGND(ruta);

                return Json(file, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ExportarMGND()
        {
            try
            {
                string file = "";
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload;
                file = servPmpodat.GenerarReporteExportarMGND(ruta);

                return Json(file, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
