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
    /// Clase de acceso a datos de la tabla RTU_CONFIGURACION_GRUPO
    /// </summary>
    public class RtuConfiguracionGrupoRepository : RepositoryBase, IRtuConfiguracionGrupoRepository
    {
        public RtuConfiguracionGrupoRepository(string strConn) : base(strConn)
        {
        }

        RtuConfiguracionGrupoHelper helper = new RtuConfiguracionGrupoHelper();

        public int Save(RtuConfiguracionGrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rtugrucodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rtugruindreporte, DbType.String, entity.Rtugruindreporte);
            dbProvider.AddInParameter(command, helper.Rtugruorden, DbType.Int32, entity.Rtugruorden);
            dbProvider.AddInParameter(command, helper.Rtuconcodi, DbType.Int32, entity.Rtuconcodi);
            dbProvider.AddInParameter(command, helper.Rtugrutipo, DbType.String, entity.Rtugrutipo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RtuConfiguracionGrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rtugruindreporte, DbType.String, entity.Rtugruindreporte);
            dbProvider.AddInParameter(command, helper.Rtugruorden, DbType.Int32, entity.Rtugruorden);
            dbProvider.AddInParameter(command, helper.Rtuconcodi, DbType.Int32, entity.Rtuconcodi);
            dbProvider.AddInParameter(command, helper.Rtugrutipo, DbType.String, entity.Rtugrutipo);
            dbProvider.AddInParameter(command, helper.Rtugrucodi, DbType.Int32, entity.Rtugrucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rtugrucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rtugrucodi, DbType.Int32, rtugrucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RtuConfiguracionGrupoDTO GetById(int rtugrucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rtugrucodi, DbType.Int32, rtugrucodi);
            RtuConfiguracionGrupoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RtuConfiguracionGrupoDTO> List()
        {
            List<RtuConfiguracionGrupoDTO> entitys = new List<RtuConfiguracionGrupoDTO>();
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

        public List<RtuConfiguracionGrupoDTO> GetByCriteria()
        {
            List<RtuConfiguracionGrupoDTO> entitys = new List<RtuConfiguracionGrupoDTO>();
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
