using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.DemandaMaxima.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Titularidad;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace COES.Servicios.Aplicacion.DemandaMaxima
{
    /// <summary>
    /// Clases con métodos del módulo General
    /// </summary>
    public class DemandaMaximaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DemandaMaximaAppServicio));
        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();

        #region Propiedades

        public int DiaCierrePeriodo = Int32.Parse(ConfigurationManager.AppSettings["DiaCierrePeriodoPR16"]);
        public int DiaAperturaPeriodo = Int32.Parse(ConfigurationManager.AppSettings["DiaAperturaPeriodoPR16"]);
        public int DiaRecordatorioPeriodo = Int32.Parse(ConfigurationManager.AppSettings["DiaRecordatorioPeriodoPR16"]);
        public int IdFormato = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoPR16"]);

        #endregion

        #region Metodos DemandaMaxima

        /// <summary>
        /// Metodo para realizar la notificación al finalizar el periodo de remisión
        /// </summary>
        public void Notificar(int idModulo)
        {
            try
            {
                if (DateTime.Now.Day == DiaCierrePeriodo)//DIA 10
                {
                    int idPlantilla = ConstantesDemandaMaxima.IdVencimientoPeriodo;
                    SiPlantillacorreoDTO siPlantillaCorreo = GetByIdSiPlantillaCorreo(idPlantilla);
                    DateTime periodo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);

                    string mailTo = "";
                    string empresa = "";
                    int emprcodi = 0;

                    List<SiEmpresaDTO> ListaEmailsUsuarios = this.ListaEmailUsuariosEmpresas(periodo.ToString("MM yyyy"), IdFormato, idModulo);

                    foreach (var SiEmpresaDTO in ListaEmailsUsuarios)
                    {
                        emprcodi = SiEmpresaDTO.Emprcodi;
                        empresa = SiEmpresaDTO.Emprnomb;
                        mailTo = SiEmpresaDTO.UserEmail;
                        string fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString(ConstantesAppServicio.FormatoFecha);
                        string asunto = string.Format(siPlantillaCorreo.Plantasunto, periodo);
                        string contenido = string.Format(siPlantillaCorreo.Plantcontenido, empresa, periodo, fechaFin);

                        COES.Base.Tools.Util.SendEmail(mailTo, asunto, contenido);//Enviar correo sin CCO

                        SiCorreoDTO correo = new SiCorreoDTO();
                        correo.Corrasunto = asunto;
                        correo.Corrcontenido = contenido;
                        correo.Corrfechaenvio = DateTime.Now;
                        correo.Corrto = mailTo;
                        correo.Emprcodi = emprcodi;
                        correo.Corrfechaperiodo = periodo;
                        correo.Plantcodi = siPlantillaCorreo.Plantcodi;
                        this.SaveSiCorreo(correo);
                    }
                }
                else if (DateTime.Now.Day == DiaAperturaPeriodo)//DIA 1
                {
                    int idPlantilla = ConstantesDemandaMaxima.IdAperturaPeriodo;
                    SiPlantillacorreoDTO siPlantillaCorreo = GetByIdSiPlantillaCorreo(idPlantilla);
                    DateTime periodo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);

                    string mailTo = "";
                    string empresa = "";
                    int emprcodi = 0;

                    List<SiEmpresaDTO> ListaEmailsUsuarios = this.ListaEmailUsuariosEmpresas(periodo.ToString("MM yyyy"), IdFormato, idModulo);

                    foreach (var SiEmpresaDTO in ListaEmailsUsuarios)
                    {
                        emprcodi = SiEmpresaDTO.Emprcodi;
                        empresa = SiEmpresaDTO.Emprnomb;
                        mailTo = SiEmpresaDTO.UserEmail;
                        string fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString(ConstantesAppServicio.FormatoFecha);
                        string asunto = string.Format(siPlantillaCorreo.Plantasunto, periodo);
                        string contenido = string.Format(siPlantillaCorreo.Plantcontenido, empresa, periodo.ToString("MM yyyy"));

                        COES.Base.Tools.Util.SendEmail(mailTo, asunto, contenido);//Enviar correo sin CCO

                        SiCorreoDTO correo = new SiCorreoDTO();
                        correo.Corrasunto = asunto;
                        correo.Corrcontenido = contenido;
                        correo.Corrfechaenvio = DateTime.Now;
                        correo.Corrto = mailTo;
                        correo.Emprcodi = emprcodi;
                        correo.Corrfechaperiodo = periodo;
                        correo.Plantcodi = siPlantillaCorreo.Plantcodi;
                        this.SaveSiCorreo(correo);
                    }
                }
                else if (DateTime.Now.Day == DiaRecordatorioPeriodo)//DIA 5
                {
                    int idPlantilla = ConstantesDemandaMaxima.IdRecordatorioPeriodo;
                    SiPlantillacorreoDTO siPlantillaCorreo = GetByIdSiPlantillaCorreo(idPlantilla);
                    DateTime periodo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);

                    string mailTo = "";
                    string empresa = "";
                    int emprcodi = 0;

                    List<SiEmpresaDTO> ListaEmailsUsuarios = this.ListaEmailUsuariosEmpresas(periodo.ToString("MM yyyy"), IdFormato, idModulo);

                    foreach (var SiEmpresaDTO in ListaEmailsUsuarios)
                    {
                        emprcodi = SiEmpresaDTO.Emprcodi;
                        empresa = SiEmpresaDTO.Emprnomb;
                        mailTo = SiEmpresaDTO.UserEmail;
                        string fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString(ConstantesAppServicio.FormatoFecha);
                        string asunto = string.Format(siPlantillaCorreo.Plantasunto, periodo);
                        string contenido = string.Format(siPlantillaCorreo.Plantcontenido, empresa, periodo.ToString("MM yyyy"));

                        COES.Base.Tools.Util.SendEmail(mailTo, asunto, contenido);//Enviar correo sin CCO

                        SiCorreoDTO correo = new SiCorreoDTO();
                        correo.Corrasunto = asunto;
                        correo.Corrcontenido = contenido;
                        correo.Corrfechaenvio = DateTime.Now;
                        correo.Corrto = mailTo;
                        correo.Emprcodi = emprcodi;
                        correo.Corrfechaperiodo = periodo;
                        correo.Plantcodi = siPlantillaCorreo.Plantcodi;
                        this.SaveSiCorreo(correo);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo para obtener la lista de tipo de empresa.
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ListTipoEmpresaCumplimiento()
        {
            List<SiTipoempresaDTO> list = FactorySic.GetSiTipoempresaRepository().List().
                Where(x => x.Tipoemprcodi == 2 || x.Tipoemprcodi == 4).ToList();
            return list;
        }

        /// <summary>
        /// Metodo para obtener la lista de periodo en el reporte
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> ListaPeriodoReporte(string fecha)
        {
            return FactorySic.GetMeEnvioRepository().ObtenerListaPeriodoReporte(fecha);
        }

        /// <summary>
        /// Obtiene la lista de empresas por tipo de cumplimiento para los reportes
        /// </summary>
        /// <param name="tipoemprcodi"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListaEmpresasPorTipoCumplimiento(int tipoemprcodi, int formato)
        {
            return FactorySic.GetSiEmpresaRepository().ListaEmpresasPorTipoCumplimiento(tipoemprcodi, formato);
        }

        /// <summary>
        /// Obtiene la lista de Emails de los usuarios por empresa a las que se notificará
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="formato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListaEmailUsuariosEmpresas(string periodo, int formato, int modulo)
        {
            return FactorySic.GetSiEmpresaRepository().ListaEmailUsuariosEmpresas(periodo, formato, modulo);
        }

        /// <summary>
        /// Metodo para obtener la lista del reporte de cumplimiento
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="qEmpresa"></param>
        /// <param name="qTipoEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="cumplimiento"></param>
        /// <param name="ulcoes"></param>
        /// <param name="origen"></param>
        /// <returns></returns>
        public System.Data.IDataReader ListaReporteCumplimiento(int formato, string qEmpresa, string qTipoEmpresa, string fechaIni, string fechaFin, string cumplimiento, string ulcoes, string abreviatura, string origen)
        {
            return FactorySic.GetMeEnvioRepository().GetListReporteCumplimiento(formato, qEmpresa, qTipoEmpresa, fechaIni, fechaFin, cumplimiento, ulcoes, abreviatura, origen);
        }

        /// <summary>
        /// Metodo para obtener la lista del reporte de informacion cargada cada 15 min
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="fechaIni"></param>
        /// <param name="periodoSicli"></param>
        /// <param name="qEmpresa"></param>
        /// <param name="qTipoEmpresa"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ListaReporteInformacion15min(int formato, string fechaIni, string periodoSicli, string qEmpresa, string qTipoEmpresa,
            string qMaxDemanda, string lectCodiPR16, string lectCodiAlpha, int regIni, int regFin)
        {
            return FactorySic.GetMeMedicion96Repository().GetListReporteInformacion15min(formato, fechaIni, periodoSicli, qEmpresa, qTipoEmpresa, qMaxDemanda,
                lectCodiPR16, lectCodiAlpha, regIni, regFin);
        }

        /// <summary>
        /// Metodo para obtener la lista del reporte de informacion cargada cada 30 min
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="fechaIni"></param>
        /// <param name="periodoSicli"></param>
        /// <param name="qEmpresa"></param>
        /// <param name="qTipoEmpresa"></param>
        /// <param name="qMaxDemanda"></param>
        /// <param name="lectCodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaReporteInformacion30min(int formato, string inicio, string lectCodiPR16, string qEmpresa, string qTipoEmpresa, string fechaIni, string periodoSicli, string lectCodiAlpha, int regIni, int regFin)
        {
            //Logger.Info("formato: " + formato + " | inicio: " + inicio + " | lectCodiPR16: " + lectCodiPR16 + " | qEmpresa: " + qEmpresa + " | qTipoEmpresa: " + qTipoEmpresa + " | fechaIni: " + fechaIni + " | fechaFin: " + fechaFin + " | lectCodiAlpha: " + lectCodiAlpha);
            return FactorySic.GetMeMedicion48Repository().GetListReporteInformacion30min(formato, inicio, lectCodiPR16, qEmpresa, qTipoEmpresa, fechaIni, periodoSicli, lectCodiAlpha, regIni, regFin);
        }

        /// <summary>
        /// Permite realizar la consulta del envio actual y anterior
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> ObtenerListaEnvioActual(int idEmpresa, string periodo)
        {
            return FactorySic.GetMeEnvioRepository().ObtenerListaEnvioActual(idEmpresa, periodo);
        }

        /// <summary>
        /// Permite realizar la consulta de las validaciones de coherencia
        /// </summary>
        /// <param name="enviocodiact"></param>
        /// <param name="enviocodiant"></param>
        /// <param name="fecIniAct"></param>
        /// <param name="fecFinAct"></param>
        /// <param name="fecIniAnt"></param>
        /// <param name="fecFinAnt"></param>
        /// <param name="variacion"></param>
        /// <param name="consumo"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObteneListaObservacionCoherencia(int enviocodiact, int enviocodiant, string fecIniAct, string fecFinAct, string fecIniAnt,
            string fecFinAnt, int variacion, string consumo)
        {
            if (consumo == "M")
            {
                return FactorySic.GetMeMedicion96Repository().ObtenerListaObservacionCoherenciaMensual(enviocodiact, enviocodiant, fecIniAct, fecFinAct, fecIniAnt,
                    fecFinAnt, variacion);
            }
            else
            {
                return FactorySic.GetMeMedicion96Repository().ObtenerListaObservacionCoherenciaDiaria(enviocodiact, enviocodiant, fecIniAct, fecFinAct, fecIniAnt,
                    fecFinAnt, variacion);
            }
        }

        /// <summary>
        /// Permite realizar la consulta del rango del periodo de envio
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>

        public List<MeFormatoEmpresaDTO> ObtenerListaPeriodoEnvio(string fecha, int formato, int idEmpresa)
        {
            return FactorySic.GetMeFormatoEmpresaRepository().ObtenerListaPeriodoEnvio(fecha, formato, idEmpresa);
        }

        /// <summary>
        /// Busca PlantillaCorreo por id
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <returns></returns>
        public SiPlantillacorreoDTO GetByIdSiPlantillaCorreo(int idPlantilla)
        {
            return FactorySic.GetSiPlantillacorreoRepository().GetById(idPlantilla);
        }

        /// <summary>
        /// Permite grabar los datos cargados del PR16
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarValoresCargados96PR16(List<MeMedicion96DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                int count = 0;
                //Traer Ultimos Valores

                var lista = Convert96DTO(this.logic.GetDataFormato(idEmpresa, formato, 0, 0));
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();


                    foreach (var reg in entitys)
                    {
                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi);

                        if (regAnt == null)
                        {
                            MeMedicion96DTO regAntCopy = new MeMedicion96DTO();
                            System.Reflection.FieldInfo[] myObjectFields = reg.GetType().GetFields(System.Reflection.BindingFlags.NonPublic
                                                                                                 | System.Reflection.BindingFlags.Public
                                                                                                 | System.Reflection.BindingFlags.Instance);

                            foreach (System.Reflection.FieldInfo fi in myObjectFields)
                            {
                                fi.SetValue(regAntCopy, fi.GetValue(reg));
                            }
                            regAnt = regAntCopy;
                            for (int i = 1; i <= 96; i++)
                            {
                                regAnt.GetType().GetProperty("H" + i.ToString()).SetValue(regAnt, null);
                            }
                        }

                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        for (int i = 1; i <= 96; i++)
                        {
                            decimal? valorOrigen = (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                            decimal? valorModificado = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                            if (valorModificado != null)
                                filaValores.Add(valorModificado.ToString());
                            else
                                filaValores.Add("");
                            if (valorOrigen != null)
                                filaValoresOrigen.Add(valorOrigen.ToString());
                            else
                                filaValoresOrigen.Add("");
                            if (valorOrigen != valorModificado)//&& valorOrigen != null && valorModificado != null)
                            {
                                if (count <= 100)
                                {
                                    filaCambios.Add(i.ToString());
                                    count++;
                                }
                            }
                        }

                        if (filaCambios.Count > 0)
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            cambio.Cambenvdatos = String.Join(",", filaValores);
                            cambio.Cambenvcolvar = String.Join(",", filaCambios);
                            cambio.Cambenvfecha = (DateTime)reg.Medifecha;
                            cambio.Enviocodi = idEnvio;
                            //cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (this.logic.ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                            {
                                int idEnvioPrevio = 0;
                                var listAux = this.logic.GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaProceso);
                                if (listAux.Count > 0)
                                    idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                origen.Cambenvcolvar = "";
                                origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                origen.Enviocodi = idEnvioPrevio;
                                //origen.Formatcodi = formato.Formatcodi;
                                origen.Ptomedicodi = reg.Ptomedicodi;
                                origen.Tipoinfocodi = reg.Tipoinfocodi;
                                origen.Lastuser = usuario;
                                origen.Lastdate = DateTime.Now;
                                listaOrigen.Add(origen);
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        this.logic.GrabarCambios(listaCambio);
                        this.logic.GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }

                #region EMPRESAS - TIEE
                TitularidadAppServicio servTitEmp = new TitularidadAppServicio();
                List<SiMigracionDTO> listTiee = servTitEmp.ListarTransferenciasXEmpresaOrigenXEmpresaDestino(-2, idEmpresa, "", 0);
                bool migracion = false;
                DateTime fechaTiee = formato.FechaInicio;
                if (listTiee.Count > 0 && listTiee.First().Migrafeccorte != null && listTiee.First().Migrafeccorte > formato.FechaInicio && listTiee.First().Migrafeccorte <= formato.FechaFin)
                {
                    migracion = true;
                    fechaTiee = listTiee.First().Migrafeccorte;
                }

                //Eliminar Valores Previos
                if (migracion) this.logic.EliminarValoresCargados96((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin, fechaTiee);
                else this.logic.EliminarValoresCargados96((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                #endregion

                foreach (MeMedicion96DTO entity in entitys)
                {
                    if (entity.Meditotal == null)
                    {
                        entity.Meditotal = 0;
                    }
                    //entity.Tipoptomedicodi = -1;
                    FactorySic.GetMeMedicion96Repository().Save(entity);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo para exponer la funcionalidad de Grabar información PR16
        /// </summary>
        /// <param name="entitys">Listado de valores de demanda diaria cada 15 min</param>
        /// <param name="periodo">Periodo de envio</param>
        /// <param name="usuario">usuario de envío</param>
        /// <param name="idEmpresa">Código de empresa</param>
        /// <returns>Código del envío</returns>
        public int GrabarValoresPR16(List<MeMedicion96DTO> entitys, string periodo, string usuario, int idEmpresa)
        {
            MeFormatoDTO formato = this.logic.GetByIdMeFormato(this.IdFormato);
            int iEnvioId = -1;
            string fecha = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, periodo, string.Empty, string.Empty, "dd/MM/yyyy").ToString("dd/MM/yyyy");
            List<MeFormatoEmpresaDTO> listPeriodoEnvio = this.ObtenerListaPeriodoEnvio(fecha, formato.Formatcodi, idEmpresa);
            MeFormatoEmpresaDTO per = new MeFormatoEmpresaDTO();
            per = listPeriodoEnvio[0];
            formato.FechaInicio = per.PeriodoFechaIni.Value;
            formato.FechaFin = per.PeriodoFechaFin.Value;
            Boolean enPlazoReal = false;
            ///////////////Grabar Envio//////////////////////////
            int horaini = 0;//Para Formato Tiempo Real
            int horafin = 0;//Para Formato Tiempo Real
            Boolean enPlazo = this.ValidarFechaPr16(idEmpresa, periodo, out enPlazoReal);

            var puntosResult = this.GetPuntosMedicionPR16(idEmpresa, periodo);
            foreach (var dia in entitys)
            {
                var puntoMedicion = puntosResult.FirstOrDefault(t => t.Ptomedicodi == dia.Ptomedicodi);
                dia.Lectcodi = formato.Lectcodi;
                dia.Tipoinfocodi = puntoMedicion.Tipoinfocodi;
            }

            MeEnvioDTO envio = new MeEnvioDTO();
            envio.Archcodi = 0;
            envio.Emprcodi = idEmpresa;
            envio.Enviofecha = DateTime.Now;
            envio.Enviofechaperiodo = formato.FechaProceso;
            envio.Envioplazo = (enPlazoReal) ? "P" : "F";
            envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
            envio.Lastdate = DateTime.Now;
            envio.Lastuser = usuario;
            envio.Userlogin = usuario;
            envio.Formatcodi = this.IdFormato;

            envio.Enviofechaini = formato.FechaInicio;
            envio.Enviofechafin = formato.FechaFin.AddDays(-1);
            envio.Modcodi = 14;//Modulo PR16
            iEnvioId = logic.SaveMeEnvio(envio);

            this.GrabarValoresCargados96PR16(entitys, usuario, iEnvioId, idEmpresa, formato);

            return iEnvioId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="envioCodi"></param>
        /// <param name="emprCodi"></param>
        /// <param name="periodo"></param>
        /// <param name="emailsAdmin"></param>
        /// <param name="userMail"></param>
        public void EnviarNotificacionPR16(int envioCodi, int emprCodi, string periodo, List<string> emailsAdmin, string userMail)
        {
            var envio = logic.GetByIdMeEnvio(envioCodi);
            MeFormatoDTO formato = this.logic.GetByIdMeFormato(this.IdFormato);
            formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, periodo, string.Empty, string.Empty, "dd/MM/yyyy");

            string strPlazo = "Fuera del Plazo";
            if (envio.Envioplazo == "P")
            {
                strPlazo = "Dentro del Plazo";
            }
            string empresa = string.Empty;
            var regEmp = this.logic.GetByIdSiEmpresa(emprCodi); ;
            //////////////////////////////////////////////////
            if (regEmp != null)
                empresa = regEmp.Emprnomb;

            //int idPlantilla = ConstantesDemandaMaxima.IdInformacionEnviada;
            //SiPlantillacorreoDTO siPlantillaCorreo = this.servicio.GetByIdSiPlantillaCorreo(idPlantilla);

            SiPlantillacorreoDTO siPlantillaCorreo = (new COES.Servicios.Aplicacion.Correo.CorreoAppServicio()).ObtenerPlantillaPorModulo(
                COES.Servicios.Aplicacion.Helper.TipoPlantillaCorreo.NotificacionEnvioExtranet, 14); //Modulo PR16

            string contenidoEmpresa = empresa;
            int contenidoIdEnvio = envioCodi;
            string contenidoPeriodo = periodo;

            string mail_ope = "";
            foreach (var item in emailsAdmin)
            {
                mail_ope = item;
            }

            string mailOperador = mail_ope;
            string asunto = string.Format(siPlantillaCorreo.Plantasunto, contenidoPeriodo);
            string contenido = string.Format(siPlantillaCorreo.Plantcontenido, contenidoEmpresa, contenidoPeriodo, contenidoIdEnvio, strPlazo);

            string mailTo = string.Empty;
            mailTo = userMail;

            COES.Base.Tools.Util.SendEmail(mailTo, mailOperador, asunto, contenido);

            SiCorreoDTO correo = new SiCorreoDTO();
            correo.Enviocodi = contenidoIdEnvio;
            correo.Corrasunto = asunto;
            correo.Corrcontenido = contenido;
            correo.Corrfechaenvio = envio.Enviofecha;
            correo.Corrfrom = ConfigurationManager.AppSettings["MailFrom"];
            correo.Corrto = mailTo;
            correo.Corrbcc = mailOperador;
            correo.Emprcodi = emprCodi;
            correo.Corrfechaperiodo = formato.FechaProceso;
            correo.Plantcodi = siPlantillaCorreo.Plantcodi;
            this.SaveSiCorreo(correo);
        }

        /// <summary>
        /// Método para retornar los valores registrados segun código de envío
        /// </summary>
        /// <param name="envioCodi">Código de envío</param>
        /// <returns>Listado de datos cada 15 minutos</returns>
        public List<MeMedicion96DTO> ObtenerDatosEnvio(int envioCodi)
        {
            FormatoMedicionAppServicio formatoMedServicio = new FormatoMedicionAppServicio();
            var envio = formatoMedServicio.GetByIdMeEnvio(envioCodi);
            var formato = formatoMedServicio.GetByIdMeFormato(this.IdFormato);
            int idEmpresa = envio.Emprcodi.Value;

            List<MeMedicion96DTO> lista96 = FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, envio.Enviofechaini.Value, envio.Enviofechafin.Value);
            if (envioCodi != 0)
            {
                var lista = formatoMedServicio.GetAllCambioEnvio(formato.Formatcodi, envio.Enviofechaini.Value, envio.Enviofechafin.Value, envioCodi, idEmpresa);
                if (lista.Count > 0)
                {
                    foreach (var reg in lista)
                    {
                        var find = lista96.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                            x.Medifecha == reg.Cambenvfecha);
                        if (find != null)
                        {
                            var fila = reg.Cambenvdatos.Split(',');
                            for (var i = 0; i < 96; i++)
                            {
                                decimal dato;
                                decimal? numero = null;
                                if (decimal.TryParse(fila[i], out dato))
                                    numero = dato;
                                find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                            }

                        }
                    }
                }
            }

            return lista96;
        }
        /// <summary>
        /// Permite grabar los datos cargados de DML
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarValoresCargados96DML(List<MeMedicion96DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato, int tptoMediCodi)
        {
            try
            {
                int count = 0;
                //Traer Ultimos Valores

                var lista = Convert96DTO(this.logic.GetDataFormato(idEmpresa, formato, 0, 0));
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();


                    foreach (var reg in entitys)
                    {
                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi);

                        if (regAnt == null)
                        {
                            MeMedicion96DTO regAntCopy = new MeMedicion96DTO();
                            System.Reflection.FieldInfo[] myObjectFields = reg.GetType().GetFields(System.Reflection.BindingFlags.NonPublic
                                                                                                 | System.Reflection.BindingFlags.Public
                                                                                                 | System.Reflection.BindingFlags.Instance);

                            foreach (System.Reflection.FieldInfo fi in myObjectFields)
                            {
                                fi.SetValue(regAntCopy, fi.GetValue(reg));
                            }
                            regAnt = regAntCopy;
                            for (int i = 1; i <= 96; i++)
                            {
                                regAnt.GetType().GetProperty("H" + i.ToString()).SetValue(regAnt, null);
                            }
                        }

                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        for (int i = 1; i <= 96; i++)
                        {
                            decimal? valorOrigen = (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                            decimal? valorModificado = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                            if (valorModificado != null)
                                filaValores.Add(valorModificado.ToString());
                            else
                                filaValores.Add("");
                            if (valorOrigen != null)
                                filaValoresOrigen.Add(valorOrigen.ToString());
                            else
                                filaValoresOrigen.Add("");
                            if (valorOrigen != valorModificado)//&& valorOrigen != null && valorModificado != null)
                            {
                                if (count <= 100)
                                {
                                    filaCambios.Add(i.ToString());
                                    count++;
                                }
                            }
                        }

                        if (filaCambios.Count > 0)
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            cambio.Cambenvdatos = String.Join(",", filaValores);
                            cambio.Cambenvcolvar = String.Join(",", filaCambios);
                            cambio.Cambenvfecha = (DateTime)reg.Medifecha;
                            cambio.Enviocodi = idEnvio;
                            //cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (this.logic.ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                            {
                                int idEnvioPrevio = 0;
                                var listAux = this.logic.GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaProceso);
                                if (listAux.Count > 0)
                                    idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                origen.Cambenvcolvar = "";
                                origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                origen.Enviocodi = idEnvioPrevio;
                                //origen.Formatcodi = formato.Formatcodi;
                                origen.Ptomedicodi = reg.Ptomedicodi;
                                origen.Tipoinfocodi = reg.Tipoinfocodi;
                                origen.Lastuser = usuario;
                                origen.Lastdate = DateTime.Now;
                                listaOrigen.Add(origen);
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        this.logic.GrabarCambios(listaCambio);
                        this.logic.GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }

                TitularidadAppServicio servTitEmp = new TitularidadAppServicio();
                List<SiMigracionDTO> listTiee = servTitEmp.ListarTransferenciasXEmpresaOrigenXEmpresaDestino(-2, idEmpresa, "", 0);
                bool migracion = false;
                DateTime fechaTiee = formato.FechaInicio;
                if (listTiee.Count > 0 && listTiee.First().Migrafeccorte != null && listTiee.First().Migrafeccorte > formato.FechaInicio && listTiee.First().Migrafeccorte <= formato.FechaFin)
                {
                    migracion = true;
                    fechaTiee = listTiee.First().Migrafeccorte;
                }
                //Eliminar Valores Previos
                if (migracion) this.logic.EliminarValoresCargados96((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin, fechaTiee);
                else this.logic.EliminarValoresCargados96((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                foreach (MeMedicion96DTO entity in entitys)
                {
                    if (entity.Meditotal == null)
                    {
                        entity.Meditotal = 0;
                    }

                    entity.TptoMediCodi = tptoMediCodi;
                    FactorySic.GetMeMedicion96Repository().SaveDemandaMercadoLibre(entity);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Convierte Lista Object a medicion96
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<MeMedicion96DTO> Convert96DTO(List<Object> lista)
        {
            List<MeMedicion96DTO> listaFinal = new List<MeMedicion96DTO>();

            foreach (var entity in lista)
            {
                MeMedicion96DTO reg = new MeMedicion96DTO();
                reg.Lectcodi = (int)entity.GetType().GetProperty("Lectcodi").GetValue(entity, null);
                reg.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                reg.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                reg.Tipoinfocodi = (int)entity.GetType().GetProperty("Tipoinfocodi").GetValue(entity, null);
                for (int i = 1; i <= 96; i++)
                {
                    decimal? valor = (decimal?)entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).GetValue(entity, null);
                    reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                }
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        /// <summary>
        /// Metodo para guardar la información enviada del correo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int SaveSiCorreo(SiCorreoDTO entity)
        {
            try
            {
                return FactorySic.GetSiCorreoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo para obtener la máxima demanda
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <returns></returns>
        public DemandadiaDTO ObtenerDatosMaximaDemanda(DateTime fechaini, DateTime fechafin)
        {
            var servMedidores = new ReporteMedidoresAppServicio();

            //obtener día de la máxima demanda
            DateTime fechaMD = servMedidores.GetDiaPeriodoDemanda96(fechaini, fechafin);

            //obtener valores de MD, HP y HFP en el día de la máxima demanda
            var listaBloque = servMedidores.ListarResumenDiaMaximaDemanda96HPyHFP(fechaMD.Date, ConstantesMedicion.IdTipogrupoCOES, ConstantesMedicion.IdTipoGeneracionTodos, ConstantesMedicion.IdEmpresaTodos, ConstanteValidacion.EstadoTodos);
            var bloqueMD = listaBloque.Find(x=>x.CodigoHorario == ConstantesRepMaxDemanda.CodigoHorarioMD);
            var bloqueHP = listaBloque.Find(x => x.CodigoHorario == ConstantesRepMaxDemanda.CodigoHorarioHP);
            var bloqueHFP = listaBloque.Find(x => x.CodigoHorario == ConstantesRepMaxDemanda.CodigoHorarioHFP);

            DemandadiaDTO resultado = new DemandadiaDTO();
            resultado.ValorMD = bloqueMD.Valor;
            resultado.HoraMD = fechaMD.Date.AddMinutes(bloqueMD.HDemanda * 15).ToString(ConstantesAppServicio.FormatoHora);
            resultado.FechaMD = fechaMD.ToString(ConstantesAppServicio.FormatoFecha);
            resultado.IndexHoraMD = bloqueMD.HDemanda;
            resultado.IndiceMDHP = bloqueHP.HDemanda;
            resultado.IndiceMDHFP = bloqueHFP.HDemanda;

            return resultado;
        }


        /// <summary>
        /// GenerarFormatoValidacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <param name="variacion"></param>
        /// <param name="consumo"></param>
        /// <param name="path"></param>
        /// <param name="pathLogo"></param>
        /// <returns></returns>
        public string GenerarFormatoValidacion(int idEmpresa, string mes, int variacion, string consumo, string path, string pathLogo)
        {
            string fileName = string.Empty;

            int imes = Int32.Parse(mes.Substring(0, 2));
            int ianho = Int32.Parse(mes.Substring(3, 4));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string text_fecha = fechaProceso.ToString(ConstantesDemandaMaxima.FormatoFecha);

            List<MeEnvioDTO> listEnvio = this.ObtenerListaEnvioActual(idEmpresa, text_fecha);
            MeEnvioDTO envio = new MeEnvioDTO();
            envio = listEnvio[0];

            List<MeMedicion96DTO> listObservaciones = this.ObteneListaObservacionCoherencia(envio.EnvioCodiAct.Value, envio.EnvioCodiAnt.Value,
                envio.EnvioFechaIniAct.Value.ToString(ConstantesDemandaMaxima.FormatoFecha), envio.EnvioFechaFinAct.Value.ToString(ConstantesDemandaMaxima.FormatoFecha),
                envio.EnvioFechaIniAnt.Value.ToString(ConstantesDemandaMaxima.FormatoFecha), envio.EnvioFechaFinAnt.Value.ToString(ConstantesDemandaMaxima.FormatoFecha),
                variacion, consumo);
            List<String> list = new List<String>();

            if (listObservaciones.Count > 0)
            {
                foreach (var obs in listObservaciones)
                {
                    if (consumo == "M")
                    {
                        list.Add("El consumo de Pto. de Suministro " + obs.PtoMediBarraNomb + " tiene una variación del " + obs.VarMensual + "% con respecto al mes anterior.");
                    }
                    else
                    {
                        list.Add("El consumo de Pto. de Suministro " + obs.PtoMediBarraNomb + " tiene una variación del " + obs.VarPromDiaria + "% con respecto al mes anterior.");
                    }
                }
            }
            fileName = "Observaciones Encontradas.xlsx";
            ExcelDocument.GenerarFormatoExcelValidacion(path + fileName, list);

            return fileName;
        }

        /// <summary>
        /// GenerarFormatoRepCumplimiento
        /// </summary>
        /// <param name="nroPagina"></param>
        /// <param name="empresas"></param>
        /// <param name="tipos"></param>
        /// <param name="ini"></param>
        /// <param name="fin"></param>
        /// <param name="cumpli"></param>
        /// <param name="path"></param>
        /// <param name="pathLogo"></param>
        /// <returns></returns>
        public string GenerarFormatoRepCumplimiento(int nroPagina, string empresas, string tipos, string ini, string fin, string cumpli, string ulcoes, string abreviatura, 
            string path, string pathLogo)
        {
            string fileName = string.Empty;

            var ListaReporteCumplimiento = this.ListaReporteCumplimiento(IdFormato, empresas, tipos, ini, fin, cumpli, ulcoes, abreviatura,
                ConfigurationManager.AppSettings["FechaInicioCumplimientoPR16"]);

            fileName = "Reporte Cumplimiento.xlsx";
            var fecInicio = DateTime.ParseExact(ini, "dd/MM/yyyy", null);
            var fecFin = DateTime.ParseExact(fin, "dd/MM/yyyy", null);

            var meses = Math.Abs((fecInicio.Month - fecFin.Month) + 12 * (fecInicio.Year - fecFin.Year));
            ExcelDocument.GenerarFormatoExcelRepCumplimiento(path + fileName, ListaReporteCumplimiento, fecInicio, meses, abreviatura);

            return fileName;
        }

        /// <summary>
        /// GenerarFormatoRepInformacion
        /// </summary>
        /// <param name="nivel"></param>
        /// <param name="empresas"></param>
        /// <param name="tipos"></param>
        /// <param name="ini"></param>
        /// <param name="periodoSicli"></param>
        /// <param name="max"></param>
        /// <param name="path"></param>
        /// <param name="pathLogo"></param>
        /// <returns></returns>
        public string GenerarFormatoRepInformacion(string nivel, string empresas, string tipos, string ini, string periodoSicli, int max,
            string path, string pathLogo)
        {
            string fileName = string.Empty;
            string diasMaxDemanda = "";

            fileName = "Reporte Informacion.xlsx";
            string lectCodiPR16 = ConfigurationManager.AppSettings["IdLecturaPR16"];
            string lectCodiAlpha = ConfigurationManager.AppSettings["IdLecturaAlphaPR16"];

            List<DemandadiaDTO> listDemanda = new List<DemandadiaDTO>();
            if (max == 1)
            {
                //Simulando la obtencion de la máxima demanda
                string[] formats = { ConstantesDemandaMaxima.FormatoFecha };
                DateTime dti = DateTime.ParseExact(ini, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                
                //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
                //DateTime dtf = DateTime.ParseExact(periodoSicli, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                DateTime dtf = dti;
                //- HDT Fin

                DateTime fec_ini = new DateTime();
                DateTime fec_fin = new DateTime();

                for (int i = dti.Month; i <= dtf.Month; i++)
                {
                    fec_ini = new DateTime(dti.Year, i, 1);
                    fec_fin = new DateTime(dtf.Year, i, 1).AddMonths(1).AddDays(-1);
                    DemandadiaDTO entity = this.ObtenerDatosMaximaDemanda(fec_ini, fec_fin);
                    if (diasMaxDemanda == "")
                    { diasMaxDemanda = diasMaxDemanda + "'" + entity.FechaMD + "'"; }
                    else
                    { diasMaxDemanda = diasMaxDemanda + ", '" + entity.FechaMD + "'"; }
                    listDemanda.Add(entity);
                }
            }

            if (nivel == "15")//cada 15min
            {
                List<MeMedicion96DTO> ListaReporteInformacion15min = this.ListaReporteInformacion15min(IdFormato, ini, periodoSicli, empresas, tipos, diasMaxDemanda, lectCodiPR16, lectCodiAlpha, 0, 0);
                if (max == 1)
                {
                    List<MeMedicion96DTO> listInfo = new List<MeMedicion96DTO>();
                    foreach (var entity in ListaReporteInformacion15min)
                    {
                        if (max == 1)
                        {
                            foreach (var item in listDemanda)
                            {
                                if (entity.FechaFila.ToString(ConstantesDemandaMaxima.FormatoFecha) == item.FechaMD)
                                {
                                    entity.HP = "H" + item.IndiceMDHP;
                                    entity.HFP = "H" + item.IndiceMDHFP;
                                }
                            }
                        }
                        listInfo.Add(entity);
                    }
                    ExcelDocument.GenerarFormatoExcel15min(path + fileName, listInfo, tipos);
                }
                else
                {
                    ExcelDocument.GenerarFormatoExcel15min(path + fileName, ListaReporteInformacion15min, tipos);
                }
            }
            else//Cada 30min
            {
                List<MeMedicion48DTO> ListaReporteInformacion30min = this.ListaReporteInformacion30min(IdFormato, ConfigurationManager.AppSettings["FechaInicioCumplimientoPR16"], lectCodiPR16, empresas, tipos, ini, periodoSicli, lectCodiAlpha, 0, 0);
                ExcelDocument.GenerarFormatoExcel30min(path + fileName, ListaReporteInformacion30min, tipos);
            }
            return fileName;
        }

        /// <summary>
        /// Metodo para obtener la lista del reporte de informacion cargada cada 15 min
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="fechaIni"></param>
        /// <param name="periodoSicli"></param>
        /// <param name="qEmpresa"></param>
        /// <param name="qTipoEmpresa"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int ListaReporteInformacion15minCount(int formato, string fechaIni, string periodoSicli, string qEmpresa, string qTipoEmpresa,
            string qMaxDemanda, string lectCodiPR16, string lectCodiAlpha)
        {
            return FactorySic.GetMeMedicion96Repository().GetListReporteInformacion15minCount(formato, fechaIni, periodoSicli, qEmpresa, qTipoEmpresa, qMaxDemanda,
                lectCodiPR16, lectCodiAlpha);
        }

        #endregion


        //- pr16conpensaciones.JDEL - Inicio 21/10/2016: Cambio para atender el requerimiento.

        /// <summary>
        /// Obtener las empresas para suministradores
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListaEmpresasSuministrador()
        {
            return FactorySic.GetSiEmpresaRepository().ListaEmpresasSuministrador();
        }


        public List<MePtosuministradorDTO> ListaEditorPtoSuministro(string periodo, int empresa, int formato)
        {
            return FactorySic.GetMePtosuministradorRepository().ListaEditorPtoSuministro(periodo, empresa, formato);
        }

        /// <summary>
        /// Permite listar los puntos de medicion la tabla MeHojaptomed
        /// </summary>
        public List<MeHojaptomedDTO> GetPtoMedicionPR16(int emprcodi, int formatcodi, string periodo, string query)
        {
            return FactorySic.GetMeHojaptomedRepository().GetPtoMedicionPR16(emprcodi, formatcodi, periodo, query);
        }

        /// <summary>
        /// Método que retorna el listado de puntos de medición para una empresa en el periodo indicado
        /// </summary>
        /// <param name="emprcodi">Código de empresa</param>
        /// <param name="periodo">Periodo</param>
        /// <returns>Listado de Puntos de medición</returns>
        public List<MeHojaptomedDTO> GetPuntosMedicionPR16(int emprcodi, string periodo)
        {
            //int IdFormato = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoPR16"]);
            var formato = logic.GetByIdMeFormato(this.IdFormato);
            string fecha = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, periodo, string.Empty, string.Empty, "dd/MM/yyyy").ToString("dd/MM/yyyy");
            List<MeFormatoEmpresaDTO> listPeriodoEnvio = this.ObtenerListaPeriodoEnvio(fecha, formato.Formatcodi, emprcodi);
            MeFormatoEmpresaDTO periodoEnvio = new MeFormatoEmpresaDTO();
            periodoEnvio = listPeriodoEnvio[0];
            var cabecera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
            return FactorySic.GetMeHojaptomedRepository().GetPtoMedicionPR16(emprcodi, IdFormato, periodoEnvio.PeriodoFechaIni.Value.ToString("dd/MM/yyyy"), cabecera.Cabquery);
        }

        /// <summary>
        /// Método que determina si la empresa puede enviar información para determinado periodo
        /// </summary>
        /// <param name="idEmpresa">Código de empresa</param>
        /// <param name="periodo">Periodo de envio</param>
        /// <param name="enPlazo">Si envio es en plazo o no</param>
        /// <returns>Indica si la empresa puede o no enviar información</returns>
        public bool ValidarFechaPr16(int idEmpresa, string periodo, out bool enPlazo)
        {

            MeFormatoDTO formato = logic.GetByIdMeFormato(this.IdFormato);

            string fecha = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, periodo, string.Empty, string.Empty, "dd/MM/yyyy").ToString("dd/MM/yyyy");
            List<MeFormatoEmpresaDTO> listPeriodoEnvio = this.ObtenerListaPeriodoEnvio(fecha, formato.Formatcodi, idEmpresa);
            MeFormatoEmpresaDTO periodoEnvio = new MeFormatoEmpresaDTO();
            periodoEnvio = listPeriodoEnvio[0];
            formato.FechaInicio = periodoEnvio.PeriodoFechaIni.Value;
            formato.FechaFin = periodoEnvio.PeriodoFechaFin.Value;
            formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, periodo, string.Empty, string.Empty, "dd/MM/yyyy");
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            int horaini = 0;
            int horafin = 0;

            DateTime fechaMinimaEnvio = formato.FechaProceso.AddMonths(1);
            DateTime fechamaximaEnvio = fechaMinimaEnvio.AddDays(formato.Formatdiaplazo - 1).AddMinutes(formato.Formatminplazo);

            if (fechaActual >= fechaMinimaEnvio && fechaActual <= fechamaximaEnvio)
            {
                //- En Plazo
                //- La fecha actual de remisión se encuentra dentro del plazo.
                enPlazo = true;
                resultado = true;
            }
            else if (fechaActual > fechamaximaEnvio)
            {
                //- Fuera de plazo
                //- La fecha actual de remisión se encuentra después de la fecha máxima permitida.
                var regfechaPlazo = this.logic.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null) // si existe registro de ampliacion
                {

                    if ((fechaActual >= fechaMinimaEnvio) && (fechaActual <= regfechaPlazo.Amplifechaplazo))
                    {
                        resultado = true;
                    }
                }
                enPlazo = false;
            }
            else
            {
                //- Envío inconsistente porque la fecha de envío es anterior a la toma de medición.
                resultado = false;
                enPlazo = false;
            }
            if ((formato.Formatdiaplazo == 0) && (resultado)) //Formato Tiempo Real
            {
                int hora = fechaActual.Hour;
                if (((hora - 1) % 3) == 0)
                {
                    horaini = hora - 1 - 1 * 3;
                    horafin = hora - 1;
                }
                else
                {
                    horafin = -1;//indica que formato tiempo real no tiene filas habilitadas
                    resultado = false;
                }
            }
            return resultado;
        }

        /// <summary>
        /// Obtener el Ptosuministro
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public MePtosuministradorDTO GetByPtoPeriodo(int ptomedicodi, string fecha)
        {
            return FactorySic.GetMePtosuministradorRepository().GetByPtoPeriodo(ptomedicodi, fecha);
        }

        /// <summary>
        /// Actualizar el registro
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateMePtosuministro(MePtosuministradorDTO entity)
        {
            try
            {
                FactorySic.GetMePtosuministradorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Grabar un nuevo punto de suministro
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int SaveMePtosuministro(MePtosuministradorDTO entity)
        {
            try
            {
                return FactorySic.GetMePtosuministradorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- JDEL Fin
        public MePtosuministradorDTO GetbyidPtoSuministrador(int ptosucodi)
        {
            try
            {
                return FactorySic.GetMePtosuministradorRepository().GetById(ptosucodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        public MePtosuministradorDTO ObtenerSuministradorVigente(int ptomedicodi)
        {
            try
            {
                return FactorySic.GetMePtosuministradorRepository().ObtenerSuministradorVigente(ptomedicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la lista de periodos Sicli.
        /// </summary>
        /// <returns></returns>
        public List<IioPeriodoSicliDTO> ListaPeriodoSicli()
        {
            try
            {
                return FactorySic.GetPeriodoSicliRepository().ListaPeriodoActivo();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<MeAmpliacionfechaDTO> GetListaAmpliacionFiltro(DateTime periodo, int empresa, int formato, int regIni, int regFin)
        {
            return FactorySic.GetMeAmpliacionfechaRepository().GetListaAmpliacionFiltro(periodo, empresa, formato, regIni, regFin);
        }

        public int GetListaAmpliacionFiltroCount(DateTime periodo, int empresa, int formato)
        {
            return FactorySic.GetMeAmpliacionfechaRepository().GetListaAmpliacionFiltroCount(periodo, empresa, formato);
        }

        public List<SiEmpresaDTO> ListaEmpresasAmpliacionPlazo()
        {
            return FactorySic.GetMeAmpliacionfechaRepository().ListaEmpresasAmpliacionPlazo();
        }

        #region Mejoras PR16
        public List<MeDemandaMLibreDTO> ListDemandaMercadoLibreReporte(string periodo, string suministrador, string empresa, int regIni, int regFin)
        {
            return FactorySic.GetMeDemandaMercadoLibreRepository().ListDemandaMercadoLibreReporte(periodo, suministrador, empresa, regIni, regFin);
        }

        public int ListDemandaMercadoLibreReporteCount(string periodo, string suministrador, string empresa)
        {
            return FactorySic.GetMeDemandaMercadoLibreRepository().ListDemandaMercadoLibreReporteCount(periodo, suministrador, empresa);
        }

        public int GenerarRegistroDemandas(string periodo, string periodoSicli, string nombreUsuario, string fechaDemandaMaxima, string fechaDemandaMaximaSicli)
        {
            var resultado = 1;
            try
            {
                FactorySic.GetMeDemandaMercadoLibreRepository().Delete(periodo);     
                var codigoMaximoML = FactorySic.GetMeDemandaMercadoLibreRepository().GetMaxId();
                FactorySic.GetMeDemandaMercadoLibreRepository().Save(periodo, periodoSicli, codigoMaximoML, nombreUsuario, fechaDemandaMaxima, fechaDemandaMaximaSicli);
                FactorySic.GetMeDemandaMercadoLibreRepository().Update(nombreUsuario, periodo);       

            }
            catch (Exception ex)
            {
                resultado = -1;

                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }

        public System.Data.IDataReader ListDemandaMercadoLibreReporteExcel(string periodo, string periodoSICLI, string suministrador, string empresa)
        {
            return FactorySic.GetMeDemandaMercadoLibreRepository().ListDemandaMercadoLibreReporteExcel(periodo, periodoSICLI, suministrador, empresa);
        }

        public List<IioPeriodoSicliDTO> ListPeriodoSicli(string permiso)
        {
            return FactorySic.GetMeDemandaMercadoLibreRepository().ListPeriodoSicli(permiso);
        }
        public int UpdatePeriodoDemandaSicli(string usuario, string periodo, string estadoPeriodo)
        {
            var resultado = 1;
            try
            {

                resultado = FactorySic.GetMeDemandaMercadoLibreRepository().UpdatePeriodoDemandaSicli(usuario, periodo, estadoPeriodo);

            }
            catch (Exception ex)
            {
                resultado = -1;

                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }
        #endregion
    }
}
