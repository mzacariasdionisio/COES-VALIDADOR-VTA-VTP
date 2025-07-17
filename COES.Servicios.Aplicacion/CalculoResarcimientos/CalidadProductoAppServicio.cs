using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Intervenciones;
using log4net;
using Novacode;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace COES.Servicios.Aplicacion.CalculoResarcimientos
{
    public class CalidadProductoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CalculoResarcimientoAppServicio));

        public readonly CalculoResarcimientoAppServicio servicioResarcimiento = new CalculoResarcimientoAppServicio();
        public readonly CorreoAppServicio servCorreo = new CorreoAppServicio();
        public readonly IntervencionesAppServicio servIntervenciones = new IntervencionesAppServicio();

        #region Correos


        #region Listado y Envio de Correos
        /// <summary>
        /// Devuelve el listado de correos enviados segun un rango de fechas
        /// </summary>
        /// <param name="strTipoCorreo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<ReLogcorreoDTO> ListarCorreosEnviados(string strTipoCorreo, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReLogcorreoDTO> lstSalida = new List<ReLogcorreoDTO>();

            string valTC = ObtenerTipoCorreo(strTipoCorreo);

            lstSalida = FactorySic.GetReLogcorreoRepository().ObtenerPorFechaYTipo(fechaInicio, fechaFin, valTC).OrderByDescending(x => x.Relcorcodi).ToList();
            lstSalida = FormatearCorreos(lstSalida);
            return lstSalida;
        }

        /// <summary>
        /// Verifica si un periodo es trimestral o semestral
        /// </summary>
        /// <param name="repercodi"></param>
        /// <returns></returns>
        public bool EsSemestral(int repercodi)
        {
            bool salida = true;
            RePeriodoDTO entityPeriodo = FactorySic.GetRePeriodoRepository().GetById(repercodi);
            string repertipo = entityPeriodo.Repertipo;

            if (repertipo == "T")
                salida = false;

            return salida;
        }

        /// <summary>
        /// Devuelve los datos de las empresas a mostrar al enviar correo
        /// </summary>
        /// <param name="repercodi"></param>
        /// <param name="tipoCorreo"></param>
        /// <param name="muestraEmp"></param>
        /// <param name="campo"></param>
        /// <returns></returns>
        public List<ReEmpresaDTO> ObtenerEmpresasMostrar(int repercodi, int tipoCorreo, out bool muestraEmp, out string campo)
        {
            List<ReEmpresaDTO> lstSalida = new List<ReEmpresaDTO>();
            RePeriodoDTO periodo = servicioResarcimiento.GetByIdRePeriodo(repercodi);
            muestraEmp = false;
            campo = "";

            switch (tipoCorreo.ToString())
            {
                #region Solicitud de envío de interrupciones Trimestral y Semestral  - plantcodi : 174/175

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesTrimestral:
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesSemestral:

                    muestraEmp = true;
                    campo = "Suministrador";

                    //Obtengo los suministradores con correos
                    List<EmpresaCorreo> listaEmpresasCorreo = ListarEmpresasYSusCorreos();
                    List<int> lstEmpresasConCorreos = listaEmpresasCorreo.Where(x => x.Emprcodi != null).Select(x => x.Emprcodi).Distinct().ToList();
                    List<ReEmpresaDTO> lstSumTotales = servicioResarcimiento.ObtenerEmpresasSuministradorasTotal();
                    List<ReEmpresaDTO> lstSumConCorreos = lstSumTotales.Where(x => lstEmpresasConCorreos.Contains(x.Emprcodi)).ToList();

                    lstSalida = lstSumConCorreos;
                    break;

                #endregion

                #region Solicitud de envío de observaciones a las interrupciones - plantcodi : 176

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones:

                    muestraEmp = true;
                    campo = "Responsable";
                    //Obtenemos todos los responsables finales del periodo 
                    lstSalida = servicioResarcimiento.ObtenerListadoGeneralAgentesResponsablesReporteCumplimiento(periodo);
                    break;

                #endregion

                #region Solicitud de envío de respuestas a las observaciones - plantcodi : 177

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones:
                    List<ReEmpresaDTO> lstTemp = new List<ReEmpresaDTO>();

                    //Obtenemos interrupciones con Conformidad responsable en NO
                    List<ReInterrupcionSuministroDetDTO> listadoInterrupcionesConformidadRespNo = servicioResarcimiento.ObtenerListadoInterrupcionesConformidadRespEnNO(periodo);
                   
                    foreach (var reg in listadoInterrupcionesConformidadRespNo)
                    {
                        ReEmpresaDTO emp = new ReEmpresaDTO();

                        emp.Emprcodi = reg.SumId.Value;
                        emp.Emprnomb = reg.SumNomb.Trim();

                        if(!lstTemp.Where(x=>x.Emprcodi ==emp.Emprcodi).Any())
                            lstTemp.Add(emp);

                    }

                    lstSalida = lstTemp;
                    muestraEmp = true;
                    campo = "Suministrador";

                    break;

                #endregion

                #region Solicitud de envío de decisiones de controversias - plantcodi : 178

                case ConstantesCalidadProducto.IdPlantillaSolicitudDecisionesControversia:

                    //Obtengo el listado general 
                    List<InterrupcionSuministroIntranet> listaGeneral = servicioResarcimiento.ObtenerListaInterrupcionIntranetPorPeriodo(periodo);
                    List<ReEmpresaDTO> lstTemp1 = new List<ReEmpresaDTO>();
                    //Obtengo los registros en controversia
                    foreach (var regInterrupcion in listaGeneral)
                    {
                        if (regInterrupcion.RegistroEnControversiaSum && regInterrupcion.RegistroEnControversiaResp)
                        {
                            ReEmpresaDTO emp = new ReEmpresaDTO();

                            emp.Emprcodi = regInterrupcion.SuministradorId;
                            emp.Emprnomb = regInterrupcion.Suministrador;

                            if (!lstTemp1.Where(x => x.Emprcodi == emp.Emprcodi).Any())
                                lstTemp1.Add(emp);
                        }

                    }
                    lstSalida = lstTemp1;
                    muestraEmp = true;
                    campo = "Suministrador";

                    break;

                #endregion

                #region Solicitud de envío de compensaciones por mala calidad de producto - plantcodi : 179

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvíoCompensacionesMalaCalidadProducto:
                    

                    break;

                #endregion

                #region Solicitud de envío de interrupciones pendientes de reportar.  - plantcodi : 320

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar:

                    muestraEmp = true;
                    campo = "Suministrador"; 
                    //Obtenemos todos los suministradores del periodo 
                    CalculoResarcimientoAppServicio servicio2 = new CalculoResarcimientoAppServicio();  
                    lstSalida = ObtenerListadoSuministradoresConContrastreOInterrupcioesSinReportar(periodo);
                    break;

                #endregion
            }

            return lstSalida;
        }        

        /// <summary>
        /// Da formato a los correos
        /// </summary>
        /// <param name="lstCorreos"></param>
        /// <returns></returns>
        public List<ReLogcorreoDTO> FormatearCorreos(List<ReLogcorreoDTO> lstCorreos)
        {
            List<ReLogcorreoDTO> lstSalida = new List<ReLogcorreoDTO>();
            List<RePeriodoDTO> lstPeriodos = FactorySic.GetRePeriodoRepository().List();

            foreach (var correo in lstCorreos)
            {
                correo.RelcorfeccreacionDesc = correo.Relcorfeccreacion.Value.ToString(ConstantesCalidadProducto.FormatoFechaFull3);
                correo.Relcorusucreacion = correo.Relcorusucreacion != null ? correo.Relcorusucreacion.Trim() : "";
                correo.Tipo = ObtenerTipoCorreo(correo.Retcorcodi.Value);
                correo.PeriodoDesc = lstPeriodos.Find(x => x.Repercodi == correo.Repercodi).Repernombre.Trim();

                lstSalida.Add(correo);
            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el tipo de correo segun plantilla usada
        /// </summary>
        /// <param name="plantcodi"></param>
        /// <returns></returns>
        public string ObtenerTipoCorreo(int retcorcodi)
        {
            string salida = "";

            switch (retcorcodi)
            {
                case 1:
                case 2:
                    salida = "Solicitud de envío de interrupciones"; break;
                case 3: salida = "Solicitud de envío de observaciones a las interrupciones"; break;
                case 4: salida = "Solicitud de envío de respuestas a las observaciones"; break;
                case 5: salida = "Solicitud de envío de decisiones de controversias"; break;
                case 7: salida = "Solicitud de envío de interrupciones pendientes de reportar"; break;
            }
            return salida;
        }

        /// <summary>
        /// Devuelve los datos del correo enviado
        /// </summary>
        /// <param name="idCorreo"></param>
        /// <returns></returns>
        public SiCorreoDTO ObtenerInfoCorreo(int idCorreo)
        {
            SiCorreoDTO salida = new SiCorreoDTO();

            ReLogcorreoDTO lcorreo = GetByIdReLogcorreo(idCorreo);

            salida.Corrcodi = idCorreo;
            salida.Corrto = lcorreo.Relcorto;
            salida.Corrcc = lcorreo.Relcorcc;
            salida.Corrbcc = lcorreo.Relcorbcc;
            salida.Corrasunto = lcorreo.Relcorasunto;
            salida.Corrcontenido = lcorreo.Relcorcuerpo;
            salida.Archivos = lcorreo.Relcorarchivosnomb;

            return salida;
        }

        /// <summary>
        /// Descarga el archivo adjuntado desde el fileserver
        /// </summary>
        /// <param name="relcorcodi"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] GetBufferArchivoAdjuntoCorreo(int relcorcodi, string fileName)
        {
            string pathAlternativo = GetPathPrincipal();

            try
            {
                string pathDestino = GetPathSubcarpeta(ConstantesCombustibles.SubcarpetaArchivoAdjuntado) + relcorcodi;

                if (FileServer.VerificarExistenciaFile(pathDestino, fileName, pathAlternativo))
                {
                    return FileServer.DownloadToArrayByte(pathDestino + "\\" + fileName, pathAlternativo);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException("No se pudo descargar el archivo del servidor.", ex);
            }

            return null;
        }        

        /// <summary>
        /// Devuelve el id de la plantilla segun su idtipo
        /// </summary>
        /// <param name="retcorcodi"></param>
        /// <returns></returns>
        public string ObtenerIdPlantillaXIdTipo(int retcorcodi)
        {
            string salida = "";

            switch (retcorcodi)
            {
                case 1: salida = ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesTrimestral; break;
                case 2: salida = ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesSemestral; break;                    
                case 3: salida = ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones; break;
                case 4: salida = ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones; break;
                case 5: salida = ConstantesCalidadProducto.IdPlantillaSolicitudDecisionesControversia; break;
                case 7: salida = ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar; break;
            }
            return salida;
        }

        /// <summary>
        /// Devuelve el listado de archivos adjuntados
        /// </summary>
        /// <param name="corrcodi"></param>
        /// <returns></returns>
        public List<CbArchivoenvioDTO> ObtenerArchivosAdjuntados(int corrcodi)
        {
            List<CbArchivoenvioDTO> lstSalida = new List<CbArchivoenvioDTO>();
            lstSalida = FactorySic.GetCbArchivoenvioRepository().GetByCorreo(corrcodi.ToString());

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el correo muestra segun los filtros seleccionados
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="repercodi"></param>
        /// <param name="tipoCorreo"></param>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public SiCorreoDTO ObtenerMuestraCorreo(int anio, int repercodi, int tipoCorreo, int? empresaId)
        {
            SiCorreoDTO salida = new SiCorreoDTO();

            int? responsableGral = ConstantesCalidadProducto.IdEmpresaNoExistente;
            int? suministradorGral = ConstantesCalidadProducto.IdEmpresaNoExistente;

            //Obtenemos responsable y suministrador
            switch (tipoCorreo.ToString())
            {
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones:
                    responsableGral = empresaId.Value;
                    break;
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones:
                case ConstantesCalidadProducto.IdPlantillaSolicitudDecisionesControversia:
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesTrimestral:
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesSemestral:
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar:
                    suministradorGral = empresaId.Value;
                    break;
                
                default:
                    responsableGral = null;
                    suministradorGral = null;
                    break;
            }


            SiPlantillacorreoDTO plantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(tipoCorreo);

            if (plantilla == null)
            {
                throw new Exception("No se encontró una plantilla de correo para el tipo de correo elegido.");
            }

            RePeriodoDTO periodo = FactorySic.GetRePeriodoRepository().GetById(repercodi);
            List<RePeriodoEtapaDTO> etapas = FactorySic.GetRePeriodoEtapaRepository().GetByPeriodo(repercodi);
            List<ReEmpresaDTO> listaEmpresas = ObtenerEmpresasTotales();

            List<VariableCorreo> listaVariables = LlenarVariablesPorEmpresa(periodo, etapas, tipoCorreo, listaEmpresas, responsableGral);
            
            string para = ObtenerCorreosParaSegunEmpresa(tipoCorreo, responsableGral, suministradorGral);


            salida.Corrto = para;
            salida.Corrcc = plantilla.PlanticorreosCc;
            salida.Corrbcc = plantilla.PlanticorreosBcc;
            salida.Corrasunto = LLenarDatosVariables(plantilla.Plantasunto, tipoCorreo, listaVariables);            
            salida.Corrcontenido = LLenarDatosVariables(plantilla.Plantcontenido, tipoCorreo, listaVariables);



            return salida;
        }

        /// <summary>
        /// Envia correos a todas las empresas (usuarios registrados)
        /// </summary>
        /// <param name="correo"></param>
        /// <param name="repercodi"></param>
        /// <param name="tipoCorreo"></param>
        /// <param name="usuario"></param>
        /// <param name="pathLogo"></param>
        public void EnviarMensajeAGrupoEmpresas(SiCorreoDTO correo, int repercodi, int tipoCorreo, int idEmpresa, string usuario, string pathLogo, string pathTempArchivos, int idArchivoOriginal)
        {
            List<string> listFiles = new List<string>();
            //Obtengo la plantilla a usar
            SiPlantillacorreoDTO plantillaUsar = FactorySic.GetSiPlantillacorreoRepository().GetById(tipoCorreo);
            if (plantillaUsar == null)
            {
                throw new Exception("No se encontró una plantilla de correo para el tipo de correo elegido.");
            }

            //Obtengo los datos para completar las variables
            RePeriodoDTO periodo = FactorySic.GetRePeriodoRepository().GetById(repercodi);
            List<RePeriodoEtapaDTO> etapas = FactorySic.GetRePeriodoEtapaRepository().GetByPeriodo(repercodi);
            List<ReEmpresaDTO> listaEmpresasTotales = ObtenerEmpresasTotales();
            List<VariableCorreo> listaVariables = new List<VariableCorreo>();

            //Obtengo el tipo de correo
            int valTC = Int32.Parse(ObtenerTipoCorreo(tipoCorreo.ToString()));


            //Obtengo todas las empresas con correos registrados para resarcimientos
            List<SiEmpresaCorreoDTO> lstEmpresasCorreoResarcimientos = FactorySic.GetSiEmpresaCorreoRepository().ListarSoloResarcimiento();
            List<int> lstEmprcodisResarcimientos = lstEmpresasCorreoResarcimientos.Select(x => x.Emprcodi).Distinct().ToList();

            //Obtengo las empresas a la cual se enviaran los correos
            List<ReEmpresaDTO> listaEmpresasAEnviar = ObtenerEmpresasMostrar(repercodi, tipoCorreo, out bool muestraEmp, out string campo);

            //Obtengo las empresas que tienen correos registrados y a los que se enviaran los correos
            List<ReEmpresaDTO> lstEmpresasFinalEnviar = new List<ReEmpresaDTO>();
            if (tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar)
            {
                ReEmpresaDTO empresaSeleccionada = new ReEmpresaDTO();
                empresaSeleccionada.Emprcodi = idEmpresa;
                lstEmpresasFinalEnviar.Add(empresaSeleccionada);
                List<ReEmpresaDTO> lstEmpresaSelConCorreos = lstEmpresasFinalEnviar.Where(x => lstEmprcodisResarcimientos.Contains(x.Emprcodi)).ToList();
                if (!lstEmpresaSelConCorreos.Any())
                    throw new ArgumentException("La empresa suministradora seleccionada no tiene correos registrados. Registre o elija otra empresa.");
            }
            else
            {
                lstEmpresasFinalEnviar = listaEmpresasAEnviar.Where(x => lstEmprcodisResarcimientos.Contains(x.Emprcodi)).ToList();
            }


            if (tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones ||
                tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones ||
                tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudDecisionesControversia ||
                tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesTrimestral ||
                tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesSemestral ||
                tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar)
            {
                //Consolidado de interrupciones en los reportes
                List<InterrupcionSuministroPE> listaRegistrosPE_Final = new List<InterrupcionSuministroPE>();
                List<InterrupcionSuministroIntranet> listaInterrupcionesGeneral = new List<InterrupcionSuministroIntranet>();
                List<InterrupcionSuministroPE> lstSalidaReporteIntSinReportar = new List<InterrupcionSuministroPE>();

                if (tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones ||
                tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones)
                {
                    //datos para reporte de consolidado de interrupciones
                    List<EventoRC> listaEventosRC = servicioResarcimiento.ObtenerListaEventosPorPeriodo(periodo);
                    List<InterrupcionSuministroPE> listaRegistrosPE_F = servicioResarcimiento.ObtenerListaInterrupcionesPEPorPeriodo(periodo, false, null);
                    List<InterrupcionSuministroRC> listaRegistrosRC_F = servicioResarcimiento.ObtenerListaInterrupcionesRCPorPeriodo(periodo, false, null);
                    List<LimiteIngreso> lista_Limtx = servicioResarcimiento.ObtenerListaLimiteTransmisionPorPeriodo(periodo, listaRegistrosPE_F, listaRegistrosRC_F, listaEventosRC);
                    listaRegistrosPE_Final = servicioResarcimiento.ObtenerListaInterrupcionesPEPorPeriodo(periodo, true, lista_Limtx);

                    //Tanto para el reporte de interrrupciones por responsable y Para el reporte de interrupciones con Conformidad Responsable igual a NO
                    //Obtengo la lista genral de interrupciones
                    listaInterrupcionesGeneral = servicioResarcimiento.ObtenerListaInterrupcionIntranetPorPeriodo(periodo);

                }

                //Lista archivos reporte de interrupciones pendientes 
                if (tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar)
                {
                    if (idArchivoOriginal == 1) //no se adjunto, se usa el archivo original autogenerado
                    {
                        lstSalidaReporteIntSinReportar = servicioResarcimiento.ObtenerListadoContrasteInterrupcionesPorSuministrador(periodo, idEmpresa);
                    }
                    else //se usa archivo adjuntado
                    {
                        //Archivos
                        List<string> listaArchivos = new List<string>();
                        string files = correo.Archivos;
                        if (!string.IsNullOrEmpty(files))
                        {
                            listaArchivos = files.Split('/').ToList();
                        }

                        foreach (string file in listaArchivos)
                        {
                            if (!string.IsNullOrEmpty(file))
                            {                                
                                if (!listFiles.Contains(file))
                                {
                                    listFiles.Add( file);
                                }
                            }
                        }
                    }
                }

                foreach (ReEmpresaDTO empresa in lstEmpresasFinalEnviar)
                {
                    listaVariables = LlenarVariablesPorEmpresa(periodo, etapas, tipoCorreo, listaEmpresasTotales, empresa.Emprcodi);

                    SiCorreoDTO newCorreo = new SiCorreoDTO();

                    newCorreo.Corrto = ObtenerDestinatariosPorEmpresa("", empresa.Emprcodi);
                    newCorreo.Corrcc = correo.Corrcc;
                    newCorreo.Corrbcc = correo.Corrbcc;
                    newCorreo.Corrasunto = LLenarDatosVariables(plantillaUsar.Plantasunto, tipoCorreo, listaVariables);
                    newCorreo.Corrcontenido = LLenarDatosVariables(plantillaUsar.Plantcontenido, tipoCorreo, listaVariables);

                    if (newCorreo.Corrto != "" && newCorreo.Corrasunto != "" && newCorreo.Corrcontenido != "")
                    {
                        List<string> lstArchivos = new List<string>();
                        if (tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar)
                        {
                            if (idArchivoOriginal == 1)
                            {
                                lstArchivos = AdjuntarArchivosAutomaticamente(pathLogo, periodo, empresa, tipoCorreo, listaRegistrosPE_Final, listaInterrupcionesGeneral, lstSalidaReporteIntSinReportar);
                            }
                            else
                            {
                                lstArchivos = listFiles;
                            }
                                
                        }

                        else
                        {
                            lstArchivos = AdjuntarArchivosAutomaticamente(pathLogo, periodo, empresa, tipoCorreo, listaRegistrosPE_Final, listaInterrupcionesGeneral, lstSalidaReporteIntSinReportar);
                        }
                            

                        //LogCorreo
                        ReLogcorreoDTO lcorreo = new ReLogcorreoDTO();
                        lcorreo.Repercodi = repercodi;
                        lcorreo.Retcorcodi = valTC;
                        lcorreo.Relcorasunto = newCorreo.Corrasunto;
                        lcorreo.Relcorto = newCorreo.Corrto;
                        lcorreo.Relcorcc = newCorreo.Corrcc;
                        lcorreo.Relcorbcc = newCorreo.Corrbcc;
                        lcorreo.Relcorcuerpo = newCorreo.Corrcontenido;
                        lcorreo.Relcorusucreacion = usuario;
                        lcorreo.Relcorfeccreacion = DateTime.Now;
                        lcorreo.Relcorempresa = empresa.Emprcodi;
                        lcorreo.Relcorarchivosnomb = String.Join("/", lstArchivos);

                        EnviarMensajeAEmpresa(newCorreo, lcorreo, usuario, tipoCorreo, lstArchivos);
                    }

                }
            }



        }
       

        /// <summary>
        /// Devuelve la lista de suministradores pendientes de enviar interrupciones o con contraste
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public List<ReEmpresaDTO> ObtenerListadoSuministradoresConContrastreOInterrupcioesSinReportar(RePeriodoDTO periodo)
        {
            List<ReEmpresaDTO> lstSalida = new List<ReEmpresaDTO>();

            //Obtengo los suministradores con correos
            List<EmpresaCorreo> listaEmpresasCorreo = ListarEmpresasYSusCorreos();
            List<int> lstEmpresasConCorreos = listaEmpresasCorreo.Where(x => x.Emprcodi != null).Select(x => x.Emprcodi).Distinct().ToList();
            List<ReEmpresaDTO> lstSumTotales = servicioResarcimiento.ObtenerEmpresasSuministradorasTotal();
            List<ReEmpresaDTO> lstSumConCorreos = lstSumTotales.Where(x => lstEmpresasConCorreos.Contains(x.Emprcodi)).ToList();
            List<int> lstIdSuministradoresConCorreos = lstSumConCorreos.Where(x => x.Emprcodi != null).Select(x => x.Emprcodi).Distinct().ToList();

            //Obtengo suministradores del reporte
            List<InterrupcionSuministroPE> lstSalidaReporteIntSinReportar = servicioResarcimiento.ObtenerListadoContrasteInterrupcionesPorSuministrador(periodo, null);
            List<int> lstEmpresasReporte = lstSalidaReporteIntSinReportar.Where(x => x.SuministradorId != null).Select(x => x.SuministradorId).Distinct().ToList();

            List<int> idSumConCorreoYReporte = lstIdSuministradoresConCorreos.Intersect(lstEmpresasReporte).ToList();
            foreach (int idSum in idSumConCorreoYReporte)
            {
                ReEmpresaDTO reg1 = lstSumConCorreos.Find(x => x.Emprcodi == idSum);
                InterrupcionSuministroPE reg2 = lstSalidaReporteIntSinReportar.Find(x => x.SuministradorId == idSum);

                if(reg1 != null || reg2 != null)
                {
                    ReEmpresaDTO newReg = new ReEmpresaDTO();
                    newReg.Emprcodi = reg1 != null ? (reg1.Emprcodi) : reg2.SuministradorId;
                    newReg.Emprnomb = reg1 != null ? (reg1.Emprnomb) : reg2.Suministrador;

                    if(newReg.Emprcodi != null && newReg.Emprnomb != null)
                    {
                        lstSalida.Add(newReg);
                    }
                }
            }

            return lstSalida;
        }
        

        /// <summary>
        /// Devuelve el tipo de correo segun el id de la plantilla
        /// </summary>
        /// <param name="tipoCorreo"></param>
        /// <returns></returns>
        public string ObtenerTipoCorreo(string tipoCorreo)
        {
            string valTC = "";

            switch (tipoCorreo)
            {
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesTrimestral: valTC = "1"; break;
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesSemestral: valTC = "2"; break;
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones: valTC = "3"; break;
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones: valTC = "4"; break;
                case ConstantesCalidadProducto.IdPlantillaSolicitudDecisionesControversia: valTC = "5"; break;
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvíoCompensacionesMalaCalidadProducto: valTC = "6"; break;
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar: valTC = "7"; break;
                case "-1": valTC = ConstantesCalidadProducto.IdTipoCorreos;  break;
                  

            }

            return valTC;
        }

        /// <summary>
        /// Envia un correo a una empresa
        /// </summary>
        /// <param name="correo"></param>
        /// <param name="logcorreo"></param>
        /// <param name="usuario"></param>
        /// <param name="tipoCorreo"></param>
        /// <param name="lstDocumentos"></param>
        public void EnviarMensajeAEmpresa(SiCorreoDTO correo, ReLogcorreoDTO logcorreo, string usuario, int tipoCorreo, List<string> lstDocumentos)
        {

            IDbConnection conn = null;
            DbTransaction tran = null;
            int cbenvcodi = 0;

            string fromEmail = correo.Corrfrom ?? "";

            if (!fromEmail.Contains("@"))
                fromEmail = TipoPlantillaCorreo.MailFrom; //webapp@coes.org.pe
                                                          //fromEmail = "webapp@coes.org.pe";

            if (fromEmail != "")
            {
                List<string> toEmails = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(correo.Corrto, false);
                List<string> ccEmails = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(correo.Corrcc, false);
                List<string> bccEmails = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(correo.Corrbcc, true);

                string asunto = correo.Corrasunto;
                string contenido = correo.Corrcontenido;


                try
                {
                    conn = FactorySic.GetCbArchivoenvioRepository().BeginConnection();
                    tran = FactorySic.GetCbArchivoenvioRepository().StartTransaction(conn);

                    //Guardo SiCorreo
                    correo.Corrfechaenvio = DateTime.Now;
                    correo.Corrfechaperiodo = DateTime.Now;
                    correo.Plantcodi = tipoCorreo;
                    correo.Corrusuenvio = usuario;
                    int miCorrcodi = FactorySic.GetSiCorreoRepository().Save(correo);

                    //Guardo LogCorreo
                    int miRelCorcodi = FactorySic.GetReLogcorreoRepository().Save(logcorreo);

                    List<CbArchivoenvioDTO> lstArchivos = new List<CbArchivoenvioDTO>();
                    List<string> listFiles = new List<string>();
                    int num = 0;


                    var patTrabajo = ConstantesCalidadProducto.RutaCarpetaTempResarcimiento;
                    string nombreCarpeta = ConstantesCalidadProducto.NombreCarpetaTempResarcimiento;
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    //Generar los .dat
                    var rutaFinalArchivoDat = ruta + patTrabajo + nombreCarpeta + "\\";
                    string pathOrigen = rutaFinalArchivoDat;


                    //Copiamos en el fileserver
                    #region Guardar archivos en FileServer

                    int n = 0;  //variable  usado para eliminar correos en bd y evitar errores al copiar archivos al file server
                    foreach (var archivoNombre in lstDocumentos)
                    {
                        //if (!string.IsNullOrEmpty(regArchivo.Cbarchnombrefisico))
                        if (!string.IsNullOrEmpty(archivoNombre))
                        {
                            string pathAlternativo = GetPathPrincipal();
                            var pathDestino = GetPathSubcarpeta(ConstantesCombustibles.SubcarpetaArchivoAdjuntado) + miRelCorcodi + "\\";

                            var rutaF = pathAlternativo + pathDestino;

                            if (n == 0)
                            {
                                if (Directory.Exists(rutaF))
                                {
                                    Directory.Delete(rutaF, true);
                                }
                                FileServer.CreateFolder("", pathDestino, pathAlternativo);
                            }

                            n++;

                            var resultado = FileServer.CopiarFileAlter(pathOrigen, pathDestino, archivoNombre, pathAlternativo);

                            //Doy la ruta donde estan los archivos a adjuntar
                            string rutaP = pathAlternativo + pathDestino;

                            if (!listFiles.Contains(rutaP + archivoNombre))
                            {
                                listFiles.Add(rutaP + archivoNombre);
                            }



                            if (resultado != 1)
                            {
                                throw new ArgumentException(string.Format("Ocurrió un error cuando se copia el archivo {0} de {1} a {2}.", archivoNombre, pathOrigen, pathDestino));
                            }
                        }
                    }

                    #endregion

                    //Enviamos Correo
                    COES.Base.Tools.Util.SendEmail(toEmails, ccEmails, bccEmails, asunto, contenido, fromEmail, listFiles);

                    //guardar definitivamente
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    cbenvcodi = 0;
                    if (tran != null)
                        tran.Rollback();
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new ArgumentException(ex.Message);
                }
                finally
                {
                    if (conn != null)
                        if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

            #region Manejo de Archivos

            /// <summary>
            /// Permite obtener la carpeta principal de Intervenciones
            /// </summary>
            /// <returns></returns>
            public string GetPathPrincipal()
        {
            //- Definimos la carpeta raiz (termina con /)
            string pathRaiz = FileServer.GetDirectory();
            return pathRaiz;
        }

        /// <summary>
        /// Devuelve la ruta del fileserver donde se guardanlos correos
        /// </summary>
        /// <param name="subcarpeta"></param>
        /// <returns></returns>
        public static string GetPathSubcarpeta(string subcarpeta)
        {
            return ConstantesCalidadProducto.FolderRaizIntranetResarcimiento + subcarpeta + @"/";
        }
        #endregion

        /// <summary>
        ///  Devuelve la lista de archivos a adjuntar segun tipo de correo
        /// </summary>
        /// <param name="pathLogo"></param>
        /// <param name="periodo"></param>
        /// <param name="empresa"></param>
        /// <param name="tipoCorreo"></param>
        /// <param name="listaRegistrosPE"></param>
        /// <param name="listaInterrupcionesGeneral"></param>
        /// <returns></returns>
        public List<string> AdjuntarArchivosAutomaticamente(string pathLogo, RePeriodoDTO periodo, ReEmpresaDTO empresa, int tipoCorreo, List<InterrupcionSuministroPE> listaRegistrosPE, List<InterrupcionSuministroIntranet> listaInterrupcionesGeneral, List<InterrupcionSuministroPE> lstIntPendientesReportar)
        {
            List<string> lstSalida = new List<string>();

            if (tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones ||
                tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones || 
                tipoCorreo.ToString() == ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar)
            {
                //Crear carpeta para generar los excel
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                var patTrabajo = ConstantesCalidadProducto.RutaCarpetaTempResarcimiento;
                string nombreCarpeta = ConstantesCalidadProducto.NombreCarpetaTempResarcimiento;
                FileServer.DeleteFolderAlter(patTrabajo, ruta);
                FileServer.CreateFolder(patTrabajo, nombreCarpeta, ruta);

                var rutaFinalArchivos = ruta + patTrabajo + nombreCarpeta + "\\";

                //Nombre Archivo consolidado
                string NombreArchivoConsolidado = "Consolidado_Interrupciones_Suministro.xlsx";

                switch (tipoCorreo.ToString())
                {
                    #region Solicitud de envío de observaciones a las interrupciones - plantcodi : 176

                    //Se adjuntan el consolidado de Interrupciones (solo por punto de entrega) e interrupciones donde el agente es responsable
                    case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones:

                        //Consolidado
                        GenerarExcelConsolidado(periodo, NombreArchivoConsolidado, listaRegistrosPE, rutaFinalArchivos, pathLogo);
                        lstSalida.Add(NombreArchivoConsolidado);

                        //Interrupciones por responsable
                        List<InterrupcionSuministroIntranet> listaPorAgente = listaInterrupcionesGeneral.Where(x => x.Responsable1Id == empresa.Emprcodi || x.Responsable2Id == empresa.Emprcodi
                                                                            || x.Responsable3Id == empresa.Emprcodi || x.Responsable4Id == empresa.Emprcodi || x.Responsable5Id == empresa.Emprcodi).ToList();
                        string nombreArchivo = "InterrupcionesPorAgenteResponsable.xlsx";
                        GenerarExcelAAndjuntar(periodo, listaPorAgente, nombreArchivo, rutaFinalArchivos, pathLogo, tipoCorreo);
                        lstSalida.Add(nombreArchivo);
                        break;

                    #endregion

                    #region Solicitud de envío de respuestas a las observaciones - plantcodi : 177

                    case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones:

                        //Consolidado
                        GenerarExcelConsolidado(periodo, NombreArchivoConsolidado, listaRegistrosPE, rutaFinalArchivos, pathLogo);
                        lstSalida.Add(NombreArchivoConsolidado);

                        //Interrupciones que tengan Conformidad Responsable igual a NO                    
                        List<InterrupcionSuministroIntranet> lista = new List<InterrupcionSuministroIntranet>();
                        List<InterrupcionSuministroIntranet> listaGeneralInterrupcionesPorSuministrador = listaInterrupcionesGeneral.Where(x => x.SuministradorId == empresa.Emprcodi).ToList();
                        foreach (var regInterrupcion in listaGeneralInterrupcionesPorSuministrador)
                        {
                            if (regInterrupcion.RegistroEnControversiaResp)
                                lista.Add(regInterrupcion);
                        }
                        string nombreArchivo2 = "ConformidadResponsableNoPorSuministrador.xlsx";
                        GenerarExcelAAndjuntar(periodo, lista, nombreArchivo2, rutaFinalArchivos, pathLogo, tipoCorreo);
                        lstSalida.Add(nombreArchivo2);

                        break;

                    #endregion

                    #region Solicitud de interrupciones pendientes de reportar - 320

                    case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar:

                        string NombreArchivoRep = "Interrupciones_Pendientes_Reportar.xlsx";
                        //Generar Excel
                        GenerarExcelIterrupcionesPendientesReportar(periodo, NombreArchivoRep, lstIntPendientesReportar, rutaFinalArchivos, pathLogo);                        ;
                        lstSalida.Add(NombreArchivoRep);
                        
                        break;

                        #endregion

                }
            }
          

            return lstSalida;
        }

        /// <summary>
        /// Genera el excel del reporte de interrupciones sin reportar
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="nameFile"></param>
        /// <param name="idSuministrador"></param> 
        /// <param name="idPeriodo"></param>
        public void GenerarReporteIntrrupcionesSinReportar(string ruta, string pathLogo, string nameFile, int idSuministrador, int idPeriodo)
        {
            RePeriodoDTO periodo = FactorySic.GetRePeriodoRepository().GetById(idPeriodo);
            List<InterrupcionSuministroPE> lstSalidaReporte = servicioResarcimiento.ObtenerListadoContrasteInterrupcionesPorSuministrador(periodo, idSuministrador);

            GenerarExcelIterrupcionesPendientesReportar(periodo, nameFile, lstSalidaReporte, ruta, pathLogo);

        }

        /// <summary>
        /// Genera un archivo excel del reporte
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="lista"></param>
        /// <param name="nombArchivo"></param>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="tipoCorreo"></param>
        public void GenerarExcelAAndjuntar(RePeriodoDTO periodo, List<InterrupcionSuministroIntranet> lista, string nombArchivo, string ruta, string pathLogo, int tipoCorreo)
        {
            ////Descargo archivo segun requieran
            string rutaFile = ruta + nombArchivo;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            string titulo = "";
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                switch (tipoCorreo.ToString())
                {
                    
                    case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones:
                        titulo = "Interrupciones por Agente Responsable";
                        break;

                    case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones:
                        titulo = "Interrupciones por Suministrador";
                        break;                 
                }

                GenerarArchivoExcelInterrupcionesAAdjuntar(xlPackage, pathLogo, lista, titulo, periodo);
                xlPackage.Save();
            }
        }

        ///// <summary>
        ///// Genera un archivo excel del reporte donde las interrupciones tienen Conformidad responsable = NO, por suministrador
        ///// </summary>
        ///// <param name="periodo"></param>
        ///// <param name="listaPorSuministrador"></param>
        ///// <param name="nombreAgente"></param>
        ///// <param name="ruta"></param>
        ///// <param name="pathLogo"></param>
        //public void GenerarExcelControversiaPorAgenteResponsable(RePeriodoDTO periodo, List<InterrupcionSuministroIntranet> listaPorSuministrador, string nombreAgente, string ruta, string pathLogo)
        //{
        //    ////Descargo archivo segun requieran
        //    string rutaFile = ruta + nombreAgente;

        //    FileInfo newFile = new FileInfo(rutaFile);
        //    if (newFile.Exists)
        //    {
        //        newFile.Delete();
        //        newFile = new FileInfo(rutaFile);
        //    }

        //    using (ExcelPackage xlPackage = new ExcelPackage(newFile))
        //    {
        //        GenerarArchivoExcelInterrupcionesAAdjuntar(xlPackage, pathLogo, listaPorAgente, nombreAgente, periodo);

        //        xlPackage.Save();
        //    }
        //}

        /// <summary>
        /// Genera el reporte Interrupciones que seran adjuntados en los correos
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="listaPorAgente"></param>
        /// <param name="nombreAgente"></param>
        /// <param name="periodo"></param>
        private void GenerarArchivoExcelInterrupcionesAAdjuntar(ExcelPackage xlPackage, string pathLogo, List<InterrupcionSuministroIntranet> listaPorAgente, string titulo, RePeriodoDTO periodo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<InterrupcionSuministroIntranet> lista = new List<InterrupcionSuministroIntranet>();

            lista = listaPorAgente;

            int numResponsables = 5;

            string nameWS = "Listado";
            
            string strPeriodo = (periodo.Repertipo == "T" ? "Trimestral" : (periodo.Repertipo == "S" ? "Semestral" : "")) + " " + periodo.Reperanio + " ( " + periodo.Repernombre + ")";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera 

            int colIniTitulo = 1;
            int rowIniTitulo = 3;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 3;

            ws.Row(rowIniTabla).Height = 30;
            ws.Row(rowIniTabla + 1).Height = 30;

            int colSuministrador = colIniTable;
            int colCorrelativo = colIniTable + 1;
            int colTipoCliente = colIniTable + 2;
            int colNombreCliente = colIniTable + 3;
            int colPuntoEntregaBarraNombre = colIniTable + 4;
            int colNumSuministroClienteLibre = colIniTable + 5;
            int colNivelTensionNombre = colIniTable + 6;
            int colAplicacionLiteral = colIniTable + 7;
            int colEnergiaPeriodo = colIniTable + 8;
            int colIncrementoTolerancia = colIniTable + 9;
            int colTipoNombre = colIniTable + 10;
            int colCausaNombre = colIniTable + 11;
            int colNi = colIniTable + 12;
            int colKi = colIniTable + 13;
            int colTiempoEjecutadoIni = colIniTable + 14;
            int colTiempoEjecutadoFin = colIniTable + 15;
            int colTiempoProgramadoIni = colIniTable + 16;
            int colTiempoProgramadoFin = colIniTable + 17;
            int colResponsable1Nombre = colIniTable + 18;
            int colResponsable1Porcentaje = colIniTable + 19;
            int colResponsable2Nombre = colIniTable + 20;
            int colResponsable2Porcentaje = colIniTable + 21;
            int colResponsable3Nombre = colIniTable + 22;
            int colResponsable3Porcentaje = colIniTable + 23;
            int colResponsable4Nombre = colIniTable + 24;
            int colResponsable4Porcentaje = colIniTable + 25;
            int colResponsable5Nombre = colIniTable + 26;
            int colResponsable5Porcentaje = colIniTable + 27;
            int colCausaResumida = colIniTable + 28;
            int colEiE = colIniTable + 29;
            int colResarcimiento = colIniTable + 30;

            int colResp1ConformidadResponsable = colIniTable + 31;
            int colResp1Observacion = colIniTable + 32;
            int colResp1DetalleObservacion = colIniTable + 33;
            int colResp1Comentario1 = colIniTable + 34;
            int colResp1ConformidadSuministrador = colIniTable + 35;
            int colResp1Comentario2 = colIniTable + 36;

            int colResp2ConformidadResponsable = colIniTable + 37;
            int colResp2Observacion = colIniTable + 38;
            int colResp2DetalleObservacion = colIniTable + 39;
            int colResp2Comentario1 = colIniTable + 40;
            int colResp2ConformidadSuministrador = colIniTable + 41;
            int colResp2Comentario2 = colIniTable + 42;

            int colResp3ConformidadResponsable = colIniTable + 43;
            int colResp3Observacion = colIniTable + 44;
            int colResp3DetalleObservacion = colIniTable + 45;
            int colResp3Comentario1 = colIniTable + 46;
            int colResp3ConformidadSuministrador = colIniTable + 47;
            int colResp3Comentario2 = colIniTable + 48;

            int colResp4ConformidadResponsable = colIniTable + 49;
            int colResp4Observacion = colIniTable + 50;
            int colResp4DetalleObservacion = colIniTable + 51;
            int colResp4Comentario1 = colIniTable + 52;
            int colResp4ConformidadSuministrador = colIniTable + 53;
            int colResp4Comentario2 = colIniTable + 54;

            int colResp5ConformidadResponsable = colIniTable + 55;
            int colResp5Observacion = colIniTable + 56;
            int colResp5DetalleObservacion = colIniTable + 57;
            int colResp5Comentario1 = colIniTable + 58;
            int colResp5ConformidadSuministrador = colIniTable + 59;
            int colResp5Comentario2 = colIniTable + 60;

            int colDesicionControversia = colIniTable + 61;
            int colComentarioFinal = colIniTable + 62;

            switch (numResponsables)
            {
                case 1:
                    colDesicionControversia = colIniTable + 37;
                    colComentarioFinal = colIniTable + 38;
                    break;
                case 2:
                    colDesicionControversia = colIniTable + 43;
                    colComentarioFinal = colIniTable + 44;
                    break;
                case 3:
                    colDesicionControversia = colIniTable + 49;
                    colComentarioFinal = colIniTable + 50;
                    break;
                case 4:
                    colDesicionControversia = colIniTable + 55;
                    colComentarioFinal = colIniTable + 56;
                    break;
                case 5:
                    colDesicionControversia = colIniTable + 61;
                    colComentarioFinal = colIniTable + 62;
                    break;
            }


            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniTitulo + 1, colIniTitulo].Value = strPeriodo;
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento, "Calibri", 13);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Calibri", 11);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento);

            ws.Cells[rowIniTabla, colSuministrador].Value = "Suministrador";
            ws.Cells[rowIniTabla, colCorrelativo].Value = "Correlativo por Punto de Entrega";
            ws.Cells[rowIniTabla, colTipoCliente].Value = "Tipo de Cliente";
            ws.Cells[rowIniTabla, colNombreCliente].Value = "Cliente";
            ws.Cells[rowIniTabla, colPuntoEntregaBarraNombre].Value = "Punto de Entrega/Barra";
            ws.Cells[rowIniTabla, colNumSuministroClienteLibre].Value = "N° de suministro cliente libre";
            ws.Cells[rowIniTabla, colNivelTensionNombre].Value = "Nivel de Tensión";
            ws.Cells[rowIniTabla, colAplicacionLiteral].Value = "Aplicación literal e) de numeral 5.2.4 Base Metodológica (meses de suministro en el semestre)";
            ws.Cells[rowIniTabla, colEnergiaPeriodo].Value = "Energía Semestral (kWh)";
            ws.Cells[rowIniTabla, colIncrementoTolerancia].Value = "Incremento de tolerancias Sector Distribución Típico 2 (Mercado regulado)";
            ws.Cells[rowIniTabla, colTipoNombre].Value = "Tipo";
            ws.Cells[rowIniTabla, colCausaNombre].Value = "Causa";
            ws.Cells[rowIniTabla, colNi].Value = "Ni";
            ws.Cells[rowIniTabla, colKi].Value = "Ki";
            ws.Cells[rowIniTabla, colTiempoEjecutadoIni].Value = "Tiempo Ejecutado";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colTiempoProgramadoIni].Value = "Tiempo Programado";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colResponsable1Nombre].Value = "Responsable 1";
            ws.Cells[rowIniTabla + 1, colResponsable1Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable1Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable2Nombre].Value = "Responsable 2";
            ws.Cells[rowIniTabla + 1, colResponsable2Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable2Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable3Nombre].Value = "Responsable 3";
            ws.Cells[rowIniTabla + 1, colResponsable3Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable3Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable4Nombre].Value = "Responsable 4";
            ws.Cells[rowIniTabla + 1, colResponsable4Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable4Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable5Nombre].Value = "Responsable 5";
            ws.Cells[rowIniTabla + 1, colResponsable5Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable5Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colCausaResumida].Value = "Causa resumida de interrupción";
            ws.Cells[rowIniTabla, colEiE].Value = "Ei / E";
            ws.Cells[rowIniTabla, colResarcimiento].Value = "Resarcimiento (US$)";

            if (numResponsables >= 1)
            {
                ws.Cells[rowIniTabla, colResp1ConformidadResponsable].Value = "Responsable 1";
                ws.Cells[rowIniTabla + 1, colResp1ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp1Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp1DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp1Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp1ConformidadSuministrador].Value = "Conformidad Responsable 1";
                ws.Cells[rowIniTabla + 1, colResp1ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp1Comentario2].Value = "Comentarios";
            }

            if (numResponsables >= 2)
            {
                ws.Cells[rowIniTabla, colResp2ConformidadResponsable].Value = "Responsable 2";
                ws.Cells[rowIniTabla + 1, colResp2ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp2Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp2DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp2Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp2ConformidadSuministrador].Value = "Conformidad Responsable 2";
                ws.Cells[rowIniTabla + 1, colResp2ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp2Comentario2].Value = "Comentarios";
            }

            if (numResponsables >= 3)
            {
                ws.Cells[rowIniTabla, colResp3ConformidadResponsable].Value = "Responsable 3";
                ws.Cells[rowIniTabla + 1, colResp3ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp3Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp3DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp3Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp3ConformidadSuministrador].Value = "Conformidad Responsable 3";
                ws.Cells[rowIniTabla + 1, colResp3ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp3Comentario2].Value = "Comentarios";
            }

            if (numResponsables >= 4)
            {
                ws.Cells[rowIniTabla, colResp4ConformidadResponsable].Value = "Responsable 4";
                ws.Cells[rowIniTabla + 1, colResp4ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp4Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp4DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp4Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp4ConformidadSuministrador].Value = "Conformidad Responsable 4";
                ws.Cells[rowIniTabla + 1, colResp4ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp4Comentario2].Value = "Comentarios";
            }

            if (numResponsables == 5)
            {
                ws.Cells[rowIniTabla, colResp5ConformidadResponsable].Value = "Responsable 5";
                ws.Cells[rowIniTabla + 1, colResp5ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp5Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp5DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp5Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp5ConformidadSuministrador].Value = "Conformidad Responsable 5";
                ws.Cells[rowIniTabla + 1, colResp5ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp5Comentario2].Value = "Comentarios";
            }

            ws.Cells[rowIniTabla, colDesicionControversia].Value = "Decisión de Controveria";
            ws.Cells[rowIniTabla, colComentarioFinal].Value = "Comentarios";

            //Estilos cabecera
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colSuministrador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCorrelativo, rowIniTabla + 1, colCorrelativo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoCliente, rowIniTabla + 1, colTipoCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNombreCliente, rowIniTabla + 1, colNombreCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colPuntoEntregaBarraNombre, rowIniTabla + 1, colPuntoEntregaBarraNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNumSuministroClienteLibre, rowIniTabla + 1, colNumSuministroClienteLibre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNivelTensionNombre, rowIniTabla + 1, colNivelTensionNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionLiteral, rowIniTabla + 1, colAplicacionLiteral);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEnergiaPeriodo, rowIniTabla + 1, colEnergiaPeriodo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colIncrementoTolerancia, rowIniTabla + 1, colIncrementoTolerancia);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoNombre, rowIniTabla + 1, colTipoNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaNombre, rowIniTabla + 1, colCausaNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNi, rowIniTabla + 1, colNi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colKi, rowIniTabla + 1, colKi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoEjecutadoIni, rowIniTabla, colTiempoEjecutadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoProgramadoIni, rowIniTabla, colTiempoProgramadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable1Nombre, rowIniTabla, colResponsable1Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable2Nombre, rowIniTabla, colResponsable2Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable3Nombre, rowIniTabla, colResponsable3Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable4Nombre, rowIniTabla, colResponsable4Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable5Nombre, rowIniTabla, colResponsable5Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaResumida, rowIniTabla + 1, colCausaResumida);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEiE, rowIniTabla + 1, colEiE);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResarcimiento, rowIniTabla + 1, colResarcimiento);

            if (numResponsables >= 1)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp1ConformidadResponsable, rowIniTabla, colResp1Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp1ConformidadSuministrador, rowIniTabla, colResp1Comentario2);
            }

            if (numResponsables >= 2)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp2ConformidadResponsable, rowIniTabla, colResp2Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp2ConformidadSuministrador, rowIniTabla, colResp2Comentario2);
            }

            if (numResponsables >= 3)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp3ConformidadResponsable, rowIniTabla, colResp3Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp3ConformidadSuministrador, rowIniTabla, colResp3Comentario2);
            }

            if (numResponsables >= 4)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp4ConformidadResponsable, rowIniTabla, colResp4Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp4ConformidadSuministrador, rowIniTabla, colResp4Comentario2);
            }

            if (numResponsables == 5)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp5ConformidadResponsable, rowIniTabla, colResp5Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp5ConformidadSuministrador, rowIniTabla, colResp5Comentario2);
            }


            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colDesicionControversia, rowIniTabla + 1, colDesicionControversia);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colComentarioFinal, rowIniTabla + 1, colComentarioFinal);


            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "Calibri", 9);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "#1659AA");
            servFormato.CeldasExcelWrapText(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal);
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "#FFFFFF");
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal);

            if (numResponsables >= 1)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp1ConformidadResponsable, rowIniTabla + 1, colResp1Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp1ConformidadResponsable, rowIniTabla + 1, colResp1Comentario1, "#000000");
            }

            if (numResponsables >= 2)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp2ConformidadResponsable, rowIniTabla + 1, colResp2Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp2ConformidadResponsable, rowIniTabla + 1, colResp2Comentario1, "#000000");
            }

            if (numResponsables >= 3)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp3ConformidadResponsable, rowIniTabla + 1, colResp3Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp3ConformidadResponsable, rowIniTabla + 1, colResp3Comentario1, "#000000");
            }

            if (numResponsables >= 4)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp4ConformidadResponsable, rowIniTabla + 1, colResp4Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp4ConformidadResponsable, rowIniTabla + 1, colResp4Comentario1, "#000000");
            }

            if (numResponsables >= 5)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp5ConformidadResponsable, rowIniTabla + 1, colResp5Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp5ConformidadResponsable, rowIniTabla + 1, colResp5Comentario1, "#000000");
            }
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 2;

            foreach (var item in lista)
            {
                ws.Cells[rowData, colSuministrador].Value = item.Suministrador != null ? item.Suministrador.Trim() : null;
                ws.Cells[rowData, colCorrelativo].Value = item.Correlativo != null ? item.Correlativo.Value : item.Correlativo;
                ws.Cells[rowData, colTipoCliente].Value = item.TipoCliente != null ? item.TipoCliente.Trim() : null;
                ws.Cells[rowData, colNombreCliente].Value = item.NombreCliente != null ? item.NombreCliente.Trim() : null;
                ws.Cells[rowData, colPuntoEntregaBarraNombre].Value = item.PuntoEntregaBarraNombre != null ? item.PuntoEntregaBarraNombre.Trim() : null;
                ws.Cells[rowData, colNumSuministroClienteLibre].Value = item.NumSuministroClienteLibre != null ? item.NumSuministroClienteLibre : item.NumSuministroClienteLibre;
                ws.Cells[rowData, colNivelTensionNombre].Value = item.NivelTensionNombre != null ? item.NivelTensionNombre.Trim() : null;
                ws.Cells[rowData, colAplicacionLiteral].Value = item.AplicacionLiteral != null ? item.AplicacionLiteral.Value : item.AplicacionLiteral;
                ws.Cells[rowData, colEnergiaPeriodo].Value = item.EnergiaPeriodo != null ? item.EnergiaPeriodo.Value : item.EnergiaPeriodo;
                ws.Cells[rowData, colEnergiaPeriodo].Style.Numberformat.Format = servicioResarcimiento.FormatoNumDecimales(2);
                ws.Cells[rowData, colIncrementoTolerancia].Value = item.IncrementoTolerancia != null ? item.IncrementoTolerancia.Trim() : null;
                ws.Cells[rowData, colTipoNombre].Value = item.TipoNombre != null ? item.TipoNombre.Trim() : null;
                ws.Cells[rowData, colCausaNombre].Value = item.CausaNombre != null ? item.CausaNombre.Trim() : null;
                ws.Cells[rowData, colNi].Value = item.Ni != null ? item.Ni.Value : item.Ni;
                ws.Cells[rowData, colNi].Style.Numberformat.Format = servicioResarcimiento.FormatoNumDecimales(2);
                ws.Cells[rowData, colKi].Value = item.Ki != null ? item.Ki.Value : item.Ki;
                ws.Cells[rowData, colKi].Style.Numberformat.Format = servicioResarcimiento.FormatoNumDecimales(2);
                ws.Cells[rowData, colTiempoEjecutadoIni].Value = item.TiempoEjecutadoIni != null ? item.TiempoEjecutadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoFin].Value = item.TiempoEjecutadoFin != null ? item.TiempoEjecutadoFin.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoIni].Value = item.TiempoProgramadoIni != null ? item.TiempoProgramadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoFin].Value = item.TiempoProgramadoFin != null ? item.TiempoProgramadoFin.Trim() : null;
                ws.Cells[rowData, colResponsable1Nombre].Value = item.Responsable1Nombre != null ? item.Responsable1Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable1Porcentaje].Value = item.Responsable1Porcentaje != null ? item.Responsable1Porcentaje.Value / 100 : item.Responsable1Porcentaje;
                ws.Cells[rowData, colResponsable1Porcentaje].Style.Numberformat.Format = servicioResarcimiento.FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable2Nombre].Value = item.Responsable2Nombre != null ? item.Responsable2Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable2Porcentaje].Value = item.Responsable2Porcentaje != null ? item.Responsable2Porcentaje.Value / 100 : item.Responsable2Porcentaje;
                ws.Cells[rowData, colResponsable2Porcentaje].Style.Numberformat.Format = servicioResarcimiento.FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable3Nombre].Value = item.Responsable3Nombre != null ? item.Responsable3Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable3Porcentaje].Value = item.Responsable3Porcentaje != null ? item.Responsable3Porcentaje.Value / 100 : item.Responsable3Porcentaje;
                ws.Cells[rowData, colResponsable3Porcentaje].Style.Numberformat.Format = servicioResarcimiento.FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable4Nombre].Value = item.Responsable4Nombre != null ? item.Responsable4Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable4Porcentaje].Value = item.Responsable4Porcentaje != null ? item.Responsable4Porcentaje.Value / 100 : item.Responsable4Porcentaje;
                ws.Cells[rowData, colResponsable4Porcentaje].Style.Numberformat.Format = servicioResarcimiento.FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable5Nombre].Value = item.Responsable5Nombre != null ? item.Responsable5Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable5Porcentaje].Value = item.Responsable5Porcentaje != null ? item.Responsable5Porcentaje.Value / 100 : item.Responsable5Porcentaje;
                ws.Cells[rowData, colResponsable5Porcentaje].Style.Numberformat.Format = servicioResarcimiento.FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colCausaResumida].Value = item.CausaResumida != null ? item.CausaResumida.Trim() : null;
                ws.Cells[rowData, colEiE].Value = item.EiE != null ? item.EiE.Value : item.EiE;
                ws.Cells[rowData, colEiE].Style.Numberformat.Format = servicioResarcimiento.FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResarcimiento].Value = item.Resarcimiento != null ? item.Resarcimiento.Value : item.Resarcimiento;
                ws.Cells[rowData, colResarcimiento].Style.Numberformat.Format = servicioResarcimiento.FormatoNumDecimales(2);

                if (numResponsables >= 1)
                {
                    ws.Cells[rowData, colResp1ConformidadResponsable].Value = item.Resp1ConformidadResponsable != null ? item.Resp1ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp1Observacion].Value = item.Resp1Observacion != null ? item.Resp1Observacion.Trim() : null;
                    ws.Cells[rowData, colResp1DetalleObservacion].Value = item.Resp1DetalleObservacion != null ? item.Resp1DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp1Comentario1].Value = item.Resp1Comentario1 != null ? item.Resp1Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp1ConformidadSuministrador].Value = item.Resp1ConformidadSuministrador != null ? item.Resp1ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp1Comentario2].Value = item.Resp1Comentario2 != null ? item.Resp1Comentario2.Trim() : null;
                }

                if (numResponsables >= 2)
                {
                    ws.Cells[rowData, colResp2ConformidadResponsable].Value = item.Resp2ConformidadResponsable != null ? item.Resp2ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp2Observacion].Value = item.Resp2Observacion != null ? item.Resp2Observacion.Trim() : null;
                    ws.Cells[rowData, colResp2DetalleObservacion].Value = item.Resp2DetalleObservacion != null ? item.Resp2DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp2Comentario1].Value = item.Resp2Comentario1 != null ? item.Resp2Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp2ConformidadSuministrador].Value = item.Resp2ConformidadSuministrador != null ? item.Resp2ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp2Comentario2].Value = item.Resp2Comentario2 != null ? item.Resp2Comentario2.Trim() : null;
                }

                if (numResponsables >= 3)
                {
                    ws.Cells[rowData, colResp3ConformidadResponsable].Value = item.Resp3ConformidadResponsable != null ? item.Resp3ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp3Observacion].Value = item.Resp3Observacion != null ? item.Resp3Observacion.Trim() : null;
                    ws.Cells[rowData, colResp3DetalleObservacion].Value = item.Resp3DetalleObservacion != null ? item.Resp3DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp3Comentario1].Value = item.Resp3Comentario1 != null ? item.Resp3Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp3ConformidadSuministrador].Value = item.Resp3ConformidadSuministrador != null ? item.Resp3ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp3Comentario2].Value = item.Resp3Comentario2 != null ? item.Resp3Comentario2.Trim() : null;
                }

                if (numResponsables >= 4)
                {
                    ws.Cells[rowData, colResp4ConformidadResponsable].Value = item.Resp4ConformidadResponsable != null ? item.Resp4ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp4Observacion].Value = item.Resp4Observacion != null ? item.Resp4Observacion.Trim() : null;
                    ws.Cells[rowData, colResp4DetalleObservacion].Value = item.Resp4DetalleObservacion != null ? item.Resp4DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp4Comentario1].Value = item.Resp4Comentario1 != null ? item.Resp4Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp4ConformidadSuministrador].Value = item.Resp4ConformidadSuministrador != null ? item.Resp4ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp4Comentario2].Value = item.Resp4Comentario2 != null ? item.Resp4Comentario2.Trim() : null;
                }

                if (numResponsables == 5)
                {
                    ws.Cells[rowData, colResp5ConformidadResponsable].Value = item.Resp5ConformidadResponsable != null ? item.Resp5ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp5Observacion].Value = item.Resp5Observacion != null ? item.Resp5Observacion.Trim() : null;
                    ws.Cells[rowData, colResp5DetalleObservacion].Value = item.Resp5DetalleObservacion != null ? item.Resp5DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp5Comentario1].Value = item.Resp5Comentario1 != null ? item.Resp5Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp5ConformidadSuministrador].Value = item.Resp5ConformidadSuministrador != null ? item.Resp5ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp5Comentario2].Value = item.Resp5Comentario2 != null ? item.Resp5Comentario2.Trim() : null;
                }

                ws.Cells[rowData, colDesicionControversia].Value = item.DesicionControversia != null ? item.DesicionControversia.Trim() : null;
                ws.Cells[rowData, colComentarioFinal].Value = item.ComentarioFinal != null ? item.ComentarioFinal.Trim() : null;


                rowData++;
            }
            if (!lista.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 2, colSuministrador, rowData - 1, colComentarioFinal, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colSuministrador, rowData - 1, colComentarioFinal);
            #endregion

            //filter                       

            if (lista.Any())
            {

                ws.Cells[rowIniTabla, colSuministrador, rowData, colComentarioFinal].AutoFitColumns(12.5);
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 14;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 18;
                ws.Column(colTiempoEjecutadoFin).Width = 18;
                ws.Column(colTiempoProgramadoIni).Width = 18;
                ws.Column(colTiempoProgramadoFin).Width = 18;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
                if (numResponsables >= 1) ws.Column(colResp1ConformidadResponsable).Width = 12;
                if (numResponsables >= 1) ws.Column(colResp1ConformidadSuministrador).Width = 13;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadResponsable).Width = 12;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadSuministrador).Width = 13;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadResponsable).Width = 12;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadSuministrador).Width = 13;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadResponsable).Width = 12;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadSuministrador).Width = 13;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadResponsable).Width = 12;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadSuministrador).Width = 13;
            }
            else
            {
                ws.Column(colSuministrador).Width = 20;
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNombreCliente).Width = 12;
                ws.Column(colPuntoEntregaBarraNombre).Width = 22;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 12;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colTipoNombre).Width = 10;
                ws.Column(colCausaNombre).Width = 10;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 13;
                ws.Column(colTiempoEjecutadoFin).Width = 12;
                ws.Column(colTiempoProgramadoIni).Width = 13;
                ws.Column(colTiempoProgramadoFin).Width = 12;
                ws.Column(colResponsable1Nombre).Width = 8;
                ws.Column(colResponsable1Porcentaje).Width = 5;
                ws.Column(colResponsable2Nombre).Width = 8;
                ws.Column(colResponsable2Porcentaje).Width = 5;
                ws.Column(colResponsable3Nombre).Width = 8;
                ws.Column(colResponsable3Porcentaje).Width = 5;
                ws.Column(colResponsable4Nombre).Width = 8;
                ws.Column(colResponsable4Porcentaje).Width = 5;
                ws.Column(colResponsable5Nombre).Width = 8;
                ws.Column(colResponsable5Porcentaje).Width = 5;
                ws.Column(colCausaResumida).Width = 27;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;

                if (numResponsables >= 1) ws.Column(colResp1ConformidadResponsable).Width = 12;
                if (numResponsables >= 1) ws.Column(colResp1ConformidadSuministrador).Width = 13;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadResponsable).Width = 12;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadSuministrador).Width = 13;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadResponsable).Width = 12;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadSuministrador).Width = 13;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadResponsable).Width = 12;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadSuministrador).Width = 13;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadResponsable).Width = 12;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadSuministrador).Width = 13;

            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 2, 1);
        }

        /// <summary>
        /// Genera un archivo excel del consolidado de interrupciones
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="listaRegistrosPE"></param>
        /// <param name="rutaDondeGuardaArchivo"></param>
        /// <param name="pathLogo"></param>
        public void GenerarExcelConsolidado(RePeriodoDTO periodo, string nombreArchivo, List<InterrupcionSuministroPE> listaRegistrosPE, string rutaDondeGuardaArchivo, string pathLogo)
        {            
            ////Descargo archivo segun requieran
            string rutaFile = rutaDondeGuardaArchivo + nombreArchivo;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {                
                servicioResarcimiento.GenerarArchivoExcelConsolidadoInterrupcionesSuministro_PE(xlPackage, pathLogo, listaRegistrosPE);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera el excel del reporte de interrfciones sin reportar
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="lstIntPendientesReportar"></param>
        /// <param name="rutaDondeGuardaArchivo"></param>
        /// <param name="pathLogo"></param>
        public void GenerarExcelIterrupcionesPendientesReportar(RePeriodoDTO periodo, string nombreArchivo, List<InterrupcionSuministroPE> lstIntPendientesReportar, string rutaDondeGuardaArchivo, string pathLogo)
        {
            ////Descargo archivo segun requieran
            string rutaFile = rutaDondeGuardaArchivo + nombreArchivo;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                servicioResarcimiento.GenerarArchivoExcelContrasteInterrupcionesSuministro(xlPackage, pathLogo, periodo, lstIntPendientesReportar);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Develve una lista de variables con los datos llenados
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="etapas"></param>
        /// <param name="tipoCorreo"></param>
        /// <param name="listaEmpresas"></param>
        /// <param name="responsableGral"></param>
        /// <returns></returns>
        public List<VariableCorreo> LlenarVariablesPorEmpresa(RePeriodoDTO periodo, List<RePeriodoEtapaDTO> etapas, int tipoCorreo, List<ReEmpresaDTO> listaEmpresas, int? responsableGral)
        {

            List<VariableCorreo> listaVariables = ObtenerListadoVariables(tipoCorreo, ConstantesCalidadProducto.VariableAsunto);

            string numPeriodo = servicioResarcimiento.ObtenerNumeroPeriodo(periodo);
            string tipoPeriodo = periodo.Repertipo == "T" ? "Trimestre" : (periodo.Repertipo == "S" ? "Semestre" : "");
            int anioPeriodo = periodo.Reperanio.Value;            
            string nLink = periodo.Repertipo == "T" ? "03" : (numPeriodo == "Primer" ? "01" : (numPeriodo == "Segundo" ? "02" : ""));

            string strPeriodo = numPeriodo + " " + tipoPeriodo + " " + anioPeriodo;
            string strMesesPeriodo = EPDate.f_NombreMes(periodo.Reperfecinicio.Value.Month) + " a " + EPDate.f_NombreMes(periodo.Reperfecfin.Value.Month);
            string strAnioPeriodo = anioPeriodo.ToString();
            string strNumPeriodo = nLink + "_" + numPeriodo.ToUpper() + " " + tipoPeriodo.ToUpper();
            string strMesFinal = EPDate.f_NombreMes(periodo.Reperfecfin.Value.Month);
            string strNombPeriodo = periodo.Repernombre;
            string strResponsable = responsableGral != null ? (responsableGral != ConstantesCalidadProducto.IdEmpresaNoExistente ? listaEmpresas.Find(x => x.Emprcodi == responsableGral).Emprnomb.Trim() : "") : "";

            RePeriodoEtapaDTO etapa1 = etapas.Find(x => x.Reetacodi == 1);
            if(etapa1 == null) throw new Exception("No se encontro fecha de culminación de la etapa 1 del periodo seleccionado.");

            RePeriodoEtapaDTO etapa2 = etapas.Find(x => x.Reetacodi == 2);
            if (etapa2 == null) throw new Exception("No se encontro fecha de culminación de la etapa 2 del periodo seleccionado.");

            RePeriodoEtapaDTO etapa3 = etapas.Find(x => x.Reetacodi == 3);
            if (etapa3 == null) throw new Exception("No se encontro fecha de culminación de la etapa 3 del periodo seleccionado.");

            RePeriodoEtapaDTO etapa6 = etapas.Find(x => x.Reetacodi == 6);
            if (etapa6 == null) throw new Exception("No se encontro fecha de culminación de la etapa 6 del periodo seleccionado.");

            string strFinEtapa1 = string.Format("{0:D2}", etapa1.Repeetfecha.Value.Day) + " de " + EPDate.f_NombreMes(etapa1.Repeetfecha.Value.Month) + " del " + etapa1.Repeetfecha.Value.Year;
            string strFinEtapa2 = string.Format("{0:D2}", etapa2.Repeetfecha.Value.Day) + " de " + EPDate.f_NombreMes(etapa2.Repeetfecha.Value.Month) + " del " + etapa2.Repeetfecha.Value.Year;
            string strFinEtapa3 = string.Format("{0:D2}", etapa3.Repeetfecha.Value.Day) + " de " + EPDate.f_NombreMes(etapa3.Repeetfecha.Value.Month) + " del " + etapa3.Repeetfecha.Value.Year;
            string strFinEtapa6 = string.Format("{0:D2}", etapa6.Repeetfecha.Value.Day) + " de " + EPDate.f_NombreMes(etapa6.Repeetfecha.Value.Month) + " del " + etapa6.Repeetfecha.Value.Year;

            switch (tipoCorreo.ToString())
            {

                #region Solicitud de envío de interrupciones Trimestral y Semestral  - plantcodi : 174/175

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesTrimestral:

                    VariableCorreo obj1 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPeriodo); //Segundo Semestre 2022
                    obj1.ValorConDato = strPeriodo;

                    VariableCorreo obj2 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesesPeriodo); //julio a setiembre
                    obj2.ValorConDato = strMesesPeriodo;

                    VariableCorreo obj3 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValAnioPeriodo); //2022
                    obj3.ValorConDato = strAnioPeriodo;

                    VariableCorreo obj4 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValNumPeriodo); //02_SEGUNDO SEMESTRE
                    obj4.ValorConDato = strNumPeriodo;                    

                    VariableCorreo obj5 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesFinalPeriodo); // Setiembre
                    obj5.ValorConDato = strMesFinal;

                    VariableCorreo obj6 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValFecFinE1Periodo); // X de octubre del 2022
                    obj6.ValorConDato = strFinEtapa1;

                    break;

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesSemestral:


                    VariableCorreo obj7 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPeriodo); //Segundo Semestre 2022
                    obj7.ValorConDato = strPeriodo;

                    VariableCorreo obj8 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesesPeriodo); //julio a setiembre
                    obj8.ValorConDato = strMesesPeriodo;

                    VariableCorreo obj9 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValAnioPeriodo); //2022
                    obj9.ValorConDato = strAnioPeriodo;

                    VariableCorreo obj10 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValNumPeriodo); //02_SEGUNDO SEMESTRE
                    obj10.ValorConDato = strNumPeriodo;

                    VariableCorreo obj11 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesFinalPeriodo); // Setiembre
                    obj11.ValorConDato = strMesFinal;

                    VariableCorreo obj12 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValFecFinE1Periodo); // X de octubre del 2022
                    obj12.ValorConDato = strFinEtapa1;

                    break;

                #endregion

                #region Solicitud de envío de observaciones a las interrupciones - plantcodi : 176

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones:
                    
                    VariableCorreo obj13 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValResponsable); //Aceros Arequipa
                    obj13.ValorConDato = strResponsable;

                    VariableCorreo obj14 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValNombrePeriodo); //2023 - S1
                    obj14.ValorConDato = strNombPeriodo;

                    VariableCorreo obj15 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesesPeriodo); //julio a setiembre
                    obj15.ValorConDato = strMesesPeriodo;

                    VariableCorreo obj16 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValAnioPeriodo); //2022
                    obj16.ValorConDato = strAnioPeriodo;

                    VariableCorreo obj17 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValFecFinE2Periodo); // X de octubre del 2022
                    obj17.ValorConDato = strFinEtapa2;
                    break;

                #endregion

                #region Solicitud de envío de respuestas a las observaciones - plantcodi : 177

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones:

                    VariableCorreo obj18 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPeriodo); //Segundo Semestre 2022
                    obj18.ValorConDato = strPeriodo;

                    VariableCorreo obj19 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValFecFinE3Periodo); // X de octubre del 2022
                    obj19.ValorConDato = strFinEtapa3;

                    break;

                #endregion

                #region Solicitud de envío de decisiones de controversias - plantcodi : 178

                case ConstantesCalidadProducto.IdPlantillaSolicitudDecisionesControversia:
                    VariableCorreo obj20 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPeriodo); //Segundo Semestre 2022
                    obj20.ValorConDato = strPeriodo;

                    VariableCorreo obj21 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValFecFinE6Periodo); // X de octubre del 2022
                    obj21.ValorConDato = strFinEtapa6;

                    break;

                #endregion

                #region Solicitud de envío de interrupciones pendientes de reportar.  - plantcodi : 320

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar:

                    VariableCorreo obj22 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPeriodo); //Segundo Semestre 2022
                    obj22.ValorConDato = strPeriodo;

                    VariableCorreo obj23 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesesPeriodo); //julio a setiembre
                    obj23.ValorConDato = strMesesPeriodo;

                    VariableCorreo obj24 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValAnioPeriodo); //2022
                    obj24.ValorConDato = strAnioPeriodo;

                    VariableCorreo obj25 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValNumPeriodo); //02_SEGUNDO SEMESTRE
                    obj25.ValorConDato = strNumPeriodo;

                    VariableCorreo obj26 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesFinalPeriodo); // Setiembre
                    obj26.ValorConDato = strMesFinal;


                    break;
                    #endregion
            }

            return listaVariables;
        }

        /// <summary>
        /// Devuelve el asunto con datos llenados
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="tipoCorreo"></param>
        /// <param name="listaVariables"></param>
        /// <returns></returns>
        public string LLenarDatosVariables(string texto, int tipoCorreo, List<VariableCorreo> listaVariables)
        {
            string salida = "";
            string textoFinal = "";
            string t1 = "";
            string t2 = "";
            string t3 = "";
            string t4 = "";
            string t5 = "";



            int idEmpresa = -9;
            switch (tipoCorreo.ToString())
            {

                #region Solicitud de envío de interrupciones Trimestral y Semestral  - plantcodi : 174/175

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesTrimestral:

                    t1 = texto.Replace(ConstantesCalidadProducto.ValPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPeriodo).ValorConDato);
                    t2 = t1.Replace(ConstantesCalidadProducto.ValMesesPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesesPeriodo).ValorConDato);
                    t3 = t2.Replace(ConstantesCalidadProducto.ValAnioPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValAnioPeriodo).ValorConDato);
                    t4 = t3.Replace(ConstantesCalidadProducto.ValNumPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValNumPeriodo).ValorConDato);
                    t5 = t4.Replace(ConstantesCalidadProducto.ValMesFinalPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesFinalPeriodo).ValorConDato);
                    textoFinal = t5.Replace(ConstantesCalidadProducto.ValFecFinE1Periodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValFecFinE1Periodo).ValorConDato);

                    break;

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesSemestral:

                    t1 = texto.Replace(ConstantesCalidadProducto.ValPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPeriodo).ValorConDato);
                    t2 = t1.Replace(ConstantesCalidadProducto.ValMesesPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesesPeriodo).ValorConDato);
                    t3 = t2.Replace(ConstantesCalidadProducto.ValAnioPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValAnioPeriodo).ValorConDato);
                    t4 = t3.Replace(ConstantesCalidadProducto.ValNumPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValNumPeriodo).ValorConDato);
                    t5 = t4.Replace(ConstantesCalidadProducto.ValMesFinalPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesFinalPeriodo).ValorConDato);
                    textoFinal = t5.Replace(ConstantesCalidadProducto.ValFecFinE1Periodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValFecFinE1Periodo).ValorConDato);

                    break;

                #endregion

                #region Solicitud de envío de observaciones a las interrupciones - plantcodi : 176

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones:

                    t1 = texto.Replace(ConstantesCalidadProducto.ValResponsable, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValResponsable).ValorConDato);
                    t2 = t1.Replace(ConstantesCalidadProducto.ValNombrePeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValNombrePeriodo).ValorConDato);
                    t3 = t2.Replace(ConstantesCalidadProducto.ValMesesPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesesPeriodo).ValorConDato);
                    t4 = t3.Replace(ConstantesCalidadProducto.ValAnioPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValAnioPeriodo).ValorConDato);
                    textoFinal = t4.Replace(ConstantesCalidadProducto.ValFecFinE2Periodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValFecFinE2Periodo).ValorConDato);

                    break;

                #endregion

                #region Solicitud de envío de respuestas a las observaciones - plantcodi : 177

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones:

                    t1 = texto.Replace(ConstantesCalidadProducto.ValPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPeriodo).ValorConDato);
                    textoFinal = t1.Replace(ConstantesCalidadProducto.ValFecFinE3Periodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValFecFinE3Periodo).ValorConDato);

                    break;

                #endregion

                #region Solicitud de envío de decisiones de controversias - plantcodi : 178

                case ConstantesCalidadProducto.IdPlantillaSolicitudDecisionesControversia:

                    t1 = texto.Replace(ConstantesCalidadProducto.ValPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPeriodo).ValorConDato);
                    textoFinal = t1.Replace(ConstantesCalidadProducto.ValFecFinE6Periodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValFecFinE6Periodo).ValorConDato);                    

                    break;

                #endregion

                #region Solicitud de envío de compensaciones por mala calidad de producto - plantcodi : 179

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvíoCompensacionesMalaCalidadProducto:
                    
                    t1 = texto.Replace(ConstantesCalidadProducto.ValMesAnioEvento, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesAnioEvento).ValorConDato);
                    textoFinal = t1.Replace(ConstantesCalidadProducto.ValPuntoEntregaEvento, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPuntoEntregaEvento).ValorConDato);

                    break;

                #endregion

                #region Solicitud de envío de interrupciones pendientes de reportar  - plantcodi : 320

                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar:

                    t1 = texto.Replace(ConstantesCalidadProducto.ValPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPeriodo).ValorConDato);
                    t2 = t1.Replace(ConstantesCalidadProducto.ValMesesPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesesPeriodo).ValorConDato);
                    t3 = t2.Replace(ConstantesCalidadProducto.ValAnioPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValAnioPeriodo).ValorConDato);
                    t4 = t3.Replace(ConstantesCalidadProducto.ValNumPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValNumPeriodo).ValorConDato);
                    textoFinal = t4.Replace(ConstantesCalidadProducto.ValMesFinalPeriodo, listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesFinalPeriodo).ValorConDato);
                    
                    break;


                    #endregion
            }

            salida = textoFinal;

            return salida;
        }

        /// <summary>
        /// Deveuelve los correos PARA segun la empresa seleccionada enel filtro
        /// </summary>
        /// <param name="tipoCorreo"></param>
        /// <param name="responsableGral"></param>
        /// <param name="suministradorGral"></param>
        /// <returns></returns>
        public string ObtenerCorreosParaSegunEmpresa(int tipoCorreo, int? responsableGral, int? suministradorGral)
        {
            string para = "No hay correos registrados para este destinatario";

            int idEmpresa = -9;
            switch (tipoCorreo.ToString())
            {
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones:
                    idEmpresa = responsableGral.Value;
                    break;
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones:
                case ConstantesCalidadProducto.IdPlantillaSolicitudDecisionesControversia:
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesTrimestral:
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesSemestral:
                case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar:
                    idEmpresa = suministradorGral.Value;
                    break;
            }

            para = ObtenerDestinatariosPorEmpresa(para, idEmpresa);

            return para;
        }

        /// <summary>
        /// Devuelve lista string de correos segun empresa
        /// </summary>
        /// <param name="paraPorDefecto"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        private string ObtenerDestinatariosPorEmpresa(string paraPorDefecto, int idEmpresa)
        {
            List<SiEmpresaCorreoDTO> lstEmpresasCorreo = FactorySic.GetSiEmpresaCorreoRepository().ObtenerCorreosPorEmpresaResarcimiento(idEmpresa);

            if (lstEmpresasCorreo.Any())
            {
                List<string> lstOrdenada = lstEmpresasCorreo.Select(x => x.Empcoremail.Trim()).OrderBy(x => x).ToList();

                paraPorDefecto = String.Join("; ", lstOrdenada);
            }

            return paraPorDefecto;
        }
        #endregion

        #region Plantillas

        /// <summary>
        /// Lista las plantillas de correos
        /// </summary>
        /// <returns></returns>
        public List<SiPlantillacorreoDTO> ListarPlantillasCorreo()
        {
            List<SiPlantillacorreoDTO> lstTemp = new List<SiPlantillacorreoDTO>();
            
            lstTemp = FactorySic.GetSiPlantillacorreoRepository().ListarPlantillas(ConstantesCalidadProducto.IdPlantillaTodosPlantilla).OrderBy(x => x.Plantcodi).ToList();
            List<SiPlantillacorreoDTO> lstSalida = DarFormatoListaPlantillasCorreo(lstTemp);

            return lstSalida;
        }

        /// <summary>
        /// Formatea el campo de fechas
        /// </summary>
        /// <param name="lstTemp"></param>
        /// <returns></returns>
        private List<SiPlantillacorreoDTO> DarFormatoListaPlantillasCorreo(List<SiPlantillacorreoDTO> lstTemp)
        {
            foreach (var plantilla in lstTemp)
            {
                plantilla.Plantusucreacion = plantilla.Plantusucreacion != null ? plantilla.Plantusucreacion : "";
                plantilla.Plantusumodificacion = plantilla.Plantusumodificacion != null ? plantilla.Plantusumodificacion : "";
                plantilla.PlantfeccreacionDesc = plantilla.Plantfeccreacion != null ? plantilla.Plantfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : "";
                plantilla.PlantfecmodificacionDesc = plantilla.Plantfecmodificacion != null ? plantilla.Plantfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : "";
            }

            return lstTemp;
        }

        /// <summary>
        /// Devuelve el listado de variables que se muestran en asunto y contenido
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <param name="campo"></param>
        /// <returns></returns>
        public List<VariableCorreo> ObtenerListadoVariables(int idPlantilla, int campo)
        {
            List<VariableCorreo> lstSalida = new List<VariableCorreo>();

            VariableCorreo obj = new VariableCorreo();

            //Al crear o eliminar  variables se debe editar tambien en plantillaCorreo.js y
            //Al agregar variables a un correo se debe agregar tambien en la funcion LlenarVariablesPorEmpresa (dentro del switch)
            if (campo == ConstantesCalidadProducto.VariablePara)
            {
               
            }
            else
            {
                if (campo == ConstantesCalidadProducto.VariableCC)
                {
                    
                }
                else
                {
                    //para Asunto y Contenido
                    switch (idPlantilla.ToString())
                    {

                        #region Solicitud de envío de interrupciones Trimestral y Semestral  - plantcodi : 174/175

                        case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesTrimestral:

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValMesesPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscMesesPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValAnioPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscAnioPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValNumPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscNumPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValMesFinalPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscMesFinalPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValFecFinE1Periodo;
                            obj.Nombre = ConstantesCalidadProducto.DscFecFinE1Periodo;
                            lstSalida.Add(obj);

                            break;

                        case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesSemestral:
                            

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValMesesPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscMesesPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValAnioPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscAnioPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValNumPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscNumPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValMesFinalPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscMesFinalPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValFecFinE1Periodo;
                            obj.Nombre = ConstantesCalidadProducto.DscFecFinE1Periodo;
                            lstSalida.Add(obj);
                            break;

                        #endregion

                        #region Solicitud de envío de observaciones a las interrupciones - plantcodi : 176

                        case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioObservacionesAInterrupciones:

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValResponsable;
                            obj.Nombre = ConstantesCalidadProducto.DscResponsable;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValNombrePeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscNombrePeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValMesesPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscMesesPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValAnioPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscAnioPeriodo;
                            lstSalida.Add(obj);                           

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValFecFinE2Periodo;
                            obj.Nombre = ConstantesCalidadProducto.DscFecFinE2Periodo;
                            lstSalida.Add(obj);
                            break;

                        #endregion

                        #region Solicitud de envío de respuestas a las observaciones - plantcodi : 177

                        case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioRespuestasAInterrupciones:

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValFecFinE3Periodo;
                            obj.Nombre = ConstantesCalidadProducto.DscFecFinE3Periodo;
                            lstSalida.Add(obj);

                            break;

                        #endregion

                        #region Solicitud de envío de decisiones de controversias - plantcodi : 178

                        case ConstantesCalidadProducto.IdPlantillaSolicitudDecisionesControversia:
                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValFecFinE6Periodo;
                            obj.Nombre = ConstantesCalidadProducto.DscFecFinE6Periodo;
                            lstSalida.Add(obj);

                            break;

                        #endregion

                        #region Solicitud de envío de compensaciones por mala calidad de producto - plantcodi : 179

                        case ConstantesCalidadProducto.IdPlantillaSolicitudEnvíoCompensacionesMalaCalidadProducto:
                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValMesAnioEvento;
                            obj.Nombre = ConstantesCalidadProducto.DscMesAnioEvento;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValPuntoEntregaEvento;
                            obj.Nombre = ConstantesCalidadProducto.DscPuntoEntregaEvento;
                            lstSalida.Add(obj);

                            break;

                        #endregion

                        #region Solicitud de envío de interrupciones pendientes de reportar.  - plantcodi : 320

                        case ConstantesCalidadProducto.IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar:

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValMesesPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscMesesPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValAnioPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscAnioPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValNumPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscNumPeriodo;
                            lstSalida.Add(obj);

                            obj = new VariableCorreo();
                            obj.Valor = ConstantesCalidadProducto.ValMesFinalPeriodo;
                            obj.Nombre = ConstantesCalidadProducto.DscMesFinalPeriodo;
                            lstSalida.Add(obj);

                            break;

                            #endregion
                    }
                }
            }
            return lstSalida.OrderBy(x => x.Nombre).ToList();
        }

        /// <summary>
        /// Actualiza y Guarda la informacion de la plantilla de correo
        /// </summary>
        /// <param name="correo"></param>
        /// <param name="usuario"></param>
        public void ActualizarDatosPlantillaCorreo(SiPlantillacorreoDTO correo, string usuario)
        {            
            /**** Actualizar la plantilla ****/
            SiPlantillacorreoDTO c = servCorreo.GetByIdSiPlantillacorreo(correo.Plantcodi);
            c.Plantcontenido = correo.Plantcontenido;
            c.Plantasunto = correo.Plantasunto;
            c.Planticorreos = correo.Planticorreos;
            c.PlanticorreosCc = correo.PlanticorreosCc;
            c.PlanticorreosBcc = correo.PlanticorreosBcc;
            c.PlanticorreoFrom = correo.PlanticorreoFrom;
            c.Plantfecmodificacion = DateTime.Now;
            c.Plantusumodificacion = usuario;

            servCorreo.UpdateSiPlantillacorreo(c);

        }

        #endregion

        #region Correos por empresa

        
        /// <summary>
        /// Devuelve el listado de empresas 
        /// </summary>
        /// <returns></returns>
        public List<ReEmpresaDTO> ObtenerEmpresasTotales()
        {
            EstructuraInterrupcion maestros = new EstructuraInterrupcion();
            maestros.ListaEmpresa = this.servicioResarcimiento.ObtenerEmpresas();

            return maestros.ListaEmpresa;
        }

        /// <summary>
        /// Devuelve el listado de empresas con sus correos
        /// </summary>
        /// <returns></returns>
        public List<EmpresaCorreo> ListarEmpresasYSusCorreos()
        {
            List<EmpresaCorreo> lstSalida = new List<EmpresaCorreo>();

            List<SiEmpresaCorreoDTO> lstEmpresasCorreo = FactorySic.GetSiEmpresaCorreoRepository().ObtenerCorreosPorEmpresaResarcimiento(Int32.Parse(ConstantesCalidadProducto.ValorPorDefecto));

            var lista = lstEmpresasCorreo.GroupBy(x => new { x.Emprcodi }).ToList();

            foreach (var item in lista)
            {
                SiEmpresaCorreoDTO e = item.First();
                EmpresaCorreo empresa = new EmpresaCorreo();                
                empresa.Emprcodi = item.Key.Emprcodi;
                empresa.Emprnomb = e != null ? e.Emprnomb : "";

                List<string> lstC = item.Select(x => x.Empcoremail).OrderBy(x=>x).ToList();
                empresa.LstCorreos = String.Join("; ", lstC);
                empresa.UsuarioModificacion = e.Lastuser != null ? e.Lastuser.Trim() : "";
                empresa.FechaModificacionDesc = e.Lastdate != null ? e.Lastdate.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";


                lstSalida.Add(empresa);
            }


            return lstSalida.OrderBy(x => x.Emprnomb).ToList();
        }

        /// <summary>
        /// Devuelve una cadena con los correos de cierta empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public string ListarCorreosPorEmpresa(int emprcodi)
        {
            string salida = "";
            List<SiEmpresaCorreoDTO> lstCorreosE = FactorySic.GetSiEmpresaCorreoRepository().ObtenerCorreosPorEmpresaResarcimiento(emprcodi);

            if (lstCorreosE.Any())
            {
                lstCorreosE = lstCorreosE.OrderBy(x => x.Empcoremail).ToList();
                salida = String.Join(",", lstCorreosE.Select(x => x.Empcoremail).ToList());
            }
           

            return salida;
        }

        /// <summary>
        /// Valida el listado de correos a guardar
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="lstCorreos"></param>
        /// <returns></returns>
        public string ValidarEmpresaCorreos(int accion, int emprcodi, string lstCorreos)
        {
            List<ReEmpresaDTO> lstEmpresas = ObtenerEmpresasTotales();
            List<SiEmpresaCorreoDTO> lstEmpresasCorreo = FactorySic.GetSiEmpresaCorreoRepository().ObtenerCorreosPorEmpresaResarcimiento(Int32.Parse(ConstantesCalidadProducto.ValorPorDefecto));

            string salida = "";
            salida = salida + ValidarCorreosVacios(lstCorreos);

            salida = salida + ValidarCorreosRepetidos(emprcodi, lstCorreos, lstEmpresas);

            if(accion == ConstantesCalculoResarcimiento.AccionNuevo)
                salida = salida + ValidarCorreosDuplicados(emprcodi, lstCorreos, lstEmpresas, lstEmpresasCorreo);

            return salida;
        }

        /// <summary>
        /// Valida que no haya correos vacios en el listado de correos
        /// </summary>
        /// <param name="lstCorreos"></param>
        /// <returns></returns>
        public string ValidarCorreosVacios(string lstCorreos)
        {
            string salida = "";
            
            List<string> listaCorreos = lstCorreos.Split(';').ToList();


            List<string> listaCorreosVacios = new List<string>();
            foreach (string correo in listaCorreos)
            {
                if (correo.Trim() == "")
                {
                    listaCorreosVacios.Add(correo);
                }
            }

            if (listaCorreosVacios.Any())
                salida = "<p>El campo Correos Electrónicos contiene correos vacios, corregir. Para ello escribir un correo válido y presionar Enter para añadirlo</p>";


            return salida;
        }
        

        /// <summary>
        /// Valida que no haya correos repetidos en el listado de correos
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="lstCorreos"></param>
        /// <param name="lstEmpresas"></param>
        /// <returns></returns>
        public string ValidarCorreosRepetidos(int emprcodi, string lstCorreos, List<ReEmpresaDTO> lstEmpresas)
        {
            string salida = "";

            ReEmpresaDTO emp = lstEmpresas.Find(x => x.Emprcodi == emprcodi);
            List<string> listaCorreos = lstCorreos.Split(';').Where(x => x != "").ToList();
            bool hayRepetidos = listaCorreos.GroupBy(x => x).Any(g => g.Count() > 1);

            if (emp != null) {                                
                if (hayRepetidos)
                    salida = "<p>La empresa " + emp.Emprnomb.Trim() + " contiene correos repetidos, corregir.</p>";
            }
            else
            {
                if (hayRepetidos)
                    salida = "<p>Contiene correos repetidos, corregir.</p>";
            }

            return salida;
        }

        /// <summary>
        /// Elimina una empresa y sus correos
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="usuario"></param>
        public void EliminarEmpresaCorreos(int emprcodi, string usuario)
        {
            try
            {
                FactorySic.GetSiEmpresaCorreoRepository().DeleteResarcimiento(emprcodi);
            }
            catch (Exception ex)
            {                
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// Valida que no haya correos duplicados en la BD
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="lstCorreos"></param>
        /// <param name="lstEmpresas"></param>
        /// <returns></returns>
        public string ValidarCorreosDuplicados(int emprcodi, string lstCorreos, List<ReEmpresaDTO> lstEmpresas, List<SiEmpresaCorreoDTO> lstEmpresasCorreo)
        {
            string salida = "";

            ReEmpresaDTO emp = lstEmpresas.Find(x => x.Emprcodi == emprcodi);
            List<SiEmpresaCorreoDTO> lstCorreosPorEmpresa = lstEmpresasCorreo.Where(x => x.Emprcodi == emprcodi).ToList();

            if (emp != null)
            {
                List<string> listaCorreosAGuardar = lstCorreos.Split(';').ToList();
                List<string> listaCorreosGuardados = lstCorreosPorEmpresa.Select(x => x.Empcoremail.Trim()).ToList();
                List<string> listIntersect = listaCorreosAGuardar.Intersect(listaCorreosGuardados).ToList();                

                if(listIntersect.Any())
                {
                    salida = "<p>La empresa " + emp.Emprnomb.Trim() + " ya tiene registrado el(los) siguiente(s) correo(s): " + String.Join(", ", listIntersect) + ".</p>";
                }                                    
            }

            return salida;
        }

        /// <summary>
        /// Guarda los correos para cierta empresa
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="empresaId"></param>
        /// <param name="correos"></param>
        /// <param name="cargaDesdeExcel"></param>
        public void GuardarEmpresaCorreos(int accion, int empresaId, string correos, bool cargaDesdeExcel, string usuario)
        {
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = (DbTransaction)UoW.StartTransaction(connection))
                {
                    if(accion == ConstantesCalculoResarcimiento.AccionEditar)
                    {
                        //Si es editar, primero elimino todas los correos de la empresa
                        FactorySic.GetSiEmpresaCorreoRepository().DeleteResarcimiento(empresaId);
                    }

                    //eliminamos los correos por empresa si se carga desde excel
                    if (cargaDesdeExcel)
                    {
                        FactorySic.GetSiEmpresaCorreoRepository().DeleteResarcimiento(empresaId);
                    }

                    //Hallo el maximo Correlativo libre
                    int idCorrelativo = FactorySic.GetSiEmpresaCorreoRepository().GetMaxId();

                    //Guardo todo
                    List<SiEmpresaCorreoDTO> lstRegistros = new List<SiEmpresaCorreoDTO>();

                    List<string> listaCorreosAGuardar = correos.Split(';').ToList();

                    foreach (var correoAGuardar in listaCorreosAGuardar)
                    {
                        SiEmpresaCorreoDTO correo = new SiEmpresaCorreoDTO();
                        correo.Empcorcodi = idCorrelativo;
                        correo.Emprcodi = empresaId;
                        correo.Modcodi = ConstantesCalidadProducto.ModuloCalculoResarcimiento;
                        correo.Empcoremail = correoAGuardar != null ? correoAGuardar.Trim() : "";
                        correo.Lastuser = usuario;

                        //Evito que guarde emails vacios, aunque se valida antes de guardar
                        if (correo.Empcoremail != "")
                        {
                            lstRegistros.Add(correo);
                            idCorrelativo++;
                        }
                    }

                    try
                    {                        
                        foreach (SiEmpresaCorreoDTO reg in lstRegistros)
                        {                            
                            FactorySic.GetSiEmpresaCorreoRepository().SaveTransaccional(reg, connection, transaction);                            
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }

        /// <summary>
        /// Guarda los correos desde archivo excel
        /// </summary>
        /// <param name="listaDataExcel"></param>
        /// <param name="usuario"></param>
        public void GuardarGrupoCorreos(List<EmpresaCorreo> listaDataExcel, string usuario)
        {
            foreach (var empresaCorreo in listaDataExcel)
            {
                GuardarEmpresaCorreos(ConstantesCalculoResarcimiento.AccionNuevo, empresaCorreo.Emprcodi, empresaCorreo.LstCorreos, true, usuario);
            }
        }

        /// <summary>
        /// Genera el archivo a exportar
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="nameFile"></param>
        public void GenerarExportacionEC(string ruta, string pathLogo, string nameFile)
        {
            List<EmpresaCorreo> listaECTotales = ListarEmpresasYSusCorreos();

            ////Descargo archivo segun requieran
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarArchivoExcelEC(xlPackage, pathLogo, listaECTotales);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera la estructura de la tabla a exportar
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="listaRegTotales"></param>
        private void GenerarArchivoExcelEC(ExcelPackage xlPackage, string pathLogo, List<EmpresaCorreo> listaRegTotales)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            string nameWS = "Listado";
            string titulo = "CUENTAS DE CORREO POR EMPRESA";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;

            UtilExcel.AddImageLocal(ws, 1, 0, pathLogo, 120, 70);

            #region  Filtros y Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 4;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 2;

            int colEmpresa = colIniTable;
            int colCorreos = colIniTable + 1;
            int colUsuario = colIniTable + 2;
            int colFecha = colIniTable + 3;
            

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha, "Calibri", 16);

            ws.Row(rowIniTabla).Height = 25;
            ws.Cells[rowIniTabla, colEmpresa].Value = "Empresa";
            ws.Cells[rowIniTabla, colCorreos].Value = "Correos";
            ws.Cells[rowIniTabla, colUsuario].Value = "Usuario Modificación";
            ws.Cells[rowIniTabla, colFecha].Value = "Fecha de Modificación";            

            //Estilos cabecera
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colEmpresa, rowIniTabla, colFecha, "Calibri", 11);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colEmpresa, rowIniTabla, colFecha, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colEmpresa, rowIniTabla, colFecha, "Centro");
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colEmpresa, rowIniTabla, colFecha, "#2980B9");
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colEmpresa, rowIniTabla, colFecha, "#FFFFFF");
            servFormato.BorderCeldas2(ws, rowIniTabla, colEmpresa, rowIniTabla, colFecha);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colEmpresa, rowIniTabla, colFecha);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;
            string fondoEnBaja = "#FFD6D6";

            foreach (var item in listaRegTotales)
            {
                ws.Cells[rowData, colEmpresa].Value = item.Emprnomb != null ? item.Emprnomb.Trim() : "";
                ws.Cells[rowData, colCorreos].Value = item.LstCorreos != null ? item.LstCorreos.Trim() : "";                
                ws.Cells[rowData, colUsuario].Value = item.UsuarioModificacion != null ? item.UsuarioModificacion.Trim() : "";
                ws.Cells[rowData, colFecha].Value = item.FechaModificacionDesc != null ? item.FechaModificacionDesc.Trim() : "";

                rowData++;
            }

            if (!listaRegTotales.Any()) rowData++;

            //Estilos registros
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colEmpresa, rowData - 1, colFecha, "Calibri", 8);
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colEmpresa, rowData - 1, colFecha, "Centro");
            servFormato.BorderCeldas2(ws, rowIniTabla + 1, colEmpresa, rowData - 1, colFecha);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colEmpresa, rowData - 1, colFecha, "Centro");

            #endregion

            //filter           
            ws.Cells[rowIniTabla, colEmpresa, rowData, colFecha].AutoFitColumns();
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        /// <summary>
        /// Obtiene data desde excel
        /// </summary>
        /// <param name="stremExcel"></param>
        /// <param name="user"></param>
        /// <param name="empresas"></param>
        /// <param name="listaCorrecta"></param>
        /// <param name="listaErrores"></param>
        public void ObtenerDataExcel(Stream stremExcel, string user, out List<EmpresaCorreo> listaCorrecta, out List<EmpresaCorreoErrorExcel> listaErrores)
        {
            try
            {
                List<EmpresaCorreo> listaReg = new List<EmpresaCorreo>();
                List<EmpresaCorreoErrorExcel> listaErroresEncontrados = new List<EmpresaCorreoErrorExcel>();

                List<ReEmpresaDTO> empresas = ObtenerEmpresasTotales();

                using (var xlPackage = new ExcelPackage(stremExcel))
                {
                    var ws = xlPackage.Workbook.Worksheets[1];
                    var dim = ws.Dimension;

                    //var ws2 = EliminarPrimerasFilasVacias(ws);
                    var ws3 = EliminarUltimasFilasVacias(ws);
                    dim = ws3.Dimension;

                    //Dimensiones
                    ExcelRange excelRangeCabecera = ws.Cells[dim.Start.Row, dim.Start.Column, dim.Start.Row, dim.End.Column];
                    ExcelRange excelRangeData = ws.Cells[dim.Start.Row + 1, dim.Start.Column, dim.End.Row, dim.End.Column];

                    var CabExcel = (object[,])excelRangeCabecera.Value;
                    var dataExcel = (object[,])excelRangeData.Value;

                    //valido cabecera
                    string textosCab = "";
                    for (int i = 0; i < dim.Columns; i++)
                    {
                        if (CabExcel[0, i] != null)
                            textosCab = textosCab + " " + CabExcel[0, i].ToString();
                    }
                    if (textosCab.ToUpper().Contains("EMPRESA") && textosCab.ToUpper().Contains("CORREOS"))
                    {

                    }
                    else
                    {
                        throw new Exception("Tabla con cabecera no reconocida.");
                    }

                    //validar estructura de la tabla
                    if (!CabExcel[0, 0].ToString().ToUpper().Contains("EMPRESA"))
                        throw new Exception("Primera Columna (Empresa) mal posicionada.");
                    if (!CabExcel[0, 1].ToString().ToUpper().Contains("CORREOS"))
                        throw new Exception("Segunda Columna (Correos) mal posicionada.");
                    

                    var ultimaFilatBloque = dim.End.Row - dim.Start.Row - 1;

                    var coluIniBloque = dim.Start.Column;
                    var coluFinBloque = dim.End.Column;

                    int filaCabecera = 0;

                    //Obtenemos las cabeceras y su posicion
                    var dictionary = new Dictionary<string, int>();
                    string[] lstCab = new string[3];
                    int c1 = 0;
                    for (int col = coluIniBloque - 1; col < coluFinBloque; col++)
                    {
                        var nomCab = CabExcel[filaCabecera, c1].ToString().Trim();
                        if (nomCab != "")
                        {
                            dictionary.Add(nomCab, c1);
                            lstCab[c1] = nomCab;
                        }
                        c1++;
                    }

                    List<string> lstEmpresasCargadas = new List<string>();

                    for (int fila = filaCabecera; fila <= ultimaFilatBloque; fila++)
                    {
                        string nombEmpresa = dataExcel[fila, dictionary[lstCab[0]]] != null ? dataExcel[fila, dictionary[lstCab[0]]].ToString().Trim() : "";

                        if (nombEmpresa != "")
                            lstEmpresasCargadas.Add(nombEmpresa);
                    }
                        

                    for (int fila = filaCabecera; fila <= ultimaFilatBloque; fila++)
                    {
                        var excelEmpresa = dataExcel[fila, dictionary[lstCab[0]]] != null ? dataExcel[fila, dictionary[lstCab[0]]].ToString().Trim() : "";
                        var excelCorreos = dataExcel[fila, dictionary[lstCab[1]]] != null ? dataExcel[fila, dictionary[lstCab[1]]].ToString().Trim() : "";

                        ReEmpresaDTO emp = empresas.Find(x => x.Emprnomb.ToUpper().Trim() == excelEmpresa.ToUpper().Trim());

                        string valorEmpresa;
                        string valorCorreos;

                        #region Validación de campos
                        this.BuscarErrorEmpresaEnCelda(empresas, fila, lstCab[0], excelEmpresa, lstEmpresasCargadas, out valorEmpresa, ref listaErroresEncontrados);
                        this.BuscarErrorCorreosEnCelda(empresas, fila, lstCab[1], excelCorreos, out valorCorreos, ref listaErroresEncontrados);

                        #endregion

                        if (emp != null) {
                            listaReg.Add(new EmpresaCorreo()
                            {
                                Emprcodi = emp.Emprcodi,
                                Emprnomb = emp.Emprnomb != null ? emp.Emprnomb.Trim() : "",
                                LstCorreos = excelCorreos,
                                
                            });
                        }
                    }
                }

                listaCorrecta = listaReg;
                listaErrores = listaErroresEncontrados;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Elimina las ultimas filas
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public ExcelWorksheet EliminarUltimasFilasVacias(ExcelWorksheet worksheet)
        {
            ExcelWorksheet worksheetSalida;

            while (EsUltimaFilaVacia(worksheet))
                worksheet.DeleteRow(worksheet.Dimension.End.Row);

            worksheetSalida = worksheet;

            return worksheetSalida;
        }

        /// <summary>
        /// Devuelve si la utima fila es vacia o no
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public bool EsUltimaFilaVacia(ExcelWorksheet worksheet)
        {
            var lstVacios = new List<bool>();

            for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
            {
                var celdaVacia = worksheet.Cells[worksheet.Dimension.End.Row, i].Value == null ? true : false;
                lstVacios.Add(celdaVacia);
            }

            return lstVacios.All(e => e);
        }

        /// <summary>
        /// Busca error en campo empresa
        /// </summary>
        /// <param name="lstEmpresas"></param>
        /// <param name="fila"></param>
        /// <param name="campo"></param>
        /// <param name="valorCelda"></param>
        /// <param name="valorEmpresa"></param>
        /// <param name="listaErrores"></param>
        private void BuscarErrorEmpresaEnCelda(List<ReEmpresaDTO> lstEmpresas, int fila, string campo, string valorCelda, List<string> lstEmpresasCargadas, out string valorEmpresa, ref List<EmpresaCorreoErrorExcel> listaErrores)
        {
            string salida = "";
            if (valorCelda != null)
            {
                if (valorCelda.Trim() != "")
                {                   
                    ReEmpresaDTO empresa = lstEmpresas.Find(x => x.Emprnomb.ToUpper().Trim() == valorCelda.ToUpper().Trim());

                    if (empresa == null)
                    {
                        EmpresaCorreoErrorExcel error = new EmpresaCorreoErrorExcel();
                        error.NumeroFilaExcel = fila + 1;
                        error.CampoExcel = campo;
                        error.ValorCeldaExcel = valorCelda;
                        error.MensajeValidacion = "La empresa ingresada no pertenece a la relación de empresas SGOCOES.";

                        listaErrores.Add(error);
                    }
                    else
                    {
                        List<string> lstEmpresasIguales = lstEmpresasCargadas.Where(x => x.ToUpper().Trim() == valorCelda.ToUpper().Trim()).ToList();
                        if (lstEmpresasIguales.Count > 1)
                        {
                            EmpresaCorreoErrorExcel error = new EmpresaCorreoErrorExcel();
                            error.NumeroFilaExcel = fila + 1;
                            error.CampoExcel = campo;
                            error.ValorCeldaExcel = valorCelda;
                            error.MensajeValidacion = "La empresa ingresada se repite en dos o más registros.";

                            listaErrores.Add(error);
                        }
                        else
                            salida = valorCelda.Trim();
                    }
                        
                    
                }
                else
                {
                    EmpresaCorreoErrorExcel error = new EmpresaCorreoErrorExcel();
                    error.NumeroFilaExcel = fila + 1;
                    error.CampoExcel = campo;
                    error.ValorCeldaExcel = valorCelda;
                    error.MensajeValidacion = "No contiene el nombre de la empresa.";

                    listaErrores.Add(error);
                }
                
            }
            else
            {
                EmpresaCorreoErrorExcel error = new EmpresaCorreoErrorExcel();
                error.NumeroFilaExcel = fila + 1;
                error.CampoExcel = campo;
                error.ValorCeldaExcel = valorCelda;
                error.MensajeValidacion = "No contiene el nombre de la empresa.";

                listaErrores.Add(error);
            }

            valorEmpresa = salida;
        }

        /// <summary>
        /// Busca errores en los correos
        /// </summary>
        /// <param name="lstEmpresas"></param>
        /// <param name="fila"></param>
        /// <param name="campo"></param>
        /// <param name="valorCelda"></param>
        /// <param name="valorCorreo"></param>
        /// <param name="listaErrores"></param>
        private void BuscarErrorCorreosEnCelda(List<ReEmpresaDTO> lstEmpresas, int fila, string campo, string valorCelda, out string valorCorreo, ref List<EmpresaCorreoErrorExcel> listaErrores)
        {

            string salida = "";
            if (valorCelda != null)
            {
                if (valorCelda.Trim() != "")
                {
                    //valido repeticiones
                    string msgErrorRepetidos = "";
                    string msgErrorFormato = "";
                    ReEmpresaDTO empresa = lstEmpresas.Find(x => x.Emprnomb.ToUpper().Trim() == valorCelda.ToUpper().Trim());
                    int empresaId = empresa != null ? empresa.Emprcodi : -999;
                    msgErrorRepetidos = ValidarCorreosRepetidos(empresaId, valorCelda, lstEmpresas);
                    int val = ValidarFormatoCorreos(valorCelda, 1, -1);

                    //valido correcto formato de correos
                    if (val < 0)
                    {
                        EmpresaCorreoErrorExcel error = new EmpresaCorreoErrorExcel();
                        error.NumeroFilaExcel = fila + 1;
                        error.CampoExcel = campo;
                        error.ValorCeldaExcel = valorCelda;
                        error.MensajeValidacion = "Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;)";

                        listaErrores.Add(error);
                    }
                    else
                    {
                        if (msgErrorRepetidos != "")
                        {
                            EmpresaCorreoErrorExcel error = new EmpresaCorreoErrorExcel();
                            error.NumeroFilaExcel = fila + 1;
                            error.CampoExcel = campo;
                            error.ValorCeldaExcel = valorCelda;
                            error.MensajeValidacion = msgErrorRepetidos;

                            listaErrores.Add(error);
                        }
                    }
                    

                    if(msgErrorRepetidos == "" &&  val >= 0)
                        salida = valorCelda.Trim();

                }
                else
                {
                    EmpresaCorreoErrorExcel error = new EmpresaCorreoErrorExcel();
                    error.NumeroFilaExcel = fila + 1;
                    error.CampoExcel = campo;
                    error.ValorCeldaExcel = valorCelda;
                    error.MensajeValidacion = "No contiene correo(s).";

                    listaErrores.Add(error);
                }

            }
            else
            {
                EmpresaCorreoErrorExcel error = new EmpresaCorreoErrorExcel();
                error.NumeroFilaExcel = fila + 1;
                error.CampoExcel = campo;
                error.ValorCeldaExcel = valorCelda;
                error.MensajeValidacion = "No contiene correo(s).";

                listaErrores.Add(error);
            }

            valorCorreo = salida;            

        }

        /// <summary>
        /// Valida que los correos ingresados tengan el formato y sintaxis correctos
        /// </summary>
        /// <param name="strCorreos"></param>
        /// <param name="numMinCorreosObligatorios"></param>
        /// <param name="numMaxCorreosObligatorios"></param>
        /// <returns></returns>
        public int ValidarFormatoCorreos(string strCorreos, int numMinCorreosObligatorios, int numMaxCorreosObligatorios)
        {
            string cadena = strCorreos;

            var arreglo = cadena.Split(';').ToArray();
            int nroCorreo = 0;
            int longitud = arreglo.Length;

            if (longitud == 0)
            {
                arreglo[0] = cadena;
                longitud = 1;
            }

            for (var i = 0; i < longitud; i++)
            {

                var email = arreglo[i].Trim();
                var validacion = ValidarDirecccionCorreo(email);                

                if (validacion)
                {
                    nroCorreo++;
                }
                else
                {
                    if (email != "")
                        return -1;
                }

            }

            if (numMinCorreosObligatorios > nroCorreo)
                return -1;

            if (numMaxCorreosObligatorios > 0 && nroCorreo > numMaxCorreosObligatorios)
                return -1;

            return 1;
        }

        /// <summary>
        /// Valida el  formato de un email (correo)
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        static bool ValidarDirecccionCorreo(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Devuelve los errores en html
        /// </summary>
        /// <param name="listaErrores"></param>
        /// <returns></returns>
        public string ObtenerTablaErroresHtml(List<EmpresaCorreoErrorExcel> listaErrores)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table id='tablaErrores' class='pretty tabla-adicional'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style=''>Fila</th>");
            strHtml.Append("<th style=''>Columna</th>");
            strHtml.Append("<th style=''>Valor</th>");
            strHtml.Append("<th style=''>Mensaje de Validación</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");
            foreach (var fila in listaErrores)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td style='text-align: center;'>" + (fila.NumeroFilaExcel + 1) + " </td>");
                strHtml.Append("<td>" + fila.CampoExcel + " </td>");
                strHtml.Append("<td>" + fila.ValorCeldaExcel + " </td>");
                strHtml.Append("<td>" + fila.MensajeValidacion + " </td>");
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");


            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Valida la estructura del archivo adjuntar de interrupciones sin reportar
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="validaciones"></param>
        public void ValidarReporteIPRExcel(string path, string file,  out List<string> validaciones)
        {
            FileInfo fileInfo = new FileInfo(path + file);
            List<string> errores = new List<string>();

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Listado"];

                if (wsInterrupcion != null)
                {
                    //-- Verificar columnas 

                    List<string> columnas = new List<string>();
                    columnas.Add("Punto de Entrega");
                    columnas.Add("Fecha hora Inicio de interrupción");
                    columnas.Add("Suministrador");
                    columnas.Add("Tipo de Interrupción");
                    columnas.Add("Causa de Interrupción");
                    columnas.Add("Fecha Hora Fin");
                    columnas.Add("Tiempo Programado");
                    columnas.Add("");
                    columnas.Add("Responsable 1");
                    columnas.Add("");
                    columnas.Add("Responsable 2");
                    columnas.Add("");
                    columnas.Add("Responsable 3");
                    columnas.Add("");
                    columnas.Add("Responsable 4");
                    columnas.Add("");
                    columnas.Add("Responsable 5");
                    columnas.Add("");
                    columnas.Add("Observación");
                    columnas.Add("Campos Diferentes");

                    //int nroColumnas = 41;
                    //int columnaInicial = 1;
                    int nroColumnas = 20;
                    int columnaInicial = 1;
                    for (int i = columnaInicial; i <= nroColumnas; i++)
                    {
                        string header = (wsInterrupcion.Cells[6, i].Value != null) ? wsInterrupcion.Cells[6, i].Value.ToString() : string.Empty;
                        string colN = columnas[i - 1];
                        if (header.Trim() != colN.Trim())
                        {
                            errores.Add("El archivo adjuntado no tiene la estructura correcta. No puede cambiar o eliminar columnas del Excel.");
                            
                            break;
                        }
                    }
                }
                else
                {
                    errores.Add("No existe la hoja 'Listado' en el libro Excel.");
                }
            }
            validaciones = errores;

        }

        #endregion


        #endregion

        #region Métodos Tabla RE_EVENTO_MEDICION

        /// <summary>
        /// Permite obtener la estructura de mediciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="evento"></param>
        /// <returns></returns>
        public EstructuraMedicion ObteenerEstructuraMedicion(int empresa, int evento)
        {
            EstructuraMedicion resultado = new EstructuraMedicion();
            try
            {                
                ReEventoProductoDTO entityEvento = FactorySic.GetReEventoProductoRepository().GetById(evento);
                ReEventoSuministradorDTO entitySuministrador = FactorySic.GetReEventoSuministradorRepository().ObtenerSuministrador(evento, empresa);
                List<ReEventoMedicionDTO> listMedicion = FactorySic.GetReEventoMedicionRepository().ObtenerMedicion(evento, empresa);
                DateTime fechaInicio;
                DateTime fechaFin;
                UtilCalculoResarcimiento.ObtenerRangoFechasMedicion(entityEvento, out fechaInicio, out fechaFin);
                List<string[]> result = new List<string[]>();
                double factor = (double)(new ParametroAppServicio()).ObtenerValorParametro(ConstantesCalidadProducto.IdParametroFCU, DateTime.Now);
                resultado.FactorCompensacionUnitaria = factor;
                resultado.Tension = (entityEvento.Reevprtension != null) ? (decimal)entityEvento.Reevprtension : 0;

                int count = 0;
                while (true)
                {
                    DateTime fecha = fechaInicio.AddMinutes(count * 15);
                    string id = fecha.Year.ToString() + fecha.Month.ToString().PadLeft(2, '0') + fecha.Day.ToString().PadLeft(2, '0') +
                            fecha.Hour.ToString().PadLeft(2, '0') + fecha.Minute.ToString().PadLeft(2, '0');

                    ReEventoMedicionDTO medicion = listMedicion.Where(x => x.Fecha == id).FirstOrDefault();

                    string[] itemData = new string[10];
                    itemData[0] = (count + 1).ToString();
                    itemData[1] = fecha.ToString(ConstantesAppServicio.FormatoFechaFull2);

                    if (medicion != null)
                    {
                        itemData[2] = (medicion.Reemedtensionrs != null) ? ((decimal)medicion.Reemedtensionrs).ToString("0.000") : string.Empty;
                        itemData[3] = (medicion.Reemedtensionst != null) ? ((decimal)medicion.Reemedtensionst).ToString("0.000") : string.Empty;
                        itemData[4] = (medicion.Reemedtensiontr != null) ? ((decimal)medicion.Reemedtensiontr).ToString("0.000") : string.Empty;
                        itemData[5] = (medicion.Reemedvarp != null) ? ((decimal)medicion.Reemedvarp).ToString("0.000") : string.Empty;
                        itemData[6] = (medicion.Reemedvala != null) ? ((decimal)medicion.Reemedvala).ToString("0.00") : string.Empty;
                        itemData[7] = (medicion.Reemedvalap != null) ? ((decimal)medicion.Reemedvalap).ToString("0.00") : string.Empty;
                        itemData[8] = (medicion.Reemedvalep != null) ? ((decimal)medicion.Reemedvalep).ToString("0.0000") : string.Empty;
                        itemData[9] = (medicion.Reemedvalaapep != null) ? ((decimal)medicion.Reemedvalaapep).ToString("0.0000") : string.Empty;
                    }
                    else
                    {
                        itemData[2] = string.Empty;
                        itemData[3] = string.Empty;
                        itemData[4] = string.Empty;
                        itemData[5] = string.Empty;
                        itemData[6] = string.Empty;
                        itemData[7] = string.Empty;
                        itemData[8] = string.Empty;
                        itemData[9] = string.Empty;
                    }

                    result.Add(itemData);
                    count++;

                    if (fecha.Year == fechaFin.Year && fecha.Month == fechaFin.Month && fecha.Day == fechaFin.Day &&
                        fecha.Hour == fechaFin.Hour && fecha.Minute == fechaFin.Minute)
                    {
                        break;
                    }
                }

                resultado.Data = result.ToArray();
                resultado.Resarcimiento = (entitySuministrador != null) ? (entitySuministrador.Reevsuresarcimiento != null) ?
                    entitySuministrador.Reevsuresarcimiento.ToString() : string.Empty : string.Empty;

                resultado.Result = 1;
            }
            catch(Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                resultado.Result = -1;
            }
            return resultado;
        }

        /// <summary>
        /// Permite generar el formato de carga de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <param name="plantilla"></param>
        /// <param name="file"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public int GenerarFormatoCargaMedicion(string path, string plantilla, string file, int idEmpresa, int idEvento)
        {
            try
            {
                EstructuraMedicion result = this.ObteenerEstructuraMedicion(idEmpresa, idEvento);

                FileInfo template = new FileInfo(path + plantilla);
                FileInfo newFile = new FileInfo(path + file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet wsData = xlPackage.Workbook.Worksheets["Mediciones"];
                    wsData.Cells[2, 9].Value = result.Resarcimiento;
                    int index = 6;
                    foreach (string[] data in result.Data)
                    {
                        wsData.Cells[index, 1].Value = data[0];
                        wsData.Cells[index, 2].Value = data[1];
                        wsData.Cells[index, 3].Value = data[2];
                        wsData.Cells[index, 4].Value = data[3];
                        wsData.Cells[index, 5].Value = data[4];
                        wsData.Cells[index, 6].Value = data[5];
                        wsData.Cells[index, 7].Value = data[6];
                        wsData.Cells[index, 8].Value = data[7];
                        wsData.Cells[index, 9].Value = data[8];
                        wsData.Cells[index, 10].Value = data[9];

                        index++;
                    }

                    ExcelRange rg = wsData.Cells[6, 1, index - 1, 10];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Carga los datos a partir de excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="empresa"></param>
        /// <param name="evento"></param>        
        /// <returns></returns>
        public EstructuraMedicion CargarMedicionesExcel(string path, string file, int empresa, int evento)
        {
            EstructuraMedicion datos = this.ObteenerEstructuraMedicion(empresa, evento);
            try
            {
                List<string[]> result = new List<string[]>();
                FileInfo fileInfo = new FileInfo(path + file);
                List<string> errores = new List<string>();                

                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet wsData = xlPackage.Workbook.Worksheets["Mediciones"];

                    if (wsData != null)
                    {
                        if (wsData.Cells[2, 9].Value != null)
                        {
                            datos.Resarcimiento = wsData.Cells[2, 9].Value.ToString();
                        }

                        int index = 6;
                        for (int i = 0; i < datos.Data.Length; i++)
                        {
                            if (wsData.Cells[index + i, 2].Value != null)
                            {
                                if (wsData.Cells[index + i, 2].Value.ToString() != datos.Data[i][1])
                                {
                                    errores.Add("No debe eliminar filas");
                                }
                                else
                                {
                                    datos.Data[i][2] = (wsData.Cells[index + i, 3].Value != null) ? wsData.Cells[index + i, 3].Value.ToString() : string.Empty;
                                    datos.Data[i][3] = (wsData.Cells[index + i, 4].Value != null) ? wsData.Cells[index + i, 4].Value.ToString() : string.Empty;
                                    datos.Data[i][4] = (wsData.Cells[index + i, 5].Value != null) ? wsData.Cells[index + i, 5].Value.ToString() : string.Empty;
                                    datos.Data[i][5] = (wsData.Cells[index + i, 6].Value != null) ? wsData.Cells[index + i, 6].Value.ToString() : string.Empty;
                                    datos.Data[i][6] = (wsData.Cells[index + i, 7].Value != null) ? wsData.Cells[index + i, 7].Value.ToString() : string.Empty;
                                    datos.Data[i][7] = (wsData.Cells[index + i, 8].Value != null) ? wsData.Cells[index + i, 8].Value.ToString() : string.Empty;
                                    datos.Data[i][8] = (wsData.Cells[index + i, 9].Value != null) ? wsData.Cells[index + i, 9].Value.ToString() : string.Empty;
                                    datos.Data[i][9] = (wsData.Cells[index + i, 10].Value != null) ? wsData.Cells[index + i, 10].Value.ToString() : string.Empty;
                                }
                            }

                        }
                    }
                    else
                    {
                        errores.Add("No existe la hoja 'Mediciones' en el libro Excel.");
                    }
                }

                datos.Result = 1;

                if(errores.Count > 0)
                {
                    datos.Validaciones = errores;
                    datos.Result = -2;
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                datos.Result = -1;
            }
            return datos;
        }

        /// <summary>
        /// Permite grabar las mediciones
        /// </summary>
        /// <param name="data"></param>
        /// <param name="empresa"></param>
        /// <param name="evento"></param>
        /// <param name="username"></param>
        /// <param name="comentario"></param>
        /// <returns></returns>
        public int GrabarMedicion(string[][] data, int empresa, int evento, string username, string comentario)
        {
            try
            {
                decimal? resarcimiento = (!string.IsNullOrEmpty(data[0][9])) ? (decimal?)decimal.Parse(data[0][9]) : null;

                List<ReEventoMedicionDTO> entitys = new List<ReEventoMedicionDTO>();
                for (int i = 4; i < data.Length; i++)
                {
                    ReEventoMedicionDTO item = new ReEventoMedicionDTO();
                    item.Reevprcodi = evento;
                    item.Emprcodi = empresa;
                    item.Reemedusucreacion = username;
                    item.Reemedfeccreacion = DateTime.Now;
                    item.Reemedfechahora = DateTime.ParseExact(data[i][1], ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                    item.Reemedtensionrs = (!string.IsNullOrEmpty(data[i][2])) ? (decimal?)decimal.Parse(data[i][2]) : null;
                    item.Reemedtensionst = (!string.IsNullOrEmpty(data[i][3])) ? (decimal?)decimal.Parse(data[i][3]) : null;
                    item.Reemedtensiontr = (!string.IsNullOrEmpty(data[i][4])) ? (decimal?)decimal.Parse(data[i][4]) : null;
                    item.Reemedvarp = (!string.IsNullOrEmpty(data[i][5])) ? (decimal?)decimal.Parse(data[i][5]) : null;
                    item.Reemedvala = (!string.IsNullOrEmpty(data[i][6])) ? (decimal?)decimal.Parse(data[i][6]) : null;
                    item.Reemedvalap = (!string.IsNullOrEmpty(data[i][7])) ? (decimal?)decimal.Parse(data[i][7]) : null;
                    item.Reemedvalep = (!string.IsNullOrEmpty(data[i][8])) ? (decimal?)decimal.Parse(data[i][8]) : null;
                    item.Reemedvalaapep = (!string.IsNullOrEmpty(data[i][9])) ? (decimal?)decimal.Parse(data[i][9]) : null;
                    entitys.Add(item);
                }

                ReEventoSuministradorDTO suministrador = FactorySic.GetReEventoSuministradorRepository().ObtenerSuministrador(evento, empresa);
                suministrador.Reevsuindcarga = ConstantesAppServicio.SI;
                suministrador.Reevsuresarcimiento = resarcimiento;
                suministrador.Reevsuusucreacion = username;
                suministrador.Reevsufeccreacion = DateTime.Now;
                FactorySic.GetReEventoSuministradorRepository().Update(suministrador);

                FactorySic.GetReEventoMedicionRepository().Delete(evento, empresa);

                foreach (ReEventoMedicionDTO entity in entitys)
                {
                    FactorySic.GetReEventoMedicionRepository().Save(entity);
                }

                ReEventoLogenvioDTO logEnvio = new ReEventoLogenvioDTO();
                logEnvio.Emprcodi = empresa;
                logEnvio.Reevlofeccreacion = DateTime.Now;
                logEnvio.Reevloindcarga = ConstantesAppServicio.SI;
                logEnvio.Reevlomotivocarga = comentario;
                logEnvio.Reevlousucreacion = username;
                logEnvio.Reevprcodi = evento;
                FactorySic.GetReEventoLogenvioRepository().Save(logEnvio);

                return 1;
            }
            catch(Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public List<ReEventoLogenvioDTO> ObtenerEnviosMediciones(int idEmpresa, int idEvento)
        {
            List<ReEventoLogenvioDTO> entitys = FactorySic.GetReEventoLogenvioRepository().ObtenerEnvios(idEmpresa, idEvento);

            foreach(ReEventoLogenvioDTO entity in entitys)
            {
                entity.Fecha = (entity.Reevlofeccreacion != null) ? ((DateTime)entity.Reevlofeccreacion).
                    ToString(ConstantesAppServicio.FormatoFechaFull) : string.Empty;
            }

            return entitys;
        }

        #endregion

        #region Métodos Tabla RE_EVENTO_PRODUCTO

        /// <summary>
        /// Inserta un registro de la tabla RE_EVENTO_PRODUCTO
        /// </summary>
        public int GrabarEvento(ReEventoProductoDTO entity, string username)
        {
            try
            {                
                int id = 0;
                if(entity.Reevprcodi == 0)
                {
                    entity.Reevprfeccreacion = DateTime.Now;
                    entity.Reevprfecmodificacion = DateTime.Now;
                    entity.Reevprusucreacion = username;
                    entity.Reevprusumodificacion = username;
                    id = FactorySic.GetReEventoProductoRepository().Save(entity);
                }
                else
                {                  
                    entity.Reevprfecmodificacion = DateTime.Now;                    
                    entity.Reevprusumodificacion = username;
                    FactorySic.GetReEventoProductoRepository().Update(entity);
                    id = entity.Reevprcodi;
                }

                FactorySic.GetReEventoSuministradorRepository().Delete(id);

                List<int> involucrados = entity.Empresas.Split(',').Select(int.Parse).ToList();
                foreach (int idInvolucrado in involucrados)
                {
                    ReEventoSuministradorDTO itemSuministrador = new ReEventoSuministradorDTO();
                    itemSuministrador.Emprcodi = idInvolucrado;
                    itemSuministrador.Reevprcodi = id;
                    itemSuministrador.Reevsuestado = ConstantesAppServicio.Activo;
                    itemSuministrador.Reevsufeccreacion = DateTime.Now;
                    itemSuministrador.Reevsuusucreacion = username;
                    FactorySic.GetReEventoSuministradorRepository().Save(itemSuministrador);
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }
               
        /// <summary>
        /// Elimina un registro de la tabla RE_EVENTO_PRODUCTO
        /// </summary>
        public int EliminarEvento(int reevprcodi)
        {
            try
            {
                FactorySic.GetReEventoProductoRepository().Delete(reevprcodi);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RE_EVENTO_PRODUCTO
        /// </summary>
        public ReEventoProductoDTO GetByIdReEventoProducto(int reevprcodi)
        {
            return FactorySic.GetReEventoProductoRepository().GetById(reevprcodi);
        }
               
        /// <summary>
        /// Permite realizar búsquedas en la tabla ReEventoProducto
        /// </summary>
        public List<ReEventoProductoDTO> ConsultarEventosProducto(int anio, int mes)
        {
            List<ReEventoProductoDTO> entitys = FactorySic.GetReEventoProductoRepository().GetByCriteria(anio, mes).OrderBy(x => x.Reevprcodi).ToList();

            List<int> listIds = entitys.Select(x => x.Reevprcodi).Distinct().ToList();

            foreach(int id in listIds)
            {
                List<ReEventoProductoDTO> subList = entitys.Where(x => x.Reevprcodi == id).ToList();
                subList[0].Indicadorpadre = true;
                subList[0].Rowspan = subList.Count;
            }

            return entitys;
        }

        /// <summary>
        /// Permite obtener el listado de eventos por empresa
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="buscar"></param>
        /// <returns></returns>
        public List<ReEventoProductoDTO> ObtenerEventosPorSuministrador(int empresa, int anio, int mes, string buscar)
        {
            return FactorySic.GetReEventoProductoRepository().ObtenerEventosPorSuministrador(empresa, anio, mes, buscar);
        }

        /// <summary>
        /// Peemite obtener los años
        /// </summary>
        /// <returns></returns>
        public List<int> ListarAnios()
        {
            List<int> anios = new List<int>();
            for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 5; i--)
            {
                anios.Add(i);
            }
            return anios;
        }

        /// <summary>
        /// Permite obtener los suministradores de un evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public List<ReEmpresaDTO> ObtenerSuministradorPorEvento(int idEvento)
        {
            return FactorySic.GetReEventoSuministradorRepository().ObtenerSuministradoresPorEvento(idEvento);
        }

        /// <summary>
        /// Permite exportar los datos de ingresos de transmision
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int ExportarEventos(int anio, int mes, string path, string filename)
        {
            try
            {
                List<ReEventoProductoDTO> entitys = this.ConsultarEventosProducto(anio, mes);
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("EVENTOS");

                    if (ws != null)
                    {
                        int index = 1;

                        ws.Cells[index, 1].Value = "AÑO";
                        ws.Cells[index, 2].Value = "MES";
                        ws.Cells[index, 3].Value = "PUNTO DE ENTREGA";
                        ws.Cells[index, 4].Value = "TENSIÓN DE OPERACIÓN (V)";
                        ws.Cells[index, 5].Value = "RESPONSABLE 1";
                        ws.Cells[index, 6].Value = "% RESP. 1";
                        ws.Cells[index, 7].Value = "RESPONSABLE 2";
                        ws.Cells[index, 8].Value = "% RESP. 2";
                        ws.Cells[index, 9].Value = "RESPONSABLE 3";
                        ws.Cells[index, 10].Value = "% RESP. 3";
                        ws.Cells[index, 11].Value = "FECHA INICIAL";
                        ws.Cells[index, 12].Value = "FECHA FINAL";
                        ws.Cells[index, 13].Value = "SUMINISTRADOR";                      
                        ws.Cells[index, 14].Value = "COMENTARIO";
                        ws.Cells[index, 15].Value = "ACCESO";

                        ExcelRange rg = ws.Cells[index, 1, index, 15];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 2;
                        foreach (ReEventoProductoDTO item in entitys)
                        {
                            if (item.Indicadorpadre)
                            {
                                ws.Cells[index, 1].Value = item.Reevpranio;
                                ws.Cells[index, 1, index + item.Rowspan - 1, 1].Merge = true;
                                ws.Cells[index, 2].Value = item.Reevprmes;
                                ws.Cells[index, 2, index + item.Rowspan - 1, 2].Merge = true;
                                ws.Cells[index, 3].Value = item.Reevprptoentrega;
                                ws.Cells[index, 3, index + item.Rowspan - 1, 3].Merge = true;
                                ws.Cells[index, 4].Value = item.Reevprtension;
                                ws.Cells[index, 4, index + item.Rowspan - 1, 4].Merge = true;
                                ws.Cells[index, 5].Value = item.Responsablenomb1;
                                ws.Cells[index, 5, index + item.Rowspan - 1, 5].Merge = true;
                                ws.Cells[index, 6].Value = item.Reevprporc1;
                                ws.Cells[index, 6, index + item.Rowspan - 1, 6].Merge = true;
                                ws.Cells[index, 7].Value = item.Responsablenomb2;
                                ws.Cells[index, 7, index + item.Rowspan - 1, 7].Merge = true;
                                ws.Cells[index, 8].Value = item.Reevprporc2;
                                ws.Cells[index, 8, index + item.Rowspan - 1, 8].Merge = true;
                                ws.Cells[index, 9].Value = item.Responsablenomb3;
                                ws.Cells[index, 9, index + item.Rowspan - 1, 9].Merge = true;
                                ws.Cells[index, 10].Value = item.Reevprporc3;
                                ws.Cells[index, 10, index + item.Rowspan - 1, 10].Merge = true;
                                ws.Cells[index, 11].Value = (item.Reevprfecinicio != null) ?
                                    ((DateTime)item.Reevprfecinicio).ToString(ConstantesAppServicio.FormatoFechaFull) : string.Empty;
                                ws.Cells[index, 11, index + item.Rowspan - 1, 11].Merge = true;
                                ws.Cells[index, 12].Value = (item.Reevprfecfin != null) ?
                                    ((DateTime)item.Reevprfecfin).ToString(ConstantesAppServicio.FormatoFechaFull) : string.Empty;
                                ws.Cells[index, 12, index + item.Rowspan - 1, 12].Merge = true;
                                ws.Cells[index, 13].Value = item.Suministrador;
                                ws.Cells[index, 14].Value = item.Reevprcomentario;
                                ws.Cells[index, 14, index + item.Rowspan - 1, 14].Merge = true;
                                ws.Cells[index, 15].Value = item.Reevpracceso;
                                ws.Cells[index, 15, index + item.Rowspan - 1, 15].Merge = true;

                                rg = ws.Cells[index, 6, index, 6];
                                rg.Style.Numberformat.Format = "#0.00";
                                rg = ws.Cells[index, 8, index, 8];
                                rg.Style.Numberformat.Format = "#0.00";
                                rg = ws.Cells[index, 10, index, 10];
                                rg.Style.Numberformat.Format = "#0.00";
                            }
                            else
                            {
                                ws.Cells[index, 13].Value = item.Suministrador;
                            }

                            rg = ws.Cells[index, 1, index, 15];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[1, 1, index - 1, 15];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        rg = ws.Cells[1, 1, index - 1, 15];
                        rg.AutoFitColumns();
                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        #region Reporte Word Compensacion X MCP

        /// <summary>
        /// Genera el reporte de compensacion de mala calidad
        /// </summary>
        /// <param name="reevprcodi"></param>
        /// <param name="ruta"></param>
        /// <param name="nameFile"></param>
        public void GenerarReporteCompensacionPorMalaCalidadWord(int reevprcodi, string ruta, string nameFile)
        {            
            ReEventoProductoDTO evento = GetByIdReEventoProducto(reevprcodi);
           
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (DocX document = DocX.Create(rutaFile))
            {
                GenerarArchivoWordCompensacionPorMalaCalidad(document, evento);

                document.SaveAs(rutaFile);
            }
        }

        /// <summary>
        /// Genera la estructura y datos del reporte de compensacion de mala calidad
        /// </summary>
        /// <param name="document"></param>
        /// <param name="evento"></param>
        public void GenerarArchivoWordCompensacionPorMalaCalidad(DocX document, ReEventoProductoDTO evento)
        {
            #region Datos generales            
            decimal? t = evento.Reevprtension != null ? evento.Reevprtension / 1000 : null;
            string tension = t!= null ? t.ToString() : "";
            string mesEvento = EPDate.f_NombreMes(evento.Reevprmes.Value);
            int anioEvento = evento.Reevpranio.Value;            
            string barraNomb = evento.Reevprptoentrega != null ? evento.Reevprptoentrega.Trim() : "";
            string periodoIni = evento.Reevprfecinicio != null ? evento.Reevprfecinicio.Value.ToString("dd.MM.yyyy") : "";
            string periodoFin = evento.Reevprfecfin != null ? evento.Reevprfecfin.Value.ToString("dd.MM.yyyy") : "";

            List<string> empResponsables = new List<string>();
            string nombR1 = evento.Responsablenomb1;
            string nombR2 = evento.Responsablenomb2;
            string nombR3 = evento.Responsablenomb3;
            if (nombR1 != null) { if (nombR1.Trim() != "") { empResponsables.Add(nombR1.ToUpper()); } }
            if (nombR2 != null) { if (nombR2.Trim() != "") { empResponsables.Add(nombR2.ToUpper()); } }
            if (nombR3 != null) { if (nombR3.Trim() != "") { empResponsables.Add(nombR3.ToUpper()); } }
            string empresaNomb = String.Join(", ", empResponsables.OrderBy(x => x));
           
            ReporteCompensacionMalaCalidad reporte = ObtenerDatosReporteMalaCalidad(evento);
            #endregion

            string fontCalibri = "Calibri";
            string fontArial = "Arial";
            string fontHelvetica = "Helvetica";

            document.MarginLeft = 76.0f;
            document.MarginRight = 76.0f;
            //document.MarginLeft = 103.0f;
            //document.MarginRight = 103.0f;
            document.MarginTop = 205.0f;

            #region Headers footer y caratula

            // Add Header and Footer support to this document.
            document.AddHeaders();
            document.AddFooters();

            // Get the odd and even Headers for this document.
            Header header_odd = document.Headers.odd;

            // Get the odd and even Footer for this document.
            Footer footer_odd = document.Footers.odd;

            #region header
            // Insert a Paragraph into the odd Header.
            Novacode.Image logo = document.AddImage(AppDomain.CurrentDomain.BaseDirectory + "Content/Images/" + "Coes.png");
            

            Table header_first_table = header_odd.InsertTable(1, 3);
            header_first_table.Design = TableDesign.TableGrid;
            header_first_table.AutoFit = AutoFit.ColumnWidth;


            header_first_table.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            header_first_table.Rows[0].Cells[0].Width = 150;

            header_first_table.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
            header_first_table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
            header_first_table.Rows[0].Cells[1].Width = 300;

            header_first_table.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
            header_first_table.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;
            header_first_table.Rows[0].Cells[2].Width = 200;

            //primera fila
            Paragraph upperRightParagraph = header_odd.Tables[0].Rows[0].Cells[0].Paragraphs[0];
            upperRightParagraph.AppendPicture(logo.CreatePicture(70, 135));
            upperRightParagraph.Alignment = Alignment.left;
            header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;

            Paragraph cabcentro = header_odd.Tables[0].Rows[0].Cells[1].Paragraphs[0];
            cabcentro.AppendLine();
            cabcentro.Append(string.Format("INFORME COES/D/DO/SEV-INF-XXX-{0}", anioEvento) ).Font(new FontFamily(fontCalibri)).Bold().FontSize(12);
            cabcentro.AppendLine();
            cabcentro.AppendLine();
            cabcentro.Append("CÁLCULO DE RESARCIMIENTOS EN APLICACIÓN DEL NUMERAL 3.5 DE LA NTCSE - CALIDAD DE PRODUCTO").Font(new FontFamily(fontArial)).Bold().FontSize(10);
            cabcentro.AppendLine();

            Paragraph cabDerecha = header_odd.Tables[0].Rows[0].Cells[2].Paragraphs[0];
            cabDerecha.AppendLine();
            cabDerecha.Append("SUBDIRECCIÓN DE EVALUACIÓN").Font(new FontFamily(fontCalibri)).Bold().FontSize(10);
            cabDerecha.AppendLine();
            cabDerecha.AppendLine();
            cabDerecha.Append("CORRESPONDIENTE:").Font(new FontFamily(fontArial)).Bold().FontSize(8);
            cabDerecha.AppendLine();
            cabDerecha.Append(string.Format("{0} {1} ", mesEvento.ToUpper(), anioEvento)).Font(new FontFamily(fontCalibri)).Bold().FontSize(10);
            cabDerecha.AppendLine();


            #endregion

            #region Footer

            Table footer_table = footer_odd.InsertTable(1, 1);
            footer_table.Design = TableDesign.TableNormal;
            footer_table.AutoFit = AutoFit.ColumnWidth;

            footer_table.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            footer_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            footer_table.Rows[0].Cells[0].Width = 680;

            Paragraph numero_pagina = footer_odd.Tables[0].Rows[0].Cells[0].Paragraphs[0];
            numero_pagina.Append("Pág. ").Font(new FontFamily(fontCalibri)).Bold().FontSize(10);
            numero_pagina.AppendPageNumber(PageNumberFormat.normal);
            numero_pagina.Append(" de ").Font(new FontFamily(fontCalibri)).Bold().FontSize(10);
            numero_pagina.AppendPageCount(PageNumberFormat.normal);

            #endregion

            // Force odd & even pages to have different Headers and Footers.
            document.DifferentOddAndEvenPages = true;
            document.DifferentFirstPage = false; //para el header y pie de pagina aparezcan desde la pagina 1

            #endregion           

            Paragraph espacioPage3_1 = document.InsertParagraph();
            espacioPage3_1.IndentationFirstLine = 20;
            espacioPage3_1.Append("  ");

            var bulletedList = document.AddList("OBJETIVO", 0, ListItemType.Numbered, 1);
            document.AddListItem(bulletedList, "BASE LEGAL");
            document.AddListItem(bulletedList, "TRANSGRESIONES A LA NTCSE POR CALIDAD DE PRODUCTO POR TENSIÓN");
            document.AddListItem(bulletedList, "RESARCIMIENTO DE CALIDAD DE PRODUCTO POR TENSIÓN");                        

            List<Paragraph> actualListData = bulletedList.Items;

            #region seccion 1

            Paragraph pListas1 = document.InsertParagraph();
            pListas1.InsertParagraphBeforeSelf(actualListData.ElementAt(0).Font(new FontFamily(fontArial)).Bold().FontSize(12));

            Paragraph pa1 = document.InsertParagraph();
            pa1.Alignment = Alignment.both;  //justify
            pa1.IndentationBefore = 0.7f;
            pa1.Append(string.Format("Determinar los Resarcimientos por calidad de producto por tensión de los agentes del SEIN, en conformidad al inciso c) del Numeral 3.5 de la Norma Técnica de Calidad de los Servicios Eléctricos - NTCSE correspondiente a la barra de {0} KV de la S.E. {1} correspondiente al mes de {2} {3}.", tension, barraNomb, mesEvento, anioEvento))
                .Font(new FontFamily(fontHelvetica)).FontSize(12);

            Paragraph paS1_1 = document.InsertParagraph();
            paS1_1.AppendLine();
            paS1_1.AppendLine();

            #endregion

            #region seccion 2
            Paragraph pListas2 = document.InsertParagraph();            
            actualListData.ElementAt(1).Font(new FontFamily(fontArial)).FontSize(12);
            pListas2.InsertParagraphBeforeSelf(actualListData.ElementAt(1)).Bold();
            pListas2.Bold();

            var bulletedListP2 = document.AddList("Mediante Decreto Supremo Nº 020-97-EM se aprobó la Norma Técnica de Calidad de los Servicios Eléctricos (NTCSE), a fin de garantizar a los usuarios una entrega eléctrica continua, adecuada, confiable y oportuna.", 1, ListItemType.Numbered);
            document.AddListItem(bulletedListP2, "El numeral 3.5 de la NTCSE, en concordancia con el literal i) del artículo 14º de la Ley Nº 28832, establece la obligación del Comité de Operación Económica del Sistema (COES) de investigar e identificar a los responsables ante casos de transgresiones a la calidad del producto y/o entrega en el Sistema Eléctrico Interconectado Nacional (SEIN), así como calcular las compensaciones que deben resarcir los responsables a los Suministradores afectados.", 1, ListItemType.Numbered);
            document.AddListItem(bulletedListP2, "Mediante la Resolución Ministerial N° 237-2012-M EM/DM publicada el 20.05.2012 se aprobó el PR-N° 40 “Procedimiento para la Aplicación del Numeral 3.5 de la NTCSE”.", 1, ListItemType.Numbered);
         

            List<Paragraph> actualListDataP2 = bulletedListP2.Items;

            Paragraph pListasP2_1 = document.InsertParagraph();
            actualListDataP2.ElementAt(0).Alignment = Alignment.both;  //justify A
            pListasP2_1.InsertParagraphBeforeSelf(actualListDataP2.ElementAt(0).Font(new FontFamily(fontHelvetica)).FontSize(12));

            Paragraph pListasP2_2 = document.InsertParagraph();
            actualListDataP2.ElementAt(1).Alignment = Alignment.both;  //justify B
            pListasP2_2.InsertParagraphBeforeSelf(actualListDataP2.ElementAt(1).Font(new FontFamily(fontHelvetica)).FontSize(12));

            Paragraph pListasP2_3 = document.InsertParagraph();
            actualListDataP2.ElementAt(2).Alignment = Alignment.both;  //justify C
            pListasP2_3.InsertParagraphBeforeSelf(actualListDataP2.ElementAt(2).Font(new FontFamily(fontHelvetica)).FontSize(12));
            pListasP2_3.AppendLine();

            #endregion

            #region seccion 3
            Paragraph pListas3 = document.InsertParagraph();            
            pListas3.InsertParagraphBeforeSelf(actualListData.ElementAt(2).Font(new FontFamily(fontArial)).Bold().FontSize(12));

            Paragraph paS3_1 = document.InsertParagraph();
            paS3_1.Alignment = Alignment.both;  //justify
            paS3_1.IndentationBefore = 0.7f;
            paS3_1.Append(string.Format("El COES mediante documento COES/D/DO-435-2019 del 02.07.2019, señaló que la responsabilidad de la mala calidad de producto por tensión en la barra {0} kV entre el periodo del {1} al {2}, recae en la(s) empresa(s) {3} debido a que no realizó las acciones necesarias en la supervisión y control de tensión en dicha subestación.", barraNomb, periodoIni, periodoFin, empresaNomb.ToUpper()))
                .Font(new FontFamily(fontHelvetica)).FontSize(12);


            Paragraph paS3_2 = document.InsertParagraph();
            paS3_2.Alignment = Alignment.both;  //justify
            paS3_2.IndentationBefore = 0.7f;
            paS3_2.AppendLine("Asimismo, el numeral 5.0.7 de la NTCSE, señala que las compensaciones se seguirán aplicando mensualmente hasta que se haya subsanado la falta y a través de un nuevo Período de Medición se haya comprobado que la Calidad de Producto satisface los estándares fijados por la Norma.  ")
                .Font(new FontFamily(fontHelvetica)).FontSize(12);

            Paragraph paS3_3 = document.InsertParagraph();
            paS3_3.AppendLine();
            paS3_3.AppendLine();
            paS3_3.AppendLine();
            paS3_3.AppendLine();

            #endregion

            #region seccion 4
            Paragraph pListas4 = document.InsertParagraph();            
            pListas4.InsertParagraphBeforeSelf(actualListData.ElementAt(3).Font(new FontFamily(fontArial)).Bold().FontSize(12));

            var bulletedList2 = document.AddList("INFORMACIÓN PROPORCIONADA POR LOS AGENTES", 1, ListItemType.Numbered, 1);
            document.AddListItem(bulletedList2, "RESARCIMIENTOS", 1, ListItemType.Numbered, 1);
            List<Paragraph> actualListData2 = bulletedList2.Items;

            Paragraph pListas4_1 = document.InsertParagraph();
            pListas4_1.InsertParagraphBeforeSelf(actualListData2.ElementAt(0).Font(new FontFamily(fontArial)).Bold().FontSize(12));

            Paragraph paS4_1 = document.InsertParagraph();
            paS4_1.Alignment = Alignment.both;  //justify
            paS4_1.IndentationBefore = 1.4f;           
            paS4_1.Append(string.Format("Para la transgresión ocurrida en el mes de {0} de {1}, el COES ha recibido la información relativa a las compensaciones efectuadas a sus clientes por la referida mala calidad de producto con la finalidad de elaborar el informe de Resarcimientos correspondiente, la cual ha sido considerada en el presente informe.  ", mesEvento, anioEvento))
                .Font(new FontFamily(fontHelvetica)).FontSize(12);

            Paragraph paS4_2 = document.InsertParagraph();
            paS4_2.AppendLine();

            Paragraph pListas4_2_ = document.InsertParagraph();
            pListas4_2_.InsertParagraphBeforeSelf(actualListData2.ElementAt(1).Font(new FontFamily(fontArial)).Bold().FontSize(12));

            Paragraph paS4_3 = document.InsertParagraph();
            paS4_3.Alignment = Alignment.both;  //justify
            paS4_3.IndentationBefore = 1.4f;
            paS4_3.Append("Con base a la información proporcionada por los agentes suministradores y la carta COES/D/DO-435-2019, respecto a la asignación de responsabilidad por mala calidad de tensión, se ha indican los siguientes montos de resarcimientos: ")
                .Font(new FontFamily(fontHelvetica)).FontSize(12);

            Paragraph paS4_4 = document.InsertParagraph();
            paS4_4.AppendLine();

            #endregion

            //Muestro tabla
            CrearTablaReporteMalaCalidad(document, reporte);

            Paragraph pFecha = document.InsertParagraph();
            pFecha.Alignment = Alignment.right;
            pFecha.AppendLine(string.Format("Lima, {0} de {1} del {2}", string.Format("{0:D2}", DateTime.Now.Day), EPDate.f_NombreMes(DateTime.Now.Month), DateTime.Now.Year)).Font(new FontFamily(fontHelvetica)).FontSize(12);


        }

        /// <summary>
        /// Devuelve los datos de la tabla del reporte de compensacion de mala calidad
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        public ReporteCompensacionMalaCalidad ObtenerDatosReporteMalaCalidad(ReEventoProductoDTO evento)
        {
            ReporteCompensacionMalaCalidad reporte = new ReporteCompensacionMalaCalidad();

            //Obtengo el total de resarcimientos
            List<ReEventoSuministradorDTO> lstDatosResarcimientos = FactorySic.GetReEventoSuministradorRepository().ListarPorEvento(evento.Reevprcodi);

            //Obtengo suministradores (ordenados por nombre) por evento
            List<ReEmpresaDTO> listaSuministradores = ObtenerEmpresasSuministradoras(evento.Reevprcodi).OrderBy(x=>x.Emprnomb).ToList();

            Dictionary<int, string> lstTemp = new Dictionary<int, string>();
            Dictionary<int, string> lstResp = new Dictionary<int, string>();
            if (evento.Reevprempr1 != null)
                lstTemp.Add(evento.Reevprempr1.Value, evento.Responsablenomb1.ToUpper().Trim());
            if (evento.Reevprempr2 != null)
                lstTemp.Add(evento.Reevprempr2.Value, evento.Responsablenomb2.ToUpper().Trim());
            if (evento.Reevprempr3 != null)
                lstTemp.Add(evento.Reevprempr3.Value, evento.Responsablenomb3.ToUpper().Trim());
            
            //Ordeno a los responsables por su nombre
            lstResp = lstTemp.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            Dictionary<int, string> lstSum = new Dictionary<int, string>();
            foreach (var sumi in listaSuministradores)
            {
                lstSum.Add(sumi.Emprcodi, sumi.Emprnomb.ToUpper().Trim());
            }            

            //Obtengo la data de resarcimientos.
            List<RegistroReporteCompensacionMalaCalidad> data = new List<RegistroReporteCompensacionMalaCalidad>();            

            foreach (var sum in lstSum)
            {
                decimal? resarcimientoTotal = lstDatosResarcimientos.Find(x => x.Emprcodi == sum.Key).Reevsuresarcimiento;

                foreach (var resp in lstResp)
                {
                    //Obtenemos el porcentaje del responsable
                    decimal porcentaje = -1;
                    if (evento.Reevprempr1 == resp.Key)
                        porcentaje = evento.Reevprporc1.Value;
                    else
                    {
                        if (evento.Reevprempr2 == resp.Key)
                            porcentaje = evento.Reevprporc2.Value;
                        else
                        {
                            if (evento.Reevprempr3 == resp.Key)
                                porcentaje = evento.Reevprporc3.Value;
                        }
                    }

                    RegistroReporteCompensacionMalaCalidad regX = new RegistroReporteCompensacionMalaCalidad();
                    regX.ResponsableId = resp.Key;
                    regX.SuministradorId = sum.Key;

                    decimal? valResc;
                    decimal? valNulo = null;
                    if (resarcimientoTotal != null)
                    {
                        valResc = resarcimientoTotal * porcentaje / 100;
                    }
                    else
                    {
                        valResc = valNulo;
                    }
                    regX.ValorResarcimiento = valResc;

                    //Agrego el registro al listado
                    data.Add(regX);
                }
            }

            reporte.ListaResponsables = lstResp;
            reporte.ListaSuministradores = lstSum;
            reporte.ListaDatos = data;

            return reporte;
        }

        /// <summary>
        /// Genera la tabla de resarcimiento en el reporte de compensacion por mala calidad
        /// </summary>
        /// <param name="document"></param>
        /// <param name="reporte"></param>
        private void CrearTablaReporteMalaCalidad(DocX document, ReporteCompensacionMalaCalidad reporte)
        {
            string fontCalibri = "Calibri";
            
            int numResponsables = reporte.ListaResponsables.Count;
            int numSuministradores = reporte.ListaSuministradores.Count;
            int numColumnasH = 1 + numResponsables;
            int numFilasH = 3 + numSuministradores;

            Table tableHorizontal = document.InsertTable(numFilasH, numColumnasH);
            tableHorizontal.Design = TableDesign.TableGrid;
            tableHorizontal.AutoFit = AutoFit.Contents;
            //tableHorizontal.AutoFit = AutoFit.Window;
            tableHorizontal.Alignment = Alignment.center;

            tableHorizontal.Rows[0].Height = 40;
            tableHorizontal.Rows[2].Height = 40;

            ////Alineo Vertical y horizontalmente a centro la cabecera            
            tableHorizontal.Rows[2].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            tableHorizontal.Rows[2].Cells[0].Paragraphs[0].Alignment = Alignment.center;

            for (int i1 = 0; i1 < numResponsables; i1++)
            {
                if (i1 == 0)
                {
                    tableHorizontal.Rows[0].Cells[1 + i1].MarginTop = 8; //Vertical no funciona, por eso aplicamos padding
                    tableHorizontal.Rows[2].Cells[1 + i1].MarginTop = 8; //Vertical no funciona, por eso aplicamos padding
                }
                
                tableHorizontal.Rows[0].Cells[1 + i1].Paragraphs[0].Alignment = Alignment.center;
                tableHorizontal.Rows[1].Cells[1 + i1].VerticalAlignment = VerticalAlignment.Center;
                tableHorizontal.Rows[1].Cells[1 + i1].Paragraphs[0].Alignment = Alignment.center;
                
                tableHorizontal.Rows[2].Cells[1 + i1].Paragraphs[0].Alignment = Alignment.center;
            }
           

            //Colores de celdas cabecera
            tableHorizontal.Rows[0].Cells[0].FillColor = ColorTranslator.FromHtml("#FFFFFF");
            tableHorizontal.Rows[1].Cells[0].FillColor = ColorTranslator.FromHtml("#FFFFFF");
            tableHorizontal.Rows[2].Cells[0].FillColor = ColorTranslator.FromHtml("#D9D9D9");//Suministradores

            for (int i2 = 0; i2 < numResponsables; i2++)
            {
                tableHorizontal.Rows[0].Cells[1 + i2].FillColor = ColorTranslator.FromHtml("#C2CADB"); //Titulo Responsables
                tableHorizontal.Rows[1].Cells[1 + i2].FillColor = ColorTranslator.FromHtml("#C2CADB"); //Responsables
                tableHorizontal.Rows[2].Cells[1 + i2].FillColor = ColorTranslator.FromHtml("#E3E7F0"); //Resarcimiento
            }

            //Textos            
            tableHorizontal.Rows[2].Cells[0].Paragraphs[0].Append("Suministrador").FontSize(12).Font(new FontFamily(fontCalibri)).Bold();

            int i = 0;
            foreach (var responsable in reporte.ListaResponsables)
            {
                if (i == 0)
                {
                    tableHorizontal.Rows[0].Cells[1].Paragraphs[0].Append("Responsable").FontSize(12).Font(new FontFamily(fontCalibri)).Bold();                    
                    tableHorizontal.Rows[2].Cells[1].Paragraphs[0].Append("Resarcimiento a suministradores (US$)").FontSize(12).Font(new FontFamily(fontCalibri)).Bold();
                }

                tableHorizontal.Rows[1].Cells[1 + i].Paragraphs[0].Append(responsable.Value).FontSize(12).Font(new FontFamily(fontCalibri)).Bold();

                i++;
            }           

            //Al final hago merge en la cabecera
            tableHorizontal.MergeCellsInColumn(0, 0, 1);
            if (numResponsables > 1)
            {
                tableHorizontal.Rows[0].MergeCells(1, numResponsables);
                tableHorizontal.Rows[2].MergeCells(1, numResponsables);
            }

            //Ingresar data en el cuerpo
            int filaSumX = 0;
            foreach (var suministrador in reporte.ListaSuministradores)
            {
                tableHorizontal.Rows[3 + filaSumX].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                tableHorizontal.Rows[3 + filaSumX].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                tableHorizontal.Rows[3 + filaSumX].Cells[0].FillColor = ColorTranslator.FromHtml("#D9D9D9");//Suministradores
                tableHorizontal.Rows[3 + filaSumX].Cells[0].Paragraphs[0].Append(suministrador.Value).FontSize(12).Font(new FontFamily(fontCalibri)).Bold();

                int colResp = 0;
                foreach (var responsable in reporte.ListaResponsables)
                {
                    tableHorizontal.Rows[3 + filaSumX].Cells[1 + colResp].VerticalAlignment = VerticalAlignment.Center;
                    tableHorizontal.Rows[3 + filaSumX].Cells[1 + colResp].Paragraphs[0].Alignment = Alignment.center;

                    RegistroReporteCompensacionMalaCalidad regDato = reporte.ListaDatos.Find(x => x.SuministradorId == suministrador.Key && x.ResponsableId == responsable.Key);
                    string valR = regDato != null ? (regDato.ValorResarcimiento != null ? (regDato.ValorResarcimiento.Value.ToString("#,##0.00")) : "") : "";
                    tableHorizontal.Rows[3 + filaSumX].Cells[1 + colResp].Paragraphs[0].Append(valR).FontSize(12).Font(new FontFamily(fontCalibri)).Bold();

                    colResp++;
                }

                filaSumX++;
            }

            Paragraph paSbCX = document.InsertParagraph();
            paSbCX.AppendLine();
            paSbCX.AppendLine();
        }


        /// <summary>
        /// Central una celda horizontal y verticalmente
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="fila"></param>
        /// <param name="col"></param>
        public void CentralCelda(Table tabla, int fila, int col)
        {
            tabla.Rows[fila].Cells[col].VerticalAlignment = VerticalAlignment.Center;
            tabla.Rows[fila].Cells[col].Paragraphs[0].Alignment = Alignment.center;
        }

        /// <summary>
        /// Rellena una celda con un texto
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="fila"></param>
        /// <param name="col"></param>
        /// <param name="tamLetra"></param>
        /// <param name="tipoTexto"></param>
        /// <param name="enNegrita"></param>
        /// <param name="texto"></param>
        public void LlenarCeldaTabla(Table tabla, int fila, int col, int tamLetra, string tipoTexto, bool enNegrita, string texto)
        {
            if (enNegrita)
                tabla.Rows[fila].Cells[col].Paragraphs[0].Append(texto).FontSize(tamLetra).Font(new FontFamily(tipoTexto)).Bold();
            else
                tabla.Rows[fila].Cells[col].Paragraphs[0].Append(texto).FontSize(tamLetra).Font(new FontFamily(tipoTexto));
        }
        #endregion

        #region Notificacion de Correos

        /// <summary>
        /// Devuelve los datos de la plantila de compensacion de mala calidad de producto
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        public SiCorreoDTO ObtenerDatosCorreoCompensacionMalaCalidad(ReEventoProductoDTO evento)
        {
            SiCorreoDTO salida = new SiCorreoDTO();

            int tipoCorreo = Int32.Parse(ConstantesCalidadProducto.IdPlantillaSolicitudEnvíoCompensacionesMalaCalidadProducto);
            SiPlantillacorreoDTO plantillaUsar = FactorySic.GetSiPlantillacorreoRepository().GetById(tipoCorreo);
            List<VariableCorreo> listaVariables = LlenarVariablesCorreoCompensacionMalaCalidad(evento);

            //Obtengo todos los correos PARA
            List<SiEmpresaCorreoDTO> lstEmpresasCorreoResarcimientos = FactorySic.GetSiEmpresaCorreoRepository().ListarSoloResarcimiento();
            List<int> lstEmprcodisResarcimientos = lstEmpresasCorreoResarcimientos.Select(x => x.Emprcodi).Distinct().ToList();            
            List<ReEmpresaDTO> listaEmpresasAEnviar = ObtenerEmpresasSuministradoras(evento.Reevprcodi);            
            List<ReEmpresaDTO> lstEmpresasFinalEnviar = listaEmpresasAEnviar.Where(x => lstEmprcodisResarcimientos.Contains(x.Emprcodi)).ToList();
            List<string> lstPara = new List<string>();
            foreach (ReEmpresaDTO empresa in lstEmpresasFinalEnviar)
            {                
                string paraEmpresa = ObtenerDestinatariosPorEmpresa("", empresa.Emprcodi);
                lstPara.Add(paraEmpresa);
            }


            salida.Corrto = String.Join("; ", lstPara); 
            salida.Corrcc = plantillaUsar.PlanticorreosCc;
            salida.Corrbcc = plantillaUsar.PlanticorreosBcc;           
            salida.Corrasunto = LLenarDatosVariables(plantillaUsar.Plantasunto, tipoCorreo, listaVariables);            
            salida.Corrcontenido = LLenarDatosVariables(plantillaUsar.Plantcontenido, tipoCorreo, listaVariables);

            return salida;
        }

        /// <summary>
        ///  Develve una lista de variables con los datos llenados
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        public List<VariableCorreo> LlenarVariablesCorreoCompensacionMalaCalidad(ReEventoProductoDTO evento)
        {
            

            List<VariableCorreo> listaVariables = ObtenerListadoVariables(Int32.Parse(ConstantesCalidadProducto.IdPlantillaSolicitudEnvíoCompensacionesMalaCalidadProducto), ConstantesCalidadProducto.VariableAsunto);

            VariableCorreo obj20 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValMesAnioEvento); //agosto del 2022
            obj20.ValorConDato = EPDate.f_NombreMes(evento.Reevprmes.Value) + " del " + evento.Reevpranio.Value;

            VariableCorreo obj21 = listaVariables.Find(x => x.Valor == ConstantesCalidadProducto.ValPuntoEntregaEvento); // Huallanca 
            obj21.ValorConDato = evento.Reevprptoentrega != null ? evento.Reevprptoentrega.Trim() : "";
            

            return listaVariables;
        }

        /// <summary>
        /// Valida que las empresas tengan correos registrados
        /// </summary>
        /// <param name="listaEmpresas"></param>
        /// <param name="ningunaEmpresaConCorreo"></param>
        /// <returns></returns>
        public string ValidarExistenciaCorreoEmpresas(List<ReEmpresaDTO> listaEmpresas, out bool ningunaEmpresaConCorreo)
        {
            string salida = "";
            ningunaEmpresaConCorreo = false;
            List<string> lstEmpresasSinCorreo = new List<string>();

            //Obtengo todas las empresas con correos registrados para resarcimientos
            List<SiEmpresaCorreoDTO> lstEmpresasCorreoResarcimientos = FactorySic.GetSiEmpresaCorreoRepository().ListarSoloResarcimiento();                       

            foreach (var empresa in listaEmpresas)
            {
                SiEmpresaCorreoDTO empConCorreo = lstEmpresasCorreoResarcimientos.Find(x => x.Emprcodi == empresa.Emprcodi);

                if(empConCorreo == null)
                {
                    lstEmpresasSinCorreo.Add(empresa.Emprnomb.Trim());
                }
            }

            if(lstEmpresasSinCorreo.Any())
                salida = String.Join(", ", lstEmpresasSinCorreo);

            if (listaEmpresas.Any())
                if (listaEmpresas.Count == lstEmpresasSinCorreo.Count)
                    ningunaEmpresaConCorreo = true;

            return salida;
        }

        /// <summary>
        /// Envia correo de compensacion de mala calidad
        /// </summary>
        /// <param name="correo"></param>
        /// <param name="reevprcodi"></param>
        /// <param name="usuario"></param>
        public void EnviarMensajeCompensacionMalaCalidad(SiCorreoDTO correo, int reevprcodi, string usuario)
        {

            int tipoCorreo = Int32.Parse(ConstantesCalidadProducto.IdPlantillaSolicitudEnvíoCompensacionesMalaCalidadProducto);

            //Obtengo la plantilla a usar
            SiPlantillacorreoDTO plantillaUsar = FactorySic.GetSiPlantillacorreoRepository().GetById(tipoCorreo);
            if (plantillaUsar == null)
            {
                throw new Exception("No se encontró una plantilla de correo para el tipo de correo elegido.");
            }
            
            ReEventoProductoDTO evento = GetByIdReEventoProducto(reevprcodi);
            
            List<VariableCorreo> listaVariables = new List<VariableCorreo>();

            //Obtengo el tipo de correo
            int valTC = Int32.Parse(ObtenerTipoCorreo(tipoCorreo.ToString()));

            //Obtengo todas las empresas con correos registrados para resarcimientos
            List<SiEmpresaCorreoDTO> lstEmpresasCorreoResarcimientos = FactorySic.GetSiEmpresaCorreoRepository().ListarSoloResarcimiento();
            List<int> lstEmprcodisResarcimientos = lstEmpresasCorreoResarcimientos.Select(x => x.Emprcodi).Distinct().ToList();


            //Obtengo las empresas a la cual se enviaran los correos
            List<ReEmpresaDTO> listaEmpresasAEnviar = ObtenerEmpresasSuministradoras(evento.Reevprcodi);            

            //Obtengo las empresas que tienen correos registrados y a los que se enviaran los correos
            List<ReEmpresaDTO> lstEmpresasFinalEnviar = listaEmpresasAEnviar.Where(x => lstEmprcodisResarcimientos.Contains(x.Emprcodi)).ToList();


            foreach (ReEmpresaDTO empresa in lstEmpresasFinalEnviar)
            {
                listaVariables = LlenarVariablesCorreoCompensacionMalaCalidad(evento);

                SiCorreoDTO newCorreo = new SiCorreoDTO();

                newCorreo.Corrto = ObtenerDestinatariosPorEmpresa("", empresa.Emprcodi);
                newCorreo.Corrcc = correo.Corrcc;
                newCorreo.Corrbcc = correo.Corrbcc;
                newCorreo.Corrasunto = LLenarDatosVariables(plantillaUsar.Plantasunto, tipoCorreo, listaVariables);
                newCorreo.Corrcontenido = LLenarDatosVariables(plantillaUsar.Plantcontenido, tipoCorreo, listaVariables);


                if (newCorreo.Corrto == "")
                    throw new ArgumentException("No se encontró destinatarios para el envío de correos.");
                if (newCorreo.Corrasunto == "")
                    throw new ArgumentException("No se encontró asunto para el envío de correos.");
                if (newCorreo.Corrcontenido == "")
                    throw new ArgumentException("No se encontró contenido para el envío de correos.");

                

                List<string> lstArchivos = new List<string>();

                //LogCorreo
                ReLogcorreoDTO lcorreo = new ReLogcorreoDTO();
                
                lcorreo.Retcorcodi = valTC;
                lcorreo.Relcorasunto = newCorreo.Corrasunto;
                lcorreo.Relcorto = newCorreo.Corrto;
                lcorreo.Relcorcc = newCorreo.Corrcc;
                lcorreo.Relcorbcc = newCorreo.Corrbcc;
                lcorreo.Relcorcuerpo = newCorreo.Corrcontenido;
                lcorreo.Relcorusucreacion = usuario;
                lcorreo.Relcorfeccreacion = DateTime.Now;
                lcorreo.Relcorempresa = empresa.Emprcodi;
                lcorreo.Relcorarchivosnomb = String.Join("/", lstArchivos);

                EnviarMensajeAEmpresa(newCorreo, lcorreo, usuario, tipoCorreo, lstArchivos);
            }

        }

        /// <summary>
        /// Devuelve el listado de empresas suministradoras relacionadas al evento.
        /// </summary>
        /// <param name="reevprcodi"></param>
        /// <returns></returns>
        public List<ReEmpresaDTO> ObtenerEmpresasSuministradoras(int reevprcodi)
        {
            
            List<ReEmpresaDTO> listaSuministradores = ObtenerSuministradorPorEvento(reevprcodi);

            return listaSuministradores;
        }
        #endregion

        #endregion

        #region Métodos Tabla RE_INTERRUPCION_ACCESO

        /// <summary>
        /// Inserta un registro de la tabla RE_INTERRUPCION_ACCESO
        /// </summary>
        public void SaveReInterrupcionAcceso(ReInterrupcionAccesoDTO entity)
        {
            try
            {
                FactorySic.GetReInterrupcionAccesoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda registros en grupo
        /// </summary>
        /// <param name="list"></param>
        public void SaveReInterrupcionAccesoEnGrupo(List<ReInterrupcionAccesoDTO> list)
        {
            try
            {
                FactorySic.GetReInterrupcionAccesoRepository().GrabarEnGrupo(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RE_INTERRUPCION_ACCESO
        /// </summary>
        public void UpdateReInterrupcionAcceso(ReInterrupcionAccesoDTO entity)
        {
            try
            {
                FactorySic.GetReInterrupcionAccesoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RE_INTERRUPCION_ACCESO
        /// </summary>
        public void DeleteReInterrupcionAcceso(int reinaccodi)
        {
            try
            {
                FactorySic.GetReInterrupcionAccesoRepository().Delete(reinaccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RE_INTERRUPCION_ACCESO
        /// </summary>
        public ReInterrupcionAccesoDTO GetByIdReInterrupcionAcceso(int reinaccodi)
        {
            return FactorySic.GetReInterrupcionAccesoRepository().GetById(reinaccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RE_INTERRUPCION_ACCESO
        /// </summary>
        public List<ReInterrupcionAccesoDTO> ListReInterrupcionAccesos()
        {
            return FactorySic.GetReInterrupcionAccesoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla ReInterrupcionAcceso
        /// </summary>
        public List<ReInterrupcionAccesoDTO> GetByCriteriaReInterrupcionAccesos()
        {
            return FactorySic.GetReInterrupcionAccesoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla RE_LOGCORREO

        /// <summary>
        /// Inserta un registro de la tabla RE_LOGCORREO
        /// </summary>
        public void SaveReLogcorreo(ReLogcorreoDTO entity)
        {
            try
            {
                FactorySic.GetReLogcorreoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RE_LOGCORREO
        /// </summary>
        public void UpdateReLogcorreo(ReLogcorreoDTO entity)
        {
            try
            {
                FactorySic.GetReLogcorreoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RE_LOGCORREO
        /// </summary>
        public void DeleteReLogcorreo(int relcorcodi)
        {
            try
            {
                FactorySic.GetReLogcorreoRepository().Delete(relcorcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RE_LOGCORREO
        /// </summary>
        public ReLogcorreoDTO GetByIdReLogcorreo(int relcorcodi)
        {
            return FactorySic.GetReLogcorreoRepository().GetById(relcorcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RE_LOGCORREO
        /// </summary>
        public List<ReLogcorreoDTO> ListReLogcorreos()
        {
            return FactorySic.GetReLogcorreoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla ReLogcorreo
        /// </summary>
        public List<ReLogcorreoDTO> GetByCriteriaReLogcorreos()
        {
            return FactorySic.GetReLogcorreoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla RE_TIPOCORREO

        /// <summary>
        /// Inserta un registro de la tabla RE_TIPOCORREO
        /// </summary>
        public void SaveReTipocorreo(ReTipocorreoDTO entity)
        {
            try
            {
                FactorySic.GetReTipocorreoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RE_TIPOCORREO
        /// </summary>
        public void UpdateReTipocorreo(ReTipocorreoDTO entity)
        {
            try
            {
                FactorySic.GetReTipocorreoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RE_TIPOCORREO
        /// </summary>
        public void DeleteReTipocorreo(int retcorcodi)
        {
            try
            {
                FactorySic.GetReTipocorreoRepository().Delete(retcorcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RE_TIPOCORREO
        /// </summary>
        public ReTipocorreoDTO GetByIdReTipocorreo(int retcorcodi)
        {
            return FactorySic.GetReTipocorreoRepository().GetById(retcorcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RE_TIPOCORREO
        /// </summary>
        public List<ReTipocorreoDTO> ListReTipocorreos()
        {
            return FactorySic.GetReTipocorreoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla ReTipocorreo
        /// </summary>
        public List<ReTipocorreoDTO> GetByCriteriaReTipocorreos()
        {
            return FactorySic.GetReTipocorreoRepository().GetByCriteria();
        }

        #endregion

        #region Habilitacion carga Interrupciones

        /// <summary>
        /// Devuelve el listado de empresas con la habilitacion de interrupciones
        /// </summary>
        /// <param name="repercodi"></param>
        /// <returns></returns>
        public List<ReEmpresaDTO> ObtenerListadoEmpresasHabilitacion(int repercodi)
        {
            List<ReEmpresaDTO> lstSalida = new List<ReEmpresaDTO>();
            List<ReEmpresaDTO> lstTemp1 = new List<ReEmpresaDTO>();
            List<ReEmpresaDTO> lstTemp2 = new List<ReEmpresaDTO>();
            List<ReEmpresaDTO>  listaSuministradores = this.servicioResarcimiento.ObtenerEmpresasSuministradoras();
            List<ReInterrupcionAccesoDTO> listaGuardados = FactorySic.GetReInterrupcionAccesoRepository().ListByPeriodo(repercodi);

            foreach (var item in listaGuardados)
            {
                ReEmpresaDTO ob1 = new ReEmpresaDTO();
                ob1.Emprcodi = item.Emprcodi.Value;
                ob1.Emprnomb = item.Emprnomb.Trim();
                ob1.HabilitacionPE = item.Reinacptoentrega == "S" ? true : false;
                ob1.HabilitacionRC = item.Reinacrechazocarga == "S" ? true : false;

                lstTemp1.Add(ob1);
            }

            foreach (var item in listaSuministradores)
            {
                ReEmpresaDTO em = lstTemp1.Find(x => x.Emprcodi == item.Emprcodi);

                //Si la empresa no se encuentra en la lista de guardados en BD, agrego al listado
                if(em == null)
                {
                    item.HabilitacionPE = false;
                    item.HabilitacionRC = false;
                    lstTemp2.Add(item);
                }
            }

            lstSalida.AddRange(lstTemp1);
            lstSalida.AddRange(lstTemp2);

            lstSalida = lstSalida.OrderBy(x => x.Emprnomb).ToList();

            return lstSalida;
        }

        /// <summary>
        /// Guarda la habilitacion de las interrupciones
        /// </summary>
        /// <param name="repercodi"></param>
        /// <param name="lstIS"></param>
        /// <param name="lstRC"></param>
        /// <param name="usuario"></param>
        public void guardarHabilitacion(int repercodi, string lstIS, string lstRC, string usuario)
        {
            List<ReEmpresaDTO> listaSuministradores = servicioResarcimiento.ObtenerEmpresasSuministradoras();
            List<string> lstMarcadosIS = lstIS.Split('/').Distinct().ToList();
            List<string> lstMarcadosRC = lstRC.Split('/').Distinct().ToList();

            List<string> lstTotal = new List<string>();
            lstTotal.AddRange(lstMarcadosIS);
            lstTotal.AddRange(lstMarcadosRC);

            lstTotal = lstTotal.Where(x => x.Trim() != "").Distinct().ToList();

            List<ReInterrupcionAccesoDTO> lstGuardar = new List<ReInterrupcionAccesoDTO>();

            foreach (string strEmprcodi in lstTotal)
            {
                var datoIS = lstMarcadosIS.Find(x => x == strEmprcodi);
                var datoRC = lstMarcadosRC.Find(x => x == strEmprcodi);

                ReInterrupcionAccesoDTO objAcceso = new ReInterrupcionAccesoDTO();                
                objAcceso.Emprcodi = Int32.Parse(strEmprcodi);
                objAcceso.Repercodi = repercodi;
                objAcceso.Reinacptoentrega = datoIS != null ? "S" : "N";
                objAcceso.Reinacrechazocarga = datoRC != null ? "S" : "N";
                objAcceso.Reinacusucreacion = usuario;
                objAcceso.Reinacfeccreacion = DateTime.Now;
                objAcceso.Reinacusumodificacion = usuario;
                objAcceso.Reinacfecmodificacion = DateTime.Now;

                lstGuardar.Add(objAcceso);
            }

            //Si hay alguno check marcado, guardo ese
            if (lstGuardar.Any())
            {
                //Eliminamos lo guardado en el periodo
                FactorySic.GetReInterrupcionAccesoRepository().DeletePorPeriodo(repercodi);
                

                foreach (var reg in lstGuardar)
                {
                    SaveReInterrupcionAcceso(reg);
                }

            } //si ningun check esta marcado
            else
            {
                //Eliminamos lo guardado en el periodo
                FactorySic.GetReInterrupcionAccesoRepository().DeletePorPeriodo(repercodi);

            }

        }

        #endregion
    }
}
