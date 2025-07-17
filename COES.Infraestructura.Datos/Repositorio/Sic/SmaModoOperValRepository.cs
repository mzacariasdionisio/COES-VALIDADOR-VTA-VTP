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
    /// Clase de acceso a datos de la tabla SMA_MODO_OPER_VAL
    /// </summary>
    public class SmaModoOperValRepository: RepositoryBase, ISmaModoOperValRepository
    {
        public SmaModoOperValRepository(string strConn): base(strConn)
        {
        }

        SmaModoOperValHelper helper = new SmaModoOperValHelper();

        public int Save(SmaModoOperValDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mopvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mopvusucreacion, DbType.String, entity.Mopvusucreacion);
            dbProvider.AddInParameter(command, helper.Mopvgrupoval, DbType.Int32, entity.Mopvgrupoval);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int GetNumVal()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetNumVal);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            return id;
        }

        public void Update(SmaModoOperValDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Mopvusumodificacion, DbType.String, entity.Mopvusumodificacion);
            dbProvider.AddInParameter(command, helper.Mopvcodi, DbType.Int32, entity.Mopvcodi);

            dbProvider.ExecuteNonQuery(command);
        }


        public void Delete(string user, int mopvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mopvusumodificacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Mopvcodi, DbType.Int32, mopvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaModoOperValDTO GetById(int mopvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mopvcodi, DbType.Int32, mopvcodi);
            SmaModoOperValDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaModoOperValDTO> List(string grupocodi)
        {
            List<SmaModoOperValDTO> entitys = new List<SmaModoOperValDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.String, grupocodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateList(dr));
                }
            }

            return entitys;
        }

        public List<SmaModoOperValDTO> ListMOVal(int? mopvgrupoval, int urscodi)
        {
            List<SmaModoOperValDTO> entitys = new List<SmaModoOperValDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMOVal);

            dbProvider.AddInParameter(command, helper.Urscodi, DbType.Int32, urscodi);
            dbProvider.AddInParameter(command, helper.Mopvgrupoval, DbType.Int32, mopvgrupoval);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListMOVal(dr));
                }
            }

            return entitys;
        }


        public List<SmaModoOperValDTO> ListAll()
        {
            List<SmaModoOperValDTO> entitys = new List<SmaModoOperValDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAll);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SmaModoOperValDTO> GetByCriteria()
        {
            List<SmaModoOperValDTO> entitys = new List<SmaModoOperValDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateList(dr));
                }
            }

            return entitys;
        }

        public List<SmaModoOperValDTO> GetListMOValxUrs(int urscodi)
        {
            List<SmaModoOperValDTO> entitys = new List<SmaModoOperValDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetListMOValxUrs);
            dbProvider.AddInParameter(command, helper.Urscodi, DbType.Int32, urscodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateMOValxUrs(dr));
                }
            }

            return entitys;
        }

    
    }
}
