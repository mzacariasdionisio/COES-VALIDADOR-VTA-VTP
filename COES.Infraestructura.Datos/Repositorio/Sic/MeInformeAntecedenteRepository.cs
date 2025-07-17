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
    /// Clase de acceso a datos de la tabla ME_INFORME_ANTECEDENTE
    /// </summary>
    public class MeInformeAntecedenteRepository: RepositoryBase, IMeInformeAntecedenteRepository
    {
        public MeInformeAntecedenteRepository(string strConn): base(strConn)
        {
        }

        MeInformeAntecedenteHelper helper = new MeInformeAntecedenteHelper();

        public int Save(MeInformeAntecedenteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Infantcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Infantorden, DbType.Int32, entity.Infantorden);
            dbProvider.AddInParameter(command, helper.Intantcontenido, DbType.String, entity.Intantcontenido);
            dbProvider.AddInParameter(command, helper.Intantestado, DbType.String, entity.Intantestado);
            dbProvider.AddInParameter(command, helper.Intantusucreacion, DbType.String, entity.Intantusucreacion);
            dbProvider.AddInParameter(command, helper.Intantfeccreacion, DbType.DateTime, entity.Intantfeccreacion);           

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeInformeAntecedenteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Infantorden, DbType.Int32, entity.Infantorden);
            dbProvider.AddInParameter(command, helper.Intantcontenido, DbType.String, entity.Intantcontenido);
            dbProvider.AddInParameter(command, helper.Intantestado, DbType.String, entity.Intantestado);            
            dbProvider.AddInParameter(command, helper.Intantusumodificacion, DbType.String, entity.Intantusumodificacion);
            dbProvider.AddInParameter(command, helper.Intantfecmodificacion, DbType.DateTime, entity.Intantfecmodificacion);
            dbProvider.AddInParameter(command, helper.Infantcodi, DbType.Int32, entity.Infantcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int infantcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Infantcodi, DbType.Int32, infantcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeInformeAntecedenteDTO GetById(int infantcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Infantcodi, DbType.Int32, infantcodi);
            MeInformeAntecedenteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeInformeAntecedenteDTO> List()
        {
            List<MeInformeAntecedenteDTO> entitys = new List<MeInformeAntecedenteDTO>();
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

        public List<MeInformeAntecedenteDTO> GetByCriteria()
        {
            List<MeInformeAntecedenteDTO> entitys = new List<MeInformeAntecedenteDTO>();
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
