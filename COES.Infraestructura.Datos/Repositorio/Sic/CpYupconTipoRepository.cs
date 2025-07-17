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
    /// Clase de acceso a datos de la tabla CP_YUPCON_TIPO
    /// </summary>
    public class CpYupconTipoRepository : RepositoryBase, ICpYupconTipoRepository
    {
        public CpYupconTipoRepository(string strConn) : base(strConn)
        {
        }

        CpYupconTipoHelper helper = new CpYupconTipoHelper();

        public int Save(CpYupconTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tyupcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tyupnombre, DbType.String, entity.Tyupnombre);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpYupconTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tyupcodi, DbType.Int32, entity.Tyupcodi);
            dbProvider.AddInParameter(command, helper.Tyupnombre, DbType.String, entity.Tyupnombre);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tyupcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tyupcodi, DbType.Int32, tyupcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpYupconTipoDTO GetById(int tyupcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tyupcodi, DbType.Int32, tyupcodi);
            CpYupconTipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpYupconTipoDTO> List()
        {
            List<CpYupconTipoDTO> entitys = new List<CpYupconTipoDTO>();
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

        public List<CpYupconTipoDTO> GetByCriteria()
        {
            List<CpYupconTipoDTO> entitys = new List<CpYupconTipoDTO>();
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
