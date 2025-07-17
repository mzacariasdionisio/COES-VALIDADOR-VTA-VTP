using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_MODELO_ENVIO
    /// </summary>
    public interface ITrnModeloEnvioRepository
    {
        int Save(TrnModeloEnvioDTO entity);
        void Update(TrnModeloEnvioDTO entity);
        void Delete(int modenvcodi);
        TrnModeloEnvioDTO GetById(int modenvcodi);
        List<TrnModeloEnvioDTO> List();
        List<TrnModeloEnvioDTO> GetByCriteria(int empresa, int periodo, int version);
    }
}
