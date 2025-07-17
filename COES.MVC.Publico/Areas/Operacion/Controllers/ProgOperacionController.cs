using COES.MVC.Publico.Helper;
using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using OfficeOpenXml;
using COES.Servicios.Aplicacion.EnviarCorreos;
using Microsoft.Office.Interop.Excel;

namespace COES.MVC.Publico.Areas.Operacion.Controllers
{
    public class ProgOperacionController : Controller
    {

        EnviarCorreosAppServicio servEnvioCorreo = new EnviarCorreosAppServicio();
        //
        // GET: /Operacion/ProgramaOperacion/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProgramaDiario()
        {
            return View();
        }
        public ActionResult ProgSemanalOp()
        {
            return View();
        }

        public ActionResult RprogDiarioOp()
        {
            ViewBag.fechaDescargaDeDocumento = DateTime.Now.ToString(Constantes.FormatoFecha); 
            return View();
        }

        [HttpGet]
        public bool ValidarExistenciaDeReportePR06(string fecha)
        {
            try
            {
                //Formato del excel en el directorio => indices20210224.xlsx
                string nombreArchivo = "indices" + fecha + ".xlsx";
                string fullPath = ConfigurationManager.AppSettings["RutaExcelsPR06"] + nombreArchivo;
                return System.IO.File.Exists(fullPath);
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        [HttpGet]
        public ActionResult DescargarReportePR06(string fecha)
        {
            try
            {
                //Formato del excel en el directorio => indices20210224.xlsx
                string app = Constantes.AppExcel;
                string nombreArchivo = "indices"+ fecha + ".xlsx";
                string fullPath = ConfigurationManager.AppSettings["RutaExcelsPR06"] + nombreArchivo;
                return File(fullPath, app, nombreArchivo);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Archivo no existente");
            }
        }

        public ActionResult DescargarListadoDeReProgramasPorFecha(string fecha)
        {
            var ListadoReProgramas = servEnvioCorreo.BuscarOperacionesDelTipoReProgramaPorFecha(fecha).ToList();

            var memoryStream = new MemoryStream();
            byte[] data = null;

            using (ExcelPackage xlPackage = new ExcelPackage(memoryStream))
            {
                var nombreHojaAgentes = "Reprogramas";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHojaAgentes);

                ws.Cells[1, 1].Value = "FECHA";
                ws.Cells[1, 2].Value = "TIPO";
                ws.Cells[1, 3].Value = "PROGRAMADOR";
                //ws.Cells[1, 4].Value = "USUARIO"; => Comentado a solicitud del usuario
                ws.Cells[1, 4].Value = "CAUSA REPROGRAMA";
                ws.Cells[1, 5].Value = "BLOQUE HORARIO";
                ws.Cells[1, 6].Value = "REPROGRAMA";
                ws.Cells[1, 7].Value = "ÚLTIMA MODIFICACIÓN";

                int fila = 2;

                foreach (var item in ListadoReProgramas)
                {
                    ws.Cells[fila, 1].Value = item.Mailfecha.ToString("dd/MM/yyyy");
                    ws.Cells[fila, 2].Value = item.Subcausadesc;
                    ws.Cells[fila, 3].Value = item.Mailprogramador;
                    //ws.Cells[fila, 4].Value = item.Lastuser; => Comentado a solicitud del usuario
                    ws.Cells[fila, 4].Value = item.Mailreprogcausa;
                    ws.Cells[fila, 5].Value = GetHorarios().FirstOrDefault(x => x.Value == item.Mailbloquehorario.ToString()).Text;
                    ws.Cells[fila, 6].Value = item.Mailhoja;
                    ws.Cells[fila, 7].Value = item.Lastdate.ToString();
                    fila++;
                }

                ws.Cells["A1:K20"].AutoFitColumns();
                data = xlPackage.GetAsByteArray();
            }

            return File(data, Constantes.AppExcel, "Reprogramas.xlsx");
        }

        public List<SelectListItem> GetHorarios()
        {
            List<SelectListItem> listaHoras = new List<SelectListItem>();

            listaHoras.Add(new SelectListItem { Text = "00:30", Value = "1" });
            listaHoras.Add(new SelectListItem { Text = "01:00", Value = "2" });
            listaHoras.Add(new SelectListItem { Text = "01:30", Value = "3" });
            listaHoras.Add(new SelectListItem { Text = "02:00", Value = "4" });
            listaHoras.Add(new SelectListItem { Text = "02:30", Value = "5" });
            listaHoras.Add(new SelectListItem { Text = "03:00", Value = "6" });
            listaHoras.Add(new SelectListItem { Text = "03:30", Value = "7" });
            listaHoras.Add(new SelectListItem { Text = "04:00", Value = "8" });
            listaHoras.Add(new SelectListItem { Text = "04:30", Value = "9" });
            listaHoras.Add(new SelectListItem { Text = "05:00", Value = "10" });
            listaHoras.Add(new SelectListItem { Text = "05:30", Value = "11" });
            listaHoras.Add(new SelectListItem { Text = "06:00", Value = "12" });
            listaHoras.Add(new SelectListItem { Text = "06:30", Value = "13" });
            listaHoras.Add(new SelectListItem { Text = "07:00", Value = "14" });
            listaHoras.Add(new SelectListItem { Text = "07:30", Value = "15" });
            listaHoras.Add(new SelectListItem { Text = "08:00", Value = "16" });
            listaHoras.Add(new SelectListItem { Text = "08:30", Value = "17" });
            listaHoras.Add(new SelectListItem { Text = "09:00", Value = "18" });
            listaHoras.Add(new SelectListItem { Text = "09:30", Value = "19" });
            listaHoras.Add(new SelectListItem { Text = "10:00", Value = "20" });
            listaHoras.Add(new SelectListItem { Text = "10:30", Value = "21" });
            listaHoras.Add(new SelectListItem { Text = "11:00", Value = "22" });
            listaHoras.Add(new SelectListItem { Text = "11:30", Value = "23" });
            listaHoras.Add(new SelectListItem { Text = "12:00", Value = "24" });
            listaHoras.Add(new SelectListItem { Text = "12:30", Value = "25" });
            listaHoras.Add(new SelectListItem { Text = "13:00", Value = "26" });
            listaHoras.Add(new SelectListItem { Text = "13:30", Value = "27" });
            listaHoras.Add(new SelectListItem { Text = "14:00", Value = "28" });
            listaHoras.Add(new SelectListItem { Text = "14:30", Value = "29" });
            listaHoras.Add(new SelectListItem { Text = "15:00", Value = "30" });
            listaHoras.Add(new SelectListItem { Text = "15:30", Value = "31" });
            listaHoras.Add(new SelectListItem { Text = "16:00", Value = "32" });
            listaHoras.Add(new SelectListItem { Text = "16:30", Value = "33" });
            listaHoras.Add(new SelectListItem { Text = "17:00", Value = "34" });
            listaHoras.Add(new SelectListItem { Text = "17:30", Value = "35" });
            listaHoras.Add(new SelectListItem { Text = "18:00", Value = "36" });
            listaHoras.Add(new SelectListItem { Text = "18:30", Value = "37" });
            listaHoras.Add(new SelectListItem { Text = "19:00", Value = "38" });
            listaHoras.Add(new SelectListItem { Text = "19:30", Value = "39" });
            listaHoras.Add(new SelectListItem { Text = "20:00", Value = "40" });
            listaHoras.Add(new SelectListItem { Text = "20:30", Value = "41" });
            listaHoras.Add(new SelectListItem { Text = "21:00", Value = "42" });
            listaHoras.Add(new SelectListItem { Text = "21:30", Value = "43" });
            listaHoras.Add(new SelectListItem { Text = "22:00", Value = "44" });
            listaHoras.Add(new SelectListItem { Text = "22:30", Value = "45" });
            listaHoras.Add(new SelectListItem { Text = "23:00", Value = "46" });
            listaHoras.Add(new SelectListItem { Text = "23:30", Value = "47" });

            return listaHoras;
        }

        public ActionResult ActualizacionPD()
        {
            return View();
        }
        public ActionResult ProgMedianoPO()
        {
            return View();
        }

        
    }
}
