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
    /// Clase de acceso a datos de la tabla SI_HISEMPENTIDAD
    /// </summary>
    public class SiHisempentidadRepository : RepositoryBase, ISiHisempentidadRepository
    {
        public SiHisempentidadRepository(string strConn) : base(strConn)
        {
        }

        SiHisempentidadHelper helper = new SiHisempentidadHelper();

        public int Save(SiHisempentidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Hempencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Hempencampoid, DbType.String, entity.Hempencampoid);
            dbProvider.AddInParameter(command, helper.Hempentitulo, DbType.String, entity.Hempentitulo);
            dbProvider.AddInParameter(command, helper.Hempentablename, DbType.String, entity.Hempentablename);
            dbProvider.AddInParameter(command, helper.Hempencampodesc, DbType.String, entity.Hempencampodesc);
            dbProvider.AddInParameter(command, helper.Hempencampodesc2, DbType.String, entity.Hempencampodesc2);
            dbProvider.AddInParameter(command, helper.Hempencampoestado, DbType.String, entity.Hempencampoestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiHisempentidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Hempencodi, DbType.Int32, entity.Hempencodi);
            dbProvider.AddInParameter(command, helper.Hempencampoid, DbType.String, entity.Hempencampoid);
            dbProvider.AddInParameter(command, helper.Hempentitulo, DbType.String, entity.Hempentitulo);
            dbProvider.AddInParameter(command, helper.Hempentablename, DbType.String, entity.Hempentablename);
            dbProvider.AddInParameter(command, helper.Hempencampodesc, DbType.String, entity.Hempencampodesc);
            dbProvider.AddInParameter(command, helper.Hempencampodesc2, DbType.String, entity.Hempencampodesc2);
            dbProvider.AddInParameter(command, helper.Hempencampoestado, DbType.String, entity.Hempencampoestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hempencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hempencodi, DbType.Int32, hempencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiHisempentidadDTO GetById(int hempencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hempencodi, DbType.Int32, hempencodi);
            SiHisempentidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiHisempentidadDTO> List()
        {
            List<SiHisempentidadDTO> entitys = new List<SiHisempentidadDTO>();
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

        public List<SiHisempentidadDTO> GetByCriteria(int migracodi)
        {
            List<SiHisempentidadDTO> entitys = new List<SiHisempentidadDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, migracodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
