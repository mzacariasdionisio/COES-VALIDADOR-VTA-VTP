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
    /// Clase de acceso a datos de la tabla SI_ROL_TURNO
    /// </summary>
    public class SiRolTurnoRepository: RepositoryBase, ISiRolTurnoRepository
    {
        public SiRolTurnoRepository(string strConn): base(strConn)
        {
        }

        SiRolTurnoHelper helper = new SiRolTurnoHelper();

        public void Save(SiRolTurnoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Roltfecha, DbType.DateTime, entity.Roltfecha);
            dbProvider.AddInParameter(command, helper.Actcodi, DbType.Int32, entity.Actcodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, entity.Percodi);
            dbProvider.AddInParameter(command, helper.Roltestado, DbType.String, entity.Roltestado);
            dbProvider.AddInParameter(command, helper.Roltfechaactualizacion, DbType.DateTime, entity.Roltfechaactualizacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(SiRolTurnoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Roltestado, DbType.String, entity.Roltestado);
            dbProvider.AddInParameter(command, helper.Roltfechaactualizacion, DbType.DateTime, entity.Roltfechaactualizacion);
            dbProvider.AddInParameter(command, helper.Roltfecha, DbType.DateTime, entity.Roltfecha);
            dbProvider.AddInParameter(command, helper.Actcodi, DbType.Int32, entity.Actcodi);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, entity.Percodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime roltfecha, int actcodi, DateTime lastdate, int percodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Roltfecha, DbType.DateTime, roltfecha);
            dbProvider.AddInParameter(command, helper.Actcodi, DbType.Int32, actcodi);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, lastdate);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, percodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiRolTurnoDTO GetById(DateTime roltfecha, int actcodi, DateTime lastdate, int percodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Roltfecha, DbType.DateTime, roltfecha);
            dbProvider.AddInParameter(command, helper.Actcodi, DbType.Int32, actcodi);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, lastdate);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, percodi);
            SiRolTurnoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiRolTurnoDTO> List()
        {
            List<SiRolTurnoDTO> entitys = new List<SiRolTurnoDTO>();
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

        public List<SiRolTurnoDTO> GetByCriteria()
        {
            List<SiRolTurnoDTO> entitys = new List<SiRolTurnoDTO>();
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

        public List<SiRolTurnoDTO> ListaRols(DateTime fecIni, DateTime fecFin, string percodi)
        {
            List<SiRolTurnoDTO> entitys = new List<SiRolTurnoDTO>();
            string query = string.Format(helper.SqlListaRols, fecIni.ToString(ConstantesBase.FormatoFechaPE), fecFin.ToString(ConstantesBase.FormatoFechaPE), percodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            SiRolTurnoDTO entity = new SiRolTurnoDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iActabrev = dr.GetOrdinal(this.helper.Actabrev);
                    if (!dr.IsDBNull(iActabrev)) entity.Actabrev = dr.GetString(iActabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void SaveSiRolTurnoMasivo(List<SiRolTurnoDTO> Rols)
        {
            dbProvider.AddColumnMapping(helper.Roltfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Actcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Lastuser, DbType.String);
            dbProvider.AddColumnMapping(helper.Lastdate, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Percodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Roltestado, DbType.String);
            dbProvider.AddColumnMapping(helper.Roltfechaactualizacion, DbType.DateTime);

            dbProvider.BulkInsert<SiRolTurnoDTO>(Rols, helper.TableName);
        }

        public void DeleteSiRolTurnoMasivo(DateTime fecIni, DateTime fecFin, string percodi)
        {
            string query = string.Format(helper.SqlDeleteSiRolTurnoMasivo, fecIni.ToString(ConstantesBase.FormatoFechaPE), fecFin.ToString(ConstantesBase.FormatoFechaPE), percodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<SiRolTurnoDTO> ListaMovimientos(DateTime fecIni, DateTime fecFin)
        {
            List<SiRolTurnoDTO> entitys = new List<SiRolTurnoDTO>();
            string query = string.Format(helper.SqlListaMovimientos, fecIni.ToString(ConstantesBase.FormatoFechaPE), fecFin.ToString(ConstantesBase.FormatoFechaPE));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            SiRolTurnoDTO entity = new SiRolTurnoDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPernomb = dr.GetOrdinal(this.helper.Pernomb);
                    if (!dr.IsDBNull(iPernomb)) entity.Pernomb = dr.GetString(iPernomb);

                    int iActabrev = dr.GetOrdinal(this.helper.Actabrev);
                    if (!dr.IsDBNull(iActabrev)) entity.Actabrev = dr.GetString(iActabrev);

                    int iActnomb = dr.GetOrdinal(this.helper.Actnomb);
                    if (!dr.IsDBNull(iActnomb)) entity.Actnomb = dr.GetString(iActnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
