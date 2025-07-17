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

using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{

    /// <summary>
    /// Interface que especifica la firma de métodos que se requiere para la funcionalidad de las asignaciones
    /// pendientes en la sicronización.
    /// </summary>
    public interface IIioAsignacionPendienteRepository
    {
        
        /// <summary>
        /// Crea un nuevo registro.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Save(IioAsignacionPendienteDTO entity);

        /// <summary>
        /// Actualiza el registro existente.
        /// </summary>
        /// <param name="entity"></param>
        void Update(IioAsignacionPendienteDTO entity);

        /// <summary>
        /// Elimina el registro.
        /// </summary>
        /// <param name="mapencodi"></param>
        void Delete(int mapencodi);

        /// <summary>
        /// Obtiene la lista de asignaciones por fecha de creación. Se debe precisar
        /// que la fecha de creación representa la fecha-hora de inicio del trabajo de sincronización.
        /// </summary>
        /// <returns></returns>
        List<IioAsignacionPendienteDTO> ListByCreationDate(string fechaHoraInicioSincronizacion);

    }
}
