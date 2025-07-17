// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 10/04/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase auxiliar que sostiene las incidencias encontradas en la información a importar.
    /// </summary>
    public class IioLogImportacionIncidenteDTO
    {

        /// <summary>
        /// Descripción del incidente en la importación.
        /// </summary>
        public string Mensaje { get; set; }

    }
}