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
    /// Clase de acceso a datos de la tabla WB_DECISIONEJECUTADO_DET
    /// </summary>
    public class WbDecisionejecutadoDetRepository: RepositoryBase, IWbDecisionejecutadoDetRepository
    {
        public WbDecisionejecutadoDetRepository(string strConn): base(strConn)
        {
        }

        WbDecisionejecutadoDetHelper helper = new WbDecisionejecutadoDetHelper();

        public int Save(WbDecisionejecutadoDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dejdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dejdetdescripcion, DbType.String, entity.Dejdetdescripcion);
            dbProvider.AddInParameter(command, helper.Dejdetfile, DbType.String, entity.Dejdetfile);
            dbProvider.AddInParameter(command, helper.Desejecodi, DbType.Int32, entity.Desejecodi);
            dbProvider.AddInParameter(command, helper.Dejdettipo, DbType.String, entity.Dejdettipo);
            dbProvider.AddInParameter(command, helper.Dejdetestado, DbType.String, entity.Dejdetestado);
            dbProvider.AddInParameter(command, helper.Desdetextension, DbType.String, entity.Desdetextension);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbDecisionejecutadoDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dejdetdescripcion, DbType.String, entity.Dejdetdescripcion);
            dbProvider.AddInParameter(command, helper.Dejdetfile, DbType.String, entity.Dejdetfile);
            dbProvider.AddInParameter(command, helper.Desejecodi, DbType.Int32, entity.Desejecodi);
            dbProvider.AddInParameter(command, helper.Dejdettipo, DbType.String, entity.Dejdettipo);
            dbProvider.AddInParameter(command, helper.Dejdetestado, DbType.String, entity.Dejdetestado);
            dbProvider.AddInParameter(command, helper.Desdetextension, DbType.String, entity.Desdetextension);
            dbProvider.AddInParameter(command, helper.Dejdetcodi, DbType.Int32, entity.Dejdetcodi);           

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarDescripcion(int id, string descripcion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarDescripcion);

            dbProvider.AddInParameter(command, helper.Dejdetdescripcion, DbType.String, descripcion);
            dbProvider.AddInParameter(command, helper.Dejdetcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int dejdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Desejecodi, DbType.Int32, dejdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteItem(int dejdetcodi)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteItem);

            dbProvider.AddInParameter(command, helper.Dejdetcodi, DbType.Int32, dejdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbDecisionejecutadoDetDTO GetById(int dejdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Dejdetcodi, DbType.Int32, dejdetcodi);
            WbDecisionejecutadoDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbDecisionejecutadoDetDTO> List()
        {
            List<WbDecisionejecutadoDetDTO> entitys = new List<WbDecisionejecutadoDetDTO>();
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

        public List<WbDecisionejecutadoDetDTO> GetByCriteria(int desejecodi)
        {
            List<WbDecisionejecutadoDetDTO> entitys = new List<WbDecisionejecutadoDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Desejecodi, DbType.Int32, desejecodi);

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
