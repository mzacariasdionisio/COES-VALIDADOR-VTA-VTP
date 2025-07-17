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
    /// Clase de acceso a datos de la tabla RE_LOGCORREO
    /// </summary>
    public class ReLogcorreoRepository: RepositoryBase, IReLogcorreoRepository
    {
        public ReLogcorreoRepository(string strConn): base(strConn)
        {
        }

        ReLogcorreoHelper helper = new ReLogcorreoHelper();

        public int Save(ReLogcorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Relcorcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Retcorcodi, DbType.Int32, entity.Retcorcodi);
            dbProvider.AddInParameter(command, helper.Relcorasunto, DbType.String, entity.Relcorasunto);
            dbProvider.AddInParameter(command, helper.Relcorto, DbType.String, entity.Relcorto);
            dbProvider.AddInParameter(command, helper.Relcorcc, DbType.String, entity.Relcorcc);
            dbProvider.AddInParameter(command, helper.Relcorbcc, DbType.String, entity.Relcorbcc);
            dbProvider.AddInParameter(command, helper.Relcorcuerpo, DbType.String, entity.Relcorcuerpo);
            dbProvider.AddInParameter(command, helper.Relcorusucreacion, DbType.String, entity.Relcorusucreacion);
            dbProvider.AddInParameter(command, helper.Relcorfeccreacion, DbType.DateTime, entity.Relcorfeccreacion);
            dbProvider.AddInParameter(command, helper.Relcorempresa, DbType.Int32, entity.Relcorempresa);
            dbProvider.AddInParameter(command, helper.Relcorarchivosnomb, DbType.String, entity.Relcorarchivosnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReLogcorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Retcorcodi, DbType.Int32, entity.Retcorcodi);
            dbProvider.AddInParameter(command, helper.Relcorasunto, DbType.String, entity.Relcorasunto);
            dbProvider.AddInParameter(command, helper.Relcorto, DbType.String, entity.Relcorto);
            dbProvider.AddInParameter(command, helper.Relcorcc, DbType.String, entity.Relcorcc);
            dbProvider.AddInParameter(command, helper.Relcorbcc, DbType.String, entity.Relcorbcc);
            dbProvider.AddInParameter(command, helper.Relcorcuerpo, DbType.String, entity.Relcorcuerpo);
            dbProvider.AddInParameter(command, helper.Relcorusucreacion, DbType.String, entity.Relcorusucreacion);
            dbProvider.AddInParameter(command, helper.Relcorfeccreacion, DbType.DateTime, entity.Relcorfeccreacion);
            dbProvider.AddInParameter(command, helper.Relcorempresa, DbType.Int32, entity.Relcorempresa);
            dbProvider.AddInParameter(command, helper.Relcorarchivosnomb, DbType.String, entity.Relcorarchivosnomb);
            dbProvider.AddInParameter(command, helper.Relcorcodi, DbType.Int32, entity.Relcorcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int relcorcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Relcorcodi, DbType.Int32, relcorcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReLogcorreoDTO GetById(int relcorcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Relcorcodi, DbType.Int32, relcorcodi);
            ReLogcorreoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReLogcorreoDTO> List()
        {
            List<ReLogcorreoDTO> entitys = new List<ReLogcorreoDTO>();
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

        public List<ReLogcorreoDTO> GetByCriteria()
        {
            List<ReLogcorreoDTO> entitys = new List<ReLogcorreoDTO>();
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

        public List<ReLogcorreoDTO> ObtenerPorFechaYTipo(DateTime fechaInicio, DateTime fechaFin, string idsPlantilla)
        {
            List<ReLogcorreoDTO> entitys = new List<ReLogcorreoDTO>();
            string query = string.Format(helper.SqlGetPorFechaYTipo, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idsPlantilla);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReLogcorreoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
