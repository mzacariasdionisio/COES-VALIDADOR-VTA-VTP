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
    /// Clase de acceso a datos de la tabla WB_EVENTOAGENDA
    /// </summary>
    public class WbEventoagendaRepository: RepositoryBase, IWbEventoagendaRepository
    {
        public WbEventoagendaRepository(string strConn): base(strConn)
        {
        }

        WbEventoagendaHelper helper = new WbEventoagendaHelper();

        public int Save(WbEventoagendaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Eveagcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Eveagtipo, DbType.Int32, entity.Eveagtipo);
            dbProvider.AddInParameter(command, helper.Eveagfechinicio, DbType.DateTime, entity.Eveagfechinicio);
            dbProvider.AddInParameter(command, helper.Eveagfechfin, DbType.DateTime, entity.Eveagfechfin);
            dbProvider.AddInParameter(command, helper.Eveagubicacion, DbType.String, entity.Eveagubicacion);
            dbProvider.AddInParameter(command, helper.Eveagextension, DbType.String, entity.Eveagextension);
            dbProvider.AddInParameter(command, helper.Eveagusuariocreacion, DbType.String, entity.Eveagusuariocreacion);
            dbProvider.AddInParameter(command, helper.Eveagfechacreacion, DbType.DateTime, entity.Eveagfechacreacion);
            dbProvider.AddInParameter(command, helper.Eveagusuarioupdate, DbType.String, entity.Eveagusuarioupdate);
            dbProvider.AddInParameter(command, helper.Eveagfechaupdate, DbType.DateTime, entity.Eveagfechaupdate);
            dbProvider.AddInParameter(command, helper.Eveagtitulo, DbType.String, entity.Eveagtitulo);
            dbProvider.AddInParameter(command, helper.Eveagdescripcion, DbType.String, entity.Eveagdescripcion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbEventoagendaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Eveagtipo, DbType.Int32, entity.Eveagtipo);
            dbProvider.AddInParameter(command, helper.Eveagfechinicio, DbType.DateTime, entity.Eveagfechinicio);
            dbProvider.AddInParameter(command, helper.Eveagfechfin, DbType.DateTime, entity.Eveagfechfin);
            dbProvider.AddInParameter(command, helper.Eveagubicacion, DbType.String, entity.Eveagubicacion);
            dbProvider.AddInParameter(command, helper.Eveagextension, DbType.String, entity.Eveagextension);
            dbProvider.AddInParameter(command, helper.Eveagusuarioupdate, DbType.String, entity.Eveagusuarioupdate);
            dbProvider.AddInParameter(command, helper.Eveagfechaupdate, DbType.DateTime, entity.Eveagfechaupdate);
            dbProvider.AddInParameter(command, helper.Eveagtitulo, DbType.String, entity.Eveagtitulo);
            dbProvider.AddInParameter(command, helper.Eveagdescripcion, DbType.String, entity.Eveagdescripcion);
            dbProvider.AddInParameter(command, helper.Eveagcodi, DbType.Int32, entity.Eveagcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int eveagcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Eveagcodi, DbType.Int32, eveagcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbEventoagendaDTO GetById(int eveagcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Eveagcodi, DbType.Int32, eveagcodi);
            WbEventoagendaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbEventoagendaDTO> List(int tipoEvento, string anio)
        {
            List<WbEventoagendaDTO> entitys = new List<WbEventoagendaDTO>();
            string query = String.Format(helper.SqlList, anio);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.Eveagtipo, DbType.Int32, tipoEvento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<WbEventoagendaDTO> GetByCriteria(int tipoEvento, DateTime fecha)
        {
            List<WbEventoagendaDTO> entitys = new List<WbEventoagendaDTO>();
            string query = String.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.Eveagtipo, DbType.Int32, tipoEvento);

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
