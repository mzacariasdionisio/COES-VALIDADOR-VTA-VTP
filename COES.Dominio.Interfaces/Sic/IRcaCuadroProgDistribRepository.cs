using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCA_CUADRO_PROG_DISTRIB
    /// </summary>
    public interface IRcaCuadroProgDistribRepository
    {
        int Save(RcaCuadroProgDistribDTO entity);
        void Update(RcaCuadroProgDistribDTO entity);
        void Delete(int rcprodcodi);
        RcaCuadroProgDistribDTO GetById(int rcprodcodi);
        List<RcaCuadroProgDistribDTO> List();
        List<RcaCuadroProgDistribDTO> GetByCriteria();
        List<RcaCuadroProgDistribDTO> ListCuadroProgDistrib(int codigoCuadroPrograma);



    }
}
