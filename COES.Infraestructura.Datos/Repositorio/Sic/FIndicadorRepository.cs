using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla F_INDICADOR
    /// </summary>
    public class FIndicadorRepository: RepositoryBase, IFIndicadorRepository
    {
        public FIndicadorRepository(string strConn): base(strConn)
        {
        }

        FIndicadorHelper helper = new FIndicadorHelper();
        public void Update(FIndicadorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fechahora, DbType.DateTime, entity.Fechahora);
            dbProvider.AddInParameter(command, helper.Gps, DbType.Int32, entity.Gps);
            dbProvider.AddInParameter(command, helper.Indiccodi, DbType.String, entity.Indiccodi);
            dbProvider.AddInParameter(command, helper.Indicitem, DbType.Int32, entity.Indicitem);
            dbProvider.AddInParameter(command, helper.Indicvalor, DbType.Decimal, entity.Indicvalor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public FIndicadorDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            FIndicadorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FIndicadorDTO> List()
        {
            List<FIndicadorDTO> entitys = new List<FIndicadorDTO>();
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

        public List<FIndicadorDTO> GetByCriteria()
        {
            List<FIndicadorDTO> entitys = new List<FIndicadorDTO>();
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


        public DataTable Get_cadena_transgresion(DateTime dtFecha, int li_gpscodi, string ls_indiccodi)
        {
            string stfecha = dtFecha.Day.ToString() + "-" + dtFecha.Month.ToString() + "-" + dtFecha.Year.ToString();
            string strCommand = string.Format(helper.SqlGetTransgresion, li_gpscodi, stfecha, ls_indiccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);
            var entitys = new DataTable();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys.Load(dr);
            }
            return entitys;
        }


        public int Get_fallaacumulada(DateTime dtFecha, int li_gpscodi, string ls_indiccodi)
        {
            int li_count = 0;
            string ls_periodo = dtFecha.ToString("yyyyMMdd HH:mm:ss").Substring(0, 6);
            string strCommand = string.Format(helper.SqlGetFallaAcumulada, li_gpscodi, ls_indiccodi, ls_periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);
            var entitys = new DataTable();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys.Load(dr);
            }
            if (entitys.Rows.Count > 0) //Si existe, seguimos
            {
                li_count = entitys.Rows.Count;
            }
            return li_count;
        }

        public List<FIndicadorDTO> ListarTransgresionXRango(DateTime fechaIni, DateTime fechaFin, int gpscodi, string indiccodi)
        {
            List<FIndicadorDTO> entitys = new List<FIndicadorDTO>();

            string sql = string.Format(helper.SqlListarTransgresionXRango, gpscodi, indiccodi, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
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

        public int GetFallaAcumuladaXRango(DateTime fechaIni, DateTime fechaFin, int gpscodi, string indiccodi)
        {
            string strCommand = string.Format(helper.SqlGetFallaAcumuladaXRango, gpscodi, indiccodi, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);

            int total = 0;
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) total = Convert.ToInt32(result);

            return total;
        }

        public DataTable Get_cadena_transgresionFrec(DateTime dtFecha, int li_gpscodi, string ls_indiccodi)
        {
            string stfecha = dtFecha.Day.ToString() + "-" + dtFecha.Month.ToString() + "-" + dtFecha.Year.ToString();
            string strCommand = string.Format(helper.SqlGetTransgresionFrec, li_gpscodi, stfecha, ls_indiccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);
            var entitys = new DataTable();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys.Load(dr);
            }
            return entitys;
        }

        public int Get_fallaacumuladaFrec(DateTime dtFecha, int li_gpscodi, string ls_indiccodi)
        {
            int li_count = 0;
            DateTime fechaIni = new DateTime(dtFecha.Year, dtFecha.Month, 1);
            string strCommand = string.Format(helper.SqlGetFallaAcumuladaFrec, li_gpscodi, ls_indiccodi, fechaIni.ToString(ConstantesBase.FormatoFecha), dtFecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);
            var entitys = new DataTable();
            List<DateTime> listaFechaHora = new List<DateTime>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys.Load(dr);
            }

            foreach (DataRow drow in entitys.Rows)
            {
                listaFechaHora.Add(Convert.ToDateTime(drow["FECHAHORA"]));
            }

            if (ls_indiccodi == "O") //sostenida
            {
                List<DateTime> listaDia = listaFechaHora.Select(x => x.Date).Distinct().ToList();
                foreach (var dia in listaDia)
                {
                    var listaXDia = listaFechaHora.Where(x => x.Date == dia);

                    //por dia quitar 1 elemento
                    li_count += (listaXDia.Count() > 1 ? listaXDia.Count() - 1 : 1);
                }
            }
            else
            {
                if (entitys.Rows.Count > 0) //Si existe, seguimos
                {
                    li_count = entitys.Rows.Count;
                }
            }

            return li_count;
        }

        #region PR5

        public List<FIndicadorDTO> ListarReporteVariacionesFrecuenciaSEIN(string empresas, string gps, DateTime dtFechaIni, DateTime dtFechaFin)
        {
            List<FIndicadorDTO> entitys = new List<FIndicadorDTO>();
            string sql = string.Format(helper.SqlListarReporteVariacionesFrecuenciaSEIN, dtFechaIni.ToString(ConstantesBase.FormatoFecha),
                dtFechaFin.ToString(ConstantesBase.FormatoFecha)
                , gps, empresas);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FIndicadorDTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iGpsnomb = dr.GetOrdinal(this.helper.Gpsnomb);
                    if (!dr.IsDBNull(iGpsnomb)) entity.Gpsnomb = dr.GetString(iGpsnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public List<FIndicadorDTO> ListaIndicador(DateTime fecIni, string indiccodi)
        {
            List<FIndicadorDTO> entitys = new List<FIndicadorDTO>();
            string sql = string.Format(helper.SqlListaIndicador, fecIni.ToString(ConstantesBase.FormatoFecha), indiccodi);
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

        public List<FIndicadorDTO> ListaIndicadorAcu(DateTime fecIni, string indiccodi)
        {
            List<FIndicadorDTO> entitys = new List<FIndicadorDTO>();
            string sql = string.Format(helper.SqlListaIndicadorAcu, fecIni.Month, indiccodi, fecIni.Year);
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
        #endregion
    }
}
