using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Asegúrate de cambiar el namespace a donde esté la entidad CpaTotalDemandaDTO.
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_TOTAL_DEMANDA
    /// </summary>
    public interface ICpaTotalDemandaRepository
    {
        int Save(CpaTotalDemandaDTO entity);
        void Delete(int cpaTdeCodi);
        CpaTotalDemandaDTO GetById(int cpaTdeCodi);
        List<CpaTotalDemandaDTO> List();
        int ObtenerNroRegistrosEnvios();
        int ObtenerNroRegistroEnviosFiltros(int cparcodi, string cpaemptipo, int cpatdmes);
        List<CpaTotalDemandaDTO> ObtenerEnvios(int cparcodi, string cpaemptipo, int cpatdmes);
        string ObtenerEstadoRevisionDemanda(int cparcodi);
        int ObtenerNroRegistrosCPPEJDemanda(int cparcodi);
        void DeleteCPPEJDemanda(int cparcodi);
        string ObtenerTipoEmpresaCPAPorNombre(int cparcodi, string cpaemptipo, string emprNom);
    }
}
