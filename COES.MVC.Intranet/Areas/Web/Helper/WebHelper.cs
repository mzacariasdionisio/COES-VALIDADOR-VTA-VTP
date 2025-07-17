using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Web.Models;
using COES.Servicios.Aplicacion.General.Helper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Web.Helper
{
    public class WebHelper
    {
        /// <summary>
        /// Permite obtener los colores de eventos
        /// </summary>
        /// <returns></returns>
        public static List<string> ObtenerColores()
        {
            List<string> list = new List<string>();
            list.Add("#1ABC9C");
            list.Add("#2ECC71");
            list.Add("#3498DB");
            list.Add("#9B59B6");
            list.Add("#34495E");
            list.Add("#F1C40F");
            list.Add("#E67E22");
            list.Add("#E74C3C");
            list.Add("#95A5A6");
            return list;                
        }
        
        /// <summary>
        /// Permite obtener los íconos para los eventos
        /// </summary>
        /// <returns></returns>
        public static List<string> ObtenerIconos()
        {
            List<string> list = new List<string>();
            list.Add("icon-report.png?v=1");
            list.Add("icon-clock.png?v=1");
            list.Add("icon-meeting.png?v=1");
            list.Add("icon-audience.png?v=1");
            list.Add("icon-birthday.png?v=1");
            return list;
        }

        /// <summary>
        /// Permite obtener los íconos para los eventos
        /// </summary>
        /// <returns></returns>
        public static List<LeyendaIcono> ObtenerIconosDef()
        {
            List<LeyendaIcono> list = new List<LeyendaIcono>();
            list.Add(new LeyendaIcono
            {
                Definicion = ConstantesPortal.TextoEventoPublicacion,
                Icono = ConstantesPortal.IconoEventoPublicacion,
                Color = ConstantesPortal.ColorEventoPublicacion
            });

            list.Add(new LeyendaIcono
            {
                Definicion = ConstantesPortal.TextoEventoVencimiento,
                Icono = ConstantesPortal.IconoEventoVencimiento,
                Color = ConstantesPortal.ColorEventoVencimiento
            });

            list.Add(new LeyendaIcono
            {
                Definicion = ConstantesPortal.TextoEventoReunion,
                Icono = ConstantesPortal.IconoEventoReunion,
                Color = ConstantesPortal.ColorEventoReunion

            });

            return list;
        }

        /// <summary>
        /// Permite listar los años
        /// </summary>
        /// <returns></returns>
        public static List<int> ListarAnios()
        {
            List<int> list = new List<int>();
            int anioInicio = 2017;
            for (int i = anioInicio; i <= anioInicio + 2; i++)
            {
                list.Add(i);
            }

            return list;
        }

        /// <summary>
        /// Permite listar los meses
        /// </summary>
        /// <returns></returns>
        public static List<MesCalendarItemModel> ListarMeses()
        {
            List<MesCalendarItemModel> list = new List<MesCalendarItemModel>();

            for (int i = 1; i <= 12; i++)
            {
                MesCalendarItemModel item = new MesCalendarItemModel();
                item.Valor = i;
                item.Texto = COES.Base.Tools.Util.ObtenerNombreMes(i);
                list.Add(item);
            }

            return list;        
        }

        /// <summary>
        /// Lee los artículos desde el formato excel cargado
        /// </summary>
        /// <param name="codCliente"></param>
        /// <returns></returns>
        public static List<WbCmvstarifaDTO> LeerDesdeFormato(string file)
        {
            string s = "";
            try
            {
                List<WbCmvstarifaDTO> list = new List<WbCmvstarifaDTO>();
                
                int cantidad = 250;

                FileInfo fileInfo = new FileInfo(file);
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                    for (int i = 7; i <= cantidad; i++)
                    {
                        if (ws.Cells[i, 1].Value != null && ws.Cells[i, 1].Value != string.Empty)
                        {
                            if (ws.Cells[i, 1].Value.ToString().Contains("Media mensual registrada")) {
                                break;
                            }
                            s = ws.Cells[i, 1].Value.ToString();

                            WbCmvstarifaDTO item = new WbCmvstarifaDTO();

                            item.Cmtarfecha = DateTime.ParseExact(s, "M/d/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                            item.Cmtarcmprom = (ws.Cells[i, 5].Value != null) ? decimal.Parse(ws.Cells[i, 5].Value.ToString()) : 0;
                            item.Cmtarprommovil = (ws.Cells[i, 6].Value != null) ? decimal.Parse(ws.Cells[i, 6].Value.ToString()) : 0;
                            item.Cmtartarifabarra = (ws.Cells[i, 11].Value != null) ? decimal.Parse(ws.Cells[i, 11].Value.ToString()) : 0;                            

                            list.Add(item);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message +" Dato:" + s, ex);
            }
        }

    }

    public class LeyendaIcono
    {
        public string Definicion { get; set; }
        public string Icono { get; set; }
        public string Color { get; set; }
    }
}