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
    /// Clase de acceso a datos de la tabla SI_PRUEBA
    /// </summary>
    public class SiPruebaRepository : RepositoryBase, ISiPruebaRepository
    {
        public SiPruebaRepository(string strConn)
            : base(strConn)
        {
        }

        SiPruebaHelper helper = new SiPruebaHelper();



        public void Update(SiPruebaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pruebacodi, DbType.String, entity.Pruebacodi);
            dbProvider.AddInParameter(command, helper.Pruebanomb, DbType.String, entity.Pruebanomb);
            dbProvider.AddInParameter(command, helper.Pruebaest, DbType.String, entity.Pruebaest);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(string pruebacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pruebacodi, DbType.String, pruebacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiPruebaDTO GetById(string pruebacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pruebacodi, DbType.String, pruebacodi);
            SiPruebaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiPruebaDTO> List()
        {
            List<SiPruebaDTO> entitys = new List<SiPruebaDTO>();
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

        public List<SiPruebaDTO> GetByCriteria()
        {
            List<SiPruebaDTO> entitys = new List<SiPruebaDTO>();
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

        public List<SiPruebaDTO> BuscarPorNombre(string nombre)
        {
            List<SiPruebaDTO> entitys = new List<SiPruebaDTO>();

            string query = String.Format(helper.SqlBuscarPorNombre, nombre);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.Pruebanomb, DbType.String, nombre);

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
