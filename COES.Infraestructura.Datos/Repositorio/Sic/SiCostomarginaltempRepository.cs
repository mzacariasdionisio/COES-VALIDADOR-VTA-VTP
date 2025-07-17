using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class SiCostomarginaltempRepository : RepositoryBase, ISiCostomarginaltempRepository
    {
        public SiCostomarginaltempRepository(string strConn)
            : base(strConn)
        {
        }

        SiCostomarginaltempHelper helper = new SiCostomarginaltempHelper();

        public void Save(SiCostomarginaltempDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cmgtenergia, DbType.Decimal, entity.Cmgtenergia);
            dbProvider.AddInParameter(command, helper.Cmgtcongestion, DbType.Decimal, entity.Cmgtcongestion);
            dbProvider.AddInParameter(command, helper.Cmgttotal, DbType.Decimal, entity.Cmgttotal);
            dbProvider.AddInParameter(command, helper.Cmgtcorrelativo, DbType.Int32, entity.Cmgtcorrelativo);
            dbProvider.AddInParameter(command, helper.Cmgtfecha, DbType.DateTime, entity.Cmgtfecha);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int enviocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, enviocodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveSiCostomarginaltempMasivo(List<SiCostomarginaltempDTO> listObj)
        {            
            dbProvider.AddColumnMapping(helper.Cmgtenergia, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cmgtcongestion, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cmgttotal, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Barrcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Enviocodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmgtcorrelativo, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmgtfecha, DbType.DateTime);
            dbProvider.BulkInsert<SiCostomarginaltempDTO>(listObj, helper.TableName);
        }

    }



}
