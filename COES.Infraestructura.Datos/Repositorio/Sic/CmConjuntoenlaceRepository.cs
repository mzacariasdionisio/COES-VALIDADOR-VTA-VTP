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
    /// Clase de acceso a datos de la tabla CM_CONJUNTOENLACE
    /// </summary>
    public class CmConjuntoenlaceRepository: RepositoryBase, ICmConjuntoenlaceRepository
    {
        public CmConjuntoenlaceRepository(string strConn): base(strConn)
        {
        }

        CmConjuntoenlaceHelper helper = new CmConjuntoenlaceHelper();

        public int Save(CmConjuntoenlaceDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cnjenlcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, entity.Grulincodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmConjuntoenlaceDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, entity.Grulincodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Cnjenlcodi, DbType.Int32, entity.Cnjenlcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int idGrupo, int idLinea)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, idGrupo);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, idLinea);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmConjuntoenlaceDTO GetById(int cnjenlcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cnjenlcodi, DbType.Int32, cnjenlcodi);
            CmConjuntoenlaceDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmConjuntoenlaceDTO> List()
        {
            List<CmConjuntoenlaceDTO> entitys = new List<CmConjuntoenlaceDTO>();
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

        public List<CmConjuntoenlaceDTO> GetByCriteria(int idGrupo, int idLinea)
        {
            List<CmConjuntoenlaceDTO> entitys = new List<CmConjuntoenlaceDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, idGrupo);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, idLinea);

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
