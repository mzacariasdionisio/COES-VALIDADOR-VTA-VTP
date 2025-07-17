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
    /// Clase de acceso a datos de la tabla SI_MIGRAPARAMETRO
    /// </summary>
    public class SiMigraparametroRepository : RepositoryBase, ISiMigraparametroRepository
    {
        public SiMigraparametroRepository(string strConn) : base(strConn)
        {
        }

        SiMigraparametroHelper helper = new SiMigraparametroHelper();

        public int Save(SiMigraParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Migparcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Migparnomb, DbType.String, entity.Migparnomb);
            dbProvider.AddInParameter(command, helper.Migpartipo, DbType.Int32, entity.Migpartipo);
            dbProvider.AddInParameter(command, helper.Migpardesc, DbType.String, entity.Migpardesc);
            dbProvider.AddInParameter(command, helper.Migparusucreacion, DbType.String, entity.Migparusucreacion);
            dbProvider.AddInParameter(command, helper.Migparfeccreacion, DbType.DateTime, entity.Migparfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiMigraParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Migparnomb, DbType.String, entity.Migparnomb);
            dbProvider.AddInParameter(command, helper.Migpartipo, DbType.Int32, entity.Migpartipo);
            dbProvider.AddInParameter(command, helper.Migpardesc, DbType.String, entity.Migpardesc);
            dbProvider.AddInParameter(command, helper.Migparusucreacion, DbType.String, entity.Migparusucreacion);
            dbProvider.AddInParameter(command, helper.Migparfeccreacion, DbType.DateTime, entity.Migparfeccreacion);

            dbProvider.AddInParameter(command, helper.Migparcodi, DbType.Int32, entity.Migparcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int migparcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Migparcodi, DbType.Int32, migparcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigraParametroDTO GetById(int migparcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Migparcodi, DbType.Int32, migparcodi);
            SiMigraParametroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigraParametroDTO> List()
        {
            List<SiMigraParametroDTO> entitys = new List<SiMigraParametroDTO>();
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

        public List<SiMigraParametroDTO> GetByCriteria()
        {
            List<SiMigraParametroDTO> entitys = new List<SiMigraParametroDTO>();
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

        public List<SiMigraParametroDTO> ObtenerByTipoOperacion(int idTipoOperacionMigracion)
        {

            List<SiMigraParametroDTO> entitys = new List<SiMigraParametroDTO>();
            string strComando = string.Format(helper.SqlObtenerByTipoOperacion, idTipoOperacionMigracion);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiMigraParametroDTO entity = helper.Create(dr);

                    int iMiqubacodi = dr.GetOrdinal(helper.Miqubacodi);
                    if (!dr.IsDBNull(iMiqubacodi)) entity.Miqubacodi = Convert.ToInt32(dr.GetValue(iMiqubacodi));

                    int iMiqubanomtabla = dr.GetOrdinal(helper.Miqubanomtabla);
                    if (!dr.IsDBNull(iMiqubanomtabla)) entity.Miqubanomtabla = Convert.ToString(dr.GetValue(iMiqubanomtabla));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
