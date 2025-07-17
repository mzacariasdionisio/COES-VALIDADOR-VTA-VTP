using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace COES.MVC.Extranet.Helper
{
    /// <summary>
    /// Clase con métodos de utilidades
    /// </summary>
    public class HelperApp
    {
        public static string ObtenerEmailRemitente ()
        {
            return ConfigurationManager.AppSettings[DatosConfiguracion.MailFrom];
        }
    }

    /// <summary>
    /// Clase con metodos que permiten generar html
    /// </summary>
    public class ToolHtml
    {
        /// <summary>
        /// Crea una lista con los errores
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static string ObtieneListaValidacion(List<string> errors)
        {
            StringBuilder str = new StringBuilder();

            str.Append("<ul>");

            foreach (string error in errors)
            {
                str.Append(string.Format("<li>{0}</li>", error));
            }

            str.Append("</ul>");

            return str.ToString();
        }

        /// <summary>
        /// Crea un lista con el error
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string ObtenerValidacion(string error)
        {
            StringBuilder str = new StringBuilder();

            str.Append("<ul>");

            str.Append(string.Format("<li>{0}</li>", error));

            str.Append("</ul>");

            return str.ToString();
        }
    }


    /// <summary>
    /// Clase para enviar el resultado de la operación
    /// </summary>
    public class ResultadoOperacion
    {
        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Validacion { get; set; }
    }

    /// <summary>
    /// Textos de validaciones de archivos
    /// </summary>
    public class ValidacionArchivo
    {
        public const string OK = "OK";
        public const string ExistenciaDatos = "EXISTE";
        public const string NoExiste = "No existe archivo, por favor cargue.";
        public const string Error = "ERROR";
        public const string FechasNoCoinciden = "La fecha de carga y la fecha del formato no coinciden.";
        public const string FechaFormatoIncorrecto = "La fecha del formato no tiene formato correcto dd/MM/yyyy.";
        public const string FechaNoExisteFormato = "No existe la fecha de carga en el formato.";
        public const string SemanasNoCoinciden = "El número de semana seleccionado y del formato no coinciden.";
        public const string SemanaNoExisteFormato = "No existe el número de semana en el formato.";
        public const string SemanaFormatoIncorrecto = "El número de semana en el formato no es correcto.";
        public const string CodigoPuntoFormatoIncorrecto = "El código de la columna {0} no tiene formato correcto.";
        public const string NoExistenPuntosCargados = "No existen códigos para cargar.";
        public const string PuntoNoPerteneceEmpresa = "El punto {0} no pertenece a la empresa seleccionada.";
        public const string FechaCargaNoTieneFormato = "El formato de las fechas de los datos debe ser dd/MM/yyyy HH:mm. Fila : {0}";
        public const string FechaNoEstanEnOrden = "La secuencia de fechas de los datos no es correcta, revisar el formato. Fila : {0}";
        public const string DatoNoExiste = "No existe dato cargado fila: {0}, columna: {1}";
        public const string DatoFormatoIncorrecto = "Dato no tiene formato correcto fila: {0}, columna {1}";
        public const string DatoNegativo = "No se permiten datos negativos fila: {0}, columna {1}";
        public const string SuperaMaximo = "Dato supera el valor máximo ({0} MW), fila: {1}, columna {2}";
        public const string FaltaPuntosEnFormato = "La cantidad de columnas en el formato no es correcto. No debe eliminar columnas.";
        public const string FormatoCorrespondeDiario = "El formato cargado corresponde al programa diario.";
        public const string FormatoCorrespondeSemanal = "El formato cargado corresponde al programa semanal.";
    }
}