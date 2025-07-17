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
    /// Clase de acceso a datos de la tabla EPO_CONFIGURA
    /// </summary>
    public class EpoConfiguraRepository: RepositoryBase, IEpoConfiguraRepository
    {
        public EpoConfiguraRepository(string strConn): base(strConn)
        {
        }

        EpoConfiguraHelper helper = new EpoConfiguraHelper();

        public int Save(EpoConfiguraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Confcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Confplazorevcoesporv, DbType.Int32, entity.Confplazorevcoesporv);
            dbProvider.AddInParameter(command, helper.Confplazorevcoesvenc, DbType.Int32, entity.Confplazorevcoesvenc);
            dbProvider.AddInParameter(command, helper.Confplazolevobsporv, DbType.Int32, entity.Confplazolevobsporv);
            dbProvider.AddInParameter(command, helper.Confplazolevobsvenc, DbType.Int32, entity.Confplazolevobsvenc);
            dbProvider.AddInParameter(command, helper.Confplazoalcancesvenc, DbType.Int32, entity.Confplazoalcancesvenc);
            dbProvider.AddInParameter(command, helper.Confplazoverificacionvenc, DbType.Int32, entity.Confplazoverificacionvenc);
            dbProvider.AddInParameter(command, helper.Confplazorevterceroinvporv, DbType.Int32, entity.Confplazorevterceroinvporv);
            dbProvider.AddInParameter(command, helper.Confplazorevterceroinvvenc, DbType.Int32, entity.Confplazorevterceroinvvenc);
            dbProvider.AddInParameter(command, helper.Confplazoenvestterceroinvporv, DbType.Int32, entity.Confplazoenvestterceroinvporv);
            dbProvider.AddInParameter(command, helper.Confplazoenvestterceroinvvenc, DbType.Int32, entity.Confplazoenvestterceroinvvenc);
            dbProvider.AddInParameter(command, helper.Confindigestionsnpepo, DbType.Int32, entity.Confindigestionsnpepo);
            dbProvider.AddInParameter(command, helper.Confindiporcatencionepo, DbType.Int32, entity.Confindiporcatencionepo);
            dbProvider.AddInParameter(command, helper.Confindigestionsnpeo, DbType.Int32, entity.Confindigestionsnpeo);
            dbProvider.AddInParameter(command, helper.Confindiporcatencioneo, DbType.Int32, entity.Confindiporcatencioneo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EpoConfiguraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Confplazorevcoesporv, DbType.Int32, entity.Confplazorevcoesporv);
            dbProvider.AddInParameter(command, helper.Confplazorevcoesvenc, DbType.Int32, entity.Confplazorevcoesvenc);
            dbProvider.AddInParameter(command, helper.Confplazolevobsporv, DbType.Int32, entity.Confplazolevobsporv);
            dbProvider.AddInParameter(command, helper.Confplazolevobsvenc, DbType.Int32, entity.Confplazolevobsvenc);
            dbProvider.AddInParameter(command, helper.Confplazoalcancesvenc, DbType.Int32, entity.Confplazoalcancesvenc);
            dbProvider.AddInParameter(command, helper.Confplazoverificacionvenc, DbType.Int32, entity.Confplazoverificacionvenc);
            dbProvider.AddInParameter(command, helper.Confplazorevterceroinvporv, DbType.Int32, entity.Confplazorevterceroinvporv);
            dbProvider.AddInParameter(command, helper.Confplazorevterceroinvvenc, DbType.Int32, entity.Confplazorevterceroinvvenc);
            dbProvider.AddInParameter(command, helper.Confplazoenvestterceroinvporv, DbType.Int32, entity.Confplazoenvestterceroinvporv);
            dbProvider.AddInParameter(command, helper.Confplazoenvestterceroinvvenc, DbType.Int32, entity.Confplazoenvestterceroinvvenc);
            dbProvider.AddInParameter(command, helper.Confindigestionsnpepo, DbType.Int32, entity.Confindigestionsnpepo);
            dbProvider.AddInParameter(command, helper.Confindiporcatencionepo, DbType.Int32, entity.Confindiporcatencionepo);
            dbProvider.AddInParameter(command, helper.Confindigestionsnpeo, DbType.Int32, entity.Confindigestionsnpeo);
            dbProvider.AddInParameter(command, helper.Confindiporcatencioneo, DbType.Int32, entity.Confindiporcatencioneo);
            dbProvider.AddInParameter(command, helper.Confcodi, DbType.Int32, entity.Confcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int confcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Confcodi, DbType.Int32, confcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EpoConfiguraDTO GetById(int confcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Confcodi, DbType.Int32, confcodi);
            EpoConfiguraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EpoConfiguraDTO> List()
        {
            List<EpoConfiguraDTO> entitys = new List<EpoConfiguraDTO>();
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

        public List<EpoConfiguraDTO> GetByCriteria()
        {
            List<EpoConfiguraDTO> entitys = new List<EpoConfiguraDTO>();
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
    }
}
