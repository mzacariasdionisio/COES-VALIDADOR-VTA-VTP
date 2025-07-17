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
    /// Clase de acceso a datos de la tabla WB_AYUDAAPP
    /// </summary>
    public class WbAyudaappRepository: RepositoryBase, IWbAyudaappRepository
    {
        public WbAyudaappRepository(string strConn): base(strConn)
        {
        }

        WbAyudaappHelper helper = new WbAyudaappHelper();

        public int Save(WbAyudaappDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ayuappcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ayuappcodigoventana, DbType.String, entity.Ayuappcodigoventana);
            dbProvider.AddInParameter(command, helper.Ayuappdescripcionventana, DbType.String, entity.Ayuappdescripcionventana);
            dbProvider.AddInParameter(command, helper.Ayuappmensaje, DbType.String, entity.Ayuappmensaje);
            dbProvider.AddInParameter(command, helper.Ayuappmensajeeng, DbType.String, entity.Ayuappmensajeeng);
            dbProvider.AddInParameter(command, helper.Ayuappestado, DbType.String, entity.Ayuappestado);
            dbProvider.AddInParameter(command, helper.Ayuappusucreacion, DbType.String, entity.Ayuappusucreacion);
            dbProvider.AddInParameter(command, helper.Ayuappfeccreacion, DbType.DateTime, entity.Ayuappfeccreacion);
            dbProvider.AddInParameter(command, helper.Ayuappusumodificacion, DbType.String, entity.Ayuappusumodificacion);
            dbProvider.AddInParameter(command, helper.Ayuappfecmodificacion, DbType.DateTime, entity.Ayuappfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbAyudaappDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);           
           
            dbProvider.AddInParameter(command, helper.Ayuappmensaje, DbType.String, entity.Ayuappmensaje);
            dbProvider.AddInParameter(command, helper.Ayuappmensajeeng, DbType.String, entity.Ayuappmensajeeng);
            dbProvider.AddInParameter(command, helper.Ayuappestado, DbType.String, entity.Ayuappestado);            
            dbProvider.AddInParameter(command, helper.Ayuappusumodificacion, DbType.String, entity.Ayuappusumodificacion);         
            dbProvider.AddInParameter(command, helper.Ayuappcodi, DbType.Int32, entity.Ayuappcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ayuappcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ayuappcodi, DbType.Int32, ayuappcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbAyudaappDTO GetById(int ayuappcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ayuappcodi, DbType.Int32, ayuappcodi);
            WbAyudaappDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbAyudaappDTO> List()
        {
            List<WbAyudaappDTO> entitys = new List<WbAyudaappDTO>();
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

        public List<WbAyudaappDTO> GetByCriteria()
        {
            List<WbAyudaappDTO> entitys = new List<WbAyudaappDTO>();
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
    }
}
