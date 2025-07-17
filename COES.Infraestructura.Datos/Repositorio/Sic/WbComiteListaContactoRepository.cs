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
    /// Clase de acceso a datos de la tabla WB_COMITE_CONTACTO
    /// </summary>
    public class WbComiteListaContactoRepository: RepositoryBase, IWbComiteListaContactoRepository
    {
        public WbComiteListaContactoRepository(string strConn): base(strConn)
        {
        }

        WbComiteListaContactoHelper helper = new WbComiteListaContactoHelper();

        public void Save(WbComiteListaContactoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.ComiteListacodi, DbType.Int32, entity.ComiteListacodi);
            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, entity.Contaccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(WbComiteListaContactoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, entity.Contaccodi);
            //dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, entity.ComiteCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int contaccodi, int comitelistacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, contaccodi);
            dbProvider.AddInParameter(command, helper.ComiteListacodi, DbType.Int32, comitelistacodi);
            dbProvider.ExecuteNonQuery(command);
        }


        public WbComiteListaContactoDTO GetById(int contaccodi, int comitecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, contaccodi);
            //dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, comitecodi);
            WbComiteListaContactoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbComiteListaContactoDTO> List()
        {
            List<WbComiteListaContactoDTO> entitys = new List<WbComiteListaContactoDTO>();
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

        public List<WbComiteListaContactoDTO> GetByCriteria(int idContacto)
        {
            List<WbComiteListaContactoDTO> entitys = new List<WbComiteListaContactoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, idContacto);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entitys.Add(helper.Create(dr));
                    WbComiteListaContactoDTO entity = new WbComiteListaContactoDTO();

                    int iComiteListacodi = dr.GetOrdinal(helper.ComiteListacodi);
                    if (!dr.IsDBNull(iComiteListacodi)) entity.ComiteListacodi = Convert.ToInt32(dr.GetValue(iComiteListacodi));

                    int iComitecodi = dr.GetOrdinal(helper.Comitecodi);
                    if (!dr.IsDBNull(iComitecodi)) entity.ComiteCodi = Convert.ToInt32(dr.GetValue(iComitecodi));

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
