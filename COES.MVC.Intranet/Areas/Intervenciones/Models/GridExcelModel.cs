using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Intervenciones.Models
{
    public class GridExcelModel
    { 
        /// <summary>
        /// Model para el manejo de grillas tipo excel
        /// </summary>

        public string[] Headers { get; set; }       //colHeaders : Setea verdadero o falso para activar o desactivar los encabezados de las columnas por defecto ( A, B , C ). También puede definir una matriz [' One' , ' Dos ', ' Tres ' , ...] o una función para definir las cabeceras. Si una función se establece el índice de la columna se pasa como parámetro.
        public int[] Widths { get; set; }           //colWidths : Define el ancho de la columna en píxeles. Acepta número, cadena ( que se convertirá en número) , matriz de números (si desea definir el ancho de columna por separado para cada columna ) o una función (si desea ajustar el ancho de columna dinámicamente en cada render )
        public object[] Columnas { get; set; }      //columns : Define las propiedades de las celdas y y los datos para ciertas columnas . Aviso: El uso de esta opción establece un número fijo de columnas ( Opciones startCols , minCols , maxCols serán ignoradas ) .
        public string[][] Data { get; set; }        //data : Fuente de datos inicial que se une a la red de datos por cuadrícula de referencia (datos de edición altera la fuente de datos . Ver Entendimiento vinculante como referencia

        public object[] ListaColumnasColor { get; set; }
        public object[] ListaCeldasColor { get; set; }

        public int FixedColumnsLeft { get; set; }   //Permite especificar el número de columnas fijas ( congelado ) en el lado izquierdo de la tabla
        public int FixedRowsTop { get; set; }       //Permite especificar el número de filas fijos ( congelado ) en la parte superior de la tabla

        public const string TipoTexto = "text";
        public const string TipoNumerico = "numeric";
        public const string TipoFecha = "date";
        public const string TipoLista = "dropdown";
        public const string TipoAutocompletar = "autocomplete";

        //Lista de Objetos complementarios
        public object[] ListaEmpresas { get; set; }

        //Otros objetos
        public string sMensaje { get; set; }
        public int CantidadEmpresas { get; set; }
        public bool Grabar { get; set; }
        public int RegError { get; set; }
        public string MensajeError { get; set; }

        public object[] ListaColumnasVencidas { get; set; }

        //manejo de errores
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Reporte { get; set; }
    }
}