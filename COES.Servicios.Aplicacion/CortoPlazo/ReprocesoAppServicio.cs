using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CortoPlazo
{
    public class ReprocesoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReprocesoAppServicio));

        #region Reproceso por ángulo óptimo

        /// <summary>
        /// Permite obtener el listado de procesos de CM
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerPeriodosCM(DateTime fecha)
        {
            return FactorySic.GetCmCostomarginalRepository().ObtenerUltimosProcesosCM(fecha);
        }

        /// <summary>
        /// Permite obtener el listado de procesos de CM
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerPeriodosCMPorVersion(DateTime fecha, int version)
        {
            return FactorySic.GetCmCostomarginalRepository().ObtenerUltimosProcesosCMPorVersion(fecha, version);
        }

        /// <summary>
        /// Permite obtener las lineas a utilizar
        /// </summary>
        /// <returns></returns>
        public List<EqCongestionConfigDTO> ObtenerLineas()
        {
            return FactorySic.GetEqCongestionConfigRepository().GetByCriteria(-1, ConstantesAppServicio.ActivoDesc.ToUpper(), -1,
                ConstantesCortoPlazo.IdLineaTransmision);
        }

        /// <summary>
        /// Permite obtener las barras
        /// </summary>
        /// <returns></returns>
        public List<CmConfigbarraDTO> ObtenerBarras()
        {
            return FactorySic.GetCmConfigbarraRepository().GetByCriteria((-1).ToString(), (-1).ToString()).
                Where(x=> !string.IsNullOrEmpty(x.Cnfbarnomtna)).ToList();
        }

        /// <summary>
        /// Obtención de parámetros para cálculo de ángulo óptimo
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="horas"></param>
        public ParametrosAnguloOptimo ObtenerParametrosAngulo(DateTime fechaConsulta, string horas, string nombreLinea)
        {
            ParametrosAnguloOptimo entity = new ParametrosAnguloOptimo();

            try
            {
                List<int> correlativos = horas.Split(',').Select(int.Parse).ToList();
                List<string[]> dataResult = new List<string[]>();
                List<string> validaciones = new List<string>();

                string[] header1 = new string[]{ "Hora", "BarraP", "", "", "BarraS", "", "", "Parámetros Línea: " + nombreLinea, "",  "", "Ángulo Óptimo", "", "" };
                string[] header2 = new string[] { "", "Nombre", "Vp", "θp", "Nombre", "Vs", "θs", "r", "x", "B", "Lím Cong.", "Flujo Lim Req", "θp (óptimo)" };

                

                string pathTrabajo = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];
                List<CmCostomarginalDTO> procesos = FactorySic.GetCmCostomarginalRepository().ObtenerUltimosProcesosCM(fechaConsulta);

                List<CmCongestionCalculoDTO> congestiones = FactorySic.GetCmCongestionCalculoRepository().ObtenerCongestionPorLinea(
                    fechaConsulta, nombreLinea);

                int index = 2;

                foreach (int correlativo in correlativos)
                {
                    CmCostomarginalDTO cm = procesos.Where(x => x.Cmgncorrelativo == correlativo).FirstOrDefault();
                    DateTime fecha = fechaConsulta;
                    if (cm.Cmgnfecha.Hour == 23 && cm.Cmgnfecha.Minute == 59) fecha = fechaConsulta.AddDays(1);

                    string path = fecha.Year + @"\" +
                                  fecha.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) +
                                  fecha.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) + @"\Corrida_" + correlativo + @"\";

                    List<FileData> files = FileServerScada.ListarArhivos(path, pathTrabajo);

                    FileData dat = files.Where(x => (x.Extension.Contains("RAW") || x.Extension.Contains("raw"))).FirstOrDefault();

                    if (dat != null)
                    {
                        path = path + dat.FileName;

                        List<NombreCodigoBarra> relacionBarra = FileHelperTna.ObtenerBarras(path, pathTrabajo);
                        List<NombreCodigoLinea> relacionLinea = FileHelperTna.ObtenerLineas(path, pathTrabajo, relacionBarra);
                        NombreCodigoLinea linea = relacionLinea.Where(x => x.Nombretna == nombreLinea).FirstOrDefault();

                        if (linea != null)
                        {
                            NombreCodigoBarra barrap = relacionBarra.Where(x => x.CodBarra == linea.CodBarra1).FirstOrDefault();
                            NombreCodigoBarra barras = relacionBarra.Where(x => x.CodBarra == linea.CodBarra2).FirstOrDefault();

                            string limite = string.Empty;
                            if(congestiones.Count>0)
                            {
                                CmCongestionCalculoDTO congestion = congestiones.Where(x => x.Cmcongperiodo == cm.Cmgncodi).FirstOrDefault();

                                if (congestion != null)
                                {
                                    limite = (congestion.Cmconglimite != null) ? (((decimal)congestion.Cmconglimite) / 100).ToString() : string.Empty;
                                }
                            }                            

                            string[] itemResult = new string[13];
                            itemResult[0] = cm.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora);
                            itemResult[1] = barrap.NombBarra;
                            itemResult[2] = barrap.VoltajePU.ToString();
                            itemResult[3] = barrap.Angulo.ToString();
                            itemResult[4] = barras.NombBarra;
                            itemResult[5] = barras.VoltajePU.ToString();
                            itemResult[6] = barras.Angulo.ToString();
                            itemResult[7] = linea.Rps.ToString();
                            itemResult[8] = linea.Xps.ToString();
                            itemResult[9] = linea.Bsh.ToString();
                            itemResult[10] = limite;
                            itemResult[11] = string.Empty;
                            itemResult[12] = string.Empty;
                            dataResult.Add(itemResult);
                        }
                        else
                        {
                            validaciones.Add("No se encontró la linea " + nombreLinea + " en el archivo .raw para la hora:" + cm.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora));
                        }
                    }
                    else
                    {
                        validaciones.Add("No se encontró archivo .raw para la hora:" + cm.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora));
                    }

                    index++;
                }

                List<string[]> datos = dataResult.OrderBy(x => x[0]).ToList();
                
                datos.Insert(0, header1);
                datos.Insert(1, header2);

                entity.Resultado = 1;
                entity.Datos = datos.ToArray();

                if (validaciones.Count > 0)
                {
                    entity.Resultado = 2;
                    entity.Validaciones = validaciones;
                }
            }
            catch(Exception ex)
            {
                entity.Resultado = -1;
                throw new Exception(ex.Message, ex);
            }

            return entity;
        }


        /// <summary>
        /// Permite calcular los ángulos óptimos
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public ParametrosAnguloOptimo CalcularAnguloOptimo(string[][] datos)
        {
            ParametrosAnguloOptimo entity = new ParametrosAnguloOptimo();
            CmUmbralComparacionDTO parametros = FactorySic.GetCmUmbralComparacionRepository().
                   GetById(ConstantesCortoPlazo.IdConfiguracionUmbral);

            int nroIteraciones = 30000;
            double variacion = 0.0001;

            if (parametros != null)
            {
                if (parametros.Cmumconumiter != null) nroIteraciones = (int)parametros.Cmumconumiter;
                if (parametros.Cmumcovarang != null) variacion = (double)parametros.Cmumcovarang;
            }

            try
            {
                int len = datos.Length;

                if (len > 2)
                {

                    for (int i = 2; i < len; i++)
                    {
                        double vp = double.Parse(datos[i][2]);
                        double op = double.Parse(datos[i][3]);
                        double vs = double.Parse(datos[i][5]);
                        double os = double.Parse(datos[i][6]);
                        double r = double.Parse(datos[i][7]);
                        double x = double.Parse(datos[i][8]);
                        double B = double.Parse(datos[i][9]);
                        double flujo = double.Parse(datos[i][11]);
                        bool flag = false;
                        double angulo = this.ObtenerAnguloOptimo(vp, op, vs, os, r, x, B, flujo, nroIteraciones, variacion, out flag);

                        if (flag)
                            datos[i][12] = angulo.ToString();
                        else
                            datos[i][12] = "No encontrado";
                    }
                }

                entity.Datos = datos;
                entity.Resultado = 1;
            }
            catch (Exception ex)
            {
                entity.Resultado = -1;
                throw new Exception(ex.Message, ex);
            }

            return entity;
        }

        /// <summary>
        /// Determinación del ángulo óptimo
        /// </summary>
        /// <param name="vp"></param>
        /// <param name="op"></param>
        /// <param name="vs"></param>
        /// <param name="os"></param>
        /// <param name="r"></param>
        /// <param name="x"></param>
        /// <param name="B"></param>
        /// <param name="flujo"></param>
        /// <param name="iteraciones"></param>
        /// <param name="variacion"></param>
        /// <returns></returns>
        public double ObtenerAnguloOptimo(double vp, double op, double vs, double os, double r, double x, double b,
            double flujo, int iteraciones, double variacion, out bool flag)
        {
            double anguloOptimo = op;
            double operador = 1;
            bool flagInicio = true;
            flag = true;
            int count = 0;

            while (true)
            {
                Complex Vp1 = new Complex(vp * Math.Cos(op * Math.PI / 180), vp * Math.Sin(op * Math.PI / 180));
                double cosop = Math.Cos(anguloOptimo * Math.PI / 180);
                double senop = Math.Sin(anguloOptimo * Math.PI / 180);
                Complex Vp2 = new Complex(vp * cosop, vp * senop);
                double cosos = Math.Cos(os * Math.PI / 180);
                double senos = Math.Sin(os * Math.PI / 180);
                Complex Vs = new Complex(vs * cosos, vs * senos);
                Complex Z = new Complex(r, x);
                Complex B = new Complex(0, b);

                Complex valor = Complex.Multiply(Vp2, Complex.Conjugate(Complex.Add(Complex.Divide(Complex.Subtract(Vp2, Vs), Z),
                    Complex.Multiply(B, Vp2))));

                double flujoFinal = valor.Real;

                if (flagInicio)
                {
                    if (flujo < flujoFinal)
                        operador = -1;
                    flagInicio = false;
                }
                else
                {
                    if (operador == -1)
                    {
                        if (flujo >= flujoFinal)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (flujo <= flujoFinal)
                        {
                            break;
                        }
                    }
                }

                anguloOptimo = anguloOptimo + operador * variacion;

                if (count > iteraciones)
                {
                    break;
                }

                count++;
            }

            return Math.Round(anguloOptimo, 4);
        }

        /// <summary>
        /// Permite ejecutar los reprocesos de CM
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="linea"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public ParametrosAnguloOptimo ReprocesarPorAnguloOptimo(DateTime fechaConsulta, string nombreLinea, string[][] data, int version)
        {
            ParametrosAnguloOptimo entity = new ParametrosAnguloOptimo();

            try
            {
                if (data.Length > 2)
                {
                    List<CmCostomarginalDTO> procesos = FactorySic.GetCmCostomarginalRepository().ObtenerUltimosProcesosCMPorVersion(fechaConsulta, version);
                    string pathTrabajo = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];
                    entity.ListaProceso = new List<DatosReproceso>();

                    for (int i = 2; i < data.Length; i++)
                    {
                        CmCostomarginalDTO cm = procesos.Where(x => x.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora) == data[i][0]).FirstOrDefault();

                        if (cm != null)
                        {
                            DateTime fecha = fechaConsulta;
                            if (cm.Cmgnfecha.Hour == 23 && cm.Cmgnfecha.Minute == 59) fecha = fechaConsulta.AddMinutes(1);

                            string path = fecha.Year + @"\" +
                                          fecha.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) +
                                          fecha.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) + @"\Corrida_" + cm.Cmgncorrelativo + @"\";

                            List<FileData> files = FileServerScada.ListarArhivos(path, pathTrabajo);

                            FileData dat = files.Where(x => (x.Extension.Contains("RAW") || x.Extension.Contains("raw"))).FirstOrDefault();

                            if (dat != null)
                            {
                                path = path + dat.FileName;

                                List<NombreCodigoBarra> relacionBarra = FileHelperTna.ObtenerBarras(path, pathTrabajo);
                                List<NombreCodigoLinea> relacionLinea = FileHelperTna.ObtenerLineas(path, pathTrabajo, relacionBarra);
                                NombreCodigoLinea linea = relacionLinea.Where(x => x.Nombretna == nombreLinea).FirstOrDefault();

                                if (linea != null)
                                {
                                    DatosReproceso result = new DatosReproceso();
                                    byte[] file = FileHelperTna.ObtenerFileRawAnguloOptimo(path, pathTrabajo, linea.CodBarra1, data[i][12]);
                                    result.FilePsse = new MemoryStream(file);
                                    result.FechaProceso = cm.Cmgnfecha;
                                    entity.ListaProceso.Add(result);
                                }
                            }
                        }
                    }
                }

                entity.Resultado = 1;
            }
            catch (Exception ex)
            {
                entity.Resultado = -1;
                throw new Exception(ex.Message, ex);
            }

            return entity;
        }


        #endregion

        #region Validación de procesos de CM


        /// <summary>
        /// Devuelve el listado de validaciones
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public ValidacionProcesosCMg ObtenerListadoValidacionesPorFecha(DateTime fechaConsulta, bool flagExportar, 
            string pathExportar, string fileExportar)
        {
            ValidacionProcesosCMg result = new ValidacionProcesosCMg();

            try
            {
                List<DetalleValidacionCM> lstValidaciones = new List<DetalleValidacionCM>();
                string pathTrabajo = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];
                List<CmCostomarginalDTO> procesos = FactorySic.GetCmCostomarginalRepository().ObtenerUltimosProcesosCM(fechaConsulta).OrderBy(x=>x.Cmgncodi).ToList();
               
                for (int bloqueHora = 1; bloqueHora <= 48; bloqueHora++)
                {
                    List<string> archivos = new List<string>();
                    bool flagExistencia = true;
                    bool flagTopologia = true;
                    bool flagArchivos = true;

                    CmCostomarginalDTO cm = procesos.Find(x => x.Cmgncodi == bloqueHora);

                    if (cm != null)
                    {
                        //- Validación de existencia de archivos

                        DateTime fecha = fechaConsulta;
                        if (cm.Cmgnfecha.Hour == 23 && cm.Cmgnfecha.Minute == 59) fecha = fechaConsulta.AddDays(1);

                        string path = fecha.Year + @"\" +
                                          fecha.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) +
                                          fecha.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) + @"\Corrida_" + cm.Cmgncorrelativo + @"\";

                        List<FileData> files = FileServer.ListarArhivos(path, pathTrabajo);

                        string[] filenames = { "ENTRADAGAMS", "LOGPROCESO", "LST", "MODELO", "PREPROCESADOR", "PSSE", "RESULTADO_GAMS_ANALISIS", "RESULTADO_GAMS"};
                        string[] extensiones = { ".DAT", ".csv", ".lst", ".gms", ".gen", ".raw", ".csv", ".csv" };
                        int index = 0;
                        foreach (string filename in filenames)
                        {
                            FileData dat = files.Where(x => x.FileName.Contains(filename)).FirstOrDefault();

                            if (dat == null)
                            {
                                string name = filename + "_" + cm.Cmgnfecha.ToString("yyyyMMddHHmm") + "." + extensiones[index];
                                archivos.Add(name);
                            }

                            index++;
                        }

                        if (archivos.Count > 0)
                        {
                            flagArchivos = false;
                        }
                    
                        //- Validación de topología

                        int periodo = UtilCortoPlazoTna.CalcularPeriodo(cm.Cmgnfecha);
                        if (periodo == 0) periodo = 48;
                        CpTopologiaDTO topologiaFinal = (new McpAppServicio()).ObtenerTopologiaFinalPorFecha(fechaConsulta, ConstantesCortoPlazo.TopologiaDiario, periodo);

                        if (topologiaFinal.Topcodi != cm.Topcodi)
                        {
                            flagTopologia = false;
                        }

                    }
                    else
                    {
                        flagExistencia = false;
                    }


                    if (!flagArchivos || !flagTopologia || !flagExistencia)
                    {
                        DetalleValidacionCM entity = new DetalleValidacionCM();

                        entity.Hora = ObtenerValHora(bloqueHora);

                        if (cm != null)
                        {
                            entity.FechaEjecucion = ((DateTime)cm.Cmgnfeccreacion).ToString(ConstantesAppServicio.FormatoFechaFull);
                            entity.Estimador = cm.TipoEstimador;
                            entity.Programa = cm.VersionPDO;
                        }
                        
                        if (!flagArchivos) entity.Archivos = archivos;
                        if (!flagArchivos) entity.MensajeArhivo = "No se encontraron los archivos:";
                        if (!flagTopologia) entity.MensajeTopologia = "El PDO-RDO no corresponde.";
                        if (!flagExistencia) entity.MensajePeriodo = "No se encontró el periodo.";

                        lstValidaciones.Add(entity);
                    }
                }

                result.Resultado = 1;
                result.ListaDetalle = lstValidaciones;

                if (result.ListaDetalle.Count == 0) result.Resultado = 2;

                if (result.Resultado == 1)
                {
                    if (flagExportar)
                    {
                        this.ExportarValidacionCM(result.ListaDetalle, pathExportar, fileExportar,
                            fechaConsulta.ToString(ConstantesAppServicio.FormatoFecha));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Resultado = -1;
                throw new Exception(ex.Message, ex);                
            }

            return result;
        }


        public void ExportarValidacionCM(List<DetalleValidacionCM> list, string path, string filename, string fecha)
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("VALIDACION CM");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "Validación de Procesos CM";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[3, 3].Value = "FECHA: " + fecha;

                        ws.Cells[index, 2].Value = "Periodo";
                        rg = ws.Cells[index, 2, index, 5];
                        rg.Merge = true;

                        ws.Cells[index, 6].Value = "Mensaje";
                        rg = ws.Cells[index, 6, index + 1, 6];
                        rg.Merge = true;                        

                        ws.Cells[index + 1, 2].Value = "HORA";
                        ws.Cells[index + 1, 3].Value = "FECHA EJECUCIÓN";
                        ws.Cells[index + 1, 4].Value = "ESTIMADOR";
                        ws.Cells[index + 1, 5].Value = "RDO - PDO";
                       
                        rg = ws.Cells[index, 2, index + 1, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 7;
                        foreach (DetalleValidacionCM item in list)
                        {
                            ws.Cells[index, 2].Value = item.Hora;
                            ws.Cells[index, 3].Value = item.FechaEjecucion;
                            ws.Cells[index, 4].Value = item.Estimador;
                            ws.Cells[index, 5].Value = item.Programa;

                            if (!string.IsNullOrEmpty(item.MensajePeriodo))
                            {
                                ws.Cells[index, 6].Value = item.MensajePeriodo;
                            }
                            else 
                            {
                                string mensaje = string.Empty;

                                if(!string.IsNullOrEmpty(item.MensajeTopologia))
                                {
                                    mensaje = mensaje + item.MensajeTopologia + "\n";
                                }

                                if (!string.IsNullOrEmpty(item.MensajeArhivo))
                                {
                                    mensaje = mensaje + item.MensajeArhivo + "\n";

                                    foreach(string fi in item.Archivos)
                                    {
                                        mensaje = mensaje + "      " + fi + "\n";
                                    }
                                }

                                ws.Cells[index, 6].Value = mensaje;
                                ws.Cells[index, 6].Style.WrapText = true;
                            }
                                                      

                            index++;
                        }

                        rg = ws.Cells[7, 2, index - 1, 6];

                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;
                        rg = ws.Cells[5, 3, index, 6];
                        rg.AutoFitColumns();
                        ws.Column(6).Width = 70;

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

        private string ObtenerValHora(int bloqueHora)
        {
            string hora = "";

            var ho = (bloqueHora) / 2;
            string strHo = ho.ToString("00");

            int res = bloqueHora % 2;
            string strMin = res == 0 ? "00" : "30";

            hora = strHo + ":" + strMin;

            if (bloqueHora == 48) hora = "23:59";

            return hora;
        }

       
      

        #endregion

        #region Reproceso por transacciones internacionales

        /// <summary>
        /// Permite obtener las transacciones internaciones para la fecha seleccionada
        /// </summary>
        /// <param name="fecha"></param>
        public List<EveIeodcuadroDTO> ObtenerTransacciones(DateTime fecha)
        {
            return FactorySic.GetEveIeodcuadroRepository().BuscarOperaciones(1, ConstantesCortoPlazo.TipoTransaccionInternacional, 
                fecha, fecha, -1, -1);
        }

        /// <summary>
        /// Permite obtener los datos de la energía importada por transacción internacional
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="operaciones"></param>
        /// <param name="validacion"></param>
        /// <returns></returns>
        public ParametrosAnguloOptimo CalcularEnergiaImportada(DateTime fecha, string operaciones, int idBarra, int version)
        {
            ParametrosAnguloOptimo result = new ParametrosAnguloOptimo();

            try
            {
                List<DatosTransaccion> dataResult = new List<DatosTransaccion>();               

                List<string> validaciones = new List<string>();
                List<int> idsOperaciones = operaciones.Split(',').Select(int.Parse).ToList();
                List<EqCongestionConfigDTO> equipos = FactorySic.GetEqCongestionConfigRepository().GetByCriteria(-1,
                    (-1).ToString(), -1, ConstantesCortoPlazo.IdLineaTransmision);
                string pathTrabajo = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];
                List<CmCostomarginalDTO> procesos = FactorySic.GetCmCostomarginalRepository().ObtenerUltimosProcesosCMPorVersion(fecha, version);
                CmConfigbarraDTO entityBarra = FactorySic.GetCmConfigbarraRepository().GetById(idBarra);

                foreach (int id in idsOperaciones)
                {
                    EveIeodcuadroDTO entity = FactorySic.GetEveIeodcuadroRepository().ObtenerIeodcuadro(id);

                    EqCongestionConfigDTO config = equipos.Where(x => x.Equicodi == entity.Equicodi).FirstOrDefault();

                    if (config != null)
                    {
                        if (config.Canalcodi != null)
                        {
                            MeScadaSp7DTO scada = FactoryScada.GetMeScadaSp7Repository().ObtenerDemandaPorInterconexion(fecha,
                                (int)config.Canalcodi);

                            if (scada == null)
                            {
                                validaciones.Add("No existen datos de potencia en SCADA para el código: " + (int)config.Canalcodi);
                            }

                            DateTime fechaIni = (DateTime)entity.Ichorini;
                            DateTime fechaFin = (DateTime)entity.Ichorfin;
                            int periodoInicial = Convert.ToInt32(Math.Ceiling(((decimal)(fechaIni.Hour * 60 + fechaIni.Minute) / 30.0M)));
                            int periodoFinal = Convert.ToInt32(Math.Floor(((decimal)(fechaFin.Hour * 60 + fechaFin.Minute) / 30.0M)));

                            if (fechaIni.Hour == 0 && fechaIni.Minute == 0) periodoInicial = 1;
                            if (fechaFin.Year == fecha.Year && fechaFin.Month == fecha.Month && fechaFin.Day == fecha.Day + 1) periodoFinal = 48;

                            for (int i = periodoInicial; i <= periodoFinal; i++)
                            {
                                decimal? energia = (scada != null) ? (decimal?)Convert.ToDecimal(scada.GetType().
                                GetProperty(ConstantesAppServicio.CaracterH + i * 2).GetValue(scada, null)) : null;

                                CmCostomarginalDTO cm = procesos.Where(x => x.Cmgncodi == i).FirstOrDefault();

                                if (cm != null)
                                {
                                    DateTime fechaFile = fecha;
                                    if (i == 48) fechaFile = fecha.AddDays(1);

                                    string path = fechaFile.Year + @"\" +
                                            fechaFile.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) +
                                            fechaFile.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) + @"\Corrida_" + cm.Cmgncorrelativo + @"\";

                                    List<FileData> files = FileServer.ListarArhivos(path, pathTrabajo);

                                    FileData dat = files.Where(x => (x.Extension.Contains("RAW") || x.Extension.Contains("raw"))).FirstOrDefault();

                                    if (dat != null)
                                    {
                                        path = path + dat.FileName;

                                        List<NombreCodigoBarra> relacionBarra = FileHelperTna.ObtenerBarras(path, pathTrabajo);

                                        NombreCodigoBarra barra = relacionBarra.Where(x => x.NombBarra == entityBarra.Cnfbarnodo).FirstOrDefault();
                                        if (barra != null)
                                        {
                                            //string[] data = new string[6];
                                            DatosTransaccion itemResult = new DatosTransaccion();

                                            itemResult.Select = true;
                                            itemResult.Fecha = fecha.ToString(ConstantesAppServicio.FormatoFecha);
                                            itemResult.Hora = cm.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora);
                                            itemResult.Codigo = barra.CodBarra;
                                            itemResult.Barra = barra.NombBarra;
                                            itemResult.Potencia = (energia != null) ? (decimal)energia : 0;

                                            dataResult.Add(itemResult);
                                        }
                                        else
                                        {
                                            validaciones.Add("No se encontró la barra " + ConstantesCortoPlazo.BarraTalara + " en el archivo .raw de las " + cm.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora));
                                        }
                                    }
                                    else
                                    {
                                        validaciones.Add("No se encontró archivo .raw para la hora:" + cm.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora));
                                    }
                                }
                                else
                                {
                                    validaciones.Add("No se encontró el periodo de CM Nro:" + i);
                                }
                            }
                        }
                        else
                        {
                            validaciones.Add("La línea " + config.Nombretna1 + " no tiene relación con el código SCADA");
                        }
                    }
                    else
                    {
                        EqEquipoDTO equipo = FactorySic.GetEqEquipoRepository().GetById((int)entity.Equicodi);
                        validaciones.Add("No existe equivalencia de códigos para la linea: " + equipo.Equinomb);
                    }
                }

                result.Resultado = 1;
                result.Validaciones = validaciones;
                //List<string[]> datos  = dataResult.OrderBy(x => x[1]).ToList();
                //string[] header = new string[] { "Seleccionar", "Fecha", "Hora", "Cod Barra", "Barra", "Potencia Importada" };
                //datos.Insert(0, header);
                result.DatosTransaccion = dataResult.OrderBy(x=>x.Hora).ToList();               

                if (validaciones.Count > 0)
                {
                    result.Resultado = 2;
                }
            }
            catch(Exception ex)
            {
                result.Resultado = -1;
                throw new Exception(ex.Message, ex);

            }

            return result;
        }

        /// <summary>
        /// Permite obtener los archivos para el recalculo de los cm por tie
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="datos"></param>
        /// <returns></returns>
        public ParametrosAnguloOptimo ReprocesoPorTransaccionInternacional(DateTime fechaConsulta, string[][] data, int idBarra, int version)
        {
            ParametrosAnguloOptimo entity = new ParametrosAnguloOptimo();

            try
            {
                if (data.Length > 0)
                {
                    List<CmCostomarginalDTO> procesos = FactorySic.GetCmCostomarginalRepository().ObtenerUltimosProcesosCMPorVersion(fechaConsulta, version);
                    string pathTrabajo = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];
                    entity.ListaProceso = new List<DatosReproceso>();

                    CmConfigbarraDTO entityBarra = FactorySic.GetCmConfigbarraRepository().GetById(idBarra);

                    for (int i = 0; i < data.Length; i++)
                    {
                        CmCostomarginalDTO cm = procesos.Where(x => x.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora) == data[i][2]).FirstOrDefault();

                        if (cm != null)
                        {
                            DateTime fecha = fechaConsulta;
                            if (cm.Cmgnfecha.Hour == 23 && cm.Cmgnfecha.Minute == 59) fecha = cm.Cmgnfecha.AddMinutes(1);

                            string path = fecha.Year + @"\" +
                                          fecha.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) +
                                          fecha.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) + @"\Corrida_" + cm.Cmgncorrelativo + @"\";

                            List<FileData> files = FileServerScada.ListarArhivos(path, pathTrabajo);

                            FileData dat = files.Where(x => (x.Extension.Contains("RAW") || x.Extension.Contains("raw"))).FirstOrDefault();

                            if (dat != null)
                            {
                                path = path + dat.FileName;

                                DatosReproceso result = new DatosReproceso();
                                byte[] file = FileHelperTna.ObtenerFileTransaccionInternacional(path, pathTrabajo, data[i][3], data[i][5],
                                    entityBarra.Cnfbarnomtna);
                                result.FilePsse = new MemoryStream(file);
                                result.FechaProceso = cm.Cmgnfecha;

                                if (entity.ListaProceso.Where(x => x.FechaProceso == result.FechaProceso).Count() == 0)
                                    entity.ListaProceso.Add(result);

                            }
                        }
                    }
                }

                entity.Resultado = 1;
            }
            catch (Exception ex)
            {
                entity.Resultado = -1;
                throw new Exception(ex.Message, ex);
            }

            return entity;
        }



        #endregion

    }

    public class ParametrosAnguloOptimo
    {
        public int Resultado { get; set; }
        public string[][] Datos { get; set; }
        public List<DatosTransaccion> DatosTransaccion { get; set; }
        public List<DatosReproceso> ListaProceso { get; set; }
        public List<string> Validaciones { get; set; }
        public int Cantidad { get; set; }
        public List<DatosReproceso> ListaProcesada { get; set; }
    }

    public class DatosReproceso
    {
        public DateTime FechaProceso { get; set; }
        public Stream FilePsse { get; set; }
    }

    public class DatosTransaccion
    { 
        public bool Select { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Codigo { get; set; }
        public string Barra { get; set; }
        public decimal Potencia { get; set; }
    }
}
