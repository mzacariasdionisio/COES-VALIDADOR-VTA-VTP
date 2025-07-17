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
    /// Clase de acceso a datos de la tabla WB_REGISTRO_MODPLAN
    /// </summary>
    public class WbRegistroModplanRepository: RepositoryBase, IWbRegistroModplanRepository
    {
        public WbRegistroModplanRepository(string strConn): base(strConn)
        {
        }

        WbRegistroModplanHelper helper = new WbRegistroModplanHelper();

        public int Save(WbRegistroModplanDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Regmodcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Regmodplan, DbType.String, entity.Regmodplan);
            dbProvider.AddInParameter(command, helper.Regmodversion, DbType.String, entity.Regmodversion);
            dbProvider.AddInParameter(command, helper.Regmodusuario, DbType.String, entity.Regmodusuario);
            dbProvider.AddInParameter(command, helper.Regmodfecha, DbType.DateTime, entity.Regmodfecha);
            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, entity.Vermplcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Regmodtipo, DbType.Int32, entity.Regmodtipo);
            dbProvider.AddInParameter(command, helper.Arcmplcodi, DbType.Int32, entity.Arcmplcodi);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbRegistroModplanDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Regmodplan, DbType.String, entity.Regmodplan);
            dbProvider.AddInParameter(command, helper.Regmodversion, DbType.String, entity.Regmodversion);
            dbProvider.AddInParameter(command, helper.Regmodusuario, DbType.String, entity.Regmodusuario);
            dbProvider.AddInParameter(command, helper.Regmodfecha, DbType.DateTime, entity.Regmodfecha);
            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, entity.Vermplcodi);
            dbProvider.AddInParameter(command, helper.Regmodtipo, DbType.Int32, entity.Regmodtipo);
            dbProvider.AddInParameter(command, helper.Regmodcodi, DbType.Int32, entity.Regmodcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int regmodcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Regmodcodi, DbType.Int32, regmodcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbRegistroModplanDTO GetById(int regmodcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Regmodcodi, DbType.Int32, regmodcodi);
            WbRegistroModplanDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbRegistroModplanDTO> List()
        {
            List<WbRegistroModplanDTO> entitys = new List<WbRegistroModplanDTO>();
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

        public List<WbRegistroModplanDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            List<WbRegistroModplanDTO> entitys = new List<WbRegistroModplanDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbRegistroModplanDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<WbRegistroModplanDTO> ObtenerReporte(int idVersion, int tipo)
        {
            List<WbRegistroModplanDTO> entitys = new List<WbRegistroModplanDTO>();          
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerReporte);
            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, idVersion);
            dbProvider.AddInParameter(command, helper.Regmodtipo, DbType.Int32, tipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbRegistroModplanDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
