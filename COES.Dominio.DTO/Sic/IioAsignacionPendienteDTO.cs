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

using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{

    /// <summary>
    /// Clase que mapea la tabla IIO_ASIGNACION_PENDIENTE
    /// </summary>
    public class IioAsignacionPendienteDTO : EntityBase
    {

        public int Mapencodi { get; set; }

        public string Mapenentidad { get; set; }

        public string Mapencodigo { get; set; }

        public string Mapendescripcion { get; set; }
        
        public int Mapenestado { get; set; }

        public string Mapenindicacionestado { get; set; }

        public string Mapenestregistro { get; set; }

        public string Mapenusucreacion { get; set; }

        public DateTime Mapenfeccreacion { get; set; }

        public string Mapenusumodificacion { get; set; }

        public DateTime? Mapenfecmodificacion { get; set; }
        
    }

}
