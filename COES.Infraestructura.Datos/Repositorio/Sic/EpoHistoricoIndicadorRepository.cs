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
    /// Clase de acceso a datos de la tabla EPO_HISTORICO_INDICADOR
    /// </summary>
    public class EpoHistoricoIndicadorRepository: RepositoryBase, IEpoHistoricoIndicadorRepository
    {
        public EpoHistoricoIndicadorRepository(string strConn): base(strConn)
        {
        }

        EpoHistoricoIndicadorHelper helper = new EpoHistoricoIndicadorHelper();

        public int Save(EpoHistoricoIndicadorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Hincodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Indcodi, DbType.Int32, entity.Indcodi);
            dbProvider.AddInParameter(command, helper.Hinanio, DbType.Int32, entity.Hinanio);
            dbProvider.AddInParameter(command, helper.Hinmeta, DbType.Decimal, entity.Hinmeta);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EpoHistoricoIndicadorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Hincodi, DbType.Int32, entity.Hincodi);
            dbProvider.AddInParameter(command, helper.Indcodi, DbType.Int32, entity.Indcodi);
            dbProvider.AddInParameter(command, helper.Hinanio, DbType.Int32, entity.Hinanio);
            dbProvider.AddInParameter(command, helper.Hinmeta, DbType.Decimal, entity.Hinmeta);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hincodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hincodi, DbType.Int32, hincodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EpoHistoricoIndicadorDTO GetById(int hincodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hincodi, DbType.Int32, hincodi);
            EpoHistoricoIndicadorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EpoHistoricoIndicadorDTO> List()
        {
            List<EpoHistoricoIndicadorDTO> entitys = new List<EpoHistoricoIndicadorDTO>();
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

        public List<EpoHistoricoIndicadorDTO> GetByCriteria()
        {
            List<EpoHistoricoIndicadorDTO> entitys = new List<EpoHistoricoIndicadorDTO>();
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
