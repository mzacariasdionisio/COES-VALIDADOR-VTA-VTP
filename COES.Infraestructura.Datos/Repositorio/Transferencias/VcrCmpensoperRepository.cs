using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VCR_CMPENSOPER
    /// </summary>
    public class VcrCmpensoperRepository: RepositoryBase, IVcrCmpensoperRepository
    {
        public VcrCmpensoperRepository(string strConn): base(strConn)
        {
        }

        VcrCmpensoperHelper helper = new VcrCmpensoperHelper();

        public int Save(VcrCmpensoperDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcmpopcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcmpopfecha, DbType.DateTime, entity.Vcmpopfecha);
            dbProvider.AddInParameter(command, helper.Vcmpopporrsf, DbType.Decimal, entity.Vcmpopporrsf);
            dbProvider.AddInParameter(command, helper.Vcmpopbajaefic, DbType.Decimal, entity.Vcmpopbajaefic);
            dbProvider.AddInParameter(command, helper.Vcmpopusucreacion, DbType.String, entity.Vcmpopusucreacion);
            dbProvider.AddInParameter(command, helper.Vcmpopfeccreacion, DbType.DateTime, entity.Vcmpopfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrCmpensoperDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcmpopfecha, DbType.DateTime, entity.Vcmpopfecha);
            dbProvider.AddInParameter(command, helper.Vcmpopporrsf, DbType.Decimal, entity.Vcmpopporrsf);
            dbProvider.AddInParameter(command, helper.Vcmpopbajaefic, DbType.Decimal, entity.Vcmpopbajaefic);
            dbProvider.AddInParameter(command, helper.Vcmpopusucreacion, DbType.String, entity.Vcmpopusucreacion);
            dbProvider.AddInParameter(command, helper.Vcmpopfeccreacion, DbType.DateTime, entity.Vcmpopfeccreacion);
            dbProvider.AddInParameter(command, helper.Vcmpopcodi, DbType.Int32, entity.Vcmpopcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrCmpensoperDTO GetById(int vcmpopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcmpopcodi, DbType.Int32, vcmpopcodi);
            VcrCmpensoperDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrCmpensoperDTO> List(int vcrrecacodi)
        {
            List<VcrCmpensoperDTO> entitys = new List<VcrCmpensoperDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrrecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrCmpensoperDTO> GetByCriteria()
        {
            List<VcrCmpensoperDTO> entitys = new List<VcrCmpensoperDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}
