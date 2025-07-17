using System;
using System.Linq;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.RDO.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.StockCombustibles;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using COES.Servicios.Aplicacion.Interconexiones.Helper;
using log4net;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Globalization;
using System.Text;
using COES.Base.Tools;

namespace COES.Servicios.Aplicacion.RDO
{
    /// <summary>
    /// Clases con métodos del módulo 
    /// </summary>
    public class RDOAppServicio : AppServicioBase
    {
        #region Declaración de variables

        EmpresaAppServicio servEmpresa = new EmpresaAppServicio();

        #endregion

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RDOAppServicio));
        
        /// <summary>
        /// Permite realizar búsquedas en la tabla EpoEstudioEo
        /// </summary>
        public List<RdoCumplimiento> GetByCriteriaRdoCumplimiento(RdoCumplimiento obj)
        {
            return FactorySic.GetRdoCumplimientoRepository().GetByCriteria(obj);          
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_FORMATO
        /// </summary>
        public MeFormatoDTO GetByIdMeFormato(int formatcodi)
        {
            var formato = FactorySic.GetMeFormatoRepository().GetById(formatcodi);
            return formato;
        }

        /// <summary>
        /// Devuelve lista de cabeceras de formato
        /// </summary>
        /// <returns></returns>
        public List<MeCabeceraDTO> GetListMeCabecera()
        {
            return FactorySic.GetCabeceraRepository().List();
        }

        /// <summary>
        /// Devuelve entidad empresadto buscado por id
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public SiEmpresaDTO GetByIdSiEmpresa(int idEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);
        }

        /// <summary>
        /// Indica si esta vigente una empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public bool EsEmpresaVigente(int emprcodi, DateTime fechaConsulta)
        {
            return this.servEmpresa.EsEmpresaVigente(emprcodi, fechaConsulta);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnvio
        /// </summary>
        public List<MeEnvioDTO> GetByCriteriaMeEnvios(int idEmpresa, int idFormato, DateTime fecha)
        {
            return FactorySic.GetMeEnvioRepository().GetByCriteria(idEmpresa, idFormato, fecha);
        }

        /// <summary>
        /// Configuración de Plazo de Envio de los Formatos
        /// </summary>
        /// <param name="formato"></param>
        public static void GetSizeFormato(MeFormatoDTO formato)
        {
            switch (formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                    if (formato.Lecttipo == ParametrosFormato.Programado)
                    {
                        formato.FechaInicio = formato.FechaProceso.AddDays(1);
                        formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte);
                    }
                    else
                    {
                        //Ejecutado o Informacion en Tiempo Real
                        formato.FechaInicio = formato.FechaProceso;
                        formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte - 1);
                    }
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    formato.FechaInicio = formato.FechaProceso;
                    formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte - 1);

                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionSemana:
                            formato.RowPorDia = 1;
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana: //Semanal Mediano Plazo
                    formato.RowPorDia = 1;

                    if (formato.Lecttipo == ParametrosFormato.Ejecutado) //Ejecutado
                    {
                        //fecha inicio es primera semana del año
                        //fecha fin es ultima semana antes del mes seleccionado
                        //si se selecciona enero se toma todo el año anterior
                        if (formato.FechaProceso.Month == 1)
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6));
                            formato.Formathorizonte = EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6);
                        }
                        else
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(-4));
                            formato.Formathorizonte = EPDate.f_numerosemana(formato.FechaFin);
                        }
                    }
                    else //Programado
                    {
                        //fecha inicio es la primera semana del mes seleccionado
                        //fecha fin es la ultima semana del año
                        formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(3));//EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(7));
                        formato.FechaFin = formato.FechaInicio.AddDays(56 * 7);//EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(56 * 7));
                        formato.Formathorizonte = 56; //EPDate.f_numerosemana(formato.FechaFin) +
                    }
                    break;
                case ParametrosFormato.PeriodoMensual:
                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days + 1;
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days;
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionMes:
                            formato.RowPorDia = 1;
                            if (formato.Lecttipo == ParametrosLectura.Ejecutado)
                            {
                                if (formato.FechaProceso.Month == 1)
                                {
                                    formato.Formathorizonte = 12;
                                    formato.FechaInicio = new DateTime(formato.FechaProceso.Year - 1, 1, 1);
                                }
                                else
                                {
                                    formato.Formathorizonte = formato.FechaProceso.Month - 1;
                                    formato.FechaInicio = new DateTime(formato.FechaProceso.Year, 1, 1);
                                }

                                formato.FechaFin = formato.FechaProceso;
                            }
                            else // Programado
                            {
                                if (formato.Formathorizonte == 90)
                                {
                                    // Carga de Demanda Coincidente GMME-LHBN
                                    formato.FechaInicio = formato.FechaProceso;
                                    formato.FechaFin = formato.FechaProceso.AddMonths(formato.Formathorizonte);
                                    formato.Formathorizonte = 3;
                                }
                                else
                                {

                                    formato.FechaInicio = formato.FechaProceso;
                                    formato.FechaFin = formato.FechaProceso.AddMonths(12);
                                    formato.Formathorizonte = 12;
                                }
                            }
                            break;
                    }

                    break;
            }

            //formato.FechaPlazoIni = formato.FechaProceso.AddMonths(formato.Formatmesplazo).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
            formato.FechaPlazoIni = formato.FechaProceso.AddMonths(formato.Formatmesplazo).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
            //formato.FechaPlazo = formato.FechaProceso.AddMonths(formato.Formatmesfinplazo).AddDays(formato.Formatdiafinplazo).AddMinutes(formato.Formatminfinplazo);
            formato.FechaPlazo = DateTime.Now.AddHours(1);
            formato.FechaPlazoFuera = formato.FechaProceso.AddMonths(formato.Formatmesfinfueraplazo).AddDays(formato.Formatdiafinfueraplazo).AddMinutes(formato.Formatminfinfueraplazo);
        }

        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuera de plazo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        public bool ValidarPlazo(MeFormatoDTO formato)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;

            //Validación de vigencia de empresa
            if (!this.EsEmpresaVigente(formato.Emprcodi, fechaActual))
            {
                return false;
            }

            DateTime fechaEnvio = formato.IdEnvio > 0 && formato.FechaEnvio != null ? formato.FechaEnvio.Value : fechaActual;

            if ((fechaEnvio >= formato.FechaPlazoIni) && (fechaEnvio <= formato.FechaPlazo))
            {
                resultado = true;
            }
            return resultado;
        }

        /// <summary>
        /// Verifica si un Envio de Información esta en plazo, fuera de plazo, deshabilitado
        /// </summary>
        /// <param name="plazo"></param>
        /// <returns></returns>
        public string EnvioValidarPlazo(MeFormatoDTO formato, int idEmpresa)
        {
            string resultado = ConstantesEnvioRdo.ENVIO_PLAZO_DESHABILITADO;

            DateTime fechaValidacion = DateTime.Now;

            //Validación de vigencia de empresa
            if (!this.EsEmpresaVigente(formato.Emprcodi, fechaValidacion))
            {
                return ConstantesEnvioRdo.ENVIO_PLAZO_DESHABILITADO;
            }

            if (formato.FechaPlazoIni <= fechaValidacion && fechaValidacion <= formato.FechaPlazoFuera)
            {
                return fechaValidacion <= formato.FechaPlazo ? ConstantesEnvioRdo.ENVIO_EN_PLAZO : ConstantesEnvioRdo.ENVIO_FUERA_PLAZO;
            }
            else
            {
                //buscar en ampliación
                MeAmpliacionfechaDTO regfechaPlazo = this.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null)
                {
                    if ((fechaValidacion >= formato.FechaPlazoIni) && (fechaValidacion <= regfechaPlazo.Amplifechaplazo))
                    {
                        return ConstantesEnvioRdo.ENVIO_FUERA_PLAZO;
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public MeAmpliacionfechaDTO GetByIdMeAmpliacionfecha(DateTime fecha, int empresa, int formato)
        {
            return FactorySic.GetMeAmpliacionfechaRepository().GetById(fecha, empresa, formato);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_ENVIO
        /// </summary>
        public MeEnvioDTO GetByIdMeEnvio(int idEnvio)
        {
            return FactorySic.GetMeEnvioRepository().GetById(idEnvio);
        }

        /// <summary>
        /// Obtiene lista de ptos de medicion del formato de acuerdo al envio indicado, toma en cuenta
        /// los ptos de medicion que habian en el momento del envio
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="idCfgFormato"></param>
        /// <param name="emprcodi"></param>
        /// <param name="formatcodi"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<MeHojaptomedDTO> GetListaPtos(DateTime fechaPeriodo, int idCfgFormato, int emprcodi, int formatcodi, string query)
        {
            var lista = GetByCriteria2MeHojaptomeds(emprcodi, formatcodi, query, fechaPeriodo, fechaPeriodo);
            if (idCfgFormato == 0)
            {
                lista = lista.Where(x => x.Hojaptoactivo == 1).ToList();
            }
            else
            {
                var config = GetByIdMeConfigformatenvio(idCfgFormato);

                if (config.Cfgenvhojas.Contains(','))
                {
                    List<int> listaHojacodi = config.Cfgenvhojas.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                    List<MeHojaptomedDTO> listTemp = new List<MeHojaptomedDTO>();

                    for (int posHoja = 0; posHoja < listaHojacodi.Count(); posHoja++)
                    {
                        int hojacodi = listaHojacodi[posHoja];
                        string strCfgenvptos = config.Cfgenvptos.Split('|')[posHoja].Trim();
                        string strCfgenvtipoinf = config.Cfgenvtipoinf.Split('|')[posHoja].Trim();
                        string strCfgenvtipopto = config.Cfgenvtipopto.Length > 0 ? config.Cfgenvtipopto.Split('|')[posHoja].Trim() : string.Empty;
                        string strCfgenvorden = config.Cfgenvorden.Split('|')[posHoja].Trim();

                        if (strCfgenvptos != string.Empty && strCfgenvtipoinf != string.Empty && strCfgenvorden != string.Empty)
                        {
                            List<int> ptos = strCfgenvptos.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                            List<int> tipoinfos = strCfgenvtipoinf.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                            List<int> tipoptos = strCfgenvtipopto.Length > 0 ? strCfgenvtipopto.Split(',').Select(s => Convert.ToInt32(s)).ToList() : new List<int>();
                            List<int> orden = strCfgenvorden.Split(',').Select(s => Convert.ToInt32(s)).ToList();

                            var listaXHoja = lista.Where(x => ptos.Contains(x.Ptomedicodi) && x.Hojacodi == hojacodi).ToList();
                            for (var i = 0; i < ptos.Count; i++)
                            {
                                if (tipoptos.Count == 0) //la hoja no tiene tptomedicodis
                                {
                                    var find = listaXHoja.Find(x => x.Ptomedicodi == ptos[i] && x.Tipoinfocodi == tipoinfos[i]);
                                    if (find != null)
                                    {
                                        find.Hojaptoorden = orden[i];
                                    }
                                }
                                else
                                {
                                    var find = listaXHoja.Find(x => x.Ptomedicodi == ptos[i] && x.Tipoinfocodi == tipoinfos[i] && x.Tptomedicodi == tipoptos[i]);
                                    if (find != null)
                                    {
                                        find.Hojaptoorden = orden[i];
                                        listTemp.Add(find);
                                    }
                                }
                            }
                        }
                    }

                    if (listTemp.Count > 0) //existen configuraciones que guardan tptomedicodi
                    {
                        lista = listTemp;
                    }

                }
                else
                {
                    List<int> ptos = config.Cfgenvptos.Replace("|", ",").Split(',').Select(s => Convert.ToInt32(s)).ToList();
                    List<int> tipoinfos = config.Cfgenvtipoinf.Replace("|", ",").Split(',').Select(s => Convert.ToInt32(s)).ToList();
                    List<int> orden = config.Cfgenvorden.Replace("|", ",").Split(',').Select(s => Convert.ToInt32(s)).ToList();
                    lista = lista.Where(x => ptos.Contains(x.Ptomedicodi)).ToList();
                    for (var i = 0; i < ptos.Count; i++)
                    {
                        var find = lista.Find(x => x.Ptomedicodi == ptos[i] && x.Tipoinfocodi == tipoinfos[i]);
                        if (find != null)
                        {
                            find.Hojaptoorden = orden[i];
                        }
                    }
                }

                lista = lista.OrderBy(x => x.Hojaptoorden).ToList();
            }

            return lista;
        }

        /// <summary>
        /// Permite listar los puntos de medicion la tabla MeHojaptomed
        /// </summary>
        public List<MeHojaptomedDTO> GetByCriteria2MeHojaptomeds(int emprcodi, int formatcodi, string query, DateTime fechaIni, DateTime fechaFin)
        {
            return FactorySic.GetMeHojaptomedRepository().GetByCriteria2(emprcodi, formatcodi, query, fechaIni, fechaFin);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_CONFIGFORMATENVIO
        /// </summary>
        public MeConfigformatenvioDTO GetByIdMeConfigformatenvio(int idCfgenv)
        {
            return FactorySic.GetMeConfigformatenvioRepository().GetById(idCfgenv);
        }

        /// <summary>
        /// Listar la configuración de plazo segun la fecha periodo y el formato seleccionado
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <param name="listaHojaPto"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public void ListarConfigPlazoXFormatoYFechaPeriodo(int formatcodi, List<MeHojaptomedDTO> listaHojaPto, DateTime fechaPeriodo)
        {
            //Configuracion de plazo
            List<MePlazoptoDTO> listPlazoBD = this.ListMePlazoptos().Where(x => x.Formatcodi == formatcodi).OrderByDescending(x => x.Plzptofechavigencia).ToList();

            foreach (var reg in listaHojaPto)
            {
                MePlazoptoDTO objConfPlazo = listPlazoBD.Find(x => x.Plzptofechavigencia.Value.Date <= fechaPeriodo && x.Ptomedicodi == reg.Ptomedicodi && x.Formatcodi == formatcodi);
                reg.ConfigPto = objConfPlazo;
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_PLAZOPTO
        /// </summary>
        public List<MePlazoptoDTO> ListMePlazoptos()
        {
            return FactorySic.GetMePlazoptoRepository().List();
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idUltimoEnvio"></param>
        /// <returns></returns>
        public List<Object> GetDataFormato(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio, string horario)
        {
            List<Object> listaGenerica = new List<Object>();

            //asignar codigo de formato temporalmente
            var formatoValidate = formato.Formatdependeconfigptos != null ? (int)formato.Formatdependeconfigptos : formato.Formatcodi;
            var formatcodiInicio = formato.Formatcodi;
            formato.Formatcodi = formatoValidate;

            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionHora:
                    //List<MeMedicion24DTO> lista24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    List<MeMedicion24DTO> lista24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivoEjecutados(formato.Formatcodi, idEmpresa, formato.FechaInicio.AddDays(-1), formato.FechaInicio, horario);
                    if (idEnvio != 0)
                    {
                        var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 24; i++)
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
                    else
                    {
                        if (formato.Formatsecundario != 0 && lista24.Count == 0)
                        {
                            lista24 = FactorySic.GetMeMedicion24Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                        }
                    }
                    foreach (var reg in lista24)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
                    if (!formato.FlagUtilizaHoja)
                    {
                        lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio, formato.FechaFin, formato.Lectcodi);
                    }
                    else
                    {
                        if (ConstantesIEOD.IdFormatoDemandaDiaria == formato.Formatcodi)
                        {
                            List<MeMedicion48DTO> lista48Tmp = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo2(formato.Formatcodi, idEmpresa.ToString(), formato.FechaProceso.Date.AddDays(-1), formato.FechaProceso.Date.AddDays(1));
                            foreach (var hoja in formato.ListaHoja)
                            {
                                if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == hoja.Lectcodi)
                                {
                                    DateTime fecha = formato.FechaProceso.Date.AddDays(1);
                                    lista48.AddRange(lista48Tmp.Where(x => x.Medifecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                                }
                                else
                                {
                                    DateTime fecha = formato.FechaProceso.Date.AddDays(-1);
                                    lista48.AddRange(lista48Tmp.Where(x => x.Medifecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                                }
                            }
                        }
                        else
                        {
                            lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo2(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio, formato.FechaFin);
                        }
                    }

                    if (idEnvio != 0)
                    {
                        List<MeCambioenvioDTO> lista = new List<MeCambioenvioDTO>();
                        if (ConstantesIEOD.IdFormatoDemandaDiaria == formato.Formatcodi)
                        {
                            List<MeCambioenvioDTO> listaTmp = this.GetAllCambioEnvio(formato.Formatcodi, formato.FechaProceso.Date.AddDays(-1), formato.FechaProceso.Date.AddDays(1), idEnvio, idEmpresa);

                            foreach (var hoja in formato.ListaHoja)
                            {
                                if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == hoja.Lectcodi)
                                {
                                    DateTime fecha = formato.FechaProceso.Date.AddDays(1);
                                    lista.AddRange(listaTmp.Where(x => x.Cambenvfecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                                }
                                else
                                {
                                    DateTime fecha = formato.FechaProceso.Date.AddDays(-1);
                                    lista.AddRange(listaTmp.Where(x => x.Cambenvfecha == fecha && x.Hojacodi == hoja.Hojacodi).ToList());
                                }
                            }
                        }
                        else
                        {
                            lista = this.GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        }

                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                MeMedicion48DTO find = !formato.FlagUtilizaHoja
                                ? lista48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha)
                                : lista48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Medifecha == reg.Cambenvfecha && x.Hojacodi == reg.Hojacodi);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 48; i++)
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

                    foreach (var reg in lista48)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;

                case ParametrosFormato.ResolucionCuartoHora:
                    List<MeMedicion96DTO> lista96 = FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    if (idEnvio != 0)
                    {
                        var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
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

                    foreach (var reg in lista96)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
            }

            formato.Formatcodi = formatcodiInicio;
            return listaGenerica;
        }

        /// <summary>
        /// Obtiene el maximo id del envio de un formato
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public int ObtenerIdMaxEnvioFormato(int idFormato, int idEmpresa)
        {
            return FactorySic.GetMeEnvioRepository().GetMaxIdEnvioFormato(idFormato, idEmpresa);
        }

        /// <summary>
        /// Lista todos los cambios realizados en un envio
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<MeCambioenvioDTO> GetAllCambioEnvio(int idFormato, DateTime fechaInicio, DateTime fechaFin, int idEnvio, int idEmpresa)
        {
            return FactorySic.GetMeCambioenvioRepository().GetAllCambioEnvio(idFormato, fechaInicio, fechaFin, idEnvio, idEmpresa);
        }

        /// <summary>
        /// Lista información de envios d3 24
        /// </summary>
        /// <param name="Formatcodi"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="FechaInicio"></param>
        /// <param name="FechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GetDataAnt(int Formatcodi, int idEmpresa, DateTime FechaInicio, DateTime FechaFin)
        {
            return FactorySic.GetMeMedicion24Repository().GetEnvioArchivo(Formatcodi, idEmpresa, FechaInicio, FechaFin);
        }

        /// <summary>
        /// Permite listar los puntos de medicion la tabla MeHojaptomed
        /// </summary>
        public List<MeHojaptomedDTO> GetByCriteriaMeHojaptomeds(int emprcodi, int formatcodi, DateTime fechaIni, DateTime fechaFin)
        {
            return FactorySic.GetMeHojaptomedRepository().GetByCriteria(emprcodi, formatcodi, fechaIni, fechaFin);
        }

        /// <summary>
        /// Obtiene el valor del formato seg{un su configuración
        /// </summary>
        /// <param name="formatoReal"></param>
        /// <returns></returns>
        public int ObtenerIdFormatoPadre(int formatoReal)
        {
            RDOAppServicio logic = new RDOAppServicio();

            var formato = logic.GetByIdMeFormato(formatoReal);
            int idFormato = formato.Formatdependeconfigptos != null ? formato.Formatdependeconfigptos.Value : formato.Formatcodi;

            return idFormato;
        }

        /// <summary>
        /// Genera Excel de Generación de Despacho.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public void GenerarFileExcelGeneracionDespacho(FormatoModel model, string ruta)
        {
            string fileTemplate = ConstantesGeneracionDespachoRDO.PlantillaGeneracionDespachoRDO;
            FileInfo template = new FileInfo(ruta + fileTemplate);
            FileInfo newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesRDO.HojaFormatoExcel];
                //Escribe nombre de encabezado
                ws.Cells[2, ParametrosGeneracionDespachoExcel.ColEmpresa].Value = model.Empresa;
                ws.Cells[ParametrosGeneracionDespachoExcel.RowFormato, 2].Value = model.Formato.Formatnombre;

                //Imprimimos Codigo Empresa y Codigo Formato
                ws.Cells[1, ParametrosFormato.ColEmpresa].Value = model.IdEmpresa.ToString();
                ws.Cells[1, ParametrosFormato.ColFormato].Value = model.Formato.Formatcodi.ToString();

                ///Descripcion del Formato
                /// Nombre de la Empresa
                int row = 2;
                int column = 2;
                switch (model.Formato.Formatperiodo)
                {
                    case ParametrosFormato.PeriodoDiario:
                        ws.Cells[row + 2, column].Value = model.Handson.ListaExcelData[4][0];
                        ws.Cells[row + 2, column].Value = model.Formato.FechaInicio.ToString(ConstantesRDO.FormatoFecha);
                        row = row + 3;
                        break;
                    case ParametrosFormato.PeriodoSemanal:
                        var ultimaFila = model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows;
                        ws.Cells[row + 2, column - 1].Value = "Semana";
                        var semanaLength = model.Semana.Length;
                        ws.Cells[row + 2, column].Value = model.Semana.Substring(4, semanaLength - 4);
                        ws.Cells[row + 3, column - 1].Value = "Fecha Desde";
                        ws.Cells[row + 3, column - 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row + 3, column - 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCFFFF"));
                        ws.Cells[row + 3, column - 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                        ws.Cells[row + 3, column].Value = model.Formato.FechaInicio.ToString(ConstantesRDO.FormatoFecha);
                        ws.Cells[row + 4, column - 1].Value = "Fecha Hasta";
                        ws.Cells[row + 4, column - 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row + 4, column - 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCFFFF"));
                        ws.Cells[row + 4, column - 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                        ws.Cells[row + 4, column].Value = model.Formato.FechaFin.ToString(ConstantesRDO.FormatoFecha);
                        row = row + 4;
                        break;
                }

                ///Imprimimos cabecera de puntos de medicion
                row = ParametrosGeneracionDespachoExcel.RowDatos;
                int totColumnas = model.ListaHojaPto.Count;
                int columnIni = ParametrosGeneracionDespachoExcel.ColDatos;

                for (var i = 0; i <= model.ListaHojaPto.Count; i++)
                {
                    for (var j = 0; j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows; j++)
                    {
                        decimal valor = 0;
                        bool canConvert = decimal.TryParse(model.Handson.ListaExcelData[j][i], out valor);
                        if (canConvert)
                            ws.Cells[row + j, i + 1].Value = valor;
                        else
                            ws.Cells[row + j, i + 1].Value = model.Handson.ListaExcelData[j][i];
                        ws.Cells[row + j, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        if (j < model.Formato.Formatrows && i >= model.Formato.Formatcols)
                        {
                            //ws.Cells[row + j, i + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells[row + j, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            ws.Cells[row + j, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            ws.Cells[row + j, i + 1].Style.WrapText = true;
                        }
                    }
                }
                /////////////////Formato a Celdas Head ///////////////////
                using (var range = ws.Cells[row, 2, row + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#99CCFF"));
                }
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[row + model.Formato.Formatrows, 2, row + model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }

                /////////////////////// Celdas Merge /////////////////////

                foreach (var reg in model.Handson.ListaMerge)
                {
                    int fili = row + reg.row;
                    int filf = row + reg.row + reg.rowspan - 1;
                    int coli = reg.col + 1;
                    int colf = reg.col + reg.colspan - 1 + 1;
                    ws.Cells[fili, coli, filf, colf].Merge = true;
                }

                xlPackage.Save();
            }

        }

        /// <summary>
        /// Verifica Formato
        /// </summary>
        /// <param name="file"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        public int VerificarIdsFormato(string file, int idEmpresa, int idFormato)
        {
            int retorno = 1;
            int idEmpresaArchivo;
            int idFormatoEmpresa;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                var valorEmp = ws.Cells[1, ParametrosFormato.ColEmpresa].Value.ToString();
                if (!int.TryParse(valorEmp, NumberStyles.Any, CultureInfo.InvariantCulture, out idEmpresaArchivo))
                    idEmpresa = 0;
                var valorFormato = ws.Cells[1, ParametrosFormato.ColFormato].Value.ToString();
                if (!int.TryParse(valorFormato, NumberStyles.Any, CultureInfo.InvariantCulture, out idFormatoEmpresa))
                    idFormatoEmpresa = 0;
                if (idEmpresaArchivo != idEmpresa)
                {
                    retorno = -1;
                }
                if (idFormatoEmpresa != idFormato)
                {
                    retorno = -2;
                }
            }
            return retorno;
        }

        /// <summary>
        /// Obtiene tamaño de formato
        /// </summary>
        /// <param name="formato"></param>
        public void GetSizeFormato2(MeFormatoDTO formato)
        {
            switch (formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    formato.FechaInicio = formato.FechaProceso;
                    formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte);
                    formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                    if (formato.Formatdiaplazo == 0) //Informacion en Tiempo Real
                    {
                        formato.FechaPlazoIni = formato.FechaProceso;
                        formato.FechaPlazo = formato.FechaProceso.AddDays(1).AddMinutes(formato.Formatminplazo);
                    }
                    else
                    {
                        if (formato.Lecttipo == ParametrosLectura.Ejecutado) //Ejecutado
                        {
                            formato.FechaPlazoIni = formato.FechaProceso.AddDays(formato.Formatdiaplazo);
                            formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                        }
                        else
                        {
                            formato.FechaPlazoIni = formato.FechaProceso.AddDays(-1);
                            formato.FechaPlazo = formato.FechaProceso.AddDays(-1).AddMinutes(formato.Formatminplazo);
                        }
                    }
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    formato.FechaInicio = formato.FechaProceso;
                    formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte);
                    if (formato.Lecttipo == ParametrosLectura.Ejecutado) //Ejecutado
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddDays(7);
                        formato.FechaPlazo = formato.FechaProceso.AddDays(7 + formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                    }
                    else
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddDays(-7);
                        formato.FechaPlazo = formato.FechaProceso.AddDays(-7 + formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                    }
                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionSemana:
                            formato.RowPorDia = 1;
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana: //Semanal Mediano Plazo
                    formato.RowPorDia = 1;

                    if (formato.Lecttipo == ParametrosLectura.Ejecutado) //Ejecutado
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddMonths(1);
                        formato.FechaPlazo = formato.FechaProceso.AddMonths(1).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                        //fecha inicio es primera semana del año
                        //fecha fin es ultima semana antes del mes seleccionado
                        //si se selecciona enero se toma todo el año anterior
                        if (formato.FechaProceso.Month == 1)
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6));
                            formato.Formathorizonte = EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6);
                        }
                        else
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(-7));
                            formato.Formathorizonte = EPDate.f_numerosemana(formato.FechaFin);
                        }
                    }
                    else //Programado
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddMonths(-1);
                        formato.FechaPlazo = formato.FechaProceso.AddMonths(-1).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                        //fecha inicio es la primera semana del mes seleccionado
                        //fecha fin es la ultima semana del año
                        formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(7));
                        formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.AddYears(1));
                        formato.Formathorizonte = EPDate.f_numerosemana(formato.FechaFin) +
                            EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6) -
                            EPDate.f_numerosemana(formato.FechaInicio) + 1;
                    }
                    break;
                case ParametrosFormato.PeriodoMensual:
                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            //formato.FechaPlazo = formato.FechaFin.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.FechaPlazo = formato.FechaProceso;
                            formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days + 1;
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            //formato.FechaPlazo = formato.FechaFin.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.FechaPlazo = formato.FechaProceso;
                            formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days;
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionMes:
                            formato.RowPorDia = 1;
                            if (formato.Lecttipo == ParametrosLectura.Ejecutado)
                            {
                                formato.Formathorizonte = formato.FechaProceso.Month;
                                formato.FechaFin = formato.FechaProceso;
                                formato.FechaInicio = new DateTime(formato.FechaProceso.Year, 1, 1);
                                //formato.FechaPlazo = formato.FechaFin.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                                formato.FechaPlazo = formato.FechaProceso.AddMonths(1);
                                formato.FechaPlazo = formato.FechaProceso.AddMonths(1).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            }
                            else // Programado
                            {
                                formato.FechaInicio = formato.FechaProceso;
                                formato.FechaFin = formato.FechaProceso.AddMonths(12);
                                formato.Formathorizonte = 12;
                                //formato.FechaPlazo = formato.FechaInicio.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                                formato.FechaPlazo = formato.FechaProceso;
                                formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            }
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Inicializa las dimensiones de la matriz de valores del objeto excel web
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public string[][] InicializaMatrizExcel(int rowsHead, int nFil, int colsHead, int nCol)
        {
            string[][] matriz = new string[nFil + rowsHead][];
            for (int i = 0; i < nFil; i++)
            {

                matriz[i + rowsHead] = new string[nCol + colsHead];
                for (int j = 0; j < nCol; j++)
                {
                    matriz[i + rowsHead][j + colsHead] = string.Empty;
                }
            }
            return matriz;
        }

        /// <summary>
        /// Lee archivo excel cargado y llena matriz de datos para visualizacion web
        /// </summary>
        /// <param name="matriz"></param>
        /// <param name="file"></param>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <param name="formatocodi"></param>
        /// <returns></returns>
        public Boolean LeerExcelFile(string[][] matriz, string file, int rowsHead, int nFil, int colsHead, int nCol, int formatocodi = 0)
        {
            var filaExecelData = ParametrosFormato.RowDatos;

            switch (formatocodi)
            {
                case ConstantesGeneracionDespachoRDO.FormatoDiarioCodi:
                    filaExecelData = ParametrosGeneracionDespachoExcel.FilaExcelData;
                    break;

                default:
                    break;
            }

            Boolean retorno = false;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                /// Verificar Formato
                for (int i = 0; i < nFil; i++)
                {
                    for (int j = 0; j < nCol; j++)
                    {
                        string valor = (ws.Cells[i + rowsHead + filaExecelData, j + colsHead + 1].Value != null) ?
                            ws.Cells[i + rowsHead + filaExecelData, j + colsHead + 1].Value.ToString() : string.Empty;
                        matriz[i + rowsHead][j + colsHead] = valor;
                    }
                }
            }
            return retorno;
        }

        /// <summary>
        /// Borrar archivos
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public void BorrarArchivo(String archivo)
        {
            if (System.IO.File.Exists(@archivo))
            {
                try
                {
                    System.IO.File.Delete(@archivo);
                }
                catch (System.IO.IOException e)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Graba Configuracion de formato envio
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarConfigFormatEnvio(MeConfigformatenvioDTO entity)
        {
            int idCfgEnvio = 0;
            var lista = GetByCriteriaMeHojaptomeds((int)entity.Emprcodi, (int)entity.Formatcodi, entity.FechaInicio, entity.FechaInicio);
            if (ConstantesStockCombustibles.IdFormatoConsumo == entity.Formatcodi)
            {
                lista = lista.OrderBy(x => x.Equipadre).ThenByDescending(x => x.Tptomedicodi).ThenBy(x => x.Equinomb).ToList();
            }
            lista = lista.Where(x => x.Hojaptoactivo == 1).ToList();

            var listaHojacodi = lista.Select(x => x.Hojacodi).Distinct().ToList();

            if (lista.Count > 0)
            {
                string strCfgenvptos = string.Empty;
                string strCfgenvorden = string.Empty;
                string strCfgenvtipoinf = string.Empty;
                string strCfgenvtipopto = string.Empty;
                string strCfgenvhojas = string.Empty;

                if (listaHojacodi.Count > 0)
                {
                    List<string> listaCfgenvptos = new List<string>();
                    List<string> listaCfgenvorden = new List<string>();
                    List<string> listaCfgenvtipoinf = new List<string>();
                    List<string> listaCfgenvtipopto = new List<string>();
                    List<string> listaCfgenvhojas = new List<string>();

                    foreach (var hojacodi in listaHojacodi)
                    {
                        listaCfgenvptos.Add(string.Join(",", lista.Where(x => x.Hojacodi == hojacodi).Select(x => x.Ptomedicodi).ToList()));
                        listaCfgenvorden.Add(entity.Cfgenvorden = string.Join(",", lista.Where(x => x.Hojacodi == hojacodi).Select(x => x.Hojaptoorden).ToList()));
                        listaCfgenvtipoinf.Add(entity.Cfgenvtipoinf = string.Join(",", lista.Where(x => x.Hojacodi == hojacodi).Select(x => x.Tipoinfocodi).ToList()));
                        listaCfgenvtipopto.Add(entity.Cfgenvtipopto = string.Join(",", lista.Where(x => x.Hojacodi == hojacodi).Select(x => x.Tptomedicodi).ToList()));
                        listaCfgenvhojas.Add(hojacodi + string.Empty);
                    }

                    strCfgenvptos = string.Join("|", listaCfgenvptos);
                    strCfgenvorden = string.Join("|", listaCfgenvorden);
                    strCfgenvtipoinf = string.Join("|", listaCfgenvtipoinf);
                    strCfgenvtipopto = string.Join("|", listaCfgenvtipopto);
                    strCfgenvhojas = string.Join(",", listaCfgenvhojas);
                }
                else
                {
                    strCfgenvptos = string.Join(",", lista.Select(x => x.Ptomedicodi).ToList());
                    strCfgenvorden = string.Join(",", lista.Select(x => x.Hojaptoorden).ToList());
                    strCfgenvtipoinf = string.Join(",", lista.Select(x => x.Tipoinfocodi).ToList());
                    strCfgenvtipopto = string.Join(",", lista.Select(x => x.Tptomedicodi).ToList());
                }

                entity.Cfgenvptos = strCfgenvptos;
                entity.Cfgenvorden = strCfgenvorden;
                entity.Cfgenvtipoinf = strCfgenvtipoinf;
                entity.Cfgenvtipopto = strCfgenvtipopto;
                entity.Cfgenvhojas = strCfgenvhojas;

                idCfgEnvio = VerificaFormatoUpdate((int)entity.Emprcodi, (int)entity.Formatcodi, entity.Cfgenvptos, entity.Cfgenvorden, entity.Cfgenvtipoinf, entity.Cfgenvtipopto, entity.Cfgenvhojas);
                if (idCfgEnvio == 0)
                {
                    entity.Cfgenvfecha = DateTime.Now;
                    idCfgEnvio = SaveMeConfigformatenvio(entity);
                }
            }

            return idCfgEnvio;
        }

        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuyera de plazo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        public bool ValidarPlazoController(MeFormatoDTO formato)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            return resultado;
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIO
        /// </summary>
        public int SaveMeEnvio(MeEnvioDTO entity)
        {
            try
            {
                return FactorySic.GetMeEnvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Validar checkblanco
        /// </summary>
        /// <param name="puntos"></param>
        /// <param name="checkBlanco"></param>
        /// <returns></returns>
        public void ValidarCheckBlancoExcelWeb48(List<MeMedicion48DTO> puntos, int checkBlanco)
        {
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;

            foreach (var listaXPto in puntos.GroupBy(x => new { x.Ptomedicodi, x.Medifecha }))
            {
                foreach (var pto in listaXPto)
                {
                    for (var i = 1; i <= 48; i++)
                    {
                        var propiedad = pto.GetType().GetProperty("H" + i.ToString()).GetValue(pto);
                        stValor = propiedad != null ? propiedad.ToString() : "";
                        if (COES.Base.Tools.Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            pto.GetType().GetProperty("H" + i.ToString()).SetValue(pto, valor);
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                pto.GetType().GetProperty("H" + i.ToString()).SetValue(pto, null);
                            else
                                pto.GetType().GetProperty("H" + i.ToString()).SetValue(pto, 0);
                        }

                    }

                }
            }
        }

        /// <summary>
        /// Verificar formato update
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="listaPtos"></param>
        /// <param name="listaOrden"></param>
        /// <param name="listaTipoinf"></param>
        /// <param name="listaTipopto"></param>
        /// <param name="listaHoja"></param>
        /// <returns></returns>
        public int VerificaFormatoUpdate(int idEmpresa, int idFormato, string listaPtos, string listaOrden, string listaTipoinf, string listaTipopto, string listaHoja)
        {
            int idCfg = 0;
            var entity = GetByCriteriaMeConfigformatenvios(idEmpresa, idFormato).FirstOrDefault();
            if (entity != null)
            {
                string ptos = entity.Cfgenvptos == null ? string.Empty : entity.Cfgenvptos.Trim();
                string orden = entity.Cfgenvorden == null ? string.Empty : entity.Cfgenvorden.Trim();
                string tipoinf = entity.Cfgenvtipoinf == null ? string.Empty : entity.Cfgenvtipoinf.Trim();
                string tipopto = entity.Cfgenvtipopto == null ? string.Empty : entity.Cfgenvtipopto.Trim();
                string hojas = entity.Cfgenvhojas == null ? string.Empty : entity.Cfgenvhojas.Trim();

                if (listaPtos == ptos && listaOrden == orden && tipoinf == listaTipoinf && hojas == listaHoja && tipopto == listaTipopto)
                {
                    idCfg = entity.Cfgenvcodi;
                }
            }

            return idCfg;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeConfigformatenvio
        /// </summary>
        public List<MeConfigformatenvioDTO> GetByCriteriaMeConfigformatenvios(int idEmpresa, int idFormato)
        {
            return FactorySic.GetMeConfigformatenvioRepository().GetByCriteria(idEmpresa, idFormato);
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_CONFIGFORMATENVIO
        /// </summary>
        public int SaveMeConfigformatenvio(MeConfigformatenvioDTO entity)
        {
            int idCfgEnvio = 0;
            try
            {
                idCfgEnvio = FactorySic.GetMeConfigformatenvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return idCfgEnvio;
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarValoresCargados48_Intranet(List<MeMedicion48DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                List<MeMedicion48DTO> listaExtranet48 = new List<MeMedicion48DTO>();
                List<MeMedicion48DTO> listaIntranet48 = new List<MeMedicion48DTO>();

                foreach (var regxEnv in entitys)
                {
                    listaExtranet48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivoEjecutados(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio.AddDays(-1), formato.FechaInicio, formato.Lectcodi,"0");
                    listaIntranet48 = FactorySic.GetMeMedicion48Repository().GetEnvioMeMedicion48Intranet(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio.AddDays(-1), formato.FechaInicio, formato.Lectcodi);

                    var findExt = listaExtranet48.Find(x => x.Ptomedicodi == regxEnv.Ptomedicodi && x.Lectcodi == regxEnv.Lectcodi && x.Emprcodi == regxEnv.Emprcodi);
                    var findInt = listaIntranet48.Find(x => x.Ptomedicodi == regxEnv.Ptomedicodi && x.Lectcodi == regxEnv.Lectcodi && x.Emprcodi == regxEnv.Emprcodi);

                    if (findExt != null)
                    {
                        for (var x = 0; x < 48; x++)
                        {
                            var valInt = regxEnv.GetType().GetProperty("H" + (x + 1)).GetValue(regxEnv, null);
                            var valExt = findExt.GetType().GetProperty("H" + (x + 1)).GetValue(findExt, null);

                            if (valInt != null && Convert.ToDecimal(valExt) != Convert.ToDecimal(valInt) && valInt.ToString() != "")
                            {
                                findExt.GetType().GetProperty("H" + (x + 1).ToString()).SetValue(findExt, Convert.ToDecimal(valInt));
                            }
                            else if (findInt != null)
                            {
                                var valIntOld = findInt.GetType().GetProperty("H" + (x + 1)).GetValue(findInt, null);
                                if (valIntOld != null && valIntOld.ToString() != "")
                                    findExt.GetType().GetProperty("H" + (x + 1).ToString()).SetValue(findExt, Convert.ToDecimal(valInt));
                                else
                                    findExt.GetType().GetProperty("H" + (x + 1).ToString()).SetValue(findExt, null);
                            }
                            else
                                findExt.GetType().GetProperty("H" + (x + 1).ToString()).SetValue(findExt, null);
                        }
                    }
                    findExt.Medifecha = formato.FechaProceso;
                    findExt.Lastdate = formato.Lastdate;
                    findExt.Lastuser = usuario;
                    FactorySic.GetMeMedicion48Repository().SaveMeMedicion48Intranet(findExt, idEnvio);

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_ENVIO
        /// </summary>
        public void UpdateMeEnvio(MeEnvioDTO entity)
        {
            try
            {
                FactorySic.GetMeEnvioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Valida la fecha
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <returns></returns>
        public bool ValidarFecha(MeFormatoDTO formato, int idEmpresa, out int horaini, out int horafin)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            horaini = 0;
            horafin = 0;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            else
            {
                var regfechaPlazo = this.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null) // si existe registro de ampliacion
                {

                    if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= regfechaPlazo.Amplifechaplazo))
                    {
                        resultado = true;
                    }
                }
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
            return true;
            //return resultado;
        }

        /// <summary>
        /// Genera el Formato en Excel HTML
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idEnvio"></param>
        /// <param name="enPlazo"></param>
        /// <returns></returns>
        public string GenerarFormatoHtml(FormatoModel model, int idEnvio, Boolean enPlazo)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("");
            strHtml.Append("<div id='tab-2' class='ui-tabs-panel ui-widget-content ui-corner-bottom tab-panel js-tab-contents' name='grid'>");

            strHtml.Append("    <nav class='tool-menu tool-menu--grid'>");


            if (idEnvio <= 0)
            {
                strHtml.Append("        <ul class='tool-menu__btn-list'>");
                strHtml.Append("            <li><a class='link--tool js-download-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-download'></i><p class='tool-menu__icon-label'>Formato</p></a></li>");
                if (enPlazo)
                {
                    strHtml.Append("            <li><a id='btnSelectExcel3' class='link--tool js-add-grid' href='javascript:;' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Agregar</p></a></li>");
                    strHtml.Append("            <li><a class='link--tool js-save-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-save'></i><p class='tool-menu__icon-label'>Grabar</p></a></li>");
                }
                strHtml.Append("            <li><a class='link--tool js-export-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Exportar</p></a></li>");
                if (model.ListaEnvios.Count > 0)
                    strHtml.Append("            <li><a id='" + model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi.ToString() + "'class='link--tool js-reenvio-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Envíos</p></a></li>");
                strHtml.Append("            <li><a class='link--tool js-exit-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Salir</p></a></li>");
                strHtml.Append("        </ul>");

                strHtml.Append("        <ul class='tool-menu__btn-list'>");
                strHtml.Append("            <li><a class='link--tool js-nonumero-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p id='idNonum' class='tool-menu__icon-label'>0</p></a></li>");
                strHtml.Append("        </ul>");
            }
            else
            {
                strHtml.Append("        <ul class='tool-menu__btn-list'>");
                foreach (var reg in model.ListaEnvios)
                {
                    strHtml.Append("            <li><a id='" + reg.Enviocodi.ToString() + "' class='link--tool js-reenvio-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>" + reg.Enviocodi + "</p></a></li>");
                }
                strHtml.Append("            <li><a id='0' class='link--tool js-reenvio-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Envíar</p></a></li>");
                strHtml.Append("            <li><a class='link--tool js-exit-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Salir</p></a></li>");
                strHtml.Append("        </ul>");
            }

            strHtml.Append("    </nav>");


            strHtml.Append("<div style='clear:both; height:20px'></div>");
            strHtml.Append("<table class='table-form-vertical'>");
            strHtml.Append("  <tr><td >" + model.Formato.Areaname + " </td>");
            strHtml.Append("  <td>" + model.Formato.Formatnombre + " </td>");
            strHtml.Append("  <td>Empresa:</td><td>" + model.Empresa + "</td>");
            strHtml.Append("  <td>Año:</td><td>" + model.Anho + "</td>");
            switch (model.Formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    strHtml.Append("  <td>Mes:</td><td>" + model.Mes + "</td>");
                    strHtml.Append("  <td>Día:</td><td>" + model.Dia + "</td>");
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    strHtml.Append("  <td>Semana:</td><td>" + model.Semana + "</td>");
                    break;
                case ParametrosFormato.PeriodoMensual:
                case ParametrosFormato.PeriodoMensualSemana:
                    strHtml.Append("  <td>Mes:</td><td>" + model.Mes + "</td><tr>");
                    break;
            }
            strHtml.Append("</table></div>");


            return strHtml.ToString();
        }

        /// <summary>
        /// Inicializa lista de filas readonly para la matriz excel web
        /// </summary>
        /// <param name="filHead"></param>
        /// <param name="filData"></param>
        /// <param name="plazo"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <returns></returns>
        public List<bool> InicializaListaFilaReadOnly(int filHead, int filData, bool plazo, int horaini, int horafin)
        {
            List<bool> lista = new List<bool>();
            for (int i = 0; i < filHead; i++)
            {
                lista.Add(true);
            }
            for (int i = 0; i < filData; i++)
            {
                if (plazo)
                {
                    if (horafin == 0)
                        lista.Add(false);
                    else
                    {
                        if ((i >= horaini) && (i < horafin))
                        {
                            lista.Add(false);
                        }
                        else
                            lista.Add(true);
                    }
                }
                else
                    lista.Add(true);
            }

            return lista;
        }

        /// <summary>
        /// Devuelve la fecha del siguiente bloque
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public DateTime GetNextFilaHorizonte(int periodo, int resolucion, int horizonte, DateTime fechaInicio)
        {
            DateTime resultado = DateTime.MinValue;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            resultado = fechaInicio.AddMonths(horizonte);
                            break;

                        default:
                            resultado = fechaInicio.AddDays(horizonte);
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    resultado = fechaInicio.AddDays(horizonte * 7);
                    break;
                default:
                    resultado = fechaInicio.AddDays(horizonte);
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el nombre de la celda fechaa mostrarse en los formatos excel.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="tipoLectura"></param>
        /// <param name="horizonte"></param>
        /// <param name="indice"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public string ObtenerCeldaFecha(int periodo, int resolucion, int tipoLectura, int horizonte, int indice, DateTime fechaInicio)
        {
            string resultado = string.Empty;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            if (tipoLectura == ParametrosLectura.Ejecutado)
                                resultado = fechaInicio.Year.ToString() + " " + COES.Base.Tools.Util.ObtenerNombreMesAbrev(horizonte + 1);
                            else
                            {
                                resultado = fechaInicio.AddMonths(horizonte).Year.ToString() + " " + COES.Base.Tools.Util.ObtenerNombreMesAbrev(fechaInicio.AddMonths(horizonte).Month);
                            }
                            break;
                        default:
                            resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice).ToString(ConstantesRDO.FormatoFechaHora);
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    int semana = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(fechaInicio, 6) + horizonte;
                    int semanaMax = COES.Base.Tools.Util.TotalSemanasEnAnho(fechaInicio.Year, 6);
                    semana = (semana > semanaMax) ? semana - semanaMax : semana;
                    string stSemana = (semana > 9) ? semana.ToString() : "0" + semana.ToString();
                    if (tipoLectura == ParametrosLectura.Ejecutado)
                    {

                        resultado = fechaInicio.AddDays(horizonte * 7).Year.ToString() + " Sem:" + stSemana;
                    }
                    else
                    {
                        resultado = fechaInicio.AddDays(horizonte * 7).Year.ToString() + " Sem:" + stSemana;
                    }
                    break;
                default:
                    resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice).ToString(ConstantesRDO.FormatoFechaHora);
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idUltimoEnvio"></param>
        /// <returns></returns>
        public List<Object> GetDataFormatoEjecutados(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio, string horario)
        {
            List<Object> listaGenerica = new List<Object>();

            //asignar codigo de formato temporalmente
            var formatoValidate = formato.Formatdependeconfigptos != null ? (int)formato.Formatdependeconfigptos : formato.Formatcodi;
            var formatcodiInicio = formato.Formatcodi;
            formato.Formatcodi = formatoValidate;

            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionHora:
                    List<MeMedicion24DTO> lista24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivoEjecutados(formato.Formatcodi, idEmpresa, formato.FechaInicio.AddDays(-1), formato.FechaInicio, horario);
                    if (idEnvio != 0)
                    {
                        var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 24; i++)
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
                    else
                    {
                        if (formato.Formatsecundario != 0 && lista24.Count == 0)
                        {
                            lista24 = FactorySic.GetMeMedicion24Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                        }
                    }
                    foreach (var reg in lista24)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
                    if (!formato.FlagUtilizaHoja)
                    {
                        lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivoEjecutados(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio.AddDays(-1), formato.FechaInicio, formato.Lectcodi, horario);
                    }
                    else
                    {
                        lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo2(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio.AddDays(-1), formato.FechaInicio);
                    }

                    foreach (var reg in lista48)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;

                case ParametrosFormato.ResolucionCuartoHora:
                    List<MeMedicion96DTO> lista96 = FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    if (idEnvio != 0)
                    {
                        var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
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

                    foreach (var reg in lista96)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
            }

            formato.Formatcodi = formatcodiInicio;
            return listaGenerica;
        }

        /// <summary>
        /// Obtiene matriz de string que son solo los valores de las celdas del excel web ingresando como parametro una lista de mediciones.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lista"></param>
        /// <param name="listaCambios"></param>
        /// <param name="idEnvio"></param>
        /// <param name="listaLimites"></param>
        public void ObtieneMatrizWebExcel48Ejecutados(FormatoModel model, List<object> lista, List<MeCambioenvioDTO> listaCambios, int idEnvio, List<RDOLimiteRer> listaLimites)
        {
            try
            {
                if (idEnvio > 0)
                {
                    foreach (var reg in listaCambios)
                    {
                        if (reg.Cambenvcolvar != null)
                        {
                            var cambios = reg.Cambenvcolvar.Split(',');
                            for (var i = 0; i < cambios.Count(); i++)
                            {
                                TimeSpan ts = reg.Cambenvfecha - model.Formato.FechaInicio;
                                var horizon = ts.Days;
                                var col = model.ListaHojaPto.Find(x => x.Ptomedicodi == reg.Ptomedicodi).Hojaptoorden + model.ColumnasCabecera - 1;
                                var ColumReal = model.ListaHojaPto.FindIndex(x => x.Ptomedicodi == reg.Ptomedicodi) + 1;
                                col = ColumReal;
                                var row = model.FilasCabecera +
                                    ObtieneRowChange((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, int.Parse(cambios[i]),
                                    model.Formato.RowPorDia, reg.Cambenvfecha, model.Formato.FechaInicio);
                                //int.Parse(cambios[i]) + model.Formato.RowPorDia * horizon;
                                model.ListaCambios.Add(new CeldaCambios()
                                {
                                    Row = row,
                                    Col = col
                                });
                            }
                        }
                    }
                }
                for (int k = 0; k < model.ListaHojaPto.Count; k++)
                {
                    for (int z = 0; z < model.Formato.Formathorizonte; z++) //Horizonte se comporta como uno cuando resolucion es mas que un dia
                    {
                        DateTime fechaFind = GetNextFilaHorizonte((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, z, model.Formato.FechaInicio);
                        var reg = lista.Find(i => (int)i.GetType().GetProperty("Ptomedicodi").GetValue(i, null) == model.ListaHojaPto[k].Ptomedicodi
                                    && (DateTime)i.GetType().GetProperty("Medifecha").GetValue(i, null) == fechaFind && (int)i.GetType().GetProperty("Tipoinfocodi").GetValue(i, null) == model.ListaHojaPto[k].Tipoinfocodi);

                        for (int j = 1; j <= model.Formato.RowPorDia; j++) // nBlock se comporta como horizonte cuando resolucion es mas de un dia
                        {
                            if (k == 0)
                            {
                                int jIni = 0;
                                if (model.Formato.Formatresolucion >= ParametrosFormato.ResolucionDia)
                                    jIni = j - 1;
                                else
                                    jIni = j;

                                model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][model.ColumnasCabecera - 1] =
                                   // ((model.FechaInicio.AddMinutes(z * ParametrosFormato.ResolucionDia + jIni * (int)model.Formato.Formatresolucion))).ToString(Constantes.FormatoFechaHora);
                                   ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, model.Formato.Lecttipo, z, jIni, model.Formato.FechaInicio);
                            }
                            if (reg != null)
                            {
                                decimal? valor = (decimal?)reg.GetType().GetProperty("H" + j).GetValue(reg, null);
                                var _estado = reg.GetType().GetProperty("E" + j).GetValue(reg, null);
                                if (valor != null)
                                {
                                    model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = valor.ToString();
                                    model.Handson.ListaExcelDataEjecutados[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = _estado != null ? _estado.ToString() : "";

                                }

                                var limit = listaLimites.Where(x => x.Ptomedicodi == Convert.ToInt32(reg.GetType().GetProperty("Ptomedicodi").GetValue(reg, null))).SingleOrDefault();
                                if (limit != null)
                                {
                                    var _LimInf = limit.GetType().GetProperty("Pmin").GetValue(limit, null);
                                    var _LimMax = limit.GetType().GetProperty("Pmax").GetValue(limit, null);
                                    model.Handson.ListaLimitesMinYupana[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = _LimInf == null ? "0" : _LimInf.ToString();
                                    model.Handson.ListaLimitesMaxYupana[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = _LimMax == null ? "0" : _LimMax.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


            //}
        }

        /// <summary>
        /// Obtiene row changes.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="indiceBloque"></param>
        /// <param name="rowPorDia"></param>
        /// <param name="fechaCambio"></param>
        /// <param name="fechaInicio"></param>
        public int ObtieneRowChange(int periodo, int resolucion, int indiceBloque, int rowPorDia, DateTime fechaCambio, DateTime fechaInicio)
        {
            int row = 0;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            row = ((fechaCambio.Year - fechaInicio.Year) * 12) + fechaCambio.Month - fechaInicio.Month;
                            break;
                        default:
                            row = indiceBloque + rowPorDia * (fechaCambio - fechaInicio).Days - 1;
                            break;
                    }
                    break;
                default:
                    row = indiceBloque + rowPorDia * (fechaCambio - fechaInicio).Days - 1;
                    break;
            }

            return row;
        }

        public List<FwAreaDTO> ListAreaXFormato(int idOrigen)
        {
            try
            {
                return FactorySic.GetFwAreaRepository().ListAreaXFormato(idOrigen);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_FORMATO
        /// </summary>
        public List<MeFormatoDTO> ListMeFormatos()
        {
            return FactorySic.GetMeFormatoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeFormato
        /// </summary>
        public List<MeFormatoDTO> GetByModuloLecturaMeFormatos(int idModulo, int idLectura, int idEmpresa)
        {
            return FactorySic.GetMeFormatoRepository().GetByModuloLectura(idModulo, idLectura, idEmpresa);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_LECTURA
        /// </summary>
        public List<MeLecturaDTO> ListMeLecturas()
        {
            return FactorySic.GetMeLecturaRepository().List();
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarValoresCargados48(List<MeMedicion48DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                //calcular Meditotal MeMedicion48DTO
                foreach (var val in entitys)
                {
                    decimal? sumTotal = 0;
                    for (int j = 1; j <= 48; j++)
                    {
                        decimal? valor = (decimal?)val.GetType().GetProperty("H" + j.ToString()).GetValue(val, null);
                        if (valor != null)
                            sumTotal += valor;
                        else
                            sumTotal += 0;
                    }
                    val.Meditotal = sumTotal;
                }

                //Traer Ultimos Valores
                var lista = Convert48DTO(GetDataFormato(idEmpresa, formato, 0, 0,"0"));

                //asignar codigo de formato temporalmente (solo debe aplicar para formatos usados en MCP Yupana)
                List<int> listaFormatcodiMcpYupana = new List<int>() { ConstantesDemandaCP.FormatoDiarioCodi, ConstantesDemandaCP. FormatoSemanalCodi
                                                            , ConstantesHidrologiaCD.FormatoDiarioCodi , ConstantesHidrologiaCD.FormatoReprogramaCodi, ConstantesHidrologiaCD.FormatoSemanalCodi
                                                            ,ConstantesHidrologiaCD.FormatoVolumenDIarioCodi, ConstantesHidrologiaCD.FormatoVolumenReprogramaCodi , ConstantesHidrologiaCD.FormatoVolumenSemanalCodi
                                                            , 112, 113, 114};

                var formatoValidate = formato.Formatdependeconfigptos != null && listaFormatcodiMcpYupana.Contains(formato.Formatcodi) ? (int)formato.Formatdependeconfigptos : formato.Formatcodi;
                var formatcodiInicio = formato.Formatcodi;
                formato.Formatcodi = formatoValidate;

                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();
                    foreach (var reg in entitys)
                    {
                        MeMedicion48DTO regAnt = null;
                        if (!formato.FlagUtilizaHoja)
                        {
                            regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == reg.Lectcodi && x.Tipoinfocodi == reg.Tipoinfocodi);
                        }
                        else
                        {
                            regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi
                                && x.Lectcodi == reg.Lectcodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Hojacodi == reg.Hojacodi);
                        }
                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        if (regAnt != null)
                        {
                            for (int i = 1; i <= 48; i++)
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
                                if (valorOrigen != valorModificado)// && valorOrigen != null && valorModificado != null)
                                {
                                    filaCambios.Add(i.ToString());
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
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Hojacodi = reg.Hojacodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                            {
                                List<MeEnvioDTO> listAux = new List<MeEnvioDTO>();
                                if (ConstantesIEOD.IdFormatoDemandaDiaria == formato.Formatcodi)
                                {
                                    if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == reg.Lectcodi)
                                    {
                                        DateTime fecha = formato.FechaProceso.Date.AddDays(1);
                                        listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, fecha);
                                    }
                                    else
                                    {
                                        DateTime fecha = formato.FechaProceso.Date.AddDays(-1);
                                        listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, fecha);
                                    }
                                }
                                else
                                {
                                    listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaProceso);
                                    listAux = GetByCriteriaMeEnviosFormatoEnergPrimaria(listAux, idEmpresa, formato.Formatcodi, formato.IdFormatoNuevo, formato.FechaProceso);
                                }

                                if (listAux.Count > 0)
                                {
                                    int idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                    MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                    origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                    origen.Cambenvcolvar = "";
                                    origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                    origen.Enviocodi = idEnvioPrevio;
                                    origen.Formatcodi = formato.Formatcodi;
                                    origen.Ptomedicodi = reg.Ptomedicodi;
                                    origen.Tipoinfocodi = reg.Tipoinfocodi;
                                    cambio.Hojacodi = reg.Hojacodi;
                                    origen.Lastuser = usuario;
                                    origen.Lastdate = DateTime.Now;
                                    listaOrigen.Add(origen);
                                }
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                //Eliminar Valores Previos
                if (!formato.FlagUtilizaHoja)
                {
                    EliminarValoresCargados48((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                }
                else
                {
                    if (ConstantesIEOD.IdFormatoDemandaDiaria == formato.Formatcodi)
                    {
                        foreach (var hoja in formato.ListaHoja)
                        {
                            if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == hoja.Lectcodi)
                            {
                                DateTime fecha = formato.FechaProceso.Date.AddDays(1);
                                EliminarValoresCargados48(hoja.Lectcodi.Value, formato.Formatcodi, idEmpresa, fecha, fecha);
                            }
                            else
                            {
                                DateTime fecha = formato.FechaProceso.Date.AddDays(-1);
                                EliminarValoresCargados48(hoja.Lectcodi.Value, formato.Formatcodi, idEmpresa, fecha, fecha);
                            }
                        }
                    }
                    else
                    {
                        foreach (var hoja in formato.ListaHoja)
                        {
                            EliminarValoresCargados48(hoja.Lectcodi.Value, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                        }
                    }
                }

                foreach (MeMedicion48DTO entity in entitys)
                {
                    if (entity.Hojacodi == ConstantesIEOD.EjecHojaCodiMVAR)
                    {
                        entity.Tipoinfocodi = ConstantesIEOD.EjecTipoInfoMVAR;
                    }
                    if (entity.Hojacodi == ConstantesIEOD.ProgHojaCodiMVAR)
                    {
                        entity.Tipoinfocodi = ConstantesIEOD.ProgTipoInfoMVAR;
                    }
                    FactorySic.GetMeMedicion48Repository().SaveMeMedicion48Id(entity);
                }

                formato.Formatcodi = formatcodiInicio;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Convierte Lista Object a medicion48
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<MeMedicion48DTO> Convert48DTO(List<Object> lista)
        {
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();

            foreach (var entity in lista)
            {
                MeMedicion48DTO reg = new MeMedicion48DTO();
                reg.Lectcodi = (int)entity.GetType().GetProperty("Lectcodi").GetValue(entity, null);
                reg.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                reg.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                reg.Tipoinfocodi = (int)entity.GetType().GetProperty("Tipoinfocodi").GetValue(entity, null);
                reg.Hojacodi = (int)entity.GetType().GetProperty("Hojacodi").GetValue(entity, null);
                for (int i = 1; i <= 48; i++)
                {
                    decimal? valor = (decimal?)entity.GetType().GetProperty(ConstantesRDO.CaracterH + (i).ToString()).GetValue(entity, null);
                    reg.GetType().GetProperty(ConstantesRDO.CaracterH + (i).ToString()).SetValue(reg, valor);
                }
                listaFinal.Add(reg);
            }
            return listaFinal;
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
                reg.Tipoptomedicodi = (int)entity.GetType().GetProperty("Tipoptomedicodi").GetValue(entity, null);
                for (int i = 1; i <= 96; i++)
                {
                    decimal? valor = (decimal?)entity.GetType().GetProperty(ConstantesRDO.CaracterH + (i).ToString()).GetValue(entity, null);
                    reg.GetType().GetProperty(ConstantesRDO.CaracterH + (i).ToString()).SetValue(reg, valor);
                }
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_CAMBIOENVIO
        /// <param name="idPto"></param>
        /// <param name="idTipoInfo"></param>
        /// <param name="idFormato"></param>
        /// <param name="fecha"></param>
        /// </summary>
        public List<MeCambioenvioDTO> ListMeCambioenvios(int idPto, int idTipoInfo, int idFormato, DateTime fecha)
        {
            return FactorySic.GetMeCambioenvioRepository().List(idPto, idTipoInfo, idFormato, fecha);
        }

        /// <summary>
        /// Listar envios con formatos de energia primaria
        /// </summary>
        /// <param name="envios"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idFormatoNuevo"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GetByCriteriaMeEnviosFormatoEnergPrimaria(List<MeEnvioDTO> envios, int idEmpresa, int idFormato, int idFormatoNuevo, DateTime fecha)
        {
            if (idFormatoNuevo > 0 && idFormato != idFormatoNuevo)
            {
                var listaEnviosNuevo = this.GetByCriteriaMeEnvios(idEmpresa, idFormatoNuevo, fecha);
                listaEnviosNuevo.AddRange(envios);
                return listaEnviosNuevo;
            }

            return envios;
        }

        /// <summary>
        /// Graba una lista de cambios 
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarCambios(List<MeCambioenvioDTO> entitys)
        {
            foreach (var entity in entitys)
                SaveMeCambioenvio(entity);
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_CAMBIOENVIO
        /// <param name="entity"></param>
        /// </summary>
        public void SaveMeCambioenvio(MeCambioenvioDTO entity)
        {
            try
            {
                FactorySic.GetMeCambioenvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados48(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion48Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarValoresCargados24(List<MeMedicion24DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                //Traer Ultimos Valores
                var lista = GetDataFormato24(idEmpresa, formato, 0, 0);
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();
                    foreach (var reg in entitys)
                    {
                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi);
                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        if (regAnt != null)
                        {
                            for (int i = 1; i <= 24; i++)
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
                                if (valorOrigen != valorModificado)// && valorOrigen != null && valorModificado != null)
                                {
                                    filaCambios.Add(i.ToString());
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
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                            {
                                int idEnvioPrevio = 0;
                                var listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaProceso);
                                if (listAux.Count > 0)
                                    idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                origen.Cambenvcolvar = "";
                                origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                origen.Enviocodi = idEnvioPrevio;
                                origen.Formatcodi = formato.Formatcodi;
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
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                //Eliminar Valores Previos
                EliminarValoresCargados24((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin.AddDays(1));
                foreach (MeMedicion24DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion24Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idUltimoEnvio"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GetDataFormato24(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {

            List<MeMedicion24DTO> lista24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
            if (idEnvio != 0)
            {
                var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                if (lista.Count > 0)
                {
                    foreach (var reg in lista)
                    {
                        var find = lista24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                            x.Medifecha == reg.Cambenvfecha);
                        if (find != null)
                        {
                            var fila = reg.Cambenvdatos.Split(',');
                            for (var i = 0; i < 24; i++)
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
            else
            {
                if (formato.Formatsecundario != 0 && lista24.Count == 0)
                {
                    lista24 = FactorySic.GetMeMedicion24Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                }
            }
            return lista24;
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados24(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion24Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos ejecutados
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarValoresEjecutados24(List<MeMedicion24DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                foreach (MeMedicion24DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion24Repository().SaveEjecutados(entity, idEnvio, usuario, idEmpresa);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Convierte una lista de mediciones en una Matriz Excel Web
        /// </summary>
        /// <param name="data"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filHead"></param>
        /// <param name="nFil"></param>
        /// <returns></returns>
        private static string[][] GetMatrizExcel(List<string> data, int colHead, int nCol, int filHead, int nFil)
        {
            var colTot = nCol + colHead + 1;
            var inicio = (colTot) * filHead;
            int col = 0;
            int fila = 0;
            string[][] arreglo = new string[nFil][];
            arreglo[0] = new string[colTot];
            for (int i = inicio; i < data.Count(); i++)
            {
                string valor = data[i];
                if (col == colTot)
                {
                    fila++;
                    col = 0;
                    arreglo[fila] = new string[colTot];
                }
                arreglo[fila][col] = valor;
                col++;
            }
            return arreglo;
        }

        /// <summary>
        /// Carga la data recibida del excel web a una lista de Medidion96DTO
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ptos"></param>
        /// <param name="idLectura"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filaHead"></param>
        /// <param name="nFil"></param>
        /// <param name="checkBlanco"></param>
        /// <returns></returns>
        public static List<MeMedicion96DTO> LeerExcelWeb96(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int colHead, int nCol, int filaHead, int nFil, int checkBlanco)
        {
            var matriz = GetMatrizExcel(data, colHead, nCol, filaHead, nFil);
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            MeMedicion96DTO reg = new MeMedicion96DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            for (var i = 1; i < nCol; i++)
            {
                for (var j = 0; j < nFil; j++)
                {
                    //verificar inicio de dia
                    if ((j % 96) == 0)
                    {
                        if (j != 0)
                            lista.Add(reg);
                        reg = new MeMedicion96DTO();
                        reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                        reg.Lectcodi = idLectura;
                        reg.Meditotal = 0;
                        reg.Tipoinfocodi = (int)ptos[i - 1].Tipoinfocodi;
                        reg.Emprcodi = ptos[i - 1].Emprcodi;
                        fecha = DateTime.ParseExact(matriz[j][0], ConstantesRDO.FormatoFechaHora, CultureInfo.InvariantCulture);
                        reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                        stValor = matriz[j][i];
                        if (Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            reg.H1 = valor;
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                reg.H1 = null;
                            else
                                reg.H1 = 0;
                        }
                    }
                    else
                    {
                        int indice = j % 96 + 1;
                        stValor = matriz[j][i];
                        if (Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                            else
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, 0);
                        }
                    }
                }
                lista.Add(reg);
            }

            #region Validación de Cantidad mínima de filas por cada Punto de medición

            foreach (var regPto in ptos)
            {
                MeHojaptomedDTO objPto = ptos.Find(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi);
                int numFilasMinimo = objPto.ConfigPto != null && objPto.ConfigPto.Plzptominfila > 0 ? objPto.ConfigPto.Plzptominfila : 0;
                if (numFilasMinimo > 0)
                {
                    int cantidadFilas = 0;
                    decimal? valorH;
                    var listaDataXPto = lista.Where(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi).ToList();
                    foreach (var regData in listaDataXPto)
                    {
                        for (int h = 1; h <= 96; h++)
                        {
                            valorH = (decimal?)regData.GetType().GetProperty("H" + h).GetValue(regData, null);
                            cantidadFilas += (valorH != null ? 1 : 0);
                        }
                    }

                    if (cantidadFilas < numFilasMinimo)
                    {
                        foreach (var regData in listaDataXPto)
                        {
                            for (int h = 1; h <= 96; h++)
                            {
                                regData.GetType().GetProperty("H" + h).SetValue(regData, null);
                            }
                        }
                    }
                }
            }

            #endregion

            return lista;
        }

        /// <summary>
        /// Borra registro de la tabla medicion 96
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="idTipoInfo"></param>
        /// <param name="fecha"></param>
        /// <param name="idLectura"></param>
        public void DeleteMeMedicion96(int idPtomedicion, int idTipoInfo, DateTime fecha, int idLectura)
        {
            try
            {
                FactorySic.GetMeMedicion96Repository().Delete(idPtomedicion, idTipoInfo, fecha, idLectura);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="idTptomedicodi"></param>
        /// <param name="idLectura"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados96(int idTptomedicodi, int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion96Repository().DeleteEnvioArchivo2(idTptomedicodi, idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarValoresCargados96(List<MeMedicion96DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                //determinar la lista de tptomedicodi
                var listaTptomedicodi = entitys.GroupBy(x => x.Tipoptomedicodi).Select(x => x.Key).ToList();

                int count = 0;
                //Traer Ultimos Valores
                var lista = Convert96DTO(GetDataFormato(idEmpresa, formato, 0, 0,"0"));
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();


                    foreach (var reg in entitys)
                    {
                        reg.Tipoptomedicodi = reg.Tipoptomedicodi != 0 ? reg.Tipoptomedicodi : -1;

                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Tipoptomedicodi == reg.Tipoptomedicodi);


                        if (regAnt != null)
                        {
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
                                cambio.Tipoptomedicodi = reg.Tipoptomedicodi;
                                cambio.Lastuser = usuario;
                                cambio.Lastdate = DateTime.Now;
                                listaCambio.Add(cambio);
                                /// Si no ha habido cambio se graba el registro original
                                if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                                {
                                    int idEnvioPrevio = 0;
                                    var listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaInicio);
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
                                    origen.Tipoptomedicodi = reg.Tipoptomedicodi;
                                    origen.Lastuser = usuario;
                                    origen.Lastdate = DateTime.Now;
                                    listaOrigen.Add(origen);
                                }
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }

                //Eliminar Valores Previos 
                //Condicional Formato de pruebas unidad
                if (formato.Formatcodi == ConstantesFormatoMedicion.IdFormatoPruebasAleatorias)
                {  //Formato de pruebas unidad

                    foreach (MeMedicion96DTO ent in entitys)//foreach (var tptomedicodi in listaTptomedicodi)
                    {
                        DeleteMeMedicion96(ent.Ptomedicodi, ConstantesFormatoMedicion.IdTipoinfocodiMw, (DateTime)ent.Medifecha, ConstantesFormatoMedicion.IdLectcodiPruebasAleatorias);
                    }
                    //Fin Formato de pruebas unidad
                }
                else
                {
                    foreach (var tptomedicodi in listaTptomedicodi)
                    {
                        var tptomedi = tptomedicodi != 0 ? tptomedicodi : -1;
                        EliminarValoresCargados96(tptomedi, (int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    }
                }


                if (formato.Formatcodi == ConstantesInterconexiones.IdFormatoInterconexion)
                {
                    //eliminar valores antiguos
                    List<DateTime> listaFechaAntiguo = entitys.Select(x => x.Medifecha.Value).Distinct().OrderBy(x => x).ToList();
                    foreach (var f in listaFechaAntiguo)
                    {
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdExportacionL2280MWh, ConstantesFormatoMedicion.IdMWh, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdImportacionL2280MWh, ConstantesFormatoMedicion.IdMWh, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdExportacionL2280MVARr, ConstantesFormatoMedicion.IdMVARh, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdImportacionL2280MVARr, ConstantesFormatoMedicion.IdMVARh, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdPtoMedicionL2280, ConstantesInterconexiones.IdTipoInfocodiKV, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                        DeleteMeMedicion96(ConstantesFormatoMedicion.IdPtoMedicionL2280, ConstantesInterconexiones.IdTipoInfocodiA, f, ConstantesFormatoMedicion.IdlecturaInterconexion);
                    }
                }

                foreach (MeMedicion96DTO entity in entitys)
                {
                    entity.Tipoptomedicodi = entity.Tipoptomedicodi != 0 ? entity.Tipoptomedicodi : -1;
                    FactorySic.GetMeMedicion96Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Convierte Lista Object a medicion 1
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<MeMedicion1DTO> Convert1DTO(List<Object> lista)
        {
            List<MeMedicion1DTO> listaFinal = new List<MeMedicion1DTO>();
            foreach (var entity in lista)
            {
                MeMedicion1DTO reg = new MeMedicion1DTO();
                reg.Lectcodi = (int)entity.GetType().GetProperty("Lectcodi").GetValue(entity, null);
                reg.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                reg.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                reg.Tipoinfocodi = (int)entity.GetType().GetProperty("Tipoinfocodi").GetValue(entity, null);
                reg.Tipoptomedicodi = (int)entity.GetType().GetProperty("Tipoptomedicodi").GetValue(entity, null);
                reg.H1 = (decimal?)entity.GetType().GetProperty("H1").GetValue(entity, null);
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="idTptomedicodi"></param>
        /// <param name="idLectura"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados1(int idTptomedicodi, int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion1Repository().DeleteEnvioArchivo2(idTptomedicodi, idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Graba Valores Cargados en  Hoja Web Excel y verifica si hay repeditos
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        /// <param name="lectcodi"></param>
        public void GrabarValoresCargados1(List<MeMedicion1DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato, int lectcodi)
        {
            try
            {
                //determinar la lista de tptomedicodi
                var listaTptomedicodi = entitys.GroupBy(x => x.Tipoptomedicodi).Select(x => x.Key).ToList();

                //asignar codigo de formato temporalmente
                var formatoValidate = formato.Formatdependeconfigptos != null ? (int)formato.Formatdependeconfigptos : formato.Formatcodi;
                var formatcodiInicio = formato.Formatcodi;
                formato.Formatcodi = formatoValidate;

                //Traer Ultimos Valores
                var lista = Convert1DTO(GetDataFormato(idEmpresa, formato, 0, 0, "0"));
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();
                    foreach (var reg in entitys)
                    {
                        reg.Tipoptomedicodi = reg.Tipoptomedicodi != 0 ? reg.Tipoptomedicodi : -1;

                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Tipoptomedicodi == reg.Tipoptomedicodi);
                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        if (regAnt != null)
                        {
                            decimal? valorOrigen = regAnt.H1; // (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                            decimal? valorModificado = reg.H1; //(decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
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
                                filaCambios.Add("1");
                            }
                        }
                        if (filaCambios.Count > 0)
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            cambio.Cambenvdatos = String.Join(",", filaValores);
                            cambio.Cambenvcolvar = String.Join(",", filaCambios);
                            cambio.Cambenvfecha = (DateTime)reg.Medifecha;
                            cambio.Enviocodi = idEnvio;
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Tipoptomedicodi = reg.Tipoptomedicodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Where(x => x.Tipoptomedicodi == reg.Tipoptomedicodi).ToList().Count == 0)
                            {
                                int idEnvioPrevio = 0;
                                var listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaInicio);
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
                                origen.Tipoptomedicodi = reg.Tipoptomedicodi;
                                origen.Lastuser = usuario;
                                origen.Lastdate = DateTime.Now;
                                if (idEnvioPrevio > 0)
                                    listaOrigen.Add(origen);
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                
                    //Eliminar Valores Previos
                    foreach (var tptomedicodi in listaTptomedicodi)
                    {
                        var tptomedi = tptomedicodi != 0 ? tptomedicodi : -1;
                        EliminarValoresCargados1(tptomedi, lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    }

                foreach (MeMedicion1DTO entity in entitys)
                {
                    entity.Tipoptomedicodi = entity.Tipoptomedicodi != 0 ? entity.Tipoptomedicodi : -1;
                    FactorySic.GetMeMedicion1Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIO_HORARIO
        /// </summary>
        public void SaveHorarioMeEnvio(int idEnvio, int horario)
        {
            try
            {
                FactorySic.GetMeEnvioRepository().SaveHorario(idEnvio, horario);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesRDO.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_LECTURA
        /// </summary>
        public MeLecturaDTO GetByIdMeLectura(int lectcodi)
        {
            return FactorySic.GetMeLecturaRepository().GetById(lectcodi);
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idUltimoEnvio"></param>
        /// <returns></returns>
        public List<Object> GetDataMeMedicionIntranet(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {
            List<Object> listaGenerica = new List<Object>();

            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionHora:
                    List<MeMedicion24DTO> listaExtranet24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivoEjecutados(formato.Formatcodi, idEmpresa, formato.FechaInicio.AddDays(-1), formato.FechaInicio, "0");
                    List<MeMedicion24DTO> listaIntranet24 = FactorySic.GetMeMedicion24Repository().GetEnvioMeMedicion24Intranet(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaInicio);

                    if (listaIntranet24.Count > 0)
                    {
                        foreach (var reg in listaIntranet24)
                        {
                            var findExt = listaExtranet24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == reg.Lectcodi && x.Medifecha == reg.Medifecha && x.Emprcodi == reg.Emprcodi);
                            if (findExt == null)
                                findExt = listaExtranet24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == reg.Lectcodi && x.Medifecha == reg.Medifecha.AddDays(-1) && x.Emprcodi == reg.Emprcodi);

                            var findInt = listaIntranet24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == reg.Lectcodi && x.Medifecha == reg.Medifecha && x.Emprcodi == reg.Emprcodi);
                            if (findExt != null)
                            {
                                for (var x = 0; x < 24; x++)
                                {
                                    var valInt = findInt.GetType().GetProperty("H" + (x + 1)).GetValue(findInt, null);
                                    var valExt = findExt.GetType().GetProperty("H" + (x + 1)).GetValue(findExt, null);

                                    if (valInt != null && Convert.ToDecimal(valExt) != Convert.ToDecimal(valInt) && valInt.ToString() != "")
                                    {
                                        findExt.GetType().GetProperty("H" + (x + 1).ToString()).SetValue(findExt, Convert.ToDecimal(valInt));
                                    }
                                }
                            }
                        }
                    }
                    foreach (var reg in listaExtranet24)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    //List<MeMedicion48DTO> listaExtranet48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivoEjecutados(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio.AddDays(-1), formato.FechaInicio, formato.Lectcodi, "0");
                    List<MeMedicion48DTO> listaExtranet48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivoEjecutadosIntranet(formato.Formatcodi, formato.FechaInicio.AddDays(-1), formato.FechaInicio, formato.Lectcodi);
                    List<MeMedicion48DTO> listaUltimosEnviosExtranet = new List<MeMedicion48DTO>();
                    if (listaExtranet48.Count > 0)
                    {

                        var codigosMedicion48 = listaExtranet48.Select(y => new { y.Emprcodi, y.Lectcodi, y.Tipoinfocodi, y.Ptomedicodi }).Distinct().ToList();

                        foreach (var item in codigosMedicion48)
                        {
                            MeMedicion48DTO med48 = new MeMedicion48DTO();
                            MeMedicion48DTO ultimoEnvio = (listaExtranet48.Where(x => x.Emprcodi == item.Emprcodi && x.Lectcodi == item.Lectcodi && x.Tipoinfocodi == item.Tipoinfocodi && x.Ptomedicodi == item.Ptomedicodi).OrderByDescending(c => c.Enviocodi).FirstOrDefault());
                            listaUltimosEnviosExtranet.Add(ultimoEnvio);
                        }
                    }


                    List<MeMedicion48DTO> listaIntranet48 = FactorySic.GetMeMedicion48Repository().GetEnvioMeMedicion48Intranet(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio.AddDays(-1), formato.FechaInicio, formato.Lectcodi);

                    if (listaIntranet48.Count > 0)
                    {
                        foreach (var reg in listaIntranet48)
                        {
                            var findExt = listaUltimosEnviosExtranet.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == reg.Lectcodi && x.Medifecha == reg.Medifecha && x.Emprcodi == reg.Emprcodi);
                            var findInt = listaIntranet48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == reg.Lectcodi && x.Medifecha == reg.Medifecha && x.Emprcodi == reg.Emprcodi);
                            if (findExt != null)
                            {
                                for (var x = 0; x < 48; x++)
                                {
                                    var valInt = findInt.GetType().GetProperty("H" + (x + 1)).GetValue(findInt, null);
                                    var valExt = findExt.GetType().GetProperty("H" + (x + 1)).GetValue(findExt, null);

                                    if (valInt != null && Convert.ToDecimal(valExt) != Convert.ToDecimal(valInt) && valInt.ToString() != "")
                                    {
                                        findExt.GetType().GetProperty("H" + (x + 1).ToString()).SetValue(findExt, Convert.ToDecimal(valInt));
                                    }
                                }
                            }
                        }
                    }

                    foreach (var reg in listaUltimosEnviosExtranet)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
            }

            return listaGenerica;
        }

        /// <summary>
        /// Listar todos los envíos de eventos
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> ListaEnviosPorEvento(MeEnvioDTO entity)
        {

            return FactorySic.GetMeEnvioRepository().ListaEnviosPorEvento(entity);
        }


        /// <summary>
        ///Generar el formato en html
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GenerarListaFallasHtml(List<MeEnvioDTO> lista)
        {
            StringBuilder strHtml = new StringBuilder();
            if (lista.Count > 0)
            {
              

                List<MeEnvioDTO> listaUltimosEnvios = new List<MeEnvioDTO>();
                var codigosEventos = lista.Select(y => new { y.Evencodi, y.Emprcodi }).Distinct().ToList();

                foreach (var item in codigosEventos)
                {
                    MeEnvioDTO envio = new MeEnvioDTO();
                    MeEnvioDTO ultimoEnvio = (lista.Where(x => x.Evencodi == item.Evencodi && x.Emprcodi == item.Emprcodi).OrderBy(c => c.Enviocodi).FirstOrDefault());
                    
                    
                    listaUltimosEnvios.Add(ultimoEnvio);
                }

                strHtml.Append("<table class='pretty tabla-icono'>");
                strHtml.Append("<thead>");
                var listaHeader = listaUltimosEnvios.Select(y => new { y.Evenini, y.Evenasunto }).Distinct().ToList().OrderBy(c => c.Evenini);
                strHtml.Append("<tr>");
                strHtml.Append("<th style='width:200px; text-align: left;'>EMPRESA</th>");

                foreach (var item in listaHeader)
                {
                    strHtml.Append("<th style='width:50px;'>" + item.Evenini + "</td>");
                }
                strHtml.Append("</tr>");
                strHtml.Append("<tr>");
                strHtml.Append("<th style='width:200px; text-align: left;'>DESCRIPCIÓN</th>");

                foreach (var item in listaHeader)
                {
                    strHtml.Append("<th style='width:50px;text-align: left;'>" + item.Evenasunto + "</td>");
                }
                strHtml.Append("</tr>");
                strHtml.Append("</thead>");

                var listaEmpresa = listaUltimosEnvios.Select(y => new { y.Emprcodi, y.Emprnomb }).Distinct().ToList().OrderBy(c => c.Emprnomb);

                strHtml.Append("<tbody>");

                foreach (var item in listaEmpresa)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append("<td style='text-align: left;'>" + item.Emprnomb + "</td>");
     
                    var enviosHeader = listaUltimosEnvios.Select(y => new { y.Evenini }).Distinct().ToList().OrderBy(c => c.Evenini);
                    string htmlDato = "";
                    foreach (var itemEnvioHeader in enviosHeader)
                    {
                        var enviosEmpresa = (listaUltimosEnvios.Where(x => x.Emprcodi == item.Emprcodi).OrderBy(c => c.Evenini)).ToList();
                        htmlDato = "<td></td>";
                        foreach (var itemEnviosEmpresa in enviosEmpresa)
                        {
                            if (itemEnvioHeader.Evenini == itemEnviosEmpresa.Evenini )
                            {
                                htmlDato = "<td>" + itemEnviosEmpresa.Envioplazo + "</td>";
                            }
                        }
                        strHtml.Append(htmlDato);
                    }


                    
                    //strHtml.Append("<td>" + itemEnviosEmpresa.Envioplazo + "</td>");
                    strHtml.Append("</tr>");
                }
                strHtml.Append("</tbody>");

                strHtml.Append("</table>");
            }
           
            return strHtml.ToString();
        }
    }
}
