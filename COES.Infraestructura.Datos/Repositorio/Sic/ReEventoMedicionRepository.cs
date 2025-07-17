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
    /// Clase de acceso a datos de la tabla RE_EVENTO_MEDICION
    /// </summary>
    public class ReEventoMedicionRepository: RepositoryBase, IReEventoMedicionRepository
    {
        public ReEventoMedicionRepository(string strConn): base(strConn)
        {
        }

        ReEventoMedicionHelper helper = new ReEventoMedicionHelper();

        public int Save(ReEventoMedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reemedcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reevprcodi, DbType.Int32, entity.Reevprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reemedfechahora, DbType.DateTime, entity.Reemedfechahora);
            dbProvider.AddInParameter(command, helper.Reemedtensionrs, DbType.Decimal, entity.Reemedtensionrs);
            dbProvider.AddInParameter(command, helper.Reemedtensionst, DbType.Decimal, entity.Reemedtensionst);
            dbProvider.AddInParameter(command, helper.Reemedtensiontr, DbType.Decimal, entity.Reemedtensiontr);
            dbProvider.AddInParameter(command, helper.Reemedvarp, DbType.Decimal, entity.Reemedvarp);
            dbProvider.AddInParameter(command, helper.Reemedvala, DbType.Decimal, entity.Reemedvala);
            dbProvider.AddInParameter(command, helper.Reemedvalap, DbType.Decimal, entity.Reemedvalap);
            dbProvider.AddInParameter(command, helper.Reemedvalep, DbType.Decimal, entity.Reemedvalep);
            dbProvider.AddInParameter(command, helper.Reemedvalaapep, DbType.Decimal, entity.Reemedvalaapep);
            dbProvider.AddInParameter(command, helper.Reemedusucreacion, DbType.String, entity.Reemedusucreacion);
            dbProvider.AddInParameter(command, helper.Reemedfeccreacion, DbType.DateTime, entity.Reemedfeccreacion);
            dbProvider.AddInParameter(command, helper.Reemedusumodificacion, DbType.String, entity.Reemedusumodificacion);
            dbProvider.AddInParameter(command, helper.Reemedfecmodificacion, DbType.DateTime, entity.Reemedfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReEventoMedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reevprcodi, DbType.Int32, entity.Reevprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reemedfechahora, DbType.DateTime, entity.Reemedfechahora);
            dbProvider.AddInParameter(command, helper.Reemedtensionrs, DbType.Decimal, entity.Reemedtensionrs);
            dbProvider.AddInParameter(command, helper.Reemedtensionst, DbType.Decimal, entity.Reemedtensionst);
            dbProvider.AddInParameter(command, helper.Reemedtensiontr, DbType.Decimal, entity.Reemedtensiontr);
            dbProvider.AddInParameter(command, helper.Reemedvarp, DbType.Decimal, entity.Reemedvarp);
            dbProvider.AddInParameter(command, helper.Reemedvala, DbType.Decimal, entity.Reemedvala);
            dbProvider.AddInParameter(command, helper.Reemedvalap, DbType.Decimal, entity.Reemedvalap);
            dbProvider.AddInParameter(command, helper.Reemedvalep, DbType.Decimal, entity.Reemedvalep);
            dbProvider.AddInParameter(command, helper.Reemedvalaapep, DbType.Decimal, entity.Reemedvalaapep);
            dbProvider.AddInParameter(command, helper.Reemedusucreacion, DbType.String, entity.Reemedusucreacion);
            dbProvider.AddInParameter(command, helper.Reemedfeccreacion, DbType.DateTime, entity.Reemedfeccreacion);
            dbProvider.AddInParameter(command, helper.Reemedusumodificacion, DbType.String, entity.Reemedusumodificacion);
            dbProvider.AddInParameter(command, helper.Reemedfecmodificacion, DbType.DateTime, entity.Reemedfecmodificacion);
            dbProvider.AddInParameter(command, helper.Reemedcodi, DbType.Int32, entity.Reemedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int evento, int empresa)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reevprcodi, DbType.Int32, evento);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, empresa);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReEventoMedicionDTO GetById(int reemedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reemedcodi, DbType.Int32, reemedcodi);
            ReEventoMedicionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReEventoMedicionDTO> List()
        {
            List<ReEventoMedicionDTO> entitys = new List<ReEventoMedicionDTO>();
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

        public List<ReEventoMedicionDTO> GetByCriteria()
        {
            List<ReEventoMedicionDTO> entitys = new List<ReEventoMedicionDTO>();
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

        public List<ReEventoMedicionDTO> ObtenerMedicion(int idEvento, int idEmpresa)
        {
            List<ReEventoMedicionDTO> entitys = new List<ReEventoMedicionDTO>();
            string sql = string.Format(helper.SqlObtenerMedicion, idEvento, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEventoMedicionDTO entity = helper.Create(dr);
                    DateTime fecha = (DateTime)entity.Reemedfechahora;
                    entity.Fecha = fecha.Year.ToString() + fecha.Month.ToString().PadLeft(2, '0') + fecha.Day.ToString().PadLeft(2, '0') + 
                        fecha.Hour.ToString().PadLeft(2, '0') + fecha.Minute.ToString().PadLeft(2, '0');

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
