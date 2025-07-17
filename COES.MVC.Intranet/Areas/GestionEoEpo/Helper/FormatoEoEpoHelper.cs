using COES.Base.Core;
using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.DemandaBarras.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Hidrologia;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using OfficeOpenXml.Drawing;

namespace COES.MVC.Intranet.Areas.GestionEoEpo.Helper
{
    public class FormatoEoEpoHelper
    {
        public static void GenerarFileExcel(FormatoModel model)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteGestionEoEpo].ToString();

            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }

           
            FileInfo newFile = new FileInfo(ruta + "Revision.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();
      
                newFile = new FileInfo(ruta + "Revision.xlsx");
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);

                int c = 1;
                foreach (var item in model.Handson.ListaExcelData)
                {
                    ws.Cells[string.Format("A{0}:B{0}", c)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Column(1).Width = 30;
                    ws.Column(2).Width = 30;

                    if (item[1] == null)
                    {
                        ws.Cells[string.Format("A{0}:B{0}", c)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[string.Format("A{0}:B{0}", c)].Style.Fill.BackgroundColor.SetColor(Color.DarkSeaGreen);
                        ws.Cells[string.Format("A{0}:B{0}", c)].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                        ws.Cells[string.Format("A{0}:B{0}", c)].Value = item[0];
                        ws.Cells[string.Format("A{0}:B{0}", c)].Merge = true;
                    }
                    else {
                        ws.Cells[string.Format("A{0}:A{0}", c)].Value = item[0];
                        ws.Cells[string.Format("B{0}:B{0}", c)].Value = item[1];
                    }

                    c++;
                }

                xlPackage.Save();
            }

        }
       
        /// <summary>
        /// Escribe los valores de las fecha en el archivo formato dependiendo de la resolucion
        /// </summary>
        private static void GenerarColumnaFecha(ExcelWorksheet ws,int nBloques,int resolucion,int totColumnas,DateTime fecha)
        {
            int row = ParamFormato.RowDatos;
            int column = ParamFormato.ColDatos;
            for (int i = 1; i <= nBloques; i++)
            {
                ws.Cells[row, 1].Value = ((fecha.AddMinutes(i * resolucion))).ToString(Constantes.FormatoFechaHora);
                if (row != ParamFormato.RowDatos)
                {
                    ws.Cells[row, ParamFormato.ColFecha].StyleID = ws.Cells[row - 1, ParamFormato.ColFecha].StyleID;

                    for (int k = 0; k < totColumnas; k++)
                    {
                        ws.Cells[row, k + column].Value = "";
                        ws.Cells[row, k + column].StyleID = ws.Cells[row - 1, k + column].StyleID;
                    }
                }
                row++;
            }
        
        }

        /// <summary>
        /// EScribe los valores de los meses en el archivo formato.
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="nBloques"></param>
        /// <param name="resolucion"></param>
        /// <param name="totColumnas"></param>
        /// <param name="fecha"></param>
        private static void GeneraColumnaMes(ExcelWorksheet ws, int nBloques, int resolucion, int totColumnas, DateTime fecha)
        {
            int row = ParamFormato.RowDatos;
            int column = ParamFormato.ColDatos;
            int mes = fecha.Month;
            int anho = fecha.Year;
            if (mes == 1)
            {
                mes = 13;
                anho--;
            }
            
            for (int i = 1; i < mes; i++)
            {
                ws.Cells[row, 1].Value = anho.ToString() + " " + Util.ObtenerNombreMesAbrev(i);
                if (row != ParamFormato.RowDatos)
                {
                    ws.Cells[row, ParamFormato.ColFecha].StyleID = ws.Cells[row - 1, ParamFormato.ColFecha].StyleID;

  
                    for (int k = 0; k < totColumnas; k++)
                    {
                        ws.Cells[row, k + column].Value = "";
                        ws.Cells[row, k + column].StyleID = ws.Cells[row - 1, k + column].StyleID;
                    }
                }
                row++;
            }           
        }

        /// <summary>
        /// Genera el Formato en Excel HTML
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idEnvio"></param>
        /// <param name="enPlazo"></param>
        /// <returns></returns>
        public static string GenerarFormatoHtml(FormatoModel model, int idEnvio, Boolean enPlazo)
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
            strHtml.Append("  <tr><td >" + model.Formato.Areaname  + " </td>");
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
        /// Ubica la posicion de una columna en el grid
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        public static int PosListaPunto(List<MeHojaptomedDTO> lista, int orden)
        {
            int pos = -1;
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Hojaptoorden == orden)
                    pos = i;
            }
            return pos;
        }


        /// <summary>
        /// Obtiene matriz de string que son solo los valores de las celdas del excel web ingresando como parametro una lista de mediciones.
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="listaCambios"></param>
        /// <param name="formato"></param>
        public static void ObtieneMatrizWebExcel(FormatoModel model, List<object> lista, List<MeCambioenvioDTO> listaCambios, int idEnvio)
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
                            var find = model.ListaHojaPto.Find(x => x.Ptomedicodi == reg.Ptomedicodi);
                            if (find != null)
                            {
                          
                                var col = PosListaPunto(model.ListaHojaPto, find.Hojaptoorden) + 1;
                                var row = model.FilasCabecera +
                                    ObtieneRowChange((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, int.Parse(cambios[i]),
                                    model.Formato.RowPorDia, reg.Cambenvfecha, model.Formato.FechaInicio);
    
                                model.ListaCambios.Add(new CeldaCambios()
                                {
                                    Row = row,
                                    Col = col
                                });
                            }
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
                                    && (DateTime)i.GetType().GetProperty("Medifecha").GetValue(i, null) == fechaFind);

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
                                    
                                   ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion,model.Formato.Lecttipo ,z, jIni, model.Formato.FechaInicio);
                            }
                            if (reg != null)
                            {
                                decimal? valor = (decimal?)reg.GetType().GetProperty("H" + j).GetValue(reg, null);
                                if (valor != null)
                                    model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = valor.ToString();
                            }
                        }
                    }
                }

            //}
        }


        public static int ObtieneRowChange(int periodo,int resolucion,int indiceBloque,int rowPorDia,DateTime fechaCambio,DateTime fechaInicio)
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
                            row = indiceBloque + rowPorDia * (fechaCambio - fechaInicio).Days -1;
                            break;
                    }
                    break;
                default:
                    row = indiceBloque + rowPorDia * (fechaCambio - fechaInicio).Days - 1;
                    break;
            }

            return row;
        }
        /// <summary>
        /// Obtiene el nombre de la celda fechaa mostrarse en los formatos excel.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="indice"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public static string ObtenerCeldaFecha(int periodo,int resolucion,int tipoLectura,int horizonte,int indice,DateTime fechaInicio)
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
                            resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice).ToString(Constantes.FormatoFechaHora);
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    int semana = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(fechaInicio, 6) + horizonte;
                    int semanaMax = COES.Base.Tools.Util.TotalSemanasEnAnho(fechaInicio.Year,6);
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
                    resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice).ToString(Constantes.FormatoFechaHora);
                    break;
            }

            return resultado;
        }
        /// <summary>
        /// Devuelve la fecha del siguiente bloque
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public static DateTime GetNextFilaHorizonte(int periodo,int resolucion,int horizonte,DateTime fechaInicio)
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
        /// Convierte Lista de Object a Lista de Medicion96DTO
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
                    decimal valor = (decimal)entity.GetType().GetProperty(Constantes.CaracterH + (i).ToString()).GetValue(entity, null);
                    reg.GetType().GetProperty(Constantes.CaracterH + (i).ToString()).SetValue(reg, valor);
                }
                listaFinal.Add(reg);
            }
            return listaFinal;
        }
        /// <summary>
        /// Convierte Lista de Object a Lista de Medicion48DTO
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
                for (int i = 1; i <= 48; i++)
                {
                    decimal valor = (decimal)entity.GetType().GetProperty(Constantes.CaracterH + (i).ToString()).GetValue(entity, null);
                    reg.GetType().GetProperty(Constantes.CaracterH + (i).ToString()).SetValue(reg,valor);
                  }
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        /// <summary>
        /// Convierte Lista de Object a Lista de Medicion24DTO
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<MeMedicion24DTO> Convert24DTO(List<Object> lista)
        {
            List<MeMedicion24DTO> listaFinal = new List<MeMedicion24DTO>();

            foreach (var entity in lista)
            {
                MeMedicion24DTO reg = new MeMedicion24DTO();
                reg.Lectcodi = (int)entity.GetType().GetProperty("Lectcodi").GetValue(entity, null);
                reg.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                reg.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                reg.Tipoinfocodi = (int)entity.GetType().GetProperty("Tipoinfocodi").GetValue(entity, null);
                for (int i = 1; i <= 24; i++)
                {
                    decimal valor = (decimal)entity.GetType().GetProperty(Constantes.CaracterH + (i).ToString()).GetValue(entity, null);
                    reg.GetType().GetProperty(Constantes.CaracterH + (i).ToString()).SetValue(reg, valor);
                }
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        /// <summary>
        /// Convierte Lista de Object a Lista de Medicion1DTO
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
                reg.H1 = (int)entity.GetType().GetProperty("H1").GetValue(entity, null);
                listaFinal.Add(reg);
            }
            return listaFinal;
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
        public static List<MeMedicion96DTO> LeerExcelWeb96(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int colHead, int nCol, int filaHead, int nFil,int checkBlanco)
        {
            var matriz = GetMatrizExcel(data,colHead ,nCol,filaHead, nFil);
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
                        fecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                        stValor = matriz[j][i];
                        if (Util.EsNumero(stValor))
                        {
                            //valor = decimal.Parse(stValor);
                            valor = 0;
                            decimal.TryParse(stValor, out valor);
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
                            try
                            {
                                valor = 0;
                                decimal.TryParse(stValor, out valor);

                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                            }
                            catch
                            {
                                string s = stValor;
                            }
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                            else
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, 0M);
                        }
                    }
                }
                lista.Add(reg);
            }
            return lista;
        }

        /// <summary>
        /// Carga la data recibida del excel web a una lista de Medidion48DTO
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
        public static List<MeMedicion48DTO> LeerExcelWeb48(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int colHead, int nCol, int filaHead, int nFil, int checkBlanco)
        {
            var matriz = GetMatrizExcel(data, colHead, nCol, filaHead, nFil);
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            MeMedicion48DTO reg = new MeMedicion48DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            for (var i = 1; i < nCol; i++)
            {
                for (var j = 0; j < nFil; j++)
                {
                    //verificar inicio de dia
                    if ((j % 48) == 0)
                    {
                        if (j != 0)
                            lista.Add(reg);
                        reg = new MeMedicion48DTO();
                        reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                        reg.Lectcodi = idLectura;
                        reg.Meditotal = 0;
                        reg.Tipoinfocodi = (int)ptos[i - 1].Tipoinfocodi;
                        fecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
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
                        int indice = j % 48 + 1;
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
            return lista;
        }

        /// <summary>
        /// Carga la data recibida del excel web a una lista de Medidion24DTO
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
        public static List<MeMedicion24DTO> LeerExcelWeb24(List<string> data,List<MeHojaptomedDTO> ptos,int idLectura,int colHead,int nCol,int filaHead,int nFil)
        {
            var matriz = GetMatrizExcel(data, colHead, nCol, filaHead, nFil);
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            MeMedicion24DTO reg = new MeMedicion24DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            for (var i = 1; i < nCol; i++)
            {
                for (var j = 0; j < nFil; j++)
                { 
                    //verificar inicio de dia
                    if ((j % 24) == 0)
                    {
                        if (j != 0)
                            lista.Add(reg);
                        reg = new MeMedicion24DTO();
                        reg.Ptomedicodi = ptos[i-1].Ptomedicodi;
                        reg.Lectcodi = idLectura;
                        reg.Tipoinfocodi =(int) ptos[i-1].Tipoinfocodi;
                        fecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                        stValor = matriz[j][i];
                        if (Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            reg.H1 = valor;
                        }
                        else
                            reg.H1 = null;
                    }
                    else 
                    {
                        stValor = matriz[j][i];
                        int indice = j % 24 + 1;
                        if (Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                        }
                        else
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);

                    }
                }
                lista.Add(reg);
            }
            return lista;
        }

        /// <summary>
        /// Carga la data recibida del excel web a una lista de Medidion1DTO
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
        public static List<MeMedicion1DTO> LeerExcelWeb1(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int periodo, int colHead, int nCol, int filaHead, int nFil)
        {
            var matriz = GetMatrizExcel(data, colHead, nCol, filaHead, nFil);
            List<MeMedicion1DTO> lista = new List<MeMedicion1DTO>();
            MeMedicion1DTO reg = new MeMedicion1DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            for (var i = 1; i < nCol; i++)
            {
                for (var j = 0; j < nFil; j++)
                {
                    //verificar inicio de dia
                    reg = new MeMedicion1DTO();
                    reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                    reg.Lectcodi = idLectura;
                    reg.Tipoinfocodi = (int)ptos[i - 1].Tipoinfocodi;
                    switch (periodo)
                    {
                        case 1:
                            reg.Medifecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            break;
                        case 2:
                            reg.Medifecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            break;
                        case 5:
                            reg.Medifecha = EPDate.GetFechaSemana(matriz[j][0]);
                            break;
                        case 3:
                            reg.Medifecha = EPDate.GetFechaYearMonth(matriz[j][0]);
                            break;
                    }

                    reg.Medifecha = new DateTime(reg.Medifecha.Year, reg.Medifecha.Month, reg.Medifecha.Day);
                    stValor = matriz[j][i];
                    if (Util.EsNumero(stValor))
                    {
                        valor = decimal.Parse(stValor);
                        reg.H1 = valor;
                    }
                    else
                        reg.H1 = null;
                    lista.Add(reg);
                }
                
            }
            return lista;
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
        private static string[][] GetMatrizExcel(List<string> data, int colHead,int nCol,int filHead, int nFil)
        {
            var inicio = (nCol + colHead - 1) * filHead;
            int col = 0;
            int fila = 0;
            string[][] arreglo = new string[nFil][];
            arreglo[0] = new string[nCol];
            for (int i = inicio; i < data.Count(); i++)
            {
                string valor = data[i];
                if (col == nCol)
                {
                    fila++;
                    col = 0;
                    arreglo[fila] = new string[nCol];
                }
                arreglo[fila][col] = valor;
                col++;
            }
            return arreglo;
        }

        public static DateTime GetFechaFinPeriodo(DateTime fechaIni, int periodo,int horizonte)
        {
            DateTime fechaFin = DateTime.MinValue;
            switch (periodo)
            {
                case 1:
                    fechaFin = fechaIni.AddDays(horizonte -1);
                    break;
                case 2:
                    fechaFin = fechaIni.AddDays(horizonte);
                    break;
                case 3:
                    fechaFin = fechaIni;
                    break;
                case 5:
                    int nMeses = horizonte / 30;
                    fechaFin = fechaIni.AddMonths(nMeses);
                    break;
            }
            return fechaFin;
        }
        /// <summary>
        /// Lee archivo excel cargado y llena matriz de datos para visualizacion web
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static Boolean LeerExcelFile(string[][] matriz,string file, int rowsHead, int nFil, int colsHead, int nCol)
        {
            Boolean retorno = false;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                /// Verificar Formato

                int c = 0;
                foreach (var item in matriz)
                {
                    if (item[1] != null) {
                        item[1] = ((object[,])ws.Cells.Value)[c, 1].ToString();
                    }

                    c++;
                }
            }
            return true;
        }
        /// <summary>
        /// Inicializa las dimensiones de la matriz de valores del objeto excel web
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static string[][] InicializaMatrizExcel(int rowsHead, int nFil, int colsHead, int nCol)
        {
            string[][] matriz = new string[rowsHead][];
            for (int i = 0; i < nFil; i++)
            {
                matriz[i] = new string[nCol + colsHead];
                for (int j = 0; j < nCol; j++)
                {
                    matriz[i][j] = string.Empty;
                }
            }
            return matriz;
        }
        /// <summary>
        /// Inicializa lista de filas readonly para la matriz excel web
        /// </summary>
        /// <param name="filHead"></param>
        /// <param name="filData"></param>
        /// <returns></returns>
        public static List<bool> InicializaListaFilaReadOnly(int filHead, int filData,bool plazo,int horaini,int horafin)
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

        public static void GetSizeFormato2(MeFormatoDTO formato)
        {
            formato.FechaInicio = formato.FechaProceso;
            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
            formato.FechaPlazo = formato.FechaProceso.AddMonths(1).AddDays(15);     
            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days + 1;
            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
        }
        
        public static void BorrarArchivo(String archivo)
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

        public static int VerificarIdsFormato(string file, int idEmpresa, int idFormato)
        {
            int retorno = 1;
            int idEmpresaArchivo;
            int idFormatoEmpresa;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                var valorEmp = ws.Cells[1, ParamFormato.ColEmpresa].Value.ToString();
                if (!int.TryParse(valorEmp, NumberStyles.Any, CultureInfo.InvariantCulture, out idEmpresaArchivo))
                    idEmpresa = 0;
                var valorFormato = ws.Cells[1, ParamFormato.ColFormato].Value.ToString();
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

        public static int EnviarCorreo(string formato, bool enPlazo,string empresa,DateTime fechaInicio,DateTime fechaFin,string areaCoes ,string usuario ,DateTime fechaEnvio,int idEnvio)
        {
            int resultado = 0;           
            return resultado;
        }
        #region Mejoras EO-EPO-II
        public static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);

            }
        }
        /// <summary>
        /// Deterina ancho en pixeles para el logo
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int MTU_PER_PIXEL = 9525;
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }
        #endregion
    }

}