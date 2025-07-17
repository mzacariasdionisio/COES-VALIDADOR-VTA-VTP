using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CompensacionRSF.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.CompensacionRSF.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.SistemasTransmision.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Controllers
{
    public class DefSupResNoSuminController : BaseController
    {
        // GET: /CompensacionRSF/DefSupResNoSumin/

        public DefSupResNoSuminController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        
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
        
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        CompensacionRSFAppServicio servicioCompensacionRSF = new CompensacionRSFAppServicio();
        BarraUrsAppServicio serviciobarraUrs = new BarraUrsAppServicio();
        EmpresaAppServicio ServicioEmpresa = new EmpresaAppServicio();

        #region PRINCIPALES METODOS

        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            DefSupResNoSuminModel model = new DefSupResNoSuminModel();
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista()
        {
            DefSupResNoSuminModel model = new DefSupResNoSuminModel();
            Log.Info("ListaVcrSuDeRns - ListVcrVersiondsrnssIndex");
            model.ListaVcrSuDeRns = this.servicioCompensacionRSF.ListVcrVersiondsrnssIndex();
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, User.Identity.Name);
            return PartialView(model);
        }

        public ActionResult New()
        {
            base.ValidarSesionUsuario();
            DefSupResNoSuminModel model = new DefSupResNoSuminModel();
            model.EntidadPeriodo = new PeriodoDTO();
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.EntidadVersiondsrn = new VcrVersiondsrnsDTO();

            if (model.EntidadVersiondsrn == null)
            {
                return HttpNotFound();
            }
            model.EntidadVersiondsrn.Vcrdsrcodi = 0;
            model.EntidadVersiondsrn.Pericodi = 0;
            model.Vcrdsrfeccreacion = System.DateTime.Now.ToString("dd/MM/yyyy");
            model.EntidadVersiondsrn.Vcrdsrestado = "Abierto";
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }
      
        /// <summary>
        /// Prepara una vista para editar un nuevo registro
        /// </summary>
        /// <param name="pericodi">Código del periodo</param>
        /// <param name="vcrdsrcodi">Código de la Versión DSRNS</param>
        /// <returns></returns>
        public ActionResult Edit(int pericodi = 0, int vcrdsrcodi = 0)
        {
            base.ValidarSesionUsuario();
            DefSupResNoSuminModel model = new DefSupResNoSuminModel();
            Log.Info("EntidadVersiondsrn - GetByIdVcrVersiondsrnsEdit");
            model.EntidadVersiondsrn = this.servicioCompensacionRSF.GetByIdVcrVersiondsrnsEdit(vcrdsrcodi, pericodi);
            if (model.EntidadVersiondsrn == null)
            {
                return HttpNotFound();
            }
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            Log.Info("ListaVcrSuDeRns - ListVcrVersionDSRNS");
            model.ListaVcrSuDeRns = this.servicioCompensacionRSF.ListVcrVersionDSRNS(pericodi);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(DefSupResNoSuminModel modelo)
        {
            base.ValidarSesionUsuario();
            Log.Info("ListaVcrSuDeRns - ListVcrVersionDSRNS");
            modelo.ListaVcrSuDeRns = this.servicioCompensacionRSF.ListVcrVersionDSRNS(modelo.EntidadVersiondsrn.Vcrdsrcodi);
            if (modelo.EntidadVersiondsrn.Vcrdsrcodi == 0)
            {
                foreach (var item in modelo.ListaVcrSuDeRns)
                {
                    if (modelo.EntidadVersiondsrn.Vcrdsrnombre == item.Vcrdsrnombre)
                    {
                        modelo.ListaVcrSuDeRns = (new CompensacionRSFAppServicio()).ListVcrVersiondsrnssIndex();
                        modelo.sError = "El nombre de la version seleccionada ya se encuentra registrada";
                        modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                        return PartialView(modelo);

                    }
                }
            }
            if (ModelState.IsValid)
            {
                modelo.EntidadVersiondsrn.Vcrdsrusucreacion = User.Identity.Name;
                modelo.EntidadVersiondsrn.Vcrdsrfeccreacion = DateTime.Now;
                modelo.EntidadVersiondsrn.Vcrdsrusumodificacion = User.Identity.Name;
                modelo.EntidadVersiondsrn.Vcrdsrfecmodificacion = DateTime.Now;

                if (modelo.EntidadVersiondsrn.Vcrdsrcodi == 0)
                {
                    Log.Info("Insertar registro - SaveVcrVersiondsrns"); 
                    this.servicioCompensacionRSF.SaveVcrVersiondsrns(modelo.EntidadVersiondsrn);
                }
                else
                {
                    Log.Info("Actualizar registro - UpdateVcrVersiondsrns"); 
                    this.servicioCompensacionRSF.UpdateVcrVersiondsrns(modelo.EntidadVersiondsrn);
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return new RedirectResult(Url.Action("Index", "DefSupResNoSumin"));
            }
            modelo.ListaVcrSuDeRns = (new CompensacionRSFAppServicio()).ListVcrVersiondsrnssIndex();
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(modelo);
        }

        /// <summary>
        /// Muestra un registro 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        public ActionResult View(int vcrdsrcodi = 0, int pericodi = 0)
        {
            DefSupResNoSuminModel model = new DefSupResNoSuminModel();
            Log.Info("EntidadVersiondsrn - GetByIdVcrVersiondsrnsView");
            model.EntidadVersiondsrn = this.servicioCompensacionRSF.GetByIdVcrVersiondsrnsView(vcrdsrcodi, pericodi);
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar un registro de forma definitiva en la base de datos
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int vcrdsrcodi = 0)
        {
            base.ValidarSesionUsuario();
            Log.Info("Eliminando el registro - DeleteVcrVersiondsrns");
            this.servicioCompensacionRSF.DeleteVcrVersiondsrns(vcrdsrcodi);
            return "true";
        }

        public ActionResult Terminos(int pericodi = 0, int vcrdsrcodi = 0)
        {
            DefSupResNoSuminModel model = new DefSupResNoSuminModel();
            Log.Info("EntidadVersiondsrn - GetByIdVcrVersiondsrns");
            model.EntidadVersiondsrn = this.servicioCompensacionRSF.GetByIdVcrVersiondsrns(vcrdsrcodi);
            Log.Info("Entidad Periodo - GetByIdPeriodo");
            model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
            model.vcrdsrcodi = vcrdsrcodi;
            return View(model);
        }

        #endregion

        #region GRILLA DÉFICIT

        /// <summary>
        /// Muestra la grilla excel con los registros de Deficit
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelDT(int vcrdsrcodi = 0)
        {
            GridExcelModel model = new GridExcelModel();
            DefSupResNoSuminModel modeldef = new DefSupResNoSuminModel();

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] header = { "FECHA", "HORA INICIO", "HORA FIN", "EMPRESA", "URS", "DÉFICIT(MW)" };
            //Ancho de cada columna
            int[] widths = { 150, 100, 100, 300, 300, 150 };
            string[] headers = header.ToArray(); //Headers final a enviar
            object[] columnas = new object[6];

            //obtener las empresas para el dropdown
            Log.Info("ListaEmpresas - ListEmpresas");
            var ListaEmpresas = this.ServicioEmpresa.ListEmpresas().Select(x => x.EmprNombre).ToList();
            //obtener las URS matriculadas para el dropdown
            Log.Info("ListaURS - ListURS");
            var ListaURS = this.serviciobarraUrs.ListURS().Select(x => x.GrupoNomb).ToList();            
            //string[] aEmpresa = { "Empresa1", "Empresa2", "Empresa3" };
            Log.Info("ListaDeficit - GetByCriteriaVcrVerdeficits");
            modeldef.ListaDeficit = this.servicioCompensacionRSF.GetByCriteriaVcrVerdeficits(vcrdsrcodi);
            //se arma la matriz de datos
            string[][] data;
            int index = 0;
            if (modeldef.ListaDeficit.Count() != 0)
            {
                data = new string[modeldef.ListaDeficit.Count()][];
                foreach (VcrVerdeficitDTO item in modeldef.ListaDeficit)
                {
                    string[] itemDato = { item.Vcrvdefecha.Value.ToString("dd/MM/yyyy"), item.Vcrvdehorinicio.Value.ToString("HH:mm"), item.Vcrvdehorfinal.Value.ToString("HH:mm"), item.EmprNombre.ToString(), item.Gruponomb.ToString(), item.Vcrvdedeficit.ToString() };
                    data[index] = itemDato;
                    index++;
                }
            }
            else
            {
                data = new string[1][];
                string[] itemDato = { "", "" };
                data[index] = itemDato;
            }

            ///////////////////ARMANDO COLUMNASSSSSSSSSSSSSSSSSSSSSS!!
            columnas[0] = new
            {   //FECHA
                type = GridExcelModel.TipoFecha,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htCenter",
                readOnly = false,
            };
            columnas[1] = new
            {   //HORA INICIO
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htCenter",
                readOnly = false,
            };
            columnas[2] = new
            {   //HORA FIN
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htCenter",
                readOnly = false,
            };
            columnas[3] = new
            {   //EMPRESA
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[4] = new
            {   //URS
                type = GridExcelModel.TipoLista,
                source = ListaURS.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[5] = new
            {   //DÉFICIT(MW)
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "000000000000,0.000000000000",
                readOnly = false,
            };

            #endregion
            //model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
            //model.ListaCentralGeneracion = ListaCentrales.ToArray();

            model.Data = data;
            model.Headers = headers;
            model.Widths = widths;
            model.Columnas = columnas;

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcelDT(int vcrdsrcodi , string[][] datos)
        {
            base.ValidarSesionUsuario();
            DefSupResNoSuminModel model = new DefSupResNoSuminModel();

            try
            {
                //////////////Eliminando datos////////
                Log.Info("Eliminar registro - DeleteVcrVerdeficit");
                this.servicioCompensacionRSF.DeleteVcrVerdeficit(vcrdsrcodi);

                //Recorrer matriz para grabar detalle: se inicia en la fila 1
                for (int f = 0; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f] == null || datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadDeficit = new VcrVerdeficitDTO();
                    model.EntidadDeficit.Vcrvdecodi = 0;
                    model.EntidadDeficit.Vcrdsrcodi = vcrdsrcodi;
                    model.EntidadDeficit.Vcrvdeusucreacion = User.Identity.Name;
                    model.EntidadDeficit.Vcrvdefeccreacion = DateTime.Now;
                    if (!datos[f][0].Equals(""))
                    {
                        string fecha = Convert.ToString(datos[f][0]);
                        string horainicio = Convert.ToString(datos[f][1]);
                        string horafin = Convert.ToString(datos[f][2]);
                        if (horainicio.Equals("24:00"))
                            horainicio = "23:59";
                        if (horafin.Equals("24:00"))
                            horafin = "23:59";
                        string empresa = Convert.ToString(datos[f][3]);
                        string urs = Convert.ToString(datos[f][4]);
                        string valor = Convert.ToString(datos[f][5]);

                        model.EntidadDeficit.Vcrvdefecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        model.EntidadDeficit.Vcrvdehorinicio = DateTime.ParseExact(fecha + " " + horainicio, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        model.EntidadDeficit.Vcrvdehorfinal = DateTime.ParseExact(fecha + " " + horafin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture); 

                        model.EntidadDeficit.Emprcodi = this.ServicioEmpresa.GetByNombre(empresa).EmprCodi;
                        Log.Info("EntidadDeficit - GetByNombrePrGrupo");
                        model.EntidadDeficit.Grupocodi = this.serviciobarraUrs.GetByNombrePrGrupo(urs).GrupoCodi;
                        model.EntidadDeficit.Gruponomb = urs;
                        model.EntidadDeficit.Vcrvdedeficit = UtilCompensacionRSF.ValidarNumero(valor);

                        //Insertar registro
                        Log.Info("Insertar registro - SaveVcrVerdeficit");
                        this.servicioCompensacionRSF.SaveVcrVerdeficit(model.EntidadDeficit);
                    }
                }
                model.sError = "";
                model.sMensaje = "Felicidades, la carga de información fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }

        /// <summary>
        /// Permite eliminar todos los registros de la versión 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatosDT(int vcrdsrcodi)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                Log.Info("Eliminar registro - DeleteVcrVerdeficit");
                this.servicioCompensacionRSF.DeleteVcrVerdeficit(vcrdsrcodi);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }

        /// <summary>
        /// Permite exportar a un archivo excel todos los registros en pantalla de consulta
        /// </summary>
        /// <param name="vcrdsrcodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataDT(int vcrdsrcodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                Log.Info("Exportar información - GenerarFormatoVcrDef");
                string file = this.servicioCompensacionRSF.GenerarFormatoVcrDef(vcrdsrcodi, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        /// <summary>
        /// Lee datos desde el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoDT(string sarchivo, int vcrdsrcodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";

            try
            {

                #region Armando de contenido
                //Definimos la cabecera como una matriz
                string[] header = { "FECHA", "HORA INICIO", "HORA FIN", "EMPRESA", "URS", "DÉFICIT(MW)" };
                //Ancho de cada columna
                int[] widths = { 150, 100, 100, 300, 300, 150 };
                string[] headers = header.ToArray(); //Headers final a enviar
                object[] columnas = new object[6];

                //obtener las empresas para el dropdown
                Log.Info("ListaEmpresas - ListEmpresas");
                var ListaEmpresas = this.ServicioEmpresa.ListEmpresas().Select(x => x.EmprNombre).ToList();
                //obtener las URS matriculadas para el dropdown
                Log.Info("ListaURS - ListURS");
                var ListaURS = this.serviciobarraUrs.ListURS().Select(x => x.GrupoNomb).ToList();
                //string[] aEmpresa = { "Empresa1", "Empresa2", "Empresa3" };

                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioCompensacionRSF.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 4][]; // Lee todo el contenido del excel y le descontamos 4 filas hasta donde empieza la data
                int index = 0;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 5)
                    {
                        continue;
                    }
                    int iNumFila = iFila + 1;
                    string sEmpresa = dtRow[4].ToString();
                    EmpresaDTO dtoEmpresa = this.ServicioEmpresa.GetByNombre(sEmpresa);
                    if (dtoEmpresa == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sEmpresa;
                        iRegError++;
                        continue;
                    }
                    string sURS = dtRow[5].ToString();
                    Log.Info("Entidad TrnBarraurs - GetByNombrePrGrupo");
                    TrnBarraursDTO dtoUrs = this.serviciobarraUrs.GetByNombrePrGrupo(sURS);
                    if (dtoUrs == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sURS;
                        iRegError++;
                        continue;
                    }
                    string[] itemDato = { dtRow[1].ToString(), dtRow[2].ToString(), dtRow[3].ToString(), sEmpresa, sURS, dtRow[6].ToString() };
                    data[index] = itemDato;
                    index++;
                }

                ///////////////////ARMANDO COLUMNASSSSSSSSSSSSSSSSSSSSSS!!
                columnas[0] = new
                {   //FECHA
                    type = GridExcelModel.TipoFecha,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htCenter",
                    readOnly = false,
                };
                columnas[1] = new
                {   //HORA INICIO
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htCenter",
                    readOnly = false,
                };
                columnas[2] = new
                {   //HORA FIN
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htCenter",
                    readOnly = false,
                };
                columnas[3] = new
                {   //EMPRESA
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[4] = new
                {   //URS
                    type = GridExcelModel.TipoLista,
                    source = ListaURS.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[5] = new
                {   //DÉFICIT(MW)
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "000000000000,0.000000000000",
                    readOnly = false,
                };

                #endregion
                model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
                model.ListaEmpresas = ListaEmpresas.ToArray();
                model.ListaURS = ListaURS.ToArray();

                model.Data = data;
                model.Headers = headers;
                model.Widths = widths;
                model.Columnas = columnas;
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;

                return Json(model);
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                return Json(sResultado);
            }

        }

        #endregion

        #region GRILLA SUPERÁVIT

        /// <summary>
        /// Muestra la grilla excel con los registros de Superávit
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelST(int vcrdsrcodi = 0)
        {
            GridExcelModel model = new GridExcelModel();
            DefSupResNoSuminModel modelsup = new DefSupResNoSuminModel();

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] header = { "FECHA", "HORA INICIO", "HORA FIN", "EMPRESA", "URS", "SUPERÁVIT(MW)" };
            //Ancho de cada columna
            int[] widths = { 150, 100, 100, 300, 300, 150 };
            string[] headers = header.ToArray(); //Headers final a enviar
            object[] columnas = new object[6];
            Log.Info("ListaSuperavit - GetByCriteriaVcrVersuperavits");
            modelsup.ListaSuperavit = this.servicioCompensacionRSF.GetByCriteriaVcrVersuperavits(vcrdsrcodi);
            //obtener las empresas para el dropdown
            Log.Info("ListaEmpresas - ListEmpresas");
            var ListaEmpresas = this.ServicioEmpresa.ListEmpresas().Select(x => x.EmprNombre).ToList();
            //obtener las URS matriculadas para el dropdown
            Log.Info("ListaURS - ListURS");
            var ListaURS = this.serviciobarraUrs.ListURS().Select(x => x.GrupoNomb).ToList();
            //string[] aEmpresa = { "Empresa1", "Empresa2", "Empresa3" };

            //se arma la matriz de datos
            string[][] data;
            int index = 0;
            if (modelsup.ListaSuperavit.Count() != 0)
            {
                data = new string[modelsup.ListaSuperavit.Count()][];
                foreach (VcrVersuperavitDTO item in modelsup.ListaSuperavit)
                {
                    string[] itemDato = { item.Vcrvsafecha.Value.ToString("dd/MM/yyyy"), item.Vcrvsahorinicio.Value.ToString("HH:mm"), item.Vcrvsahorfinal.Value.ToString("HH:mm"), item.EmprNombre.ToString(), item.Gruponomb.ToString(), item.Vcrvsasuperavit.ToString() };
                    data[index] = itemDato;
                    index++;
                }
            }
            else
            {
                data = new string[1][];
                string[] itemDato = { "", "" };
                data[index] = itemDato;
            }


            ///////////////////ARMANDO COLUMNASSSSSSSSSSSSSSSSSSSSSS!!
            columnas[0] = new
            {   //FECHA
                type = GridExcelModel.TipoFecha,
                source = "",
                strict = false,
                correctFormat = true,
                className = "htCenter",
                readOnly = false,
            };
            columnas[1] = new
            {   //HORA INICIO
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htCenter",
                readOnly = false,
            };
            columnas[2] = new
            {   //HORA FIN
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htCenter",
                readOnly = false,
            };
            columnas[3] = new
            {   //EMPRESA
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[4] = new
            {   //URS
                type = GridExcelModel.TipoLista,
                source = ListaURS.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[5] = new
            {   //DÉFICIT(MW)
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "000000000000,0.000000000000",
                readOnly = false,
            };

            #endregion
            //model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
            //model.ListaCentralGeneracion = ListaCentrales.ToArray();

            model.Data = data;
            model.Headers = headers;
            model.Widths = widths;
            model.Columnas = columnas;

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcelST(int vcrdsrcodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            DefSupResNoSuminModel model = new DefSupResNoSuminModel();

            try
            {
                //////////////Eliminando datos////////
                Log.Info("Eliminar registro - DeleteVcrVersuperavit");
                this.servicioCompensacionRSF.DeleteVcrVersuperavit(vcrdsrcodi);

                //Recorrer matriz para grabar detalle: se inicia en la fila 1
                for (int f = 0; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f] == null || datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadSuperavit = new VcrVersuperavitDTO();
                    model.EntidadSuperavit.Vcrvsacodi = 0;
                    model.EntidadSuperavit.Vcrdsrcodi = vcrdsrcodi;
                    model.EntidadSuperavit.Vcrvsausucreacion = User.Identity.Name;
                    model.EntidadSuperavit.Vcrvsafeccreacion = DateTime.Now;
                    if (!datos[f][0].Equals(""))
                    {
                        string fecha = Convert.ToString(datos[f][0]);
                        string horainicio = Convert.ToString(datos[f][1]);
                        string horafin = Convert.ToString(datos[f][2]);
                        if (horainicio.Equals("24:00"))
                            horainicio = "23:59";
                        if (horafin.Equals("24:00"))
                            horafin = "23:59";
                        string empresa = Convert.ToString(datos[f][3]);
                        string urs = Convert.ToString(datos[f][4]);
                        string valor = Convert.ToString(datos[f][5]);

                        model.EntidadSuperavit.Vcrvsafecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        model.EntidadSuperavit.Vcrvsahorinicio = DateTime.ParseExact(fecha + " " + horainicio, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        model.EntidadSuperavit.Vcrvsahorfinal = DateTime.ParseExact(fecha + " " + horafin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture); 
                        model.EntidadSuperavit.Emprcodi = this.ServicioEmpresa.GetByNombre(empresa).EmprCodi;
                        Log.Info("EntidadSuperavit - GetByNombrePrGrupo");
                        model.EntidadSuperavit.Grupocodi = this.serviciobarraUrs.GetByNombrePrGrupo(urs).GrupoCodi;
                        model.EntidadSuperavit.Gruponomb = urs;
                        model.EntidadSuperavit.Vcrvsasuperavit = UtilCompensacionRSF.ValidarNumero(valor);

                    }
                    //Insertar registro
                    Log.Info("Insertar registro - SaveVcrVersuperavit");
                    this.servicioCompensacionRSF.SaveVcrVersuperavit(model.EntidadSuperavit);
                }
                model.sError = "";
                model.sMensaje = "Felicidades, la carga de información fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }

        /// <summary>
        /// Permite eliminar todos los registros de la versión 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatosST(int vcrdsrcodi)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                Log.Info("Eliminar información - DeleteVcrVersuperavit");
                this.servicioCompensacionRSF.DeleteVcrVersuperavit(vcrdsrcodi);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }
        
        /// <summary>
        /// Permite exportar a un archivo excel todos los registros en pantalla de consulta
        /// </summary>
        /// <param name="vcrdsrcodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataST(int vcrdsrcodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                Log.Info("Exportar información - GenerarFormatoVcrSup");
                string file = this.servicioCompensacionRSF.GenerarFormatoVcrSup(vcrdsrcodi, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        /// <summary>
        /// Lee datos desde el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoST(string sarchivo, int vcrdsrcodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";

            try
            {

                #region Armando de contenido
                //Definimos la cabecera como una matriz
                string[] header = { "FECHA", "HORA INICIO", "HORA FIN", "EMPRESA", "URS", "SUPERÁVIT(MW)" };
                //Ancho de cada columna
                int[] widths = { 150, 100, 100, 300, 300, 150 };
                string[] headers = header.ToArray(); //Headers final a enviar
                object[] columnas = new object[6];

                //obtener las empresas para el dropdown
                Log.Info("ListaEmpresas - ListEmpresas");
                var ListaEmpresas = this.ServicioEmpresa.ListEmpresas().Select(x => x.EmprNombre).ToList();
                //obtener las URS matriculadas para el dropdown
                Log.Info("ListaURS - ListURS");
                var ListaURS = this.serviciobarraUrs.ListURS().Select(x => x.GrupoNomb).ToList();
                //string[] aEmpresa = { "Empresa1", "Empresa2", "Empresa3" };

                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioCompensacionRSF.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 4][]; // Lee todo el contenido del excel y le descontamos 4 filas hasta donde empieza la data
                int index = 0;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 5)
                    {
                        continue;
                    }
                    int iNumFila = iFila + 1;
                    string sEmpresa = dtRow[4].ToString();
                    EmpresaDTO dtoEmpresa = this.ServicioEmpresa.GetByNombre(sEmpresa);
                    if (dtoEmpresa == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sEmpresa;
                        iRegError++;
                        continue;
                    }
                    string sURS = dtRow[5].ToString();
                    Log.Info("Entidad TrnBarraurs - GetByNombrePrGrupo");
                    TrnBarraursDTO dtoUrs = this.serviciobarraUrs.GetByNombrePrGrupo(sURS);
                    if (dtoUrs == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sURS;
                        iRegError++;
                        continue;
                    }
                    string[] itemDato = { dtRow[1].ToString(), dtRow[2].ToString(), dtRow[3].ToString(), sEmpresa, sURS, dtRow[6].ToString() };
                    data[index] = itemDato;
                    index++;
                }

                ///////////////////ARMANDO COLUMNASSSSSSSSSSSSSSSSSSSSSS!!
                columnas[0] = new
                {   //FECHA
                    type = GridExcelModel.TipoFecha,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htCenter",
                    readOnly = false,
                };
                columnas[1] = new
                {   //HORA INICIO
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htCenter",
                    readOnly = false,
                };
                columnas[2] = new
                {   //HORA FIN
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htCenter",
                    readOnly = false,
                };
                columnas[3] = new
                {   //EMPRESA
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[4] = new
                {   //URS
                    type = GridExcelModel.TipoLista,
                    source = ListaURS.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[5] = new
                {   //DÉFICIT(MW)
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "000000000000,0.000000000000",
                    readOnly = false,
                };

                #endregion
                model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
                model.ListaEmpresas = ListaEmpresas.ToArray();
                model.ListaURS = ListaURS.ToArray();

                model.Data = data;
                model.Headers = headers;
                model.Widths = widths;
                model.Columnas = columnas;
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;

                return Json(model);
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                return Json(sResultado);
            }

        }

        #endregion

        #region GRILLA RESERVA NO SUMINISTRADA subida = 1 / bajada = 2 <- vcvrnstipocarga

        /// <summary>
        /// Muestra la grilla excel con los registros de Reserva No Suministrada
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelRNS(int vcrdsrcodi = 0, int vcvrnstipocarga = 1)
        {
            GridExcelModel model = new GridExcelModel();
            DefSupResNoSuminModel modelsup = new DefSupResNoSuminModel();

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] header = { "FECHA", "HORA INICIO", "HORA FIN", "EMPRESA", "URS", "RNS(MW)" };
            //Ancho de cada columna
            int[] widths = { 150, 100, 100, 300, 300, 150 };
            string[] headers = header.ToArray(); //Headers final a enviar
            object[] columnas = new object[6];
            Log.Info("ListaRNS - GetByCriteriaVcrVerrnss");
            modelsup.ListaRNS = this.servicioCompensacionRSF.GetByCriteriaVcrVerrnss(vcrdsrcodi, vcvrnstipocarga);
            //obtener las empresas para el dropdown
            Log.Info("ListaEmpresas - ListEmpresas");
            var ListaEmpresas = this.ServicioEmpresa.ListEmpresas().Select(x => x.EmprNombre).ToList();
            //obtener las URS matriculadas para el dropdown
            Log.Info("ListaURS - ListURS");
            var ListaURS = this.serviciobarraUrs.ListURS().Select(x => x.GrupoNomb).ToList();
            //string[] aEmpresa = { "Empresa1", "Empresa2", "Empresa3" };

            //se arma la matriz de datos
            string[][] data;
            int index = 0;
            if (modelsup.ListaRNS.Count() != 0)
            {
                data = new string[modelsup.ListaRNS.Count()][];
                foreach (VcrVerrnsDTO item in modelsup.ListaRNS)
                {
                    string[] itemDato = { item.Vcvrnsfecha.Value.ToString("dd/MM/yyyy"), item.Vcvrnshorinicio.Value.ToString("HH:mm"), item.Vcvrnshorfinal.Value.ToString("HH:mm"), item.EmprNombre.ToString(), item.Gruponomb.ToString(), item.Vcvrnsrns.ToString() };
                    data[index] = itemDato;
                    index++;
                }
            }
            else
            {
                data = new string[1][];
                string[] itemDato = { "", "" };
                data[index] = itemDato;
            }

            ///////////////////ARMANDO COLUMNASSSSSSSSSSSSSSSSSSSSSS!!
            columnas[0] = new
            {   //FECHA
                type = GridExcelModel.TipoFecha,
                source = "",
                strict = false,
                correctFormat = true,
                className = "htCenter",
                readOnly = false,
            };
            columnas[1] = new
            {   //HORA INICIO
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htCenter",
                readOnly = false,
            };
            columnas[2] = new
            {   //HORA FIN
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htCenter",
                readOnly = false,
            };
            columnas[3] = new
            {   //EMPRESA
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[4] = new
            {   //URS
                type = GridExcelModel.TipoLista,
                source = ListaURS.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[5] = new
            {   //DÉFICIT(MW)
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "000000000000,0.000000000000",
                readOnly = false,
            };

            #endregion
            //model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
            //model.ListaCentralGeneracion = ListaCentrales.ToArray();

            model.Data = data;
            model.Headers = headers;
            model.Widths = widths;
            model.Columnas = columnas;

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcelRNS(int vcrdsrcodi, string[][] datos, int vcvrnstipocarga = 1)
        {
            base.ValidarSesionUsuario();
            DefSupResNoSuminModel model = new DefSupResNoSuminModel();

            try
            {
                //////////////Eliminando datos////////
                Log.Info("Eliminar registro - DeleteVcrVerrns");
                this.servicioCompensacionRSF.DeleteVcrVerrns(vcrdsrcodi, vcvrnstipocarga);

                //Recorrer matriz para grabar detalle: se inicia en la fila 1
                for (int f = 0; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f] == null || datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadRNS = new VcrVerrnsDTO();
                    model.EntidadRNS.Vcvrnscodi = 0;
                    model.EntidadRNS.Vcrdsrcodi = vcrdsrcodi;
                    model.EntidadRNS.Vcvrnstipocarga = vcvrnstipocarga;
                    model.EntidadRNS.Vcvrnsusucreacion = User.Identity.Name;
                    model.EntidadRNS.Vcvrnsfeccreacion = DateTime.Now;
                    if (!datos[f][0].Equals(""))
                    {
                        string fecha = Convert.ToString(datos[f][0]);
                        string horainicio = Convert.ToString(datos[f][1]);
                        string horafin = Convert.ToString(datos[f][2]);
                        if (horainicio.Equals("24:00"))
                            horainicio = "23:59";
                        if (horafin.Equals("24:00"))
                            horafin = "23:59";
                        string empresa = Convert.ToString(datos[f][3]);
                        string urs = Convert.ToString(datos[f][4]);
                        string valor = Convert.ToString(datos[f][5]);

                        model.EntidadRNS.Vcvrnsfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        model.EntidadRNS.Vcvrnshorinicio = DateTime.ParseExact(fecha + " " + horainicio, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        model.EntidadRNS.Vcvrnshorfinal = DateTime.ParseExact(fecha + " " + horafin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture); 
                        model.EntidadRNS.Emprcodi = this.ServicioEmpresa.GetByNombre(empresa).EmprCodi;
                        Log.Info("EntidadRNS - GetByNombrePrGrupo");
                        model.EntidadRNS.Grupocodi = this.serviciobarraUrs.GetByNombrePrGrupo(urs).GrupoCodi;
                        model.EntidadRNS.Gruponomb = urs;
                        model.EntidadRNS.Vcvrnsrns = UtilCompensacionRSF.ValidarNumero(valor);
                    }
                    //Insertar registro
                    Log.Info("Insertar registro - SaveVcrVerrns");
                    this.servicioCompensacionRSF.SaveVcrVerrns(model.EntidadRNS);
                }
                model.sError = "";
                model.sMensaje = "Felicidades, la carga de información fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }

        /// <summary>
        /// Permite eliminar todos los registros de la versión 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatosRNS(int vcrdsrcodi, int vcvrnstipocarga = 1)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                Log.Info("Eliminar registro - DeleteVcrVerrns");
                this.servicioCompensacionRSF.DeleteVcrVerrns(vcrdsrcodi, vcvrnstipocarga);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }

        /// <summary>
        /// Permite exportar a un archivo excel todos los registros en pantalla de consulta
        /// </summary>
        /// <param name="vcrdsrcodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataRNS(int vcrdsrcodi = 0, int vcvrnstipocarga = 1, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                Log.Info("Exportar información - GenerarFormatoVcrRNS");
                string file = this.servicioCompensacionRSF.GenerarFormatoVcrRNS(vcrdsrcodi, vcvrnstipocarga, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        /// <summary>
        /// Lee datos desde el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoRNS(string sarchivo, int vcrdsrcodi = 0, int vcvrnstipocarga = 1)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";

            try
            {
                #region Armando de contenido
                //Definimos la cabecera como una matriz
                string[] header = { "FECHA", "HORA INICIO", "HORA FIN", "EMPRESA", "URS", "RNS(MW)" };
                //Ancho de cada columna
                int[] widths = { 150, 100, 100, 300, 300, 150 };
                string[] headers = header.ToArray(); //Headers final a enviar
                object[] columnas = new object[6];

                //obtener las empresas para el dropdown
                Log.Info("ListaEmpresas - ListEmpresas");
                var ListaEmpresas = this.ServicioEmpresa.ListEmpresas().Select(x => x.EmprNombre).ToList();
                //obtener las URS matriculadas para el dropdown
                Log.Info("ListaURS - ListURS");
                var ListaURS = this.serviciobarraUrs.ListURS().Select(x => x.GrupoNomb).ToList();
                //string[] aEmpresa = { "Empresa1", "Empresa2", "Empresa3" };

                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioCompensacionRSF.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 4][]; // Lee todo el contenido del excel y le descontamos 4 filas hasta donde empieza la data
                int index = 0;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 5)
                    {
                        continue;
                    }
                    int iNumFila = iFila + 1;
                    string sEmpresa = dtRow[4].ToString();
                    EmpresaDTO dtoEmpresa = this.ServicioEmpresa.GetByNombre(sEmpresa);
                    if (dtoEmpresa == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sEmpresa;
                        iRegError++;
                        continue;
                    }
                    string sURS = dtRow[5].ToString();
                    Log.Info("Entidad TrnBarraurs - GetByNombrePrGrupo");
                    TrnBarraursDTO dtoUrs = this.serviciobarraUrs.GetByNombrePrGrupo(sURS);
                    if (dtoUrs == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sURS;
                        iRegError++;
                        continue;
                    }
                    string[] itemDato = { dtRow[1].ToString(), dtRow[2].ToString(), dtRow[3].ToString(), sEmpresa, sURS, dtRow[6].ToString() };
                    data[index] = itemDato;
                    index++;
                }

                ///////////////////ARMANDO COLUMNASSSSSSSSSSSSSSSSSSSSSS!!
                columnas[0] = new
                {   //FECHA
                    type = GridExcelModel.TipoFecha,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htCenter",
                    readOnly = false,
                };
                columnas[1] = new
                {   //HORA INICIO
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htCenter",
                    readOnly = false,
                };
                columnas[2] = new
                {   //HORA FIN
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htCenter",
                    readOnly = false,
                };
                columnas[3] = new
                {   //EMPRESA
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[4] = new
                {   //URS
                    type = GridExcelModel.TipoLista,
                    source = ListaURS.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[5] = new
                {   //RNS(MW)
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "000000000000,0.000000000000",
                    readOnly = false,
                };

                #endregion
                model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
                model.ListaEmpresas = ListaEmpresas.ToArray();
                model.ListaURS = ListaURS.ToArray();

                model.Data = data;
                model.Headers = headers;
                model.Widths = widths;
                model.Columnas = columnas;
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;

                return Json(model);
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                return Json(sResultado);
            }
        }


        #endregion

        #region FUNCIONES GENERALES PARA TODOS LOS TABS

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            return File(path, app, sFecha + "_" + file);
        }

        /// <summary>
        /// Permite cargar un archivo Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
