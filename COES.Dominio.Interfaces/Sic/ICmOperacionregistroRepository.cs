using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_OPERACIONREGISTRO
    /// </summary>
    public interface ICmOperacionregistroRepository
    {
        int Save(CmOperacionregistroDTO entity);
        void Update(CmOperacionregistroDTO entity);
        void Delete(int operegcodi);
        CmOperacionregistroDTO GetById(int operegcodi);
        List<CmOperacionregistroDTO> List();
        List<CmOperacionregistroDTO> GetByCriteria();
        int SaveCmOperacionregistroId(CmOperacionregistroDTO entity);
        List<CmOperacionregistroDTO> BuscarOperaciones(int grupocodi,int subcausaCodi,DateTime operegFecinicio,DateTime operegFecfin,int nroPage, int pageSize);
        int ObtenerNroFilas(int grupocodi,int subcausaCodi,DateTime operegFecinicio,DateTime operegFecfin);
    }
}
