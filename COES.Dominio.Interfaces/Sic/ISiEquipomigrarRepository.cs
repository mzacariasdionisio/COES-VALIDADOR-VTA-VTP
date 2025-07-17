using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_EQUIPOMIGRAR
    /// </summary>
    public interface ISiEquipoMigrarRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        //int Save(SiEquipomigrarDTO entity, IDbConnection conn, DbTransaction tran);
        int Save(SiEquipomigrarDTO entity, IDbConnection conn, DbTransaction tran);


        void Update(SiEquipomigrarDTO entity);
        void Delete(int equmigcodi);
        SiEquipomigrarDTO GetById(int equmigcodi);
        List<SiEquipomigrarDTO> List();
        List<SiEquipomigrarDTO> GetByCriteria();
        int GetMaxId();
    }
}
