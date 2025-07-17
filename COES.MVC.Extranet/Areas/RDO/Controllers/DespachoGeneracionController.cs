using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using System.Reflection;
using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Controllers;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.RDO;
using COES.MVC.Extranet.Areas.RDO.Helper;
using COES.Framework.Base.Tools;
using System.Web.Script.Serialization;
using COES.MVC.Extranet.Helper;

namespace COES.MVC.Extranet.Areas.RDO.Controllers
{
    public class DespachoGeneracionController : FormatoController
    {
        #region Declaracion de variables de Sesión

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
        // GET: RDO/DespachoGeneracion
        public ActionResult IndexExcelWeb()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            var listaFmt = this.servFormato.ListarFormatosDespachoGeneracion();

            FormatoModel model = this.GenerarValoresDefecto(listaFmt.First().Formatcodi);
            model.ListaFormato = listaFmt;
            model.IdFormato = listaFmt.First().Formatcodi;

            return View(model);
        }

        /// <summary>
        /// Hojas segun el formato
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        public JsonResult CargarFormato(int formatcodi)
        {
            FormatoModel model = this.GenerarValoresDefecto(formatcodi);
            return Json(model);
        }

        /// <summary>
        /// Devuelve Vista Parcial
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewHojaCargaDatos(int idHoja, int idFormato)
        {
            var formato = base.servFormato.ListMeFormatos().Find(x => x.Formatcodi == idFormato);

            var modelHoja = this.GenerarValoresDefecto(idFormato);

            modelHoja.IdAplicativo = ConstantesFormatoMedicion.AplicativoProgRER;
            modelHoja.IdHoja = idHoja;
            modelHoja.Titulo = formato.Formatnombre;
            modelHoja.Periodo = formato.Formatperiodo ?? 0;
            modelHoja.IdFormato = idFormato;

            return PartialView(modelHoja);
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Despacho diario
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEmpresa, int idEnvio, string fecha, string semana, string mes, int idFormato, int verUltimoEnvio, string horario)
        {
            List<MeHojaptomedDTO> entitys = servFormato.ObtenerPtosXFormato(idFormato, idEmpresa);
            if (entitys.Count > 0)
            {
                FormatoModel jsModel = BuildHojaExcelDespachoGeneracion(idEmpresa, idEnvio, fecha, semana, mes, idFormato, verUltimoEnvio, horario);
                return Json(jsModel);
            }
            else
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Devuelve el model con informacion de Despacho diario
        /// </summary>sic
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcelDespachoGeneracion(int idEmpresa, int idEnvio, string fecha, string semana, string mes, int idFormato, int verUltimoEnvio, string horario)
        {
            FormatoModel model = new FormatoModel();
            model.ValidaHorasOperacion = false;
            model.ValidaMantenimiento = false;
            model.ValidaRestricOperativa = false;
            model.ValidaEventos = false;
            model.UtilizaScada = false;
            model.UtilizaFiltroCentral = true;
            model.ValidaTiempoReal = false;
            model.MostrarDataBDSinEnvioPrevio = true;

            model.Semana = semana;
            model.Mes = mes;

            BuildHojaExcelRDO(model, idEmpresa, idEnvio, fecha, idFormato, verUltimoEnvio, horario,"0");

            return model;
        }

        /// <summary>
        /// Graba los datos enviados por el agente del formato Despacho diario
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWeb(string[][] data, int idEmpresa, string fecha, string semana, string mes, int idFormato, string horario)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaExcelData = data;
            model.IdEmpresa = idEmpresa;
            model.Fecha = fecha;
            model.Semana = semana;
            model.Mes = mes;
            model.IdFormato = idFormato;

            FormatoResultado modelResultado = GrabarExcelWebGeneracionDespacho(model, Convert.ToInt32(horario));
            return Json(modelResultado);
        }
        /// Mejoras IEOD -- Modificó Función
        /// <summary>
        /// Permite generar el formato en formato excel de Despacho diario
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormato(string[][] data, int idEmpresa, string fecha, string semana, string mes, int idFormato, int idEnvio, string horario)
        {
            string ruta = string.Empty;
            try
            {
                idEnvio = idEnvio > 0 ? idEnvio : -1;
                this.MatrizExcel = data;
                FormatoModel model = BuildHojaExcelDespachoGeneracion(idEmpresa, idEnvio, fecha, semana, mes, idFormato, ConstantesExtranetRDO.NoVerUltimoEnvio, horario);
                model.IdEnvio = idEnvio;
                ruta = ToolsFormato.GenerarFileExcelDespachoGeneracion(model, horario);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                ruta = "-1";
            }
            return ruta;
        }

