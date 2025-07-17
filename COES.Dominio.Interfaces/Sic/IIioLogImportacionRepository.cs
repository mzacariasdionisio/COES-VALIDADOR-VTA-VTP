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

using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_LOG_IMPORTACION
    /// </summary>
    public interface IIioLogImportacionRepository
    {
        List<IioLogImportacionDTO> List();

        int Save(IioLogImportacionDTO oIioLogImportacionDTO);

        IioLogImportacionDTO GetById(int uLogCodi);

        //- alpha.HDT - Inicio 10/04/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Permite identificar si existen duplicados en la configuración de equipos del COES.
        /// Este método se debe ejecutar antes realizar la importación de los datos del SICLI.
        /// </summary>
        /// <returns></returns>
        List<IioLogImportacionIncidenteDTO> GetDuplicadosConfiguracionCOES();

        //- alpha.HDT - 10/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la lista de incidentes en la tabla IIO_TMP_CONSUMO que no tienen puntos de medición.
        /// </summary>
        /// <returns></returns>
        List<IioLogImportacionIncidenteDTO> GetIncidentesSinPuntoMedicionCOES();

        //- alpha.HDT - 10/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener el correlativo disponible para el registro del log de la importación Sicli.
        /// </summary>
        /// <returns></returns>
        int GetCorrelativoDisponibleLogImportacionSicli();

        //- alpha.HDT - 10/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite eliminar las incidencias registradas en una importación previa.
        /// </summary>
        /// <param name="Rcimcodi"></param>
        /// <param name="periodo"></param>
        void EliminarIncidenciasImportacionSicli(int Rcimcodi, string periodo);

        //- alpha.HDT - 10/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener las inidencias de importación para un código de importación determinado.
        /// </summary>
        /// <param name="rcImCodi"></param>
        /// <returns></returns>
        List<IioLogImportacionDTO> GetIncidenciasImportacion(int rcImCodi);

        //- alpha.HDT - 12/04/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Permite crear un nuevo registro de lo de importación.
        /// </summary>
        /// <param name="oIioLogImportacionDTO"></param>
        void SaveIioLogImportacion(IioLogImportacionDTO oIioLogImportacionDTO);

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la lista de objetos DTO para la generación del reporte
        /// de datos importados de la tabla 04.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="empresasIn"></param>
        /// <returns></returns>
        List<IioTabla04DTO> GetDatosTabla04(string periodo, string empresasIn, string fechaDia);

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la lista de objetos DTO para la generación del reporte
        /// de datos importados de la tabla 05.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="empresasIn"></param>
        /// <returns></returns>
        List<IioTabla05DTO> GetDatosTabla05(string periodo, string empresasIn);

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener las incidencias de la importación relacionadas con suministros.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="empresasIn"></param>
        /// <returns></returns>
        List<IioLogImportacionDTO> GetIncidenciasImportacionSuministro(string periodo, string empresasIn);

        //- alpha.HDT - 22/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite eliminar el incidente de importación.
        /// </summary>
        /// <param name="ulogCodi"></param>
        void Delete(int ulogCodi);


        //Assetec - Demanda DPO - Iteracion 2
        List<IioTabla04DTO> ListMedidorDemandaSicli(string cargas, string inicio, string fin, int tipo);
        List<IioTabla04DTO> ListGroupByMonthYear(string anio, string mes, string cargas, string tipo);
        List<IioTabla04DTO> ListDatosSICLI(int anio, string mes, string cargas, string tipo);
        List<IioTabla04DTO> ListSicliByDateRange(string codigo, string inicio, string fin, string tipo);
    }
}