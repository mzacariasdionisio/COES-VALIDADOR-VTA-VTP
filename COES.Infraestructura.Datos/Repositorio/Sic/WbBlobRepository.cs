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
    /// Clase de acceso a datos de la tabla WB_BLOB
    /// </summary>
    public class WbBlobRepository : RepositoryBase, IWbBlobRepository
    {
        public WbBlobRepository(string strConn) : base(strConn)
        {
        }

        WbBlobHelper helper = new WbBlobHelper();

        public int Save(WbBlobDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Blobcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Bloburl, DbType.String, entity.Bloburl);
            dbProvider.AddInParameter(command, helper.Padrecodi, DbType.Int32, entity.Padrecodi);
            dbProvider.AddInParameter(command, helper.Blobname, DbType.String, entity.Blobname);
            dbProvider.AddInParameter(command, helper.Blobsize, DbType.String, entity.Blobsize);
            dbProvider.AddInParameter(command, helper.Blobdatecreated, DbType.DateTime, entity.Blobdatecreated);
            dbProvider.AddInParameter(command, helper.Blobusercreate, DbType.String, entity.Blobusercreate);
            dbProvider.AddInParameter(command, helper.Blobdateupdate, DbType.DateTime, entity.Blobdateupdate);
            dbProvider.AddInParameter(command, helper.Blobuserupdate, DbType.String, entity.Blobuserupdate);
            dbProvider.AddInParameter(command, helper.Blobstate, DbType.String, entity.Blobstate);
            dbProvider.AddInParameter(command, helper.Blobtype, DbType.String, entity.Blobtype);
            dbProvider.AddInParameter(command, helper.Blobfoldertype, DbType.String, entity.Blobfoldertype);
            dbProvider.AddInParameter(command, helper.Blobissuu, DbType.String, entity.Blobissuu);
            dbProvider.AddInParameter(command, helper.Blobissuulink, DbType.String, entity.Blobissuulink);
            dbProvider.AddInParameter(command, helper.Blobissuupos, DbType.String, entity.Blobissuupos);
            dbProvider.AddInParameter(command, helper.Blobissuulenx, DbType.String, entity.Blobissuulenx);
            dbProvider.AddInParameter(command, helper.Blobissuuleny, DbType.String, entity.Blobissuuleny);
            dbProvider.AddInParameter(command, helper.Blobhiddcol, DbType.String, entity.Blobhiddcol);
            dbProvider.AddInParameter(command, helper.Blobbreadname, DbType.String, entity.Blobbreadname);
            dbProvider.AddInParameter(command, helper.Bloborderfolder, DbType.String, entity.Bloborderfolder);
            dbProvider.AddInParameter(command, helper.Blobhide, DbType.String, entity.Blobhide);
            dbProvider.AddInParameter(command, helper.Indtree, DbType.String, entity.Indtree);
            dbProvider.AddInParameter(command, helper.Blobtreepadre, DbType.Int32, entity.Blobtreepadre);
            dbProvider.AddInParameter(command, helper.Blobfuente, DbType.Int32, entity.Blobfuente);
            dbProvider.AddInParameter(command, helper.Blofuecodi, DbType.Int32, entity.Blofuecodi);
            dbProvider.AddInParameter(command, helper.Blobconfidencial, DbType.Int32, entity.Blobconfidencial);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbBlobDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Blobcodi, DbType.Int32, entity.Blobcodi);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Bloburl, DbType.String, entity.Bloburl);
            dbProvider.AddInParameter(command, helper.Padrecodi, DbType.Int32, entity.Padrecodi);
            dbProvider.AddInParameter(command, helper.Blobname, DbType.String, entity.Blobname);
            dbProvider.AddInParameter(command, helper.Blobsize, DbType.String, entity.Blobsize);
            dbProvider.AddInParameter(command, helper.Blobdatecreated, DbType.DateTime, entity.Blobdatecreated);
            dbProvider.AddInParameter(command, helper.Blobusercreate, DbType.String, entity.Blobusercreate);
            dbProvider.AddInParameter(command, helper.Blobdateupdate, DbType.DateTime, entity.Blobdateupdate);
            dbProvider.AddInParameter(command, helper.Blobuserupdate, DbType.String, entity.Blobuserupdate);
            dbProvider.AddInParameter(command, helper.Blobstate, DbType.String, entity.Blobstate);
            dbProvider.AddInParameter(command, helper.Blobtype, DbType.String, entity.Blobtype);
            dbProvider.AddInParameter(command, helper.Blobfoldertype, DbType.String, entity.Blobfoldertype);
            dbProvider.AddInParameter(command, helper.Blobissuu, DbType.String, entity.Blobissuu);
            dbProvider.AddInParameter(command, helper.Blobissuulink, DbType.String, entity.Blobissuulink);
            dbProvider.AddInParameter(command, helper.Blobissuupos, DbType.String, entity.Blobissuupos);
            dbProvider.AddInParameter(command, helper.Blobissuulenx, DbType.String, entity.Blobissuulenx);
            dbProvider.AddInParameter(command, helper.Blobissuuleny, DbType.String, entity.Blobissuuleny);
            dbProvider.AddInParameter(command, helper.Blobhiddcol, DbType.String, entity.Blobhiddcol);
            dbProvider.AddInParameter(command, helper.Blobbreadname, DbType.String, entity.Blobbreadname);
            dbProvider.AddInParameter(command, helper.Bloborderfolder, DbType.String, entity.Bloborderfolder);
            dbProvider.AddInParameter(command, helper.Blobhide, DbType.String, entity.Blobhide);
            dbProvider.AddInParameter(command, helper.Indtree, DbType.String, entity.Indtree);
            dbProvider.AddInParameter(command, helper.Blobtreepadre, DbType.Int32, entity.Blobtreepadre);
            dbProvider.AddInParameter(command, helper.Blobfuente, DbType.Int32, entity.Blobfuente);
            dbProvider.AddInParameter(command, helper.Blofuecodi, DbType.Int32, entity.Blofuecodi);
            dbProvider.AddInParameter(command, helper.Blobconfidencial, DbType.Int32, entity.Blobconfidencial);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int blobcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Blobcodi, DbType.Int32, blobcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbBlobDTO GetById(int blobcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Blobcodi, DbType.Int32, blobcodi);
            WbBlobDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public WbBlobDTO ObtenerBlobByUrl(string url)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerBlobByUrl);

            dbProvider.AddInParameter(command, helper.Bloburl, DbType.String, url);
            WbBlobDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public WbBlobDTO ObtenerBlobByUrl2(string url, int idFuente)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerBlobByUrl2);

            dbProvider.AddInParameter(command, helper.Bloburl, DbType.String, url);
            dbProvider.AddInParameter(command, helper.Blobcodi, DbType.Int32, idFuente);
            WbBlobDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public WbBlobDTO ObtenerPorPadre(int blobcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorPadre);

            dbProvider.AddInParameter(command, helper.Blobcodi, DbType.Int32, blobcodi);
            WbBlobDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbBlobDTO> List()
        {
            List<WbBlobDTO> entitys = new List<WbBlobDTO>();
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

        public List<WbBlobDTO> GetByCriteria()
        {
            List<WbBlobDTO> entitys = new List<WbBlobDTO>();
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

        public List <WbBlobDTO> ObtenerByUrlParcial(string url,DateTime fechaInicio, DateTime fechaFin)
        {
            List<WbBlobDTO> entitys = new List<WbBlobDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCodiPadre);
            
            dbProvider.AddInParameter(command, "fechaInicio", DbType.Date, fechaInicio);
            dbProvider.AddInParameter(command, "fechaFin", DbType.Date, fechaFin);
            dbProvider.AddInParameter(command, "ruta", DbType.String, url);

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
