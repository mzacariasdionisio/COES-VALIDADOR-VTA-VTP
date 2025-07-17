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
    /// Clase de acceso a datos de la tabla ME_LOGENVIO
    /// </summary>
    public class MeLogenvioRepository: RepositoryBase, IMeLogenvioRepository
    {
        public MeLogenvioRepository(string strConn): base(strConn)
        {
        }

        MeLogenvioHelper helper = new MeLogenvioHelper();


        public void Save(MeLogenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Logenvsec, DbType.Int32, entity.Logenvsec);
            dbProvider.AddInParameter(command, helper.Logenvfecha, DbType.DateTime, entity.Logenvfecha);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Mencodi, DbType.Int32, entity.Mencodi);
            dbProvider.AddInParameter(command, helper.Logenvmencant, DbType.Int32, entity.Logenvmencant);
            dbProvider.AddInParameter(command, helper.Logenvdescrip, DbType.String, entity.Logenvdescrip);

            dbProvider.ExecuteNonQuery(command);
        }
        public void Update(MeLogenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Logenvsec, DbType.Int32, entity.Logenvsec);
            dbProvider.AddInParameter(command, helper.Logenvfecha, DbType.DateTime, entity.Logenvfecha);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Mencodi, DbType.Int32, entity.Mencodi);
            dbProvider.AddInParameter(command, helper.Logenvmencant, DbType.Int32, entity.Logenvmencant);
            dbProvider.AddInParameter(command, helper.Logenvdescrip, DbType.String, entity.Logenvdescrip);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public MeLogenvioDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            MeLogenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeLogenvioDTO> List()
        {
            List<MeLogenvioDTO> entitys = new List<MeLogenvioDTO>();
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

        public List<MeLogenvioDTO> GetByCriteria()
        {
            List<MeLogenvioDTO> entitys = new List<MeLogenvioDTO>();
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
