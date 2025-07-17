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
    /// Clase de acceso a datos de la tabla ME_AMPLIACIONFECHA
    /// </summary>
    public class MeAmpliacionfechaRepository : RepositoryBase, IMeAmpliacionfechaRepository
    {
        public MeAmpliacionfechaRepository(string strConn)
            : base(strConn)
        {
        }

        MeAmpliacionfechaHelper helper = new MeAmpliacionfechaHelper();

        public void Update(MeAmpliacionfechaDTO entity)
        {
            string sqlQuery = string.Format(helper.SqlUpdate, entity.Amplifechaplazo.ToString(ConstantesBase.FormatoFechaExtendido),
                entity.Lastuser, ((DateTime)entity.Lastdate).ToString(ConstantesBase.FormatoFechaExtendido),
                entity.Amplifecha.ToString(ConstantesBase.FormatoFechaExtendido), entity.Emprcodi, entity.Formatcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateById(MeAmpliacionfechaDTO entity)
        {
            string sqlQuery = string.Format(helper.SqlUpdateById, entity.Amplifechaplazo.ToString(ConstantesBase.FormatoFechaExtendido),
                entity.Lastuser, ((DateTime)entity.Lastdate).ToString(ConstantesBase.FormatoFechaExtendido),
                entity.Amplifecha.ToString(ConstantesBase.FormatoFechaExtendido), entity.Amplicodi); 
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Save(MeAmpliacionfechaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Amplicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Amplifecha, DbType.DateTime, entity.Amplifecha);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Amplifechaplazo, DbType.DateTime, entity.Amplifechaplazo);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int amplicodi) 
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Amplicodi, DbType.DateTime, amplicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeAmpliacionfechaDTO GetById(DateTime fecha, int empresa, int formato)
        {
            string queryString = string.Format(helper.SqlGetById, fecha.ToString(ConstantesBase.FormatoFecha), empresa, formato);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            MeAmpliacionfechaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }

        public List<MeAmpliacionfechaDTO> List()
        {
            List<MeAmpliacionfechaDTO> entitys = new List<MeAmpliacionfechaDTO>();
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

        public List<MeAmpliacionfechaDTO> GetByCriteria(string formato, string empresa, DateTime fechaIni)
        {
            List<MeAmpliacionfechaDTO> entitys = new List<MeAmpliacionfechaDTO>();

            string queryString = string.Format(helper.SqlGetByCriteria, formato, empresa, fechaIni.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeAmpliacionfechaDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iFormatnomb = dr.GetOrdinal(helper.Formatnombre);
                    if (!dr.IsDBNull(iFormatnomb)) entity.Formatnombre = dr.GetString(iFormatnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeAmpliacionfechaDTO> GetListaAmpliacion(DateTime fechaIni, DateTime fechaFin, int empresa, int formato)
        {
            List<MeAmpliacionfechaDTO> entitys = new List<MeAmpliacionfechaDTO>();
            string queryString = string.Format(helper.SqlListaAmpliacion, fechaIni.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), empresa, formato);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeAmpliacionfechaDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iFormatnomb = dr.GetOrdinal(helper.Formatnombre);
                    if (!dr.IsDBNull(iFormatnomb)) entity.Formatnombre = dr.GetString(iFormatnomb);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeAmpliacionfechaDTO> GetListaMultiple(DateTime fechaIni, DateTime fechaFin, string empresa, string formato)
        {
            List<MeAmpliacionfechaDTO> entitys = new List<MeAmpliacionfechaDTO>();
            string queryString = string.Format(helper.SqlListaAmpliacionMultiple, fechaIni.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), empresa, formato);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeAmpliacionfechaDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iFormatnomb = dr.GetOrdinal(helper.Formatnombre);
                    if (!dr.IsDBNull(iFormatnomb)) entity.Formatnombre = dr.GetString(iFormatnomb);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeAmpliacionfechaDTO> GetAmpliacionNow(int emprCodi, string fecha)
        {
            List<MeAmpliacionfechaDTO> entitys = new List<MeAmpliacionfechaDTO>();
            string queryString = string.Format(helper.SqlGetAmpliacionNow, emprCodi, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<SiEmpresaDTO> ListaEmpresasAmpliacionPlazo()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            string condicion = "";

            string queryString = string.Format(helper.SqlListaEmpresasAmpliacionPlazo, condicion);


            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
                        
            //dbProvider.AddInParameter(command, helper.Emprestado, DbType.String, estadoRegistro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEMPRRAZSOCIAL = dr.GetOrdinal("EMPRRAZSOCIAL");
                    if (!dr.IsDBNull(iEMPRRAZSOCIAL)) entity.Emprrazsocial = dr.GetString(iEMPRRAZSOCIAL);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeAmpliacionfechaDTO> GetListaAmpliacionFiltro(DateTime periodo, int empresa, int formato, int regIni, int regFin)
        {
            List<MeAmpliacionfechaDTO> entitys = new List<MeAmpliacionfechaDTO>();

            string sentenciaWhere = string.Empty;
            sentenciaWhere = " WHERE f.formatcodi = " + formato;

            if (empresa > 0)
            {
                sentenciaWhere = sentenciaWhere + " AND AM.emprcodi = " + empresa;
            }

            if (periodo != DateTime.MinValue)
            {
                sentenciaWhere = sentenciaWhere + string.Format(" AND AM.AMPLIFECHA BETWEEN  to_date('{0}','YYYY-MM-DD') and to_date('{1}','YYYY-MM-DD') ",
                    periodo.ToString(ConstantesBase.FormatoFecha), periodo.AddMonths(1).AddDays(-1).ToString(ConstantesBase.FormatoFecha));
            }

            string queryString = string.Format(helper.SqListaAmpliacionFiltro, sentenciaWhere, regFin, regIni);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeAmpliacionfechaDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iFormatnomb = dr.GetOrdinal(helper.Formatnombre);
                    if (!dr.IsDBNull(iFormatnomb)) entity.Formatnombre = dr.GetString(iFormatnomb);
                    int iFormatdiafinplazo = dr.GetOrdinal(helper.Formatdiafinplazo);
                    if (!dr.IsDBNull(iFormatdiafinplazo)) entity.Formatdiafinplazo = Convert.ToInt32(dr.GetValue(iFormatdiafinplazo));
                    int iFormatdiaplazo = dr.GetOrdinal(helper.Formatdiaplazo);
                    if (!dr.IsDBNull(iFormatdiaplazo)) entity.Formatdiaplazo = Convert.ToInt32(dr.GetValue(iFormatdiaplazo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int GetListaAmpliacionFiltroCount(DateTime periodo, int empresa, int formato)
        {
            string sentenciaWhere = string.Empty;
            sentenciaWhere = " WHERE f.formatcodi = " + formato;

            if (empresa > 0)
            {
                sentenciaWhere = sentenciaWhere + " AND AM.emprcodi = " + empresa;
            }

            if (periodo != DateTime.MinValue)
            {
                sentenciaWhere = sentenciaWhere + string.Format(" AND AM.AMPLIFECHA BETWEEN  to_date('{0}','YYYY-MM-DD') and to_date('{1}','YYYY-MM-DD') ",
                    periodo.ToString(ConstantesBase.FormatoFecha), periodo.AddMonths(1).AddDays(-1).ToString(ConstantesBase.FormatoFecha));
            }

            string sqlQuery = string.Format(helper.SqListaAmpliacionCount, sentenciaWhere);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            int cant = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iQregistros = dr.GetOrdinal(helper.Qregistros);
                    cant = Convert.ToInt32(dr.GetValue(iQregistros));
                }
            }
            return cant;
        }
    }
}
