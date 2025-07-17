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
    /// Clase de acceso a datos de la tabla IN_DESTINATARIOMENSAJE
    /// </summary>
    public class InDestinatariomensajeRepository : RepositoryBase, IInDestinatariomensajeRepository
    {
        public InDestinatariomensajeRepository(string strConn) : base(strConn)
        {
        }

        InDestinatariomensajeHelper helper = new InDestinatariomensajeHelper();

        public int Save(InDestinatariomensajeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Indemecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Indemeestado, DbType.String, entity.Indemeestado);
            dbProvider.AddInParameter(command, helper.Indememotivobaja, DbType.String, entity.Indememotivobaja);
            dbProvider.AddInParameter(command, helper.Indemeusucreacion, DbType.String, entity.Indemeusucreacion);
            dbProvider.AddInParameter(command, helper.Indemefeccreacion, DbType.DateTime, entity.Indemefeccreacion);
            dbProvider.AddInParameter(command, helper.Indemeusumodificacion, DbType.String, entity.Indemeusumodificacion);
            dbProvider.AddInParameter(command, helper.Indemefecmodificacion, DbType.DateTime, entity.Indemefecmodificacion);
            dbProvider.AddInParameter(command, helper.Indemevigente, DbType.String, entity.Indemevigente);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(InDestinatariomensajeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Indemeestado, DbType.String, entity.Indemeestado);
            dbProvider.AddInParameter(command, helper.Indememotivobaja, DbType.String, entity.Indememotivobaja);
            dbProvider.AddInParameter(command, helper.Indemeusucreacion, DbType.String, entity.Indemeusucreacion);
            dbProvider.AddInParameter(command, helper.Indemefeccreacion, DbType.DateTime, entity.Indemefeccreacion);
            dbProvider.AddInParameter(command, helper.Indemeusumodificacion, DbType.String, entity.Indemeusumodificacion);
            dbProvider.AddInParameter(command, helper.Indemefecmodificacion, DbType.DateTime, entity.Indemefecmodificacion);
            dbProvider.AddInParameter(command, helper.Indemevigente, DbType.String, entity.Indemevigente);
            dbProvider.AddInParameter(command, helper.Indemecodi, DbType.Int32, entity.Indemecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int indemecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Indemecodi, DbType.Int32, indemecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InDestinatariomensajeDTO GetById(int indemecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Indemecodi, DbType.Int32, indemecodi);
            InDestinatariomensajeDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InDestinatariomensajeDTO> List()
        {
            List<InDestinatariomensajeDTO> entitys = new List<InDestinatariomensajeDTO>();
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

        public List<InDestinatariomensajeDTO> GetByCriteria()
        {
            List<InDestinatariomensajeDTO> entitys = new List<InDestinatariomensajeDTO>();
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

        public List<InDestinatariomensajeDTO> ObtenerConfiguracionNotificacion(int empresa, string estado)
        {
            List<InDestinatariomensajeDTO> entitys = new List<InDestinatariomensajeDTO>();
            string sql = string.Format(helper.SqlObtenerConsulta, empresa, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InDestinatariomensajeDTO entity = helper.Create(dr);

                    int iEvenclasecodi = dr.GetOrdinal(helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

                    int iIndmdeacceso = dr.GetOrdinal(helper.Indmdeacceso);
                    if (!dr.IsDBNull(iIndmdeacceso)) entity.Indmdeacceso = Convert.ToInt32(dr.GetValue(iIndmdeacceso));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iUsername = dr.GetOrdinal(helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

                    int iUseremail = dr.GetOrdinal(helper.Useremail);
                    if (!dr.IsDBNull(iUseremail)) entity.Useremail = dr.GetString(iUseremail);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<InDestinatariomensajeDTO> ObtenerHistorico(int empresa, int usuario)
        {
            List<InDestinatariomensajeDTO> entitys = new List<InDestinatariomensajeDTO>();
            string sql = string.Format(helper.SqlObtenerHistorico, empresa, usuario);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InDestinatariomensajeDTO entity = helper.Create(dr);

                    int iEvenclasecodi = dr.GetOrdinal(helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

                    int iIndmdeacceso = dr.GetOrdinal(helper.Indmdeacceso);
                    if (!dr.IsDBNull(iIndmdeacceso)) entity.Indmdeacceso = Convert.ToInt32(dr.GetValue(iIndmdeacceso));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<InDestinatariomensajeDTO> ObtenerConfiguracionVigente()
        {
            List<InDestinatariomensajeDTO> entitys = new List<InDestinatariomensajeDTO>();
            string sql = string.Format(helper.SqlObtenerConfiguracionVigente);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InDestinatariomensajeDTO entity = helper.Create(dr);

                    int iIndmdecodi = dr.GetOrdinal(helper.Indmdecodi);
                    if (!dr.IsDBNull(iIndmdecodi)) entity.Indmdecodi = Convert.ToInt32(dr.GetValue(iIndmdecodi));

                    int iEvenclasecodi = dr.GetOrdinal(helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

                    int iIndmdeacceso = dr.GetOrdinal(helper.Indmdeacceso);
                    if (!dr.IsDBNull(iIndmdeacceso)) entity.Indmdeacceso = Convert.ToInt32(dr.GetValue(iIndmdeacceso));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<InDestinatariomensajeDTO> ObtenerConfiguracionVigentePorUsuario(int usuario)
        {
            List<InDestinatariomensajeDTO> entitys = new List<InDestinatariomensajeDTO>();
            string sql = string.Format(helper.SqlObtenerConfiguracionVigentePorUsuario, usuario);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InDestinatariomensajeDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
