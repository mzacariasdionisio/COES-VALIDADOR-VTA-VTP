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
    /// Clase de acceso a datos de la tabla SI_MENUREPORTE
    /// </summary>
    public class SiMenureporteRepository : RepositoryBase, ISiMenureporteRepository
    {
        public SiMenureporteRepository(string strConn)
            : base(strConn)
        {
        }

        SiMenureporteHelper helper = new SiMenureporteHelper();

        public int Save(SiMenureporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repdescripcion, DbType.String, entity.Repdescripcion);
            dbProvider.AddInParameter(command, helper.Repabrev, DbType.String, entity.Repabrev);
            dbProvider.AddInParameter(command, helper.Reptiprepcodi, DbType.Int32, entity.Reptiprepcodi);
            dbProvider.AddInParameter(command, helper.Repcatecodi, DbType.Int32, entity.Repcatecodi);
            dbProvider.AddInParameter(command, helper.Repstado, DbType.Int32, entity.Repstado);
            dbProvider.AddInParameter(command, helper.Repusucreacion, DbType.String, entity.Repusucreacion);
            dbProvider.AddInParameter(command, helper.Repffeccreacion, DbType.DateTime, entity.Repffeccreacion);
            dbProvider.AddInParameter(command, helper.Repusumodificacion, DbType.String, entity.Repusumodificacion);
            dbProvider.AddInParameter(command, helper.Repfecmodificacion, DbType.DateTime, entity.Repfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiMenureporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, entity.Repcodi);
            dbProvider.AddInParameter(command, helper.Repdescripcion, DbType.String, entity.Repdescripcion);
            dbProvider.AddInParameter(command, helper.Repabrev, DbType.String, entity.Repabrev);
            dbProvider.AddInParameter(command, helper.Reptiprepcodi, DbType.Int32, entity.Reptiprepcodi);
            dbProvider.AddInParameter(command, helper.Repcatecodi, DbType.Int32, entity.Repcatecodi);
            dbProvider.AddInParameter(command, helper.Repstado, DbType.Int32, entity.Repstado);
            dbProvider.AddInParameter(command, helper.Repusucreacion, DbType.String, entity.Repusucreacion);
            dbProvider.AddInParameter(command, helper.Repffeccreacion, DbType.DateTime, entity.Repffeccreacion);
            dbProvider.AddInParameter(command, helper.Repusumodificacion, DbType.String, entity.Repusumodificacion);
            dbProvider.AddInParameter(command, helper.Repfecmodificacion, DbType.DateTime, entity.Repfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int repcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, repcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMenureporteDTO GetById(int repcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, repcodi);
            SiMenureporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMenureporteDTO> List()
        {
            List<SiMenureporteDTO> entitys = new List<SiMenureporteDTO>();
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

        public List<SiMenureporteDTO> GetByCriteria()
        {
            List<SiMenureporteDTO> entitys = new List<SiMenureporteDTO>();
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

        public List<SiMenureporteDTO> GetListaAdmReporte(int tmrepcodi)
        {
            List<SiMenureporteDTO> entitys = new List<SiMenureporteDTO>();
            string query = string.Format(helper.SqlGetListaAdmReporte, tmrepcodi);
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

        #region siosein2
        public SiMenureporteDTO GetSimenureportebyIndex(string mrepabrev, int tmrepcodi)
        {
            string query = string.Format(helper.SqlGetSimenureportebyIndex, mrepabrev, tmrepcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            SiMenureporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        #endregion
    }
}
