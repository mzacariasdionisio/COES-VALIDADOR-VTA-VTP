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
    /// Clase de acceso a datos de la tabla MD_VALIDACION
    /// </summary>
    public class MdValidacionRepository: RepositoryBase, IMdValidacionRepository
    {
        public MdValidacionRepository(string strConn): base(strConn)
        {
        }

        MdValidacionHelper helper = new MdValidacionHelper();

        public void Save(MdValidacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Validames, DbType.DateTime, entity.Validames);
            dbProvider.AddInParameter(command, helper.Validafecha, DbType.DateTime, entity.Validafecha);
            dbProvider.AddInParameter(command, helper.Validaestado, DbType.String, entity.Validaestado);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MdValidacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Validafecha, DbType.DateTime, entity.Validafecha);
            dbProvider.AddInParameter(command, helper.Validaestado, DbType.String, entity.Validaestado);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Validames, DbType.DateTime, entity.Validames);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int emprcodi, DateTime validames)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Validames, DbType.DateTime, validames);

            dbProvider.ExecuteNonQuery(command);
        }

        public MdValidacionDTO GetById(int emprcodi, DateTime validames)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Validames, DbType.DateTime, validames);
            MdValidacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MdValidacionDTO> List()
        {
            List<MdValidacionDTO> entitys = new List<MdValidacionDTO>();
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

        public List<MdValidacionDTO> GetByCriteria(DateTime fecha)
        {
            DateTime fechaValidacion = new DateTime(fecha.Year, fecha.Month, 1);
            string query = string.Format(helper.SqlGetByCriteria, fechaValidacion.ToString(ConstantesBase.FormatoFecha));
            List<MdValidacionDTO> entitys = new List<MdValidacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MdValidacionDTO entity = helper.Create(dr);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
