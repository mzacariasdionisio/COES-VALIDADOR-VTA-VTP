using COES.Base.Core;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Repositorio.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data.Common;
using System.Data;
using System.Globalization;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class WbNotificacionRepository : RepositoryBase, IWbNotificacionRepository
    {
        WbNotificacionHelper helper = new WbNotificacionHelper();
        private string strConexion;

        public WbNotificacionRepository(string strConn) : base(strConn)
        {
        }


        public void Delete(int notiCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.NotiCodi, DbType.Int32, notiCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<WbNotificacionDTO> GetByCriteria(string titulo, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<WbNotificacionDTO> entitys = new List<WbNotificacionDTO>();

            string query = string.Format(helper.SqlGetByCriteria, titulo,
               (fechaInicio != null) ? ((DateTime)fechaInicio).ToString(ConstantesBase.FormatoFecha) : " ",
               (fechaFin != null) ? ((DateTime)fechaFin).ToString(ConstantesBase.FormatoFecha) : " ");

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

        public WbNotificacionDTO GetById(int notiCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.NotiCodi, DbType.Int32, notiCodi);
            WbNotificacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbNotificacionDTO> List()
        {
            List<WbNotificacionDTO> entitys = new List<WbNotificacionDTO>();
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

        public int Save(WbNotificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.NotiCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.NotiTitulo, DbType.String, entity.NotiTitulo);
            dbProvider.AddInParameter(command, helper.NotiDescripcion, DbType.String, entity.NotiDescripcion);
            dbProvider.AddInParameter(command, helper.NotiEjecucion, DbType.DateTime, entity.NotiEjecucion);
            dbProvider.AddInParameter(command, helper.NotiUsuCreacion, DbType.String, entity.NotiUsuCreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbNotificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.NotiTitulo, DbType.String, entity.NotiTitulo);
            dbProvider.AddInParameter(command, helper.NotiDescripcion, DbType.String, entity.NotiDescripcion);
            dbProvider.AddInParameter(command, helper.NotiEjecucion, DbType.DateTime, entity.NotiEjecucion);
            dbProvider.AddInParameter(command, helper.NotiUsuCreacion, DbType.String, entity.NotiUsuModificacion);

            dbProvider.AddInParameter(command, helper.NotiCodi, DbType.Int32, entity.NotiCodi);


            dbProvider.ExecuteNonQuery(command);
        }

        public void CambiarEstadoNotificacion(int notiCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCambiarEstadoNotificacion);

            dbProvider.AddInParameter(command, helper.NotiCodi, DbType.Int32, notiCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<WbTipoNotificacionDTO> ObtenerTipoNotificacionEventos()
        {
            List<WbTipoNotificacionDTO> entitys = new List<WbTipoNotificacionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerTipoNotificacionEventos);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbTipoNotificacionDTO entity = new WbTipoNotificacionDTO();

                    int iTiponoticodi = dr.GetOrdinal(helper.Tiponoticodi);
                    if (!dr.IsDBNull(iTiponoticodi)) entity.Tiponoticodi = Convert.ToInt32(dr.GetValue(iTiponoticodi));

                    int iTiponotidesc = dr.GetOrdinal(helper.Tiponotidesc);
                    if (!dr.IsDBNull(iTiponotidesc)) entity.Tiponotidesc = dr.GetString(iTiponotidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
