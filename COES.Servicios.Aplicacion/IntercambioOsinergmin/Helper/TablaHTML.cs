// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 26/10/2016
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper
{
   
    public static class TablaHTML
    {
        public class Tabla : IDisposable
        {
            private StringBuilder cadena;
            public Tabla(StringBuilder sb)
            {
                cadena = sb;
                cadena.Append("<table>");
            }
            public void Dispose()
            {
                cadena.Append("</table>");
            }
            public Fila AddRow()
            {
                return new Fila(cadena);
            }
        }
        public class Fila : IDisposable
        {
            private StringBuilder cadena;

            public Fila(StringBuilder sb)
            {
                cadena = sb;
                cadena.Append("<tr>");
            }

            public void Dispose()
            {
                cadena.Append("</tr>");
            }

            public void AgregarCeldaCabecera(string valor)
            {
                cadena.Append("<td><b>");
                cadena.Append(valor);
                cadena.Append("</b></td>");
            }

            public void AgregarCelda(string valor)
            {
                cadena.Append("<td>");
                cadena.Append(valor);
                cadena.Append("</td>");
            }

        }
    }

}
