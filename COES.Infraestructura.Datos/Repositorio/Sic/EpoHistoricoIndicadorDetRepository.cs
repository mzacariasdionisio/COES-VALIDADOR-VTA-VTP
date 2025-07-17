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
    /// <summary>
    /// Clase de acceso a datos de la tabla EPO_HISTORICO_INDICADOR_DET
    /// </summary>
    public class EpoHistoricoIndicadorDetRepository: RepositoryBase, IEpoHistoricoIndicadorDetRepository
    {
        public EpoHistoricoIndicadorDetRepository(string strConn): base(strConn)
        {
        }

        EpoHistoricoIndicadorDetHelper helper = new EpoHistoricoIndicadorDetHelper();

        public void Save(EpoHistoricoIndicadorDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Hincodi, DbType.Int32, entity.Hincodi);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, entity.Percodi);
            dbProvider.AddInParameter(command, helper.Hidvalor, DbType.Decimal, entity.Hidvalor);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EpoHistoricoIndicadorDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Hincodi, DbType.Int32, entity.Hincodi);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, entity.Percodi);
            dbProvider.AddInParameter(command, helper.Hidvalor, DbType.Decimal, entity.Hidvalor);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hincodi, int percodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hincodi, DbType.Int32, hincodi);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, percodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EpoHistoricoIndicadorDetDTO GetById(int hincodi, int percodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hincodi, DbType.Int32, hincodi);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, percodi);
            EpoHistoricoIndicadorDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EpoHistoricoIndicadorDetDTO> List()
        {
            List<EpoHistoricoIndicadorDetDTO> entitys = new List<EpoHistoricoIndicadorDetDTO>();
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

        public List<EpoHistoricoIndicadorDetDTO> GetByCriteria()
        {
            List<EpoHistoricoIndicadorDetDTO> entitys = new List<EpoHistoricoIndicadorDetDTO>();
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
