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
    /// Clase de acceso a datos de la tabla CP_FORZADO_DET
    /// </summary>
    public class CpForzadoDetRepository : RepositoryBase, ICpForzadoDetRepository
    {
        public CpForzadoDetRepository(string strConn) : base(strConn)
        {
        }

        CpForzadoDetHelper helper = new CpForzadoDetHelper();

        public int Save(CpForzadoDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpfzdtcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpfzcodi, DbType.Int32, entity.Cpfzcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpfzdtperiodoini, DbType.Int32, entity.Cpfzdtperiodoini);
            dbProvider.AddInParameter(command, helper.Cpfzdtperiodofin, DbType.Int32, entity.Cpfzdtperiodofin);
            dbProvider.AddInParameter(command, helper.Cpfzdtflagcreacion, DbType.String, entity.Cpfzdtflagcreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpForzadoDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpfzcodi, DbType.Int32, entity.Cpfzcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpfzdtperiodoini, DbType.Int32, entity.Cpfzdtperiodoini);
            dbProvider.AddInParameter(command, helper.Cpfzdtperiodofin, DbType.Int32, entity.Cpfzdtperiodofin);
            dbProvider.AddInParameter(command, helper.Cpfzdtflagcreacion, DbType.String, entity.Cpfzdtflagcreacion);
            dbProvider.AddInParameter(command, helper.Cpfzdtcodi, DbType.Int32, entity.Cpfzdtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpfzdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpfzdtcodi, DbType.Int32, cpfzdtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpForzadoDetDTO GetById(int cpfzdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpfzdtcodi, DbType.Int32, cpfzdtcodi);
            CpForzadoDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpForzadoDetDTO> List()
        {
            List<CpForzadoDetDTO> entitys = new List<CpForzadoDetDTO>();
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

        public List<CpForzadoDetDTO> GetByCriteria()
        {
            List<CpForzadoDetDTO> entitys = new List<CpForzadoDetDTO>();
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

        public List<CpForzadoDetDTO> GetByCpfzcodi(int cpfzcodi)
        {
            List<CpForzadoDetDTO> entitys = new List<CpForzadoDetDTO>();
            var querySql = string.Format(helper.SqlGetBycpfzcodi, cpfzcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(querySql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
