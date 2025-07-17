using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCA_CUADRO_EJEC_USU_DET
    /// </summary>
    public interface IRcaCuadroEjecUsuarioDetRepository
    {
        int Save(RcaCuadroEjecUsuarioDetDTO entity);
        void Update(RcaCuadroEjecUsuarioDetDTO entity);
        void Delete(int rcejedcodi);
        RcaCuadroEjecUsuarioDetDTO GetById(int rcejedcodi);
        List<RcaCuadroEjecUsuarioDetDTO> List();
        List<RcaCuadroEjecUsuarioDetDTO> GetByCriteria();
        List<RcaCuadroEjecUsuarioDetDTO> ListFiltro(int codigoCuadroEjecucion);
        void DeletePorCliente(int rcejeucodi);
    }
}
