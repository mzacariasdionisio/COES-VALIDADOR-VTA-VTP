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
    /// Clase de acceso a datos de la tabla SI_TIPOINFORMACION
    /// </summary>
    public class SiTipoinformacionRepository: RepositoryBase, ISiTipoinformacionRepository
    {
        public SiTipoinformacionRepository(string strConn): base(strConn)
        {
        }

        SiTipoinformacionHelper helper = new SiTipoinformacionHelper();

        public void Save(SiTipoinformacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tipoinfoabrev, DbType.String, entity.Tipoinfoabrev);
            dbProvider.AddInParameter(command, helper.Tipoinfodesc, DbType.String, entity.Tipoinfodesc);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Tipoinfoabrev, DbType.String, entity.Tipoinfoabrev);
            dbProvider.AddInParameter(command, helper.Tipoinfodesc, DbType.String, entity.Tipoinfodesc);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(SiTipoinformacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tipoinfoabrev, DbType.String, entity.Tipoinfoabrev);
            dbProvider.AddInParameter(command, helper.Tipoinfodesc, DbType.String, entity.Tipoinfodesc);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Tipoinfoabrev, DbType.String, entity.Tipoinfoabrev);
            dbProvider.AddInParameter(command, helper.Tipoinfodesc, DbType.String, entity.Tipoinfodesc);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tipoinfocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiTipoinformacionDTO GetById(int tipoinfocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            SiTipoinformacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiTipoinformacionDTO> List()
        {
            List<SiTipoinformacionDTO> entitys = new List<SiTipoinformacionDTO>();
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

        public List<SiTipoinformacionDTO> GetByCriteria(string tipoinfocodi)
        {
            List<SiTipoinformacionDTO> entitys = new List<SiTipoinformacionDTO>();
            string query = string.Format(helper.SqlGetByCriteria, tipoinfocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
