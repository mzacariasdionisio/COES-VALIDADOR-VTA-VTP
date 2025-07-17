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
    /// Clase de acceso a datos de la tabla WB_PROCESO_CONTACTO
    /// </summary>
    public class WbProcesoContactoRepository : RepositoryBase, IWbProcesoContactoRepository
    {
        public WbProcesoContactoRepository(string strConn) : base(strConn)
        {
        }

        WbProcesoContactoHelper helper = new WbProcesoContactoHelper();

        public void Save(WbProcesoContactoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, entity.Contaccodi);
            dbProvider.AddInParameter(command, helper.Procesocodi, DbType.Int32, entity.Procesocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(WbProcesoContactoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, entity.Contaccodi);
            dbProvider.AddInParameter(command, helper.Procesocodi, DbType.Int32, entity.Procesocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int contaccodi, int procesocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, contaccodi);
            dbProvider.AddInParameter(command, helper.Procesocodi, DbType.Int32, procesocodi);
            dbProvider.ExecuteNonQuery(command);
        }


        public WbProcesoContactoDTO GetById(int contaccodi, int procesocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, contaccodi);
            dbProvider.AddInParameter(command, helper.Procesocodi, DbType.Int32, procesocodi);
            WbProcesoContactoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbProcesoContactoDTO> List()
        {
            List<WbProcesoContactoDTO> entitys = new List<WbProcesoContactoDTO>();
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

        public List<WbProcesoContactoDTO> GetByCriteria(int idContacto)
        {
            List<WbProcesoContactoDTO> entitys = new List<WbProcesoContactoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, idContacto);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entitys.Add(helper.Create(dr));
                    WbProcesoContactoDTO entity = new WbProcesoContactoDTO();

                    int iProcesocodi = dr.GetOrdinal(helper.Procesocodi);
                    if (!dr.IsDBNull(iProcesocodi)) entity.Procesocodi = Convert.ToInt32(dr.GetValue(iProcesocodi));

                    int iDescomite = dr.GetOrdinal(helper.Descomite);
                    if (!dr.IsDBNull(iDescomite)) entity.Descomite = dr.GetString(iDescomite);

                    int iIndicador = dr.GetOrdinal(helper.Indicador);
                    if (!dr.IsDBNull(iIndicador)) entity.Indicador = Convert.ToInt32(dr.GetValue(iIndicador));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
