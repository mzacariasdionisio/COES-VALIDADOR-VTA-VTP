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
    /// Clase de acceso a datos de la tabla TR_OBSERVACION_ESTADO
    /// </summary>
    public class TrObservacionEstadoRepository: RepositoryBase, ITrObservacionEstadoRepository
    {
        public TrObservacionEstadoRepository(string strConn): base(strConn)
        {
        }

        TrObservacionEstadoHelper helper = new TrObservacionEstadoHelper();

        public int Save(TrObservacionEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Obsestcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Obscancodi, DbType.Int32, entity.Obscancodi);
            dbProvider.AddInParameter(command, helper.Obsestestado, DbType.String, entity.Obsestestado);
            dbProvider.AddInParameter(command, helper.Obsestcomentario, DbType.String, entity.Obsestcomentario);
            dbProvider.AddInParameter(command, helper.Obsestusuario, DbType.String, entity.Obsestusuario);
            dbProvider.AddInParameter(command, helper.Obsestfecha, DbType.DateTime, entity.Obsestfecha);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrObservacionEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Obscancodi, DbType.Int32, entity.Obscancodi);
            dbProvider.AddInParameter(command, helper.Obsestestado, DbType.String, entity.Obsestestado);
            dbProvider.AddInParameter(command, helper.Obsestcomentario, DbType.String, entity.Obsestcomentario);
            dbProvider.AddInParameter(command, helper.Obsestusuario, DbType.String, entity.Obsestusuario);
            dbProvider.AddInParameter(command, helper.Obsestfecha, DbType.DateTime, entity.Obsestfecha);
            dbProvider.AddInParameter(command, helper.Obsestcodi, DbType.Int32, entity.Obsestcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int obsestcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Obsestcodi, DbType.Int32, obsestcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrObservacionEstadoDTO GetById(int obsestcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Obsestcodi, DbType.Int32, obsestcodi);
            TrObservacionEstadoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrObservacionEstadoDTO> GetByCriteria(int obscodi)
        {
            List<TrObservacionEstadoDTO> entitys = new List<TrObservacionEstadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Obscancodi, DbType.Int32, obscodi);

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
