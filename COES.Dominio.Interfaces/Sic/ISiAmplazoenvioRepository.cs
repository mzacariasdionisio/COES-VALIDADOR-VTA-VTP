using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_AMPLAZOENVIO
    /// </summary>
    public interface ISiAmplazoenvioRepository
    {
        int Save(SiAmplazoenvioDTO entity);
        void Update(SiAmplazoenvioDTO entity);
        void Delete(int amplzcodi);
        SiAmplazoenvioDTO GetById(int amplzcodi);
        List<SiAmplazoenvioDTO> List();
        List<SiAmplazoenvioDTO> GetByCriteria();

        List<SiAmplazoenvioDTO> GetListaMultiple(DateTime fechaIni, DateTime fechaFin, string sEmpresa, string fdatcodi);
        SiAmplazoenvioDTO GetByIdCriteria(DateTime fecha, int empresa, int fdatcodi);
    }
}
