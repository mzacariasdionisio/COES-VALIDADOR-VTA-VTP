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
    /// Clase de acceso a datos de la tabla SI_CAMBIO_TURNO_AUDIT
    /// </summary>
    public class SiCambioTurnoAuditRepository: RepositoryBase, ISiCambioTurnoAuditRepository
    {
        public SiCambioTurnoAuditRepository(string strConn): base(strConn)
        {
        }

        SiCambioTurnoAuditHelper helper = new SiCambioTurnoAuditHelper();

        public int Save(SiCambioTurnoAuditDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Turnoauditcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cambioturnocodi, DbType.Int32, entity.Cambioturnocodi);
            dbProvider.AddInParameter(command, helper.Desaccion, DbType.String, entity.Desaccion);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiCambioTurnoAuditDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Desaccion, DbType.String, entity.Desaccion);
            dbProvider.AddInParameter(command, helper.Cambioturnocodi, DbType.Int32, entity.Cambioturnocodi);
            dbProvider.AddInParameter(command, helper.Turnoauditcodi, DbType.Int32, entity.Turnoauditcodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int turnoauditcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Turnoauditcodi, DbType.Int32, turnoauditcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiCambioTurnoAuditDTO GetById(int turnoauditcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Turnoauditcodi, DbType.Int32, turnoauditcodi);
            SiCambioTurnoAuditDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiCambioTurnoAuditDTO> List()
        {
            List<SiCambioTurnoAuditDTO> entitys = new List<SiCambioTurnoAuditDTO>();
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

        public List<SiCambioTurnoAuditDTO> GetByCriteria(int cambioTurnoCodi)
        {
            List<SiCambioTurnoAuditDTO> entitys = new List<SiCambioTurnoAuditDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Cambioturnocodi, DbType.Int32, cambioTurnoCodi);

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
