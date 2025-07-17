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
    /// Clase de acceso a datos de la tabla RI_SOLICITUDDETALLE
    /// </summary>
    public class RiSolicituddetalleRepository : RepositoryBase, IRiSolicituddetalleRepository
    {
        public RiSolicituddetalleRepository(string strConn) : base(strConn)
        {
        }

        RiSolicituddetalleHelper helper = new RiSolicituddetalleHelper();

        public int Save(RiSolicituddetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Sdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Sdetvalor, DbType.String, entity.Sdetvalor);
            dbProvider.AddInParameter(command, helper.Sdetadjunto, DbType.String, entity.Sdetadjunto);
            dbProvider.AddInParameter(command, helper.Sdetvaloradjunto, DbType.String, entity.Sdetvaloradjunto);
            dbProvider.AddInParameter(command, helper.Solicodi, DbType.Int32, entity.Solicodi);
            dbProvider.AddInParameter(command, helper.Sdetusucreacion, DbType.String, entity.Sdetusucreacion);
            dbProvider.AddInParameter(command, helper.Sdetfeccreacion, DbType.DateTime, entity.Sdetfeccreacion);
            dbProvider.AddInParameter(command, helper.Sdetusumodificacion, DbType.String, entity.Sdetusumodificacion);
            dbProvider.AddInParameter(command, helper.Sdetfecmodificacion, DbType.DateTime, entity.Sdetfecmodificacion);
            dbProvider.AddInParameter(command, helper.Sdetcampo, DbType.String, entity.Sdetcampo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RiSolicituddetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Sdetvalor, DbType.String, entity.Sdetvalor);
            dbProvider.AddInParameter(command, helper.Sdetadjunto, DbType.String, entity.Sdetadjunto);
            dbProvider.AddInParameter(command, helper.Sdetvaloradjunto, DbType.String, entity.Sdetvaloradjunto);
            dbProvider.AddInParameter(command, helper.Solicodi, DbType.Int32, entity.Solicodi);
            dbProvider.AddInParameter(command, helper.Sdetusucreacion, DbType.String, entity.Sdetusucreacion);
            dbProvider.AddInParameter(command, helper.Sdetfeccreacion, DbType.DateTime, entity.Sdetfeccreacion);
            dbProvider.AddInParameter(command, helper.Sdetusumodificacion, DbType.String, entity.Sdetusumodificacion);
            dbProvider.AddInParameter(command, helper.Sdetfecmodificacion, DbType.DateTime, entity.Sdetfecmodificacion);
            dbProvider.AddInParameter(command, helper.Sdetcampo, DbType.String, entity.Sdetcampo);
            dbProvider.AddInParameter(command, helper.Sdetcodi, DbType.Int32, entity.Sdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int sdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Sdetcodi, DbType.Int32, sdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RiSolicituddetalleDTO GetById(int sdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Sdetcodi, DbType.Int32, sdetcodi);
            RiSolicituddetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RiSolicituddetalleDTO> List()
        {
            List<RiSolicituddetalleDTO> entitys = new List<RiSolicituddetalleDTO>();
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

        //

        public List<RiSolicituddetalleDTO> ListBySolicodi(int solicodi)
        {
            List<RiSolicituddetalleDTO> entitys = new List<RiSolicituddetalleDTO>();
            String query = String.Format(helper.SqlListBySoliCodi, solicodi);
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


        public List<RiSolicituddetalleDTO> GetByCriteria()
        {
            List<RiSolicituddetalleDTO> entitys = new List<RiSolicituddetalleDTO>();
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
