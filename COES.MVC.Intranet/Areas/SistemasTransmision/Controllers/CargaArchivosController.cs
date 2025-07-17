using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.SistemasTransmision.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.SistemasTransmision;
using COES.Servicios.Aplicacion.SistemasTransmision.Helper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Controllers
{
    public class CargaArchivosController : BaseController
    {
        // GET: /SistemasTransmision/CargaArchivos

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        SistemasTransmisionAppServicio servicioSistemasTransmision = new SistemasTransmisionAppServicio();
        CentralGeneracionAppServicio servicioCentral = new CentralGeneracionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();

        public ActionResult Index(int stpercodi = 0, int strecacodi = 0, int emprcodi = 0)
        {
            base.ValidarSesionUsuario();
            CargaArchivosModel model = new CargaArchivosModel();
            model.ListaPeriodos = this.servicioSistemasTransmision.ListStPeriodos();
            if (model.ListaPeriodos.Count > 0 && stpercodi == 0)
            { stpercodi = model.ListaPeriodos[0].Stpercodi; }
            if (stpercodi > 0)
            {
                model.ListaRecalculo = this.servicioSistemasTransmision.ListStRecalculos(stpercodi); //Ordenado en descendente
                if (model.ListaRecalculo.Count > 0 && strecacodi == 0)
                { strecacodi = (int)model.ListaRecalculo[0].Strecacodi; }
            }

            if (stpercodi > 0 && strecacodi > 0)
            {
                model.EntidadRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(strecacodi);
            }
            else
            {
                model.EntidadRecalculo = new StRecalculoDTO();
            }
            //model.ListaEmpresas = this.servicioEmpresa.ListaEmpresasCombo();
            //model.ListaBarras = this.servicioBarra.ListBarras();
            model.Stpercodi = stpercodi;
            model.Strecacodi = strecacodi;
            //model.Emprcodi = emprcodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        #region Grilla Excel DISTANCIAS ELECTRICAS

        /// <summary>
        /// Muestra la grilla excel con los registros de Distancias Electricas
        /// </summary>
        /// <param name="stpercodi">Código del Mes de carga de datos</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelDE(int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            CargaArchivosModel modelde = new CargaArchivosModel();
            modelde.Stpercodi = stpercodi;
            modelde.Strecacodi = strecacodi;
            //Obtener los sistemas de transmision para el dropdown
            //var ListaSistemas = this.servicioSistemasTransmision.ListStSistematranss(modelde.Strecacodi).Select(x => x.Sistrnnombre).ToList();
            var ListaBarras = this.servicioBarra.ListBarras().Select(x => x.BarrNombre).ToList();
            //Lista de Energias netas del periodo
            modelde.ListaDistelectrica = this.servicioSistemasTransmision.GetByCriteriaStDistelectricas(modelde.Strecacodi);

            #region Armando de contenido
            //Lista de Barras, solo listo barras 1
            //modelde.ListaCompensacion = this.servicioSistemasTransmision.GetByCriteriaStCompensacions(modelde.Strecacodi);
            modelde.ListaSTBarra = this.servicioSistemasTransmision.GetByCriteriaStBarra(modelde.Strecacodi);
            int iNroColumnas = modelde.ListaSTBarra.Count * 2;
            //Definimos la cabecera como una matriz
            string[] Cabecera1 = { "BARRA" }; //Titulos de cada columna
            string[] Cabecera2 = { "" };
            int[] widths = { 350 }; //Ancho de cada columna
            string[] itemDato = { "" };

            //La lista de barras es dinamica
            if (iNroColumnas > 0)
            {
                Array.Resize(ref Cabecera1, Cabecera1.Length + iNroColumnas);
                Array.Resize(ref Cabecera2, Cabecera2.Length + iNroColumnas);
                Array.Resize(ref widths, widths.Length + iNroColumnas);
                Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
            }
            //Formato de columnas
            object[] columnas = new object[iNroColumnas + 1];
            columnas[0] = new
            {   //BARRCODI - BARRNOMBRE
                type = GridExcelModel.TipoLista,
                source = ListaBarras.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false,
            };
            int iAux = 1;
            foreach (StBarraDTO dto in modelde.ListaSTBarra)
            {
                Cabecera1[iAux] = dto.Barrnomb;
                Cabecera1[iAux + 1] = "";
                Cabecera2[iAux] = "R(pu)";
                Cabecera2[iAux + 1] = "X(pu)";
                widths[iAux] = widths[iAux + 1] = 100;
                itemDato[iAux] = itemDato[iAux + 1] = "";
                columnas[iAux] = new
                {   //R(pu)
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00000000000000000000",
                    readOnly = false,
                };
                columnas[iAux + 1] = new
                {   //X(pu)
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00000000000000000000",
                    readOnly = false,
                };
                iAux = iAux + 2;
            }

            //Se arma la matriz de datos
            string[][] data;
            int index = 2;
            if (modelde.ListaDistelectrica.Count() != 0)
            {
                data = new string[modelde.ListaDistelectrica.Count() + 2][];
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                foreach (StDistelectricaDTO item in modelde.ListaDistelectrica)
                {
                    string[] itemDato2 = { item.Barrnombre.ToString() };
                    Array.Resize(ref itemDato2, itemDato2.Length + iNroColumnas);
                    int iAux2 = 1;
                    foreach (StBarraDTO dto in modelde.ListaSTBarra)
                    {
                        modelde.EntidadDistelecBarra = this.servicioSistemasTransmision.GetByIdStDsteleBarra(item.Dstelecodi, dto.Barrcodi);
                        if (modelde.EntidadDistelecBarra != null)
                        {
                            itemDato2[iAux2++] = modelde.EntidadDistelecBarra.Delbarrpu.ToString();
                            itemDato2[iAux2++] = modelde.EntidadDistelecBarra.Delbarxpu.ToString();
                        }
                    }
                    data[index] = itemDato2;
                    index++;
                }
            }
            else
            {
                data = new string[3][];
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                data[index] = itemDato;
            }


            #endregion
            model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
            model.ListaBarras = ListaBarras.ToArray();
            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;
            model.NumeroColumnas = iNroColumnas; //Es el numero de columnas dobles por barra
            model.FixedColumnsLeft = 1;
            model.FixedRowsTop = 2;
            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcelDE(int stpercodi, int strecacodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            CargaArchivosModel model = new CargaArchivosModel();

            if (stpercodi == 0 || strecacodi == 0)
            {
                model.sError = "Lo sentimos, debe seleccionar un mes y un versión";
                return Json(model);
            }
            try
            {
                //////////////Eliminando datos////////
                this.servicioSistemasTransmision.DeleteStDsteleBarra(strecacodi);
                this.servicioSistemasTransmision.DeleteStDistelectrica(strecacodi);
                ///////////////////////////////////
                //List<StCompensacionDTO> ListaCompensacion = this.servicioSistemasTransmision.GetByCriteriaStCompensacions(strecacodi);
                List<StBarraDTO> ListaBarra = this.servicioSistemasTransmision.GetByCriteriaStBarra(strecacodi);
                int iNumBarras = ListaBarra.Count;
                //Recorrer matriz para grabar detalle: se inicia en la fila 1
                for (int f = 2; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f] == null || datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadDistelectrica = new StDistelectricaDTO();
                    model.EntidadDistelectrica.Strecacodi = strecacodi;
                    model.EntidadDistelectrica.Dsteleusucreacion = User.Identity.Name;
                    if (!datos[f][0].Equals(""))
                    {
                        string sBarra = Convert.ToString(datos[f][0]);
                        BarraDTO dtoBarra = this.servicioBarra.GetByBarra(sBarra);
                        if (dtoBarra != null)
                        {
                            model.EntidadDistelectrica.Barrcodi = dtoBarra.BarrCodi;
                            //Insertar registro en Distancia Electrica
                            int iDstelecodi = this.servicioSistemasTransmision.SaveStDistelectrica(model.EntidadDistelectrica);
                            //Preparamos la data para registrar los detalles
                            for (int c = 1; c < iNumBarras * 2; c = c + 2)
                            {
                                model.EntidadDistelecBarra = new StDsteleBarraDTO();
                                model.EntidadDistelecBarra.Dstelecodi = iDstelecodi;
                                //barra
                                foreach (StBarraDTO dto in ListaBarra)
                                {
                                    if (dto.Barrnomb == datos[0][c].ToString())
                                    {
                                        model.EntidadDistelecBarra.Barrcodi = dto.Barrcodi;
                                        break;
                                    }

                                }
                                //R(pu)
                                model.EntidadDistelecBarra.Delbarrpu = UtilSistemasTransmision.ValidarNumero(datos[f][c].ToString());
                                //X(pu)
                                model.EntidadDistelecBarra.Delbarxpu = UtilSistemasTransmision.ValidarNumero(datos[f][c + 1].ToString());
                                this.servicioSistemasTransmision.SaveStDsteleBarra(model.EntidadDistelecBarra);
                            }
                        }
                        else
                        { continue; }
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
        /// Permite eliminar todos los registros del periodo y versión 
        /// </summary>
        /// <param name="stpercodi">Código del Mes de carga de datos</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatosDE(int stpercodi, int strecacodi)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                this.servicioSistemasTransmision.DeleteStDsteleBarra(strecacodi);
                this.servicioSistemasTransmision.DeleteStDistelectrica(strecacodi);
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
        /// <param name="stpercodi">Código del Mes de carga de datos</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataDE(int stpercodi = 0, int strecacodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                string file = this.servicioSistemasTransmision.GenerarFormatoStDistelectricas(strecacodi, formato, pathFile, pathLogo);
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
        /// <param name="stpercodi">Código del Mes de valorización</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoDE(string sarchivo, int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
            int iRegError = 0;
            string sMensajeError = "";
            CargaArchivosModel modelde = new CargaArchivosModel();
            modelde.Stpercodi = stpercodi;
            modelde.Strecacodi = strecacodi;
            //Obtener los sistemas de transmision para el dropdown
            //var ListaSistemas = this.servicioSistemasTransmision.ListStSistematranss(modelde.Strecacodi).Select(x => x.Sistrnnombre).ToList();
            var ListaBarras = this.servicioBarra.ListBarras().Select(x => x.BarrNombre).ToList();
            try
            {
                #region Armando de contenido
                //Lista de Barras, solo listo barras 1
                //modelde.ListaCompensacion = this.servicioSistemasTransmision.GetByCriteriaStCompensacions(modelde.Strecacodi);
                modelde.ListaSTBarra = this.servicioSistemasTransmision.GetByCriteriaStBarra(modelde.Strecacodi);
                int iNroColumnas = modelde.ListaSTBarra.Count * 2;
                //Definimos la cabecera como una matriz
                string[] Cabecera1 = { "BARRA" }; //Titulos de cada columna
                string[] Cabecera2 = { "" };
                int[] widths = { 350 }; //Ancho de cada columna
                string[] itemDato = { "" };

                //La lista de barras es dinamica
                if (iNroColumnas > 0)
                {
                    Array.Resize(ref Cabecera1, Cabecera1.Length + iNroColumnas);
                    Array.Resize(ref Cabecera2, Cabecera2.Length + iNroColumnas);
                    Array.Resize(ref widths, widths.Length + iNroColumnas);
                    Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
                }
                //Formato de columnas
                object[] columnas = new object[iNroColumnas + 1];
                columnas[0] = new
                {   //SISTRNCODI - SISTRNNOMBRE
                    type = GridExcelModel.TipoLista,
                    source = ListaBarras.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                int iAux = 1;
                foreach (StBarraDTO dto in modelde.ListaSTBarra)
                {
                    Cabecera1[iAux] = dto.Barrnomb;
                    Cabecera1[iAux + 1] = "";
                    Cabecera2[iAux] = "R(pu)";
                    Cabecera2[iAux + 1] = "X(pu)";
                    widths[iAux] = widths[iAux + 1] = 100;
                    itemDato[iAux] = itemDato[iAux + 1] = "";
                    columnas[iAux] = new
                    {   //R(pu)
                        type = GridExcelModel.TipoNumerico,
                        source = (new List<String>()).ToArray(),
                        strict = false,
                        correctFormat = true,
                        className = "htRight",
                        format = "0,0.00000000000000000000",
                        readOnly = false,
                    };
                    columnas[iAux + 1] = new
                    {   //X(pu)
                        type = GridExcelModel.TipoNumerico,
                        source = (new List<String>()).ToArray(),
                        strict = false,
                        correctFormat = true,
                        className = "htRight",
                        format = "0,0.00000000000000000000",
                        readOnly = false,
                    };
                    iAux = iAux + 2;
                }

                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioSistemasTransmision.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 2][]; // Lee todo el contenido del excel y le descontamos 4 filas hasta donde empieza la data
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                int index = 2;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 6)
                    {
                        continue;
                    }
                    int iNumFila = iFila + 1;
                    string sBarra = dtRow[1].ToString();
                    //StSistematransDTO dtoSistema = this.servicioSistemasTransmision.GetBySisTransNomb(sSistema);
                    BarraDTO dtoBarra = this.servicioBarra.GetByBarra(sBarra);
                    if (dtoBarra == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sBarra;
                        iRegError++;
                        continue;
                    }
                    string[] itemDato2 = { sBarra };
                    Array.Resize(ref itemDato2, itemDato2.Length + iNroColumnas);
                    for (int iAux2 = 1; iAux2 <= iNroColumnas; iAux2++)
                    {
                        itemDato2[iAux2] = dtRow[iAux2 + 1].ToString();
                    }
                    data[index] = itemDato2;
                    index++;
                }

                #endregion
                model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
                model.ListaBarras = ListaBarras.ToArray();
                model.Data = data;
                model.Widths = widths;
                model.Columnas = columnas;
                model.NumeroColumnas = iNroColumnas; //Es el numero de columnas dobles por barra
                model.FixedColumnsLeft = 1;
                model.FixedRowsTop = 2;
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;
                return Json(model);
            }
            catch (Exception e)
            {
                model.MensajeError = e.Message;
                return Json(model);
            }

        }

        #endregion

        #region Grilla Excel ENERGIAS NETAS

        /// <summary>
        /// Muestra la grilla excel con los registros de Egresos y Peajes - Mejor información
        /// </summary>
        /// <param name="stpercodi">Código del Mes de carga de datos</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelEN(int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            CargaArchivosModel modelen = new CargaArchivosModel();
            modelen.Stpercodi = stpercodi;
            modelen.Strecacodi = strecacodi;

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] header = { "CENTRAL", "ENERGÍA" };
            //Ancho de cada columna
            int[] widths = { 350, 200 };
            string[] headers = header.ToArray(); //Headers final a enviar
            object[] columnas = new object[2];

            //Obtener las centrales de generación para el dropdown
            //var ListaCentrales = this.servicioCentral.ListCentralGeneracion().Select(x => x.CentGeneNombre).ToList();
            var ListaCentrales = this.servicioSistemasTransmision.GetByCriteriaStCentralgens(strecacodi).Select(x => x.Equinomb).ToList();
            //Lista de Energias netas del periodo
            modelen.ListaEnergiaNeta = this.servicioSistemasTransmision.GetByCriteriaStEnergias(modelen.Strecacodi);
            //Se arma la matriz de datos
            string[][] data;
            int index = 0;
            if (modelen.ListaEnergiaNeta.Count() != 0)
            {
                data = new string[modelen.ListaEnergiaNeta.Count()][];
                foreach (StEnergiaDTO item in modelen.ListaEnergiaNeta)
                {
                    if (item.Equinomb == null) item.Equinomb = "No hay Equipo";
                    string[] itemDato = { item.Equinomb.ToString(), item.Stenrgrgia.ToString() };
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
            {   //Equicodi - Equinomb
                type = GridExcelModel.TipoLista,
                source = ListaCentrales.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false,
            };
            columnas[1] = new
            {   //Precio Potencia
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
            model.ListaCentralGeneracion = ListaCentrales.ToArray();

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
        public JsonResult GrabarGrillaExcelEN(int stpercodi, int strecacodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            CargaArchivosModel model = new CargaArchivosModel();

            if (stpercodi == 0 || strecacodi == 0)
            {
                model.sError = "Lo sentimos, debe seleccionar un mes y un versión";
                return Json(model);
            }
            try
            {
                //////////////Eliminando datos////////
                this.servicioSistemasTransmision.DeleteStEnergia(strecacodi);
                ///////////////////////////////////

                //Recorrer matriz para grabar detalle: se inicia en la fila 1
                for (int f = 0; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f] == null || datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadEnergiaNeta = new StEnergiaDTO();
                    model.EntidadEnergiaNeta.Strecacodi = strecacodi;
                    model.EntidadEnergiaNeta.Stenrgusucreacion = User.Identity.Name;
                    if (!datos[f][0].Equals(""))
                    {
                        string sGenemprnombre = Convert.ToString(datos[f][0]);
                        CentralGeneracionDTO dtoCentral = this.servicioCentral.GetByCentGeneNombVsEN(sGenemprnombre, strecacodi);
                        if (dtoCentral != null)
                        {
                            model.EntidadEnergiaNeta.Equicodi = dtoCentral.CentGeneCodi;
                        }
                        else
                        {
                            model.sError = "Se ha interrumpido la carga de datos, al no encontrar a la central: " + sGenemprnombre;
                            return Json(model);
                        }
                    }
                    model.EntidadEnergiaNeta.Stenrgrgia = UtilSistemasTransmision.ValidarNumero(datos[f][1].ToString());

                    //Insertar registro
                    this.servicioSistemasTransmision.SaveStEnergia(model.EntidadEnergiaNeta);
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
        /// Permite eliminar todos los registros del periodo y versión de recalculo para mejor información
        /// </summary>
        /// <param name="stpercodi">Código del Mes de carga de datos</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatosEN(int stpercodi, int strecacodi)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                this.servicioSistemasTransmision.DeleteStEnergia(strecacodi);
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
        /// <param name="stpercodi">Código del Mes de carga de datos</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataEN(int stpercodi = 0, int strecacodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                string file = this.servicioSistemasTransmision.GenerarFormatoStEnergias(strecacodi, formato, pathFile, pathLogo);
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
        /// <param name="stpercodi">Código del Mes de valorización</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoEN(string sarchivo, int stpercodi = 0, int strecacodi = 0)
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
                string[] header = { "CENTRAL", "ENRRGÍA" };
                //Ancho de cada columna
                int[] widths = { 350, 200 };
                string[] headers = header.ToArray(); //Headers final a enviar
                object[] columnas = new object[2];

                //Obtener las centrales de generación para el dropdown
                var ListaCentrales = this.servicioSistemasTransmision.GetByCriteriaStCentralgens(strecacodi).Select(x => x.Equinomb).ToList();
                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioSistemasTransmision.GeneraDataset(path + sarchivo, 1);

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
                    string sGenemprnombre = dtRow[1].ToString();
                    StCentralgenDTO dtoCentral = this.servicioSistemasTransmision.GetByCentNomb(sGenemprnombre, strecacodi);
                    if (dtoCentral == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sGenemprnombre;
                        iRegError++;
                        continue;
                    }
                    string[] itemDato = { sGenemprnombre, dtRow[2].ToString() };
                    data[index] = itemDato;
                    index++;
                }
                ///////////////////ARMANDO COLUMNASSSSSSSSSSSSSSSSSSSSSS!!
                columnas[0] = new
                {   //Equicodi - Equinomb
                    type = GridExcelModel.TipoLista,
                    source = ListaCentrales.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                columnas[1] = new
                {   //Precio Potencia
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.000000000000",
                    readOnly = false,
                };
                #endregion
                model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
                model.ListaCentralGeneracion = ListaCentrales.ToArray();

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

        #region Grilla Excel FACTORES DE ACTUALIZACION

        /// <summary>
        /// Muestra la grilla excel con los registros de Egresos y Peajes - Mejor información
        /// </summary>
        /// <param name="stpercodi">Código del Mes de carga de datos</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelFA(int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            CargaArchivosModel modelen = new CargaArchivosModel();
            modelen.Stpercodi = stpercodi;
            modelen.Strecacodi = strecacodi;

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] header = { "SISTEMA", "FACTOR" };
            //Ancho de cada columna
            int[] widths = { 350, 200 };
            string[] headers = header.ToArray(); //Headers final a enviar
            object[] columnas = new object[2];

            //Obtener las centrales de generación para el dropdown
            var ListaSistemasTrans = this.servicioSistemasTransmision.ListStSistematranss(modelen.Strecacodi).Select(x => x.Sistrnnombre).ToList();
            //var ListaCentrales = this.servicioCentral.ListCentralGeneracion().Select(x => x.CentGeneNombre).ToList();
            //Lista de Energias netas del periodo
            modelen.ListaFactorActualizacion = this.servicioSistemasTransmision.GetByCriteriaStFactors(modelen.Strecacodi);
            //modelen.ListaEnergiaNeta = this.servicioSistemasTransmision.GetByCriteriaStEnergias(modelen.Strecacodi);
            //Se arma la matriz de datos
            string[][] data;
            int index = 0;
            if (modelen.ListaFactorActualizacion.Count() != 0)
            {
                data = new string[modelen.ListaFactorActualizacion.Count()][];
                foreach (StFactorDTO item in modelen.ListaFactorActualizacion)
                {
                    string[] itemDato = { item.SisTrnnombre.ToString(), item.Stfacttor.ToString() };
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
            {   //Equicodi - Equinomb
                type = GridExcelModel.TipoLista,
                source = ListaSistemasTrans.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false,
            };
            columnas[1] = new
            {   //Precio Potencia
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000000000000",
                readOnly = false,
            };

            #endregion
            model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
            model.ListaSistemasTrans = ListaSistemasTrans.ToArray();

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
        public JsonResult GrabarGrillaExcelFA(int stpercodi, int strecacodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            CargaArchivosModel model = new CargaArchivosModel();

            if (stpercodi == 0 || strecacodi == 0)
            {
                model.sError = "Lo sentimos, debe seleccionar un mes y un versión";
                return Json(model);
            }
            try
            {
                //Eliminamos los datos de Calculo para eliminar las relaciones con los factores//
                this.servicioSistemasTransmision.DeleteStDistelectricaGenele(strecacodi);
                this.servicioSistemasTransmision.DeleteStElementoCompensado(strecacodi);
                this.servicioSistemasTransmision.DeleteStFactorpago(strecacodi);
                //////////////Eliminando datos////////
                this.servicioSistemasTransmision.DeleteStFactor(strecacodi);
                ///////////////////////////////////

                //Recorrer matriz para grabar detalle: se inicia en la fila 1
                for (int f = 0; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f] == null || datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadFactorActualizacion = new StFactorDTO();
                    model.EntidadFactorActualizacion.Strecacodi = strecacodi;
                    model.EntidadFactorActualizacion.Stfactusucreacion = User.Identity.Name;
                    if (!datos[f][0].Equals(""))
                    {
                        string sSisTransnombre = Convert.ToString(datos[f][0]);
                        StSistematransDTO dtrSistemas = this.servicioSistemasTransmision.GetBySisTransNomb(strecacodi, sSisTransnombre);
                        if (dtrSistemas != null)
                        {
                            model.EntidadFactorActualizacion.Sistrncodi = dtrSistemas.Sistrncodi;
                        }
                        else
                        { continue; }
                    }
                    model.EntidadFactorActualizacion.Stfacttor = UtilSistemasTransmision.ValidarNumero(datos[f][1].ToString());

                    //Insertar registro
                    this.servicioSistemasTransmision.SaveStFactor(model.EntidadFactorActualizacion);
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
        /// Permite eliminar todos los registros del periodo y versión de recalculo para mejor información
        /// </summary>
        /// <param name="stpercodi">Código del Mes de carga de datos</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatosFA(int stpercodi, int strecacodi)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                this.servicioSistemasTransmision.DeleteStFactor(strecacodi);
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
        /// <param name="stpercodi">Código del Mes de carga de datos</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataFA(int stpercodi = 0, int strecacodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                string file = this.servicioSistemasTransmision.GenerarFormatoStFactor(strecacodi, formato, pathFile, pathLogo);
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
        /// <param name="stpercodi">Código del Mes de valorización</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoFA(string sarchivo, int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";
            CargaArchivosModel modelde = new CargaArchivosModel();
            modelde.Stpercodi = stpercodi;
            modelde.Strecacodi = strecacodi;
            try
            {
                #region Armando de contenido
                //Definimos la cabecera como una matriz
                string[] header = { "SISTEMA", "FACTOR" };
                //Ancho de cada columna
                int[] widths = { 350, 200 };
                string[] headers = header.ToArray(); //Headers final a enviar
                object[] columnas = new object[2];

                //Obtener las centrales de generación para el dropdown
                var ListaSistemasTrans = this.servicioSistemasTransmision.ListStSistematranss(modelde.Strecacodi).Select(x => x.Sistrnnombre).ToList();
                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioSistemasTransmision.GeneraDataset(path + sarchivo, 1);

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
                    string sSisTransnombre = dtRow[1].ToString();
                    StSistematransDTO dtrSistemas = this.servicioSistemasTransmision.GetBySisTransNomb(strecacodi, sSisTransnombre);
                    if (dtrSistemas == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sSisTransnombre;
                        iRegError++;
                        continue;
                    }
                    string[] itemDato = { sSisTransnombre, dtRow[2].ToString() };
                    data[index] = itemDato;
                    index++;
                }
                ///////////////////ARMANDO COLUMNASSSSSSSSSSSSSSSSSSSSSS!!
                columnas[0] = new
                {   //Equicodi - Equinomb
                    type = GridExcelModel.TipoLista,
                    source = ListaSistemasTrans.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                columnas[1] = new
                {   //Precio Potencia
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.000000000000",
                    readOnly = false,
                };
                #endregion
                model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
                model.ListaSistemasTrans = ListaSistemasTrans.ToArray();

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

        #region Grilla Excel CENTRALES DE RESPONSABILIDAD

        public JsonResult GrillaExcelCR(int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            CargaArchivosModel modelCR = new CargaArchivosModel();
            modelCR.Stpercodi = stpercodi;
            modelCR.Strecacodi = strecacodi;

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] header = { "CENTRAL GENERACIÓN" };
            //Ancho de cada columna
            int[] widths = { 250 };
            string[] itemDato = { "" };

            //Obtener los codigos de elementos para el dropdown
            var ListaCentralesGen = this.servicioSistemasTransmision.GetByCriteriaStCentralgens(modelCR.Strecacodi).Select(y => y.Equinomb).ToList();

            //Lista de Responsabilidades de Pago del periodo
            modelCR.ListaCentralesResp = this.servicioSistemasTransmision.GetByCriteriaStRespagos(modelCR.Strecacodi);
            modelCR.ListaCompensacion = this.servicioSistemasTransmision.GetByCriteriaStCompensacions(modelCR.Strecacodi);
            int iNroColumnas = modelCR.ListaCompensacion.Count;

            if (iNroColumnas > 0)
            {
                Array.Resize(ref header, header.Length + iNroColumnas);
                Array.Resize(ref widths, widths.Length + iNroColumnas);
                Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
            }

            object[] columnas = new object[iNroColumnas + 1];
            columnas[0] = new
            {   //Equicodi - Equinomb
                type = GridExcelModel.TipoLista,
                source = ListaCentralesGen.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false,
            };
            int iAux = 1;
            foreach (StCompensacionDTO dto in modelCR.ListaCompensacion)
            {
                header[iAux] = dto.Stcompcodelemento;
                widths[iAux] = 100;
                itemDato[iAux] = "1";
                columnas[iAux] = new
                {
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0",
                    readOnly = false,
                };
                iAux++;
            }
            string[][] data;
            int index = 1;
            if (modelCR.ListaCentralesResp.Count() != 0)
            {
                data = new string[modelCR.ListaCentralesResp.Count() + 1][];
                data[0] = header;
                foreach (StRespagoDTO item in modelCR.ListaCentralesResp)
                {
                    if (item.Equinomb == null) item.Equinomb = "No hay Equipo";
                    string[] itemDato2 = { item.Equinomb.ToString() };
                    Array.Resize(ref itemDato2, itemDato2.Length + iNroColumnas);
                    int iAux2 = 1;
                    foreach (StCompensacionDTO dto in modelCR.ListaCompensacion)
                    {
                        modelCR.EntidadCentralesRespEle = this.servicioSistemasTransmision.GetByIdStRespagoele(item.Respagcodi, dto.Stcompcodi);
                        if (modelCR.EntidadCentralesRespEle != null)
                        {
                            itemDato2[iAux2++] = modelCR.EntidadCentralesRespEle.Respaevalor.ToString();
                        }
                    }
                    data[index] = itemDato2;
                    index++;
                }
            }
            else
            {
                data = new string[2][];
                data[0] = header;
                data[index] = itemDato;
            }
            #endregion
            model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
            model.ListaCentralesGen = ListaCentralesGen.ToArray();

            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;
            model.NumeroColumnas = iNroColumnas; //Es el numero de columnas dobles por barra
            model.FixedColumnsLeft = 1;
            model.FixedRowsTop = 1;

            return Json(model);

        }

        public JsonResult GrabarGrillaExcelCR(int stpercodi, int strecacodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            CargaArchivosModel model = new CargaArchivosModel();

            if (stpercodi == 0 || strecacodi == 0)
            {
                model.sError = "Lo sentimos, debe seleccionar un mes y un versión";
                return Json(model);
            }
            try
            {
                //////////////Eliminando datos////////
                this.servicioSistemasTransmision.DeleteStRespagoele(strecacodi);
                this.servicioSistemasTransmision.DeleteStRespago(strecacodi);
                ///////////////////////////////////
                List<StCompensacionDTO> ListaCompensacion = this.servicioSistemasTransmision.GetByCriteriaStCompensacions(strecacodi);
                int iNumElemento = ListaCompensacion.Count;
                //Recorrer matriz para grabar detalle: se inicia en la fila 1
                for (int f = 1; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f] == null || datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadCentralesResp = new StRespagoDTO();
                    model.EntidadCentralesResp.Strecacodi = strecacodi;
                    model.EntidadCentralesResp.Respagusucreacion = User.Identity.Name;

                    if (!datos[f][0].Equals(""))
                    {
                        string sGenemprnombre = Convert.ToString(datos[f][0]);
                        //string sSisTransnombre = Convert.ToString(datos[f][1]);
                        StCentralgenDTO dtoStCentralGen = this.servicioSistemasTransmision.GetByCentNomb(sGenemprnombre, strecacodi);
                        if (dtoStCentralGen != null)
                        {
                            model.EntidadCentralesResp.Stcntgcodi = dtoStCentralGen.Stcntgcodi;
                            //model.EntidadCentralesResp.Sistrncodi = dtrSistemas.Sistrncodi;
                            int iResPagoCodi = this.servicioSistemasTransmision.SaveStRespago(model.EntidadCentralesResp);
                            for (int c = 1; c < iNumElemento + 1; c++)
                            {
                                model.EntidadCentralesRespEle = new StRespagoeleDTO();
                                model.EntidadCentralesRespEle.Respagcodi = iResPagoCodi;
                                foreach (StCompensacionDTO dto in ListaCompensacion)
                                {
                                    if (dto.Stcompcodelemento == datos[0][c].ToString())
                                    {
                                        model.EntidadCentralesRespEle.Stcompcodi = dto.Stcompcodi;
                                        model.EntidadCentralesRespEle.Respaecodelemento = dto.Stcompcodelemento;
                                        break;
                                    }
                                }
                                model.EntidadCentralesRespEle.Respaevalor = Convert.ToInt32(datos[f][c].ToString());
                                this.servicioSistemasTransmision.SaveStRespagoele(model.EntidadCentralesRespEle);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    //Insertar registro

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

        public JsonResult EliminarDatosCR(int stpercodi, int strecacodi)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                this.servicioSistemasTransmision.DeleteStRespagoele(strecacodi);
                this.servicioSistemasTransmision.DeleteStRespago(strecacodi);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }

        public JsonResult ExportarDataCR(int stpercodi = 0, int strecacodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                string file = this.servicioSistemasTransmision.GenerarFormatoStRespago(strecacodi, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        public JsonResult ProcesarArchivoCR(string sarchivo, int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();

            CargaArchivosModel modelCR = new CargaArchivosModel();
            modelCR.Stpercodi = stpercodi;
            modelCR.Strecacodi = strecacodi;

            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";

            //Obtener los codigos de elementos para el dropdown
            var ListaCentralesGen = this.servicioSistemasTransmision.GetByCriteriaStCentralgens(modelCR.Strecacodi).Select(y => y.Equinomb).ToList();

            try
            {
                #region Armando de contenido
                string[] header = { "CENTRAL GENERACIÓN" };
                int[] widths = { 250 };
                string[] itemDato = { "" };
                modelCR.ListaCompensacion = this.servicioSistemasTransmision.GetByCriteriaStCompensacions(modelCR.Strecacodi);
                int iNroColumnas = modelCR.ListaCompensacion.Count;
                if (iNroColumnas > 0)
                {
                    Array.Resize(ref header, header.Length + iNroColumnas);
                    Array.Resize(ref widths, widths.Length + iNroColumnas);
                    Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
                }
                //Formato de Columna
                object[] columnas = new object[iNroColumnas + 1];
                columnas[0] = new
                {   //Equicodi - Equinomb
                    type = GridExcelModel.TipoLista,
                    source = ListaCentralesGen.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                int iAux = 1;
                foreach (StCompensacionDTO dto in modelCR.ListaCompensacion)
                {
                    header[iAux] = dto.Stcompcodelemento;
                    widths[iAux] = 100;
                    itemDato[iAux] = "0";
                    columnas[iAux] = new
                    {
                        type = GridExcelModel.TipoNumerico,
                        source = (new List<String>()).ToArray(),
                        strict = false,
                        correctFormat = true,
                        className = "htRight",
                        format = "0,0",
                        readOnly = false,
                    };
                    iAux++;
                }
                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioSistemasTransmision.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 2][]; // Lee todo el contenido del excel y le descontamos 4 filas hasta donde empieza la data
                data[0] = header;
                int index = 1;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 5)
                    {
                        continue;
                    }
                    int iNumFila = iFila + 1;
                    string sGenemprnombre = dtRow[1].ToString();
                    StCentralgenDTO dtoStCentralGen = this.servicioSistemasTransmision.GetByCentNomb(sGenemprnombre, strecacodi);
                    if (dtoStCentralGen == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sGenemprnombre;
                        iRegError++;
                        continue;
                    }
                    string[] itemDato2 = { sGenemprnombre };
                    Array.Resize(ref itemDato2, itemDato2.Length + iNroColumnas);
                    for (int iAux2 = 1; iAux2 <= iNroColumnas; iAux2++)
                    {
                        itemDato2[iAux2] = dtRow[iAux2 + 1].ToString();
                    }
                    data[index] = itemDato2;
                    index++;
                }

                #endregion
                model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
                model.ListaCentralesGen = ListaCentralesGen.ToArray();

                model.NumeroColumnas = iNroColumnas; //Es el numero de columnas dobles por barra
                model.FixedColumnsLeft = 1;
                model.FixedRowsTop = 1;

                model.Data = data;
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

        #region Grilla Excel COMPENSACIÓN MENSUAL

        public JsonResult GrillaExcelCM(int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            CargaArchivosModel modelCM = new CargaArchivosModel();
            modelCM.Stpercodi = stpercodi;
            modelCM.Strecacodi = strecacodi;

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] header = { "CENTRAL GENERACIÓN" };
            //Ancho de cada columna
            int[] widths = { 250 };
            string[] itemDato = { "" };

            //Obtener los codigos de elementos para el dropdown
            var ListaCentralesGen = this.servicioSistemasTransmision.GetByCriteriaStCentralgens(modelCM.Strecacodi).Select(y => y.Equinomb).ToList();
            //Lista de Responsabilidades de Pago del periodo
            modelCM.ListaCompMensual = this.servicioSistemasTransmision.GetByCriteriaStCompmensuals(modelCM.Strecacodi);
            modelCM.ListaCompensacion = this.servicioSistemasTransmision.GetByCriteriaStCompensacions(modelCM.Strecacodi);
            int iNroColumnas = modelCM.ListaCompensacion.Count;

            if (iNroColumnas > 0)
            {
                Array.Resize(ref header, header.Length + iNroColumnas);
                Array.Resize(ref widths, widths.Length + iNroColumnas);
                Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
            }

            object[] columnas = new object[iNroColumnas + 1];
            columnas[0] = new
            {
                type = GridExcelModel.TipoLista,
                source = ListaCentralesGen.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false,
            };
            int iAux = 1;
            foreach (StCompensacionDTO dto in modelCM.ListaCompensacion)
            {
                header[iAux] = dto.Stcompcodelemento;
                widths[iAux] = 100;
                itemDato[iAux] = "0";
                columnas[iAux] = new
                {
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = false,
                };
                iAux++;
            }
            string[][] data;
            int index = 1;
            if (modelCM.ListaCompMensual.Count() != 0)
            {
                data = new string[modelCM.ListaCompMensual.Count() + 1][];
                data[0] = header;
                foreach (StCompmensualDTO item in modelCM.ListaCompMensual)
                {
                    string[] itemDato2 = { item.Equinomb.ToString() };
                    Array.Resize(ref itemDato2, itemDato2.Length + iNroColumnas);
                    int iAux2 = 1;
                    foreach (StCompensacionDTO dto in modelCM.ListaCompensacion)
                    {
                        modelCM.EntidadCompMensualEle = this.servicioSistemasTransmision.GetByIdStCompmensualele(item.Cmpmencodi, dto.Stcompcodi);
                        if (modelCM.EntidadCompMensualEle != null)
                        {
                            itemDato2[iAux2++] = modelCM.EntidadCompMensualEle.Cmpmelvalor.ToString();
                        }
                    }
                    data[index] = itemDato2;
                    index++;
                }
            }
            else
            {
                data = new string[2][];
                data[0] = header;
                data[index] = itemDato;
            }
            #endregion
            model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
            model.ListaCentralesGen = ListaCentralesGen.ToArray();
            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;
            model.NumeroColumnas = iNroColumnas;
            model.FixedColumnsLeft = 1;
            model.FixedRowsTop = 1;
            return Json(model);
        }

        public JsonResult GrabarGrillaExcelCM(int stpercodi, int strecacodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            CargaArchivosModel model = new CargaArchivosModel();

            if (stpercodi == 0 || strecacodi == 0)
            {
                model.sError = "Lo sentimos, debe seleccionar un mes y un versión";
                return Json(model);
            }
            try
            {
                //////////////Eliminando datos////////
                this.servicioSistemasTransmision.DeleteStCompmensualele(strecacodi);
                this.servicioSistemasTransmision.DeleteStCompmensual(strecacodi);
                ///////////////////////////////////
                List<StCompensacionDTO> ListaCompensacion = this.servicioSistemasTransmision.GetByCriteriaStCompensacions(strecacodi);
                int iNumElemento = ListaCompensacion.Count;
                //Recorrer matriz para grabar detalle: se inicia en la fila 1
                for (int f = 1; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f] == null || datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadCompMensual = new StCompmensualDTO();
                    model.EntidadCompMensual.Strecacodi = strecacodi;
                    model.EntidadCompMensual.Cmpmenusucreacion = User.Identity.Name;

                    if (!datos[f][0].Equals(""))
                    {
                        string sGenemprnombre = Convert.ToString(datos[f][0]);
                        StCentralgenDTO dtoStCentralGen = this.servicioSistemasTransmision.GetByCentNomb(sGenemprnombre, strecacodi);
                        if (dtoStCentralGen != null)
                        {
                            model.EntidadCompMensual.Stcntgcodi = dtoStCentralGen.Stcntgcodi;
                            int iCmpmencodi = this.servicioSistemasTransmision.SaveStCompmensual(model.EntidadCompMensual);
                            for (int c = 1; c < iNumElemento + 1; c++)
                            {
                                model.EntidadCompMensualEle = new StCompmensualeleDTO();
                                model.EntidadCompMensualEle.Cmpmencodi = iCmpmencodi;
                                foreach (StCompensacionDTO dto in ListaCompensacion)
                                {
                                    if (dto.Stcompcodelemento == datos[0][c].ToString())
                                    {
                                        model.EntidadCompMensualEle.Stcompcodi = dto.Stcompcodi;
                                        model.EntidadCompMensualEle.Cmpmelcodelemento = dto.Stcompcodelemento;
                                        break;
                                    }
                                }
                                decimal dValor = UtilSistemasTransmision.ValidarNumero(datos[f][c].ToString());
                                model.EntidadCompMensualEle.Cmpmelvalor = dValor;
                                this.servicioSistemasTransmision.SaveStCompmensualele(model.EntidadCompMensualEle);
                            }
                        }
                        else
                        {
                            continue;
                        }
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

        public JsonResult EliminarDatosCM(int stpercodi, int strecacodi)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                this.servicioSistemasTransmision.DeleteStCompmensualele(strecacodi);
                this.servicioSistemasTransmision.DeleteStCompmensual(strecacodi);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }

        public JsonResult ExportarDataCM(int stpercodi = 0, int strecacodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                string file = this.servicioSistemasTransmision.GenerarFormatoStCompmensual(strecacodi, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        public JsonResult ProcesarArchivoCM(string sarchivo, int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();

            CargaArchivosModel modelCR = new CargaArchivosModel();
            modelCR.Stpercodi = stpercodi;
            modelCR.Strecacodi = strecacodi;

            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";

            //Obtener los codigos de elementos para el dropdown
            var ListaCentralesGen = this.servicioSistemasTransmision.GetByCriteriaStCentralgens(modelCR.Strecacodi).Select(y => y.Equinomb).ToList();

            try
            {
                #region Armando de contenido

                string[] header = { "CENTRAL GENERACIÓN" };
                int[] widths = { 250 };
                string[] itemDato = { "" };


                modelCR.ListaCompensacion = this.servicioSistemasTransmision.GetByCriteriaStCompensacions(modelCR.Strecacodi);
                int iNroColumnas = modelCR.ListaCompensacion.Count;
                if (iNroColumnas > 0)
                {
                    Array.Resize(ref header, header.Length + iNroColumnas);
                    Array.Resize(ref widths, widths.Length + iNroColumnas);
                    Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
                }

                //Formato de Columna
                object[] columnas = new object[iNroColumnas + 1];
                columnas[0] = new
                {   //Equicodi - Equinomb
                    type = GridExcelModel.TipoLista,
                    source = ListaCentralesGen.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                int iAux = 1;
                foreach (StCompensacionDTO dto in modelCR.ListaCompensacion)
                {
                    header[iAux] = dto.Stcompcodelemento;
                    widths[iAux] = 100;
                    itemDato[iAux] = "1";
                    columnas[iAux] = new
                    {
                        type = GridExcelModel.TipoNumerico,
                        source = (new List<String>()).ToArray(),
                        strict = false,
                        correctFormat = true,
                        className = "htRight",
                        format = "0,0.00",
                        readOnly = false,
                    };
                    iAux++;
                }

                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioSistemasTransmision.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 2][]; // Lee todo el contenido del excel y le descontamos 4 filas hasta donde empieza la data
                data[0] = header;
                int index = 1;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 5)
                    {
                        continue;
                    }
                    int iNumFila = iFila + 1;
                    string sGenemprnombre = dtRow[1].ToString();
                    StCentralgenDTO dtoStCentralGen = this.servicioSistemasTransmision.GetByCentNomb(sGenemprnombre, strecacodi);
                    if (dtoStCentralGen == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sGenemprnombre;
                        iRegError++;
                        continue;
                    }
                    string[] itemDato2 = { sGenemprnombre };
                    Array.Resize(ref itemDato2, itemDato2.Length + iNroColumnas);
                    for (int iAux2 = 1; iAux2 <= iNroColumnas; iAux2++)
                    {
                        itemDato2[iAux2] = dtRow[iAux2 + 1].ToString();
                    }
                    data[index] = itemDato2;
                    index++;
                }
                #endregion
                model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
                model.ListaCentralesGen = ListaCentralesGen.ToArray();

                model.NumeroColumnas = iNroColumnas; //Es el numero de columnas dobles por barra
                model.FixedColumnsLeft = 1;
                model.FixedRowsTop = 1;

                model.Data = data;
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //// funciones generales para todos los tabs
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
    }
}
