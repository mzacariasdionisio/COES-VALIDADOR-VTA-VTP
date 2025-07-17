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
    /// Clase de acceso a datos de la tabla EPR_PROP_CATALOGO_DATA
    /// </summary>
    public class EprPropCatalogoDataRepository : RepositoryBase, IEprPropCatalogoDataRepository
    {
        public EprPropCatalogoDataRepository(string strConn) : base(strConn)
        {
        }

        EprPropCatalogoDataHelper helper = new EprPropCatalogoDataHelper();

        public int Save(EprPropCatalogoDataDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Eqcatdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Eqcatccodi, DbType.Int32, entity.Eqcatccodi);
            dbProvider.AddInParameter(command, helper.Eqcatdabrev, DbType.String, entity.Eqcatdabrev);
            dbProvider.AddInParameter(command, helper.Eqcatddescripcion, DbType.String, entity.Eqcatddescripcion);

            dbProvider.AddInParameter(command, helper.Eqcatdorden, DbType.Int32, entity.Eqcatdorden);
            dbProvider.AddInParameter(command, helper.Eqcatdvalor, DbType.Double, entity.Eqcatdvalor);
            dbProvider.AddInParameter(command, helper.Eqcatdnota, DbType.String, entity.Eqcatdnota);

            dbProvider.AddInParameter(command, helper.Eqcatdestregistro, DbType.String, entity.Eqcatdestregistro);
            dbProvider.AddInParameter(command, helper.Eqcatdfeccreacion, DbType.String, entity.Eqcatdfeccreacion);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EprPropCatalogoDataDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Eqcatccodi, DbType.Int32, entity.Eqcatccodi);
            dbProvider.AddInParameter(command, helper.Eqcatdabrev, DbType.String, entity.Eqcatdabrev);
            dbProvider.AddInParameter(command, helper.Eqcatddescripcion, DbType.String, entity.Eqcatddescripcion);
            dbProvider.AddInParameter(command, helper.Eqcatdorden, DbType.Int32, entity.Eqcatdorden);
            dbProvider.AddInParameter(command, helper.Eqcatdvalor, DbType.Double, entity.Eqcatdvalor);
            dbProvider.AddInParameter(command, helper.Eqcatdnota, DbType.String, entity.Eqcatdnota);
            dbProvider.AddInParameter(command, helper.Eqcatdestregistro, DbType.String, entity.Eqcatdestregistro);
            dbProvider.AddInParameter(command, helper.Eqcatdusumodificacion, DbType.String, entity.Eqcatdusumodificacion);
            dbProvider.AddInParameter(command, helper.Eqcatdcodi, DbType.Int32, entity.Eqcatdcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(EprPropCatalogoDataDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);
            dbProvider.AddInParameter(command, helper.Eqcatdestregistro, DbType.String, entity.Eqcatdestregistro);
            dbProvider.AddInParameter(command, helper.Eqcatdusumodificacion, DbType.String, entity.Eqcatdusumodificacion);
            dbProvider.AddInParameter(command, helper.Eqcatdcodi, DbType.Int32, entity.Eqcatdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EprPropCatalogoDataDTO GetById(int epareacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Eqcatdcodi, DbType.Int32, epareacodi);
            EprPropCatalogoDataDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EprPropCatalogoDataDTO> List(int Eqcatccodi)
        {
            List<EprPropCatalogoDataDTO> entitys = new List<EprPropCatalogoDataDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.List);
            dbProvider.AddInParameter(command, helper.Eqcatccodi, DbType.Int32, Eqcatccodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<EprPropCatalogoDataDTO> ListMaestroMarcaProteccion()
        {
            List<EprPropCatalogoDataDTO> entitys = new List<EprPropCatalogoDataDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaestroMarcaProteccion);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprPropCatalogoDataDTO entity = new EprPropCatalogoDataDTO();
                   
                    int iEqcatdabrev = dr.GetOrdinal(helper.Eqcatdabrev);
                    if (!dr.IsDBNull(iEqcatdabrev)) entity.Eqcatdabrev = dr.GetString(iEqcatdabrev);

                    int iEqcatddescripcion = dr.GetOrdinal(helper.Eqcatddescripcion);
                    if (!dr.IsDBNull(iEqcatddescripcion)) entity.Eqcatddescripcion = dr.GetString(iEqcatddescripcion);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
    }
}
