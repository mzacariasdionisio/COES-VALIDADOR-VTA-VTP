using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_COMUNICADOS
    /// </summary>
    public interface IWbComunicadosRepository
    {
        int Save(WbComunicadosDTO entity);
        void Update(WbComunicadosDTO entity);
        void Delete(int comcodi);
        WbComunicadosDTO GetById(int comcodi);
        List<WbComunicadosDTO> List();
        List<WbComunicadosDTO> GetByCriteria();
    }
}
