using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;
namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_TIPO_COMPORTAMIENTO
    /// </summary>
    public interface ISiTipoComportamientoRepository
    {
        int Save(SiTipoComportamientoDTO entity, IDbConnection conn, DbTransaction tran);
        int Save(SiTipoComportamientoDTO entity);
        void Update(SiTipoComportamientoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiTipoComportamientoDTO entity);
        void Delete(int tipocodi);
        SiTipoComportamientoDTO GetById(int tipocodi);
        List<SiTipoComportamientoDTO> List();

        List<SiTipoComportamientoDTO> GetByCriteria();
        List<SiTipoComportamientoDTO> ListByEmprcodi(int emprcodi);
    }
}
