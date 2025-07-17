// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 13/07/2017
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

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase utilitaria que permite sostener y transportar - del servidor al cliente - los datos
    /// de la lista de incidencias de suministros y la lista de áreas respectivas para la generación de registros.
    /// </summary>
    public class IncidenciaSuministroDTO
    {
        /// <summary>
        /// Listado de objetos DTO de las áreas.
        /// </summary>
         public List<EqAreaDTO> ListaEqAreaDTO { get; set; }

        /// <summary>
        /// Listado de objetivos DTO de las incidencias de importación.
        /// </summary>
         public List<IioLogImportacionDTO> ListaIioLogImportacionDTO { get; set; }
    }
}
