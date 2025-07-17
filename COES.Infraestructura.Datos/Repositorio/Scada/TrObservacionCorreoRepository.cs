using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_OBSERVACION_CORREO
    /// </summary>
    public class TrObservacionCorreoRepository: RepositoryBase, ITrObservacionCorreoRepository
    {
        public TrObservacionCorreoRepository(string strConn): base(strConn)
        {
        }

        TrObservacionCorreoHelper helper = new TrObservacionCorreoHelper();

        public int Save(TrObservacionCorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Obscorcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Obscoremail, DbType.String, entity.Obscoremail);
            dbProvider.AddInParameter(command, helper.Obscorestado, DbType.String, entity.Obscorestado);
            dbProvider.AddInParameter(command, helper.Obscornombre, DbType.String, entity.Obscornombre);
            dbProvider.AddInParameter(command, helper.Obscorusumodificacion, DbType.String, entity.Obscorusumodificacion);
            dbProvider.AddInParameter(command, helper.Obscorfecmodificacion, DbType.DateTime, entity.Obscorfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrObservacionCorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Obscoremail, DbType.String, entity.Obscoremail);
            dbProvider.AddInParameter(command, helper.Obscorestado, DbType.String, entity.Obscorestado);
            dbProvider.AddInParameter(command, helper.Obscornombre, DbType.String, entity.Obscornombre);
            dbProvider.AddInParameter(command, helper.Obscorusumodificacion, DbType.String, entity.Obscorusumodificacion);
            dbProvider.AddInParameter(command, helper.Obscorfecmodificacion, DbType.DateTime, entity.Obscorfecmodificacion);
            dbProvider.AddInParameter(command, helper.Obscorcodi, DbType.Int32, entity.Obscorcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int obscorcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Obscorcodi, DbType.Int32, obscorcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrObservacionCorreoDTO GetById(int obscorcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Obscorcodi, DbType.Int32, obscorcodi);
            TrObservacionCorreoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrObservacionCorreoDTO> List()
        {
            List<TrObservacionCorreoDTO> entitys = new List<TrObservacionCorreoDTO>();
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

        public List<TrObservacionCorreoDTO> GetByCriteria(int idEmpresa)
        {
            List<TrObservacionCorreoDTO> entitys = new List<TrObservacionCorreoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);           

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrObservacionCorreoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