        /// <summary>
        /// Generar valor por defecto
        /// </summary>
        /// <param name="model"></param>
        private FormatoModel GenerarValoresDefecto(int formatcodi)
        {
            FormatoModel model = new FormatoModel();
            //lista de empresas
            base.IndexFormato(model, formatcodi);

            //fechas Formato Diario
            DateTime fechaActual = DateTime.Now;

            var formato = base.servFormato.ListMeFormatos().Find(x => x.Formatcodi == formatcodi);
            model.Formato = formato;
            model.Formato.FechaProceso = fechaActual.Date;
            RDOAppServicio.GetSizeFormato(model.Formato);
            if ((DateTime.Now >= formato.FechaPlazoIni) && (DateTime.Now <= formato.FechaPlazo)) { }
            else
            {
                fechaActual = fechaActual.AddDays(1);
            }

            model.Fecha = fechaActual.ToString(ConstantesExtranetRDO.FormatoFecha);
            model.Dia = fechaActual.ToString(ConstantesExtranetRDO.FormatoFecha);

            Tuple<int, int> tupla = EPDate.f_numerosemana_y_anho(fechaActual);

            model.NroSemana = tupla.Item1;

            List<GenericoDTO> entitys = new List<GenericoDTO>();

            int nsemanas = EPDate.TotalSemanasEnAnho(tupla.Item2, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + tupla.Item2;
                reg.String2 = i == tupla.Item1 ? "selected" : "";
                entitys.Add(reg);

            }
            model.Anho = tupla.Item2.ToString();
            model.ListaSemanas2 = entitys;

            //Hojas
            model.ListaMeHoja = base.servFormato.GetByCriteriaMeHoja(formatcodi);
            model.ListaMeHojaPadre = this.servFormato.ListHojaPadre(formatcodi);

            return model;
        }

        /// <summary>
        /// Lista de Semana por Año
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemanas(string idAnho)
        {
            FormatoModel model = new FormatoModel();
            List<GenericoDTO> entitys = new List<GenericoDTO>();
            if (idAnho == "0")
            {
                idAnho = DateTime.Now.Year.ToString();
            }
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = EPDate.TotalSemanasEnAnho(Int32.Parse(idAnho), FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemanas2 = entitys;
            return PartialView(model);
        }
        /// <summary>
        /// Lee el archivo excel el cual sera cargado en la interfaz excel web.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns> devuelve vrdadero si la carga ha sido correcta
        [HttpPost]
        public JsonResult LeerFileUpExcelDespacho(int idEmpresa, string fecha, string semana, string mes, int idFormato, string horario, Boolean tieneHojaView = false, int? idHoja = 0)
        {
            FormatoResultado modelResultado = new FormatoResultado();

            try
            {
                base.ValidarSesionJsonResult();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                //int retorno = ToolsFormato.VerificarIdsFormatoDespachoGeneracion(this.NombreFile, idEmpresa, idFormato);

                //if (retorno > 0 && idFormato > 0)
                //{
                MeFormatoDTO formato = servFormato.GetByIdMeFormato(idFormato);
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, ConstantesExtranetRDO.FormatoFecha);
                formato.FechaProceso = fechaProceso;
                var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                var listaPtos = servFormato.GetByCriteria2MeHojaptomeds(idEmpresa, idFormato, cabecera.Cabquery, fechaProceso, fechaProceso);
                FormatoMedicionAppServicio.GetSizeFormato(formato);

                formato.Formatcols = cabecera.Cabcolumnas;
                formato.Formatrows = cabecera.Cabfilas;
                formato.Formatheaderrow = cabecera.Cabcampodef;
                fecha = formato.FechaInicio.ToString(ConstantesExtranetRDO.FormatoFecha);

                if (!tieneHojaView)
                {
                    this.MatrizExcel = ToolsFormato.LeerExcelFileDespacho(this.NombreFile, listaPtos, formato, null, horario);
                }
                else
                {
                    FormatoModel modelPrincipal = new FormatoModel();
                    modelPrincipal.IdFormato = idFormato;
                    modelPrincipal.Formato = formato;
                    modelPrincipal.ListaHojaPto = listaPtos;
                    modelPrincipal.OpGrabar = true;
                    modelPrincipal.UtilizaHoja = true;
                    modelPrincipal.Formato.FlagUtilizaHoja = modelPrincipal.UtilizaHoja;
                    modelPrincipal.Formato.ListaHoja = servFormato.GetByCriteriaMeHoja(idFormato);
                    modelPrincipal.IdHojaPadre = idHoja.Value;

                    GenerarHojaFormato(modelPrincipal);

                    this.ListaHoja = new List<int>();
                    this.ListaMatrizExcel = new List<string[][]>();

                    foreach (var model in modelPrincipal.ListaFormatoModel)
                    {
                        this.ListaHoja.Add(model.IdHoja);
                        var matrizExcel = ToolsFormato.LeerExcelFile(this.NombreFile, model.ListaHojaPto, model.Formato, model.Hoja.Hojanombre);
                        this.ListaMatrizExcel.Add(matrizExcel);
                    }
                }
                //}
                //else
                //{
                //    throw new Exception("El archivo importado no coincide con el formato.");
                //}

                ToolsFormato.BorrarArchivo(this.NombreFile);

                modelResultado.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResultado.Mensaje = ex.Message;
                modelResultado.Detalle = ex.StackTrace;
            }

            return Json(modelResultado);
        }
        private void GenerarHojaFormato(FormatoModel modelPrincipal)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            modelPrincipal.ListaFormatoModel = new List<FormatoModel>();
            modelPrincipal.ListaMeHoja = modelPrincipal.Formato.ListaHoja;
            if (modelPrincipal.IdHojaPadre > 0)
            {
                modelPrincipal.ListaMeHoja = modelPrincipal.ListaMeHoja.Where(x => x.Hojapadre == modelPrincipal.IdHojaPadre).ToList();
            }

