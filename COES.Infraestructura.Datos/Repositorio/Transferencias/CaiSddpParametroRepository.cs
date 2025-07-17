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
    /// Clase de acceso a datos de la tabla CAI_SDDP_PARAMETRO
    /// </summary>

    public class CaiSddpParametroRepository : RepositoryBase, ICaiSddpParametroRepository
    {

        public CaiSddpParametroRepository(string strConn)
            : base(strConn)
        {
        }

        CaiSddpParametroHelper helper = new CaiSddpParametroHelper();

        public int Save(CaiSddpParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Sddppmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Sddppmtc, DbType.Decimal, entity.Sddppmtc);
            dbProvider.AddInParameter(command, helper.Sddppmsemini, DbType.Int32, entity.Sddppmsemini);
            dbProvider.AddInParameter(command, helper.Sddppmnumsem, DbType.Int32, entity.Sddppmnumsem);
            dbProvider.AddInParameter(command, helper.Sddppmcantbloque, DbType.Int32, entity.Sddppmcantbloque);
            dbProvider.AddInParameter(command, helper.Sddppmnumserie, DbType.Int32, entity.Sddppmnumserie);
            dbProvider.AddInParameter(command, helper.Sddppmusucreacion, DbType.String, entity.Sddppmusucreacion);
            dbProvider.AddInParameter(command, helper.Sddppmfeccreacion, DbType.DateTime, entity.Sddppmfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiSddpParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Sddppmtc, DbType.Int32, entity.Sddppmtc);
            dbProvider.AddInParameter(command, helper.Sddppmsemini, DbType.Int32, entity.Sddppmsemini);
            dbProvider.AddInParameter(command, helper.Sddppmnumsem, DbType.Int32, entity.Sddppmnumsem);
            dbProvider.AddInParameter(command, helper.Sddppmcantbloque, DbType.Int32, entity.Sddppmcantbloque);
            dbProvider.AddInParameter(command, helper.Sddppmnumserie, DbType.Int32, entity.Sddppmnumserie);
            dbProvider.AddInParameter(command, helper.Sddppmusucreacion, DbType.String, entity.Sddppmusucreacion);
            dbProvider.AddInParameter(command, helper.Sddppmfeccreacion, DbType.DateTime, entity.Sddppmfeccreacion);
            dbProvider.AddInParameter(command, helper.Sddppmcodi, DbType.Int32, entity.Sddppmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiSddpParametroDTO GetById(int caiajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            CaiSddpParametroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiSddpParametroDTO> List(int Sddppmcodi)
        {
            List<CaiSddpParametroDTO> entitys = new List<CaiSddpParametroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, Sddppmcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CaiSddpParametroDTO> GetByCriteria()
        {
            List<CaiSddpParametroDTO> entitys = new List<CaiSddpParametroDTO>();
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

