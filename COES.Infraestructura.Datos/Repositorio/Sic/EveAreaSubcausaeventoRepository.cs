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
    /// Clase de acceso a datos de la tabla EVE_AREA_SUBCAUSAEVENTO
    /// </summary>
    public class EveAreaSubcausaeventoRepository : RepositoryBase, IEveAreaSubcausaeventoRepository
    {
        public EveAreaSubcausaeventoRepository(string strConn)
            : base(strConn)
        {
        }

        EveAreaSubcausaeventoHelper helper = new EveAreaSubcausaeventoHelper();

        public int Save(EveAreaSubcausaeventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Arscaucodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Arscauusucreacion, DbType.String, entity.Arscauusucreacion);
            dbProvider.AddInParameter(command, helper.Arscaufeccreacion, DbType.DateTime, entity.Arscaufeccreacion);
            dbProvider.AddInParameter(command, helper.Arscauusumodificacion, DbType.String, entity.Arscauusumodificacion);
            dbProvider.AddInParameter(command, helper.Arscaufecmodificacion, DbType.DateTime, entity.Arscaufecmodificacion);
            dbProvider.AddInParameter(command, helper.Arscauactivo, DbType.Int32, entity.Arscauactivo);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveAreaSubcausaeventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Arscauusucreacion, DbType.String, entity.Arscauusucreacion);
            dbProvider.AddInParameter(command, helper.Arscaufeccreacion, DbType.DateTime, entity.Arscaufeccreacion);
            dbProvider.AddInParameter(command, helper.Arscauusumodificacion, DbType.String, entity.Arscauusumodificacion);
            dbProvider.AddInParameter(command, helper.Arscaufecmodificacion, DbType.DateTime, entity.Arscaufecmodificacion);
            dbProvider.AddInParameter(command, helper.Arscauactivo, DbType.Int32, entity.Arscauactivo);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);

            dbProvider.AddInParameter(command, helper.Arscaucodi, DbType.Int32, entity.Arscaucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int arscaucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Arscaucodi, DbType.Int32, arscaucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveAreaSubcausaeventoDTO GetById(int arscaucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Arscaucodi, DbType.Int32, arscaucodi);
            EveAreaSubcausaeventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveAreaSubcausaeventoDTO> List()
        {
            List<EveAreaSubcausaeventoDTO> entitys = new List<EveAreaSubcausaeventoDTO>();
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

        public List<EveAreaSubcausaeventoDTO> GetByCriteria()
        {
            List<EveAreaSubcausaeventoDTO> entitys = new List<EveAreaSubcausaeventoDTO>();
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

        public List<int> ListarSubcausacodiRegistrados()
        {
            List<int> entitys = new List<int>();
            string query = string.Format(helper.SqlListarSubcausacodiRegistrados);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entitys.Add(Convert.ToInt32(dr.GetValue(iSubcausacodi)));
                }
            }

            return entitys;
        }

        public List<EveAreaSubcausaeventoDTO> ListBySubcausacodi(int subcausacodi, string estado)
        {
            List<EveAreaSubcausaeventoDTO> entitys = new List<EveAreaSubcausaeventoDTO>();
            string query = string.Format(helper.SqlListBySubcausacodi, subcausacodi, estado);
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
    }
}
