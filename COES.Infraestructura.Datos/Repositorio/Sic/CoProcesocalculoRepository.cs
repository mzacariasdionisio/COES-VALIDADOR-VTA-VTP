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
    /// Clase de acceso a datos de la tabla CO_PROCESOCALCULO
    /// </summary>
    public class CoProcesocalculoRepository : RepositoryBase, ICoProcesocalculoRepository
    {
        public CoProcesocalculoRepository(string strConn) : base(strConn)
        {
        }

        CoProcesocalculoHelper helper = new CoProcesocalculoHelper();

        public int Save(CoProcesocalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Coprcacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Coprcafecproceso, DbType.DateTime, entity.Coprcafecproceso);
            dbProvider.AddInParameter(command, helper.Coprcausuproceso, DbType.String, entity.Coprcausuproceso);
            dbProvider.AddInParameter(command, helper.Coprcaestado, DbType.String, entity.Coprcaestado);
            dbProvider.AddInParameter(command, helper.Coprcafecinicio, DbType.DateTime, entity.Coprcafecinicio);
            dbProvider.AddInParameter(command, helper.Coprcafecfin, DbType.DateTime, entity.Coprcafecfin);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Vcrecaversion, DbType.Int32, entity.Vcrecaversion);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Coprcafuentedato, DbType.String, entity.Coprcafuentedato);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoProcesocalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Coprcafecproceso, DbType.DateTime, entity.Coprcafecproceso);
            dbProvider.AddInParameter(command, helper.Coprcausuproceso, DbType.String, entity.Coprcausuproceso);
            dbProvider.AddInParameter(command, helper.Coprcaestado, DbType.String, entity.Coprcaestado);
            dbProvider.AddInParameter(command, helper.Coprcafecinicio, DbType.DateTime, entity.Coprcafecinicio);
            dbProvider.AddInParameter(command, helper.Coprcafecfin, DbType.DateTime, entity.Coprcafecfin);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Vcrecaversion, DbType.Int32, entity.Vcrecaversion);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Coprcafuentedato, DbType.String, entity.Coprcafuentedato);
            dbProvider.AddInParameter(command, helper.Coprcacodi, DbType.Int32, entity.Coprcacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int coprcacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Coprcacodi, DbType.Int32, coprcacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoProcesocalculoDTO GetById(int Covercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, Covercodi);
            CoProcesocalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoProcesocalculoDTO> List()
        {
            List<CoProcesocalculoDTO> entitys = new List<CoProcesocalculoDTO>();
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

        public List<CoProcesocalculoDTO> GetByCriteria()
        {
            List<CoProcesocalculoDTO> entitys = new List<CoProcesocalculoDTO>();
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

        public int ValidarExistencia(int idPeriodo, int idVersion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarExistencia);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, idPeriodo);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, idVersion);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;                
        }
               
    }
}
