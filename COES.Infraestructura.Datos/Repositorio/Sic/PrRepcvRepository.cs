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
    /// Clase de acceso a datos de la tabla PR_REPCV
    /// </summary>
    public class PrRepcvRepository: RepositoryBase, IPrRepcvRepository
    {
        public PrRepcvRepository(string strConn): base(strConn)
        {
        }

        PrRepcvHelper helper = new PrRepcvHelper();

        public int Save(PrRepcvDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repfecha, DbType.DateTime, entity.Repfecha);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Repobserva, DbType.String, entity.Repobserva);
            dbProvider.AddInParameter(command, helper.Reptipo, DbType.String, entity.Reptipo);
            dbProvider.AddInParameter(command, helper.Repnomb, DbType.String, entity.Repnomb);
            dbProvider.AddInParameter(command, helper.Repdetalle, DbType.String, entity.Repdetalle);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.String, entity.Deleted);
            dbProvider.AddInParameter(command, helper.Repfechafp, DbType.DateTime, entity.Repfechafp);
            dbProvider.AddInParameter(command, helper.Repfechaem, DbType.DateTime, entity.Repfechaem);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrRepcvDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repfecha, DbType.DateTime, entity.Repfecha);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Repobserva, DbType.String, entity.Repobserva);
            dbProvider.AddInParameter(command, helper.Reptipo, DbType.String, entity.Reptipo);
            dbProvider.AddInParameter(command, helper.Repnomb, DbType.String, entity.Repnomb);
            dbProvider.AddInParameter(command, helper.Repdetalle, DbType.String, entity.Repdetalle);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.String, entity.Deleted);
            dbProvider.AddInParameter(command, helper.Repfechafp, DbType.DateTime, entity.Repfechafp);
            dbProvider.AddInParameter(command, helper.Repfechaem, DbType.DateTime, entity.Repfechaem);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, entity.Repcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int repcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, repcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrRepcvDTO GetById(int repcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, repcodi);
            PrRepcvDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrRepcvDTO> List()
        {
            List<PrRepcvDTO> entitys = new List<PrRepcvDTO>();
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

        public List<PrRepcvDTO> GetByCriteria(DateTime dFechaInicio, DateTime dFechaFin)
        {
            List<PrRepcvDTO> entitys = new List<PrRepcvDTO>();
            string strComando = string.Format(helper.SqlGetByCriteria, dFechaInicio.ToString(ConstantesBase.FormatoFecha), dFechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<PrRepcvDTO> ObtenerReporte(string repFechaIni, string repFechaFin)
        {
            List<PrRepcvDTO> entitys = new List<PrRepcvDTO>();
            String sql = String.Format(this.helper.ObtenerReporte, repFechaIni, repFechaFin);
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

        #region MigracionSGOCOES-GrupoB
        public PrRepcvDTO GetByFechaAndTipo(DateTime fecha, string tipo)
        {
            string query = string.Format(helper.SqlGetByFechaAndTipo, fecha.ToString(ConstantesBase.FormatoFecha), tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            PrRepcvDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        #endregion

        #region siosein2
        public List<PrRepcvDTO> ObtenerReportecvYVariablesXPeriodo(string periodo)
        {
            List<PrRepcvDTO> entitys = new List<PrRepcvDTO>();
            PrRepcvDTO entity = null;

            string query = string.Format(helper.SqlObtenerReportecvYVariablesXPeriodo, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);


                    int iGrupopadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iCvBase = dr.GetOrdinal(helper.CvBase);
                    if (!dr.IsDBNull(iCvBase)) entity.CvBase = Convert.ToDecimal(dr.GetValue(iCvBase));

                    int iCvMedia = dr.GetOrdinal(helper.CvMedia);
                    if (!dr.IsDBNull(iCvMedia)) entity.CvMedia = Convert.ToDecimal(dr.GetValue(iCvMedia));

                    int iCvPunta = dr.GetOrdinal(helper.CvPunta);
                    if (!dr.IsDBNull(iCvPunta)) entity.CvPunta = Convert.ToDecimal(dr.GetValue(iCvPunta));

                    entitys.Add(entity);

                }
            }

            return entitys;
        }
        #endregion

        public List<PrRepcvDTO> GetRepcvByEnvcodi(int cbenvcodi)
        {
            List<PrRepcvDTO> entitys = new List<PrRepcvDTO>();
            var sqlQuery = string.Format(helper.SqlGetRepcvByEnvcodi, cbenvcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

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
