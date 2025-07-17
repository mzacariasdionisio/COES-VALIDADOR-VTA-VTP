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
    /// Clase de acceso a datos de la tabla RER_CENTRAL_PMPO
    /// </summary>
    public class RerCentralPmpoRepository : RepositoryBase, IRerCentralPmpoRepository
    {
        public RerCentralPmpoRepository(string strConn)
            : base(strConn)
        {
        }

        RerCentralPmpoHelper helper = new RerCentralPmpoHelper();

        public int Save(RerCentralPmpoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rercpmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, entity.Rercencodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Rercpmusucreacion, DbType.String, entity.Rercpmusucreacion);
            dbProvider.AddInParameter(command, helper.Rercpmfeccreacion, DbType.DateTime, entity.Rercpmfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerCentralPmpoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, entity.Rercencodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Rercpmusucreacion, DbType.String, entity.Rercpmusucreacion);
            dbProvider.AddInParameter(command, helper.Rercpmfeccreacion, DbType.DateTime, entity.Rercpmfeccreacion);
            dbProvider.AddInParameter(command, helper.Rercpmcodi, DbType.Int32, entity.Rercpmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerCpmCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rercpmcodi, DbType.Int32, rerCpmCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerCentralPmpoDTO GetById(int rerCpmCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rercpmcodi, DbType.Int32, rerCpmCodi);
            RerCentralPmpoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerCentralPmpoDTO> List()
        {
            List<RerCentralPmpoDTO> entities = new List<RerCentralPmpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<RerCentralPmpoDTO> ListByRercencodi(int Rercencodi)
        {
            List<RerCentralPmpoDTO> entities = new List<RerCentralPmpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByRercencodi);
            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, Rercencodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralPmpoDTO entity = new RerCentralPmpoDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iGrupotipocogen = dr.GetOrdinal(helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);


                    entities.Add(entity);
                }
            }

            return entities;
        }

        public void DeleteAllByRercencodi(int Rercencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAllByRercencodi);

            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, Rercencodi);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}
