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
    /// Clase de acceso a datos de la tabla PF_CUADRO
    /// </summary>
    public class PfCuadroRepository : RepositoryBase, IPfCuadroRepository
    {
        public PfCuadroRepository(string strConn) : base(strConn)
        {
        }

        PfCuadroHelper helper = new PfCuadroHelper();

        public int Save(PfCuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfcuacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfcuanombre, DbType.String, entity.Pfcuanombre);
            dbProvider.AddInParameter(command, helper.Pfcuatitulo, DbType.String, entity.Pfcuatitulo);
            dbProvider.AddInParameter(command, helper.Pfcuasubtitulo, DbType.String, entity.Pfcuasubtitulo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfCuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfcuacodi, DbType.Int32, entity.Pfcuacodi);
            dbProvider.AddInParameter(command, helper.Pfcuanombre, DbType.String, entity.Pfcuanombre);
            dbProvider.AddInParameter(command, helper.Pfcuatitulo, DbType.String, entity.Pfcuatitulo);
            dbProvider.AddInParameter(command, helper.Pfcuasubtitulo, DbType.String, entity.Pfcuasubtitulo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfcuacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfcuacodi, DbType.Int32, pfcuacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfCuadroDTO GetById(int pfcuacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfcuacodi, DbType.Int32, pfcuacodi);
            PfCuadroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfCuadroDTO> List()
        {
            List<PfCuadroDTO> entitys = new List<PfCuadroDTO>();
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

        public List<PfCuadroDTO> GetByCriteria()
        {
            List<PfCuadroDTO> entitys = new List<PfCuadroDTO>();
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
