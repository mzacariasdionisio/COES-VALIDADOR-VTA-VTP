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
    /// Clase de acceso a datos de la tabla PR_CONGESTION_GRUPO
    /// </summary>
    public class PrCongestionGrupoRepository: RepositoryBase, IPrCongestionGrupoRepository
    {
        public PrCongestionGrupoRepository(string strConn): base(strConn)
        {
        }

        PrCongestionGrupoHelper helper = new PrCongestionGrupoHelper();

        public int Save(PrCongestionGrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Congrpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Congescodi, DbType.Int32, entity.Congescodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrCongestionGrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Congescodi, DbType.Int32, entity.Congescodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Congrpcodi, DbType.Int32, entity.Congrpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int congrpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Congescodi, DbType.Int32, congrpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrCongestionGrupoDTO GetById(int congrpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Congrpcodi, DbType.Int32, congrpcodi);
            PrCongestionGrupoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrCongestionGrupoDTO> List()
        {
            List<PrCongestionGrupoDTO> entitys = new List<PrCongestionGrupoDTO>();
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

        public List<PrCongestionGrupoDTO> GetByCriteria(int congescodi)
        {
            List<PrCongestionGrupoDTO> entitys = new List<PrCongestionGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Congescodi, DbType.Int32, congescodi);

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
