using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_EQUIVALENCIAMODOP
    /// </summary>
    public interface ICmEquivalenciamodopRepository
    {
        int Save(CmEquivalenciamodopDTO entity);
        void Update(CmEquivalenciamodopDTO entity);
        void Delete(int equimocodi);
        CmEquivalenciamodopDTO GetById(int equimocodi);
        List<CmEquivalenciamodopDTO> List();
        List<CmEquivalenciamodopDTO> GetByCriteria();
        int SaveCmEquivalenciamodopId(CmEquivalenciamodopDTO entity);
        List<CmEquivalenciamodopDTO> BuscarOperaciones(int grupocodi,int nroPage, int pageSize);
        int ObtenerNroFilas(int grupocodi);
    }
}
