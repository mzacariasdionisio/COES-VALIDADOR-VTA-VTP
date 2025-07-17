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
    /// Clase de acceso a datos de la tabla PFR_CUADRO
    /// </summary>
    public class PfrCuadroRepository: RepositoryBase, IPfrCuadroRepository
    {
        public PfrCuadroRepository(string strConn): base(strConn)
        {
        }

        PfrCuadroHelper helper = new PfrCuadroHelper();

        public int Save(PfrCuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfrcuacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfrcuanombre, DbType.String, entity.Pfrcuanombre);
            dbProvider.AddInParameter(command, helper.Pfrcuatitulo, DbType.String, entity.Pfrcuatitulo);
            dbProvider.AddInParameter(command, helper.Pfrcuasubtitulo, DbType.String, entity.Pfrcuasubtitulo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrCuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrcuacodi, DbType.Int32, entity.Pfrcuacodi);
            dbProvider.AddInParameter(command, helper.Pfrcuanombre, DbType.String, entity.Pfrcuanombre);
            dbProvider.AddInParameter(command, helper.Pfrcuatitulo, DbType.String, entity.Pfrcuatitulo);
            dbProvider.AddInParameter(command, helper.Pfrcuasubtitulo, DbType.String, entity.Pfrcuasubtitulo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrcuacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrcuacodi, DbType.Int32, pfrcuacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrCuadroDTO GetById(int pfrcuacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrcuacodi, DbType.Int32, pfrcuacodi);
            PfrCuadroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrCuadroDTO> List()
        {
            List<PfrCuadroDTO> entitys = new List<PfrCuadroDTO>();
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

        public List<PfrCuadroDTO> GetByCriteria()
        {
            List<PfrCuadroDTO> entitys = new List<PfrCuadroDTO>();
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
