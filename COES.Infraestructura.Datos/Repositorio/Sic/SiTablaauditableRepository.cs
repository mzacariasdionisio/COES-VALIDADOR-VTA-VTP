using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SI_TABLA_AUDITABLE
    /// </summary>
    public class SiTablaAuditableRepository : RepositoryBase, ISiTablaAuditableRepository
    {
        public SiTablaAuditableRepository(string strConn)
            : base(strConn)
        {
        }

        SiTablaAuditableHelper helper = new SiTablaAuditableHelper();

        public int Save(SiTablaAuditableDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tauditcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tauditnomb, DbType.String, entity.TauditNomb);
            dbProvider.AddInParameter(command, helper.Taudittipaudit, DbType.Int32, entity.TauditTipAudit);
            dbProvider.AddInParameter(command, helper.Tauditestado, DbType.String, entity.TauditEstado);
            dbProvider.AddInParameter(command, helper.Tauditusuariocreacion, DbType.String, entity.TauditUsuarioCreacion);
            dbProvider.AddInParameter(command, helper.Tauditfechacreacion, DbType.DateTime, entity.TauditFechaCreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiTablaAuditableDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tauditnomb, DbType.String, entity.TauditNomb);
            dbProvider.AddInParameter(command, helper.Taudittipaudit, DbType.Int32, entity.TauditTipAudit);
            dbProvider.AddInParameter(command, helper.Tauditestado, DbType.String, entity.TauditEstado);
            dbProvider.AddInParameter(command, helper.Tauditusuarioupdate, DbType.String, entity.TauditUsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Tauditfechaupdate, DbType.DateTime, entity.TauditFechaUpdate);
            dbProvider.AddInParameter(command, helper.Tauditcodi, DbType.Int32, entity.TauditCodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Tauditcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tauditcodi, DbType.Int32, Tauditcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiTablaAuditableDTO GetById(int Tauditcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tauditcodi, DbType.Int32, Tauditcodi);
            SiTablaAuditableDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiTablaAuditableDTO> List()
        {
            List<SiTablaAuditableDTO> entitys = new List<SiTablaAuditableDTO>();
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
        public List<SiTablaAuditableDTO> GetByCriteria()
        {
            List<SiTablaAuditableDTO> entitys = new List<SiTablaAuditableDTO>();
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

        public List<fwUserDTO> ListUserRol(int rolcode)
        {
            List<fwUserDTO> entity = new List<fwUserDTO>();

            String query = String.Format(helper.SqlListUserRol, rolcode);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity.Add(helper.CreateUser(dr));
                }
            }
            return entity;
        }
    }
}
