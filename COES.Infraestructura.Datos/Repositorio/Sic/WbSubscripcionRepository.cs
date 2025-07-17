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
    /// Clase de acceso a datos de la tabla WB_SUBSCRIPCION
    /// </summary>
    public class WbSubscripcionRepository: RepositoryBase, IWbSubscripcionRepository
    {
        public WbSubscripcionRepository(string strConn): base(strConn)
        {
        }

        WbSubscripcionHelper helper = new WbSubscripcionHelper();

        public int Save(WbSubscripcionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Subscripcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Subscripnombres, DbType.String, entity.Subscripnombres);
            dbProvider.AddInParameter(command, helper.Subscripapellidos, DbType.String, entity.Subscripapellidos);
            dbProvider.AddInParameter(command, helper.Subscripemail, DbType.String, entity.Subscripemail);
            dbProvider.AddInParameter(command, helper.Subscriptelefono, DbType.String, entity.Subscriptelefono);
            dbProvider.AddInParameter(command, helper.Subscripempresa, DbType.String, entity.Subscripempresa);
            dbProvider.AddInParameter(command, helper.Subscripestado, DbType.String, entity.Subscripestado);
            dbProvider.AddInParameter(command, helper.Subscripfecha, DbType.DateTime, entity.Subscripfecha);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbSubscripcionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Subscripnombres, DbType.String, entity.Subscripnombres);
            dbProvider.AddInParameter(command, helper.Subscripapellidos, DbType.String, entity.Subscripapellidos);
            dbProvider.AddInParameter(command, helper.Subscripemail, DbType.String, entity.Subscripemail);
            dbProvider.AddInParameter(command, helper.Subscriptelefono, DbType.String, entity.Subscriptelefono);
            dbProvider.AddInParameter(command, helper.Subscripempresa, DbType.String, entity.Subscripempresa);
            dbProvider.AddInParameter(command, helper.Subscripestado, DbType.String, entity.Subscripestado);
            dbProvider.AddInParameter(command, helper.Subscripfecha, DbType.DateTime, entity.Subscripfecha);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Subscripcodi, DbType.Int32, entity.Subscripcodi);


            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int subscripcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Subscripcodi, DbType.Int32, subscripcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbSubscripcionDTO GetById(int subscripcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Subscripcodi, DbType.Int32, subscripcodi);
            WbSubscripcionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbSubscripcionDTO> List()
        {
            List<WbSubscripcionDTO> entitys = new List<WbSubscripcionDTO>();
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

        public List<WbSubscripcionDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, int? idPublicacion)
        {
            if (idPublicacion == null) idPublicacion = -1;
            List<WbSubscripcionDTO> entitys = new List<WbSubscripcionDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), idPublicacion);

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

        public List<WbSubscripcionDTO> ObtenerExportacion(DateTime fechaInicio, DateTime fechaFin, int? idPublicacion)
        {
            if (idPublicacion == null) idPublicacion = -1;
            List<WbSubscripcionDTO> entitys = new List<WbSubscripcionDTO>();
            string sql = string.Format(helper.SqlObtenerExportacion, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), idPublicacion);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbSubscripcionDTO entity = helper.Create(dr);

                    int iPublicname = dr.GetOrdinal(helper.Publicname);
                    if (!dr.IsDBNull(iPublicname)) entity.Publicname = dr.GetString(iPublicname);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
