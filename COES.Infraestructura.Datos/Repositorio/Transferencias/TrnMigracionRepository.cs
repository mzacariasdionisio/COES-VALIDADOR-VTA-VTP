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
    /// Clase de acceso a datos de la tabla TRN_MIGRACION
    /// </summary>
    public class TrnMigracionRepository: RepositoryBase, ITrnMigracionRepository
    {
        public TrnMigracionRepository(string strConn): base(strConn)
        {
        }

        TrnMigracionHelper helper = new TrnMigracionHelper();

        public int Save(TrnMigracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Trnmigcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Emprcodiorigen, DbType.Int32, entity.Emprcodiorigen);
            dbProvider.AddInParameter(command, helper.Emprcodidestino, DbType.Int32, entity.Emprcodidestino);
            dbProvider.AddInParameter(command, helper.Trnmigdescripcion, DbType.String, entity.Trnmigdescripcion);
            dbProvider.AddInParameter(command, helper.Trnmigsql, DbType.String, entity.Trnmigsql);
            dbProvider.AddInParameter(command, helper.Trnmigestado, DbType.String, entity.Trnmigestado);
            dbProvider.AddInParameter(command, helper.Trnmigusucreacion, DbType.String, entity.Trnmigusucreacion);
            dbProvider.AddInParameter(command, helper.Trnmigfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Trnmigusumodificacion, DbType.String, entity.Trnmigusucreacion);
            dbProvider.AddInParameter(command, helper.Trnmigfecmodificacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrnMigracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Emprcodiorigen, DbType.Int32, entity.Emprcodiorigen);
            dbProvider.AddInParameter(command, helper.Emprcodidestino, DbType.Int32, entity.Emprcodidestino);
            dbProvider.AddInParameter(command, helper.Trnmigdescripcion, DbType.String, entity.Trnmigdescripcion);
            dbProvider.AddInParameter(command, helper.Trnmigsql, DbType.String, entity.Trnmigsql);
            dbProvider.AddInParameter(command, helper.Trnmigestado, DbType.String, entity.Trnmigestado);
            dbProvider.AddInParameter(command, helper.Trnmigusumodificacion, DbType.String, entity.Trnmigusumodificacion);
            dbProvider.AddInParameter(command, helper.Trnmigfecmodificacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Trnmigcodi, DbType.Int32, entity.Trnmigcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int trnmigcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Trnmigcodi, DbType.Int32, trnmigcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrnMigracionDTO GetById(int trnmigcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Trnmigcodi, DbType.Int32, trnmigcodi);
            TrnMigracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrnMigracionDTO> List()
        {
            List<TrnMigracionDTO> entitys = new List<TrnMigracionDTO>();
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
        public List<TrnMigracionDTO> ListMigracion()
        {
            List<TrnMigracionDTO> entitys = new List<TrnMigracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMigracion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<TrnMigracionDTO> GetByCriteria()
        {
            List<TrnMigracionDTO> entitys = new List<TrnMigracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnMigracionDTO entity = new TrnMigracionDTO();
                    int iMigracodi = dr.GetOrdinal(this.helper.Migracodi);
                    if (!dr.IsDBNull(iMigracodi)) entity.Migracodi = Convert.ToInt32(dr.GetValue(iMigracodi));

                    int iEmprcodiorigen = dr.GetOrdinal(this.helper.Emprcodiorigen);
                    if (!dr.IsDBNull(iEmprcodiorigen)) entity.Emprcodiorigen = Convert.ToInt32(dr.GetValue(iEmprcodiorigen));

                    int iEmprcodidestino = dr.GetOrdinal(this.helper.Emprcodidestino);
                    if (!dr.IsDBNull(iEmprcodidestino)) entity.Emprcodidestino = Convert.ToInt32(dr.GetValue(iEmprcodidestino));

                    int iTrnmigsql = dr.GetOrdinal(this.helper.Trnmigsql);
                    if (!dr.IsDBNull(iTrnmigsql)) entity.Trnmigsql = dr.GetString(iTrnmigsql);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
