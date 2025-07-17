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
    public class WbComiteContactoRepository: RepositoryBase, IWbComiteContactoRepository
    {
        public WbComiteContactoRepository(string strConn): base(strConn)
        {
        }

        WbComiteContactoHelper helper = new WbComiteContactoHelper();

        public void Save(WbComiteContactoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, entity.Contaccodi);
            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, entity.Comitecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(WbComiteContactoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, entity.Contaccodi);
            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, entity.Comitecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int contaccodi, int comitecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, contaccodi);
            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, comitecodi);
            dbProvider.ExecuteNonQuery(command);
        }


        public WbComiteContactoDTO GetById(int contaccodi, int comitecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, contaccodi);
            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, comitecodi);
            WbComiteContactoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbComiteContactoDTO> List()
        {
            List<WbComiteContactoDTO> entitys = new List<WbComiteContactoDTO>();
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

        public List<WbComiteContactoDTO> GetByCriteria(int idContacto)
        {
            List<WbComiteContactoDTO> entitys = new List<WbComiteContactoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, idContacto);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entitys.Add(helper.Create(dr));
                    WbComiteContactoDTO entity = new WbComiteContactoDTO();

                    int iComitecodi = dr.GetOrdinal(helper.Comitecodi);
                    if (!dr.IsDBNull(iComitecodi)) entity.Comitecodi = Convert.ToInt32(dr.GetValue(iComitecodi));

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
