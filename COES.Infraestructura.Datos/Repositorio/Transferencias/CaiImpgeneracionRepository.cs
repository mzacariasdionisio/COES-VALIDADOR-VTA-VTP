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
    /// Clase de acceso a datos de la tabla CAI_IMPGENERACION
    /// </summary>
    public class CaiImpgeneracionRepository: RepositoryBase, ICaiImpgeneracionRepository
    {
        public CaiImpgeneracionRepository(string strConn): base(strConn)
        {
        }

        CaiImpgeneracionHelper helper = new CaiImpgeneracionHelper();

        public int Save(CaiImpgeneracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Caimpgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Caimpgfuentedat, DbType.String, entity.Caimpgfuentedat);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Caimpgmes, DbType.Int32, entity.Caimpgmes);
            dbProvider.AddInParameter(command, helper.Caimpgtotenergia, DbType.Decimal, entity.Caimpgtotenergia);
            dbProvider.AddInParameter(command, helper.Caimpgimpenergia, DbType.Decimal, entity.Caimpgimpenergia);
            dbProvider.AddInParameter(command, helper.Caimpgtotpotencia, DbType.Decimal, entity.Caimpgtotpotencia);
            dbProvider.AddInParameter(command, helper.Caimpgimppotencia, DbType.Decimal, entity.Caimpgimppotencia);
            dbProvider.AddInParameter(command, helper.Caimpgusucreacion, DbType.String, entity.Caimpgusucreacion);
            dbProvider.AddInParameter(command, helper.Caimpgfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiImpgeneracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Caimpgfuentedat, DbType.String, entity.Caimpgfuentedat);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Caimpgmes, DbType.Int32, entity.Caimpgmes);
            dbProvider.AddInParameter(command, helper.Caimpgtotenergia, DbType.Decimal, entity.Caimpgtotenergia);
            dbProvider.AddInParameter(command, helper.Caimpgimpenergia, DbType.Decimal, entity.Caimpgimpenergia);
            dbProvider.AddInParameter(command, helper.Caimpgtotpotencia, DbType.Decimal, entity.Caimpgtotpotencia);
            dbProvider.AddInParameter(command, helper.Caimpgimppotencia, DbType.Decimal, entity.Caimpgimppotencia);
            dbProvider.AddInParameter(command, helper.Caimpgusucreacion, DbType.String, entity.Caimpgusucreacion);
            dbProvider.AddInParameter(command, helper.Caimpgfeccreacion, DbType.DateTime, entity.Caimpgfeccreacion);
            dbProvider.AddInParameter(command, helper.Caimpgcodi, DbType.Int32, entity.Caimpgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int caiajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Caimpgcodi, DbType.Int32, caiajcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiImpgeneracionDTO GetById(int caimpgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Caimpgcodi, DbType.Int32, caimpgcodi);
            CaiImpgeneracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiImpgeneracionDTO> List()
        {
            List<CaiImpgeneracionDTO> entitys = new List<CaiImpgeneracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CaiImpgeneracionDTO> GetByCriteria()
        {
            List<CaiImpgeneracionDTO> entitys = new List<CaiImpgeneracionDTO>();
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
