using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_FACTOR_PERDIDA_MEDIA
    /// </summary>
    public interface ITrnFactorPerdidaMediaRepository
    {
        int Save(TrnFactorPerdidaMediaDTO entity);
        void Update(TrnFactorPerdidaMediaDTO entity);
        void Delete(int pericodi, int version);
        TrnFactorPerdidaMediaDTO GetById(int trnfpmcodi);
        List<TrnFactorPerdidaMediaDTO> List(int pericodi, int version);
        List<TrnFactorPerdidaMediaDTO> GetByCriteria();
        //ASSETEC 20190104
        List<TrnFactorPerdidaMediaDTO> ListDesviacionesEntregas(int pericodi, int version);
    }
}