            var listaHoja = modelPrincipal.ListaMeHoja.Select(x => x.Hojacodi).ToList();

            foreach (var hoja in listaHoja)
            {
                string strFormato = serializer.Serialize(modelPrincipal.Formato);
                MeFormatoDTO formato = serializer.Deserialize<MeFormatoDTO>(strFormato);

                FormatoModel model = new FormatoModel();
                //datos filtros
                model.IdEmpresa = modelPrincipal.IdEmpresa;
                model.Empresa = modelPrincipal.Empresa;
                model.Anho = modelPrincipal.Anho;
                model.Mes = modelPrincipal.Mes;
                model.Dia = modelPrincipal.Formato.FechaProceso.Day.ToString();
                model.Semana = modelPrincipal.Semana;
                //hoja
                model.IdHoja = hoja;
                model.Hoja = this.servFormato.GetByIdMeHoja(model.IdHoja);
                //formato
                model.UtilizaHoja = modelPrincipal.UtilizaHoja;
                model.Formato = formato;

                model.Formato.FechaProceso = modelPrincipal.Formato.FechaProceso;
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                
                model.Fecha = model.Formato.FechaProceso.ToString(ConstantesExtranetRDO.FormatoFecha);
                model.ColumnasCabecera = modelPrincipal.ColumnasCabecera;
                model.FilasCabecera = modelPrincipal.FilasCabecera;
                model.ValidacionFormatoCheckblanco = modelPrincipal.Formato.Formatcheckblanco == 1;

                var hojaPadre = this.servFormato.GetByIdMeHoja(model.Hoja.Hojapadre);
                if (hojaPadre != null)
                {
                    model.Formato.Formatnombre = model.Formato.Formatnombre + " (" + hojaPadre.Hojanombre + ")";
                }

                //puntos
                var listaPto = modelPrincipal.ListaHojaPto.Where(x => x.Hojacodi == hoja).ToList();
                model.ListaHojaPto = listaPto;

                //handson
                model.Handson = new HandsonModel();
                model.Handson.ListaMerge = new List<CeldaMerge>();
                model.Handson.ListaColWidth = new List<int>();
                model.Handson.ListaColWidth.Add(150);

                if (!modelPrincipal.OpGrabar)
                {
                    model.Handson.ReadOnly = modelPrincipal.Handson.ReadOnly;
                    for (var i = 0; i < model.ListaHojaPto.Count; i++)
                    {
                        model.Handson.ListaColWidth.Add(100);
                    }

                    //matriz estado
                    int estado = !model.Handson.ReadOnly ? 0 : -1;
                    var matrizTipoEstado = ToolsFormato.IncializaMatrizEstado(model.ListaHojaPto, model.FilasCabecera, model.Formato.RowPorDia, (short)estado, model.Formato.Formathorizonte);
                    model.Handson.MatrizTipoEstado = matrizTipoEstado;
                }

                //agregar
                modelPrincipal.ListaFormatoModel.Add(model);
            }
        }
        public ActionResult UploadDespacho()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesDespachoGeneracion.FolderUpload;
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    string fileName = ruta + fileRandom + "." + ConstantesExtranetRDO.ExtensionFileUpload;
                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}