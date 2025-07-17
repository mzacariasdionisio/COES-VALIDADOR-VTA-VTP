using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.TiempoReal;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace COES.Servicios.Aplicacion.Medidores
{
    /// <summary>
    /// Clases con métodos del módulo Medidores
    /// </summary>
    public class MedidoresAppServicio : AppServicioBase
    {
        ReporteMedidoresAppServicio servReporte = new ReporteMedidoresAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MedidoresAppServicio));


        #region Métodos Tabla ME_MEDICION96


        /// <summary>
        /// Reporte de máxima demanda diaria
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        public List<DemandadiaDTO> ListarDemandaPorDia(DateTime fechaini, DateTime fechafin, string tiposEmpresa, string empresas,
            string tiposGeneracion, int central)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }
            
            List<MeMedicion96DTO> listaDemanda = this.servReporte.ListaDataMDGeneracion(fechaini, fechafin, central, tiposGeneracion, empresas, ConstanteValidacion.EstadoTodos);
            List<MeMedicion96DTO> listaInterconexion = this.servReporte.ListaDataMDInterconexion96(fechaini, fechafin);
            List<MeMedicion96DTO> listaDia = this.servReporte.ListaDataMDTotalSEIN(listaDemanda, listaInterconexion);

            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaRangoNormaHP = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            List<SiParametroValorDTO> listaBloqueHorario = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            foreach (var reg in listaDia)
            {
                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoMDNormativa, reg.Medifecha.Value, new List<MeMedicion96DTO>() { reg }, listaRangoNormaHP, listaBloqueHorario,
                                                                out decimal valorMDDia, out int hMD, out DateTime fechaHoraMD, out _);


                reg.Ptomedicodi = hMD; //ptomedicodi solo es para guardar la posición del H
                reg.Meditotal = valorMDDia;
            }

            var lista = this.servReporte.ListaDataMDGeneracionConsolidado(fechaini, fechafin, central, tiposGeneracion, empresas, ConstanteValidacion.EstadoTodos
                , ConstantesMedicion.IdTipoRecursoTodos.ToString(), false);

            DemandadiaDTO entity;
            List<DemandadiaDTO> listaResultado = new List<DemandadiaDTO>();
            foreach (var reg in lista)
            {
                var regFecha = listaDia.FirstOrDefault(x => x.Medifecha == reg.Medifecha);
                if (regFecha != null)
                {
                    try
                    {
                        entity = new DemandadiaDTO();
                        entity.Medifecha = (DateTime)reg.Medifecha;
                        entity.Empresanomb = reg.Emprnomb;
                        entity.Centralnomb = reg.Central;
                        if (reg.Central == ConstantesMedicion.KeyTodos)
                            entity.Centralnomb = reg.Equinomb;
                        entity.Gruponomb = reg.Equinomb;
                        entity.Tipogeneracion = string.Empty;
                        if (reg.Tgenernomb != null)
                            entity.Tipogeneracion = reg.Tgenernomb;
                        entity.HMax = regFecha.Ptomedicodi;
                        entity.Valor = ((decimal?)reg.GetType().GetProperty("H" + entity.HMax.ToString()).GetValue(reg, null)).GetValueOrDefault(0);
                        entity.DestinoPotencia = ConstantesMedicion.GeneracionPeru;

                        listaResultado.Add(entity);
                    }
                    catch (Exception ex)
                    {
                        var text = ex.Message;
                    }
                }
            }

            //Interconexion
            foreach (var reg in listaInterconexion)
            {
                var regFecha = listaDia.FirstOrDefault(x => x.Medifecha == reg.Medifecha);
                var regInter = listaInterconexion.FirstOrDefault(x => x.Medifecha == reg.Medifecha);
                if (regFecha!=null && regInter != null) 
                {
                    try
                    {
                        entity = new DemandadiaDTO();
                        entity.Medifecha = (DateTime)reg.Medifecha;
                        entity.HMax = regFecha.Ptomedicodi;
                        entity.Valor = ((decimal?)regInter.GetType().GetProperty("H" + entity.HMax.ToString()).GetValue(reg, null)).GetValueOrDefault(0)/4;
                        if (entity.Valor > 0) // Exportacion
                            entity.DestinoPotencia = ConstantesMedicion.ExportacionEcuador;
                        else
                            entity.DestinoPotencia = ConstantesMedicion.ImportacionEcuador;

                        listaResultado.Add(entity);
                    }
                    catch (Exception ex)
                    {
                        var text = ex.Message;
                    }
                }
            }

            listaResultado = listaResultado.OrderBy(x => x.Empresanomb).ThenBy(x => x.Centralnomb).ThenBy(x => x.Gruponomb).ThenBy(x => x.Medifecha.Date).ToList();

            return listaResultado;
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
        /// Muestra el reporte de máxima demanda HFP y HP
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        public List<DemandadiaDTO> ListarDemandaDiaHFPHP(DateTime fechaini, DateTime fechafin, string tiposEmpresa, string empresas,
            string tiposGeneracion, int central)
        {

            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            var listaDia = FactorySic.GetMeMedicion96Repository().ListarTotalH(fechaini, fechafin, empresas, tiposGeneracion, central);

            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaBloqueHorario = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            DemandadiaDTO entity;
            List<DemandadiaDTO> entitys = new List<DemandadiaDTO>();
            foreach (var reg in listaDia)
            {
                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoHoraPunta, reg.Medifecha.Value, new List<MeMedicion96DTO>() { reg}, null, listaBloqueHorario, 
                                                                out decimal valorHP, out int horaHP, out DateTime fechaHoraHP, out _);

                MedidoresHelper.ObtenerValorHXPeriodoDemandaM96(ConstantesRepMaxDemanda.TipoFueraHoraPunta, reg.Medifecha.Value, new List<MeMedicion96DTO>() { reg }, null, listaBloqueHorario,
                                                                out decimal valorHFP, out int horaHFP, out DateTime fechaHoraHFP, out _);

                entity = new DemandadiaDTO();
                entity.ValorHFP = valorHFP;
                entity.ValorHP = valorHP;
                entity.Medifecha = (DateTime)reg.Medifecha;
                entity.MedifechaHFP = ((DateTime)reg.Medifecha).AddMinutes(horaHFP * 15).ToString("HH:mm");
                entity.MedifechaHP = ((DateTime)reg.Medifecha).AddMinutes(horaHP * 15).ToString("HH:mm");
                entitys.Add(entity);
                valorHFP = 0M;
                valorHP = 0M;
                horaHFP = 0;
                horaHP = 0;
            }

            return entitys;
        }

        /// <summary>
        /// Permite obtener la consulta de los datos de Interconexion cargados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ObtenerConsultaInterconexion(int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            List<MeMedicion96DTO> list = FactorySic.GetMeMedicion96Repository().ObtenerEnvioInterconexion(idEmpresa, fechaInicio, fechaFin);

            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table border='1' class='pretty tabla-adicional cell-border' cellspacing='0' width='100%' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th colspan='7'>LINEA DE TRANSMISIÓN L-2280 (ZORRITOS - MACHALA)</th>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA HORA</th>");
            strHtml.Append("<th>EXPORTACIÓN <br> L-2280 (ZORRITOS) <br> MWh</th>");
            strHtml.Append("<th>IMPORTACIÓN <br> L-2280 (ZORRITOS) <br> MWh</th>");
            strHtml.Append("<th>EXPORTACIÓN <br> L-2280 (ZORRITOS) <br> MVARh</th>");
            strHtml.Append("<th>IMPORTACIÓN <br> L-2280 (ZORRITOS) <br> MVARh</th>");
            strHtml.Append("<th>L-2280 (ZORRITOS) kV</th>");
            strHtml.Append("<th>L-2280 (ZORRITOS) Amp.</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            int minuto = 0;

            if (list.Count > 0)
            {
                for (int k = 1; k <= 96; k++)
                {
                    minuto = minuto + 15;
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td>{0}</td>", fechaInicio.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm")));
                    MeMedicion96DTO entity = list.Where(x => x.Tipoinfocodi == 3).ToList().FirstOrDefault();
                    decimal valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                    string mwExport = "";
                    string mwImport = "";
                    if (valor > 0)
                    {
                        mwExport = valor.ToString("N", nfi);
                        mwImport = 0.ToString("N", nfi);
                    }
                    else
                    {
                        valor = valor * (-1);
                        mwImport = valor.ToString("N", nfi);
                        mwExport = 0.ToString("N", nfi);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", mwExport));
                    strHtml.Append(string.Format("<td>{0}</td>", mwImport));

                    string mvarExport = "";
                    string mvarImport = "";
                    entity = list.Where(x => x.Tipoinfocodi == 4).ToList().FirstOrDefault();
                    valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                    if (valor > 0)
                    {
                        mvarExport = valor.ToString("N", nfi);
                        mvarImport = 0.ToString("N", nfi);
                    }
                    else
                    {
                        valor = valor * (-1);
                        mvarImport = valor.ToString("N", nfi);
                        mvarExport = 0.ToString("N", nfi);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", mvarExport));
                    strHtml.Append(string.Format("<td>{0}</td>", mvarImport));

                    entity = list.Where(x => x.Tipoinfocodi == 5).ToList().FirstOrDefault();
                    valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                    string kVvalor = valor.ToString("N", nfi);
                    strHtml.Append(string.Format("<td>{0}</td>", kVvalor));

                    entity = list.Where(x => x.Tipoinfocodi == 9).ToList().FirstOrDefault();
                    valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                    string AmpValor = valor.ToString("N", nfi);
                    strHtml.Append(string.Format("<td>{0}</td>", AmpValor));
                    strHtml.Append("</tr>");
                }

            }
            else
            {
                strHtml.Append("<td  style='text-align:center'>No existen registros.</td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }



        /// <summary>
        /// Obtiene Lista Historico Interconexion
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObtenerHistoricoInterconexion(int idPtomedicion, DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetMeMedicion96Repository().ObtenerHistoricoInterconexion(idPtomedicion, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Obtiene el Reporte Historico de Interconexion en formato html
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ObtenerConsultaHistoricaInterconexion(int idPtomedicion, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion96DTO> list = FactorySic.GetMeMedicion96Repository().ObtenerHistoricoInterconexion(idPtomedicion, fechaInicio, fechaFin);
            return ObtenerHtmlReporteInterconexion(list, fechaInicio, fechaFin);
        }
        /// <summary>
        /// Obtiene el reporte historico por pagina
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        public string ObtenerConsultaHistoricaPagInterconexion(int idPtomedicion, DateTime fechaInicio, DateTime fechaFin, int pagina)
        {
            List<MeMedicion96DTO> list = FactorySic.GetMeMedicion96Repository().ObtenerHistoricoPagInterconexion(idPtomedicion, fechaInicio, fechaFin, pagina);
            return ObtenerHtmlReporteInterconexion(list, fechaInicio, fechaFin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fini"></param>
        /// <param name="ffin"></param>
        /// <returns></returns>
        public string ObtenerHtmlReporteInterconexion(List<MeMedicion96DTO> lista, DateTime fini, DateTime ffin)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table border='1' class='pretty tabla-adicional' cellspacing='0' width='100%' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th colspan='7'>LINEA DE TRANSMISIÓN L-2280 (ZORRITOS - MACHALA)</th>");

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA HORA</th>");
            strHtml.Append("<th>EXPORTACIÓN <br> L-2280 (ZORRITOS) <br> MWh</th>");
            strHtml.Append("<th>IMPORTACIÓN <br> L-2280 (ZORRITOS) <br> MWh</th>");
            strHtml.Append("<th>EXPORTACIÓN <br> L-2280 (ZORRITOS) <br> MVARh</th>");
            strHtml.Append("<th>IMPORTACIÓN <br> L-2280 (ZORRITOS) <br> MVARh</th>");
            strHtml.Append("<th>L-2280 (ZORRITOS) kV</th>");
            strHtml.Append("<th>L-2280 (ZORRITOS) Amp.</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            int minuto = 0;

            for (DateTime f = fini; f <= ffin; f = f.AddDays(1))
            {
                var list = lista.Where(x => x.Medifecha == f);
                if (list.Count() != 0)
                    for (int k = 1; k <= 96; k++)
                    {

                        minuto = minuto + 15;
                        MeMedicion96DTO entity = list.Where(x => x.Tipoinfocodi == 3).ToList().FirstOrDefault();
                        strHtml.Append("<tr>");
                        strHtml.Append(string.Format("<td>{0}</td>", entity.Medifecha.Value.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm")));
                        decimal valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                        string mwExport = "";
                        string mwImport = "";
                        if (valor > 0)
                        {
                            mwExport = valor.ToString("N", nfi);
                            mwImport = 0.ToString("N", nfi);
                        }
                        else
                        {
                            valor = valor * (-1);
                            mwImport = valor.ToString("N", nfi);
                            mwExport = 0.ToString("N", nfi);
                        }
                        strHtml.Append(string.Format("<td>{0}</td>", mwExport));
                        strHtml.Append(string.Format("<td>{0}</td>", mwImport));

                        string mvarExport = "";
                        string mvarImport = "";
                        entity = list.Where(x => x.Tipoinfocodi == 4).ToList().FirstOrDefault();
                        valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                        if (valor > 0)
                        {
                            mvarExport = valor.ToString("N", nfi);
                            mvarImport = 0.ToString("N", nfi);
                        }
                        else
                        {
                            valor = valor * (-1);
                            mvarImport = valor.ToString("N", nfi);
                            mvarExport = 0.ToString("N", nfi);
                        }
                        strHtml.Append(string.Format("<td>{0}</td>", mvarExport));
                        strHtml.Append(string.Format("<td>{0}</td>", mvarImport));

                        entity = list.Where(x => x.Tipoinfocodi == 5).ToList().FirstOrDefault();
                        valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                        string kVvalor = valor.ToString("N", nfi);
                        strHtml.Append(string.Format("<td>{0}</td>", kVvalor));
                        valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                        string AmpValor = valor.ToString("N", nfi);
                        entity = list.Where(x => x.Tipoinfocodi == 9).ToList().FirstOrDefault();
                        strHtml.Append(string.Format("<td>{0}</td>", AmpValor));
                        strHtml.Append("</tr>");
                    }

            }
            if (lista.Count == 0)
            {
                strHtml.Append("<tr><td  style='text-align:center'>No existen registros.</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <returns></returns>
        public int ObtenerTotalHistoricoInterconexion(int ptomedicodi, DateTime fechaini, DateTime fechafin)
        {
            return FactorySic.GetMeMedicion96Repository().ObtenerTotalHistoricoInterconexion(ptomedicodi, fechaini, fechafin);
        }
        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados(int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion96Repository().DeleteEnvioInterconexion(fechaInicio);
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
        public void GrabarInterconexion(List<MeMedicion96DTO> entitys)
        {
            try
            {
                foreach (MeMedicion96DTO entity in entitys)
                {

                    entity.Lectcodi = 1;
                    FactorySic.GetMeMedicion96Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idPtoInter"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> ObtenerListaIntercambiosElectricidad(int idPtoInter, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            //Lista total por dia de interconexiones
            decimal? totalImpor = 0;
            decimal? totalExpor = 0;
            var listaInter = ObtenerHistoricoInterconexion(idPtoInter, fechaInicio, fechaFin).Where(x => x.Tipoinfocodi == 3);
            //Sumar total por dia
            foreach (var reg in listaInter)
            {
                totalImpor = 0M;
                totalExpor = 0M;
                for (var i = 1; i <= 96; i++)
                {
                    var valor = (decimal?)reg.GetType().GetProperty("H" + i).GetValue(reg, null);
                    if (valor != null)
                    {
                        if (valor > 0)
                            totalExpor += valor;
                        else
                            totalImpor += Math.Abs((decimal)valor);
                    }
                }
                MeMedicion24DTO registro = new MeMedicion24DTO();
                registro.Ptomedicodi = idPtoInter;
                registro.H1 = totalExpor;
                registro.Medifecha = (DateTime)reg.Medifecha;
                registro.H3 = totalImpor;
                int colH = ObtieneMaximaDemandaDia((DateTime)reg.Medifecha);
                var valor1 = (decimal?)reg.GetType().GetProperty("H" + colH.ToString()).GetValue(reg, null);
                totalExpor = 0;
                totalImpor = 0;
                if (valor1 != null)
                {
                    if (valor1 > 0)
                        totalExpor = valor1 * 4;
                    else
                        totalImpor = -1 * valor1 * 4;
                }
                registro.H2 = totalExpor;
                registro.H4 = totalImpor;
                lista.Add(registro);
            }
            //Lista total exportada en maxima demanda
            //Unirla y retornar la lista
            return lista;
        }

        /// <summary>
        /// Obtiene Listado Historico de interconexiones por parametro (MW,MVAr,KV,Amp)
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="idParametro"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObtenerInterconexionParametro(int idPtomedicion, int idParametro, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            lista = FactorySic.GetMeMedicion96Repository().ObtenerHistoricoInterconexion(3, fechaInicio, fechaFin);
            switch (idParametro)
            {
                case 1:
                case 3:
                    lista = lista.Where(x => x.Tipoinfocodi == 3).ToList();
                    break;
                case 2:
                case 4:
                    lista = lista.Where(x => x.Tipoinfocodi == 4).ToList();
                    break;
                case 5:
                    lista = lista.Where(x => x.Tipoinfocodi == 5 || x.Tipoinfocodi == 9).ToList();
                    break;
            }

            return lista;

        }

        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="idParametro"></param>
        /// <param name="fini"></param>
        /// <param name="ffin"></param>
        /// <returns></returns>
        public string GetHtmlInterconexionXParametro(int idPtomedicion, int idParametro, DateTime fini, DateTime ffin)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            List<MeMedicion96DTO> lista = FactorySic.GetMeMedicion96Repository().ObtenerHistoricoInterconexion(idPtomedicion, fini, ffin);
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table border='1' class='pretty tabla-adicional' cellspacing='0' width='100%' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA HORA</th>");

            switch (idParametro)
            {
                case 1:
                    strHtml.Append("<th>L-2280 (ZORRITOS) <br> Exportación <br> MW</th>");
                    strHtml.Append("<th>L-2280 (ZORRITOS) <br> Importación <br> MW</th>");
                    break;
                case 2:
                    strHtml.Append("<th>L-2280 (ZORRITOS) <br> Exportación <br> MVAR</th>");
                    strHtml.Append("<th>L-2280 (ZORRITOS) <br> Importación <br> MVAR</th>");
                    break;
                case 3:
                    strHtml.Append("<th>L-2280 (ZORRITOS) <br> Exportación <br> MWh</th>");
                    strHtml.Append("<th>L-2280 (ZORRITOS) <br> Importación <br> MWh</th>");
                    break;
                case 4:
                    strHtml.Append("<th>L-2280 (ZORRITOS) <br> Exportación <br> MVARh</th>");
                    strHtml.Append("<th>L-2280 (ZORRITOS) <br> Importación <br> MVARh</th>");
                    break;
                case 5:
                    strHtml.Append("<th>L-2280 (ZORRITOS) <br> kV</th>");
                    strHtml.Append("<th>L-2280 (ZORRITOS) <br> Amp</th>");
                    break;
            }

            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            int minuto = 0;
            MeMedicion96DTO entity;
            for (DateTime f = fini; f <= ffin; f = f.AddDays(1))
            {
                var list = lista.Where(x => x.Medifecha == f);
                minuto = 0;
                if (list.Count() != 0)
                    for (int k = 1; k <= 96; k++)
                    {
                        minuto = minuto + 15;

                        switch (idParametro)
                        {
                            case 1:
                            case 3:
                                ///Exportacion
                                entity = list.Where(x => x.Tipoinfocodi == 3 && x.Ptomedicodi ==
                                    41103).ToList().FirstOrDefault();
                                strHtml.Append("<tr>");
                                strHtml.Append(string.Format("<td>{0}</td>", entity.Medifecha.Value.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm")));
                                decimal valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 1)
                                    valor = valor * 4;
                                string mwExport = valor.ToString("N", nfi);
                                ///Importacion
                                entity = list.Where(x => x.Tipoinfocodi == 3 && x.Ptomedicodi ==
                                    41104).ToList().FirstOrDefault();
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 1)
                                    valor = valor * 4;
                                string mwImport = valor.ToString("N", nfi); ;

                                strHtml.Append(string.Format("<td>{0}</td>", mwExport));
                                strHtml.Append(string.Format("<td>{0}</td>", mwImport));
                                strHtml.Append("</tr>");
                                break;
                            case 2:
                            case 4:


                                strHtml.Append("<tr>");
                                entity = list.Where(x => x.Tipoinfocodi == 4 && x.Ptomedicodi ==
                                    41105).ToList().FirstOrDefault();
                                strHtml.Append(string.Format("<td>{0}</td>", entity.Medifecha.Value.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm")));
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 2)
                                    valor = valor * 4;
                                string mvarExport = valor.ToString("N", nfi);
                                entity = list.Where(x => x.Tipoinfocodi == 4 && x.Ptomedicodi ==
                                    41105).ToList().FirstOrDefault();
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 2)
                                    valor = valor * 4;
                                string mvarImport = valor.ToString("N", nfi);
                                strHtml.Append(string.Format("<td>{0}</td>", mvarExport));
                                strHtml.Append(string.Format("<td>{0}</td>", mvarImport));
                                strHtml.Append("</tr>");
                                break;
                            //case 3:
                            //    break;
                            //case 4:
                            //    break;
                            case 5:
                                entity = list.Where(x => x.Tipoinfocodi == 5).ToList().FirstOrDefault();
                                strHtml.Append(string.Format("<td>{0}</td>", entity.Medifecha.Value.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm")));
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                string kVvalor = valor.ToString("N", nfi);
                                strHtml.Append(string.Format("<td>{0}</td>", kVvalor));
                                entity = list.Where(x => x.Tipoinfocodi == 9).ToList().FirstOrDefault();
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                string AmpValor = valor.ToString("N", nfi);
                                strHtml.Append(string.Format("<td>{0}</td>", AmpValor));
                                strHtml.Append("</tr>");
                                break;
                        }
                    }
            }
            if (lista.Count == 0)
            {
                strHtml.Append("<tr><td  style='text-align:center'>No existen registros.</td><td></td><td></td></tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Obtiene el cuarto de hora de la máxima demanda del día
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public int ObtieneMaximaDemandaDia(DateTime fecha)
        {
            decimal valorMax = 0M;
            int horaMD = 1;
            var listaDia = FactorySic.GetMeMedicion96Repository().ListarTotalH(fecha, fecha, "-1", "-1", 1);
            foreach (var reg in listaDia)
            {
                for (int i = 1; i <= 96; i++)
                {
                    var valor = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i.ToString()).GetValue(reg, null);
                    if (valor != null)
                        if (valorMax < (decimal)valor)
                        {
                            valorMax = (decimal)valor;
                            horaMD = i;
                        }
                }
            }

            return horaMD;
        }

        #endregion

        //inicio agregado
        #region Métodos SI_EMPRESA
        //fin agregado
        /// <summary>
        /// Permite listar todos los registros de la tabla SI_EMPRESA
        /// </summary>
        public List<SiEmpresaDTO> ListSiEmpresas(int tipoemprcodi)
        {
            return FactorySic.GetSiEmpresaRepository().List(tipoemprcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userlogin"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasPorUsuario(string userlogin)
        {
            return FactorySic.GetSiEmpresaRepository().GetByUser(userlogin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasSEIN()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
        }

        /// <summary>
        /// Obtiene registro de empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public SiEmpresaDTO GetByIdEmpresa(int emprcodi)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(emprcodi);
        }

        //inicio agregado
        /// <summary>
        /// Devuelve lista de empresa por id formato
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> GetListaEmpresaFormato(int idFormato)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormato(idFormato);
        }
        #endregion
        //fin agregado


        /// <summary>
        /// Devuelve lista de empresa por id formato
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> GetListaEmpresaActivasFormato(int idFormato)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresaActivasFormato(idFormato);
        }

        #region Métodos Tabla SI_FUENTEENERGIA

        /// <summary>
        /// Permite obtener un registro de la tabla SI_FUENTEENERGIA
        /// </summary>
        public SiFuenteenergiaDTO GetByIdSiFuenteenergia(int fenergcodi)
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetById(fenergcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_FUENTEENERGIA
        /// </summary>
        public List<SiFuenteenergiaDTO> ListSiFuenteenergias()
        {
            return FactorySic.GetSiFuenteenergiaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiFuenteenergia
        /// </summary>
        public List<SiFuenteenergiaDTO> GetByCriteriaSiFuenteenergias()
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_TIPOGENERACION

        /// <summary>
        /// Inserta un registro de la tabla SI_TIPOGENERACION
        /// </summary>
        public void SaveSiTipogeneracion(SiTipogeneracionDTO entity)
        {
            try
            {
                FactorySic.GetSiTipogeneracionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_TIPOGENERACION
        /// </summary>
        public void UpdateSiTipogeneracion(SiTipogeneracionDTO entity)
        {
            try
            {
                FactorySic.GetSiTipogeneracionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_TIPOGENERACION
        /// </summary>
        public void DeleteSiTipogeneracion(int tgenercodi)
        {
            try
            {
                FactorySic.GetSiTipogeneracionRepository().Delete(tgenercodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_TIPOGENERACION
        /// </summary>
        public SiTipogeneracionDTO GetByIdSiTipogeneracion(int tgenercodi)
        {
            return FactorySic.GetSiTipogeneracionRepository().GetById(tgenercodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TIPOGENERACION
        /// </summary>
        public List<SiTipogeneracionDTO> ListSiTipogeneracions()
        {
            return FactorySic.GetSiTipogeneracionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiTipogeneracion
        /// </summary>
        public List<SiTipogeneracionDTO> GetByCriteriaSiTipogeneracions()
        {
            return FactorySic.GetSiTipogeneracionRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla ME_HOJAPTOMED

        /// <summary>
        /// Inserta un registro de la tabla ME_HOJAPTOMED
        /// </summary>
        public void SaveMeHojaptomed(MeHojaptomedDTO entity, int empresa)
        {
            try
            {
                FactorySic.GetMeHojaptomedRepository().Save(entity, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_HOJAPTOMED
        /// </summary>
        public List<MeHojaptomedDTO> ListMeHojaptomeds()
        {
            return FactorySic.GetMeHojaptomedRepository().List();
        }

        //inicio agregado
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        public List<MeHojaptomedDTO> ListPtosWithTipoGeneracion(int formatcodi, int tgenercodi)
        {
            return FactorySic.GetMeHojaptomedRepository().ListPtosWithTipoGeneracion(formatcodi, tgenercodi);
        }
        //fin agregado
        #endregion

        #region Métodos Tabla ME_MEDIDOR

        /// <summary>
        /// Inserta un registro de la tabla ME_MEDIDOR
        /// </summary>
        public void SaveMeMedidor(MeMedidorDTO entity)
        {
            try
            {
                FactorySic.GetMeMedidorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_MEDIDOR
        /// </summary>
        public void UpdateMeMedidor(MeMedidorDTO entity)
        {
            try
            {
                FactorySic.GetMeMedidorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_MEDIDOR
        /// </summary>
        public void DeleteMeMedidor(int medicodi)
        {
            try
            {
                FactorySic.GetMeMedidorRepository().Delete(medicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_MEDIDOR
        /// </summary>
        public MeMedidorDTO GetByIdMeMedidor(int medicodi)
        {
            return FactorySic.GetMeMedidorRepository().GetById(medicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_MEDIDOR
        /// </summary>
        public List<MeMedidorDTO> ListMeMedidors()
        {
            return FactorySic.GetMeMedidorRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeMedidor
        /// </summary>
        public List<MeMedidorDTO> GetByCriteriaMeMedidors()
        {
            return FactorySic.GetMeMedidorRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla ME_PERIODOMEDIDOR

        /// <summary>
        /// Inserta un registro de la tabla ME_PERIODOMEDIDOR
        /// </summary>
        public void SaveMePeriodomedidor(MePeriodomedidorDTO entity)
        {
            try
            {
                FactorySic.GetMePeriodomedidorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Grabba una lista de periodos
        /// </summary>
        /// <param name="entitys"></param>
        public void SaveListaMePeriodomedidor(List<MePeriodomedidorDTO> entitys)
        {
            foreach (var reg in entitys)
            {
                SaveMePeriodomedidor(reg);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_PERIODOMEDIDOR
        /// </summary>
        public void UpdateMePeriodomedidor(MePeriodomedidorDTO entity)
        {
            try
            {
                FactorySic.GetMePeriodomedidorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_PERIODOMEDIDOR
        /// </summary>
        public void DeleteMePeriodomedidor()
        {
            try
            {
                FactorySic.GetMePeriodomedidorRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_PERIODOMEDIDOR
        /// </summary>
        public MePeriodomedidorDTO GetByIdMePeriodomedidor()
        {
            return FactorySic.GetMePeriodomedidorRepository().GetById();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_PERIODOMEDIDOR
        /// </summary>
        public List<MePeriodomedidorDTO> ListMePeriodomedidors()
        {
            return FactorySic.GetMePeriodomedidorRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MePeriodomedidor
        /// </summary>
        public List<MePeriodomedidorDTO> GetByCriteriaMePeriodomedidors(int idEnvio)
        {
            return FactorySic.GetMePeriodomedidorRepository().GetByCriteria(idEnvio);
        }

        #endregion

        #region Métodos Tabla ME_AMPLIACIONFECHA

        /// <summary>
        /// Inserta un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public void SaveMeAmpliacionfecha(MeAmpliacionfechaDTO entity)
        {
            try
            {
                FactorySic.GetMeAmpliacionfechaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public void UpdateMeAmpliacionfecha(MeAmpliacionfechaDTO entity)
        {
            try
            {
                FactorySic.GetMeAmpliacionfechaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
            
        /// <summary>
        /// Permite obtener un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public MeAmpliacionfechaDTO GetByIdMeAmpliacionfecha(DateTime fecha, int empresa, int formato)
        {
            return FactorySic.GetMeAmpliacionfechaRepository().GetById(fecha, empresa, formato);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public List<MeAmpliacionfechaDTO> ListMeAmpliacionfechas()
        {
            return FactorySic.GetMeAmpliacionfechaRepository().List();
        }

        public List<MeAmpliacionfechaDTO> ObtenerListaMeAmpliacionfechas(DateTime fechaIni, DateTime fechaFin, int empresa, int formato)
        {
            return FactorySic.GetMeAmpliacionfechaRepository().GetListaAmpliacion(fechaIni, fechaFin, empresa, formato);
        }

        #endregion

        #region Métodos Tabla EXT_ARCHIVO

        /// <summary>
        /// Inserta un registro de la tabla EXT_ARCHIVO
        /// </summary>
        public int SaveExtArchivo(ExtArchivoDTO entity)
        {
            int idEnvio = 0;
            try
            {
                idEnvio = FactorySic.GetExtArchivoRepository().Save(entity);
                FactorySic.GetExtArchivoRepository().UpdateMaxId(idEnvio);
                return idEnvio;

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int SaveUploadExtArchivo(ExtArchivoDTO entity, string nombreFile, string extension)
        {
            int idEnvio = 0;
            try
            {
                idEnvio = FactorySic.GetExtArchivoRepository().SaveUpload(entity, nombreFile, extension);
                FactorySic.GetExtArchivoRepository().UpdateMaxId(idEnvio);
                return idEnvio;

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza un registro de la tabla EXT_ARCHIVO
        /// </summary>
        public void UpdateExtArchivo(ExtArchivoDTO entity)
        {
            try
            {
                FactorySic.GetExtArchivoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EXT_ARCHIVO
        /// </summary>
        public void DeleteExtArchivo(int earcodi)
        {
            try
            {
                FactorySic.GetExtArchivoRepository().Delete(earcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EXT_ARCHIVO
        /// </summary>
        public ExtArchivoDTO GetByIdExtArchivo(int earcodi)
        {
            return FactorySic.GetExtArchivoRepository().GetById(earcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EXT_ARCHIVO
        /// </summary>
        public List<ExtArchivoDTO> ListExtArchivos()
        {
            return FactorySic.GetExtArchivoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla ExtArchivo
        /// </summary>
        public List<ExtArchivoDTO> GetByCriteriaExtArchivos(int empresa, int estado, DateTime fechaInicial, DateTime fechaFinal)
        {
            return FactorySic.GetExtArchivoRepository().GetByCriteria(empresa, estado, fechaInicial, fechaFinal);
        }

        public List<ExtArchivoDTO> GetListaEnvioPagInterconexion(int empresa, int estado, DateTime fechaInicial, DateTime fechaFinal, int nroPagina, int nroFilas)
        {
            return FactorySic.GetExtArchivoRepository().ListaEnvioPagInterconexion(empresa, estado, fechaInicial, fechaFinal, nroPagina, nroFilas);
        }

        public int TotalEnvio(DateTime fechaini, DateTime fechafin, int empresa)
        {
            return FactorySic.GetExtArchivoRepository().TotalEnvioInterconexion(fechaini, fechafin, empresa);
        }

        #endregion

        #region Métodos Tabla ME_PTOMEDICION

        /// <summary>
        /// Se ingresa una cadena de puntos de medicion y se devuelve un List de todos los puntos
        /// de medicion con el detalle de dichos puntos(central,Grupo,Empresa)
        /// </summary>
        /// <param name="listaptos"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicion(string listaptos)
        {
            return FactorySic.GetMePtomedicionRepository().ListarPtoMedicion(listaptos);
        }

        /// <summary>
        /// Actualiza los datos de la tabla ME_PTOMEDICION DE AGC
        /// </summary>
        public void UpdateMePtoMedicion(MePtomedicionDTO entity)
        {
            try
            {
                FactorySic.GetMePtomedicionRepository().UpdateMePtoMedicion(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza los datos de la tabla ME_PTOMEDICION DE AGC relacionado a Costo Variable
        /// </summary>
        public void UpdateMePtoMedicionCVariable(MePtomedicionDTO entity)
        {
            try
            {
                FactorySic.GetMePtomedicionRepository().UpdateMePtoMedicionCVariable(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los datos de mediciones
        /// </summary>
        /// <param name="equicodi">Código de equipo</param>
        /// <param name="lectcodi">Código de lectura</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerDatosEquipoLectura(string equicodi, int lectcodi, string fecha)
        {
            return FactorySic.GetMeMedicion48Repository().ObtenerDatosEquipoLectura(equicodi, lectcodi, fecha);
        }

        /// <summary>
        /// Permite obtener los datos de mediciones
        /// </summary>
        /// <param name="ptomedicodi">Código de punto de medición</param>
        /// <param name="lectcodi">Código de lectura</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerDatosPtoMedicionLectura(string ptomedicodi, int lectcodi, string fecha)
        {
            return FactorySic.GetMeMedicion48Repository().ObtenerDatosPtoMedicionLectura(ptomedicodi, lectcodi, fecha);
        }

        /// <summary>
        /// Listar las centrales por origlectcodi
        /// </summary>
        /// <param name="listaptos"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarCentralByOriglectcodiAndFormato(string origlectcodi, string famcodi, int formatcodi)
        {
            return FactorySic.GetMePtomedicionRepository().ListarCentralByOriglectcodiAndFormato(origlectcodi, famcodi, formatcodi);
        }

        /// <summary>
        /// Permite obtener el punto de medición
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public MePtomedicionDTO GetByIdPtoMedicion (int ptomedicodi)
        {
            return FactorySic.GetMePtomedicionRepository().GetById(ptomedicodi);
        }

        #endregion

        #region Métodos Tabla EQ_EQUICANAL

        /// <summary>
        /// Inserta un registro de la tabla EQ_EQUICANAL
        /// </summary>
        public void SaveEqEquicanal(EqEquicanalDTO entity)
        {
            try
            {
                FactorySic.GetEqEquicanalRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_EQUICANAL
        /// </summary>
        public void UpdateEqEquicanal(EqEquicanalDTO entity)
        {
            try
            {
                FactorySic.GetEqEquicanalRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_EQUICANAL
        /// </summary>
        public void DeleteEqEquicanal(int areacode, int canalcodi, int equicodi, int tipoinfocodi, string user)
        {
            try
            {
                FactorySic.GetEqEquicanalRepository().Delete(areacode, canalcodi, equicodi, tipoinfocodi);
                FactorySic.GetEqEquicanalRepository().Delete_UpdateAuditoria(areacode, canalcodi, equicodi, tipoinfocodi, user);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_EQUICANAL
        /// </summary>
        public EqEquicanalDTO GetByIdEqEquicanal(int areacode, int canalcodi, int equicodi, int tipoinfocodi)
        {
            EqEquicanalDTO reg = FactorySic.GetEqEquicanalRepository().GetById(areacode, canalcodi, equicodi, tipoinfocodi);

            this.SetEqEquicanalFormateado(reg);

            return reg;
        }


        /// <summary>
        /// Permite realizar listar las equivalencias en la tabla EqEquicanal
        /// </summary>
        public List<EqEquicanalDTO> ListarEquivalenciaEquipoCanal(int areacode, int empresa, int familia, int medida)
        {
            //lista BD SICOES
            List<EqEquicanalDTO> lista = FactorySic.GetEqEquicanalRepository().ListarEquivalencia(areacode, empresa, familia, medida);

            //lista BD TREAL
            List<TrCanalSp7DTO> listaCanal = (new ScadaSp7AppServicio()).ListTrCanalSp7sByIds("-1");

            foreach (var reg in lista)
            {
                var objCanal = listaCanal.Find(x => x.Canalcodi == reg.Canalcodi);
                if (objCanal != null)
                {
                    reg.Canalnomb = objCanal.Canalnomb;
                    reg.Canaliccp = objCanal.Canaliccp;
                    reg.Canalunidad = objCanal.Canalunidad;
                    reg.CanalPointType = objCanal.CanalPointType;
                    reg.Canalabrev = objCanal.Canalabrev;
                    reg.Zonacodi = objCanal.Zonacodi ?? 0;
                    reg.Zonanomb = objCanal.Zonanomb;
                    reg.Zonaabrev = objCanal.Zonaabrev;
                    reg.TrEmprcodi = objCanal.TrEmprcodi;
                    reg.TrEmprnomb = objCanal.TrEmprnomb;
                }

                this.SetEqEquicanalFormateado(reg);
            }

            return lista;
        }

        /// <summary>
        /// Formatear data
        /// </summary>
        /// <param name="reg"></param>
        private void SetEqEquicanalFormateado(EqEquicanalDTO reg)
        {
            if (reg != null)
            {
                reg.Emprnomb = reg.Emprnomb != null ? reg.Emprnomb.Trim() : string.Empty;
                reg.Equinomb = reg.Equinomb != null ? reg.Equinomb.Trim() : string.Empty;
                reg.Famabrev = reg.Famabrev != null ? reg.Famabrev.Trim() : string.Empty;
                reg.Canalabrev = reg.Canalabrev != null ? reg.Canalabrev.Trim() : string.Empty;
                reg.Canaliccp = reg.Canaliccp != null ? reg.Canaliccp.Trim() : string.Empty;
                reg.Canalnomb = reg.Canalnomb != null ? reg.Canalnomb.Trim() : string.Empty;

                reg.EquipoDesc = reg.Areadesc + " / " + reg.Equinomb;
                reg.CanalScadaDesc = reg.Canaliccp + " " + reg.Canalnomb;
                reg.Central = reg.Equipadre != 0 ? reg.Central.Trim() : string.Empty;

                reg.EcanestadoDesc = Util.EstadoDescripcion(reg.Ecanestado);
                reg.EcanfeccreacionDesc = reg.Ecanfeccreacion.HasValue ? reg.Ecanfeccreacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
                reg.EcanfecmodificacionDesc = reg.Ecanfecmodificacion.HasValue ? reg.Ecanfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;

                reg.EcanfactorDesc = reg.Ecanfactor > 0 ? ConstantesAppServicio.NODesc : ConstantesAppServicio.SIDesc;
            }
        }

        #endregion

        #region Métodos Tabla EQ_EQUIPO

        /// <summary>
        /// Permite obtener los equipos por familia
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerEquiposPorFamilia(int emprcodi, int famcodi)
        {
            List<EqEquipoDTO> list = FactorySic.GetEqEquipoRepository().ObtenerEquipoPorFamilia(emprcodi, famcodi);

            if (list.Count > 0)
            {
                int max = list.Select(x => x.AREANOMB.Length).Max();

                foreach (EqEquipoDTO item in list)
                {
                    int count = max - item.AREANOMB.Length;
                    string espacio = string.Empty;
                    for (int i = 0; i <= count; i++)
                    {
                        espacio = espacio + "-";
                    }


                    item.Equinomb = item.AREANOMB + espacio + " " + item.Equinomb;
                }

                return list.OrderBy(x => x.Equinomb).ToList();
            }

            return new List<EqEquipoDTO>();
        }

        #endregion

        #region Métodos Tabla ME_PTOMEDCANAL

        /// <summary>
        /// Inserta un registro de la tabla ME_PTOMEDCANAL
        /// </summary>
        public void SaveMePtomedcanal(MePtomedcanalDTO entity)
        {
            try
            {
                FactorySic.GetMePtomedcanalRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_PTOMEDCANAL
        /// </summary>
        public void UpdateMePtomedcanal(MePtomedcanalDTO entity)
        {
            try
            {
                FactorySic.GetMePtomedcanalRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_PTOMEDCANAL
        /// </summary>
        public void DeleteMePtomedcanal(int canalcodi, int ptomedicodi, int tipoinfocodi)
        {
            try
            {
                FactorySic.GetMePtomedcanalRepository().Delete(canalcodi, ptomedicodi, tipoinfocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_PTOMEDCANAL
        /// </summary>
        public MePtomedcanalDTO GetByIdMePtomedcanal(int canalcodi, int ptomedicodi, int tipoinfocodi)
        {
            MePtomedcanalDTO reg = FactorySic.GetMePtomedcanalRepository().GetById(canalcodi, ptomedicodi, tipoinfocodi);

            if (reg != null)
            {
                reg.Central = reg.Equipadre != 0 ? reg.Central.Trim() : string.Empty;
                reg.PcanestadoDesc = Util.EstadoDescripcion(reg.Pcanestado);
                reg.PuntoPR5 = reg.Ptomedicodi + " " + reg.Ptomedielenomb;
                reg.PuntoCanalScada = reg.Canalcodi + " " + reg.Canalabrev + " " + reg.Canalnomb;
                reg.PcanfeccreacionDesc = reg.Pcanfeccreacion.HasValue ? reg.Pcanfeccreacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
                reg.PcanfecmodificacionDesc = reg.Pcanfecmodificacion.HasValue ? reg.Pcanfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            }

            return reg;
        }

        #endregion

        #region Métodos SI_TIPOINFORMACION
        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TIPOINFORMACION
        /// </summary>
        public List<SiTipoinformacionDTO> ListSiTipoinformacions()
        {
            return FactorySic.GetSiTipoinformacionRepository().List().OrderBy(x => x.Tipoinfoabrev).ToList();
        }
        #endregion

        #region Equivalencia Puntos de Medicion - Canal Scada

        /// <summary>
        /// Obtener ListaEmpresaByOriglectcodi
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerListaEmpresaByOriglectcodi(string origlectcodi, string famcodi, int formatcodi)
        {
            List<SiEmpresaDTO> lista = this.ListarCentralByOriglectcodiAndFormato(origlectcodi, famcodi, formatcodi)
                .GroupBy(x => new { x.Emprcodi, x.Emprnomb })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi.Value, Emprnomb = x.Key.Emprnomb })
                .OrderBy(x => x.Emprnomb).ToList();
            return lista;
        }

        /// <summary>
        /// Obtener ListaEmpresaByOriglectcodi
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerListaCentralByOriglectcodiAndEmpresa(string origlectcodi, string famcodi, int empresa, int formatcodi)
        {
            List<EqEquipoDTO> lista = this.ListarCentralByOriglectcodiAndFormato(origlectcodi, famcodi, formatcodi).Where(x => x.Equipadre > 0 && (empresa == -1 || x.Emprcodi == empresa))
                .GroupBy(x => new { x.Equipadre, x.Central })
                .Select(x => new EqEquipoDTO() { Equicodi = x.Key.Equipadre, Equinomb = x.Key.Central })
                .OrderBy(x => x.Equinomb).ToList();
            return lista;
        }

        /// <summary>
        /// Obtener ListaPtomedicionByOriglectcodi
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ObtenerListaPtomedicionByOriglectcodi(string origlectcodi, string famcodi, int formatcodi, int idEmpresa)
        {
            List<MePtomedicionDTO> lista = this.ListarCentralByOriglectcodiAndFormato(origlectcodi, famcodi, formatcodi)
                .Where(x => x.Emprcodi == idEmpresa)
                .GroupBy(x => new { x.Ptomedicodi, x.Ptomedielenomb, x.Equinomb })
                .Select(x => new MePtomedicionDTO() { Ptomedicodi = x.Key.Ptomedicodi, Ptomedielenomb = x.Key.Ptomedielenomb, Equinomb = x.Key.Equinomb })
                .OrderBy(x => x.Emprnomb).ThenBy(x => x.Ptomedielenomb).ToList();
            return lista;
        }

        /// <summary>
        /// Permite realizar listar las equivalencias en la tabla MePtomedcanal
        /// </summary>
        public List<MePtomedcanalDTO> ListarEquivalenciaPtomedicionCanal(string empresa, int origlectcodi, int medida, string famcodis)
        {
            List<MePtomedcanalDTO> lista = FactorySic.GetMePtomedcanalRepository().ListarEquivalencia(empresa, origlectcodi, medida, famcodis);

            //lista BD TREAL
            string canalcodis = string.Join(",", lista.Select(x => x.Canalcodi).Distinct().ToList());
            List<TrCanalSp7DTO> listaCanal = (new ScadaSp7AppServicio()).ListTrCanalSp7sByIds(canalcodis);

            foreach (var reg in lista)
            {
                var objCanal = listaCanal.Find(x => x.Canalcodi == reg.Canalcodi);
                if (objCanal != null)
                {
                    reg.Canalnomb = objCanal.Canalnomb;
                    reg.Canaliccp = objCanal.Canaliccp;
                    reg.Canalunidad = objCanal.Canalunidad;
                    reg.CanalPointType = objCanal.CanalPointType;
                    reg.Canalabrev = objCanal.Canalabrev;
                    reg.Zonacodi = objCanal.Zonacodi ?? 0;
                    reg.Zonanomb = objCanal.Zonanomb;
                    reg.Zonaabrev = objCanal.Zonaabrev;
                    reg.TrEmprcodi = objCanal.TrEmprcodi;
                    reg.TrEmprnomb = objCanal.TrEmprnomb;
                    reg.TrEmprabrev = objCanal.TrEmprabrev;
                }

                //formatear campos scada
                reg.Canalnomb = (reg.Canalnomb ??"").Trim();
                reg.Canaliccp = (reg.Canaliccp ?? "").Trim();
                reg.Canalabrev = (reg.Canalabrev ?? "").Trim();
                reg.Canalunidad = (reg.Canalunidad ?? "").Trim();
                reg.CanalPointType = (reg.CanalPointType ?? "").Trim();
                reg.Zonanomb = (reg.Zonanomb ?? "").Trim();
                reg.Zonaabrev = (reg.Zonaabrev ?? "").Trim();
                reg.TrEmprnomb = (reg.TrEmprnomb ?? "").Trim();
                reg.TrEmprabrev = (reg.TrEmprabrev ?? "").Trim();

                //formatear datos sicoes
                reg.Emprnomb = (reg.Emprnomb ?? "").Trim();
                reg.Equinomb = (reg.Equinomb ?? "").Trim();
                reg.Famabrev = (reg.Famabrev ?? "").Trim();

                reg.Central = (reg.Central ?? "").Trim();
                reg.PcanestadoDesc = Util.EstadoDescripcion(reg.Pcanestado);
                reg.PuntoPR5 = reg.Ptomedicodi + " " + reg.Ptomedielenomb;
                reg.PuntoCanalScada = reg.Canalcodi + " " + reg.Canalabrev + " " + reg.Canalnomb;
                reg.PcanfactorDesc = reg.Pcanfactor == -1 ? "SÍ" : "NO";
            }

            return lista
                .OrderBy(x => x.Origlectnombre).ThenBy(x => x.Emprnomb).ThenBy(x => x.Famabrev).ThenBy(x => x.Equinomb)
                .ThenBy(x => x.Tipoinfocodi).ThenBy(x => x.PuntoPR5).ThenBy(x => x.PuntoCanalScada).ToList();
        }

        /// <summary>
        /// Reporte de equivalencias HTML
        /// </summary>
        /// <returns></returns>
        public string ReporteEquivalenciaPtomedicionCanalHtml(int idEmpresa, int idCentral, int medida, string url)
        {
            StringBuilder str = new StringBuilder();
            List<MePtomedcanalDTO> listaData = this.ListarEquivalenciaPtomedicionCanal(idEmpresa.ToString(), idCentral, medida, ConstantesAppServicio.ParametroDefecto);

            str.Append("<div id='resultado' style='height: auto;'>");
            str.Append("<table id='reporte' class='pretty tabla-icono' style='width: 100%' >");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 80px'></th>");
            str.Append("<th style=''>Empresa</th>");
            str.Append("<th style=''>Tipo de Equipo</th>");
            str.Append("<th style=''>Equipo</th>");
            str.Append("<th style='width: 70px'>Unidad</th>");
            str.Append("<th style='background-color: #61c13a;width: 500px'>PUNTOS </th>");
            str.Append("<th style='background-color: #FFBF00;width: 500px'>CANAL SCADA</th>");
            str.Append("<th style='width: 70px'>Valor Inverso</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var puntoMedicion in listaData)
            {
                str.Append("<tr>");

                str.AppendFormat("<td style='width: 80px;'>");
                str.AppendFormat("<a href='JavaScript:verEquivalencia({0},{1},{2})'><img src='" + url + "Content/Images/btn-open.png' alt='Ver equivalencia' /></a>", puntoMedicion.Canalcodi, puntoMedicion.Ptomedicodi, puntoMedicion.Tipoinfocodi);
                str.AppendFormat("<a href='JavaScript:editarEquivalencia({0},{1},{2})'><img src='" + url + "Content/Images/btn-edit.png' alt='Editar equivalencia' /></a>", puntoMedicion.Canalcodi, puntoMedicion.Ptomedicodi, puntoMedicion.Tipoinfocodi);
                str.AppendFormat("<a href='JavaScript:eliminarEquivalencia({0},{1},{2})'><img src='" + url + "Content/Images/btn-cancel.png' alt='Eliminar equivalencia' /></a>", puntoMedicion.Canalcodi, puntoMedicion.Ptomedicodi, puntoMedicion.Tipoinfocodi);
                str.AppendFormat("</td>");
                str.AppendFormat("<td>{0}</td>", puntoMedicion.Emprnomb);
                str.AppendFormat("<td>{0}</td>", puntoMedicion.Famabrev);
                str.AppendFormat("<td>{0}</td>", (puntoMedicion.Central == "" ? "" : puntoMedicion.Central + " / ") + puntoMedicion.Equiabrev);
                str.AppendFormat("<td>{0}</td>", puntoMedicion.Tipoinfoabrev);
                str.AppendFormat("<td>{0}</td>", puntoMedicion.PuntoPR5);
                str.AppendFormat("<td>{0}</td>", puntoMedicion.PuntoCanalScada);
                str.AppendFormat("<td>{0}</td>", puntoMedicion.PcanfactorDesc);

                str.Append("</tr>");
            }
            #endregion
            str.Append("</tbody>");

            str.Append("</table>");
            str.Append("</div>");

            return str.ToString();
        }

        /// <summary>
        /// Listar las unidades de si_tipoinformacion y canal scada
        /// </summary>
        /// <returns></returns>
        public List<SiTipoinformacionDTO> ListaUnidadTr()
        {
            List<SiTipoinformacionDTO> lista = this.ListSiTipoinformacions();

            return lista.Where(x => !string.IsNullOrEmpty(x.Canalunidad)).ToList();
        }

        /// <summary>
        /// Obtener tipo informacion por descripcion de Tr unidad
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public SiTipoinformacionDTO GetTipoinformacionByTrUnidad(string unidad)
        {
            return this.ListaUnidadTr().Find(x => x.Canalunidad == unidad);
        }

        /// <summary>
        /// Obtener tipo informacion por descripcion de Tr unidad
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public SiTipoinformacionDTO GetTipoinformacionByTipoinfocodi(int tipoinfocodi)
        {
            return this.ListaUnidadTr().Find(x => x.Tipoinfocodi == tipoinfocodi);
        }

        public List<TrCanalSp7DTO> ListarUnidadPorZona(int zonacodi)
        {
            var listaUnidadScada = (new ScadaSp7AppServicio()).ListarUnidadPorZona(zonacodi);
            var listaUnidadSic = this.ListaUnidadTr();

            foreach (var item in listaUnidadScada)
            {
                var objTipoSic = listaUnidadSic.Find(x => x.Tinfcanalunidad == item.Canalunidad);
                if (objTipoSic != null)
                {
                    item.Tipoinfocodi = objTipoSic.Tipoinfocodi;
                    item.Tipoinfoabrev = objTipoSic.Tipoinfoabrev;
                    item.Canalunidad += string.Format(" ({0})", objTipoSic.Tipoinfoabrev);
                }
            }

            return listaUnidadScada.Where(x => x.Tipoinfocodi > 0).OrderBy(x => x.Canalunidad).ToList();
        }

        /// <summary>
        /// Genera el reporte Histórico de Quema de Gas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaPlantilla"></param>
        /// <param name="rutaNombreArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GenerarArchivoExcelEquivalencia(List<MePtomedcanalDTO> listaReporte, string rutaNombreArchivo, string rutaLogo)
        {
            FileInfo newFile = new FileInfo(rutaNombreArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                string titulo = "Equivalencia de Puntos de medición y Canal Scada";
                string sheetName = "EQUIVALENCIA";
                ws = xlPackage.Workbook.Worksheets.Add(sheetName);
                ws = xlPackage.Workbook.Worksheets[sheetName];

                ConfiguraEncabezadoHojaExcel(ws, titulo, rutaLogo);
                ConfiguracionHojaExcelEquivalencia(ws, listaReporte);
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Configura encabezado de Reporte Hoja Excel 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        public void ConfiguraEncabezadoHojaExcel(ExcelWorksheet ws, string titulo, string rutaLogo)
        {
            //Logo
            UtilExcel.AddImageLocal(ws, 1, 0, rutaLogo);

            ws.Cells[1, 3].Value = titulo;
            var font = ws.Cells[1, 3].Style.Font;
            font.Size = 16;
            font.Bold = true;
            font.Name = "Calibri";
            //Borde, font cabecera de Tabla Fecha
            // var borderFecha = ws.Cells[3, 2, 4, 3].Style.Border;
            // borderFecha.Bottom.Style = borderFecha.Top.Style = borderFecha.Left.Style = borderFecha.Right.Style = ExcelBorderStyle.Thin;
            //var fontTabla = ws.Cells[3, 2, 4, 3].Style.Font;
            //fontTabla.Size = 8;
            //fontTabla.Name = "Calibri";
            //fontTabla.Bold = true;          
        }

        /// <summary>
        /// Muestra datos de consulta para el listado Equivalencia de Puntos de medicion y canal scada
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaReporte"></param>
        public static void ConfiguracionHojaExcelEquivalencia(ExcelWorksheet ws, List<MePtomedcanalDTO> listaReporte)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            int nfil = listaReporte.Count;
            int xFil = 4;
            int ncol = 9;

            ws.View.ShowGridLines = false;

            ws.Cells[xFil, 2].Value = "EMPRESA";
            ws.Cells[xFil, 3].Value = "TIPO DE EQUIPO";
            ws.Cells[xFil, 4].Value = "EQUIPO";
            ws.Cells[xFil, 5].Value = "ORIGEN";
            ws.Cells[xFil, 6].Value = "UNIDAD";
            ws.Cells[xFil, 7].Value = "PUNTOS";
            ws.Cells[xFil, 8].Value = "CANAL SCADA";
            ws.Cells[xFil, 9].Value = "VALOR INVERSO";
            ws.Cells[xFil, 10].Value = "ESTADO";

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 35;
            ws.Column(3).Width = 35;
            ws.Column(4).Width = 35;
            ws.Column(5).Width = 30;
            ws.Column(6).Width = 9;
            ws.Column(7).Width = 40;
            ws.Column(8).Width = 50;
            ws.Column(9).Width = 12;
            ws.Column(10).Width = 15;

            using (var range = ws.Cells[xFil, 2, xFil, 10])
            {
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Font.Color.SetColor(Color.White);
            }

            ws.Cells[xFil, 2].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));
            ws.Cells[xFil, 3].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));
            ws.Cells[xFil, 4].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));
            ws.Cells[xFil, 5].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));
            ws.Cells[xFil, 6].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));
            ws.Cells[xFil, 7].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#61c13a"));
            ws.Cells[xFil, 8].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFBF00"));
            ws.Cells[xFil, 9].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));
            ws.Cells[xFil, 10].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));

            //****************************************
            var i = 1;

            foreach (var obj in listaReporte)
            {
                ws.Cells[xFil + i, 2].Value = obj.Emprnomb;
                ws.Cells[xFil + i, 3].Value = obj.Famnomb;
                ws.Cells[xFil + i, 4].Value = (obj.Central == "" ? "" : obj.Central + " / ") + obj.Equinomb;
                ws.Cells[xFil + i, 5].Value = obj.Origlectnombre;
                ws.Cells[xFil + i, 6].Value = obj.Tipoinfoabrev;
                ws.Cells[xFil + i, 7].Value = obj.PuntoPR5;
                ws.Cells[xFil + i, 8].Value = obj.PuntoCanalScada;
                ws.Cells[xFil + i, 9].Value = obj.PcanfactorDesc;
                ws.Cells[xFil + i, 10].Value = obj.PcanestadoDesc;
                xFil++;
            }

            ////////////// Formato de Celdas Valores            

            var fontTabla = ws.Cells[4, 2, 4 + nfil, 1 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[4, 2, 4 + nfil, 1 + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            using (var range = ws.Cells[5, 2, 5 + nfil, 1 + ncol])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            using (var range = ws.Cells[5, 7, 5 + nfil, 9])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }
        }

        /// <summary>
        /// Convierte numero a string con formato
        /// </summary>
        /// <returns></returns>
        public static NumberFormatInfo GenerarNumberFormatInfo()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        #endregion

        #region Equivalencia de Equipos y Scada

        /// <summary>
        /// Reporte de equivalencias HTML
        /// </summary>
        /// <returns></returns>
        public string ReporteEquivalenciaEquipoCanalHtml(int areacode, int idEmpresa, int idFamilia, int medida, string url)
        {
            StringBuilder str = new StringBuilder();
            List<EqEquicanalDTO> listaData = this.ListarEquivalenciaEquipoCanal(areacode, idEmpresa, idFamilia, medida);

            str.Append("<div id='resultado' style='height: auto;'>");
            str.Append("<table id='reporte' class='pretty tabla-icono' style='width: 1500px;' >");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 250px'>Empresa</th>");
            str.Append("<th style='width: 50px'>Teq</th>");
            str.Append("<th style='background-color: #61c13a;'>Código</th>");
            str.Append("<th style='background-color: #61c13a;'>EQUIPO</th>");
            str.Append("<th style='background-color: #FFBF00;width: 70px'>Código</th>");
            str.Append("<th style='background-color: #FFBF00;'>NOMBRE CANAL SCADA</th>");
            str.Append("<th style='background-color: #FFBF00;'>ICCP CANAL SCADA</th>");
            str.Append("<th style='width: 70px'>Unidad</th>");
            str.Append("<th style='width: 70px'>Valor Inverso</th>");
            str.Append("<th style='width: 75px'>Estado</th>");
            str.Append("<th style='width: 75px'>Area</th>");
            str.Append("<th style='width: 75px'></th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var relacion in listaData)
            {

                str.Append("<tr>");

                str.AppendFormat("<td>{0}</td>", relacion.Emprnomb);
                str.AppendFormat("<td>{0}</td>", relacion.Famabrev);

                str.AppendFormat("<td>{0}</td>", relacion.Equicodi);
                str.AppendFormat("<td style='text-align: left;'>{0}</td>", relacion.EquipoDesc);

                str.AppendFormat("<td>{0}</td>", relacion.Canalcodi);
                str.AppendFormat("<td style='text-align: left;'>{0}</td>", relacion.Canalnomb);
                str.AppendFormat("<td style='text-align: left;'>{0}</td>", relacion.Canaliccp);

                str.AppendFormat("<td>{0}</td>", relacion.Tipoinfoabrev);
                str.AppendFormat("<td>{0}</td>", relacion.EcanfactorDesc);
                str.AppendFormat("<td>{0}</td>", relacion.EcanestadoDesc);
                str.AppendFormat("<td>{0}</td>", relacion.Areaabrev);

                str.AppendFormat("<td>");
                str.AppendFormat("<a href='JavaScript:verEquivalencia({0},{1},{2},{3})'><img src='" + url + "Content/Images/btn-open.png' alt='Ver equivalencia' /></a>", relacion.Canalcodi, relacion.Equicodi, relacion.Tipoinfocodi, relacion.Areacode);
                str.AppendFormat("<a href='JavaScript:editarEquivalencia({0},{1},{2},{3})'><img src='" + url + "Content/Images/btn-edit.png' alt='Editar equivalencia' /></a>", relacion.Canalcodi, relacion.Equicodi, relacion.Tipoinfocodi, relacion.Areacode);
                str.AppendFormat("<a href='JavaScript:eliminarEquivalencia({0},{1},{2},{3})'><img src='" + url + "Content/Images/btn-cancel.png' alt='Eliminar equivalencia' /></a>", relacion.Canalcodi, relacion.Equicodi, relacion.Tipoinfocodi, relacion.Areacode);
                str.AppendFormat("</td>");

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");
            str.Append("</div>");

            return str.ToString();
        }

        /// <summary>
        /// Genera el reporte Histórico de Quema de Gas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaPlantilla"></param>
        /// <param name="rutaNombreArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GenerarArchivoExcelEquivalenciaEquipoCanal(List<EqEquicanalDTO> listaReporte, string rutaNombreArchivo, string rutaLogo)
        {
            FileInfo newFile = new FileInfo(rutaNombreArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                string titulo = "Equivalencia de Equipos y Canal Scada";
                string sheetName = "EQUIVALENCIA";
                ws = xlPackage.Workbook.Worksheets.Add(sheetName);
                ws = xlPackage.Workbook.Worksheets[sheetName];

                ConfiguraEncabezadoHojaExcel(ws, titulo, rutaLogo);
                ConfiguracionHojaExcelEquivalenciaEquipoCanal(ws, listaReporte);
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Muestra datos de consulta para el listado Equivalencia de Equipo y canal scada
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaReporte"></param>
        public static void ConfiguracionHojaExcelEquivalenciaEquipoCanal(ExcelWorksheet ws, List<EqEquicanalDTO> listaReporte)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            int nfil = listaReporte.Count;
            int xFil = 4;
            int ncol = 10;

            ws.View.ShowGridLines = false;

            ws.Cells[xFil, 2].Value = "EMPRESA";
            ws.Cells[xFil, 3].Value = "Teq";
            ws.Cells[xFil, 4].Value = "CÓDIGO";
            ws.Cells[xFil, 5].Value = "EQUIPO";
            ws.Cells[xFil, 6].Value = "CÓDIGO";
            ws.Cells[xFil, 7].Value = "CANAL SCADA";
            ws.Cells[xFil, 8].Value = "UNIDAD";
            ws.Cells[xFil, 9].Value = "VALOR INVERSO";
            ws.Cells[xFil, 10].Value = "ESTADO";
            ws.Cells[xFil, 11].Value = "AREA";

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 35;
            ws.Column(3).Width = 10;
            ws.Column(4).Width = 10;
            ws.Column(5).Width = 27;
            ws.Column(6).Width = 10;
            ws.Column(7).Width = 50;
            ws.Column(8).Width = 11;
            ws.Column(9).Width = 11;
            ws.Column(10).Width = 11;
            ws.Column(11).Width = 11;

            using (var range = ws.Cells[xFil, 2, xFil, 11])
            {
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Font.Color.SetColor(Color.White);
            }

            ws.Cells[xFil, 2].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));
            ws.Cells[xFil, 3].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));
            ws.Cells[xFil, 4].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#61c13a"));
            ws.Cells[xFil, 5].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#61c13a"));
            ws.Cells[xFil, 6].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFBF00"));
            ws.Cells[xFil, 7].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFBF00"));
            ws.Cells[xFil, 8].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));
            ws.Cells[xFil, 9].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));
            ws.Cells[xFil, 10].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));
            ws.Cells[xFil, 11].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0d8bde"));

            //****************************************
            var i = 1;

            foreach (var obj in listaReporte)
            {
                ws.Cells[xFil + i, 2].Value = obj.Emprnomb;
                ws.Cells[xFil + i, 3].Value = obj.Famabrev;
                ws.Cells[xFil + i, 4].Value = obj.Equicodi;
                ws.Cells[xFil + i, 5].Value = obj.EquipoDesc;
                ws.Cells[xFil + i, 6].Value = obj.Canalcodi;
                ws.Cells[xFil + i, 7].Value = obj.CanalScadaDesc;
                ws.Cells[xFil + i, 8].Value = obj.Tipoinfoabrev;
                ws.Cells[xFil + i, 9].Value = obj.EcanfactorDesc;
                ws.Cells[xFil + i, 10].Value = obj.EcanestadoDesc;
                ws.Cells[xFil + i, 11].Value = obj.Areaabrev;
                xFil++;
            }

            ////////////// Formato de Celdas Valores            

            var fontTabla = ws.Cells[4, 2, 4 + nfil, 1 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[4, 2, 4 + nfil, 1 + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            using (var range = ws.Cells[5, 2, 5 + nfil, 1 + ncol])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            using (var range = ws.Cells[5, 5, 5 + nfil, 5])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }
            using (var range = ws.Cells[5, 7, 5 + nfil, 7])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }
        }

        #endregion

        #region Mejoras IEOD

        /// <summary>
        /// Obtener ListaEmpresaByOriglectcodi
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerListaEmpresaByOriglectcodi(int origlectcodi)
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresaPorOrigenPtoMedicion(origlectcodi);
        }

        /// <summary>
        /// Lista Familia By Origen Lectura y Empresa
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqFamiliaDTO> ObtenerFamiliaPorOrigenLecturaEquipo(int origlectcodi, int emprcodi)
        {
            return FactorySic.GetEqFamiliaRepository().ListarFamiliaPorOrigenLecturaEquipo(origlectcodi, emprcodi);
        }

        /// <summary>
        /// Obtener ListaPtomedicionByOriglectcodi
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ObtenerListaPtomedicionByOriglectcodiEmpresa(string origlectcodi, string famcodi, int emprcodi)
        {
            List<MePtomedicionDTO> lista = FactorySic.GetMePtomedicionRepository().ListarPtomedicionByOriglectcodi(origlectcodi, famcodi, emprcodi)
                .GroupBy(x => new { x.Ptomedicodi, x.Ptomedielenomb, x.Equinomb })
                .Select(x => new MePtomedicionDTO() { Ptomedicodi = x.Key.Ptomedicodi, Ptomedielenomb = x.Key.Ptomedielenomb, Equinomb = x.Key.Equinomb })
                .OrderBy(x => x.Emprnomb).ThenBy(x => x.Ptomedielenomb).ThenBy(x => x.Ptomedicodi).ToList();

            return lista;
        }

        #endregion
    }
}
