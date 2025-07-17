using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.Admin.Helper
{
    /// <summary>
    /// Clase de utilidad para el reporte de usuarios extranet
    /// </summary>
    public class ReporteHelper
    {
        /// <summary>
        /// Permite generar el reporte de usuarios de la extranet
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GenerarReporteUsuario(List<ReporteUsuarioDTO> list)
        {
            StringBuilder str = new StringBuilder();

            str.Append("<table class='tabla-adicional pretty' id='tablaReporte'>");
            str.Append("    <thead>");
            str.Append("        <tr>");
            str.Append("            <th>Empresa</th>");
            str.Append("            <th>Estado</th>");
            str.Append("            <th>Cantidad</th>");
            str.Append("            <th>Total</th>");
            str.Append("        </tr>");
            str.Append("    </thead>");
            str.Append("    <tbody>");

            var empresas = list.Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().ToList();
            int contador = 0;
            string stilo = string.Empty;

            foreach (var empresa in empresas)
            {
                if (contador % 2 != 0) stilo = "class='odd'";
                else stilo = "";

                List<ReporteUsuarioDTO> listUsuario = list.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();
                int index = 0;
                int rowspan = 1;

                foreach (ReporteUsuarioDTO item in listUsuario)
                {
                    string functionSubTotal = string.Format("mostrarUsuario({0},\"{1}\")", item.Emprcodi, item.Estado);
                    string functionTotal = string.Format("mostrarUsuarioTotal({0})", item.Emprcodi);
                                        
                    if (item.Estado == "A") item.Estado = "Activo";
                    else if (item.Estado == "B") item.Estado = "Baja";
                    else if (item.Estado == "P") item.Estado = "Pendiente";

                    if (index == 0)
                    {
                        rowspan = listUsuario.Count;
                        str.AppendFormat("        <tr {0}>", stilo);
                        str.AppendFormat("            <td rowspan='{1}'>{0}</td>", item.Emprnomb, listUsuario.Count);
                        str.AppendFormat("            <td>{0}</td>", item.Estado);
                        str.AppendFormat("            <td style='text-align:right'><a href='JavaScript:{1};'>{0}</a></td>", item.Cantidad, functionSubTotal);
                        str.AppendFormat("            <td rowspan='{1}' style='text-align:right'><a href='JavaScript:{2}'>{0}</a></td>", 
                            listUsuario.Sum(x => x.Cantidad), listUsuario.Count, functionTotal);
                        str.Append("        </tr>");
                    }
                    else
                    {
                        str.AppendFormat("        <tr {0}>", stilo);
                        str.AppendFormat("            <td>{0}</td>", item.Estado);
                        str.AppendFormat("            <td style='text-align:right'><a href='JavaScript:{1}'>{0}</a></td>", item.Cantidad, functionSubTotal);
                        str.Append("        </tr>");
                    }

                    index++;
                }

                contador++;
            }

            decimal total = list.Sum(x => x.Cantidad);

            str.Append("    </tbody>");
            str.Append("    <tfoot>");
            str.Append("        <tr>");
            str.Append("            <td colspan='2'>TOTAL:</td>");
            str.AppendFormat("            <td style='text-align:right'>{0}</td>", total);
            str.AppendFormat("            <td style='text-align:right'>{0}</td>", total);
            str.Append("        </tr>");
            str.Append("    </tfoot>");
            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Permite generar el reporte de solicitudes de acceso
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GenerarReporteSolicitud(List<ReporteUsuarioDTO> list) 
        {
            StringBuilder str = new StringBuilder();

            str.Append("<table class='tabla-adicional pretty' id='tablaReporte'>");
            str.Append("    <thead>");
            str.Append("        <tr>");
            str.Append("            <th>Empresa</th>");
            str.Append("            <th>Módulo</th>");
            str.Append("            <th>Estado</th>");
            str.Append("            <th>Cantidad</th>");
            str.Append("            <th>Total Módulo</th>");
            str.Append("            <th>Total Empresa</th>");
            str.Append("        </tr>");
            str.Append("    </thead>");
            str.Append("    <tbody>");

            var empresas = list.Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().ToList();           
            string stilo = string.Empty;
            int contador = 0;
            foreach (var empresa in empresas)
            {
                var modulos = list.Where(x => x.Emprcodi == empresa.Emprcodi).Select(x => new { x.Modcodi, x.Modnomb }).Distinct().ToList();

                int countEmpresa = list.Where(x => x.Emprcodi == empresa.Emprcodi).Count();
                int indEmpresa = 0;

                if (contador % 2 != 0) stilo = "class='odd'";
                else stilo = "";
                
                foreach (var modulo in modulos)
                {
                    List<ReporteUsuarioDTO> listSolicitud = list.Where(x => x.Emprcodi == empresa.Emprcodi && x.Modcodi == modulo.Modcodi).ToList();

                    int indModulo = 0;

                    foreach (ReporteUsuarioDTO item in listSolicitud)
                    {
                        string functionSubTotal = string.Format("mostrarSolicitud({0}, {1},\"{2}\")", item.Emprcodi, item.Modcodi, item.Estado);
                        string functionTotalModulo = string.Format("mostrarSolicitudPorModulo({0}, {1})", item.Emprcodi, item.Modcodi);
                        string functionTotalEmpresa = string.Format("mostrarSolicitudPorEmpresa({0})", item.Emprcodi);

                        if (item.Estado == "A") item.Estado = "Aceptado";
                        else if (item.Estado == "P") item.Estado = "Pendiente";

                        str.AppendFormat("        <tr {0}>", stilo);
                        if (indEmpresa == 0)
                            str.AppendFormat("            <td rowspan='{1}'>{0}</td>", item.Emprnomb, countEmpresa);

                        if (indModulo == 0) {
                            str.AppendFormat("            <td rowspan='{1}'>{0}</td>", item.Modnomb, listSolicitud.Count);
                        }
                        str.AppendFormat("            <td>{0}</td>", item.Estado);
                        str.AppendFormat("            <td><a href='JavaScript:{1};'>{0}</a></td>", item.Cantidad, functionSubTotal);

                        if (indModulo == 0)
                            str.AppendFormat("            <td rowspan='{1}'><a href='JavaScript:{2};'>{0}</a></td>", listSolicitud.Sum(x => x.Cantidad), 
                                listSolicitud.Count, functionTotalModulo);

                        if (indEmpresa == 0)
                            str.AppendFormat("            <td rowspan='{1}'><a href='JavaScript:{2};'>{0}</a></td>", list.Where(x => x.Emprcodi == empresa.Emprcodi).Sum(x => x.Cantidad),
                                countEmpresa, functionTotalEmpresa);

                        str.Append("        </tr>");
                        indEmpresa++;
                        indModulo++;
                    }
                }

                contador++;
            }

            decimal total = list.Sum(x => x.Cantidad);

            str.Append("    </tbody>");
            str.Append("    <tfoot>");
            str.Append("        <tr>");
            str.Append("            <td colspan='3'>TOTAL:</td>");
            str.AppendFormat("            <td style='text-align:right'>{0}</td>", total);
            str.AppendFormat("            <td style='text-align:right'>{0}</td>", total);
            str.AppendFormat("            <td style='text-align:right'>{0}</td>", total);
            str.Append("        </tr>");
            str.Append("    </tfoot>");
            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Permite generar el documento excel con los datos de los usuarios
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public static void GenerarReporteUsuarioExcel(List<ReporteUsuarioDTO> list, string path, string filename)
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("USUARIOS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE USUARIOS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "Empresa";
                        ws.Cells[index, 3].Value = "Logín";
                        ws.Cells[index, 4].Value = "Nombre";
                        ws.Cells[index, 5].Value = "Estado";

                        rg = ws.Cells[index, 2, index, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (ReporteUsuarioDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Emprnomb;
                            ws.Cells[index, 3].Value = item.Userlogin;
                            ws.Cells[index, 4].Value = item.Username;
                            ws.Cells[index, 5].Value = item.Estado;

                            rg = ws.Cells[index, 2, index, 5];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 5];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 5];
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
        /// Permite generar el documento excel con los datos de los usuarios
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public static void GenerarReporteContactoExcel(List<WbContactoDTO> list, string path, string filename, string indPublico)
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CONTACTOS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE CONTACTOS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "EMPRESA";
                        ws.Cells[index, 3].Value = "NOMBRES Y APELLIDOS";
                        ws.Cells[index, 4].Value = "CORREO ELECTRÓNICO";
                        ws.Cells[index, 5].Value = "TELÉFONO";
                        ws.Cells[index, 6].Value = "CELULAR";
                        ws.Cells[index, 7].Value = "CARGO";
                        ws.Cells[index, 8].Value = "ÁREA";

                        int colmax = 8;
                        if (indPublico == Constantes.NO)
                        {
                            ws.Cells[index, 9].Value = "FUENTE";
                            ws.Cells[index, 10].Value = "COES";
                            colmax = 10;
                        }

                        rg = ws.Cells[index, 2, index, colmax];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (WbContactoDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Emprnomb;
                            ws.Cells[index, 3].Value = item.Contacnombre;
                            ws.Cells[index, 4].Value = item.Contacemail;
                            ws.Cells[index, 5].Value = item.Contactelefono;
                            ws.Cells[index, 6].Value = item.Contacmovil;                            
                            ws.Cells[index, 7].Value = item.Contaccargo;
                            ws.Cells[index, 8].Value = item.Contacarea;

                            if (indPublico == Constantes.NO)
                            {
                                ws.Cells[index, 9].Value = item.Fuente;
                                ws.Cells[index, 10].Value = item.Emprcoes;
                            }

                            rg = ws.Cells[index, 2, index, colmax];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, colmax];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, colmax];
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
        /// Permite generar el documento excel con los datos de los usuarios
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public static void GenerarReporteContactoPorEmpresa(List<SiEmpresaCorreoDTO> list, string path, string filename)
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CONTACTOS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE CONTACTOS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "TIPO EMPRESA";
                        ws.Cells[index, 3].Value = "EMPRESA";
                        ws.Cells[index, 4].Value = "RUC";
                        ws.Cells[index, 5].Value = "NOMBRES Y APELLIDOS";
                        ws.Cells[index, 6].Value = "CORREO ELECTRÓNICO";
                        ws.Cells[index, 7].Value = "CARGO";
                        ws.Cells[index, 8].Value = "TELÉFONO";
                        ws.Cells[index, 9].Value = "CELULAR";                       
                        ws.Cells[index, 10].Value = "TIPO";
                        ws.Cells[index, 11].Value = "NOTIFICAR";

                        rg = ws.Cells[index, 2, index, 11];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (SiEmpresaCorreoDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Tipoemprnomb;
                            ws.Cells[index, 3].Value = item.Emprnomb;
                            ws.Cells[index, 4].Value = item.Emprruc;
                            ws.Cells[index, 5].Value = item.Empcornomb;
                            ws.Cells[index, 6].Value = item.Empcoremail;
                            ws.Cells[index, 7].Value = item.Empcorcargo;
                            ws.Cells[index, 8].Value = item.Empcortelefono;
                            ws.Cells[index, 9].Value = item.Empcormovil;                            
                            ws.Cells[index, 10].Value = item.Emprcortipo;
                            ws.Cells[index, 10].Value = item.Emprcortipo;
                            ws.Cells[index, 11].Value = (item.Empcorindnotic!="" ? item.Empcorindnotic=="S" ? "Si" : "No" : "No");

                            rg = ws.Cells[index, 2, index, 11];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 11];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 11];
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
        /// Permite generar el reporte de solicitudes a formato excel
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public static void GenerarReporteSolicitudExcel(List<ReporteUsuarioDTO> list, string path, string filename)
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("SOLICITUDES");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE SOLICITUDES DE ACCESO A LA EXTRANET";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "Empresa";
                        ws.Cells[index, 3].Value = "Módulo";
                        ws.Cells[index, 4].Value = "Usuario";
                        ws.Cells[index, 5].Value = "Logín";
                        ws.Cells[index, 6].Value = "Estado Solicitud";
                        ws.Cells[index, 7].Value = "Fecha Solicitud";

                        rg = ws.Cells[index, 2, index, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (ReporteUsuarioDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Emprnomb;
                            ws.Cells[index, 3].Value = item.Modnomb;
                            ws.Cells[index, 4].Value = item.Username;
                            ws.Cells[index, 5].Value = item.Userlogin;
                            ws.Cells[index, 6].Value = item.Estado;
                            ws.Cells[index, 7].Value = (item.Solicfecha != null) ? ((DateTime)item.Solicfecha).ToString(Constantes.FormatoFecha) : string.Empty;

                            rg = ws.Cells[index, 2, index, 7];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 7];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 7];
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
        /// Permite generar el reporte de empresas
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public static void GenerarReporteEmpresa(List<SiEmpresaDTO> list, string path, string filename)
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("EMPRESAS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE EMPRESAS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "Abreviatura";
                        ws.Cells[index, 3].Value = "Nombre";
                        ws.Cells[index, 4].Value = "Tipo Empresa";
                        ws.Cells[index, 5].Value = "RUC";
                        ws.Cells[index, 6].Value = "Razón Social";
                        ws.Cells[index, 7].Value = "Pertenece al SEIN";
                        ws.Cells[index, 8].Value = "Estado";
                        ws.Cells[index, 9].Value = "Last User";
                        ws.Cells[index, 10].Value = "Last Date";
                        ws.Cells[index, 11].Value = "Código";

                        rg = ws.Cells[index, 2, index, 11];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (SiEmpresaDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Emprabrev;
                            ws.Cells[index, 3].Value = item.Emprnomb;
                            ws.Cells[index, 4].Value = item.Tipoemprdesc;
                            ws.Cells[index, 5].Value = item.Emprruc;
                            ws.Cells[index, 6].Value = item.Emprrazsocial;
                            ws.Cells[index, 7].Value = item.Emprsein;
                            ws.Cells[index, 8].Value = item.Emprestado;
                            ws.Cells[index, 9].Value = item.Lastuser;
                            ws.Cells[index, 10].Value = (item.Lastdate != null) ? ((DateTime)item.Lastdate).ToString(Constantes.FormatoFecha) : string.Empty;
                            ws.Cells[index, 11].Value = item.Emprcodi;

                            rg = ws.Cells[index, 2, index, 11];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 11];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 11];
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
    }
}