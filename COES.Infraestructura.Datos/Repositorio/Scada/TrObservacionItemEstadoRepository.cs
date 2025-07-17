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
    /// Clase de acceso a datos de la tabla TR_OBSERVACION_ITEM_ESTADO
    /// </summary>
    public class TrObservacionItemEstadoRepository: RepositoryBase, ITrObservacionItemEstadoRepository
    {
        public TrObservacionItemEstadoRepository(string strConn): base(strConn)
        {
        }

        TrObservacionItemEstadoHelper helper = new TrObservacionItemEstadoHelper();


        public int Save(TrObservacionItemEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Obitescodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Obsitecodi, DbType.Int32, entity.Obsitecodi);
            dbProvider.AddInParameter(command, helper.Obitesestado, DbType.String, entity.Obitesestado);
            dbProvider.AddInParameter(command, helper.Obitescomentario, DbType.String, entity.Obitescomentario);
            dbProvider.AddInParameter(command, helper.Obitesusuario, DbType.String, entity.Obitesusuario);
            dbProvider.AddInParameter(command, helper.Obitesfecha, DbType.DateTime, entity.Obitesfecha);            

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrObservacionItemEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Obsitecodi, DbType.Int32, entity.Obsitecodi);
            dbProvider.AddInParameter(command, helper.Obitesestado, DbType.String, entity.Obitesestado);
            dbProvider.AddInParameter(command, helper.Obitescomentario, DbType.String, entity.Obitescomentario);
            dbProvider.AddInParameter(command, helper.Obitesusuario, DbType.String, entity.Obitesusuario);
            dbProvider.AddInParameter(command, helper.Obitesfecha, DbType.DateTime, entity.Obitesfecha);
            dbProvider.AddInParameter(command, helper.Obitescodi, DbType.Int32, entity.Obitescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int obitescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Obitescodi, DbType.Int32, obitescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrObservacionItemEstadoDTO GetById(int obitescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Obitescodi, DbType.Int32, obitescodi);
            TrObservacionItemEstadoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrObservacionItemEstadoDTO> GetByCriteria(int obsitemcodi)
        {
            List<TrObservacionItemEstadoDTO> entitys = new List<TrObservacionItemEstadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Obsitecodi, DbType.Int32, obsitemcodi);

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
