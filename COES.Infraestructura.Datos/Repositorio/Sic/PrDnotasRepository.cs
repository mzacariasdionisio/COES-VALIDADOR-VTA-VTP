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
    /// Clase de acceso a datos de la tabla PR_DNOTAS
    /// </summary>
    public class PrDnotasRepository: RepositoryBase, IPrDnotasRepository
    {
        public PrDnotasRepository(string strConn): base(strConn)
        {
        }

        PrDnotasHelper helper = new PrDnotasHelper();

        public void Save(PrDnotasDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, entity.Fecha);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Notaitem, DbType.Int32, entity.Notaitem);
            dbProvider.AddInParameter(command, helper.Notadesc, DbType.String, entity.Notadesc);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrDnotasDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, entity.Fecha);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Notaitem, DbType.Int32, entity.Notaitem);
            dbProvider.AddInParameter(command, helper.Notadesc, DbType.String, entity.Notadesc);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime fecha, int lectcodi, int notaitem)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, fecha);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Notaitem, DbType.Int32, notaitem);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrDnotasDTO GetById(DateTime fecha, int lectcodi, int notaitem)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, fecha);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Notaitem, DbType.Int32, notaitem);
            PrDnotasDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrDnotasDTO> List()
        {
            List<PrDnotasDTO> entitys = new List<PrDnotasDTO>();
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

        public List<PrDnotasDTO> GetByCriteria(DateTime fecha)
        {
            List<PrDnotasDTO> entitys = new List<PrDnotasDTO>();
            string query = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
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
