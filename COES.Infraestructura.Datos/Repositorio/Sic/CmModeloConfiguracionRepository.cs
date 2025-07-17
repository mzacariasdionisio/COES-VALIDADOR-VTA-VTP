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
    /// Clase de acceso a datos de la tabla CM_MODELO_CONFIGURACION
    /// </summary>
    public class CmModeloConfiguracionRepository : RepositoryBase, ICmModeloConfiguracionRepository
    {
        public CmModeloConfiguracionRepository(string strConn) : base(strConn)
        {
        }

        CmModeloConfiguracionHelper helper = new CmModeloConfiguracionHelper();

        public int Save(CmModeloConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Modconcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Modagrcodi, DbType.Int32, entity.Modagrcodi);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Modcontipo, DbType.String, entity.Modcontipo);
            dbProvider.AddInParameter(command, helper.Modconsigno, DbType.String, entity.Modconsigno);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmModeloConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Modconcodi, DbType.Int32, entity.Modconcodi);
            dbProvider.AddInParameter(command, helper.Modagrcodi, DbType.Int32, entity.Modagrcodi);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Modcontipo, DbType.String, entity.Modcontipo);
            dbProvider.AddInParameter(command, helper.Modconsigno, DbType.String, entity.Modconsigno);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int modconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Modconcodi, DbType.Int32, modconcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmModeloConfiguracionDTO GetById(int modconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Modconcodi, DbType.Int32, modconcodi);
            CmModeloConfiguracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmModeloConfiguracionDTO> List()
        {
            List<CmModeloConfiguracionDTO> entitys = new List<CmModeloConfiguracionDTO>();
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

        public List<CmModeloConfiguracionDTO> GetByCriteria(int modembcodi)
        {
            List<CmModeloConfiguracionDTO> entitys = new List<CmModeloConfiguracionDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, modembcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
