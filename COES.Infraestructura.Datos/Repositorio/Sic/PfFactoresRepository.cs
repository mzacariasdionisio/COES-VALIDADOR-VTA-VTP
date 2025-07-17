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
    /// Clase de acceso a datos de la tabla PF_FACTORES
    /// </summary>
    public class PfFactoresRepository : RepositoryBase, IPfFactoresRepository
    {
        public PfFactoresRepository(string strConn) : base(strConn)
        {
        }

        PfFactoresHelper helper = new PfFactoresHelper();

        public int Save(PfFactoresDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pffactcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, entity.Pfpericodi);
            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, entity.Pfverscodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pffacttipo, DbType.Int32, entity.Pffacttipo);
            dbProvider.AddInParameter(command, helper.Pffactfi, DbType.Decimal, entity.Pffactfi);
            dbProvider.AddInParameter(command, helper.Pffactfp, DbType.Decimal, entity.Pffactfp);
            dbProvider.AddInParameter(command, helper.Pffactincremental, DbType.Int32, entity.Pffactincremental);
            dbProvider.AddInParameter(command, helper.Pffactunidadnomb, DbType.String, entity.Pffactunidadnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfFactoresDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, entity.Pfpericodi);
            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, entity.Pfverscodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pffacttipo, DbType.Int32, entity.Pffacttipo);
            dbProvider.AddInParameter(command, helper.Pffactfi, DbType.Decimal, entity.Pffactfi);
            dbProvider.AddInParameter(command, helper.Pffactfp, DbType.Decimal, entity.Pffactfp);
            dbProvider.AddInParameter(command, helper.Pffactincremental, DbType.Int32, entity.Pffactincremental);
            dbProvider.AddInParameter(command, helper.Pffactunidadnomb, DbType.String, entity.Pffactunidadnomb);
            dbProvider.AddInParameter(command, helper.Pffactcodi, DbType.Int32, entity.Pffactcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pffactcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pffactcodi, DbType.Int32, pffactcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfFactoresDTO GetById(int pffactcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pffactcodi, DbType.Int32, pffactcodi);
            PfFactoresDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfFactoresDTO> List()
        {
            List<PfFactoresDTO> entitys = new List<PfFactoresDTO>();
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

        public List<PfFactoresDTO> GetByCriteria()
        {
            List<PfFactoresDTO> entitys = new List<PfFactoresDTO>();
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

        public List<PfFactoresDTO> ListarFactoresFiltro(int pericodi, int recacodi, int emprcodi, int centralId, int versionId)
        {
            List<PfFactoresDTO> entitys = new List<PfFactoresDTO>();

            string query = string.Format(helper.SqlListarFactorIndispFiltro, recacodi, pericodi, emprcodi, centralId, versionId);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCentral = dr.GetOrdinal(helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
