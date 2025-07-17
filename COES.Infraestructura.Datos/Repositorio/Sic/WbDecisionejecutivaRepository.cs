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
    /// Clase de acceso a datos de la tabla WB_DECISIONEJECUTIVA
    /// </summary>
    public class WbDecisionejecutivaRepository: RepositoryBase, IWbDecisionejecutivaRepository
    {
        public WbDecisionejecutivaRepository(string strConn): base(strConn)
        {
        }

        WbDecisionejecutivaHelper helper = new WbDecisionejecutivaHelper();

        public int Save(WbDecisionejecutivaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Desejecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Desejedescripcion, DbType.String, entity.Desejedescripcion);
            dbProvider.AddInParameter(command, helper.Desejefechapub, DbType.DateTime, entity.Desejefechapub);
            dbProvider.AddInParameter(command, helper.Desejetipo, DbType.String, entity.Desejetipo);
            dbProvider.AddInParameter(command, helper.Desejeestado, DbType.String, entity.Desejeestado);
            dbProvider.AddInParameter(command, helper.Desejefile, DbType.String, entity.Desejefile);
            dbProvider.AddInParameter(command, helper.Desejeextension, DbType.String, entity.Desejeextension);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbDecisionejecutivaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Desejedescripcion, DbType.String, entity.Desejedescripcion);
            dbProvider.AddInParameter(command, helper.Desejefechapub, DbType.DateTime, entity.Desejefechapub);
            dbProvider.AddInParameter(command, helper.Desejetipo, DbType.String, entity.Desejetipo);
            dbProvider.AddInParameter(command, helper.Desejeestado, DbType.String, entity.Desejeestado);
            dbProvider.AddInParameter(command, helper.Desejefile, DbType.String, entity.Desejefile);
            dbProvider.AddInParameter(command, helper.Desejeextension, DbType.String, entity.Desejeextension);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Desejecodi, DbType.Int32, entity.Desejecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int desejecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Desejecodi, DbType.Int32, desejecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbDecisionejecutivaDTO GetById(int desejecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Desejecodi, DbType.Int32, desejecodi);
            WbDecisionejecutivaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbDecisionejecutivaDTO> List()
        {
            List<WbDecisionejecutivaDTO> entitys = new List<WbDecisionejecutivaDTO>();
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

        public List<WbDecisionejecutivaDTO> GetByCriteria(string tipo)
        {
            List<WbDecisionejecutivaDTO> entitys = new List<WbDecisionejecutivaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Desejetipo, DbType.String, tipo);

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
