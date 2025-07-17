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
    /// Clase de acceso a datos de la tabla ST_GENERADOR
    /// </summary>
    public class StGeneradorRepository : RepositoryBase, IStGeneradorRepository
    {
        public StGeneradorRepository(string strConn)
            : base(strConn)
        {
        }

        StGeneradorHelper helper = new StGeneradorHelper();

        public int Save(StGeneradorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Stgenrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Stgenrusucreacion, DbType.String, entity.Stgenrusucreacion);
            dbProvider.AddInParameter(command, helper.Stgenrfeccreacion, DbType.DateTime, entity.Stgenrfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StGeneradorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Stgenrusucreacion, DbType.String, entity.Stgenrusucreacion);
            dbProvider.AddInParameter(command, helper.Stgenrfeccreacion, DbType.DateTime, entity.Stgenrfeccreacion);
            dbProvider.AddInParameter(command, helper.Stgenrcodi, DbType.Int32, entity.Stgenrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int stgenrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Stgenrcodi, DbType.Int32, stgenrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteVersion(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteVersion);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StGeneradorDTO GetById(int stgenrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Stgenrcodi, DbType.Int32, stgenrcodi);
            StGeneradorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                }
            }

            return entity;
        }

        public List<StGeneradorDTO> List(int strecacodi)
        {
            List<StGeneradorDTO> entitys = new List<StGeneradorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.String, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StGeneradorDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StGeneradorDTO> GetByCriteria()
        {
            List<StGeneradorDTO> entitys = new List<StGeneradorDTO>();
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

        public List<StGeneradorDTO> ListByStGeneradorVersion(int strecacodi)
        {
            List<StGeneradorDTO> entitys = new List<StGeneradorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByStGeneradorVersion);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.String, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StGeneradorDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StGeneradorDTO> ListByStGeneradorReporte(int strecacodi)
        {
            List<StGeneradorDTO> entitys = new List<StGeneradorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByStGeneradorReporte);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.String, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StGeneradorDTO entity = new StGeneradorDTO();

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iBarrnombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
