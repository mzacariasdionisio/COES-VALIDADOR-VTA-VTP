using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Asegúrate de cambiar el namespace a donde esté la entidad CpaTotalTransmisoresDTO.
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_TOTAL_TRANSMISORES
    /// </summary>
    public interface ICpaTotalTransmisoresRepository
    {
        int Save(CpaTotalTransmisoresDTO entity);
        void Delete(int cpaTtrCodi);
        CpaTotalTransmisoresDTO GetById(int cpaTtrCodi);
        List<CpaTotalTransmisoresDTO> List();
        int ObtenerNroRegistrosEnvios();
        int ObtenerNroRegistroEnviosFiltros(int cparcodi);
        List<CpaTotalTransmisoresDTO> ObtenerEnvios(int cparcodi);
        string ObtenerEstadoRevisionTransmisores(int cparcodi);
        int ObtenerNroRegistrosCPPEJTransmisores(int cparcodi);
        void DeleteCPPEJTransmisores(int cparcodi);
        string ObtenerTipoEmpresaCPAPorNombre(int cparcodi, string emprNom);
    }
}
