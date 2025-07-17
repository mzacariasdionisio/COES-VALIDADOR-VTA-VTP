using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Helper
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
        public string Mensaje { get; set; }
        public string[] Validaciones { get; set; }
    }

    /// <summary>
    /// Permisos sobre una opcion
    /// </summary>
    public struct Permisos
    { 
        public const int Grabar = 1;
        public const int Editar = 2;
        public const int Nuevo = 3;
        public const int Consultar = 4;
        public const int Eliminar = 5;
        public const int Copiar = 6;
        public const int Exportar = 7;
        public const int Anular = 8;
        public const int Imprimir = 9;
        public const int Detalle = 10;
        public const int Rechazar = 11;
        public const int Aprobar = 12;
    }

  
    /// <summary>
    /// Clase que contiene métodos de utilidad
    /// </summary>
    public class Tools
    {
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
                default :
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
    }
}