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
    /// <summary>
    /// Clase que sostiene las propiedades de los errores de la sincronización.
    /// </summary>
    public class ErrorSincroniza
    {
        private string codigoMensaje;

        public string CodigoMensaje
        {
            get { return codigoMensaje; }
            set { codigoMensaje = value; }
        }

        private string mensaje;

        public string Mensaje
        {
            get { return mensaje; }
            set { mensaje = value; }
        }

        public ErrorSincroniza(string codMensaje, string valMensaje)
        {
            this.codigoMensaje = codMensaje;
            this.mensaje = valMensaje;
        }
        
    }

}
