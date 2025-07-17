using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Asegúrate de cambiar el namespace a donde esté la entidad CpaTotalDemandaDetDTO.
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_TOTAL_DEMANDADET
    /// </summary>
    public interface ICpaTotalDemandaDetRepository
    {
        int Save(CpaTotalDemandaDetDTO entity);
        void Delete(int cpaTddCodi);
        CpaTotalDemandaDetDTO GetById(int cpaTddCodi);
        List<CpaTotalDemandaDetDTO> List();

        List<CpaTotalDemandaDetDTO> GetByIdDemanda(int cpaTdCodi);
        List<CpaTotalDemandaDetDTO> GetLastEnvio(int IdRevision, string Tipo, int Mes);
        List<CpaTotalDemandaDetDTO> EnvioVacio(int revision, string tipo, int mes);

        /* CU17: INICIO */
        List<CpaTotalDemandaDetDTO> ListLastByRevision(int cparcodi);
        List<CpaTotalDemandaDetDTO> ListByCpatdcodi(int cpatdcodi);
        /* CU17: FIN */
    }
}
