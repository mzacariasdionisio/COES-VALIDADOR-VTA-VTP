using COES.Dominio.DTO.Sic;
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
    public class SaldoMesDirectoController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        CompensacionRSFAppServicio servicioCompensacionRSF = new CompensacionRSFAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        CentralGeneracionAppServicio servicioCentralGeneracion = new CentralGeneracionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();

        // GET: /CompensacionRSF/SaldoMesDirecto/

        public ActionResult Index(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            CalculoCompRSFModel model = new CalculoCompRSFModel();
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                pericodi = model.ListaPeriodos[0].PeriCodi;

            }
            Log.Info("Entidad Periodo - GetByIdPeriodo");
            model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);

            Log.Info("Lista de Versiones - ListVcrRecalculos");
            model.ListaRecalculo = this.servicioCompensacionRSF.ListVcrRecalculos(pericodi);
            if (model.ListaRecalculo.Count > 0 && vcrecacodi == 0)
            {
                vcrecacodi = (int)model.ListaRecalculo[0].Vcrecacodi;
            }

            if (pericodi > 0 && vcrecacodi > 0)
            {
                Log.Info("EntidadRecalculo - GetByIdVcrRecalculoView");
                model.EntidadRecalculo = this.servicioCompensacionRSF.GetByIdVcrRecalculoView(pericodi, vcrecacodi);
            }
            else
            {
                model.EntidadRecalculo = new VcrRecalculoDTO();
            }
            model.Pericodi = pericodi;
            model.Vcrecacodi = vcrecacodi;
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return View(model);
        }

        #region GRILLA

        /// <summary>
        /// Muestra la grilla excel con los registros de Saldo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelSD(int vcrecacodi = 0)
        {
            GridExcelModel model = new GridExcelModel();
            CalculoCompRSFModel modelCalculo = new CalculoCompRSFModel();

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] header = { "EMPRESA", "CENTRAL", "UNIDAD", "SALDO" };
            //Ancho de cada columna
            int[] widths = { 250, 200, 200, 200 };
            string[] headers = header.ToArray(); //Headers final a enviar
            object[] columnas = new object[4];
            //Obtener las centrales
            Log.Info("Lista Central Generación - ListCentralGeneracion");
            var ListaCentrales = this.servicioCentralGeneracion.ListCentralGeneracion().Select(x => x.CentGeneNombre).ToList();
            //Obtener las unidades
            Log.Info("Lista Central Unidad - ListUnidad");
            var ListaUnidades = this.servicioCentralGeneracion.ListUnidad().Select(x => x.CentGeneNombre).ToList();
            //obtener las empresas
            Log.Info("ListaEmpresas - ListEmpresas");
            var ListaEmpresas = this.servicioEmpresa.ListEmpresas().Select(x => x.EmprNombre).ToList();

            //obtener las empresas, centrales y unidades con sus respectivos saldos actuales.
            modelCalculo.ListaIncumpl = this.servicioCompensacionRSF.ListVcrCargoincumpls(vcrecacodi);
            //se arma la matriz de datos
            string[][] data;
            int index = 0;
            if (modelCalculo.ListaIncumpl.Count() != 0)
            {
                data = new string[modelCalculo.ListaIncumpl.Count()][];
                foreach (VcrCargoincumplDTO item in modelCalculo.ListaIncumpl)
                {
                    int codigoPadre = 0;
                    var Unidad = this.servicioCompensacionRSF.GetByEquicodi(item.Equicodi);//se obtiene la unidad
                    if (Unidad.Famcodi == 4 && Unidad.Equipadre == 0) 
                    {
                        codigoPadre = item.Equicodi;
                    }
                    else 
                    {
                        codigoPadre = (int)Unidad.Equipadre;
                    }
                   
                    var Central = this.servicioCompensacionRSF.GetByEquicodi(codigoPadre);//se obtiene la central
                    if (Central != null)
                    {
                        Unidad.Equinomb = Unidad.Equinomb.ToString() + " [" + Central.Equinomb.ToString() + "]";
                    }
                    int codigoEmpresa = (int)Central.Emprcodi;
                    var Empresa = this.servicioEmpresa.GetByIdEmpresa(codigoEmpresa);

                    

                    string[] itemDato = { Empresa.EmprNombre.ToString(), 
                                          Central.Equinomb.ToString(),
                                          Unidad.Equinomb.ToString(),
                                          item.Vcrcisaldomes.ToString() };
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

            //----------------------ARMANDO COLUMNAS
            columnas[0] = new
            {   //EMPRESA
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[1] = new
            {   //CENTRAL
                type = GridExcelModel.TipoLista,
                source = ListaCentrales.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[2] = new
            {   //UNIDAD
                type = GridExcelModel.TipoLista,
                source = ListaUnidades.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[3] = new
            {   //SALDOS
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
        public JsonResult GrabarGrillaExcelSD(int pericodi, int vcrecacodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            CalculoCompRSFModel modelCalculo = new CalculoCompRSFModel();

            try
            {
                //Recorrer matriz para grabar detalle: se inicia en la fila 1
                for (int f = 0; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f] == null || datos[f][0] == null)
                        break; //FIN

                    //ACTUALIZAR EL REGISTRO
                    modelCalculo.EntidadIncumpl = new VcrCargoincumplDTO();
                                            
                    if (!datos[f][0].Equals(""))
                    {
                        string nombreEmpresa = Convert.ToString(datos[f][0]).Trim();
                        string nombreCentral = Convert.ToString(datos[f][1]).Trim();
                        string nombreUnidad = Convert.ToString(datos[f][2]).Trim();
                        string seudoUnidad = nombreUnidad;
                   
                        if (nombreUnidad.IndexOf("[") > 0)
                        {
                            nombreUnidad = nombreUnidad.Substring(0,nombreUnidad.IndexOf("[") - 1).Trim();
                            var unidadBase = seudoUnidad.Split('[');
                            var unidadRecortada = unidadBase[1].Split(']');
                            if (unidadRecortada[0].Trim() != nombreCentral) 
                            {
                                modelCalculo.sError = "La central de la linea " + (f + 1).ToString() + ": " + nombreCentral + " no pertenece a la unidad"; //"-1"
                                return Json(modelCalculo);
                            }
                            
                        }
                        string saldo = Convert.ToString(datos[f][3]);

                        //Validamos la empresa
                        var Empresa = this.servicioEmpresa.GetByNombre(nombreEmpresa);
                        //if (Empresa.EmprCodi <= 0)
                        //{
                        //    modelCalculo.sError = "La empresa de la linea " + (f + 1).ToString() + ": " + nombreEmpresa + " no existe"; //"-1"
                        //    return Json(modelCalculo);
                        //}
                        //Validamos la Central

                        var Central = this.servicioCompensacionRSF.GetByEquiNomb(nombreCentral);
                    
                        if ((int)Central.Equicodi <= 0)
                        {   //ASSETEC - 20181205
                            modelCalculo.sError = "La central de la linea " + (f + 1).ToString() + ": " + nombreCentral + " no existe"; //"-1"
                            return Json(modelCalculo);
                        }
                       
                        int codigoCentral = (int)Central.Equicodi;
                       
                        var Unidad = this.servicioCentralGeneracion.ListUnidadCentral(codigoCentral);
              
                        int codigoUnidad = 0;
                        foreach (var item in Unidad)
                        {
                            if (item.CentGeneNombre.Equals(nombreUnidad))
                            {
                                codigoUnidad = item.CentGeneCodi;
                            }
      
                        }

                        if (codigoUnidad == 0)
                        {
                            var unidadVal = this.servicioCompensacionRSF.GetByEquiNomb(nombreUnidad);
                            if(unidadVal.Famcodi==4){
                                codigoUnidad = (int)Central.Equicodi;
                            }
                            else{
                                modelCalculo.sError = "La Unidad de la linea " + (f + 1).ToString() + ": " + nombreUnidad + " no corresponde a la Central " + nombreCentral;
                                return Json(modelCalculo);
                                }
                        }

                        modelCalculo.EntidadIncumpl = this.servicioCompensacionRSF.GetByIdVcrCargoincumpl(vcrecacodi, codigoUnidad);
                        if (modelCalculo.EntidadIncumpl != null)
                        {
                            modelCalculo.EntidadIncumpl.Vcrcisaldomes = UtilCompensacionRSF.ValidarNumero(saldo);
                            //Actualizar Registro
                            Log.Info("Actualizar registro - UpdateVcrCargoincumpl");
                            this.servicioCompensacionRSF.UpdateVcrCargoincumpl(modelCalculo.EntidadIncumpl);
                        }
                        else
                        {
                            //Ingresar Registro
                            modelCalculo.EntidadIncumpl = new VcrCargoincumplDTO();
                            modelCalculo.EntidadIncumpl.Vcrcicodi = 0;
                            modelCalculo.EntidadIncumpl.Vcrecacodi = vcrecacodi;
                            modelCalculo.EntidadIncumpl.Equicodi = codigoUnidad;
                            modelCalculo.EntidadIncumpl.Vcrcicargoincumplmes = 0;
                            modelCalculo.EntidadIncumpl.Vcrcisaldoanterior = 0;
                            modelCalculo.EntidadIncumpl.Vcrcicargoincumpl = 0;
                            modelCalculo.EntidadIncumpl.Vcrcicarginctransf = 0;
                            modelCalculo.EntidadIncumpl.Vcrcisaldomes = UtilCompensacionRSF.ValidarNumero(saldo);
                            modelCalculo.EntidadIncumpl.Pericodidest = 0;
                            modelCalculo.EntidadIncumpl.Vcrciusucreacion = User.Identity.Name;
                            modelCalculo.EntidadIncumpl.Vcrcifeccreacion = DateTime.Now;
                            
                            Log.Info("Ingresar registro - SaveVcrCargoincumpl");
                            this.servicioCompensacionRSF.SaveVcrCargoincumpl(modelCalculo.EntidadIncumpl);
                        }                   
              
                    }
                }
                modelCalculo.sError = "";
                modelCalculo.sMensaje = "Felicidades, la carga de información fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(modelCalculo);
            }
            catch (Exception e)
            {
                modelCalculo.sError = e.Message; //"-1"
                return Json(modelCalculo);
            }
        }

        /// <summary>
        /// Permite exportar a un archivo excel todos los registros en pantalla de consulta
        /// </summary>
        /// <param name="vcrdsrcodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataSD(int pericodi = 0, int vcrecacodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                Log.Info("Exportar información - GenerarFormatoVcrDef");
                string file = this.servicioCompensacionRSF.GenerarFormatoVcrSaldos(pericodi, vcrecacodi, formato, pathFile, pathLogo);
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
        public JsonResult ProcesarArchivoSD(string sarchivo, int pericodi = 0, int vcrecacodi = 0)
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
                string[] header = { "EMPRESA", "CENTRAL", "UNIDAD", "SALDO" };
                //Ancho de cada columna
                int[] widths = { 250, 200, 200, 200 };
                string[] headers = header.ToArray(); //Headers final a enviar
                object[] columnas = new object[4];

                //Obtener las centrales
                Log.Info("Lista Central Generación - ListCentralGeneracion");
                var ListaCentrales = this.servicioCentralGeneracion.ListCentralGeneracion().Select(x => x.CentGeneNombre).ToList();
                //Obtener las unidades
                Log.Info("Lista Central Unidad - ListUnidad");
                var ListaUnidades = this.servicioCentralGeneracion.ListUnidad().Select(x => x.CentGeneNombre).ToList();
                //obtener las empresas
                Log.Info("ListaEmpresas - ListEmpresas");
                var ListaEmpresas = this.servicioEmpresa.ListEmpresas().Select(x => x.EmprNombre).ToList();

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
                    
                    string[] itemDato = { dtRow[1].ToString(), dtRow[2].ToString(), dtRow[3].ToString(), dtRow[4].ToString() };
                    data[index] = itemDato;
                    index++;
                }

                //----------------------ARMANDO COLUMNAS
                columnas[0] = new
                {   //EMPRESA
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[1] = new
                {   //CENTRAL
                    type = GridExcelModel.TipoLista,
                    source = ListaCentrales.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[2] = new
                {   //UNIDAD
                    type = GridExcelModel.TipoLista,
                    source = ListaUnidades.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[3] = new
                {   //SALDOS
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "000000000000,0.000000000000",
                    readOnly = false,
                };

                #endregion
                model.Grabar = false;
                //model.ListaEmpresas = ListaEmpresas.ToArray();
                //model.ListaURS = ListaURS.ToArray();

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

        /// <summary>
        /// Permite eliminar todos los registros de la versión 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatosSD(int vcrecacodi)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                Log.Info("Eliminar información - DeleteVcrCargoincumpl");
                this.servicioCompensacionRSF.DeleteVcrCargoincumpl(vcrecacodi);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

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
