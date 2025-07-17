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
    /// Clase de acceso a datos de la tabla RER_FAC_PER_MED
    /// </summary>
    public class RerFacPerMedRepository : RepositoryBase, IRerFacPerMedRepository
    {
        public RerFacPerMedRepository(string strConn)
            : base(strConn)
        {
        }

        RerFacPerMedHelper helper = new RerFacPerMedHelper();

        public int Save(RerFacPerMedDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Rerfpmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rerfpmnombrearchivo, DbType.String, entity.Rerfpmnombrearchivo);
            dbProvider.AddInParameter(command, helper.Rerfpmdesde, DbType.DateTime, entity.Rerfpmdesde);
            dbProvider.AddInParameter(command, helper.Rerfpmhasta, DbType.DateTime, entity.Rerfpmhasta);
            dbProvider.AddInParameter(command, helper.Rerfpmusucreacion, DbType.String, entity.Rerfpmusucreacion);
            dbProvider.AddInParameter(command, helper.Rerfpmfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerFacPerMedDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerfpmnombrearchivo, DbType.String, entity.Rerfpmnombrearchivo);
            dbProvider.AddInParameter(command, helper.Rerfpmdesde, DbType.DateTime, entity.Rerfpmdesde);
            dbProvider.AddInParameter(command, helper.Rerfpmhasta, DbType.DateTime, entity.Rerfpmhasta);
            dbProvider.AddInParameter(command, helper.Rerfpmusucreacion, DbType.String, entity.Rerfpmusucreacion);
            dbProvider.AddInParameter(command, helper.Rerfpmfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Rerfpmcodi, DbType.Int32, entity.Rerfpmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerFpmCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerfpmcodi, DbType.Int32, rerFpmCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerFacPerMedDTO GetById(int rerFpmCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerfpmcodi, DbType.Int32, rerFpmCodi);
            RerFacPerMedDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerFacPerMedDTO> List()
        {
            List<RerFacPerMedDTO> entities = new List<RerFacPerMedDTO>();
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

    }
}
