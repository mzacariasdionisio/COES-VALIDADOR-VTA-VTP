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
    /// Clase de acceso a datos de la tabla IND_CUADRO
    /// </summary>
    public class IndCuadroRepository : RepositoryBase, IIndCuadroRepository
    {
        public IndCuadroRepository(string strConn) : base(strConn)
        {
        }

        IndCuadroHelper helper = new IndCuadroHelper();

        public int Save(IndCuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Icuacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Icuatitulo, DbType.String, entity.Icuatitulo);
            dbProvider.AddInParameter(command, helper.Icuanombre, DbType.String, entity.Icuanombre);
            dbProvider.AddInParameter(command, helper.Icuasubtitulo, DbType.String, entity.Icuasubtitulo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IndCuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Icuacodi, DbType.Int32, entity.Icuacodi);
            dbProvider.AddInParameter(command, helper.Icuatitulo, DbType.String, entity.Icuatitulo);
            dbProvider.AddInParameter(command, helper.Icuanombre, DbType.String, entity.Icuanombre);
            dbProvider.AddInParameter(command, helper.Icuasubtitulo, DbType.String, entity.Icuasubtitulo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int icuacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Icuacodi, DbType.Int32, icuacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndCuadroDTO GetById(int icuacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Icuacodi, DbType.Int32, icuacodi);
            IndCuadroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndCuadroDTO> List()
        {
            List<IndCuadroDTO> entitys = new List<IndCuadroDTO>();
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

        public List<IndCuadroDTO> GetByCriteria()
        {
            List<IndCuadroDTO> entitys = new List<IndCuadroDTO>();
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
