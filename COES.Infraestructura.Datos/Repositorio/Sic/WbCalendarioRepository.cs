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
    /// Clase de acceso a datos de la tabla WB_CALENDARIO
    /// </summary>
    public class WbCalendarioRepository: RepositoryBase, IWbCalendarioRepository
    {
        public WbCalendarioRepository(string strConn): base(strConn)
        {
        }

        WbCalendarioHelper helper = new WbCalendarioHelper();

        public int Save(WbCalendarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Calendcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Calendicon, DbType.String, entity.Calendicon);
            dbProvider.AddInParameter(command, helper.Calendestado, DbType.String, entity.Calendestado);
            dbProvider.AddInParameter(command, helper.Calendusumodificacion, DbType.String, entity.Calendusumodificacion);
            dbProvider.AddInParameter(command, helper.Calendfecmodificacion, DbType.DateTime, entity.Calendfecmodificacion);
            dbProvider.AddInParameter(command, helper.Calenddescripcion, DbType.String, entity.Calenddescripcion);
            dbProvider.AddInParameter(command, helper.Calendtitulo, DbType.String, entity.Calendtitulo);
            dbProvider.AddInParameter(command, helper.Calendfecinicio, DbType.DateTime, entity.Calendfecinicio);
            dbProvider.AddInParameter(command, helper.Calendfecfin, DbType.DateTime, entity.Calendfecfin);
            dbProvider.AddInParameter(command, helper.Calendcolor, DbType.String, entity.Calendcolor);
            dbProvider.AddInParameter(command, helper.Calendtipo, DbType.String, entity.Calendtipo);
            dbProvider.AddInParameter(command, helper.Tipcalcodi, DbType.Int32, entity.Tipcalcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbCalendarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Calendicon, DbType.String, entity.Calendicon);
            dbProvider.AddInParameter(command, helper.Calendestado, DbType.String, entity.Calendestado);
            dbProvider.AddInParameter(command, helper.Calendusumodificacion, DbType.String, entity.Calendusumodificacion);
            dbProvider.AddInParameter(command, helper.Calendfecmodificacion, DbType.DateTime, entity.Calendfecmodificacion);
            dbProvider.AddInParameter(command, helper.Calenddescripcion, DbType.String, entity.Calenddescripcion);
            dbProvider.AddInParameter(command, helper.Calendtitulo, DbType.String, entity.Calendtitulo);
            dbProvider.AddInParameter(command, helper.Calendfecinicio, DbType.DateTime, entity.Calendfecinicio);
            dbProvider.AddInParameter(command, helper.Calendfecfin, DbType.DateTime, entity.Calendfecfin);
            dbProvider.AddInParameter(command, helper.Calendcolor, DbType.String, entity.Calendcolor);
            dbProvider.AddInParameter(command, helper.Calendtipo, DbType.String, entity.Calendtipo);
            dbProvider.AddInParameter(command, helper.Tipcalcodi, DbType.Int32, entity.Tipcalcodi);
            dbProvider.AddInParameter(command, helper.Calendcodi, DbType.Int32, entity.Calendcodi);            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int calendcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Calendcodi, DbType.Int32, calendcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbCalendarioDTO GetById(int calendcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Calendcodi, DbType.Int32, calendcodi);
            WbCalendarioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbCalendarioDTO> List()
        {
            List<WbCalendarioDTO> entitys = new List<WbCalendarioDTO>();
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

        public List<WbCalendarioDTO> GetByCriteria(string nombre, DateTime fechaInicio, DateTime fechaFin)
        {
            List<WbCalendarioDTO> entitys = new List<WbCalendarioDTO>();
            string query = string.Format(helper.SqlGetByCriteria, nombre, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
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
