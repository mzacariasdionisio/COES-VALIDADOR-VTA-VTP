using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CostoOportunidad.Helper;
using COES.MVC.Intranet.Areas.CostoOportunidad.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Controllers
{
    public class CostoOportunidadController : BaseController
    {
        //
        // GET: /CostoOportunidad/CostoOportunidad/
        CostoOportunidadAppServicio servicio = new CostoOportunidadAppServicio();
        List<EveMailsDTO> listaReprogramado = null;

        #region Propiedades TAB01 y TAB 02
        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[DatosSesionDespachoDiario.SesionNombreArchivo] != null) ?
                    Session[DatosSesionDespachoDiario.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionDespachoDiario.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public string[][] MatrizExcel
        {
            get
            {
                return (Session[DatosSesionDespachoDiario.SesionMatrizExcel] != null) ?
                    (string[][])Session[DatosSesionDespachoDiario.SesionMatrizExcel] : new string[1][];
            }
            set { Session[DatosSesionDespachoDiario.SesionMatrizExcel] = value; }
        }

        /// <summary>
        /// Matriz de Color de celdas
        /// </summary>
        public short[][] MatrizExcelColores
        {
            get
            {
                return (Session[DatosSesionDespachoDiario.SesionMatrizExcelColores] != null) ?
                    (short[][])Session[DatosSesionDespachoDiario.SesionMatrizExcelColores] : new short[1][];
            }
            set { Session[DatosSesionDespachoDiario.SesionMatrizExcelColores] = value; }
        }
        #endregion

        #region Propiedades TAB03
        public List<MeMedicion48DTO> ListaDespacho
        {
            get
            {
                return (Session[ConstantesCOportunidad.ListaDespacho] != null) ?
                    (List<MeMedicion48DTO>)Session[ConstantesCOportunidad.ListaDespacho] : new List<MeMedicion48DTO>();
            }
            set { Session[ConstantesCOportunidad.ListaDespacho] = value; }
        }

        public List<MeMedicion48DTO> ListaDespachoSin
        {
            get
            {
                return (Session[ConstantesCOportunidad.ListaDespachoSin] != null) ?
                    (List<MeMedicion48DTO>)Session[ConstantesCOportunidad.ListaDespachoSin] : new List<MeMedicion48DTO>();
            }
            set { Session[ConstantesCOportunidad.ListaDespachoSin] = value; }
        }

        public List<EveRsfdetalleDTO> ListaReservaEjec
        {
            get
            {
                return (Session[ConstantesCOportunidad.ListaReservaEjec] != null) ?
                    (List<EveRsfdetalleDTO>)Session[ConstantesCOportunidad.ListaReservaEjec] : new List<EveRsfdetalleDTO>();
            }
            set { Session[ConstantesCOportunidad.ListaReservaEjec] = value; }
        }

        public List<MeMedicion48DTO> ListaReservaProg
        {
            get
            {
                return (Session[ConstantesCOportunidad.ListaReservaProg] != null) ?
                    (List<MeMedicion48DTO>)Session[ConstantesCOportunidad.ListaReservaProg] : new List<MeMedicion48DTO>();
            }
            set { Session[ConstantesCOportunidad.ListaReservaProg] = value; }
        }

        public List<MeMedicion48DTO> ListaCruceReserva
        {
            get
            {
                return (Session[ConstantesCOportunidad.ListaCruceReserva] != null) ?
                    (List<MeMedicion48DTO>)Session[ConstantesCOportunidad.ListaCruceReserva] : new List<MeMedicion48DTO>();
            }
            set { Session[ConstantesCOportunidad.ListaCruceReserva] = value; }
        }
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CostoOportunidadController));
        #endregion

        /// <summary>
        /// Lista de centrales 
        /// </summary>
        public List<PrGrupoDTO> ListaPtos
        {
            get
            {
                return (Session[DatosSesionDespachoDiario.SesionListaPtos] != null) ?
                    (List<PrGrupoDTO>)Session[DatosSesionDespachoDiario.SesionListaPtos] : new List<PrGrupoDTO>();
            }
            set { Session[DatosSesionDespachoDiario.SesionListaPtos] = value; }
        }

        public CostoOportunidadController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public ActionResult Index()
        {
            FormatoModel model = new FormatoModel();
            model.IdEnvio = 0;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        #region "Envio de ZIP al servidor"
        /// <summary>
        /// Recibe los archivos leidos para cargarlos en web
        [HttpPost]
        public ActionResult Upload()
        {
            string sNombreArchivo = "";

            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.PathArchivoCsv;
            System.IO.Directory.CreateDirectory(path);

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
                    this.NombreFile = sNombreArchivo;
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Proceso de Carga del ZIP"
        /// <summary>
        /// LeerFileUpArchivo - Oliver
        /// Metodo de Lectura ZIP
        /// </summary>
        /// <param name="fecha">fecha de consulta</param>
        /// <returns>1,2,3,4,...</returns>
        public JsonResult LeerFileUpArchivo(string fecha)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.PathArchivoCsv;

            CostoOportunidadModel model = new CostoOportunidadModel();
            string file = path + NombreFile;
            int result = 0;

            ZipFile.ExtractToDirectory(file, path);

            string msj = string.Empty;
            bool valida_ = true;

            //Verificamos si la cantidad de reprogramados es igual a la cantidad de reprogramados del archivo zip
            using (ZipArchive archive = ZipFile.OpenRead(file))
            {
                //Obtenemos solo los archivos que necesitamos para obtener informacion
                var datos = archive.Entries.Where(x => x.FullName.Contains(ConstantesDespachoDiario.CsvGerHidrocp) || x.FullName.Contains(ConstantesDespachoDiario.CsvGerTermocp) ||
                    x.FullName.Contains(ConstantesDespachoDiario.CsvResagHidrocp) || x.FullName.Contains(ConstantesDespachoDiario.CsvResagTermocp));

                //Datos de Reserva Asignada Programada
                var reservaAsigProgramada = datos.Where(x => x.FullName.Contains(ConstantesDespachoDiario.CsvResagHidrocp) ||
                x.FullName.Contains(ConstantesDespachoDiario.CsvResagTermocp)).Where(x => x.FullName.Contains(ConstantesDespachoDiario.NomCarpetaReProgramados)).ToList();

                DateTime sFecha = new DateTime();
                if (fecha != null)
                {
                    //Se setea y se establece el formato de fecha y mostrarlo en reporte matriz excel
                    sFecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                var listaReporgramado = servicio.GetListaReprogramado(sFecha);

                if (listaReporgramado.Count != (reservaAsigProgramada.Count) / 2) { valida_ = false; }
            }

            if (valida_)
            {
                //Leemos el archivo ZIP
                using (ZipArchive archive = ZipFile.OpenRead(file))
                {
                    //Obtenemos solo los archivos que necesitamos para obtener informacion
                    var datos = archive.Entries.Where(x => x.FullName.Contains(ConstantesDespachoDiario.CsvGerHidrocp) || x.FullName.Contains(ConstantesDespachoDiario.CsvGerTermocp) ||
                        x.FullName.Contains(ConstantesDespachoDiario.CsvResagHidrocp) || x.FullName.Contains(ConstantesDespachoDiario.CsvResagTermocp));

                    //Archivos DAT
                    var archivosDat = archive.Entries.Where(x => x.FullName.Contains(ConstantesDespachoDiario.DatMaxRsHidro) || x.FullName.Contains(ConstantesDespachoDiario.DatMaxRsTermo) ||
                        x.FullName.Contains(ConstantesDespachoDiario.DatMinRsHidro) || x.FullName.Contains(ConstantesDespachoDiario.DatMinRsTermo));
                    archivosDat = archivosDat.Where(x => x.FullName.Contains(ConstantesDespachoDiario.NomCarpetaProgramados));

                    //Datos de Reserva Asignada Programada
                    var reservaAsigProgramada = datos.Where(x => x.FullName.Contains(ConstantesDespachoDiario.CsvResagHidrocp) ||
                    x.FullName.Contains(ConstantesDespachoDiario.CsvResagTermocp)).ToList();

                    //Datos de Despacho con Reserva
                    var despachoConReserva = datos.Where(x => x.FullName.Contains(ConstantesDespachoDiario.CsvGerHidrocp) ||
                    x.FullName.Contains(ConstantesDespachoDiario.CsvGerTermocp));
                    despachoConReserva = despachoConReserva.Where(x => !x.FullName.Contains(ConstantesDespachoDiario.SR)).ToList();
                    despachoConReserva = despachoConReserva.Where(x => !x.FullName.Contains(ConstantesDespachoDiario.RSF)).ToList();

                    //Datos de Despacho sin Reserva
                    var despachoSinReserva = datos.Where(x => x.FullName.Contains(ConstantesDespachoDiario.SR) || x.FullName.Contains(ConstantesDespachoDiario.RSF));

                    //Proceso de Lectura y grabado de datos de los archiso DAT
                    ProcesoSaveArchivosDat(archivosDat, fecha);

                    result = ProcesoSaveMatriz(reservaAsigProgramada, fecha, ConstantesCostoOportunidad.LectcodiReservaProgramada, ref msj);
                    if (result != -1)
                    {
                        result = ProcesoSaveMatriz(despachoConReserva, fecha, ConstantesCostoOportunidad.LectcodiDespachoConReserva, ref msj);
                        if (result != -1)
                        {
                            result = ProcesoSaveMatriz(despachoSinReserva, fecha, ConstantesCostoOportunidad.LectcodiDespachoSinReserva, ref msj);
                        }
                    }
                }
            }
            else { result = 10; msj = "El total de reprogramados es distinto del total de reprogramados del archivo ZIP"; }

            //Eliminamos Carpeta Temporal (Archivo y Carpetas extraidas del ZIP)
            System.IO.Directory.Delete(path, true);
            model.Resultado = result.ToString() + "," + msj;
            return Json(model);
        }

        private void ProcesoSaveArchivosDat(IEnumerable<ZipArchiveEntry> datos, string fecha)
        {
            int tipoDatos = 0;
            string tipoProgramacion = string.Empty;

            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.PathArchivoDat;
            List<CoBandancpDTO> listEntityRead = new List<CoBandancpDTO>();
            List<TipoDatos> listaHojaPto = new List<TipoDatos>();

            foreach (ZipArchiveEntry entry in datos)
            {
                int i = 1;
                using (StreamReader dbProviderReader2 = new StreamReader(path + entry.FullName))
                {
                    string sLine = string.Empty;
                    while (sLine != null)
                    {
                        sLine = dbProviderReader2.ReadLine();
                        if (sLine != null)
                        {
                            var arrLine = sLine.Split(' ');
                            //Eliminamos campos vacios
                            arrLine = arrLine.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                            if (i == 1) // si es primera linea
                            {
                                string cadena = arrLine[1].Substring(1, 3);
                                if (cadena == "MAX")
                                {
                                    tipoDatos = ConstantesCOportunidad.BandaMaxima; // Banda máxima
                                }
                                if (cadena == "MIN")
                                {
                                    tipoDatos = ConstantesCOportunidad.BandaMinima; // Banda mínima
                                }
                            }
                            else
                            {
                                var equipo = servicio.GetPrGrupo(Convert.ToInt32(arrLine[0]));
                                if (equipo != null)
                                {
                                    var obj = listEntityRead.Find(x => x.Grupocodi == equipo.Grupocodi); // buscamos si ya esta registrado el modo de operacion

                                    if (obj == null)
                                    { // nuevo modo de operación
                                        CoBandancpDTO entity = new CoBandancpDTO();
                                        TipoDatos objModoOp = new TipoDatos();
                                        entity.Grupocodi = objModoOp.Codi = equipo.Grupocodi;
                                        entity.Gruponomb = objModoOp.DetName = equipo.Gruponomb;

                                        if (tipoDatos == 1) // Banda máxima
                                        {
                                            entity.Bandmax = Convert.ToDecimal(arrLine[1]);
                                        }
                                        else // Banda mínima
                                        {
                                            entity.Bandmin = Convert.ToDecimal(arrLine[1]);
                                        }
                                        entity.Bandusumodificacion = User.Identity.Name;
                                        entity.Bandfecmodificacion = DateTime.Now;
                                        listEntityRead.Add(entity);
                                        listaHojaPto.Add(objModoOp);
                                    }
                                    else // ya existe modificamos el valor max o min
                                    {
                                        if (tipoDatos == 1) // Banda máxima
                                        {
                                            obj.Bandmax = Convert.ToDecimal(arrLine[1]);
                                        }
                                        else // Banda mínima
                                        {
                                            obj.Bandmin = Convert.ToDecimal(arrLine[1]);
                                        }
                                    }
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            int nfilas = listEntityRead.Count;
            string[][] MatrizDAT = ToolsCostoOport.InicializaMatriz(ConstantesCOportunidad.NrofilasCabecera, nfilas, ConstantesCOportunidad.NroColumasMatriz);
            ToolsCostoOport.CargaDatosArchivo(MatrizDAT, listEntityRead, ConstantesCOportunidad.NrofilasCabecera);

            GrabarExcelWebBandaNCP(MatrizDAT, fecha, listaHojaPto, User.Identity.Name);
        }

        private int ProcesoSaveMatriz(IEnumerable<ZipArchiveEntry> datos, string fecha, int Lectcodi, ref string msj)
        {
            //Inicializamos Matrices 
            this.MatrizExcel = ToolsCostoOport.InicializaMatriz(ConstantesDespachoDiario.NrofilasCabeceraCSV, ConstantesDespachoDiario.NroFilasDataCSV, this.ListaPtos.Count + 1);
            string[][] matrizExcelTemp = ToolsCostoOport.InicializaMatriz(ConstantesDespachoDiario.NrofilasCabeceraCSV, ConstantesDespachoDiario.NroFilasDataCSV, this.ListaPtos.Count + 1);

            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.PathArchivoCsv;
            List<string> listaCSVnew = new List<string>();
            string tipoProgramacion = string.Empty;
            int result = 0;
            DateTime sFecha = new DateTime();
            if (fecha != null)
            {
                //Se setea y se establece el formato de fecha y mostrarlo en reporte matriz excel
                sFecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            int reprograma = 0;

            ToolsCostoOport.GeneraCabeceraReserva(MatrizExcel, this.ListaPtos, sFecha);

            int res = 0;
            foreach (ZipArchiveEntry entry in datos)
            {
                //Se obtiene si el archivo leido es un reprogramado
                reprograma = VerificarReprograma((ConstantesCostoOportunidad.LectcodiDespachoSinReserva.Equals(Lectcodi) ?
                    entry.FullName.Replace(ConstantesDespachoDiario.SR, "/") : entry.FullName));

                //obtenemos el nro de reprograma para el metodo GeneraRecargaMatriz
                tipoProgramacion = (entry.FullName.Contains(ConstantesDespachoDiario.NomCarpetaProgramados)) ?
                    ConstantesDespachoDiario.NomCarpetaProgramados :
                    ConstantesDespachoDiario.NomCarpetaReProgramados;

                // Adecuamos archivo CSV a estructura de la Matriz y generanos un objeto lista listaCSVnew
                listaCSVnew = GeneraExcelTemp(path + entry.FullName);

                //Llenamos MatrizExcel y MatrizExcelTemp para los dos programados y reprogramado
                matrizExcelTemp = ListaMatrixCSV(listaCSVnew, tipoProgramacion, matrizExcelTemp);

                //Condicional para los reprogramas RDO 
                if (tipoProgramacion == ConstantesDespachoDiario.NomCarpetaReProgramados && reprograma != 0)
                {
                    res = GeneraRecargaMatrizReprogramados(matrizExcelTemp, reprograma, sFecha, listaCSVnew.Count);
                }
                if (res == 10) { result = res; msj = "Ruta: " + entry.FullName + " contiene " + (listaCSVnew.Count - 1) + " registros."; break; }
            }

            if (res != 10) { result = GrabarMatrizWeb(this.MatrizExcel, Lectcodi, fecha); }

            return result;
        }

        /// <summary>
        /// Metodo donde se obtiene el Nro de Reprograma existente
        /// de acuerdo al archivo ZIP cargado
        /// </summary>
        /// <param name="data">/A/,/B/,..</param>
        /// <returns>1,2,3,4,....</returns>
        private int VerificarReprograma(string data)
        {
            int result = 0;

            if (data.IndexOf("A/") != -1) result = 1;
            if (data.IndexOf("B/") != -1) result = 2;
            if (data.IndexOf("C/") != -1) result = 3;
            if (data.IndexOf("D/") != -1) result = 4;
            if (data.IndexOf("E/") != -1) result = 5;
            if (data.IndexOf("F/") != -1) result = 6;
            if (data.IndexOf("G/") != -1) result = 7;
            if (data.IndexOf("H/") != -1) result = 8;
            if (data.IndexOf("I/") != -1) result = 9;
            if (data.IndexOf("J/") != -1) result = 10;

            return result;
        }

        /// <summary>
        /// Adecuamos archivo CSV a estructura de la Matriz para los programados y Matriz para 
        /// los reprogramados y generanos un objeto lista
        /// Oliver
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<string> GeneraExcelTemp(string path)
        {
            int _result = 0;
            //Leemos el archivo CSV
            string[] lineasCSV = System.IO.File.ReadAllLines(path, System.Text.Encoding.Default);

            var caracteCabeceraDelete = 0;
            string[] splitExcel = null;

            //Llenamos el objeto lista con el archivo CSV
            List<string> lista = new List<string>(lineasCSV);
            //Eliminamos 3 filas de la lista - filas que no se usaran para la MatrizExcel
            lista.RemoveAt(0);
            lista.RemoveAt(0);
            lista.RemoveAt(0);

            //Consultamos si el objeto lista tiene datos que no estan relacionados con la cabecera de la Matriz
            if (lista.Count > 0) { lista[0] = lista[0].Replace(" ", ""); }
            for (int a = 0; a < this.ListaPtos.Count; a++)
            {
                if (lista[0].ToUpper().Contains("," + this.ListaPtos[a].Gruponombncp.Replace(" ", "").ToUpper() + ","))
                {
                    _result++;
                }
            }
            if (_result > 0)
            {
                for (int b = 0; b < lista.Count; b++)
                {
                    if (b == 0)
                    {
                        splitExcel = lista[b].Trim().Split(',');
                        for (int c = 0; c < 3; c++)
                        {
                            caracteCabeceraDelete += splitExcel[c].Length;
                        }
                        //Obtenemos la cabecera correcta
                        lista[b] = lista[b].ToUpper().Trim().Substring(caracteCabeceraDelete + 3, lista[b].Trim().Length - caracteCabeceraDelete - 3);
                    }
                    else
                    {
                        lista[b] = lista[b].Replace(" ", "").Trim();
                        caracteCabeceraDelete = 0;
                        splitExcel = lista[b].Trim().Split(',');
                        for (int d = 0; d < 3; d++)
                        {
                            caracteCabeceraDelete += splitExcel[d].Length;
                        }
                        //Obtenemos los datos correctos
                        lista[b] = lista[b].Substring(caracteCabeceraDelete + 3, lista[b].Length - caracteCabeceraDelete - 3);
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Metodo ListaMatrixCSV - Oliver
        /// Se verifica la cabecera de la BD con la cabecera del objeto lista.
        /// Una vez obtenido cabecera de objeto lista por medio de la posicion
        /// recuperamos y llenamos matriz para programados y reprogramados
        /// </summary>
        /// <param name="listaCSVnew">Objeto Lista Programados y Reprogramados</param>
        /// <param name="tipoProgramacion">PDO, RDO</param>
        /// <param name="MatrizExcelTemp">Matriz para Reprogramados</param>
        /// <returns>MatrizExcelTemp</returns>
        private string[][] ListaMatrixCSV(List<string> listaCSVnew, string tipoProgramacion, string[][] MatrizExcelTemp)
        {
            //Consultamos la cabecera que se mostrara en Matriz
            for (int a = 0; a < this.ListaPtos.Count; a++)
            {
                //Verificamos si objeto lista contiene datos de la cabecera
                if (listaCSVnew[0].Contains("," + this.ListaPtos[a].Gruponombncp.Replace(" ", "").ToUpper() + ","))
                {
                    //Obtenemos la posicion dato de la cabecera
                    int posicion = listaCSVnew[0].Substring(0, listaCSVnew[0].IndexOf(this.ListaPtos[a].Gruponombncp.Replace(" ", "").ToUpper())).Split(',').Length - 1;

                    //Recorremos la lista para obtener datos Programados y Reprogramados
                    for (int b = 1; b < listaCSVnew.Count; b++)
                    {
                        //Verificamos y llenamos MatrizExcel para los programados y MatrizExcelTemp para los reprogramados
                        if (tipoProgramacion == ConstantesDespachoDiario.NomCarpetaProgramados)
                        {
                            this.MatrizExcel[b][a + 1] = listaCSVnew[b].Split(',')[posicion].Trim();
                        }
                        else
                        {
                            MatrizExcelTemp[b][a + 1] = listaCSVnew[b].Split(',')[posicion].Trim();
                        }
                    }
                }
            }

            return MatrizExcelTemp;
        }

        /// <summary>
        /// EDDY funcion que actualiza la matriz excel web con los nuevos datos de la matriz reprogranada
        /// </summary>
        /// <param name="matrizRecarga"></param>
        /// <param name="nrecarga"></param>
        /// <param name="dFecha"></param>
        public int GeneraRecargaMatrizReprogramados(string[][] matrizRecarga, int nrecarga, DateTime dFecha, int totalExcel)
        {
            int ret = 1;
            int nBloque = 0, nBloque2 = 0;
            int nCol = this.ListaPtos.Count + 1;
            int nFilas = ConstantesCOportunidad.NrofilasCabecera + ConstantesCOportunidad.NBloquesMattrizReservas;

            List<EveMailsDTO> listaReporgramado = new List<EveMailsDTO>();
            listaReporgramado = servicio.GetListaReprogramado(dFecha);
            if (listaReporgramado.Count > 0)
            {
                nBloque = (int)listaReporgramado[nrecarga - 1].Mailbloquehorario;

                if (listaReporgramado.Count == nrecarga) nBloque2 = nFilas - 1;
                else nBloque2 = (int)listaReporgramado[nrecarga].Mailbloquehorario;

                for (int i = nBloque2; i >= ((nBloque == 1) ? 0 : nBloque) + 1; i--)
                {
                    for (int j = nCol - 1; j > 0; j--)
                    {
                        if ((i - ((nFilas - 1) - totalExcel) - 1) <= 0) { ret = 10; break; }
                        this.MatrizExcel[i][j] = matrizRecarga[i - ((nFilas - 1) - totalExcel) - 1][j];
                    }
                    if (ret == 10) { break; }
                }
            }

            return ret;
        }

        /// <summary>
        /// Grabar matriz excel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="Lectcodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public int GrabarMatrizWeb(string[][] data, int Lectcodi, string fecha)
        {
            base.ValidarSesionUsuario();

            ///////// Definicion de Variables ////////////////
            int exito = 0;

            try
            {
                var lista48 = ObtenerDatosDespachoDiario(data, this.ListaPtos, Lectcodi, 0, ConstantesCOportunidad.NrofilasCabecera + ConstantesCOportunidad.NBloquesMattrizReservas, ConstantesCOportunidad.NrofilasCabecera);
                servicio.EliminarValoresCargados48(lista48);
                servicio.GuardarValoresCargados48(lista48);

                exito = 1;
            }
            catch (Exception ex)
            {
                log.Error("GrabarMatrizWeb", ex);
                exito = -1;
            }

            return exito;
        }

        /// <summary>
        /// Lee los datos del  Excel web Carga Reserva  Asignada Previa y los almacena en una lista de DTO Medicion48
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> ObtenerDatosDespachoDiario(string[][] datos, List<PrGrupoDTO> ptos, int lectura, int checkBlanco, int nFil, int filCabecera)
        {
            int nCol = ptos.Count + 1;
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            MeMedicion48DTO reg;
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            fecha = DateTime.ParseExact(datos[filCabecera][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
            for (var i = 1; i < nCol; i++)
            {
                reg = new MeMedicion48DTO();
                for (var j = filCabecera; j < nFil; j++)
                {
                    int indice = j - filCabecera + 1;
                    reg.Ptomedicodi = ptos[i - 1].PtoMediCodi;
                    reg.Tipoinfocodi = 1;
                    reg.Meditotal = 0;
                    reg.Lectcodi = lectura;
                    reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                    stValor = datos[j][i];
                    if (COES.Base.Tools.Util.EsNumero(stValor))
                    {
                        valor = decimal.Parse(stValor);
                        reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                    }
                    else
                    {
                        if (checkBlanco == 0)
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                        else
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, (decimal?)0);
                    }
                }
                lista.Add(reg);
            }
            return lista;
        }
        #endregion

        #region "TAB 01 - Consulta de Costo Oportunidad"
        /// <summary>
        /// Muestra El formato Excel en la Web 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="desEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarHojaExcelWeb(int idFormato, string fecha, int idEnvio)
        {
            try
            {
                CostoOportunidadModel jsModel = BuildHojaExcel(fecha, idEnvio, idFormato);
                Session["DatosJSON"] = jsModel.Handson.ListaExcelData;

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.PathArchivoCsv;
                if (System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.Delete(path, true);
                }

                return Json(jsModel);
            }
            catch (Exception ex)
            {
                log.Error("MostrarHojaExcelWeb", ex);
                Session["DatosJSON"] = null;
                return Json(-1);

            }
        }

        /// <summary>
        ///Devuelve el model necesario para mostrar en la web
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public CostoOportunidadModel BuildHojaExcel(string fecha, int idEnvio, int idFormato)
        {
            CostoOportunidadModel model = new CostoOportunidadModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaColWidth = new List<int>();
            model.Handson.ReadOnly = false;
            model.FilasCabecera = ConstantesCOportunidad.NrofilasCabecera;
            model.ColumnasCabecera = ConstantesCOportunidad.NroColCabecera;

            DateTime dfecha = DateTime.Now;

            if (fecha != null)
            {
                dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>(); // Lista de datos
            List<PrGrupoDTO> listaModosOperacion = new List<PrGrupoDTO>(); // Lista de NModos de operacion para cabecera

            listaModosOperacion = servicio.GetListaModosOpNCP().OrderBy(x => x.Grupoabrev).ToList();
            this.ListaPtos = listaModosOperacion;
            int nCol = listaModosOperacion.Count;
            int nBloques = ConstantesCOportunidad.NBloquesMattrizReservas;// +ConstantesCOportunidad.NrofilasCabecera;

            ////para pintar los colores de la matriz y si existe reprogramado
            this.MatrizExcelColores = ToolsCostoOport.InicializaMatrizColor(ConstantesCOportunidad.NrofilasCabecera, nBloques, nCol + ConstantesCOportunidad.NroColCabecera + 1);

            // pinta los colores de las filas de la matriz segun los reporogramados encontrados
            listaReprogramado = new List<EveMailsDTO>();
            listaReprogramado = servicio.GetListaReprogramado(dfecha);
            model.ListaReprograma = listaReprogramado;
            if (listaReprogramado.Count > 0)
            {
                int color = 1;
                foreach (var obj in listaReprogramado)
                {
                    var xfil = (int)obj.Mailbloquehorario + 1;
                    if ((int)obj.Mailbloquehorario == 1) { xfil = 1; }
                    ToolsCostoOport.SetColoresMatrizExcel(this.MatrizExcelColores, xfil, nBloques + ConstantesCOportunidad.NrofilasCabecera, nCol + ConstantesCOportunidad.NroColCabecera, color);

                    color++;
                }
            }

            //Consultamos informacion de acuerdo al combo Tipo de Informacion
            if (idFormato == 0) lista = servicio.GetListadoReservas(dfecha, ConstantesCostoOportunidad.LectcodiReservaProgramada);

            if (idFormato == 1) lista = servicio.GetListadoReservas(dfecha, ConstantesCostoOportunidad.LectcodiDespachoConReserva);

            if (idFormato == 2) lista = servicio.GetListadoReservas(dfecha, ConstantesCostoOportunidad.LectcodiDespachoSinReserva);

            if (lista.Count == 0)
            {
                this.MatrizExcelColores = ToolsCostoOport.InicializaMatrizColor(ConstantesCOportunidad.NrofilasCabecera, nBloques, nCol + ConstantesCOportunidad.NroColCabecera + 1);
            }
            if (lista.Count > 0)
            {
                model.IdEnvio = lista.Count;
            }

            model.Handson.ListaExcelData = ToolsCostoOport.InicializaMatriz(ConstantesCOportunidad.NrofilasCabecera, nBloques, nCol + ConstantesCOportunidad.NroColCabecera);
            ToolsCostoOport.CargaDatosMeMedicion48(model.Handson.ListaExcelData, lista, listaModosOperacion, dfecha, ConstantesCOportunidad.NrofilasCabecera);

            //Creamos Cabecera de excel web
            ToolsCostoOport.GeneraCabeceraReserva(model.Handson.ListaExcelData, listaModosOperacion, dfecha);

            model.MatrizExcelColores = this.MatrizExcelColores;
            #region Filas Cabeceras

            model.Handson.ListaColWidth.Add(200);
            for (var ind = 1; ind < nCol + 1; ind++)
            {
                model.Handson.ListaColWidth.Add(120);
            }
            #endregion

            return model;
        }
        #endregion

        #region "TAB 02 - Parametros"
        [HttpPost]
        public JsonResult MostrarCostosOportunidad(string fecha, int idEnvio)
        {

            try
            {
                CostoOportunidadModel jsModel = BuildHojaExcelParametros(fecha, idEnvio);
                return Json(jsModel);
            }
            catch (Exception ex)
            {
                log.Error("MostrarCostosOportunidad", ex);
                return Json(-1);
            }
        }

        /// <summary>
        ///Devuelve el model necesario para mostrar en la web
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public CostoOportunidadModel BuildHojaExcelParametros(string fecha, int idEnvio)
        {
            CostoOportunidadModel model = new CostoOportunidadModel();
            List<CoBandancpDTO> lista = new List<CoBandancpDTO>();
            model.Handson = new HandsonModel();
            model.Handson.ListaColWidth = new List<int>();
            model.IdEnvio = idEnvio;

            DateTime dfecha = DateTime.Now;

            if (fecha != null)
            {
                dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (idEnvio >= 0) // Es nuevo envio se consulta envio seleccionado de la BD
            {
                lista = servicio.GetListaCoBandaNCP(dfecha);
                int nfilas = lista.Count;
                model.Handson.ListaExcelData = ToolsCostoOport.InicializaMatriz(ConstantesCOportunidad.NrofilasCabecera, nfilas, ConstantesCOportunidad.NroColumasMatriz);
                ToolsCostoOport.CargaDatosBandaNCP(model.Handson.ListaExcelData, lista, ConstantesCOportunidad.NrofilasCabecera, ConstantesCOportunidad.NroColumasMatriz);
            }
            else
            { // los datos para visualizar en el excel web provienen de un archivo dat cargado por el usuario
                //Carga de archivo dat               
                model.Handson.ListaExcelData = this.MatrizExcel; /// Data del archivo cargado previamente se ha guardado en una variable session                    
            }

            listaReprogramado = new List<EveMailsDTO>();
            listaReprogramado = servicio.GetListaReprogramado(dfecha);
            model.ListaReprograma = listaReprogramado;

            // pinta potencia efectiva de las centrales
            ParametroAppServicio srvParametro = new ParametroAppServicio();
            List<PrGrupodatDTO> rpf = new List<PrGrupodatDTO>();
            //List<PrGrupodatDTO> rpf = servicio.GetListaPotenciaEfectiva(dfecha, ConstantesCostoOportunidad.PorcentajeRpf, ConstantesCOportunidad.origlectcodi);
            //var rptHid = servicio.GetListaPotenciaEfectiva(dfecha, ConstantesCostoOportunidad.PorcentajeRpfHidro, ConstantesCOportunidad.origlectcodi);
            //if (rptHid.Count > 0)
            //{
            //    foreach (var ad in rptHid) { rpf.Add(ad); }
            //}
            rpf.Add(new PrGrupodatDTO() { Fechadat = dfecha, Formuladat = srvParametro.GetMagnitudRPF(dfecha).ToString() });
            model.ListaPotenciaEfec = rpf;

            #region Filas Cabeceras

            model.Handson.ListaColWidth.Add(300);
            for (var ind = 1; ind < ConstantesCOportunidad.NroColumasMatriz; ind++)
            {
                model.Handson.ListaColWidth.Add(160);
            }
            ToolsCostoOport.GeneraCabecera(model.Handson.ListaExcelData, ConstantesCOportunidad.NroColumasMatriz);

            #endregion

            return model;
        }

        /// <summary>
        /// Graba los datos enviados de bandas máximas y mínimas
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWebParametros(string[][] data, string fecha/*, List<TipoDatos> listaPtos*/)
        {
            List<TipoDatos> listaPtos = null;
            FormatResultado model = GrabarExcelWebBandaNCP(data, fecha, listaPtos, User.Identity.Name);
            return Json(model);
        }

        /// <summary>
        /// Graba los datos enviados a traves de la grilla excel de bandas máximas y mínimas
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public FormatResultado GrabarExcelWebBandaNCP(string[][] data, string fecha, List<TipoDatos> listaPtos, string usuario)
        {
            int exito = 0;
            FormatResultado model = new FormatResultado();
            model.Resultado = 0;
            DateTime dfecha = DateTime.Now;

            if (fecha != null)
            {
                dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            try
            {
                //base.ValidarSesionUsuario();
                var listaBandaNCP = ObtenerDatosBandaNCP(data, listaPtos, fecha, usuario);

                // Elimina los registros existentes con la fecha procesada
                var listaBandaNCPDelete = servicio.GetListaCoBandaNCP(dfecha);
                if (listaBandaNCPDelete != null)
                {
                    foreach (var obj in listaBandaNCPDelete)
                    {
                        servicio.DeleteCoBandancp(obj.Bandcodi);
                    }
                }

                servicio.GrabarValoresCargadosBAndaNCP(listaBandaNCP);
                exito = 1;
                model.Resultado = 1;

            }
            catch (Exception ex)
            {
                exito = -1;
                model.Resultado = -1;
                model.Mensaje = ex.Message;
            }
            model.Resultado = exito;
            return model;
        }

        public List<CoBandancpDTO> ObtenerDatosBandaNCP(string[][] data, List<TipoDatos> listaPtos, string fecha, string usuario)
        {
            List<CoBandancpDTO> entitys = new List<CoBandancpDTO>();
            DateTime dfecha = DateTime.Now;

            if (fecha != null)
            {
                dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (listaPtos != null)
            {
                for (var i = 0; i < listaPtos.Count; i++)
                {
                    var objBanda = new CoBandancpDTO();
                    objBanda.Grupocodi = listaPtos[i].Codi;
                    if (COES.Base.Tools.Util.EsNumero(data[i + ConstantesCOportunidad.NrofilasCabecera][1]))
                    {
                        objBanda.Bandmin = Convert.ToDecimal(data[i + ConstantesCOportunidad.NrofilasCabecera][1]);
                    }
                    if (COES.Base.Tools.Util.EsNumero(data[i + ConstantesCOportunidad.NrofilasCabecera][2]))
                    {
                        objBanda.Bandmax = Convert.ToDecimal(data[i + ConstantesCOportunidad.NrofilasCabecera][2]);
                    }
                    objBanda.Bandfecha = dfecha;
                    objBanda.Bandfecmodificacion = DateTime.Now;
                    objBanda.Bandusumodificacion = usuario;
                    entitys.Add(objBanda);
                }
            }
            else
            {
                for (var i = 0; i < data.ToList().Count - 1; i++)
                {
                    var objBanda = new CoBandancpDTO();
                    if (COES.Base.Tools.Util.EsNumero(data[i + ConstantesCOportunidad.NrofilasCabecera][1]))
                    {
                        objBanda.Bandmin = Convert.ToDecimal(data[i + ConstantesCOportunidad.NrofilasCabecera][1]);
                    }
                    if (COES.Base.Tools.Util.EsNumero(data[i + ConstantesCOportunidad.NrofilasCabecera][2]))
                    {
                        objBanda.Bandmax = Convert.ToDecimal(data[i + ConstantesCOportunidad.NrofilasCabecera][2]);
                    }
                    objBanda.Grupocodi = int.Parse(data[i + ConstantesCOportunidad.NrofilasCabecera][5]);
                    objBanda.Bandfecha = dfecha;
                    objBanda.Bandfecmodificacion = DateTime.Now;
                    objBanda.Bandusumodificacion = usuario;
                    entitys.Add(objBanda);
                }
            }
            return entitys;
        }
        #endregion

        #region "TAB 03 - REPORTES"

        [HttpPost]
        public PartialViewResult ListaReporte(string fecha)
        {
            CostoOportunidadModel model = new CostoOportunidadModel();
            DateTime fechaIni = DateTime.MinValue;
            fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            var lista48 = this.servicio.GetReservaProgramado(fechaIni, ConstantesCostoOportunidad.LectcodiDespachoConReserva);
            this.ListaDespacho = lista48;
            var resultado = this.servicio.GenerarHtmlDespacho(lista48, fechaIni);
            model.Resultado = resultado;
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ListaDespachoSinReserva(string fecha)
        {
            CostoOportunidadModel model = new CostoOportunidadModel();
            DateTime fechaIni = DateTime.MinValue;
            fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            var lista48Sin = this.servicio.GetReservaProgramado(fechaIni, ConstantesCostoOportunidad.LectcodiDespachoSinReserva);
            this.ListaDespachoSin = lista48Sin;
            string resultado = string.Empty;
            resultado = this.servicio.GenerarHtmlDespachoSinReserva(lista48Sin, fechaIni);
            model.Resultado = resultado;
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ReporteReservaEject(string fecha)
        {
            CostoOportunidadModel model = new CostoOportunidadModel();
            DateTime dtfecha = DateTime.MinValue;
            dtfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            this.ListaReservaEjec = this.servicio.GetReservaEjec(dtfecha);
            string resultado = this.servicio.GenerarHtmlReservaEjec(this.ListaReservaEjec, dtfecha);
            model.Resultado = resultado;
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ReporteReservaProg(string fecha)
        {
            CostoOportunidadModel model = new CostoOportunidadModel();
            DateTime dtfecha = DateTime.MinValue;
            dtfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            this.ListaReservaProg = this.servicio.GetReservaProgramado(dtfecha, ConstantesCostoOportunidad.LectcodiReservaProgramada);
            string resultado = this.servicio.GenerarHtmlReservaProg(this.ListaReservaProg, dtfecha);
            model.Resultado = resultado;
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ReporteCOConReserva(string fecha)
        {
            CostoOportunidadModel model = new CostoOportunidadModel();
            DateTime dtfecha = DateTime.MinValue;
            dtfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            this.ListaCruceReserva = CrearlistaCruceReserva();
            string resultado = this.servicio.GenerarHtmlCruceReserva(this.ListaReservaEjec, this.ListaDespacho, this.ListaDespachoSin, this.ListaReservaProg, this.ListaCruceReserva, dtfecha, 1);
            model.Resultado = resultado;
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ReporteCOSinReserva(string fecha)
        {
            CostoOportunidadModel model = new CostoOportunidadModel();
            DateTime dtfecha = DateTime.MinValue;
            dtfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            this.ListaCruceReserva = CrearlistaCruceReserva();
            string resultado = this.servicio.GenerarHtmlCruceReserva(this.ListaReservaEjec, this.ListaDespacho, this.ListaDespachoSin, this.ListaReservaProg, this.ListaCruceReserva, dtfecha, 0);
            model.Resultado = resultado;
            return PartialView(model);
        }

        private List<MeMedicion48DTO> CrearlistaCruceReserva()
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            MeMedicion48DTO registro;
            foreach (var reg in this.ListaReservaProg)
            {
                registro = new MeMedicion48DTO();
                registro.Grupocodi = reg.Grupocodi;
                lista.Add(registro);
            }
            return lista;
        }

        [HttpPost]
        public JsonResult GenerarReporteXls(string fecha)
        {
            int indicador = 1;
            DateTime dtfecha = DateTime.MinValue;
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.Directorio;
                dtfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                this.servicio.GenerarArchivoExcelDespacho(dtfecha, ruta + ConstantesCOportunidad.RptExcel);
            }
            catch (Exception ex)
            {
                log.Error("GenerarReporteXls", ex);
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte general a formato excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesCOportunidad.RptExcel;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.Directorio;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
        #endregion

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("DespachoDiarioController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("DespachoDiarioController", ex);
                throw;
            }
        }
    }
}