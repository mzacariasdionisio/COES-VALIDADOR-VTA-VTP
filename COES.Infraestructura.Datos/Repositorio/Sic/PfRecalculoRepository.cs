using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PF_RECALCULO
    /// </summary>
    public class PfRecalculoRepository : RepositoryBase, IPfRecalculoRepository
    {
        public PfRecalculoRepository(string strConn) : base(strConn)
        {
        }

        PfRecalculoHelper helper = new PfRecalculoHelper();

        public int Save(PfRecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            int? irecacodi = entity.Irecacodi > 0 ? (int?)entity.Irecacodi : null;

            dbProvider.AddInParameter(command, helper.Pfrecacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfrecanombre, DbType.String, entity.Pfrecanombre);
            dbProvider.AddInParameter(command, helper.Pfrecadescripcion, DbType.String, entity.Pfrecadescripcion);
            dbProvider.AddInParameter(command, helper.Pfrecausucreacion, DbType.String, entity.Pfrecausucreacion);
            dbProvider.AddInParameter(command, helper.Pfrecafeccreacion, DbType.DateTime, entity.Pfrecafeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrecausumodificacion, DbType.String, entity.Pfrecausumodificacion);
            dbProvider.AddInParameter(command, helper.Pfrecafecmodificacion, DbType.DateTime, entity.Pfrecafecmodificacion);
            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, entity.Pfpericodi);
            dbProvider.AddInParameter(command, helper.Pfrecainforme, DbType.String, entity.Pfrecainforme);
            dbProvider.AddInParameter(command, helper.Pfrecatipo, DbType.String, entity.Pfrecatipo);
            dbProvider.AddInParameter(command, helper.Pfrecafechalimite, DbType.DateTime, entity.Pfrecafechalimite);
            dbProvider.AddInParameter(command, helper.Irecacodi, DbType.Int32, irecacodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfRecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrecanombre, DbType.String, entity.Pfrecanombre);
            dbProvider.AddInParameter(command, helper.Pfrecadescripcion, DbType.String, entity.Pfrecadescripcion);
            dbProvider.AddInParameter(command, helper.Pfrecausucreacion, DbType.String, entity.Pfrecausucreacion);
            dbProvider.AddInParameter(command, helper.Pfrecafeccreacion, DbType.DateTime, entity.Pfrecafeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrecausumodificacion, DbType.String, entity.Pfrecausumodificacion);
            dbProvider.AddInParameter(command, helper.Pfrecafecmodificacion, DbType.DateTime, entity.Pfrecafecmodificacion);
            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, entity.Pfpericodi);
            dbProvider.AddInParameter(command, helper.Pfrecainforme, DbType.String, entity.Pfrecainforme);
            dbProvider.AddInParameter(command, helper.Pfrecatipo, DbType.String, entity.Pfrecatipo);
            dbProvider.AddInParameter(command, helper.Pfrecafechalimite, DbType.DateTime, entity.Pfrecafechalimite);
            dbProvider.AddInParameter(command, helper.Irecacodi, DbType.Int32, entity.Irecacodi);

            dbProvider.AddInParameter(command, helper.Pfrecacodi, DbType.Int32, entity.Pfrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrecacodi, DbType.Int32, pfrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfRecalculoDTO GetById(int pfrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrecacodi, DbType.Int32, pfrecacodi);
            PfRecalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPfrperanio = dr.GetOrdinal(helper.Pfperianio);
                    if (!dr.IsDBNull(iPfrperanio)) entity.Pfperianio = Convert.ToInt32(dr.GetValue(iPfrperanio));

                    int iPfrpermes = dr.GetOrdinal(helper.Pfperimes);
                    if (!dr.IsDBNull(iPfrpermes)) entity.Pfperimes = Convert.ToInt32(dr.GetValue(iPfrpermes));
                }
            }

            return entity;
        }

        public List<PfRecalculoDTO> List()
        {
            List<PfRecalculoDTO> entitys = new List<PfRecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPfrperanio = dr.GetOrdinal(helper.Pfperianio);
                    if (!dr.IsDBNull(iPfrperanio)) entity.Pfperianio = Convert.ToInt32(dr.GetValue(iPfrperanio));

                    int iPfrpermes = dr.GetOrdinal(helper.Pfperimes);
                    if (!dr.IsDBNull(iPfrpermes)) entity.Pfperimes = Convert.ToInt32(dr.GetValue(iPfrpermes));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PfRecalculoDTO> GetByCriteria(int pfpericodi, int anio, int mes)
        {
            List<PfRecalculoDTO> entitys = new List<PfRecalculoDTO>();

            string query = string.Format(helper.SqlGetByCriteria, pfpericodi, anio, mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPfrperanio = dr.GetOrdinal(helper.Pfperianio);
                    if (!dr.IsDBNull(iPfrperanio)) entity.Pfperianio = Convert.ToInt32(dr.GetValue(iPfrperanio));

                    int iPfrpermes = dr.GetOrdinal(helper.Pfperimes);
                    if (!dr.IsDBNull(iPfrpermes)) entity.Pfperimes = Convert.ToInt32(dr.GetValue(iPfrpermes));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
