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
    /// Clase de acceso a datos de la tabla EXT_LOGENVIO
    /// </summary>
    public class ExtLogenvioRepository : RepositoryBase, IExtLogenvioRepository
    {
        public ExtLogenvioRepository(string strConn)
            : base(strConn)
        {
        }

        ExtLogenvioHelper helper = new ExtLogenvioHelper();

        public int Save(ExtLogenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Logcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Filenomb, DbType.String, entity.Filenomb);
            dbProvider.AddInParameter(command, helper.Origlectcodi, DbType.Int32, entity.Origlectcodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);
            dbProvider.AddInParameter(command, helper.Feccarga, DbType.DateTime, entity.Feccarga);
            dbProvider.AddInParameter(command, helper.Nrosemana, DbType.Int32, entity.Nrosemana);
            dbProvider.AddInParameter(command, helper.Fecproceso, DbType.DateTime, entity.Fecproceso);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.NroAnio, DbType.Int32, entity.NroAnio);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ExtLogenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Logcodi, DbType.Int32, entity.Logcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Filenomb, DbType.String, entity.Filenomb);
            dbProvider.AddInParameter(command, helper.Origlectcodi, DbType.Int32, entity.Origlectcodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);
            dbProvider.AddInParameter(command, helper.Feccarga, DbType.DateTime, entity.Feccarga);
            dbProvider.AddInParameter(command, helper.Nrosemana, DbType.Int32, entity.Nrosemana);
            dbProvider.AddInParameter(command, helper.Fecproceso, DbType.DateTime, entity.Fecproceso);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.NroAnio, DbType.Int32, entity.NroAnio);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int logcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Logcodi, DbType.Int32, logcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ExtLogenvioDTO GetById(int logcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Logcodi, DbType.Int32, logcodi);
            ExtLogenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ExtLogenvioDTO> List()
        {
            List<ExtLogenvioDTO> entitys = new List<ExtLogenvioDTO>();
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

        public List<ExtLogenvioDTO> GetByCriteria(int lectCodi)
        {
            List<ExtLogenvioDTO> entitys = new List<ExtLogenvioDTO>();

            String query = String.Format(helper.SqlGetByCriteria, lectCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ExtLogenvioDTO entity = helper.Create(dr);

                    int iEmprNomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iEstEnvNomb = dr.GetOrdinal(this.helper.EstEnvNomb);
                    if (!dr.IsDBNull(iEstEnvNomb)) entity.EstEnvNomb = dr.GetString(iEstEnvNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
