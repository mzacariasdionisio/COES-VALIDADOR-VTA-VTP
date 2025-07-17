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
    /// Clase de acceso a datos de la tabla SI_FACTORCONVERSION
    /// </summary>
    public class SiFactorconversionRepository : RepositoryBase, ISiFactorconversionRepository
    {
        public SiFactorconversionRepository(string strConn) : base(strConn)
        {
        }

        SiFactorconversionHelper helper = new SiFactorconversionHelper();

        public int Save(SiFactorconversionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tconvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tinforigen, DbType.Int32, entity.Tinforigen);
            dbProvider.AddInParameter(command, helper.Tinfdestino, DbType.Int32, entity.Tinfdestino);
            dbProvider.AddInParameter(command, helper.Tconvfactor, DbType.Decimal, entity.Tconvfactor);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiFactorconversionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tinforigen, DbType.Int32, entity.Tinforigen);
            dbProvider.AddInParameter(command, helper.Tinfdestino, DbType.Int32, entity.Tinfdestino);
            dbProvider.AddInParameter(command, helper.Tconvfactor, DbType.Decimal, entity.Tconvfactor);

            dbProvider.AddInParameter(command, helper.Tconvcodi, DbType.Int32, entity.Tconvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tconvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tconvcodi, DbType.Int32, tconvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiFactorconversionDTO GetById(int tconvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tconvcodi, DbType.Int32, tconvcodi);
            SiFactorconversionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iTinfdestinoabrev = dr.GetOrdinal(helper.Tinfdestinoabrev);
                    if (!dr.IsDBNull(iTinfdestinoabrev)) entity.Tinfdestinoabrev = dr.GetString(iTinfdestinoabrev);

                    int iTinforigenabrev = dr.GetOrdinal(helper.Tinforigenabrev);
                    if (!dr.IsDBNull(iTinforigenabrev)) entity.Tinforigenabrev = dr.GetString(iTinforigenabrev);

                    int iTinfdestinodesc = dr.GetOrdinal(helper.Tinfdestinodesc);
                    if (!dr.IsDBNull(iTinfdestinodesc)) entity.Tinfdestinodesc = dr.GetString(iTinfdestinodesc);

                    int iTinforigendesc = dr.GetOrdinal(helper.Tinforigendesc);
                    if (!dr.IsDBNull(iTinforigendesc)) entity.Tinforigendesc = dr.GetString(iTinforigendesc);
                }
            }

            return entity;
        }

        public List<SiFactorconversionDTO> List()
        {
            List<SiFactorconversionDTO> entitys = new List<SiFactorconversionDTO>();
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

        public List<SiFactorconversionDTO> GetByCriteria()
        {
            List<SiFactorconversionDTO> entitys = new List<SiFactorconversionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iTinfdestinoabrev = dr.GetOrdinal(helper.Tinfdestinoabrev);
                    if (!dr.IsDBNull(iTinfdestinoabrev)) entity.Tinfdestinoabrev = dr.GetString(iTinfdestinoabrev);

                    int iTinforigenabrev = dr.GetOrdinal(helper.Tinforigenabrev);
                    if (!dr.IsDBNull(iTinforigenabrev)) entity.Tinforigenabrev = dr.GetString(iTinforigenabrev);

                    int iTinfdestinodesc = dr.GetOrdinal(helper.Tinfdestinodesc);
                    if (!dr.IsDBNull(iTinfdestinodesc)) entity.Tinfdestinodesc = dr.GetString(iTinfdestinodesc);

                    int iTinforigendesc = dr.GetOrdinal(helper.Tinforigendesc);
                    if (!dr.IsDBNull(iTinforigendesc)) entity.Tinforigendesc = dr.GetString(iTinforigendesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
