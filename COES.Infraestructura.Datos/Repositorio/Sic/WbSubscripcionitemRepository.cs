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
    /// Clase de acceso a datos de la tabla WB_SUBSCRIPCIONITEM
    /// </summary>
    public class WbSubscripcionitemRepository: RepositoryBase, IWbSubscripcionitemRepository
    {
        public WbSubscripcionitemRepository(string strConn): base(strConn)
        {
        }

        WbSubscripcionitemHelper helper = new WbSubscripcionitemHelper();

        public void Save(WbSubscripcionitemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Subscripcodi, DbType.Int32, entity.Subscripcodi);
            dbProvider.AddInParameter(command, helper.Publiccodi, DbType.Int32, entity.Publiccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(WbSubscripcionitemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Subscripcodi, DbType.Int32, entity.Subscripcodi);
            dbProvider.AddInParameter(command, helper.Publiccodi, DbType.Int32, entity.Publiccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int subscripcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Subscripcodi, DbType.Int32, subscripcodi);            

            dbProvider.ExecuteNonQuery(command);
        }

        public WbSubscripcionitemDTO GetById(int subscripcodi, int publiccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Subscripcodi, DbType.Int32, subscripcodi);
            dbProvider.AddInParameter(command, helper.Publiccodi, DbType.Int32, publiccodi);
            WbSubscripcionitemDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbSubscripcionitemDTO> List()
        {
            List<WbSubscripcionitemDTO> entitys = new List<WbSubscripcionitemDTO>();
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

        public List<WbSubscripcionitemDTO> GetByCriteria(int idSubscripcion)
        {
            List<WbSubscripcionitemDTO> entitys = new List<WbSubscripcionitemDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Subscripcodi, DbType.Int32, idSubscripcion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbSubscripcionitemDTO entity = new WbSubscripcionitemDTO();

                    int iPubliccodi = dr.GetOrdinal(helper.Publiccodi);
                    if (!dr.IsDBNull(iPubliccodi)) entity.Publiccodi = Convert.ToInt32(dr.GetValue(iPubliccodi));

                    int iDespublicacion = dr.GetOrdinal(helper.Despublicacion);
                    if (!dr.IsDBNull(iDespublicacion)) entity.DesPublicacion = dr.GetString(iDespublicacion);

                    int iIndicador = dr.GetOrdinal(helper.Indicador);
                    if (!dr.IsDBNull(iIndicador)) entity.Indicador = Convert.ToInt32(dr.GetValue(iIndicador));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }       
    }
}
