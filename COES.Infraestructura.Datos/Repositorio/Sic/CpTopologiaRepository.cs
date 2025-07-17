using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.CortoPlazo.Modelo.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CP_ESCTOPOLOGIA
    /// </summary>
    public class CpTopologiaRepository : RepositoryBase, ICpTopologiaRepository
    {
        public CpTopologiaRepository(string strConn)
            : base(strConn)
        {
        }

        CpTopologiaHelper helper = new CpTopologiaHelper();

        public int Save(CpTopologiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Topnombre, DbType.String, entity.Topnombre);
            dbProvider.AddInParameter(command, helper.Topfecha, DbType.DateTime, entity.Topfecha);
            dbProvider.AddInParameter(command, helper.Topinicio, DbType.Int32, entity.Topinicio);
            dbProvider.AddInParameter(command, helper.Tophorizonte, DbType.Int32, entity.Tophorizonte);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Tophora, DbType.Int32, entity.Tophora);
            dbProvider.AddInParameter(command, helper.Topdiasproc, DbType.Int32, entity.Topdiasproc);
            dbProvider.AddInParameter(command, helper.Toptipo, DbType.Int32, entity.Toptipo);
            dbProvider.AddInParameter(command, helper.Topiniciohora, DbType.Int32, entity.Topiniciohora);
            dbProvider.AddInParameter(command, helper.Topsinrsf, DbType.Int32, entity.Topsinrsf);
            dbProvider.AddInParameter(command, helper.Fverscodi, DbType.Int32, entity.Fverscodi);
            dbProvider.AddInParameter(command, helper.Avercodi, DbType.Int32, entity.Avercodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpTopologiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Topnombre, DbType.String, entity.Topnombre);
            dbProvider.AddInParameter(command, helper.Topfecha, DbType.DateTime, entity.Topfecha);
            dbProvider.AddInParameter(command, helper.Topinicio, DbType.Int32, entity.Topinicio);
            dbProvider.AddInParameter(command, helper.Tophorizonte, DbType.Int32, entity.Tophorizonte);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Tophora, DbType.Int32, entity.Tophora);
            dbProvider.AddInParameter(command, helper.Topdiasproc, DbType.Int32, entity.Topdiasproc);
            dbProvider.AddInParameter(command, helper.Toptipo, DbType.Int32, entity.Toptipo);
            dbProvider.AddInParameter(command, helper.Topiniciohora, DbType.Int32, entity.Topiniciohora);
            dbProvider.AddInParameter(command, helper.Topsinrsf, DbType.Int32, entity.Topsinrsf);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int topcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpTopologiaDTO GetById(int topcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            CpTopologiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iFverscodi = dr.GetOrdinal(helper.Fverscodi);
                    if (!dr.IsDBNull(iFverscodi)) entity.Fverscodi = Convert.ToInt32(dr.GetValue(iFverscodi));

                    int iAvercodi = dr.GetOrdinal(helper.Avercodi);
                    if (!dr.IsDBNull(iAvercodi)) entity.Avercodi = Convert.ToInt32(dr.GetValue(iAvercodi));
                }
            }

            return entity;
        }

        public List<CpTopologiaDTO> List()
        {
            List<CpTopologiaDTO> entitys = new List<CpTopologiaDTO>();
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

        public CpTopologiaDTO GetByFechaTopfinal(DateTime topfecha, string toptipo)
        {
            var query = string.Format(helper.SqlObtenerTopologiaFinal, topfecha.ToString(ConstantesBase.FormatoFecha), toptipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpTopologiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpTopologiaDTO> GetByCriteria(DateTime topfechaIni, DateTime topfechaFin, short toptipo)
        {
            List<CpTopologiaDTO> entitys = new List<CpTopologiaDTO>();
            string query = string.Format(helper.SqlGetByCriteria, topfechaIni.ToString(ConstantesBase.FormatoFecha), topfechaFin.ToString(ConstantesBase.FormatoFecha), toptipo);
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

        public CpTopologiaDTO GetByIdRsf(int topcodi)
        {
            string sqlquery = string.Format(helper.SqlGetByIdRsf, topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlquery);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            CpTopologiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpTopologiaDTO> ListNombre(string nombre)
        {
            string sqlquery = string.Format(helper.SqlListNombre, nombre.Trim().ToUpper());
            List<CpTopologiaDTO> entitys = new List<CpTopologiaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlquery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CpTopologiaDTO> GetTopReprogramadas(int topcodi)
        {
            string sqlquery = string.Format(helper.SqlListReprogramados, topcodi);
            List<CpTopologiaDTO> entitys = new List<CpTopologiaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlquery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CpTopologiaDTO> ObtenerEscenariosPorDia(DateTime fechaProceso)
        {
            List<CpTopologiaDTO> entitys = new List<CpTopologiaDTO>();
            string query = string.Format(helper.SqlObtenerEscenarioPorDia, fechaProceso.ToString(ConstantesBase.FormatoFecha));

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

        public List<CpTopologiaDTO> ObtenerEscenariosPorDiaConsulta(DateTime fechaProceso, int tipo)
        {
            List<CpTopologiaDTO> entitys = new List<CpTopologiaDTO>();
            string query = string.Format(helper.SqlObtenerEscenarioPorDiaConsulta, fechaProceso.ToString(ConstantesBase.FormatoFecha), tipo);
           
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

        public List<CpTopologiaDTO> ObtenerTipoRestriccion()
        {
            List<CpTopologiaDTO> entitys = new List<CpTopologiaDTO>();            

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerTipoRestricciones);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpTopologiaDTO entity = new CpTopologiaDTO();

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iTopnombre = dr.GetOrdinal(helper.Topnombre);
                    if (!dr.IsDBNull(iTopnombre)) entity.Topnombre = dr.GetString(iTopnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public CpTopologiaDTO ObtenerUltimoEscenarioReprogramado(DateTime topfecha)
        {
            string sql = string.Format(helper.SqlObtenerUltimoEscenarioReprogramado, topfecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            CpTopologiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpTopologiaDTO> ListEscenarioReprograma(DateTime fecha)
        {
            string sql = string.Format(helper.SqlListEscenarioReprograma, fecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            List<CpTopologiaDTO> entitys = new List<CpTopologiaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpTopologiaDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #region IMME
        public List<CpTopologiaDTO> ListaTopFinalDiario(DateTime fini, DateTime ffin)
        {
            string sqlquery = string.Format(helper.SqlListaTopFinalDiario, fini.ToString(ConstantesBase.FormatoFecha), ffin.ToString(ConstantesBase.FormatoFecha));
            List<CpTopologiaDTO> entitys = new List<CpTopologiaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlquery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        #endregion
    }
}
