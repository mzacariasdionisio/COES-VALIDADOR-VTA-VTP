using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_HISEMPEQ
    /// </summary>
    public interface ISiHisempeqRepository
    {
        int GetMaxId();
        int Save(SiHisempeqDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiHisempeqDTO entity);
        void DeleteLogico(SiHisempeqDTO entity);
        void UpdateAnularTransf(int migracodi, IDbConnection conn, DbTransaction tran);
        void Delete(int hempeqcodi);
        SiHisempeqDTO GetById(int hempeqcodi);
        List<SiHisempeqDTO> List();
        List<SiHisempeqDTO> GetByCriteria();

        List<SiHisempeqDTO> ListEquiposXMigracion(int migracodi);
        int ConsultarEquipMigracion(int migracodi, int equicodi, DateTime fechacorte);
    }
}
