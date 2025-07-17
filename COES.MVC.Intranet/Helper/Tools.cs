using COES.MVC.Intranet.SeguridadServicio;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace COES.MVC.Intranet.Helper
{
    /// <summary>
    /// Nombre de vistas parciales
    /// </summary>
    public struct VistasParciales
    {
        public const string Grilla = "Grilla";
        public const string EditConfig = "EditConfig";
        public const string Stock = "Stock";
        public const string Copia = "Copiado";
        public const string Editar = "Editar";
        public const string Detalle = "Detalle";
        public const string Empresa = "Empresa";
        public const string Matriz = "Matriz";
        public const string CargarHora = "CargarHora";
    }

    /// <summary>
    /// Clase para manejo de mensajes
    /// </summary>
    public class ResultadoMensaje
    {
        public int IdResultado { get; set; }
        public int Id { get; set; }
        public string Mensaje { get; set; }
        public string[] Validaciones { get; set; }

        public string Ruc { get; set; }
    }

    /// <summary>
    /// Clase con métodos de validación
    /// </summary>
    public class Validaciones
    {
        /// <summary>
        /// Permite validar el acceso a una determinada opción
        /// </summary>
        /// <param name="idAplicacion"></param>
        /// <param name="idOpcion"></param>
        /// <param name="userLogin"></param>
        /// <param name="idPermiso"></param>
        /// <returns></returns>
        public static bool ValidarAcceso(object idOpcion, int idPermiso, string userLogin)
        {
            if (idOpcion != null)
            {
                bool flag = (new SeguridadServicio.SeguridadServicioClient()).ValidarPermisoOpcion(Constantes.IdAplicacion,
                    (int)idOpcion, idPermiso, userLogin);

                return flag;
            }
            return false;
        }
    }

    /// <summary>
    /// Clase que contiene métodos de utilidad
    /// </summary>
    public class Tools
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(Tools));
        /// <summary>
        /// Permite obtener la hora en base al número indicado
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string ObtenerHoraMedicion(int i)
        {
            string hora = "";
            string minuto = (i % 2 == 0) ? "00" : "30";

            if (i == 1 || i == 48) hora = "00";
            else
            {
                hora = (i / 2).ToString().PadLeft(2, '0');
            }

            return hora + ":" + minuto;
        }

        /// <summary>
        /// Permite obtener el color de la lista definida inicialmente
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public static string GetColor(int indice)
        {
            string[] colores = {
                "#00FF78","#EDFD64","#0000FF","#FF0000","#01FFFE","#FFA6FE","#FFDB66",
                "#E7B2FA","#01D26F","#EFE3AF","#007DB5","#FF00F6","#F98500","#C8FE85",
                "#86F0E2","#0076FF","#D5FF00","#FF937E","#6A826C","#FF029D","#FE8900",
                "#7A4782","#7E2DD2","#85A900","#FF0056","#A42400","#00AE7E","#683D3B",
                "#BDC6FF","#263400","#C28C9F","#FF74A3","#004754","#E56FFE","#788231",
                "#0E4CA1","#91D0CB","#BE9970","#968AE8","#BB8800","#DEFF74","#00FFC6",
                "#FFE502","#620E00","#008F9C","#98FF52","#7544B1","#B500FF","#FF6E41",
                "#005F39","#6B6882","#5FAD4E","#A75740","#A5FFD2","#FFB167","#009BFF",
                "#00FF78","#EDFD64","#0000FF","#FF0000","#01FFFE","#FFA6FE","#FFDB66",
                "#006401","#01D0FF","#95003A","#007DB5","#FF00F6","#F98500","#774D00",
                "#90FB92","#0076FF","#D5FF00","#FF937E","#6A826C","#FF029D","#FE8900",
                "#7A4782","#7E2DD2","#85A900","#FF0056","#A42400","#00AE7E","#683D3B",
                "#BDC6FF","#263400","#C28C9F","#FF74A3","#004754","#E56FFE","#788231",
                "#0E4CA1","#91D0CB","#BE9970","#968AE8","#BB8800","#DEFF74","#00FFC6",
                "#FFE502","#620E00","#008F9C","#98FF52","#7544B1","#B500FF","#FF6E41",
                "#005F39","#6B6882","#5FAD4E","#A75740","#A5FFD2","#FFB167","#009BFF",
                "#00FF78","#EDFD64","#0000FF","#FF0000","#01FFFE","#FFA6FE","#FFDB66",
                "#006401","#01D0FF","#95003A","#007DB5","#FF00F6","#F98500","#774D00",
                "#90FB92","#0076FF","#D5FF00","#FF937E","#6A826C","#FF029D","#FE8900",
                "#7A4782","#7E2DD2","#85A900","#FF0056","#A42400","#00AE7E","#683D3B",
                "#BDC6FF","#263400","#C28C9F","#FF74A3","#004754","#E56FFE","#788231",
                "#0E4CA1","#91D0CB","#BE9970","#968AE8","#BB8800","#DEFF74","#00FFC6",
                "#FFE502","#620E00","#008F9C","#98FF52","#7544B1","#B500FF","#FF6E41",
                "#005F39","#6B6882","#5FAD4E","#A75740","#A5FFD2","#FFB167","#009BFF",
                "#00FF78","#EDFD64","#0000FF","#FF0000","#01FFFE","#FFA6FE","#FFDB66",
                "#006401","#01D0FF","#95003A","#007DB5","#FF00F6","#F98500","#774D00",
                "#90FB92","#0076FF","#D5FF00","#FF937E","#6A826C","#FF029D","#FE8900",
                "#7A4782","#7E2DD2","#85A900","#FF0056","#A42400","#00AE7E","#683D3B",
                "#BDC6FF","#263400","#C28C9F","#FF74A3","#004754","#E56FFE","#788231",
                "#0E4CA1","#91D0CB","#BE9970","#968AE8","#BB8800","#DEFF74","#00FFC6",
                "#FFE502","#620E00","#008F9C","#98FF52","#7544B1","#B500FF","#FF6E41",
                "#005F39","#6B6882","#5FAD4E","#A75740","#A5FFD2","#FFB167","#009BFF"
            };

            if (indice <= 224)
            {
                return colores[indice];
            }

            return "#000000";
        }

        /// <summary>
        /// Permite obtener la mediana de un conjunto de datos
        /// </summary>
        /// <param name="xs"></param>
        /// <returns></returns>
        public static decimal GetMediana(List<decimal> xs)
        {
            var ys = xs.OrderBy(x => x).ToList();
            double mid = (ys.Count - 1) / 2.0;
            return (ys[(int)(mid)] + ys[(int)(mid + 0.5)]) / 2;
        }

        /// <summary>
        /// Permite obtener el mayor valor del conjunto de datos
        /// </summary>
        /// <param name="xs"></param>
        /// <returns></returns>
        public static decimal GetMaximo(List<decimal> xs)
        {
            return xs.Max();
        }

        /// <summary>
        /// Permite obtener el menor valor del conjunto de datos
        /// </summary>
        /// <param name="xs"></param>
        /// <returns></returns>
        public static decimal GetMinimo(List<decimal> xs)
        {
            return xs.Min();
        }

        /// <summary>
        /// Permite obtener la descripción de la agrupación
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        public static string ObtenerClasificacion(int clasificacion)
        {
            string resultado = string.Empty;

            switch (clasificacion)
            {
                case 1:
                    resultado = "MARTES - VIERNES";
                    break;
                case 2:
                    resultado = "SÁBADO";
                    break;
                case 3:
                    resultado = "DOMINGO";
                    break;
                case 4:
                    resultado = "LUNES";
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener el nombre de un dia de la semana
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string ObtenerNombreDia(int index)
        {
            string resultado = string.Empty;

            switch (index)
            {
                case 1:
                    resultado = "SÁBADO";
                    break;
                case 2:
                    resultado = "DOMINGO";
                    break;
                case 3:
                    resultado = "LUNES";
                    break;
                case 4:
                    resultado = "MARTES";
                    break;
                case 5:
                    resultado = "MIÉRCOLES";
                    break;
                case 6:
                    resultado = "JUEVES";
                    break;
                case 7:
                    resultado = "VIERNES";
                    break;
            }

            return resultado;
        }

        public static string ObtenerNombreDia(DayOfWeek index)
        {
            string resultado = string.Empty;

            switch (index)
            {
                case DayOfWeek.Saturday:
                    resultado = "SÁBADO";
                    break;
                case DayOfWeek.Sunday:
                    resultado = "DOMINGO";
                    break;
                case DayOfWeek.Monday:
                    resultado = "LUNES";
                    break;
                case DayOfWeek.Tuesday:
                    resultado = "MARTES";
                    break;
                case DayOfWeek.Wednesday:
                    resultado = "MIÉRCOLES";
                    break;
                case DayOfWeek.Thursday:
                    resultado = "JUEVES";
                    break;
                case DayOfWeek.Friday:
                    resultado = "VIERNES";
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener el nombre del mes en base al número
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string ObtenerNombreMes(int index)
        {
            string resultado = string.Empty;

            switch (index)
            {
                case 1:
                    resultado = "Enero";
                    break;
                case 2:
                    resultado = "Febrero";
                    break;
                case 3:
                    resultado = "Marzo";
                    break;
                case 4:
                    resultado = "Abril";
                    break;
                case 5:
                    resultado = "Mayo";
                    break;
                case 6:
                    resultado = "Junio";
                    break;
                case 7:
                    resultado = "Julio";
                    break;
                case 8:
                    resultado = "Agosto";
                    break;
                case 9:
                    resultado = "Setiembre";
                    break;
                case 10:
                    resultado = "Octubre";
                    break;
                case 11:
                    resultado = "Noviembre";
                    break;
                case 12:
                    resultado = "Diciembre";
                    break;
                default:
                    resultado = string.Empty;
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener el número de dias de un mes
        /// </summary>
        /// <param name="nroMes"></param>
        /// <returns></returns>
        public static int ObtenerNroDias(int anio, int nroMes)
        {
            return DateTime.DaysInMonth(anio, nroMes);
        }

        /// <summary>
        /// Permite obtener el tree de opciones
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ObtenerTreeOpciones(List<OptionDTO> list, string nodos)
        {
            int idPadre = 1;

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("[\n");

            List<OptionDTO> listItem = list.Where(x => x.PadreCodi == idPadre).ToList();
            int contador = 0;
            foreach (OptionDTO item in listItem)
            {
                List<OptionDTO> listHijos = list.Where(x => x.PadreCodi == item.OptionCode).ToList();
                if (listHijos.Count > 0)
                {
                    strHtml.Append("   {'key': '" + item.OptionCode + "', 'title': '" + item.OptionName +
                        "' , 'expanded' : 'true', checkbox : true,  'children':[\n");
                    strHtml.Append(ObtenerSubMenu(listHijos, list, "   ", nodos));
                    if (contador < listItem.Count - 1) strHtml.Append("   ]},\n");
                    else strHtml.Append("   ]}\n");
                }
                else
                {
                    if (contador < listItem.Count - 1)
                        strHtml.Append("   {'key': '" + item.OptionCode + "', 'title': '" +
                            item.OptionName + "' , selected : " + ObtieneSeleccionNodo(item.OptionCode, item.Favorito, nodos) + "},\n");
                    else
                        strHtml.Append("   {'key': '" + item.OptionCode + "', 'title': '" +
                            item.OptionName + "',  selected : " + ObtieneSeleccionNodo(item.OptionCode, item.Favorito, nodos) + "}\n");
                }
                contador++;
            }

            strHtml.Append("]");
            return strHtml.ToString();
        }

        /// <summary>
        /// Funcion recursiva para obtener el menu
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string ObtenerSubMenu(List<OptionDTO> list, List<OptionDTO> listGeneral, string pad, string nodos)
        {
            StringBuilder strHtml = new StringBuilder();

            int contador = 0;
            foreach (OptionDTO item in list)
            {
                List<OptionDTO> listHijos = listGeneral.Where(x => x.PadreCodi == item.OptionCode).ToList();

                if (listHijos.Count > 0)
                {
                    strHtml.Append(pad + "    {'key': '" + item.OptionCode + "' , checkbox : true , 'title': '" +
                        item.OptionName + "', 'children':[\n");
                    strHtml.Append(ObtenerSubMenu(listHijos, listGeneral, pad + "  ", nodos));
                    if (contador < list.Count - 1) strHtml.Append(pad + "    ]},\n");
                    else strHtml.Append(pad + "    ]}\n");
                }
                else
                {
                    if (contador < list.Count - 1)
                        strHtml.Append("   {'key': '" + item.OptionCode + "', 'title': '" +
                           item.OptionName + "' , selected : " + ObtieneSeleccionNodo(item.OptionCode, item.Favorito, nodos) + "},\n");
                    else
                        strHtml.Append("   {'key': '" + item.OptionCode + "', 'title': '" +
                            item.OptionName + "',  selected : " + ObtieneSeleccionNodo(item.OptionCode, item.Favorito, nodos) + "}\n");
                }
                contador++;
            }

            return strHtml.ToString();
        }

        /// <summary>
        /// Permite verificar la selección de un nodo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selected"></param>
        /// <param name="nodos"></param>
        /// <returns></returns>
        private static string ObtieneSeleccionNodo(int id, int selected, string nodos)
        {
            string[] nodes = nodos.Split(',');

            if (nodes.Contains(id.ToString()) || selected > 0)
            {
                return "true";
            }
            return "false";
        }

        /// <summary>
        /// Permite verificar el acceso
        /// </summary>
        /// <param name="idOpcion"></param>
        /// <param name="userName"></param>
        /// <param name="idAccion"></param>
        /// <returns></returns>
        public static bool VerificarAcceso(object idOpcion, string userName, int idAccion)
        {
            string appIISName = ConfigurationManager.AppSettings["AppIISName"];
            SeguridadServicio.SeguridadServicioClient cliente = new SeguridadServicio.SeguridadServicioClient();
            Boolean isCodigoOpcion = false;
            try
            {
                Uri uriRequest = HttpContext.Current.Request.UrlReferrer;
                String[] pathValidador = uriRequest.LocalPath.Replace(appIISName, "").Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                isCodigoOpcion = cliente.ValidarAccesoOpcion(pathValidador[1], pathValidador[2], Constantes.IdAplicacion, userName);

                log.Debug("PERMISO - debug: uriRequest.LocalPath => " + uriRequest.LocalPath);

                if (isCodigoOpcion == false)
                {
                    isCodigoOpcion = cliente.ValidarAccesoOpcion(pathValidador[0]+"/"+ pathValidador[1], pathValidador[2], Constantes.IdAplicacion, userName);
                }
            }
            catch(Exception ex)
            {
                log.Error("PERMISO - ERROR: ", ex);
            }

            if (isCodigoOpcion == true)
            {
                return true;
            }else if (idOpcion != null)
            {
                return cliente.ValidarPermisoOpcion(Constantes.IdAplicacion, Convert.ToInt32(idOpcion), idAccion, userName);
            }

            return false;
        }

        /// <summary>
        /// Permite obtener el bloque horario
        /// </summary>
        /// <param name="Bloque">Nro de bloque: 1= 00:30</param>
        /// <returns>retorna el bloque en formato 30 minutos</returns>
        public static string ObtenerBloqueHorario(int Bloque)
        {
            string BloqueResultado = "";

            if (Bloque / 2 < 10)
                BloqueResultado = "0";

            BloqueResultado += (Bloque / 2).ToString() + ":";
            Bloque -= 2 * (Bloque / 2);

            if (Bloque == 0)
                BloqueResultado += "00";
            else
                BloqueResultado += "30";

            return BloqueResultado;
        }


        /// <summary>
        /// Permite obtener el número de semana del año
        /// </summary>
        /// <param name="Fecha">Fecha a consultar</param>
        /// <returns>Semana del año</returns>
        public static int ObtenerNroSemanaAnio(DateTime Fecha)
        {
            GregorianCalendar Calendario = new GregorianCalendar();
            return Calendario.GetWeekOfYear(Fecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Saturday);
        }

        /// <summary>
        /// Permite obtener la fecha de inicio de semana operativa siguiente
        /// </summary>
        /// <param name="Fecha">Fecha de la semana</param>
        /// <returns>Día de inicio de semana operativa</returns>
        public static DateTime ObtenerFechaInicioSemanaOperativa(DateTime Fecha)
        {
            if (Fecha.DayOfWeek == DayOfWeek.Saturday)
                return Fecha.AddDays(6);
            else
                return Fecha.AddDays(5 - (int)Fecha.DayOfWeek);
        }

        public static DateTime ObtenerFechaInicioSemana(DateTime Fecha)
        {
            if (Fecha.DayOfWeek == DayOfWeek.Saturday)
                return Fecha;
            else
                return Fecha.AddDays(-(int)Fecha.DayOfWeek - 1);
        }

        /// <summary>
        /// Devuelve el ultimo dia del mes de una fecha
        /// </summary>
        /// <param name="Fecha"></param>
        /// <returns></returns>
        public static DateTime ObtenerUltimoDiaDelMes(DateTime Fecha)
        {
            DateTime primerDiaMes = new DateTime(Fecha.Year, Fecha.Month, 1);
            DateTime primerDiaMesSiguiente = primerDiaMes.AddMonths(1);
            DateTime ultimoDiaMesActual = primerDiaMesSiguiente.AddDays(-1);

            return ultimoDiaMesActual;
        }
    }
}