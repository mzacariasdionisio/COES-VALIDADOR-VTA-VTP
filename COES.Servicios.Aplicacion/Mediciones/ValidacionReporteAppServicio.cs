using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace COES.Servicios.Aplicacion.Mediciones
{
    public class ValidacionReporteAppServicio
    {
        ReporteMedidoresAppServicio servRepMedi = new ReporteMedidoresAppServicio();
        EjecutadoAppServicio servEjec = new EjecutadoAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConsultaMedidoresAppServicio));

        #region Métodos Tabla WB_MEDIDORES_VALIDACION

        /// <summary>
        /// Inserta un registro de la tabla WB_MEDIDORES_VALIDACION
        /// </summary>
        public void SaveWbMedidoresValidacion(WbMedidoresValidacionDTO entity)
        {
            try
            {
                FactorySic.GetWbMedidoresValidacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_MEDIDORES_VALIDACION
        /// </summary>
        public void UpdateWbMedidoresValidacion(WbMedidoresValidacionDTO entity)
        {
            try
            {
                FactorySic.GetWbMedidoresValidacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_MEDIDORES_VALIDACION
        /// </summary>
        public void DeleteWbMedidoresValidacion(int medivalcodi)
        {
            try
            {
                FactorySic.GetWbMedidoresValidacionRepository().Delete(medivalcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_MEDIDORES_VALIDACION
        /// </summary>
        public WbMedidoresValidacionDTO GetByIdWbMedidoresValidacion(int medivalcodi)
        {
            return FactorySic.GetWbMedidoresValidacionRepository().GetById(medivalcodi);
        }

        /// <summary>
        /// Lista las fuentes de energía
        /// </summary>
        /// <returns></returns>
        public List<SiFuenteenergiaDTO> ListaFuenteEnergia(int idTipoGeneracion)
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetByCriteria().Where(x => x.Fenergcodi != -1
                && x.Fenergcodi != 0 && (x.Tgenercodi == idTipoGeneracion || idTipoGeneracion == 0)).ToList();
        }

        /// <summary>
        /// Permite obtener las empresa por tipo
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObteneEmpresasPorTipo(string tiposEmpresa)
        {
            if (string.IsNullOrEmpty(tiposEmpresa)) tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
            return (new IEODAppServicio()).ListarEmpresasTienenCentralGenxTipoEmpresa(tiposEmpresa);
        }

        /// <summary>
        /// Permite listar los tipos de empresas
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ListaTipoEmpresas()
        {
            return FactorySic.GetSiTipoempresaRepository().List();
        }

        public List<WbMedidoresValidacionDTO> ObtenerEmpresasGrafico()
        {
            return FactorySic.GetWbMedidoresValidacionRepository().ObtenerEmpresasGrafico();
        }

        public List<WbMedidoresValidacionDTO> ObtenerGruposGrafico(int idEmpresa)
        {
            return FactorySic.GetWbMedidoresValidacionRepository().ObtenerGruposGrafico(idEmpresa);
        }

        /// <summary>
        /// Lista los tipos de generación
        /// </summary>
        /// <returns></returns>
        public List<SiTipogeneracionDTO> ListaTipoGeneracion()
        {
            return FactorySic.GetSiTipogeneracionRepository().GetByCriteria().Where(x => x.Tgenercodi != -1 && x.Tgenercodi != 0 && x.Tgenercodi != 5).ToList();
        }

        /// <summary>
        /// Permite generar el reporte de validación
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="fuentesEnergia"></param>
        /// <param name="central"></param>
        /// <param name="objDespachoMD"></param>
        /// <param name="objMedidorMD"></param>
        /// <returns></returns>
        public List<ReporteValidacionMedidor> ObtenerReporteValidacion(DateTime fechaInicial, DateTime fechaFinal, string tiposEmpresa, string empresas,
            string tiposGeneracion, string fuentesEnergia, int central, out ItemReporteMedicionDTO objDespachoMD, out ItemReporteMedicionDTO objMedidorMD, out string msjValidacion)
        {
            //Listar configuración de Equivalencias de Puntos de medición de Medidores vs Despacho
            List<WbMedidoresValidacionDTO> listConfiguracion = FactorySic.GetWbMedidoresValidacionRepository().GetByCriteria();

            //Obtener Data de Medidores de Generación 96
            List<MeMedicion96DTO> listMedidores = this.servRepMedi.ListaDataMDGeneracionConsolidado(fechaInicial, fechaFinal, central, tiposGeneracion, empresas, ConstanteValidacion.EstadoTodos, fuentesEnergia, true);
            List<MeMedicion96DTO> listaDemandaGen96 = this.servRepMedi.ListaDataMDGeneracionFromConsolidado(fechaInicial, fechaFinal, listMedidores);

            //Obtener Data de Despacho 48 ('Scada')
            List<MeMedicion48DTO> listDespacho = this.servEjec.ListaDataMDGeneracionConsolidado48(fechaInicial, fechaFinal, central, tiposGeneracion, empresas, ConstanteValidacion.EstadoTodos, fuentesEnergia, true, ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto);
            List<MeMedicion48DTO> listaDemandaGen48 = this.servEjec.ListaDataMDGeneracionFromConsolidado48(fechaInicial, fechaFinal, listDespacho);

            //Data Interconexion
            List<MeMedicion96DTO> listaInterconexion96 = this.servRepMedi.ListaDataMDInterconexion96(fechaInicial, fechaFinal);
            List<MeMedicion48DTO> listaInterconexion48 = this.servEjec.ListaDataMDInterconexion48(fechaInicial, fechaFinal);

            //Data Total (incluyendo interconexion)
            List<MeMedicion96DTO> listaMedicionTotal96 = this.servRepMedi.ListaDataMDTotalSEIN(listaDemandaGen96, listaInterconexion96);
            List<MeMedicion48DTO> listaMedicionTotal48 = this.servEjec.ListaDataMDTotalSEIN48(listaDemandaGen48, listaInterconexion48);

            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaRangoNormaHP = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            List<SiParametroValorDTO> listaBloqueHorario = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Maxima Demanda Coincidente de Potencia por tipo de Generación (MW)
            this.servEjec.GetDiaMaximaDemandaFromDataMD48(fechaInicial, fechaFinal, ConstantesRepMaxDemanda.TipoMDNormativa, listaMedicionTotal48, listaRangoNormaHP, listaBloqueHorario, 
                                                        out DateTime fechaMD48, out DateTime fechaDia48, out int hMax48);
            this.servRepMedi.GetDiaMaximaDemandaFromDataMD96(fechaInicial, fechaFinal, ConstantesRepMaxDemanda.TipoMDNormativa, listaMedicionTotal96, listaRangoNormaHP, listaBloqueHorario, 
                                                        out DateTime fechaMD96, out DateTime fechaDia96, out int hMax96);

            objDespachoMD = new ItemReporteMedicionDTO();
            objDespachoMD.FechaMaximaDemanda = fechaMD48;
            objDespachoMD.HoraMaximaDemanda = objDespachoMD.FechaMaximaDemanda.ToString(ConstantesAppServicio.FormatoHora);
            objDespachoMD.Indice = objDespachoMD.FechaMaximaDemanda.Hour * 2 + objDespachoMD.FechaMaximaDemanda.Minute / 30;

            objMedidorMD = new ItemReporteMedicionDTO();
            objMedidorMD.FechaMaximaDemanda = fechaMD96;
            objMedidorMD.HoraMaximaDemanda = objMedidorMD.FechaMaximaDemanda.ToString(ConstantesAppServicio.FormatoHora);
            objMedidorMD.Indice = objMedidorMD.FechaMaximaDemanda.Hour * 4 + objMedidorMD.FechaMaximaDemanda.Minute / 15;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Cálculo de Validación

            #region Obtener los grupos que se mostrarán en el reporte

            List<PrGrupoDTO> listaGrupoByData = new List<PrGrupoDTO>();
            List<PrGrupoDTO> listaAllGrupo = FactorySic.GetPrGrupoRepository().List();

            List<int> listaAllPtomedicodiDesp48 = listDespacho.Select(x => x.Ptomedicodi).Distinct().ToList();
            List<int> listaAllPtomedicodiMed96 = listMedidores.Select(x => x.Ptomedicodi).Distinct().ToList();

            List<WbMedidoresValidacionDTO> listaConfSegunData = listConfiguracion.Where(x => listaAllPtomedicodiDesp48.Contains(x.Ptomedicodidesp ?? 0) || listaAllPtomedicodiMed96.Contains(x.Ptomedicodimed ?? 0)).ToList();
            List<int> grupocodiSegunData = listaConfSegunData.Select(x => x.Grupocodi).Distinct().ToList();
            foreach (var grupocodi in grupocodiSegunData)
            {
                if (grupocodi == 432)
                { }
                PrGrupoDTO regGrupo = listaAllGrupo.Find(x => x.Grupocodi == grupocodi);

                List<int> listaAllPtodespByGrupo = listaConfSegunData.Where(x => x.Grupocodi == grupocodi && x.Ptomedicodidesp > 0).Select(x => x.Ptomedicodidesp.Value).Distinct().ToList();
                var listaTmpDesp = listDespacho.Where(x => listaAllPtodespByGrupo.Contains(x.Ptomedicodi)).GroupBy(x => new { x.Ptomedicodi, x.Emprcodi, x.Emprnomb }).Select(x => x.First());
                foreach (var reg in listaTmpDesp)
                {
                    listaGrupoByData.Add(new PrGrupoDTO()
                    {
                        Grupocodi = grupocodi,
                        Grupoabrev = regGrupo.Grupoabrev,
                        Emprcodi = reg.Emprcodi,
                        Emprnomb = reg.Emprnomb
                    });
                }

                List<int> listaAllPtoMedByGrupo = listaConfSegunData.Where(x => x.Grupocodi == grupocodi && x.Ptomedicodimed > 0).Select(x => x.Ptomedicodimed.Value).Distinct().ToList();
                var listaTmpMed = listMedidores.Where(x => listaAllPtoMedByGrupo.Contains(x.Ptomedicodi)).GroupBy(x => new { x.Ptomedicodi, x.Emprcodi, x.Emprnomb }).Select(x => x.First());
                foreach (var reg in listaTmpMed)
                {
                    listaGrupoByData.Add(new PrGrupoDTO()
                    {
                        Grupocodi = grupocodi,
                        Grupoabrev = regGrupo.Grupoabrev,
                        Emprcodi = reg.Emprcodi,
                        Emprnomb = reg.Emprnomb
                    });
                }
            }

            #endregion

            //Listar todos los grupos
            listaGrupoByData = listaGrupoByData.GroupBy(x => new { x.Grupocodi, x.Emprcodi })
                .Select(x => new PrGrupoDTO() { Grupocodi = x.Key.Grupocodi, Emprcodi = x.Key.Emprcodi, Grupoabrev = x.First().Grupoabrev, Emprnomb = x.First().Emprnomb })
                .OrderBy(x => x.Emprnomb).ThenBy(x => x.Grupoabrev).ToList();

            List<int> listaAllPtomedicodiDespConfigurados = new List<int>();
            List<int> listaAllPtomedicodiMedConfigurados = new List<int>();

            List<ReporteValidacionMedidor> resultado = new List<ReporteValidacionMedidor>();

            foreach (var regGrupo in listaGrupoByData)
            {
                int grupocodi = regGrupo.Grupocodi;
                int emprcodi = regGrupo.Emprcodi.GetValueOrDefault(-100);

                ReporteValidacionMedidor itemResult = new ReporteValidacionMedidor();
                itemResult.DesEmpresa = regGrupo.Emprnomb != null ? regGrupo.Emprnomb.Trim() : string.Empty;
                itemResult.DesGrupo = regGrupo.Grupoabrev != null ? regGrupo.Grupoabrev.Trim() : string.Empty;

                List<int> ptomedicodiDespacho = listConfiguracion.Where(x => x.Grupocodi == grupocodi && x.Emprcodi == emprcodi).Select(x => (int)x.Ptomedicodidesp).ToList();
                List<int> ptomedicodiMedidores = listConfiguracion.Where(x => x.Grupocodi == grupocodi && x.Emprcodi == emprcodi).Select(x => (int)x.Ptomedicodimed).ToList();

                List<MeMedicion48DTO> lista48XGrupo = listDespacho.Where(x => ptomedicodiDespacho.Contains(x.Ptomedicodi) && x.Emprcodi == emprcodi).ToList();
                List<MeMedicion96DTO> lista96XGrupo = listMedidores.Where(x => ptomedicodiMedidores.Contains(x.Ptomedicodi) && x.Emprcodi == emprcodi).ToList();

                itemResult.ValorDespacho = lista48XGrupo.Sum(x => (decimal)x.Meditotal) / 2.0M;

                if (objDespachoMD != null)
                    itemResult.MDDespacho = this.ObtenerMaximaDemandaDespacho(objDespachoMD, lista48XGrupo);

                itemResult.ValorMedidor = lista96XGrupo.Sum(x => (decimal)x.Meditotal) / 4.0M;
                if (objMedidorMD != null)
                    itemResult.MDMedidor = this.ObtenerMaximaDemandaMedidor(objMedidorMD, lista96XGrupo);

                if (itemResult.ValorMedidor != 0)
                {
                    if (itemResult.ValorDespacho != 0)
                    {
                        itemResult.Desviacion = (itemResult.ValorMedidor - itemResult.ValorDespacho) * 100 / (itemResult.ValorMedidor);

                        if (objDespachoMD != null && objMedidorMD != null)
                        {
                            if (itemResult.MDDespacho != 0)
                            {
                                itemResult.MDDesviacion = (itemResult.MDMedidor - itemResult.MDDespacho) * 100 / (itemResult.MDDespacho);
                            }
                        }
                    }
                    else
                    {
                        itemResult.Desviacion = 0;
                        itemResult.MDDesviacion = 0;
                    }
                    itemResult.IndMuestra = ConstantesBase.SI;
                    itemResult.IndColor = ConstantesBase.SI;

                    if (itemResult.Desviacion < 5)
                    {
                        itemResult.IndColor = ConstantesBase.NO;
                        itemResult.Filtro = 1;
                    }
                    if (itemResult.Desviacion == 5) //verde
                    {
                        itemResult.Color = "#00CC00";
                        itemResult.Filtro = 2;
                    }
                    if (itemResult.Desviacion > 5 && itemResult.Desviacion < 20) //amarillo
                    {
                        itemResult.Color = "#FFFF00";
                        itemResult.Filtro = 3;
                    }
                    if (itemResult.Desviacion >= 20) //rojo
                    {
                        itemResult.Color = "#FF3300";
                        itemResult.Filtro = 4;
                    }
                }
                else
                {
                    itemResult.IndMuestra = ConstantesBase.NO;
                    itemResult.IndColor = ConstantesBase.NO;
                }

                resultado.Add(itemResult);
                listaAllPtomedicodiDespConfigurados.AddRange(ptomedicodiDespacho);
                listaAllPtomedicodiMedConfigurados.AddRange(ptomedicodiMedidores);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Puntos de medición no configurados
            List<string> listaMsjValidacion = new List<string>();

            listaAllPtomedicodiDespConfigurados = listaAllPtomedicodiDespConfigurados.Distinct().ToList();
            listaAllPtomedicodiMedConfigurados = listaAllPtomedicodiMedConfigurados.Distinct().ToList();

            List<int> listaPtoFaltante48 = listaAllPtomedicodiDesp48.Where(x => !listaAllPtomedicodiDespConfigurados.Contains(x)).ToList();
            List<int> listaPtoFaltante96 = listaAllPtomedicodiMed96.Where(x => !listaAllPtomedicodiMedConfigurados.Contains(x)).ToList();

            var listaPto48Val = listDespacho.Where(x => listaPtoFaltante48.Contains(x.Ptomedicodi)).GroupBy(x => x.Ptomedicodi).Select(x => x.Last());
            var listaPto96Val = listMedidores.Where(x => listaPtoFaltante96.Contains(x.Ptomedicodi)).GroupBy(x => x.Ptomedicodi).Select(x => x.Last());

            foreach (var reg in listaPto96Val)
            {
                listaMsjValidacion.Add("Punto MEDIDORES[" + reg.Ptomedicodi + "]: " + reg.Emprnomb + "," + reg.Central + "," + reg.Equinomb);
            }
            foreach (var reg in listaPto48Val)
            {
                listaMsjValidacion.Add("Punto DESPACHO[" + reg.Ptomedicodi + "]: " + reg.Emprnomb + "," + reg.Central + "," + reg.Grupoabrev);
            }

            msjValidacion = string.Join("<br/>", listaMsjValidacion);

            return resultado;
        }

        /// <summary>
        /// Permite obtener los datos de MD de despacho
        /// </summary>
        /// <param name="objMD"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private decimal ObtenerMaximaDemandaDespacho(ItemReporteMedicionDTO objMD, List<MeMedicion48DTO> list)
        {
            List<MeMedicion48DTO> subList = list.Where(x => x.Medifecha.Date == objMD.FechaMaximaDemanda.Date).ToList();

            MeMedicion48DTO item = new MeMedicion48DTO();
            item.H1 = subList.Sum(t => t.H1);
            item.H2 = subList.Sum(t => t.H2);
            item.H3 = subList.Sum(t => t.H3);
            item.H4 = subList.Sum(t => t.H4);
            item.H5 = subList.Sum(t => t.H5);
            item.H6 = subList.Sum(t => t.H6);
            item.H7 = subList.Sum(t => t.H7);
            item.H8 = subList.Sum(t => t.H8);
            item.H9 = subList.Sum(t => t.H9);
            item.H10 = subList.Sum(t => t.H10);
            item.H11 = subList.Sum(t => t.H11);
            item.H12 = subList.Sum(t => t.H12);
            item.H13 = subList.Sum(t => t.H13);
            item.H14 = subList.Sum(t => t.H14);
            item.H15 = subList.Sum(t => t.H15);
            item.H16 = subList.Sum(t => t.H16);
            item.H17 = subList.Sum(t => t.H17);
            item.H18 = subList.Sum(t => t.H18);
            item.H19 = subList.Sum(t => t.H19);
            item.H20 = subList.Sum(t => t.H20);
            item.H21 = subList.Sum(t => t.H21);
            item.H22 = subList.Sum(t => t.H22);
            item.H23 = subList.Sum(t => t.H23);
            item.H24 = subList.Sum(t => t.H24);
            item.H25 = subList.Sum(t => t.H25);
            item.H26 = subList.Sum(t => t.H26);
            item.H27 = subList.Sum(t => t.H27);
            item.H28 = subList.Sum(t => t.H28);
            item.H29 = subList.Sum(t => t.H29);
            item.H30 = subList.Sum(t => t.H30);
            item.H31 = subList.Sum(t => t.H31);
            item.H32 = subList.Sum(t => t.H32);
            item.H33 = subList.Sum(t => t.H33);
            item.H34 = subList.Sum(t => t.H34);
            item.H35 = subList.Sum(t => t.H35);
            item.H36 = subList.Sum(t => t.H36);
            item.H37 = subList.Sum(t => t.H37);
            item.H38 = subList.Sum(t => t.H38);
            item.H39 = subList.Sum(t => t.H39);
            item.H40 = subList.Sum(t => t.H40);
            item.H41 = subList.Sum(t => t.H41);
            item.H42 = subList.Sum(t => t.H42);
            item.H43 = subList.Sum(t => t.H43);
            item.H44 = subList.Sum(t => t.H44);
            item.H45 = subList.Sum(t => t.H45);
            item.H46 = subList.Sum(t => t.H46);
            item.H47 = subList.Sum(t => t.H47);
            item.H48 = subList.Sum(t => t.H48);

            var valor = item.GetType().GetProperty(ConstantesAppServicio.CaracterH + objMD.Indice).GetValue(item, null);

            if (valor != null)
            {
                return Convert.ToDecimal(valor);
            }

            return 0;
        }

        /// <summary>
        /// Permite obtener los datos de MD de medidor
        /// </summary>
        /// <param name="objMD"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private decimal ObtenerMaximaDemandaMedidor(ItemReporteMedicionDTO objMD, List<MeMedicion96DTO> list)
        {
            List<MeMedicion96DTO> subList = list.Where(x => x.Medifecha.Value.Date == objMD.FechaMaximaDemanda.Date).ToList();

            MeMedicion96DTO item = new MeMedicion96DTO();

            item.H1 = subList.Sum(t => t.H1);
            item.H2 = subList.Sum(t => t.H2);
            item.H3 = subList.Sum(t => t.H3);
            item.H4 = subList.Sum(t => t.H4);
            item.H5 = subList.Sum(t => t.H5);
            item.H6 = subList.Sum(t => t.H6);
            item.H7 = subList.Sum(t => t.H7);
            item.H8 = subList.Sum(t => t.H8);
            item.H9 = subList.Sum(t => t.H9);
            item.H10 = subList.Sum(t => t.H10);
            item.H11 = subList.Sum(t => t.H11);
            item.H12 = subList.Sum(t => t.H12);
            item.H13 = subList.Sum(t => t.H13);
            item.H14 = subList.Sum(t => t.H14);
            item.H15 = subList.Sum(t => t.H15);
            item.H16 = subList.Sum(t => t.H16);
            item.H17 = subList.Sum(t => t.H17);
            item.H18 = subList.Sum(t => t.H18);
            item.H19 = subList.Sum(t => t.H19);
            item.H20 = subList.Sum(t => t.H20);
            item.H21 = subList.Sum(t => t.H21);
            item.H22 = subList.Sum(t => t.H22);
            item.H23 = subList.Sum(t => t.H23);
            item.H24 = subList.Sum(t => t.H24);
            item.H25 = subList.Sum(t => t.H25);
            item.H26 = subList.Sum(t => t.H26);
            item.H27 = subList.Sum(t => t.H27);
            item.H28 = subList.Sum(t => t.H28);
            item.H29 = subList.Sum(t => t.H29);
            item.H30 = subList.Sum(t => t.H30);
            item.H31 = subList.Sum(t => t.H31);
            item.H32 = subList.Sum(t => t.H32);
            item.H33 = subList.Sum(t => t.H33);
            item.H34 = subList.Sum(t => t.H34);
            item.H35 = subList.Sum(t => t.H35);
            item.H36 = subList.Sum(t => t.H36);
            item.H37 = subList.Sum(t => t.H37);
            item.H38 = subList.Sum(t => t.H38);
            item.H39 = subList.Sum(t => t.H39);
            item.H40 = subList.Sum(t => t.H40);
            item.H41 = subList.Sum(t => t.H41);
            item.H42 = subList.Sum(t => t.H42);
            item.H43 = subList.Sum(t => t.H43);
            item.H44 = subList.Sum(t => t.H44);
            item.H45 = subList.Sum(t => t.H45);
            item.H46 = subList.Sum(t => t.H46);
            item.H47 = subList.Sum(t => t.H47);
            item.H48 = subList.Sum(t => t.H48);
            item.H49 = subList.Sum(t => t.H49);
            item.H50 = subList.Sum(t => t.H50);
            item.H51 = subList.Sum(t => t.H51);
            item.H52 = subList.Sum(t => t.H52);
            item.H53 = subList.Sum(t => t.H53);
            item.H54 = subList.Sum(t => t.H54);
            item.H55 = subList.Sum(t => t.H55);
            item.H56 = subList.Sum(t => t.H56);
            item.H57 = subList.Sum(t => t.H57);
            item.H58 = subList.Sum(t => t.H58);
            item.H59 = subList.Sum(t => t.H59);
            item.H60 = subList.Sum(t => t.H60);
            item.H61 = subList.Sum(t => t.H61);
            item.H62 = subList.Sum(t => t.H62);
            item.H63 = subList.Sum(t => t.H63);
            item.H64 = subList.Sum(t => t.H64);
            item.H65 = subList.Sum(t => t.H65);
            item.H66 = subList.Sum(t => t.H66);
            item.H67 = subList.Sum(t => t.H67);
            item.H68 = subList.Sum(t => t.H68);
            item.H69 = subList.Sum(t => t.H69);
            item.H70 = subList.Sum(t => t.H70);
            item.H71 = subList.Sum(t => t.H71);
            item.H72 = subList.Sum(t => t.H72);
            item.H73 = subList.Sum(t => t.H73);
            item.H74 = subList.Sum(t => t.H74);
            item.H75 = subList.Sum(t => t.H75);
            item.H76 = subList.Sum(t => t.H76);
            item.H77 = subList.Sum(t => t.H77);
            item.H78 = subList.Sum(t => t.H78);
            item.H79 = subList.Sum(t => t.H79);
            item.H80 = subList.Sum(t => t.H80);
            item.H81 = subList.Sum(t => t.H81);
            item.H82 = subList.Sum(t => t.H82);
            item.H83 = subList.Sum(t => t.H83);
            item.H84 = subList.Sum(t => t.H84);
            item.H85 = subList.Sum(t => t.H85);
            item.H86 = subList.Sum(t => t.H86);
            item.H87 = subList.Sum(t => t.H87);
            item.H88 = subList.Sum(t => t.H88);
            item.H89 = subList.Sum(t => t.H89);
            item.H90 = subList.Sum(t => t.H90);
            item.H91 = subList.Sum(t => t.H91);
            item.H92 = subList.Sum(t => t.H92);
            item.H93 = subList.Sum(t => t.H93);
            item.H94 = subList.Sum(t => t.H94);
            item.H95 = subList.Sum(t => t.H95);
            item.H96 = subList.Sum(t => t.H96);

            var valor = item.GetType().GetProperty(ConstantesAppServicio.CaracterH + objMD.Indice).GetValue(item, null);

            if (valor != null)
            {
                return Convert.ToDecimal(valor);
            }

            return 0;
        }

        #endregion

        #region Mantenimiento de Equivalencias

        /// <summary>
        /// Permite obtener los puntos de medicion por empresas
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<WbMedidoresValidacionDTO> ObtenerPuntosMedicion(int origlectcodi, int emprcodi)
        {
            return FactorySic.GetWbMedidoresValidacionRepository().ObtenerPuntosPorEmpresa(origlectcodi, emprcodi);
        }

        /// <summary>
        /// Permite obtener las empresas del SEIN
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasTienenCentralGen()
        {
            return (new IEODAppServicio()).ListarEmpresasTienenCentralGenxTipoEmpresa(ConstantesAppServicio.ParametroDefecto);
        }

        /// <summary>
        /// Permite relacionar los codigos de despacho y medidores
        /// </summary>
        /// <param name="idMedicion"></param>
        /// <param name="idDespacho"></param>
        /// <returns></returns>
        public int RelacionarPuntos(int idMedicion, int idDespacho, string userName)
        {
            try
            {
                int count = FactorySic.GetWbMedidoresValidacionRepository().ValidarExistencia(idMedicion, idDespacho);

                if (count == 0)
                {
                    WbMedidoresValidacionDTO relacion = new WbMedidoresValidacionDTO();

                    relacion.Ptomedicodimed = idMedicion;
                    relacion.Ptomedicodidesp = idDespacho;
                    relacion.Indestado = ConstantesAppServicio.Activo;
                    relacion.Lastuser = userName;
                    relacion.Lastdate = DateTime.Now;

                    FactorySic.GetWbMedidoresValidacionRepository().Save(relacion);

                    return 1;
                }
                return 2;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar las relaciones entre puntos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<WbMedidoresValidacionDTO> ListarRelaciones(int? idEmpresa)
        {
            if (idEmpresa == null) idEmpresa = -1;

            List<WbMedidoresValidacionDTO> list = FactorySic.GetWbMedidoresValidacionRepository().List();
            List<WbMedidoresValidacionDTO> relaciones = FactorySic.GetWbMedidoresValidacionRepository().ObtenerRelaciones((int)idEmpresa);

            foreach (WbMedidoresValidacionDTO entity in relaciones)
            {
                WbMedidoresValidacionDTO itemMedidor = list.Where(x => x.Ptomedicodi == entity.Ptomedicodimed).FirstOrDefault();
                WbMedidoresValidacionDTO itemDespacho = list.Where(x => x.Ptomedicodi == entity.Ptomedicodidesp).FirstOrDefault();

                if (itemMedidor != null)
                {
                    entity.Centralmed = itemMedidor.Central;
                    entity.Empresamed = itemMedidor.Emprnomb;
                    entity.Equipomed = itemMedidor.Equinomb;
                    entity.Ptomediestadomed = itemMedidor.Ptomediestado;
                    entity.Grupocodi = itemMedidor.Grupocodi;
                }

                if (itemDespacho != null)
                {
                    entity.Centraldesp = itemDespacho.Central;
                    entity.Empresadesp = itemDespacho.Emprnomb;
                    entity.Equipodesp = itemDespacho.Equinomb;
                    entity.Ptomediestadodesp = itemDespacho.Ptomediestado;
                    entity.Grupocodi = itemDespacho.Grupocodi;
                }
            }

            return relaciones.OrderBy(x => x.Empresamed).ThenBy(x => x.Centralmed).ThenBy(x => x.Equipomed).ToList();
        }

        public List<MeMedicion96DTO> ObtenerDatosMedicionComparativo(DateTime fechaInicio, DateTime fechaFin, string puntos)
        {
            int tipoInfocodi = ConstantesMedicion.IdTipoInfoPotenciaActiva;
            int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);
            return FactorySic.GetMeMedicion96Repository().ObtenerDatosMedicionComparativo(fechaInicio, fechaFin, puntos, lectcodi, tipoInfocodi);
        }

        public List<MeMedicion48DTO> ObtenerDatosDespachoComparativo(DateTime fechaInicio, DateTime fechaFin, string puntos)
        {
            int tipoinfocodi = ConstantesMedicion.IdTipoInfoPotenciaActiva;
            int lectcodi = ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto;
            return FactorySic.GetMeMedicion48Repository().ObtenerDatosDespachoComparativo(fechaInicio, fechaFin, puntos, lectcodi, tipoinfocodi);
        }

        public List<WbMedidoresValidacionDTO> ObtenerConfiguracionRelacion(int? idEmpresa)
        {
            return this.ListarRelaciones(idEmpresa).Where(x => x.Ptomediestadodesp == ConstantesAppServicio.Activo).ToList();
        }

        #endregion
    }



   



    public class ReporteValidacionMedidor
    {
        public string DesEmpresa { get; set; }
        public string DesGrupo { get; set; }
        public decimal ValorMedidor { get; set; }
        public decimal ValorDespacho { get; set; }
        public decimal MDMedidor { get; set; }
        public decimal MDDespacho { get; set; }
        public string IndColor { get; set; }
        public string Color { get; set; }
        public decimal Desviacion { get; set; }
        public decimal MDDesviacion { get; set; }
        public string IndMuestra { get; set; }
        public int Filtro { get; set; }
        public int PtoMediCodiDesp { get; set; }
        public int PtoMediCodiMed { get; set; }
    }
}
