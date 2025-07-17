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
    /// Clase de acceso a datos de la tabla EN_ENSAYOMODO
    /// </summary>
    public class EnEnsayomodoRepository : RepositoryBase, IEnEnsayomodoRepository
    {
        public EnEnsayomodoRepository(string strConn)
            : base(strConn)
        {
        }

        EnEnsayomodoHelper helper = new EnEnsayomodoHelper();

        public int Save(EnEnsayomodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ensayocodi, DbType.Int32, entity.Ensayocodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);

            dbProvider.AddInParameter(command, helper.Enmodocodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EnEnsayomodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ensayocodi, DbType.Int32, entity.Ensayocodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Enmodocodi, DbType.Int32, entity.Enmodocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int enmodocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Enmodocodi, DbType.Int32, enmodocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EnEnsayomodoDTO GetById(int enmodocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Enmodocodi, DbType.Int32, enmodocodi);
            EnEnsayomodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EnEnsayomodoDTO> List()
        {
            List<EnEnsayomodoDTO> entitys = new List<EnEnsayomodoDTO>();
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

        public List<EnEnsayomodoDTO> GetByCriteria()
        {
            List<EnEnsayomodoDTO> entitys = new List<EnEnsayomodoDTO>();
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

        public List<EnEnsayomodoDTO> ListarEnsayosModo(int idEnsayo)
        {

            List<EnEnsayomodoDTO> entitys = new List<EnEnsayomodoDTO>();
            String sql = String.Format(helper.SqlListarEnsayoModo, idEnsayo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EnEnsayomodoDTO entity = new EnEnsayomodoDTO();

                    int iEnmodocodi = dr.GetOrdinal(helper.Enmodocodi);
                    if (!dr.IsDBNull(iEnmodocodi)) entity.Enmodocodi = Convert.ToInt32(dr.GetValue(iEnmodocodi));
                    int iEnsayocodi = dr.GetOrdinal(helper.Ensayocodi);
                    if (!dr.IsDBNull(iEnsayocodi)) entity.Ensayocodi = Convert.ToInt32(dr.GetValue(iEnsayocodi));
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }



    }
}
