using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class SicCostomarginalGraficosRepository : RepositoryBase, ISicCostomarginalGraficosRepository
    {

        public SicCostomarginalGraficosRepository(string strConn)
            : base(strConn)
        {
        }

        SicCostomarginalGraficosHelper helper = new SicCostomarginalGraficosHelper();

        public List<BarraDTO> ListarBarrasPorCMG(CostoMarginalDTO parametro)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            string query = string.Format(helper.SqlListarBarrasPorCostoMarginal, parametro.DiaInicio, parametro.DiaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, parametro.PeriCodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, parametro.CosMarVersion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraDTO entidad = new BarraDTO();
                    entidad.BarrCodi = Convert.ToInt32(dr["barrCodi"].ToString());
                    entidad.BarrNombre = Convert.ToString(dr["barrNombre"].ToString());
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }

        public List<BarraDTO> ListarBarrasPorCMGDiario(CostoMarginalDTO parametro)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            string query = string.Format(helper.SqlListarBarrasPorCostoMarginalProm,
                parametro.FechaInicio.Year,
                parametro.FechaInicio.Month,
                parametro.FechaInicio.Day,
                parametro.FechaFin.Year,
                parametro.FechaFin.Month,
                parametro.FechaFin.Day);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraDTO entidad = new BarraDTO();
                    entidad.BarrCodi = Convert.ToInt32(dr["barrCodi"].ToString());
                    entidad.BarrNombre = Convert.ToString(dr["barrNombre"].ToString());
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }
        public List<BarraDTO> ListarBarrasPorCMGMensual(CostoMarginalDTO parametro)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            string query = string.Format(helper.SqlListarBarrasPorCostoMarginalMensual,
                parametro.PeriCodi,
                parametro.PeriCodiFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraDTO entidad = new BarraDTO();
                    entidad.BarrCodi = Convert.ToInt32(dr["barrCodi"].ToString());
                    entidad.BarrNombre = Convert.ToString(dr["barrNombre"].ToString());
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }
        public DataTable ListarCostoMarginalTotalPorBarras_NEW(CostoMarginalDTO parametro)
        {
            DataTable tbl = new DataTable();
            string query = string.Format(helper.SqlListarCostoMarginalTotalPorBarrasNew, string.Join(",", parametro.ListaBarras), parametro.DiaInicio, parametro.DiaFin);
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, parametro.PeriCodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, parametro.CosMarVersion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;
        }
        public List<CostoMarginalDTO> ListarCostoMarginalTotalPorBarras(CostoMarginalDTO parametro)
        {
            string query = string.Format(helper.SqlListarCostoMarginalTotalPorBarras, string.Join(",", parametro.ListaBarras), parametro.FechaInicio.ToString(ConstantesBase.FormatoFecha), parametro.FechaFin.ToString(ConstantesBase.FormatoFecha));
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalDTO entidad = new CostoMarginalDTO();
                    entidad.BarrCodi = Convert.ToInt32(dr["barrcodi"].ToString());
                    entidad.FechaColumna = Convert.ToString(dr["FechaColumna"].ToString());
                    entidad.CMGRTotal = dr["CMGRTOTAL"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["CMGRTOTAL"].ToString());
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }
        public DataTable ListarCostoMarginalCongestionPorBarras_NEW(int tipoCosto, CostoMarginalDTO parametro)
        {
            DataTable tbl = new DataTable();
            string query = string.Format(helper.SqlListarCostoMarginalCongestionPorBarrasNew, string.Join(",", parametro.ListaBarras), parametro.DiaInicio, parametro.DiaFin);
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, parametro.PeriCodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, parametro.CosMarVersion);
            dbProvider.AddInParameter(command, helper.TipCosto, DbType.Int32, tipoCosto);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;
        }
        public List<CostoMarginalDTO> ListarCostoMarginalCongestionPorBarras(CostoMarginalDTO parametro)
        {
            string query = string.Format(helper.SqlListarCostoMarginalCongestionPorBarras, string.Join(",", parametro.ListaBarras), parametro.FechaInicio.ToString(ConstantesBase.FormatoFecha), parametro.FechaFin.ToString(ConstantesBase.FormatoFecha));
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalDTO entidad = new CostoMarginalDTO();
                    entidad.BarrCodi = Convert.ToInt32(dr["barrcodi"].ToString());
                    entidad.FechaColumna = Convert.ToString(dr["FechaColumna"].ToString());
                    entidad.CMGRCongestion = dr["CMGRCONGESTION"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["CMGRCONGESTION"].ToString());
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }
        public List<CostoMarginalDTO> ListarCostoMarginalEnergiaPorBarras(CostoMarginalDTO parametro)
        {
            string query = string.Format(helper.SqlListarCostoMarginalEnergiaPorBarras, string.Join(",", parametro.ListaBarras), parametro.FechaInicio.ToString(ConstantesBase.FormatoFecha), parametro.FechaFin.ToString(ConstantesBase.FormatoFecha));
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalDTO entidad = new CostoMarginalDTO();
                    entidad.BarrCodi = Convert.ToInt32(dr["barrcodi"].ToString());
                    entidad.FechaColumna = Convert.ToString(dr["FechaColumna"].ToString());
                    entidad.CMGREnergia = dr["CMGRENERGIA"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["CMGRENERGIA"].ToString());
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }


        public DataTable ListarCostoMarginalDesviacion(CostoMarginalDesviacionDTO parametro)
        {

            DataTable tbl = new DataTable();
            string query = string.Format(helper.SqlListarCostoMarginalDesviacion,
                parametro.PeriCodi1,
               string.Join(",", parametro.Version1Array),
               string.Join(",", parametro.BarrasArray),
                string.Join(",", parametro.Dia1Array),
                parametro.PeriCodi2,
                string.Join(",", parametro.Version2Array),
                string.Join(",", parametro.BarrasArray),
                string.Join(",", parametro.Dia2Array)
                );
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;

        }
        public DataTable ListarCostoMarginalCongestionDesviacion(CostoMarginalDesviacionDTO parametro)
        {
            DataTable tbl = new DataTable();
            string query = string.Format(helper.SqlListarCostoMarginalCongestionDesviacion,
                parametro.PeriCodi1,
               string.Join(",", parametro.Version1Array),
               string.Join(",", parametro.BarrasArray),
                string.Join(",", parametro.Dia1Array),
                parametro.PeriCodi2,
                string.Join(",", parametro.Version2Array),
                string.Join(",", parametro.BarrasArray),
                string.Join(",", parametro.Dia2Array)
                );
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.TipCosto, DbType.Int32, parametro.TipCosto);
            dbProvider.AddInParameter(command, helper.TipCosto, DbType.Int32, parametro.TipCosto);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;

        }

        public List<CostoMarginalGraficoValoresDTO> ListarPromedioMarginalTotalDiario(CostoMarginalDTO parametro)
        {
            List<CostoMarginalGraficoValoresDTO> entitys = new List<CostoMarginalGraficoValoresDTO>();
            string query = string.Format(helper.SqlListarPromedioMarginalTotalDiario, string.Join(",", parametro.ListaBarras), parametro.FechaInicio.ToString(ConstantesBase.FormatoFecha), parametro.FechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalGraficoValoresDTO entidad = new CostoMarginalGraficoValoresDTO();
                    entidad.BarrCodi = Convert.ToInt32(dr["barrcodi"].ToString());
                    entidad.FechaIntervalo = Convert.ToDateTime(dr["CMGRFECHA"].ToString());
                    entidad.CMGRPromedio = dr["COSMARPROMEDIODIA"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["COSMARPROMEDIODIA"].ToString());
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }
        public List<CostoMarginalGraficoValoresDTO> ListarPromedioMarginalTotalMensual(CostoMarginalDTO parametro)
        {
            List<CostoMarginalGraficoValoresDTO> entitys = new List<CostoMarginalGraficoValoresDTO>();
            string query =
                string.Format(helper.SqlListarPromedioMarginalTotalMensual,
                string.Join(",", parametro.ListaBarras), parametro.PeriCodi, parametro.PeriCodiFin);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalGraficoValoresDTO entidad = new CostoMarginalGraficoValoresDTO();
                    entidad.BarrCodi = Convert.ToInt32(dr["barrcodi"].ToString());
                    entidad.FechaIntervalo = Convert.ToDateTime(dr["CMGRFECHA"].ToString());
                    entidad.CMGRPromedio = dr["cosmarpromedio"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["cosmarpromedio"].ToString());
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }
        public List<CostoMarginalGraficoValoresDTO> ListarPromedioMarginalCongeneMensual(CostoMarginalDTO parametro)
        {
            List<CostoMarginalGraficoValoresDTO> entitys = new List<CostoMarginalGraficoValoresDTO>();
            string query =
                string.Format(helper.SqlListarPromedioMarginalCongeneMensual,
                string.Join(",", parametro.ListaBarras), parametro.PeriCodi, parametro.PeriCodiFin, parametro.tipoCongene);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalGraficoValoresDTO entidad = new CostoMarginalGraficoValoresDTO();
                    entidad.BarrCodi = Convert.ToInt32(dr["barrcodi"].ToString());
                    entidad.FechaIntervalo = Convert.ToDateTime(dr["CMGRFECHA"].ToString());
                    entidad.CMGRPromedio = dr["cosmarpromedio"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["cosmarpromedio"].ToString());
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }
        public List<CostoMarginalGraficoValoresDTO> ListarPromedioMarginalCongeneDiario(CostoMarginalDTO parametro)
        {
            List<CostoMarginalGraficoValoresDTO> entitys = new List<CostoMarginalGraficoValoresDTO>();
            string query =
                string.Format(helper.SqListarPromedioMarginalCongeneDiario,
                string.Join(",", parametro.ListaBarras), parametro.FechaInicio.ToString(ConstantesBase.FormatoFecha), parametro.FechaFin.ToString(ConstantesBase.FormatoFecha), parametro.tipoCongene);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalGraficoValoresDTO entidad = new CostoMarginalGraficoValoresDTO();
                    entidad.BarrCodi = Convert.ToInt32(dr["barrcodi"].ToString());
                    entidad.FechaIntervalo = Convert.ToDateTime(dr["CMGRFECHA"].ToString());
                    entidad.CMGRPromedio = dr["cosmarpromedio"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["cosmarpromedio"].ToString());
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }


        public List<BarraDTO> ListarBarrasPorArray(CostoMarginalDTO parametro)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlListarBarrasPorCodigo, string.Join(",", parametro.ListaBarras)));
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraDTO entidad = new BarraDTO();
                    entidad.BarrCodi = Convert.ToInt32(dr["barrCodi"].ToString());
                    entidad.BarrNombre = Convert.ToString(dr["barrNombre"].ToString());
                    entitys.Add(entidad);
                }
            }
            return entitys;
        }


    }
}
