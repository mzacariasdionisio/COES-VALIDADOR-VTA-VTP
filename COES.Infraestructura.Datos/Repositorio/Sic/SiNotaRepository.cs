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
    /// Clase de acceso a datos de la tabla SI_NOTA
    /// </summary>
    public class SiNotaRepository : RepositoryBase, ISiNotaRepository
    {
        public SiNotaRepository(string strConn)
            : base(strConn)
        {
        }

        SiNotaHelper helper = new SiNotaHelper();

        public int Save(SiNotaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Sinotacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Sinotadesc, DbType.String, entity.Sinotadesc);
            dbProvider.AddInParameter(command, helper.Sinotausucreacion, DbType.String, entity.Sinotausucreacion);
            dbProvider.AddInParameter(command, helper.Sinotafeccreacion, DbType.DateTime, entity.Sinotafeccreacion);
            dbProvider.AddInParameter(command, helper.Sinotausumodificacion, DbType.String, entity.Sinotausumodificacion);
            dbProvider.AddInParameter(command, helper.Sinotafecmodificacion, DbType.DateTime, entity.Sinotafecmodificacion);
            dbProvider.AddInParameter(command, helper.Sinotaestado, DbType.Int32, entity.Sinotaestado);
            dbProvider.AddInParameter(command, helper.Sinotaperiodo, DbType.DateTime, entity.Sinotaperiodo);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);
            dbProvider.AddInParameter(command, helper.Sinotaorden, DbType.Int32, entity.Sinotaorden);
            dbProvider.AddInParameter(command, helper.Sinotatipo, DbType.Int32, entity.Sinotatipo);
            dbProvider.AddInParameter(command, helper.Verscodi, DbType.Int32, entity.Verscodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiNotaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Sinotadesc, DbType.String, entity.Sinotadesc);
            //dbProvider.AddInParameter(command, helper.Sinotausucreacion, DbType.String, entity.Sinotausucreacion);
            //dbProvider.AddInParameter(command, helper.Sinotafeccreacion, DbType.DateTime, entity.Sinotafeccreacion);
            dbProvider.AddInParameter(command, helper.Sinotausumodificacion, DbType.String, entity.Sinotausumodificacion);
            dbProvider.AddInParameter(command, helper.Sinotafecmodificacion, DbType.DateTime, entity.Sinotafecmodificacion);
            dbProvider.AddInParameter(command, helper.Sinotaestado, DbType.Int32, entity.Sinotaestado);
            //dbProvider.AddInParameter(command, helper.Sinotaperiodo, DbType.DateTime, entity.Sinotaperiodo);
            //dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);
            dbProvider.AddInParameter(command, helper.Sinotacodi, DbType.Int32, entity.Sinotacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int sinotacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Sinotacodi, DbType.Int32, sinotacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiNotaDTO GetById(int sinotacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Sinotacodi, DbType.Int32, sinotacodi);
            SiNotaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiNotaDTO> List()
        {
            List<SiNotaDTO> entitys = new List<SiNotaDTO>();
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

        public List<SiNotaDTO> GetByCriteria(DateTime periodo, int mrepcodi, int verscodi)
        {
            List<SiNotaDTO> entitys = new List<SiNotaDTO>();
            var query = string.Format(helper.SqlGetByCriteria, mrepcodi, periodo.ToString(ConstantesBase.FormatoFechaBase), verscodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void UpdateOrden(SiNotaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateOrden);

            dbProvider.AddInParameter(command, helper.Sinotaorden, DbType.Int32, entity.Sinotaorden);
            dbProvider.AddInParameter(command, helper.Sinotausumodificacion, DbType.String, entity.Sinotausumodificacion);
            dbProvider.AddInParameter(command, helper.Sinotafecmodificacion, DbType.DateTime, entity.Sinotafecmodificacion);
            dbProvider.AddInParameter(command, helper.Sinotacodi, DbType.Int32, entity.Sinotacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public int GetMaxSinotaorden(DateTime periodo, int mrepcodi, int? verscodi)
        {
            var query = string.Format(helper.SqlGetMaxSinotaorden, mrepcodi, periodo.ToString(ConstantesBase.FormatoFechaBase), verscodi.GetValueOrDefault(-1));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            return id;
        }


    }
}
