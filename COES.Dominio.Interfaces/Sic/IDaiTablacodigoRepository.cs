using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla DAI_TABLACODIGO
    /// </summary>
    public interface IDaiTablacodigoRepository
    {
        int Save(DaiTablacodigoDTO entity);
        void Update(DaiTablacodigoDTO entity);
        void Delete(int tabcodi);
        DaiTablacodigoDTO GetById(int tabcodi);
        List<DaiTablacodigoDTO> List();
        List<DaiTablacodigoDTO> GetByCriteria();
    }
}
