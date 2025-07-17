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
    /// Clase de acceso a datos de la tabla PMO_SDDP_TIPO
    /// </summary>
    public class PmoSddpTipoRepository : RepositoryBase, IPmoSddpTipoRepository
    {
        public PmoSddpTipoRepository(string strConn) : base(strConn)
        {
        }

        PmoSddpTipoHelper helper = new PmoSddpTipoHelper();

        public int Save(PmoSddpTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tsddpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tsddpnomb, DbType.String, entity.Tsddpnomb);
            dbProvider.AddInParameter(command, helper.Tsddpabrev, DbType.String, entity.Tsddpabrev);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoSddpTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tsddpcodi, DbType.Int32, entity.Tsddpcodi);
            dbProvider.AddInParameter(command, helper.Tsddpnomb, DbType.String, entity.Tsddpnomb);
            dbProvider.AddInParameter(command, helper.Tsddpabrev, DbType.String, entity.Tsddpabrev);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tsddpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tsddpcodi, DbType.Int32, tsddpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoSddpTipoDTO GetById(int tsddpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tsddpcodi, DbType.Int32, tsddpcodi);
            PmoSddpTipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoSddpTipoDTO> List()
        {
            List<PmoSddpTipoDTO> entitys = new List<PmoSddpTipoDTO>();
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

        public List<PmoSddpTipoDTO> GetByCriteria()
        {
            List<PmoSddpTipoDTO> entitys = new List<PmoSddpTipoDTO>();
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
