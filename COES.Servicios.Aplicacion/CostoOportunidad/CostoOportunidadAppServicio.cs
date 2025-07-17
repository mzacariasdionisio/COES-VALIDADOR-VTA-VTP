using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.TiempoReal;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CostoOportunidad
{
    public class CostoOportunidadAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CostoOportunidadAppServicio));        
        private readonly DespachoAppServicio wsDespacho = new DespachoAppServicio();

        #region Métodos Tabla MeMedicion48

        /// <summary>
        /// Guarda Valores Cargados48
        /// </summary>
        /// <param name="entitys"></param>
        public void GuardarValoresCargados48(List<MeMedicion48DTO> entitys)
        {
            foreach (MeMedicion48DTO entity in entitys)
            {
                FactorySic.GetMeMedicion48Repository().Save(entity);
            }

        }

        /// <summary>
        /// Elimina Valores Cargados48
        /// </summary>
        /// <param name="entitys"></param>
        public void EliminarValoresCargados48(List<MeMedicion48DTO> entitys)
        {
            foreach (MeMedicion48DTO entity in entitys)
            {
                FactorySic.GetMeMedicion48Repository().Delete(entity.Lectcodi, entity.Medifecha, entity.Tipoinfocodi, entity.Ptomedicodi);
            }

        }

        /// <summary>
        /// Devuelve lista de empresa por tipo de empresa
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>

        public List<MeMedicion48DTO> GetDespachoProgramado(DateTime fecha, int lectcodi)
        {
            return FactorySic.GetMeMedicion48Repository().GetDespachoProgramado(fecha, lectcodi);
        }

        /// <summary>
        /// Obtiene Reserva Programado
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetReservaProgramado(DateTime fecha, int lectcodi)
        {
            return FactorySic.GetMeMedicion48Repository().GetReservaProgramado(fecha, lectcodi);
        }

        /// <summary>
        /// Devuelve Listado Reservas
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListadoReservas(DateTime fecha, int lectcodi)
        {
            return FactorySic.GetMeMedicion48Repository().GetListadoReserva(fecha, lectcodi);
        }

        /// <summary>
        /// Genera el Html de ReservaProg
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string GenerarHtmlReservaProg(List<MeMedicion48DTO> lista, DateTime fecha)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            StringBuilder strHtml = new StringBuilder();
            List<EveRsfdetalleDTO> cabecera = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracionCO();
            strHtml.Append("<H3>RESERVA ASIGNADA URS - RDO (MW) - FECHA: " + fecha.ToString(ConstantesBase.FormatoFecha) + "</H3>");
            strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla3'>");
            strHtml.Append("<thead>");
            string filaUrsNomb = "<th>URS</th>";
            string filaEmprNomb = "<th>EMPRESA</th>";
            string filaGrupoNomb = "<th>GRUPO</th>";
            foreach (var config in cabecera)
            {
                filaUrsNomb += "<th>" + config.Ursnomb.Trim() + "</th>";
                filaEmprNomb += "<th>" + config.Emprnomb.Trim() + "</th>";
                filaGrupoNomb += "<th>" + config.Gruponomb.Trim() + "</th>";
            }

            strHtml.Append("<tr>" + filaUrsNomb + "</tr>");
            strHtml.Append("<tr>" + filaEmprNomb + "</tr>");
            strHtml.Append("<tr>" + filaGrupoNomb + "</tr>");
            strHtml.Append("</thead>");

            string bodyStr = BodyReporte(lista);
            strHtml.Append(bodyStr);
            strHtml.Append("</table>");

            return strHtml.ToString();
        }
      

        /// <summary>
        /// Cuerpo del reporte
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string BodyReporte(List<MeMedicion48DTO> lista)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<tbody>");
            List<EveRsfdetalleDTO> cabecera = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracionCO();
            int minuto = 0;

            if (lista.Any())
            {
                for (int k = 1; k <= 48; k++)
                {
                    minuto = minuto + 30;
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td>{0}</td>", lista[0].Medifecha.AddMinutes(minuto).ToString("HH:mm")));
                    foreach (EveRsfdetalleDTO config in cabecera)
                    {

                        MeMedicion48DTO entity = lista.Where(x => x.Grupourspadre == config.Grupocodi).ToList().FirstOrDefault();
                        if (entity != null)
                        {
                            var valorTemp = (decimal?)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                            if (valorTemp != null)
                            {
                                string kVvalor = ((decimal)valorTemp).ToString("N", nfi);
                                strHtml.Append(string.Format("<td>{0}</td>", kVvalor));
                            }
                            else
                            {
                                strHtml.Append(string.Format("<td>{0}</td>", ""));
                            }
                        }
                        else
                        {
                            strHtml.Append(string.Format("<td>--</td>"));
                        }

                    }
                    strHtml.Append("</tr>");

                }
            }
            strHtml.Append("</tbody>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera Lista HTML de despacho programado diario
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string GenerarHtmlDespacho(List<MeMedicion48DTO> lista, DateTime fecha)
        {

            StringBuilder strHtml = new StringBuilder();

            var listaptos = lista.OrderBy(x => x.Ptomedicodi).ToList();
            List<EveRsfdetalleDTO> cabecera = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracionCO();

            strHtml.Append("<H3>RDO URS CON RESERVA (MW) - FECHA: " + fecha.ToString(ConstantesBase.FormatoFecha) + "</H3>");

            strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>");
            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 140px;'>HORA/Grupo</th>");
            //foreach (var reg in lista)
            //{
            //    strHtml.Append("<th>" + reg.Equinomb + "</th>");
            //}
            foreach (var reg in cabecera)
            {
                strHtml.Append("<th>" + reg.Gruponomb.Trim() + "</th>");
            }

            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            if (lista.Count > 0)
            {
                string bodyStr = BodyReporte(lista);
                strHtml.Append(bodyStr);
            }
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera Lista HTML de despacho programado diario
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string GenerarHtmlDespachoSinReserva(List<MeMedicion48DTO> lista, DateTime fecha)
        {

            StringBuilder strHtml = new StringBuilder();

            var listaptos = lista.OrderBy(x => x.Ptomedicodi).ToList();

            List<EveRsfdetalleDTO> cabecera = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracionCO();

            strHtml.Append("<H3>RDO URS SIN RESERVA (MW) - FECHA: " + fecha.ToString(ConstantesBase.FormatoFecha) + "</H3>");

            strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' width='100%' id='tablaSin'>");
            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 140px;'>HORA/Grupo</th>");
            foreach (var reg in cabecera)
            {
                strHtml.Append("<th>" + reg.Gruponomb.Trim() + "</th>");
            }

            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            if (lista.Count > 0)
            {
                string bodyStr = BodyReporte(lista);
                strHtml.Append(bodyStr);
            }
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera contenido de informacion de listado de datos de medicion 48
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string BodyHtmlDespacho(List<MeMedicion48DTO> lista)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<tbody>");

            int minuto = 0;
            List<EveRsfdetalleDTO> cabecera = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracionCO();

            if (lista.Count() != 0)
            {
                for (int k = 1; k <= 48; k++)
                {
                    minuto = minuto + 30;
                    //for (int j = 0; j < lista.Count(); j++)
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td>{0}</td>", lista[0].Medifecha.AddMinutes(minuto).ToString("HH:mm")));
                    foreach (var reg in cabecera)
                    {
                        decimal valor = 0M;
                        List<MeMedicion48DTO> entitys = lista.Where(x => x.Equipadre == reg.Equicodi).ToList();
                        if (entitys.Count > 0)
                        {
                            foreach (var registro in entitys)
                            {
                                valor += (decimal)registro.GetType().GetProperty("H" + k).GetValue(registro, null);
                            }
                            string kVvalor = valor.ToString("N", nfi);
                            strHtml.Append(string.Format("<td>{0}</td>", kVvalor));
                        }
                        else
                        {
                            var entity = lista.Where(x => x.Equicodi == reg.Equicodi).FirstOrDefault();
                            if (entity != null)
                            {
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                string kVvalor = valor.ToString("N", nfi);
                                strHtml.Append(string.Format("<td>{0}</td>", kVvalor));
                            }
                            else
                            {
                                strHtml.Append(string.Format("<td>--</td>"));
                            }
                        }
                    }
                    strHtml.Append("</tr>");
                }
            }

            strHtml.Append("</tbody>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera contenido de informacion de listado de datos de medicion 48
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string BodyHtmlDespachoSinReserva(List<MeMedicion48DTO> lista)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<tbody>");

            int minuto = 0;
            List<PrGrupoDTO> cabecera = GetListaModosOpNCP();
            if (lista.Count() != 0)

                for (int k = 1; k <= 48; k++)
                {
                    minuto = minuto + 30;
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td>{0}</td>", lista[0].Medifecha.AddMinutes(minuto).ToString("HH:mm")));
                    foreach (var reg in cabecera)
                    {
                        decimal valor = 0M;
                        var entity = lista.Where(x => x.Ptomedicodi == reg.PtoMediCodi).FirstOrDefault();
                        if (entity != null)
                        {
                            var res = (decimal?)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                            if (res != null)
                            {
                                valor = (decimal)res;
                                string kVvalor = valor.ToString("N", nfi);
                                strHtml.Append(string.Format("<td>{0}</td>", kVvalor));
                            }
                            else
                            {
                                strHtml.Append(string.Format("<td>-</td>"));
                            }

                        }
                        else
                        {
                            strHtml.Append(string.Format("<td>--</td>"));
                        }

                    }
                    strHtml.Append("</tr>");
                }

            strHtml.Append("</tbody>");

            return strHtml.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<EveRsfdetalleDTO> GetReservaEjec(DateTime fecha)
        {
            return FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(fecha);
        }

        /// <summary>
        /// Permite exportar los datos cada 30 minutos
        /// </summary>
        /// <param name="detalle"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string GenerarHtmlReservaEjec(List<EveRsfdetalleDTO> detalle, DateTime fecha)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            #region Obtencion de datos

            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion();
            List<EveRsfdetalleDTO> cabecera = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracionCO();
            List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);

            foreach (EveRsfdetalleDTO item in detalle)
            {
                EveRsfhoraDTO hora = horas.Where(x => x.Rsfhorcodi == item.Rsfhorcodi).First();
                item.HorInicio = hora.Rsfhorinicio;
                item.HorFin = hora.Rsfhorfin;
            }

            List<HoraExcel> resultadoAutomatico = new List<HoraExcel>();
            List<HoraExcel> horaExcel = new List<HoraExcel>();


            List<HoraExcel> listHora = GeneraDetalleReserva(resultadoAutomatico, horaExcel, configuracion, detalle, cabecera, 1);

            #endregion

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<H3>RESERVA EJECUTADA (MW) - FECHA: " + fecha.ToString(ConstantesBase.FormatoFecha) + "</H3>");
            strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla2'>");
            strHtml.Append("<thead>");
            string filaUrsNomb = "<th>URS</th>";
            string filaEmprNomb = "<th>EMPRESA</th>";
            string filaGrupoNomb = "<th>GRUPO</th>";
            foreach (EveRsfdetalleDTO config in cabecera)
            {
                filaUrsNomb += "<th>" + config.Ursnomb.Trim() + "</th>";
                filaEmprNomb += "<th>" + config.Emprnomb.Trim() + "</th>";
                filaGrupoNomb += "<th>" + config.Gruponomb.Trim() + "</th>";
            }

            strHtml.Append("<tr>" + filaUrsNomb + "<th>-</th></tr>");
            strHtml.Append("<tr>" + filaEmprNomb + "<th>-</th></tr>");
            strHtml.Append("<tr>" + filaGrupoNomb + "<th>TOTAL</th></tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            string celda = "";
            string strhora = "";
            foreach (HoraExcel item in listHora)
            {
                strhora = item.Hora.ToString().PadLeft(2, '0') + ":" + item.Minuto.ToString().PadLeft(2, '0');
                strHtml.Append("<tr><td>" + strhora + "</td>");
                decimal totales = 0;
                foreach (EveRsfdetalleDTO config in cabecera)
                {
                    celda = "<td></td>";
                    if (config.Grupotipo != ConstantesAppServicio.SI)
                    {
                        var childs = resultadoAutomatico.Where(x => x.Hora == item.Hora && x.Minuto == item.Minuto &&
                            x.IdGrupo == config.Grupocodi && x.IdEquipo == config.Equicodi && x.Automatico != null);
                        HoraExcel child = childs.FirstOrDefault();
                        if (child != null)
                        {
                            if (child.Automatico != null) { celda = "<td>" + ((decimal)child.Automatico).ToString("N", nfi) + "</td>"; }
                            totales += (decimal)child.Automatico;
                        }
                    }
                    else
                    {
                        decimal total = 0;
                        bool hayDatos = false;
                        List<HoraExcel> childs = resultadoAutomatico.Where(x => x.Hora == item.Hora && x.Minuto == item.Minuto &&
                            x.IdGrupo == config.Grupocodi && x.IdEquipoPadre == config.Equicodi && x.Automatico != null).ToList();
                        foreach (var reg in childs)
                        {
                            if (reg.Automatico != null)
                            {
                                total = (decimal)reg.Automatico;
                                hayDatos = true;
                            }

                        }
                        if (childs.Count > 0) { totales += (decimal)childs[0].Automatico; }

                        if (hayDatos) { celda = "<td>" + total.ToString("N", nfi) + "</td>"; }
                    }
                    strHtml.Append(celda);
                }

                strHtml.Append("<td>" + totales.ToString("N", nfi) + "</td>");
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");


            return strHtml.ToString();
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="m"></param>
        /// <param name="r"></param>
        /// <param name="v"></param>
        /// <param name="pr"></param>
        /// <param name="eq"></param>
        /// <param name="eq_pa"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public HoraExcel AddAutomatico(int h, int m, int r, decimal v, int pr, int eq, int eq_pa, int val)
        {
            return new HoraExcel
            {
                Hora = h,
                Minuto = m,
                restoMin = r,
                Automatico = v,
                IdGrupo = pr,
                IdEquipo = eq,
                IdEquipoPadre = eq_pa,
                valida = val
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resulAutomatico"></param>
        /// <param name="horaExcel"></param>
        /// <param name="configuracion"></param>
        /// <param name="detalle"></param>
        /// <param name="cabe"></param>
        /// <param name="tipoReserva"></param>
        /// <returns></returns>
        public List<HoraExcel> GeneraDetalleReserva(List<HoraExcel> resulAutomatico, List<HoraExcel> horaExcel, List<EveRsfdetalleDTO> configuracion, List<EveRsfdetalleDTO> detalle, List<EveRsfdetalleDTO> cabe, int tipoReserva)
        {
            //resulAutomatico = new List<HoraExcel>();
            foreach (EveRsfdetalleDTO item in cabe)
            {
                bool val = true;
                List<EveRsfdetalleDTO> list = detalle.Where(x => x.Grupocodi == item.Grupocodi && x.Equipadre == item.Equicodi).OrderBy(y => y.HorInicio).ToList();

                if (list.Count == 0) { list = detalle.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).OrderBy(y => y.HorInicio).ToList(); val = false; }

                #region "Union de Fragmento de Horarios por URS"
                List<EveRsfdetalleDTO> newlist = new List<EveRsfdetalleDTO>();
                if (val)
                {
                    for (int v = 0; v < list.Count; v++)
                    {
                        decimal vFill = 0;
                        var upd = list.Where(x => x.HorInicio == list[v].HorInicio).ToList();
                        if (upd.Count > 1)
                        {
                            foreach (var up in upd)
                            {
                                if (up.vali == 1) { vFill = (decimal)up.Rsfdetvalaut; break; }
                                else { vFill += (up.Rsfdetvalaut == null) ? 0 : (decimal)up.Rsfdetvalaut; }
                            }
                        }
                        else { foreach (var up in upd) { vFill += (up.Rsfdetvalaut == null) ? 0 : (decimal)up.Rsfdetvalaut; } }

                        list[v].Rsfdetvalaut = vFill;
                        list[v].vali = 1;
                        newlist.Add(list[v]);

                        foreach (var del in upd) { list.Remove(del); }
                        v = -1;
                    }

                    foreach (var change in newlist) { list.Add(change); }
                    newlist = new List<EveRsfdetalleDTO>();
                }

                for (int v = 0; v < list.Count; v++)
                {
                    if (v == 0) { newlist.Add(list[v]); }
                    else
                    {
                        if (list[v].Rsfdetvalaut == list[v - 1].Rsfdetvalaut) { var upd = newlist.Find(x => x.HorFin == list[v - 1].HorFin); upd.HorFin = list[v].HorFin; }
                        else { newlist.Add(list[v]); }
                    }
                }
                #endregion

                foreach (EveRsfdetalleDTO child in newlist)
                {
                    int hymIni = 0, hymFin = 0, rest = 0;
                    int grupo = (int)item.Grupocodi;
                    int equi = (int)item.Equicodi;
                    int equipadre = (int)child.Equipadre;

                    int horaIni = ((DateTime)child.HorInicio).Hour;
                    int mIni = ((DateTime)child.HorInicio).Minute;
                    int horaFin = ((DateTime)child.HorFin).Hour;
                    int mFin = ((DateTime)child.HorFin).Minute;

                    decimal valor = 0;
                    valor = (child.Rsfdetvalaut == null) ? 0 : (decimal)child.Rsfdetvalaut;

                    hymIni = (horaIni * 60) + mIni;
                    hymFin = (horaFin * 60) + mFin;

                    int restMin = hymFin - hymIni;//if (horaIni == horaFin) - Se agrego para eliminar hora 00:22
                    if (restMin < 30)
                    {
                        rest = 0;
                        if (horaIni == horaFin) { resulAutomatico.Add(AddAutomatico(horaIni, mFin, rest, valor, grupo, equi, equipadre, 0)); }
                        if (horaIni != horaFin && mIni > 30)
                        {
                            resulAutomatico.Add(AddAutomatico(horaFin, 0, rest, valor, grupo, equi, equipadre, 0));
                            resulAutomatico.Add(AddAutomatico(horaFin, 30, mFin, valor, grupo, equi, equipadre, 0));
                        }
                    }

                    int valMinIni = 0, valMinFin = 0;
                    if (restMin > 30)
                    {
                        DateTime fec = DateTime.Parse("01/01/1999 12:00 am");
                        if (mIni <= 30) { valMinIni = (horaIni * 60) + 30; }
                        if (mIni > 30) { valMinIni = (horaIni + 1) * 60; }

                        if (mFin <= 30) { valMinFin = (horaFin * 60) + 0; }
                        if (mFin > 30) { valMinFin = (horaFin * 60) + 30; }
                        for (int m = valMinIni; m <= valMinFin; m++)
                        {
                            int hor = int.Parse(fec.AddMinutes(m).ToString("HH"));
                            int min = int.Parse(fec.AddMinutes(m).ToString("mm"));

                            var consul = resulAutomatico.Find(c => c.IdEquipoPadre == equipadre && c.IdEquipo == equi && c.IdGrupo == grupo && c.Hora == hor && c.Minuto == min);
                            if (consul == null)
                            {
                                if (child.HorInicio.Value.Hour == hor && child.HorInicio.Value.Minute == min)
                                {
                                    var veri_ = resulAutomatico.Find(c => c.IdEquipoPadre == equipadre && c.IdEquipo == equi && c.IdGrupo == grupo && c.Hora == (min == 30 ? hor : hor - 1) && c.Minuto == (min == 0 ? 30 : min - 30));
                                    if (veri_ != null) { resulAutomatico.Add(AddAutomatico(hor, min, 0, ((veri_.Automatico == null) ? 0 : (decimal)veri_.Automatico), grupo, equi, equipadre, 0)); }
                                    else { resulAutomatico.Add(AddAutomatico(hor, min, 0, valor, grupo, equi, equipadre, 0)); }
                                }
                                else { resulAutomatico.Add(AddAutomatico(hor, min, 0, valor, grupo, equi, equipadre, 0)); }
                            }
                            else
                            {
                                if (consul.valida == 0) { consul.Valor = valor; }
                                if (consul.Automatico > 0 && valor > 0) { /*consul.Automatico = valor;*/ consul.restoMin = /*(tipoReserva == 0 && hor != 0) ? consul.restoMin :*/ 30; }
                                if (consul.Automatico == 0 && valor > 0) { consul.Automatico = valor; }
                                if (tipoReserva == 0)
                                {

                                }
                            }

                            m = m + 29;
                        }

                        if (mFin > 30)
                        {
                            if (mFin != 59) { resulAutomatico.Add(AddAutomatico(horaFin + 1, 0, (valor != 0) ? mFin - 30 : 30 - (mFin - 30), valor, grupo, equi, equipadre, 1)); }
                            if (mFin == 59) { resulAutomatico.Add(AddAutomatico(horaFin, mFin, 0, valor, grupo, equi, equipadre, 0)); }
                        }
                        if (mFin < 30) { resulAutomatico.Add(AddAutomatico(horaFin, 30, (valor != 0) ? mFin : 30 - mFin, valor, grupo, equi, equipadre, 0)); }
                    }
                }
            }

            List<HoraExcel> listAutomatico = (from itemAuto in resulAutomatico
                                              orderby itemAuto.Hora, itemAuto.Minuto
                                              select new HoraExcel { Hora = itemAuto.Hora, Minuto = itemAuto.Minuto }).Distinct().ToList();

            foreach (HoraExcel item in listAutomatico)
            {
                if (horaExcel.Where(x => x.Hora == item.Hora && x.Minuto == item.Minuto).Count() == 0)
                {
                    horaExcel.Add(new HoraExcel { Hora = item.Hora, Minuto = item.Minuto });
                }
            }

            return (from hora in horaExcel orderby hora.Hora, hora.Minuto select hora).ToList();
        }

        /// <summary>
        /// Permite obtener la configuracion
        /// </summary>
        /// <returns></returns>
        public List<EveRsfdetalleDTO> ObtenerConfiguracion()
        {
            List<EveRsfdetalleDTO> configuracion = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracionCO();
            List<EveRsfdetalleDTO> resultado = new List<EveRsfdetalleDTO>();

            foreach (EveRsfdetalleDTO item in configuracion)
            {
                //if (item.Grupotipo != ConstantesAppServicio.SI)
                //{
                resultado.Add(item);
                //}
                //else 
                if (item.Grupotipo == ConstantesAppServicio.SI)
                {
                    List<EqEquipoDTO> equipos = FactorySic.GetEqEquipoRepository().ObtenerPorPadre(item.Equicodi);

                    foreach (EqEquipoDTO equipo in equipos)
                    {
                        EveRsfdetalleDTO entity = new EveRsfdetalleDTO
                        {
                            Grupocodi = item.Grupocodi,
                            Emprnomb = item.Emprnomb,
                            Gruponomb = item.Gruponomb,
                            Ursnomb = item.Ursnomb + " - " + equipo.Equinomb,
                            Equicodi = equipo.Equicodi
                        };
                        resultado.Add(entity);
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Genera Reporte Html final de Despacho con Reserva y Sin Reserva
        /// </summary>
        /// <param name="listaReserEjec"></param>
        /// <param name="listaDespacho"></param>
        /// <param name="listaDespachoSin"></param>
        /// <param name="listaReserProg"></param>
        /// <param name="listaCruceReserva"></param>
        /// <param name="fecha"></param>
        /// <param name="tipoReserva"></param>
        /// <returns></returns>
        public string GenerarHtmlCruceReserva(List<EveRsfdetalleDTO> listaReserEjec, List<MeMedicion48DTO> listaDespacho, List<MeMedicion48DTO> listaDespachoSin, List<MeMedicion48DTO> listaReserProg, List<MeMedicion48DTO> listaCruceReserva, DateTime fecha, int tipoReserva)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            StringBuilder strHtml = new StringBuilder();

            List<EveRsfdetalleDTO> cabecera = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracionCO().ToList();
            if (tipoReserva == 1)
            {
                strHtml.Append("<H3>DESPACHO CON ASIGNACIÓN DE RESERVA (MW) - FECHA: " + fecha.ToString(ConstantesBase.FormatoFecha) + "</H3>");
                strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla4'>");
            }
            else
            {
                strHtml.Append("<H3>DESPACHO SIN ASIGNACIÓN DE RESERVA (MW) - FECHA: " + fecha.ToString(ConstantesBase.FormatoFecha) + "</H3>");
                strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla5'>");
            }

            strHtml.Append("<thead>");
            string filaUrsNomb = "<th>URS</th>";
            string filaEmprNomb = "<th>EMPRESA</th>";
            string filaGrupoNomb = "<th>GRUPO</th>";
            foreach (EveRsfdetalleDTO config in cabecera)
            {
                filaUrsNomb += "<th>" + config.Ursnomb.Trim() + "</th>";
                filaEmprNomb += "<th>" + config.Emprnomb.Trim() + "</th>";
                filaGrupoNomb += "<th>" + config.Gruponomb.Trim() + "</th>";
            }

            strHtml.Append("<tr>" + filaUrsNomb + "</tr>");
            strHtml.Append("<tr>" + filaEmprNomb + "</tr>");
            strHtml.Append("<tr>" + filaGrupoNomb + "</tr>");
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            int minuto = 0;

            #region "Union de Fragmento de Horarios por URS"
            List<EveRsfdetalleDTO> listReserEjec = new List<EveRsfdetalleDTO>();
            List<EveRsfdetalleDTO> list = new List<EveRsfdetalleDTO>();

            foreach (var chang in listaReserEjec) { list.Add(chang); }
            for (int v = 0; v < list.Count; v++)
            {
                decimal vFill = 0;
                var upd = list.Where(x => x.Grupocodi == list[v].Grupocodi && x.HorInicio == list[v].HorInicio).ToList();
                if (upd.Count > 1)
                {
                    foreach (var up in upd) { if (up.vali == 1) { vFill = (decimal)up.Rsfdetvalaut; } }
                }
                else { foreach (var up in upd) { vFill += (up.Rsfdetvalaut == null) ? 0 : (decimal)up.Rsfdetvalaut; } }

                list[v].Rsfdetvalaut = vFill;
                listReserEjec.Add(list[v]);

                foreach (var del in upd) { list.Remove(del); }
                v = -1;
            }

            foreach (var change in listReserEjec) { list.Add(change); }
            listReserEjec = new List<EveRsfdetalleDTO>();

            for (int v = 0; v < list.Count; v++)
            {
                if (v == 0) { listReserEjec.Add(list[v]); }
                else
                {
                    if (list[v].Rsfdetvalaut == list[v - 1].Rsfdetvalaut && list[v].Grupocodi == list[v - 1].Grupocodi)
                    { var upd = listReserEjec.Find(x => x.HorFin == list[v - 1].HorFin); upd.HorFin = list[v].HorFin; }
                    else { listReserEjec.Add(list[v]); }
                }
            }
            #endregion

            #region Obtencion de datos

            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion();
            List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);

            foreach (EveRsfdetalleDTO item in listaReserEjec)
            {
                EveRsfhoraDTO hora = horas.Where(x => x.Rsfhorcodi == item.Rsfhorcodi).First();
                item.HorInicio = hora.Rsfhorinicio;
                item.HorFin = hora.Rsfhorfin;
            }

            List<HoraExcel> resultadoAutomatico = new List<HoraExcel>();
            List<HoraExcel> horaExcel = new List<HoraExcel>();


            List<HoraExcel> listHora = GeneraDetalleReserva(resultadoAutomatico, horaExcel, configuracion, listaReserEjec, cabecera, 1);

            #endregion
            /*
            //Inicio * Oliver - 20170912
            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion();
            List<HoraExcel> resultadoAutomatico = new List<HoraExcel>();
            List<HoraExcel> horaExcel = new List<HoraExcel>();
            List<HoraExcel> listHora = GeneraDetalleReserva(resultadoAutomatico, horaExcel, configuracion, listaReserEjec, cabecera, tipoReserva);
            //Fin * Oliver - 20170912
            */
            for (int k = 1; k <= 48; k++)
            {
                minuto = minuto + 30;
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td>{0}</td>", fecha.AddMinutes(minuto).ToString("HH:mm")));
                foreach (EveRsfdetalleDTO config in cabecera)
                {
                    string estilo = string.Empty;
                    decimal despachoValor = 0;
                    decimal despachoValorSin = 0;
                    decimal nValor = 0, rsvaProg = 0, rsvaEjec = 0;
                    int minutos = 0;
                    bool valMin = true;

                    if (minuto == 1440) { minuto--; }
                    DateTime fec = DateTime.Parse(fecha.ToShortDateString() + " " + fecha.AddMinutes(minuto).ToString("HH:mm"));

                    ///Reserva Asignada Programada
                    var findProg = listaReserProg.Find(x => x.Grupourspadre == config.Grupocodi);
                    if (findProg != null)
                    {
                        var rsvprog = (decimal?)findProg.GetType().GetProperty("H" + k.ToString()).GetValue(findProg, null);
                        if (rsvprog != null) rsvaProg = (decimal)rsvprog;
                    }

                    //Reserva Asignada Ejecutada
                    List<HoraExcel> findEjec = new List<HoraExcel>();
                    if (fec.Hour == 0 && fec.Minute == 30)
                    {
                        findEjec = resultadoAutomatico.Where(x => x.IdGrupo == config.Grupocodi && x.IdEquipo == config.Equicodi
                        && x.Hora == int.Parse(fec.ToString("HH")) && x.Minuto <= int.Parse(fec.ToString("mm"))).ToList();
                        if (findEjec.Count > 1) {
                            rsvaEjec = (decimal)findEjec[0].Automatico;
                            minutos = (rsvaEjec == 0) ? (30 - findEjec[0].Minuto) : findEjec[0].Minuto;
                            valMin = false;
                        }
                    }
                    else {
                        findEjec = resultadoAutomatico.Where(x => x.IdGrupo == config.Grupocodi && x.IdEquipo == config.Equicodi
                        && x.Hora == int.Parse(fec.ToString("HH")) && x.Minuto == int.Parse(fec.ToString("mm"))).ToList();
                    }

                    if (findEjec.Count > 0 && valMin)
                    {
                        rsvaEjec = (decimal)findEjec[0].Automatico;
                        minutos = (findEjec[0].restoMin == 0) ? 30 : findEjec[0].restoMin;
                    }

                    if (rsvaProg == 0 && rsvaEjec == 0) { estilo = "<td>" + 0.ToString("N", nfi) + "</td>"; }
                    else
                    {
                        /// Se busca Despacho
                        MeMedicion48DTO entity = listaDespacho.Where(x => x.Grupourspadre == config.Grupocodi).FirstOrDefault();
                        if (entity != null)
                        {
                            var despValor = (decimal?)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                            if (despValor != null) despachoValor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                        }

                        /// Se busca DespachiSin
                        entity = listaDespachoSin.Where(x => x.Grupourspadre == config.Grupocodi).FirstOrDefault();
                        if (entity != null)
                        {
                            var despValorSin = (decimal?)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                            if (despValorSin != null) despachoValorSin = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                        }

                        nValor = despachoValorSin;
                        if (tipoReserva == 1)
                        {
                            nValor = despachoValor;
                            if (rsvaProg == 0)
                            {
                                var result = CalcularCeldaDespacho(fecha, k, (int)config.Grupocodi, minutos, despachoValor, rsvaEjec, despachoValorSin);

                                estilo = "<td style='background:#FF69B4 !important;'>" + result.ToString("N", nfi) + "</td>";
                            }
                            else
                            {
                                if (minutos > 0 && minutos < 30) { nValor = ((decimal)minutos / 30) * nValor; }
                                if (findEjec.Count > 0) { if (findEjec[0].restoMin == 0 && findEjec[0].Automatico == 0 && valMin) { nValor = 0; } }
                                estilo = "<td>" + nValor.ToString("N", nfi) + "</td>";
                            }
                        }
                        else
                        {
                            if (rsvaProg == 0 && rsvaEjec > 0) { nValor = despachoValor; }
                            if (rsvaProg > 0 && rsvaEjec > 0) { nValor = despachoValorSin; }
                            //if (rsvaProg > 0 && rsvaEjec == 0) { nValor = 0; }

                            if (findEjec.Count > 0)
                            {
                                if (findEjec[0].restoMin != 0 || minutos < 30) { nValor = ((decimal)minutos / 30) * nValor; }
                                if (findEjec[0].restoMin == 0 && findEjec[0].Automatico == 0 && valMin) { nValor = 0; }
                            }
                            //if (rsvaEjec == 0) { nValor = 0; }

                            estilo = "<td>" + nValor.ToString("N", nfi) + "</td>";
                        }
                    }

                    strHtml.Append(estilo);
                }
                strHtml.Append("</tr>");
            }

            return strHtml.ToString();
        }

        /// <summary>
        /// Crea lista de cruces y lista de reserva ejecutada
        /// </summary>
        /// <param name="listaReserProg"></param>
        /// <param name="listaReservaEjec"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> CrearlistaCruceReserva(List<MeMedicion48DTO> listaReserProg, List<MeMedicion48DTO> listaReservaEjec)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            MeMedicion48DTO registro, regEject;
            foreach (var reg in listaReserProg)
            {
                registro = new MeMedicion48DTO();
                registro.Grupocodi = reg.Grupourspadre;
                lista.Add(registro);
                regEject = new MeMedicion48DTO();
                regEject.Grupocodi = reg.Grupourspadre;
                listaReservaEjec.Add(regEject);
            }


            return lista;
        }

        /// <summary>
        /// Encuentra las celdas que tienen diferencias entre la reserva ejecutada y reserva programada, llena loas celdas con diferencias de minutos
        /// </summary>
        /// <param name="listaReserEjec"></param>
        /// <param name="listaReserProg"></param>
        /// <param name="listaReservaEjec"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaCruce(List<EveRsfdetalleDTO> listaReserEjec, List<MeMedicion48DTO> listaReserProg, List<MeMedicion48DTO> listaReservaEjec, DateTime fecha)
        {
            List<MeMedicion48DTO> listaCruceReserva = CrearlistaCruceReserva(listaReserProg, listaReservaEjec);
            if (listaReserEjec.Count == 0)
                return listaCruceReserva;
            List<EveRsfdetalleDTO> cabecera = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracionCO();
            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion();
            List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);
            foreach (EveRsfdetalleDTO item in listaReserEjec)
            {
                EveRsfhoraDTO hora = horas.Where(x => x.Rsfhorcodi == item.Rsfhorcodi).First();
                item.HorInicio = hora.Rsfhorinicio;
                item.HorFin = hora.Rsfhorfin;
            }

            List<HoraExcel> resultadoAutomatico = new List<HoraExcel>();
            List<HoraExcel> horaExcel = new List<HoraExcel>();
            List<HoraExcel> listHora = GeneraDetalleReserva(resultadoAutomatico, horaExcel, configuracion, listaReserEjec, cabecera, 1);
            foreach (HoraExcel item in listHora)
            {
                foreach (EveRsfdetalleDTO config in cabecera)
                {
                    HoraExcel child = resultadoAutomatico.Where(x => x.Hora == item.Hora && x.Minuto == item.Minuto &&
                                                   x.IdGrupo == config.Grupocodi && x.IdEquipo == config.Equicodi && x.Automatico != null).FirstOrDefault();
                    if (child != null)
                    {
                        if (child.Automatico != null)
                        {
                            if (child.Hora == 0 && child.Minuto < 30) { }
                            else
                            {
                                if ((int)item.Hora == 24 || item.Hora == 24 || item.Hora.ToString() == "24") { item.Hora = 23; item.Minuto = 59; }
                                string hor = (item.Hora == 0) ? "0" : item.Hora.ToString();
                                string min = (item.Minuto == 0) ? "00" : item.Minuto.ToString();

                                DateTime fec = DateTime.Parse(fecha.ToShortDateString() + " " + hor + ":" + min);
                                var findEjec = listaReserEjec.Where(x => x.Grupocodi == config.Grupocodi && x.Equipadre == config.Equicodi && (x.HorInicio <= fec && x.HorFin >= fec)).ToList();
                                if (findEjec.Count > 0)
                                {
                                    decimal vFill = 0;
                                    foreach (var fill in findEjec) { vFill += (fill.Rsfdetvalaut == null) ? 0 : (decimal)fill.Rsfdetvalaut; }
                                    if (child.Automatico == 0) { child.Automatico = vFill; }
                                }
                            }

                            decimal reserva = (decimal)child.Automatico;
                            if (reserva > 0)
                            {
                                int indice = child.Hora * 2 + child.Minuto / 30;
                                int resto = child.Minuto % 30;
                                if (resto > 0)
                                    indice += 1;
                                if (indice == 0)
                                    indice++;
                                var find = listaReserProg.Where(x => x.Grupourspadre == config.Grupocodi).FirstOrDefault();
                                if (find != null)
                                {
                                    var reservaProg = (decimal?)find.GetType().GetProperty("H" + indice.ToString()).GetValue(find, null);
                                    if (reservaProg == null || reservaProg == 0)
                                    {
                                        ///Se encontro diferencia en cruce
                                        var regFind = listaCruceReserva.Find(x => x.Grupocodi == config.Grupocodi);
                                        if (regFind != null)
                                        {
                                            if (resto == 0) resto = 30;
                                            if (child.restoMin != 0) resto = child.restoMin;

                                            regFind.GetType().GetProperty("H" + indice.ToString()).SetValue(regFind, (decimal?)resto);
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }

            return listaCruceReserva;
        }

        /// <summary>
        /// Calcula Valor final de celda con diferencia de reserva ejecutado vs programado
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="fila"></param>
        /// <param name="urscodi"></param>
        /// <param name="minutos"></param>
        /// <param name="despacho"></param>
        /// <param name="reserva"></param>
        /// <param name="despachoSin"></param>
        /// <returns></returns>
        private decimal CalcularCeldaDespacho(DateTime fecha, int fila, int urscodi, int minutos, decimal despacho, decimal reserva, decimal despachoSin)
        {
            decimal valor = 0;
            decimal valorA = 0;
            decimal valorB = 0;
            decimal valorC = 0;
            decimal potRef = 0;
            decimal porcentajeRpf = 0;
            decimal potMinima = 0;
            decimal potEfectiva = 0;
            int modo = 0;
            fecha = fecha.AddMinutes(fila * 30);
            var lista = FactorySic.GetEveHoraoperacionRepository().GetHorasURS(fecha, fecha.AddDays(1)).Where(x => x.Grupourspadre == urscodi).ToList();
            if (lista.Count > 0)
            {
                var countGrupo = FactorySic.GetPrGrupoRepository().GetByIdGrupoPadre(urscodi);
                if (countGrupo.Count > 1)
                {
                    foreach (var reg in lista)
                    {
                        if (reg.Hophorini <= fecha && reg.Hophorfin >= fecha)
                        {
                            modo = (int)reg.Grupocodi;
                            break;
                        }
                    }
                }
                else { modo = countGrupo[0].Grupocodi; }
            }
            else
            {
                List<PrGrupoDTO> arr = FactorySic.GetPrGrupoRepository().GetByIdGrupoPadre(urscodi);
                if (arr.Count > 0) { if (arr[0].Fenergcodi == 1 || arr[0].Fenergcodi == 2) { modo = arr[0].Grupocodi; } }
            }
            var regbanda = FactorySic.GetCoBandancpRepository().GetByCriteria(fecha, modo).FirstOrDefault();
            var listaParametros = wsDespacho.ObtenerDatosMO_URS(modo, fecha);
            var oGrupo = FactorySic.GetPrGrupoRepository().GetById(modo);
            string tipo = oGrupo.Grupotipo;

            if (listaParametros.Count > 0)
            {
                porcentajeRpf = 0;
                potEfectiva = 0;
                potMinima = 0;
                for (int j = 0; j < listaParametros.Count; j++)
                {
                    if (tipo == "T")
                    {
                        switch (listaParametros[j].Concepcodi)
                        {
                            case ConstantesCostoOportunidad.PotenciaEfectiva:
                                potEfectiva = this.AnalizarNumerico(listaParametros[j].Formuladat) ? Convert.ToDecimal(listaParametros[j].Formuladat) : ConstantesAppServicio.ErrorPotMax;
                                break;
                            case ConstantesCostoOportunidad.PotenciaMinima:
                                potMinima = this.AnalizarNumerico(listaParametros[j].Formuladat) ? Convert.ToDecimal(listaParametros[j].Formuladat) : ConstantesAppServicio.ErrorPotMin;
                                break;
                            case ConstantesCostoOportunidad.PorcentajeRpf:
                                porcentajeRpf = this.AnalizarNumerico(listaParametros[j].Formuladat) ? Convert.ToDecimal(listaParametros[j].Formuladat) / 100 : ConstantesAppServicio.ErrorPorcRPF;
                                break;
                        }
                    }
                    if (tipo == "H")
                    {
                        switch (listaParametros[j].Concepcodi)
                        {
                            case ConstantesCostoOportunidad.PorcentajeRpfHidro:
                                porcentajeRpf = this.AnalizarNumerico(listaParametros[j].Formuladat) ? Convert.ToDecimal(listaParametros[j].Formuladat) / 100 : ConstantesAppServicio.ErrorPorcRPF;
                                break;
                        }
                    }
                }
            }

            var rpf_grupodat = FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(fecha).Find(x => x.Concepcodi == ConstantesCostoOportunidad.ConcepcodiRpf);
            if (rpf_grupodat != null) { porcentajeRpf = decimal.Parse(rpf_grupodat.Formuladat); }

            // calcular potencia de referencia
            if (regbanda != null)
            {
                valorA = despachoSin;
                potRef = (decimal)regbanda.Bandmin;
                valorB = (potRef + reserva) / (1 - porcentajeRpf);
                potRef = (decimal)regbanda.Bandmax;
                valorC = (potRef - reserva) / (1 + porcentajeRpf);
            }
            if (valorA < valorB)
                valor = valorB;
            if (valorA > valorC)
                valor = valorC;
            if (valorA > valorB && valorA < valorC)
                valor = valorA;
            // calcular valor final
            valor = (decimal)minutos / 30 * valor;
            return valor;
        }

        /// <summary>
        /// Genera Excel de Despacho Final con Reserva y Sin Reserva
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaReserEjec"></param>
        /// <param name="listaDespacho"></param>
        /// <param name="listaDespachoSin"></param>
        /// <param name="listaReserProg"></param>
        /// <param name="fecha"></param>
        /// <param name="tipoReserva"></param>
        public void GeneraTablaExcelDespacho(ExcelWorksheet ws, List<EveRsfdetalleDTO> listaReserEjec, List<MeMedicion48DTO> listaDespacho, List<MeMedicion48DTO> listaDespachoSin, List<MeMedicion48DTO> listaReserProg/*, List<MeMedicion48DTO> listaCruceReserva*/, DateTime fecha, int tipoReserva)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            int xFil = 3;
            int iniCol = 0;
            string titulo = string.Empty;

            List<EveRsfdetalleDTO> cabecera = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracionCO();
            int ancho = cabecera.Count();
            if (tipoReserva == 0) //Sin Reserva
            {
                titulo = "DESPACHO SIN ASIGNACIÓN DE RESERVA (MW)";
                iniCol = 2;
            }
            else //Con Reserva
            {
                titulo = "DESPACHO CON ASIGNACIÓN DE RESERVA (MW)";
                iniCol = cabecera.Count() + 4;
            }
            ws.Cells[xFil, iniCol].Value = titulo;
            using (ExcelRange r = ws.Cells[xFil, iniCol, xFil, iniCol + ancho])
            {
                r.Style.Font.Bold = true;
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(188, 186, 184));
                r.Merge = true;
            }
            var border = ws.Cells[xFil, iniCol, xFil, iniCol + ancho].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            ws.Cells[xFil + 2, iniCol].Value = "HORA";
            ExcelRange rs = ws.Cells[xFil + 2, iniCol, xFil + 3, iniCol];
            rs.Merge = true;
            int sizeFont = 8;
            using (ExcelRange r = ws.Cells[xFil + 2, iniCol, xFil + 3, iniCol + ancho])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.Font.Size = sizeFont;
                r.Style.Font.Bold = true;
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(188, 186, 184));
            }
            var border2 = ws.Cells[xFil + 2, iniCol, xFil + 3, iniCol + ancho].Style.Border;
            border2.Bottom.Style = border2.Top.Style = border2.Left.Style = border2.Right.Style = ExcelBorderStyle.Thin;

            using (ExcelRange r1 = ws.Cells[xFil + 4, iniCol, xFil + 51, iniCol])
            {
                r1.Style.Font.Color.SetColor(Color.White);
                r1.Style.Font.Size = sizeFont;
                r1.Style.Font.Bold = true;
                r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r1.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(188, 186, 184));
            }
            var border3 = ws.Cells[xFil + 4, iniCol, xFil + 51, iniCol].Style.Border;
            border3.Bottom.Style = border3.Top.Style = border3.Left.Style = border3.Right.Style = ExcelBorderStyle.Thin;

            int j = 0;

            #region Obtencion de datos

            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion();
            List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);

            foreach (EveRsfdetalleDTO item in listaReserEjec)
            {
                EveRsfhoraDTO hora = horas.Where(x => x.Rsfhorcodi == item.Rsfhorcodi).First();
                item.HorInicio = hora.Rsfhorinicio;
                item.HorFin = hora.Rsfhorfin;
            }

            List<HoraExcel> resultadoAutomatico = new List<HoraExcel>();
            List<HoraExcel> horaExcel = new List<HoraExcel>();


            List<HoraExcel> listHora = GeneraDetalleReserva(resultadoAutomatico, horaExcel, configuracion, listaReserEjec, cabecera, 1);

            #endregion

            #region "Union de Fragmento de Horarios por URS"
            List<EveRsfdetalleDTO> listReserEjec = new List<EveRsfdetalleDTO>();
            List<EveRsfdetalleDTO> list = new List<EveRsfdetalleDTO>();

            foreach (var chang in listaReserEjec) { list.Add(chang); }
            for (int v = 0; v < list.Count; v++)
            {
                decimal vFill = 0;
                var upd = list.Where(x => x.Grupocodi == list[v].Grupocodi && x.HorInicio == list[v].HorInicio).ToList();
                if (upd.Count > 1)
                {
                    foreach (var up in upd) { if (up.vali == 1) { vFill = (decimal)up.Rsfdetvalaut; } }
                }
                else { foreach (var up in upd) { vFill += (up.Rsfdetvalaut == null) ? 0 : (decimal)up.Rsfdetvalaut; } }

                list[v].Rsfdetvalaut = vFill;
                listReserEjec.Add(list[v]);

                foreach (var del in upd) { list.Remove(del); }
                v = -1;
            }

            foreach (var change in listReserEjec) { list.Add(change); }
            listReserEjec = new List<EveRsfdetalleDTO>();

            for (int v = 0; v < list.Count; v++)
            {
                if (v == 0) { listReserEjec.Add(list[v]); }
                else
                {
                    if (list[v].Rsfdetvalaut == list[v - 1].Rsfdetvalaut && list[v].Grupocodi == list[v - 1].Grupocodi)
                    { var upd = listReserEjec.Find(x => x.HorFin == list[v - 1].HorFin); upd.HorFin = list[v].HorFin; }
                    else { listReserEjec.Add(list[v]); }
                }
            }
            #endregion
            /*
            //Inicio * Oliver - 20170912
            resultadoAutomatico = new List<HoraExcel>();
            horaExcel = new List<HoraExcel>();
            listHora = GeneraDetalleReserva(resultadoAutomatico, horaExcel, configuracion, listaReserEjec, cabecera, tipoReserva);
            //Fin * Oliver - 20170912
            */
            foreach (EveRsfdetalleDTO config in cabecera)
            {
                j++;
                ws.Cells[xFil + 2, iniCol + j].Value = config.Ursnomb.Trim();
                ws.Cells[xFil + 3, iniCol + j].Value = config.Gruponomb.Trim();
                ws.Column(iniCol + j).AutoFit();
            }

            int minuto = 0;
            for (int k = 1; k <= 48; k++)
            {
                minuto = minuto + 30;
                j = 0;
                ws.Cells[xFil + 3 + k, iniCol + j].Value = fecha.AddMinutes(minuto).ToString("HH:mm");
                foreach (EveRsfdetalleDTO config in cabecera)
                {
                    j++;
                    string estilo = string.Empty;
                    decimal despachoValor = 0;
                    decimal despachoValorSin = 0;
                    decimal nValor = 0, rsvaProg = 0, rsvaEjec = 0;
                    int minutos = 0;
                    bool valMin = true;

                    if (minuto == 1440) { minuto--; }
                    DateTime fec = DateTime.Parse(fecha.ToShortDateString() + " " + fecha.AddMinutes(minuto).ToString("HH:mm"));

                    ///Reserva Asignada Programada
                    var findProg = listaReserProg.Find(x => x.Grupourspadre == config.Grupocodi);
                    if (findProg != null)
                    {
                        var rsvprog = (decimal?)findProg.GetType().GetProperty("H" + k.ToString()).GetValue(findProg, null);
                        if (rsvprog != null) rsvaProg = (decimal)rsvprog;
                    }

                    //Reserva Asignada Ejecutada
                    List<HoraExcel> findEjec = new List<HoraExcel>();
                    if (fec.Hour == 0 && fec.Minute == 30)
                    {
                        findEjec = resultadoAutomatico.Where(x => x.IdGrupo == config.Grupocodi && x.IdEquipo == config.Equicodi
                        && x.Hora == int.Parse(fec.ToString("HH")) && x.Minuto <= int.Parse(fec.ToString("mm"))).ToList();
                        if (findEjec.Count > 1)
                        {
                            rsvaEjec = (decimal)findEjec[0].Automatico;
                            minutos = (rsvaEjec == 0) ? (30 - findEjec[0].Minuto) : findEjec[0].Minuto;
                            valMin = false;
                        }
                    }
                    else
                    {
                        findEjec = resultadoAutomatico.Where(x => x.IdGrupo == config.Grupocodi && x.IdEquipo == config.Equicodi
                        && x.Hora == int.Parse(fec.ToString("HH")) && x.Minuto == int.Parse(fec.ToString("mm"))).ToList();
                    }

                    if (findEjec.Count > 0 && valMin)
                    {
                        rsvaEjec = (decimal)findEjec[0].Automatico;
                        minutos = (findEjec[0].restoMin == 0) ? 30 : findEjec[0].restoMin;
                    }

                    if (rsvaProg == 0 && rsvaEjec == 0)
                    {
                        // Copiar Despacho
                        ws.Cells[xFil + 3 + k, iniCol + j].Value = despachoValor;
                        if (despachoValor != 0)
                        {
                            using (var range = ws.Cells[xFil + 3 + k, iniCol + j])
                            {
                                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(241, 205, 155));
                            }
                        }
                    }
                    else
                    {
                        /// Se busca Despacho
                        MeMedicion48DTO entity = listaDespacho.Where(x => x.Grupourspadre == config.Grupocodi).FirstOrDefault();
                        if (entity != null)
                        {
                            var despValor = (decimal?)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                            if (despValor != null) despachoValor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                        }

                        /// Se busca DespachiSin
                        entity = listaDespachoSin.Where(x => x.Grupourspadre == config.Grupocodi).FirstOrDefault();
                        if (entity != null)
                        {
                            var despValorSin = (decimal?)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                            if (despValorSin != null) despachoValorSin = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                        }

                        nValor = despachoValorSin;
                        if (tipoReserva == 1)
                        {
                            nValor = despachoValor;
                            if (rsvaProg == 0)
                            {
                                var result = CalcularCeldaDespacho(fecha, k, (int)config.Grupocodi, minutos, despachoValor, rsvaEjec, despachoValorSin);
                                nValor = result;
                            }
                            else
                            {
                                if (minutos > 0 && minutos < 30) { nValor = ((decimal)minutos / 30) * nValor; }
                                if (findEjec.Count > 0) { if (findEjec[0].restoMin == 0 && findEjec[0].Automatico == 0 && valMin) { nValor = 0; } }
                            }

                            ws.Cells[xFil + 3 + k, iniCol + j].Value = nValor;
                            if (nValor != 0)
                            {
                                using (var range = ws.Cells[xFil + 3 + k, iniCol + j])
                                {
                                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(241, 205, 155));
                                    range.Style.Font.Bold = true;
                                }
                            }
                        }
                        else
                        {
                            if (rsvaProg == 0 && rsvaEjec > 0) { nValor = despachoValor; }
                            if (rsvaProg > 0 && rsvaEjec > 0) { nValor = despachoValorSin; }
                            //if (rsvaProg > 0 && rsvaEjec == 0) { nValor = 0; }

                            if (findEjec.Count > 0)
                            {
                                if (findEjec[0].restoMin != 0 || minutos < 30) { nValor = ((decimal)minutos / 30) * nValor; }
                                if (findEjec[0].restoMin == 0 && findEjec[0].Automatico == 0 && valMin) { nValor = 0; }
                            }
                            //if (rsvaEjec == 0) { nValor = 0; }

                            ws.Cells[xFil + 3 + k, iniCol + j].Value = nValor;
                            if (nValor != 0)
                            {
                                using (var range = ws.Cells[xFil + 3 + k, iniCol + j])
                                {
                                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    range.Style.Font.Bold = true;
                                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(241, 205, 155));
                                }
                            }
                        }
                    }
                }
                using (var range = ws.Cells[xFil + 4, iniCol + 1, xFil + 51, iniCol + ancho])
                {
                    range.Style.Numberformat.Format = @"0.0";
                    range.Style.Font.Size = sizeFont;
                }
                var border4 = ws.Cells[xFil + 51, iniCol + 1, xFil + 51, iniCol + ancho].Style.Border;
                border4.Bottom.Style = ExcelBorderStyle.Thin;
                var border5 = ws.Cells[xFil + 4, iniCol + ancho, xFil + 51, iniCol + ancho].Style.Border;
                border5.Right.Style = ExcelBorderStyle.Thin;
            }
        }
        #endregion

        #region Métodos Tabla CO_BANDANCP
        /// <summary>
        /// Inserta un registro de la tabla CO_BANDANCP
        /// </summary>
        public void SaveCoBandancp(CoBandancpDTO entity)
        {
            try
            {
                FactorySic.GetCoBandancpRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_BANDANCP
        /// </summary>
        public void UpdateCoBandancp(CoBandancpDTO entity)
        {
            try
            {
                FactorySic.GetCoBandancpRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_BANDANCP
        /// </summary>
        public void DeleteCoBandancp(int bandcodi)
        {
            try
            {
                FactorySic.GetCoBandancpRepository().Delete(bandcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_BANDANCP
        /// </summary>
        public CoBandancpDTO GetByIdCoBandancp(int bandcodi)
        {
            return FactorySic.GetCoBandancpRepository().GetById(bandcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_BANDANCP
        /// </summary>
        public List<CoBandancpDTO> ListCoBandancps()
        {
            return FactorySic.GetCoBandancpRepository().List();
        }

        ///// <summary>
        ///// Permite realizar búsquedas en la tabla CoBandancp
        ///// </summary>
        //public List<CoBandancpDTO> GetByCriteriaCoBandancps()
        //{
        //    return FactorySic.GetCoBandancpRepository().GetByCriteria();
        //}

        /// <summary>
        /// Graba lista de objetos 
        /// </summary>
        /// <param name="listaBandaNCP"></param>
        public void GrabarValoresCargadosBAndaNCP(List<CoBandancpDTO> listaBandaNCP)
        {

            foreach (var obj in listaBandaNCP)
            {
                SaveCoBandancp(obj);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CoBandancpDTO> GetListaCoBandaNCP(DateTime fecha)
        {
            return FactorySic.GetCoBandancpRepository().ListBandaNCPxFecha(fecha);
        }

        #endregion

        #region Tabla PR_GRUPO

        /// <summary>
        /// Permite entregar un registro de la tabla CO_BANDANCP
        /// </summary>
        public PrGrupoDTO GetPrGrupo(int grupocodincp)
        {
            return FactorySic.GetPrGrupoRepository().GetByIdNCP(grupocodincp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> GetListaModosOpNCP()
        {
            return FactorySic.GetPrGrupoRepository().GetByListaModosOperacionNCP();
        }
        public List<PrGrupoDTO> ListaCompensacionGrupo(int pecacodi)
        {
            return FactorySic.GetPrGrupoRepository().ListaCompensacionGrupo(pecacodi);
        }

        /// <summary>
        /// Oliver - 20170814
        /// </summary>
        /// <param name="dfecha"></param>
        /// <param name="porcentajerpf"></param>
        /// <param name="origlectcodi"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> GetListaPotenciaEfectiva(DateTime dfecha, int porcentajerpf, int origlectcodi)
        {
            return FactorySic.GetPrGrupoRepository().GetListaPotenciaEfectiva(dfecha, porcentajerpf, origlectcodi);
        }
        #endregion

        #region Metodos de la tabla EveMails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<EveMailsDTO> GetListaReprogramado(DateTime fecha)
        {
            return FactorySic.GetEveMailsRepository().GetListaReprogramado(fecha);
        }
        #endregion

        /// <summary>
        /// Genera Formato Decimal
        /// </summary>
        /// <returns></returns>
        public NumberFormatInfo GenerarNumberFormatInfo()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaNombreArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GenerarArchivoExcelDespacho(DateTime fecha, string rutaNombreArchivo)
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
                string sheetName = "RESUMEN";
                ws = xlPackage.Workbook.Worksheets.Add(sheetName);
                ws = xlPackage.Workbook.Worksheets[sheetName];
                ws.Cells[1, 2].Value = "FECHA:" + fecha.ToString(ConstantesBase.FormatoFecha);
                var listaReservEjec = GetReservaEjec(fecha);
                var listaDespachoSin = GetReservaProgramado(fecha, ConstantesCostoOportunidad.LectcodiDespachoSinReserva); // Sin Reserva
                var listaDespacho = GetReservaProgramado(fecha, ConstantesCostoOportunidad.LectcodiDespachoConReserva); // Con Reserva
                var listaReservProg = GetReservaProgramado(fecha, ConstantesCostoOportunidad.LectcodiReservaProgramada);
                //List<MeMedicion48DTO> listaReservaEjec = new List<MeMedicion48DTO>();
                //var listaCruceReserva = GetListaCruce(listaReservEjec, listaReservProg, listaReservaEjec, fecha);
                GeneraTablaExcelDespacho(ws, listaReservEjec, listaDespacho, listaDespachoSin, listaReservProg/*, listaCruceReserva*/, fecha, 0); // Sin Reserva
                GeneraTablaExcelDespacho(ws, listaReservEjec, listaDespacho, listaDespachoSin, listaReservProg/*, listaCruceReserva*/, fecha, 1); // Con Reserva
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite analizar si un dato es numérico
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        private Boolean AnalizarNumerico(string valor)
        {
            Boolean bresult = false;
            decimal number3;
            bresult = decimal.TryParse(valor, out number3);

            return bresult;
        }


        #region Mejoras Costos de Oportunidad - Movisoft

        #region Métodos Tabla CO_PERIODO

        /// <summary>
        /// Crea estructura de la tabla medicion60
        /// </summary>
        /// <param name="nombre"></param>
        public void CrearTablaMedicion60(string nombre)
        {
            try
            {
                //- Validar existencia de la tabla
                int validarExistencia = FactorySic.GetCoMedicion60Repository().VerificarExistenciaTabla(nombre);

                if (validarExistencia == 0)
                {
                    for (int i = 0; i <= 8; i++)
                    {
                        FactorySic.GetCoMedicion60Repository().CrearTabla(i, nombre);
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
        /// Crea estructura de la tabla RpfMedicion60
        /// </summary>
        /// <param name="nombre"></param>
        public void CrearTablaRpfMedicion60(string nombre)
        {
            try
            {
                //- Validar existencia de la tabla
                int validarExistencia = FactorySic.GetRpfMedicion60Repository().VerificarExistenciaTabla(nombre);

                if (validarExistencia == 0)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        FactorySic.GetRpfMedicion60Repository().CrearTabla(i, nombre);
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
        /// Inserta un registro de la tabla CO_PERIODO
        /// </summary>
        public void SaveCoPeriodo(CoPeriodoDTO entity)
        {
            try
            {
                if (entity.Copercodi == 0)
                {
                    //busca si existe periodo (incluye eliminados)
                    var existePeriodo = ListCoPeriodos().Where(x => x.Coperanio == entity.Coperanio && x.Copermes == entity.Copermes).ToList();

                    FactorySic.GetCoPeriodoRepository().Save(entity);

                    //CREAR LA TABLA CO_MEDICION60_MM_YYYY PARA EL PERIODO RESPECTIVO
                    //if (!existePeriodo.Any())
                    {
                        var nombrePeriodo = entity.Coperanio.ToString() + entity.Copermes.Value.ToString("00");
                        this.CrearTablaMedicion60(nombrePeriodo);
                        this.CrearTablaRpfMedicion60(nombrePeriodo);
                    }
                }
                else
                {
                    FactorySic.GetCoPeriodoRepository().Update(entity);

                    var nombrePeriodo = entity.Coperanio.ToString() + entity.Copermes.Value.ToString("00");
                    this.CrearTablaMedicion60(nombrePeriodo);
                    this.CrearTablaRpfMedicion60(nombrePeriodo);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite validar la existencia de un periodo
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public bool ValidarExistenciaPeriodo(int anio, int mes)
        {
            return FactorySic.GetCoPeriodoRepository().ValidarExistencia(anio, mes);
        }

        /// <summary>
        /// Obtiene el periodo mensual de cierta fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public CoPeriodoDTO ObtenerPeriodoMensual(string fecha)
        {
            return FactorySic.GetCoPeriodoRepository().GetMensualByFecha(fecha);
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_PERIODO
        /// </summary>
        public void UpdateCoPeriodo(CoPeriodoDTO entity)
        {
            try
            {
                FactorySic.GetCoPeriodoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_PERIODO
        /// </summary>
        public void DeleteCoPeriodo(int copercodi)
        {
            try
            {
                FactorySic.GetCoPeriodoRepository().Delete(copercodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_PERIODO
        /// </summary>
        public CoPeriodoDTO GetByIdCoPeriodo(int copercodi)
        {
            return FactorySic.GetCoPeriodoRepository().GetById(copercodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_PERIODO
        /// </summary>
        public List<CoPeriodoDTO> ListCoPeriodos()
        {
            return FactorySic.GetCoPeriodoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoPeriodo
        /// </summary>
        public List<CoPeriodoDTO> GetByCriteriaCoPeriodos(int anio)
        {
            return FactorySic.GetCoPeriodoRepository().GetByCriteria(anio);
        }

        /// <summary>
        /// Lista los años
        /// </summary>
        /// <returns></returns>
        public List<int> ListarAnios()
        {
            List<int> result = new List<int>();
            for (int i = DateTime.Now.Year + 10; i >= DateTime.Now.Year - 5; i--)
            {
                result.Add(i);
            }
            return result;
        }

        /// <summary>
        /// Lista los meses del año
        /// </summary>
        /// <returns></returns>
        public List<CoPeriodoDTO> ListarMeses()
        {
            List<CoPeriodoDTO> entitys = new List<CoPeriodoDTO>();
            for (int i = 1; i <= 12; i++)
            {
                CoPeriodoDTO entity = new CoPeriodoDTO();

                entity.Copermes = i;
                entity.Descmes = COES.Base.Tools.Util.ObtenerNombreMes(i);

                entitys.Add(entity);
            }

            return entitys;
        }

        #endregion

        #region Métodos Tabla CO_PROCESOCALCULO

        /// <summary>
        /// Inserta un registro de la tabla CO_PROCESOCALCULO
        /// </summary>
        public void SaveCoProcesocalculo(CoProcesocalculoDTO entity)
        {
            try
            {
                FactorySic.GetCoProcesocalculoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_PROCESOCALCULO
        /// </summary>
        public void UpdateCoProcesocalculo(CoProcesocalculoDTO entity)
        {
            try
            {
                FactorySic.GetCoProcesocalculoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_PROCESOCALCULO
        /// </summary>
        public void DeleteCoProcesocalculo(int coprcacodi)
        {
            try
            {
                FactorySic.GetCoProcesocalculoRepository().Delete(coprcacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_PROCESOCALCULO
        /// </summary>
        public CoProcesocalculoDTO GetByIdCoProcesocalculo(int coprcacodi)
        {
            return FactorySic.GetCoProcesocalculoRepository().GetById(coprcacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_PROCESOCALCULO
        /// </summary>
        public List<CoProcesocalculoDTO> ListCoProcesocalculos()
        {
            return FactorySic.GetCoProcesocalculoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoProcesocalculo
        /// </summary>
        public List<CoProcesocalculoDTO> GetByCriteriaCoProcesocalculos()
        {
            return FactorySic.GetCoProcesocalculoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CO_RAEJECUTADA

        /// <summary>
        /// Inserta un registro de la tabla CO_RAEJECUTADA
        /// </summary>
        public void SaveCoRaejecutada(CoRaejecutadaDTO entity)
        {
            try
            {
                FactorySic.GetCoRaejecutadaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_RAEJECUTADA
        /// </summary>
        public void UpdateCoRaejecutada(CoRaejecutadaDTO entity)
        {
            try
            {
                FactorySic.GetCoRaejecutadaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_RAEJECUTADA
        /// </summary>
        public void DeleteCoRaejecutada(int coraejcodi)
        {
            try
            {
                FactorySic.GetCoRaejecutadaRepository().Delete(coraejcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_RAEJECUTADA
        /// </summary>
        public CoRaejecutadaDTO GetByIdCoRaejecutada(int coraejcodi)
        {
            return FactorySic.GetCoRaejecutadaRepository().GetById(coraejcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_RAEJECUTADA
        /// </summary>
        public List<CoRaejecutadaDTO> ListCoRaejecutadas()
        {
            return FactorySic.GetCoRaejecutadaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoRaejecutada
        /// </summary>
        public List<CoRaejecutadaDTO> GetByCriteriaCoRaejecutadas()
        {
            return FactorySic.GetCoRaejecutadaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CO_TIPOINFORMACION

        /// <summary>
        /// Inserta un registro de la tabla CO_TIPOINFORMACION
        /// </summary>
        public void SaveCoTipoinformacion(CoTipoinformacionDTO entity)
        {
            try
            {
                FactorySic.GetCoTipoinformacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_TIPOINFORMACION
        /// </summary>
        public void UpdateCoTipoinformacion(CoTipoinformacionDTO entity)
        {
            try
            {
                FactorySic.GetCoTipoinformacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_TIPOINFORMACION
        /// </summary>
        public void DeleteCoTipoinformacion(int cotinfcodi)
        {
            try
            {
                FactorySic.GetCoTipoinformacionRepository().Delete(cotinfcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_TIPOINFORMACION
        /// </summary>
        public CoTipoinformacionDTO GetByIdCoTipoinformacion(int cotinfcodi)
        {
            return FactorySic.GetCoTipoinformacionRepository().GetById(cotinfcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_TIPOINFORMACION
        /// </summary>
        public List<CoTipoinformacionDTO> ListCoTipoinformacions()
        {
            return FactorySic.GetCoTipoinformacionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoTipoinformacion
        /// </summary>
        public List<CoTipoinformacionDTO> GetByCriteriaCoTipoinformacions()
        {
            return FactorySic.GetCoTipoinformacionRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CO_URS_ESPECIAL

        /// <summary>
        /// Inserta un registro de la tabla CO_URS_ESPECIAL
        /// </summary>
        public void SaveCoUrsEspecial(CoUrsEspecialDTO entity)
        {
            try
            {
                FactorySic.GetCoUrsEspecialRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_URS_ESPECIAL
        /// </summary>
        public void UpdateCoUrsEspecial(CoUrsEspecialDTO entity)
        {
            try
            {
                FactorySic.GetCoUrsEspecialRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_URS_ESPECIAL
        /// </summary>
        public void DeleteCoUrsEspecial(int courescodi)
        {
            try
            {
                FactorySic.GetCoUrsEspecialRepository().Delete(courescodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_URS_ESPECIAL
        /// </summary>
        public CoUrsEspecialDTO GetByIdCoUrsEspecial(int courescodi)
        {
            return FactorySic.GetCoUrsEspecialRepository().GetById(courescodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_URS_ESPECIAL
        /// </summary>
        public List<CoUrsEspecialDTO> ListCoUrsEspecials()
        {
            return FactorySic.GetCoUrsEspecialRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoUrsEspecial
        /// </summary>
        public List<CoUrsEspecialDTO> GetByCriteriaCoUrsEspecials(int idVersion)
        {
            List<CoUrsEspecialDTO> entitys = new List<CoUrsEspecialDTO>();

            if (idVersion == 0)
            {
                List<CoUrsEspecialbaseDTO> listUrsBase = this.ListCoUrsEspecialbases();
                entitys = listUrsBase.Select(x => new CoUrsEspecialDTO { Grupocodi = x.Grupocodi, Gruponomb = x.Gruponomb }).ToList();
            }
            else
            {
                entitys = FactorySic.GetCoUrsEspecialRepository().GetByCriteria(idVersion);
            }

            List<CoUrsEspecialDTO> configuracion = this.ObtenerListadoURS();

            foreach (CoUrsEspecialDTO item in entitys)
            {
                CoUrsEspecialDTO conf = configuracion.Where(x => x.Grupocodi == item.Grupocodi).FirstOrDefault();

                if (conf != null)
                {
                    item.Gruponomb = conf.Gruponomb;
                    item.Centralnomb = conf.Centralnomb;
                }
            }

            return entitys;
        }

        #endregion

        #region Métodos Tabla CO_URS_ESPECIALBASE

        /// <summary>
        /// Inserta un registro de la tabla CO_URS_ESPECIALBASE
        /// </summary>
        public void SaveCoUrsEspecialbase(CoUrsEspecialbaseDTO entity)
        {
            try
            {
                FactorySic.GetCoUrsEspecialbaseRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_URS_ESPECIALBASE
        /// </summary>
        public void UpdateCoUrsEspecialbase(CoUrsEspecialbaseDTO entity)
        {
            try
            {
                FactorySic.GetCoUrsEspecialbaseRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_URS_ESPECIALBASE
        /// </summary>
        public void DeleteCoUrsEspecialbase(int couebacodi)
        {
            try
            {
                FactorySic.GetCoUrsEspecialbaseRepository().Delete(couebacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_URS_ESPECIALBASE
        /// </summary>
        public CoUrsEspecialbaseDTO GetByIdCoUrsEspecialbase(int couebacodi)
        {
            return FactorySic.GetCoUrsEspecialbaseRepository().GetById(couebacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_URS_ESPECIALBASE
        /// </summary>
        public List<CoUrsEspecialbaseDTO> ListCoUrsEspecialbases()
        {
            return FactorySic.GetCoUrsEspecialbaseRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoUrsEspecialbase
        /// </summary>
        public List<CoUrsEspecialbaseDTO> GetByCriteriaCoUrsEspecialbases()
        {
            return FactorySic.GetCoUrsEspecialbaseRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CO_VERSIONBASE

        /// <summary>
        /// Inserta un registro de la tabla CO_VERSIONBASE
        /// </summary>
        public void SaveCoVersionbase(CoVersionbaseDTO entity)
        {
            try
            {
                FactorySic.GetCoVersionbaseRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_VERSIONBASE
        /// </summary>
        public void UpdateCoVersionbase(CoVersionbaseDTO entity)
        {
            try
            {
                FactorySic.GetCoVersionbaseRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_VERSIONBASE
        /// </summary>
        public void DeleteCoVersionbase(int covebacodi)
        {
            try
            {
                FactorySic.GetCoVersionbaseRepository().Delete(covebacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_VERSIONBASE
        /// </summary>
        public CoVersionbaseDTO GetByIdCoVersionbase(int covebacodi)
        {
            return FactorySic.GetCoVersionbaseRepository().GetById(covebacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_VERSIONBASE
        /// </summary>
        public List<CoVersionbaseDTO> ListCoVersionbases()
        {
            return FactorySic.GetCoVersionbaseRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoVersionbase
        /// </summary>
        public List<CoVersionbaseDTO> GetByCriteriaCoVersionbases()
        {
            return FactorySic.GetCoVersionbaseRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener el listado de URS
        /// </summary>
        /// <returns></returns>
        public List<CoUrsEspecialDTO> ObtenerListadoURS()
        {
            List<EveRsfdetalleDTO> configuracion = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracion(DateTime.Now);
            List<CoUrsEspecialDTO> entitys = configuracion.Select(x => new CoUrsEspecialDTO
            { Centralnomb = x.Gruponomb, Gruponomb = x.Ursnomb, Grupocodi = x.Grupocodi }).ToList();

            return entitys;
        }

        /// <summary>
        /// Permite obtener los datos de una URS
        /// </summary>
        /// <param name="grupoCodi"></param>
        /// <returns></returns>
        public CoUrsEspecialDTO ObtenerURS(int grupoCodi)
        {
            return this.ObtenerListadoURS().Where(x => x.Grupocodi == grupoCodi).FirstOrDefault();
        }


        #endregion

        #region Métodos Tabla CO_VERSION

        /// <summary>
        /// Inserta un registro de la tabla CO_VERSION
        /// </summary>
        public void SaveCoVersion(CoVersionDTO entity)
        {
            try
            {
                int idVersion = 0;

                if (entity.Covercodi == 0)
                {
                    //busca si existe versión (incluye eliminados)
                    var existeVersion = ListCoVersions().Where(x => x.Copercodi == entity.Copercodi && x.Covebacodi == entity.Covebacodi).ToList();

                    idVersion = FactorySic.GetCoVersionRepository().Save(entity);

                    //CREAR LA TABLA CO_MEDICION60_MMYYYY_VERSION PARA LA VERSIÓN DEL PERIODO RESPECTIVO
                    //if (!existeVersion.Any())
                    {
                        var periodo = FactorySic.GetCoPeriodoRepository().GetById(entity.Copercodi.Value);//Obtiene el periodo
                        var nombrePeriodo = periodo.Coperanio.ToString() + periodo.Copermes.Value.ToString("00") + "_" + entity.Covebacodi.ToString();
                        this.CrearTablaMedicion60(nombrePeriodo);
                    }
                }
                else
                {
                    FactorySic.GetCoVersionRepository().Update(entity);
                    idVersion = entity.Covercodi;
                    FactorySic.GetCoUrsEspecialRepository().Delete(entity.Covercodi);

                    //CREAR LA TABLA CO_MEDICION60_MMYYYY_VERSION PARA LA VERSIÓN DEL PERIODO RESPECTIVO
                    //if (!existeVersion.Any())
                    {
                        var periodo = FactorySic.GetCoPeriodoRepository().GetById(entity.Copercodi.Value);//Obtiene el periodo
                        var nombrePeriodo = periodo.Coperanio.ToString() + periodo.Copermes.Value.ToString("00") + "_" + entity.Covebacodi.ToString();
                        this.CrearTablaMedicion60(nombrePeriodo);
                    }
                }

                List<int> idsURS = entity.ListaURS.Split(ConstantesAppServicio.CaracterComa).Select(int.Parse).ToList();

                foreach (int id in idsURS)
                {
                    CoUrsEspecialDTO detalle = new CoUrsEspecialDTO();
                    detalle.Covercodi = idVersion;
                    detalle.Copercodi = entity.Copercodi;
                    detalle.Grupocodi = id;
                    detalle.Couresusucreacion = entity.Coverusumodificacion;
                    detalle.Couresfeccreacion = DateTime.Now;

                    FactorySic.GetCoUrsEspecialRepository().Save(detalle);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_VERSION
        /// </summary>
        public void UpdateCoVersion(CoVersionDTO entity)
        {
            try
            {
                FactorySic.GetCoVersionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene todas las versiones del mes
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<CoVersionDTO> ObtenerVersionesPorMes(int mes, int anio)
        {
            return FactorySic.GetCoVersionRepository().GetVersionesPorMes(mes, anio);
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_VERSION
        /// </summary>
        public void DeleteCoVersion(int covercodi)
        {
            try
            {
                FactorySic.GetCoVersionRepository().Delete(covercodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_VERSION
        /// </summary>
        public CoVersionDTO GetByIdCoVersion(int covercodi)
        {
            return FactorySic.GetCoVersionRepository().GetById(covercodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_VERSION
        /// </summary>
        public List<CoVersionDTO> ListCoVersions()
        {
            return FactorySic.GetCoVersionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoVersion
        /// </summary>
        public List<CoVersionDTO> GetByCriteriaCoVersions(int idPeriodo)
        {
            return FactorySic.GetCoVersionRepository().GetByCriteria(idPeriodo);
        }

        /// <summary>
        /// Permite obtener los datos de la versión base
        /// </summary>
        /// <param name="idVersionBase"></param>
        /// <returns></returns>
        public CoVersionDTO ObtenerDatosVersionBase(int idPeriodo, int idVersionBase)
        {
            CoVersionDTO entity = new CoVersionDTO();
            CoPeriodoDTO periodo = FactorySic.GetCoPeriodoRepository().GetById(idPeriodo);
            CoVersionbaseDTO config = FactorySic.GetCoVersionbaseRepository().GetById(idVersionBase);

            int fin = 0;
            int diaFin = (int)config.Covebadiafin;

            if (diaFin < 28)
            {
                fin = diaFin;
            }
            else
            {
                fin = DateTime.DaysInMonth((int)periodo.Coperanio, (int)periodo.Copermes);
            }

            entity.Coverdesc = config.Covebadesc;
            entity.Coverfecinicio = new DateTime((int)periodo.Coperanio, (int)periodo.Copermes, (int)config.Covebadiainicio);
            entity.Coverfecfin = new DateTime((int)periodo.Coperanio, (int)periodo.Copermes, fin);
            entity.FechaInicio = ((DateTime)entity.Coverfecinicio).ToString(ConstantesAppServicio.FormatoFecha);
            entity.FechaFin = ((DateTime)entity.Coverfecfin).ToString(ConstantesAppServicio.FormatoFecha);


            return entity;
        }

        #endregion

        #region Métodos Tabla CO_ENVIOLIQUIDACION

        /// <summary>
        /// Inserta un registro de la tabla CO_ENVIOLIQUIDACION
        /// </summary>
        public void SaveCoEnvioliquidacion(CoEnvioliquidacionDTO entity)
        {
            try
            {
                FactorySic.GetCoEnvioliquidacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_ENVIOLIQUIDACION
        /// </summary>
        public void UpdateCoEnvioliquidacion(CoEnvioliquidacionDTO entity)
        {
            try
            {
                FactorySic.GetCoEnvioliquidacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_ENVIOLIQUIDACION
        /// </summary>
        public void DeleteCoEnvioliquidacion(int coenlicodi)
        {
            try
            {
                FactorySic.GetCoEnvioliquidacionRepository().Delete(coenlicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_ENVIOLIQUIDACION
        /// </summary>
        public CoEnvioliquidacionDTO GetByIdCoEnvioliquidacion(int coenlicodi)
        {
            return FactorySic.GetCoEnvioliquidacionRepository().GetById(coenlicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_ENVIOLIQUIDACION
        /// </summary>
        public List<CoEnvioliquidacionDTO> ListCoEnvioliquidacions()
        {
            return FactorySic.GetCoEnvioliquidacionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoEnvioliquidacion
        /// </summary>
        public List<CoEnvioliquidacionDTO> GetByCriteriaCoEnvioliquidacions(int idVersion)
        {
            return FactorySic.GetCoEnvioliquidacionRepository().GetByCriteria(idVersion);
        }

        /// <summary>
        /// Permite obtener el listado de envíos por periodos
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public List<CoEnvioliquidacionDTO> ObtenerEnviosPorPeriodo(int idPeriodo)
        {
            return FactorySic.GetCoEnvioliquidacionRepository().ObtenerEnviosPorPeriodo(idPeriodo);
        }


        #endregion

        #region Métodos Tabla CO_MEDICION48

        /// <summary>
        /// Inserta un registro de la tabla CO_MEDICION48
        /// </summary>
        public void SaveCoMedicion48(CoMedicion48DTO entity)
        {
            try
            {
                FactorySic.GetCoMedicion48Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_MEDICION48
        /// </summary>
        public void UpdateCoMedicion48(CoMedicion48DTO entity)
        {
            try
            {
                FactorySic.GetCoMedicion48Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_MEDICION48
        /// </summary>
        public void DeleteCoMedicion48(int comedcodi)
        {
            try
            {
                FactorySic.GetCoMedicion48Repository().Delete(comedcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_MEDICION48
        /// </summary>
        public CoMedicion48DTO GetByIdCoMedicion48(int comedcodi)
        {
            return FactorySic.GetCoMedicion48Repository().GetById(comedcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_MEDICION48
        /// </summary>
        public List<CoMedicion48DTO> ListCoMedicion48s()
        {
            return FactorySic.GetCoMedicion48Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoMedicion48
        /// </summary>
        public List<CoMedicion48DTO> GetByCriteriaCoMedicion48s()
        {
            return FactorySic.GetCoMedicion48Repository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener los datos insumos para el proceso de calculo
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="datosPrograma"></param>
        /// <param name="datosRAUp"></param>
        /// <param name="datosRADown"></param>
        /// <param name="datosRAEjecutadoUp"></param>
        /// <param name="datosRAEjecutadoDown"></param>
        /// <param name="datosProgramaSinReserva"></param>
        public void ObtenerInsumosProceso(int idVersion, out string[][] datosPrograma, out string[][] datosRAUp,
            out string[][] datosRADown, out string[][] datosRAEjecutadoUp, out string[][] datosRAEjecutadoDown, out string[][] datosProgramaSinReserva)
        {
            try
            {
                List<CpMedicion48DTO> resultPrograma = new List<CpMedicion48DTO>();
                List<CpMedicion48DTO> resultProgramaSinReserva = new List<CpMedicion48DTO>();
                List<CpMedicion48DTO> resultRAUp = new List<CpMedicion48DTO>();
                List<CpMedicion48DTO> resultRADown = new List<CpMedicion48DTO>();
                List<CoMedicion48DTO> resultRAEject = new List<CoMedicion48DTO>();

                CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);
                List<CoRaejecutadadetDTO> listRaDetalle = new List<CoRaejecutadadetDTO>();

                this.ConsultarDatosProceso(idVersion, out resultPrograma, out resultProgramaSinReserva, out resultRAUp, out resultRADown,
                    out resultRAEject, (DateTime)version.Coverfecinicio, (DateTime)version.Coverfecfin, out listRaDetalle);

                datosPrograma = this.MostrarInformacionInsumo(resultPrograma);
                datosRAUp = this.MostrarInformacionInsumoRA(resultRAUp);
                datosRADown = this.MostrarInformacionInsumoRA(resultRADown);
                datosProgramaSinReserva = this.MostrarInformacionInsumo(resultProgramaSinReserva);
                this.ObtenerDatosReservaEjecutada(resultRAEject, out datosRAEjecutadoUp, out datosRAEjecutadoDown);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        ///  Permite almacenar los datos para el proceso
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void CopiarDatos(int idVersion, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                List<CpMedicion48DTO> resultPrograma = new List<CpMedicion48DTO>();
                List<CpMedicion48DTO> resultProgramaSinReserva = new List<CpMedicion48DTO>();
                List<CpMedicion48DTO> resultRAUp = new List<CpMedicion48DTO>();
                List<CpMedicion48DTO> resultRADown = new List<CpMedicion48DTO>();
                List<CoMedicion48DTO> resultRAEject = new List<CoMedicion48DTO>();
                List<CoRaejecutadadetDTO> listRaDetalle = new List<CoRaejecutadadetDTO>();
                this.ConsultarDatosProceso(idVersion, out resultPrograma, out resultProgramaSinReserva, out resultRAUp,
                    out resultRADown, out resultRAEject, fechaInicio, fechaFin, out listRaDetalle);

                CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);

                // 1: Programa, 2: Programa Sin Reserva, 3: RAUp programa, 4: RADown programa, 5: RAUp ejecutado, 6: RADown ejecutado

                //- Obteniendo la estructura de programa y reserva asignada programada
                List<CoMedicion48DTO> listPrograma = this.TransformarData(resultPrograma, version, 1, 1);
                List<CoMedicion48DTO> listProgramaSinReserva = this.TransformarData(resultProgramaSinReserva, version, 2, 1);
                List<CoMedicion48DTO> listRaUp = this.TransformarData(resultRAUp, version, 3, 0);
                List<CoMedicion48DTO> listRaDown = this.TransformarData(resultRADown, version, 4, 0);

                //- Obteniendo la estructura de RA ejecutada
                List<CoMedicion48DTO> listSubEjec = new List<CoMedicion48DTO>();
                List<CoMedicion48DTO> listBajEjec = new List<CoMedicion48DTO>();

                this.TransformarDataRA(resultRAEject, version, out listSubEjec, out listBajEjec);

                //- Almacenando los datos
                FactorySic.GetCoMedicion48Repository().GrabarDatosBulk(listPrograma, (int)version.Copercodi, version.Covercodi, 1, fechaInicio, fechaFin);
                FactorySic.GetCoMedicion48Repository().GrabarDatosBulk(listProgramaSinReserva, (int)version.Copercodi, version.Covercodi, 2, fechaInicio, fechaFin);
                FactorySic.GetCoMedicion48Repository().GrabarDatosBulk(listRaUp, (int)version.Copercodi, version.Covercodi, 3, fechaInicio, fechaFin);
                FactorySic.GetCoMedicion48Repository().GrabarDatosBulk(listRaDown, (int)version.Copercodi, version.Covercodi, 4, fechaInicio, fechaFin);
                FactorySic.GetCoMedicion48Repository().GrabarDatosBulkResult(listSubEjec, (int)version.Copercodi, version.Covercodi, 5, fechaInicio, fechaFin);
                FactorySic.GetCoMedicion48Repository().GrabarDatosBulkResult(listBajEjec, (int)version.Copercodi, version.Covercodi, 6, fechaInicio, fechaFin);

                //- Almacenando los datos de ra detalle

                FactorySic.GetCoRaejecutadadetRepository().Delete((int)version.Copercodi, version.Covercodi);

                foreach (CoRaejecutadadetDTO itemDetalle in listRaDetalle)
                {
                    itemDetalle.Copercodi = version.Copercodi;
                    itemDetalle.Covercodi = version.Covercodi;
                    FactorySic.GetCoRaejecutadadetRepository().Save(itemDetalle);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener la estructura para la inserción en bulk
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="version"></param>
        /// <param name="tipoInfo"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        public List<CoMedicion48DTO> TransformarData(List<CpMedicion48DTO> entitys, CoVersionDTO version, int tipoInfo, int indicador)
        {
            return entitys.Select(x => new CoMedicion48DTO
            {
                Comedfecha = x.Medifecha,
                Copercodi = version.Copercodi,
                Covercodi = version.Covercodi,
                Cotinfcodi = tipoInfo,
                Comedtipo = (indicador == 1) ? x.Famcodi : 2,
                Grupocodi = (indicador == 1) ? (x.Famcodi == 2) ? x.Recurcodisicoes : -1 : x.Recurcodisicoes,
                Equicodi = (indicador == 1) ? (x.Famcodi == 1) ? x.Recurcodisicoes : -1 : -1,
                H1 = x.H1,
                H2 = x.H2,
                H3 = x.H3,
                H4 = x.H4,
                H5 = x.H5,
                H6 = x.H6,
                H7 = x.H7,
                H8 = x.H8,
                H9 = x.H9,
                H10 = x.H10,
                H11 = x.H11,
                H12 = x.H12,
                H13 = x.H13,
                H14 = x.H14,
                H15 = x.H15,
                H16 = x.H16,
                H17 = x.H17,
                H18 = x.H18,
                H19 = x.H19,
                H20 = x.H20,
                H21 = x.H21,
                H22 = x.H22,
                H23 = x.H23,
                H24 = x.H24,
                H25 = x.H25,
                H26 = x.H26,
                H27 = x.H27,
                H28 = x.H28,
                H29 = x.H29,
                H30 = x.H30,
                H31 = x.H31,
                H32 = x.H32,
                H33 = x.H33,
                H34 = x.H34,
                H35 = x.H35,
                H36 = x.H36,
                H37 = x.H37,
                H38 = x.H38,
                H39 = x.H39,
                H40 = x.H40,
                H41 = x.H41,
                H42 = x.H42,
                H43 = x.H43,
                H44 = x.H44,
                H45 = x.H45,
                H46 = x.H46,
                H47 = x.H47,
                H48 = x.H48
            }).ToList();
        }

        /// <summary>
        /// Permite obtener la estructura para la inserción en bulk de la RA ejecutada
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="version"></param>
        /// <param name="listSub"></param>
        /// <param name="listBaj"></param>
        public void TransformarDataRA(List<CoMedicion48DTO> entitys, CoVersionDTO version, out List<CoMedicion48DTO> listSub, out List<CoMedicion48DTO> listBaj)
        {
            List<CoMedicion48DTO> resultSub = new List<CoMedicion48DTO>();
            List<CoMedicion48DTO> resultBaj = new List<CoMedicion48DTO>();
            var subListFecha = entitys.Select(x => new { Fecha = (DateTime)x.Rsffecha }).Distinct().ToList();

            foreach (var fecha in subListFecha)
            {
                var subListGrupo = entitys.Where(x => x.Rsffecha == fecha.Fecha).Select(x => new { Grupocodi = x.Grupocodi }).Distinct().ToList();

                foreach (var grupo in subListGrupo)
                {
                    CoMedicion48DTO entitySub = new CoMedicion48DTO();
                    CoMedicion48DTO entityBaj = new CoMedicion48DTO();

                    entitySub.Comedfecha = fecha.Fecha;
                    entitySub.Cotinfcodi = 5;
                    entitySub.Copercodi = version.Copercodi;
                    entitySub.Covercodi = version.Covercodi;
                    entitySub.Grupocodi = grupo.Grupocodi;
                    entitySub.Comedtipo = 2;
                    entitySub.Equicodi = -1;

                    entityBaj.Comedfecha = fecha.Fecha;
                    entityBaj.Cotinfcodi = 6;
                    entityBaj.Copercodi = version.Copercodi;
                    entityBaj.Covercodi = version.Covercodi;
                    entityBaj.Grupocodi = grupo.Grupocodi;
                    entityBaj.Comedtipo = 2;
                    entityBaj.Equicodi = -1;

                    List<CoMedicion48DTO> list = entitys.Where(x => x.Rsffecha == fecha.Fecha && x.Grupocodi == grupo.Grupocodi).OrderBy(x => x.Rsffechainicio).ToList();

                    if (list.Count == 48)
                    {
                        int i = 1;
                        foreach (CoMedicion48DTO item in list)
                        {
                            entitySub.GetType().GetProperty("H" + i).SetValue(entitySub, item.Rsfsubida);
                            entityBaj.GetType().GetProperty("H" + i).SetValue(entityBaj, item.Rsfbajada);

                            entitySub.GetType().GetProperty("T" + i).SetValue(entitySub, item.Rsfindicador);
                            entityBaj.GetType().GetProperty("T" + i).SetValue(entityBaj, item.Rsfindicador);

                            i++;
                        }
                    }

                    resultSub.Add(entitySub);
                    resultBaj.Add(entityBaj);
                }
            }

            listSub = resultSub;
            listBaj = resultBaj;
        }


        /// <summary>
        /// Permite extraer los datos desde las fuentes de datos
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="listPrograma"></param>
        /// <param name="listProgramaSR"></param>
        /// <param name="listRAUp"></param>
        /// <param name="listRADown"></param>
        /// <param name="listRAEject"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listRaDetalle"></param>
        public void ConsultarDatosProceso(int idVersion, out List<CpMedicion48DTO> listPrograma, out List<CpMedicion48DTO> listProgramaSR,
            out List<CpMedicion48DTO> listRAUp, out List<CpMedicion48DTO> listRADown, out List<CoMedicion48DTO> listRAEject,
            DateTime fechaInicio, DateTime fechaFin, out List<CoRaejecutadadetDTO> listRaDetalle)
        {
            try
            {
                List<CpMedicion48DTO> resultPrograma = new List<CpMedicion48DTO>();
                List<CpMedicion48DTO> resultProgramaSinReserva = new List<CpMedicion48DTO>();
                List<CpMedicion48DTO> resultRAUp = new List<CpMedicion48DTO>();
                List<CpMedicion48DTO> resultRADown = new List<CpMedicion48DTO>();

                //CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);
                //DateTime fechaInicio = (DateTime)version.Coverfecinicio;
                //DateTime fechaFin = (DateTime)version.Coverfecfin;

                List<CoMedicion48DTO> topologias = FactorySic.GetCoMedicion48Repository().ObtenerTopologias(fechaInicio, fechaFin);
                List<CoMedicion48DTO> topologiasBase = topologias.Where(x => x.Topfinal == 1).ToList();

                string propiedadPrograma = "62,63";
                string propiedadRADown = "107";
                string propiedadRAUp = "106";

                foreach (CoMedicion48DTO item in topologiasBase)
                {
                    List<CoMedicion48DTO> reprogramas = topologias.Where(x => x.Topcodi1 == item.Topcodi && x.Topfinal != 1).
                        OrderByDescending(y => y.Topcodi).ToList();

                    /*foreach (CoMedicion48DTO itemReprog in reprogramas)
                    {
                        if (itemReprog.Topiniciohora > 1) itemReprog.Topiniciohora = itemReprog.Topiniciohora + 1;
                    }*/

                    List<CpMedicion48DTO> itemResultPrograma = this.ObtenerDatosYupana(item, reprogramas, propiedadPrograma, 1);
                    resultPrograma.AddRange(itemResultPrograma);

                    List<CpMedicion48DTO> itemResultRAUp = this.ObtenerDatosYupana(item, reprogramas, propiedadRAUp, 2);
                    resultRAUp.AddRange(itemResultRAUp);

                    List<CpMedicion48DTO> itemResultRADown = this.ObtenerDatosYupana(item, reprogramas, propiedadRADown, 2);
                    resultRADown.AddRange(itemResultRADown);
                }

                List<CoMedicion48DTO> topologiasSinReserva = FactorySic.GetCoMedicion48Repository().ObtenerTopologiasSinReserva(fechaInicio, fechaFin);
                List<CoMedicion48DTO> topologiasBaseSinReserva = topologiasSinReserva.Where(x => x.Topfinal == 1).ToList();

                foreach (CoMedicion48DTO item in topologiasBaseSinReserva)
                {
                    List<CoMedicion48DTO> reprogramas = topologiasSinReserva.Where(x => x.Topcodi1 == item.Topcodi1 && x.Topfinal != 1).
                        OrderByDescending(y => y.Topiniciohora).ToList();

                    /*foreach (CoMedicion48DTO itemReprog in reprogramas)
                    {
                        if (itemReprog.Topiniciohora > 1) itemReprog.Topiniciohora = itemReprog.Topiniciohora + 1;
                    }*/

                    List<CpMedicion48DTO> itemResultPrograma = this.ObtenerDatosYupana(item, reprogramas, propiedadPrograma, 1);
                    resultProgramaSinReserva.AddRange(itemResultPrograma);
                }

                listPrograma = resultPrograma;
                listProgramaSR = resultProgramaSinReserva;
                listRAUp = resultRAUp;
                listRADown = resultRADown;

                List<CoRaejecutadadetDTO> listDetalleRA = new List<CoRaejecutadadetDTO>();
                listRAEject = FactorySic.GetCoMedicion48Repository().ObtenerDatosRAEjecutado(fechaInicio, fechaFin, out listDetalleRA);
                listRaDetalle = listDetalleRA;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener la reserva asignada ejecutada 
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="rsfUp"></param>
        /// <param name="rsfDown"></param>
        public void ObtenerDatosReservaEjecutada(List<CoMedicion48DTO> entitys, out string[][] rsfUp, out string[][] rsfDown)
        {
            try
            {
                List<DateTime> fechas = entitys.Select(x => x.Rsffecha).Distinct().ToList();
                var urs = entitys.Select(x => new { Grupocodi = x.Grupocodi, Gruponomb = x.Gruponomb, Ursnomb = x.Ursnomb }).Distinct().ToList();

                string[][] datosUp = new string[fechas.Count * 48 + 3][];
                string[][] datosDown = new string[fechas.Count * 48 + 3][];

                string[] header = new string[urs.Count + 1];
                string[] header1 = new string[urs.Count + 1];
                header[0] = "FechaHora/Central";
                header1[0] = "URS";

                int index = 1;
                foreach (var item in urs)
                {
                    header[index] = item.Gruponomb;
                    header1[index] = item.Ursnomb;
                    index++;
                }
                datosUp[0] = header;
                datosDown[0] = header;
                datosUp[1] = header1;
                datosDown[1] = header1;

                index = 2;

                foreach (DateTime fecha in fechas)
                {
                    for (int k = 1; k <= 48; k++)
                    {
                        datosUp[index] = new string[urs.Count + 1];
                        datosUp[index][0] = fecha.AddMinutes((k) * 30).ToString("dd/MM/yyyy HH:mm");
                        datosDown[index] = new string[urs.Count + 1];
                        datosDown[index][0] = fecha.AddMinutes((k) * 30).ToString("dd/MM/yyyy HH:mm");
                        index++;
                    }
                    int col = 1;

                    foreach (var item in urs)
                    {
                        List<CoMedicion48DTO> subList = entitys.Where(x => x.Rsffecha == fecha && x.Grupocodi == item.Grupocodi).
                            OrderBy(x => x.Rsffechainicio).ToList();

                        if (subList.Count == 48)
                        {
                            index = index - 48;

                            foreach (CoMedicion48DTO dato in subList)
                            {
                                datosUp[index][col] = (dato.Rsfsubida != null) ? ((decimal)dato.Rsfsubida).ToString() : string.Empty;
                                datosDown[index][col] = (dato.Rsfbajada != null) ? ((decimal)dato.Rsfbajada).ToString() : string.Empty;
                                index++;
                            }
                        }

                        col++;
                    }
                }

                rsfUp = datosUp;
                rsfDown = datosDown;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite mostrar los datos en la estructura correcta
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected string[][] MostrarInformacionInsumo(List<CpMedicion48DTO> result)
        {
            List<DateTime> fechas = result.Select(x => x.Medifecha).Distinct().OrderBy(x => x).ToList();
            string[][] datos = new string[fechas.Count * 48 + 2][];
            var cabeceras = result.Select(x => new { x.Recurcodisicoes, x.Famcodi, x.Recurnombre }).Distinct().ToList();

            string[] header = new string[cabeceras.Count + 1];
            header[0] = "FechaHora/Equipo";
            int index = 1;
            foreach (var item in cabeceras)
            {
                header[index] = item.Recurnombre;
                index++;
            }
            datos[0] = header;

            index = 1;

            foreach (DateTime fecha in fechas)
            {
                for (int k = 1; k <= 48; k++)
                {
                    datos[index] = new string[cabeceras.Count + 1];
                    datos[index][0] = fecha.AddMinutes((k) * 30).ToString("dd/MM/yyyy HH:mm");
                    index++;
                }
                int col = 1;
                try
                {
                    foreach (var cabecera in cabeceras)
                    {
                        CpMedicion48DTO dato = result.Where(x => x.Medifecha == fecha && x.Recurcodisicoes == cabecera.Recurcodisicoes
                        && x.Famcodi == cabecera.Famcodi).FirstOrDefault();

                        if (dato != null)
                        {
                            index = index - 48;
                            for (int i = 1; i <= 48; i++)
                            {
                                object hi = dato.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(dato);
                                datos[index][col] = (hi != null) ? hi.ToString() : string.Empty;
                                index++;
                            }
                        }

                        col++;
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }

            }

            return datos;
        }

        /// <summary>
        /// Permite mostrar los datos en la estructura correcta
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected string[][] MostrarInformacionInsumoRA(List<CpMedicion48DTO> result)
        {
            List<DateTime> fechas = result.Select(x => x.Medifecha).Distinct().OrderBy(x => x).ToList();
            string[][] datos = new string[fechas.Count * 48 + 3][];
            var cabeceras = result.Select(x => new { x.Recurcodisicoes, x.Famcodi, x.Recurnombre, x.Equinomb }).Distinct().ToList();

            string[] header = new string[cabeceras.Count + 1];
            string[] header1 = new string[cabeceras.Count + 1];
            header[0] = "FechaHora/Equipo";
            header1[0] = "URS";
            int index = 1;
            foreach (var item in cabeceras)
            {
                header[index] = item.Recurnombre;
                header1[index] = item.Equinomb;
                index++;
            }
            datos[0] = header;
            datos[1] = header1;

            index = 2;

            foreach (DateTime fecha in fechas)
            {
                for (int k = 1; k <= 48; k++)
                {
                    datos[index] = new string[cabeceras.Count + 1];
                    datos[index][0] = fecha.AddMinutes((k) * 30).ToString("dd/MM/yyyy HH:mm");
                    index++;
                }
                int col = 1;
                try
                {
                    foreach (var cabecera in cabeceras)
                    {
                        CpMedicion48DTO dato = result.Where(x => x.Medifecha == fecha && x.Recurcodisicoes == cabecera.Recurcodisicoes
                        && x.Famcodi == cabecera.Famcodi).FirstOrDefault();

                        if (dato != null)
                        {
                            index = index - 48;
                            for (int i = 1; i <= 48; i++)
                            {
                                object hi = dato.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(dato);
                                datos[index][col] = (hi != null) ? hi.ToString() : string.Empty;
                                index++;
                            }
                        }

                        col++;
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }

            }

            return datos;
        }

        /// <summary>
        /// Permite obtener los datos desde YUPANA
        /// </summary>
        /// <param name="item"></param>
        /// <param name="reprogramas"></param>
        /// <param name="propiedad"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<CpMedicion48DTO> ObtenerDatosYupana(CoMedicion48DTO item, List<CoMedicion48DTO> reprogramas, string propiedad, int tipo)
        {
            List<CpMedicion48DTO> resultado = new List<CpMedicion48DTO>();
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            bool flag = false;
            foreach (CoMedicion48DTO subList in reprogramas)
            {
                if (subList.Topiniciohora > 0)
                {
                    entitys.Add(new CoMedicion48DTO { Topcodi = subList.Topcodi, Topiniciohora = subList.Topiniciohora });
                    if (subList.Topiniciohora == 1)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                entitys.Add(new CoMedicion48DTO { Topcodi = item.Topcodi, Topiniciohora = 1 });
            }

            List<int> listTopcodi = entitys.Select(x => x.Topcodi).ToList();

            List<CpMedicion48DTO> listDatos = (tipo == 1) ? FactorySic.GetCoMedicion48Repository().ObtenerDatosAgrupados(string.Join<int>(",", listTopcodi),
                item.Topfecha, propiedad) : FactorySic.GetCoMedicion48Repository().GetByCriteria(string.Join<int>(",", listTopcodi),
                item.Topfecha, propiedad);

            List<CoMedicion48DTO> orden = entitys.OrderBy(x => x.Topiniciohora).ToList();

            //- Hacemos un completado de los datos en caso no existan al inicio del dia
            var totalEquipos = listDatos.Select(x => new { x.Recurcodisicoes, x.Famcodi, x.Srestcodi, x.Equinomb, x.Recurnombre }).Distinct().ToList();


            int count = 0;
            foreach (CoMedicion48DTO itemOrden in orden)
            {
                List<CpMedicion48DTO> datos = listDatos.Where(x => x.Topcodi == itemOrden.Topcodi).ToList();

                if (count == 0)
                {
                    resultado = datos;
                    var subTotalEquipos = resultado.Select(x => new { x.Recurcodisicoes, x.Famcodi, x.Srestcodi }).Distinct().ToList();
                    var faltantes = totalEquipos.Where(x => !subTotalEquipos.Any(y => x.Recurcodisicoes ==
                    y.Recurcodisicoes && x.Famcodi == y.Famcodi && x.Srestcodi == y.Srestcodi)).ToList();

                    foreach (var itemFaltante in faltantes)
                    {
                        resultado.Add(new CpMedicion48DTO
                        {
                            Recurcodisicoes = itemFaltante.Recurcodisicoes,
                            Famcodi = itemFaltante.Famcodi,
                            Srestcodi = itemFaltante.Srestcodi,
                            Medifecha = item.Topfecha,
                            Equinomb = itemFaltante.Equinomb,
                            Recurnombre = itemFaltante.Recurnombre
                        });
                    }
                }
                else
                {
                    int fin = 48;
                    if (count < orden.Count - 1)
                    {
                        fin = orden[count + 1].Topiniciohora;
                    }

                    foreach (CpMedicion48DTO subDato in resultado)
                    {
                        CpMedicion48DTO reemplazo = datos.Where(x => x.Recurcodisicoes == subDato.Recurcodisicoes && x.Famcodi == subDato.Famcodi &&
                        x.Srestcodi == subDato.Srestcodi).FirstOrDefault();

                        if (reemplazo != null)
                        {
                            for (int i = itemOrden.Topiniciohora; i <= fin; i++)
                            {
                                subDato.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(subDato,
                                    reemplazo.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(reemplazo));
                            }
                        }
                        else
                        {
                            for (int i = itemOrden.Topiniciohora; i <= fin; i++)
                            {
                                subDato.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(subDato, 0.0M);
                            }
                        }
                    }
                }

                count++;
            }

            return resultado;
        }

        #endregion                       

        #region Métodos Tabla CO_PERIODO_PROG

        /// <summary>
        /// Inserta un registro de la tabla CO_PERIODO_PROG
        /// </summary>
        public void SaveCoPeriodoProg(CoPeriodoProgDTO entity)
        {
            try
            {
                FactorySic.GetCoPeriodoProgRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_PERIODO_PROG
        /// </summary>
        public void UpdateCoPeriodoProg(CoPeriodoProgDTO entity)
        {
            try
            {
                FactorySic.GetCoPeriodoProgRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_PERIODO_PROG
        /// </summary>
        public void DeleteCoPeriodoProg(int perprgcodi)
        {
            try
            {
                FactorySic.GetCoPeriodoProgRepository().Delete(perprgcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_PERIODO_PROG
        /// </summary>
        public CoPeriodoProgDTO GetByIdCoPeriodoProg(int perprgcodi)
        {
            return FactorySic.GetCoPeriodoProgRepository().GetById(perprgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_PERIODO_PROG
        /// </summary>
        public List<CoPeriodoProgDTO> ListCoPeriodoProgs()
        {
            return FactorySic.GetCoPeriodoProgRepository().List().OrderByDescending(x=>x.Perprgvigencia).ToList();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoPeriodoProg
        /// </summary>
        public List<CoPeriodoProgDTO> GetByCriteriaCoPeriodoProgs()
        {
            return FactorySic.GetCoPeriodoProgRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CO_PROCESO_DIARIO

        /// <summary>
        /// Inserta un registro de la tabla CO_PROCESO_DIARIO
        /// </summary>
        public void SaveCoProcesoDiario(CoProcesoDiarioDTO entity)
        {
            try
            {
                FactorySic.GetCoProcesoDiarioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_PROCESO_DIARIO
        /// </summary>
        public void UpdateCoProcesoDiario(CoProcesoDiarioDTO entity)
        {
            try
            {
                FactorySic.GetCoProcesoDiarioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_PROCESO_DIARIO
        /// </summary>
        public void DeleteCoProcesoDiario(int prodiacodi)
        {
            try
            {
                FactorySic.GetCoProcesoDiarioRepository().Delete(prodiacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_PROCESO_DIARIO
        /// </summary>
        public CoProcesoDiarioDTO GetByIdCoProcesoDiario(int prodiacodi)
        {
            return FactorySic.GetCoProcesoDiarioRepository().GetById(prodiacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_PROCESO_DIARIO
        /// </summary>
        public List<CoProcesoDiarioDTO> ListCoProcesoDiarios()
        {
            return FactorySic.GetCoProcesoDiarioRepository().List();
        }
       
        #endregion

        #region Métodos Tabla CO_PROCESO_ERROR

        /// <summary>
        /// Inserta un registro de la tabla CO_PROCESO_ERROR
        /// </summary>
        public void SaveCoProcesoError(CoProcesoErrorDTO entity)
        {
            try
            {
                FactorySic.GetCoProcesoErrorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_PROCESO_ERROR
        /// </summary>
        public void UpdateCoProcesoError(CoProcesoErrorDTO entity)
        {
            try
            {
                FactorySic.GetCoProcesoErrorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_PROCESO_ERROR
        /// </summary>
        public void DeleteCoProcesoError(int proerrcodi)
        {
            try
            {
                FactorySic.GetCoProcesoErrorRepository().Delete(proerrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_PROCESO_ERROR
        /// </summary>
        public CoProcesoErrorDTO GetByIdCoProcesoError(int proerrcodi)
        {
            return FactorySic.GetCoProcesoErrorRepository().GetById(proerrcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_PROCESO_ERROR
        /// </summary>
        public List<CoProcesoErrorDTO> ListCoProcesoErrors()
        {
            return FactorySic.GetCoProcesoErrorRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_PROCESO_ERROR
        /// </summary>
        public List<CoProcesoErrorDTO> ListarCoProcesoErrorsPorDia(int prodiacodi)
        {
            return FactorySic.GetCoProcesoErrorRepository().ListarPorDia(prodiacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoProcesoError
        /// </summary>
        public List<CoProcesoErrorDTO> GetByCriteriaCoProcesoErrors()
        {
            return FactorySic.GetCoProcesoErrorRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CO_FACTOR_UTILIZACION

        /// <summary>
        /// Inserta un registro de la tabla CO_FACTOR_UTILIZACION
        /// </summary>
        public void SaveCoFactorUtilizacion(CoFactorUtilizacionDTO entity)
        {
            try
            {
                FactorySic.GetCoFactorUtilizacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_FACTOR_UTILIZACION
        /// </summary>
        public void UpdateCoFactorUtilizacion(CoFactorUtilizacionDTO entity)
        {
            try
            {
                FactorySic.GetCoFactorUtilizacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }        

        /// <summary>
        /// Elimina un registro de la tabla CO_FACTOR_UTILIZACION
        /// </summary>
        public void DeleteCoFactorUtilizacion(int facuticodi)
        {
            try
            {
                FactorySic.GetCoFactorUtilizacionRepository().Delete(facuticodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_FACTOR_UTILIZACION
        /// </summary>
        public CoFactorUtilizacionDTO GetByIdCoFactorUtilizacion(int facuticodi)
        {
            return FactorySic.GetCoFactorUtilizacionRepository().GetById(facuticodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_FACTOR_UTILIZACION
        /// </summary>
        public List<CoFactorUtilizacionDTO> ListCoFactorUtilizacions()
        {
            return FactorySic.GetCoFactorUtilizacionRepository().List();
        }        

        #endregion

        #region Métodos Tabla CO_MEDICION60

        /// <summary>
        /// Inserta un registro de la tabla CO_MEDICION60
        /// </summary>
        public void SaveCoMedicion60(CoMedicion60DTO entity)
        {
            try
            {
                FactorySic.GetCoMedicion60Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CO_MEDICION60
        /// </summary>
        public void UpdateCoMedicion60(CoMedicion60DTO entity)
        {
            try
            {
                FactorySic.GetCoMedicion60Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CO_MEDICION60
        /// </summary>
        public void DeleteCoMedicion60(int comedicodi)
        {
            try
            {
                FactorySic.GetCoMedicion60Repository().Delete(comedicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CO_MEDICION60
        /// </summary>
        public CoMedicion60DTO GetByIdCoMedicion60(int comedicodi)
        {
            return FactorySic.GetCoMedicion60Repository().GetById(comedicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CO_MEDICION60
        /// </summary>
        public List<CoMedicion60DTO> ListCoMedicion60s()
        {
            return FactorySic.GetCoMedicion60Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CoMedicion60
        /// </summary>
        public List<CoMedicion60DTO> GetByCriteriaCoMedicion60s()
        {
            return FactorySic.GetCoMedicion60Repository().GetByCriteria();
        }

        #endregion


        #region Nuevo Proceso de Cálculo

        /// <summary>
        /// Permite obtener los datos insumos para el proceso de calculo
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="datosPrograma"></param>
        /// <param name="datosRA"></param>
        public void ObtenerInsumosProcesoFinal(int idVersion, out string[][] datosPrograma, out string[][] datosRAUp,
            out string[][] datosRADown, out string[][] datosRAEjecutadoUp, out string[][] datosRAEjecutadoDown, out string[][] datosProgramaSinReserva)
        {
            try
            {
                CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);
                DateTime fechaInicio = (DateTime)version.Coverfecinicio;
                DateTime fechaFin = (DateTime)version.Coverfecfin;
                List<CoMedicion48DTO> listaPrograma = FactorySic.GetCoMedicion48Repository().ObtenerDatosProgramaFinal(idVersion, fechaInicio, fechaFin, 1);
                List<CoMedicion48DTO> listaProgramaSinReserva = FactorySic.GetCoMedicion48Repository().ObtenerDatosProgramaFinal(idVersion, fechaInicio, fechaFin, 2);
                List<CoMedicion48DTO> listaRaProgUp = FactorySic.GetCoMedicion48Repository().ObtenerDatosReservaFinal(idVersion, fechaInicio, fechaFin, 3);
                List<CoMedicion48DTO> listaRaProgDown = FactorySic.GetCoMedicion48Repository().ObtenerDatosReservaFinal(idVersion, fechaInicio, fechaFin, 4);
                List<CoMedicion48DTO> listaRaEjecUp = FactorySic.GetCoMedicion48Repository().ObtenerDatosReservaFinal(idVersion, fechaInicio, fechaFin, 5);
                List<CoMedicion48DTO> listaRaEjeDown = FactorySic.GetCoMedicion48Repository().ObtenerDatosReservaFinal(idVersion, fechaInicio, fechaFin, 6);

                int[][] colores = null;

                datosPrograma = this.MostrarInformacionProgramaFinal(listaPrograma);
                datosProgramaSinReserva = this.MostrarInformacionProgramaFinal(listaProgramaSinReserva);
                datosRAUp = this.MostrarInformacionReservaFinal(listaRaProgUp, out colores);
                datosRADown = this.MostrarInformacionReservaFinal(listaRaProgDown, out colores);
                datosRAEjecutadoUp = this.MostrarInformacionReservaFinal(listaRaEjecUp, out colores);
                datosRAEjecutadoDown = this.MostrarInformacionReservaFinal(listaRaEjeDown, out colores);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los datos del proceso
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="datosDespacho"></param>
        /// <param name="datosDespachoSinR"></param>
        public void ObtenerResultadoProceso(int idVersion, out string[][] datosDespacho, out string[][] datosDespachoSinR, out int[][] datosColorDespacho, out int[][] datosColorDespachoSinR)
        {
            try
            {
                //- Debemos aplicar lógica para obtener los datos de 7 y 8
                List<CoMedicion48DTO> listaDespacho = FactorySic.GetCoMedicion48Repository().ObtenerDatosResultadoFinal(idVersion, 11);
                List<CoMedicion48DTO> listaDespachoSinR = FactorySic.GetCoMedicion48Repository().ObtenerDatosResultadoFinal(idVersion, 12);
                int[][] coloresDespacho = null;
                int[][] coloresDespachoSinR = null;
                datosDespacho = this.MostrarInformacionReservaFinal(listaDespacho, out coloresDespacho);
                datosDespachoSinR = this.MostrarInformacionReservaFinal(listaDespachoSinR, out coloresDespachoSinR);
                datosColorDespacho = coloresDespacho;
                datosColorDespachoSinR = coloresDespachoSinR;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite mostrar los datos en la estructura correcta
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected string[][] MostrarInformacionProgramaFinal(List<CoMedicion48DTO> result)
        {
            List<DateTime> fechas = result.Select(x => x.Comedfecha).Distinct().OrderBy(x => x).ToList();
            string[][] datos = new string[fechas.Count * 48 + 2][];
            var cabeceras = result.Select(x => new { x.Comedtipo, x.Equicodi, x.Grupocodi, x.Gruponomb }).Distinct().ToList();
            string[] header = new string[cabeceras.Count + 1];
            header[0] = "FechaHora/Equipo";
            int index = 1;
            foreach (var item in cabeceras)
            {
                header[index] = item.Gruponomb;
                index++;
            }
            datos[0] = header;
            index = 1;

            foreach (DateTime fecha in fechas)
            {
                for (int k = 1; k <= 48; k++)
                {
                    datos[index] = new string[cabeceras.Count + 1];
                    datos[index][0] = fecha.AddMinutes((k) * 30).ToString("dd/MM/yyyy HH:mm");
                    index++;
                }
                int col = 1;
                try
                {
                    foreach (var cabecera in cabeceras)
                    {
                        List<CoMedicion48DTO> list = (cabecera.Comedtipo == 2) ? result.Where(x => x.Comedfecha == fecha && x.Grupocodi == cabecera.Grupocodi).OrderBy(x => x.Orden).ToList() :
                            result.Where(x => x.Comedfecha == fecha && x.Equicodi == cabecera.Equicodi).OrderBy(x => x.Orden).ToList();

                        if (list.Count == 48)
                        {
                            index = index - 48;
                            for (int i = 1; i <= 48; i++)
                            {
                                datos[index][col] = list[i - 1].Valor.ToString();
                                index++;
                            }
                        }

                        col++;
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }

            }

            return datos;
        }

        /// <summary>
        /// Permite mostrar los datos en la estructura correcta
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected string[][] MostrarInformacionReservaFinal(List<CoMedicion48DTO> result, out int[][] indicesColor)
        {
            List<DateTime> fechas = result.Select(x => x.Comedfecha).Distinct().OrderBy(x => x).ToList();
            string[][] datos = new string[fechas.Count * 48 + 3][];
            int[][] colores = new int[fechas.Count * 48][];
            var cabeceras = result.Select(x => new { x.Famcodi, x.Equicodi, x.Grupocodi, x.Gruponomb, x.Ursnomb }).Distinct().ToList();

            string[] header = new string[cabeceras.Count + 1];
            string[] header1 = new string[cabeceras.Count + 1];
            header[0] = "FechaHora/Equipo";
            header1[0] = "URS";
            int index = 1;
            foreach (var item in cabeceras)
            {
                header[index] = item.Gruponomb;
                header1[index] = item.Ursnomb;
                index++;
            }
            datos[0] = header;
            datos[1] = header1;

            index = 2;

            foreach (DateTime fecha in fechas)
            {
                for (int k = 1; k <= 48; k++)
                {
                    datos[index] = new string[cabeceras.Count + 1];
                    colores[index - 2] = new int[cabeceras.Count];
                    datos[index][0] = fecha.AddMinutes((k) * 30).ToString("dd/MM/yyyy HH:mm");
                    index++;
                }
                int col = 1;
                try
                {
                    foreach (var cabecera in cabeceras)
                    {
                        List<CoMedicion48DTO> list = result.Where(x => x.Comedfecha == fecha && ((x.Grupocodi == cabecera.Grupocodi))).OrderBy(x => x.Orden).ToList();

                        if (list.Count == 48)
                        {
                            index = index - 48;
                            for (int i = 1; i <= 48; i++)
                            {
                                datos[index][col] = list[i - 1].Valor.ToString();
                                colores[index - 2][col - 1] = list[i - 1].Color;

                                index++;
                            }
                        }

                        col++;
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
            indicesColor = colores;

            return datos;
        }


        protected string[][] MostrarInformacionReservaPublicacion(List<CoMedicion48DTO> resultConRSF, List<CoMedicion48DTO> resultSinRSF,
            out int[][] indicesColor)
        {
            List<DateTime> fechas = resultConRSF.Select(x => x.Comedfecha).Distinct().OrderBy(x => x).ToList();
            string[][] datos = new string[fechas.Count * 48 + 3][];
            int[][] colores = new int[fechas.Count * 48][];
            var cabeceras = resultConRSF.Select(x => new { x.Famcodi, x.Equicodi, x.Grupocodi, x.Gruponomb, x.Ursnomb }).OrderBy(x=>x.Ursnomb).Distinct().ToList();

            string[] header = new string[cabeceras.Count * 2 + 2];
            string[] header1 = new string[cabeceras.Count * 2 + 2];
            header[0] = "Día";
            header[1] = "Hora";
            header1[0] = "";
            header1[1] = "";

            int index = 2;

            for (int i = 0; i < 2; i++)
            {
                foreach (var item in cabeceras)
                {
                    header[index] = item.Ursnomb;
                    header1[index] = item.Gruponomb;
                    index++;
                }
            }

            datos[0] = header;
            datos[1] = header1;

            index = 2;

            foreach (DateTime fecha in fechas)
            {

                for (int k = 1; k <= 48; k++)
                {
                    datos[index] = new string[cabeceras.Count * 2 + 2];
                    colores[index - 2] = new int[cabeceras.Count * 2];
                    datos[index][0] = fecha.AddMinutes((k) * 30).ToString("dd/MM/yyyy");
                    datos[index][1] = fecha.AddMinutes((k) * 30).ToString("HH:mm");
                    index++;
                }
                int col = 2;

                foreach (var cabecera in cabeceras)
                {
                    List<CoMedicion48DTO> list = resultConRSF.Where(x => x.Comedfecha == fecha && ((x.Grupocodi == cabecera.Grupocodi))).OrderBy(x => x.Orden).ToList();

                    if (list.Count == 48)
                    {
                        index = index - 48;
                        for (int i = 1; i <= 48; i++)
                        {
                            datos[index][col] = list[i - 1].Valor.ToString();
                            colores[index - 2][col - 2] = list[i - 1].Color;

                            index++;
                        }
                    }

                    col++;
                }


                int count = 0;
                foreach (var cabecera in cabeceras)
                {
                    count++;
                    List<CoMedicion48DTO> list = resultSinRSF.Where(x => x.Comedfecha == fecha && ((x.Grupocodi == cabecera.Grupocodi))).OrderBy(x => x.Orden).ToList();

                    if (list.Count == 48)
                    {
                        index = index - 48;
                        for (int i = 1; i <= 48; i++)
                        {
                            try
                            {
                                datos[index][col] = list[i - 1].Valor.ToString();

                                colores[index - 2][col - 2] = list[i - 1].Color;

                                index++;

                            }
                            catch (Exception ex)
                            {
                                string s = ex.Message;
                            }
                        }
                    }

                    col++;
                }

                //index = index + 48;

            }
            indicesColor = colores;

            return datos;
        }

        #endregion

        /// <summary>
        /// Permite validar la existencia de los datos procesados
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        public int ValidarProcesoPeriodo(int idVersion)
        {
            CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);
            return FactorySic.GetCoProcesocalculoRepository().ValidarExistencia((int)version.Copercodi, version.Covercodi);
        }

        /// <summary>
        /// Pemite obtener la lista de URS
        /// </summary>
        /// <returns></returns>
        public List<CoMedicion48DTO> ObtenerListaURS()
        {
            return FactorySic.GetCoMedicion48Repository().ObtenerListadoURS();
        }

        /// <summary>
        /// Permite validar los insumos para el proceso de costos de oportunidad
        /// </summary>
        /// <param name="version"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="option"></param>
        /// <param name="factorUtilizacion"></param>
        /// <returns></returns>
        public int ValidarInsumoFactorPresencia(CoVersionDTO version, DateTime fechaInicio, DateTime fechaFin, int option, out List<CoFactorUtilizacionFecha> factorUtilizacion )
        {
            int result = 1;
            factorUtilizacion = new List<CoFactorUtilizacionFecha>();
            int nroDias = (int)fechaFin.Subtract(fechaInicio).TotalDays;
            string tipo = (option == ConstantesCostoOportunidad.ProcesoNormal) ? ConstantesCostoOportunidad.ProcesoMensual : ConstantesCostoOportunidad.ProcesoDiario;
            List<CoProcesoDiarioDTO> listProcesos = FactorySic.GetCoProcesoDiarioRepository().GetByCriteria(
                    tipo, (int)version.Copercodi, version.Covercodi, fechaInicio, fechaFin);

            for (int i = 0; i <= nroDias; i++)
            {
                DateTime fecha = (fechaInicio).AddDays(i);
                CoProcesoDiarioDTO proceso = listProcesos.Where(x => ((DateTime)x.Prodiafecha).Year == fecha.Year &&
                    ((DateTime)x.Prodiafecha).Month == fecha.Month && ((DateTime)x.Prodiafecha).Day == fecha.Day).FirstOrDefault();
               
                if(proceso != null)
                {
                    if (proceso.Prodiaestado == ConstantesCostoOportunidad.EstadoExitoso)
                    {
                        int resultCalculo = 1;
                        if (option == ConstantesCostoOportunidad.ProcesoNormal)
                        {
                            resultCalculo = this.CalcularFactoresUtilizacíon(proceso);
                        }
                        if(resultCalculo == 1)
                        {
                            CoFactorUtilizacionFecha factor = new CoFactorUtilizacionFecha();
                            factor.Fecha = fecha;
                            factor.ListaFactorUtilizacion = FactorySic.GetCoFactorUtilizacionRepository().GetByCriteria(proceso.Prodiacodi);
                            factorUtilizacion.Add(factor);

                            if(factor.ListaFactorUtilizacion.Count != 48)
                            {
                                result = 4; //- El periodo de programación es distinto de 0.5;
                            }
                        } 
                        else
                        {
                            result = 5; //- Errores en el cálculo de alfa y beta
                        }
                    }
                    else
                    {
                        result = 3; //- Dias con estado de proceso fallido
                    }
                }
                else
                {
                    result = 2; // No existen datos para todos los dias del periodo
                }
            }

            return result;
        }

        /// <summary>
        /// Permite ejecutar el proceso de calculo
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        public int ProcesarCalculo(int idVersion, DateTime fechaInicio, DateTime fechaFin, string usuario, int option)
        {
            try
            {
                //- Listado de Insumos del Proceso
                List<CoMedicion48DTO> listUrs = FactorySic.GetCoMedicion48Repository().ObtenerListadoURS();

                //- Obtener datos de la versión
                CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);

                //- Obtenemos los factores de utilizacion

                List<CoFactorUtilizacionFecha> listFactorUtilizacion = new List<CoFactorUtilizacionFecha>();
                int validacionFU = this.ValidarInsumoFactorPresencia(version, fechaInicio, fechaFin, option, out listFactorUtilizacion);

                if(validacionFU != 1)
                {
                    return validacionFU;
                }

                //- Obtenemos la configuración de las URS
                List<CoConfiguracionDetDTO> configuracionURS = FactorySic.GetCoConfiguracionDetRepository().ObtenerConfiguracion((int)version.Copercodi, version.Covercodi);

                //- Listado de URS con formulación especial
                List<CoUrsEspecialDTO> listURSEspecial = FactorySic.GetCoUrsEspecialRepository().GetByCriteria(idVersion);

                List<CoMedicion48DTO> listaPrograma = FactorySic.GetCoMedicion48Repository().ObtenerDatosProgramaFinal(idVersion, fechaInicio, fechaFin, 1);
                List<CoMedicion48DTO> listaProgramaSinReserva = FactorySic.GetCoMedicion48Repository().ObtenerDatosProgramaFinal(idVersion, fechaInicio, fechaFin, 2);
                List<CoMedicion48DTO> listaRaProgUp = FactorySic.GetCoMedicion48Repository().ObtenerDatosReservaFinal(idVersion, fechaInicio, fechaFin, 3);
                List<CoMedicion48DTO> listaRaProgDown = FactorySic.GetCoMedicion48Repository().ObtenerDatosReservaFinal(idVersion, fechaInicio, fechaFin, 4);
                List<CoMedicion48DTO> listaRaEjecUp = FactorySic.GetCoMedicion48Repository().ObtenerDatosReservaEjecutadaFinal(idVersion, fechaInicio, fechaFin, 5);
                List<CoMedicion48DTO> listaRaEjeDown = FactorySic.GetCoMedicion48Repository().ObtenerDatosReservaEjecutadaFinal(idVersion, fechaInicio, fechaFin, 6);

                //- Obtener las topologias
                List<CoMedicion48DTO> topologias = FactorySic.GetCoMedicion48Repository().ObtenerTopologias(fechaInicio, fechaFin);
                List<CoMedicion48DTO> topologiasBase = topologias.Where(x => x.Topfinal == 1).ToList();

                //- Obteniendo los datos de las bandas de todo el rango de fechas
                List<CoMedicion48DTO> listaPropHidro = FactorySic.GetCoMedicion48Repository().ObtenerPropiedadesHidraulica(fechaInicio, fechaFin);
                List<CoMedicion48DTO> listaPropTerm = FactorySic.GetCoMedicion48Repository().ObtenerPropiedadTermica(fechaInicio, fechaFin);

                //- Obteniendo las horas de operacion para determinar
                List<CoMedicion48DTO> listHoraOperacion = FactorySic.GetCoMedicion48Repository().ObtenerHorasOperacion(fechaInicio, fechaFin);

                //- Obtenemos las potencias efectivas
                List<CoMedicion48DTO> listaPotenciaEfectiva = new List<CoMedicion48DTO>();

                //- Obtenemos las potencias mínimas
                List<CoMedicion48DTO> listaPotenciaMinima = new List<CoMedicion48DTO>();

                foreach (CoUrsEspecialDTO ursEspecial in listURSEspecial)
                {
                    CoMedicion48DTO urs = listUrs.Where(x => x.Grupocodi == ursEspecial.Grupocodi).FirstOrDefault();

                    if (urs != null)
                    {
                        if (urs.Famcodi == 1)
                        {
                            listaPotenciaEfectiva.AddRange(FactorySic.GetCoMedicion48Repository().ObtenerPropiedadPotenciaEfectiva((int)urs.Equicodi));
                            listaPotenciaMinima.AddRange(FactorySic.GetCoMedicion48Repository().ObtenerPropiedadPotenciaMinima((int)urs.Equicodi));
                        }
                    }
                }

                decimal r_equipo = 0;
                decimal check2 = 0;


                //- Obtener datos de modos de operación y datos de las bandas

                int nroDias = (int)fechaFin.Subtract(fechaInicio).TotalDays;

                //- Estructura para almacenar resultados

                List<CoMedicion48DTO> resultDespacho = new List<CoMedicion48DTO>();
                List<CoMedicion48DTO> resultDespachoSinR = new List<CoMedicion48DTO>();
                List<CoMedicion48DTO> resultDepachoOpcional = new List<CoMedicion48DTO>();
                List<CoMedicion48DTO> resultDespachoSinROpcional = new List<CoMedicion48DTO>();
                List<CoMedicion48DTO> resultDespachoAjustado = new List<CoMedicion48DTO>();
                List<CoMedicion48DTO> resultDespachoSinRAjustado = new List<CoMedicion48DTO>();


                foreach (CoMedicion48DTO urs in listUrs)
                {
                    bool flagEspecial = (listURSEspecial.Where(x => x.Grupocodi == urs.Grupocodi).Count() > 0) ? true : false;
                    decimal pefec = 0;
                    decimal pMin = 0;

                    if (flagEspecial)
                    {
                        CoMedicion48DTO potenciaEfectiva = listaPotenciaEfectiva.Where(x => x.Equicodi == urs.Equicodi).FirstOrDefault();

                        if (potenciaEfectiva != null)
                        {
                            pefec = potenciaEfectiva.Pefec;
                        }

                        CoMedicion48DTO potenciaMinima = listaPotenciaMinima.Where(x => x.Equicodi == urs.Equicodi).FirstOrDefault();

                        if (potenciaMinima != null)
                        {
                            pMin = potenciaMinima.Pefec;
                        }
                    }

                    //- Obtenemos las configuraciones necesarias                    
                    List<CoConfiguracionDetDTO> configURS = configuracionURS.Where(x => x.Grupocodi == urs.Grupocodi).ToList();
                    List<CoConfiguracionDetDTO> configOperacion = configURS.Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoOperacionURS).OrderBy(x => x.Courdevigenciadesde).ToList();
                    List<CoConfiguracionDetDTO> configEquipo  = configURS.Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoEquipoRPF).OrderBy(x => x.Courdevigenciadesde).ToList();
                    DateTime fecInicioHab = DateTime.MinValue;
                    DateTime fecFinHab = DateTime.MaxValue;
                    if (configURS.Count > 0)
                    {
                        fecInicioHab  = (configURS[0].FecInicioHabilitacion != null) ? (DateTime)configURS[0].FecInicioHabilitacion : DateTime.MinValue;
                        fecFinHab = (configURS[0].FecFinHabilitacion != null) ? (DateTime)configURS[0].FecFinHabilitacion : DateTime.MaxValue;
                    }

                    for (int i = 0; i <= nroDias; i++)
                    {
                        DateTime fecha = (fechaInicio).AddDays(i);

                        #region Ticket INC 2023-000014
                        //- Valor del porcentaje RPF
                        double rpf = (double)(new ParametroAppServicio()).ObtenerValorParametro(CortoPlazo.Helper.ConstantesCortoPlazo.IdParametroRpf, fecha);
                        if (rpf == 0)
                            rpf = 0.024;

                        #endregion

                        //- Obtener los factores de utilización por dia
                        CoFactorUtilizacionFecha factorUtilizacion = listFactorUtilizacion.Where(x => x.Fecha.Year == fecha.Year 
                            && x.Fecha.Month == fecha.Month && x.Fecha.Day == fecha.Day).FirstOrDefault();

                        //- Validamos fechas de habilitación
                        bool flagHabilitacion = true;
                        if (!(fecFinHab.Subtract(fecha).TotalSeconds > 0 && fecha.Subtract(fecInicioHab).TotalSeconds > 0))
                            flagHabilitacion = false;

                        //- Validamos existencia de señales
                        bool flagSeniales = false;
                        CoConfiguracionDetDTO configuracionOperacion = this.ObtenerConfiguracionPorDia(configOperacion, version, fecha);
                        if (configuracionOperacion != null && configuracionOperacion.ContadorSenial > 0 ) flagSeniales = true;


                        //- Obtenemos check2 y r_equipo
                        CoConfiguracionDetDTO configuracionEquipo = this.ObtenerConfiguracionPorDia(configEquipo, version, fecha);
                        if (configuracionEquipo != null)
                        {
                            check2 = (configuracionEquipo.Courdeequipo == ConstantesCostoOportunidad.TieneEquipoParaRPF) ? 1 : 0;
                            r_equipo = (configuracionEquipo.Courderequip != null) ? (decimal)configuracionEquipo.Courderequip : 0;
                        }

                        //- Obteniendo los datos por dia y por URS

                        List<CoMedicion48DTO> listaRaEjec = listaRaEjecUp.Where(x => x.Grupocodi == urs.Grupocodi && x.Comedfecha == fecha).
                            OrderBy(x => x.Orden).ToList();

                        List<CoMedicion48DTO> listaRaEjecD = listaRaEjeDown.Where(x => x.Grupocodi == urs.Grupocodi && x.Comedfecha == fecha).
                            OrderBy(x => x.Orden).ToList();

                        List<CoMedicion48DTO> listaRaProg = listaRaProgUp.Where(x => x.Grupocodi == urs.Grupocodi && x.Comedfecha == fecha).
                            OrderBy(x => x.Orden).ToList();

                        List<CoMedicion48DTO> listaRaProgD = listaRaProgDown.Where(x => x.Grupocodi == urs.Grupocodi && x.Comedfecha == fecha).
                            OrderBy(x => x.Orden).ToList();

                        List<CoMedicion48DTO> listaProg = (urs.Famcodi == 1) ?
                            listaPrograma.Where(x => x.Comedtipo == 1 && x.Equicodi == urs.Equicodi && x.Comedfecha == fecha).ToList() :
                            listaPrograma.Where(x => x.Comedtipo == 2 && x.Grupocodi == urs.Equicodi && x.Comedfecha == fecha).ToList();

                        List<CoMedicion48DTO> listaProgSinReserva = (urs.Famcodi == 1) ?
                            listaProgramaSinReserva.Where(x => x.Comedtipo == 1 && x.Equicodi == urs.Equicodi && x.Comedfecha == fecha).ToList() :
                            listaProgramaSinReserva.Where(x => x.Comedtipo == 2 && x.Grupocodi == urs.Equicodi && x.Comedfecha == fecha).ToList();

                        //- Obtenemos los registros de los bloques de reserva ejecutada
                        List<CoRaejecutadadetDTO> listRaDetalle = FactorySic.GetCoRaejecutadadetRepository().GetByCriteria((int)version.Copercodi,
                            version.Covercodi, fecha);


                        //- Código de la topologia por dia
                        int topcodi = topologiasBase.Where(x => x.Topfecha == fecha).First().Topcodi;

                        decimal bandaMin = 0;
                        decimal bandaMax = 0;

                        if (urs.Famcodi == 1)
                        {
                            CoMedicion48DTO propHidro = listaPropHidro.Where(x => x.Topcodi == topcodi && x.Equicodi == urs.Equicodi).FirstOrDefault();

                            if (propHidro != null)
                            {
                                bandaMin = propHidro.Pmin;
                                bandaMax = propHidro.Pmax;
                            }
                        }

                        //- Obteniendo los datos para las térmicas
                        List<CoMedicion48DTO> subListPropTermina = listaPropTerm.Where(x => x.Topcodi == topcodi && x.Equicodi == urs.Equicodi).ToList();
                        List<CoMedicion48DTO> subListHoraOperacion = listHoraOperacion.Where(x => x.Equicodi == urs.Equicodi && x.Fechahop == fecha).ToList();

                        CoMedicion48DTO entityDespacho = new CoMedicion48DTO();
                        CoMedicion48DTO entityDespachoSinR = new CoMedicion48DTO();
                        CoMedicion48DTO entityDespachoOpcional = new CoMedicion48DTO();
                        CoMedicion48DTO entityDespachoSinROpcional = new CoMedicion48DTO();
                        CoMedicion48DTO entityDespachoAjustado = new CoMedicion48DTO();
                        CoMedicion48DTO entityDespachoSinRAjustado = new CoMedicion48DTO();

                        entityDespacho.Grupocodi = urs.Grupocodi;
                        entityDespacho.Comedtipo = 2;
                        entityDespacho.Equicodi = -1;
                        entityDespacho.Comedfecha = fecha;
                        entityDespacho.Cotinfcodi = 7;
                        entityDespacho.Copercodi = version.Copercodi;
                        entityDespacho.Covercodi = version.Covercodi;

                        entityDespachoSinR.Grupocodi = urs.Grupocodi;
                        entityDespachoSinR.Comedtipo = 2;
                        entityDespachoSinR.Equicodi = -1;
                        entityDespachoSinR.Comedfecha = fecha;
                        entityDespachoSinR.Cotinfcodi = 8;
                        entityDespachoSinR.Copercodi = version.Copercodi;
                        entityDespachoSinR.Covercodi = version.Covercodi;

                        entityDespachoOpcional.Grupocodi = urs.Grupocodi;
                        entityDespachoOpcional.Comedtipo = 2;
                        entityDespachoOpcional.Equicodi = -1;
                        entityDespachoOpcional.Comedfecha = fecha;
                        entityDespachoOpcional.Cotinfcodi = 9;
                        entityDespachoOpcional.Copercodi = version.Copercodi;
                        entityDespachoOpcional.Covercodi = version.Covercodi;

                        entityDespachoSinROpcional.Grupocodi = urs.Grupocodi;
                        entityDespachoSinROpcional.Comedtipo = 2;
                        entityDespachoSinROpcional.Equicodi = -1;
                        entityDespachoSinROpcional.Comedfecha = fecha;
                        entityDespachoSinROpcional.Cotinfcodi = 10;
                        entityDespachoSinROpcional.Copercodi = version.Copercodi;
                        entityDespachoSinROpcional.Covercodi = version.Covercodi;

                        entityDespachoAjustado.Grupocodi = urs.Grupocodi;
                        entityDespachoAjustado.Comedtipo = 2;
                        entityDespachoAjustado.Equicodi = -1;
                        entityDespachoAjustado.Comedfecha = fecha;
                        entityDespachoAjustado.Cotinfcodi = 11;
                        entityDespachoAjustado.Copercodi = version.Copercodi;
                        entityDespachoAjustado.Covercodi = version.Covercodi;

                        entityDespachoSinRAjustado.Grupocodi = urs.Grupocodi;
                        entityDespachoSinRAjustado.Comedtipo = 2;
                        entityDespachoSinRAjustado.Equicodi = -1;
                        entityDespachoSinRAjustado.Comedfecha = fecha;
                        entityDespachoSinRAjustado.Cotinfcodi = 12;
                        entityDespachoSinRAjustado.Copercodi = version.Copercodi;
                        entityDespachoSinRAjustado.Covercodi = version.Covercodi;

                        for (int k = 1; k <= 48; k++)
                        {
                            DateTime fechaM = fecha.AddMinutes(k * 30);
                            decimal valor = 0;
                            decimal valorSinR = 0;
                            decimal valor_ajustado = 0;
                            decimal valorSinR_ajustado = 0;
                            int color = 0;
                            decimal comax = 0;
                            decimal comax_ajustado = 0;
                            decimal alfa = (factorUtilizacion.ListaFactorUtilizacion[k - 1].Facutialfa != null) ? (decimal)factorUtilizacion.ListaFactorUtilizacion[k - 1].Facutialfa : 0;
                            decimal beta = (factorUtilizacion.ListaFactorUtilizacion[k - 1].Facutibeta != null) ? (decimal)factorUtilizacion.ListaFactorUtilizacion[k - 1].Facutibeta : 0;

                            CoMedicion48DTO itemRaEject = listaRaEjec.Where(x => x.Orden == k).FirstOrDefault();
                            CoMedicion48DTO itemRaEjectD = listaRaEjecD.Where(x => x.Orden == k).FirstOrDefault();
                            CoMedicion48DTO itemRaProg = listaRaProg.Where(x => x.Orden == k).FirstOrDefault();
                            CoMedicion48DTO itemRaProgD = listaRaProgD.Where(x => x.Orden == k).FirstOrDefault();
                            CoMedicion48DTO itemDespacho = listaProg.Where(x => x.Orden == k).FirstOrDefault();
                            CoMedicion48DTO itemDespachoSinR = listaProgSinReserva.Where(x => x.Orden == k).FirstOrDefault();

                            decimal raProg = (itemRaProg != null) ? itemRaProg.Valor : 0;
                            decimal raEjec = (itemRaEject != null) ? itemRaEject.Valor : 0;
                            decimal raProgD = (itemRaProgD != null) ? itemRaProgD.Valor : 0;
                            decimal raEjecD = (itemRaEjectD != null) ? itemRaEjectD.Valor : 0;
                            decimal despacho = (itemDespacho != null) ? itemDespacho.Valor : 0;
                            decimal despachoSinR = (itemDespachoSinR != null) ? itemDespachoSinR.Valor : 0;

                            List<CoRaejecutadadetDTO> itemRaDetalle = new List<CoRaejecutadadetDTO>();
                            //- Verificando si el bloque de rsf no es de 30 minutos
                            if (itemRaEject != null)
                            {
                                if (itemRaEject.Rsfindicador == 1)
                                {
                                    itemRaDetalle = listRaDetalle.Where(x => x.Coradeindice == k && x.Grupocodi == urs.Grupocodi).ToList();
                                    despacho = despacho * (itemRaDetalle.Sum(x => (int)x.Corademinutos)) / 30;
                                    despachoSinR = despachoSinR * (itemRaDetalle.Sum(x => (int)x.Corademinutos)) / 30;
                                }

                            }

                            //- Cálculo para la formulación especial

                            if (flagEspecial)
                            {
                                comax = pefec / (decimal)(1 + rpf * (1 - (double)(check2 * r_equipo))) - (bandaMin + raEjecD) / (decimal)(1 - rpf * (1 - (double)(check2 * r_equipo)));
                                comax_ajustado = pefec / (decimal)(1 + rpf * (1 - (double)(check2 * r_equipo))) - (bandaMin + raEjecD * (1 - beta)) / (decimal)(1 - rpf * (1 - (double)(check2 * r_equipo)));
                            }

                            //- Fin formulación especial

                            if (raProg + raProgD == 0 && raEjec + raEjecD == 0)
                            {
                                valor = 0;
                            }
                            else
                            {
                                //- Con reserva
                                if (raProg + raProgD == 0)
                                {
                                    //- Obteniendo las bandas
                                    if (urs.Famcodi == 2)
                                    {
                                        bandaMin = 0;
                                        bandaMax = 0;
                                        CoMedicion48DTO itemHO = subListHoraOperacion.Where(x => x.Hophorini <= fechaM && fechaM <= x.Hophorfin).FirstOrDefault();
                                        if (itemHO != null)
                                        {
                                            CoMedicion48DTO itemPropT = subListPropTermina.Where(x => x.Grupocodi == itemHO.Grupocodi).FirstOrDefault();

                                            if (itemPropT != null)
                                            {
                                                bandaMin = itemPropT.Pmin;
                                                bandaMax = itemPropT.Pmax;
                                            }
                                        }
                                    }

                                    decimal valorA = despachoSinR;
                                    decimal valorB = (bandaMin + raEjecD) / (1 - (decimal)rpf);
                                    decimal valorC = (bandaMax - raEjec) / (1 + (decimal)rpf);
                                    decimal valorA_ajustado = despachoSinR;
                                    decimal valorB_ajustado = (bandaMin + raEjecD ) / (1 - (decimal)rpf * (1 - check2 * r_equipo));
                                    decimal valorC_ajustado = (bandaMax - raEjec ) / (1 + (decimal)rpf * (1 - check2 * r_equipo));

                                    if (itemRaEject != null)
                                    {
                                        if (itemRaEject.Rsfindicador == 1)
                                        {
                                            valorB = 0;
                                            valorC = 0;
                                            valorB_ajustado = 0;
                                            valorC_ajustado = 0;
                                            foreach (CoRaejecutadadetDTO itemRa in itemRaDetalle)
                                            {
                                                valorB = valorB + ((bandaMin + ((itemRa.Coraderabaj != null) ? (decimal)itemRa.Coraderabaj : 0)) / (1 - (decimal)rpf)) * (int)itemRa.Corademinutos / 30;
                                                valorC = valorC + ((bandaMax - ((itemRa.Coraderasub != null) ? (decimal)itemRa.Coraderasub : 0)) / (1 + (decimal)rpf)) * (int)itemRa.Corademinutos / 30;

                                                valorB_ajustado = valorB_ajustado + ((bandaMin + ((itemRa.Coraderabaj != null) ? (decimal)itemRa.Coraderabaj * (1 - beta) : 0)) / (1 - (decimal)rpf * (1 - check2 * r_equipo))) * (int)itemRa.Corademinutos / 30;
                                                valorC_ajustado = valorC_ajustado + ((bandaMax - ((itemRa.Coraderasub != null) ? (decimal)itemRa.Coraderasub * (1 - alfa) : 0)) / (1 + (decimal)rpf * (1 - check2 * r_equipo))) * (int)itemRa.Corademinutos / 30;
                                            }
                                        }
                                    }

                                    if (valorA < valorB)
                                        valor = valorB;
                                    if (valorA > valorC)
                                        valor = valorC;
                                    if (valorA > valorB && valorA < valorC)
                                        valor = valorA;

                                    if (valorA_ajustado <= valorB_ajustado)                                    
                                        valor_ajustado = valorB_ajustado;                                    
                                    if(valorA_ajustado >= valorC_ajustado)                                    
                                        valor_ajustado = valorC_ajustado;                                    
                                    if (valorA_ajustado > valorB_ajustado && valorA_ajustado < valorC_ajustado)                                    
                                        valor_ajustado = valorA_ajustado;

                                    valor_ajustado = valor_ajustado - beta * raEjecD + alfa * raEjec;
                                    color = (bandaMin == 0 && bandaMax == 0) ? 2 : 1;

                                }
                                else if (raEjec + raEjecD == 0)
                                {
                                    valor = 0;
                                    valor_ajustado = 0;
                                }
                                else {
                                    valor = despacho;
                                    valor_ajustado = valor - beta * raEjecD + alfa * raEjec;
                                }

                                //- Sin Reserva
                                if (raProg + raProgD == 0 && raEjec + raEjecD > 0) {
                                    valorSinR = despacho;
                                }
                                if (raProg + raProgD > 0 && raEjec + raEjecD > 0) {
                                    valorSinR = despachoSinR;
                                }
                                if (raProg + raProgD > 0 && raEjec + raEjecD == 0) {
                                    valorSinR = 0;
                                }
                            }
                                                        
                            valorSinR_ajustado = valorSinR;

                            //- Colocamos los valores correctos de las especiales
                            decimal valorOpcional = valorSinR;
                            if (flagEspecial)
                            {
                                if (valorSinR - valor > comax)
                                {
                                    valorSinR = comax + valor;
                                }

                                if(valorSinR_ajustado - valor_ajustado > comax_ajustado)
                                {
                                    valorSinR_ajustado = valor_ajustado + comax_ajustado;
                                }

                            }

                            //- Inicador que el bloque no es de media hora exacta
                            if (itemRaEject != null)
                            {
                                if (itemRaEject.Rsfindicador == 1)
                                {
                                    color = 3;
                                }
                            }

                            entityDespacho.GetType().GetProperty("H" + k).SetValue(entityDespacho, valor);
                            entityDespacho.GetType().GetProperty("T" + k).SetValue(entityDespacho, color);
                            entityDespachoSinR.GetType().GetProperty("H" + k).SetValue(entityDespachoSinR, valorSinR);
                            entityDespachoSinR.GetType().GetProperty("T" + k).SetValue(entityDespachoSinR, color);

                            if (!flagHabilitacion)
                            {
                                valor_ajustado = 0;
                                valorSinR_ajustado = 0;
                            }

                            if (!flagSeniales)
                            {
                                valor_ajustado = valor;
                                valorSinR_ajustado = valorSinR;
                            }

                            entityDespachoAjustado.GetType().GetProperty("H" + k).SetValue(entityDespachoAjustado, valor_ajustado);
                            entityDespachoAjustado.GetType().GetProperty("T" + k).SetValue(entityDespachoAjustado, color);
                            entityDespachoSinRAjustado.GetType().GetProperty("H" + k).SetValue(entityDespachoSinRAjustado, valorSinR_ajustado);
                            entityDespachoSinRAjustado.GetType().GetProperty("T" + k).SetValue(entityDespachoSinRAjustado, color);


                            if (flagEspecial)
                            {
                                entityDespachoOpcional.GetType().GetProperty("H" + k).SetValue(entityDespachoOpcional, valor);
                                entityDespachoOpcional.GetType().GetProperty("T" + k).SetValue(entityDespachoOpcional, color);
                                entityDespachoSinROpcional.GetType().GetProperty("H" + k).SetValue(entityDespachoSinROpcional, valorOpcional);
                            }
                        }

                        resultDespacho.Add(entityDespacho);
                        resultDespachoSinR.Add(entityDespachoSinR);
                        resultDespachoAjustado.Add(entityDespachoAjustado);
                        resultDespachoSinRAjustado.Add(entityDespachoSinRAjustado);

                        if (flagEspecial)
                        {
                            resultDepachoOpcional.Add(entityDespachoOpcional);
                            resultDespachoSinROpcional.Add(entityDespachoSinROpcional);
                        }
                    }
                }

                //- Debemos almacenar los datos en la DB en Bulk

                FactorySic.GetCoMedicion48Repository().GrabarDatosBulkResult(resultDespacho, (int)version.Copercodi, version.Covercodi, 7, fechaInicio, fechaFin);
                FactorySic.GetCoMedicion48Repository().GrabarDatosBulkResult(resultDespachoSinR, (int)version.Copercodi, version.Covercodi, 8, fechaInicio, fechaFin);
                FactorySic.GetCoMedicion48Repository().GrabarDatosBulkResult(resultDepachoOpcional, (int)version.Copercodi, version.Covercodi, 9, fechaInicio, fechaFin);
                FactorySic.GetCoMedicion48Repository().GrabarDatosBulkResult(resultDespachoSinROpcional, (int)version.Copercodi, version.Covercodi, 10, fechaInicio, fechaFin);
                FactorySic.GetCoMedicion48Repository().GrabarDatosBulkResult(resultDespachoAjustado, (int)version.Copercodi, version.Covercodi, 11, fechaInicio, fechaFin);
                FactorySic.GetCoMedicion48Repository().GrabarDatosBulkResult(resultDespachoSinRAjustado, (int)version.Copercodi, version.Covercodi, 12, fechaInicio, fechaFin);

                CoProcesocalculoDTO entityAuditoria = FactorySic.GetCoProcesocalculoRepository().GetById(version.Covercodi);

                //- Insertamos la auditoria del proceso
                CoProcesocalculoDTO auditoria = new CoProcesocalculoDTO();
                auditoria.Copercodi = version.Copercodi;
                auditoria.Covercodi = version.Covercodi;
                auditoria.Coprcafecproceso = DateTime.Now;
                auditoria.Coprcausuproceso = usuario;
                auditoria.Coprcaestado = ConstantesAppServicio.Activo;
                auditoria.Coprcafuentedato = (option == ConstantesCostoOportunidad.ProcesoNormal) ? 
                    ConstantesCostoOportunidad.ProcesoMensual : ConstantesCostoOportunidad.ProcesoDiario;

                if (entityAuditoria == null)
                {
                    FactorySic.GetCoProcesocalculoRepository().Save(auditoria);
                }
                else
                {
                    auditoria.Coprcacodi = entityAuditoria.Coprcacodi;
                    FactorySic.GetCoProcesocalculoRepository().Update(auditoria);
                }

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener la configuración de una dia
        /// </summary>
        /// <param name="configuracion"></param>
        /// <param name="version"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public CoConfiguracionDetDTO ObtenerConfiguracionPorDia(List<CoConfiguracionDetDTO> configuracion, CoVersionDTO version, DateTime fecha)
        {
            CoConfiguracionDetDTO entity = null;

            if (configuracion.Count > 0)
            {
                if (configuracion.Count == 1) configuracion[0].Courdevigenciahasta = version.Coverfecfin;

                if(configuracion.Count > 1)
                {
                    for(int i = 0; i < configuracion.Count; i++)
                    {
                        if(i < configuracion.Count - 1)
                        {
                            if (configuracion[i].Courdevigenciahasta == null)
                                configuracion[i].Courdevigenciahasta = ((DateTime)configuracion[i + 1].Courdevigenciadesde).AddDays(-1);
                        }
                        else
                        {
                            if (configuracion[i].Courdevigenciahasta == null)
                                configuracion[i].Courdevigenciahasta = version.Coverfecfin;
                        }
                    }
                }

                entity = configuracion.Where(x => ((DateTime)x.Courdevigenciadesde).Subtract(fecha).TotalSeconds <= 0 &&
                                        ((DateTime)x.Courdevigenciahasta).Subtract(fecha).TotalSeconds >= 0).FirstOrDefault();
            }

            return entity;
        }

        /// <summary>
        /// Permite processar el envío a loquidación
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="usuario"></param>
        public void EnviarLiquidacion(int idVersion, string usuario, int idPeriodoTrn, int idVersionTrn)
        {
            try
            {
                CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);
                //- Reemplazando valores de 7 y 8
                //- Obteniendo la data para el envío
                List<Dominio.DTO.Transferencias.VcrDespachoursDTO> listDespacho = FactorySic.GetCoMedicion48Repository().ObtenerLiquidacionResultadoFinal
                    (idVersion, 11, idVersionTrn, usuario);
                List<Dominio.DTO.Transferencias.VcrDespachoursDTO> listDespachoSinR = FactorySic.GetCoMedicion48Repository().ObtenerLiquidacionResultadoFinal
                    (idVersion, 12, idVersionTrn, usuario);
                listDespacho.AddRange(listDespachoSinR);

                FactorySic.GetCoMedicion48Repository().EliminarLiquidacionFinal(idVersionTrn);

                int maxCodi = FactoryTransferencia.GetVcrDespachoursRepository().GetMaxId();

                //- Almacenando los datos
                foreach (Dominio.DTO.Transferencias.VcrDespachoursDTO item in listDespacho)
                {
                    item.Vcdurscodi = maxCodi;
                    maxCodi++;
                }

                FactoryTransferencia.GetVcrDespachoursRepository().GrabarBulk(listDespacho);

                //- Generando la auditoría del envío 

                CoEnvioliquidacionDTO entity = new CoEnvioliquidacionDTO
                {
                    Coenlifecha = DateTime.Now,
                    Coenliusuario = usuario,
                    Copercodi = version.Copercodi,
                    Covercodi = version.Covercodi,
                    Pericodi = idPeriodoTrn,
                    Vcrecacodi = idVersionTrn
                };

                FactorySic.GetCoEnvioliquidacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite ejecutar el reproceso de un conjunto de datos
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="indicador"></param>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        /// <param name="indicadorDatos"></param>
        /// <param name="usuario"></param>
        /// <param name="option"></param>
        /// <param name="importarSP7"></param>
        /// <returns></returns>
        public int EjecutarReproceso(int idVersion, int indicador, DateTime fecInicio, DateTime fecFin,
            int indicadorDatos, string usuario, int option, int importarSP7)
        {
            int result = 1;
            try
            {
                //- Obtenemos los datos de la versión
                CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);
                DateTime fechaInicio = (DateTime)version.Coverfecinicio;
                DateTime fechaFin = (DateTime)version.Coverfecfin;

                //- Cambiamos las fechas de reproceso
                if (indicador == 1)
                {
                    fechaInicio = fecInicio;
                    fechaFin = fecFin;
                }

                //- En caso se tengan que importar nuevamente los datos de los insumos
                if (indicadorDatos == 1)
                {
                    this.CopiarDatos(idVersion, fechaInicio, fechaFin);
                }

                if(importarSP7 == 1)
                {
                    this.ImportarTodoSeñalesSP7(ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo, null, (int)version.Copercodi, idVersion, usuario, string.Empty, out bool hayEjecucionEnCurso);
                }

                //- Se debe volver a enviar a reprocesar
                result = this.ProcesarCalculo(idVersion, fechaInicio, fechaFin, usuario, option);
            }
            catch (Exception ex)
            {
                //Si ocurrio un error al importar sp7
                if (importarSP7 == 1)
                {
                    //Indico que terminó la ejecución en curso
                    ActualizarEstadoEjecucion(ConstantesCostoOportunidad.EstadoNoHayEjecucionEnCurso, usuario);
                }
                throw new Exception(ex.Message, ex);
                result = -1;
            }
            return result;
        }

        /// <summary>
        /// Permite obtener los bloques donde no existe ra en los bloques completos
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        public List<CoRaejecutadadetDTO> ObtenerDetalleRAEjecutada(int idVersion)
        {
            CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);
            return FactorySic.GetCoRaejecutadadetRepository().ObtenerConsulta((int)version.Copercodi, idVersion);
        }

        #endregion

        #region Reportes
        /// <summary>
        /// Permite obtener los datos para el reporte
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idUrs"></param>
        /// <param name="idTipoInformacion"></param>
        /// <returns></returns>
        public string[][] ObtenerReporteProceso(int idVersion, DateTime fechaInicio, DateTime fechaFin, int idUrs, int idTipoInformacion,
            out int indicador, out List<List<int>> datosReprog, out int[][] colores)
        {
            List<CoMedicion48DTO> list = new List<CoMedicion48DTO>();
            indicador = 1;
            string[][] result = null;
            colores = null;

            if (idTipoInformacion == 1 || idTipoInformacion == 2)
            {
                list = FactorySic.GetCoMedicion48Repository().ObtenerReporteProgramadoFinal(idVersion, idTipoInformacion, fechaInicio,
                    fechaFin, idUrs);
                result = this.MostrarInformacionProgramaFinal(list);
            }
            else if (idTipoInformacion == 3 || idTipoInformacion == 4 || idTipoInformacion == 5 || idTipoInformacion == 6)
            {
                list = FactorySic.GetCoMedicion48Repository().ObtenerReporteReservaFinal(idVersion, idTipoInformacion, fechaInicio,
                    fechaFin, idUrs);
                int[][] coloresDespacho = null;
                result = this.MostrarInformacionReservaFinal(list, out coloresDespacho);
                indicador = 2;
            }
            else if (idTipoInformacion == 7 || idTipoInformacion == 8 || idTipoInformacion == 9 || idTipoInformacion == 10 || idTipoInformacion == 11 || idTipoInformacion == 12)
            {
                list = FactorySic.GetCoMedicion48Repository().ObtenerReporteDespachoFinal(idVersion, idTipoInformacion, fechaInicio,
                    fechaFin, idUrs);
                int[][] coloresDespacho = null;
                result = this.MostrarInformacionReservaFinal(list, out coloresDespacho);
                indicador = 2;
                colores = coloresDespacho;
            }

            List<CoMedicion48DTO> topologias = FactorySic.GetCoMedicion48Repository().ObtenerTopologias(fechaInicio, fechaFin);

            int dias = (int)fechaFin.Subtract(fechaInicio).TotalDays;
            List<List<int>> datos = new List<List<int>>();

            for (int i = 0; i <= dias; i++)
            {
                DateTime fecha = fechaInicio.AddDays(i);
                List<CoMedicion48DTO> subTopologias = topologias.Where(x => x.Topfecha == fecha).ToList();
                CoMedicion48DTO topologiaBase = subTopologias.Where(x => x.Topfinal == 1).First();
                List<CoMedicion48DTO> reprogramas = topologias.Where(x => x.Topcodi1 == topologiaBase.Topcodi && x.Topfinal != 1).
                     OrderBy(y => y.Topcodi).ToList();

                List<int> horas = new List<int>();

                bool flag = false;
                foreach (CoMedicion48DTO subList in reprogramas)
                {
                    //horas.Add(subList.Topiniciohora);

                    //if (subList.Topiniciohora > 1) subList.Topiniciohora = subList.Topiniciohora + 1;
                    if (subList.Topiniciohora == 1)
                    {
                        subList.Topiniciohora = subList.Topiniciohora - 1;
                        flag = true;
                        //break;
                    }
                    horas.Add(subList.Topiniciohora);
                }
                if (!flag)
                {
                    horas.Insert(0, topologiaBase.Topiniciohora);
                }


                datos.Add(horas);
            }

            datosReprog = datos;

            return result;
        }

        /// <summary>
        /// Permite generar el reporte de publicación
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="colores"></param>
        /// <returns></returns>
        public string[][] ObtenerReportePublicacion(int idVersion, DateTime fechaInicio, DateTime fechaFin, out int[][] colores, int tipo)
        {
            string[][] result = null;
            colores = null;

            //- Cambio para obtener
            int idDespacho = (tipo == 0) ? 11 : 7;
            var listReserva = FactorySic.GetCoMedicion48Repository().ObtenerReporteDespachoFinal(idVersion, idDespacho, fechaInicio,
                   fechaFin, 0);
            int idDespachoSinR = (tipo == 0) ? 12 : 8;
            var listSinReserva = FactorySic.GetCoMedicion48Repository().ObtenerReporteDespachoFinal(idVersion, idDespachoSinR, fechaInicio,
                   fechaFin, 0);

            int[][] coloresDespacho = null;
            result = this.MostrarInformacionReservaPublicacion(listSinReserva, listReserva, out coloresDespacho);

            colores = coloresDespacho;

            return result;
        }


        /// <summary>
        /// Permite obtener el reporte de los reprogramadas
        /// </summary>
        /// <param name="idPeriodo"></param>
        public void ObtenerReporteReprograma(int idPeriodo, string path, string filename)
        {
            try
            {

                CoPeriodoDTO entity = FactorySic.GetCoPeriodoRepository().GetById(idPeriodo);
                DateTime fechaInicio = new DateTime((int)entity.Coperanio, (int)entity.Copermes, 1);
                DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
                List<CoMedicion48DTO> list = FactorySic.GetCoMedicion48Repository().ObtenerReporteReprograma(fechaInicio, fechaFin);
                List<CoMedicion48DTO> listSinRsf = FactorySic.GetCoMedicion48Repository().ObtenerReporteReprogramaSinRSF(fechaInicio, fechaFin);
                List<CoMedicion48DTO> listHora = list.Where(x => x.Topiniciohora != 0).ToList();


                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {

                    #region Hoja Reprogramas con Reserva

                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("HORA INICIO REPROGRAMA");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE REPROGRAMAS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "DÍA";
                        ws.Cells[index, 3].Value = "RDO_A";
                        ws.Cells[index, 4].Value = "RDO_B";
                        ws.Cells[index, 5].Value = "RDO_C";
                        ws.Cells[index, 6].Value = "RDO_D";
                        ws.Cells[index, 7].Value = "RDO_E";
                        ws.Cells[index, 8].Value = "RDO_F";
                        ws.Cells[index, 9].Value = "RDO_G";
                        ws.Cells[index, 10].Value = "RDO_H";

                        rg = ws.Cells[index, 2, index, 10];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        List<DateTime> fechas = listHora.Select(x => x.Topfecha).Distinct().OrderBy(x => x).ToList();

                        index = 6;
                        foreach (DateTime item in fechas)
                        {
                            List<int> bloques = listHora.Where(x => x.Topfecha == item).Select(x => x.Topiniciohora).OrderBy(x => x).ToList();

                            ws.Cells[index, 2].Value = item.ToString("dd/MM/yyyy");
                            int j = 1;
                            foreach (int i in bloques)
                            {
                                ws.Cells[index, 2 + j].Value = item.AddMinutes(30 * i).ToString("HH:mm");
                                j++;
                            }

                            rg = ws.Cells[index, 2, index, 10];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 10];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 10];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    #endregion


                    #region Hoja Reprogramas con Reserva

                    ExcelWorksheet wsConRsf = xlPackage.Workbook.Worksheets.Add("REPROGRAMAS CON RSF");

                    if (wsConRsf != null)
                    {
                        wsConRsf.Cells[2, 3].Value = "REPORTE DE REPROGRAMAS CON RSF";

                        ExcelRange rg = wsConRsf.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        wsConRsf.Cells[index, 2].Value = "DÍA";
                        wsConRsf.Cells[index, 3].Value = "PDO";
                        wsConRsf.Cells[index, 4].Value = "RDO_A";
                        wsConRsf.Cells[index, 5].Value = "RDO_B";
                        wsConRsf.Cells[index, 6].Value = "RDO_C";
                        wsConRsf.Cells[index, 7].Value = "RDO_D";
                        wsConRsf.Cells[index, 8].Value = "RDO_E";
                        wsConRsf.Cells[index, 9].Value = "RDO_F";
                        wsConRsf.Cells[index, 10].Value = "RDO_G";
                        wsConRsf.Cells[index, 11].Value = "RDO_H";

                        rg = wsConRsf.Cells[index, 2, index, 11];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        List<DateTime> fechas = list.Select(x => x.Topfecha).Distinct().OrderBy(x => x).ToList();

                        index = 6;
                        foreach (DateTime item in fechas)
                        {
                            var bloques = list.Where(x => x.Topfecha == item).Select(x => new { x.Topcodi, x.Topnombre, x.Topiniciohora }).
                                OrderBy(x => x.Topiniciohora).ToList();

                            wsConRsf.Cells[index, 2].Value = item.ToString("dd/MM/yyyy");
                            int j = 1;
                            foreach (var i in bloques)
                            {
                                wsConRsf.Cells[index, 2 + j].Value = i.Topcodi + "-" + i.Topnombre;
                                j++;
                            }

                            rg = wsConRsf.Cells[index, 2, index, 11];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = wsConRsf.Cells[5, 2, index - 1, 11];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        wsConRsf.Column(2).Width = 30;

                        rg = wsConRsf.Cells[5, 3, index, 11];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = wsConRsf.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    #endregion

                    #region Hoja Reprogramas sin Reserva

                    ExcelWorksheet wsSinRsf = xlPackage.Workbook.Worksheets.Add("REPROGRAMAS SIN RSF");

                    if (wsSinRsf != null)
                    {
                        wsSinRsf.Cells[2, 3].Value = "REPORTE DE REPROGRAMAS SIN RSF";

                        ExcelRange rg = wsSinRsf.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        wsSinRsf.Cells[index, 2].Value = "DÍA";
                        wsSinRsf.Cells[index, 3].Value = "PDO";
                        wsSinRsf.Cells[index, 4].Value = "RDO_A";
                        wsSinRsf.Cells[index, 5].Value = "RDO_B";
                        wsSinRsf.Cells[index, 6].Value = "RDO_C";
                        wsSinRsf.Cells[index, 7].Value = "RDO_D";
                        wsSinRsf.Cells[index, 8].Value = "RDO_E";
                        wsSinRsf.Cells[index, 9].Value = "RDO_F";
                        wsSinRsf.Cells[index, 10].Value = "RDO_G";
                        wsSinRsf.Cells[index, 11].Value = "RDO_H";

                        rg = wsSinRsf.Cells[index, 2, index, 11];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        List<DateTime> fechas = listSinRsf.Select(x => x.Topfecha).Distinct().OrderBy(x => x).ToList();

                        index = 6;
                        foreach (DateTime item in fechas)
                        {
                            var bloques = listSinRsf.Where(x => x.Topfecha == item).Select(x => new { x.Topcodi, x.Topnombre, x.Topiniciohora }).
                                OrderBy(x => x.Topiniciohora).ToList();

                            wsSinRsf.Cells[index, 2].Value = item.ToString("dd/MM/yyyy");
                            int j = 1;
                            foreach (var i in bloques)
                            {
                                wsSinRsf.Cells[index, 2 + j].Value = i.Topcodi + "-" + i.Topnombre;
                                j++;
                            }

                            rg = wsSinRsf.Cells[index, 2, index, 11];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = wsSinRsf.Cells[5, 2, index - 1, 11];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        wsSinRsf.Column(2).Width = 30;

                        rg = wsSinRsf.Cells[5, 3, index, 11];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = wsSinRsf.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    #endregion


                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar el documento excel con los datos de los usuarios
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public void GenerarReporteExcel(string[][] list, string path, string filename, int indicador, int idTipoInformacion,
            List<List<int>> reprogramas, int[][] colores)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE COSTOS DE OPORTUNIDAD";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;
                        int col = 2;
                        int colmax = list[0].Length;
                        int rowmax = list.Length - 1;

                        if (idTipoInformacion == 13) rowmax = list.Length;

                        for (int i = 0; i < rowmax; i++)
                        {
                            for (int j = 0; j < colmax; j++)
                            {
                                if (i < indicador)
                                {
                                    ws.Cells[index + i, col + j].Value = list[i][j].Trim();
                                }
                                else
                                {
                                    if (j > 0)
                                    {
                                        ws.Cells[index + i, col + j].Value = (!string.IsNullOrEmpty(list[i][j])) ? (decimal?)Convert.ToDecimal(list[i][j]) : null;
                                    }
                                    else
                                    {
                                        ws.Cells[index + i, col + j].Value = list[i][j];
                                    }
                                }
                            }
                        }

                        rg = ws.Cells[index, col, index + indicador - 1, colmax + 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int dif = 0;
                        if (idTipoInformacion == 13) dif = -2;

                        rg = ws.Cells[index, col, index + rowmax + indicador + dif, colmax + 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));


                        if (idTipoInformacion < 5)
                        {
                            int elementos = rowmax - indicador;
                            int inicio = index + indicador;

                            if (elementos / 48 == reprogramas.Count)
                            {
                                foreach (List<int> reprograma in reprogramas)
                                {
                                    int color = 0;
                                    foreach (int posicion in reprograma)
                                    {
                                        rg = ws.Cells[inicio + posicion, col, inicio + 48 - 1, colmax + 1];
                                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(this.ObtenerColor(color)));
                                        color++;
                                    }

                                    inicio = inicio + 48;
                                }
                            }
                        }

                        if (idTipoInformacion == 7)
                        {
                            for (int k = 0; k < colores.Length; k++)
                            {
                                for (int m = 0; m < colores[k].Length; m++)
                                {
                                    if (colores[k][m] == 1)
                                    {
                                        rg = ws.Cells[index + indicador + k, col + 1 + m, index + indicador + k, col + 1 + m];
                                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF69B4"));
                                    }
                                }
                            }
                        }

                        if (idTipoInformacion != 13) { 
                            for (int t = 3; t <= col + 1; t++)
                            {
                                ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                            }
                        }

                        rg = ws.Cells[index, col, rowmax, colmax + 1];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el reporte
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="indicador"></param>
        /// <param name="colores"></param>
        public void GenerarReporteExcelPublicacion(string[][] listAjustado, string[][] list, string path, string filename, int indicador, int[][] colores)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    if (listAjustado.Length > 3) this.GenerarHojaReportePublicacion(xlPackage, listAjustado, indicador, "DESPACHO AJUSTADO", 0);
                    this.GenerarHojaReportePublicacion(xlPackage, list, indicador, "DESPACHO", 1);

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar la hoja del reporte de publicación
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="list"></param>
        /// <param name="indicador"></param>
        public void GenerarHojaReportePublicacion(ExcelPackage xlPackage, string[][] list, int indicador, string hoja, int tipo)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(hoja);

            if (ws != null)
            {
                int index = 5;
                int col = 2;
                int colmax = list[0].Length;
                int rowmax = list.Length - 1;
                int grupos = (colmax - 2) / 2;

                ws.Cells[3, 4].Value = (tipo == 0)? "DESPACHO AJUSTADO SIN ASIGNACIÓN DE RESERVA (MW)": "DESPACHO SIN ASIGNACIÓN DE RESERVA (MW)";
                ws.Cells[3, 4 + grupos].Value = (tipo == 0) ? "DESPACHO AJUSTADO CON ASIGNACIÓN DE RESERVA (MW)": "DESPACHO CON ASIGNACIÓN DE RESERVA (MW)";

                ExcelRange rg = ws.Cells[3, 4, 3, 4 + colmax - 3];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4C97C3"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 14;
                rg.Style.Font.Bold = true;

                rg = ws.Cells[3, 4, 3, 4 + grupos - 1];
                rg.Merge = true;

                rg.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#000000"));


                rg = ws.Cells[3, 4 + grupos, 3, 4 + colmax - 3];
                rg.Merge = true;

                rg.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#000000"));

                rg = ws.Cells[5, 2, 6, 2];
                rg.Merge = true;
                rg = ws.Cells[5, 3, 6, 3];
                rg.Merge = true;

                rg = ws.Cells[7, 2, 7 + rowmax - 3, 3];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4C97C3"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;


                for (int i = 0; i < rowmax; i++)
                {
                    for (int j = 0; j < colmax; j++)
                    {
                        if (i < indicador)
                        {
                            ws.Cells[index + i, col + j].Value = list[i][j].Trim();
                        }
                        else
                        {
                            if (j > 1)
                            {
                                if (!string.IsNullOrEmpty(list[i][j]))
                                {
                                    decimal? valor = (decimal?)Convert.ToDecimal(list[i][j]);
                                    ws.Cells[index + i, col + j].Value = valor;

                                    if (valor > 0)
                                    {
                                        rg = ws.Cells[index + i, col + j, index + i, col + j];
                                        rg.Style.Numberformat.Format = "#,##0.00";
                                        rg.Style.Font.Bold = true;
                                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDEBF7"));
                                    }
                                    else
                                    {
                                        rg = ws.Cells[index + i, col + j, index + i, col + j];
                                        rg.Style.Numberformat.Format = "#,##0.00";
                                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }
                                }
                                else
                                {
                                    ws.Cells[index + i, col + j].Value = null;
                                }

                            }
                            else
                            {
                                ws.Cells[index + i, col + j].Value = list[i][j];
                            }
                        }
                    }
                }

                rg = ws.Cells[index, col, index + indicador - 1, colmax + 1];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4C97C3"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                rg = ws.Cells[index, col, index + rowmax + indicador - 3, colmax + 1];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#000000"));


                /*for (int t = 3; t <= col + 1; t++)
                {
                    ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                }*/

                rg = ws.Cells[index, col, rowmax, colmax + 1];
                rg.AutoFitColumns();

                 ws.View.FreezePanes(7, 4);            

                ws.View.ShowGridLines = false;
                 ws.View.ZoomScale = 80;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(80, 40);
            }
        }


        /// <summary>
        /// Permite obtener el codigo de color
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public string ObtenerColor(int indice)
        {
            List<string> colores = new List<string> { "#DFF4FF", "#C4EAFF", "#A8E0FF", "#97DBFF", "#82D3FF", "#64C9FF" };
            return colores[indice];
        }

        /// <summary>
        /// Permite obtener el reporte de bandas
        /// </summary>
        /// <param name="idPeriodo"></param>
        public List<CoMedicion48DTO> ObtenerReporteBandas(int idPeriodo)
        {
            CoPeriodoDTO periodo = FactorySic.GetCoPeriodoRepository().GetById(idPeriodo);
            DateTime fechaInicio = new DateTime((int)periodo.Coperanio, (int)periodo.Copermes, 1);
            DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

            //- Obteniendo los datos de las bandas de todo el rango de fechas
            List<CoMedicion48DTO> listaBandas = FactorySic.GetCoMedicion48Repository().ObtenerReporteBandas(fechaInicio, fechaFin);

            return listaBandas;
        }

        #endregion

        #region RSF_PR22

        #region Configuracion de URS


        /// <summary>
        /// Permite obtener la configuración de la una URS
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersion"></param>
        /// <param name="idUrs"></param>
        public CoConfiguracionUrsDTO ObtenerConfiguracionURS(int idPeriodo, int idVersion, int idUrs)
        {
            CoConfiguracionUrsDTO configuracion = FactorySic.GetCoConfiguracionUrsRepository().GetById(idPeriodo, idVersion, idUrs);
            List<CoConfiguracionDetDTO> configuracionDetalle = new List<CoConfiguracionDetDTO>();

            if (configuracion != null)
            {
                configuracionDetalle = FactorySic.GetCoConfiguracionDetRepository().GetByCriteria(configuracion.Conurscodi);
                configuracion.FechaInicio = (configuracion.Conursfecinicio != null) ?
                    ((DateTime)configuracion.Conursfecinicio).ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
                configuracion.FechaFin = (configuracion.Conursfecfin != null) ?
                    ((DateTime)configuracion.Conursfecfin).ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
            }
            else
            {
                configuracion = new CoConfiguracionUrsDTO();
            }

            List<CoConfiguracionDetDTO> listOperacion = configuracionDetalle.Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoOperacionURS).ToList();
            List<List<string>> dataOperacion = new List<List<string>>();
            foreach (CoConfiguracionDetDTO item in listOperacion)
            {
                string[] data = {item.Courdecodi.ToString(), item.Courdeoperacion,
                    (item.Courdevigenciadesde!=null)?((DateTime)item.Courdevigenciadesde).ToString(ConstantesAppServicio.FormatoFecha):string.Empty,
                    (item.Courdevigenciahasta!=null)?((DateTime)item.Courdevigenciahasta).ToString(ConstantesAppServicio.FormatoFecha):string.Empty};
                dataOperacion.Add(data.ToList());
            }
            if (dataOperacion.Count == 0) dataOperacion.Add((new string[4]).ToList());
            configuracion.DatosOperacionURS = dataOperacion;

            List<CoConfiguracionDetDTO> listReporteExtranet = configuracionDetalle.Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoReporteExtranet).ToList();
            List<List<string>> dataReporteExtranet = new List<List<string>>();
            foreach (CoConfiguracionDetDTO item in listReporteExtranet)
            {
                string[] data = {item.Courdecodi.ToString(), item.Courdereporte,
                    (item.Courdevigenciadesde!=null)?((DateTime)item.Courdevigenciadesde).ToString(ConstantesAppServicio.FormatoFecha):string.Empty,
                    (item.Courdevigenciahasta!=null)?((DateTime)item.Courdevigenciahasta).ToString(ConstantesAppServicio.FormatoFecha):string.Empty};
                dataReporteExtranet.Add(data.ToList());
            }
            if (dataReporteExtranet.Count == 0) dataReporteExtranet.Add((new string[4]).ToList());
            configuracion.DatosReporteExtranet = dataReporteExtranet;

            List<CoConfiguracionDetDTO> listEquipoRPF = configuracionDetalle.Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoEquipoRPF).ToList();
            List<List<string>> dataEquipoRPF = new List<List<string>>();
            foreach (CoConfiguracionDetDTO item in listEquipoRPF)
            {
                string[] data = {item.Courdecodi.ToString(), item.Courdeequipo, (item.Courderequip!=null)?item.Courderequip.ToString():string.Empty,
                    (item.Courdevigenciadesde!=null)?((DateTime)item.Courdevigenciadesde).ToString(ConstantesAppServicio.FormatoFecha):string.Empty,
                    (item.Courdevigenciahasta!=null)?((DateTime)item.Courdevigenciahasta).ToString(ConstantesAppServicio.FormatoFecha):string.Empty};
                dataEquipoRPF.Add(data.ToList());
            }
            if (dataEquipoRPF.Count == 0) dataEquipoRPF.Add((new string[5]).ToList());
            configuracion.DatosEquipoRPF = dataEquipoRPF;
            configuracion.ListaZonas = FactoryScada.GetTrZonaSp7Repository().List();

            return configuracion;
        }

        /// <summary>
        /// Permite obtener la configuracion para las señales
        /// </summary>
        /// <param name="idCondiguracionDet"></param>
        /// <param name="tipo"></param>
        /// <param name="idUrs"></param>
        public CoConfiguracionUrsDTO ObtenerConfiguracionSenial(int? idCondiguracionDet, int tipo, int idUrs)
        {
            CoConfiguracionUrsDTO entity = new CoConfiguracionUrsDTO();
            List<List<string>> result = new List<List<string>>();
            List<ConfiguracionURSData> resultCanal = new List<ConfiguracionURSData>();

            List<EveRsfdetalleDTO> configuracion = (new RsfAppServicio()).ObtenerConfiguracion(DateTime.Now).Where(x => x.Grupocodi == idUrs).ToList();
            List<EveRsfdetalleDTO> datos = configuracion.Where(x => x.Grupotipo == ConstantesAppServicio.SI).ToList();
            if (tipo == ConstantesCostoOportunidad.TipoUnidad)
            {
                datos = configuracion.Where(x => x.Grupotipo != ConstantesAppServicio.SI).ToList();
            }

            List<CoTipoDatoDTO> listTipoDato = FactorySic.GetCoTipoDatoRepository().GetByCriteria(ConstantesCostoOportunidad.TipoDatoSenial);
            List<CoConfiguracionSenialDTO> detalle = (idCondiguracionDet != null) ?
                FactorySic.GetCoConfiguracionSenialRepository().GetByCriteria((int)idCondiguracionDet) : new List<CoConfiguracionSenialDTO>();

            int indice = 0;
            foreach (EveRsfdetalleDTO item in datos)
            {
                int? idZona = null;
                foreach (CoTipoDatoDTO tipoDato in listTipoDato)
                {
                    CoConfiguracionSenialDTO config = detalle.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi && x.Cotidacodi == tipoDato.Cotidacodi).FirstOrDefault();

                    string[] data = {
                                    item.Equicodi.ToString(),
                                    item.Ursnomb,
                                    (config!=null)?config.Zonacodi.ToString(): string.Empty,
                                    ConstantesCostoOportunidad.TipoDatosDesc[tipoDato.Cotidacodi  - 1],
                                    (config!=null)?config.Canalcodi.ToString(): string.Empty,
                                    (config!=null)?config.Consenvalinicial.ToString(): string.Empty
                    };

                    result.Add(data.ToList());

                    if (config != null) idZona = config.Zonacodi;
                }
                ConfiguracionURSData itemCanal = new ConfiguracionURSData();
                itemCanal.Indice = indice;
                itemCanal.Data = new List<Dominio.DTO.Scada.TrCanalSp7DTO>();
                if (idZona != null)
                {
                    itemCanal.Data = (new ScadaSp7AppServicio()).GetByZonaTrCanalSp7((int)idZona);
                }
                resultCanal.Add(itemCanal);
                indice++;
            }

            entity.DataSeniales = result;
            entity.DataCanales = resultCanal;
            return entity;
        }
                

        /// <summary>
        /// Permite grabar la configuración de la URS
        /// </summary>
        /// <param name="idUrs"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataOperacion"></param>
        /// <param name="dataReporte"></param>
        /// <param name="dataEquipo"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int GrabarConfiguracionURS(int idUrs, int idPeriodo, int idVersion, string fechaInicio, string fechaFin,
            string[][] dataOperacion, string[][] dataReporte, string[][] dataEquipo, string usuario)
        {
            try
            {
                int id = 0;
                CoConfiguracionUrsDTO configuracion = FactorySic.GetCoConfiguracionUrsRepository().GetById(idPeriodo, idVersion, idUrs);

                CoConfiguracionUrsDTO entity = new CoConfiguracionUrsDTO();
                entity.Grupocodi = idUrs;
                entity.Copercodi = idPeriodo;
                entity.Covercodi = idVersion;
                entity.Conursfecinicio = (!string.IsNullOrEmpty(fechaInicio)) ?
                    (DateTime?)DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : null;
                entity.Conursfecfin = (!string.IsNullOrEmpty(fechaFin)) ?
                    (DateTime?)DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : null;
                entity.Conursfecmodificacion = DateTime.Now;
                entity.Conursusumodificacion = usuario;

                if (configuracion == null)
                {
                    entity.Conursfeccreacion = DateTime.Now;
                    entity.Conursusucreacion = usuario;
                    id = FactorySic.GetCoConfiguracionUrsRepository().Save(entity);
                }
                else
                {
                    entity.Conurscodi = configuracion.Conurscodi;
                    FactorySic.GetCoConfiguracionUrsRepository().Update(entity);
                    id = configuracion.Conurscodi;
                }

                List<CoConfiguracionDetDTO> configuracionDetalle = FactorySic.GetCoConfiguracionDetRepository().GetByCriteria(id);

                this.GrabarConfiguracionDetalle(id, dataOperacion, ConstantesCostoOportunidad.TipoOperacionURS,
                    configuracionDetalle.Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoOperacionURS).ToList(), usuario);
                this.GrabarConfiguracionDetalle(id, dataReporte, ConstantesCostoOportunidad.TipoReporteExtranet,
                    configuracionDetalle.Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoReporteExtranet).ToList(), usuario);
                this.GrabarConfiguracionDetalle(id, dataEquipo, ConstantesCostoOportunidad.TipoEquipoRPF,
                    configuracionDetalle.Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoEquipoRPF).ToList(), usuario);

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar la configuración detalle de una URS
        /// </summary>
        /// <param name="idConfiguracion"></param>
        /// <param name="data"></param>
        /// <param name="tipo"></param>
        /// <param name="configuracionDetalle"></param>
        /// <param name="usuario"></param>
        public void GrabarConfiguracionDetalle(int idConfiguracion, string[][] data, string tipo, List<CoConfiguracionDetDTO> configuracionDetalle,
            string usuario)
        {
            List<CoConfiguracionDetDTO> entitys = new List<CoConfiguracionDetDTO>();

            foreach (string[] item in data)
            {
                CoConfiguracionDetDTO entity = new CoConfiguracionDetDTO();
                entity.Conurscodi = idConfiguracion;
                entity.Courdeoperacion = (tipo == ConstantesCostoOportunidad.TipoOperacionURS) ? item[1] : null;
                entity.Courdereporte = (tipo == ConstantesCostoOportunidad.TipoReporteExtranet) ? item[1] : null;
                entity.Courdeequipo = (tipo == ConstantesCostoOportunidad.TipoEquipoRPF) ? item[1] : null;
                int indice = 2;
                if (tipo == ConstantesCostoOportunidad.TipoEquipoRPF) indice = 3;
                entity.Courdevigenciadesde = DateTime.ParseExact(item[indice], ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Courdevigenciahasta = (!string.IsNullOrEmpty(item[indice + 1])) ?
                    (DateTime?)DateTime.ParseExact(item[indice + 1], ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : null;
                if (tipo == ConstantesCostoOportunidad.TipoEquipoRPF)
                    entity.Courderequip = (!string.IsNullOrEmpty(item[2])) ? (decimal?)Convert.ToDecimal(item[2]) : null;
                entity.Courdecodi = (!string.IsNullOrEmpty(item[0])) ? int.Parse(item[0]) : 0;
                entity.Courdetipo = tipo;
                entitys.Add(entity);
            }

            //- Seleccionar los que se eliminarán
            List<CoConfiguracionDetDTO> listEliminar = configuracionDetalle.Where(x => !entitys.Any(y => x.Courdecodi == y.Courdecodi)).ToList();
            foreach (CoConfiguracionDetDTO item in listEliminar)
            {
                FactorySic.GetCoConfiguracionDetRepository().Delete(item.Courdecodi);
                FactorySic.GetCoConfiguracionSenialRepository().Delete(item.Courdecodi);
            }

            //- seleccionar los que actualizarán
            List<CoConfiguracionDetDTO> listActualizar = entitys.Where(x => configuracionDetalle.Any(y => y.Courdecodi == x.Courdecodi)).ToList();
            foreach (CoConfiguracionDetDTO item in listActualizar)
            {
                item.Courdefecmodificacion = DateTime.Now;
                item.Courdeusumodificacion = usuario;
                FactorySic.GetCoConfiguracionDetRepository().Update(item);
            }

            //- Seleccionar los que se insertarán
            List<CoConfiguracionDetDTO> listInsertar = entitys.Where(x => x.Courdecodi == 0).ToList();
            foreach (CoConfiguracionDetDTO item in listInsertar)
            {
                item.Courdefeccreacion = DateTime.Now;
                item.Courdeusucreacion = usuario;
                FactorySic.GetCoConfiguracionDetRepository().Save(item);
            }
        }

        /// <summary>
        /// Permite grabar la configuración de la señal
        /// </summary>
        /// <param name="idConfiguracionDet"></param>
        /// <param name="data"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int GrabarConfiguracionSenial(int idConfiguracionDet, int idUrs, string[][] data, string userName)
        {
            try
            {
                FactorySic.GetCoConfiguracionSenialRepository().Delete(idConfiguracionDet);
                int indice = 0;
                int pos = 0;
                foreach (string[] item in data)
                {
                    if (indice % 6 == 0)
                    {
                        pos = indice;
                    }

                    CoConfiguracionSenialDTO entity = new CoConfiguracionSenialDTO();
                    entity.Grupocodi = idUrs;
                    entity.Equicodi = int.Parse(item[0]);
                    entity.Courdecodi = idConfiguracionDet;
                    entity.Cotidacodi = ConstantesCostoOportunidad.TipoDatosIds[Array.IndexOf(ConstantesCostoOportunidad.TipoDatosDesc, item[3])];
                    entity.Canalcodi = int.Parse(item[4]);
                    entity.Zonacodi = int.Parse(data[pos][2]);
                    entity.Consenvalinicial = (!string.IsNullOrEmpty(item[5])) ? (decimal?)decimal.Parse(item[5]) : null;
                    entity.Consenfeccreacion = DateTime.Now;
                    entity.Consenusucreacion = userName;

                    FactorySic.GetCoConfiguracionSenialRepository().Save(entity);
                    indice++;
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar la importación de la configuración de URS
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int ImportarConfiguracionURS(int idPeriodo, int idVersion, string usuario)
        {
            try
            {
                int result = 0;

                //- Validacion de existencia de configuracioes en el periodo actual

                List<CoConfiguracionDetDTO> validacionConfiguracion = FactorySic.GetCoConfiguracionDetRepository().ObtenerConfiguracion(idPeriodo, idVersion);

                if(validacionConfiguracion.Count > 0)
                {
                    result = 4;
                    return result;
                }

                int periodoOrigen = 0;
                int versionOrigen = 0;
                List<CoPeriodoDTO> listPeriodo = this.GetByCriteriaCoPeriodos(-1);
                List<CoVersionDTO> listVersion = this.GetByCriteriaCoVersions(idPeriodo);


                List<CoVersionDTO> listVerificacionVersion = listVersion.Where(x => x.Covercodi < idVersion).OrderByDescending(x => x.Covercodi).ToList();

                if (listVerificacionVersion.Count == 0)
                {
                    List<CoPeriodoDTO> subListPeriodo = listPeriodo.Where(x => x.Copercodi < idPeriodo).OrderByDescending(x => x.Copercodi).ToList();

                    if(subListPeriodo.Count == 0)
                        result = 3;                    
                    else
                    {
                        periodoOrigen = subListPeriodo[0].Copercodi;
                        List<CoVersionDTO> subListVersion = this.GetByCriteriaCoVersions(periodoOrigen).OrderByDescending(x => x.Covercodi).ToList();

                        if(subListVersion.Count == 0)                        
                            result = 3;                       
                        else                       
                            versionOrigen = subListVersion[0].Covercodi;  
                    }
                }
                else
                {
                    periodoOrigen = idPeriodo;
                    versionOrigen = listVerificacionVersion[0].Covercodi;
                }
                
                if(result == 0)
                {
                    List<CoMedicion48DTO> listUrs = FactorySic.GetCoMedicion48Repository().ObtenerListadoURS();
                    CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);

                    bool existenciaData = false;

                    foreach(CoMedicion48DTO item in listUrs)
                    {
                        CoConfiguracionUrsDTO configuracion = FactorySic.GetCoConfiguracionUrsRepository().GetById(periodoOrigen, 
                            versionOrigen, (int)item.Grupocodi);

                        if(configuracion != null)
                        {
                            configuracion.Copercodi = idPeriodo;
                            configuracion.Covercodi = idVersion;
                            configuracion.Conursfeccreacion = DateTime.Now;

                            int idConfiguracion = FactorySic.GetCoConfiguracionUrsRepository().Save(configuracion);

                            existenciaData = true;

                            List<CoConfiguracionDetDTO> listDetalle = FactorySic.GetCoConfiguracionDetRepository().GetByCriteria(configuracion.Conurscodi);

                            List<string> tipos = listDetalle.Select(x => x.Courdetipo).Distinct().ToList();

                            foreach(string tipo in tipos)
                            {
                                CoConfiguracionDetDTO itemDetalle = listDetalle.Where(x => x.Courdetipo == tipo).
                                    OrderByDescending(x => x.Courdevigenciadesde).FirstOrDefault();

                                if(itemDetalle != null)
                                {
                                    itemDetalle.Conurscodi = idConfiguracion;
                                    itemDetalle.Courdevigenciadesde = version.Coverfecinicio;
                                    itemDetalle.Courdevigenciahasta = version.Coverfecfin;
                                    itemDetalle.Courdefeccreacion = DateTime.Now;

                                    int idConfiguracionDet = FactorySic.GetCoConfiguracionDetRepository().Save(itemDetalle);
                                    
                                    if (tipo == ConstantesCostoOportunidad.TipoOperacionURS)
                                    {
                                        List<CoConfiguracionSenialDTO> listSeniales = FactorySic.GetCoConfiguracionSenialRepository().GetByCriteria(itemDetalle.Courdecodi);
                                    
                                        foreach(CoConfiguracionSenialDTO itemSenial in listSeniales)
                                        {
                                            itemSenial.Courdecodi = idConfiguracionDet;
                                            itemSenial.Consenvalinicial = null;
                                            itemSenial.Consenfeccreacion = DateTime.Now;
                                            FactorySic.GetCoConfiguracionSenialRepository().Save(itemSenial);
                                        }
                                    }

                                    if(tipo == ConstantesCostoOportunidad.TipoReporteExtranet)
                                    {
                                        List<CoConfiguracionGenDTO> listGeneracion = FactorySic.GetCoConfiguracionGenRepository().GetByCriteria(itemDetalle.Courdecodi);

                                        foreach(CoConfiguracionGenDTO itemGen in listGeneracion)
                                        {
                                            itemGen.Courdecodi = idConfiguracionDet;
                                            itemGen.Courgefeccreacion = DateTime.Now;
                                            FactorySic.GetCoConfiguracionGenRepository().Save(itemGen);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (existenciaData) result = 1;
                    else result = 2;
                    
                }

                return result;
            }

            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar el reporte de seniales sp7 y factores de presencia
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="idTipoInformacion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public int GenerarReporteMedicion60(int idVersion, int idTipoInformacion, int idUrs, DateTime fechaInicio, DateTime fechaFin, string path, out string file)
        {
            try
            {
                //- Obtenemos detalle de las URS
                List<EveRsfdetalleDTO> configuracion = (new RsfAppServicio()).ObtenerConfiguracion(DateTime.Now);

                //- Obtenemos el listado de URS
                List<CoMedicion48DTO> listUrs = new List<CoMedicion48DTO>();
                if (idUrs != 0) 
                    listUrs = FactorySic.GetCoMedicion48Repository().ObtenerListadoURS().Where(x => x.Grupocodi == idUrs).ToList();                
                else
                    listUrs = FactorySic.GetCoMedicion48Repository().ObtenerListadoURS();               

                //- Obtenemos la versión
                CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);
                CoPeriodoDTO periodo = FactorySic.GetCoPeriodoRepository().GetById((int)version.Copercodi);
                               
                String tipo = ConstantesCostoOportunidad.ProcesoDiario;
                String nombreTabla = "CO_MEDICION60_" + periodo.Coperanio + string.Format("{0:D2}", periodo.Copermes);

                List<CoProcesoDiarioDTO> listProcesos = FactorySic.GetCoProcesoDiarioRepository().GetByCriteria(
                        tipo, (int)version.Copercodi, version.Covercodi, fechaInicio, fechaFin);

                //- Obtenenemos la configuración del periodo y versión
                List<CoConfiguracionDetDTO> configuracionURS = FactorySic.GetCoConfiguracionDetRepository().ObtenerConfiguracion((int)version.Copercodi, idVersion);

                //This creates our list of files to be added
                List<string> filesToArchive = new List<string>();

                int nroDias = (int)fechaFin.Subtract(fechaInicio).TotalDays;

                for (int i = 0; i <= nroDias; i++)
                {
                    DateTime fecha = (fechaInicio).AddDays(i);

                    string folderName = (idTipoInformacion == ConstantesCostoOportunidad.IdTipoInformacionFP) ? "" : "DatosSP7-" + fecha.ToString("ddMMyyyy");

                    if (idTipoInformacion == ConstantesCostoOportunidad.IdTipoInformacionSenialSP7)
                    {
                        if (System.IO.Directory.Exists(path + folderName))
                        {
                            System.IO.Directory.Delete(path + folderName, true);
                        }
                        System.IO.Directory.CreateDirectory(path + folderName);

                    }

                    CoProcesoDiarioDTO proceso = listProcesos.Where(x => ((DateTime)x.Prodiafecha).Year == fecha.Year &&
                        ((DateTime)x.Prodiafecha).Month == fecha.Month && ((DateTime)x.Prodiafecha).Day == fecha.Day).FirstOrDefault();

                    if (proceso != null && proceso.Prodiaestado == ConstantesCostoOportunidad.EstadoExitoso)
                    {
                        List<CoConfiguracionSenialDTO> listSeniales = new List<CoConfiguracionSenialDTO>();

                        foreach (CoMedicion48DTO urs in listUrs)
                        {
                            List<CoConfiguracionDetDTO> itemConfiguracionURS = configuracionURS.Where(x => x.Grupocodi == urs.Grupocodi && x.Courdetipo == ConstantesCostoOportunidad.TipoOperacionURS).
                                OrderBy(x => x.Courdevigenciadesde).ToList();

                            CoConfiguracionDetDTO configuracionOperacion = this.ObtenerConfiguracionPorDia(itemConfiguracionURS, version, fecha);
                            if (configuracionOperacion != null && configuracionOperacion.ContadorSenial > 0)
                            {
                                List<CoConfiguracionSenialDTO> seniales = FactorySic.GetCoConfiguracionSenialRepository().GetByCriteria(configuracionOperacion.Courdecodi).ToList();
                                listSeniales.AddRange(seniales);
                            }
                        }
                        int[] tipoDatos = (idTipoInformacion == 14) ? ConstantesCostoOportunidad.TipoDatoFPId : ConstantesCostoOportunidad.TipoDatosIds;
                        string canalcodis = string.Join(",", listSeniales.Select(n => n.Canalcodi.ToString()).ToArray());
                        string tiposdatosids = string.Join(",", tipoDatos.Select(n => n.ToString()).ToArray());
                        List<CoMedicion60DTO> data = new List<CoMedicion60DTO>();

                        if (idTipoInformacion == ConstantesCostoOportunidad.IdTipoInformacionSenialSP7)
                            data = FactorySic.GetCoMedicion60Repository().ObtenerDataReporte(proceso.Prodiacodi, canalcodis, tiposdatosids, nombreTabla);
                        else
                            data = FactorySic.GetCoMedicion60Repository().ObtenerDataReporteFP(proceso.Prodiacodi, idUrs, ConstantesCostoOportunidad.TipoDatoFPId[0], nombreTabla);

                        int indexTipoDato = 0;
                        foreach (int tipoDato in tipoDatos)
                        {
                            int tdato = tipoDato;
                            if (idTipoInformacion == ConstantesCostoOportunidad.IdTipoInformacionFP) tdato = ConstantesCostoOportunidad.TipoDatosIds[0];
                            string[][] result = new string[4 + 24 * 60 * 60][];

                            List<CoMedicion60DTO> subData = data.Where(x => x.Cotidacodi == tipoDato).ToList();
                            List<CoConfiguracionSenialDTO> listSenialesPortipo = listSeniales.Where(x => x.Cotidacodi == tdato).ToList();
                            int row = 0;
                            int column = 1;
                            result[0] = new string[1 + listSenialesPortipo.Count];
                            result[1] = new string[1 + listSenialesPortipo.Count];
                            result[2] = new string[1 + listSenialesPortipo.Count];
                            result[3] = new string[1 + listSenialesPortipo.Count];

                            result[0][0] = "Empresa";
                            result[1][0] = "Central";
                            result[2][0] = "CodAGC";
                            result[3][0] = "Fecha";
                            row = 4;

                            for (int j = 1; j <= 24; j++)
                            {
                                for (int m = 1; m <= 60; m++)
                                {
                                    for (int k = 1; k <= 60; k++)
                                    {
                                        if (column == 1)
                                        {
                                            result[row] = new string[1 + listSenialesPortipo.Count];
                                            result[row][0] = fecha.AddMinutes((j-1) * 60 + (m-1)).AddSeconds(k - 1).ToString(ConstantesAppServicio.FormatoHHmmss);

                                            for (int n = 1; n <= listSenialesPortipo.Count; n++)
                                            {
                                                result[row][n] = "0";
                                            }


                                            row++;
                                        }
                                    }
                                }
                            }

                            foreach (CoConfiguracionSenialDTO itemSenial in listSenialesPortipo)
                            {
                                EveRsfdetalleDTO itemConfig = configuracion.Where(x => x.Grupocodi == itemSenial.Grupocodi && x.Equicodi == itemSenial.Equicodi).FirstOrDefault();

                                result[0][column] = itemConfig.Emprnomb;
                                result[1][column] = itemConfig.Gruponomb;
                                result[2][column] = itemConfig.Ursnomb;
                                result[3][column] = fecha.ToString(ConstantesAppServicio.FormatoFecha);
                                row = 4;

                                List<CoMedicion60DTO> dataSenial = new List<CoMedicion60DTO>();
                                
                                if(idTipoInformacion == ConstantesCostoOportunidad.IdTipoInformacionSenialSP7)
                                    dataSenial = subData.Where(x => x.Canalcodi == itemSenial.Canalcodi).OrderBy(x => x.Comedihora).ThenBy(x => x.Comediminuto).ToList();
                                else
                                    dataSenial = subData.Where(x => x.Grupocodi == itemSenial.Grupocodi && x.Equicodi == itemSenial.Equicodi).OrderBy(x => x.Comedihora).ThenBy(x => x.Comediminuto).ToList();

                                foreach (CoMedicion60DTO item in dataSenial)
                                {
                                    for (int k = 1; k <= 60; k++)
                                    {
                                        var valor = item.GetType().GetProperty("H" + k).GetValue(item);
                                        result[row][column] = (valor != null) ? valor.ToString() : "0";

                                        row++;
                                    }
                                }
                                column++;
                            }
                            string tipoDatoName = (idTipoInformacion == ConstantesCostoOportunidad.IdTipoInformacionFP) ? ConstantesCostoOportunidad.TipoDatoFP[indexTipoDato] : 
                                ConstantesCostoOportunidad.TipoDatosDescFile[indexTipoDato];
                            indexTipoDato++;

                            //- generamos archivo con la data del array

                            string fileName = path  + folderName + "/" + tipoDatoName + "-" + fecha.ToString("ddMMyyyy") + ".csv";

                            if (System.IO.File.Exists(fileName))
                            {
                                System.IO.File.Delete(fileName);
                            }

                            using (var outFile = System.IO.File.CreateText(fileName))
                            {
                                foreach (string[] itemResult in result)
                                {
                                    outFile.WriteLine(string.Join(",", itemResult));
                                }
                            }

                            if(idTipoInformacion == ConstantesCostoOportunidad.IdTipoInformacionFP)
                            {
                                filesToArchive.Add(fileName);
                            }

                        }

                        if (idTipoInformacion == ConstantesCostoOportunidad.IdTipoInformacionSenialSP7)
                        {
                            filesToArchive.Add(folderName);
                        }
                    }

                }
                file = (idTipoInformacion == ConstantesCostoOportunidad.IdTipoInformacionFP) ? "FactorPresencia-" + fechaInicio.ToString("ddMMyyyy") + "-al-" + fechaFin.ToString("ddMMyyyy") + ".zip":
                "DatosSP7-del-" + fechaInicio.ToString("ddMMyyyy") + "-al-" + fechaFin.ToString("ddMMyyyy") + ".zip";

                if (idTipoInformacion == ConstantesCostoOportunidad.IdTipoInformacionFP)
                {
                    GeneracionZip.AddToArchive(path + file,
                           filesToArchive,
                           GeneracionZip.ArchiveAction.Replace,
                           GeneracionZip.Overwrite.IfNewer,
                           System.IO.Compression.CompressionLevel.Optimal);
                }
                if (idTipoInformacion == ConstantesCostoOportunidad.IdTipoInformacionSenialSP7)
                {
                    using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                    {
                        foreach (string sourceBlobName in filesToArchive)
                        {
                            if (sourceBlobName.Contains(":"))
                            {
                                zip.AddDirectory(sourceBlobName, sourceBlobName);
                            }
                            else
                            {
                                zip.AddDirectory(path + sourceBlobName, sourceBlobName);
                            }
                        }

                        zip.Save(path + file);
                    }
                }

                return 1;
            }
            catch(Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                file = string.Empty;
                return -1;
            }
        }

        #endregion

        #region Periodos de Programacion

        /// <summary>
        /// Devuelve el listado de periodos de programacion en html
        /// </summary>
        /// <param name="url"></param>
        /// <param name="lstPeriodos"></param>
        /// <returns></returns>
        public string GenerarHtmlPeriodosProg(string url, List<CoPeriodoProgDTO> lstPeriodos)
        {
            List<CoPeriodoProgDTO> listaPeriodosProg = lstPeriodos;

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='1' cellspacing='0' id='tabla_lstPeriodosProg' style='width:100%;'>");
            str.Append("<thead>");

            #region cabecera            

            str.Append("<tr>");
            str.Append("<th style='width: 120px;'>Acciones</th>");            
            str.Append("<th style='width: 180px;'>Fecha Vigencia</th>");            
            str.Append("<th style='width: 180px;'>Valor</th>");
            str.Append("<th style='width: 240px;'>Estado</th>");            
            str.Append("<th style='width: 180px;'>Usuario Modificación</th>");
            str.Append("<th style='width: 180px;'>Fecha Últ. Modificación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            DateTime hoy = DateTime.Now;

            foreach (var item in listaPeriodosProg)
            {
                
                bool esEliminado = (item.Perprgestado == "E") ? true : false;                
                DateTime vigencia = item.Perprgvigencia.Value;
                int resultado = DateTime.Compare(hoy.Date, vigencia.Date);
                string claseFila = esEliminado ? "fila_eliminada" : "";

                str.AppendFormat("<tr style='height: 27px'>");

                str.AppendFormat("<td class='{0}' style='width: 120px'>", claseFila);
                str.AppendFormat("<div style='width: {0}px; margin: 0 auto;'>", 45);

                if (!esEliminado)
                {
                    if (resultado <= 0)
                    {
                        str.AppendFormat("<a class='' href='JavaScript:editarPeriodoProg({0},{1},{2},{3},{4});' ><img style='margin-top: 4px; margin-bottom: 4px; cursor:pointer;' src='{5}Content/Images/btn-edit.png' title='Editar Periodo de Programación' alt='Editar Periodo de Programación' /></a>", item.Perprgcodi, item.Perprgvalor, vigencia.Day, vigencia.Month, vigencia.Year, url);
                        str.AppendFormat("<a class='' href='JavaScript:eliminarPeriodoProg({0});' ><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-cancel.png' alt='Eliminar Periodo de Programación' title='Eliminar Periodo de Programación' /></a>", item.Perprgcodi, url);
                    }
                }

                str.Append("</div>");
                str.Append("</td>");
                 
                str.AppendFormat("<td class='{0}' style='width: 180px; text-align: center'>{1}</td>", claseFila, item.PerprgvigenciaDesc);                
                str.AppendFormat("<td class='{0}' style='width: 180px; text-align: center'>{1}</td>", claseFila, item.Perprgvalor);
                str.AppendFormat("<td class='{0}' style='width: 240px; text-align: center'>{1}</td>", claseFila, item.PerprgestadoDesc);
                str.AppendFormat("<td class='{0}' style='width: 180px; text-align: center'>{1}</td>", claseFila, item.Perprgusumodificacion);
                str.AppendFormat("<td class='{0}' style='width: 180px; text-align: center'>{1}</td>", claseFila, item.Perprgfecmodificacion);
                str.Append("</tr>");

            }
            str.Append("</tbody>");

            return str.ToString();
        }

        /// <summary>
        /// Devuelve todo el listado de periodos de programacion
        /// </summary>
        /// <returns></returns>
        public List<CoPeriodoProgDTO> ListarTodosPeriodosDeProgramacion()
        {
            List<CoPeriodoProgDTO> lstSalida = ListCoPeriodoProgs();
            foreach (var reg in lstSalida)
                this.FormatearPeriodoProg(reg);

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el listado de periodos de programacion activos
        /// </summary>
        /// <returns></returns>
        public List<CoPeriodoProgDTO> ListarPeriodosDeProgramacionActivos()
        {
            List<CoPeriodoProgDTO> lstSalida = ListarTodosPeriodosDeProgramacion().Where(x=>x.Perprgestado == "A").ToList();
            
            return lstSalida;
        }

        /// <summary>
        /// Da formato a los registros de periodos de programacion
        /// </summary>
        /// <param name="reg"></param>
        private void FormatearPeriodoProg(CoPeriodoProgDTO reg)
        {
            if (reg != null)
            {
                reg.PerprgvigenciaDesc = reg.Perprgvigencia.Value.ToString(ConstantesAppServicio.FormatoFecha);
                reg.PerprgfecmodificacionDesc = reg.Perprgfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFecha);                
                reg.PerprgestadoDesc = reg.Perprgestado == "A" ? "Activo" : (reg.Perprgestado == "E" ? "Eliminado" : "");                
            }
        }

        /// <summary>
        /// valida el registro/edicion de un periodo de programacion 
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="valor"></param>
        /// <param name="vigencia"></param>
        /// <returns></returns>
        public string ValidarPeriodoProg(int accion, decimal valor, DateTime vigencia, string fechaIni)
        {
            string mensaje = "";
            List<CoPeriodoProgDTO> lstPeriodosExistentes = this.ListCoPeriodoProgs().Where(x => x.Perprgestado == "A").OrderByDescending(x => x.Perprgvigencia).ToList();            
            List<CoPeriodoProgDTO> lstPeridosMismaVigencia = lstPeriodosExistentes.Where(x => x.Perprgvigencia == vigencia).ToList();

            if (lstPeridosMismaVigencia.Any())
            {
                CoPeriodoProgDTO periodoDuplicado = lstPeridosMismaVigencia.Find(x => x.Perprgvalor == valor);

                if (accion == ConstantesCostoOportunidad.AccionCrear)
                {
                    if(periodoDuplicado != null)
                        mensaje = "Ya existe un periodo de programación con los mismos datos.";
                }
                    
                if (accion == ConstantesCostoOportunidad.AccionEditar)
                {
                    DateTime fecIni = DateTime.ParseExact(fechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    if (lstPeridosMismaVigencia.Count == 1 )
                    {
                        if (periodoDuplicado != null)
                        {
                            int result = DateTime.Compare(vigencia.Date, fecIni.Date);
                            if (result == 0)
                            {
                                mensaje = "Se intentó actualizar los datos sin  realizar ningún cambio.";
                            }
                            else
                            {
                                mensaje = "Ya existe un periodo de programación con los mismos datos.";
                            }
                        }
                    }
                    else
                    {
                        mensaje = "Ya existe un periodo de programación con la misma vigencia, debe editar dicho registro.";
                    }
                }

            }
                
            return mensaje;
        }

        /// <summary>
        /// Guarda un registro de periodo de programacion
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="valor"></param>
        /// <param name="vigencia"></param>
        /// <param name="userName"></param>
        public void RegistrarPeriodosProg(int accion, int? idPeriodo, decimal valor, DateTime vigencia, string userName)
        {
            try
            {
                CoPeriodoProgDTO entity = new CoPeriodoProgDTO();
                 
                if (accion == ConstantesCostoOportunidad.AccionCrear)
                {
                    entity.Perprgvigencia = vigencia;
                    entity.Perprgvalor = valor;
                    entity.Perprgestado = "A";
                    entity.Perprgusucreacion = userName;
                    entity.Perprgfeccreacion = DateTime.Now;
                    entity.Perprgusumodificacion = userName;
                    entity.Perprgfecmodificacion = DateTime.Now;

                    FactorySic.GetCoPeriodoProgRepository().Save(entity);
                }
                else
                {
                    if (accion == ConstantesCostoOportunidad.AccionEditar)
                    {
                        entity = FactorySic.GetCoPeriodoProgRepository().GetById(idPeriodo.Value);
                        entity.Perprgvalor = valor;
                        entity.Perprgvigencia = vigencia;
                        entity.Perprgusumodificacion = userName;
                        entity.Perprgfecmodificacion = DateTime.Now;

                        FactorySic.GetCoPeriodoProgRepository().Update(entity);
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
        /// Elimina un registro de periodo de programacion
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="userName"></param>
        public void EliminarPeriodosProg(int idPeriodo, string userName)
        {
            try
            {
                CoPeriodoProgDTO entity = new CoPeriodoProgDTO();

                entity = FactorySic.GetCoPeriodoProgRepository().GetById(idPeriodo);
                entity.Perprgestado = "E";
                entity.Perprgusumodificacion = userName;
                entity.Perprgfecmodificacion = DateTime.Now;

                FactorySic.GetCoPeriodoProgRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene los periodos de programacion y variables delta para luego verificar soincidencias entre ellos
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="version"></param>
        /// <param name="existeDiferencias"></param>
        /// <returns></returns>
        public List<DiferenciaValor> ComprobarDiferenciasPeriodosProg(int periodo, int version, out bool existeDiferencias)
        {
            List<DiferenciaValor> lstSalida = new List<DiferenciaValor>();
            CoPeriodoDTO objPeriodo = GetByIdCoPeriodo(periodo);

            //periodos de programacion
            List<DiferenciaValor> lstPeriodosProg = ObtenerListadoPeriodosProgAVerificar(objPeriodo.Coperanio.Value, objPeriodo.Copermes.Value, version, out string rango);
            if (rango != "") throw new ArgumentException("No existen periodos de programación registrados para el siguiente rango: " + rango);

            //variables delta
            List<DiferenciaValor> lstVariablesDelta = ObtenerListadoVariablesDeltaAVerificar(objPeriodo.Coperanio.Value, objPeriodo.Copermes.Value, version);

            //verificacion
            lstSalida = VerificarYObtenerDiferencias(objPeriodo.Coperanio.Value, objPeriodo.Copermes.Value, version, lstPeriodosProg, lstVariablesDelta, out existeDiferencias);       

            return lstSalida;
        }

        /// <summary>
        /// Comprueba la existencia de diferencias entre periodos de programacion y variables delta
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="version"></param>
        /// <param name="lstPeriodosProg"></param>
        /// <param name="lstVariablesDelta"></param>
        /// <param name="existeDiferencias"></param>
        /// <returns></returns>
        private List<DiferenciaValor> VerificarYObtenerDiferencias(int anio, int mes, int version, List<DiferenciaValor> lstPeriodosProg, List<DiferenciaValor> lstVariablesDelta, out bool existeDiferencias)
        {
            List<DiferenciaValor> lstSalida = new List<DiferenciaValor>();
            existeDiferencias = false;

            DateTime fechaIniMes = new DateTime(anio, mes, 1);
            int maxDia = version == 0 ? 15 : (fechaIniMes.AddMonths(1).AddDays(-1).Day);

            for (int i = 1; i <= maxDia; i++)
            {
                DiferenciaValor obj = new DiferenciaValor();
                DateTime fecha = new DateTime(anio, mes, i);

                DiferenciaValor periodoProg = lstPeriodosProg.Find(x => x.Fecha == fecha);
                DiferenciaValor variableDelta = lstVariablesDelta.Find(x => x.Fecha == fecha);

                if(periodoProg.PeriodoProg != variableDelta.VariableDelta)
                {
                    existeDiferencias = true;

                    obj.Fecha = fecha;
                    obj.FechaDesc = fecha.ToString(ConstantesAppServicio.FormatoFecha);
                    obj.PeriodoProg = periodoProg.PeriodoProg;
                    obj.VariableDelta = variableDelta.VariableDelta;

                    lstSalida.Add(obj);
                }
            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el listado de periodos de programacion para ser verificados con las variables delta
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="version"></param>
        /// <param name="rango"></param>
        /// <returns></returns>
        private List<DiferenciaValor> ObtenerListadoPeriodosProgAVerificar(int anio, int mes, int version, out string rango)
        {
            List<DiferenciaValor> lstSalida = new List<DiferenciaValor>();
            List<CoPeriodoProgDTO> lstPeriodosTotales = ListarPeriodosDeProgramacionActivos();
            List<DateTime> lstFechasSinPeriodos = new List<DateTime>();
            rango = "";
            
            if (lstPeriodosTotales.Any())
            {
                DateTime fechaIniMes = new DateTime(anio, mes, 1);
                int maxDia = version == 0 ? 15 : (fechaIniMes.AddMonths(1).AddDays(-1).Day);

                //para cada dia
                for (int i = 1; i <= maxDia; i++)
                {
                    DiferenciaValor obj = new DiferenciaValor();
                    DateTime fecha = new DateTime(anio, mes, i);
                    List<CoPeriodoProgDTO> lstPer = lstPeriodosTotales.Where(x => x.Perprgvigencia <= fecha).ToList();

                    CoPeriodoProgDTO pp = null;
                    if (lstPer.Any())
                    {
                         pp = lstPer.First();
                    }
                    else
                    {
                        lstFechasSinPeriodos.Add(fecha);
                    }
                    
                    obj.Fecha = fecha;
                    obj.FechaDesc = fecha.ToString(ConstantesAppServicio.FormatoFecha);
                    obj.PeriodoProg = pp != null ? pp.Perprgvalor : null;
                    
                    lstSalida.Add(obj);
                }

                //Obtengo el rango (días) para los periodos de programacion no existentes
                if (lstFechasSinPeriodos.Any())
                {
                    if(lstFechasSinPeriodos.Count >= 2)
                    {
                        DateTime fecIni = lstFechasSinPeriodos.First();
                        DateTime fecFin = lstFechasSinPeriodos.Last();
                        
                        rango = fecIni.ToString(ConstantesAppServicio.FormatoFecha) + " al " + fecFin.ToString(ConstantesAppServicio.FormatoFecha);
                    }
                    else
                    {
                        DateTime fec = lstFechasSinPeriodos.First();

                        rango = fec.ToString(ConstantesAppServicio.FormatoFecha);
                    }
                }
            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el listado de variables delta para ser verificados con los periodos de programacion 
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private List<DiferenciaValor> ObtenerListadoVariablesDeltaAVerificar(int anio, int mes, int version)
        {
            List<DiferenciaValor> lstSalida = new List<DiferenciaValor>();

            DateTime fechaIniMes = new DateTime(anio, mes, 1);
            int maxDia = version == 0 ? 15 : (fechaIniMes.AddMonths(1).AddDays(-1).Day);

            DateTime fechaIni = new DateTime(anio, mes, 1);
            DateTime fechaFin = new DateTime(anio, mes, maxDia);

            List<CpTopologiaDTO> programadosDiario = FactorySic.GetCpTopologiaRepository().ListaTopFinalDiario(fechaIni, fechaFin);

            if (programadosDiario.Any())
            {
                //obtengo los detallesEtapa
                List<int> lstTopcodis = programadosDiario.GroupBy(x => x.Topcodi).Select(m => m.Key).ToList();
                string srtTopcodis = "";
                int num = 0;
                foreach (var topcodi in lstTopcodis)
                {
                    srtTopcodis = num == 0 ? "" : srtTopcodis + ",";
                    srtTopcodis = srtTopcodis + topcodi;
                    num++;
                }

                List<CpDetalleetapaDTO> lstDetalles = FactorySic.GetCpDetalleetapaRepository().Listar(srtTopcodis);

                //para cada dia de periodo y version
                for (int i = 1; i <= maxDia; i++)
                {
                    CpDetalleetapaDTO detalle = null;
                    DiferenciaValor obj = new DiferenciaValor();

                    DateTime fecha = new DateTime(anio, mes, i);

                    CpTopologiaDTO progDiario = programadosDiario.Find(x => x.Topfecha == fecha);

                    if(progDiario != null)
                    {
                        List<CpDetalleetapaDTO> detalles = lstDetalles.Where(x => x.Topcodi == progDiario.Topcodi).ToList();

                        if (detalles.Any())
                        {
                            detalle = detalles.First();
                        }
                    }

                    obj.Fecha = fecha;
                    obj.FechaDesc = fecha.ToString(ConstantesAppServicio.FormatoFecha);
                    obj.VariableDelta = detalle != null ? detalle.Etpdelta : null;                    

                    lstSalida.Add(obj);
                }
            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el listado en html de los dias con periodos y variables delta diferentes
        /// </summary>
        /// <param name="lstDiferencias"></param>
        /// <returns></returns>
        public string GenerarHtmlVerificacion(List<DiferenciaValor> lstDiferencias)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_Verificados' style=''>");
            str.Append("<thead>");

            #region cabecera            

            str.Append("<tr>");
            str.Append("<th style='width: 120px;'>Fecha</th>");
            str.Append("<th style='width: 180px;'>Periodo de Programación T</th>");
            str.Append("<th style='width: 180px;'>Variable Delta</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");

            foreach (var item in lstDiferencias)
            {                
                str.AppendFormat("<tr>");                                           

                str.AppendFormat("<td  style='width: 180px; text-align: center'>{0}</td>", item.FechaDesc);
                str.AppendFormat("<td  style='width: 180px; text-align: center'>{0}</td>", item.PeriodoProg);
                str.AppendFormat("<td  style='width: 180px; text-align: center'>{0}</td>", item.VariableDelta);

                str.Append("</tr>");

            }
            str.Append("</tbody>");

            return str.ToString();
        }

        /// <summary>
        /// Devuelve el rango de la verificacion en cadena
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public string ObtenerRangoVerificacion(int periodo, int version)
        {
            CoPeriodoDTO objPeriodo = GetByIdCoPeriodo(periodo);
            DateTime fechaIniRango = new DateTime(objPeriodo.Coperanio.Value, objPeriodo.Copermes.Value, 1);
            int maxDia = version == 0 ? 15 : (fechaIniRango.AddMonths(1).AddDays(-1).Day);
            DateTime fechaFinRango = new DateTime(objPeriodo.Coperanio.Value, objPeriodo.Copermes.Value, maxDia);

            StringBuilder str = new StringBuilder();
            str.AppendFormat("Fecha Inicio Versión: <b>{0}</b>,  Fecha Fin Versión: <b>{1}</b>", fechaIniRango.ToString(ConstantesAppServicio.FormatoFecha), fechaFinRango.ToString(ConstantesAppServicio.FormatoFecha));                      
            return str.ToString();
        }

        #endregion

        #region Datos SP7

        /// <summary>
        /// Devuelve el listado de la importacion en html
        /// </summary>
        /// <param name="url"></param>
        /// <param name="periodo"></param>
        /// <param name="version"></param>
        /// <param name="existeErrores"></param>
        /// <returns></returns>
        public string GenerarHtmlImportados(string url, int periodo, int version, out bool existeErrores)
        {
            List<CoProcesoDiarioDTO> listaDias = ObtenerProcesosDiariosPorPeriodoYVersion(periodo, version).Where(D=>D.Prodiatipo == "M").ToList();
            existeErrores = listaDias.Where(g => g.Prodiaestado == "F").Any();

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_lstImportados' style='width:1000px'>");
            str.Append("<thead>");

            #region cabecera            

            str.Append("<tr>");
            str.Append("<th style='width: 120px;'>Acción</th>");
            str.Append("<th style='width: 180px;'>Día</th>");
            
            str.Append("<th style='width: 180px;'>Usuario</th>");
            str.Append("<th style='width: 180px;'>Fecha Proceso</th>");
            str.Append("<th style='width: 240px;'>Estado</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");

            foreach (var item in listaDias)
            {
                bool esCorrecto = (item.Prodiaestado == "E") ? true : false;
                string strEstado = esCorrecto ? "Correcto" : "Con Errores"; 

                string claseFila = !esCorrecto ? "fila_error" : "fila_correcta";
                int ancho = 20;

                str.AppendFormat("<tr style='height: 25px'>");

                str.AppendFormat("<td style='width: 120px'>");
                str.AppendFormat("<div style='width: {0}px; margin: 0 auto;'>", ancho);

                if (!esCorrecto)
                {
                    str.AppendFormat("<a class='' href='JavaScript:descargarReporteError({0});' ><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-properties-2.png' alt='Descargar Reporte Error' title='Descargar Reporte Error' /></a>", item.Prodiacodi, url);
                }

                str.Append("</div>");
                str.Append("</td>");

                str.AppendFormat("<td  style='width: 180px; text-align: center'>{0}</td>", item.ProdiafechaDesc);
                
                str.AppendFormat("<td  style='width: 180px; text-align: center'>{0}</td>", item.Prodiausumodificacion);
                str.AppendFormat("<td  style='width: 180px; text-align: center'>{0}</td>", item.ProdiafecmodificacionDesc);
                str.AppendFormat("<td class='{0}' style='width: 240px; text-align: center'>{1}</td>", claseFila, strEstado);
                str.Append("</tr>");

            }
            str.Append("</tbody>");

            return str.ToString();
        }

        /// <summary>
        /// Devuelve todos los procesos diarios (con formato) para cierto periodo y version
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private List<CoProcesoDiarioDTO> ObtenerProcesosDiariosPorPeriodoYVersion(int periodo, int version)
        {
            List<CoProcesoDiarioDTO> lstSalida = FactorySic.GetCoProcesoDiarioRepository().Listar(periodo, version).OrderBy(x=>x.Prodiafecha).ToList();
            foreach (var reg in lstSalida)
                this.FormatearProcesoDiario(reg);

            return lstSalida;
        }

        /// <summary>
        /// Devuelve todos los procesos diarios (con formato) para cierto periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private List<CoProcesoDiarioDTO> ObtenerProcesosDiariosPorPeriodo(int periodo)
        {
            List<CoProcesoDiarioDTO> lstSalida = FactorySic.GetCoProcesoDiarioRepository().ListarByPeriodo(periodo).OrderBy(x => x.Prodiafecha).ToList();
            foreach (var reg in lstSalida)
                this.FormatearProcesoDiario(reg);

            return lstSalida;
        }

        /// <summary>
        /// Da formato a campos de registros de proceso diario
        /// </summary>
        /// <param name="reg"></param>
        private void FormatearProcesoDiario(CoProcesoDiarioDTO reg)
        {
            if (reg != null)
            {
                var tipo = reg.Prodiaindreproceso;
                reg.ProdiafechaDesc = reg.Prodiafecha != null ? reg.Prodiafecha.Value.ToString(ConstantesAppServicio.FormatoFecha) : "";
                reg.ProdiafecmodificacionDesc = reg.Prodiafecmodificacion != null ? reg.Prodiafecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : "";
                reg.ProdiaindreprocesoDesc = tipo != null ? (tipo == "A"  ? "Automático": (tipo == "M" ? "Manual" : (tipo == "R" ? "Reproceso" : ""))) : "";
            }
        }

        /// <summary>
        /// Importa las señales SP7
        /// </summary>
        /// <param name="tipoImportacion"></param>
        /// <param name="FechaDiario"></param>
        /// <param name="copercodi"></param>
        /// <param name="covercodi"></param>
        /// <param name="usuario"></param>
        /// <param name="tipo"></param>
        public void ImportarTodoSeñalesSP7(int tipoImportacion, DateTime? FechaDiario, int copercodi, int covercodi, string usuario, string tipo, out bool hayEjecucionIniciada)
        {
            hayEjecucionIniciada = false;
            CoVersionDTO version = GetByIdCoVersion(covercodi);
            CoPeriodoDTO periodo = GetByIdCoPeriodo(copercodi);

            //Buscar dias a importar
            List<int> diasAImportar = ObtenerDiasAImportar(tipoImportacion, FechaDiario, periodo, version);
            if (!diasAImportar.Any()) throw new ArgumentException("No hay dias a importar para el periodo y versión seleccionados");

            DateTime rangoIni = version.Coverfecinicio.Value;
            DateTime rangoFin = version.Coverfecfin.Value;

            //Verifico si  los dias a ejecutar se encuentran en el rango de la version
            List<int> lstDiasError = new List<int>();
            foreach (var dia in diasAImportar)
            {
                if (rangoIni.Day > dia || rangoFin.Day < dia)
                {
                    lstDiasError.Add(dia);
                }
            }
            string lstStrDiasError = String.Join(",", lstDiasError);
            if (lstDiasError.Any()) throw new ArgumentException("El/Los día(s): " + lstStrDiasError + " no estan dentro del rango de la versión vigente para el periodo (periodo: " + periodo.Copernomb + ", versión: " + version.Coverdesc + ")");


            //Verificamos si hay ejecución en curso
            bool hayEjecucionEnCurso = VerificarEjecucionEnCurso();
            if (hayEjecucionEnCurso)
            {
                hayEjecucionIniciada = true;
                return;
            }

            //Indico que hay una ejecución en curso
            ActualizarEstadoEjecucion(ConstantesCostoOportunidad.EstadoHayEjecucionEnCurso, usuario);

            //otenemos los canalCodis por periodo y version
            List<CoConfiguracionSenialDTO> lstSeniales = ListarSenialesExistentes(copercodi, covercodi);
            List<int?> lstCanalcodisTotales = lstSeniales.Where(n=>n.Canalcodi != null).Select(x => x.Canalcodi).ToList();
            if (!lstCanalcodisTotales.Any())
            {
                GrabarImportacionFallida(new List<int>(), diasAImportar, new List<TrCanalSp7DTO>(), rangoIni, rangoFin, version, periodo, usuario, tipoImportacion, ConstantesCostoOportunidad.ErrorExistenciaSeniales);

            }
            else
            {

                string lstStrCanalcodis = String.Join(",", lstCanalcodisTotales);
                List<TrCanalSp7DTO> lstCanales = ObtenerListadoDatosCanalesSP7(lstStrCanalcodis);

                //Obtenemos la data de Valores iniciales de las señales para periodos y versiones anteriores o iguales
                List<CoConfiguracionSenialDTO> lstSenialesAnterioresConValInicial = ObtenerSenialesAnterioresConValoresIniciales(periodo, lstStrCanalcodis);

                //Verifico si todas las señales tienen valor inicial para cualquier periodo/version anterior
                bool todasSenialesTienenValorInicial = VerificarValoresInicialesParaSeniales(lstCanalcodisTotales, lstSenialesAnterioresConValInicial, out List<int> lstCanalcodisSinValInicial);

                if (todasSenialesTienenValorInicial)
                {                    
                    //Obtenemos los nombres de las tablas scada (TR_FECHA_SP7) existentes
                    List<TablasTRScada> lstExistenciaTablasScada = ObtenerExistenciaTablasScada(rangoIni, rangoFin, diasAImportar);

                    //Obtenemos la data de las tablas SCADA (TR_FECHA_SP7)
                    string query = ArmarQueryParaObtenerDatosScada(lstExistenciaTablasScada, lstStrCanalcodis);
                    List<TrCanalSp7DTO> lstDatosSP7 = FactoryScada.GetTrCanalSp7Repository().GetDatosSP7(query);

                    //Obtenemos la data de Medicion60 para las 23:59 de dias anteriores                
                    List<CoMedicion60DTO> lstMedicion60DiasAnteriores = ObtenerMedicio60DelDiaAnterior(rangoIni, rangoFin, lstStrCanalcodis, diasAImportar, periodo);
                    
                    //Obtenemos lista de existencias de tablas scada y datos sp7 ingresados manualmente (excel importado)
                    List<CoDatoSenialDTO> lstIngresadaManualmente = ObtenerListaSP7ManualPorRango(rangoIni, rangoFin);
                    List<TablasTRScada> lstExistenciaTablasScadaManual = ObtenerExistenciaTablasScadaManual(rangoIni, rangoFin, diasAImportar, lstIngresadaManualmente);
                    List<TrCanalSp7DTO> lstDatosSP7Manual = ObtenerDatosSp7Manual(lstIngresadaManualmente);

                    //uno ambas informaciones de tablas scada y de lo carago manualmente (excel importado)
                    List<TablasTRScada> lstExistenciaTablasScadaFinal = UnirListaExistenciaTablas(lstExistenciaTablasScada, lstExistenciaTablasScadaManual);
                    List<TrCanalSp7DTO> lstDatosSP7Final = UnirListaDatosSp7(lstDatosSP7, lstDatosSP7Manual);

                    int numDiaAnalizados = 0;
                    //Funcion de importacion para el periodo y version
                    for (DateTime dia = rangoIni; rangoFin.CompareTo(dia) >= 0; dia = dia.AddDays(1))
                    {
                        //solo dias que se deben importar                
                        if (!(diasAImportar.Where(d => d == dia.Day).Any()))
                        {
                            continue;
                        }

                        List<CoProcesoDiarioDTO> lstProcesoDiario = new List<CoProcesoDiarioDTO>();
                        List<CoMedicion60DTO> lstMedicion60 = new List<CoMedicion60DTO>();
                        List<CoProcesoErrorDTO> lstErrores = new List<CoProcesoErrorDTO>();

                        //Obtenemos informacion para el dia
                        List<TablasTRScada> lstExistenciaTablasScadaXDia = lstExistenciaTablasScadaFinal.Where(x => x.FechaDia.Date == dia.Date).ToList();
                        List<TrCanalSp7DTO> lstDatosSP7XDia = lstDatosSP7Final.Where(x => x.Canalfhora2.Value.Date == dia.Date).ToList();
                        List<CoMedicion60DTO> lstMedicion60XDiaAnterior = lstMedicion60DiasAnteriores.Where(x => x.Fecha.Value.Date == (dia.Date).AddDays(-1)).ToList();

                        ImportarDatosSP7PorDia(tipoImportacion, version, dia, lstExistenciaTablasScadaXDia, lstDatosSP7XDia, lstMedicion60XDiaAnterior, lstSenialesAnterioresConValInicial, lstCanalcodisTotales, lstCanales, out List<ErrorScada> listaErroresDiario, out List<Medicion60XSegundo> listaMedicion60DiariaAGuardar);

                        //Formateamos mediciones60 y errores parciales (por día)
                        List<CoMedicion60DTO> lstM60Parcial = new List<CoMedicion60DTO>();
                        List<CoProcesoErrorDTO> lstErrParcial = new List<CoProcesoErrorDTO>();
                        lstM60Parcial = FormatearRegistrosMedicion60(listaMedicion60DiariaAGuardar, lstSeniales, version, tipoImportacion);
                        lstErrParcial = FormatearRegistrosErrores(listaErroresDiario, usuario);
                        lstMedicion60.AddRange(lstM60Parcial); //lstM60Parcial tienen origen = Med60Calculado, usado para cuando se busque en Med60 del "dia anterior"
                        lstErrores.AddRange(lstErrParcial);

                        //- Obtenemos los factores de presencia y utilización
                        string tipoProceso = (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo || tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_DiasFaltantes) ? "M" : (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario ? "D" : "");
                        List<CoMedicion60DTO> listFactores = this.CalcularFactoresPresencia(lstM60Parcial, tipoProceso, copercodi, covercodi, dia);
                        lstMedicion60.AddRange(listFactores);
                        //- Fin calculo de factores de presencia y utilización

                        //Agregamos registro recien calculados Med60 (solo de la hora 23:59) para usar en calculos de dias posteriores 
                        List<CoMedicion60DTO> lstMed60DeUltMinutos = ObtenerUltimoMinutoMedicion60(lstM60Parcial);
                        lstMedicion60DiasAnteriores.AddRange(lstMed60DeUltMinutos);

                        //Creamos objeto ProcesoDiario
                        CoProcesoDiarioDTO objProceso = new CoProcesoDiarioDTO();

                        objProceso.Prodiafecha = dia.Date;
                        objProceso.Copercodi = copercodi;
                        //objProceso.Perprgcodi  //No se le envía
                        objProceso.Covercodi = covercodi;
                        objProceso.Prodiaindreproceso = tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario ? tipo : "";
                        objProceso.Prodiatipo = (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo || tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_DiasFaltantes) ? "M" : (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario ? "D" : "");
                        objProceso.Prodiaestado = listaErroresDiario.Any() ? "F" : "E";
                        objProceso.Prodiausucreacion = usuario;
                        objProceso.Prodiafeccreacion = DateTime.Now;
                        objProceso.Prodiausumodificacion = usuario;
                        objProceso.Prodiafecmodificacion = DateTime.Now;

                        lstProcesoDiario.Add(objProceso);

                        numDiaAnalizados++;

                        //Obtengo el dia analizado
                        List<int> diaImportado = diasAImportar.Where(x => x == dia.Day).ToList();

                        EliminarDataAntiguaYGrabarDataActualizadaPorDia(numDiaAnalizados, tipoImportacion, lstProcesoDiario, lstMedicion60, lstErrores, version, periodo, diaImportado);
                    }

                }
                else
                {
                    GrabarImportacionFallida(lstCanalcodisSinValInicial, diasAImportar, lstCanales, rangoIni, rangoFin, version, periodo, usuario, tipoImportacion, ConstantesCostoOportunidad.ErrorValoresIniciales);
                }
            }

            //Indico que terminó la ejecución en curso
            ActualizarEstadoEjecucion(ConstantesCostoOportunidad.EstadoNoHayEjecucionEnCurso, usuario);
        }

        /// <summary>
        /// Une los elementos de los datos sp7 de las tablas scada con lo cargado manualmente, donde lo cargado manualmente tiene prioridad en caso haya coincidencia
        /// </summary>
        /// <param name="lstDatosSP7"></param>
        /// <param name="lstDatosSP7Manual"></param>
        /// <returns></returns>
        private List<TrCanalSp7DTO> UnirListaDatosSp7(List<TrCanalSp7DTO> lstDatosSP7, List<TrCanalSp7DTO> lstDatosSP7Manual)
        {
            List<TrCanalSp7DTO> lstSalida = new List<TrCanalSp7DTO>();
            List<TrCanalSp7DTO> lstReemplazantes = new List<TrCanalSp7DTO>();
            List<TrCanalSp7DTO> lstAgregados = new List<TrCanalSp7DTO>();

            foreach (var item in lstDatosSP7Manual)
            {
                int canalcodi = item.Canalcodi;
                DateTime fecha = item.Canalfhora2.Value;
                decimal? valor = item.Canalvalor;

                int dia = fecha.Day;
                int hora = fecha.Hour;
                int minuto = fecha.Minute;
                int segundo = fecha.Second;


                //para cada elemento busco si se encuentra en la lista oficial (lstDatosSP7), si esta entonces reemplazo, sino agrego
                TrCanalSp7DTO objIgual = lstDatosSP7.Find(x => x.Canalcodi == canalcodi && x.Canalfhora2.Value.Day == dia && x.Canalfhora2.Value.Hour == hora &&
                                                        x.Canalfhora2.Value.Minute == minuto && x.Canalfhora2.Value.Second == segundo);
                //si existe igual, elimino del principal
                if(objIgual != null)
                {
                    TrCanalSp7DTO valReemplazo = new TrCanalSp7DTO();
                    valReemplazo.Canalcodi = objIgual.Canalcodi;
                    valReemplazo.Canalfhora2 = objIgual.Canalfhora2;
                    valReemplazo.Canalvalor = valor; //reemplazo el valor con lo cargado manualmente
                    lstReemplazantes.Add(valReemplazo);

                    lstDatosSP7.Remove(objIgual);
                }
                else
                {
                    //si no existe en el principal, agrego el objeto cargado manualmente
                    TrCanalSp7DTO valAgregado = new TrCanalSp7DTO();
                    valAgregado.Canalcodi = canalcodi;
                    valAgregado.Canalfhora2 = fecha;
                    valAgregado.Canalvalor = valor; 
                    lstAgregados.Add(valAgregado);
                }                
            }

            lstSalida.AddRange(lstDatosSP7);
            lstSalida.AddRange(lstReemplazantes);
            lstSalida.AddRange(lstAgregados);

            return lstSalida;
        }

        /// <summary>
        /// Une las listas de existencia de tablas 
        /// </summary>
        /// <param name="lstExistenciaTablasScada"></param>
        /// <param name="lstExistenciaTablasScadaManual"></param>
        /// <returns></returns>
        private List<TablasTRScada> UnirListaExistenciaTablas(List<TablasTRScada> lstExistenciaTablasScada, List<TablasTRScada> lstExistenciaTablasScadaManual)
        {
            List<TablasTRScada> lstSalida = new List<TablasTRScada>();
            List<TablasTRScada> lstTemp = new List<TablasTRScada>();
            List<TablasTRScada> lstReemplazantes = new List<TablasTRScada>();
            List<TablasTRScada> lstAgregados = new List<TablasTRScada>();

            foreach (var item in lstExistenciaTablasScadaManual)
            {
                string existe = item.Existe;
                DateTime dia = item.FechaDia;
                string nombreTabla = item.NombreTabla;

                TablasTRScada objIgual = lstExistenciaTablasScada.Find(x => x.Existe == existe && x.FechaDia == dia && x.NombreTabla == nombreTabla);

                
                if (objIgual == null)
                {
                    //si no existe en el principal, agrego el objeto cargado manualmente solo si EXISTE = Y
                    if (item.Existe == "Y")
                    {
                        TablasTRScada valAgregado = new TablasTRScada();
                        valAgregado.Existe = existe;
                        valAgregado.FechaDia = dia;
                        valAgregado.NombreTabla = nombreTabla;
                        lstAgregados.Add(valAgregado);
                    }
                }
                

            }

            lstSalida.AddRange(lstExistenciaTablasScada);            
            lstSalida.AddRange(lstAgregados);

            return lstSalida;
        }

        /// <summary>
        /// Devuelve la data ingresada manualmente en formato TrCanalSp7
        /// </summary>
        /// <param name="lstIngresadaManualmente"></param>
        /// <returns></returns>
        private List<TrCanalSp7DTO> ObtenerDatosSp7Manual(List<CoDatoSenialDTO> lstIngresadaManualmente)
        {
            List<TrCanalSp7DTO> lstSalida = new List<TrCanalSp7DTO>();

            foreach (var item in lstIngresadaManualmente)
            {
                TrCanalSp7DTO obj = new TrCanalSp7DTO();
                obj.Canalcodi = item.Canalcodi.Value;
                obj.Canalfhora2 = item.Codasefechahora;
                obj.Canalvalor = item.Codasevalor;

                lstSalida.Add(obj);
            }
            
            return lstSalida;
        }

        /// <summary>
        /// Devuelve la informacion ingresada manualmente por rango
        /// </summary>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        /// <returns></returns>
        private List<CoDatoSenialDTO> ObtenerListaSP7ManualPorRango(DateTime rangoIni, DateTime rangoFin)
        {
            string fecIni = rangoIni.ToString(ConstantesBase.FormatoFecha);
            string fecFin = rangoFin.ToString(ConstantesBase.FormatoFecha);

            List<CoDatoSenialDTO> lstSalida = new List<CoDatoSenialDTO>();
            lstSalida = FactorySic.GetCoDatoSenialRepository().ObtenerListaPorFechas(fecIni, fecFin);

            return lstSalida.OrderBy(x=>x.Codasefechahora).ThenBy(x=>x.Canalcodi).ToList();
        }


        /// <summary>
        /// Elimina la data de las tablas y las reemplaza por la nueva informacion
        /// </summary>
        /// <param name="numDiaAnalizados"></param>
        /// <param name="tipoImportacion"></param>
        /// <param name="procesoDiario"></param>
        /// <param name="lstMedicion60"></param>
        /// <param name="lstErrores"></param>
        /// <param name="version"></param>
        /// <param name="periodo"></param>
        /// <param name="diaAImportado"></param>
        private void EliminarDataAntiguaYGrabarDataActualizadaPorDia(int numDiaAnalizados, int tipoImportacion, List<CoProcesoDiarioDTO> procesoDiario, List<CoMedicion60DTO> lstMedicion60, List<CoProcesoErrorDTO> lstErrores, CoVersionDTO version, CoPeriodoDTO periodo, List<int> diaAImportado)
        {
            string prodiasCodis = string.Empty;
            //Si Prodiatipo es Mensual, Eliminamos registros previos (ProcesoDiario, Medicion60 y ProcesoError) para el periodo y version
            EliminarDatosPreviosEnTablas(numDiaAnalizados, tipoImportacion, periodo, version, diaAImportado, out prodiasCodis);
            int maxIdMed60 = -1;

            //Completar comendicodis a los registros de Medicion60, para grabarlos  por bloques
            if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo || tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_DiasFaltantes)
            {
                
                string nombTabla = "CO_MEDICION60_" + periodo.Coperanio + string.Format("{0:D2}", periodo.Copermes) + "_" + version.Covebacodi;
                maxIdMed60 = FactorySic.GetCoMedicion60Repository().ObtenerIdInicial(nombTabla);
            }
            else
            {

                if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario)
                {
                    string nombTabla = "CO_MEDICION60_" + periodo.Coperanio + string.Format("{0:D2}", periodo.Copermes);
                    maxIdMed60 = FactorySic.GetCoMedicion60Repository().ObtenerIdInicial(nombTabla);
                }

            }
            List<CoMedicion60DTO> lstMed60FinalXDia = CompletarDatosMedicion60(lstMedicion60, maxIdMed60);

            //Completar proerrcodis a los registros de CoProcesoError,  para grabarlos  por bloques
            int maxIdError = FactorySic.GetCoProcesoErrorRepository().GetMaximoID();
            List<CoProcesoErrorDTO> lstErrorFinalXDia = CompletarDatosCoProcesoError(lstErrores, maxIdError);

            //Guardar toda la informacion de la importacion                        
            this.GuardarImportacionSP7XDia(procesoDiario, lstMed60FinalXDia, lstErrorFinalXDia, version, periodo, tipoImportacion, prodiasCodis);
        }

        /// <summary>
        /// Verifica la existencia de ejecuciones en curso
        /// </summary>
        /// <returns></returns>
        public bool VerificarEjecucionEnCurso()
        {
            bool salida = true;
            CoProcesoDiarioDTO procDiarioFlag = GetByIdCoProcesoDiario(ConstantesCostoOportunidad.IdRegistroBandera);
            string estado = procDiarioFlag.Prodiaestado;
            if (estado == ConstantesCostoOportunidad.EstadoHayEjecucionEnCurso)
                salida = true;
            if (estado == ConstantesCostoOportunidad.EstadoNoHayEjecucionEnCurso)
                salida = false;
            return salida;
        }

        /// <summary>
        /// Registra el estado de la ejecucion, evita ejecutar ejecuciones en paralelo
        /// </summary>
        /// <param name="estado"></param>
        private void ActualizarEstadoEjecucion(string estado, string usuario)
        {
            CoProcesoDiarioDTO procesoFlag = new CoProcesoDiarioDTO();
            procesoFlag.Prodiacodi = ConstantesCostoOportunidad.IdRegistroBandera;
            procesoFlag.Prodiaestado = estado;
            procesoFlag.Prodiafecmodificacion = DateTime.Now;
            procesoFlag.Prodiausumodificacion = usuario;
            UpdateCoProcesoDiario(procesoFlag);
        }

        /// <summary>
        /// Graba los procesos diarios fallidos, donde se deben mostrar sus reportes de errores
        /// </summary>
        /// <param name="lstCanalcodisSinValInicial"></param>
        /// <param name="diasAImportar"></param>
        /// <param name="lstCanales"></param>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        /// <param name="version"></param>
        /// <param name="periodo"></param>
        /// <param name="usuario"></param>
        /// <param name="tipoImportacion"></param>
        private void GrabarImportacionFallida(List<int> lstCanalcodisSinValInicial, List<int> diasAImportar, List<TrCanalSp7DTO> lstCanales, DateTime rangoIni, DateTime rangoFin, CoVersionDTO version, CoPeriodoDTO periodo, string usuario, int tipoImportacion, int tipoError)
        {            
            int numDiaAnalizados = 0;
            //Obtengo la informacion a guardar
            for (DateTime dia = rangoIni; rangoFin.CompareTo(dia) >= 0; dia = dia.AddDays(1))
            {
                //solo dias que se deben importar (Todos, Dias faltantes o Diarios)             
                if (!(diasAImportar.Where(d => d == dia.Day).Any()))
                {
                    continue;
                }

                List<CoProcesoDiarioDTO> lstProcesoDiario = new List<CoProcesoDiarioDTO>();
                List<CoProcesoErrorDTO> lstErrores = new List<CoProcesoErrorDTO>();
                List<CoMedicion60DTO> lstMed60 = new List<CoMedicion60DTO>();
                List<CoProcesoErrorDTO> lstErrParcial = new List<CoProcesoErrorDTO>();

                if (tipoError == ConstantesCostoOportunidad.ErrorValoresIniciales)
                    lstErrParcial = ObtenerErroresDeValIniciales(dia, lstCanalcodisSinValInicial, lstCanales, usuario, version, tipoImportacion);

                if (tipoError == ConstantesCostoOportunidad.ErrorExistenciaSeniales)
                    lstErrParcial = ObtenerErroresDeExistenciaSeniales(dia, usuario, tipoImportacion, version, periodo);

                lstErrores.AddRange(lstErrParcial);

                //Creamos objeto ProcesoDiario
                CoProcesoDiarioDTO objProceso = new CoProcesoDiarioDTO();
                
                objProceso.Prodiafecha = dia.Date;
                objProceso.Copercodi = periodo.Copercodi;
                objProceso.Covercodi = version.Covercodi;
                objProceso.Prodiatipo = (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo || tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_DiasFaltantes) ? "M" : (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario ? "D" : "");
                objProceso.Prodiaestado = lstErrParcial.Any() ? "F" : "E";
                objProceso.Prodiausucreacion = usuario;
                objProceso.Prodiafeccreacion = DateTime.Now;
                objProceso.Prodiausumodificacion = usuario;
                objProceso.Prodiafecmodificacion = DateTime.Now;

                lstProcesoDiario.Add(objProceso);
                
                numDiaAnalizados++;

                //Obtengo el dia analizado
                List<int> diaImportado = diasAImportar.Where(x => x == dia.Day).ToList();
                EliminarDataAntiguaYGrabarDataActualizadaPorDia(numDiaAnalizados, tipoImportacion, lstProcesoDiario, lstMed60, lstErrores, version, periodo, diaImportado);
            }
                        
        }

        /// <summary>
        /// Obtiene el listado de errores (Sin Valores iniciales) por dia
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="lstCanalcodisSinValInicial"></param>
        /// <param name="lstCanales"></param>
        /// <param name="usuario"></param>
        /// <param name="version"></param>
        /// <param name="tipoImportacion"></param>
        /// <returns></returns>
        private List<CoProcesoErrorDTO> ObtenerErroresDeValIniciales(DateTime dia, List<int> lstCanalcodisSinValInicial, List<TrCanalSp7DTO> lstCanales, string usuario, CoVersionDTO version, int tipoImportacion)
        {
            List<CoProcesoErrorDTO> lstSalida = new List<CoProcesoErrorDTO>();

            List<CoConfiguracionSenialDTO> lstdoSenialesActivosXDia = ObtenerListadoCanalcodisUsadosXDia(dia.Date, tipoImportacion, version);
            List<int> canalcodisActivosEnElDia = lstdoSenialesActivosXDia.GroupBy(x => x.Canalcodi.Value).Select(x => x.Key).ToList();

            foreach (var canalcodiSinVI in lstCanalcodisSinValInicial)
            {
                List<int> lstExistencia = canalcodisActivosEnElDia.Where(x => x == canalcodiSinVI).ToList();

                if (lstExistencia.Any())                                    
                {
                    CoProcesoErrorDTO objErr = new CoProcesoErrorDTO();

                    objErr.Proerrmsg = "La señal: " + lstCanales.Find(d => d.Canalcodi == canalcodiSinVI).Canalnomb.Trim() + " (canalcodi: " + canalcodiSinVI + ") no tiene valores iniciales para periodos anteriores.";
                    objErr.Proerrtipo = "S";
                    objErr.Proerrusucreacion = usuario;
                    objErr.Proerrfeccreacion = DateTime.Now;
                    objErr.Fecha = dia;

                    lstSalida.Add(objErr);
                }
            }
            

            return lstSalida;
        }

        /// <summary>
        /// Obtiene el listado de errores (Si no encuentra seniales) por dia
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="usuario"></param>
        /// <param name="tipoImportacion"></param>
        /// <param name="version"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private List<CoProcesoErrorDTO> ObtenerErroresDeExistenciaSeniales(DateTime dia, string usuario, int tipoImportacion, CoVersionDTO version, CoPeriodoDTO periodo)
        {
            List<CoProcesoErrorDTO> lstSalida = new List<CoProcesoErrorDTO>();
            
            string msg = "";
            if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo || tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_DiasFaltantes)
            {
                msg = "No se encontró Configuración URS para el periodo y version escogidos (periodo: " + periodo.Copernomb + " y versión: " + version.Coverdesc + ").";
            }
                

            if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario)
            {
                msg = "No se encontró Configuración URS (periodo: " + periodo.Copernomb + " y versión:" + version.Coverdesc + ") para el día evaludado.";
            }
               

            CoProcesoErrorDTO objErr = new CoProcesoErrorDTO();
            objErr.Proerrmsg = msg;
            objErr.Proerrtipo = "S";
            objErr.Proerrusucreacion = usuario;
            objErr.Proerrfeccreacion = DateTime.Now;
            objErr.Fecha = dia;

            lstSalida.Add(objErr);


            return lstSalida;
        }        

        /// <summary>
        /// Verifica si todas las seniales tienen valor inicial
        /// </summary>
        /// <param name="lstCanalcodisExistentes"></param>
        /// <param name="lstSenialesAnterioresConValInicial"></param>
        /// <param name="lstSenialesSinValInicial"></param>
        /// <returns></returns>
        private bool VerificarValoresInicialesParaSeniales(List<int?> lstCanalcodisExistentes, List<CoConfiguracionSenialDTO> lstSenialesAnterioresConValInicial, out List<int> lstSenialesSinValInicial)
        {
            bool salida = true;
            lstSenialesSinValInicial = new List<int>();

            foreach (var canalcodi in lstCanalcodisExistentes)
            {
                List<CoConfiguracionSenialDTO> lstTemp = lstSenialesAnterioresConValInicial.Where(s => s.Canalcodi == canalcodi).ToList();

                if (!lstTemp.Any())
                {
                    lstSenialesSinValInicial.Add(canalcodi.Value);
                    salida = false;
                }
            }

            return salida;
        }

        /// <summary>
        /// Obtiene datos de los canales
        /// </summary>
        /// <param name="lstStrCanalcodis"></param>
        /// <returns></returns>
        private List<TrCanalSp7DTO> ObtenerListadoDatosCanalesSP7(string lstStrCanalcodis)
        {
            List<TrCanalSp7DTO> lstSalida = FactoryScada.GetTrCanalSp7Repository().GetByIds(lstStrCanalcodis);            

            return lstSalida;
        }

        /// <summary>
        /// Obtengo los dias que se debera importar segun si es Mensual (Dias Faltantes, todo) o Diario
        /// </summary>
        /// <param name="tipoImportacion"></param>
        /// <param name="fechaDiario"></param>
        /// <param name="periodo"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private List<int> ObtenerDiasAImportar(int tipoImportacion, DateTime? fechaDiario, CoPeriodoDTO periodo, CoVersionDTO version)
        {
            List<int> lstSalida = new List<int>();

            if(tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo)
            {
                DateTime rangoIni = version.Coverfecinicio.Value;
                DateTime rangoFin = version.Coverfecfin.Value;
                for (DateTime dia = rangoIni; rangoFin.CompareTo(dia) >= 0; dia = dia.AddDays(1))
                {
                    lstSalida.Add(dia.Day);
                }
            }

            if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_DiasFaltantes)
            {
                List<CoProcesoDiarioDTO> listaDiasConEstadoFallido = ObtenerProcesosDiariosPorPeriodoYVersion(periodo.Copercodi, version.Covercodi).Where(e => e.Prodiaestado == "F").ToList();
                foreach (var item in listaDiasConEstadoFallido)
                {
                    lstSalida.Add(item.Prodiafecha.Value.Day);
                }
            }

            if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario)
            {
                lstSalida.Add(fechaDiario.Value.Day);
            }

            return lstSalida.OrderBy(x=>x).ToList();
        }

        /// <summary>
        /// Obtiene las mediciones60 del ultimo minuto del dia (23:59) para todos los canalcodis
        /// </summary>
        /// <param name="lstM60Parcial"></param>
        /// <returns></returns>
        private List<CoMedicion60DTO> ObtenerUltimoMinutoMedicion60(List<CoMedicion60DTO> lstM60Parcial)
        {
            List<CoMedicion60DTO> lstSalida = new List<CoMedicion60DTO>();

            lstSalida = lstM60Parcial.Where(x => x.Comedihora == 23 && x.Comediminuto == 59).ToList();
            
            return lstSalida;
        }
                

        /// <summary>
        /// Elimina la data previa antes de volver a guardarla
        /// </summary>
        /// <param name="numDiaAnalizados"></param>
        /// <param name="tipoImportacion"></param>
        /// <param name="periodo"></param>
        /// <param name="version"></param>
        /// <param name="diaAImportado"></param>
        private void EliminarDatosPreviosEnTablas(int numDiaAnalizados, int tipoImportacion, CoPeriodoDTO periodo, CoVersionDTO version, List<int> diaAImportado, out string procesosDiarios)
        {
            procesosDiarios = string.Empty;
            List<CoProcesoDiarioDTO> listaProcesosDiarios = new List<CoProcesoDiarioDTO>();
            List<CoProcesoDiarioDTO> listaProcesosDiariosAnteriores = new List<CoProcesoDiarioDTO>();

            //Obtenemos todos los procesos diarios para el periodo y version
            List<CoProcesoDiarioDTO> listaProcesosDiariosTemporal = ObtenerProcesosDiariosPorPeriodoYVersion(periodo.Copercodi, version.Covercodi);

            //Dependiendo el tipo de importacion, selecciono diarios y mensuales
            if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo || tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_DiasFaltantes)
                listaProcesosDiarios = listaProcesosDiariosTemporal.Where(c => c.Prodiatipo == "M").ToList();
            else
                listaProcesosDiarios = listaProcesosDiariosTemporal.Where(c => c.Prodiatipo == "D").ToList();

            //Obtenemos el procesoDiario a eliminar (dado que diaImportado tiene un solo valor)
            List<CoProcesoDiarioDTO> listaProcesosDiariosAEliminar = listaProcesosDiarios.Where(d => diaAImportado.Contains(d.Prodiafecha.Value.Day)).ToList();

            List<int> lstProdiacodis = listaProcesosDiariosAEliminar.GroupBy(x => x.Prodiacodi).Select(v => v.Key).ToList();

            
            /**Aumentar Prodiacodis que pertenezcan al periodo pero a una versión anterior
            (evita que para un mismo dia haya un procesoDiario con v quincenal y otra con version final, solo debería haber una con la version mas actual)**/
            List<CoProcesoDiarioDTO> lstProcesosAnterioresTotales = ObtenerProcesosDiariosPorPeriodo(periodo.Copercodi).Where(x=>x.Covercodi != version.Covercodi).ToList();
            //Dependiendo el tipo de importacion, selecciono diarios y mensuales
            if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo || tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_DiasFaltantes)
                listaProcesosDiariosAnteriores = lstProcesosAnterioresTotales.Where(c => c.Prodiatipo == "M").ToList();
            else
                listaProcesosDiariosAnteriores = lstProcesosAnterioresTotales.Where(c => c.Prodiatipo == "D").ToList();
            //Obtenemos el procesoDiario a eliminar (dado que diaImportado tiene un solo valor)
            List<CoProcesoDiarioDTO> listaProcesosAnterioresDiariosAEliminar = listaProcesosDiariosAnteriores.Where(d => diaAImportado.Contains(d.Prodiafecha.Value.Day)).ToList();
            List<int> lstProdiacodisAnteriores = listaProcesosAnterioresDiariosAEliminar.GroupBy(x => x.Prodiacodi).Select(v => v.Key).ToList();

            lstProdiacodis.AddRange(lstProdiacodisAnteriores);

            //Si es importacion de todo, elimino toda la tabla Medicion60 y si tiene errores, tambien elimino esas
            if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo)
            {
                string nombTabla = "CO_MEDICION60_" + periodo.Coperanio + string.Format("{0:D2}", periodo.Copermes) + "_" + version.Covebacodi;
                if(numDiaAnalizados == 1)
                    EliminarDataTablaEntera(nombTabla);

                if (lstProdiacodis.Any())
                {
                    string strProdiacodis = String.Join(",", lstProdiacodis);

                    EliminarRegistrosErrores(strProdiacodis);
                    EliminarRegistrosProcesosDiario(strProdiacodis);
                }
            }
            else
            {
                if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_DiasFaltantes)
                {
                    if (lstProdiacodis.Any())
                    {
                        string nombTabla = "CO_MEDICION60_" + periodo.Coperanio + string.Format("{0:D2}", periodo.Copermes) + "_" + version.Covebacodi;
                        string strProdiacodis = String.Join(",", lstProdiacodis);

                        EliminarRegistrosErrores(strProdiacodis);
                        EliminarRegistrosMedicion60(nombTabla, strProdiacodis);
                        EliminarRegistrosProcesosDiario(strProdiacodis);
                    }
                }
                else
                {
                    if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario)
                    {
                        if (lstProdiacodis.Any())
                        {
                            //solo elimino la data de medicion60_YYYYMM
                            string nombTabla = "CO_MEDICION60_" + periodo.Coperanio + string.Format("{0:D2}", periodo.Copermes);
                            string strProdiacodis = String.Join(",", lstProdiacodis);

                            EliminarRegistrosErrores(strProdiacodis);
                            //EliminarRegistrosMedicion60(nombTabla, strProdiacodis);
                            EliminarRegistrosFactorUtilizacion(strProdiacodis);
                            //EliminarRegistrosProcesosDiario(strProdiacodis);
                            procesosDiarios = strProdiacodis;

                        }
                        else
                        {
                            procesosDiarios = diaAImportado.Any() ? diaAImportado.First().ToString() : "";
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Limpia toda la tabla
        /// </summary>
        /// <param name="tabla"></param>
        private void EliminarDataTablaEntera(string tabla)
        {
            FactorySic.GetCoMedicion60Repository().EliminarDataTabla(tabla);
        }

        /// <summary>
        /// Eliminar todos los registros tipo Mensual del CoProcesoError
        /// </summary>
        /// <param name="strProdiacodis"></param>
        /// <returns></returns>
        public void EliminarRegistrosErrores(string strProdiacodis)
        {
            FactorySic.GetCoProcesoErrorRepository().EliminarProcesosError(strProdiacodis);
        }

        /// <summary>
        /// Eliminar todos los registros tipo Mensual del CoFactorUtilizacion
        /// </summary>
        /// <param name="strProdiacodis"></param>
        public void EliminarRegistrosFactorUtilizacion(string strProdiacodis)
        {
            FactorySic.GetCoFactorUtilizacionRepository().EliminarFactoresUtilizacion(strProdiacodis);
        }

        /// <summary>
        /// Eliminar todos los registros  del CoMedicion60
        /// </summary>
        /// <param name="nombTabla"></param>
        /// <param name="strProdiacodis"></param>
        public void EliminarRegistrosMedicion60(string nombTabla, string strProdiacodis)
        {
            FactorySic.GetCoMedicion60Repository().EliminarMedicion60XTabla(nombTabla, strProdiacodis);
        }

        /// <summary>
        /// Eliminar todos los registros tipo Mensual del coProcesoDiario
        /// </summary>
        /// <param name="strProdiacodis"></param>
        /// <returns></returns>
        public void EliminarRegistrosProcesosDiario(string strProdiacodis)
        {
            FactorySic.GetCoProcesoDiarioRepository().EliminarProcesosDiarios(strProdiacodis);
        }

        /// <summary>
        /// LLena comedicodis a todos los registros a guardar (por bloques)
        /// </summary>
        /// <param name="listaMed60"></param>
        /// <param name="maxId"></param>
        /// <returns></returns>
        private List<CoMedicion60DTO> CompletarDatosMedicion60(List<CoMedicion60DTO> listaMed60, int maxId)
        {
            List<CoMedicion60DTO> lstSalida = new List<CoMedicion60DTO>();

            foreach (var item in listaMed60)
            {
                int comedicodi = maxId++;
                item.Comedicodi = comedicodi;

                lstSalida.Add(item);
            }

            return lstSalida;
        }

        /// <summary>
        /// LLena proerrcodis a todos los registros a guardar (por bloques)
        /// </summary>
        /// <param name="listaErrores"></param>
        /// <param name="maxId"></param>
        /// <returns></returns>
        private List<CoProcesoErrorDTO> CompletarDatosCoProcesoError(List<CoProcesoErrorDTO> listaErrores, int maxId)
        {
            List<CoProcesoErrorDTO> lstSalida = new List<CoProcesoErrorDTO>();

            foreach (var item in listaErrores)
            {
                int codi = maxId++;
                item.Proerrcodi = codi;

                lstSalida.Add(item);
            }

            return lstSalida;
        }


        /// <summary>
        /// Guarda los datos de la importacion a tablas CoProcesoDiario, Medicion60 y coProcesoError
        /// </summary>
        /// <param name="procesoDiario"></param>
        /// <param name="lstMedicion60Diario"></param>
        /// <param name="lstErroresDiario"></param>
        /// <param name="version"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoImportacion"></param>
        private void GuardarImportacionSP7XDia(List<CoProcesoDiarioDTO> procesoDiario, List<CoMedicion60DTO> lstMedicion60Diario, 
            List<CoProcesoErrorDTO> lstErroresDiario, CoVersionDTO version, CoPeriodoDTO periodo, int tipoImportacion, string prodiasCodis)
        {
            int prodiacodi = 0;

            try
            {
                foreach (var procDiario in procesoDiario)
                {
                    CoProcesoDiarioDTO proceso = FactorySic.GetCoProcesoDiarioRepository().ObtenerProcesoDiario((DateTime)procDiario.Prodiafecha);
                    //Guardar Proceso Diario

                    if (proceso != null)
                    {
                        procDiario.Prodiacodi = proceso.Prodiacodi;
                        FactorySic.GetCoProcesoDiarioRepository().Update(procDiario);
                        prodiacodi = proceso.Prodiacodi;
                    }
                    else
                    {
                        prodiacodi = FactorySic.GetCoProcesoDiarioRepository().Save(procDiario);
                    }

                    //MEDICION60
                    List<CoMedicion60DTO> lstMedicion60XDia = lstMedicion60Diario.Where(x => x.Fecha.Value.Date == procDiario.Prodiafecha.Value.Date).ToList();
                    //Completamos datos
                    foreach (CoMedicion60DTO med60 in lstMedicion60XDia)
                    {
                        med60.Prodiacodi = prodiacodi;
                    }
                    if (lstMedicion60XDia.Any())
                    {
                        //Guardamos Medicion60 por bloques                        
                        GrabarMed60PorBloques(lstMedicion60XDia, version, periodo, tipoImportacion, prodiasCodis);
                    }


                    //ERRORES
                    List<CoProcesoErrorDTO> lstErroresXDia = lstErroresDiario.Where(x => x.Fecha.Value.Date == procDiario.Prodiafecha.Value.Date).ToList();
                    //Guardar Errores
                    foreach (CoProcesoErrorDTO error in lstErroresXDia)
                    {
                        error.Prodiacodi = prodiacodi;
                    }
                    if (lstErroresXDia.Any())
                    {
                        //Guardamos Errores por bloques
                        GrabarErroresPorBloques(lstErroresXDia);
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
        /// Guarda informacion de Medicion60 por bloques
        /// </summary>
        /// <param name="lstMedicion60XDia"></param>
        /// <param name="version"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoImportacion"></param>
        private void GrabarMed60PorBloques(List<CoMedicion60DTO> lstMedicion60XDia, CoVersionDTO version, CoPeriodoDTO periodo, 
            int tipoImportacion, string prodiasCodis)
        {
            //La estructura al guardar debe respetar la Columna_ID de la estructura de la tabla CO_MEDICION60_YYYYMM_IdTipo 
            try
            {
                string nombTabla = "";
                string parcial = "CO_MEDICION60_" + periodo.Coperanio + string.Format("{0:D2}", periodo.Copermes);

                if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo || tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_DiasFaltantes)
                {
                    nombTabla = parcial + "_" + version.Covebacodi;                    
                }
                else
                {
                    if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario)
                    {
                        nombTabla = parcial;
                    }    
                }
                //FactorySic.GetCoMedicion60Repository().GrabarDataXBloquesMed60(lstMedicion60XDia, nombTabla);

                string tablaTemporal = "CO_MEDICION60";
                this.EliminarDataTablaEntera(tablaTemporal);
                FactorySic.GetCoMedicion60Repository().GrabarDataXBloquesMed60(lstMedicion60XDia, tablaTemporal);
                FactorySic.GetCoMedicion60Repository().ProcesarTabla(nombTabla, prodiasCodis);

                //- Procesar datos
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda informacion de CoProcesoError por bloques
        /// </summary>
        /// <param name="lstErroresXDia"></param>
        private void GrabarErroresPorBloques(List<CoProcesoErrorDTO> lstErroresXDia)
        {            
            //La estructura al guardar debe respetar la Columna_ID de la estructura de la tabla CO_PROCESOS_ERROR 
            try
            {
                FactorySic.GetCoProcesoErrorRepository().GrabarDatosXBloques(lstErroresXDia);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Devuelve una lista de CoMedicion60DTO a partir de una lista de Medicion60XSegundo, dandole formato
        /// </summary>
        /// <param name="lstRegistrosMedicion60Parcial"></param>
        /// <param name="lstSeniales"></param>
        /// <param name="version"></param>
        /// <param name="tipoImportacion"></param>
        /// <returns></returns>
        private List<CoMedicion60DTO> FormatearRegistrosMedicion60(List<Medicion60XSegundo> lstRegistrosMedicion60Parcial, List<CoConfiguracionSenialDTO> lstSeniales, CoVersionDTO version, int tipoImportacion)
        {
            List<CoMedicion60DTO> lstSalida = new List<CoMedicion60DTO>();

            var lista = lstRegistrosMedicion60Parcial.GroupBy(x => new { x.FechaDia, x.Canalcodi, x.Hora, x.Minuto }).ToList();

            foreach (var item in lista)
            {
                CoMedicion60DTO objM60 = new CoMedicion60DTO();
               
                objM60.Cotidacodi = lstSeniales.Find(x => x.Canalcodi == item.Key.Canalcodi).Cotidacodi;
                objM60.Grupocodi = lstSeniales.Find(x => x.Canalcodi == item.Key.Canalcodi).Grupocodi;
                objM60.Equicodi = lstSeniales.Find(x => x.Canalcodi == item.Key.Canalcodi).Equicodi;
                objM60.Canalcodi = item.Key.Canalcodi;
                objM60.Comedihora = item.Key.Hora;
                objM60.Comediminuto = item.Key.Minuto;
                objM60.Fecha = item.Key.FechaDia;
                objM60.Origen = ConstantesCostoOportunidad.Med60Calculado; 
                if(tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario)
                {
                    objM60.Comeditipo = 0;
                }
                else
                {
                    objM60.Comeditipo = version.Covebacodi.Value;
                }

                foreach (var reg in item)
                {
                    int segundo = reg.Segundo;
                    object value = reg.Valor;
                    objM60.GetType().GetProperty(ConstantesAppServicio.CaracterH + (segundo + 1)).SetValue(objM60, value);
                }

                lstSalida.Add(objM60);
            }


            return lstSalida;
        }

        
        /// <summary>
        /// Devuelve una lista de CoProcesoErrorDTO a partir de una lista de ErrorScada, dandole formato
        /// </summary>
        /// <param name="lstRegistrosErroresParcial"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private List<CoProcesoErrorDTO> FormatearRegistrosErrores(List<ErrorScada> lstRegistrosErroresParcial, string usuario)
        {
            List<CoProcesoErrorDTO> lstSalida = new List<CoProcesoErrorDTO>();

            //Agrupamos por dia
            var lstGrupoErrorXFecha = lstRegistrosErroresParcial.GroupBy(x => new { x.FechaHora }).ToList();

            foreach (var grupoErroresXFecha in lstGrupoErrorXFecha)
            {                
                foreach (var regErrorScada in grupoErroresXFecha)
                {
                    CoProcesoErrorDTO objErr = new CoProcesoErrorDTO();

                    objErr.Proerrmsg = regErrorScada.Mensaje;
                    objErr.Proerrtipo = "S";
                    objErr.Proerrusucreacion = usuario;
                    objErr.Proerrfeccreacion = DateTime.Now;
                    objErr.Fecha = regErrorScada.FechaHora;//solo se necesitara el Date

                    lstSalida.Add(objErr);
                }
            }            

            return lstSalida;
        }

        /// <summary>
        /// Realiza la importacion por dia
        /// </summary>
        /// <param name="tipoImportacion"></param>
        /// <param name="version"></param>
        /// <param name="dia"></param>
        /// <param name="lstExistenciaTablasScadaXDia"></param>
        /// <param name="lstDatosSP7XDia"></param>
        /// <param name="lstMedicion60XDiaAnterior"></param>
        /// <param name="lstSenialesAnterioresConValInicial"></param>
        /// <param name="lstCanalcodisTotales"></param>
        /// <param name="lstCanales"></param>
        /// <param name="listaErroresDiario"></param>
        /// <param name="listaMedicion60DiariaAGuardar"></param>
        private void ImportarDatosSP7PorDia(int tipoImportacion, CoVersionDTO version, DateTime dia, List<TablasTRScada> lstExistenciaTablasScadaXDia, List<TrCanalSp7DTO> lstDatosSP7XDia, List<CoMedicion60DTO> lstMedicion60XDiaAnterior, List<CoConfiguracionSenialDTO> lstSenialesAnterioresConValInicial,
             List<int?> lstCanalcodisTotales, List<TrCanalSp7DTO> lstCanales, out List<ErrorScada> listaErroresDiario, out List<Medicion60XSegundo> listaMedicion60DiariaAGuardar)
        {

            listaErroresDiario = new List<ErrorScada>();  
            listaMedicion60DiariaAGuardar = new List<Medicion60XSegundo>();

            List<CoConfiguracionSenialDTO> lstdoSenialesActivosXDia = ObtenerListadoCanalcodisUsadosXDia(dia.Date, tipoImportacion, version);            
            List<int> canalcodisActivosEnElDia = lstdoSenialesActivosXDia.GroupBy(x => x.Canalcodi.Value).Select(x => x.Key).ToList();

            bool existeTablaScadaParaElDia = lstExistenciaTablasScadaXDia.Find(x => x.FechaDia == dia).Existe == "Y" ? true : false;

            //para cada canalcodi (URS)
            foreach (int canalcodiX in lstCanalcodisTotales)
            {                
                //Solo canalcodis que son admitidos en el dia                
                if (!(canalcodisActivosEnElDia.Where(c => c == canalcodiX).Any()))
                {
                    continue;
                }

                //INICIA IMPORTACION                
                decimal? valorXSegundo = null;
                
                //DATA A USAR: obtenemos informacion por canalcodi
                List<TrCanalSp7DTO> lstDatosSP7XDiaXCanalcodi = lstDatosSP7XDia.Where(x => x.Canalcodi == canalcodiX).OrderBy(m=>m.Canalfhora2).ToList();
                List<CoMedicion60DTO> lstMedicion60XDiaAnteriorXCanalcodi = lstMedicion60XDiaAnterior.Where(x => x.Canalcodi == canalcodiX).ToList();
                List<CoConfiguracionSenialDTO> lstSenialesAntConValInicialXCanalcodi = lstSenialesAnterioresConValInicial.Where(x => x.Canalcodi == canalcodiX).ToList();
                
                //Para el canalcodi se hace una busqueda en las tablas SCADA (TR_FECHA_SP7) 
                if (existeTablaScadaParaElDia && lstDatosSP7XDiaXCanalcodi.Any())
                {
                    int numRegistrosTrFechas = lstDatosSP7XDiaXCanalcodi.Count();

                    int primeraHora = lstDatosSP7XDiaXCanalcodi.First().Canalfhora2.Value.Hour;
                    int primerMinuto = lstDatosSP7XDiaXCanalcodi.First().Canalfhora2.Value.Minute;
                    int primerSegundo = lstDatosSP7XDiaXCanalcodi.First().Canalfhora2.Value.Second;
                    
                    //Si el primer registro pertenece a la hora 0 y minuto 0, es decir, FECHA_00:00:00
                    if (primeraHora == 0 && primerMinuto == 0 && primerSegundo == 0)
                    {
                        int hora = -1;
                        int minuto = -1;
                        int segundo = -1;

                        //completo al segundo registros de CoMedicion60                        
                        int n = 0;
                        bool esUltimoReg;

                        foreach (var regTrFechaSP7XCanalcodi in lstDatosSP7XDiaXCanalcodi)
                        {                            
                            n++;
                            esUltimoReg = n == numRegistrosTrFechas ? true : false;

                            CompletarAlSegundoGrupoAnteriorDesdeTablasScada(n, dia, valorXSegundo, hora, minuto, segundo, regTrFechaSP7XCanalcodi.Canalvalor, regTrFechaSP7XCanalcodi.Canalfhora2.Value.Hour, regTrFechaSP7XCanalcodi.Canalfhora2.Value.Minute, regTrFechaSP7XCanalcodi.Canalfhora2.Value.Second, canalcodiX, lstCanales, esUltimoReg, out List<ErrorScada> lstErroresParcial, out List<Medicion60XSegundo> lstMedicion60Parcial);
                            listaErroresDiario.AddRange(lstErroresParcial);
                            listaMedicion60DiariaAGuardar.AddRange(lstMedicion60Parcial);

                            valorXSegundo = regTrFechaSP7XCanalcodi.Canalvalor;
                            hora = regTrFechaSP7XCanalcodi.Canalfhora2.Value.Hour;
                            minuto = regTrFechaSP7XCanalcodi.Canalfhora2.Value.Minute;
                            segundo = regTrFechaSP7XCanalcodi.Canalfhora2.Value.Second;
                        }
                        
                    }
                    //Revisamos en otras tablas el valor que debe tomar el 00:00:00
                    else
                    {
                        decimal? valorEncontrado = BuscarValorCanalEnOtrasTablas(tipoImportacion, dia, lstMedicion60XDiaAnteriorXCanalcodi, lstSenialesAntConValInicialXCanalcodi);

                        int hora = -1;
                        int minuto = -1;
                        int segundo = -1;

                        if(valorEncontrado != null)
                        {
                            hora = 0;
                            minuto = 0;
                            segundo = 0;

                            int n = 0;
                            bool esUltimoReg;
                            valorXSegundo = valorEncontrado;

                            foreach (var regTrFechaSP7XCanalcodi in lstDatosSP7XDiaXCanalcodi)
                            {
                                //List<ErrorScada> lstErroresParcial = new List<ErrorScada>();
                                n++;
                                esUltimoReg = n == numRegistrosTrFechas ? true : false;

                                CompletarAlSegundoGrupoAnteriorDesdeTablasScada(n, dia, valorXSegundo, hora, minuto, segundo, regTrFechaSP7XCanalcodi.Canalvalor, regTrFechaSP7XCanalcodi.Canalfhora2.Value.Hour, regTrFechaSP7XCanalcodi.Canalfhora2.Value.Minute, regTrFechaSP7XCanalcodi.Canalfhora2.Value.Second, canalcodiX, lstCanales, esUltimoReg, out List<ErrorScada> lstErroresParcial, out List<Medicion60XSegundo> lstMedicion60Parcial);
                                listaErroresDiario.AddRange(lstErroresParcial);
                                listaMedicion60DiariaAGuardar.AddRange(lstMedicion60Parcial);

                                valorXSegundo = regTrFechaSP7XCanalcodi.Canalvalor;
                                hora = regTrFechaSP7XCanalcodi.Canalfhora2.Value.Hour;
                                minuto = regTrFechaSP7XCanalcodi.Canalfhora2.Value.Minute;
                                segundo = regTrFechaSP7XCanalcodi.Canalfhora2.Value.Second;
                            }
                        }
                        else
                        {
                            //Alertamos al usuario que NO EXISTEN VALORES PARA CANAL                             
                            ErrorScada oError = new ErrorScada();

                            oError.Canalcodi = canalcodiX;
                            oError.Valor = null;
                            oError.FechaHora = new DateTime(dia.Year, dia.Month, dia.Day);
                            
                            oError.Mensaje = "Dia: " + dia.ToString(ConstantesAppServicio.FormatoFecha + ". Existe información en las tablas Scada para el canalcodi: " + canalcodiX + " pero el primer registro no pertenece a 00:00:00, se buscó valor para el canal en la BD pero no se encontró.");

                            listaErroresDiario.Add(oError);
                        }

                    }
                }
                //No existen tablas scada (TR_FECHA_SP7) para el dia Y CANALCODI
                //Se busca en la tabla Medicion60del dia anterior (ultimo segundo) o en la tabla Configuracion_Senial (valor inicial)
                else
                {
                    decimal? valorEncontrado = BuscarValorCanalEnOtrasTablas(tipoImportacion, dia, lstMedicion60XDiaAnteriorXCanalcodi, lstSenialesAntConValInicialXCanalcodi);

                    if (valorEncontrado != null)
                    {
                        int hora = 0;
                        int minuto = 0;
                        int segundo = 0;
                        
                        //Guarda todo el dia (00:00:00 - 23:59:59) con el mismo valor
                        List<Medicion60XSegundo> lstMed60Parcial = CrearRegistrosCoMedicion60(dia, hora, minuto, segundo, 23, 59, 59, canalcodiX, valorEncontrado.Value);
                        listaMedicion60DiariaAGuardar.AddRange(lstMed60Parcial);                        
                    }
                    else
                    {
                        //Alertamos al usuario que NO EXISTEN VALORES PARA CANAL                             
                        ErrorScada oError = new ErrorScada();

                        oError.Canalcodi = canalcodiX;
                        oError.Valor = null;
                        oError.FechaHora = new DateTime(dia.Year, dia.Month, dia.Day);
                        oError.Mensaje = "Dia: " + dia.ToString(ConstantesAppServicio.FormatoFecha+ ". No existe información en las tablas Scada para el canalcodi: " + canalcodiX + ", tampoco se encontró valor del canal en la BD");

                        listaErroresDiario.Add(oError);
                    }
                }
            }

        }

        /// <summary>
        /// Devuelve el grupo de señales que trabajan cierto dia
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="tipoImportacion"></param>
        /// <param name="versionGral"></param>
        /// <returns></returns>
        private List<CoConfiguracionSenialDTO> ObtenerListadoCanalcodisUsadosXDia(DateTime dia, int tipoImportacion, CoVersionDTO versionGral)
        {
            List<CoConfiguracionSenialDTO> lstSalida = new List<CoConfiguracionSenialDTO>();
            CoVersionDTO versionUsar = new CoVersionDTO();
            List<CoConfiguracionDetDTO> configuracionURS = new List<CoConfiguracionDetDTO>();
            
            //Obtengo listado de configuraciones URS
            if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario)
            {
                versionUsar = FactorySic.GetCoVersionRepository().ObtenerVersionPorFecha(dia);
            }
            else
            {
                versionUsar = versionGral;
            }

            configuracionURS = (FactorySic.GetCoConfiguracionDetRepository().ObtenerConfiguracion((int)versionUsar.Copercodi, versionUsar.Covercodi)).Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoOperacionURS).
                    OrderBy(x => x.Courdevigenciadesde).ToList();

            //Obtengo listado de URS
            List<CoMedicion48DTO> listadoUrs = FactorySic.GetCoMedicion48Repository().ObtenerListadoURS();

            foreach (var itemUrs in listadoUrs)
            {
                List<CoConfiguracionDetDTO> configURS = configuracionURS.Where(x => x.Grupocodi == itemUrs.Grupocodi).ToList();

                if (configURS.Any())
                {
                    List<CoConfiguracionDetDTO> configEquipo = configURS.Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoOperacionURS).OrderBy(x => x.Courdevigenciadesde).ToList();                    
                    CoConfiguracionDetDTO configuracionOperacion = this.ObtenerConfiguracionPorDia(configEquipo, versionUsar, dia);
                    List<CoConfiguracionSenialDTO> seniales = FactorySic.GetCoConfiguracionSenialRepository().GetByCriteria(configuracionOperacion.Courdecodi).ToList();
                    lstSalida.AddRange(seniales);
                }
            }

            return lstSalida;
        }


        /// <summary>
        /// Busca el valor de canal en 2 tablas: Medicion60 (DE BD O RECIEN CALCULADO) o en CoConfiguracionSenial (Valor inicial)
        /// </summary>
        /// <param name="tipoImportacion"></param>
        /// <param name="dia"></param>
        /// <param name="lstMedicion60XDiaAnteriorXCanalcodi"></param>
        /// <param name="lstSenialesAntConValInicialXCanalcodi"></param>
        /// <returns></returns>
        private decimal? BuscarValorCanalEnOtrasTablas(int tipoImportacion, DateTime dia, List<CoMedicion60DTO> lstMedicion60XDiaAnteriorXCanalcodi, List<CoConfiguracionSenialDTO> lstSenialesAntConValInicialXCanalcodi)
        {
            decimal? valorADevolver = null;

            //Se ordena para que el primero sea el valor inicial mas reciente de todos los periodos/versiones pasados
            List<CoConfiguracionSenialDTO> lstValIniSenialesXCanalcodi = lstSenialesAntConValInicialXCanalcodi.OrderByDescending(x => x.Copermes).ToList();

            //PRIMERO: BUSQUEDA EN TABLA MEDICION60 dia anterior 23:59:59            
            if (lstMedicion60XDiaAnteriorXCanalcodi.Any())
            {
                //Si es mensual e Importacion de todo
                if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_Todo)
                {
                    List<CoMedicion60DTO> lstMed60Usar = new List<CoMedicion60DTO>();

                    //No se debetomar en cuenta la informacion DIARIA
                    List<CoMedicion60DTO> lstTemp = lstMedicion60XDiaAnteriorXCanalcodi.Where(f => f.Comeditipo != 0).ToList();

                    //Si es dia 1 (no tengo med60 calculados) por eso uso med60 de dias anterior de la BD
                    if (dia.Day == 1)
                    {
                        //se usara el  registro que tenga mayor covercodi (el mas reciente) por eso hay que ordenarlo
                        lstMed60Usar = lstTemp.Where(x => x.Origen == ConstantesCostoOportunidad.Med60VieneDeBD).OrderByDescending(x=>x.Covercodi).ToList();
                    }
                    else
                    {
                        lstMed60Usar = lstTemp.Where(x => x.Origen == ConstantesCostoOportunidad.Med60Calculado).ToList();
                    }

                    if (lstMed60Usar.Any())
                    {
                        //busco el registro (si es dia 1, el que tenga mayor covercodi y como ya esta ordenado seria el primero y dia > 1 el calculado)

                        //Obtenemos el valor del ultimo segundo  seg 59 (H60)
                        valorADevolver = lstMed60Usar.First().H60;

                        if (valorADevolver == null)
                        {
                            //Busca en tabla Co_Configuracion_Senial
                            valorADevolver = lstValIniSenialesXCanalcodi.Any() ? lstValIniSenialesXCanalcodi.First().Consenvalinicial : null;
                        }
                    }
                    else
                    {
                        //Busca en tabla Co_Configuracion_Senial
                        valorADevolver = lstValIniSenialesXCanalcodi.Any() ? lstValIniSenialesXCanalcodi.First().Consenvalinicial : null;
                    }
                }
                else
                {
                    //Si es mensual e Importacion de dias Faltantes
                    if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Mensual_DiasFaltantes)
                    {
                        List<CoMedicion60DTO> lstMed60Usar = new List<CoMedicion60DTO>();

                        //No se debetomar en cuenta la informacion DIARIA
                        List<CoMedicion60DTO> lstMed60Temporal = lstMedicion60XDiaAnteriorXCanalcodi.Where(f => f.Comeditipo != 0).ToList();
                        List<CoMedicion60DTO> lstMed60Temp = lstMed60Temporal.Where(x => x.Origen == ConstantesCostoOportunidad.Med60Calculado).ToList();

                        //Si existe un registro recientemente calculado, se usa ese
                        if (lstMed60Temp.Any())
                        {
                            lstMed60Usar = lstMed60Temp;
                        }
                        //sino uso los de la BD pero el que tenga mayor covercodi (el mas reciente)
                        else
                        {
                            lstMed60Usar = lstMed60Temporal.Where(x => x.Origen == ConstantesCostoOportunidad.Med60VieneDeBD).OrderByDescending(x => x.Covercodi).ToList();
                        }
                        

                        if (lstMed60Usar.Any()) 
                        {
                            //Obtenemos el valor del ultimo segundo  seg 59 (H60)
                            valorADevolver = lstMed60Usar.First().H60.Value; 

                            if (valorADevolver == null)
                            {
                                //Busca en tabla Co_Configuracion_Senial
                                valorADevolver = lstValIniSenialesXCanalcodi.First().Consenvalinicial;
                            }
                        }
                        else
                        {
                            //Busca en tabla Co_Configuracion_Senial
                            valorADevolver = lstValIniSenialesXCanalcodi.First().Consenvalinicial;
                        }
                    }
                    else
                    {
                        //Si es Diario (usado en importaciones automaticas) se toma toda la info, tanto de tablas mensual y tablas diaria
                        if (tipoImportacion == ConstantesCostoOportunidad.ImportarSeniales_Diario)
                        {
                            List<CoMedicion60DTO> lstMed60Usar = new List<CoMedicion60DTO>();
                            List<CoMedicion60DTO> lstMed60Temp = new List<CoMedicion60DTO>();

                            //Se usara lo que viene de BD                            
                            lstMed60Temp = lstMedicion60XDiaAnteriorXCanalcodi.Where(x => x.Origen == ConstantesCostoOportunidad.Med60VieneDeBD).ToList();

                            //Se da prioridad a la tabla diaria (CoMedicion_YYYYMM)
                            List<CoMedicion60DTO> lstMed60Diaria = lstMed60Temp.Where(d => d.Comeditipo == 0).ToList();
                            if (lstMed60Diaria.Any())
                                lstMed60Usar = lstMed60Diaria;
                            else
                                lstMed60Usar = lstMed60Temp.OrderByDescending(x => x.Covercodi).ToList(); //tomo de las tablas mensuales (el más reciente)

                            if (lstMed60Usar.Any()) 
                            {
                                //Obtenemos el valor del ultimo segundo  seg 59 (H60)
                                valorADevolver = lstMed60Usar.First().H60.Value; //Deberia sí o sí existir info aqui

                                if (valorADevolver == null)
                                {
                                    //Busca en tabla Co_Configuracion_Senial
                                    valorADevolver = lstValIniSenialesXCanalcodi.First().Consenvalinicial;
                                }
                            }
                            else
                            {
                                //Busca en tabla Co_Configuracion_Senial
                                valorADevolver = lstValIniSenialesXCanalcodi.First().Consenvalinicial;
                            }
                        }
                    }
                }
            }
            //Si no existe en medicion60, busca en Configuracion_Senial
            else
            {
                valorADevolver = lstValIniSenialesXCanalcodi.Any() ? lstValIniSenialesXCanalcodi.First().Consenvalinicial : null;
            }


            return valorADevolver;
        }

        /// <summary>
        /// Completa al segundo la informacion de los valores del canal
        /// </summary>
        /// <param name="numRegActual"></param>
        /// <param name="dia"></param>
        /// <param name="valorRegAnterior"></param>
        /// <param name="horaAnt"></param>
        /// <param name="minutoAnt"></param>
        /// <param name="segundoAnt"></param>
        /// <param name="valorRegActual"></param>
        /// <param name="horaActual"></param>
        /// <param name="minutoActual"></param>
        /// <param name="segundoActual"></param>
        /// <param name="canalcodiX"></param>
        /// <param name="lstCanales"></param>
        /// <param name="esUltimoReg"></param>
        /// <param name="lstErroresParcial"></param>
        /// <param name="lstMedicion60Parcial"></param>
        private void CompletarAlSegundoGrupoAnteriorDesdeTablasScada(int numRegActual, DateTime dia, decimal? valorRegAnterior, int horaAnt, int minutoAnt, int segundoAnt, 
            decimal? valorRegActual,  int horaActual, int minutoActual, int segundoActual, int canalcodiX, List<TrCanalSp7DTO> lstCanales, bool esUltimoReg, out List<ErrorScada> lstErroresParcial, out List<Medicion60XSegundo> lstMedicion60Parcial)
        {
            lstErroresParcial = new List<ErrorScada>();
            lstMedicion60Parcial = new List<Medicion60XSegundo>();

            //Guarda grupo anterior y completa hasta las 23:59:59
            if (esUltimoReg)
            {
                //Solo existe un registro
                if(numRegActual == 1)
                {
                    //Guarda el dia completo (00:00:00 - 23:59:59) con el mismo valor
                    if (horaActual == 0 && minutoActual == 0 && segundoActual == 0)
                    {
                        if(valorRegActual != null)
                        {
                            if(valorRegActual >= 0)
                            {
                                List<Medicion60XSegundo> lstMed60Parcial = CrearRegistrosCoMedicion60(dia, horaActual, minutoActual, segundoActual, 23, 59, 59, canalcodiX, valorRegActual.Value);
                                lstMedicion60Parcial.AddRange(lstMed60Parcial);
                            }
                            else
                            {
                                List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaActual, minutoActual, segundoActual, 23, 59, 59, "Valor negativo", canalcodiX, lstCanales);
                                lstErroresParcial.AddRange(lstErrParcial);
                            }
                            
                        }
                        else
                        {
                            List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaActual, minutoActual, segundoActual, 23, 59, 59, "Valor nulo", canalcodiX, lstCanales);
                            lstErroresParcial.AddRange(lstErrParcial);
                        }
                    }
                    //Guarda anterior y completa hasta las 23:59:59
                    else
                    {
                        //Guarda anterior
                        DateTime hora1SegAntes = new DateTime(dia.Year, dia.Month, dia.Day, horaActual, minutoActual, segundoActual).AddSeconds(-1);
                        if (valorRegAnterior != null)
                        {            
                            if(valorRegAnterior >= 0)
                            {
                                List<Medicion60XSegundo> lstMed60Parcial = CrearRegistrosCoMedicion60(dia, horaAnt, minutoAnt, segundoAnt, hora1SegAntes.Hour, hora1SegAntes.Minute, hora1SegAntes.Second, canalcodiX, valorRegAnterior.Value);
                                lstMedicion60Parcial.AddRange(lstMed60Parcial);
                            }
                            else
                            {
                                List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaAnt, minutoAnt, segundoAnt, hora1SegAntes.Hour, hora1SegAntes.Minute, hora1SegAntes.Second, "Valor negativo", canalcodiX, lstCanales);
                                lstErroresParcial.AddRange(lstErrParcial);
                            }
                            
                        }
                        else
                        {
                            List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaAnt, minutoAnt, segundoAnt, hora1SegAntes.Hour, hora1SegAntes.Minute, hora1SegAntes.Second, "Valor nulo", canalcodiX, lstCanales);
                            lstErroresParcial.AddRange(lstErrParcial);
                        }

                        //Completo hasta las 23:59:59
                        if (valorRegActual != null)
                        {
                            if(valorRegActual >= 0)
                            {
                                List<Medicion60XSegundo> lstMed60Parcial = CrearRegistrosCoMedicion60(dia, horaActual, minutoActual, segundoActual, 23, 59, 59, canalcodiX, valorRegActual.Value);
                                lstMedicion60Parcial.AddRange(lstMed60Parcial);
                            }
                            else
                            {
                                List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaActual, minutoActual, segundoActual, 23, 59, 59, "Valor negativo", canalcodiX, lstCanales);
                                lstErroresParcial.AddRange(lstErrParcial);
                            }
                            
                        }
                        else
                        {
                            List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaActual, minutoActual, segundoActual, 23, 59, 59, "Valor nulo", canalcodiX, lstCanales);
                            lstErroresParcial.AddRange(lstErrParcial);
                        }
                    }
                }
                //Es ultimo pero Existe mas de un registro
                //Guarda anterior y completa hasta las 23:59:59
                else
                {
                    //Guarda anterior
                    DateTime hora1SegAntes = new DateTime(dia.Year, dia.Month, dia.Day, horaActual, minutoActual, segundoActual).AddSeconds(-1);
                    if (valorRegAnterior != null)
                    {
                        List<Medicion60XSegundo> lstMed60Parcial = CrearRegistrosCoMedicion60(dia, horaAnt, minutoAnt, segundoAnt, hora1SegAntes.Hour, hora1SegAntes.Minute, hora1SegAntes.Second, canalcodiX, valorRegAnterior.Value);
                        lstMedicion60Parcial.AddRange(lstMed60Parcial);
                    }
                    else
                    {
                        List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaAnt, minutoAnt, segundoAnt, hora1SegAntes.Hour, hora1SegAntes.Minute, hora1SegAntes.Second, "Valor nulo", canalcodiX, lstCanales);
                        lstErroresParcial.AddRange(lstErrParcial);
                    }

                    //Completo hasta las 23:59:59
                    if (valorRegActual != null)
                    {
                        List<Medicion60XSegundo> lstMed60Parcial = CrearRegistrosCoMedicion60(dia, horaActual, minutoActual, segundoActual, 23, 59, 59, canalcodiX, valorRegActual.Value);
                        lstMedicion60Parcial.AddRange(lstMed60Parcial);
                    }
                    else
                    {
                        List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaActual, minutoActual, segundoActual, 23, 59, 59, "Valor nulo", canalcodiX, lstCanales);
                        lstErroresParcial.AddRange(lstErrParcial);
                    }
                }
            }
            //No es el ultimo registro, entonces solo guarda grupo anterior
            else
            {
                if (numRegActual == 1)
                {
                    //No hace nada, este se guardará la siguiente iteracion
                    if (horaActual == 0 && minutoActual == 0 && segundoActual == 0)
                    {
                        
                    }
                    //Guarda anterior, desde las 00:00:00 hasta un segundo antes del primer registro
                    else
                    {
                        //Guarda anterior
                        DateTime hora1SegAntes = new DateTime(dia.Year, dia.Month, dia.Day, horaActual, minutoActual, segundoActual).AddSeconds(-1);
                        if (valorRegAnterior != null)
                        {
                            if(valorRegAnterior >= 0)
                            {
                                List<Medicion60XSegundo> lstMed60Parcial = CrearRegistrosCoMedicion60(dia, horaAnt, minutoAnt, segundoAnt, hora1SegAntes.Hour, hora1SegAntes.Minute, hora1SegAntes.Second, canalcodiX, valorRegAnterior.Value);
                                lstMedicion60Parcial.AddRange(lstMed60Parcial);
                            }
                            else
                            {
                                List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaAnt, minutoAnt, segundoAnt, hora1SegAntes.Hour, hora1SegAntes.Minute, hora1SegAntes.Second, "Valor negativo", canalcodiX, lstCanales);
                                lstErroresParcial.AddRange(lstErrParcial);
                            }
                            
                        }
                        else
                        {
                            List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaAnt, minutoAnt, segundoAnt, hora1SegAntes.Hour, hora1SegAntes.Minute, hora1SegAntes.Second, "Valor nulo", canalcodiX, lstCanales);
                            lstErroresParcial.AddRange(lstErrParcial);
                        }
                    }
                }
                //Guarda bloque anterior
                else
                {
                    //Guarda anterior
                    DateTime hora1SegAntes = new DateTime(dia.Year, dia.Month, dia.Day, horaActual, minutoActual, segundoActual).AddSeconds(-1);
                    if (valorRegAnterior != null)
                    {
                        if(valorRegAnterior >= 0)
                        {
                            List<Medicion60XSegundo> lstMed60Parcial = CrearRegistrosCoMedicion60(dia, horaAnt, minutoAnt, segundoAnt, hora1SegAntes.Hour, hora1SegAntes.Minute, hora1SegAntes.Second, canalcodiX, valorRegAnterior.Value);
                            lstMedicion60Parcial.AddRange(lstMed60Parcial);
                        }
                        else {
                            List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaAnt, minutoAnt, segundoAnt, hora1SegAntes.Hour, hora1SegAntes.Minute, hora1SegAntes.Second, "Valor negativo", canalcodiX, lstCanales);
                            lstErroresParcial.AddRange(lstErrParcial);
                        }
                        
                    }
                    else
                    {
                        List<ErrorScada> lstErrParcial = CrearRegistrosErrores(dia, horaAnt, minutoAnt, segundoAnt, hora1SegAntes.Hour, hora1SegAntes.Minute, hora1SegAntes.Second, "Valor nulo", canalcodiX, lstCanales);
                        lstErroresParcial.AddRange(lstErrParcial);
                    }
                }
                                    
            }
        }


        /// <summary>
        /// Crar registros de errores
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="horaIni"></param>
        /// <param name="minutoIni"></param>
        /// <param name="segundoIni"></param>
        /// <param name="horaFin"></param>
        /// <param name="minutoFin"></param>
        /// <param name="segundoFin"></param>
        /// <param name="msgError"></param>
        /// <param name="canalcodi"></param>
        /// <param name="lstCanales"></param>
        /// <returns></returns>
        private List<ErrorScada> CrearRegistrosErrores(DateTime dia, int horaIni, int minutoIni, int segundoIni, int horaFin, int minutoFin, int segundoFin, string msgError, int canalcodi, List<TrCanalSp7DTO> lstCanales)
        {
            List<ErrorScada> lstErrorAGuardar = new List<ErrorScada>();

            DateTime fecIni = new DateTime(dia.Year, dia.Month, dia.Day, horaIni, minutoIni, segundoIni);
            DateTime fecFin = new DateTime(dia.Year, dia.Month, dia.Day, horaFin, minutoFin, segundoFin);
            TimeSpan Diff = fecFin.Subtract((fecIni).AddSeconds(-1));

            int numSegundos = (int)Diff.TotalSeconds;
            int numRegACrear = numSegundos;
            string nomCanal = lstCanales.Find(x => x.Canalcodi == canalcodi) != null ? lstCanales.Find(x => x.Canalcodi == canalcodi).Canalnomb.Trim() : "";

            for (int i = 0; i < numRegACrear; i++)
            {
                DateTime fecInicio = fecIni;

                ErrorScada oError = new ErrorScada();
                
                oError.Canalcodi = canalcodi;
                oError.Valor = null;
                oError.FechaHora = new DateTime(fecInicio.Year, fecInicio.Month, fecInicio.Day, fecInicio.Hour, fecInicio.Minute, fecInicio.Second);
                oError.Mensaje = "Al momento del completado de registros al segundo se encontró " + msgError + " para el canal: " + nomCanal + " (" + canalcodi + "), dia: " + oError.FechaHora.ToString(ConstantesAppServicio.FormatoFecha) + ", hora: " + oError.FechaHora.ToString(ConstantesAppServicio.FormatoHHmmss);

                fecIni = fecInicio.AddSeconds(1);

                lstErrorAGuardar.Add(oError);
            }

            return lstErrorAGuardar;
        }

        /// <summary>
        /// Crea registros de medicion60
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="horaIni"></param>
        /// <param name="minutoIni"></param>
        /// <param name="segundoIni"></param>
        /// <param name="horaFin"></param>
        /// <param name="minutoFin"></param>
        /// <param name="segundoFin"></param>
        /// <param name="canalcodiX"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        private List<Medicion60XSegundo> CrearRegistrosCoMedicion60(DateTime dia, int horaIni, int minutoIni, int segundoIni, int horaFin, int minutoFin, int segundoFin, int canalcodiX, decimal valor)
        {
            List<Medicion60XSegundo> lstCoMed60AGuardar = new List<Medicion60XSegundo>();

            DateTime fecIni = new DateTime(dia.Year, dia.Month, dia.Day, horaIni, minutoIni, segundoIni);
            DateTime fecFin = new DateTime(dia.Year, dia.Month, dia.Day, horaFin, minutoFin, segundoFin);
            TimeSpan Diff = fecFin.Subtract((fecIni).AddSeconds(-1));

            //int numSegundos = Diff.Hours * 60 * 60 + Diff.Minutes * 60 + Diff.Seconds;
            int numSegundos = (int)Diff.TotalSeconds;
            int numRegACrear = numSegundos;

            for (int i = 0; i < numRegACrear; i++)
            {
                DateTime fecInicio = fecIni;

                Medicion60XSegundo oMed60 = new Medicion60XSegundo();
                oMed60.FechaDia = fecInicio.Date;
                oMed60.Hora = fecInicio.Hour;
                oMed60.Minuto = fecInicio.Minute;
                oMed60.Segundo = fecInicio.Second;
                oMed60.Canalcodi = canalcodiX;
                oMed60.Valor = valor; 

                fecIni = fecInicio.AddSeconds(1);

                lstCoMed60AGuardar.Add(oMed60);
            }

            return lstCoMed60AGuardar;
    }


        /// <summary>
        ///  Devuelve los registros de medicion60 para todas las urs del dia anterior a las 23:59. Ejm. Agosto [31/07-30/08] tomando solo los "dias anteriores" a importar
        /// </summary>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        /// <param name="lstStrCanalcodis"></param>
        /// <param name="diasAImportar"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private List<CoMedicion60DTO> ObtenerMedicio60DelDiaAnterior(DateTime rangoIni, DateTime rangoFin, string lstStrCanalcodis, List<int> diasAImportar, CoPeriodoDTO periodo)
        {
            List<CoMedicion60DTO> lstSalida = new List<CoMedicion60DTO>();
            List<DateTime> listadoDiasAnteriores = new List<DateTime>();

            //Obtenemos solo los dias a importar
            for (DateTime dia = rangoIni; rangoFin.CompareTo(dia) >= 0; dia = dia.AddDays(1))
            {
                //solo dias que se deben importar                
                if (!(diasAImportar.Where(d => d == dia.Day).Any()))
                {
                    continue;
                }

                DateTime fechaAnteriorAUsar = dia.AddDays(-1); //tomar dias anteriores
                listadoDiasAnteriores.Add(fechaAnteriorAUsar);
            }

            //Obtenemos versiones para el periodo anterio y actual
            DateTime fechaPeriodoActual = new DateTime(periodo.Coperanio.Value, periodo.Copermes.Value, 1);
            List<CoPeriodoDTO> lstPeriodosActivos = ListCoPeriodos().Where(f=>f.Coperestado == "A").ToList();
            List<CoPeriodoDTO> lstPeriodoMesAnterior = lstPeriodosActivos.Where(x => x.Coperanio.Value == fechaPeriodoActual.AddMonths(-1).Year && x.Copermes.Value == fechaPeriodoActual.AddMonths(-1).Month).ToList();
            CoPeriodoDTO periodoMesAnterior = lstPeriodoMesAnterior.Any() ? lstPeriodoMesAnterior.First() : null;

            List<CoVersionDTO> lstVersionesPeriodoAnterior = periodoMesAnterior != null ? GetByCriteriaCoVersions(periodoMesAnterior.Copercodi).Where(r => r.Coverestado == "Abierto").ToList() : new List<CoVersionDTO>();
            List<CoVersionDTO> lstVersionesPeriodoActual = GetByCriteriaCoVersions(periodo.Copercodi).Where(r => r.Coverestado == "Abierto").ToList();

            //Obtenemos los nombres de las tablas Med60 existentes para el mes pasado y el mes actual
            List<TablaMedicion60> lstExistenciaTablasMed60MesPasado = ObtenerExistenciaTablasMed60(lstVersionesPeriodoAnterior, fechaPeriodoActual.AddMonths(-1));
            List<TablaMedicion60> lstExistenciaTablasMed60MesActual = ObtenerExistenciaTablasMed60(lstVersionesPeriodoActual, fechaPeriodoActual);
            List<TablaMedicion60> lstExistenciaTablasMed60Totales = new List<TablaMedicion60>();

            //Obtenemos los dias (anteriores) del cual se deben traer la informacion
            List<string> lstDiasAnt = new List<string>();
            foreach (DateTime fechaDiaAnterior in listadoDiasAnteriores)
            {
                string tempQuery = "TO_DATE ('" + fechaDiaAnterior.ToString(ConstantesAppServicio.FormatoFecha) + "','dd/mm/yyyy')";
                lstDiasAnt.Add(tempQuery);
            }
            string bloqueQueryDias = String.Join(",", lstDiasAnt);

            //verifico si necesitaré med60 del mes pasado
            if(diasAImportar.First() == 1)
            {
                lstExistenciaTablasMed60Totales.AddRange(lstExistenciaTablasMed60MesPasado);
                lstExistenciaTablasMed60Totales.AddRange(lstExistenciaTablasMed60MesActual);
            }
            else //solo necesitare med60 (dias anteriores) del presente mes
            {
                lstExistenciaTablasMed60Totales.AddRange(lstExistenciaTablasMed60MesActual);
            }
                

            //Armamos bloque de query para traer data de dias anteriores
            List<string> lstTablas = new List<string>();

            foreach (var tablaMed60 in lstExistenciaTablasMed60Totales)
            {
                string tempQuery = "( SELECT PD.PRODIAFECHA AS FECHA, PD.COVERCODI, M" + tablaMed60.TipoTabla + ".* FROM " + tablaMed60.NombreTabla + " M" + tablaMed60.TipoTabla + " INNER JOIN CO_PROCESO_DIARIO PD ON PD.PRODIACODI = M" + tablaMed60.TipoTabla + ".PRODIACODI WHERE (TO_DATE(PD.PRODIAFECHA) IN (" + bloqueQueryDias + ") ) AND M" + tablaMed60.TipoTabla + ".COMEDIHORA = 23 AND COMEDIMINUTO = 59 AND M" + tablaMed60.TipoTabla + ".CANALCODI IN (" + lstStrCanalcodis + ") ) ";
                lstTablas.Add(tempQuery);
            }
            string queryGeneral = String.Join("UNION", lstTablas);

            lstSalida = FactorySic.GetCoMedicion60Repository().ListarUltimoMinutoDiaAnteriorMuchasTablas(queryGeneral);
            
            //Dado que ya existen antes de iniciar la importacion, data viene de BD
            foreach (var item in lstSalida)
            {
                
                item.Origen = ConstantesCostoOportunidad.Med60VieneDeBD;
            }
            return lstSalida;
        }

        /// <summary>
        /// Devuelve el listado de seniales (de cierto periodo y version) con valores iniciales para periodos anteriores o iguales
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="strCanalcodis"></param>
        /// <returns></returns>
        private List<CoConfiguracionSenialDTO> ObtenerSenialesAnterioresConValoresIniciales(CoPeriodoDTO periodo, string strCanalcodis)
        {
            List<CoConfiguracionSenialDTO> lstSalida = FactorySic.GetCoConfiguracionSenialRepository().ListarSenialesPeriodosAnteriores(periodo.Coperanio.Value, periodo.Copermes.Value, strCanalcodis);
            
            return lstSalida;
        }

        /// <summary>
        /// Devuelve la query para obtener toda la informacion de las tablas SCADA (TR_FECHA_SP7) para cierto periodo y version
        /// </summary>
        /// <param name="lstExistenciaTablasScada"></param>
        /// <param name="lstStrCanalcodis"></param>
        /// <returns></returns>
        private string ArmarQueryParaObtenerDatosScada(List<TablasTRScada> lstExistenciaTablasScada, string lstStrCanalcodis)
        {
            List<string> lstTablas = new List<string>();

            foreach (var item in lstExistenciaTablasScada)
            {
                string tablanomb = item.NombreTabla + "@DBL_SICOES_SCADA ";
                string tempQuery = " (SELECT CANALCODI, CANALFHSIST AS CANALFHORA2, CANALVALOR, CANALCALIDAD, CANALFECHAHORA AS CANALFHORA FROM " + tablanomb + " WHERE CANALCODI IN (" + lstStrCanalcodis + ")) ";
                lstTablas.Add(tempQuery);
            }
            string query = String.Join("UNION", lstTablas);
            
            return query;
        }

        /// <summary>
        ///  Devuelve un listado con informacion de existencia de tablas SCADA para cada dia del periodo y version y segun el tipo de informacion
        /// </summary>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        /// <param name="diasAImportar"></param>
        /// <returns></returns>
        private List<TablasTRScada> ObtenerExistenciaTablasScada(DateTime rangoIni, DateTime rangoFin, List<int> diasAImportar)
        {
            List<TablasTRScada> lstSalida = new List<TablasTRScada>();
            List<string> lstTablas = new List<string>();

            DateTime diaAdic = new DateTime(rangoIni.Year, rangoIni.Month, diasAImportar.Last()).AddDays(1);

            //Obtengo cadena de tablas SCADA a usar
            for (DateTime dia = rangoIni; rangoFin.CompareTo(dia) >= 0; dia = dia.AddDays(1))
            {
                //solo dias que se deben importar                
                if(!(diasAImportar.Where(d=>d == dia.Day).Any()))
                {
                    continue;
                }

                //armo listado con nombre de tablas
                var nombTabla = "TR_" + dia.Year + string.Format("{0:D2}", dia.Month) + string.Format("{0:D2}", dia.Day) + "_SP7";
                lstTablas.Add(nombTabla);
            }

            //Agrego la tabla de 1 dia adicional (dado que hay data del dia X en la tabla X+1)
            var nombTablaAdic = "TR_" + diaAdic.Year + string.Format("{0:D2}", diaAdic.Month) + string.Format("{0:D2}", diaAdic.Day) + "_SP7";
            lstTablas.Add(nombTablaAdic);

            //trasformo a cadena
            string lstStrTablas = String.Join(",", lstTablas);
            string strTablasScada = lstStrTablas.Replace(",", "','");

            //verifico que existan las tablas           
            List<CoProcesoErrorDTO> lstTablasExistentes = FactorySic.GetCoProcesoErrorRepository().ListarTablas(strTablasScada);
            lstSalida = ObtenerListaTablasScada(rangoIni, rangoFin, lstTablasExistentes, diasAImportar);

            return lstSalida;
        }

        /// <summary>
        /// Devuelve un listado con informacion de existencia de tablas cargadas de manera manual, similar al metodo ObtenerExistenciaTablasScada
        /// </summary>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        /// <param name="diasAImportar"></param>
        /// <param name="lstIngresadaManualmente"></param>
        /// <returns></returns>
        private List<TablasTRScada> ObtenerExistenciaTablasScadaManual(DateTime rangoIni, DateTime rangoFin, List<int> diasAImportar, List<CoDatoSenialDTO> lstIngresadaManualmente)
        {
            List<TablasTRScada> lstSalida = new List<TablasTRScada>();
            
            //Obtengo cadena de tablas SCADA a usar
            for (DateTime dia = rangoIni; rangoFin.CompareTo(dia) >= 0; dia = dia.AddDays(1))
            {
                //solo dias que se deben importar                
                if (!(diasAImportar.Where(d => d == dia.Day).Any()))
                {
                    continue;
                }

                //armo listado con nombre de tablas                
                var nombTabla = "TR_" + dia.Year + string.Format("{0:D2}", dia.Month) + string.Format("{0:D2}", dia.Day) + "_SP7";

                //verifico que exista almenos un registro para el dia
                var t = lstIngresadaManualmente.Find(x => x.Codasefechahora.Value.Date == dia.Date);

                TablasTRScada obj = new TablasTRScada();
                obj.FechaDia = dia;
                obj.NombreTabla = nombTabla;
                obj.Existe = t != null ? "Y" : "N";

                lstSalida.Add(obj);

            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve listado de informacion de tablas scada para el periodo y version
        /// </summary>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        /// <param name="lstTablasExistentes"></param>
        /// <param name="diasAImportar"></param>
        /// <returns></returns>
        private List<TablasTRScada> ObtenerListaTablasScada(DateTime rangoIni, DateTime rangoFin, List<CoProcesoErrorDTO> lstTablasExistentes, List<int> diasAImportar)
        {
            List<TablasTRScada> lstSalida = new List<TablasTRScada>();

            //Obtengo cadena de tablas SCADA a usar
            for (DateTime dia = rangoIni; rangoFin.CompareTo(dia) >= 0; dia = dia.AddDays(1))
            {
                //solo dias que se deben importar                
                if (!(diasAImportar.Where(d => d == dia.Day).Any()))
                {
                    continue;
                }

                var nombTabla = "TR_" + dia.Year + string.Format("{0:D2}", dia.Month) + string.Format("{0:D2}", dia.Day) + "_SP7";
                var t = lstTablasExistentes.Find(x => x.Tablanomb == nombTabla);

                TablasTRScada obj = new TablasTRScada();
                obj.FechaDia = dia;
                obj.NombreTabla = nombTabla;
                obj.Existe = t != null ? "Y" : "N";

                lstSalida.Add(obj);
            }

            //Agrego la tabla de 1 dia adicional (dado que hay data del dia X en la tabla X+1)
            DateTime diaAdic = new DateTime(rangoIni.Year, rangoIni.Month, diasAImportar.Last()).AddDays(1);
            var nombTablaAdic = "TR_" + diaAdic.Year + string.Format("{0:D2}", diaAdic.Month) + string.Format("{0:D2}", diaAdic.Day) + "_SP7";
            var t2 = lstTablasExistentes.Find(x => x.Tablanomb == nombTablaAdic);
            TablasTRScada objAdic = new TablasTRScada();
            objAdic.FechaDia = diaAdic;
            objAdic.NombreTabla = nombTablaAdic;
            objAdic.Existe = t2 != null ? "Y" : "N";
            lstSalida.Add(objAdic);

            return lstSalida;
        }

        /// <summary>
        /// Devuelve un listado de existencias de tablas med60 para cierto periodo/version
        /// </summary>
        /// <param name="lstVersionesPeriodo"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        private List<TablaMedicion60> ObtenerExistenciaTablasMed60(List<CoVersionDTO> lstVersionesPeriodo, DateTime fechaPeriodo)
        {
            List<TablaMedicion60> lstSalida = new List<TablaMedicion60>();             
            Dictionary<string, int> lstTablas = new Dictionary<string, int>();

            string tabPrincipal = "CO_MEDICION60_" + fechaPeriodo.Year + string.Format("{0:D2}", fechaPeriodo.Month);
            lstTablas.Add(tabPrincipal, 0);

            foreach (var version in lstVersionesPeriodo)
            {
                //armo listado con nombre de tablas
                int idTipo = version.Covebacodi.Value;                
                var nombTabla = tabPrincipal + "_" + idTipo;
                lstTablas.Add(nombTabla, idTipo);
            }
            string lstStrTablas = String.Join(",", lstTablas.Keys);
            string strTablasM60 = lstStrTablas.Replace(",", "','");

            //verifico que existan las tablas           
            List<CoMedicion60DTO> lstTablasExistentes = FactorySic.GetCoMedicion60Repository().ListarTablas(strTablasM60);            

            //devuelvo listado de tablas con mas informacion de ellas
            foreach (var item in lstTablasExistentes)
            {
                TablaMedicion60 obj = new TablaMedicion60();               

                obj.MesTabla = fechaPeriodo.Month;
                obj.AnioTabla = fechaPeriodo.Year;
                obj.NombreTabla = item.Tablanomb;
                obj.TipoTabla = lstTablas[obj.NombreTabla];

                lstSalida.Add(obj);
            }            

            return lstSalida;
        }  

        /// <summary>
        /// Devuelve las seniales existentes para cierto periodo y version
        /// </summary>
        /// <param name="copercodi"></param>
        /// <param name="covercodi"></param>
        /// <returns></returns>
        private List<CoConfiguracionSenialDTO> ListarSenialesExistentes(int copercodi, int covercodi)
        {
            List<CoConfiguracionSenialDTO> lstSalida = FactorySic.GetCoConfiguracionSenialRepository().ListarSeniales(copercodi, covercodi);

            return lstSalida;
        }

        /// <summary>
        /// Genera el reporte de errores para cierto dia
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="prodiacodi"></param>
        /// <param name="nameFile"></param>
        public void GenerarReporteDeErrores(string ruta, int prodiacodi, string nameFile)
        {            
            //Descargo archivo segun requieran
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }
            
            List<CoProcesoErrorDTO> listaErrores = ListarCoProcesoErrorsPorDia(prodiacodi);            

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarExcelErroresImportacion(xlPackage, listaErrores);

                xlPackage.Save();

            }
        }

        /// <summary>
        /// Genera el reporte de errores en html
        /// </summary>
        /// <param name="prodiacodi"></param>
        /// <returns></returns>
        public string GenerarHtmlReporteDeErrores( int prodiacodi)
        {            
            List<CoProcesoErrorDTO> listaErrores = ListarCoProcesoErrorsPorDia(prodiacodi);

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tblListaerrores' style='width:600px;'>");
            str.Append("<thead>");

            #region cabecera            
            str.Append("<tr>");
            str.Append("<th scope='col' style='width: 40px; min-width: 40px'>#</th>");
            str.Append("<th scope='col' style='width: 620px; min-width: 620px'>Descripción</th>");
            
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");

            int num = 1;
            foreach (var item in listaErrores)
            {               
                str.AppendFormat("<tr>");
                
                str.AppendFormat("<td  style='width: 40px; text-align: center'>{0}</td>", num);
                str.AppendFormat("<td  style='width: 620px; text-align: center'>{0}</td>", item.Proerrmsg);
                num++;
                str.Append("</tr>");

            }
            str.Append("</tbody>");

            return str.ToString();
        }
        

        /// <summary>
        /// Genera la estructura del reporte de errores a exportar
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="listaErrores"></param>
        private void GenerarExcelErroresImportacion(ExcelPackage xlPackage, List<CoProcesoErrorDTO> listaErrores)
        {
            ExcelWorksheet ws = null;

            string nameWS = "Errores_Encontrados";

            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];            
            string fontFamily = "Arial Narrow";

            var fuenteTabla = new Font(fontFamily, 8);

            int rowIniTabla = 4;
            int colIniTable = 2;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 10, 100})//columna A-I
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }

            #region  Filtros y Cabecera

            var colNum = colIniTable;
            int colDescripcion = colIniTable + 1;

            ws.Cells[rowIniTabla - 2, colIniTable].Value = "Reporte de Errores";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colDescripcion, "Arial Narrow", 14);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colDescripcion);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colDescripcion);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colDescripcion, "Centro");

            
            ws.Row(rowIniTabla).Height = 20;
            ws.Cells[rowIniTabla, colNum].Value = "#";
            ws.Cells[rowIniTabla, colDescripcion].Value = "Descripción";
            

            var rangoCab = ws.Cells[rowIniTabla, colNum, rowIniTabla, colDescripcion];
            rangoCab.SetAlignment();

            UtilExcel.CeldasExcelColorTexto(ws, rowIniTabla, colNum, rowIniTabla, colDescripcion, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniTabla, colNum, rowIniTabla, colDescripcion, "#0D43A5");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colNum, rowIniTabla, colDescripcion, "Arial", 11);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTabla, colIniTable, rowIniTabla, colDescripcion);            
            UtilExcel.BorderCeldas4_1(ws, rowIniTabla, colNum, rowIniTabla, colDescripcion, Color.Gray);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            int n = 1;
            foreach (var item in listaErrores)
            {
                
                ws.Cells[rowData, colNum].Value = n++;
                ws.Cells[rowData, colDescripcion].Value = item.Proerrmsg;

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowData, colNum, rowData, colNum, "Centro");
                UtilExcel.BorderCeldas2(ws, rowData, colNum, rowData, colDescripcion);

                rowData++;
            }
            

            var rangotabla = ws.Cells[rowIniTabla + 1, colNum, rowData, colDescripcion];
            rangotabla.SetFont(fuenteTabla);

            #endregion

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniTabla + 1, 1);

        }

        #endregion


        #region Factores de Utilizacion

        /// <summary>
        /// Devuelve el listado de factores de utilizacion para cierto rango en html
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="existeErrores"></param>
        /// <returns></returns>
        public string GenerarHtmlFactoresUtilizacion(string url, DateTime fechaIni, DateTime fechaFin, out bool existeErrores)
        {
            List<CoProcesoDiarioDTO> listaFactores = ObtenerListadoFactoresUtilizacion(fechaIni, fechaFin);
            existeErrores = listaFactores.Where(g => g.Prodiaestado == "F").Any();
            

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_lstFactores' style='width:100%'>");
            str.Append("<thead>");

            #region cabecera            
            str.Append("<tr>");
            str.Append("<th style='width: 120px;'>Acciones</th>");
            str.Append("<th style='width: 180px;'>Fecha Proceso</th>");
            str.Append("<th style='width: 240px;'>Cálculo Alfa y Beta</th>");
            str.Append("<th style='width: 180px;'>Fecha Ejecución</th>");
            str.Append("<th style='width: 180px;'>Usuario Ejecución</th>");
            str.Append("<th style='width: 180px;'>Tipo Proceso</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");

            foreach (var item in listaFactores)
            {
                CoPeriodoDTO periodoMensual = ObtenerPeriodoMensual(item.Prodiafecha.Value.ToString(ConstantesAppServicio.FormatoFecha));

                bool esCorrecto = (item.Prodiaestado == "E") ? true : false;
                string strEstado = esCorrecto ? "Se Ejecutó correctamente" : "No se ejecutó";

                string claseFila = !esCorrecto ? "fila_error" : "fila_correcta";
                
                int ancho = !esCorrecto ? ((periodoMensual != null && periodoMensual.Coperestado == "A") ? 40 : 20) : 20;

                str.AppendFormat("<tr>");

                str.AppendFormat("<td style='width: 120px'>");
                str.AppendFormat("<div style='width: {0}px; margin: 0 auto;'>", ancho);

                if (!esCorrecto)
                {
                    if (periodoMensual != null && periodoMensual.Coperestado == "A")
                    {
                        str.AppendFormat("<a class='' href='JavaScript:reprocesarCalculo({0});' ><img style='margin-top: 4px; margin-bottom: 4px; cursor:pointer;' src='{1}Content/Images/btn-procesar.png' title='Reprocesar Cálculo' alt='Reprocesar Cálculo' /></a>", item.Prodiacodi, url);
                    }
                    
                    str.AppendFormat("<a class='' href='JavaScript:mostrarReporteError({0});' ><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-properties-2.png' alt='Descargar reporte de Errores' title='Descargar reporte de Errores' /></a>", item.Prodiacodi, url);
                }
                else
                {
                    str.AppendFormat("<a class='' href='JavaScript:descargarResultados({0});' ><img style=' margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/exportarExcel_.png' alt='Descargar Resultados' title='Descargar Resultados' /></a>", item.Prodiacodi, url);
                }

                str.Append("</div>");
                str.Append("</td>");

                str.AppendFormat("<td  style='width: 180px; text-align: center'>{0}</td>", item.ProdiafechaDesc);
                str.AppendFormat("<td class='{0}' style='width: 240px; text-align: center'>{1}</td>", claseFila, strEstado);
                str.AppendFormat("<td  style='width: 180px; text-align: center'>{0}</td>", item.ProdiafecmodificacionDesc);
                str.AppendFormat("<td  style='width: 180px; text-align: center'>{0}</td>", item.Prodiausumodificacion);
                str.AppendFormat("<td  style='width: 180px; text-align: center'>{0}</td>", item.ProdiaindreprocesoDesc);
                str.Append("</tr>");

            }
            str.Append("</tbody>");

            return str.ToString();
        }

        /// <summary>
        /// Reprocesa todos losdias con errores
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="usuario"></param>
        public void ReprocesarCalculoTodos(DateTime fechaIni, DateTime fechaFin, string usuario)
        {
            List<CoProcesoDiarioDTO> listaFactores = ObtenerListadoFactoresUtilizacion(fechaIni, fechaFin);
            List<CoProcesoDiarioDTO> listaFactoresAReprocesar = listaFactores.Where(g => g.Prodiaestado == "F").ToList();

            foreach (var pdiario in listaFactoresAReprocesar)
            {
                bool hayEjecucionEnCurso = EjecutarProcesoDiario(pdiario.Prodiafecha.Value.Date, ConstantesCostoOportunidad.TipoReproceso, usuario);

            }
        }

        /// <summary>
        /// Devuelve el listado de factores
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        private List<CoProcesoDiarioDTO> ObtenerListadoFactoresUtilizacion(DateTime fechaIni, DateTime fechaFin)
        {
            List<CoProcesoDiarioDTO> lstSalida = new List<CoProcesoDiarioDTO>();

            lstSalida = FactorySic.GetCoProcesoDiarioRepository().ListarPorRangoFechas(fechaIni.ToString(ConstantesAppServicio.FormatoFecha), fechaFin.ToString(ConstantesAppServicio.FormatoFecha), ConstantesCostoOportunidad.ProdiatipoDiario).OrderBy(x => x.Prodiafecha).ToList();
            foreach (var reg in lstSalida)
                this.FormatearProcesoDiario(reg);

            return lstSalida;
        }

        /// <summary>
        /// Permite obtener el reporte de factores de utilización
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string[][] ConsultaFactoresUtilizacion(int idVersion, DateTime fechaInicio, DateTime fechaFin)
        {
            CoVersionDTO version = FactorySic.GetCoVersionRepository().GetById(idVersion);
         
            List<CoFactorUtilizacionDTO> entitys = 
                entitys = FactorySic.GetCoFactorUtilizacionRepository().ObtenerReporteDiario(
                    fechaInicio, fechaFin);

            //- Haremos ajuste en data

            string[][] data = new string[entitys.Count + 1][];

            string[] headers = new string[3];
            headers[0] = "Fecha - Hora";
            headers[1] = "Factor de utilización a subir (Alpha)";
            headers[2] = "Factor de utilización a bajar (Beta)";
            data[0] = headers;

            int index = 1;
            foreach (CoFactorUtilizacionDTO entity in entitys)
            {
                entity.FechaHora = entity.Prodiafecha.AddMinutes((int)(entity.Perprgvalor * 60) * (int)entity.Facutiperiodo).
                    ToString(ConstantesAppServicio.FormatoFechaFull);
                string[] row = new string[3];
                row[0] = entity.FechaHora;
                row[1] = entity.Facutialfa.ToString();
                row[2] = entity.Facutibeta.ToString();
                data[index] = row;
                index++;
            }

            return data;
        }

        /// <summary>
        /// Genera el reporte de resultados para cierto dia
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="procDiario"></param>
        /// <param name="nameFile"></param>
        public void GenerarReporteDeResultados(string ruta, CoProcesoDiarioDTO procDiario, string nameFile)
        {
            //Descargo archivo segun requieran
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            List<CoFactorUtilizacionDTO> listaFactores = FactorySic.GetCoFactorUtilizacionRepository().ObtenerReporteResultados(procDiario.Prodiacodi);            

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarExcelResultados(xlPackage, listaFactores);

                xlPackage.Save();

            }
        }

        /// <summary>
        /// Genera la estructura del reporte de resultados a exportar
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="listaFactores"></param>
        private void GenerarExcelResultados(ExcelPackage xlPackage, List<CoFactorUtilizacionDTO> listaFactores)
        {
            ExcelWorksheet ws = null;

            string nameWS = "Resultados";

            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            string fontFamily = "Arial Narrow";

            var fuenteTabla = new Font(fontFamily, 11);
            

            int colIniTable = 3;
            int rowIniTabla = 5;
            int colIniDynamic = colIniTable;
            

            foreach (var columnWidth in new List<double>() { 20, 25, 25 })//columna A-I 
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }                       

            #region  Filtros y Cabecera

            var colFechaHora = colIniTable;
            int colAlpha = colIniTable + 1;
            int colBeta = colIniTable + 2;

            ws.Cells[rowIniTabla - 2, colIniTable].Value = "Reporte de Factores de Utilización";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colBeta, "Arial Narrow", 14);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colBeta);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colBeta);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colBeta, "Centro");

            ws.Row(rowIniTabla).Height = 45;
            ws.Cells[rowIniTabla, colFechaHora].Value = "Fecha - Hora";
            ws.Cells[rowIniTabla, colAlpha].Value = "Factor de utilización a subir (Alpha)";
            ws.Cells[rowIniTabla, colBeta].Value = "Factor de utilización a bajar (Beta)";


            var rangoCab = ws.Cells[rowIniTabla, colFechaHora, rowIniTabla, colBeta];
            rangoCab.SetAlignment();

            UtilExcel.CeldasExcelColorTexto(ws, rowIniTabla, colFechaHora, rowIniTabla, colBeta, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniTabla, colFechaHora, rowIniTabla, colBeta, "#0D43A5");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colFechaHora, rowIniTabla, colBeta, "Arial", 11);
            UtilExcel.BorderCeldas2(ws, rowIniTabla, colFechaHora, rowIniTabla, colBeta);

            UtilExcel.CeldasExcelWrapText(ws, rowIniTabla, colAlpha, rowIniTabla, colAlpha);
            UtilExcel.CeldasExcelWrapText(ws, rowIniTabla, colBeta, rowIniTabla, colBeta);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            
            foreach (var item in listaFactores)
            {
                item.FechaHora = item.Prodiafecha.AddMinutes((int)(item.Perprgvalor * 60) * (int)item.Facutiperiodo).ToString(ConstantesAppServicio.FormatoFechaFull);


                ws.Cells[rowData, colFechaHora].Value = item.FechaHora;
                ws.Cells[rowData, colAlpha].Value = item.Facutialfa;
                ws.Cells[rowData, colBeta].Value = item.Facutibeta;

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowData, colFechaHora, rowData, colFechaHora, "Centro");
                UtilExcel.BorderCeldas2(ws, rowData, colFechaHora, rowData, colBeta);

                rowData++;
            }
            ws.Cells[rowIniTabla + 1, colAlpha, rowData, colBeta].Style.Numberformat.Format = "#,####0.0000";


            var rangotabla = ws.Cells[rowIniTabla + 1, colFechaHora, rowData, colBeta];
            rangotabla.SetFont(fuenteTabla);

            #endregion

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniTabla + 1, 1);

        }

        /// <summary>
        /// Obtiene registros con el valor de factores de presencia
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tipoProceso"></param>
        /// <param name="periodo"></param>
        /// <param name="idVersion"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CoMedicion60DTO> CalcularFactoresPresencia(List<CoMedicion60DTO> list, string tipoProceso, int periodo, int idVersion, DateTime fecha)
        {
            List<CoMedicion60DTO> result = new List<CoMedicion60DTO>();

            //- Debemos obtener la versíón para la fecha procesada
            CoVersionDTO version = null;
            if (tipoProceso == "M") version = FactorySic.GetCoVersionRepository().GetById(idVersion);
            else version = FactorySic.GetCoVersionRepository().ObtenerVersionPorFecha(fecha);

            //- Obtenemos el listado de configuraciones
            List<CoConfiguracionDetDTO> configuracionURS = (FactorySic.GetCoConfiguracionDetRepository().ObtenerConfiguracion
                ((int)version.Copercodi, version.Covercodi)).Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoOperacionURS).
                OrderBy(x => x.Courdevigenciadesde).ToList();

            //- Listado de URS
            List<CoMedicion48DTO> listUrs = FactorySic.GetCoMedicion48Repository().ObtenerListadoURS();

            //- Analizamos la configuración de cada una de las señales
            foreach (CoMedicion48DTO itemUrs in listUrs)
            {
                List<CoConfiguracionDetDTO> configURS = configuracionURS.Where(x => x.Grupocodi == itemUrs.Grupocodi).ToList();
                DateTime fecInicioHab = DateTime.MinValue;
                DateTime fecFinHab = DateTime.MaxValue;
                if (configURS.Count > 0)
                {
                    fecInicioHab = (configURS[0].FecInicioHabilitacion != null) ? (DateTime)configURS[0].FecInicioHabilitacion : DateTime.MinValue;
                    fecFinHab = (configURS[0].FecFinHabilitacion != null) ? (DateTime)configURS[0].FecFinHabilitacion : DateTime.MaxValue;
                }
                //- Validamos fechas de habilitación
                bool flagHabilitacion = true;
                if (!(fecFinHab.Subtract(fecha).TotalSeconds > 0 && fecha.Subtract(fecInicioHab).TotalSeconds > 0))
                    flagHabilitacion = false;
                itemUrs.Habilitado = flagHabilitacion;
            }            

            var subList = list.GroupBy(x => new { x.Grupocodi, x.Equicodi, x.Comedihora, x.Comediminuto }).ToList();

            foreach (var itemList in subList)
            {
                if (listUrs.Where(x => x.Grupocodi == itemList.Key.Grupocodi && x.Habilitado).Count() > 0)
                {
                    if (itemList.Count() == 6)
                    {
                        CoMedicion60DTO entityFP = new CoMedicion60DTO();
                        CoMedicion60DTO entityFU = new CoMedicion60DTO();
                        CoMedicion60DTO entityFU1 = new CoMedicion60DTO();

                        entityFP.Grupocodi = itemList.Key.Grupocodi;
                        entityFP.Equicodi = itemList.Key.Equicodi;
                        entityFP.Comedihora = itemList.Key.Comedihora;
                        entityFP.Comediminuto = itemList.Key.Comediminuto;
                        entityFP.Cotidacodi = 7;
                        entityFP.Fecha = fecha;

                        entityFU.Grupocodi = itemList.Key.Grupocodi;
                        entityFU.Equicodi = itemList.Key.Equicodi;
                        entityFU.Comedihora = itemList.Key.Comedihora;
                        entityFU.Comediminuto = itemList.Key.Comediminuto;
                        entityFU.Cotidacodi = 8;
                        entityFU.Fecha = fecha;

                        entityFU1.Grupocodi = itemList.Key.Grupocodi;
                        entityFU1.Equicodi = itemList.Key.Equicodi;
                        entityFU1.Comedihora = itemList.Key.Comedihora;
                        entityFU1.Comediminuto = itemList.Key.Comediminuto;
                        entityFU1.Cotidacodi = 9;
                        entityFU1.Fecha = fecha;

                        CoMedicion60DTO senial01 = null;
                        CoMedicion60DTO senial02 = null;
                        CoMedicion60DTO senial03 = null;
                        CoMedicion60DTO senial04 = null;
                        CoMedicion60DTO senial05 = null;
                        CoMedicion60DTO senial06 = null;

                        foreach (var reg in itemList)
                        {
                            if (reg.Cotidacodi == ConstantesCostoOportunidad.TipoDatosIds[0]) senial01 = reg;
                            if (reg.Cotidacodi == ConstantesCostoOportunidad.TipoDatosIds[1]) senial02 = reg;
                            if (reg.Cotidacodi == ConstantesCostoOportunidad.TipoDatosIds[2]) senial03 = reg;
                            if (reg.Cotidacodi == ConstantesCostoOportunidad.TipoDatosIds[3]) senial04 = reg;
                            if (reg.Cotidacodi == ConstantesCostoOportunidad.TipoDatosIds[4]) senial05 = reg;
                            if (reg.Cotidacodi == ConstantesCostoOportunidad.TipoDatosIds[5]) senial06 = reg;
                        }

                        for (int i = 1; i <= 60; i++)
                        {
                            object obj01 = senial01.GetType().GetProperty("H" + i).GetValue(senial01);
                            object obj02 = senial02.GetType().GetProperty("H" + i).GetValue(senial02);
                            object obj03 = senial03.GetType().GetProperty("H" + i).GetValue(senial03);
                            object obj04 = senial04.GetType().GetProperty("H" + i).GetValue(senial04);
                            object obj05 = senial05.GetType().GetProperty("H" + i).GetValue(senial05);
                            object obj06 = senial06.GetType().GetProperty("H" + i).GetValue(senial06);

                            decimal val01 = (obj01 != null) ? (decimal)obj01 : 0;
                            decimal val02 = (obj02 != null) ? (decimal)obj02 : 0;
                            decimal val03 = (obj03 != null) ? (decimal)obj03 : 0;
                            decimal val04 = (obj04 != null) ? (decimal)obj04 : 0;
                            decimal val05 = (obj05 != null) ? (decimal)obj05 : 0;
                            decimal val06 = (obj06 != null) ? (decimal)obj06 : 0;

                            decimal fp = val01 * val02 * (val03 - val04) == 0 ? 0 : 1;
                            decimal fu = Math.Max(val05 - val06, 0) * fp;
                            decimal fu1 = Math.Max(val06 - val05, 0) * fp;

                            entityFP.GetType().GetProperty("H" + i).SetValue(entityFP, fp);
                            entityFU.GetType().GetProperty("H" + i).SetValue(entityFU, fu);
                            entityFU1.GetType().GetProperty("H" + i).SetValue(entityFU1, fu1);
                        }

                        result.Add(entityFP);
                        result.Add(entityFU);
                        result.Add(entityFU1);
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Permtite realizar el cálculo de las alftas y betas
        /// </summary>
        /// <param name="proceso"></param>      
        /// <returns></returns>
        public int CalcularFactoresUtilizacíon(CoProcesoDiarioDTO proceso)
        {
            try
            {
                //- Estructura para agregar los factores de utilización
                List<CoFactorUtilizacionDTO> result = new List<CoFactorUtilizacionDTO>();

                //- Listado de URS
                List<CoMedicion48DTO> listUrs = FactorySic.GetCoMedicion48Repository().ObtenerListadoURS();

                string nombreTabla = string.Empty;
                //- Debemos obtener la versíón para la fecha procesada
                CoVersionDTO version = null;
                if (proceso.Prodiatipo == "D")
                {
                    version = FactorySic.GetCoVersionRepository().ObtenerVersionPorFecha(proceso.Prodiafecha.Value);
                    CoPeriodoDTO periodo = FactorySic.GetCoPeriodoRepository().GetById((int)version.Copercodi);
                    nombreTabla = "CO_MEDICION60_" + periodo.Coperanio + string.Format("{0:D2}", periodo.Copermes);
                    
                }
                else
                {
                    version = FactorySic.GetCoVersionRepository().GetById((int)proceso.Covercodi);
                    CoPeriodoDTO periodo = FactorySic.GetCoPeriodoRepository().GetById((int)version.Copercodi);
                    nombreTabla = "CO_MEDICION60_" + periodo.Coperanio + string.Format("{0:D2}", periodo.Copermes) + "_" + version.Covebacodi;
                }

                //- Obtenemos el listado de configuraciones
                List<CoConfiguracionDetDTO> configuracionURS = (FactorySic.GetCoConfiguracionDetRepository().ObtenerConfiguracion
                    ((int)version.Copercodi, version.Covercodi)).Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoOperacionURS).
                    OrderBy(x => x.Courdevigenciadesde).ToList();

                //- Analizamos la configuración de cada una de las señales
                foreach (CoMedicion48DTO itemUrs in listUrs)
                {
                    List<CoConfiguracionDetDTO> configURS = configuracionURS.Where(x => x.Grupocodi == itemUrs.Grupocodi).ToList();
                    DateTime fecInicioHab = DateTime.MinValue;
                    DateTime fecFinHab = DateTime.MaxValue;
                    if (configURS.Count > 0)
                    {
                        fecInicioHab = (configURS[0].FecInicioHabilitacion != null) ? (DateTime)configURS[0].FecInicioHabilitacion : DateTime.MinValue;
                        fecFinHab = (configURS[0].FecFinHabilitacion != null) ? (DateTime)configURS[0].FecFinHabilitacion : DateTime.MaxValue;
                    }
                    //- Validamos fechas de habilitación
                    bool flagHabilitacion = true;
                    if (!(fecFinHab.Subtract(proceso.Prodiafecha.Value).TotalSeconds > 0 && proceso.Prodiafecha.Value.Subtract(fecInicioHab).TotalSeconds > 0))
                        flagHabilitacion = false;
                    itemUrs.Habilitado = flagHabilitacion;
                }

                //- Obtener el periodo de programación vigente para la fecha
                CoPeriodoProgDTO periodoProg = FactorySic.GetCoPeriodoProgRepository().ObtenerPeriodoProgVigente(proceso.Prodiafecha.Value);

                //- Obtenemos la data
                List<CoMedicion60DTO> data = FactorySic.GetCoMedicion60Repository().ObtenerDataReporteFU(proceso.Prodiacodi, nombreTabla, 8);
                decimal[] factores = new decimal[86400];

                List<CoMedicion60DTO> dataBeta = FactorySic.GetCoMedicion60Repository().ObtenerDataReporteFU(proceso.Prodiacodi, nombreTabla, 9);
                decimal[] factoresBeta = new decimal[86400];

                List<CoMedicion48DTO> listReservaSubir = new List<CoMedicion48DTO>();
                List<CoMedicion48DTO> listReservaBajar = new List<CoMedicion48DTO>();

                this.ObtenerReservaFP(version, (DateTime)(proceso.Prodiafecha), (DateTime)(proceso.Prodiafecha), out listReservaSubir, out listReservaBajar);

                //- Obtenemos la data de la reserva para subir y para bajar
                List<CoMedicion48DTO> listaRaEjecUp = listReservaSubir
                    .Where(x => (listUrs.Where(m => m.Habilitado)).Any(y => x.Grupocodi == y.Grupocodi)).ToList();
                List<CoMedicion48DTO> listaRaEjeDown = listReservaBajar
                    .Where(x => (listUrs.Where(m => m.Habilitado)).Any(y => x.Grupocodi == y.Grupocodi)).ToList();

                string[][] dataRAS = this.TransformarDataReservaEjecutada(listaRaEjecUp);
                string[][] dataRAB = this.TransformarDataReservaEjecutada(listaRaEjeDown);

                if (data.Count == 1440)
                {
                    int index = 0;
                    foreach (CoMedicion60DTO item in data)
                    {
                        for (int i = 1; i <= 60; i++)
                        {
                            object obj = item.GetType().GetProperty("H" + i).GetValue(item);
                            decimal valor = (obj != null) ? (decimal)obj : 0;
                            factores[index] = valor;
                            index++;
                        }
                    }

                    index = 0;
                    foreach (CoMedicion60DTO item in dataBeta)
                    {
                        for (int i = 1; i <= 60; i++)
                        {
                            object obj = item.GetType().GetProperty("H" + i).GetValue(item);
                            decimal valor = (obj != null) ? (decimal)obj : 0;
                            factoresBeta[index] = valor;
                            index++;
                        }
                    }

                    //- Realizamos el calculo por cada bloque
                    int bloques = (int)(24 / periodoProg.Perprgvalor);
                    int segundos = (int)(periodoProg.Perprgvalor * 60 * 60);
                    DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    for (int i = 0; i < bloques; i++)
                    {
                        DateTime fecInicio = fecha.AddSeconds(i * segundos);
                        DateTime fecFin = fecha.AddSeconds((i + 1) * segundos - 1);

                        CoFactorUtilizacionDTO item = new CoFactorUtilizacionDTO();

                        decimal alfa = 0;
                        decimal beta = 0;
                        decimal valor = 0;
                        decimal valorBeta = 0;
                        decimal ras = 0;
                        decimal rab = 0;

                        for (int k = (i * segundos); k < (i * segundos) + segundos; k++)
                        {
                            valor = valor + factores[k];
                            valorBeta = valorBeta + factoresBeta[k];
                        }

                        int add = 0;
                        if (fecInicio.Minute == 0 || fecInicio.Minute == 30) add = 1;
                        int periodoInicio = this.CalcularPeriodo(fecInicio) + add;
                        int periodoFin = this.CalcularPeriodo(fecFin);

                        decimal minutos = 30M;
                        if (periodoProg.Perprgvalor < 0.5M) minutos = 15M;

                        for (int k = periodoInicio - 1; k <= periodoFin - 1; k++)
                        {
                            if (periodoProg.Perprgvalor == 0.75M)
                            {
                                if (minutos == 30M) minutos = 15M;
                                else minutos = 30M;
                            }

                            for (int j = 1; j < dataRAS[k + 1].Length; j++)
                            {
                                ras = ras + (decimal.Parse(dataRAS[k + 1][j])) * minutos / 30M;
                            }
                            for (int j = 1; j < dataRAB[k + 1].Length; j++)
                            {
                                rab = rab + (decimal.Parse(dataRAB[k + 1][j])) * minutos / 30M;
                            }
                        }

                        if (ras > 0) alfa = valor / (ras * segundos);
                        if (rab > 0) beta = valorBeta / (rab * segundos);

                        item.Facutialfa = alfa;
                        item.Facutibeta = beta;
                        item.Facutiperiodo = (i + 1);
                        item.Prodiacodi = proceso.Prodiacodi;
                        item.Facutifeccreacion = DateTime.Now;
                        item.Facutiusucreacion = proceso.Prodiausucreacion;

                        result.Add(item);
                    }

                    //- Debemos eliminar la data previa de los factores de utilización
                    FactorySic.GetCoFactorUtilizacionRepository().Delete(proceso.Prodiacodi);

                    //- Realizamos el almacenado de los datos
                    foreach (CoFactorUtilizacionDTO item in result)
                    {
                        FactorySic.GetCoFactorUtilizacionRepository().Save(item);
                    }

                    //- Actualizamos los datos de la cabecera
                    proceso.Perprgcodi = periodoProg.Perprgcodi;
                    proceso.Prodiafecmodificacion = DateTime.Now;
                    proceso.Prodiausumodificacion = proceso.Prodiausucreacion;
                    FactorySic.GetCoProcesoDiarioRepository().Update(proceso);

                    return 1; //- Calculo realizado correctamente
                }
                else
                {
                    return 2; //- No existen datos completos de los factores de presencia
                }


            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1; //- Error en el procesamiento de datos
            }
        }

        /// <summary>
        /// Permite obtener los datos de la reserva
        /// </summary>
        /// <param name="listRAUp"></param>
        /// <param name="listRADown"></param>
        /// <param name="listRAEject"></param>      
        public void ObtenerReservaFP(CoVersionDTO version, DateTime fechaInicio, DateTime fechaFin,
            out List<CoMedicion48DTO> resultRSEject, out List<CoMedicion48DTO> resultRBEject)
        {
            try
            {                
                List<CoMedicion48DTO> listRAEject = new List<CoMedicion48DTO>();
                List<CoRaejecutadadetDTO> listDetalleRA = new List<CoRaejecutadadetDTO>();
                listRAEject = FactorySic.GetCoMedicion48Repository().ObtenerDatosRAEjecutado(fechaInicio, fechaFin, out listDetalleRA);              

                //- Obteniendo la estructura de RA ejecutada
                List<CoMedicion48DTO> listSubEjec = new List<CoMedicion48DTO>();
                List<CoMedicion48DTO> listBajEjec = new List<CoMedicion48DTO>();
                this.TransformarDataRA(listRAEject, version, out listSubEjec, out listBajEjec);
                
                List<CoMedicion48DTO> dataRS = new List<CoMedicion48DTO>();

                foreach (CoMedicion48DTO item in listSubEjec)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        object result = item.GetType().GetProperty("H" + i).GetValue(item);

                        if (result != null) valor = Convert.ToDecimal(result);

                        CoMedicion48DTO entity = new CoMedicion48DTO();

                        entity.Famcodi = item.Famcodi;
                        entity.Grupocodi = item.Grupocodi;
                        entity.Equicodi = item.Equicodi;
                        entity.Comedfecha = item.Comedfecha;
                        entity.Gruponomb = item.Gruponomb;
                        entity.Ursnomb = item.Ursnomb;
                        entity.Valor = valor;
                        entity.Orden = i;

                        dataRS.Add(entity);
                    }
                }

                List<CoMedicion48DTO> dataRB = new List<CoMedicion48DTO>();

                foreach (CoMedicion48DTO item in listBajEjec)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        object result = item.GetType().GetProperty("H" + i).GetValue(item);

                        if (result != null) valor = Convert.ToDecimal(result);

                        CoMedicion48DTO entity = new CoMedicion48DTO();

                        entity.Famcodi = item.Famcodi;
                        entity.Grupocodi = item.Grupocodi;
                        entity.Equicodi = item.Equicodi;
                        entity.Comedfecha = item.Comedfecha;
                        entity.Gruponomb = item.Gruponomb;
                        entity.Ursnomb = item.Ursnomb;
                        entity.Valor = valor;
                        entity.Orden = i;

                        dataRB.Add(entity);
                    }
                }

                resultRSEject = dataRS;
                resultRBEject = dataRB;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite mostrar los datos en la estructura correcta
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected string[][] TransformarDataReservaEjecutada(List<CoMedicion48DTO> result)
        {
            List<DateTime> fechas = result.Select(x => x.Comedfecha).Distinct().OrderBy(x => x).ToList();
            string[][] datos = new string[fechas.Count * 48 + 1][];

            var cabeceras = result.Select(x => new { x.Famcodi, x.Equicodi, x.Grupocodi, x.Gruponomb, x.Ursnomb }).Distinct().ToList();

            string[] header = new string[cabeceras.Count + 1];
            header[0] = "FechaHora/URS";

            int index = 1;
            foreach (var item in cabeceras)
            {
                header[index] = item.Grupocodi.ToString();
                index++;
            }
            datos[0] = header;
            index = 1;

            foreach (DateTime fecha in fechas)
            {
                for (int k = 1; k <= 48; k++)
                {
                    datos[index] = new string[cabeceras.Count + 1];
                    datos[index][0] = k.ToString();
                    index++;
                }
                int col = 1;
                try
                {
                    foreach (var cabecera in cabeceras)
                    {
                        List<CoMedicion48DTO> list = result.Where(x => x.Comedfecha == fecha && ((x.Grupocodi == cabecera.Grupocodi))).OrderBy(x => x.Orden).ToList();

                        if (list.Count == 48)
                        {
                            index = index - 48;
                            for (int i = 1; i <= 48; i++)
                            {
                                datos[index][col] = list[i - 1].Valor.ToString();
                                index++;
                            }
                        }

                        col++;
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }

            return datos;
        }

        /// <summary>
        /// Calcula el periodo en el que se está ejecutando el proceso
        /// </summary>
        /// <returns></returns>
        public int CalcularPeriodo(DateTime fecha)
        {
            int totalMinutes = fecha.Hour * 60 + fecha.Minute;
            return Convert.ToInt32(Math.Ceiling(((decimal)totalMinutes / 30.0M)));
        }

        /// <summary>
        /// Permite ejecutar el proceso diario para una fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public bool EjecutarProcesoDiario(DateTime fecha, string tipo, string usuario)
        {
            try
            {
                bool salida = false;
                //- Debemos obtener la versíón para la fecha procesada
                CoVersionDTO version = FactorySic.GetCoVersionRepository().ObtenerVersionPorFecha(fecha);                
                if (version == null) throw new ArgumentException("No se encontró una versión con configuración URS para la fecha ingresada. Verificar que exista configuración URS para dicha fecha.");

                //- Realizamos el importado de los datos SP7 y calculamos los factores de presencia
                this.ImportarTodoSeñalesSP7(ConstantesCostoOportunidad.ImportarSeniales_Diario, fecha, (int)version.Copercodi, 
                    version.Covercodi, usuario, tipo, out bool hayEjecucionEnCurso);

                salida = hayEjecucionEnCurso;

                if (!hayEjecucionEnCurso)
                {
                    //- Obtener el proceso diaro asociado
                    CoProcesoDiarioDTO proceso = FactorySic.GetCoProcesoDiarioRepository().ObtenerProcesoDiario(fecha);

                    //- Calculamos los factores de utilizacion               
                    this.CalcularFactoresUtilizacíon(proceso);
                }
                return salida;
            }
            catch (Exception ex)
            {
                //Si ocurrio un error al importar sp7, indico que terminó la ejecución en curso
                ActualizarEstadoEjecucion(ConstantesCostoOportunidad.EstadoNoHayEjecucionEnCurso, usuario);
                
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
                
            }
        }

        /// <summary>
        /// Reemplaza valroes de Fact Utilizacion de forma manual
        /// </summary>
        /// <param name="listaDataExcel"></param>
        /// <param name="usuario"></param>
        public void ReemplazarValoresFUManualmente(List<FactorUtilizacionExcel> listaDataExcel, string usuario)
        {
            var lstAgrupadaPorDia = listaDataExcel.GroupBy(x => x.FechaDia.Date).ToList();

            foreach (var grupoDiario in lstAgrupadaPorDia)
            {
                DateTime diaDate = grupoDiario.Key;                

                foreach (var reg in grupoDiario) 
                {                    
                    DateTime dia = reg.FechaDia;
                    int hora = dia.Hour;
                    int minuto = dia.Minute;

                    if (minuto == 0 || minuto == 30) {
                        decimal? alpha = reg.ValorAlpha;
                        decimal? beta = reg.ValorBeta;

                        CoProcesoDiarioDTO procesoDiario = new CoProcesoDiarioDTO();
                        int periodo = -1;
                        if (hora == 0 && minuto == 0)
                        {
                            procesoDiario = FactorySic.GetCoProcesoDiarioRepository().ObtenerProcesoDiario(dia.AddDays(-1));
                            periodo = 48;
                        }
                        else
                        {
                            procesoDiario = FactorySic.GetCoProcesoDiarioRepository().ObtenerProcesoDiario(dia);

                            if (minuto == 0)
                            {
                                periodo = 2 * hora;
                            }
                            if (minuto == 30)
                            {
                                periodo = 2 * hora + 1;
                            }
                        }

                        CoFactorUtilizacionDTO regFU= FactorySic.GetCoFactorUtilizacionRepository().GetByProdiacodiYPeriodo(procesoDiario.Prodiacodi, periodo);

                        if (regFU != null)
                        {
                            regFU.Facutialfa = alpha;
                            regFU.Facutibeta = beta;
                            regFU.Facutifecmodificacion = DateTime.Now;
                            regFU.Facutiusumodificacion = usuario;

                            UpdateCoFactorUtilizacion(regFU);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Obtiene la informacion del excel subido
        /// </summary>
        /// <param name="stremExcel"></param>
        /// <param name="user"></param>
        /// <param name="listaCorrecta"></param>
        /// <param name="listaErrores"></param>
        public void ObtenerDataExcel(Stream stremExcel, string user, out List<FactorUtilizacionExcel> listaCorrecta, out List<FactorUtilizacionErrorExcel> listaErrores)
        {
            try
            {
                List<FactorUtilizacionExcel> listaFUE = new List<FactorUtilizacionExcel>();
                List<FactorUtilizacionErrorExcel> listaErroresEncontrados = new List<FactorUtilizacionErrorExcel>();

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
                        if(CabExcel[0, i] != null)
                            textosCab = textosCab + " " + CabExcel[0, i].ToString();
                    }
                    if(textosCab.ToUpper().Contains("FECHA") && textosCab.ToUpper().Contains("ALPHA") && textosCab.ToUpper().Contains("BETA"))
                    {

                    }
                    else
                    {
                        throw new Exception("Tabla con cabecera no reconocida.");
                    }

                    //validar estructura de la tabla
                    if (!CabExcel[0,0].ToString().ToUpper().Contains("FECHA") )
                        throw new Exception("Primera Columna (Fecha - Hora) mal posicionada.");
                    if (!CabExcel[0, 1].ToString().ToUpper().Contains("ALPHA"))
                        throw new Exception("Segunda Columna (Alpha) mal posicionada.");
                    if (!CabExcel[0, 2].ToString().ToUpper().Contains("BETA"))
                        throw new Exception("Tercera Columna (Beta) mal posicionada.");

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


                    for (int fila = filaCabecera; fila <= ultimaFilatBloque; fila++)
                    {
                        var excelDia = dataExcel[fila, dictionary[lstCab[0]]] != null ? dataExcel[fila, dictionary[lstCab[0]]].ToString().Trim() : "";
                        var excelAlpha = dataExcel[fila, dictionary[lstCab[1]]] != null ? dataExcel[fila, dictionary[lstCab[1]]].ToString().Trim() : "";
                        var excelBeta =  dataExcel[fila, dictionary[lstCab[2]]] != null ? dataExcel[fila, dictionary[lstCab[2]]].ToString().Trim() : "";



                        DateTime valorDia;
                        decimal valorAlpha;
                        decimal valorBeta;

                        #region Validación de campos
                        this.BuscarErrorFechaEnCelda(fila, lstCab[0], excelDia, out valorDia, ref listaErroresEncontrados);
                        this.BuscarErrorValorEnCelda(fila, lstCab[1], excelAlpha, out valorAlpha, ref listaErroresEncontrados);
                        this.BuscarErrorValorEnCelda(fila, lstCab[2], excelBeta, out valorBeta, ref listaErroresEncontrados);

                        #endregion

                        listaFUE.Add(new FactorUtilizacionExcel()
                        {
                            FechaDia = valorDia,
                            ValorAlpha = valorAlpha,
                            ValorBeta = valorBeta

                        });
                    }
                }

                listaCorrecta = listaFUE;
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
        /// Elimina la primera fila
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public ExcelWorksheet EliminarPrimerasFilasVacias(ExcelWorksheet worksheet)
        {
            ExcelWorksheet worksheetSalida;

            while (EsPrimeraFilaVacia(worksheet))
            {
                ////worksheet.DeleteRow(worksheet.Dimension.Start.Row);
                ////worksheet.Dimension.Start.Row;
                //worksheet.Dimension.Start = new ExcelAddressBase(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.Start.Column, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column);
            }

            worksheetSalida = worksheet;

            return worksheetSalida;
        }

        /// <summary>
        /// Devuelve si la primera fila es vacia o no
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public bool EsPrimeraFilaVacia(ExcelWorksheet worksheet)
        {
            var lstVacios = new List<bool>();

            for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
            {
                var celdaVacia = worksheet.Cells[worksheet.Dimension.Start.Row, i].Value == null ? true : false;
                lstVacios.Add(celdaVacia);
            }

            return lstVacios.All(e => e);
        }

        /// <summary>
        /// Busca errores en valor de alpha y beta
        /// </summary>
        /// <param name="fila"></param>
        /// <param name="campo"></param>
        /// <param name="valorCelda"></param>
        /// <param name="valorNumerico"></param>
        /// <param name="listaErrores"></param>
        private void BuscarErrorValorEnCelda(int fila, string campo, string valorCelda, out decimal valorNumerico, ref List<FactorUtilizacionErrorExcel> listaErrores)
        {
            if (!decimal.TryParse(valorCelda, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out valorNumerico))
            {
                FactorUtilizacionErrorExcel error = new FactorUtilizacionErrorExcel();
                error.NumeroFilaExcel = fila + 1;
                error.CampoExcel = campo;
                error.ValorCeldaExcel = valorCelda;
                error.MensajeValidacion = "Valor numérico incorrecto.";

                listaErrores.Add(error);
            }
            else
            {
                if (Convert.ToDecimal(valorCelda) < 0)
                {
                    FactorUtilizacionErrorExcel error = new FactorUtilizacionErrorExcel();
                    error.NumeroFilaExcel = fila + 1;
                    error.CampoExcel = campo;
                    error.ValorCeldaExcel = valorCelda;
                    error.MensajeValidacion = "Valor numérico negativo.";

                    listaErrores.Add(error);
                }
            }



        }

        /// <summary>
        /// Busca error en campo fecha
        /// </summary>
        /// <param name="fila"></param>
        /// <param name="campo"></param>
        /// <param name="valorCelda"></param>
        /// <param name="valorNumerico"></param>
        /// <param name="listaErrores"></param>
        private void BuscarErrorFechaEnCelda(int fila, string campo, string valorCelda, out DateTime valorNumerico, ref List<FactorUtilizacionErrorExcel> listaErrores)
        {            
            if (!esCampoFechaCorrecto(valorCelda, out valorNumerico)) 
            { 
                FactorUtilizacionErrorExcel error = new FactorUtilizacionErrorExcel();
                error.NumeroFilaExcel = fila + 1;
                error.CampoExcel = campo;
                error.ValorCeldaExcel = valorCelda;
                error.MensajeValidacion = "Valor de fecha y hora (dd/mm/yyyy hh:mm) incorrecto.";

                listaErrores.Add(error);
            }


            else
            {
                string fechita = valorNumerico.ToString(ConstantesAppServicio.FormatoFechaHora);
                DateTime fechaConsulta = DateTime.ParseExact(fechita, ConstantesAppServicio.FormatoFechaHora, CultureInfo.InvariantCulture);

                int result = DateTime.Compare(fechaConsulta, DateTime.Now);

                if (result > 0)
                {
                    FactorUtilizacionErrorExcel error = new FactorUtilizacionErrorExcel();
                    error.NumeroFilaExcel = fila + 1;
                    error.CampoExcel = campo;
                    error.ValorCeldaExcel = valorCelda;
                    error.MensajeValidacion = "Valor de fecha y hora supera la fecha y hora actual.";

                    listaErrores.Add(error);
                }
                else
                {
                    if (fechaConsulta.Minute != 0 && fechaConsulta.Minute != 30)
                    {
                        FactorUtilizacionErrorExcel error = new FactorUtilizacionErrorExcel();
                        error.NumeroFilaExcel = fila + 1;
                        error.CampoExcel = campo;
                        error.ValorCeldaExcel = valorCelda;
                        error.MensajeValidacion = "Valor de fecha y hora no corresponde a los periodos de programación.";

                        listaErrores.Add(error);
                    }
                    else
                    {
                        //evaluo si existe informacion de procesos diarios 
                        CoProcesoDiarioDTO objPD = new CoProcesoDiarioDTO();

                        if (fechaConsulta.Hour == 0 && fechaConsulta.Minute == 0)
                        {
                            objPD = FactorySic.GetCoProcesoDiarioRepository().ObtenerProcesoDiario(fechaConsulta.AddDays(-1));

                        }
                        else
                        {
                            objPD = FactorySic.GetCoProcesoDiarioRepository().ObtenerProcesoDiario(fechaConsulta);
                        }

                        if (objPD == null)
                        {
                            FactorUtilizacionErrorExcel error = new FactorUtilizacionErrorExcel();
                            error.NumeroFilaExcel = fila + 1;
                            error.CampoExcel = campo;
                            error.ValorCeldaExcel = valorCelda;
                            error.MensajeValidacion = "No existe cálculo de Factor de Utilización para dicha fecha y hora.";

                            listaErrores.Add(error);
                        }
                    }
                }

            }

        }

        /// <summary>
        /// Valida campo fechas Datetime
        /// </summary>
        /// <param name="valorCelda"></param>
        /// <param name="valorNumerico"></param>
        /// <returns></returns>
        private bool esCampoFechaCorrecto(string valorCelda, out DateTime valorNumerico)
        {
            bool salida = true;

            if (!DateTime.TryParseExact(valorCelda, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out valorNumerico))//1 espacio
            {
                if (!DateTime.TryParseExact(valorCelda, "dd/MM/yyyy  HH:mm", null, System.Globalization.DateTimeStyles.None, out valorNumerico))//2 espacio
                {
                    if (!DateTime.TryParseExact(valorCelda, "dd/MM/yyyy   HH:mm", null, System.Globalization.DateTimeStyles.None, out valorNumerico))//3 espacio
                    {
                        if (!DateTime.TryParseExact(valorCelda, "d/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out valorNumerico))//1 espacio
                        {
                            if (!DateTime.TryParseExact(valorCelda, "d/MM/yyyy  HH:mm", null, System.Globalization.DateTimeStyles.None, out valorNumerico))//2 espacios
                            {
                                if (!DateTime.TryParseExact(valorCelda, "d/MM/yyyy   HH:mm", null, System.Globalization.DateTimeStyles.None, out valorNumerico))//3 espacios
                                {
                                    if (!DateTime.TryParseExact(valorCelda, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out valorNumerico)) // 1 espacio
                                    {
                                        if (!DateTime.TryParseExact(valorCelda, "dd/MM/yyyy  HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out valorNumerico)) // 2 espacio
                                        {
                                            if (!DateTime.TryParseExact(valorCelda, "dd/MM/yyyy   HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out valorNumerico)) // 3 espacio
                                            {
                                                if (!DateTime.TryParseExact(valorCelda, "d/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out valorNumerico))//1 espacio
                                                {
                                                    if (!DateTime.TryParseExact(valorCelda, "d/MM/yyyy  HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out valorNumerico))//2 espacio
                                                    {
                                                        if (!DateTime.TryParseExact(valorCelda, "d/MM/yyyy   HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out valorNumerico))//3 espacio
                                                        {
                                                            salida = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return salida;
        }
        /// <summary>
        /// Devuelve los errores en html
        /// </summary>
        /// <param name="listaErrores"></param>
        /// <returns></returns>
        public string ObtenerTablaErroresHtml(List<FactorUtilizacionErrorExcel> listaErrores)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table id='tablaErrores' class='pretty tabla-adicional'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style=''>Número de registro</th>");
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
                strHtml.Append("<td style='text-align: center;'>" + fila.NumeroFilaExcel + " </td>");
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

        #endregion

        #endregion

        #region Métodos Tabla CO_CONFIGURACION_GEN


        /// <summary>
        /// Permite obtener la configuracion para las señales
        /// </summary>
        /// <param name="idCondiguracionDet"></param>
        /// <param name="tipo"></param>
        /// <param name="idUrs"></param>
        public List<CoConfiguracionGenDTO> ObtenerConfiguracionGenerador(int? idCondiguracionDet, int idUrs)
        {
            List<CoConfiguracionGenDTO> entitys = new List<CoConfiguracionGenDTO>();
            List<EveRsfdetalleDTO> configuracion = (new RsfAppServicio()).ObtenerConfiguracion(DateTime.Now).Where(x => x.Grupocodi == idUrs).ToList();
            List<EveRsfdetalleDTO> datos = configuracion.Where(x => x.Grupotipo != ConstantesAppServicio.SI).ToList();

            List<CoConfiguracionGenDTO> detalle = (idCondiguracionDet != null) ?
                FactorySic.GetCoConfiguracionGenRepository().GetByCriteria((int)idCondiguracionDet) : new List<CoConfiguracionGenDTO>();

            foreach (EveRsfdetalleDTO item in datos)
            {
                CoConfiguracionGenDTO entity = new CoConfiguracionGenDTO();
                entity.Equinomb = item.Ursnomb;
                entity.Equicodi = item.Equicodi;
                if (detalle.Where(x => x.Equicodi == item.Equicodi).Count() > 0)
                {
                    entity.Seleccion = 1;
                }
                entitys.Add(entity);
            }

            return entitys;
        }

        /// <summary>
        /// Permite grabar la configuración de la señal
        /// </summary>
        /// <param name="idConfiguracionDet"></param>
        /// <param name="data"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int GrabarConfiguracionGenerador(int idConfiguracionDet, string data, string userName)
        {
            try
            {
                FactorySic.GetCoConfiguracionGenRepository().Delete(idConfiguracionDet);
                List<int> idGeneradores = data.Split(',').Select(int.Parse).ToList();

                foreach (int id in idGeneradores)
                {
                    CoConfiguracionGenDTO item = new CoConfiguracionGenDTO();
                    item.Courdecodi = idConfiguracionDet;
                    item.Courgefeccreacion = DateTime.Now;
                    item.Courgefecmodificacion = DateTime.Now;
                    item.Courgeusucreacion = userName;
                    item.Courgeusumodificacion = userName;
                    item.Equicodi = id;

                    FactorySic.GetCoConfiguracionGenRepository().Save(item);
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Ticket 6964

        /// <summary>
        /// Permite obtener la consulta de seniales
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="urs"></param>
        /// <param name="canalcodi"></param>
        /// <returns></returns>
        public List<CoDatoSenialDTO> ObtenerConsultaDatosSeniales(DateTime fechaInicio, DateTime fechaFin, int urs, int canalcodi)
        {
            return FactorySic.GetCoDatoSenialRepository().GetByCriteria(fechaInicio, fechaFin, urs, canalcodi);
        }

        /// <summary>
        /// Permite obtener los canales por URS
        /// </summary>
        /// <param name="idUrs"></param>
        /// <returns></returns>
        public List<CoConfiguracionSenialDTO> ObtenerSenialesPorURS(int idUrs)
        {
            return FactorySic.GetCoConfiguracionSenialRepository().ObtenerSenialesPorUrs(idUrs);
        }

        /// <summary>
        /// Permite almacenar los datos de senales
        /// </summary>
        /// <param name="list"></param>
        /// <param name="username"></param>
        /// <param name="validaciones"></param>
        /// <returns></returns>
        public int AlmacenarDatoSenialManual(List<CoDatoSenialDTO> list, string username, out List<string> validaciones)
        {
            try
            {
                List<string> errores = new List<string>();
                List<CoConfiguracionSenialDTO> listCanales = this.ObtenerSenialesPorURS(0);
                foreach (CoDatoSenialDTO entity in list)
                {
                    var subList = listCanales.Where(x => x.Canalnomb.Replace(" ", string.Empty).Trim() == entity.Canalnomb.Replace(" ", string.Empty).Trim()).ToList();
                    if (subList.Count > 0)
                    {
                        entity.Canalcodi = subList[0].Canalcodi;
                        entity.Codasefeccreacion = DateTime.Now;
                        entity.Codaseusucreacion = username;
                    }
                    else
                    {
                        errores.Add("La señal " + entity.Canalnomb + " no existe en la base de datos.");
                    }
                }

                if(errores.Count() == 0)
                {
                    if (list.Count == 0)
                    {
                        errores.Add("El archivo excel no contiene datos de señales.");
                    }
                    else
                    {

                        foreach (CoDatoSenialDTO entity in list)
                        {
                            FactorySic.GetCoDatoSenialRepository().Save(entity);
                        }
                    }
                }
                validaciones = errores;

                return 1;
            }
            catch(Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                validaciones = new List<string>();
                return -1;
            }
        }

        #endregion
    }
}