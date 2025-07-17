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
    /// Clase de acceso a datos de la tabla MMM_JUSTIFICACION
    /// </summary>
    public class MmmJustificacionRepository : RepositoryBase, IMmmJustificacionRepository
    {
        public MmmJustificacionRepository(string strConn)
            : base(strConn)
        {
        }

        MmmJustificacionHelper helper = new MmmJustificacionHelper();

        public int Save(MmmJustificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mjustcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Immecodi, DbType.Int32, entity.Immecodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Mjustfecha, DbType.DateTime, entity.Mjustfecha);
            dbProvider.AddInParameter(command, helper.Mjustdescripcion, DbType.String, entity.Mjustdescripcion);
            dbProvider.AddInParameter(command, helper.Mjustusucreacion, DbType.String, entity.Mjustusucreacion);
            dbProvider.AddInParameter(command, helper.Mjustfeccreacion, DbType.DateTime, entity.Mjustfeccreacion);
            dbProvider.AddInParameter(command, helper.Mjustusumodificacion, DbType.String, entity.Mjustusumodificacion);
            dbProvider.AddInParameter(command, helper.Mjustfecmodificacion, DbType.DateTime, entity.Mjustfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MmmJustificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Immecodi, DbType.Int32, entity.Immecodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Mjustfecha, DbType.DateTime, entity.Mjustfecha);
            dbProvider.AddInParameter(command, helper.Mjustdescripcion, DbType.String, entity.Mjustdescripcion);
            dbProvider.AddInParameter(command, helper.Mjustusucreacion, DbType.String, entity.Mjustusucreacion);
            dbProvider.AddInParameter(command, helper.Mjustfeccreacion, DbType.DateTime, entity.Mjustfeccreacion);
            dbProvider.AddInParameter(command, helper.Mjustusumodificacion, DbType.String, entity.Mjustusumodificacion);
            dbProvider.AddInParameter(command, helper.Mjustfecmodificacion, DbType.DateTime, entity.Mjustfecmodificacion);

            dbProvider.AddInParameter(command, helper.Mjustcodi, DbType.Int32, entity.Mjustcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mjustcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mjustcodi, DbType.Int32, mjustcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MmmJustificacionDTO GetById(int mjustcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mjustcodi, DbType.Int32, mjustcodi);
            MmmJustificacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MmmJustificacionDTO> List()
        {
            List<MmmJustificacionDTO> entitys = new List<MmmJustificacionDTO>();
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

        public List<MmmJustificacionDTO> GetByCriteria()
        {
            List<MmmJustificacionDTO> entitys = new List<MmmJustificacionDTO>();
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

        public List<MmmJustificacionDTO> ListByFechaAndIndicador(int immecodi, DateTime fechaIni, DateTime fechaFin)
        {
            List<MmmJustificacionDTO> entitys = new List<MmmJustificacionDTO>();

            string sql = string.Format(helper.SqlListByFechaAndIndicador, immecodi, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
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
    }
}
