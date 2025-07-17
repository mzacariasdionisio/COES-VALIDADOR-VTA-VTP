using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Servicios.Aplicacion.CortoPlazo
{
    public class CostoMarginalTnaAppServicio
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CostoMarginalAppServicio));

        public CostoMarginalTnaAppServicio()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Procesar el calculo de costos marginales
        /// </summary>
        /// <summary>
        /// Permite ejecutar el proceso
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="path"></param>
        public void Procesar(DateTime fechaProceso, int indicadorPSSE, bool reproceso, bool indicadorNCP, bool flagWeb, string rutaNCP, bool flagMD, int idEscenario, string usuario, int tipo)
        {
            bool indicadorProceso = false;
            string mensaje = string.Empty;
            this.ProcesarGeneral(fechaProceso, indicadorPSSE, reproceso, indicadorNCP, false, out indicadorProceso, flagWeb, rutaNCP, flagMD, idEscenario, usuario, tipo, string.Empty, out mensaje) ;
        }

        /// <summary>
        /// Permite ejecutar el proceso masivamente
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="horas"></param>
        public void ProcesarMasivo(DateTime fechaInicio, DateTime fechaFin, List<string> horas, bool flagMD, string usuario)
        {
            try
            {
                List<ResultadoProcesoMasivo> entitys = new List<ResultadoProcesoMasivo>();

                int nroDias = (int)fechaFin.Subtract(fechaInicio).TotalDays;

                for (int i = 0; i <= nroDias; i++)
                {
                    foreach (string hora in horas)
                    {
                        string[] arrayHora = hora.Split(ConstantesAppServicio.CaracterDosPuntos);
                        DateTime fechaProceso = fechaInicio.AddDays(i).AddHours(int.Parse(arrayHora[0])).AddMinutes(int.Parse(arrayHora[1]));

                        //- Realizamos el cambio para el caso del último periodo
                        //if (hora == ConstantesCortoPlazo.UltimoPeriodo)
                        //{
                        //    fechaProceso = fechaInicio.AddDays(i + 1);
                        //}

                        bool indicadorProceso = false;
                        string mensaje = string.Empty;
                        this.ProcesarGeneral(fechaProceso, 0, true, false, true, out indicadorProceso, false, string.Empty, flagMD, 0, usuario, 1, string.Empty, out mensaje);

                        ResultadoProcesoMasivo item = new ResultadoProcesoMasivo();
                        item.FechaProceso = fechaProceso.ToString(ConstantesAppServicio.FormatoFechaHora);
                        item.Resultado = indicadorProceso;

                        entitys.Add(item);
                    }
                }

                UtilCortoPlazo.EnviarCorreoEjecucionReprocesoMasivo(entitys, fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// Permite reprocesar masivamente con modificacion de parametros
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="usuario"></param>
        public void ProcesarMasivoModificado(string[][] datos, string usuario)
        {
            try
            {
                List<ResultadoProcesoMasivo> entitys = new List<ResultadoProcesoMasivo>();
                foreach (string[] data in datos)
                {
                    string fecha = data[1];
                    string hora = data[2];
                    string strFecha = fecha;
                    string filename = data[4];
                    int indicadorPSSE = 0;

                    int idEsceneario = 0;

                    if(!string.IsNullOrEmpty(data[3]))
                    {
                        idEsceneario = int.Parse(data[3]);
                        if (idEsceneario == -1) idEsceneario = 0;
                    }

                    if (!string.IsNullOrEmpty(filename)) 
                    {
                        if (int.Parse(filename) == 1)
                        {
                            indicadorPSSE = 2;
                            filename = data[0] + ".raw";
                        }
                    }

                    if (hora == ConstantesCortoPlazo.UltimoPeriodo)
                    {
                        DateTime newFecha = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture).AddDays(1);
                        strFecha = newFecha.ToString(ConstantesAppServicio.FormatoFecha);
                        hora = ConstantesCortoPlazo.PrimerPeriodo;
                    }

                    string time = strFecha + " " + hora;
                    DateTime fechaProceso = DateTime.ParseExact(time, ConstantesAppServicio.FormatoFechaHora, CultureInfo.InvariantCulture);

                    bool indicadorProceso = false;
                    string mensaje = string.Empty;
                    this.ProcesarGeneral(fechaProceso, indicadorPSSE, true, false, true, out indicadorProceso, false, string.Empty, true, idEsceneario, usuario, 1, filename, out mensaje);

                    ResultadoProcesoMasivo item = new ResultadoProcesoMasivo();
                    item.FechaProceso = fechaProceso.ToString(ConstantesAppServicio.FormatoFechaHora);
                    item.Resultado = indicadorProceso;
                    item.Mensaje = mensaje;

                    entitys.Add(item);
                }

                UtilCortoPlazo.EnviarCorreoEjecucionReprocesoMasivoModificado(entitys);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }


        /// <summary>
        /// Permite reprocesar masivamente con modificacion de parametros
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="usuario"></param>
        public void ProcesarMasivoTIE(ParametrosAnguloOptimo result, string usuario)
        {
            try
            {
                List<ResultadoProcesoMasivo> entitys = new List<ResultadoProcesoMasivo>();
                string path = string.Empty;
                List<DatosReproceso> datosReproceso = result.ListaProceso.Select(x => new DatosReproceso
                { FilePsse = x.FilePsse, FechaProceso = x.FechaProceso }).Distinct().ToList();

                foreach (DatosReproceso item in datosReproceso)
                {
                    path = String.Format(ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathExportacionTNA] + "{0:0000}\\{1:00}\\{2:00}\\", item.FechaProceso.Year, item.FechaProceso.Month, item.FechaProceso.Day);
                    string fileName = ConstantesCortoPlazo.FileRawTIE;

                    if (FileServerScada.VerificarExistenciaFile("tie", fileName, path))
                    {
                        FileServerScada.DeleteBlob(@"tie\" + fileName, path);
                    }

                    FileServerScada.UploadFromStream(item.FilePsse, @"tie\", fileName, path);

                    bool indicadorProceso = false;
                    string mensaje = string.Empty;

                    this.ProcesarGeneral(item.FechaProceso, 3, true, false, true, out indicadorProceso, false, string.Empty, true, 
                        0, usuario, 3, fileName, out mensaje);

                    ResultadoProcesoMasivo itemMensaje = new ResultadoProcesoMasivo();
                    itemMensaje.FechaProceso = item.FechaProceso.ToString(ConstantesAppServicio.FormatoFechaHora);
                    itemMensaje.Resultado = indicadorProceso;
                    itemMensaje.Mensaje = mensaje;

                    entitys.Add(itemMensaje);
                }

                UtilCortoPlazo.EnviarCorreoEjecucionReprocesoTIE(entitys);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// Permite realizar el proceso con todos los datos
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="indicadorPSSE"></param>
        /// <param name="reproceso"></param>
        /// <param name="indicadorNCP"></param>
        private void ProcesarGeneral(DateTime fechaProceso, int indicadorPSSE, bool reproceso, bool indicadorNCP, bool indicadorMasivo, out bool indicadorProceso,
            bool flagWeb, string rutaNCP, bool flagMD, int idEscenario, string usuario, int tipo, string fileReproceso, out string mensajeResultado)
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger.Info("ProcesarGeneral (ProcesarGeneral):\t" + fechaProceso.ToString());
            //- Para el manejo de errores
            int indicadorOperacion = ConstantesCortoPlazo.OperacionCorrecta;
            string mensaje = string.Empty;
            string logInconsistencias = string.Empty;
            string logModosOperacion = string.Empty;
            string logOperacionEMS = string.Empty;
            bool flagInconsistencias = false;
            bool flagModosOperacion = false;
            bool flagOperacionEMS = false;
            bool flagValoresNegativos = false;
            indicadorProceso = false;
            //- Obtenemos el correlativo correspondiente al proceso
            int correlativo = FactorySic.GetCmCostomarginalRepository().ObtenerMaxCorrelativo();

            List<ResultadoGams> listaResultado = new List<ResultadoGams>();
            listaResultado.Add(new ResultadoGams
            {
                Nombarra = ConstantesAppServicio.NoExisteResultado,
                Congestion = 0,
                Energia = 0,
                Total = 0
            });

            #region Ticket 2022-004345
            List<EqRelacionDTO> listEquiposSinBarra = new List<EqRelacionDTO>();
            #endregion

            bool flagValidacionDatos = true;

            Logger.Info("AlmacenarCostosMarginales (correlativo):\t" + correlativo);

            (new CortoPlazoAppServicio()).AlmacenarCostosMarginales(listaResultado, fechaProceso, correlativo, new List<NombreCodigoBarra>(), reproceso,
                flagWeb, out flagValidacionDatos, 0, usuario, flagMD);

            try
            {
                //- Definimos la cadena para el manejo de logs
                string log = string.Empty;

                //- Definimos la carpeta raiz
                string pathRaiz = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];

                Logger.Info("pathRaiz:\t" + pathRaiz);

                //- Definir que periodo se está ejecutando
                int periodo = UtilCortoPlazoTna.CalcularPeriodo(fechaProceso);

                //- Obtenemos los archivos NCP a trabajar
                string reprograma = string.Empty; // (new CortoPlazoAppServicio()).ObtenerReprogramaActual(fechaProceso);
                CpTopologiaDTO topologiaFinal = null;
                FileHelperTna.VerificarExistenciaReprograma(fechaProceso, pathRaiz, indicadorNCP, out reprograma, rutaNCP, flagMD, out topologiaFinal, idEscenario, periodo);

                //- Carpeta con los datos de la corrida
                bool flagPrograma = false;
                string pathTrabajo = FileHelperTna.EstablecerCarpetaTrabajo(fechaProceso, pathRaiz, correlativo, out flagPrograma, flagMD, topologiaFinal);
                Logger.Info("pathTrabajo:\t" + pathTrabajo);

                if (flagPrograma)
                {

                    //- Obtenemos el archivo .raw
                    string filePsse = ConstantesCortoPlazo.ArchivoPSSE + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionRaw);
                    //-- Cambio para nuevo TNA --//
                    bool indicadorPsse = FileHelperTna.ObtenerArchivoRaw(filePsse, pathTrabajo, pathRaiz, fechaProceso, indicadorPSSE, fileReproceso);

                    if (indicadorPsse)
                    {
                        //- Obtenemos la configuracion de los nombres y códigos de las barras
                        //-- Cambio para nuevo TNA --//
                        List<NombreCodigoBarra> relacionBarra = FileHelperTna.ObtenerBarras(pathTrabajo + filePsse, pathRaiz);

                        //- Obtenemos la configuracion de las líneas
                        //- Cambio para nuevo TNA --//
                        List<NombreCodigoLinea> relacionLinea = FileHelperTna.ObtenerLineas(pathTrabajo + filePsse, pathRaiz, relacionBarra);

                        //- Obtenemos la relacion de los trafos 2D y 3D
                        //- Cambio para nuevo TNA --//
                        List<TrafoEms> relacionTrafo = FileHelperTna.ObtenerListadoTrafo(pathTrabajo + filePsse, pathRaiz, relacionBarra);

                        #region Variables para MDCOES

                        List<CpRecursoDTO> listaRelBarraCentral = new List<CpRecursoDTO>();
                        List<CpRecursoDTO> lstRecursosVolMaxMin = new List<CpRecursoDTO>();
                        List<CpMedicion48DTO> lstVolumenesEmbalses = new List<CpMedicion48DTO>();
                        List<CpMedicion48DTO> lstCmgBarras = new List<CpMedicion48DTO>();

                        if (flagMD)
                        {
                            //Topologia final
                            //CpTopologiaDTO topologiaFinal = (new McpAppServicio()).ObtenerTopologiaFinalPorFecha(fechaProceso, ConstantesCortoPlazo.TopologiaDiario);

                            //Equivalencias entre barra y centrales hidraúlicas
                            listaRelBarraCentral = (new McpAppServicio()).ObtenerListaRelacionBarraCentral(topologiaFinal);

                            //Lista de embalse turbinado a central, mas propiedades volumen máximo y mínimo
                            lstRecursosVolMaxMin = (new McpAppServicio()).ObtenerVolMinMaxDeEmbalseCentralPorTopologiaFinal(topologiaFinal);

                            DateTime fechaProcesoOrigen;

                            if (fechaProceso.Hour == 0 && fechaProceso.Minute == 0)
                                fechaProcesoOrigen = (new DateTime(fechaProceso.Year, fechaProceso.Month, fechaProceso.Day)).AddDays(-1).AddHours(23).AddMinutes(59);
                            else
                                fechaProcesoOrigen = fechaProceso;

                            // Lista de volumen de embalses programados
                            lstVolumenesEmbalses = (new McpAppServicio()).ObtenerCpMedicion48(topologiaFinal, fechaProcesoOrigen, ConstantesCortoPlazo.SrestcodiVolumenesEmbalses.ToString(), true);

                            lstCmgBarras = (new McpAppServicio()).ObtenerCpMedicion48(topologiaFinal, fechaProcesoOrigen, ConstantesCortoPlazo.SrestcodiCmgPorBarra.ToString(), true);

                            if (periodo == 0) periodo = 48;
                        }

                        #endregion


                        //- Cargado datos de generacion del PSSE y NCP
                        List<string[]> datosEPPS = FileHelperTna.ObtenerDatosEPPS(pathTrabajo + filePsse, pathRaiz);

                        //- Obteniendo datos necesarios desde el NCP
                        List<string[]> datosPotHidraulico = new List<string[]>();
                        List<string[]> costoMarginalBarra = new List<string[]>();
                        List<string[]> equivalenciaBarraCentral = new List<string[]>();
                        List<string[]> datosVolumenHidraulico = new List<string[]>();

                        if (!flagMD)
                        {
                            datosPotHidraulico = FileHelper.ObtenerPotenciasHidraulico(pathTrabajo + ConstantesCortoPlazo.FileDatoHidraulico, pathRaiz);
                            //List<string[]> datosValorAgua = FileHelper.ObtenerDatosCentralesHidraulicas(pathTrabajo + ConstantesCortoPlazo.FileValorAgua, pathRaiz);
                            //- Jalamos los costos marginales por barra
                            costoMarginalBarra = FileHelper.ObtenerDatosCentralesHidraulicas(pathTrabajo + ConstantesCortoPlazo.FileCMBarra, pathRaiz);
                            //- Jalamos las equivalencias entre barra y centrales hidraúlicas
                            equivalenciaBarraCentral = FileHelper.ObtenerEquivalenciaBarraCentral(pathTrabajo + ConstantesCortoPlazo.FileEquivBarraCentral, pathRaiz);
                            //- Obtiene el valomun programado de las centrales hidroelectricas del NCP
                            datosVolumenHidraulico = FileHelper.ObtenerDatosCentralesHidraulicas(pathTrabajo + ConstantesCortoPlazo.FileVolumenProgramado, pathRaiz);
                        }

                        // Obtiene la cantidad de grupos para las hidráulicas
                        List<EqRelacionDTO> contadorGrupo = FactorySic.GetEqRelacionRepository().ObtenerContadorGrupo();

                        //- Lista con la relación de generadores a procesar indicando sus modos de operacion y si pertenece a un cc                       
                        bool flagFenixValidacion = false;
                        List<EqRelacionDTO> listaRestricciones = new List<EqRelacionDTO>();
                        //-- Cambio por nuevo tna --//
                        #region Ticket 2022-004345                       
                        List<EqRelacionDTO> list = this.ObtenerRelacion(relacionBarra, datosEPPS, fechaProceso, ref log, equivalenciaBarraCentral, reproceso, out flagFenixValidacion,
                            out listaRestricciones, flagMD, listaRelBarraCentral, out listEquiposSinBarra);
                        #endregion
                        //- Evaluacion de unidades para determinar cuales participan en la evaluacion de CM              
                        this.EvaluacionUnidades(fechaProceso, datosPotHidraulico, datosVolumenHidraulico, ref list, periodo, ref log, lstRecursosVolMaxMin, lstVolumenesEmbalses, flagMD);

                        //- Obtenemnos los datos de generación forzada
                        List<PrGenforzadaDTO> forzadaInicial = new List<PrGenforzadaDTO>();
                        //-- Cambio por nuevo tna --//
                        List<PrGenforzadaDTO> resultadoForzada = this.ObtenerGeneracionForzada(fechaProceso, relacionBarra, datosEPPS, list, ref log, out forzadaInicial);

                        //- Obtenemos los datos de congestion 
                        List<PrCongestionDTO> congestionTrafo3D = new List<PrCongestionDTO>();
                        List<PrCongestionDTO> resultadoCongestion = this.ObtenerCongestion(fechaProceso, relacionBarra, relacionLinea, relacionTrafo, ref log, out congestionTrafo3D);



                        //- Obtenemos los datos de congestion conjunta
                        List<PrCongestionDTO> resultadoCongestionGrupo = this.ObtenerCongestionGrupo(fechaProceso, relacionBarra, relacionLinea, ref log, congestionTrafo3D);


                        #region Regiones_seguridad

                        List<PrCongestionDTO> congestionRegionSeguridad = this.ObtenerCongestionRegionSeguridad(fechaProceso, relacionBarra, relacionLinea, list, ref log);

                        #endregion


                        //- Obtenemos los datos de la seccion de generacion                        
                        //List<RegistroGenerado> resultadoGeneracion = this.ObtenerDatosGeneracion(periodo, list, datosEPPS, datosValorAgua, ref log, out logInconsistencias,
                        //    out flagInconsistencias, out logModosOperacion, out flagModosOperacion);
                        //-- Cambio por nuevo tna --// 
                        List<RegistroGenerado> resultadoGeneracion = this.ObtenerDatosGeneracion(fechaProceso, periodo, list, datosEPPS, costoMarginalBarra, ref log, out logInconsistencias,
                            out flagInconsistencias, out logModosOperacion, out flagModosOperacion, out logOperacionEMS, out flagOperacionEMS, flagFenixValidacion,
                            flagMD, lstCmgBarras);

                        //- Generamos el archivo resultado
                        #region Ticket 2022-004345

                        List<string> validacionArchivoResultado = new List<string>();

                        string fileResultado = ConstantesCortoPlazo.ArchivoProprocesador + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionGen);
                        FileHelperTna.GenerarArchivoResultado(resultadoGeneracion, resultadoCongestion, resultadoCongestionGrupo, resultadoForzada,
                            pathRaiz, pathTrabajo + fileResultado, out validacionArchivoResultado);

                        if (validacionArchivoResultado.Count > 0)
                        {
                            throw new InvalidOperationException(UtilCortoPlazoTna.ObtenerMensajeValidacionArchivoResultado(validacionArchivoResultado));
                        }

                        #endregion

                        #region Quitamos las secciones temporalmente

                        //- Generamos el archivo Log de datos
                        string fileLog = ConstantesCortoPlazo.ArchivoLOG + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionCsv);
                        FileHelperTna.GenerarArchivoLog(pathTrabajo + fileLog, pathRaiz, log);

                        //- Generamos el archivo DAT que será entrada al GAMS

                        //- Creamos los objetos: contienen sus propiedades calculadas
                        List<LineaEms> objetoLinea = UtilCortoPlazoTna.ObtenerObjetoLinea(relacionLinea);

                        //- Obtenemos las cargas
                        List<Carga> relacionCarga = UtilCortoPlazoTna.ObtenerCargas(pathTrabajo + filePsse, pathRaiz, relacionBarra);

                        //- Obtener los shunts
                        List<Shunt> relacionShunt = UtilCortoPlazoTna.ObtenerShunts(pathTrabajo + filePsse, pathRaiz);

                        ////- Obtener datos de generadores
                        //List<Generator> relacionGeneracion = UtilCortoPlazoTna.ObtenerGeneracion(datosEPPS);

                        ////- Lectura de Switch dinámicos
                        //List<SwitchedShunt> relacionShuntDinamicos = UtilCortoPlazoTna.ObtenerShuntsDinamicos(pathTrabajo + filePsse, pathRaiz);


                        //- Creación de nuevas instancias para AC

                        //List<Carga> relacionCargaAC = new List<Carga>(relacionCarga);
                        //List<NombreCodigoBarra> relacionBarraAC = new List<NombreCodigoBarra>(relacionBarra);
                        //List<Shunt> relacionShuntAC = new List<Shunt>(relacionShunt);
                        //List<LineaEms> objetoLineaAC = new List<LineaEms>(objetoLinea);
                        //List<TrafoEms> relacionTrafoAC = new List<TrafoEms>(relacionTrafo);
                        //List<RegistroGenerado> resultadoGeneracionAC = new List<RegistroGenerado>(resultadoGeneracion);
                        //List<PrGenforzadaDTO> resultadoForzadaAC = new List<PrGenforzadaDTO>(resultadoForzada);
                        //List<PrCongestionDTO> resultadoCongestionAC = new List<PrCongestionDTO>(resultadoCongestion);
                        //List<PrCongestionDTO> resultadoCongestionGrupoAC = new List<PrCongestionDTO>(resultadoCongestionGrupo);
                        //List<Generator> relacionGeneracionAC = new List<Generator>(relacionGeneracion);
                        //List<SwitchedShunt> relacionShuntDinamicosAC = new List<SwitchedShunt>(relacionShuntDinamicos);
                        //- Finalización de nuevas instancias para AC

                        //- Generar archivo GAMS DC
                        #region Regiones_seguridad
                        string entradaGams = UtilCortoPlazoTna.GenerarEntradaGams(relacionCarga, relacionBarra, relacionShunt, objetoLinea, relacionTrafo, resultadoGeneracion,
                            resultadoForzada, resultadoCongestion, resultadoCongestionGrupo, congestionRegionSeguridad, relacionBarra);
                        #endregion

                        ////- Escribimos en una ruta el archivo entrada
                        string fileEntradaGams = ConstantesCortoPlazo.ArchivoEntradaGams + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionDat);
                        bool flagEntradaGams = FileHelperTna.GenerarArchivoEntradaGams(pathTrabajo + fileEntradaGams, pathRaiz, entradaGams);

                        //- Ejecucion gams
                        string fileSalidaGams = ConstantesCortoPlazo.ArchivoResultadoGams + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionCsv);
                        string fileSalidaGamsAnalisis = ConstantesCortoPlazo.ArchivoResultadoGams2 + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionCsv);
                        //string fileSalidaGamsAlternativo = ConstantesCortoPlazo.ArchivoResultadoAlternativoGams + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionCsv);
                        //string fileSalidaGamsAlternativoAnalisis = ConstantesCortoPlazo.ArchivoResultadoAlternativoGamsAnalisis + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionCsv);

                        try
                        {
                            #region Movisoft - Ticket 18685
                            //- Ejecutamos el modelo GAMS
                            string workspace = "Workspace\\";
                            int resultadoGams = EjecucionGams.Ejecutar(fechaProceso, pathRaiz, pathTrabajo + fileEntradaGams, pathTrabajo + fileSalidaGams, pathTrabajo + fileSalidaGamsAnalisis, pathTrabajo + workspace);
                            listaResultado = FileHelperTna.ObtenerResultadoGams(pathTrabajo + fileSalidaGams, pathRaiz);

                            string fileGams = "MODELO_" + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ".gms");
                            string fileLst = "LST_" + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ".lst");
                            FileHelperTna.CopiarArchivoLST(pathTrabajo, pathTrabajo + workspace, fileGams, fileLst, pathRaiz);

                            #endregion

                            //- Ejecutamos el modelo anternativo GAMS                            
                            //int resultadoGamsAlternativo = EjecucionGams.EjecutarAlternativo(fechaProceso, pathRaiz, pathTrabajo + fileEntradaGams, pathTrabajo + fileSalidaGamsAlternativo,
                            //    pathTrabajo + fileSalidaGamsAlternativoAnalisis);

                            indicadorProceso = true;
                        }
                        catch (Exception ex)
                        {
                            indicadorOperacion = ConstantesCortoPlazo.ErrorGamsNoEjecuto;
                            Logger.Error("EjecucionGams.Ejecutar" + fechaProceso, ex);
                        }

                        try
                        {
                            // Generar archivo GSMS AC
                            //string entradaGamsAC = UtilCortoPlazoTna.GenerarEntradaGamsAC(relacionCargaAC, relacionBarraAC, relacionShuntAC, objetoLineaAC, relacionTrafoAC, resultadoGeneracionAC,
                            //    resultadoForzadaAC, resultadoCongestionAC, resultadoCongestionGrupoAC, relacionBarraAC, relacionGeneracionAC, relacionShuntDinamicosAC);

                            ////- Escribimos en la ruta el archivo de entrada AC
                            //string fileEntradaGamsAC = ConstantesCortoPlazo.ArchivoEntradaGamsAC + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionDat);
                            //bool flagEntradaGamsAC = FileHelperTna.GenerarArchivoEntradaGams(pathTrabajo + fileEntradaGamsAC, pathRaiz, entradaGamsAC);

                            //string fileSalidaCmgAC = ConstantesCortoPlazo.ArchivoResultadoAC + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionCsv);
                            //string fileSalidaFlujoAC = ConstantesCortoPlazo.ArchivoFlujoAC + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionCsv);
                            //string fileSalidaAddAC = ConstantesCortoPlazo.ArchivoAddAC + UtilCortoPlazoTna.ObtenerFileName(fechaProceso, ConstantesCortoPlazo.ExtensionCsv);

                            //int resultadoGamsAC = EjecucionGams.EjecutarModeloAC(fechaProceso, pathRaiz, pathTrabajo + fileEntradaGamsAC, pathTrabajo + fileSalidaCmgAC,
                            //    pathTrabajo + fileSalidaFlujoAC, pathTrabajo + fileSalidaAddAC);                            

                        }
                        catch (Exception ex)
                        {

                        }


                        //- Artificio temporal para que permita grabar datos

                        if (listaResultado.Count == 0 || listaResultado.Count == 1)
                        {
                            listaResultado.Add(new ResultadoGams
                            {
                                Nombarra = "OCONA500XC2",
                                Congestion = 0,
                                Energia = 0,
                                Total = 0
                            });
                        }

                        //- Fin artificio

                        flagValidacionDatos = true;


                        (new CortoPlazoAppServicio()).AlmacenarCostosMarginales(listaResultado, fechaProceso, correlativo,
                          relacionBarra, reproceso, flagWeb, out flagValidacionDatos, 1, usuario, flagMD);


                        //- Sección modificada para alertas de CM

                        if (listaResultado.Where(x => x.Total < 0).Count() > 0)
                        {
                            flagValoresNegativos = true;
                        }

                        string indNegativo = ConstantesAppServicio.NO;
                        if (reproceso == false)
                        {
                            if (flagValoresNegativos)
                                indNegativo = ConstantesAppServicio.SI;

                            decimal maxCM = listaResultado.Max(x => x.Total);
                            decimal maxCMCongestion = (listaResultado.Where(x => x.Congestion > 0).Count() > 0) ?
                                listaResultado.Where(x => x.Congestion > 0).Max(x => x.Total) : 0;
                            decimal maxCMSinCongestion = (listaResultado.Where(x => x.Congestion <= 0).Count() > 0) ?
                                listaResultado.Where(x => x.Congestion <= 0).Max(x => x.Total) : 0;
                            decimal maxCICongestion = (resultadoGeneracion.Where(x => x.IndLimTrans).Count() > 0) ?
                                resultadoGeneracion.Where(x => x.IndLimTrans).Max(x => (decimal)x.CostoIncremental) : 0;
                            decimal maxCISinCongestion = (resultadoGeneracion.Where(x => !x.IndLimTrans).Count() > 0) ?
                                resultadoGeneracion.Where(x => !x.IndLimTrans).Max(x => (decimal)x.CostoIncremental) : 0;

                            (new CortoPlazoAppServicio()).GenerarAlertaValoresNegativos(indNegativo, maxCM, maxCMCongestion, maxCMSinCongestion, maxCICongestion,
                                maxCISinCongestion, fechaProceso);
                        }

                        //(new CortoPlazoAppServicio()).GenerarAlertaValoresNegativos(indNegativo, maxCM);

                        try
                        {
                            (new CortoPlazoAppServicio()).AlmacenarDataGeneracionEms(list, fechaProceso, correlativo,
                                ConstantesCortoPlazo.EstimadorTNA);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("AlmacenarDataGeneracionEms" + fechaProceso, ex);
                        }

                        try
                        {
                            //- Calculamos los flujos en la líneas
                            List<EqCongestionConfigDTO> listaLineas = FactorySic.GetEqCongestionConfigRepository().ObtenerListadoEquipos();
                            List<CmFlujoPotenciaDTO> listaFlujos = this.ObtenerFlujosPotencia(listaLineas, fechaProceso, correlativo, relacionBarra, relacionLinea, relacionTrafo);
                            (new CortoPlazoAppServicio()).AlmacenarFlujoLineas(listaFlujos, fechaProceso);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("AlmacenarDataFlujoLinea" + fechaProceso, ex);
                        }

                        //-- Permite almacenar los datos de analisis
                        try
                        {
                            if (indicadorOperacion != ConstantesCortoPlazo.ErrorGamsNoEjecuto)
                            {
                                bool resultado = false;
                                resultado = (new CortoPlazoAppServicio()).ObtenerCostosMarginalesValorizacionDiaria(fechaProceso, periodo, pathTrabajo + fileSalidaGamsAnalisis, pathRaiz, list,
                                    resultadoCongestion, resultadoCongestionGrupo, congestionRegionSeguridad, resultadoGeneracion, correlativo);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("ObtenerCostosMarginalesValorizacionDiaria" + ex);
                        }
                        //-- Fin almacenamiento de datos de análisis

                        //- Almacenamiento del dato de programa y restricciones operativas utilizadas
                        try
                        {
                            CmVersionprogramaDTO versionPrograma = new CmVersionprogramaDTO();
                            versionPrograma.Cmgncorrelativo = correlativo;
                            versionPrograma.Cmveprvalor = reprograma;
                            versionPrograma.Cmveprtipoprograma = (flagMD) ? ConstantesCortoPlazo.TipoMDCOES : ConstantesCortoPlazo.TipoNCP;
                            versionPrograma.Cmveprtipoestimador = ConstantesCortoPlazo.EstimadorTNA;
                            versionPrograma.Topcodi = (flagMD) ? (int?)topologiaFinal.Topcodi : null;
                            versionPrograma.Cmveprtipocorrida = tipo.ToString();
                            versionPrograma.Cmveprversion = ConstantesCortoPlazo.VersionCMOriginal;

                            //versionPrograma.Cmveprtipocorrida = tipo.ToString();

                            FactorySic.GetCmVersionprogramaRepository().Save(versionPrograma);

                            foreach (EqRelacionDTO entity in listaRestricciones)
                            {
                                CmRestriccionDTO itemRestriccion = new CmRestriccionDTO();
                                itemRestriccion.Subcausacodi = entity.Subcausacodi;
                                itemRestriccion.Equicodi = (int)entity.Equicodi;
                                itemRestriccion.Cmgncorrelativo = correlativo;
                                FactorySic.GetCmRestriccionRepository().Save(itemRestriccion);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("Error grabar datos adicionales (2): " + fechaProceso, ex);
                        }

                        #endregion

                    }
                    else
                    {
                        indicadorOperacion = ConstantesCortoPlazo.ErrorNoExistePSSE;
                    }
                }
                else
                {
                    //- Debemos realizar el cambio para obtener el mensaje correcto según fuente de datos NCP o YUPANA
                    if (!flagMD)
                        indicadorOperacion = ConstantesCortoPlazo.ErrorNoExisteArchivosNCP;
                    else
                        indicadorOperacion = ConstantesCortoPlazo.ErrorNoExisteTopologiaMD;
                }
            }            

            catch (InvalidOperationException ex)
            {
                #region Ticket 2022-004345
                //mensaje = ex.Message;               

                mensaje = ex.Message;
                #endregion

                indicadorOperacion = ConstantesCortoPlazo.ErrorEnOperacion;
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            catch (Exception ex)
            {
                #region Ticket 2022-004345
                //mensaje = ex.Message;               

                mensaje = ConstantesCortoPlazo.ErrorGenericoCM;
                #endregion

                indicadorOperacion = ConstantesCortoPlazo.ErrorEnOperacion;
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }

            //- Verificación de notificaciones por correo electrónico
            if (flagInconsistencias && flagModosOperacion && flagOperacionEMS)
            {
                indicadorOperacion = ConstantesCortoPlazo.ErrorInconsistenciaModoOperacionOperacionEMS;
                string mensajeInconsistencia = UtilCortoPlazoTna.ObtenerMensajeInconsistencia(logInconsistencias);
                string mensajeModoOperacion = UtilCortoPlazoTna.ObtenerMensajeModoOperacion(logModosOperacion);
                string mensajeOperacionEMS = UtilCortoPlazoTna.ObtenerMensajeOperacionEMS(logOperacionEMS);
                mensaje = mensajeInconsistencia + ConstantesCortoPlazo.MensajeModoOperacion + mensajeModoOperacion +
                    ConstantesCortoPlazo.MensajeOperacionEMS + mensajeOperacionEMS;
            }
            else if (flagInconsistencias && !flagModosOperacion && !flagOperacionEMS)
            {
                indicadorOperacion = ConstantesCortoPlazo.ErrorInconsistencias;
                mensaje = UtilCortoPlazoTna.ObtenerMensajeInconsistencia(logInconsistencias);
            }
            else if (!flagInconsistencias && flagModosOperacion && !flagOperacionEMS)
            {
                indicadorOperacion = ConstantesCortoPlazo.ErrorModosOperacion;
                mensaje = UtilCortoPlazoTna.ObtenerMensajeModoOperacion(logModosOperacion);
            }
            else if (!flagInconsistencias && !flagModosOperacion && flagOperacionEMS)
            {
                indicadorOperacion = ConstantesCortoPlazo.ErrorOperacionEMS;
                mensaje = UtilCortoPlazoTna.ObtenerMensajeOperacionEMS(logOperacionEMS);
            }
            else if (flagInconsistencias && flagModosOperacion && !flagOperacionEMS)
            {
                indicadorOperacion = ConstantesCortoPlazo.ErrorInconsistenciaModoOperacion;
                string mensajeInconsistencia = UtilCortoPlazoTna.ObtenerMensajeInconsistencia(logInconsistencias);
                string mensajeModoOperacion = UtilCortoPlazoTna.ObtenerMensajeModoOperacion(logModosOperacion);
                mensaje = mensajeInconsistencia + ConstantesCortoPlazo.MensajeModoOperacion + mensajeModoOperacion;
            }
            else if (!flagInconsistencias && flagModosOperacion && flagOperacionEMS)
            {
                indicadorOperacion = ConstantesCortoPlazo.ErrorModosOperacionOperacionEMS;
                string mensajeModoOperacion = UtilCortoPlazoTna.ObtenerMensajeModoOperacion(logModosOperacion);
                string mensajeOperacionEMS = UtilCortoPlazoTna.ObtenerMensajeOperacionEMS(logOperacionEMS);
                mensaje = ConstantesCortoPlazo.MensajeModoOperacion + mensajeModoOperacion + ConstantesCortoPlazo.MensajeOperacionEMS + mensajeOperacionEMS;
            }
            else if (flagInconsistencias && !flagModosOperacion && flagOperacionEMS)
            {
                indicadorOperacion = ConstantesCortoPlazo.ErrorInconsistenciaOperacionEMS;
                string mensajeInconsistencia = UtilCortoPlazoTna.ObtenerMensajeInconsistencia(logInconsistencias);
                string mensajeOperacionEMS = UtilCortoPlazoTna.ObtenerMensajeOperacionEMS(logOperacionEMS);
                mensaje = mensajeInconsistencia + ConstantesCortoPlazo.MensajeOperacionEMS + mensajeOperacionEMS;
            }

            #region Ticket 2022-004345
            string mensajeNoModeladas = string.Empty;
            if (listEquiposSinBarra.Count > 0)
            {
                 mensajeNoModeladas = UtilCortoPlazoTna.ObtenerMensajeUnidadesNoMoedeladasTNA(listEquiposSinBarra);                
            }

            #endregion

            //- Debemos enviar correo electrónico
            string mensajeEmail = UtilCortoPlazoTna.ObtenerMensajeCorreo(indicadorOperacion, mensaje, fechaProceso, flagValidacionDatos, flagWeb, flagValoresNegativos, mensajeNoModeladas);
            mensajeResultado = mensajeEmail;
            try
            {
                if (!indicadorMasivo)
                {
                    UtilCortoPlazoTna.EnviarCorreo(fechaProceso, mensajeEmail, reproceso);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// Validación del proceso
        /// </summary>
        /// <param name="fechaProceso"></param>
        public string ValidacionProceso(DateTime fechaProceso)
        {
            //- Verificamos existencia de NCP
            int validacionNCP = FileHelper.ValidarExistenciaNCP(fechaProceso);

            //- Verificamos existencia de archivo PSSE
            string filepsse = string.Empty;
            string pathpsse = string.Empty;
            int validacionPSSE = FileHelperTna.VerificarExistenciaRaw(fechaProceso, out pathpsse, out filepsse);

            //- Verificamos Horas de Operación
            //- Obtenemos la configuracion de los nombres y códigos de las barras
            List<NombreCodigoBarra> relacionBarra = FileHelperTna.ObtenerBarras(filepsse, pathpsse);
            List<string[]> datosEPPS = FileHelper.ObtenerDatosEPPS(filepsse, pathpsse);
            List<RegistroGenerado> operacionEMS = new List<RegistroGenerado>();
            bool generacionNegativa = false;
            List<RegistroGenerado> horasOperacion = this.VerificarHorasOperacion(relacionBarra, datosEPPS, fechaProceso, out operacionEMS, out generacionNegativa);

            //-Validamos existencia RSF
            List<EveRsfdetalleDTO> listadoRsf = FactorySic.GetEveRsfdetalleRepository().ObtenerUnidadesRSF(fechaProceso);

            //-Preparamos el correo a enviar
            string validacion = UtilCortoPlazo.ObtenerCorreoValidacion(validacionPSSE, validacionNCP, horasOperacion, listadoRsf, operacionEMS);

            if (!string.IsNullOrEmpty(validacion))
            {
                //- Enviar correo electrónico con validación
                string mensaje = UtilCortoPlazoTna.ObtenerCuerpoCorreoValidacion(validacion, fechaProceso);
                UtilCortoPlazo.EnviarCorreoValidacion(fechaProceso, mensaje);
            }

            return validacion;
        }

        /// <summary>
        /// Permite obtener las alertas para el aplicativo de costos marginales
        /// </summary>
        /// <param name="fechaProceso"></param>
        public ResultadoValidacion ObtenerAlertasCostosMarginales(DateTime fechaProceso)
        {
            //- Verificamos existencia de NCP
            int validacionNCP = FileHelperTna.ValidarExistenciaNCP(fechaProceso);
            bool flagPotenciaNegativa = false;
            //- Verificamos existencia de archivo PSSE
            string filepsse = string.Empty;
            string pathpsse = string.Empty;
            int validacionPSSE = FileHelperTna.VerificarExistenciaRaw(fechaProceso, out pathpsse, out filepsse);
            List<RegistroGenerado> resultadoEMS = new List<RegistroGenerado>();
            List<RegistroGenerado> horasOperacion = new List<RegistroGenerado>();
            bool comparacionRAW = false;
            //- Verificamos Horas de Operación
            if (validacionPSSE == 1)
            {
                //- Obtenemos la configuracion de los nombres y códigos de las barras
                List<NombreCodigoBarra> relacionBarra = FileHelperTna.ObtenerBarras(filepsse, pathpsse);
                List<string[]> datosEPPS = FileHelperTna.ObtenerDatosEPPS(filepsse, pathpsse);

                horasOperacion = this.VerificarHorasOperacion(relacionBarra, datosEPPS, fechaProceso, out resultadoEMS, out flagPotenciaNegativa);

                //- Realizamos la comparación con el archivo de la media anterior
                DateTime fechaProcesoAnterior = fechaProceso.AddMinutes(-30);
                string filepsseAnterior = string.Empty;
                string pathpsseAnterior = string.Empty;
                List<RegistroGenerado> resultadoEMSAnterior = new List<RegistroGenerado>();
                int validacionPSSEAnterior = FileHelperTna.VerificarExistenciaRaw(fechaProcesoAnterior, out pathpsseAnterior, out filepsseAnterior);

                if (validacionPSSEAnterior == 1)
                {
                    List<NombreCodigoBarra> relacionBarraAnterior = FileHelperTna.ObtenerBarras(filepsseAnterior, pathpsseAnterior);
                    List<string[]> datosEPPSAnterior = FileHelperTna.ObtenerDatosEPPS(filepsseAnterior, pathpsseAnterior);

                    List<RegistroGenerado> listaGeneracion = this.ObtenerDatosGeneracion(relacionBarra, datosEPPS);
                    List<RegistroGenerado> listaGeneracionAnterior = this.ObtenerDatosGeneracion(relacionBarraAnterior, datosEPPSAnterior);

                    foreach (RegistroGenerado item in listaGeneracion)
                    {
                        RegistroGenerado comparar = listaGeneracionAnterior.Where(x => x.BarraNombre == item.BarraNombre && x.GenerID == item.GenerID).FirstOrDefault();

                        if (comparar != null)
                        {
                            if (item.Potencia != comparar.Potencia)
                            {
                                comparacionRAW = true;
                            }
                        }
                    }
                }
                else
                {
                    comparacionRAW = true;
                }

                //- Fin comparación con archivo anterior
            }
            else
            {
                comparacionRAW = true;
            }


            //-Validamos existencia RSF
            List<EveRsfdetalleDTO> listadoRsf = FactorySic.GetEveRsfdetalleRepository().ObtenerUnidadesRSF(fechaProceso);

            //-Validamos los valores previos con valores negativos
            CmAlertavalorDTO resumenCM = FactorySic.GetCmAlertavalorRepository().GetById();

            bool indicadorMaximoCMCongestion = false;
            bool indicadorMaximoCMSinCongestion = false;

            try
            {
                //- Validamos los datos de cm minimo
                //string pathRaizNCP = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];
                ////- Seteamos la carpeta correspondiente al dia
                //string pathArchivo = fechaProceso.Year + @"\" +
                //          fechaProceso.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) +
                //          fechaProceso.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) + @"\cmgdemcp.csv ";
                //decimal valorCI = FileHelper.ObtenerMaximoCostosIncrementales(pathArchivo, pathRaizNCP);

                //- Listado de formulas generales
                List<PrGrupodatDTO> formulasGeneral = FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(fechaProceso);

                n_parameter parameterGeneral = new n_parameter();
                foreach (PrGrupodatDTO itemConcepto in formulasGeneral)
                {
                    if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                        parameterGeneral.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
                }

                double tipoCambio = parameterGeneral.GetEvaluate(ConstantesCortoPlazo.PropTipoCambio);
                //double valorMaximoCI = (double)valorCI * tipoCambio;
                //double valorMaximoCM = (double)valorNegativo.Alevalmax;

                //if (valorMaximoCM - valorMaximoCI >= 3 * tipoCambio)
                //{
                //    indicadorMaximoCM = true;
                //}

                //- Multiplicamos valorCI por tipo de cambio

                double cm1 = (double)resumenCM.Alevalmaxsinconge;
                double cm2 = (double)resumenCM.Alevalmaxconconge;
                double ci1 = (double)resumenCM.Alevalcisinconge;
                double ci2 = (double)resumenCM.Alevalciconconge;


                if (cm1 - ci1 >= 4 * tipoCambio)
                {
                    indicadorMaximoCMSinCongestion = true;
                }
                if (cm2 - ci2 >= 4 * tipoCambio)
                {
                    indicadorMaximoCMCongestion = true;
                }

                // temporal

                indicadorMaximoCMCongestion = false;
                indicadorMaximoCMSinCongestion = false;
            }
            catch
            {

            }

            //-Preparamos el correo a enviar
            ResultadoValidacion validacion = UtilCortoPlazoTna.ObtenerDetalleValidacion(validacionPSSE, validacionNCP, horasOperacion, listadoRsf,
                resultadoEMS, resumenCM.Alevalindicador, flagPotenciaNegativa, comparacionRAW, indicadorMaximoCMSinCongestion, indicadorMaximoCMCongestion);

            return validacion;
        }

        /// <summary>
        /// Verificar existencia de horas de operacion
        /// </summary>
        /// <param name="list"></param>
        /// <param name="datosPSSE"></param>
        /// <param name="fechaProceso"></param>
        public List<RegistroGenerado> VerificarHorasOperacion(List<NombreCodigoBarra> list, List<string[]> datosPSSE, DateTime fechaProceso,
            out List<RegistroGenerado> resultadoOperacionEMS,
            out bool potenciaNegativa)
        {
            bool flagPotenciaNegativa = false;
            List<RegistroGenerado> resultado = new List<RegistroGenerado>();
            List<RegistroGenerado> resultadoEMS = new List<RegistroGenerado>();
            //- Listamos las relaciones
            List<EqRelacionDTO> relacion = FactorySic.GetEqRelacionRepository().ObtenerConfiguracionProceso(ConstantesCortoPlazo.FuenteGeneracion).Where(
                x => x.IndTipo == ConstantesCortoPlazo.TipoTermica).ToList();

            //- Lista los modos de operación activos en la hora
            List<int> modosOperacion = FactorySic.GetEqRelacionRepository().ObtenerModosOperacion(fechaProceso);

            //- Cambio agregado para nueva alarma 04.03.2020
            List<int> modosOperacionEspeciales = FactorySic.GetEqRelacionRepository().ObtenerModosOperacionEspeciales();

            NombreCodigoBarra adicional = null;

            //- Recorrido de cada unidad de generacion
            foreach (EqRelacionDTO entity in relacion)
            {
                string tension = string.Empty;
                entity.Codbarra = UtilCortoPlazo.ObtenerCodigoBarra(entity.Nombarra, list, out tension, out adicional);
                entity.IndicadorEliminar = false;
                if (string.IsNullOrEmpty(entity.Codbarra))
                {
                    entity.IndicadorEliminar = true;
                }
                string[] datos = UtilCortoPlazo.ObtenerValorPSSE(entity.Codbarra, entity.Idgener, datosPSSE);

                if (datos.Length == 2)
                {
                    if (datos[0] != null && datos[1] != null)
                    {
                        entity.PotGenerada = double.Parse(datos[0]);
                        entity.IndOperacion = datos[1];
                    }
                }

                //if (entity.Equicodi == 22 || entity.Equicodi == 23 || entity.Equicodi == 24 || entity.Equicodi == 25)
                //{
                if (entity.IndOperacion == 1.ToString())
                {
                    if (entity.PotGenerada < -5.0)
                    {
                        flagPotenciaNegativa = true;
                    }
                }
                //}

                if (entity.Indcoes == ConstantesAppServicio.SI)
                {
                    int idModoOperacion = 0;

                    //- Buscamos el modo de operación en las horas de operación
                    if (!string.IsNullOrEmpty(entity.Modosoperacion))
                    {
                        List<int> modos = entity.Modosoperacion.Split(ConstantesAppServicio.CaracterComa).Select(int.Parse).ToList();
                        foreach (int modo in modos)
                        {
                            if (modosOperacion.Where(x => x == modo).Count() > 0)
                            {
                                idModoOperacion = modo;
                                break;
                            }
                        }
                    }

                    entity.IdModoOperacion = idModoOperacion;
                }
                else
                {
                    entity.IndEspecial = true;
                }
            }

            //- Verificando los modos de operación de los ciclos combinados
            List<RegistroGenerado> listCicloCombinado = new List<RegistroGenerado>();

            foreach (EqRelacionDTO item in relacion)
            {
                if (item.IndOperacion != null)
                {
                    if (!item.IndEspecial)
                    {
                        RegistroGenerado entity = new RegistroGenerado();

                        if (item.Indcc == 0)
                        {
                            //- Verificamos las unidades que no tienen modos de operación
                            if (item.IndOperacion == 1.ToString() && item.IdModoOperacion == 0)
                            {
                                resultado.Add(new RegistroGenerado
                                {
                                    BarraNombre = item.Nombarra,
                                    GenerID = item.Idgener,
                                    Tension = item.Tension,
                                    Potencia = (decimal)item.PotGenerada,
                                    IndOpe = int.Parse(item.IndOperacion)
                                });
                            }

                            if (item.IdModoOperacion > 0)
                            {
                                if (modosOperacionEspeciales.Contains(item.IdModoOperacion))
                                {
                                    if (item.IndOperacion == 0.ToString())
                                    {
                                        //- Verificamos que la unidad este en el desploge
                                        List<int> unidadesEnOperacion = FactorySic.GetEqRelacionRepository().ObtenerUnidadesEnOperacion(fechaProceso, item.IdModoOperacion);

                                        if (unidadesEnOperacion.Contains((int)item.Equicodi))
                                        {
                                            //- Efectivamente la unidad se encuentra en horas de operacion pero aparece no operativa en el EMS
                                            resultadoEMS.Add(new RegistroGenerado
                                            {
                                                BarraNombre = item.Nombarra,
                                                GenerID = item.Idgener,
                                                Tension = item.Tension,
                                                Potencia = (decimal)item.PotGenerada,
                                                IndOpe = int.Parse(item.IndOperacion)
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    //- Verificamos las unidades que tienen modo de operacion pero aparecen no operativos en el EMS
                                    if (item.IndOperacion == 0.ToString() && item.IdModoOperacion > 0)
                                    {
                                        resultadoEMS.Add(new RegistroGenerado
                                        {
                                            BarraNombre = item.Nombarra,
                                            GenerID = item.Idgener,
                                            Tension = item.Tension,
                                            Potencia = (decimal)item.PotGenerada,
                                            IndOpe = int.Parse(item.IndOperacion)
                                        });
                                    }
                                }
                            }
                        }
                        else
                        {
                            entity.Relacion = item;
                            entity.Ccombcodi = item.Ccombcodi;
                            entity.IdModoOperacion = item.IdModoOperacion;
                            entity.IndOpe = int.Parse(item.IndOperacion);
                            listCicloCombinado.Add(entity);
                        }
                    }
                }
            }

            //- Obtenemos los grupos que pertenecen a un ciclo combinado
            List<int> idCcombinados = listCicloCombinado.Where(x => x.Ccombcodi != null).Select(x => (int)x.Ccombcodi).Distinct().ToList();

            foreach (int idCiclo in idCcombinados)
            {
                List<RegistroGenerado> listCC = listCicloCombinado.Where(x => x.Ccombcodi == idCiclo).ToList();
                RegistroGenerado tvcc = listCC.Where(x => x.Relacion.Indtvcc == ConstantesAppServicio.SI).FirstOrDefault();
                List<RegistroGenerado> tgcc = listCC.Where(x => x.Relacion.Indtvcc != ConstantesAppServicio.SI).ToList();

                bool flagTV = false;
                if (tvcc != null)
                {
                    if (tvcc.IndOpe == 1) flagTV = true;
                }

                foreach (RegistroGenerado itemTG in tgcc)
                {
                    if (!(itemTG.IndOpe == 1 && flagTV))
                    {
                        //- Verificamos las unidades que no tienen modos de operación
                        if (itemTG.IndOpe == 1 && itemTG.IdModoOperacion == 0)
                        {
                            resultado.Add(new RegistroGenerado
                            {
                                BarraNombre = itemTG.Relacion.Nombarra,
                                GenerID = itemTG.Relacion.Idgener,
                                Tension = itemTG.Relacion.Tension,
                                Potencia = (decimal)itemTG.Relacion.PotGenerada,
                                IndOpe = int.Parse(itemTG.Relacion.IndOperacion)
                            });
                        }
                    }

                    if (itemTG.IdModoOperacion > 0)
                    {
                        if (modosOperacionEspeciales.Contains(itemTG.IdModoOperacion))
                        {
                            if (itemTG.IndOpe == 0)
                            {
                                //- Verificamos que la unidad este en el desploge
                                List<int> unidadesEnOperacion = FactorySic.GetEqRelacionRepository().ObtenerUnidadesEnOperacion(fechaProceso, itemTG.IdModoOperacion);

                                if (unidadesEnOperacion.Contains((int)itemTG.Relacion.Equicodi))
                                {
                                    //- Efectivamente la unidad se encuentra en horas de operacion pero aparece no operativa en el EMS
                                    resultadoEMS.Add(new RegistroGenerado
                                    {
                                        BarraNombre = itemTG.Relacion.Nombarra,
                                        GenerID = itemTG.Relacion.Idgener,
                                        Tension = itemTG.Relacion.Tension,
                                        Potencia = (decimal)itemTG.Relacion.PotGenerada,
                                        IndOpe = int.Parse(itemTG.Relacion.IndOperacion)
                                    });
                                }
                            }
                        }
                        else
                        {
                            //- Verificamos las unidades que tienen modo de operacion pero aparecen no operativos en el EMS
                            if (itemTG.IndOpe == 0 && itemTG.IdModoOperacion > 0)
                            {
                                resultadoEMS.Add(new RegistroGenerado
                                {
                                    BarraNombre = itemTG.Relacion.Nombarra,
                                    GenerID = itemTG.Relacion.Idgener,
                                    Tension = itemTG.Relacion.Tension,
                                    Potencia = (decimal)itemTG.Relacion.PotGenerada,
                                    IndOpe = int.Parse(itemTG.Relacion.IndOperacion)
                                });
                            }
                        }
                    }
                }

                //- Verificamos que la TV tenga modo de operación
                if (tvcc != null)
                {
                    if (tvcc.IndOpe == 1 && tvcc.IdModoOperacion == 0)
                    {
                        resultado.Add(new RegistroGenerado
                        {
                            BarraNombre = tvcc.Relacion.Nombarra,
                            GenerID = tvcc.Relacion.Idgener,
                            Tension = tvcc.Relacion.Tension,
                            Potencia = (decimal)tvcc.Relacion.PotGenerada,
                            IndOpe = int.Parse(tvcc.Relacion.IndOperacion)
                        });
                    }

                    if (tvcc.IdModoOperacion > 0)
                    {
                        if (modosOperacionEspeciales.Contains(tvcc.IdModoOperacion))
                        {
                            if (tvcc.IndOpe == 0)
                            {
                                //- Verificamos que la unidad este en el desploge
                                List<int> unidadesEnOperacion = FactorySic.GetEqRelacionRepository().ObtenerUnidadesEnOperacion(fechaProceso, tvcc.Relacion.IdModoOperacion);

                                if (unidadesEnOperacion.Contains((int)tvcc.Relacion.Equicodi))
                                {
                                    //- Efectivamente la unidad se encuentra en horas de operacion pero aparece no operativa en el EMS
                                    resultadoEMS.Add(new RegistroGenerado
                                    {
                                        BarraNombre = tvcc.Relacion.Nombarra,
                                        GenerID = tvcc.Relacion.Idgener,
                                        Tension = tvcc.Relacion.Tension,
                                        Potencia = (decimal)tvcc.Relacion.PotGenerada,
                                        IndOpe = int.Parse(tvcc.Relacion.IndOperacion)
                                    });
                                }
                            }
                        }
                        else
                        {
                            //- Verificamos las unidades que tienen modo de operacion pero aparecen no operativos en el EMS
                            if (tvcc.IndOpe == 0 && tvcc.IdModoOperacion > 0)
                            {
                                resultadoEMS.Add(new RegistroGenerado
                                {
                                    BarraNombre = tvcc.Relacion.Nombarra,
                                    GenerID = tvcc.Relacion.Idgener,
                                    Tension = tvcc.Relacion.Tension,
                                    Potencia = (decimal)tvcc.Relacion.PotGenerada,
                                    IndOpe = int.Parse(tvcc.Relacion.IndOperacion)
                                });
                            }
                        }
                    }
                }
            }

            resultadoOperacionEMS = resultadoEMS;
            potenciaNegativa = flagPotenciaNegativa;
            return resultado;
        }

        public List<RegistroGenerado> ObtenerDatosGeneracion(List<NombreCodigoBarra> list, List<string[]> datosPSSE)
        {
            List<RegistroGenerado> resultado = new List<RegistroGenerado>();

            //- Listamos las relaciones
            List<EqRelacionDTO> relacion = FactorySic.GetEqRelacionRepository().ObtenerConfiguracionProceso(ConstantesCortoPlazo.FuenteGeneracion).Where(
                x => x.IndTipo == ConstantesCortoPlazo.TipoTermica).ToList();

            NombreCodigoBarra adicional = null;
            //- Recorrido de cada unidad de generacion
            foreach (EqRelacionDTO entity in relacion)
            {
                string tension = string.Empty;
                entity.Codbarra = UtilCortoPlazo.ObtenerCodigoBarra(entity.Nombarra, list, out tension, out adicional);
                entity.IndicadorEliminar = false;
                if (string.IsNullOrEmpty(entity.Codbarra))
                {
                    entity.IndicadorEliminar = true;
                }
                string[] datos = UtilCortoPlazo.ObtenerValorPSSE(entity.Codbarra, entity.Idgener, datosPSSE);

                if (datos.Length == 2)
                {
                    if (datos[0] != null && datos[1] != null)
                    {
                        entity.PotGenerada = double.Parse(datos[0]);
                        entity.IndOperacion = datos[1];

                        RegistroGenerado generado = new RegistroGenerado();
                        generado.BarraNombre = entity.Nombarra;
                        generado.GenerID = entity.Idgener;
                        generado.Potencia = (decimal)entity.PotGenerada;
                        resultado.Add(generado);
                    }
                }
            }


            return resultado;
        }

        /// <summary>
        /// Permite obtener la relacion de generadores y obtencion de datos adicionales
        /// </summary>
        /// <returns></returns>
        public List<EqRelacionDTO> ObtenerRelacion(List<NombreCodigoBarra> list, List<string[]> datosPSSE, DateTime fechaDatos, ref string log,
            List<string[]> equivalenciaCentral, bool reproceso, out bool flagFenixValidacion, out List<EqRelacionDTO> listaRestricciones,
            bool flagMD, List<CpRecursoDTO> listaBarraCentral, out List<EqRelacionDTO> equiposSinBarra)
        {

            DateTime fechaProceso = (fechaDatos.Hour == 0 && fechaDatos.Minute == 0) ? fechaDatos.AddMinutes(-1) : fechaDatos;

            //- Listamos las relaciones
            List<EqRelacionDTO> relacion = FactorySic.GetEqRelacionRepository().ObtenerConfiguracionProceso(ConstantesCortoPlazo.FuenteGeneracion);

            //- Lista los modos de operación activos en la hora
            List<int> modosOperacion = FactorySic.GetEqRelacionRepository().ObtenerModosOperacion(fechaProceso);

            //- Obteniendo las calificaciones del módulo de horas de operacion
            List<EqRelacionDTO> calificacion = FactorySic.GetEqRelacionRepository().ObtenerCalificacionUnidades(fechaProceso);

            //- Obteniendo las restricciones operativas del dia
            List<EqRelacionDTO> restricciones = FactorySic.GetEqRelacionRepository().ObtenerRestriccionOperativa(fechaProceso);
            listaRestricciones = restricciones;

            //- Obteniendo las propiedades necesarias de las unidades hidraulicas
            List<EqRelacionDTO> propHidraulicas = FactorySic.GetEqRelacionRepository().ObtenerPropiedadHidraulicos();

            //- Obteniendo las propiedades necesarias de las unidades hidraulicas
            List<EqRelacionDTO> propHidraulicasCentral = FactorySic.GetEqRelacionRepository().ObtenerPropiedadHidraulicosCentral();

            //- Listado de formulas generales
            List<PrGrupodatDTO> formulasGeneral = FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(fechaProceso);

            //- Listado de unidades de Reserva Secundaria
            List<EveRsfdetalleDTO> listadoRsf = FactorySic.GetEveRsfdetalleRepository().ObtenerUnidadesRSF(fechaProceso);

            n_parameter parameterGeneral = new n_parameter();
            foreach (PrGrupodatDTO itemConcepto in formulasGeneral)
            {
                if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                    parameterGeneral.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
            }

            #region Ticket 2022-004345
            List<CmGeneradorBarraemsDTO> listBarrasEMSUnidadNoTNA = FactorySic.GetCmGeneradorBarraemsRepository().List();
            List<CmGeneradorPotenciagenDTO> listPotenciasUnidadNoTNA = FactorySic.GetCmGeneradorPotenciagenRepository().List();
            #endregion


            double tipoCambio = parameterGeneral.GetEvaluate(ConstantesCortoPlazo.PropTipoCambio);

            //-- Inicio de duraceli- sev

            #region Codigo caso especial

            #region Ticket 2022-004345

            EqRelacionDTO genFenixG11 = relacion.Where(x => x.Equicodi == 13604).FirstOrDefault();
            EqRelacionDTO genFenixG12 = relacion.Where(x => x.Equicodi == 13603).FirstOrDefault();
            EqRelacionDTO genFenixTV = relacion.Where(x => x.Equicodi == 13602).FirstOrDefault();

            bool flagFenix = false;
            flagFenixValidacion = false;
            #endregion

            if (genFenixG11 != null && genFenixG12 != null && genFenixTV != null)
            {                
                string tension1 = string.Empty;
                //NombreCodigoBarra adicional = null;
                //genFenixG11.Codbarra = UtilCortoPlazoTna.ObtenerCodigoBarra(genFenixG11.Nombarra, list, out tension1, out adicional);
                //genFenixG12.Codbarra = UtilCortoPlazoTna.ObtenerCodigoBarra(genFenixG12.Nombarra, list, out tension1, out adicional);
                //genFenixTV.Codbarra = UtilCortoPlazoTna.ObtenerCodigoBarra(genFenixTV.Nombarra, list, out tension1, out adicional);

                string[] datosFenixG11 = UtilCortoPlazoTna.ObtenerValorPSSE(genFenixG11.Nombretna, datosPSSE);
                string[] datosFenixG12 = UtilCortoPlazoTna.ObtenerValorPSSE(genFenixG12.Nombretna, datosPSSE);
                string[] datosFenixTV = UtilCortoPlazoTna.ObtenerValorPSSE(genFenixTV.Nombretna, datosPSSE);

                if (datosFenixG11.Length == 3)
                {
                    if (datosFenixG11[0] != null && datosFenixG11[1] != null)
                    {
                        genFenixG11.PotGenerada = double.Parse(datosFenixG11[0]);
                        genFenixG11.IndOperacion = datosFenixG11[1];
                        genFenixG11.Codbarra = datosFenixG11[2];
                    }
                }

                if (datosFenixG12.Length == 3)
                {
                    if (datosFenixG12[0] != null && datosFenixG12[1] != null)
                    {
                        genFenixG12.PotGenerada = double.Parse(datosFenixG12[0]);
                        genFenixG12.IndOperacion = datosFenixG12[1];
                        genFenixG12.Codbarra = datosFenixG12[2];
                    }
                }

                if (datosFenixTV.Length == 3)
                {
                    if (datosFenixTV[0] != null && datosFenixTV[1] != null)
                    {
                        genFenixTV.PotGenerada = double.Parse(datosFenixTV[0]);
                        genFenixTV.IndOperacion = datosFenixTV[1];
                        genFenixTV.Codbarra = datosFenixTV[2];
                    }
                }

                int cicloFenixGt11Gas = 348;
                int cicloFenixGt12D2 = 555;
                int cicloFenixGt11D2 = 553;
                int cicloFenixGt12Gas = 345;

                if (genFenixG11.IndOperacion == 1.ToString() && genFenixG12.IndOperacion == 1.ToString() && genFenixTV.IndOperacion == 1.ToString())
                {
                    if (modosOperacion.Contains(cicloFenixGt11Gas) && modosOperacion.Contains(cicloFenixGt12D2))
                    {
                        genFenixG11.IdModoOperacion = 0;
                        genFenixG11.Ccombcodi = 100;
                        genFenixG11.IndOperacion = 1.ToString();
                        genFenixTV.IdModoOperacion = cicloFenixGt11Gas;
                        genFenixTV.Ccombcodi = 100;
                        genFenixTV.IndOperacion = 1.ToString(); //quitar mas adelante
                        genFenixG11.PotGenerada = genFenixG11.PotGenerada + genFenixTV.PotGenerada - 0.51 * genFenixG12.PotGenerada;
                        genFenixG11.PotenciaMaxima = 0;
                        genFenixTV.PotGenerada = 0;
                        genFenixTV.PotenciaMaxima = 0;
                        genFenixG12.IdModoOperacion = 0;
                        genFenixG12.IndOperacion = 1.ToString();
                        genFenixG12.Ccombcodi = 101;

                        EqRelacionDTO genFenixTV1 = new EqRelacionDTO
                        {
                            Relacioncodi = genFenixTV.Relacioncodi,
                            Equicodi = genFenixTV.Equicodi,
                            Codincp = genFenixTV.Codincp,
                            Nombrencp = genFenixTV.Nombrencp,
                            Codbarra = genFenixTV.Codbarra,
                            Idgener = genFenixTV.Idgener,
                            Descripcion = genFenixTV.Descripcion,
                            Nombarra = genFenixTV.Nombarra,
                            Estado = genFenixTV.Estado,
                            Indfuente = genFenixTV.Indfuente,
                            IndTipo = genFenixTV.IndTipo,
                            Indcc = genFenixTV.Indcc,
                            Modosoperacion = genFenixTV.Modosoperacion,
                            Grupocodi = genFenixTV.Grupocodi,
                            Grupopadre = genFenixTV.Grupopadre,
                            Indtvcc = genFenixTV.Indtvcc,
                            Ccombcodi = genFenixTV.Ccombcodi,
                            Equipadre = genFenixTV.Equipadre,
                            Indcoes = genFenixTV.Indcoes,
                            IndOperacion = genFenixTV.IndOperacion
                        };

                        genFenixTV1.Equicodi = -100;
                        genFenixTV1.IdModoOperacion = cicloFenixGt12D2;
                        genFenixTV1.Ccombcodi = 101;
                        genFenixTV1.PotGenerada = 0;
                        genFenixG12.PotGenerada = 1.51 * genFenixG12.PotGenerada;
                        genFenixG12.PotenciaMaxima = 0;
                        genFenixTV1.PotenciaMaxima = 0;

                        relacion.Add(genFenixTV1);
                        flagFenix = true;
                    }
                    else if (modosOperacion.Contains(cicloFenixGt11D2) && modosOperacion.Contains(cicloFenixGt12Gas))
                    {
                        double potenciaTV = genFenixTV.PotGenerada;
                        double potenciaG11 = genFenixG11.PotGenerada;
                        genFenixG11.IdModoOperacion = 0;
                        genFenixG11.Ccombcodi = 100;
                        genFenixG11.IndOperacion = 1.ToString();
                        genFenixTV.IdModoOperacion = cicloFenixGt11D2;
                        genFenixTV.Ccombcodi = 100;
                        genFenixTV.IndOperacion = 1.ToString(); //quitar mas adelante
                        genFenixG11.PotGenerada = 1.51 * genFenixG11.PotGenerada;
                        genFenixG11.PotenciaMaxima = 0;
                        genFenixTV.PotGenerada = 0;
                        genFenixTV.PotenciaMaxima = 0;
                        genFenixG12.IdModoOperacion = 0;
                        genFenixG12.IndOperacion = 1.ToString();
                        genFenixG12.Ccombcodi = 101;
                        EqRelacionDTO genFenixTV1 = new EqRelacionDTO
                        {
                            Relacioncodi = genFenixTV.Relacioncodi,
                            Equicodi = genFenixTV.Equicodi,
                            Codincp = genFenixTV.Codincp,
                            Nombrencp = genFenixTV.Nombrencp,
                            Codbarra = genFenixTV.Codbarra,
                            Idgener = genFenixTV.Idgener,
                            Descripcion = genFenixTV.Descripcion,
                            Nombarra = genFenixTV.Nombarra,
                            Estado = genFenixTV.Estado,
                            Indfuente = genFenixTV.Indfuente,
                            IndTipo = genFenixTV.IndTipo,
                            Indcc = genFenixTV.Indcc,
                            Modosoperacion = genFenixTV.Modosoperacion,
                            Grupocodi = genFenixTV.Grupocodi,
                            Grupopadre = genFenixTV.Grupopadre,
                            Indtvcc = genFenixTV.Indtvcc,
                            Ccombcodi = genFenixTV.Ccombcodi,
                            Equipadre = genFenixTV.Equipadre,
                            Indcoes = genFenixTV.Indcoes,
                            IndOperacion = genFenixTV.IndOperacion

                        };

                        genFenixTV1.Equicodi = -100;
                        genFenixTV1.IdModoOperacion = cicloFenixGt12Gas;
                        genFenixTV1.Ccombcodi = 101;
                        genFenixTV1.PotGenerada = 0;
                        genFenixG12.PotGenerada = genFenixG12.PotGenerada + potenciaTV - 0.51 * potenciaG11;
                        genFenixG12.PotenciaMaxima = 0;
                        genFenixTV1.PotenciaMaxima = 0;

                        relacion.Add(genFenixTV1);
                        flagFenix = true;
                    }
                }

                flagFenixValidacion = flagFenix;

            }

            //- Fin de duraceli             
            #endregion 

            int contador = 0;

            try
            {

                //- Recorrido de cada unidad de generacion
                foreach (EqRelacionDTO entity in relacion)
                {
                    contador++;

                    if (true)
                    {
                        //- INICIO: Hacer jugada temporal para trabajar con codigos nuevos

                        //- Falta validar la fecha

                        if (reproceso)
                        {
                            DateTime fechaValidacion = new DateTime(2018, 1, 13);

                            if (DateTime.Now.Subtract(fechaValidacion).TotalMinutes > 10) //es a partir de las 00:30                 
                            {
                                if (fechaProceso.Subtract(fechaValidacion).TotalMinutes <= 0) //fecha de reproceso
                                {
                                    #region Cambio de códigos

                                    if (entity.Equicodi == 22)
                                    {
                                        entity.Codincp = 9001;
                                        entity.Nombrencp = "Huinco1";
                                    }
                                    if (entity.Equicodi == 23)
                                    {
                                        entity.Codincp = 9002;
                                        entity.Nombrencp = "Huinco2";
                                    }
                                    if (entity.Equicodi == 24)
                                    {
                                        entity.Codincp = 9003;
                                        entity.Nombrencp = "Huinco3";
                                    }
                                    if (entity.Equicodi == 25)
                                    {
                                        entity.Codincp = 9004;
                                        entity.Nombrencp = "Huinco4";
                                    }
                                    if (entity.Equicodi == 61)
                                    {
                                        entity.Codincp = 9123;
                                        entity.Nombrencp = "Carhuaquero";
                                    }
                                    if (entity.Equicodi == 62)
                                    {
                                        entity.Codincp = 9123;
                                        entity.Nombrencp = "Carhuaquero";
                                    }
                                    if (entity.Equicodi == 63)
                                    {
                                        entity.Codincp = 9123;
                                        entity.Nombrencp = "Carhuaquero";
                                    }
                                    if (entity.Equicodi == 204)
                                    {
                                        entity.Codincp = 9099;
                                        entity.Nombrencp = "Carhuaquero4";
                                    }
                                    if (entity.Equicodi == 205)
                                    {
                                        entity.Codincp = 9099;
                                        entity.Nombrencp = "Carhuaquero4";
                                    }

                                    #endregion
                                }
                            }
                        }

                        //- FIN: ajuste                                  

                        //inicio de duro
                        bool flagPotencia = true;
                        if (flagFenix)
                        {
                            if (entity.Equicodi == 13602 || entity.Equicodi == 13603 || entity.Equicodi == 13604 || entity.Equicodi == -100)
                            {
                                flagPotencia = false;
                            }
                        }

                        //fin de duro
                        entity.Codbarra = string.Empty;
                        entity.Nombarra = string.Empty;
                        if (!string.IsNullOrEmpty(entity.Nombretna))
                        {
                            #region Ticket 2022-004345
                            //if (entity.Equicodi != 11897) //- Lineda agregada para oquendo
                            if (entity.Indnomodeladatna != ConstantesAppServicio.SI)
                            #endregion
                            {//- Lineda agregada para oquendo
                                string[] datos = UtilCortoPlazoTna.ObtenerValorPSSE(entity.Nombretna, datosPSSE);

                                if (datos.Length == 3)
                                {
                                    if (datos[0] != null && datos[1] != null)
                                    {
                                        if (flagPotencia) //-cambio realizado
                                        {
                                            entity.PotGenerada = double.Parse(datos[0]);
                                            entity.IndOperacion = datos[1];
                                            entity.Codbarra = datos[2];
                                        }
                                    }
                                }
                            }//- Lineda agregada para oquendo
                            else//- Lineda agregada para oquendo
                            {
                                entity.PotGenerada = 0;
                                entity.IndOperacion = 1.ToString();

                                #region Ticket 2022-004345

                                List<CmGeneradorBarraemsDTO> subListBarraEMS = listBarrasEMSUnidadNoTNA.Where(x => x.Relacioncodi == entity.Relacioncodi).ToList();
                                bool flagBarra = false;
                                string codigoBarra = string.Empty;
                                foreach(CmGeneradorBarraemsDTO itemBarraEMS in subListBarraEMS)
                                {
                                    NombreCodigoBarra itemCodigoBarra = list.Where(x => x.NombBarra == itemBarraEMS.Cnfbarnombre).FirstOrDefault();

                                    if(itemCodigoBarra != null)
                                    {
                                        codigoBarra = itemCodigoBarra.CodBarra;
                                        flagBarra = true;
                                        break;
                                    }
                                }
                                if(flagBarra)
                                {
                                    entity.Codbarra = codigoBarra;
                                }
                                else 
                                {
                                    entity.IndBarraNoEncontrada = ConstantesAppServicio.SI;
                                }

                                //entity.Codbarra = list.Where(x => x.NombBarra == "CHAVARR60").Select(x => x.CodBarra).First();

                                #endregion

                            }//- Lineda agregada para oquendo
                        }

                        string tension = string.Empty;
                        entity.Nombarra = UtilCortoPlazoTna.ObtenerCodigoBarra(entity.Codbarra, list, out tension);

                        entity.IndicadorEliminar = false;
                        if (string.IsNullOrEmpty(entity.Codbarra))
                        {
                            entity.IndicadorEliminar = true;
                        }
                        entity.Tension = tension;

                        //- Obtenemos las propiedades adicionales   

                        if (entity.IndTipo == ConstantesCortoPlazo.TipoTermica)
                        {
                            entity.IdCalificacionCompensacion = -1;
                            if (entity.Indcoes == ConstantesAppServicio.SI)
                            {
                                #region Obtencion del modo de operacion

                                int idModoOperacion = 0;

                                //- Buscamos el modo de operación en las horas de operación
                                if (!string.IsNullOrEmpty(entity.Modosoperacion))
                                {
                                    List<int> modos = entity.Modosoperacion.Split(ConstantesAppServicio.CaracterComa).Select(int.Parse).ToList();
                                    foreach (int modo in modos)
                                    {
                                        if (modosOperacion.Where(x => x == modo).Count() > 0)
                                        {
                                            idModoOperacion = modo;
                                            entity.IndModoOperacion = ConstantesCortoPlazo.ModoOperacionHO;
                                            break;
                                        }
                                    }
                                }

                                //- Inicio duracel
                                if (flagFenix)
                                {
                                    if (entity.Equicodi == 13602 || entity.Equicodi == 13603 || entity.Equicodi == 13604 || entity.Equicodi == -100)
                                    {
                                        idModoOperacion = entity.IdModoOperacion;
                                    }
                                }
                                //- fin duracel

                                ////- En caso no exista sacamos el modo de operación de las propiedades (No deberia emplearse)
                                //if (idModoOperacion == 0)
                                //{
                                //    idModoOperacion = FactorySic.GetEqRelacionRepository().ObtenerModoOperacionUnidad(entity.Grupocodi);
                                //    entity.IndModoOperacion = ConstantesCortoPlazo.ModoOperacionDefecto;
                                //}

                                #endregion

                                if (idModoOperacion == 0)
                                {
                                    entity.IndModoOperacion = ConstantesCortoPlazo.ModoOperacionNoExiste;
                                    entity.IdCalificacionCompensacion = -2;
                                }
                                else
                                {
                                    #region Obteniendo las propiedades de las térmicas

                                    string idsGrupos = idModoOperacion + ConstantesAppServicio.CaracterComa.ToString() + entity.Grupocodi +
                                                       ConstantesAppServicio.CaracterComa.ToString() + entity.Grupopadre;
                                    List<PrGrupodatDTO> listFormulas = new List<PrGrupodatDTO>();
                                    listFormulas.AddRange(formulasGeneral);
                                    listFormulas.AddRange(FactorySic.GetPrGrupodatRepository().ObtenerParametroModoOperacion(idsGrupos, fechaProceso));

                                    //- Declaramos el objeto parameter para calcular los valores
                                    n_parameter parameter = new n_parameter();
                                    foreach (PrGrupodatDTO itemConcepto in listFormulas)
                                    {
                                        if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                                            parameter.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
                                    }

                                    //- Con esto sacamos las propiedades para el modo de operación como son curva de consumo, costo combustible, CV OYM
                                    double costoCombustible = parameter.GetEvaluate(ConstantesCortoPlazo.PropCostoCombustible);
                                    double costoVariableOM = parameter.GetEvaluate(ConstantesCortoPlazo.PropCostoVariable);
                                    double potMaxima = parameter.GetEvaluate(ConstantesCortoPlazo.PropPotenciaMaxima); //- Pmax
                                    double potMinima = parameter.GetEvaluate(ConstantesCortoPlazo.PropPotenciaMinima);  //- Pmin
                                    double potEfectiva = parameter.GetEvaluate(ConstantesCortoPlazo.PropPotenciaEfectiva); //- Pe
                                    double factorConversion = parameter.GetEvaluate(ConstantesCortoPlazo.PropFactorConversion); //PCI_SI

                                    List<CoordenadaConsumo> curva = new List<CoordenadaConsumo>();
                                    string tipoCurva = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropIndicadorCurva).FirstOrDefault().Formuladat; //- CMGNTC
                                    string indCentral = (listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropIndCentral).Count() > 0) ?
                                        listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropIndCentral).FirstOrDefault().Formuladat : string.Empty; //;
                                    string formulacostoCombustible = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropCostoCombustible).FirstOrDefault().Formuladat;

                                    double factor = 1;
                                    if (!string.IsNullOrEmpty(formulacostoCombustible))
                                        if (formulacostoCombustible.ToUpper() == ConstantesCortoPlazo.UnidadCostoCombustible.ToUpper())
                                            factor = factorConversion / 1000000;

                                    int nroUnidades = 1;
                                    if (indCentral == ConstantesAppServicio.SI)
                                    {
                                        nroUnidades = FactorySic.GetEqRelacionRepository().ObtenerNroUnidades(entity.Grupocodi);
                                        if (nroUnidades == 0) nroUnidades = 1;
                                    }

                                    if (tipoCurva == 1.ToString())
                                    {
                                        //- Punto donde puede fallar
                                        string curvaAjustada = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropCurvaAjustadaSPR).FirstOrDefault().Formuladat; //- CoordConsumComb

                                        string[] strPuntos = curvaAjustada.Split(ConstantesAppServicio.CaracterNumeral);

                                        foreach (string strPunto in strPuntos)
                                        {
                                            string[] strCorrdenada = strPunto.Split(ConstantesAppServicio.CaracterPorcentaje);

                                            if (strCorrdenada.Length == 2)
                                            {
                                                decimal x = 0;
                                                decimal y = 0;

                                                if (decimal.TryParse(strCorrdenada[0], out x) && decimal.TryParse(strCorrdenada[1], out y))
                                                {
                                                    curva.Add(new CoordenadaConsumo
                                                    {
                                                        Potencia = x,
                                                        Consumo = y
                                                    });
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int[] categoriasCurva = { 14, 175, 176, 177, 178, 179, 180, 181, 182, 183 };
                                        List<PrGrupodatDTO> puntos = listFormulas.Where(x => categoriasCurva.Any(y => x.Concepcodi == y)).
                                            Select(x => new PrGrupodatDTO { Concepcodi = x.Concepcodi, Formuladat = x.Formuladat }).ToList();

                                        curva = UtilCortoPlazo.ObtenerCurvaConsumo(puntos);
                                    }

                                    //- Obteniendo velocidad de toma y reduccion de carga
                                    //- double velocidadDescarga = parameter.GetEvaluate(ConstantesCortoPlazo.PropVelocidadDescarga);
                                    //- double velocidadCarga = parameter.GetEvaluate(ConstantesCortoPlazo.PropVelocidadCarga);
                                    string velocidadToma = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropVelocidadCarga).FirstOrDefault().Formuladat;
                                    string velocidadReduccion = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropVelocidadDescarga).FirstOrDefault().Formuladat;
                                    bool existeVelocidaCarga = false;
                                    bool existeVelocidaDescarga = false;
                                    double velocidadCarga = this.ObtenerPropiedadVelocidad(velocidadToma, out existeVelocidaCarga);
                                    double velocidadDescarga = this.ObtenerPropiedadVelocidad(velocidadReduccion, out existeVelocidaDescarga);

                                    //- Obteniendo calificacion en horas de operacion
                                    string desCalificacion = string.Empty;
                                    int idCalificacion = -1;
                                    EqRelacionDTO itemCalificacion = calificacion.Where(x => x.Grupocodi == idModoOperacion).FirstOrDefault();
                                    if (itemCalificacion != null)
                                    {
                                        desCalificacion = itemCalificacion.Subcausacmg;
                                        idCalificacion = itemCalificacion.Subcausacodi;
                                        entity.IdCalificacionCompensacion = itemCalificacion.Subcausacodi;
                                    }

                                    //- Llenado propiedades

                                    entity.CostoCombustible = costoCombustible;
                                    entity.CostoVariableOYM = costoVariableOM;
                                    entity.PotenciaMaxima = potEfectiva / nroUnidades;
                                    //entity.PotenciaMaxima = (potMaxima > potEfectiva) ? potMaxima / nroUnidades : potEfectiva / nroUnidades;
                                    entity.PotenciaMinima = potMinima / nroUnidades;
                                    entity.VelocidadCarga = velocidadCarga;
                                    entity.VelocidadDescarga = velocidadDescarga;
                                    entity.Calificacion = desCalificacion;
                                    entity.IdCalificacion = idCalificacion;
                                    entity.ListaCurva = curva;
                                    entity.IdModoOperacion = idModoOperacion;
                                    entity.FactorConversion = factor;


                                    //- Cambio adicional por oquendo
                                    #region Ticket 2022-004345
                                    /*if (entity.Equicodi == 11897)
                                    {
                                        if (entity.IdModoOperacion == 352)
                                        {
                                            entity.PotGenerada = 22.0;
                                        }
                                        else if (entity.IdModoOperacion == 298)
                                        {
                                            entity.PotGenerada = 25.0;
                                        }
                                    }*/

                                    if (entity.Indnomodeladatna == ConstantesAppServicio.SI)
                                    {
                                        CmGeneradorPotenciagenDTO itemPotencia = listPotenciasUnidadNoTNA.Where(x => x.Grupocodi == entity.IdModoOperacion).FirstOrDefault();

                                        if(itemPotencia != null)
                                        {
                                            entity.PotGenerada = (double)itemPotencia.Genpotvalor;
                                        }
                                    }

                                    #endregion

                                    //- Region completado por RSF

                                    #endregion
                                }
                            }
                            else
                            {
                                entity.IndEspecial = true;
                            }
                        }
                        else if (entity.IndTipo == ConstantesCortoPlazo.TipoHidraulica)
                        {
                            if (entity.Indcoes == ConstantesAppServicio.SI)
                            {
                                #region Obteniendo las propiedades de las hidráulicas

                                //- Jalando los valores de las propiedades
                                EqRelacionDTO propPotMax = propHidraulicas.Where(x => x.Equicodi == entity.Equicodi && x.Propcodi == ConstantesCortoPlazo.IdPropPotMaxH).FirstOrDefault();
                                EqRelacionDTO propPotMin = propHidraulicas.Where(x => x.Equicodi == entity.Equicodi && x.Propcodi == ConstantesCortoPlazo.IdPropPotMinH).FirstOrDefault();
                                EqRelacionDTO propVelCarga = propHidraulicas.Where(x => x.Equicodi == entity.Equicodi && x.Propcodi == ConstantesCortoPlazo.IdPropVelCargaH).FirstOrDefault();
                                EqRelacionDTO propVelDescarga = propHidraulicas.Where(x => x.Equicodi == entity.Equicodi && x.Propcodi == ConstantesCortoPlazo.IdPropVelDescargaH).FirstOrDefault();
                                EqRelacionDTO propPotEfectiva = propHidraulicas.Where(x => x.Equicodi == entity.Equicodi && x.Propcodi == ConstantesCortoPlazo.IdPropPotEfeH).FirstOrDefault();
                                //- EqRelacionDTO propVelDescarga = propHidraulicas.Where(x => x.Equicodi == entity.Equicodi && x.Propcodi == ConstantesCortoPlazo.IdPropVelDescargaH).FirstOrDefault();

                                //- obtener capacidad de regulacion con equipadre
                                EqRelacionDTO propCapacRegulacion = propHidraulicasCentral.Where(x => x.Equicodi == entity.Equipadre && x.Propcodi == ConstantesCortoPlazo.IdPropCapacRegulacion).FirstOrDefault();

                                #region Ticket 19342 - Movisoft
                                //#region Corrección Temporal Ticket 19342
                                //if (entity.Equicodi == 61 || entity.Equicodi == 62 || entity.Equicodi == 63)
                                //{
                                //    entity.Equipadre = entity.Equicodi;
                                //}
                                //#endregion
                                #endregion

                                //- Definiendo las variables y indicadores de existencia de las propiedades evaluadas
                                bool flagPotMaxH = false;
                                bool flagPotMinH = false;
                                bool flagPotEfectiva = false;
                                bool flagVelCargaH = false;
                                bool flagVelDescargaH = false;

                                double potMaxH = this.ObtenerPropiedadHidraulico(propPotMax, out flagPotMaxH);
                                double potMinH = this.ObtenerPropiedadHidraulico(propPotMin, out flagPotMinH);
                                double potEfectivaH = this.ObtenerPropiedadHidraulico(propPotEfectiva, out flagPotEfectiva);
                                double velCargaH = this.ObtenerPropiedadHidraulico(propVelCarga, out flagVelCargaH);
                                double velDescargaH = this.ObtenerPropiedadHidraulico(propVelDescarga, out flagVelDescargaH);

                                //- Llenando a la estructura los valores e indicadores cargados         
                                entity.PotenciaMaxima = potEfectivaH;
                                //entity.PotenciaMaxima = (potMaxH > potEfectivaH) ? potMaxH : potEfectivaH;
                                entity.PotenciaMinima = potMinH;
                                entity.VelocidadCarga = velCargaH;
                                entity.VelocidadDescarga = velDescargaH;
                                entity.FactorConversion = tipoCambio;

                                //- pasar valor de capacidad de regulacion en base el equipadre 
                                if (propCapacRegulacion != null)
                                    entity.CapacidadRegulacion = propCapacRegulacion.Propiedad;

                                //- Verificamos su correspondiente codigo de barra

                                if (flagMD)
                                {
                                    #region Ticket 19342 - Movisoft

                                    //if (entity.Equipadre != null)
                                    //    entity.Recurcodibarra = listaBarraCentral.Find(x => x.Recurcodisicoescentral == entity.Equipadre)?.Recurcodibarra;

                                    if (entity.Equipadre != null)
                                    {
                                        if (listaBarraCentral.Where(x => x.Recurcodisicoescentral == entity.Equipadre).Count() > 0)
                                        {
                                            entity.Recurcodibarra = listaBarraCentral.Find(x => x.Recurcodisicoescentral == entity.Equipadre)?.Recurcodibarra;
                                        }
                                    }

                                    if (entity.Recurcodibarra == null || entity.Recurcodibarra == 0)
                                    {
                                        if (listaBarraCentral.Where(x => x.Recurcodisicoescentral == entity.Equicodi).Count() > 0)
                                        {
                                            entity.Recurcodibarra = listaBarraCentral.Find(x => x.Recurcodisicoescentral == entity.Equicodi)?.Recurcodibarra;
                                        }
                                    }

                                    #endregion
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(entity.Nombrencp))
                                        entity.Codbarrancp = FileHelper.ObtenerEquivalenciaCentral(entity.Nombrencp, equivalenciaCentral);
                                    else
                                        entity.Codbarrancp = string.Empty;
                                }

                                #endregion
                            }
                            else
                            {
                                entity.IndEspecial = true;
                            }
                        }
                        else if (entity.IndTipo == ConstantesCortoPlazo.TipoSolar || entity.IndTipo == ConstantesCortoPlazo.TipoEolico)
                        {
                            entity.IndTipo = ConstantesCortoPlazo.TipoTermica;
                            entity.IndEspecial = true;
                        }

                        //- Verificando las restricciones operativas para las unidades: Pot Fija, Pot Máxima y Pot Mínima

                        List<EqRelacionDTO> listRestriccion = restricciones.Where(x => x.Equicodi == entity.Equicodi).ToList();
                        List<RestriccionUnidad> ListaRestricciones = new List<RestriccionUnidad>();

                        foreach (EqRelacionDTO itemRestriccion in listRestriccion)
                        {
                            if (itemRestriccion.Subcausacodi == COES.Servicios.Aplicacion.Eventos.Helper.ConstantesOperacionesVarias.SubcausacodiPotenciaFija)
                            {
                                ListaRestricciones.Add(new RestriccionUnidad
                                {
                                    Tipo = ConstantesCortoPlazo.RestriccionPotFija,
                                    Valor = itemRestriccion.Valor
                                });
                            }
                            else if (itemRestriccion.Subcausacodi == COES.Servicios.Aplicacion.Eventos.Helper.ConstantesOperacionesVarias.SubcausacodiPotenciaMax)
                            {
                                ListaRestricciones.Add(new RestriccionUnidad
                                {
                                    Tipo = ConstantesCortoPlazo.RestriccionPotMaxima,
                                    Valor = itemRestriccion.Valor
                                });
                            }
                            else if (itemRestriccion.Subcausacodi == COES.Servicios.Aplicacion.Eventos.Helper.ConstantesOperacionesVarias.SubcausacodiPotenciaMin)
                            {
                                ListaRestricciones.Add(new RestriccionUnidad
                                {
                                    Tipo = ConstantesCortoPlazo.RestriccionPotMinima,
                                    Valor = itemRestriccion.Valor
                                });
                            }
                            else if (itemRestriccion.Subcausacodi == COES.Servicios.Aplicacion.Eventos.Helper.ConstantesOperacionesVarias.SubcausacodiPlenacarga)
                            {
                                ListaRestricciones.Add(new RestriccionUnidad
                                {
                                    Tipo = ConstantesCortoPlazo.RestriccionPlenaCarga,
                                    Valor = itemRestriccion.Valor
                                });
                            }
                        }

                        entity.ListaRestriccion = ListaRestricciones;

                        //- Verificamos si tiene restriccion RSF
                        EveRsfdetalleDTO rsfDetalle = listadoRsf.Where(x => x.Equicodi == entity.Equicodi).FirstOrDefault();

                        if (rsfDetalle != null)
                        {
                            entity.ExistenciaRsf = true;
                            entity.ValorRsf = (decimal)(rsfDetalle.Rsfdetvalaut.HasValue ? rsfDetalle.Rsfdetvalaut : 0);
                            entity.ValorRsfUp = (decimal)(rsfDetalle.RSFDETSUB.HasValue ? rsfDetalle.RSFDETSUB : 0);
                            entity.ValorRsfDown = (decimal)(rsfDetalle.RSFDETBAJ.HasValue ? rsfDetalle.RSFDETBAJ : 0);
                            entity.PadreRsf = rsfDetalle.Equipadre;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string s = contador.ToString();
            }

            //- Calculamos las valores RSF para cada equipo
            List<int> equispadre = relacion.Where(x => x.ExistenciaRsf).Select(x => x.PadreRsf).Distinct().ToList();

            foreach (int equipadre in equispadre)
            {
                List<EqRelacionDTO> elementos = relacion.Where(x => x.ExistenciaRsf && x.PadreRsf == equipadre && x.IndOperacion == 1.ToString()).ToList();

                foreach (EqRelacionDTO itemRSF in elementos)
                {
                    itemRSF.CantidadRsf = elementos.Count();
                }
            }

            //escribir log
            log += "*** LISTADO PROPIEDADES" + ConstantesCortoPlazo.SaltoLinea;
            log += "Cod. Barra" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Nom. Barra" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "ID" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Tension" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "PotGenerada" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "IndOperacion" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Tipo" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Modo Operacion" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Costo Combustible" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Costo Variable OYM" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Potencia Maxima" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Potencia Minima" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Velocidad Carga" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Velocidad Descarga" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Calificacion" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "ModoOperacion" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Capacidad Regulacion" + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "Lista Curva" + ConstantesCortoPlazo.CaracterSeparacionCSV;
            log += ConstantesCortoPlazo.SaltoLinea;

            foreach (EqRelacionDTO entity in relacion)
            {
                log += (entity.Codbarra == null ? "" : entity.Codbarra) + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.Nombarra != null ? entity.Nombarra : "") + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.Idgener != null ? entity.Idgener : "") + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.Tension != null ? entity.Tension : "") + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.PotGenerada) + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.IndOperacion != null ? entity.IndOperacion : "") + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.IndTipo != null ? entity.IndTipo : "") + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.IndModoOperacion != null ? entity.IndModoOperacion : "") + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.CostoCombustible) + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.CostoVariableOYM) + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.PotenciaMaxima) + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.PotenciaMinima) + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.VelocidadCarga) + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.VelocidadDescarga) + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    (entity.Calificacion != null ? entity.Calificacion : "") + ConstantesCortoPlazo.CaracterSeparacionCSV +
                    entity.IdModoOperacion + ConstantesCortoPlazo.CaracterSeparacionCSV + (entity.CapacidadRegulacion != null ? entity.CapacidadRegulacion : "") + ConstantesCortoPlazo.CaracterSeparacionCSV;

                if (entity.ListaCurva != null)
                {
                    int iCurva = 0;
                    foreach (CoordenadaConsumo curva in entity.ListaCurva)
                    {
                        if (iCurva < entity.ListaCurva.Count - 1)
                            log += "[" + curva.Potencia + ":" + curva.Consumo + "]" + ":";
                        else
                            log += "[" + curva.Potencia + ":" + curva.Consumo + "]";

                        iCurva++;
                    }
                }

                log += ConstantesCortoPlazo.SaltoLinea;
            }

            log += ConstantesCortoPlazo.SaltoLinea;
            log += ConstantesCortoPlazo.SaltoLinea;

            #region Ticket 2022-004345
            equiposSinBarra = relacion.Where(x => x.IndBarraNoEncontrada == ConstantesAppServicio.SI).ToList();
            #endregion

            return relacion.Where(x => x.IndicadorEliminar == false).ToList();
        }

        /// <summary>
        /// Permite obtener la relacion de generadores y obtencion de datos adicionales
        /// </summary>
        /// <returns></returns>
        public List<EqRelacionDTO> ObtenerConfiguracionGeneracionEMS(List<NombreCodigoBarra> list, List<string[]> datosPSSE)
        {
            List<EqRelacionDTO> relacion = FactorySic.GetEqRelacionRepository().ObtenerConfiguracionReservaRotante();

            //- Recorrido de cada unidad de generacion
            foreach (EqRelacionDTO entity in relacion)
            {
                string tension = string.Empty;
                NombreCodigoBarra adicional = null;
                entity.Codbarra = UtilCortoPlazo.ObtenerCodigoBarra(entity.Nombarra, list, out tension, out adicional);

                string[] datos = UtilCortoPlazo.ObtenerValorPSSE(entity.Codbarra, entity.Idgener, datosPSSE);

                if (datos.Length == 2)
                {
                    if (datos[0] != null && datos[1] != null)
                    {

                        entity.PotGenerada = double.Parse(datos[0]);
                        entity.IndOperacion = datos[1];

                    }
                }
            }

            return relacion;
        }

        /// <summary>
        /// Permite obtener la relacion de generadores y obtencion de datos adicionales
        /// </summary>
        /// <returns></returns>
        public List<EqRelacionDTO> ObtenerRelacionEquivalencia(DateTime fechaProceso)
        {
            //- Listamos las relaciones
            List<EqRelacionDTO> relacion = FactorySic.GetEqRelacionRepository().ObtenerConfiguracionProceso(ConstantesCortoPlazo.FuenteGeneracion);

            //- Recorrido de cada unidad de generacion
            foreach (EqRelacionDTO entity in relacion)
            {
                //- INICIO: Hacer jugada temporal para trabajar con codigos nuevos

                //- Falta validar la fecha

                DateTime fechaValidacion = new DateTime(2018, 1, 13);

                if (DateTime.Now.Subtract(fechaValidacion).TotalMinutes > 10) //es a partir de las 00:30                 
                {
                    if (fechaProceso.Subtract(fechaValidacion).TotalMinutes <= 0) //fecha de reproceso
                    {
                        #region Cambio de códigos

                        if (entity.Equicodi == 22)
                        {
                            entity.Codincp = 9001;
                            entity.Nombrencp = "Huinco1";
                        }
                        if (entity.Equicodi == 23)
                        {
                            entity.Codincp = 9002;
                            entity.Nombrencp = "Huinco2";
                        }
                        if (entity.Equicodi == 24)
                        {
                            entity.Codincp = 9003;
                            entity.Nombrencp = "Huinco3";
                        }
                        if (entity.Equicodi == 25)
                        {
                            entity.Codincp = 9004;
                            entity.Nombrencp = "Huinco4";
                        }
                        if (entity.Equicodi == 61)
                        {
                            entity.Codincp = 9123;
                            entity.Nombrencp = "Carhuaquero";
                        }
                        if (entity.Equicodi == 62)
                        {
                            entity.Codincp = 9123;
                            entity.Nombrencp = "Carhuaquero";
                        }
                        if (entity.Equicodi == 63)
                        {
                            entity.Codincp = 9123;
                            entity.Nombrencp = "Carhuaquero";
                        }
                        if (entity.Equicodi == 204)
                        {
                            entity.Codincp = 9099;
                            entity.Nombrencp = "Carhuaquero4";
                        }
                        if (entity.Equicodi == 205)
                        {
                            entity.Codincp = 9099;
                            entity.Nombrencp = "Carhuaquero4";
                        }

                        #endregion
                    }
                }
            }

            return relacion.Where(x => x.IndTipo == ConstantesCortoPlazo.TipoTermica).ToList();
        }

        /// <summary>
        /// Permite obtener el valor de una propiedad
        /// </summary>
        /// <returns></returns>
        public double ObtenerPropiedadVelocidad(string valor, out bool existePropiedad)
        {
            existePropiedad = false;
            double velocidad = 0;

            if (!string.IsNullOrEmpty(valor))
            {
                //- Verificamos que no existan los caracteres / o -

                if (valor.Contains('/'))
                {
                    string[] component = valor.Split('/');

                    if (component.Length > 0)
                    {
                        existePropiedad = true;

                        if (!double.TryParse(component[component.Length - 1], out velocidad))
                        {
                            existePropiedad = false;
                        }
                    }
                    else
                    {
                        existePropiedad = false;
                    }
                }
                else if (valor.Contains('-'))
                {
                    string[] component = valor.Split('-');

                    if (component.Length > 0)
                    {
                        existePropiedad = true;

                        if (!double.TryParse(component[component.Length - 1], out velocidad))
                        {
                            existePropiedad = false;
                        }
                    }
                    else
                    {
                        existePropiedad = false;
                    }
                }
                else
                {
                    existePropiedad = true;
                    if (!double.TryParse(valor, out velocidad))
                    {
                        existePropiedad = false;
                    }
                }
            }

            return velocidad;
        }

        /// <summary>
        /// Permite evaluar la formula de las unidades hidraulicas
        /// </summary>
        /// <param name="item"></param>
        /// <param name="existePropiedad"></param>
        /// <returns></returns>
        private double ObtenerPropiedadHidraulico(EqRelacionDTO item, out bool existePropiedad)
        {
            existePropiedad = false;
            double propiedad = 0;

            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.Propiedad))
                {
                    if (double.TryParse(item.Propiedad, out propiedad))
                    {
                        existePropiedad = true;
                    }
                }
            }

            return propiedad;
        }

        /// <summary>
        /// Evaluacion de condiciones para determinar si participan en los CM
        /// </summary>
        /// <param name="datosPotHidraulico"></param>
        /// <param name="datosVolumenHidraulico"></param>
        /// <param name="list"></param>
        /// <param name="periodo"></param>
        /// <param name="log"></param>
        public void EvaluacionUnidades(DateTime fechaProceso, List<string[]> datosPotHidraulico, List<string[]> datosVolumenHidraulico, ref List<EqRelacionDTO> list,
            int periodo, ref string log, List<CpRecursoDTO> lstRecursosVolMaxMin, List<CpMedicion48DTO> lstVolumenesEmbalses, bool flagMD)
        {
            decimal porcentajeVolumen = (new ParametroAppServicio()).ObtenerValorParametro(ConstantesCortoPlazo.IdParametroVolumen, fechaProceso);

            if (porcentajeVolumen == 0)
                porcentajeVolumen = 0.05M;
            else
                porcentajeVolumen = porcentajeVolumen / 100;

            log += "*** EVALUACION DE UNIDADES" + ConstantesCortoPlazo.SaltoLinea;
            log += "Cod. Barra" + ConstantesCortoPlazo.CaracterSeparacionCSV + "Nomb. Barra" + ConstantesCortoPlazo.CaracterSeparacionCSV + "Condicion" +
                ConstantesCortoPlazo.CaracterSeparacionCSV + "Forzada" + ConstantesCortoPlazo.SaltoLinea;

            //- Valores por defecto para unidades hidroeléctricas
            //foreach (EqRelacionDTO item in list)
            //{
            //    if (item.IndTipo == ConstantesCortoPlazo.TipoHidraulica)
            //    {
            //        item.Procesado = false;
            //        item.CumplePotMinimaUnidades = false;
            //    }

            //    item.Forzada = true;
            //    item.CentralEvaluada = false;
            //}

            foreach (EqRelacionDTO item in list)
            {
                //- Condicion 1: Sincronizadas al SEIN y haber alcanzado su pmin
                //if (item.PotGenerada >= item.PotenciaMinima)
                //{
                //    //- Cumple condición 1?
                //    if (item.IndTipo == ConstantesCortoPlazo.TipoHidraulica)
                //    {
                //        if (!item.Procesado)
                //        {
                //            this.EvaluarCondicion1Hidro((int)item.Equicodi, (int)item.Equipadre, ref list);
                //        }

                //        if (item.CumplePotMinimaUnidades)
                //        {
                //            //cumple condición 1
                //            item.Forzada = false;
                //            log += item.Codbarra + ConstantesCortoPlazo.CaracterSeparacionCSV + item.Nombarra + ConstantesCortoPlazo.CaracterSeparacionCSV + "1" +
                //                ConstantesCortoPlazo.CaracterSeparacionCSV + item.Forzada + ConstantesCortoPlazo.SaltoLinea;
                //        }
                //    }

                //    if (item.IndTipo == ConstantesCortoPlazo.TipoTermica)
                //    {
                //        //cumple condición 1
                //        if (item.PotGenerada >= item.PotenciaMinima)
                //        {
                //            item.Forzada = false;
                //            log += item.Codbarra + ConstantesCortoPlazo.CaracterSeparacionCSV + item.Nombarra + ConstantesCortoPlazo.CaracterSeparacionCSV + "1" +
                //                ConstantesCortoPlazo.CaracterSeparacionCSV + item.Forzada + ConstantesCortoPlazo.SaltoLinea;
                //        }
                //    }
                //}

                //- Condicion 2: Aquellas unidades que estén calificadas como PE, Minima carga CAR o RSF
                //if (item.IndTipo == ConstantesCortoPlazo.TipoTermica)
                //{
                //    if (item.Calificacion == "PE" || item.Calificacion == "MIN" || item.Calificacion == "RSF")
                //    {
                //        //- Cumple condicion 2
                //        item.Forzada = false;
                //        log += item.Codbarra + ConstantesCortoPlazo.CaracterSeparacionCSV + item.Nombarra + ConstantesCortoPlazo.CaracterSeparacionCSV + "2" +
                //            ConstantesCortoPlazo.CaracterSeparacionCSV + item.Forzada + ConstantesCortoPlazo.SaltoLinea;
                //    }
                //}

                //- Condicion 3: Que no tengan reestriciones operativas
                foreach (var restriccion in item.ListaRestriccion)
                {
                    if (restriccion.Tipo == ConstantesCortoPlazo.RestriccionPotFija)
                    {
                        //SE EXCLUYE: es forzada
                        item.Calificacion = "RES"; //restriccion potencia fija
                        item.Forzada = true;
                    }
                    if (restriccion.Tipo == ConstantesCortoPlazo.RestriccionPotMaxima)
                    {
                        double valor = Convert.ToDouble(restriccion.Valor);
                        if (valor < item.PotenciaMaxima)
                        {
                            item.PotenciaMaxima = valor;
                            //item.Forzada = false;
                            log += item.Codbarra + ConstantesCortoPlazo.CaracterSeparacionCSV + item.Nombarra + ConstantesCortoPlazo.CaracterSeparacionCSV + "3" +
                                ConstantesCortoPlazo.CaracterSeparacionCSV + item.Forzada + ConstantesCortoPlazo.SaltoLinea;
                        }
                    }

                    if (restriccion.Tipo == ConstantesCortoPlazo.RestriccionPotMinima)
                    {
                        double valor = Convert.ToDouble(restriccion.Valor);
                        if (valor > item.PotenciaMinima)
                        {
                            item.PotenciaMinima = valor;
                            //item.Forzada = false;
                            log += item.Codbarra + ConstantesCortoPlazo.CaracterSeparacionCSV + item.Nombarra + ConstantesCortoPlazo.CaracterSeparacionCSV + "3" +
                                ConstantesCortoPlazo.CaracterSeparacionCSV + item.Forzada + ConstantesCortoPlazo.SaltoLinea;
                        }
                    }
                    if (restriccion.Tipo == ConstantesCortoPlazo.RestriccionPlenaCarga)
                    {
                        //- Cuando exista restriccion a plena carga la potencia maxima = sera igual a potencia scada
                        item.PotenciaMaxima = item.PotGenerada;
                    }
                }

                //- Verificamos las forzadas
                if (item.IndTipo == ConstantesCortoPlazo.TipoTermica)
                {
                    if (!string.IsNullOrEmpty(item.Calificacion))
                    {
                        if (!(item.IdCalificacion == 101 || item.IdCalificacion == 102))//continuar
                        {
                            item.Forzada = true;
                        }
                    }
                }

                //- Condicion 4: CH con capacidad de RD verificar volúmenes
                if (item.IndTipo == ConstantesCortoPlazo.TipoHidraulica)
                {
                    if (item.CapacidadRegulacion == ConstantesCortoPlazo.CapacidadRegulacionDiaria)
                    {
                        if (flagMD)
                        {
                            var lstPropiedadEmbalse = lstRecursosVolMaxMin.Where(x => x.Equipadre == item.Equipadre).ToList();

                            #region Ticket 19342 - Movisoft

                            if (lstPropiedadEmbalse.Count == 0)
                            {
                                lstPropiedadEmbalse = lstRecursosVolMaxMin.Where(x => x.Equipadre == item.Equicodi).ToList();
                            }

                            #endregion


                            if (lstPropiedadEmbalse.Any())
                            {
                                string VolMinTexto = lstPropiedadEmbalse.Find(x => x.Propcodi == ConstantesCortoPlazo.PropcodiVolumenMinimo)?.Valor.Trim();
                                string VolMaxTexto = lstPropiedadEmbalse.Find(x => x.Propcodi == ConstantesCortoPlazo.PropcodiVolumenMaximo)?.Valor.Trim();

                                double VolMin = string.IsNullOrEmpty(VolMinTexto) ? 0 : Convert.ToDouble(VolMinTexto);
                                double VolMax = string.IsNullOrEmpty(VolMinTexto) ? 0 : Convert.ToDouble(VolMaxTexto);

                                double volumenProgramado = (new McpAppServicio()).ObtenerValorHxPorRecurso(lstVolumenesEmbalses, periodo, lstPropiedadEmbalse.First().Recurcodi) ?? 0;

                                log = EvaluarCondicionVolumenes(log, porcentajeVolumen, item, volumenProgramado, VolMin, VolMax);
                            }
                            else
                            {
                                log += "NO EXISTE VOLUMNES VMAX y VMIN: " + item.Nombarra + ConstantesCortoPlazo.CaracterSeparacionCSV + item.Idgener +
                                    ConstantesCortoPlazo.CaracterSeparacionCSV + item.Equipadre + ConstantesCortoPlazo.SaltoLinea;
                            }
                        }
                        else
                        {
                            double volumen = (double)UtilCortoPlazo.ObtenerVolumenProgramado(item.Nombrencp.Trim(), periodo, datosVolumenHidraulico, fechaProceso);
                            int filaVolumen = -1;

                            int idx = -1;
                            foreach (string[] itemPhidro in datosPotHidraulico)
                            {
                                idx++;
                                if (itemPhidro[1].Trim() == item.Nombrencp.Trim())
                                {
                                    filaVolumen = idx;
                                    break;
                                }
                            }

                            if (filaVolumen >= 0)
                            {
                                string[] valoresMinMax = datosPotHidraulico[filaVolumen];
                                string VolMinTexto = valoresMinMax[2];
                                string VolMaxTexto = valoresMinMax[3];
                                double VolMin = (VolMinTexto == null || VolMinTexto.Trim() == "") ? 0 : Convert.ToDouble(VolMinTexto);
                                double VolMax = (VolMaxTexto == null || VolMaxTexto.Trim() == "") ? 0 : Convert.ToDouble(VolMaxTexto);

                                log = EvaluarCondicionVolumenes(log, porcentajeVolumen, item, volumen, VolMin, VolMax);

                                //double volumenA = VolMin + (VolMax - VolMin) * (double)porcentajeVolumen;
                                //double volumenB = VolMax - (VolMax - VolMin) * (double)porcentajeVolumen;

                                //if (!(volumenA <= volumen && volumen <= volumenB))
                                //{
                                //    item.Forzada = true;
                                //    item.Calificacion = "VOL"; //- No cumple la condicion de volúmenes
                                //    log += item.Codbarra + ConstantesCortoPlazo.CaracterSeparacionCSV + item.Nombarra + ConstantesCortoPlazo.CaracterSeparacionCSV + "4" +
                                //        ConstantesCortoPlazo.CaracterSeparacionCSV + item.Forzada + ConstantesCortoPlazo.SaltoLinea;
                                //}

                                //double volumenB = VolMax * 0.95;
                                //double volumenA = VolMin * 1.05;

                                //if (!(volumen >= volumenA && volumen <= volumenB))
                                //{
                                //    item.Forzada = true;
                                //    item.Calificacion = "VOL"; //- No cumple la condicion de volúmenes
                                //    log += item.Codbarra + ConstantesCortoPlazo.CaracterSeparacionCSV + item.Nombarra + ConstantesCortoPlazo.CaracterSeparacionCSV + "4" +
                                //        ConstantesCortoPlazo.CaracterSeparacionCSV + item.Forzada + ConstantesCortoPlazo.SaltoLinea;
                                //}
                            }
                        }
                    }

                    if (item.CapacidadRegulacion == null || item.CapacidadRegulacion.Trim().Length < 3 || item.CapacidadRegulacion == ConstantesCortoPlazo.NoTieneCapacidadRegulacion)
                    {
                        item.Forzada = true;
                        item.Calificacion = "CAL"; //- No tiene capacidad de regulación
                    }
                }
            }

            log += ConstantesCortoPlazo.SaltoLinea;
            log += ConstantesCortoPlazo.SaltoLinea;

        }

        /// <summary>
        /// Permite evaluar la condicion 4 volumenes
        /// </summary>
        /// <param name="log"></param>
        /// <param name="porcentajeVolumen"></param>
        /// <param name="item"></param>
        /// <param name="volumen"></param>
        /// <param name="VolMin"></param>
        /// <param name="VolMax"></param>
        /// <returns></returns>
        private string EvaluarCondicionVolumenes(string log, decimal porcentajeVolumen, EqRelacionDTO item, double volumen, double VolMin, double VolMax)
        {
            double volumenA = VolMin + (VolMax - VolMin) * (double)porcentajeVolumen;
            double volumenB = VolMax - (VolMax - VolMin) * (double)porcentajeVolumen;

            if (!(volumenA <= volumen && volumen <= volumenB))
            {
                item.Forzada = true;
                item.Calificacion = "VOL"; //- No cumple la condicion de volúmenes
                log += item.Nombarra + ConstantesCortoPlazo.CaracterSeparacionCSV + item.Idgener + ConstantesCortoPlazo.CaracterSeparacionCSV + "FORZADA"
                    + ConstantesCortoPlazo.SaltoLinea;
            }

            log += "DATOS VOLUMENES HIDRO: " + item.Nombarra + ConstantesCortoPlazo.CaracterSeparacionCSV + item.Idgener + ConstantesCortoPlazo.CaracterSeparacionCSV +
                "VOL PROGR :" + volumen + ", VOLMIN: " + VolMin + ", VOLMAX: " + VolMax + ConstantesCortoPlazo.SaltoLinea;

            return log;
        }

        /// <summary>
        /// Permite evaluar la condicion 1 para todas las unidades que conforman la central
        /// </summary>
        /// <param name="list"></param>
        public void EvaluarCondicion1Hidro(int equicodi, int equipadre, ref List<EqRelacionDTO> list)
        {
            bool CumplePotminimo = true;

            foreach (EqRelacionDTO item in list)
            {
                if (item.IndTipo == ConstantesCortoPlazo.TipoHidraulica)
                {
                    if (item.Equipadre == equipadre)
                    {
                        CumplePotminimo = CumplePotminimo && item.PotGenerada >= item.PotenciaMinima;

                        if (!CumplePotminimo) break;
                    }
                }
            }

            //pasando el tipo procesado y cumplimiento de unidades
            foreach (EqRelacionDTO item in list)
            {
                if (item.IndTipo == ConstantesCortoPlazo.TipoHidraulica)
                {
                    if (item.Equipadre == equipadre)
                    {
                        item.Procesado = true;
                        item.CumplePotMinimaUnidades = CumplePotminimo;
                    }
                }
            }
        }

        /// <summary>
        /// Permite actualizar si la central ha sido evaluada
        /// </summary>        
        /// <param name="equipadre"></param>
        /// <param name="list"></param>
        public void EvaluarCondicion4Hidro(int equipadre, ref List<EqRelacionDTO> list)
        {
            //pasando el tipo procesado y cumplimiento de unidades
            foreach (EqRelacionDTO item in list)
            {
                if (item.IndTipo == ConstantesCortoPlazo.TipoHidraulica)
                {
                    if (item.Equipadre == equipadre)
                    {
                        item.CentralEvaluada = true;
                    }
                }
            }
        }

        /// <summary>
        /// Permite obtener los flujos de potencia
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fecha"></param>
        /// <param name="relacionBarra"></param>
        /// <param name="relacionLinea"></param>
        /// <param name="relacionTrafo"></param>
        /// <returns></returns>
        public List<CmFlujoPotenciaDTO> ObtenerFlujosPotencia(List<EqCongestionConfigDTO> list, DateTime fecha, int correlativo, List<NombreCodigoBarra> relacionBarra,
            List<NombreCodigoLinea> relacionLinea, List<TrafoEms> relacionTrafo)
        {
            try
            {
                List<CmFlujoPotenciaDTO> entitys = new List<CmFlujoPotenciaDTO>();

                foreach (EqCongestionConfigDTO entity in list)
                {
                    CmFlujoPotenciaDTO item = new CmFlujoPotenciaDTO();

                    item.Equicodi = entity.Equicodi;
                    item.Emprcodi = entity.Emprcodi;
                    item.Configcodi = entity.Configcodi;
                    item.Flupotfecha = fecha;
                    item.Cmgncorrelativo = correlativo;
                    item.Flupotvalor = 0;

                    double flujo = 0;
                    double flujo1 = 0;
                    double flujo2 = 0;
                    if (entity.Famcodi == ConstantesCortoPlazo.IdLineaTransmision)
                    {
                        LineaEms lineaEms = ObtenerLinea(out flujo, entity.Nombretna1, relacionBarra, entity.Idems, relacionLinea, out flujo1, out flujo2);

                        if (lineaEms != null)
                        {
                            item.Flupotvalor = (decimal)flujo;
                            item.Flupotvalor1 = (decimal)flujo1;
                            item.Flupotvalor2 = (decimal)flujo2;
                        }

                    }
                    else if (entity.Famcodi == ConstantesCortoPlazo.IdTrafo2D)
                    {
                        NombreCodigoBarra barra1 = relacionBarra.Where(x => x.NombBarra == entity.Nodobarra1 && entity.Nodobarra1 != null).FirstOrDefault();
                        NombreCodigoBarra barra2 = relacionBarra.Where(x => x.NombBarra == entity.Nodobarra2 && entity.Nodobarra2 != null).FirstOrDefault();

                        if (barra1 != null && barra2 != null)
                        {
                            TrafoEms trafo = relacionTrafo.Where(x =>
                                x.IdBarra1 == barra1.CodBarra &&
                                x.IdBarra2 == barra2.CodBarra &&
                                x.Orden == entity.Idems).FirstOrDefault();

                            if (trafo != null)
                            {
                                item.Flupotvalor = (decimal)Math.Max(Math.Max(trafo.Pp, trafo.Ps), trafo.Pt);
                                item.Flupotvalor1 = (decimal)trafo.Pp;
                                item.Flupotvalor2 = (decimal)trafo.Ps;
                            }
                        }
                    }

                    if (entity.Famcodi == ConstantesCortoPlazo.IdLineaTransmision || entity.Famcodi == ConstantesCortoPlazo.IdTrafo2D)
                    {
                        entitys.Add(item);
                    }
                }

                return entitys;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los datos de congestión
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="relacionBarra"></param>
        /// <returns></returns>
        private List<PrCongestionDTO> ObtenerCongestion(DateTime fechaDatos, List<NombreCodigoBarra> relacionBarra, List<NombreCodigoLinea> relacionLinea,
            List<TrafoEms> relacionTrafo, ref string log, out List<PrCongestionDTO> listCongestionConjunto)
        {
            DateTime fecha = (fechaDatos.Hour == 0 && fechaDatos.Minute == 0) ? fechaDatos.AddMinutes(-1) : fechaDatos;
            log += "*** OBTENER CONGESTION" + ConstantesCortoPlazo.SaltoLinea;

            List<PrCongestionDTO> resultadoTrafo = new List<PrCongestionDTO>();
            List<PrCongestionDTO> list = FactorySic.GetPrCongestionRepository().ObtenerCongestionRegistro(fecha);
            List<PrCongestionDTO> result = new List<PrCongestionDTO>();

            foreach (PrCongestionDTO entity in list)
            {
                if (entity.Nodobarra1.Contains("Ñ")) entity.Nodobarra1 = entity.Nodobarra1.Replace("Ñ", "�");
                if (entity.Nodobarra2.Contains("Ñ")) entity.Nodobarra2 = entity.Nodobarra2.Replace("Ñ", "�");


                double flujo = 0;
                double flujo1 = 0;
                double flujo2 = 0;
                if (entity.Famcodi == ConstantesCortoPlazo.IdLineaTransmision)
                {
                    LineaEms lineaEms = ObtenerLinea(out flujo, entity.Nombretna1, relacionBarra, entity.NombLinea, relacionLinea, out flujo1, out flujo2);

                    log += "Nodo1:" + (entity.Nodobarra1 == null ? "" : entity.Nodobarra1) +
                        " Nodo2:" + (entity.Nodobarra2 == null ? "" : entity.Nodobarra2) +
                        " Línea:" + (entity.NombLinea == null ? "" : entity.NombLinea);

                    if (lineaEms != null)
                    {
                        entity.Flujo = flujo;
                        entity.Nodobarra1 = relacionBarra.Where(x => x.CodBarra == lineaEms.IdBarra1).FirstOrDefault().NombBarra;
                        entity.Nodobarra2 = relacionBarra.Where(x => x.CodBarra == lineaEms.IdBarra2).FirstOrDefault().NombBarra;
                        entity.Nombretna = entity.Nombretna1;
                        result.Add(entity);
                    }

                    log += " Flujo:" + (lineaEms != null ? entity.Flujo.ToString() : "NO ENCONTRADA");
                    log += ConstantesCortoPlazo.SaltoLinea;

                }
                else if (entity.Famcodi == ConstantesCortoPlazo.IdTrafo2D)
                {
                    TrafoEms trafo = relacionTrafo.Where(x => x.NombreTna == entity.Nombretna1).FirstOrDefault();

                    if (trafo != null)
                    {
                        entity.Flujo = Math.Max(Math.Max(!double.IsNaN(trafo.Pp) ? trafo.Pp : 0, !double.IsNaN(trafo.Ps) ? trafo.Ps : 0), !double.IsNaN(trafo.Pt) ? trafo.Pt : 0);
                        entity.Nodobarra1 = relacionBarra.Where(x => x.CodBarra == trafo.IdBarra1).FirstOrDefault().NombBarra;
                        entity.Nodobarra2 = relacionBarra.Where(x => x.CodBarra == trafo.IdBarra2).FirstOrDefault().NombBarra;
                        entity.Nombretna = trafo.NombreTna;
                        result.Add(entity);
                        log += " Flujo Trafo:" + (trafo != null ? entity.Flujo.ToString() : "NO ENCONTRADA");
                        log += ConstantesCortoPlazo.SaltoLinea;
                    }
                }
                else if (entity.Famcodi == ConstantesCortoPlazo.IdTrafo3D)
                {

                    if (entity.Equicodi == 1)
                    {
                        DateTime fechaCompara = DateTime.ParseExact("31/08/2020 18:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                        if (fechaCompara.Subtract(fechaDatos).TotalMinutes >= 0)
                        {
                            entity.Nombretna1 = "Marcona 220 T95-H";
                            entity.Nombretna2 = "Marcona 60 T95-M";
                            entity.Nombretna3 = "Marcona 10 T95-L";
                        }
                    }



                    TrafoEms trafo1 = relacionTrafo.Where(x => x.NombreTna == entity.Nombretna1).FirstOrDefault();
                    TrafoEms trafo2 = relacionTrafo.Where(x => x.NombreTna == entity.Nombretna2).FirstOrDefault();
                    TrafoEms trafo3 = relacionTrafo.Where(x => x.NombreTna == entity.Nombretna3).FirstOrDefault();

                    double congestion1 = Math.Max(Math.Max(!double.IsNaN(trafo1.Pp) ? trafo1.Pp : 0, !double.IsNaN(trafo1.Ps) ? trafo1.Ps : 0), !double.IsNaN(trafo1.Pt) ? trafo1.Pt : 0);
                    double congestion2 = Math.Max(Math.Max(!double.IsNaN(trafo2.Pp) ? trafo2.Pp : 0, !double.IsNaN(trafo2.Ps) ? trafo2.Ps : 0), !double.IsNaN(trafo2.Pt) ? trafo2.Pt : 0);
                    double congestion3 = Math.Max(Math.Max(!double.IsNaN(trafo3.Pp) ? trafo3.Pp : 0, !double.IsNaN(trafo3.Ps) ? trafo3.Ps : 0), !double.IsNaN(trafo3.Pt) ? trafo3.Pt : 0);

                    if (congestion1 > congestion2 && congestion1 > congestion3)
                    {
                        entity.Flujo = congestion1;
                        entity.Nodobarra1 = relacionBarra.Where(x => x.CodBarra == trafo1.IdBarra1).FirstOrDefault().NombBarra;
                        entity.Nodobarra2 = relacionBarra.Where(x => x.CodBarra == trafo1.IdBarra2).FirstOrDefault().NombBarra;
                        entity.Nombretna = trafo1.NombreTna;
                    }
                    else if (congestion2 > congestion1 && congestion2 > congestion3)
                    {
                        entity.Flujo = congestion2;
                        entity.Nodobarra1 = relacionBarra.Where(x => x.CodBarra == trafo2.IdBarra1).FirstOrDefault().NombBarra;
                        entity.Nodobarra2 = relacionBarra.Where(x => x.CodBarra == trafo2.IdBarra2).FirstOrDefault().NombBarra;
                        entity.Nombretna = trafo2.NombreTna;
                    }
                    else if (congestion3 > congestion1 && congestion3 > congestion2)
                    {
                        entity.Flujo = congestion3;
                        entity.Nodobarra1 = relacionBarra.Where(x => x.CodBarra == trafo3.IdBarra1).FirstOrDefault().NombBarra;
                        entity.Nodobarra2 = relacionBarra.Where(x => x.CodBarra == trafo3.IdBarra2).FirstOrDefault().NombBarra;
                        entity.Nombretna = trafo3.NombreTna;
                    }

                    result.Add(entity);
                }

                entity.NombreResultado = entity.Nodobarra1 + "-" + entity.Nodobarra2 + " *" + entity.Nombretna;
            }

            //- Colocamos las congestiones en trafos 3D representados como conjunto de lineas
            listCongestionConjunto = resultadoTrafo;

            return result;
        }

        /// <summary>
        /// Obtiene los datos de la linea
        /// </summary>
        /// <param name="flujo"></param>
        /// <param name="nodobarra1"></param>
        /// <param name="nodobarra2"></param>
        /// <param name="relacionBarra"></param>
        /// <param name="nombreLinea"></param>
        /// <param name="relacionLinea"></param>
        /// <returns></returns>
        private LineaEms ObtenerLinea(out double flujo, string nombreTna, List<NombreCodigoBarra> relacionBarra, string nombreLinea,
            List<NombreCodigoLinea> relacionLinea, out double flujo1, out double flujo2)
        {
            LineaEms lineaEms;
            flujo = 0;
            flujo1 = 0;
            flujo2 = 0;

            NombreCodigoLinea linea = relacionLinea.Where(x => x.Nombretna == nombreTna).FirstOrDefault();

            if (linea != null)
            {
                lineaEms = new LineaEms(linea.CodBarra1, linea.CodBarra2, 0, 0, 1, linea.NombLinea, linea.Rps, linea.Xps, linea.Bsh, linea.GshP, linea.BshP,
                    linea.GshS, linea.BshS, linea.BitEstado, linea.VoltajePU1, linea.Angulo1, linea.VoltajePU2, linea.Angulo2, linea.Pot, 0, 0, linea.Nombretna);

                flujo = Math.Max(Math.Abs(lineaEms.PP), Math.Abs(lineaEms.Ps));
                flujo1 = lineaEms.PP;
                flujo2 = lineaEms.Ps;

                return lineaEms;
            }


            return null;
        }

        /// <summary>
        /// Permite obtener los datos de congestión conjunta
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="relacion"></param>
        /// <returns></returns>
        private List<PrCongestionDTO> ObtenerCongestionGrupo(DateTime fechaDatos, List<NombreCodigoBarra> relacionBarra,
            List<NombreCodigoLinea> relacionLinea, ref string log, List<PrCongestionDTO> congestionTrafo3D)
        {
            DateTime fecha = (fechaDatos.Hour == 0 && fechaDatos.Minute == 0) ? fechaDatos.AddMinutes(-1) : fechaDatos;
            //- Agregamos una lista con las congestiones conjuntas para los trafos 3D

            List<PrCongestionDTO> list = FactorySic.GetPrCongestionRepository().ObtenerCongestionConjuntoRegistro(fecha, "1");
            List<PrCongestionDTO> result = new List<PrCongestionDTO>();

            log += "*** OBTENER CONGESTION GRUPO" + ConstantesCortoPlazo.SaltoLinea;

            foreach (PrCongestionDTO entity in list)
            {
                log += "Nodo1:" + (entity.Nodobarra1 == null ? "" : entity.Nodobarra1) + " " +
                       "Nodo2:" + (entity.Nodobarra2 == null ? "" : entity.Nodobarra2) + " " +
                       "Línea:" + (entity.NombLinea == null ? "" : entity.NombLinea);

                double flujo = 0;
                double flujo1 = 0;
                double flujo2 = 0;
                LineaEms lineaEms = ObtenerLinea(out flujo, entity.Nombretna1, relacionBarra, entity.NombLinea, relacionLinea, out flujo1, out flujo2);

                if (lineaEms != null)
                {
                    entity.Flujo = flujo;
                    result.Add(entity);
                }

                log += " Flujo:" + (lineaEms != null ? entity.Flujo.ToString() : "NO ENCONTRADA");
                log += ConstantesCortoPlazo.SaltoLinea;
            }

            //Sacamos los distintos grupos
            List<int> grupos = new List<int>();

            foreach (PrCongestionDTO item in result)
            {
                if (!grupos.Contains((int)item.Grulincodi)) grupos.Add((int)item.Grulincodi);
            }

            List<PrCongestionDTO> resultado = new List<PrCongestionDTO>();

            foreach (int id in grupos)
            {
                PrCongestionDTO item = new PrCongestionDTO();
                item.ListaItems = new List<PrCongestionitemDTO>();
                item.Flujo = 0;

                #region Mejoras CMgN
                item.Grulincodi = id;
                #endregion

                foreach (PrCongestionDTO congestion in result)
                {
                    if (congestion.Grulincodi == id)
                    {
                        double flujo = 0;
                        double flujo1 = 0;
                        double flujo2 = 0;
                        LineaEms lineaEms = ObtenerLinea(out flujo, congestion.Nombretna1, relacionBarra,
                            congestion.NombLinea, relacionLinea, out flujo1, out flujo2);

                        if (lineaEms != null)
                        {
                            item.Flujo += flujo;
                            PrCongestionitemDTO congestionItem = new PrCongestionitemDTO();
                            congestionItem.Nombarra1 = congestion.Nodobarra1;
                            congestionItem.Nombarra2 = congestion.Nodobarra2;
                            congestionItem.NombLinea = congestion.NombLinea;// (int)congestion.Configcodi;
                            congestionItem.Nombretna = congestion.Nombretna1;
                            item.ListaItems.Add(congestionItem);
                        }
                    }
                }
                resultado.Add(item);
            }

            resultado.AddRange(congestionTrafo3D);

            return resultado;
        }

        #region Region_seguridad

        /// <summary>
        /// Permite obtener los datos de la congestión de regiones de seguridad
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="relacion"></param>
        /// <returns></returns>
        private List<PrCongestionDTO> ObtenerCongestionRegionSeguridad(DateTime fechaDatos, List<NombreCodigoBarra> relacionBarra,
            List<NombreCodigoLinea> relacionLinea, List<EqRelacionDTO> generadores, ref string log)
        {
            DateTime fecha = (fechaDatos.Hour == 0 && fechaDatos.Minute == 0) ? fechaDatos.AddMinutes(-1) : fechaDatos;
            List<PrCongestionDTO> list = FactorySic.GetPrCongestionRepository().ObtenerCongestionRegionSeguridad(fecha);

            log += "*** OBTENER CONGESTION REGIÓN DE SEGURIDAD" + ConstantesCortoPlazo.SaltoLinea;

            foreach (PrCongestionDTO entity in list)
            {
                entity.ListaItems = new List<PrCongestionitemDTO>();
                List<CmRegionseguridadDetalleDTO> listEquipos = FactorySic.GetCmRegionseguridadDetalleRepository().GetByCriteria((int)entity.Regsegcodi);

                decimal sumGeneracion = 0;
                decimal sumFlujo = 0;
                foreach (CmRegionseguridadDetalleDTO itemEquipo in listEquipos)
                {
                    if (itemEquipo.Famcodi == 1)
                    {
                        double flujo = 0;
                        double flujo1 = 0;
                        double flujo2 = 0;
                        LineaEms lineaEms = ObtenerLinea(out flujo, itemEquipo.Nombretna, relacionBarra, entity.NombLinea, relacionLinea, out flujo1, out flujo2);

                        if (lineaEms != null)
                        {
                            sumFlujo = sumFlujo + (decimal)flujo;
                        }

                        entity.ListaItems.Add(new PrCongestionitemDTO
                        {
                            Nombretna = itemEquipo.Nombretna,
                            TipoEquipo = ConstantesCortoPlazo.IdLineaTransmision
                        });

                    }
                    else if (itemEquipo.Famcodi == 2)
                    {
                        EqRelacionDTO generador = generadores.Where(x => x.Equicodi == itemEquipo.Equicodi).First();
                        if (generador != null)
                        {
                            sumGeneracion = sumGeneracion + (decimal)generador.PotGenerada;

                            int tipo = 0;
                            if (generador.IndTipo == ConstantesCortoPlazo.TipoHidraulica)
                            {
                                tipo = ConstantesCortoPlazo.TipoGeneradorHidraulico;
                            }
                            else if (generador.IndTipo == ConstantesCortoPlazo.TipoTermica)
                            {
                                tipo = ConstantesCortoPlazo.TipoGeneradorTermico;
                            }

                            entity.ListaItems.Add(new PrCongestionitemDTO
                            {
                                Nombretna = generador.Nombretna,
                                TipoEquipo = tipo
                            });
                        }
                    }
                }

                entity.ParamB = sumGeneracion - entity.Regsegvalorm * sumFlujo;
                entity.Pmax = sumGeneracion + sumFlujo;

                #region CambioRS29122020
                entity.GenT = sumGeneracion;
                entity.FlujoT = sumFlujo;
                #endregion


            }


            return list;
        }

        #endregion

        /// <summary>
        /// Permite obtener la generación forzada
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="lista"></param>
        /// <param name="datosEPPS"></param>
        /// <returns></returns>
        public List<PrGenforzadaDTO> ObtenerGeneracionForzada(DateTime fechaDatos,
            List<NombreCodigoBarra> lista, List<string[]> datosEPPS, List<EqRelacionDTO> listaForzada, ref string log, out List<PrGenforzadaDTO> forzadaInicial)
        {
            DateTime fechaProceso = (fechaDatos.Hour == 0 && fechaDatos.Minute == 0) ? fechaDatos.AddMinutes(-1) : fechaDatos;
            log += "*** GENERACION FORZADA" + ConstantesCortoPlazo.SaltoLinea;

            //-- Cambios por nuevo tna --//
            List<PrGenforzadaDTO> entitys = FactorySic.GetPrGenforzadaRepository().ObtenerGeneracionForzadaProceso(fechaProceso)
                .Where(x => !string.IsNullOrEmpty(x.Nombretna)).ToList();


            forzadaInicial = entitys;

            //List<int> listNoForzada1 = listaForzada.Where(x => x.Indnoforzada == ConstantesAppServicio.SI).Select(x => (int)x.Equicodi).ToList();

            //- Modificación Movisoft 18022021
            List<int> listNoForzada = listaForzada.Where(x => x.Indnoforzada == ConstantesAppServicio.SI &&
                !entitys.Any(y => y.Equicodi == x.Equicodi)).Select(x => (int)x.Equicodi).ToList();
            //- Fin modificación Movisoft 18022021

            foreach (PrGenforzadaDTO entity in entitys)
            {
                string[] datos = UtilCortoPlazoTna.ObtenerValorPSSE(entity.Nombretna, datosEPPS);

                decimal valor;
                if (datos != null)
                {
                    if (datos.Length > 0)
                    {
                        if (datos[0] != null)
                        {
                            if (decimal.TryParse(datos[0], out valor))
                            {
                                entity.Valor = valor;
                            }
                        }
                        entity.Codbarra = datos[2];
                    }
                }

                string tension = string.Empty;
                entity.Nombarra = UtilCortoPlazoTna.ObtenerCodigoBarra(entity.Codbarra, lista, out tension);
                entity.Tension = tension;
                //entity.Valor = 0;

                log += "Cod Barra:" + (entity.Codbarra == null ? "" : entity.Codbarra) + " Tension:" + (entity.Tension == null ? "" : entity.Tension);
                log += " Valor:" + entity.Valor + ConstantesCortoPlazo.SaltoLinea;
            }

            List<PrGenforzadaDTO> list = new List<PrGenforzadaDTO>();
            PrGenforzadaDTO item = null;
            int idGrupo = 0;

            foreach (PrGenforzadaDTO entity in entitys)
            {
                item = new PrGenforzadaDTO();
                item.ListaItems.Add(new PrgenforzadaitemDTO
                {
                    Codbarra = entity.Codbarra,
                    Idgenerador = entity.Idgener,
                    Nombarra = entity.Nombarra,
                    Tension = entity.Tension,
                    Nombretna = entity.Nombretna
                }
                );
                item.Cantidad++;
                item.Suma += (entity.Valor != null) ? (decimal)entity.Valor : 0;
                item.Subcausacmg = entity.Subcausacmg;

                if (!listNoForzada.Contains(entity.Equicodi))
                {
                    list.Add(item);
                }
                //idGrupo = entity.Grupocodi;
            }


            //- Elementos a excluir
            List<EqRelacionDTO> listExcluir = new List<EqRelacionDTO>();
            foreach (PrGenforzadaDTO itemForzada in list)
            {
                foreach (PrgenforzadaitemDTO subItem in itemForzada.ListaItems)
                {
                    listExcluir.Add(new EqRelacionDTO
                    {
                        Nombarra = subItem.Nombarra,
                        Idgener = subItem.Idgenerador,
                        Nombretna = subItem.Nombretna
                    });
                }
            }

            string logForzada = "";
            //agregar las unidades forzadas
            foreach (EqRelacionDTO itemRel in listaForzada)
            {
                if (itemRel.Indnoforzada != ConstantesAppServicio.SI)
                {
                    if (itemRel.Forzada && listExcluir.Where(x => x.Nombretna == itemRel.Nombretna).Count() == 0)
                    {
                        item = new PrGenforzadaDTO();
                        item.ListaItems.Add(new PrgenforzadaitemDTO
                        {
                            Codbarra = itemRel.Codbarra,
                            Idgenerador = itemRel.Idgener,
                            Nombarra = itemRel.Nombarra,
                            Tension = itemRel.Tension,
                            Nombretna = itemRel.Nombretna
                        }
                        );
                        item.Cantidad++;
                        item.Suma += (!string.IsNullOrEmpty(itemRel.Calificacion)) ? (decimal)itemRel.PotGenerada : 0;
                        item.Subcausacmg = itemRel.Calificacion;
                        list.Add(item);
                        logForzada += "Item: codbarra," + itemRel.Codbarra + ",Idgenerador," + itemRel.Idgener + ",Nombarra," + itemRel.Nombarra + "\r\n";
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Permite obtener el código asociado a un nombre de barra
        /// </summary>
        /// <param name="nomBarra"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private string ObtenerCodigoBarra(string nomBarra, List<NombreCodigoBarra> list, out string tension)
        {
            string codBarra = string.Empty;
            tension = string.Empty;

            if (nomBarra != null)
            {
                foreach (NombreCodigoBarra item in list)
                {
                    if (item.NombBarra.Trim().ToUpper() == nomBarra.Trim().ToUpper())
                    {
                        codBarra = item.CodBarra;
                        tension = item.Tension;
                        break;
                    }
                }
            }

            return codBarra;
        }

        /// <summary>
        /// Permite obtener los datos de la sección de generacion
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="list"></param>
        /// <param name="datosEPPS"></param>
        /// <param name="datosValorAgua"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public List<RegistroGenerado> ObtenerDatosGeneracion(DateTime fechaDatos, int periodo, List<EqRelacionDTO> list, List<string[]> datosEPPS,
            List<string[]> datosValorAgua, ref string log, out string logInconsistencias, out bool flagInconsistencias,
            out string logModosOperacion, out bool flagModosOperacion, out string logOperacionEMS, out bool flagOperacionEMS, bool flagFenix,
            bool flagMD, List<CpMedicion48DTO> lstCmgBarras)
        {
            DateTime fechaNCP = fechaDatos;
            DateTime fechaProceso = (fechaDatos.Hour == 0 && fechaDatos.Minute == 0) ? fechaDatos.AddMinutes(-1) : fechaDatos;

            List<RegistroGenerado> resultado = new List<RegistroGenerado>();
            List<RegistroGenerado> listCicloCombinado = new List<RegistroGenerado>();
            decimal potencia = 0;
            string ci = string.Empty;
            string genMax = string.Empty;
            string genMin = string.Empty;

            double minutos = (double)(new ParametroAppServicio()).ObtenerValorParametro(ConstantesCortoPlazo.IdParametroMinutos, fechaProceso);
            if (minutos == 0)
                minutos = 10;

            double rpf = (double)(new ParametroAppServicio()).ObtenerValorParametro(ConstantesCortoPlazo.IdParametroRpf, fechaProceso);
            if (rpf == 0)
                rpf = 0.024;

            double variacionPotencia = (double)(new ParametroAppServicio()).ObtenerValorParametro(ConstantesCortoPlazo.IdParametroVariacionPotencia, fechaProceso);

            #region Calculo
            int contador = 1;
            logInconsistencias = "Barra, ID, Tensión, Pot. Generada, Ind. Operación, Pot. Max Calculada, Pot. Min Calculada, Pot. Max FT, Pot. Min FT, Vel. Carga, Vel. Reducción\n";
            flagInconsistencias = false;
            logModosOperacion = string.Empty;
            flagModosOperacion = false;
            logOperacionEMS = string.Empty;
            flagOperacionEMS = false;

            foreach (EqRelacionDTO item in list)
            {
                bool flag = true;

                RegistroGenerado entity = new RegistroGenerado();
                entity.BarraID = Convert.ToInt32(item.Codbarra);
                entity.GenerID = item.Idgener;
                entity.Nombretna = item.Nombretna;
                entity.Indnoforzada = item.Indnoforzada;
                //- Lineas adicionales para AC
                entity.BarraID2 = (!string.IsNullOrEmpty(item.Codbarra1)) ? Convert.ToInt32(item.Codbarra1) : 0;
                entity.Tension2 = item.Tension1;
                //- Fin lineas adicionales para AC

                #region Mejoras CMgN
                entity.Equicodi = item.Equicodi;
                #endregion

                entity.Tipo = item.IndTipo;
                entity.BarraNombre = item.Nombarra;
                entity.Tension = item.Tension;
                entity.Potencia = (decimal)item.PotGenerada;
                entity.IndOpe = int.Parse((item.IndOperacion != null) ? item.IndOperacion : 0.ToString());
                entity.ExistenciaRsf = item.ExistenciaRsf;
                entity.ValorRsf = item.ValorRsf;
                entity.IdCalificacion = item.IdCalificacionCompensacion;

                //-inciio cambios
                bool flagPotencia = true;
                if (flagFenix)
                {
                    if (item.Equicodi == 13602 || item.Equicodi == 13603 || item.Equicodi == 13604 || item.Equicodi == -100)
                    {
                        flagPotencia = false;
                        item.PotenciaMaxima = item.PotGenerada;

                    }
                }
                if (flagPotencia)
                { //fin cambios
                    if (entity.Potencia <= 0)
                    {
                        entity.IndOpe = 0;
                    }
                }  //quitar lineas

                //- Calculo para las eolicas solares y no pertenecientes a coes
                if (!item.IndEspecial)
                {
                    if (item.VelocidadCarga == null) item.VelocidadCarga = 0;
                    if (item.VelocidadDescarga == null) item.VelocidadDescarga = 0;

                    double rsf = 0;
                    if (item.ExistenciaRsf)
                    {
                        if (item.CantidadRsf > 0)
                            rsf = (double)(item.ValorRsf / item.CantidadRsf);
                    }

                    item.PotMax = (decimal)((item.PotenciaMaxima) / (1 + rpf));
                    item.PotMin = (decimal)((item.PotenciaMinima) / (1 - rpf));

                    entity.PotenciaMaxima = (decimal)(Math.Min((double)item.PotGenerada + minutos * (double)item.VelocidadCarga, (item.PotenciaMaxima - rsf) / (1 + rpf)));
                    entity.PotenciaMinima = (decimal)(Math.Max((double)item.PotGenerada - minutos * (double)item.VelocidadDescarga, (item.PotenciaMinima + rsf) / (1 - rpf)));
                    entity.PotenciaMaxima = Math.Max(entity.PotenciaMaxima, entity.Potencia);
                    entity.PotenciaMinima = Math.Min(entity.PotenciaMinima, entity.Potencia);

                    if (variacionPotencia >= 0 && variacionPotencia <= 100)
                    {
                        entity.PotenciaMaxima = Math.Min(entity.PotenciaMaxima, entity.Potencia * (1 + (decimal)variacionPotencia / 100));
                        entity.PotenciaMinima = Math.Max(entity.PotenciaMinima, entity.Potencia * (1 - (decimal)variacionPotencia / 100));
                    }

                    //- Agregado para CC
                    entity.PotGeneradaCC = item.PotGenerada;
                    entity.VelocidadCargaCC = (double)item.VelocidadCarga;
                    entity.VelocidadDescargaCC = (double)item.VelocidadDescarga;
                    entity.PotenciaMaximaCC = item.PotenciaMaxima;
                    entity.PotenciaMinimaCC = item.PotenciaMinima;
                    //- Fin agregado para CC

                    //item.PotMax = entity.PotenciaMaxima;
                    //item.PotMin = entity.PotenciaMinima;

                    potencia = entity.Potencia;

                    if (potencia == 0 || entity.IndOpe == 0)
                    {
                        entity.PotenciaMaxima = potencia;
                        entity.PotenciaMinima = potencia;
                    }

                    //- Generaremos log de inconsistencias                

                    if (entity.Potencia > entity.PotenciaMaxima || entity.PotenciaMinima > entity.PotenciaMaxima)
                    {
                        logInconsistencias += entity.BarraNombre + "," +
                                              entity.GenerID + "," +
                                              entity.Tension + "," +
                                              entity.Potencia + "," +
                                              entity.IndOpe + "," +
                                              entity.PotenciaMaxima + "," +
                                              entity.PotenciaMinima + "," +
                                              item.PotenciaMaxima + "," +
                                              item.PotenciaMinima + "," +
                                              item.VelocidadCarga + "," +
                                              item.VelocidadDescarga + "\n";
                        flagInconsistencias = true;
                    }


                    if (item.IndTipo == ConstantesCortoPlazo.TipoHidraulica)
                    {
                        #region Calculo Hidraulico

                        if (flagMD)
                        {
                            var cmgUSDMwh = (new McpAppServicio()).ObtenerValorHxPorRecurso(lstCmgBarras, periodo, item.Recurcodibarra);
                            var cmgPENMwh = cmgUSDMwh * item.FactorConversion;
                            ci = cmgPENMwh?.ToString();
                        }
                        else
                        {
                            //ci = UtilCortoPlazo.ObtenerValorAgua(item.Nombrencp, periodo, datosValorAgua, item.FactorConversion);
                            ci = UtilCortoPlazo.ObtenerValorAgua(item.Codbarrancp, periodo, datosValorAgua, item.FactorConversion, fechaNCP);
                        }


                        entity.IndNcv = ConstantesCortoPlazo.CaracterCero;
                        entity.Costo1 = 0;
                        entity.Cod = contador;
                        contador++;

                        if (!string.IsNullOrEmpty(ci))
                        {
                            entity.Ci1 = ci;
                            entity.Pmax1 = potencia.ToString();

                            if (potencia >= 0)
                            {
                                entity.IndNcv = ConstantesCortoPlazo.CaracterUno;
                            }
                            else
                            {
                                entity.IndNcv = ConstantesCortoPlazo.CaracterCero;
                                potencia = 0;
                            }
                        }

                        ///- Linea agregada movisoft 06.03.2021
                        if (item.Indgeneracionrer == ConstantesAppServicio.SI)
                        {
                            entity.Ci1 = 0.ToString();
                            entity.PotenciaMinima = 0;
                        }

                        #endregion
                    }

                    if (item.IndTipo == ConstantesCortoPlazo.TipoTermica)
                    {
                        #region Calculo Termico

                        if (item.Indcc == 0)
                        {
                            //- Verificamos las unidades que no tienen modos de operación
                            if (item.IndOperacion == 1.ToString() && item.IdModoOperacion == 0)
                            {
                                logModosOperacion += entity.BarraNombre + "," +
                                                  entity.GenerID + "," +
                                                  entity.Tension + "," +
                                                  entity.Potencia + "," +
                                                  entity.IndOpe + "\n";
                                flagModosOperacion = true;
                            }

                            //- Verificamos las unidades que tienen modo de operacion pero estan no operativos en el EMS

                            if (item.IndOperacion == 0.ToString() && item.IdModoOperacion > 0)
                            {
                                logOperacionEMS += entity.BarraNombre + "," +
                                                  entity.GenerID + "," +
                                                  entity.Tension + "," +
                                                  entity.Potencia + "," +
                                                  entity.IndOpe + "\n";
                                flagOperacionEMS = true;
                            }

                            List<RegistroGenerado> listTGS = new List<RegistroGenerado>();
                            RegistroGenerado result = this.ObtenerRegistroTermico(item, entity);
                            entity.IndNcv = result.IndNcv;
                            entity.Cod = contador;
                            entity.ModoOperacion = item.IdModoOperacion;
                            contador++;

                            if (result.IndNcv.Trim() != 0.ToString())
                            {
                                entity.Costo1 = result.Costo1;
                                entity.Ci1 = result.Ci1;
                                entity.Ci2 = result.Ci2;
                                entity.Ci3 = result.Ci3;
                                entity.Ci4 = result.Ci4;
                                entity.Ci5 = result.Ci5;
                                entity.Pmax1 = result.Pmax1;
                                entity.Pmax2 = result.Pmax2;
                                entity.Pmax3 = result.Pmax3;
                                entity.Pmax4 = result.Pmax4;
                                entity.Pmax5 = result.Pmax5;
                            }
                        }
                        else
                        {
                            entity.Relacion = item;
                            entity.Ccombcodi = item.Ccombcodi;
                            entity.IdModoOperacion = item.IdModoOperacion;
                            listCicloCombinado.Add(entity);
                            flag = false;
                        }


                        ///- Linea agregada movisoft 06.03.2021
                        if (item.Indgeneracionrer == ConstantesAppServicio.SI)
                        {
                            entity.Ci1 = 0.ToString();
                            entity.PotenciaMinima = 0;
                        }

                        #endregion
                    }

                    ///- Linea agregada movisoft 09.03.2021
                    if (item.Indgeneracionrer == ConstantesAppServicio.SI)
                    {
                        entity.Ci1 = 0.ToString();
                        entity.PotenciaMinima = 0;
                    }

                    if (flag)
                    {
                        resultado.Add(entity);
                    }
                }
                else
                {
                    entity.PotenciaMaxima = entity.Potencia;
                    entity.PotenciaMinima = entity.Potencia;
                    entity.IndNcv = ConstantesCortoPlazo.CaracterCero;
                    entity.Costo1 = 0;
                    entity.Ci1 = string.Empty;
                    entity.Ci2 = string.Empty;
                    entity.Pmax1 = entity.Potencia.ToString();
                    entity.Pmax2 = entity.Potencia.ToString();
                    entity.Cod = contador;
                    contador++;

                    ///- Linea agregada movisoft 09.03.2021
                    if (item.Indgeneracionrer == ConstantesAppServicio.SI)
                    {
                        entity.Ci1 = 0.ToString();
                        entity.PotenciaMinima = 0;
                    }

                    resultado.Add(entity);
                }
            }


            //- Obtenemos los grupos que pertenecen a un ciclo combinado
            List<int> idCcombinados = listCicloCombinado.Where(x => x.Ccombcodi != null).Select(x => (int)x.Ccombcodi).Distinct().ToList();

            foreach (int idCiclo in idCcombinados)
            {
                List<RegistroGenerado> listCC = listCicloCombinado.Where(x => x.Ccombcodi == idCiclo).ToList();
                RegistroGenerado tvcc = listCC.Where(x => x.Relacion.Indtvcc == ConstantesAppServicio.SI).FirstOrDefault();
                List<RegistroGenerado> tgcc = listCC.Where(x => x.Relacion.Indtvcc != ConstantesAppServicio.SI).ToList();
                List<RegistroGenerado> listCCProceso = new List<RegistroGenerado>();

                bool flagTV = false;
                if (tvcc.IndOpe == 1) flagTV = true;

                foreach (RegistroGenerado itemTG in tgcc)
                {
                    if (flagTV && itemTG.IndOpe == 1)
                    {
                        if (itemTG.IdModoOperacion == 0)
                        {
                            listCCProceso.Add(itemTG);
                        }
                        else
                        {
                            RegistroGenerado result = this.ObtenerRegistroTermico(itemTG.Relacion, itemTG);
                            itemTG.IndNcv = result.IndNcv;
                            itemTG.Cod = contador;
                            itemTG.ModoOperacion = itemTG.IdModoOperacion;
                            contador++;

                            if (result.IndNcv.Trim() != 0.ToString())
                            {
                                itemTG.Costo1 = result.Costo1;
                                itemTG.Ci1 = result.Ci1;
                                itemTG.Ci2 = result.Ci2;
                                itemTG.Ci3 = result.Ci3;
                                itemTG.Ci4 = result.Ci4;
                                itemTG.Ci5 = result.Ci5;
                                itemTG.Pmax1 = result.Pmax1;
                                itemTG.Pmax2 = result.Pmax2;
                                itemTG.Pmax3 = result.Pmax3;
                                itemTG.Pmax4 = result.Pmax4;
                                itemTG.Pmax5 = result.Pmax5;
                            }
                            resultado.Add(itemTG);
                        }
                    }
                    else
                    {
                        //- Verificamos las unidades que no tienen modos de operación
                        if (itemTG.IndOpe == 1 && itemTG.IdModoOperacion == 0)
                        {
                            logModosOperacion += itemTG.BarraNombre + "," +
                                              itemTG.GenerID + "," +
                                              itemTG.Tension + "," +
                                              itemTG.Potencia + "," +
                                              itemTG.IndOpe + "\n";
                            flagModosOperacion = true;
                        }

                        //- Verificamos las unidades que tienen modo de operacion pero no estan operativas en el EMS
                        if (itemTG.IndOpe == 0 && itemTG.IdModoOperacion > 0)
                        {
                            logOperacionEMS += itemTG.BarraNombre + "," +
                                                  itemTG.GenerID + "," +
                                                  itemTG.Tension + "," +
                                                  itemTG.Potencia + "," +
                                                  itemTG.IndOpe + "\n";
                            flagOperacionEMS = true;
                        }

                        RegistroGenerado result = this.ObtenerRegistroTermico(itemTG.Relacion, itemTG);
                        itemTG.IndNcv = result.IndNcv;
                        itemTG.Cod = contador;
                        itemTG.ModoOperacion = itemTG.IdModoOperacion;
                        contador++;

                        if (result.IndNcv.Trim() != 0.ToString())
                        {
                            itemTG.Costo1 = result.Costo1;
                            itemTG.Ci1 = result.Ci1;
                            itemTG.Ci2 = result.Ci2;
                            itemTG.Ci3 = result.Ci3;
                            itemTG.Ci4 = result.Ci4;
                            itemTG.Ci5 = result.Ci5;
                            itemTG.Pmax1 = result.Pmax1;
                            itemTG.Pmax2 = result.Pmax2;
                            itemTG.Pmax3 = result.Pmax3;
                            itemTG.Pmax4 = result.Pmax4;
                            itemTG.Pmax5 = result.Pmax5;
                        }
                        resultado.Add(itemTG);
                    }
                }

                //- Verificamos que la TV tenga modo de operación
                if (tvcc.IndOpe == 1 && tvcc.IdModoOperacion == 0)
                {
                    logModosOperacion += tvcc.BarraNombre + "," +
                                      tvcc.GenerID + "," +
                                      tvcc.Tension + "," +
                                      tvcc.Potencia + "," +
                                      tvcc.IndOpe + "\n";
                    flagModosOperacion = true;
                }

                //- Verificamos que la tv tenga modo de operacion pero en el ems aparezca desconectado
                if (tvcc.IndOpe == 0 && tvcc.IdModoOperacion == 1)
                {
                    logOperacionEMS += tvcc.BarraNombre + "," +
                                                 tvcc.GenerID + "," +
                                                 tvcc.Tension + "," +
                                                 tvcc.Potencia + "," +
                                                 tvcc.IndOpe + "\n";
                    flagOperacionEMS = true;
                }

                //- Calculos para el ciclo combinado

                //- Calculamos los siguientes valores
                //- Pmax
                //- Pmin
                //- Pgen

                double pgencc = tvcc.PotGeneradaCC;
                double pmaxcc = tvcc.PotenciaMaximaCC;
                double pmincc = tvcc.PotenciaMinimaCC;

                foreach (RegistroGenerado itemTG in listCCProceso)
                {
                    pgencc += itemTG.PotGeneradaCC;
                    pmaxcc += itemTG.PotenciaMaximaCC;
                    pmincc += itemTG.PotenciaMinimaCC;
                }


                double rsfcc = 0;

                if (tvcc.ExistenciaRsf)
                {
                    rsfcc = (double)tvcc.ValorRsf;
                }

                //- Calculo de los potencias maximas y mínimas del ciclo combinado
                tvcc.PotenciaMaxima = (decimal)(Math.Min((double)pgencc + minutos * (double)tvcc.VelocidadCargaCC, (pmaxcc - rsfcc) / (1 + rpf)));
                tvcc.PotenciaMinima = (decimal)(Math.Max((double)pgencc - minutos * (double)tvcc.VelocidadDescargaCC, (pmincc + rsfcc) / (1 - rpf)));

                tvcc.PotenciaMaxima = Math.Max(tvcc.PotenciaMaxima, (decimal)pgencc);
                tvcc.PotenciaMinima = Math.Min(tvcc.PotenciaMinima, (decimal)pgencc);
                tvcc.Potencia = (decimal)pgencc;
                tvcc.ModoOperacion = tvcc.IdModoOperacion;


                if (variacionPotencia >= 0 && variacionPotencia <= 100)
                {
                    tvcc.PotenciaMaxima = Math.Min(tvcc.PotenciaMaxima, tvcc.Potencia * (1 + (decimal)variacionPotencia / 100));
                    tvcc.PotenciaMinima = Math.Max(tvcc.PotenciaMinima, tvcc.Potencia * (1 - (decimal)variacionPotencia / 100));
                }

                RegistroGenerado resultTV = this.ObtenerRegistroTermico(tvcc.Relacion, tvcc);
                tvcc.IndNcv = resultTV.IndNcv;
                tvcc.Cod = contador;

                if (resultTV.IndNcv.Trim() != 0.ToString())
                {
                    tvcc.Costo1 = resultTV.Costo1;
                    tvcc.Ci1 = resultTV.Ci1;
                    tvcc.Ci2 = resultTV.Ci2;
                    tvcc.Ci3 = resultTV.Ci3;
                    tvcc.Ci4 = resultTV.Ci4;
                    tvcc.Ci5 = resultTV.Ci5;
                    tvcc.Pmax1 = resultTV.Pmax1;
                    tvcc.Pmax2 = resultTV.Pmax2;
                    tvcc.Pmax3 = resultTV.Pmax3;
                    tvcc.Pmax4 = resultTV.Pmax4;
                    tvcc.Pmax5 = resultTV.Pmax5;
                }

                resultado.Add(tvcc);

                foreach (RegistroGenerado itemTG in listCCProceso)
                {
                    //- Aca debemos verificar las potencias minimas y pmax, pmax2, etc, el resto queda igual
                    itemTG.IndNcv = resultTV.IndNcv;
                    itemTG.Cod = contador;
                    itemTG.ModoOperacion = tvcc.ModoOperacion;

                    if (resultTV.IndNcv.Trim() != 0.ToString())
                    {
                        itemTG.Potencia = tvcc.Potencia;
                        itemTG.PotenciaMaxima = tvcc.PotenciaMaxima;
                        itemTG.PotenciaMinima = tvcc.PotenciaMinima;
                        itemTG.Costo1 = resultTV.Costo1;
                        itemTG.Ci1 = resultTV.Ci1;
                        itemTG.Ci2 = resultTV.Ci2;
                        itemTG.Ci3 = resultTV.Ci3;
                        itemTG.Ci4 = resultTV.Ci4;
                        itemTG.Ci5 = resultTV.Ci5;
                        itemTG.Pmax1 = resultTV.Pmax1;
                        itemTG.Pmax2 = resultTV.Pmax2;
                        itemTG.Pmax3 = resultTV.Pmax3;
                        itemTG.Pmax4 = resultTV.Pmax4;
                        itemTG.Pmax5 = resultTV.Pmax5;
                    }
                    resultado.Add(itemTG);
                }
                contador++;
            }

            #endregion

            return resultado;
        }

        /// <summary>
        /// Permite obtener los datos para el caso de térmicos
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public RegistroGenerado ObtenerRegistroTermico(EqRelacionDTO entity, RegistroGenerado registroPadre)
        {
            RegistroGenerado registro = new RegistroGenerado();
            List<CoordenadaConsumo> curva = entity.ListaCurva;
            List<CoordenadaConsumo> coordenadas = new List<CoordenadaConsumo>();

            if (curva != null)
            {
                if (curva.Count > 0)
                {
                    //if (curva[0].Potencia > (decimal)registroPadre.PotenciaMinima) registroPadre.PotenciaMinima = (decimal)curva[0].Potencia;
                    //if (curva[curva.Count - 1].Potencia < (decimal)registroPadre.PotenciaMaxima) registroPadre.PotenciaMaxima = (decimal)curva[curva.Count - 1].Potencia;

                    //- Realizamos la interpolación de ser necesario

                    //- Evaluamos la potencia mínima
                    if (curva[0].Potencia > (decimal)registroPadre.PotenciaMinima)
                    {
                        decimal px = (decimal)registroPadre.PotenciaMinima;
                        decimal p0 = curva[0].Potencia;
                        decimal p1 = curva[1].Potencia;
                        decimal c0 = curva[0].Consumo;
                        decimal c1 = curva[1].Consumo;
                        decimal cx = (c0 * (p1 - px) + c1 * (px - p0)) / (p1 - p0);

                        curva[0].Consumo = cx;
                        curva[0].Potencia = px;
                    }

                    //- Evaluamos la potencia máxima
                    if (curva[curva.Count - 1].Potencia < (decimal)registroPadre.PotenciaMaxima)
                    {
                        decimal px = (decimal)registroPadre.PotenciaMaxima;
                        decimal p0 = curva[curva.Count - 2].Potencia;
                        decimal p1 = curva[curva.Count - 1].Potencia;
                        decimal c0 = curva[curva.Count - 2].Consumo;
                        decimal c1 = curva[curva.Count - 1].Consumo;
                        decimal cx = ((c1 - c0) * (px - p0) + c0 * (p1 - p0)) / (p1 - p0);
                        curva[curva.Count - 1].Consumo = cx;
                        curva[curva.Count - 1].Potencia = px;
                    }

                    int indexInicial = 0;
                    int indexFinal = 0;
                    decimal coorxmin = 0;
                    decimal coorymin = 0;
                    decimal coorxmax = 0;
                    decimal coorymax = 0;
                    for (int i = 0; i < curva.Count(); i++)
                    {
                        if (curva[i].Potencia > registroPadre.PotenciaMinima)
                        {
                            indexInicial = i;
                            coorxmin = registroPadre.PotenciaMinima;
                            coorymin = (curva[i].Consumo - curva[i - 1].Consumo) * (registroPadre.PotenciaMinima - curva[i - 1].Potencia) /
                                (curva[i].Potencia - curva[i - 1].Potencia) + curva[i - 1].Consumo;
                            break;
                        }
                        else if (curva[i].Potencia == registroPadre.PotenciaMinima)
                        {
                            indexInicial = i + 1;
                            coorxmin = curva[i].Potencia;
                            coorymin = curva[i].Consumo;
                            break;
                        }
                    }

                    for (int i = curva.Count() - 1; i >= 0; i--)
                    {
                        if (curva[i].Potencia < registroPadre.PotenciaMaxima)
                        {
                            indexFinal = i;
                            coorxmax = registroPadre.PotenciaMaxima;
                            coorymax = (curva[i + 1].Consumo - curva[i].Consumo) * (registroPadre.PotenciaMaxima - curva[i].Potencia) /
                                (curva[i + 1].Potencia - curva[i].Potencia) + curva[i].Consumo;
                            break;
                        }
                        else if (curva[i].Potencia == registroPadre.PotenciaMaxima)
                        {
                            indexFinal = i - 1;
                            coorxmax = curva[i].Potencia;
                            coorymax = curva[i].Consumo;
                        }
                    }

                    coordenadas.Add(new CoordenadaConsumo { Potencia = coorxmin, Consumo = coorymin });
                    for (int i = indexInicial; i <= indexFinal; i++)
                        coordenadas.Add(new CoordenadaConsumo { Potencia = curva[i].Potencia, Consumo = curva[i].Consumo });
                    coordenadas.Add(new CoordenadaConsumo { Potencia = coorxmax, Consumo = coorymax });

                    //- Aca debemos modificar para obtener los costos incrementales para cada tramo


                }
            }
            if (coordenadas.Count > 1)
            {
                int count = 0;
                for (int i = 1; i < coordenadas.Count; i++)
                {
                    decimal ci = 0;
                    decimal pmax = coordenadas[i].Potencia;
                    if (coordenadas[i].Potencia - coordenadas[i - 1].Potencia != 0)
                        ci = ((coordenadas[i].Consumo - coordenadas[i - 1].Consumo) / (coordenadas[i].Potencia - coordenadas[i - 1].Potencia)) *
                            (decimal)entity.CostoCombustible * (decimal)entity.FactorConversion + (decimal)entity.CostoVariableOYM * (decimal)ConstantesCortoPlazo.FactorMW;

                    registro.GetType().GetProperty(ConstantesCortoPlazo.PropCi + i).SetValue(registro, ci.ToString());
                    registro.GetType().GetProperty(ConstantesCortoPlazo.PropPmax + i).SetValue(registro, pmax.ToString());
                    count++;
                }

                registro.IndNcv = count.ToString();
                registro.Costo1 = (decimal)entity.CostoCombustible * coordenadas[0].Consumo * (decimal)entity.FactorConversion +
                    registroPadre.PotenciaMinima * (decimal)entity.CostoVariableOYM * (decimal)ConstantesCortoPlazo.FactorMW;
            }
            else
            {
                registro.IndNcv = ConstantesCortoPlazo.CaracterCero;
            }

            return registro;
        }

        /// <summary>
        /// Permite obtener los archivos de trabajo del NCP
        /// </summary>
        public void ObtenerDirectorioNCP()
        {
            //- Definimos fecha de datos
            DateTime fechaProceso = DateTime.Now.AddDays(1);

            //- Obtenemos los archivos NCP
            FileHelper.ObtenerArchivoNCP(fechaProceso);

            //- Obtenemos los registros programados
            List<CmOperacionregistroDTO> listaProgramado = (new CortoPlazoAppServicio()).ObtenerOperacionRegistroProgramado(fechaProceso);

            //- Grabamos en la base de datos
            foreach (CmOperacionregistroDTO item in listaProgramado)
            {
                item.Subcausacodi = ConstantesCortoPlazo.IdCalificacionPorDefecto;
                FactorySic.GetCmOperacionregistroRepository().Save(item);
            }
        }

    }
}
