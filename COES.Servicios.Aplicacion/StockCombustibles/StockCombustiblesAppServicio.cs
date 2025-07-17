using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.StockCombustibles
{
    public class StockCombustiblesAppServicio
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StockCombustiblesAppServicio));
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();


        #region Métodos Tabla SI_EMPRESA

        /// <summary> 
        /// Obtiene las empresas por usuario
        /// </summary>
        /// <param name="userlogin"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasPorUsuario(string userlogin)
        {
            return FactorySic.GetSiEmpresaRepository().GetByUser(userlogin);
        }
        /// <summary>
        /// Obtiene empresas del sein
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasSEIN()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
        }

        /// <summary>
        /// Obtiene la lista de empresas por combustible
        /// </summary>
        /// <param name="tipocombustible"></param>
        /// <returns></re
        /// O:8ns>
        public List<SiEmpresaDTO> ObtenerEmpresasxCombustible(string tipocombustible)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasxCombustible(tipocombustible);
        }
        /// <summary>
        /// Obtiene las empresas por tipo de combustible y por Usuario
        /// </summary>
        /// <param name="tipocombustible"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasxCombustiblexUsuario(string tipocombustible, string usuario)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasxCombustiblexUsuario(tipocombustible, usuario);
        }

        /// <summary>
        /// Obtiene listado de las empresas de tipo Generadoras
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerListaEmpresasGeneradoras()
        {
            return FactorySic.GetSiEmpresaRepository().ListEmpresasGeneradoras();
        }

        #endregion

        #region Métodos Tabla ME_MEDICION1

        /// <summary>
        /// Inserta un registro de la tabla ME_MEDICION1
        /// </summary>
        public void SaveMeMedicion1(MeMedicion1DTO entity)
        {
            try
            {
                FactorySic.GetMeMedicion1Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_MEDICION1
        /// </summary>
        public void DeleteMeMedicion1(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi)
        {
            try
            {
                FactorySic.GetMeMedicion1Repository().Delete(lectcodi, medifecha, tipoinfocodi, ptomedicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_MEDICION1
        /// </summary>
        public MeMedicion1DTO GetByIdMeMedicion1(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi)
        {
            return FactorySic.GetMeMedicion1Repository().GetById(lectcodi, medifecha, tipoinfocodi, ptomedicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_MEDICION1
        /// </summary>
        public List<MeMedicion1DTO> ListMeMedicion1s()
        {
            return FactorySic.GetMeMedicion1Repository().List();
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados1(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion1Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
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
        public void GrabarValoresCargados11(List<MeMedicion1DTO> entitys)
        {
            try
            {
                foreach (MeMedicion1DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion1Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //public List<MeMedicion1DTO> ListaMed1Hidrologia(int lectocodi, int origlect, string idsEmpresa, string idsCuenca, string idsFamilia,
        //    DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        //{

        //    return FactorySic.GetMeMedicion1Repository().GetHidrologia(lectocodi, origlect, idsEmpresa, idsCuenca, idsFamilia,
        //        fechaInicio, fechaFin, idsPtoMedicion);
        //}

        /// <summary>
        /// Se obtiene el los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> GetDataFormato(int idEmpresa, MeFormatoDTO formato, DateTime fecha)
        {
            List<MeMedicion1DTO> lista1 = new List<MeMedicion1DTO>();
            lista1 = FactorySic.GetMeMedicion1Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, fecha.AddDays(-1), fecha.AddDays(-1));
            return lista1;
        }

        /// <summary>
        /// Se obtiene los registros de la tabla Me_medicion1 de un rango de  fechas y por tipo de lectura
        /// </summary>
        /// <param name="lectCodiRecepcion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> GetListaMedicion1(int lectCodiRecepcion, DateTime fechaInicial, DateTime fechaFinal)
        {
            List<MeMedicion1DTO> lista = new List<MeMedicion1DTO>();
            lista = FactorySic.GetMeMedicion1Repository().GetListaMedicion1(lectCodiRecepcion, fechaInicial, fechaFinal);
            return lista;
        }

        public void BorrarValidacionEmpresa(int formatcodi, int emprcodi)
        {
            FactorySic.GetMeValidacionRepository().DeleteAllEmpresa(formatcodi, emprcodi);
        }


        #endregion

        #region Métodos Tabla ME_MEDICION24

        /// <summary>
        /// Genera el view con los datos del reporte Presión de Gas ó Temperatura Ambiente de las centrales termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="idParametro"></param>
        /// <returns></returns>
        public string GeneraViewReportePresionGasTemperatura(List<MeMedicion24DTO> listaReporte, DateTime fechaInicio, int idParametro)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();

            List<MeMedicion24DTO> listaCabeceraM24 = new List<MeMedicion24DTO>();

            if (idParametro == 1) // Presión de Gas
            {
                listaCabeceraM24 = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb, x.Ptomedielenomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Equinomb = y.Key.Equinomb,
                                     Ptomedielenomb = y.Key.Ptomedielenomb,
                                 }
                                 ).ToList();
            }
            else //Temperatura Ambiente
            {
                listaCabeceraM24 = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Equinomb = y.Key.Equinomb,
                                 }
                                 ).ToList();
            }
            int nCol = listaCabeceraM24.Count;
            strHtml.Append("<table  class='pretty tabla-icono' id='tabla'>");

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            if (idParametro == 1) //Presión de Gas
            {
                strHtml.Append("<div class='title-seccion'>HISTÓRICO PRESIÓN DE GAS</div>");
            }
            else //Temperatura Ambiente 
                strHtml.Append("<div class='title-seccion'>HISTÓRICO TEMPERATURA AMBIENTE(ºC)</div>");
            strHtml.Append("</tr>");
            if (listaReporte.Count > 0)
                strHtml.Append("<tr ><th rowspan = '3' width=100px>HORA</th>");
            else
                strHtml.Append("<tr ><th width=100px >HORA</th>");
            if (idParametro == 1) //Presión de Gas
            {
                strHtml.Append("<th colspan = '" + nCol + "'>PRESION GAS (kPa</th>");
            }
            else //Temperatura Ambiente
                strHtml.Append("<th colspan = '" + nCol + "'>TEMPERATURA AMBIENTE(ºC)</th>");

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            foreach (var reg in listaCabeceraM24)
            {
                strHtml.Append(string.Format("<th style='width:100px;'>{0}</th>", reg.Emprnomb));
            }
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            foreach (var reg in listaCabeceraM24)
            {
                if (idParametro == 1)//Presión de Gas
                {
                    strHtml.Append(string.Format("<th style='width:100px;'>{0}</th>", reg.Ptomedielenomb));
                }
                else //Temperatura Ambiente
                {
                    strHtml.Append(string.Format("<th style='width:100px;'>{0}</th>", reg.Equinomb));
                }
            }
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            //*********************************************** fin de encabezado *********************************************
            strHtml.Append("<tbody>");


            int resolucion = 60;
            int nBloques = 24;
            if (listaReporte.Count > 0)
            {
                int i = 0;
                for (int k = 1; k <= nBloques; k++)
                {
                    var fechaMin = ((fechaInicio.AddMinutes((k - 1) * resolucion + i * 60 * 24))).ToString(ConstantesStockCombustibles.FormatoFechaHora);
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td>{0}</td>", fechaMin));
                    foreach (var p in listaReporte)
                    {
                        decimal? valor;
                        valor = (decimal?)p.GetType().GetProperty("H" + k).GetValue(p, null);
                        if (valor != null)
                            strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                        else
                            strHtml.Append(string.Format("<td>--</td>"));
                    }

                    strHtml.Append("</tr>");
                }
            }
            else
            {
                strHtml.Append("<tr><td  colspan='2' style='text-align:center'>No existen registros.</td></tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }


        /// <summary>
        /// Se obtiene los registros de la tabla ME_MEDICION24 de un rango de fechas y por tipo de lectura presion de gas natural
        /// </summary>
        /// <param name="lectCodi"></param>
        /// <param name="origlectcodi"></param>
        /// <param name="sEmprCodi"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> ListaM24PresionGas(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, int tipotomedicodiPresionGas, int tipoinfocodiPresion, string strCentralInt)
        {
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            if (string.IsNullOrEmpty(sEmprCodi)) sEmprCodi = ConstantesAppServicio.ParametroDefecto;
            //if (string.IsNullOrEmpty(idsRecurso)) idsRecurso = ConstantesAppServicio.ParametroDefecto;D:\Solucion\FrameworkCOES\ProduccionBranch\COES.MVC.Intranet\Areas\StockCombustibles\Controllers\ReportesController.cs

            lista = FactorySic.GetMeMedicion24Repository().GetLista24PresionGas(lectCodi, origlectcodi, sEmprCodi, fechaInicial, fechaFinal, tipotomedicodiPresionGas, tipoinfocodiPresion, strCentralInt);

            foreach (var reg in lista)
            {
                reg.Equipadre = (reg.Famcodi == ConstantesMedidores.CentralHidraulica || reg.Famcodi == ConstantesMedidores.CentralTermica || reg.Famcodi == ConstantesMedidores.CentralSolar || reg.Famcodi == ConstantesMedidores.CentralEolica) ? reg.Equipadre : reg.Equicodi;
                reg.Equipopadre = (reg.Famcodi == ConstantesMedidores.CentralHidraulica || reg.Famcodi == ConstantesMedidores.CentralTermica || reg.Famcodi == ConstantesMedidores.CentralSolar || reg.Famcodi == ConstantesMedidores.CentralEolica) ? reg.Equipopadre : reg.Equinomb;
            }

            return lista;
        }

        /// <summary>
        /// Obtiene listado de registro de Temperatura Ambiente
        /// </summary>
        /// <param name="lectCodi"></param>
        /// <param name="origlectcodi"></param>
        /// <param name="sEmprCodi"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="strCentralInt"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> ListaM24TemperaturaAmbiente(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string strCentralInt)
        {
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            if (string.IsNullOrEmpty(sEmprCodi)) sEmprCodi = ConstantesAppServicio.ParametroDefecto;
            lista = FactorySic.GetMeMedicion24Repository().GetLista24TemperaturaAmbiente(lectCodi, origlectcodi, sEmprCodi, fechaInicial, fechaFinal, strCentralInt);

            foreach (var reg in lista)
            {
                reg.Equipadre = (reg.Famcodi == ConstantesMedidores.CentralHidraulica || reg.Famcodi == ConstantesMedidores.CentralTermica || reg.Famcodi == ConstantesMedidores.CentralSolar || reg.Famcodi == ConstantesMedidores.CentralEolica) ? reg.Equipadre : reg.Equicodi;
                reg.Equipopadre = (reg.Famcodi == ConstantesMedidores.CentralHidraulica || reg.Famcodi == ConstantesMedidores.CentralTermica || reg.Famcodi == ConstantesMedidores.CentralSolar || reg.Famcodi == ConstantesMedidores.CentralEolica) ? reg.Equipopadre : reg.Equinomb;
            }

            return lista;
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>

        public void GrabarValoresCargados24(List<MeMedicion24DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            MeValidacionDTO validacion = new MeValidacionDTO();
            validacion.Emprcodi = idEmpresa;
            validacion.Formatcodi = formato.Formatcodi;
            validacion.Validfechaperiodo = formato.FechaProceso;
            validacion.Validestado = ConstantesStockCombustibles.NoValidado;
            validacion.Validusumodificacion = usuario;
            validacion.Validfecmodificacion = DateTime.Now;
            try
            {
                var findValicion = servFormato.GetByIdMeValidacion(formato.Formatcodi, idEmpresa, formato.FechaProceso);
                if (findValicion != null)
                {
                    servFormato.UpdateMeValidacion(validacion);
                }
                else
                {
                    servFormato.SaveMeValidacion(validacion);
                }
                //Traer Ultimos Valores
                var lista = servFormato.GetDataFormato24(idEmpresa, formato, 0, 0);
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
                            if (servFormato.ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                            {
                                int idEnvioPrevio = 0;
                                var listAux = servFormato.GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaProceso);
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
                        servFormato.GrabarCambios(listaCambio);
                        servFormato.GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                //Eliminar Valores Previos
                servFormato.EliminarValoresCargados24((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin.AddDays(1));
                foreach (MeMedicion24DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion24Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region Métodos Tabla ME_TIPOPUNTOMEDICION

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeTipopuntomedicion
        /// </summary>
        public List<MeTipopuntomedicionDTO> ListaMeTipoPuntoMedicion(string StrTptoMedicodi, string idsestado)
        {
            List<MeTipopuntomedicionDTO> entitys = new List<MeTipopuntomedicionDTO>();
            try
            {
                entitys = FactorySic.GetMeTipopuntomedicionRepository().ListarMeTipoPuntoMedicion(StrTptoMedicodi, idsestado);
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }

        #endregion

        #region Métodos Tabla ME_MEDICIONXINTERVALO

        /// <summary>
        /// Graba Envio de Disponibilidad.
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarDisponibilidadGas(List<MeMedicionxintervaloDTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            int idEnvioAnt = 0;
            var fechaIni = formato.FechaProceso;
            var fechaFin = formato.FechaProceso.AddDays(1).AddSeconds(-1);
            MeValidacionDTO validacion = new MeValidacionDTO();
            validacion.Emprcodi = idEmpresa;
            validacion.Formatcodi = formato.Formatcodi;
            validacion.Validfechaperiodo = formato.FechaProceso;
            validacion.Validestado = ConstantesStockCombustibles.NoValidado;
            validacion.Validusumodificacion = usuario;
            validacion.Validfecmodificacion = DateTime.Now;
            try
            {
                var findValicion = servFormato.GetByIdMeValidacion(formato.Formatcodi, idEmpresa, formato.FechaProceso);
                if (findValicion != null)
                {
                    servFormato.UpdateMeValidacion(validacion);
                }
                else
                {
                    servFormato.SaveMeValidacion(validacion);
                }
                //Traer Ultimos Valores
                var lista = servFormato.GetEnvioMedicionXIntervalo(formato.Formatcodi, idEmpresa, fechaIni, fechaFin);
                var listaCambio = new List<MeCambioenvioDTO>();
                idEnvioAnt = servFormato.ObtenerIdMaxEnvioFormatoPeriodo(formato.Formatcodi, idEmpresa, formato.FechaProceso);

                if (idEnvioAnt > 0)
                {
                    foreach (var reg in lista)
                    {
                        MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                        cambio.Cambenvdatos = reg.Medinth1.ToString() + "#" + reg.Medintdescrip + "#" + reg.Medestcodi;
                        cambio.Cambenvcolvar = "1";
                        cambio.Cambenvfecha = (DateTime)reg.Medintfechaini;
                        cambio.Enviocodi = idEnvioAnt;
                        cambio.Formatcodi = formato.Formatcodi;
                        cambio.Ptomedicodi = reg.Ptomedicodi;
                        cambio.Tipoinfocodi = reg.Tipoinfocodi;
                        cambio.Lastuser = usuario;
                        cambio.Lastdate = DateTime.Now;
                        listaCambio.Add(cambio);
                    }
                }
                if (listaCambio.Count > 0)
                {
                    servFormato.GrabarCambios(listaCambio);
                }
                //Eliminar Valores Previos
                EliminarValoresCargados1XInter(formato.Formatcodi, idEmpresa, formato.FechaProceso, formato.FechaProceso.AddDays(1).AddSeconds(-1));
                GrabarMedicionesXIntevaloDisponibilidad(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Graba una lista de mediciones
        /// </summary>
        /// <param name="lista"></param>
        public void GrabarMedicionesXIntevaloDisponibilidad(List<MeMedicionxintervaloDTO> lista)
        {
            try
            {
                foreach (var reg in lista)
                {
                    DateTime fechafin = reg.Medintfechaini.AddDays(1).AddSeconds(-1);
                    var find = FactorySic.GetMeMedicionxintervaloRepository().BuscarRegistroPeriodo(reg.Medintfechaini, fechafin, reg.Ptomedicodi, reg.Tipoinfocodi, reg.Lectcodi);
                    if (find.Count > 0)
                    {
                        reg.Medintfechaini = find[find.Count - 1].Medintfechaini.AddSeconds(1);
                    }
                    servFormato.SaveMeMedicionxintervalo(reg);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void GrabarQuemaGas(List<MeMedicionxintervaloDTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            int idEnvioAnt = 0;
            var fechaIni = formato.FechaProceso;
            var fechaFin = formato.FechaProceso.AddDays(1).AddSeconds(-1);
            MeValidacionDTO validacion = new MeValidacionDTO();
            validacion.Emprcodi = idEmpresa;
            validacion.Formatcodi = formato.Formatcodi;
            validacion.Validfechaperiodo = formato.FechaProceso;
            validacion.Validestado = ConstantesStockCombustibles.NoValidado;
            validacion.Validusumodificacion = usuario;
            validacion.Validfecmodificacion = DateTime.Now;
            try
            {
                var findValicion = servFormato.GetByIdMeValidacion(formato.Formatcodi, idEmpresa, formato.FechaProceso);
                if (findValicion != null)
                {
                    servFormato.UpdateMeValidacion(validacion);
                }
                else
                {
                    servFormato.SaveMeValidacion(validacion);
                }
                //Traer Ultimos Valores
                var lista = servFormato.GetEnvioMedicionXIntervalo(formato.Formatcodi, idEmpresa, fechaIni, fechaFin);
                var listaCambio = new List<MeCambioenvioDTO>();
                idEnvioAnt = servFormato.ObtenerIdMaxEnvioFormatoPeriodo(formato.Formatcodi, idEmpresa, formato.FechaProceso);
                if (idEnvioAnt > 0)
                {
                    foreach (var reg in lista)
                    {
                        MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                        cambio.Cambenvdatos = reg.Medinth1.ToString() + "#" + reg.Medintdescrip + "#" + reg.Medestcodi;
                        cambio.Cambenvcolvar = "1";
                        cambio.Cambenvfecha = (DateTime)reg.Medintfechaini;
                        cambio.Enviocodi = idEnvioAnt;
                        cambio.Formatcodi = formato.Formatcodi;
                        cambio.Ptomedicodi = reg.Ptomedicodi;
                        cambio.Tipoinfocodi = reg.Tipoinfocodi;
                        cambio.Lastuser = usuario;
                        cambio.Lastdate = DateTime.Now;
                        listaCambio.Add(cambio);
                    }
                }
                if (listaCambio.Count > 0)
                {
                    servFormato.GrabarCambios(listaCambio);
                }
                //Eliminar Valores Previos
                EliminarValoresCargados1XInter(formato.Formatcodi, idEmpresa, formato.FechaProceso, formato.FechaProceso.AddDays(1).AddSeconds(-1));
                servFormato.GrabarMedicionesXIntevalo(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        ///  Permite generar lista de medicion por intervalos desde lista de cambioenvio
        /// </summary>
        /// <param name="enviocodi"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> GetEnvioCambioMedicionXIntervalo(int enviocodi)
        {
            List<MeMedicionxintervaloDTO> listaDatos = new List<MeMedicionxintervaloDTO>();
            MeMedicionxintervaloDTO registro = null;
            var lista = FactorySic.GetMeCambioenvioRepository().GetById(enviocodi);
            foreach (var reg in lista)
            {
                registro = new MeMedicionxintervaloDTO();
                decimal valor;
                if (decimal.TryParse(reg.Cambenvdatos, out valor))
                {
                    registro.Medinth1 = valor;
                    registro.Medintfechaini = reg.Cambenvfecha;
                    registro.Ptomedicodi = reg.Ptomedicodi;
                    listaDatos.Add(registro);
                }
            }
            return listaDatos;
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados1XInter(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicionxintervaloRepository().DeleteEnvioFormato(fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Eliminar valores de una columnaa de una hoja de envio
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="hoja"></param>
        /// <param name="Tptomedicion"></param>
        public void EliminarValoresEnvioColumna(DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa, int hoja, string sTptomedicion)
        {
            try
            {
                FactorySic.GetMeMedicionxintervaloRepository().DeleteEnvioFormatoHojaColumna(fechaInicio, fechaFin, idFormato, idEmpresa, hoja, sTptomedicion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Graba los datos enviados en el formato
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarMedicionesXIntevalo(List<MeMedicionxintervaloDTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            var lista = new List<MeMedicionxintervaloDTO>();
            var envioPrevio = servFormato.GetEnvioMedicionXIntervalo(formato.Formatcodi, idEmpresa, formato.FechaProceso, formato.FechaProceso);
            foreach (var reg in entitys)
            {
                var find = envioPrevio.Find(x => x.Medintfechaini == reg.Medintfechaini && x.Ptomedicodi == reg.Ptomedicodi &&
                    x.Tipoinfocodi == reg.Tipoinfocodi && x.Lectcodi == reg.Lectcodi);
                if (find == null)
                {
                    lista.Add(reg);
                }
            }

            try
            {
                //EliminarValoresCargados1XInter(formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaProceso, formato.FechaProceso);
                servFormato.GrabarMedicionesXIntevalo(lista);
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
        public void GrabarValoresCargadosMedxIntervalo(List<MeMedicionxintervaloDTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            MeValidacionDTO validacion = new MeValidacionDTO();
            validacion.Emprcodi = idEmpresa;
            validacion.Formatcodi = formato.Formatcodi;
            validacion.Validfechaperiodo = formato.FechaProceso;
            validacion.Validestado = ConstantesStockCombustibles.NoValidado;
            validacion.Validusumodificacion = usuario;
            validacion.Validfecmodificacion = DateTime.Now;
            try
            {
                var findValicion = servFormato.GetByIdMeValidacion(formato.Formatcodi, idEmpresa, formato.FechaProceso);
                if (findValicion != null)
                {
                    servFormato.UpdateMeValidacion(validacion);
                }
                else
                {
                    servFormato.SaveMeValidacion(validacion);
                }
                //Traer Ultimos Valores
                var envioPrevio = servFormato.GetEnvioMedicionXIntervalo(formato.Formatcodi, idEmpresa, formato.FechaProceso.AddDays(-1), formato.FechaProceso);
                var envioHoy = envioPrevio.Where(x => x.Medintfechaini == formato.FechaProceso).ToList();
                var envioActual = entitys.Where(x => x.Medintfechaini == formato.FechaProceso).ToList();
                //var lista = GetDataFormato(idEmpresa, formato, 0, 0);
                // verificar Stock Inicial
                var envioPrevioStockIni = envioPrevio.Where(x => x.Medintfechaini == formato.FechaProceso.AddDays(-1) &&
                    ConstantesStockCombustibles.IdsTptoStock.Contains(x.Tptomedicodi));
                var envioStockIni = entitys.Where(x => x.Medintfechaini == formato.FechaProceso.AddDays(-1));
                if (envioHoy.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();

                    foreach (var reg in entitys)
                    {
                        var regAnt = envioPrevio.Find(x => x.Medintfechaini == reg.Medintfechaini && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi);
                        decimal? valorOrigen = null;
                        string descripOrigen = string.Empty;
                        if (regAnt != null)
                        {
                            valorOrigen = (decimal?)regAnt.Medinth1;
                            descripOrigen = regAnt.Medintdescrip;
                        }
                        string stValor = string.Empty;
                        string stValorOrigen = string.Empty;
                        decimal? valorModificado = (decimal?)reg.Medinth1;
                        string descripModificado = reg.Medintdescrip;
                        if (valorModificado != null)
                            stValor = valorModificado.ToString();
                        if (valorOrigen != null)
                            stValorOrigen = valorOrigen.ToString();
                        if ((valorOrigen != valorModificado) || (descripOrigen != descripModificado))
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            if (descripOrigen != descripModificado && !string.IsNullOrEmpty(descripModificado))
                                cambio.Cambenvdatos = stValor + "#" + descripModificado;
                            else
                                cambio.Cambenvdatos = stValor;
                            cambio.Cambenvcolvar = "1";
                            cambio.Cambenvfecha = (DateTime)reg.Medintfechaini;
                            cambio.Enviocodi = idEnvio;
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambios en tabla MeCambioenvio se graba el registro original
                            if (servFormato.ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medintfechaini).Count == 0)
                            {
                                int idEnvioPrevio = 0;
                                var listAux = servFormato.GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaInicio); // lista de envios aprobados
                                if (listAux.Count > 0)
                                    idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                if (descripOrigen != descripModificado && !string.IsNullOrEmpty(descripModificado))
                                    origen.Cambenvdatos = stValorOrigen + "#" + descripOrigen;
                                else
                                    origen.Cambenvdatos = stValorOrigen;
                                origen.Cambenvcolvar = "";
                                origen.Cambenvfecha = (DateTime)reg.Medintfechaini;
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
                        servFormato.GrabarCambios(listaCambio);
                        servFormato.GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                ////Eliminar Valores Previos
                EliminarValoresCargados1XInter(formato.Formatcodi, idEmpresa, formato.FechaProceso, formato.FechaProceso);
                EliminarValoresEnvioColumna(formato.FechaProceso.AddDays(-1), formato.FechaProceso.AddDays(-1), formato.Formatcodi, idEmpresa, 3, //ANTES ESTABA 2(STOCK DE COMBUSTIBLES), pero su hojacodi es 3
                    string.Join(", ", ConstantesStockCombustibles.IdsTptoStock));
                foreach (MeMedicionxintervaloDTO entity in entitys)
                {
                    servFormato.SaveMeMedicionxintervalo(entity);
                }
            } // end try
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Genera el listado del stock de combustible de las centrales termoeléctricas
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsRecurso"> "tipoptomedicodi"</param>
        /// <param name="idsAgente"> codigos de la empresa : "emprcodi"</param>
        /// <param name="idsEstado"> "tipoinfocodi"</param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> GetListaReporteStock(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin, string idsEquipo, string tptomedicodi)
        {
            if (idsRecurso != null && idsRecurso != "-1")
                idsRecurso = string.Concat(idsRecurso, ",", ConstantesStockCombustibles.StrTptoRecepcion);
            string strCentralInt = this.GeneraCodCentralIntegrante(idsCentralInt);
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            fechaInicial = DateTime.ParseExact(idsFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            List<MeMedicionxintervaloDTO> listaMedxintervStock = this.ListaMedxIntervStock(ConstantesStockCombustibles.LectCodiStock, ConstantesStockCombustibles.Origlectcodi, idsAgente, ConstantesStockCombustibles.CentralTermica, fechaInicial, fechaFinal, idsEstado, idsRecurso, strCentralInt, idsEquipo, tptomedicodi);
            List<DateTime> ListaFechas = listaMedxintervStock.Select(x => x.Medintfechaini).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaInicial = ListaFechas[0];
                fechaFinal = ListaFechas[ListaFechas.Count - 1];
            }

            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();

            List<int> codRecepcion = new List<int>();
            List<int> codStock = new List<int>();
            codRecepcion = ConstantesStockCombustibles.StrTptoRecepcion.Split(',').Select(Int32.Parse).ToList();
            codStock = ConstantesStockCombustibles.StrTptoStock.Split(',').Select(Int32.Parse).ToList();
            foreach (var reg in listaMedxintervStock)
            {
                MeMedicionxintervaloDTO regStock = new MeMedicionxintervaloDTO();
                if (reg.Medintfechaini == fechaInicial)
                {
                    //if (reg.Tipoptomedicodi == ConstantesCombutiblesPr5.TipotomedicodiStock) // Stock Combustible
                    if (ConstantesStockCombustibles.StrTptoStock.Contains(reg.Tptomedicodi.ToString())) // Stock Combustible
                    {
                        var obj = servFormato.GetByIdMeMedicionxintervalo(reg.Ptomedicodi, reg.Medintfechaini.AddDays(-1));
                        if (obj != null)
                        {
                            regStock.Medinth1 = obj.Medinth1; // stock inicio = fin del dia anterior
                        }
                        regStock.H1Fin = reg.Medinth1;
                        int index = codStock.FindIndex(x => x == reg.Tptomedicodi);
                        // var obj2 = listaMedxintervStock.Find(x => x.Equicodi == reg.Equicodi && x.Medintfechaini == reg.Medintfechaini && codRecepcion.Contains(x.Tipoptomedicodi));
                        var obj2 = listaMedxintervStock.Find(x => x.Equicodi == reg.Equicodi && x.Medintfechaini == reg.Medintfechaini && codRecepcion[index] == x.Tptomedicodi);
                        if (obj2 != null)
                        {
                            regStock.H1Recep = obj2.Medinth1;
                        }
                        regStock.Medintfechaini = reg.Medintfechaini;
                        regStock.Emprcodi = reg.Emprcodi;
                        regStock.Emprnomb = reg.Emprnomb;
                        regStock.Equipadre = reg.Equicodi;
                        regStock.Equinomb = reg.Equinomb;
                        regStock.Fenergcodi = reg.Fenergcodi;
                        regStock.Fenergnomb = reg.Fenergnomb;
                        regStock.Fenercolor = reg.Fenercolor;
                        regStock.Tptomedicodi = reg.Tptomedicodi;
                        regStock.Tipoinfoabrev = reg.Tipoinfoabrev;
                        regStock.Ptomedicodi = reg.Ptomedicodi; // Reservamos pto de medicion de stock
                        regStock.Medintdescrip = reg.Medintdescrip != null ? reg.Medintdescrip : string.Empty;

                        listaReporte.Add(regStock);
                    }
                }
                else
                {
                    //if (reg.Tipoptomedicodi == ConstantesCombutiblesPr5.TipotomedicodiStock) // Stock Combustible
                    if (ConstantesStockCombustibles.StrTptoStock.Contains(reg.Tptomedicodi.ToString())) // Stock Combustible
                    {
                        var obj = listaMedxintervStock.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medintfechaini == reg.Medintfechaini.AddDays(-1));
                        if (obj != null)
                        {
                            regStock.Medinth1 = obj.Medinth1; // stock inicio = fin del dia anterior
                        }
                        regStock.H1Fin = reg.Medinth1;
                        //                        var obj2 = listaMedxintervStock.Find(x => x.Equicodi == reg.Equicodi && codRecepcion.Contains(x.Tipoptomedicodi)
                        //                                                          && x.Medintfechaini == reg.Medintfechaini);
                        int index = codStock.FindIndex(x => x == reg.Tptomedicodi);
                        // var obj2 = listaMedxintervStock.Find(x => x.Equicodi == reg.Equicodi && x.Medintfechaini == reg.Medintfechaini && codRecepcion.Contains(x.Tipoptomedicodi));
                        var obj2 = listaMedxintervStock.Find(x => x.Equicodi == reg.Equicodi && x.Medintfechaini == reg.Medintfechaini && codRecepcion[index] == x.Tptomedicodi);

                        if (obj2 != null)
                        {
                            regStock.H1Recep = obj2.Medinth1;
                        }
                        regStock.Medintfechaini = reg.Medintfechaini;
                        regStock.Emprcodi = reg.Emprcodi;
                        regStock.Emprnomb = reg.Emprnomb;
                        regStock.Equipadre = reg.Equicodi;
                        regStock.Equinomb = reg.Equinomb;
                        regStock.Fenergcodi = reg.Fenergcodi;
                        regStock.Fenergnomb = reg.Fenergnomb;
                        regStock.Fenercolor = reg.Fenercolor;
                        regStock.Tptomedicodi = reg.Tptomedicodi;
                        regStock.Tipoinfoabrev = reg.Tipoinfoabrev;
                        regStock.Ptomedicodi = reg.Ptomedicodi; // Reservamos pto de medicon de stock
                        regStock.Medintdescrip = reg.Medintdescrip != null ? reg.Medintdescrip : string.Empty;

                        listaReporte.Add(regStock);

                    }
                }
            }
            return listaReporte;
        }

        /// <summary>
        /// Genera el listado del stock de combustible de las centrales termoeléctricas
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsRecurso"> "tipoptomedicodi"</param>
        /// <param name="idsAgente"> codigos de la empresa : "emprcodi"</param>
        /// <param name="idsEstado"> "tipoinfocodi"</param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> GetListaReporteStockPag(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin, string idsEquipo, int nroPaginas, int pageSize, string tptomedicodi)
        {
            if (idsRecurso != null && idsRecurso != "-1")
                idsRecurso = string.Concat(idsRecurso, ",", ConstantesStockCombustibles.StrTptoRecepcion);
            string strCentralInt = this.GeneraCodCentralIntegrante(idsCentralInt);
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            fechaInicial = DateTime.ParseExact(idsFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            List<MeMedicionxintervaloDTO> listaMedxintervStock = this.ListaMedxIntervStockPag(ConstantesStockCombustibles.LectCodiStock, ConstantesStockCombustibles.Origlectcodi, idsAgente, ConstantesStockCombustibles.CentralTermica, fechaInicial, fechaFinal, idsEstado, idsRecurso, strCentralInt, idsEquipo, nroPaginas, pageSize, tptomedicodi);
            List<DateTime> ListaFechas = listaMedxintervStock.Select(x => x.Medintfechaini).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaInicial = ListaFechas[0];
                fechaFinal = ListaFechas[ListaFechas.Count - 1];
            }

            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();

            List<int> codRecepcion = new List<int>();
            List<int> codStock = new List<int>();
            codRecepcion = ConstantesStockCombustibles.StrTptoRecepcion.Split(',').Select(Int32.Parse).ToList();
            codStock = ConstantesStockCombustibles.StrTptoStock.Split(',').Select(Int32.Parse).ToList();
            foreach (var reg in listaMedxintervStock)
            {
                MeMedicionxintervaloDTO regStock = new MeMedicionxintervaloDTO();
                if (reg.Medintfechaini == fechaInicial)
                {
                    //if (reg.Tipoptomedicodi == ConstantesCombutiblesPr5.TipotomedicodiStock) // Stock Combustible
                    if (ConstantesStockCombustibles.StrTptoStock.Contains(reg.Tptomedicodi.ToString())) // Stock Combustible
                    {
                        var obj = servFormato.GetByIdMeMedicionxintervalo(reg.Ptomedicodi, reg.Medintfechaini.AddDays(-1));
                        if (obj != null)
                        {
                            regStock.Medinth1 = obj.Medinth1; // stock inicio = fin del dia anterior
                        }
                        regStock.H1Fin = reg.Medinth1;
                        int index = codStock.FindIndex(x => x == reg.Tptomedicodi);
                        // var obj2 = listaMedxintervStock.Find(x => x.Equicodi == reg.Equicodi && x.Medintfechaini == reg.Medintfechaini && codRecepcion.Contains(x.Tipoptomedicodi));
                        var obj2 = listaMedxintervStock.Find(x => x.Equicodi == reg.Equicodi && x.Medintfechaini == reg.Medintfechaini && codRecepcion[index] == x.Tptomedicodi);
                        if (obj2 != null)
                        {
                            regStock.H1Recep = obj2.Medinth1;
                        }
                        regStock.Medintfechaini = reg.Medintfechaini;
                        regStock.Emprnomb = reg.Emprnomb;
                        regStock.Equinomb = reg.Equinomb;
                        regStock.Fenergnomb = reg.Fenergnomb;
                        regStock.Fenercolor = reg.Fenercolor;
                        regStock.Tptomedicodi = reg.Tptomedicodi;
                        regStock.Tipoinfoabrev = reg.Tipoinfoabrev;
                        regStock.Ptomedicodi = reg.Ptomedicodi; // Reservamos pto de medicion de stock
                        regStock.Medintdescrip = reg.Medintdescrip != null ? reg.Medintdescrip : string.Empty;

                        listaReporte.Add(regStock);
                    }
                }
                else
                {
                    //if (reg.Tipoptomedicodi == ConstantesCombutiblesPr5.TipotomedicodiStock) // Stock Combustible
                    if (ConstantesStockCombustibles.StrTptoStock.Contains(reg.Tptomedicodi.ToString())) // Stock Combustible
                    {
                        var obj = listaMedxintervStock.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medintfechaini == reg.Medintfechaini.AddDays(-1));
                        if (obj != null)
                        {
                            regStock.Medinth1 = obj.Medinth1; // stock inicio = fin del dia anterior
                        }

                        else
                        {  /****Si no encontramos en la lista lo buscamos en la base de datos****/
                            var obj0 = servFormato.GetByIdMeMedicionxintervalo(reg.Ptomedicodi, reg.Medintfechaini.AddDays(-1));
                            if (obj0 != null)
                            {
                                regStock.Medinth1 = obj0.Medinth1; // stock inicio = fin del dia anterior
                            }
                        }
                        /////////////////////////////////////////////////////////////////////////////

                        regStock.H1Fin = reg.Medinth1;
                        //                        var obj2 = listaMedxintervStock.Find(x => x.Equicodi == reg.Equicodi && codRecepcion.Contains(x.Tipoptomedicodi)
                        //                                                          && x.Medintfechaini == reg.Medintfechaini);
                        int index = codStock.FindIndex(x => x == reg.Tptomedicodi);
                        // var obj2 = listaMedxintervStock.Find(x => x.Equicodi == reg.Equicodi && x.Medintfechaini == reg.Medintfechaini && codRecepcion.Contains(x.Tipoptomedicodi));
                        var obj2 = listaMedxintervStock.Find(x => x.Equicodi == reg.Equicodi && x.Medintfechaini == reg.Medintfechaini && codRecepcion[index] == x.Tptomedicodi);

                        if (obj2 != null)
                        {
                            regStock.H1Recep = obj2.Medinth1;
                        }
                        regStock.Medintfechaini = reg.Medintfechaini;
                        regStock.Emprnomb = reg.Emprnomb;
                        regStock.Equinomb = reg.Equinomb;
                        regStock.Fenergnomb = reg.Fenergnomb;
                        regStock.Fenercolor = reg.Fenercolor;
                        regStock.Tptomedicodi = reg.Tptomedicodi;
                        regStock.Tipoinfoabrev = reg.Tipoinfoabrev;
                        regStock.Ptomedicodi = reg.Ptomedicodi; // Reservamos pto de medicon de stock
                        regStock.Medintdescrip = reg.Medintdescrip != null ? reg.Medintdescrip : string.Empty;
                        listaReporte.Add(regStock);

                    }
                }
            }
            return listaReporte;
        }


        /// <summary>
        /// Genera el view con los datos del stock de combustibles de las centrales termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        public string GeneraViewReporteStockCombustible(List<MeMedicionxintervaloDTO> listaReporte, string idsEstado)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append(string.Format("<div class='title-seccion'>{0}</div>", GeneraTituloListado("REGISTRO DE STOCK DE COMBUSTIBLES - ", idsEstado)));
            string width = listaReporte.Count == 0 ? "100%" : "1500px";
            strHtml.AppendFormat("<table border='1' class='pretty tabla-icono' cellspacing='0' style='width: {0};' id='tabla'>", width);

            strHtml.Append("<thead>");
            strHtml.Append("<tr><th width=80px>FECHA</th>");
            strHtml.Append("<th width=250px>EMPRESA</th>");
            strHtml.Append("<th width=250px>CENTRAL</th>");
            strHtml.Append("<th width=150px>TIPO</th>");
            strHtml.Append("<th width=150px>INICIO</th>");
            strHtml.Append("<th width=150px>RECEPCIÓN</th>");
            //strHtml.Append("<th width=100px>CONSUMO</th>");
            strHtml.Append("<th width=150px>FINAL DECLARADO</th>");
            strHtml.Append("<th width=100px>UNIDADES</th>");
            strHtml.Append("<th width=400px>OBSERVACIONES</th>");
            strHtml.Append("</thead>");

            //*********************************************** fin de encabezado *********************************************
            strHtml.Append("<tbody>");
            foreach (var reg in listaReporte)
            {
                //k++;
                strHtml.Append("<tr>");
                strHtml.Append("<td >" + reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha) + "</td>");
                strHtml.Append("<td >" + reg.Emprnomb + "</td>");
                strHtml.Append("<td >" + reg.Equinomb + "</td>");
                strHtml.Append(string.Format("<td><div class='symbol' style='background-color:{0}'></div><div class='serieName'>{1}</div></td>",
                     reg.Fenercolor, reg.Fenergnomb));
                //IMPRIRME VALOR STOCK INICIO
                decimal? valorInicio;
                valorInicio = (decimal?)reg.Medinth1;
                if (valorInicio == null)
                    valorInicio = 0;

                strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valorInicio).ToString("N", nfi)));

                //IMPRIME VALOR RECEPCIÓN
                decimal? valorRecep = (decimal?)reg.H1Recep;
                if (valorRecep == null)
                    valorRecep = 0;

                if (valorRecep > 0) // Pinta celda de rojo si existe valor de recepción
                    strHtml.Append(string.Format("<td style='background-color:#F08080'>{0}</td>", ((decimal)valorRecep).ToString("N", nfi)));
                else
                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valorRecep).ToString("N", nfi)));

                //IMPRIME VALOR CONSUMO
                decimal? valorFin = (decimal?)reg.H1Fin;
                if (valorFin == null)
                    valorFin = 0;
                decimal? valorConsumo = valorInicio + valorRecep - valorFin;
                if (valorConsumo == null)
                    valorConsumo = 0;
                // strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valorConsumo).ToString("N", nfi)));

                //IMPRIME VALOR STOCK FIN
                strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valorFin).ToString("N", nfi)));

                strHtml.Append("<td >" + reg.Tipoinfoabrev + "</td>");
                strHtml.Append("<td >" + reg.Medintdescrip + "</td>"); strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }


        /// <summary>
        /// Genera el view con los datos de combustibles acumulados de las centrales termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        public string GeneraViewReporteAcumuladoCombustible(List<MeMedicionxintervaloDTO> listaReporte, string idsEstado)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append(string.Format("<div class='title-seccion'>{0}</div>", GeneraTituloListado("COMBUSTIBLES ACUMULADOS - ", idsEstado)));
            strHtml.Append("<table border='1'  class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>");

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th width=100px>EMPRESA</th>");
            strHtml.Append("<th width=100px>CENTRAL</th>");
            strHtml.Append("<th width=60px>TIPO</th>");
            strHtml.Append("<th width=100px>ACUMULADO</th>");
            strHtml.Append("<th width=100px>UNIDADES</th></tr>");
            strHtml.Append("</thead>");

            //*********************************************** fin de encabezado *********************************************
            strHtml.Append("<tbody>");

            List<MeMedicionxintervaloDTO> listAcumulado = listaReporte.GroupBy(t => new { t.Ptomedicodi, t.Emprnomb, t.Equinomb, t.Tipoptomedinomb, t.Tipoinfoabrev })
                                    .Select(g => new MeMedicionxintervaloDTO()
                                    {
                                        Ptomedicodi = g.Key.Ptomedicodi,
                                        Emprnomb = g.Key.Emprnomb,
                                        Equinomb = g.Key.Equinomb,
                                        Tipoptomedinomb = g.Key.Tipoptomedinomb,
                                        Tipoinfoabrev = g.Key.Tipoinfoabrev,
                                        H1Recep = g.Sum(t => t.H1Recep)
                                    }).ToList();



            foreach (var reg in listAcumulado)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td >" + reg.Emprnomb + "</td>");
                strHtml.Append("<td >" + reg.Equinomb + "</td>");
                strHtml.Append(string.Format("<td><div class='symbol' style='background-color:{0}'></div><div class='serieName'>{1}</div></td>",
                      reg.Fenercolor, reg.Tipoptomedinomb.Substring(19, reg.Tipoptomedinomb.Length - 19)));

                //IMPRIME VALOR ACUMULADO
                decimal? valorAcum = (decimal?)reg.H1Recep;
                if (valorAcum == null)
                    valorAcum = 0;
                strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valorAcum).ToString("N", nfi)));

                strHtml.Append("<td >" + reg.Tipoinfoabrev + "</td>");
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// Se obtine los registros de la tabla ME_MEDICIONXINTERVALO de un rango de fechas y por tipo de lectura stock de combustibles
        /// </summary>
        /// <param name="lectCodi"></param>
        /// <param name="origlectcodi"></param>
        /// <param name="sEmprCodi"></param>
        /// <param name="famCodi"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsRecurso"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> ListaMedxIntervStock(int lectCodi, int origlectcodi, string sEmprCodi, int famCodi, DateTime fechaInicial, DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt, string idsEquipo, string tptomedicodi)
        {
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            if (string.IsNullOrEmpty(sEmprCodi)) sEmprCodi = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsRecurso)) idsRecurso = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEquipo)) idsEquipo = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(tptomedicodi)) tptomedicodi = ConstantesAppServicio.ParametroDefecto;
            lista = FactorySic.GetMeMedicionxintervaloRepository().GetListaMedxintervStock(lectCodi, origlectcodi, sEmprCodi, famCodi, fechaInicial, fechaFinal, idsEstado, idsRecurso, strCentralInt, idsEquipo, tptomedicodi);
            return lista;
        }

        /// <summary>
        /// Se obtine los registros de la tabla ME_MEDICIONXINTERVALO de un rango de fechas y por tipo de lectura stock de combustibles para paginado
        /// </summary>
        /// <param name="lectCodi"></param>
        /// <param name="origlectcodi"></param>
        /// <param name="sEmprCodi"></param>
        /// <param name="famCodi"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsRecurso"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> ListaMedxIntervStockPag(int lectCodi, int origlectcodi, string sEmprCodi, int famCodi, DateTime fechaInicial, DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt, string idsEquipo, int nroPaginas, int pageSize, string tptomedicodi)
        {
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            if (string.IsNullOrEmpty(sEmprCodi)) sEmprCodi = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsRecurso)) idsRecurso = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEquipo)) idsEquipo = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(tptomedicodi)) tptomedicodi = ConstantesAppServicio.ParametroDefecto;
            lista = FactorySic.GetMeMedicionxintervaloRepository().GetListaMedxintervStockPag(lectCodi, origlectcodi, sEmprCodi, famCodi, fechaInicial, fechaFinal, idsEstado, idsRecurso, strCentralInt, idsEquipo, nroPaginas, pageSize, tptomedicodi);
            return lista;
        }

        /// <summary>
        /// Genera el view con los datos del consumo de combustibles de las centrales termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        public string GeneraViewReporteConsumoCombustible(List<MeMedicionxintervaloDTO> listaReporte, List<DateTime> ListaFechas, string idsEstado)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            int nCol = ListaFechas.Count + 7;
            List<MeMedicionxintervaloDTO> listaCabeceraM = listaReporte.GroupBy(x => new
            {
                x.Ptomedicodi,
                x.Emprnomb,
                x.Ptomedibarranomb,
                x.Tipoinfoabrev,
                x.Tptomedicodi,
                x.Fenergnomb,
                x.Fenercolor,
                x.Equinomb,
                x.Emprcoes,
                x.Ptomedielenomb,
                x.Equipadre,
                x.Equipopadre
            })
                                .Select(y => new MeMedicionxintervaloDTO()
                                {
                                    Emprnomb = y.Key.Emprnomb,
                                    Ptomedicodi = y.Key.Ptomedicodi,
                                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                    Tptomedicodi = y.Key.Tptomedicodi,
                                    Fenergnomb = y.Key.Fenergnomb,
                                    Fenercolor = y.Key.Fenercolor,
                                    Equinomb = y.Key.Equinomb,
                                    Equipadre = y.Key.Equipadre,
                                    Equipopadre = y.Key.Equipopadre,
                                    Emprcoes = y.Key.Emprcoes,
                                    Ptomedielenomb = y.Key.Ptomedielenomb,
                                }
                                ).ToList();


            strHtml.Append("<table border='1'  class='pretty tabla-icono'  width=100% cellpadding='10' cellspacing='10' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<div class='title-seccion'>{0}</div>", GeneraTituloListado("CONSUMO DE COMBUSTIBLES - ", idsEstado)));
            strHtml.Append("</tr>");

            strHtml.Append("<tr><th>EMPRESA</th>");
            strHtml.Append("<th>TIPO AGENTE</th>");
            strHtml.Append("<th>CENTRAL</th>");
            strHtml.Append("<th>CENTRAL INTEGRANTE</th>");
            strHtml.Append("<th>GRUPO</th>");
            strHtml.Append("<th width=100px>TIPO</th>");
            strHtml.Append("<th>UNIDAD</th>");
            foreach (var reg in ListaFechas)
            {
                strHtml.Append(string.Format("<th>{0}</th>", reg.ToString(ConstantesStockCombustibles.FormatoDiaMes)));

            }
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            //*********************************************** fin de encabezado *********************************************
            strHtml.Append("<tbody>");

            foreach (var reg in listaCabeceraM)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td class='th1'>" + reg.Emprnomb + "</td>");

                if (reg.Emprcoes == "S")
                    strHtml.Append("<td class='th4'>" + "INTEGRANTE" + "</td>");
                else
                    strHtml.Append("<td class='th4'>" + "NO INTEGRANTE" + "</td>");


                /* strHtml.Append("<td class='th2'>" + "TERMOELÉCTRICA" + "</td>");*/

                if (reg.Equipadre > 0)
                    strHtml.Append("<td class='th3'>" + reg.Equipopadre + "</td>");
                else
                    strHtml.Append("<td class='th3'>" + reg.Equinomb + "</td>");

                if (reg.Emprcoes == "S")
                    strHtml.Append("<td class='th4'>" + "COES" + "</td>");
                else
                    strHtml.Append("<td class='th4'>" + "NO COES" + "</td>");

                strHtml.Append("<td class='th5'>" + reg.Equinomb + "</td>");

                strHtml.Append(string.Format("<td><div class='symbol' style='background-color:{0}'></div><div class='serieName' style='padding-top: 4px;'>{1}</div></td>",
                reg.Fenercolor, reg.Fenergnomb));
                strHtml.Append("<td >" + reg.Tipoinfoabrev + "</td>");

                foreach (var reg2 in ListaFechas)
                {
                    decimal? valor;
                    var fil = listaReporte.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medintfechaini == reg2);
                    if (fil != null)
                    {
                        valor = fil.Medinth1;
                        if (valor != null)
                            strHtml.Append(string.Format("<td>{0}</th>", ((decimal)valor).ToString("N", nfi)));
                        else
                            strHtml.Append(string.Format("<td>--</td>"));
                    }
                    else
                    {
                        strHtml.Append(string.Format("<td>--</td>"));
                    }
                }

                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// Genera Los Tab dinamicamente para el reporte
        /// </summary>
        /// <param name="idsRecurso"></param>
        /// <param name="idsEstado"></param>
        /// <returns></returns>
        public string GeneraTabReporteGrafico(string idsRecurso, string idsEstado)
        {
            StringBuilder strHtml = new StringBuilder();
            List<MeTipopuntomedicionDTO> entitys = new List<MeTipopuntomedicionDTO>();
            string tablista = string.Empty;
            string idGrafico = string.Empty;
            string legend = string.Empty;
            string titulo = string.Empty;
            string idli = string.Empty;
            entitys = this.ListaMeTipoPuntoMedicion(idsRecurso, idsEstado);
            strHtml.Append("<script type='text/javascript'>");
            strHtml.Append("$(document).ready( function() {");
            strHtml.Append("$('#tab-container').easytabs({");
            strHtml.Append("animate: false});});</script>");
            strHtml.Append("<div id='tab-container' class='tab-container'>");

            strHtml.Append("<ul class='etabs'>");

            foreach (var reg in entitys)
            {
                idli = "li" + reg.Tipoptomedinomb.Substring(24, reg.Tipoptomedinomb.Length - 24);
                strHtml.Append(string.Format("<li class='tab' id='{2}'><a href='#{0}'>{1}</a></li>", reg.Tipoptomedinomb.Substring(24, reg.Tipoptomedinomb.Length - 24), reg.Tipoptomedinomb.Substring(24, reg.Tipoptomedinomb.Length - 24), idli));
            }

            strHtml.Append("</ul>");
            strHtml.Append("<div class='panel-container'>");
            foreach (var r in entitys)
            {
                tablista = r.Tipoptomedinomb.Substring(24, r.Tipoptomedinomb.Length - 24);
                idGrafico = "grafico" + r.Tipoptomedinomb.Substring(24, r.Tipoptomedinomb.Length - 24);
                legend = "legend" + r.Tipoptomedinomb.Substring(24, r.Tipoptomedinomb.Length - 24);
                titulo = "titulo" + r.Tipoptomedinomb.Substring(24, r.Tipoptomedinomb.Length - 24);
                strHtml.Append(string.Format("<div id='{0}'>", tablista));
                strHtml.Append("<table   border='0' cellspacing='0' width='100%' id='tabla'>");
                strHtml.Append("<tr><td colspan='2' class='titulo-reporte-mantenimiento'>");
                strHtml.Append(string.Format("<div id='{0}'></div></td></tr>", titulo));
                strHtml.Append("<tr><td colspan='2' class='separacion-reporte-mantenimiento'><div>&nbsp;</div></td></tr>");
                strHtml.Append("<tr><td valign='top'>");
                strHtml.Append(string.Format("<div id='{0}'></div></td>", legend));
                strHtml.Append("<td valign='top'>");
                strHtml.Append(string.Format("<div id='{0}' style='width: 900px; height: 500px; margin: 0 auto'></div></td></tr>", idGrafico));
                strHtml.Append("</table>");
                strHtml.Append("</div>");
            }

            strHtml.Append("</div>"); ///end panel-container
            strHtml.Append("</div>");  // end tab-container
            return strHtml.ToString();
        }

        /// <summary>
        /// Se obtine los registros de la tabla ME_MEDICIONXINTERVALO de un rango de fechas y por tipo de lectura consumo de combustibles
        /// </summary>
        /// <param name="lectCodi"></param>
        /// <param name="origlectcodi"></param>
        /// <param name="sEmprCodi"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsRecurso"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> ListaMedxIntervConsumo(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt)
        {
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            if (string.IsNullOrEmpty(sEmprCodi)) sEmprCodi = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsRecurso)) idsRecurso = ConstantesAppServicio.ParametroDefecto;
            lista = FactorySic.GetMeMedicionxintervaloRepository().GetListaMedxintervConsumo(lectCodi, origlectcodi, sEmprCodi, fechaInicial, fechaFinal, idsEstado, idsRecurso, strCentralInt, ConstantesAppServicio.ParametroDefecto);

            foreach (var reg in lista)
            {
                reg.Emprnomb = reg.Emprnomb != null ? reg.Emprnomb.Trim() : string.Empty;
                reg.Equipopadre = reg.Equipopadre != null ? reg.Equipopadre.Trim() : string.Empty;
                reg.Equinomb = reg.Emprnomb != null ? reg.Equinomb.Trim() : string.Empty;
                reg.Emprnomb = reg.Emprnomb != null ? reg.Emprnomb.Trim() : string.Empty;
                reg.Fenergnomb = reg.Fenergnomb != null ? reg.Fenergnomb.Trim() : string.Empty;
                reg.Tipoinfoabrev = reg.Tipoinfoabrev != null ? reg.Tipoinfoabrev.Trim() : string.Empty;
            }

            return lista;
        }

        /// <summary>
        /// Genera el view con los datos de la disponibilidad de gas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        public string GeneraViewReporteDisponibilidadGas(List<MeMedicionxintervaloDTO> listaReporte, List<DateTime> ListaFechas)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table class='pretty tabla-icono' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr><div class='title-seccion'>HISTÓRICO DISPONIBILIDAD DE GAS NATURAL</div>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr><th>FECHA</th>");
            strHtml.Append("<th>EMPRESA</th>");
            strHtml.Append("<th>GASEODUCTO</th>");

            /* strHtml.Append("<th colspan = '3'>DISPONIBILDAD DE GAS NATURAL INFORMADA EN TIEMPO REAL</th>");
             strHtml.Append("</tr>");*/
            strHtml.Append("<th>VOLUMEN DE GAS (Mm3)</th>");
            strHtml.Append("<th>INICIO</th>");
            strHtml.Append("<th>FINAL</th>");
            strHtml.Append("<th>FECHA HORA ENVIO</th>");
            strHtml.Append("<th>ESTADO</th>");
            strHtml.Append("<th>OBSERVACIONES</th>");
            strHtml.Append("<th>USUARIO</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            //*********************************************** fin de encabezado *********************************************
            strHtml.Append("<tbody>");

            foreach (var reg in listaReporte)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td >" + reg.Medintfechaini.ToString(ConstantesStockCombustibles.FormatoFecha) + "</td>");
                strHtml.Append("<td >" + reg.Emprnomb + "</td>");
                strHtml.Append("<td >" + reg.Equinomb + "</td>");
                decimal? valor;
                valor = (decimal?)reg.Medinth1;

                //************DAtos tiempo Real *****************
                if (valor != null)
                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                else
                    strHtml.Append(string.Format("<td>--</td>"));
                /* strHtml.Append("<td >" + reg.Medintfechaini.ToString(ConstantesStockCombustibles.FormatoHoraMinuto) + "</td>");
                 strHtml.Append("<td >" + reg.Medintfechafin.ToString(ConstantesStockCombustibles.FormatoHoraMinuto) + "</td>");*/
                strHtml.Append("<td >" + reg.Medintfechaini.ToString(ConstantesStockCombustibles.FormatoFecha) + " 06:00</td>");
                strHtml.Append("<td >" + reg.Medintfechaini.AddDays(1).ToString(ConstantesStockCombustibles.FormatoFecha) + " 06:00</td>");

                strHtml.Append("<td >" + reg.Medintfechafin.ToString(ConstantesStockCombustibles.FormatoFechaHora) + "</td>");

                string estado = "";
                if (reg.Medestcodi == 1) { estado = "Declaró"; }
                if (reg.Medestcodi == 2) { estado = "Renominó"; }

                strHtml.Append("<td >" + estado + "</td>");

                strHtml.Append("<td >" + reg.Medintdescrip + "</td>");
                strHtml.Append("<td >" + reg.Medintusumodificacion + "</td>");

                //*****fin datos tiempo real**************

                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// Se obtine los registros de la tabla ME_MEDICIONXINTERVALO de un rango de fechas y por tipo de lectura disponibilidad de gas
        /// </summary>
        /// <param name="lectCodi"></param>
        /// <param name="origlectcodi"></param>
        /// <param name="sEmprCodi"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> ListaMedxIntervDisponibilidad(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string strCentralInt, int idYacimGas, string idsYacimientos)
        {
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            if (string.IsNullOrEmpty(sEmprCodi)) sEmprCodi = ConstantesAppServicio.ParametroDefecto;
            lista = FactorySic.GetMeMedicionxintervaloRepository().GetListaMedxintervDisponibilidad(lectCodi, origlectcodi, sEmprCodi, fechaInicial, fechaFinal, strCentralInt, idYacimGas, idsYacimientos);
            return lista;
        }

        /// <summary>
        /// Genera el view con los datos de la quema de gas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        public string GeneraViewReporteQuemaGas(List<MeMedicionxintervaloDTO> listaReporte, List<DateTime> ListaFechas)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table  class='pretty tabla-icono'  id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<div class='title-seccion'>HISTÓRICO QUEMA DE GAS NATURAL</div>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr><th >FECHA</th>");
            strHtml.Append("<th>EMPRESA</th>");
            strHtml.Append("<th>CENTRAL</th>");
            strHtml.Append("<th>TIPO DE COMBUSTIBLE</th>");
            strHtml.Append("<th>INICIO</th>");
            strHtml.Append("<th>FINAL</th>");
            strHtml.Append("<th>VOLUMEN DE GAS (Mm3)</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            //*********************************************** fin de encabezado *********************************************
            strHtml.Append("<tbody>");

            foreach (var reg in listaReporte)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td >" + reg.Medintfechaini.ToString(ConstantesStockCombustibles.FormatoFecha) + "</td>");
                strHtml.Append("<td >" + reg.Emprnomb + "</td>");
                strHtml.Append("<td >" + reg.Equinomb + "</td>");
                strHtml.Append("<td >" + reg.Tipoptomedinomb + "</td>");
                strHtml.Append("<td >" + reg.Medintfechaini.ToString(ConstantesStockCombustibles.FormatoHoraMinuto) + "</td>");
                strHtml.Append("<td >" + reg.Medintfechafin.ToString(ConstantesStockCombustibles.FormatoHoraMinuto) + "</td>");

                decimal? valor;
                valor = (decimal?)reg.Medinth1;
                if (valor != null)
                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                else
                    strHtml.Append(string.Format("<td>--</td>"));

                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// Se obtine los registros de la tabla ME_MEDICIONXINTERVALO de un rango de fechas y por tipo de lectura quema de gas
        /// </summary>
        /// <param name="lectCodi"></param>
        /// <param name="origlectcodi"></param>
        /// <param name="sEmprCodi"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> ListaMedxIntervQuema(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string strCentralInt)
        {
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            if (string.IsNullOrEmpty(sEmprCodi)) sEmprCodi = ConstantesAppServicio.ParametroDefecto;
            lista = FactorySic.GetMeMedicionxintervaloRepository().GetListaMedxintervQuema(lectCodi, origlectcodi, sEmprCodi, fechaInicial, fechaFinal, strCentralInt);
            return lista;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_MEDICIONXINTERVALO
        /// </summary>
        public List<MeMedicionxintervaloDTO> ListConsumoCentral(DateTime fecha, int ptomedicion, int empresa)
        {
            return FactorySic.GetMeMedicionxintervaloRepository().GetConsumoCentral(fecha, ptomedicion, empresa);
        }

        #endregion

        #region Métodos Tabla ME_CAMBIOENVIO

        public List<MeMedicionxintervaloDTO> GetDataEnvio(int enviocodi)
        {
            List<MeMedicionxintervaloDTO> listaDatos = new List<MeMedicionxintervaloDTO>();
            MeMedicionxintervaloDTO registro = new MeMedicionxintervaloDTO();
            var lista = FactorySic.GetMeCambioenvioRepository().GetById(enviocodi);
            foreach (var reg in lista)
            {
                var listCampos = reg.Cambenvdatos.Split('#');
                registro = new MeMedicionxintervaloDTO();
                registro.Medintfechaini = reg.Cambenvfecha;
                registro.Ptomedicodi = reg.Ptomedicodi;
                decimal valor = 0;
                decimal.TryParse(listCampos[0], out valor);
                registro.Medinth1 = valor;
                int estado = 0;
                int.TryParse(listCampos[1], out estado);
                registro.Medestcodi = estado;
                listaDatos.Add(registro);
            }
            return listaDatos;
        }

        #endregion

        #region Métodos EVE_HORAOPERACION
        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnvio
        /// </summary>
        public List<EveHoraoperacionDTO> GetByDetalleHO(DateTime fecha)
        {
            List<EveHoraoperacionDTO> lista = FactorySic.GetEveHoraoperacionRepository().GetByDetalleHO(fecha);
            List<EveHoraoperacionDTO> listaFinal = new List<EveHoraoperacionDTO>();
            EveHoraoperacionDTO registro = new EveHoraoperacionDTO();
            //Se unen horas de operación definidas por tramos consecutivos.
            foreach (var reg in lista)
            {
                if (registro.Hophorini == null)
                {
                    registro.Hophorini = reg.Hophorini;
                    registro.Hophorfin = reg.Hophorfin;
                    registro.Unidad = reg.Unidad;
                    registro.Gruponomb = reg.Gruponomb;
                    registro.Grupopadre = reg.Grupopadre;
                }
                else
                {
                    if (registro.Hophorfin == reg.Hophorini)
                        registro.Hophorfin = reg.Hophorfin;
                    else
                    {
                        listaFinal.Add(registro);
                        registro = new EveHoraoperacionDTO();
                        registro.Hophorini = reg.Hophorini;
                        registro.Hophorfin = reg.Hophorfin;
                        registro.Unidad = reg.Unidad;
                        registro.Gruponomb = reg.Gruponomb;
                        registro.Grupopadre = reg.Grupopadre;
                    }
                }

            }
            if (lista.Count > 0)
            {
                listaFinal.Add(registro);
            }
            return listaFinal;
        }

        /// <summary>
        /// /Genera Vista del reporte de Validacion Horas de Operacion vs Despacho
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>

        public string GeneraViewReporteValidacion(DateTime fecha)
        {
            string vista = string.Empty;
            var lista = GetByDetalleHO(fecha);
            List<MeMedicion48DTO> listaDetalle = new List<MeMedicion48DTO>();
            List<EveHoraoperacionDTO> listaCabecera = lista.GroupBy(x => new { x.Unidad })
                .Select(y => y.First()).ToList();
            var listaDespacho = FactorySic.GetMeMedicion48Repository().SqlObtenerDatosEjecutado(fecha);
            GenerarMatrizHoraOperacionDespacho(listaDetalle, lista, listaDespacho);

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:auto'  cellspacing='0' width='100%' id='tabla'>");

            strHtml.Append("<thead>");
            strHtml.Append(string.Format("<tr><th style='width:100px;'>{0}</th>", "Hora"));
            foreach (var reg in listaCabecera)
            {
                var tam = reg.Gruponomb.Length;
                strHtml.Append(string.Format("<th width=250 style='width:100px;'>{0}</th>", reg.Gruponomb));
            }
            strHtml.Append("</tr></thead>");
            strHtml.Append("<tbody>");

            for (var i = 1; i <= 48; i++)
            {
                strHtml.Append("<tr>");
                var fechaFila = fecha.AddMinutes(i * 30).ToString("HH:mm");
                strHtml.Append(string.Format("<td>{0}</td>", fechaFila));
                foreach (var reg in listaCabecera)
                {
                    var registro = listaDetalle.Find(x => x.Grupocodi == reg.Unidad);
                    if (registro != null)
                    {
                        var valor = (decimal?)registro.GetType().GetProperty("H" + i).GetValue(registro, null);
                        var color = "#FFFFFF";
                        switch ((int)valor)
                        {
                            case 1:
                                color = "#4682B4";
                                break;
                            case 2:
                                color = "#DB7093";
                                break;
                            case 3:
                                color = "#9370DB";
                                break;
                        }
                        strHtml.Append(string.Format("<td bgcolor='" + color + "'></td>"));
                    }
                }
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// /Obtiene matriz del cruce de horas de operación vs despacho
        /// </summary>
        /// <param name="listaMatriz"></param>
        /// <param name="listaHO"></param>
        /// <param name="listaDespacho"></param>
        public void GenerarMatrizHoraOperacionDespacho(List<MeMedicion48DTO> listaMatriz, List<EveHoraoperacionDTO> listaHO, List<MeMedicion48DTO> listaDespacho)
        {
            List<EveHoraoperacionDTO> listaCabecera = listaHO.GroupBy(x => new { x.Unidad, x.Gruponomb, x.Grupocodi })
               .Select(y => new EveHoraoperacionDTO()
               {
                   Unidad = y.Key.Unidad,
                   Gruponomb = y.Key.Gruponomb,
                   Grupocodi = y.Key.Grupocodi
               }
               ).ToList();
            MeMedicion48DTO registro;

            foreach (var reg in listaCabecera)
            {
                registro = new MeMedicion48DTO();
                registro.Grupocodi = reg.Unidad;
                for (var i = 1; i <= 48; i++)
                    registro.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(registro, (decimal?)0);
                listaMatriz.Add(registro);
            }
            /// Agregar  ptos que no estan en horas de operacion y si en despacho
            var gruposDespachoAdicional = listaDespacho.Where(x => listaMatriz.Any(y => x.Grupocodi != y.Grupocodi)).
                Select(x => new MeMedicion48DTO()
                {
                    Grupocodi = x.Grupocodi,
                    Gruponomb = x.Gruponomb,
                });
            foreach (var reg in gruposDespachoAdicional)
            {
                var entity = new EveHoraoperacionDTO();
                entity.Unidad = reg.Grupocodi;
                entity.Gruponomb = reg.Gruponomb;
                listaCabecera.Add(entity);
            }

            foreach (var reg in listaHO)
            {
                var find = listaMatriz.Find(x => x.Grupocodi == reg.Unidad);
                if (find != null)
                {
                    int indiceIni = 0;
                    int indiceFin = 1;
                    var hora = reg.Hophorini.Value.Hour;
                    indiceIni = hora * 2;
                    var minuto = reg.Hophorini.Value.Minute;
                    if (minuto <= 30)
                    {
                        if (hora == 0)
                        {
                            indiceIni = 1;
                        }
                        else
                            if (minuto > 0)
                        {
                            indiceIni++;
                        }
                    }
                    else
                    {
                        indiceIni += 2;
                    }

                    if (reg.Hophorini.Value.AddDays(1).Day == reg.Hophorfin.Value.Day)
                    {
                        indiceFin = 48;
                    }
                    else
                    {
                        hora = reg.Hophorfin.Value.Hour;
                        indiceFin = hora * 2;
                        minuto = reg.Hophorfin.Value.Minute;
                        if (minuto >= 30)
                        {
                            indiceFin++;
                        }
                    }
                    if (indiceFin >= indiceIni)
                    {
                        for (var i = indiceIni; i <= indiceFin; i++)
                        {
                            find.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(find, (decimal?)1);
                        }
                    }
                }

            }
            foreach (var reg in listaDespacho)
            {
                var find = listaMatriz.Find(x => x.Grupocodi == reg.Grupocodi);
                if (find != null)
                {
                    for (var i = 1; i <= 48; i++)
                    {
                        var valorHO = (decimal)find.GetType().GetProperty("H" + i).GetValue(find, null);
                        var valorD = (decimal)reg.GetType().GetProperty("H" + i).GetValue(reg, null);
                        if (valorD > 0)
                        {
                            switch ((int)valorHO)
                            {
                                case 0:
                                    find.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(find, (decimal?)2);
                                    break;
                                case 1:
                                    find.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(find, (decimal?)3);
                                    break;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// /Genera Vista del reporte de Validacion Horas de Operacion vs Medidores
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>

        public string GeneraViewReporteValidacion96(DateTime fecha)
        {
            string vista = string.Empty;
            var lista = GetByDetalleHO(fecha);
            List<MeMedicion96DTO> listaDetalle = new List<MeMedicion96DTO>();
            List<EveHoraoperacionDTO> listaCabecera = lista.GroupBy(x => new { x.Unidad })
                .Select(y => y.First()).ToList();
            var listaDespacho = FactorySic.GetMeMedicion96Repository().SqlObtenerDatosEjecutado(fecha);
            GenerarMatrizHoraOperacionMedidores(listaDetalle, lista, listaDespacho);

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:auto'  cellspacing='0' width='100%' id='tabla'>");

            strHtml.Append("<thead>");
            strHtml.Append(string.Format("<tr><th style='width:100px;'>{0}</th>", "Hora"));
            foreach (var reg in listaCabecera)
            {
                var tam = reg.Gruponomb.Length;
                strHtml.Append(string.Format("<th width=250 style='width:100px;'>{0}</th>", reg.Gruponomb));
            }
            strHtml.Append("</tr></thead>");
            strHtml.Append("<tbody>");

            for (var i = 1; i <= 96; i++)
            {
                strHtml.Append("<tr>");
                var fechaFila = fecha.AddMinutes(i * 15).ToString("HH:mm");
                strHtml.Append(string.Format("<td>{0}</td>", fechaFila));
                foreach (var reg in listaCabecera)
                {
                    var registro = listaDetalle.Find(x => x.Grupocodi == reg.Unidad);
                    if (registro != null)
                    {
                        var valor = (decimal?)registro.GetType().GetProperty("H" + i).GetValue(registro, null);
                        var color = "#FFFFFF";
                        switch ((int)valor)
                        {
                            case 1:
                                color = "#4682B4";
                                break;
                            case 2:
                                color = "#DB7093";
                                break;
                            case 3:
                                color = "#9370DB";
                                break;
                        }
                        strHtml.Append(string.Format("<td bgcolor='" + color + "'></td>"));
                    }
                }
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// /Obtiene matriz del cruce de horas de operación vs despacho
        /// </summary>
        /// <param name="listaMatriz"></param>
        /// <param name="listaHO"></param>
        /// <param name="listaDespacho"></param>
        public void GenerarMatrizHoraOperacionMedidores(List<MeMedicion96DTO> listaMatriz, List<EveHoraoperacionDTO> listaHO, List<MeMedicion96DTO> listaMedidores)
        {
            int indiceFin = 1;
            List<EveHoraoperacionDTO> listaCabecera = listaHO.GroupBy(x => new { x.Unidad, x.Gruponomb, x.Grupocodi })
               .Select(y => new EveHoraoperacionDTO()
               {
                   Unidad = y.Key.Unidad,
                   Gruponomb = y.Key.Gruponomb,
                   Grupocodi = y.Key.Grupocodi
               }
               ).ToList();
            MeMedicion96DTO registro;

            foreach (var reg in listaCabecera)
            {
                registro = new MeMedicion96DTO();
                registro.Grupocodi = reg.Unidad;
                for (var i = 1; i <= 96; i++)
                    registro.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(registro, (decimal?)0);
                listaMatriz.Add(registro);
            }
            /// Agregar  ptos que no estan en horas de operacion y si en despacho
            var gruposDespachoAdicional = listaMedidores.Where(x => listaMatriz.Any(y => x.Grupocodi != y.Grupocodi)).
                Select(x => new MeMedicion96DTO()
                {
                    Grupocodi = x.Grupocodi,
                    Gruponomb = x.Gruponomb,
                });
            foreach (var reg in gruposDespachoAdicional)
            {
                var entity = new EveHoraoperacionDTO();
                entity.Unidad = reg.Grupocodi;
                entity.Gruponomb = reg.Gruponomb;
                listaCabecera.Add(entity);
            }

            foreach (var reg in listaHO)
            {
                var find = listaMatriz.Find(x => x.Grupocodi == reg.Unidad);
                if (find != null)
                {
                    int indiceIni = IndiceInicio((DateTime)reg.Hophorini, 15);
                    if (reg.Hophorini.Value.AddDays(1).Day == reg.Hophorfin.Value.Day)
                    {
                        indiceFin = 96;
                    }
                    else
                    {
                        indiceFin = IndiceFinal((DateTime)reg.Hophorfin, 15);
                    }

                    if (indiceFin >= indiceIni)
                    {
                        for (var i = indiceIni; i <= indiceFin; i++)
                        {
                            find.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(find, (decimal?)1);
                        }
                    }
                }

            }
            foreach (var reg in listaMedidores)
            {
                var find = listaMatriz.Find(x => x.Grupocodi == reg.Grupocodi);
                if (find != null)
                {
                    for (var i = 1; i <= 96; i++)
                    {
                        var valorHO = (decimal)find.GetType().GetProperty("H" + i).GetValue(find, null);
                        var valorD = (decimal)reg.GetType().GetProperty("H" + i).GetValue(reg, null);
                        if (valorD > 0)
                        {
                            switch ((int)valorHO)
                            {
                                case 0:
                                    find.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(find, (decimal?)2);
                                    break;
                                case 1:
                                    find.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(find, (decimal?)3);
                                    break;
                            }
                        }
                    }
                }
            }
        }



        #endregion

        #region EQ_equipo

        public List<EqEquipoDTO> ListarCentralesXEmpresaGEN(string idsEmpresa)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetEqEquipoRepository().ListarCentralesXEmpresaGEN(idsEmpresa);
        }

        #endregion

        #region Si_fuenteenergia

        public List<SiFuenteenergiaDTO> ListaCombustible()
        {
            var lista = FactorySic.GetSiFuenteenergiaRepository().List().Where(x => x.Estcomcodi >= 1 && x.Estcomcodi < 3).ToList();
            return lista;
        }

        #endregion

        #region Util

        /// <summary>
        /// Encuentra el indice inicial para Horas de Operacion en un periodo de horas
        /// </summary>
        /// <param name="dFecha"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private Int32 IndiceInicio(DateTime dFecha, int periodo)
        {
            Int32 iHora, iMinuto, iCol, resto;
            iHora = Int32.Parse(dFecha.ToString("HH"));
            iMinuto = dFecha.Minute;
            resto = iMinuto % periodo;
            if (resto == 0)
            {
                if (iHora == 0)
                    iCol = 1;
                else
                    iCol = iHora * (60 / periodo) + (iMinuto / periodo);
            }
            else
                iCol = iHora * (60 / periodo) + (iMinuto / periodo) + 1;
            //iCol = iHora * 4 + (iMinuto / 15) + 1;////// (iMinuto % 15) Modificado 10/12/2007
            return iCol;
        }

        /// <summary>
        /// Encuentra el indice final para Horas de Operacion en un periodo de horas
        /// </summary>
        /// <param name="dFecha"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private int IndiceFinal(DateTime dFecha, int periodo)
        {
            Int32 iHora;
            iHora = Int32.Parse(dFecha.ToString("HH"));
            return iHora * (60 / periodo);
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

        /// <summary>
        /// Recodifica tipo de central a integrante y no integrante
        /// </summary>
        /// <param name="idsCentralInt"></param>
        /// <returns></returns>
        public string GeneraCodCentralIntegrante(string idsCentralInt)
        {
            StringBuilder strCentralInt = new StringBuilder();
            switch (idsCentralInt)
            {
                case ConstantesStockCombustibles.ValueCbCoes: // COES
                    strCentralInt.Append(ConstantesStockCombustibles.StrCtralIntCoes);
                    break;
                case ConstantesStockCombustibles.ValueCbNoCoes: // NO COES
                    strCentralInt.Append(ConstantesStockCombustibles.StrCtralIntNoCoes);
                    break;
                default:
                    strCentralInt.Append(ConstantesStockCombustibles.StrCtralIntCoes);
                    strCentralInt = strCentralInt.Append(",").Append(ConstantesStockCombustibles.StrCtralIntNoCoes);
                    break;

            }

            return strCentralInt.ToString();
        }

        /// <summary>
        /// Completa el titulo en el reporte grafico de combustible de acuerdo al id del estado de combustible
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="idEstadoFisico"></param>
        /// <returns></returns>
        public static string GeneraTituloListado(string titulo, string idEstadoFisico)
        {
            StringBuilder strTitulo = new StringBuilder();
            strTitulo.Append(titulo);
            int sw = 0;
            List<string> lstEstado = new List<string>();
            lstEstado = idEstadoFisico.Split(',').ToList();

            foreach (var reg in lstEstado)
            {
                if (sw == 0) // inicio de cadena
                {
                    switch (reg)
                    {
                        case ConstantesStockCombustibles.ValueCbEstadoLiquido: //Liquido
                            strTitulo.Append(ConstantesStockCombustibles.TxtLiquido);
                            break;
                        case ConstantesStockCombustibles.ValueCbEstadoSolido: //sólido
                            strTitulo.Append(ConstantesStockCombustibles.TxtSolido);
                            break;
                        case ConstantesStockCombustibles.ValueCbEstadoGaseoso: //gaseoso
                            strTitulo.Append(ConstantesStockCombustibles.TxtGaseoso);
                            break;
                    }
                    sw++;
                }
                else
                {
                    switch (reg)
                    {
                        case ConstantesStockCombustibles.ValueCbEstadoLiquido: //Liquido
                            strTitulo.Append(", ").Append(ConstantesStockCombustibles.TxtLiquido);
                            break;
                        case ConstantesStockCombustibles.ValueCbEstadoSolido: //sólido
                            strTitulo.Append(", ").Append(ConstantesStockCombustibles.TxtSolido);
                            break;
                        case ConstantesStockCombustibles.ValueCbEstadoGaseoso: //gaseoso
                            strTitulo.Append(", ").Append(ConstantesStockCombustibles.TxtGaseoso);
                            break;
                    }
                }

            }
            return strTitulo.ToString();
        }

        /// <summary>
        /// Completa los parametros del DTO Formato
        /// </summary>
        /// <param name="formato"></param>
        public void GetSizeFormato(COES.Dominio.DTO.Sic.MeFormatoDTO formato)
        {

            formato.FechaInicio = formato.FechaProceso;
            formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte - 1);
            formato.RowPorDia = 24;
            formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
            formato.FechaPlazoIni = formato.FechaProceso.AddDays(formato.Formatdiaplazo);
            if (formato.Formatdiaplazo == 0)
            {
                formato.FechaPlazo = formato.FechaProceso.AddDays(1).AddMinutes(formato.Formatminplazo);
            }
            else
            {
                formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
            }
        }

        #endregion

        #region Metodos Vistas para Presentacion

        /// <summary>
        /// Devuelve el nombre del periodo segun sea diario, semanal o mensual
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public string GetNombrePeriodo(DateTime fecha, int periodo)
        {
            string fechaPeriodo = string.Empty;
            switch (periodo)
            {
                case 1:
                    fechaPeriodo = fecha.ToString(ConstantesBase.FormatoFecha);
                    break;
                case 2:
                    fechaPeriodo = fecha.Year.ToString() + " Sem " + EPDate.f_numerosemana(fecha);
                    break;
                case 3:
                case 5:
                    fechaPeriodo = fecha.Year.ToString() + " " + EPDate.f_NombreMes(fecha.Month);
                    break;
            }
            return fechaPeriodo;
        }

        /// <summary>
        /// Devuelve string html de la cebecera de reportes de cumplimiento
        /// </summary>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="cont"></param>
        /// <returns></returns>
        public string GetHtmlCabecera(DateTime fInicio, DateTime fFin, int idPeriodo, ref int cont)
        {
            StringBuilder strHtml = new StringBuilder();
            switch (idPeriodo)
            {
                case 1:
                    for (var f = fInicio; f <= fFin; f = f.AddDays(1))
                    {
                        strHtml.Append("<th>" + GetNombrePeriodo(f, idPeriodo) + "</th>");
                        cont++;
                    }
                    break;
                case 2:
                    for (var f = fInicio; f <= fFin; f = f.AddDays(7))
                    {
                        strHtml.Append("<th>" + GetNombrePeriodo(f, idPeriodo) + "</th>");
                        cont++;
                    }
                    break;
                case 3:
                case 5:
                    for (var f = fInicio; f <= fFin; f = f.AddMonths(1))
                    {
                        strHtml.Append("<th>" + GetNombrePeriodo(f, idPeriodo) + "</th>");
                        cont++;
                    }
                    break;
            }
            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el View de reportes de Cumplimiento.
        /// </summary>
        /// <returns></returns>
        public string GeneraViewCumplimiento(string sEmpresas, DateTime fInicio, DateTime fFin, int idFormato, int idPeriodo, string envio, string estado)
        {
            StringBuilder strHtml = new StringBuilder();
            var listaEmpresa = sEmpresas.Split(',');
            List<SiEmpresaDTO> listadoEmpresa = FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormatoPorEstado(idFormato, estado).Where(x => listaEmpresa.Contains(x.Emprcodi.ToString())).ToList();
            List<SiEmpresaDTO> empresas = new List<SiEmpresaDTO>();
            List<MeEnvioDTO> listaEnvio = FactorySic.GetMeEnvioRepository().ObtenerReporteCumplimiento(sEmpresas, idFormato, fInicio, fFin).ToList();

            //- Debemos hacer jugada con los datos para imprimier el reporte correctamente
            if(envio == ConstantesAppServicio.SI)            
                empresas = listadoEmpresa.Where(x => listaEnvio.Any(y => x.Emprcodi == y.Emprcodi)).ToList();            
            else if(envio == ConstantesAppServicio.NO)            
                empresas = listadoEmpresa.Where(x => !listaEnvio.Any(y => x.Emprcodi == y.Emprcodi)).ToList();            
            else
                empresas = listadoEmpresa;
            
            strHtml.Append("<table border='1' width:'100%' class='pretty tabla-icono cell-border' cellspacing='0'  id='tabla'>");
            ///Cabecera de Reporte
            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th width='300px'>EMPRESAS</th>");
            int cont = 0;
            string htmlCabecera = GetHtmlCabecera(fInicio, fFin, idPeriodo, ref cont);
            strHtml.Append(htmlCabecera);
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            /// Fin de cabecera de Reporte
            strHtml.Append("<tbody>");
            string colorFondo = string.Empty;
            foreach (var emp in empresas)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + emp.Emprnomb + "</td>");
                for (int i = 0; i < cont; i++)
                {
                    colorFondo = "style='background-color:orange;color:white'";
                    var find = listaEnvio.Find(x => x.Emprcodi == emp.Emprcodi && x.Enviofechaperiodo == fInicio.AddDays(i));
                    if (find != null)
                    {
                        if (find.Envioplazo == "P")
                            colorFondo = "style='background-color:SteelBlue;color:white'";
                        strHtml.Append("<td " + colorFondo + " title='" + ((DateTime)find.Enviofecha).ToString("hh:mm:ss")
                            + "'>" + ((DateTime)find.Enviofecha).ToString(ConstantesBase.FormatoFechaHora) + "</td>");
                    }
                    else
                        strHtml.Append("<td >--</td>");
                }
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();

        }

        /// <summary>
        /// Lista de cumplimiento de envios de hidrologia,
        /// </summary>
        /// <param name="sEmpresas"></param>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="idFormato"></param>
        /// <param name="nombreFormato"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="rutaArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GeneraExcelCumplimiento(string sEmpresas, DateTime fInicio, DateTime fFin, int idFormato, string rutaArchivo,
            string rutaLogo, string envio, string estado)
        {
            try
            {
               
                FileInfo newFile = new FileInfo(rutaArchivo);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(rutaArchivo);
                }
                var listaEmpresa = sEmpresas.Split(',');
                List<MeFormatoDTO> formatos = this.ListaFormatos().OrderBy(x => x.Formatdescrip).Where(x => x.Formatcodi == idFormato || idFormato == -1).ToList();


                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    int indicador = 0;
                    foreach (MeFormatoDTO formato in formatos)
                    {
                        indicador++;
                        List<SiEmpresaDTO> listadoEmpresa = FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormatoPorEstado(formato.Formatcodi, estado).
                            Where(x => listaEmpresa.Contains(x.Emprcodi.ToString())).ToList();
                        List<SiEmpresaDTO> empresas = new List<SiEmpresaDTO>();
                        List<MeEnvioDTO> listaEnvio = FactorySic.GetMeEnvioRepository().ObtenerReporteCumplimiento(sEmpresas, formato.Formatcodi, fInicio, fFin);

                        if (envio == ConstantesAppServicio.SI)
                            empresas = listadoEmpresa.Where(x => listaEnvio.Any(y => x.Emprcodi == y.Emprcodi)).ToList();
                        else if (envio == ConstantesAppServicio.NO)
                            empresas = listadoEmpresa.Where(x => !listaEnvio.Any(y => x.Emprcodi == y.Emprcodi)).ToList();
                        else
                            empresas = listadoEmpresa;
                         
                        MeFormatoDTO formatoDTO = FactorySic.GetMeFormatoRepository().GetById(formato.Formatcodi);
                        int cont = 0;
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(formato.Formatdescrip);
                        ws = xlPackage.Workbook.Worksheets[formato.Formatdescrip];
                        AddImageAdicional(ws, 1, 0, rutaLogo, indicador);
                        var fontTabla = ws.Cells[3, 2].Style.Font;
                        fontTabla.Size = 14;
                        fontTabla.Bold = true;
                        ws.Cells[3, 2].Value = "REPORTE CUMPLIMIENTO:";
                        ws.Cells[3, 3].Value = formato.Formatnombre;
                        GetExcelCabecera(ws, fInicio, fFin, (int)formatoDTO.Formatperiodo, ref cont);
                        int filLeyenda = empresas.Count() + 6;
                        ws.Cells[filLeyenda + 1, 2].Value = "LEYENDA:";
                        ws.Cells[filLeyenda + 2, 2].Value = "EN PLAZO";
                        ws.Cells[filLeyenda + 3, 2].Value = "FUERA DE PLAZO";
                        var borderLeyenda = ws.Cells[filLeyenda + 1, 2, filLeyenda + 3, 2].Style.Border;
                        borderLeyenda.Bottom.Style = borderLeyenda.Top.Style = borderLeyenda.Left.Style = borderLeyenda.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[filLeyenda + 2, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[filLeyenda + 2, 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 180));
                        ws.Cells[filLeyenda + 3, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[filLeyenda + 3, 2].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                        ws.Cells[filLeyenda + 2, 2].Style.Font.Color.SetColor(Color.White);
                        ws.Cells[filLeyenda + 3, 2].Style.Font.Color.SetColor(Color.White);

                        int fila = 6;
                        int col = 2;
                        string colorFondo = string.Empty;
                        foreach (var emp in empresas)
                        {
                            ws.Cells[fila, col].Value = emp.Emprnomb;
                            for (int i = 0; i < cont; i++)
                            {
                                colorFondo = "style='background-color:orange;color:white'";
                                var find = listaEnvio.Find(x => x.Emprcodi == emp.Emprcodi && x.Enviofechaperiodo == fInicio.AddDays(i));
                                if (find != null)
                                {
                                    ws.Cells[fila, col + i + 1].Value = ((DateTime)find.Enviofecha).ToString(ConstantesBase.FormatoFechaHora);
                                    if (find.Envioplazo == "P")
                                    {
                                        ws.Cells[fila, col + i + 1].StyleID = ws.Cells[filLeyenda + 2, 2].StyleID;
                                    }
                                    else
                                    {
                                        ws.Cells[fila, col + i + 1].StyleID = ws.Cells[filLeyenda + 3, 2].StyleID;
                                    }
                                }
                            }
                            fila++;
                        }
                        // borde de region de datos
                        var borderReg = ws.Cells[5, 2, fila - 1, cont + 2].Style.Border;
                        borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                        using (ExcelRange r = ws.Cells[5, 2, 5, cont + 2])
                        {
                            r.Style.Font.Color.SetColor(Color.White);
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 210));
                        }
                        if (fila - 1 >= 6)
                        {
                            using (ExcelRange r = ws.Cells[6, 2, fila - 1, 2])
                            {
                                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(173, 216, 230));
                            }
                        }
                        //ws.Column(col).AutoFit();
                        ExcelRange rg = ws.Cells[6, 1, 1 + fila, cont + 2];
                        rg.AutoFitColumns();
                    }

                    xlPackage.Save();
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }


        }
        /// <summary>
        /// Genera Reporte Excevl de Envios formatos de Combustibles
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GeneraExcelEnvio(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin, string rutaArchivo,
            string rutaLogo)
        {
            FileInfo newFile = new FileInfo(rutaArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }
            var lista = FactorySic.GetMeEnvioRepository().GetListaMultipleXLS(idsEmpresa, ConstantesAppServicio.ParametroDefecto,
                idsFormato, idsEstado, fechaIni, fechaFin);

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("Envío");
                ws = xlPackage.Workbook.Worksheets["Envío"];
                AddImage(ws, 1, 0, rutaLogo);
                ws.Cells[1, 3].Value = "REPORTE HISTORICO DE ENVÍOS";
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                //Borde, font cabecera de Tabla Fecha
                var borderFecha = ws.Cells[3, 2, 3, 3].Style.Border;
                borderFecha.Bottom.Style = borderFecha.Top.Style = borderFecha.Left.Style = borderFecha.Right.Style = ExcelBorderStyle.Thin;
                var fontTabla = ws.Cells[3, 2, 4, 3].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";
                fontTabla.Bold = true;
                ws.Cells[3, 2].Value = "Fecha:";
                ws.Cells[3, 3].Value = DateTime.Now.ToString(ConstantesBase.FormatoFechaHora);
                var fill = ws.Cells[5, 2, 5, 11].Style;
                fill.Fill.PatternType = ExcelFillStyle.Solid;
                fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
                var border = ws.Cells[5, 2, 5, 11].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                ws.Cells[5, 2].Value = "ID ENVÍO";
                ws.Cells[5, 3].Value = "FECHA PERIODO";
                ws.Cells[5, 4].Value = "EMPRESA";
                ws.Cells[5, 5].Value = "ESTADO";
                ws.Cells[5, 6].Value = "CUMPLIMIENTO";
                ws.Cells[5, 7].Value = "FECHA ENVÍO";
                ws.Cells[5, 8].Value = "FORMATO";
                ws.Cells[5, 9].Value = "USUARIO";
                ws.Cells[5, 10].Value = "CORREO";
                ws.Cells[5, 11].Value = "TELÉFONO";

                ws.Column(1).Width = 5;
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 30;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 30;
                ws.Column(9).Width = 30;
                ws.Column(10).Width = 30;
                ws.Column(11).Width = 20;

                int row = 6;
                int column = 2;
                if (lista.Count > 0)
                {
                    foreach (var reg in lista)
                    {
                        ws.Cells[row, column].Value = reg.Enviocodi;
                        ws.Cells[row, column + 1].Value = reg.FechaPeriodo;
                        ws.Cells[row, column + 2].Value = reg.Emprnomb;
                        ws.Cells[row, column + 3].Value = reg.Estenvnombre;
                        var eplazo = "";
                        if (reg.Envioplazo == "F")
                        {
                            eplazo = "Fuera de Plazo";
                        }
                        else
                        {
                            eplazo = "En Plazo";
                        }
                        ws.Cells[row, column + 4].Value = eplazo;
                        DateTime fechaenvio = (DateTime)reg.Enviofecha;
                        ws.Cells[row, column + 5].Value = fechaenvio.ToString(ConstantesBase.FormatoFechaHora); ;
                        ws.Cells[row, column + 6].Value = reg.Formatnombre;
                        ws.Cells[row, column + 7].Value = reg.Username;
                        ws.Cells[row, column + 8].Value = reg.Lastuser;
                        ws.Cells[row, column + 9].Value = reg.Usertlf;
                        border = ws.Cells[row, 2, row, 11].Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        fontTabla = ws.Cells[row, 2, row, 11].Style.Font;
                        fontTabla.Size = 8;
                        fontTabla.Name = "Calibri";
                        row++;
                    }
                }
                //ws.Column(column + 11).AutoFit();
                xlPackage.Save();
            }
        }



        /// <summary>
        /// Obtiene Cabecera de reporte excel de cumplimiento
        /// </summary>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="cont"></param>
        /// <returns></returns>
        private void GetExcelCabecera(ExcelWorksheet ws, DateTime fInicio, DateTime fFin, int idPeriodo, ref int cont)
        {
            int col = 2;
            ws.Cells[5, col].Value = "Empresa/Fecha";
            col++;
            for (var f = fInicio; f <= fFin; f = f.AddDays(1))
            {
                ws.Cells[5, col].Value = GetNombrePeriodo(f, idPeriodo);
                ws.Cells[5, col].AutoFitColumns();
                cont++;
                col++;
            }
        }


        #endregion

        #region Metodos Exportar a Excel

        /// <summary>
        /// Permite Exportar el histórico de Combustibles Acumulados de las Centrales Termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaPlantilla"></param>
        /// <param name="rutaNombreArchivo"></param>
        public void GenerarArchivoExcelAcumulado(List<MeMedicionxintervaloDTO> listaReporte, string fechaIni, string fechaFin,
            string rutaNombreArchivo, string rutaLogo)
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
                ws = xlPackage.Workbook.Worksheets.Add("ACUMULADO");
                ws = xlPackage.Workbook.Worksheets["ACUMULADO"];
                string titulo = "Reporte de Combustibles Acumulados de las Centrales Termoeléctricas";


                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaIni, fechaFin, rutaLogo);
                ConfiguracionHojaExcelAcumulado(ws, listaReporte);

                xlPackage.Save();
            }

        }

        /// <summary>
        /// Permite Exportar el histórico de Combustibles Acumulados de las Centrales Termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaPlantilla"></param>
        /// <param name="rutaNombreArchivo"></param>
        public void GenerarArchivoExcelAcumuladoDet(List<MeMedicionxintervaloDTO> listaReporte, string fechaIni, string fechaFin,
            string rutaNombreArchivo, string rutaLogo)
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
                ws = xlPackage.Workbook.Worksheets.Add("ACUMULADO");
                ws = xlPackage.Workbook.Worksheets["ACUMULADO"];
                string titulo = "Reporte de Combustibles Acumulados de las Centrales Termoeléctricas";


                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaIni, fechaFin, rutaLogo);
                ConfiguracionHojaExcelAcumuladoDet(ws, listaReporte);

                xlPackage.Save();
            }

        }

        /// <summary>
        /// Permite Exportar el histórico de Stock de Combustibles de las Centrales Termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaPlantilla"></param>
        /// <param name="rutaNombreArchivo"></param>
        public void GenerarArchivoExcelStock(List<MeMedicionxintervaloDTO> listaReporte, string fechaIni, string fechaFin,
            string rutaNombreArchivo, string rutaLogo)
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
                ws = xlPackage.Workbook.Worksheets.Add("STOCK");
                ws = xlPackage.Workbook.Worksheets["STOCK"];
                string titulo = "Reporte de Stock de Combustibles";


                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaIni, fechaFin, rutaLogo);
                ConfiguracionHojaExcelStock(ws, listaReporte);

                xlPackage.Save();
            }

        }

        /// <summary>
        /// Permite Exportar el histórico de Consumo de Combustibles de las Centrales Termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaPlantilla"></param>
        /// <param name="rutaNombreArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GenerarArchivoExcelConsumo(List<MeMedicionxintervaloDTO> listaReporte, string fechaIni, string fechaFin,
            string rutaNombreArchivo, string rutaLogo, string idsEstado)
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
                ws = xlPackage.Workbook.Worksheets.Add("CONSUMO");
                ws = xlPackage.Workbook.Worksheets["CONSUMO"];
                string titulo = "Reporte de Consumo de Combustibles";
                string titulo2 = GeneraTituloListado("CONSUMO DE COMBUSTIBLES - ", idsEstado);

                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaIni, fechaFin, rutaLogo);
                ConfiguracionHojaExcelConsumo(ws, 6, listaReporte, titulo2);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite Exportar el histórico de Presión de Gas o Temperatura Ambiente de las Centrales Termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idParametro"></param>
        /// <param name="rutaPlantilla"></param>
        /// <param name="rutaNombreArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GenerarExcelPresTemp(List<MeMedicion24DTO> listaReporte, string fechaIni, string fechaFin, int idParametro,
            string rutaNombreArchivo, string rutaLogo)
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
                string titulo = string.Empty;
                string sheetName = string.Empty;
                if (idParametro == 1) // Presión de Gas
                {
                    sheetName = "PRESION-GAS";
                    titulo = "Presiones de Gas Natural de las Centrales Termoeléctricas";
                }
                else
                {
                    sheetName = "TEMPERATURA";
                    titulo = "Temperatura Ambiente de las Centrales Termoeléctricas";
                }
                ws = xlPackage.Workbook.Worksheets.Add(sheetName);
                ws = xlPackage.Workbook.Worksheets[sheetName];

                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaIni, fechaFin, rutaLogo);
                ConfiguracionHojaExcelPresTemp(ws, 6, listaReporte, idParametro);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Muestra los datos de consulta para las presiones horarias ó temperatura ambiente de gas natural en el reporte con gráfico
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        /// <param name="nfil"></param>
        /// <param name="ncol"></param>
        /// <param name="idParametro"></param>
        public void ConfiguracionHojaExcelPresTempGrafico(ExcelWorksheet ws, List<MeMedicion24DTO> lista, out int nfil, out int ncol, int idParametro, DateTime fechaIni)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            List<MeMedicion24DTO> listPtosMed = new List<MeMedicion24DTO>();
            if (idParametro == 1) // Presión de Gas
            {
                listPtosMed = lista.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Gruponomb, x.Ptomedielenomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Gruponomb = y.Key.Gruponomb,
                                     Ptomedielenomb = y.Key.Ptomedielenomb,
                                 }
                                 ).ToList();
            }
            else //Temperatura Ambiente
            {
                listPtosMed = lista.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Equinomb = y.Key.Equinomb,
                                 }
                                 ).ToList();
            }
            nfil = 24;
            ncol = listPtosMed.Count;
            int xFil = 6;
            using (ExcelRange r = ws.Cells[xFil, 2, xFil, 2 + ncol])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 15;
            ws.Cells[xFil, 2].Value = "HORA";
            int i = 1;
            foreach (var reg in listPtosMed)
            {
                if (idParametro == 1) // Presión de Gas
                {
                    ws.Cells[xFil, 2 + i].Value = reg.Ptomedielenomb;
                }
                else
                {
                    ws.Cells[xFil, 2 + i].Value = reg.Ptomedibarranomb;
                }

                ws.Column(2 + i).Width = 15;
                i++;
            }

            i = 1;
            int j = 0;
            for (int k = 1; k <= nfil; k++)
            {
                j = 0;
                string hora = ("0" + (k - 1).ToString()).Substring(("0" + (k - 1).ToString()).Length - 2, 2) + ":00";
                ws.Cells[xFil + i, 2 + j].Value = hora;
                j++;
                foreach (var col in listPtosMed)
                {
                    var reg = lista.Find(x => x.Ptomedicodi == col.Ptomedicodi && x.Medifecha == fechaIni);

                    if (reg != null)
                    {
                        decimal? valor;
                        valor = (decimal?)reg.GetType().GetProperty("H" + k).GetValue(reg, null);
                        if (valor != null)
                            ws.Cells[xFil + i, 2 + j].Value = valor;
                        else
                        {
                            valor = 0;
                            //ws.Cells[xFil + i, 2 + j].Value = valor;
                        }
                        j++;
                    }
                    else
                    {
                        decimal valor = 0;
                        //ws.Cells[xFil + i, 2 + j].Value = valor;
                        j++;
                    }
                }
                i++;
            }

            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[7, 3, 7 + nfil - 1, 3 + ncol - 1])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[6, 2, 6 + nfil, 2 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[6, 2, 6 + nfil, 2 + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Exporta el reporte historico de Disponibilidad de Gas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idParametro"></param>
        /// <param name="rutaPlantilla"></param>
        /// <param name="rutaNombreArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GenerarArchivoExcelDisponibilidadGas(List<MeMedicionxintervaloDTO> listaReporte, string fechaIni, string fechaFin,
            string rutaNombreArchivo, string rutaLogo)
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
                string sheetName = "DISPONIBILIDAD DE GAS";
                string titulo = "Disponibilidad de Gas Natural de las Centrales Termoeléctricas";
                ws = xlPackage.Workbook.Worksheets.Add(sheetName);
                ws = xlPackage.Workbook.Worksheets[sheetName];
                ws.View.FreezePanes(8, 1);
                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaIni, fechaFin, rutaLogo);
                ConfiguracionHojaExcelDisponibilidad(ws, listaReporte, fechaIni, fechaFin);
                xlPackage.Save();
            }

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
        public void GenerarArchivoExcelQuemaGas(List<MeMedicionxintervaloDTO> listaReporte, string fechaIni, string fechaFin,
            string rutaNombreArchivo, string rutaLogo)
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
                string titulo = "Quema de Gas Natural de las Centrales Termoeléctricas";
                string sheetName = "QUEMA DE GAS";
                ws = xlPackage.Workbook.Worksheets.Add(sheetName);
                ws = xlPackage.Workbook.Worksheets[sheetName];

                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaIni, fechaFin, rutaLogo);
                ConfiguracionHojaExcelQuema(ws, listaReporte);
                xlPackage.Save();
            }
        }

        //genera archivo de grafico Stock de Combustible de las centrales hidroelectricas
        public void GenerarArchivoGrafStock(List<MeMedicionxintervaloDTO> listaReporte, string fechaIni, string fechaFin,
            string rutaNombreArchivo, string rutaLogo)
        {
            int nfil;
            int ncol;
            FileInfo newFile = new FileInfo(rutaNombreArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;


                ws = xlPackage.Workbook.Worksheets.Add("STOCK");
                ws = xlPackage.Workbook.Worksheets["STOCK"];
                string titulo = "Reporte Gráfico de Stock de Combustibles";
                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaIni, fechaFin, rutaLogo);
                ConfiguracionHojaExcelStockGrafico(ws, listaReporte, out nfil, out ncol, 1);

                string xAxisTitle = "Centrales Térmoelectricas";
                string yAxisTitle = "Unidades";
                AddGraficoLineas(ws, nfil, ncol, xAxisTitle, yAxisTitle, titulo);
                //AddGraficoColumn(ws, 7, 3, nfil, ncol-1);

                ws = xlPackage.Workbook.Worksheets.Add("RECEPCIÓN EVOLUCIÓN");
                ws = xlPackage.Workbook.Worksheets["RECEPCIÓN EVOLUCIÓN"];
                titulo = "Reporte Gráfico de Recepción Evolución de Combustibles";
                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaIni, fechaFin, rutaLogo);
                ConfiguracionHojaExcelStockGrafico(ws, listaReporte, out nfil, out ncol, 2);

                //string xAxisTitle = "Centrales Térmoelectricas";
                //string yAxisTitle = "Unidades";
                //AddGraficoLineas(ws, nfil, ncol, xAxisTitle, yAxisTitle, titulo);
                AddGraficoLineas(ws, nfil, ncol, xAxisTitle, yAxisTitle, titulo);

                ws = xlPackage.Workbook.Worksheets.Add("RECEPCIÓN ACUMULADO");
                ws = xlPackage.Workbook.Worksheets["RECEPCIÓN ACUMULADO"];
                titulo = "Reporte Gráfico de Recepción Acumulada de Combustibles";
                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaIni, fechaFin, rutaLogo);
                ConfiguracionHojaExcelStockGrafico(ws, listaReporte, out nfil, out ncol, 2);

                xAxisTitle = "Centrales Térmoelectricas";
                yAxisTitle = "Unidades";
                //AddGraficoLineas(ws, nfil, ncol, xAxisTitle, yAxisTitle, titulo);
                AddGraficoColumn(ws, 7, 3, nfil, ncol - 1);


                xlPackage.Save();
            }

        }

        /// <summary>
        /// genera archivo de grafico Consumo de Combustible de las centrales hidroelectricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaNombreArchivo"></param>
        /// <param name="rutaLogo"></param>
        /// <param name="tipoGrafico"></param>
        public void GenerarArchivoGrafConsumo(List<MeMedicionxintervaloDTO> listaReporte, string fechaIni, string fechaFin,
            string rutaNombreArchivo, string rutaLogo, int tipoGrafico)
        {
            int nfil;
            int ncol;

            FileInfo newFile = new FileInfo(rutaNombreArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }
            List<MeMedicionxintervaloDTO> listTipoPtosMed = listaReporte.GroupBy(x => new { x.Tptomedicodi, x.Fenergnomb })
                               .Select(y => new MeMedicionxintervaloDTO()
                               {
                                   Tptomedicodi = y.Key.Tptomedicodi,
                                   Fenergnomb = y.Key.Fenergnomb
                               }
                               ).ToList();

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                foreach (var reg in listTipoPtosMed)
                {
                    var nombreTipoPtoMed = reg.Fenergnomb.Trim();
                    ws = xlPackage.Workbook.Worksheets.Add(nombreTipoPtoMed);
                    ws = xlPackage.Workbook.Worksheets[nombreTipoPtoMed];
                    string titulo = "Reporte Gráfico de Consumo de Combustibles - " + nombreTipoPtoMed;
                    ConfiguraEncabezadoHojaExcel(ws, titulo, fechaIni, fechaFin, rutaLogo);
                    ConfiguracionHojaExcelConsumoGrafico(ws, listaReporte, reg.Tptomedicodi, out nfil, out ncol, tipoGrafico);

                    string xAxisTitle = "Centrales Térmoelectricas";
                    string yAxisTitle = "Unidades";
                    if (tipoGrafico == 1)
                        AddGraficoLineas(ws, nfil, ncol, xAxisTitle, yAxisTitle, titulo);
                    else
                        AddGraficoPie(ws, ncol, titulo);
                }
                xlPackage.Save();
            }

        }


        //genera archivo de grafico de Presión ó Temperatura ambiente de las centrales hidroeléctricas
        public void GenerarArchivoGrafPresTemp(List<MeMedicion24DTO> listaReporte, DateTime fechaIni,
            string rutaNombreArchivo, string rutaLogo, int idParametro)
        {
            int nfil;
            int ncol;
            FileInfo newFile = new FileInfo(rutaNombreArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }
            string nameWorkSheet = String.Empty;
            string titulo = String.Empty;
            if (idParametro == 1) // Presion gas
            {
                nameWorkSheet = "PRESION-GAS";
                titulo = "Reporte Gráfico de Presion Horaria de Gas Natural(kPa)";
            }
            else //Temperatura
            {
                nameWorkSheet = "TEMPERATURA AMBIENTE";
                titulo = "Reporte Gráfico de Temperatura Ambiente(ºC)";
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add(nameWorkSheet);
                ws = xlPackage.Workbook.Worksheets[nameWorkSheet];

                ConfiguraEncabezadoHojaExcel2(ws, titulo, fechaIni, rutaLogo);
                ConfiguracionHojaExcelPresTempGrafico(ws, listaReporte, out nfil, out ncol, idParametro, fechaIni);

                string xAxisTitle = "Centrales Térmoelectricas";
                string yAxisTitle = "Unidades";
                AddGraficoLineas(ws, nfil, ncol, xAxisTitle, yAxisTitle, titulo);
                xlPackage.Save();
            }

        }

        //******************************************************
        /// <summary>
        /// Muestra los datos de consulta para los Stock de Combustibles
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        public void ConfiguracionHojaExcelStock(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> lista)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            int nfil = lista.Count;
            int xFil = 6;
            using (ExcelRange r = ws.Cells[xFil, 2, xFil, 10])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells[xFil, 2].Value = "FECHA";
            ws.Cells[xFil, 3].Value = "EMPRESA";
            ws.Cells[xFil, 4].Value = "CENTRAL";
            ws.Cells[xFil, 5].Value = "TIPO DE COMBUSTIBLE";
            ws.Cells[xFil, 6].Value = "INICIO";
            ws.Cells[xFil, 7].Value = "FINAL DECLARADO";
            ws.Cells[xFil, 8].Value = "RECEPCIÓN";
            //ws.Cells[xFil, 9].Value = "CONSUMO";
            ws.Cells[xFil, 9].Value = "UNIDADES";
            ws.Cells[xFil, 10].Value = "OBSERVACIONES";

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 20;
            ws.Column(3).Width = 35;
            ws.Column(4).Width = 28;
            ws.Column(5).Width = 18;
            ws.Column(6).Width = 15;
            ws.Column(7).Width = 15;
            ws.Column(8).Width = 15;
            //ws.Column(9).Width = 15;
            ws.Column(9).Width = 10;
            ws.Column(10).Width = 60;

            int row = 7;
            int column = 2;

            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha);
                    ws.Cells[row, column + 1].Value = reg.Emprnomb;
                    ws.Cells[row, column + 2].Value = reg.Equinomb;
                    ws.Cells[row, column + 3].Value = reg.Fenergnomb;
                    ws.Cells[row, column + 4].Value = reg.Medinth1;
                    ws.Cells[row, column + 5].Value = reg.H1Fin;
                    ws.Cells[row, column + 6].Value = reg.H1Recep;
                    // ws.Cells[row, column + 7].Value = (reg.Medinth1 + reg.H1Recep) - reg.H1Fin; 
                    ws.Cells[row, column + 7].Value = reg.Tipoinfoabrev;
                    ws.Cells[row, column + 8].Value = reg.Medintdescrip;
                    row++;
                }
            }
            else
            {
                //No existen registros);
            }


            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[7, 2, row, 9])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[6, 2, row, 10].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[6, 2, row - 1, 10].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Muestra los datos de consulta para los Combustibles Acumulados
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        public void ConfiguracionHojaExcelAcumulado(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> lista)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();


            int nfil = lista.Count;
            int xFil = 6;
            using (ExcelRange r = ws.Cells[xFil, 2, xFil, 6])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells[xFil, 2].Value = "EMPRESA";
            ws.Cells[xFil, 3].Value = "CENTRAL";
            ws.Cells[xFil, 4].Value = "TIPO DE COMBUSTIBLE";
            ws.Cells[xFil, 5].Value = "ACUMULADO";
            ws.Cells[xFil, 6].Value = "UNIDADES";



            int row = 7;
            int column = 2;

            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.Emprnomb;
                    ws.Cells[row, column + 1].Value = reg.Equinomb;
                    ws.Cells[row, column + 2].Value = reg.Fenergnomb;
                    ws.Cells[row, column + 3].Value = reg.H1Recep;
                    ws.Cells[row, column + 4].Value = reg.Tipoinfoabrev;
                    row++;
                }
            }
            else
            {
                //No existen registros);
            }

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 35;
            ws.Column(3).Width = 28;
            ws.Column(4).Width = 18;
            ws.Column(5).Width = 15;
            ws.Column(6).Width = 15;



            //for (var j = 2; j < 3 + ncol; j++)
            //{
            //    ws.Column(j).AutoFit(ws.Cells[8, j].Text.Length + 1);
            //}

            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[7, 2, row, 6])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[6, 2, row, 6].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[6, 2, row - 1, 6].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Muestra los datos de consulta para los Combustibles Acumulados
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        public void ConfiguracionHojaExcelAcumuladoDet(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> lista)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();


            int nfil = lista.Count;
            int xFil = 6;
            using (ExcelRange r = ws.Cells[xFil, 2, xFil, 7])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells[xFil, 2].Value = "FECHA";
            ws.Cells[xFil, 3].Value = "EMPRESA";
            ws.Cells[xFil, 4].Value = "CENTRAL";
            ws.Cells[xFil, 5].Value = "TIPO DE COMBUSTIBLE";
            ws.Cells[xFil, 6].Value = "RECEPCIÓN";
            ws.Cells[xFil, 7].Value = "UNIDADES";



            int row = 7;
            int column = 2;

            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    if (reg.H1Recep != null && reg.H1Recep > 0)
                    {
                        ws.Cells[row, column].Value = reg.Medintfechaini.ToString(ConstantesBase.FormatoFecha);
                        ws.Cells[row, column + 1].Value = reg.Emprnomb;
                        ws.Cells[row, column + 2].Value = reg.Equinomb;
                        ws.Cells[row, column + 3].Value = reg.Fenergnomb;
                        ws.Cells[row, column + 4].Value = reg.H1Recep;
                        ws.Cells[row, column + 5].Value = reg.Tipoinfoabrev;
                        row++;
                    }
                }
            }
            else
            {
                //No existen registros);
            }

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 28;
            ws.Column(3).Width = 35;
            ws.Column(4).Width = 28;
            ws.Column(5).Width = 18;
            ws.Column(6).Width = 15;
            ws.Column(7).Width = 15;

            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[7, 2, row, 7])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[6, 2, row, 7].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[6, 2, row - 1, 7].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Muestra los datos de consulta para los Consumos de Combustibles
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        public void ConfiguracionHojaExcelConsumo(ExcelWorksheet ws, int xFil, List<MeMedicionxintervaloDTO> lista, string titulo2)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            int nfil = lista.Count;

            var listaFechas = lista.Select(x => x.Medintfechaini).Distinct().ToList();
            int ncol = listaFechas.Count + 7;
            List<MeMedicionxintervaloDTO> listaCabeceraM = lista.GroupBy(x => new
            {
                x.Ptomedicodi,
                x.Emprnomb,
                x.Ptomedibarranomb,
                x.Tipoinfoabrev,
                x.Tptomedicodi,
                x.Fenergnomb,
                x.Equinomb,
                x.Emprcoes,
                x.Ptomedielenomb,
                x.Equipadre,
                x.Equipopadre
            })
                                .Select(y => new MeMedicionxintervaloDTO()
                                {
                                    Emprnomb = y.Key.Emprnomb,
                                    Ptomedicodi = y.Key.Ptomedicodi,
                                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                    Tptomedicodi = y.Key.Tptomedicodi,
                                    Fenergnomb = y.Key.Fenergnomb,
                                    Equinomb = y.Key.Equinomb,
                                    Equipadre = y.Key.Equipadre,
                                    Equipopadre = y.Key.Equipopadre,
                                    Emprcoes = y.Key.Emprcoes,
                                    Ptomedielenomb = y.Key.Ptomedielenomb,
                                }
                                ).ToList();

            using (ExcelRange r = ws.Cells[xFil, 2, xFil + 1, ncol + 1])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }



            ws.Cells[xFil, 2].Value = titulo2;
            ws.Cells[xFil + 1, 2].Value = "EMPRESA";
            ws.Cells[xFil + 1, 3].Value = "TIPO AGENTE";
            ws.Cells[xFil + 1, 4].Value = "CENTRAL";
            ws.Cells[xFil + 1, 5].Value = "CENTRAL INTEGRANTE";
            ws.Cells[xFil + 1, 6].Value = "MEDIDOR";
            ws.Cells[xFil + 1, 7].Value = "TIPO COMBUSTIBLE";
            ws.Cells[xFil + 1, 8].Value = "UNIDAD";

            for (int i = 0; i < listaFechas.Count; i++)
            {
                ws.Cells[xFil + 1, 9 + i].Value = listaFechas[i].ToString(ConstantesAppServicio.FormatoDiaMes);
            }

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 20;
            ws.Column(3).Width = 12;
            ws.Column(4).Width = 23;
            ws.Column(5).Width = 15;
            ws.Column(6).Width = 21;
            ws.Column(7).Width = 16;
            ws.Column(8).Width = 6;

            int row = xFil + 2;
            int column = 2;

            if (lista.Count > 0)
            {
                foreach (var reg in listaCabeceraM)
                {


                    ws.Cells[row, column].Value = reg.Emprnomb;
                    if (reg.Emprcoes == "S")
                        ws.Cells[row, column + 1].Value = "INTEGRANTE";
                    else
                        ws.Cells[row, column + 1].Value = "NO INTEGRANTE";
                    // ws.Cells[row, column + 1].Value = "TERMOELÉCTRICA";
                    if (reg.Equipadre > 0)
                        ws.Cells[row, column + 2].Value = reg.Equipopadre;
                    else
                        ws.Cells[row, column + 2].Value = reg.Equinomb;

                    if (reg.Emprcoes == "S")
                        ws.Cells[row, column + 3].Value = "COES";
                    else
                        ws.Cells[row, column + 3].Value = "NO COES";
                    ws.Cells[row, column + 4].Value = reg.Equinomb;
                    ws.Cells[row, column + 5].Value = reg.Fenergnomb;
                    ws.Cells[row, column + 6].Value = reg.Tipoinfoabrev;
                    int i = 1;
                    foreach (var reg2 in listaFechas)
                    {
                        decimal? valor;
                        var fil = lista.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medintfechaini == reg2);
                        if (fil != null)
                        {
                            valor = fil.Medinth1;
                            if (valor != null)
                            {
                                using (var range = ws.Cells[row, column + 6 + i, row, column + 6 + i])
                                {
                                    range.Style.Numberformat.Format = @"0.000";
                                }
                                ws.Cells[row, column + 6 + i].Value = valor;

                            }
                            else
                                ws.Cells[row, column + 6 + i].Value = "--";
                        }

                        i++;
                    }
                    row++;

                }
                //****************                
            }
            else
            {
                //No existen registros);
            }


            var fontTabla = ws.Cells[xFil, 2, row, ncol + 1].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[xFil, 2, row - 1, ncol + 1].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Muestra los datos de consulta para el listado de La Presión de Gas ó Temperatura Ambiente
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        public void ConfiguracionHojaExcelPresTemp(ExcelWorksheet ws, int xFil, List<MeMedicion24DTO> listaReporte, int idParametro)
        {
            if (listaReporte.Count == 0)
                return;
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            int nfil = listaReporte.Count;
            var listaFechas = listaReporte.Select(x => x.Medifecha).Distinct().ToList();
            var titulo = string.Empty;
            List<MeMedicion24DTO> listaCabeceraM24 = new List<MeMedicion24DTO>();
            if (idParametro == 1) // Presión de Gas
            {
                titulo = "PRESIONES GAS(kPa)";
                listaCabeceraM24 = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Gruponomb, x.Ptomedielenomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Gruponomb = y.Key.Gruponomb,
                                     Ptomedielenomb = y.Key.Ptomedielenomb,
                                 }
                                 ).ToList();
            }
            else //Temperatura Ambiente
            {
                titulo = "TEMPERATURA AMBIENTE (ºC)";
                listaCabeceraM24 = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Equinomb = y.Key.Equinomb,
                                 }
                                 ).ToList();
            }
            int ncol = listaCabeceraM24.Count;

            using (ExcelRange r = ws.Cells[xFil, 2, xFil + 2, ncol + 2])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }
            using (ExcelRange r = ws.Cells[xFil + 1, 3, xFil + 1, ncol + 2])
            {
                //r.Style.Font.Color.SetColor(Color.White);
                //r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                //r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            }


            ws.Cells[xFil, 2].Value = "HORA";

            ExcelRange rg = ws.Cells[xFil, 2, xFil + 2, 2];
            rg.Merge = true;
            rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //rg.Style.Font.Size = 10;
            //rg.Style.Font.Bold = true;
            //rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));
            //rg.Style.Border.Left.Style = ExcelBorderStyle.Double;
            //rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
            //rg.Style.Border.Right.Style = ExcelBorderStyle.Double;
            //rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
            //rg.Style.Border.Top.Style = ExcelBorderStyle.Double;
            //rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
            //rg.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
            //rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));




            ws.Cells[xFil, 3].Value = titulo;
            int i = 3;
            foreach (var reg in listaCabeceraM24)
            {
                if (idParametro == 1) // Presión de Gas
                {
                    ws.Cells[xFil + 1, i].Value = reg.Emprnomb;
                    ws.Cells[xFil + 2, i].Value = reg.Ptomedielenomb;
                }
                else //Temperatura Ambiente
                {
                    ws.Cells[xFil + 1, i].Value = reg.Emprnomb;
                    ws.Cells[xFil + 2, i].Value = reg.Equinomb;
                }
                i++;
            }
            i = 0;
            int nBloques = 24;
            int resolucion = 60;
            xFil = xFil + 3;
            for (int j = 0; j < listaFechas.Count; j++)
            {
                for (int k = 1; k <= nBloques; k++)
                {
                    ws.Cells[xFil + i, 2].Value = listaFechas[j].AddMinutes((k - 1) * resolucion).ToString(ConstantesAppServicio.FormatoFechaHora); ;
                    int z = 1;
                    foreach (var reg in listaCabeceraM24)
                    {

                        decimal? valor;
                        var p = listaReporte.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medifecha == listaFechas[j]);
                        if (p != null)
                        {
                            valor = (decimal?)p.GetType().GetProperty("H" + k).GetValue(p, null);
                            if (valor != null)
                            {
                                using (var range = ws.Cells[xFil + i, 2 + z, xFil + i, 2 + z])
                                {
                                    range.Style.Numberformat.Format = @"0.000";
                                }
                                ws.Cells[xFil + i, 2 + z, xFil + i, 2 + z].Value = valor;

                            }
                            else
                                ws.Cells[xFil + i, 2 + z, xFil + i, 2 + z].Value = "--";
                        }
                        z++;
                    }
                    i++;
                }
            }

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 15;
            ws.Column(3).Width = 20;
            for (var j = 4; j < 3 + ncol; j++)
            {
                ws.Column(j).AutoFit(ws.Cells[xFil + 2, j].Text.Length + 1);
            }
            var fontTabla = ws.Cells[xFil - 3, 2, xFil + i - 1, 2 + listaCabeceraM24.Count].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[xFil, 2, xFil + i - 1, 2 + listaCabeceraM24.Count].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Muestra datos de consulta para el listado de Disponibilidad de Comsbustibles
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaReporte"></param>
        public static void ConfiguracionHojaExcelDisponibilidad(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> listaReporte, string fInicio, string fFin)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            int nfil = listaReporte.Count + 1;
            int xFil = 8;
            int ncol = 10;

            ws.Cells[3, 3].Value = fInicio;
            ws.Cells[4, 3].Value = fFin;
            var i = 0;

            ws.Cells[xFil - 1, 2].Value = "FECHA";
            ws.Cells[xFil - 1, 3].Value = "EMPRESA";
            ws.Cells[xFil - 1, 4].Value = "GASEODUCTO";
            ws.Cells[xFil - 1, 5].Value = "VOLUMEN DE GAS (Mm3)";
            ws.Cells[xFil - 1, 6].Value = "INICIO";
            ws.Cells[xFil - 1, 7].Value = "FINAL";
            ws.Cells[xFil - 1, 8].Value = "FECHA HORA ENVIO";
            ws.Cells[xFil - 1, 9].Value = "ESTADO";
            ws.Cells[xFil - 1, 10].Value = "OBSERVACIONES";
            ws.Cells[xFil - 1, 11].Value = "USUARIO";

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 15;
            ws.Column(3).Width = 35;
            ws.Column(4).Width = 35;
            ws.Column(5).Width = 20;
            ws.Column(6).Width = 10;
            ws.Column(7).Width = 10;
            ws.Column(8).Width = 20;
            ws.Column(9).Width = 20;
            ws.Column(10).Width = 20;
            ws.Column(11).Width = 20;



            foreach (var reg in listaReporte)
            {

                ws.Cells[xFil + i, 2].Value = reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha);
                ws.Cells[xFil + i, 2].StyleID = ws.Cells[8, 2].StyleID;
                ws.Cells[xFil + i, 3].Value = reg.Emprnomb;
                ws.Cells[xFil + i, 3].StyleID = ws.Cells[8, 3].StyleID;
                ws.Cells[xFil + i, 4].Value = reg.Equinomb;
                ws.Cells[xFil + i, 4].StyleID = ws.Cells[8, 4].StyleID;
                //************Datos para el PDO *****************
                decimal? valor = 0;
                if (reg.Medinth1 != null)
                    valor = (decimal?)reg.Medinth1;
                ws.Cells[xFil + i, 5].Value = valor;
                ws.Cells[xFil + i, 5].StyleID = ws.Cells[8, 5].StyleID;
                ws.Cells[xFil + i, 6].Value = reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha) + " 6:00";
                ws.Cells[xFil + i, 6].StyleID = ws.Cells[8, 6].StyleID;
                ws.Cells[xFil + i, 7].Value = reg.Medintfechaini.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha) + " 6:00";
                ws.Cells[xFil + i, 7].StyleID = ws.Cells[8, 7].StyleID;

                ws.Cells[xFil + i, 8].Value = reg.Medintfechafin.ToString(ConstantesAppServicio.FormatoFechaHora);
                ws.Cells[xFil + i, 8].StyleID = ws.Cells[8, 8].StyleID;

                string estado = "";
                if (reg.Medestcodi == 1) { estado = "Declaró"; }
                if (reg.Medestcodi == 2) { estado = "Renominó"; }

                ws.Cells[xFil + i, 9].Value = estado;
                ws.Cells[xFil + i, 9].StyleID = ws.Cells[8, 9].StyleID;

                ws.Cells[xFil + i, 10].Value = reg.Medintdescrip;
                ws.Cells[xFil + i, 10].StyleID = ws.Cells[8, 10].StyleID;

                ws.Cells[xFil + i, 11].Value = reg.Medintusumodificacion;
                ws.Cells[xFil + i, 11].StyleID = ws.Cells[8, 11].StyleID;

                //************DAtos tiempo Real *****************


                /*   ws.Cells[xFil + i, 8].Value = valor;
                   ws.Cells[xFil + i, 8].StyleID = ws.Cells[8, 8].StyleID;
                   ws.Cells[xFil + i, 9].Value = reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoOnlyHora);
                   ws.Cells[xFil + i, 9].StyleID = ws.Cells[8, 9].StyleID;
                   ws.Cells[xFil + i, 10].Value = reg.Medintfechafin.ToString(ConstantesAppServicio.FormatoOnlyHora);
                   ws.Cells[xFil + i, 10].StyleID = ws.Cells[8, 10].StyleID;
                   */
                //*******************
                i++;
            }
            ////////////////
            using (ExcelRange r = ws.Cells[xFil - 1, 2, xFil - 1, 11])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }



            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[7, 5, 7 + nfil - 1, ncol - 1])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[7, 2, 7 + nfil, 1 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[7, 2, 7 + nfil - 1, 1 + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        }
        /// <summary>
        /// Muestra datos de consulta para el listado Quema de Gas
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaReporte"></param>
        public static void ConfiguracionHojaExcelQuema(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> listaReporte)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            int nfil = listaReporte.Count;
            int xFil = 6;
            int ncol = 7;
            using (ExcelRange r = ws.Cells[xFil, 2, xFil, 8])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells[xFil, 2].Value = "FECHA";
            ws.Cells[xFil, 3].Value = "EMPRESA";
            ws.Cells[xFil, 4].Value = "CENTRAL";
            ws.Cells[xFil, 5].Value = "TIPO COMBUSTIBLE";
            ws.Cells[xFil, 6].Value = "INICIO";
            ws.Cells[xFil, 7].Value = "FINAL";
            ws.Cells[xFil, 8].Value = "VOLUMEN DE GAS (Mm3)";

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 15;
            ws.Column(3).Width = 35;
            ws.Column(4).Width = 35;
            ws.Column(5).Width = 20;
            ws.Column(6).Width = 10;
            ws.Column(7).Width = 10;
            ws.Column(8).Width = 20;
            //****************************************
            var i = 1;
            foreach (var reg in listaReporte)
            {

                ws.Cells[xFil + i, 2].Value = reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha);

                ws.Cells[xFil + i, 3].Value = reg.Emprnomb;

                ws.Cells[xFil + i, 4].Value = reg.Equinomb;
                ws.Cells[xFil + i, 5].Value = reg.Tipoptomedinomb;

                ws.Cells[xFil + i, 6].Value = reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoOnlyHora);

                ws.Cells[xFil + i, 7].Value = reg.Medintfechafin.ToString(ConstantesAppServicio.FormatoOnlyHora);
                decimal? valor = 0;

                if (reg.Medinth1 != null)
                    valor = (decimal?)reg.Medinth1;
                ws.Cells[xFil + i, 8].Value = valor;
                i++;
            }
            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[6, 8, 6 + nfil, 8])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[6, 2, 6 + nfil, 1 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[6, 2, 6 + nfil, 1 + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Muestra los datos de consulta para los consumos de combustibles en el reporte con gráfico
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        /// <param name="nfil"></param>
        /// <param name="ncol"></param>
        public void ConfiguracionHojaExcelStockGrafico(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> lista, out int nfil, out int ncol, int opt)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            List<DateTime> ListaFechas = new List<DateTime>();
            ListaFechas = lista.Select(x => x.Medintfechaini).Distinct().ToList(); // Lista de fechas ordenados para la categoria del grafico
            List<MeMedicionxintervaloDTO> listPtosMed = lista.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Fenergnomb, x.Equinomb })
                               .Select(y => new MeMedicionxintervaloDTO()
                               {
                                   Ptomedicodi = y.Key.Ptomedicodi,
                                   Emprnomb = y.Key.Emprnomb,
                                   Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                   Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                   Fenergnomb = y.Key.Fenergnomb,
                                   Equinomb = y.Key.Equinomb,
                               }
                               ).ToList();

            nfil = ListaFechas.Count;
            ncol = listPtosMed.Count;
            int xFil = 6;
            using (ExcelRange r = ws.Cells[xFil, 2, xFil, 2 + ncol])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 15;
            ws.Cells[xFil, 2].Value = "FECHA";
            int i = 1;
            foreach (var reg in listPtosMed)
            {
                ws.Cells[xFil, 2 + i].Value = reg.Equinomb;
                ws.Column(2 + i).Width = 15;
                i++;
            }

            i = 1;
            int j = 0;
            foreach (var fil in ListaFechas)
            {
                j = 0;
                ws.Cells[xFil + i, 2 + j].Value = fil.ToString(ConstantesAppServicio.FormatoFecha);
                j++;
                foreach (var col in listPtosMed)
                {
                    var entity = lista.Find(x => x.Ptomedicodi == col.Ptomedicodi && x.Medintfechaini == fil);
                    decimal? valor = 0;
                    if (entity != null)
                    {
                        if (opt == 1) // Stock
                            valor = (decimal?)entity.H1Fin;
                        else //Recepcion
                            valor = (decimal?)entity.H1Recep;
                    }
                    if (valor == null)
                        valor = 0;
                    ws.Cells[xFil + i, 2 + j].Value = valor;
                    j++;
                }
                i++;
            }


            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[7, 3, 7 + nfil - 1, 3 + ncol - 1])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[6, 2, 6 + nfil, 2 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[6, 2, 6 + nfil, 2 + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Muestra los datos de consulta para los Consumos de Combustibles en el reporte con gráfico
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        /// <param name="nfil"></param>
        /// <param name="ncol"></param>
        public void ConfiguracionHojaExcelConsumoGrafico(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> lista, int tipoPtoMedicodi, out int nfil, out int ncol, int tipoGrafico)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            List<DateTime> ListaFechas = new List<DateTime>();
            ListaFechas = lista.Select(x => x.Medintfechaini).Distinct().ToList(); // Lista de fechas ordenados para la categoria del grafico
            List<MeMedicionxintervaloDTO> listPtosMed = lista.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tptomedicodi, x.Fenergnomb, x.Equinomb })
                               .Select(y => new MeMedicionxintervaloDTO()
                               {
                                   Ptomedicodi = y.Key.Ptomedicodi,
                                   Emprnomb = y.Key.Emprnomb,
                                   Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                   Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                   Tptomedicodi = y.Key.Tptomedicodi,
                                   Fenergnomb = y.Key.Fenergnomb,
                                   Equinomb = y.Key.Equinomb,
                               }
                               ).Where(x => x.Tptomedicodi == tipoPtoMedicodi).ToList();

            if (tipoGrafico == 1) // grafico Lineas
                nfil = ListaFechas.Count;
            else
                nfil = 1;
            ncol = listPtosMed.Count;
            int xFil = 6;
            using (ExcelRange r = ws.Cells[xFil, 2, xFil, 2 + ncol])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 15;
            ws.Cells[xFil, 2].Value = "FECHA";
            int i = 1;
            int j = 0;
            foreach (var reg in listPtosMed)
            {
                //if (reg.Tipoptomedicodi == tipoPtoMedicodi)
                //{
                ws.Cells[xFil, 2 + i].Value = reg.Ptomedibarranomb;
                ws.Column(2 + i).Width = 15;
                i++;
                //}
            }

            if (tipoGrafico == 1) //Grafico lineas
            {
                i = 1;
                foreach (var fil in ListaFechas)
                {
                    j = 0;
                    ws.Cells[xFil + i, 2 + j].Value = fil.ToString(ConstantesAppServicio.FormatoFecha);
                    j++;
                    foreach (var col in listPtosMed)
                    {
                        var entity = lista.Find(x => x.Ptomedicodi == col.Ptomedicodi && x.Medintfechaini == fil);
                        decimal? valor = 0;
                        if (entity != null)
                        {
                            valor = (decimal?)entity.Medinth1;
                            if (valor == null)
                                valor = 0;
                            ws.Cells[xFil + i, 2 + j].Value = valor;
                            j++;
                        }
                    }
                    i++;
                }
            }
            else
            {
                /// lista acumulada, gráfico pie
                /// 
                decimal? valorAcumulado;
                i = 1;
                j = 1;
                //ws.Cells[xFil + i, 2].Value = "Del " + ListaFechas.Min().ToString(ConstantesAppServicio.FormatoFecha) + "Al " + ListaFechas.Max().ToString(ConstantesAppServicio.FormatoFecha);
                foreach (var ptos in listPtosMed)
                {
                    //j = 0;
                    //ws.Cells[xFil + i, 2 + j].Value = fil.ToString(ConstantesAppServicio.FormatoFecha);
                    //j++;
                    valorAcumulado = 0;
                    foreach (var fec in ListaFechas)
                    {
                        var entity = lista.Find(x => x.Ptomedicodi == ptos.Ptomedicodi && x.Medintfechaini == fec);

                        decimal? valor = 0;
                        if (entity != null)
                        {
                            valor = (decimal?)entity.Medinth1;
                        }
                        valorAcumulado += valor;
                    }
                    ws.Cells[xFil + i, 2 + j].Value = valorAcumulado;
                    j++;
                }
            }


            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[7, 3, 7 + nfil - 1, 3 + ncol - 1])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[6, 2, 6 + nfil, 2 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[6, 2, 6 + nfil, 2 + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Configura encabezado de Reporte Hoja Excel 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        public void ConfiguraEncabezadoHojaExcel(ExcelWorksheet ws, string titulo, string fInicio, string fFin, string rutaLogo)
        {
            AddImage(ws, 1, 0, rutaLogo);
            ws.Cells[1, 3].Value = titulo;
            var font = ws.Cells[1, 3].Style.Font;
            font.Size = 16;
            font.Bold = true;
            font.Name = "Calibri";
            //Borde, font cabecera de Tabla Fecha
            var borderFecha = ws.Cells[3, 2, 4, 3].Style.Border;
            borderFecha.Bottom.Style = borderFecha.Top.Style = borderFecha.Left.Style = borderFecha.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[3, 2, 4, 3].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Bold = true;
            ws.Cells[3, 2].Value = "Fecha Inicio:";
            ws.Cells[4, 2].Value = "Fecha Fin:";
            ws.Cells[3, 3].Value = fInicio;
            ws.Cells[4, 3].Value = fFin;
        }

        /// <summary>
        /// Configura encabezado de Reporte Hoja Excel 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="fInicio"></param>
        /// <param name="rutaLogo"></param>
        public void ConfiguraEncabezadoHojaExcel2(ExcelWorksheet ws, string titulo, DateTime fInicio, string rutaLogo)
        {
            AddImage(ws, 1, 0, rutaLogo);
            ws.Cells[1, 3].Value = titulo;
            var font = ws.Cells[1, 3].Style.Font;
            font.Size = 16;
            font.Bold = true;
            font.Name = "Calibri";
            //Borde, font cabecera de Tabla Fecha
            var borderFecha = ws.Cells[3, 2, 3, 3].Style.Border;
            borderFecha.Bottom.Style = borderFecha.Top.Style = borderFecha.Left.Style = borderFecha.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[3, 2, 3, 3].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Bold = true;
            ws.Cells[3, 2].Value = "Fecha:";
            ws.Cells[3, 3].Value = fInicio.ToString(ConstantesStockCombustibles.FormatoFecha);
        }

        /// <summary>
        /// Genera grafico tipo Linea en archivo excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="titulo"></param>
        public static void AddGraficoLineas(ExcelWorksheet ws, int row, int col, string xAxisTitle, string yAxisTitle, string titulo)
        {
            var LineaChart = ws.Drawings.AddChart("crtExtensionsSize", eChartType.Line) as ExcelLineChart;
            //Set top left corner to row 1 column 2
            LineaChart.SetPosition(5, 0, col + 3, 0);
            LineaChart.SetSize(1200, 600);

            for (int i = 0; i < col; i++)
            {

                var ran1 = ws.Cells[7, 3 + i, row + 6, 3 + i];
                var ran2 = ws.Cells[7, 2, row + 6, 2];

                var serie = (ExcelChartSerie)LineaChart.Series.Add(ran1, ran2);
                serie.Header = ws.Cells[6, 3 + i].Value.ToString();
            }
            LineaChart.Title.Text = titulo;
            //LineaChart.DataLabel.ShowSeriesName = true;
            //LineaChart.DataLabel.ShowLegendKey = true;
            LineaChart.DataLabel.ShowLeaderLines = true;
            //LineaChart.DataLabel.ShowCategory = true;
            //LineaChart.XAxis.Title.Text = xAxisTitle;
            LineaChart.YAxis.Title.Text = yAxisTitle;

            LineaChart.Legend.Position = eLegendPosition.Bottom;


        }

        /// <summary>
        ///  Genera grafico tipo Columnas en archivo excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowini"></param>
        /// <param name="colini"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public static void AddGraficoColumn(ExcelWorksheet ws, int rowini, int colini, int rows, int cols)
        {
            var colChart = ws.Drawings.AddChart("crtExtensionsSize", eChartType.ColumnStacked);
            //Set top left corner to row 1 column 2
            colChart.SetPosition(rowini + 1 + rows, 0, 1, 0);
            colChart.SetSize(1000, 400);
            List<ExcelChartSerie> series = new List<ExcelChartSerie>();
            for (var i = 0; i < rows; i++)
            {

                series.Add(colChart.Series.Add(ExcelRange.GetAddress(rowini + i, colini, rowini + i, colini + cols), ExcelRange.GetAddress(rowini - 1, colini, rowini - 1, colini + cols)));
                series[i].Header = ws.Cells[rowini + i, colini - 1].Value.ToString();

            }
            //pieChart.Title.Text = "Mantenimientos Ejecutados";
            //Set datalabels and remove the legend
            //pieChart.DataLabel.ShowCategory = true;
            //pieChart.DataLabel.ShowPercent = true;
            //pieChart.DataLabel.ShowLeaderLines = true;
            //colChart.Legend.Remove();
        }

        /// <summary>
        ///  Genera grafico tipo Pie en archivo excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="col"></param>
        /// <param name="titulo"></param>
        public static void AddGraficoPie(ExcelWorksheet ws, int col, string titulo)
        {
            var pieChart = ws.Drawings.AddChart("crtExtensionsSize", eChartType.PieExploded3D) as ExcelPieChart;
            //Set top left corner to row 1 column 2
            pieChart.SetPosition(8, 0, 1, 0);
            pieChart.SetSize(800, 600);
            pieChart.Series.Add(ExcelRange.GetAddress(7, 3, 7, 3 + col - 1), ExcelRange.GetAddress(6, 3, 6, 3 + col - 1));

            pieChart.Title.Text = titulo;
            //Set datalabels and remove the legend
            pieChart.DataLabel.ShowCategory = true;
            pieChart.DataLabel.ShowPercent = true;
            pieChart.DataLabel.ShowLeaderLines = true;
            pieChart.Legend.Remove();
        }

        /// <summary>
        /// Inserta Imagen COES en Archivo Excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="filePath"></param>
        private void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = Pixel2MTU(2); //Two pixel space for better alignment
                picture.From.RowOff = Pixel2MTU(2);//Two pixel space for better alignment
                picture.SetSize(100, 40);

            }
        }

        /// <summary>
        /// Inserta Imagen COES en Archivo Excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="filePath"></param>
        private void AddImageAdicional(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath, int indicator)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + indicator + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = Pixel2MTU(2); //Two pixel space for better alignment
                picture.From.RowOff = Pixel2MTU(2);//Two pixel space for better alignment
                picture.SetSize(100, 40);

            }
        }

        /// <summary>
        /// Deterina ancho en pixeles para el logo
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int MTU_PER_PIXEL = 9525;
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }

        #endregion

        #region Metodos Tabla EQ_CATEGORIADET
        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA_DETALLE
        /// </summary>
        public List<EqCategoriaDetDTO> ListEqCategoriaDetalleByCategoria(int ctgcodi)
        {
            return FactorySic.GetEqCategoriaDetalleRepository().ListByCategoriaAndEstado(ctgcodi, ConstantesAppServicio.Activo);
        }

        #endregion

        /// <summary>
        /// Listar los formatos utilizados en la extranet
        /// </summary>
        /// <returns></returns>
        public List<MeFormatoDTO> ListaFormatos()
        {
            List<MeFormatoDTO> ListaCombo = new List<MeFormatoDTO>();
            ListaCombo.Add(LlenarItem(62, "DESPACHO EJECUTADO DIARIO", "01", 9));
            ListaCombo.Add(LlenarItem(98, "FUENTE DE ENERGIA PRIMARIA - CENTRALES EOLICAS, TÉRMICAS", "02", 9));
            ListaCombo.Add(LlenarItem(97, "FUENTE DE ENERGIA PRIMARIA - SOLARES", "03", 9));
            ListaCombo.Add(LlenarItem(65, "CALOR UTIL EJECUTADO DIARIO", "04", 9));
            ListaCombo.Add(LlenarItem(57, "STOCK Y CONSUMO DE COMBUSTIBLES", "05", 9));
            ListaCombo.Add(LlenarItem(59, "DISPONIBILIDAD DE GAS", "06", 9));
            ListaCombo.Add(LlenarItem(58, "PRESIÓN DE GAS Y TEMPERATURA", "07", 9));
            ListaCombo.Add(LlenarItem(38, "TURBINADO - EJECUTADO HORARIO", "08", 3));
            ListaCombo.Add(LlenarItem(29, "NATURALES - EJECUTADO HORARIO", "09", 3));
            ListaCombo.Add(LlenarItem(24, "ESTACIONALES - EJECUTADO DIARIO", "10", 3));
            ListaCombo.Add(LlenarItem(44, "CAUDALES EN ESTACIONES - HORARIO", "11", 3));
            ListaCombo.Add(LlenarItem(8, "HORARIOS - EMBALSE HORARIO EJECUTADO", "12", 3));
            ListaCombo.Add(LlenarItem(10, "HORARIOS - EMBALSE DE COMPENSACION EJECUTADO", "13", 3));
            ListaCombo.Add(LlenarItem(64, "TENSION DE GENERACION DIARIA", "14", 9));
            ListaCombo.Add(LlenarItem(63, "FLUJO DE POTENCIA", "15", 9));
            ListaCombo.Add(LlenarItem(67, "POTENCIA DE AUTOPRODUCTORES", "16", 9));
            ListaCombo.Add(LlenarItem(60, "QUEMA DE GAS", "17", 9));
            ListaCombo.Add(LlenarItem(66, "TENSION EN BARRAS", "18", 9));

            return ListaCombo;
        }

        /// <summary>
        /// Setear objeto item
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        private MeFormatoDTO LlenarItem(int formato, string nombre, string hoja, int modcodi)
        {
            MeFormatoDTO objFormato = new MeFormatoDTO();
            objFormato.Formatcodi = formato; 
            objFormato.Formatnombre = nombre;
            objFormato.Formatdescrip = hoja;
            objFormato.Modcodi = modcodi;
            return objFormato;
        }

        #region Mejoras RDO
        /// <summary>
        /// Setear objeto item
        /// </summary>
        /// <param name="lectCodi"></param>
        /// <param name="origlectcodi"></param>
        /// <param name="sEmprCodi"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsRecurso"></param>
        /// <param name="strCentralInt"></param>
        /// <param name="horario"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> ListaMedDisponibilidadCombustible(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt, string horario)
        {
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            if (string.IsNullOrEmpty(sEmprCodi)) sEmprCodi = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsRecurso)) idsRecurso = ConstantesAppServicio.ParametroDefecto;
            lista = FactorySic.GetMeMedicionxintervaloRepository().GetListaMedDisponibilidadCombustible(lectCodi, origlectcodi, sEmprCodi, fechaInicial, fechaFinal, idsEstado, idsRecurso, strCentralInt, ConstantesAppServicio.ParametroDefecto, horario);

            foreach (var reg in lista)
            {
                reg.Emprnomb = reg.Emprnomb != null ? reg.Emprnomb.Trim() : string.Empty;
                reg.Equipopadre = reg.Equipopadre != null ? reg.Equipopadre.Trim() : string.Empty;
                reg.Equinomb = reg.Emprnomb != null ? reg.Equinomb.Trim() : string.Empty;
                reg.Emprnomb = reg.Emprnomb != null ? reg.Emprnomb.Trim() : string.Empty;
                reg.Fenergnomb = reg.Fenergnomb != null ? reg.Fenergnomb.Trim() : string.Empty;
                reg.Tipoinfoabrev = reg.Tipoinfoabrev != null ? reg.Tipoinfoabrev.Trim() : string.Empty;
            }

            return lista;
        }
        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarValoresDisponibilidadCombustible(List<MeMedicionxintervaloDTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            MeValidacionDTO validacion = new MeValidacionDTO();
            validacion.Emprcodi = idEmpresa;
            validacion.Formatcodi = formato.Formatcodi;
            validacion.Validfechaperiodo = formato.FechaProceso;
            validacion.Validestado = ConstantesStockCombustibles.NoValidado;
            validacion.Validusumodificacion = usuario;
            validacion.Validfecmodificacion = DateTime.Now;
            try
            {
                var findValicion = servFormato.GetByIdMeValidacion(formato.Formatcodi, idEmpresa, formato.FechaProceso);
                if (findValicion != null)
                {
                    servFormato.UpdateMeValidacion(validacion);
                }
                else
                {
                    servFormato.SaveMeValidacion(validacion);
                }
                //Traer Ultimos Valores
                var envioPrevio = servFormato.GetEnvioMedicionXIntervalo(formato.Formatcodi, idEmpresa, formato.FechaProceso.AddDays(-1), formato.FechaProceso);
                var envioHoy = envioPrevio.Where(x => x.Medintfechaini == formato.FechaProceso).ToList();
                var envioActual = entitys.Where(x => x.Medintfechaini == formato.FechaProceso).ToList();
                //var lista = GetDataFormato(idEmpresa, formato, 0, 0);
                // verificar Stock Inicial
                var envioPrevioStockIni = envioPrevio.Where(x => x.Medintfechaini == formato.FechaProceso.AddDays(-1) &&
                    ConstantesStockCombustibles.IdsTptoStock.Contains(x.Tptomedicodi));
                var envioStockIni = entitys.Where(x => x.Medintfechaini == formato.FechaProceso.AddDays(-1));
                if (envioHoy.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();

                    foreach (var reg in entitys)
                    {
                        var regAnt = envioPrevio.Find(x => x.Medintfechaini == reg.Medintfechaini && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi);
                        decimal? valorOrigen = null;
                        string descripOrigen = string.Empty;
                        if (regAnt != null)
                        {
                            valorOrigen = (decimal?)regAnt.Medinth1;
                            descripOrigen = regAnt.Medintdescrip;
                        }
                        string stValor = string.Empty;
                        string stValorOrigen = string.Empty;
                        decimal? valorModificado = (decimal?)reg.Medinth1;
                        string descripModificado = reg.Medintdescrip;
                        if (valorModificado != null)
                            stValor = valorModificado.ToString();
                        if (valorOrigen != null)
                            stValorOrigen = valorOrigen.ToString();
                        if ((valorOrigen != valorModificado) || (descripOrigen != descripModificado))
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            if (descripOrigen != descripModificado && !string.IsNullOrEmpty(descripModificado))
                                cambio.Cambenvdatos = stValor + "#" + descripModificado;
                            else
                                cambio.Cambenvdatos = stValor;
                            cambio.Cambenvcolvar = "1";
                            cambio.Cambenvfecha = (DateTime)reg.Medintfechaini;
                            cambio.Enviocodi = idEnvio;
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambios en tabla MeCambioenvio se graba el registro original
                            if (servFormato.ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medintfechaini).Count == 0)
                            {
                                int idEnvioPrevio = 0;
                                var listAux = servFormato.GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaInicio); // lista de envios aprobados
                                if (listAux.Count > 0)
                                    idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                if (descripOrigen != descripModificado && !string.IsNullOrEmpty(descripModificado))
                                    origen.Cambenvdatos = stValorOrigen + "#" + descripOrigen;
                                else
                                    origen.Cambenvdatos = stValorOrigen;
                                origen.Cambenvcolvar = "";
                                origen.Cambenvfecha = (DateTime)reg.Medintfechaini;
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
                        servFormato.GrabarCambios(listaCambio);
                        servFormato.GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                ////Eliminar Valores Previos
                EliminarValoresCargados1XInter(formato.Formatcodi, idEmpresa, formato.FechaProceso, formato.FechaProceso);
                EliminarValoresEnvioColumna(formato.FechaProceso.AddDays(-1), formato.FechaProceso.AddDays(-1), formato.Formatcodi, idEmpresa, 3, //ANTES ESTABA 2(STOCK DE COMBUSTIBLES), pero su hojacodi es 3
                    string.Join(", ", ConstantesStockCombustibles.IdsTptoStock));
                foreach (MeMedicionxintervaloDTO entity in entitys)
                {
                    entity.Enviocodi = idEnvio;
                    servFormato.SaveMeMedicionxintervalo(entity);

                    servFormato.SaveMeMedicionxintervaloRDO(entity);
                }
            } // end try
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Notificaciones IEOD

        /// <summary>
        /// Actualiza y Guarda la informacion de la plantilla de correo
        /// </summary>
        /// <param name="correo"></param>
        /// <param name="usuario"></param>
        public void ActualizarDatosPlantillaCorreo(SiPlantillacorreoDTO correo, string usuario)
        {
            /**** Actualizar la plantilla ****/
            SiPlantillacorreoDTO c = (new CorreoAppServicio()).GetByIdSiPlantillacorreo(correo.Plantcodi);
            c.Plantcontenido = correo.Plantcontenido;
            c.Plantasunto = correo.Plantasunto;
            c.Planticorreos = correo.Planticorreos;
            c.PlanticorreosCc = correo.PlanticorreosCc;
            c.PlanticorreosBcc = correo.PlanticorreosBcc;
            c.PlanticorreoFrom = correo.PlanticorreoFrom;
            c.Plantfecmodificacion = DateTime.Now;
            c.Plantusumodificacion = usuario;

            (new CorreoAppServicio()).UpdateSiPlantillacorreo(c);
        }

        /// <summary>
        /// Devuelve el listado de variables que se muestran en asunto y contenido
        /// </summary>
        /// <param name="idPlantilla"></param>    
        /// <returns></returns>
        public List<VariableCorreo> ObtenerListadoVariables(int idPlantilla)
        {
            List<VariableCorreo> lstSalida = new List<VariableCorreo>();
            VariableCorreo obj = new VariableCorreo();

            //obj = new VariableCorreo();
            //obj.Valor = ConstantesStockCombustibles.VariableListaCorreoEmpresa;
            //obj.Nombre = ConstantesStockCombustibles.DesvariableListaCorreoEmpresa;
            //lstSalida.Add(obj);

            obj = new VariableCorreo();
            obj.Valor = ConstantesStockCombustibles.VariableFechaIeod;
            obj.Nombre = ConstantesStockCombustibles.DesvariableFechaIeod;
            lstSalida.Add(obj);

            obj = new VariableCorreo();
            obj.Valor = ConstantesStockCombustibles.VariableEmresa;
            obj.Nombre = ConstantesStockCombustibles.DesvariableEmresa;
            lstSalida.Add(obj);

            obj = new VariableCorreo();
            obj.Valor = ConstantesStockCombustibles.VariableListaFormato;
            obj.Nombre = ConstantesStockCombustibles.DesvariableListaFormato;
            lstSalida.Add(obj);

            return lstSalida.OrderBy(x => x.Nombre).ToList();
        }

        /// <summary>
        /// Permite obtener la configuracion de envio de correo
        /// </summary>
        /// <returns></returns>
        public MeEnvcorreoConfDTO ObtenerConfiguracionCorreo()
        {
            return FactorySic.GetMeEnvcorreoConfRepository().GetById(ConstantesStockCombustibles.IdConfiguracionCorreo);
        }

        /// <summary>
        /// Permite grabar la configuracion de correo
        /// </summary>
        /// <param name="entity"></param>
        public void GrabarConfuguracionCorreo(MeEnvcorreoConfDTO entity)
        {
            try 
            {
                entity.Ecconfcodi = ConstantesStockCombustibles.IdConfiguracionCorreo;

                FactorySic.GetMeEnvcorreoConfRepository().Update(entity);

                SiProcesoDTO proceso = FactorySic.GetSiProcesoRepository().GetById(ConstantesStockCombustibles.IdProcesoNotificacion);
                proceso.Prschorainicio = Convert.ToInt32(entity.Ecconfhoraenvio.Split(':')[0]);
                proceso.Prcsestado = entity.Ecconfestadonot;
                FactorySic.GetSiProcesoRepository().Update(proceso);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener las empresas disponibles
        /// </summary>
        /// <param name="tipoEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasConfiguracion(int tipoEmpresa)
        {
            string formatos = string.Join(",", this.ListaFormatos().Select(x => x.Formatcodi).ToArray());
            List<SiEmpresaDTO> empresas = FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEINPorFormato(formatos).
                Where(x=>x.Tipoemprcodi == 1 || x.Tipoemprcodi == 2 || x.Tipoemprcodi == 3).ToList();

            return empresas.Where(x => x.Tipoemprcodi == tipoEmpresa || tipoEmpresa == -1).ToList();
        }

        /// <summary>
        /// Permite obtener la consulta de los formatos por empresa
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public string[][] ObtenerConsultaFormato(int idTipoEmpresa, int idEmpresa, int idFormato)
        {
            List<SiEmpresaDTO> listEmpresa = this.ObtenerEmpresasConfiguracion(idTipoEmpresa).Where(x=> x.Emprcodi == idEmpresa || idEmpresa == -1).ToList();
            List<MeFormatoDTO> listFormato = this.ListaFormatos().Where(x => x.Formatcodi == idFormato || idFormato == -1).ToList();
            List<MeEnvcorreoFormatoDTO> listData = FactorySic.GetMeEnvcorreoFormatoRepository().List();

            string[][] result = new string[listEmpresa.Count + 2][];

            string[] row = new string[2 + listFormato.Count];
            row[0] = row[1] = string.Empty;

            string[] rowFormato = new string[2 + listFormato.Count];
            rowFormato[0] = string.Empty;
            rowFormato[1] = "Empresa/Formato";

            result[0] = row;
            result[1] = rowFormato;

            int k = 0;
            foreach (MeFormatoDTO itemFormato in listFormato)
            {
                row[2 + k] = itemFormato.Formatcodi.ToString();
                rowFormato[2 + k] = itemFormato.Formatnombre;
                k++;
            }

            int i = 2;
            foreach(SiEmpresaDTO itemEmpresa in listEmpresa)
            {
                string[] rowEmpresa = new string[2 + listFormato.Count];
                rowEmpresa[0] = itemEmpresa.Emprcodi.ToString();
                rowEmpresa[1] = itemEmpresa.Emprnomb.Trim();

                int j = 0;
                foreach(MeFormatoDTO itemFormato in listFormato)
                {
                    string configuracion = string.Empty;
                    MeEnvcorreoFormatoDTO itemData = listData.Where(x => x.Emprcodi == itemEmpresa.Emprcodi && x.Formatcodi == itemFormato.Formatcodi).FirstOrDefault();

                    if (itemData != null)
                        configuracion = itemData.Ecformhabilitado;

                    rowEmpresa[2 + j] = configuracion;
                    j++;
                }

                result[i] = rowEmpresa;
                i++;
            }

            return result;
        }

        /// <summary>
        /// Permite grabar la configuracion de formatos por empresa
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int GrabarConfiguracionFormato(string[][] data, string userName)
        {
            try
            {
                if (data.Length > 0)
                {
                    List<int> idsFormatos = new List<int>();
                    for (int j = 2; j < data[0].Length; j++)
                    {
                        int idFormato = int.Parse(data[0][j]);
                        idsFormatos.Add(idFormato);
                    }

                    string formatos = string.Join(",", idsFormatos.Select(n => n.ToString()).ToArray());

                    for (int i = 2; i < data.Length; i++)
                    {
                        int idEmpresa = int.Parse(data[i][0]);
                        FactorySic.GetMeEnvcorreoFormatoRepository().Delete(idEmpresa, formatos);

                        for (int j = 2; j < data[0].Length; j++)
                        {
                            int idFormato = int.Parse(data[0][j]);

                            string valor = data[i][j];
                            if (valor == ConstantesAppServicio.SI || valor == ConstantesAppServicio.NO)
                            {
                                MeEnvcorreoFormatoDTO entity = new MeEnvcorreoFormatoDTO();
                                entity.Emprcodi = idEmpresa;
                                entity.Formatcodi = idFormato;
                                entity.Ecformhabilitado = valor;
                                entity.Ecformfeccreacion = DateTime.Now;
                                entity.Ecformfecmodificacion = DateTime.Now;
                                entity.Ecformusucreacion = userName;
                                entity.Ecformusumodificacion = userName;
                                FactorySic.GetMeEnvcorreoFormatoRepository().Save(entity);
                            }

                        }
                    }

                }
                return 1;
            }
            catch(Exception)
            {
                return -1;
            }
        }

        #endregion

        #region Envio de notificacion

        /// <summary>
        /// Permite enviar el correo de notificación
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public int EnviarCorreoNotificacion(DateTime fecha)
        {
            try
            {
                List<EnvioCorreoNotificacion> result = new List<EnvioCorreoNotificacion>();
                List<MeFormatoDTO> formatos = this.ListaFormatos();
                List<MeEnvcorreoFormatoDTO> formatosHabilitados = FactorySic.GetMeEnvcorreoFormatoRepository().List();
                List<MeEnvcorreoFormatoDTO> correosEmpresas = FactorySic.GetMeEnvcorreoFormatoRepository().ObtenerCorreoEmpresa();

                foreach(MeFormatoDTO formato in formatos)
                {                   
                    List<SiEmpresaDTO> listadoEmpresa = FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormato(formato.Formatcodi);                   
                    List<MeEnvioDTO> listaEnvio = FactorySic.GetMeEnvioRepository().ObtenerReporteCumplimiento("-1", formato.Formatcodi, fecha, fecha);
                    List<SiEmpresaDTO> empresas = listadoEmpresa.Where(x => !listaEnvio.Any(y => x.Emprcodi == y.Emprcodi)).ToList();
                    List<SiEmpresaDTO> empresasNotificar = empresas.Where(x => formatosHabilitados.Any(y => y.Emprcodi == x.Emprcodi
                        && y.Formatcodi == formato.Formatcodi)).ToList();

                    foreach(SiEmpresaDTO empresa in empresasNotificar)
                    {
                        EnvioCorreoNotificacion entity = new EnvioCorreoNotificacion();
                        entity.Emprcodi = empresa.Emprcodi;
                        entity.Formatnomb = formato.Formatnombre;
                        entity.Emprnomb = empresa.Emprnomb;
                        entity.Formatcodi = formato.Formatcodi;
                        entity.Modcodi = formato.Modcodi;
                        result.Add(entity);
                    }                   
                }

                var listEmpresas = result.Select(x => new { x.Emprcodi, x.Emprnomb }).ToList();
                SiPlantillacorreoDTO plantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(ConstantesStockCombustibles.IdPlantillaNotificacion);
                MeEnvcorreoConfDTO configuracion = FactorySic.GetMeEnvcorreoConfRepository().GetById(ConstantesStockCombustibles.IdConfiguracionCorreo);
                string asunto = plantilla.Plantasunto.Replace(ConstantesStockCombustibles.VariableFechaIeod, fecha.ToString(ConstantesAppServicio.FormatoFecha));
                List<string> correosDestino = plantilla.Planticorreos.Split(';').ToList();
                List<string> correoscc = (!string.IsNullOrEmpty(plantilla.PlanticorreosCc))?plantilla.PlanticorreosCc.Split(';').ToList(): new List<string>();
                string correoFrom = plantilla.PlanticorreoFrom;

                string firma = "<br/><br/>" + configuracion.Ecconfnombre + "<br/>" + configuracion.Ecconfcargo + "<br/>Anexo:" + configuracion.Ecconfanexo;

                foreach (var item in listEmpresas)
                {
                    List<EnvioCorreoNotificacion> subList = result.Where(x => x.Emprcodi == item.Emprcodi).ToList();
                    List<int> subListModulos = subList.Select(x => x.Modcodi).ToList();
                    List<string> correos = correosEmpresas.Where(x => x.Emprcodi == item.Emprcodi && subListModulos.Any(y => y == x.Modcodi)).Select(x=>x.Empremail).ToList();
                    string mensaje = plantilla.Plantcontenido.Replace(ConstantesStockCombustibles.VariableEmresa, item.Emprnomb);

                    string listFormato = "<ul>";
                    foreach (EnvioCorreoNotificacion itemFormato in subList)
                    {
                        listFormato = listFormato + "<li>" + itemFormato.Formatnomb + "</li>";
                    }
                    listFormato = listFormato + "</ul>";

                    mensaje = mensaje.Replace(ConstantesStockCombustibles.VariableListaFormato, listFormato);
                    mensaje = mensaje + firma;
                    COES.Base.Tools.Util.SendEmail(correosDestino, correoscc, correos, asunto, mensaje, correoFrom);
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }

        }

        #endregion
    }

    /// <summary>
    /// Estructura para manejar variables de correo
    /// </summary>
    public class VariableCorreo
    {
        public string Valor { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string ValorConDato { get; set; } = string.Empty;
    }

    public class EnvioCorreoNotificacion
    { 
        public int Formatcodi { get; set; }
        public string Formatnomb { get; set; }
        public string Emprnomb { get; set; }
        public int Emprcodi { get; set; }
        public int Modcodi { get; set; }
    }
}
