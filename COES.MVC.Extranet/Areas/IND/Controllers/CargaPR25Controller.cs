using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.IND.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.Indisponibilidades;
using System.Configuration;
using COES.Servicios.Aplicacion.Indisponibilidades.Helper;
using COES.MVC.Extranet.Helper;
using System.IO;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Extranet.Areas.IND.Controllers
{
    public class CargaPR25Controller : BaseController
    {
        // GET: IND/CargaPR25
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        INDAppServicio servicioIndisponibilidad = new INDAppServicio();
        public ActionResult Index(int pericodi = 0)
        {
            base.ValidarSesionUsuario();

            #region Autentificando Empresa

            BaseModel model = new BaseModel();
            int iEmprCodi = 0;
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

            List<SeguridadServicio.EmpresaDTO> listTotal = new List<SeguridadServicio.EmpresaDTO>();
 
            bool accesoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);
            if (accesoEmpresas)
            {
                listTotal = seguridad.ListarEmpresas().ToList().Where(x => x.TIPOEMPRCODI == 3 || x.EMPRCODI == 11772 || x.EMPRCODI == 13 || x.EMPRCODI == 67).OrderBy(x => x.EMPRNOMB).ToList();
            }
            else
            {
                listTotal = base.ListaEmpresas;
            }

            TempData["EMPRNRO"] = listTotal.Count();
            if (listTotal.Count() == 1)
            {
                TempData["EMPRNOMB"] = listTotal[0].EMPRNOMB;
                Session["EmprNomb"] = listTotal[0].EMPRNOMB;
                Session["EmprCodi"] = listTotal[0].EMPRCODI;
                //agregado por assetec 20230124
                model.EntidadEmpresa = this.servicioEmpresa.GetByIdEmpresa(listTotal[0].EMPRCODI);

                Dominio.DTO.Sic.SiEmpresaDTO empresa = (new COES.Servicios.Aplicacion.General.EmpresaAppServicio()).ObtenerEmpresa(listTotal[0].EMPRCODI);

                if (listTotal[0].EMPRCODI > 0)
                {
                    if (empresa.Emprestado != "A")
                    {
                        TempData["EmprNomb"] = listTotal[0].EMPRNOMB + "  / (EN BAJA)";
                    }
                }

                iEmprCodi = Convert.ToInt32(listTotal[0].EMPRCODI);
            }
            else if (Session["EmprCodi"] != null)
            {
                iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                model.EntidadEmpresa = this.servicioEmpresa.GetByIdEmpresa(iEmprCodi);
                TempData["EMPRNOMB"] = model.EntidadEmpresa.EmprNombre;
                Session["EmprNomb"] = model.EntidadEmpresa.EmprNombre;

                Dominio.DTO.Sic.SiEmpresaDTO empresa = (new COES.Servicios.Aplicacion.General.EmpresaAppServicio()).ObtenerEmpresa(iEmprCodi);

                if (iEmprCodi > 0)
                {
                    if (empresa.Emprestado != "A")
                    {
                        TempData["EmprNomb"] = model.EntidadEmpresa.EmprNombre + " / (EN BAJA)";
                    }
                }
            }
            else if (listTotal.Count() > 1)
            {
                TempData["EMPRNOMB"] = "";
                return View();
            }
            else
            {
                //No hay empresa asociada a la cuenta
                TempData["EMPRNOMB"] = "";
                TempData["EMPRNRO"] = -1;
                return View();
            }

            #endregion

            #region INDEX
            model.ListaPeriodos = this.servicioIndisponibilidad.ListPeriodo()
                                        .Where(x => x.Iperihorizonte == ConstantesIndisponibilidades.HorizonteMensual)
                                        .ToList();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            { pericodi = model.ListaPeriodos[0].Ipericodi; }

            model.Pericodi = pericodi;
            #endregion
            return View(model);
        }

        #region Cuadros A1 y A2
        /// <summary>
        /// Permite generar consultar la informacion para los cuadros A1 y A2
        /// </summary>
        /// <param name="periodo">Identificador del periodo que se consulta</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarCuadros(int periodo)
        {
            object res = new object();
            int empresa = int.Parse(Session["EmprCodi"].ToString());
            //Consulta la informacion de los cuadros CDU, CRD, CCD y A2
            res = this.servicioIndisponibilidad.ListaCuadros(empresa, periodo);
            return Json(res);
        }

        /// <summary>
        /// Permite validar si se consulta por el periodo acutal o posterior
        /// </summary>
        /// <param name="periodo">Identificador del periodo que se consulta</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarPeriodo (int periodo)
        {
            object res = new object();
            //Consulta por el periodo
            res = this.servicioIndisponibilidad.ValidaPeriodo(periodo);
            return Json(res);
        }

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <param name="periodo">Identificador del periodo que se consulta</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int periodo)
        {
            int indicador = 0;
            try
            {
                int empresa = int.Parse(Session["EmprCodi"].ToString());
                IndFormatoExcel modelo = this.servicioIndisponibilidad.ModeloCuadroA1A2Excel(empresa, periodo);
                if (modelo.HabilitadoCuadro2 || modelo.HabilitadoCuadro1)
                {
                    string pathFile = ConfigurationManager.AppSettings[ConstantesIndisponibilidades.ReporteCuadroA1A2].ToString();
                    string filename = "Reporte_cuadroA1_cuadroA2";
                    string reporte = this.servicioIndisponibilidad.ExportarReporteCuadroA1A2(modelo, pathFile, filename);
                    indicador = 1;
                }
                else {
                    indicador = 5;
                    return Json(indicador);
                }

            }
            catch (Exception ex)
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = ConfigurationManager.AppSettings[ConstantesIndisponibilidades.ReporteCuadroA1A2].ToString() + "Reporte_cuadroA1_cuadroA2" + ".xlsx";
            return File(fullPath, Constantes.AppExcel, "Reporte_cuadroA1_cuadroA2" + ".xlsx");
        }
        #endregion

        #region Elegir / Cambiar de Empresa
        /// Permite seleccionar a la empresa con la que desea trabajar
        [HttpPost]
        public ActionResult EscogerEmpresa()
        {
            base.ValidarSesionUsuario();

            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<SeguridadServicio.EmpresaDTO> listTotal = new List<SeguridadServicio.EmpresaDTO>();//

            bool accesoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);
            if (accesoEmpresas)
            {
                listTotal = seguridad.ListarEmpresas().ToList().Where(x => x.TIPOEMPRCODI == 3 || x.EMPRCODI == 11772 || x.EMPRCODI == 13 || x.EMPRCODI == 67).OrderBy(x => x.EMPRNOMB).ToList();
            }
            else
            {
                listTotal = base.ListaEmpresas;
            }

            //UtilTransfPotencia.ObtenerEmpresasPorUsuario(User.Identity.Name);
            BaseModel model = new BaseModel();
            model.ListaEmpresas = new List<COES.Dominio.DTO.Transferencias.EmpresaDTO>();
            foreach (var item in listTotal)
            {
                model.ListaEmpresas.Add(new COES.Dominio.DTO.Transferencias.EmpresaDTO { EmprCodi = item.EMPRCODI, EmprNombre = item.EMPRNOMB });
            }
            return PartialView(model);
        }

        /// Permite actualizar o grabar un registro
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmpresaElegida(int EmprCodi)
        {
            //if (EmprCodi != 0)
            //{
            //    Session["EmprCodi"] = EmprCodi;
            //}
            //Pregunta si empresa existe y tiene gaseoducto va esto
            List<IndRelacionEmpresaDTO> lista = this.servicioIndisponibilidad.ListIndRelacionEmpresaByIdEmpresa(EmprCodi);

            if (lista.Count > 0 && EmprCodi != 0)
            {
                Session["EmprCodi"] = EmprCodi;
                //return RedirectToAction("Index");
            }
            //else { 
            //    return RedirectToAction("Index");
            //}

            return RedirectToAction("Index");

            //Sino emitir mensaje no reporta pr - 25
        }

        /// <summary>
        /// Importa el archivo con la información de la demanda Yupana y calcula el % de recálculo
        /// </summary>
        /// <param name="archivo">Archivo importado que sera subido al servidor temporalmente</param>
        /// <returns></returns>
        public JsonResult ImportarExcel(HttpPostedFileBase archivo, int periodo)
        {
            object res = new object();
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;
            int validator = 0;

            //string ruta = @"D:\Oficina\COES\Directorio\";
            string ruta = ConfigurationManager.AppSettings[ConstantesIndisponibilidades.ReporteCuadroA1A2].ToString();
            string nombreArchivo = Path.GetFileName(archivo.FileName);
            if (!Directory.Exists(ruta))
            {
                typeMsg = "error";
                dataMsg = "La carpeta requerida para la importación no existe";
                res = new { typeMsg, dataMsg };
                return Json(res);
            }

            try
            {
                //Crea el archivo en el servidor                
                FileInfo nuevoArchivo = new FileInfo(ruta + nombreArchivo);
                if (nuevoArchivo.Exists) nuevoArchivo.Delete();
                archivo.SaveAs(ruta + nombreArchivo);

                //Procesa los datos del archivo
                int empresa = int.Parse(Session["EmprCodi"].ToString());
                object datos = this.servicioIndisponibilidad.ImportarExcelPR25(empresa, ruta + nombreArchivo, periodo);
                int flag  = int.Parse((datos.GetType().GetProperty("flag").GetValue(datos, null)).ToString());
                if (flag == 0)
                {
                    //Validando informacion reportada
                    List<IndValidacionFormato> validaciones = this.servicioIndisponibilidad.ValidarDatos(datos, periodo);            
                    //Elimina el archivo del servidor
                    res = new { validator, datos, validaciones };
                }
                else {
                    validator = flag;
                    typeMsg = "msg-warning";
                    dataMsg = (flag == 2) ? "El cuadro A2 se encuentra vacio..." : 
                              "El archivo seleccionado no tiene el formato correspondiente, descarge nuevamente el formato...";
                    res = new { validator, typeMsg, dataMsg };
                }
                nuevoArchivo.Delete();
            }
            catch (Exception ex)
            {
                typeMsg = "msg-error";
                dataMsg = ex.Message;
                res = new { typeMsg, dataMsg };
                return Json(res);
            }

            return Json(res);
        }

        /// <summary>
        /// Permite enviar los datos importados por el excel a la base de datos
        /// </summary>
        /// <param name="htCDU"> Juego de datos de la tabla CDU</param>
        /// <param name="htCRD"> Juego de datos de la tabla CRD</param>
        /// <param name="htCCD"> Juego de datos de la tabla CCD</param>
        /// <param name="htA2"> Juego de datos de la tabla del cuadro A2</param>
        /// <param name="idPeriodo">Identificador del periodo</param>
        /// <param name="inicioCDU"> Fecha de inicio de vigencia CDU</param>
        /// <param name="finCDU"> Fecha de fin de vigencia CDU</param>
        /// <param name="inicioCCD"> Fecha de inicio de vigencia CCD</param>
        /// <param name="finCCD"> Fecha de fin de vigencia CCD</param>
        /// <param name="enPlazo"> Lista de arrays que indica si se esta en plazo o no</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnviarDatos(string[][] htCDU, string[][] htCRD, string[][] htCCD, 
                                           string[][] htA2, int idPeriodo, string inicioCDU, 
                                           string finCDU, string inicioCCD, string finCCD, List<string[]> enPlazo)
        {
            object res = new object();
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;
            try
            {
                int idEmpresa = int.Parse(Session["EmprCodi"].ToString());
                this.servicioIndisponibilidad.SaveCuadroPR25(idEmpresa, idPeriodo, htCDU, htCRD, htCCD, htA2, 
                                                                inicioCDU, finCDU, inicioCCD, finCCD, base.UserName, enPlazo);
                object plz = this.servicioIndisponibilidad.PopupPlazo(htCRD, idEmpresa, idPeriodo, enPlazo);
                typeMsg = "msg-success";
                dataMsg = "Registro satisfactorio..";
                res = new { plz, typeMsg, dataMsg };
            }
            catch (Exception ex)
            {
                typeMsg = "msg-error";
                dataMsg = ex.Message;
                res = new { typeMsg, dataMsg };
                return Json(res);
            }
            return Json(res);
        }
        #endregion

        #region SUGAD
        /// <summary>
        /// Muestra un pop up donde se visulizara el detalle del MostrarSugad
        /// </summary>
        /// <param name="emprcodi">EMPRCODI</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MostrarSugad(int pericodi, int emprcodi)
        {
            BaseModel model = new BaseModel();
            //Validamos si algunas de las centrales de la empresa tiene SUGAD
            model.ListaRelacionEmpresa = this.servicioIndisponibilidad.ListIndRelacionEmpresaByIdEmpresa(emprcodi);
            var sExisteSugad = model.ListaRelacionEmpresa.Where(x => x.Relempsugad == "S").First().Relempsugad;
            if (sExisteSugad.Equals("S"))
            {
                model.sMensaje = "1"; //Si existe SUGAD
                Dominio.DTO.Sic.SiEmpresaDTO empresa = (new COES.Servicios.Aplicacion.General.EmpresaAppServicio()).ObtenerEmpresa(emprcodi);
                model.EntidadEmpresa = new COES.Dominio.DTO.Transferencias.EmpresaDTO();
                model.EntidadEmpresa.EmprCodi = empresa.Emprcodi;
                model.EntidadEmpresa.EmprNombre = empresa.Emprnomb;
                model.Pericodi = pericodi;
            }
            else
            {
                //La empresa no tiene ninguna CENTRAL que requiera reportar SUGAD
                model.sMensaje = "La Empresa no tiene pendiente de registro de información SUGAD"; 
            }
            return PartialView(model);
        }

        /// <summary>
        /// Muestra la grilla excel con los datos del sugad
        /// </summary>
        /// <param name="pericodi">codigo del periodo</param>
        /// <param name="emprcodi">codigo de la empresa</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarExcelWeb(int pericodi, int emprcodi)
        {
            GrillaExcelModel model = new GrillaExcelModel();
            try
            {
                //Intervalos de fechas del mes
                IndPeriodoDTO EntidadPeriodo = this.servicioIndisponibilidad.GetByIdIndPeriodo(pericodi);
                DateTime dDateIni = new DateTime(EntidadPeriodo.Iperianio, EntidadPeriodo.Iperimes, 1);
                DateTime dDateFin = dDateIni.AddMonths(1).AddDays(-1);

                //Traemos la lista de Sugad si existiese
                List<IndCrdSugadDTO> listSugad = this.servicioIndisponibilidad.ListCrdSugadJoinCabecera(emprcodi, pericodi, ConstantesIndisponibilidades.TipoSUGAD);

                #region Armando la grilla Excel
                string[][] data; //hoja excel web de datos
                //Definimos la cabecera como una matriz
                string[] Cabecera = { "Periodo [" + dDateIni.ToString("dd/MM/yyyy") + " - " + dDateFin.ToString("dd/MM/yyyy")  + "]" }; //Titulo de CELDA[0,0]
                int[] widths = { 300 }; //Ancho de la primera columna 0
                string[] itemDato = { "" };

                string[] Fila1 = { "Capacidad total almacenada" }; //Titulo de CELDA[1,0]
                //Las siguientes filas son por central
                int iFila = 2; //0:Cabecera y 1: Fila1 => 2
                data = new string[iFila][]; //2 ubicaciones para la cabecera y la primera fila
                
                int iNumDias = DateTime.DaysInMonth(dDateIni.Year, dDateIni.Month); //Numero de dias del mes
                if (iNumDias > 0)
                {
                    //Calculamos las dimenciones de toda la tabla: + 4: central, unidad, grupo y familia
                    Array.Resize(ref Cabecera, Cabecera.Length + iNumDias + 4);
                    Array.Resize(ref Fila1, Fila1.Length + iNumDias + 4);
                    Array.Resize(ref widths, widths.Length + iNumDias + 4);
                    Array.Resize(ref itemDato, itemDato.Length + iNumDias + 4);

                    //Formato de columnas
                    object[] columnas = new object[iNumDias + 5]; //La primera columna y los 4 codigos
                    int iColumna = 0;
                    //Formateamos las primeras columnas
                    for (int i =0; i < 5; i++)
                    {
                        columnas[iColumna++] = new
                        {   //Información de las centrales/unidades[0] / equicodicentral[1] / equicodiunidad[2] / grupocodi [3] / famcodi[4]
                            type = GrillaExcelModel.TipoTexto,
                            source = (new List<String>()).ToArray(),
                            strict = false,
                            className = "htLeft",
                            correctFormat = true,
                            readOnly = true,
                        };
                    }

                    //Columna de fechas
                    for (var dt = dDateIni; dt <= dDateFin; dt = dt.AddDays(1))
                    {
                        //Agregamos el día en cada celda de la Cabecera
                        Cabecera[iColumna] = dt.ToString("dd");
                        Fila1[iColumna] = "0"; //Se inicializa con cero el total
                        //Formateamos la primera columna
                        columnas[iColumna] = new
                        {   //Columna por día
                            type = GrillaExcelModel.TipoNumerico,
                            source = (new List<String>()).ToArray(),
                            strict = false,
                            correctFormat = true,
                            className = "htRight",
                            format = "0,0.0000",
                            readOnly = false,
                        };
                        iColumna++;
                    }
                    model.Columnas = columnas; //Almacena el formato de cada columna
                    //Agregamos las dos primeras filas completas a data
                    data[0] = Cabecera;
                    data[1] = Fila1;

                    //Traemos la lista de Centrales con SUGAD, al menos uno debe tener disponible
                    model.ListaRelacionEmpresa = this.servicioIndisponibilidad.ListIndRelacionEmpresaByIdEmpresa(emprcodi).Where(x => x.Relempsugad == "S").ToList();
                    foreach (IndRelacionEmpresaDTO dtoEmpCentral in model.ListaRelacionEmpresa)
                    {
                        //Para cada central / unidad
                        string sCentral = dtoEmpCentral.Equinombcentral;
                        if (dtoEmpCentral.Equicodicentral != dtoEmpCentral.Equicodiunidad) sCentral = sCentral + " [" + dtoEmpCentral.Equinombunidad + "]";
                        string[] itemFila = { "Capacidad asignada: " + sCentral, dtoEmpCentral.Equicodicentral.ToString(), dtoEmpCentral.Equicodiunidad.ToString(), dtoEmpCentral.Grupocodi.ToString(), dtoEmpCentral.Famcodi.ToString() };
                        Array.Resize(ref itemFila, itemFila.Length + iNumDias);
                        
                        //leemos la data registrada
                        if(listSugad.Count > 0)
                        {
                            IndCrdSugadDTO entity = listSugad.Where(x => x.Equicodicentral == dtoEmpCentral.Equicodicentral
                                                                   && x.Equicodiunidad == dtoEmpCentral.Equicodiunidad
                                                                   && x.Grupocodi == dtoEmpCentral.Grupocodi
                                                                   && x.Famcodi == dtoEmpCentral.Famcodi
                                                                   && x.Crdsgdtipo == ConstantesIndisponibilidades.Central).FirstOrDefault();
                            if(entity != null)
                            {
                                for (int i = 5; i <= iNumDias + 4; i++)
                                {
                                    decimal valor = (decimal)entity.GetType().GetProperty("D" + (i - 4)).GetValue(entity, null);
                                    itemFila[i] = valor.ToString();
                                }
                            }
                        }
                        else
                        {
                            for (int i = 5; i <= iNumDias + 4; i++)
                            {
                                itemFila[i] = "0";
                            }
                        }
                        Array.Resize(ref data, data.Length + 1);
                        data[iFila] = itemFila; //Verificar que se esta poblando de ceros
                        iFila++;
                    }
                    //Asignamos la información de la Capacidad Total Almacenada
                    IndCrdSugadDTO entityCapTot = listSugad.Where(x => x.Equicodicentral == 0
                                                                   && x.Equicodiunidad == 0
                                                                   && x.Grupocodi == 0
                                                                   && x.Famcodi == 0
                                                                   && x.Crdsgdtipo == ConstantesIndisponibilidades.Total).FirstOrDefault();
                    if (entityCapTot != null)
                    {
                        for (int i = 5; i <= iNumDias + 4; i++)
                        {
                            decimal valor = (decimal)entityCapTot.GetType().GetProperty("D" + (i - 4)).GetValue(entityCapTot, null);
                            data[1][i] = valor.ToString();
                        }
                    }

                    model.Data = data;
                    model.Widths = widths;
                    model.Columnas = columnas;
                    model.FixedRowsTop = 1;
                    model.FixedColumnsLeft = 1;
                    model.NroColumnas = iNumDias + 1;
                    model.Emprcodi = emprcodi;
                    model.LimiteMax = 100000.0M; 
                }

                #endregion

                var JsonResult = Json(model);
                JsonResult.MaxJsonLength = Int32.MaxValue;
                return JsonResult;
            }
            catch (Exception e)
            {
                model.MensajeError = e.Message + "<br><br>" + e.StackTrace;
                return Json(model);
            }
        }

        /// <summary>
        /// Guarda la info de la grilla excel con los datos del sugad
        /// </summary>
        /// <param name="pericodi">codigo del periodo</param>
        /// <param name="emprcodi">codigo de la empresa</param>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarSugad(int pericodi, int emprcodi, List<string[]> datos)
        {
            string sMensaje = "1";
            try
            {
                //datos
                //Fila[0] contiene la cabecera de los días del mes
                //Fila[1] contendra el total de la suma de cada central por dia
                //A partir de la Fila[2] estan los datos de la central
                int iFila = 2;

                //Traemos la lista de Sugad si existiese
                List<IndCrdSugadDTO> listSugad = this.servicioIndisponibilidad.ListCrdSugadJoinCabecera(emprcodi, pericodi, ConstantesIndisponibilidades.TipoSUGAD);
                int idCabecera;
                if (listSugad.Count == 0)
                {
                    //NUEVO: Grabar Cabecera
                    IndCabeceraDTO cabecera = new IndCabeceraDTO()
                    {
                        Emprcodi = emprcodi,
                        Ipericodi = pericodi,
                        Indcbrtipo = ConstantesIndisponibilidades.TipoSUGAD,
                        Indcbrusucreacion = User.Identity.Name,
                        Indcbrfeccreacion = DateTime.Now
                    };
                    //Metodo para grabar en cabecera con retorno del id
                    idCabecera = this.servicioIndisponibilidad.SaveIndCabecera(cabecera);
                    //Preparamos el registro: Capacidad Toltal almacenada
                    IndCrdSugadDTO crdSugadTotAlmacenada = new IndCrdSugadDTO();
                    crdSugadTotAlmacenada.Indcbrcodi = idCabecera;
                    crdSugadTotAlmacenada.Crdsgdtipo = ConstantesIndisponibilidades.Total;

                    //Almacenamos el detalle
                    for (int i = iFila; i < datos.Count; i++)
                    {
                        //Codigos
                        int central = (string.IsNullOrEmpty(datos[i][1])) ? 0 : int.Parse(datos[i][1]);
                        int unidad = (string.IsNullOrEmpty(datos[i][2])) ? 0 : int.Parse(datos[i][2]);
                        int grupo = (string.IsNullOrEmpty(datos[i][3])) ? 0 : int.Parse(datos[i][3]);
                        int familia = (string.IsNullOrEmpty(datos[i][4])) ? 0 : int.Parse(datos[i][4]);

                        IndCrdSugadDTO crdSugad = new IndCrdSugadDTO();
                        crdSugad.Equicodicentral = central;
                        crdSugad.Equicodiunidad = unidad;
                        crdSugad.Grupocodi = grupo;
                        crdSugad.Famcodi = familia;
                        crdSugad.Crdsgdtipo = ConstantesIndisponibilidades.Central;
                        crdSugad.Indcbrcodi = idCabecera;
                        int diasContar = datos[i].Count() - 5;
                        for (int iDia = 1; iDia <= diasContar; iDia++) //dias D1,D2, .... , D31
                        {
                            decimal valor = Decimal.Round(decimal.Parse(datos[i][iDia + 4]), 12);
                            crdSugad.GetType().GetProperty("D" + (iDia)).SetValue(crdSugad, valor);
                            crdSugad.GetType().GetProperty("E" + (iDia)).SetValue(crdSugad, "S");

                            decimal valorTotAlmacenada = (decimal)crdSugadTotAlmacenada.GetType().GetProperty("D" + (iDia)).GetValue(crdSugadTotAlmacenada, null) + valor;
                            crdSugadTotAlmacenada.GetType().GetProperty("D" + (iDia)).SetValue(crdSugadTotAlmacenada, valorTotAlmacenada);
                        }
                        crdSugad.Crdsgdusucreacion = User.Identity.Name;
                        crdSugad.Crdsgdusumodificacion = User.Identity.Name;
                        crdSugad.Crdsgdfeccreacion = DateTime.Now;
                        crdSugad.Crdsgdfecmodificacion = DateTime.Now;
                        this.servicioIndisponibilidad.SaveIndCrdSugad(crdSugad);
                    }
                    //Almacenamnos el registro Capacidad total
                    crdSugadTotAlmacenada.Crdsgdusucreacion = User.Identity.Name;
                    crdSugadTotAlmacenada.Crdsgdusumodificacion = User.Identity.Name;
                    crdSugadTotAlmacenada.Crdsgdfeccreacion = DateTime.Now;
                    crdSugadTotAlmacenada.Crdsgdfecmodificacion = DateTime.Now;
                    this.servicioIndisponibilidad.SaveIndCrdSugad(crdSugadTotAlmacenada);
                }
                else
                {
                    //ACTUALIZAR: 
                    //Preparamos el registro: Capacidad Toltal almacenada
                    IndCrdSugadDTO entityTotAlmacenada = listSugad.Where(x => x.Equicodicentral == 0
                                                                   && x.Equicodiunidad == 0
                                                                   && x.Grupocodi == 0
                                                                   && x.Famcodi == 0
                                                                   && x.Crdsgdtipo == ConstantesIndisponibilidades.Total).FirstOrDefault();
                    for (int iDia = 1; iDia <= (datos[2].Count() - 5); iDia++) {
                        entityTotAlmacenada.GetType().GetProperty("D" + (iDia)).SetValue(entityTotAlmacenada, 0M);
                    }
                    for (int i = iFila; i < datos.Count; i++)
                    {
                        //Codigos
                        int central = (string.IsNullOrEmpty(datos[i][1])) ? 0 : int.Parse(datos[i][1]);
                        int unidad = (string.IsNullOrEmpty(datos[i][2])) ? 0 : int.Parse(datos[i][2]);
                        int grupo = (string.IsNullOrEmpty(datos[i][3])) ? 0 : int.Parse(datos[i][3]);
                        int familia = (string.IsNullOrEmpty(datos[i][4])) ? 0 : int.Parse(datos[i][4]);

                        IndCrdSugadDTO entity = listSugad.Where(x => x.Equicodicentral == central
                                                                   && x.Equicodiunidad == unidad
                                                                   && x.Grupocodi == grupo
                                                                   && x.Famcodi == familia
                                                                   && x.Crdsgdtipo == ConstantesIndisponibilidades.Central).FirstOrDefault();
                        if (entity != null) {
                            int diasContar = datos[i].Count() - 5;
                            for (int iDia = 1; iDia <= diasContar; iDia++) //dias D1,D2, .... , D31
                            {
                                decimal valor = Decimal.Round(decimal.Parse(datos[i][iDia + 4]), 12);
                                entity.GetType().GetProperty("D" + (iDia)).SetValue(entity, valor);
                                entity.GetType().GetProperty("E" + (iDia)).SetValue(entity, "S");

                                decimal valorTotAlmacenada = (decimal)entityTotAlmacenada.GetType().GetProperty("D" + (iDia)).GetValue(entityTotAlmacenada, null) + valor;
                                entityTotAlmacenada.GetType().GetProperty("D" + (iDia)).SetValue(entityTotAlmacenada, valorTotAlmacenada);
                            }
                            entity.Crdsgdusumodificacion = User.Identity.Name;
                            entity.Crdsgdfecmodificacion = DateTime.Now;
                            this.servicioIndisponibilidad.UpdateIndCrdSugad(entity);
                        }
                    }
                    //Almacenamnos el registro Capacidad total
                    entityTotAlmacenada.Crdsgdusumodificacion = User.Identity.Name;
                    entityTotAlmacenada.Crdsgdfecmodificacion = DateTime.Now;
                    this.servicioIndisponibilidad.UpdateIndCrdSugad(entityTotAlmacenada);
                }
            }
            catch (Exception e)
            {
                sMensaje = e.Message;
            }
            return Json(sMensaje);
        }
        #endregion
    }
}