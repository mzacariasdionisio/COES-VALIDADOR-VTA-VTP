using COES.Base.Core;
using COES.Dominio.DTO.Sic;
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
    public class IndTransporteDetalleRepository : RepositoryBase, IIndTransporteDetalleRepository
    {
        public IndTransporteDetalleRepository(string strConn) : base(strConn)
        {
        }

        IndTransporteDetalleHelper helper = new IndTransporteDetalleHelper();

        public int Save(IndTransporteDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tnsdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpctnscodi, DbType.Int32, entity.Cpctnscodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Emprnombalter, DbType.String, entity.Emprnombalter);
            dbProvider.AddInParameter(command, helper.Tnsdetfecha, DbType.Date, entity.Tnsdetfecha);
            dbProvider.AddInParameter(command, helper.Tnsdetcntadquirida, DbType.Decimal, entity.Tnsdetcntadquirida);
            dbProvider.AddInParameter(command, helper.Tnsdetprctransferencia, DbType.Decimal, entity.Tnsdetprctransferencia);
            dbProvider.AddInParameter(command, helper.Tnsdetptosuministro, DbType.String, entity.Tnsdetptosuministro);
            dbProvider.AddInParameter(command, helper.Tnsdetcompraventa, DbType.Int32, entity.TnsdetCompraventa);
            dbProvider.AddInParameter(command, helper.Tnsdetusucreacion, DbType.String, entity.Tnsdetusucreacion);
            dbProvider.AddInParameter(command, helper.Tnsdetfeccreacion, DbType.DateTime, entity.Tnsdetfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void DeleteByCapacidadTransporte(int cpctnscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCapacidadTransporte);

            dbProvider.AddInParameter(command, helper.Cpctnscodi, DbType.Int32, cpctnscodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<IndTransporteDetalleDTO> ListTransporteDetalle(int cpctnscodi)
        {
            IndTransporteDetalleDTO entity = new IndTransporteDetalleDTO();
            List<IndTransporteDetalleDTO> entitys = new List<IndTransporteDetalleDTO>();
            string query = string.Format(helper.SqlListTransporteDetalle, cpctnscodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndTransporteDetalleDTO();

                    int iTnsdetdescripcion = dr.GetOrdinal(helper.Tnsdetdescripcion);
                    if (!dr.IsDBNull(iTnsdetdescripcion)) entity.Tnsdetdescripcion = dr.GetString(iTnsdetdescripcion);

                    int iTnsdetfecha = dr.GetOrdinal(helper.Tnsdetfecha);
                    if (!dr.IsDBNull(iTnsdetfecha)) entity.Tnsdetfecha = dr.GetDateTime(iTnsdetfecha);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprnombalter = dr.GetOrdinal(helper.Emprnombalter);
                    if (!dr.IsDBNull(iEmprnombalter)) entity.Emprnombalter = dr.GetString(iEmprnombalter);

                    int iTnsdetptosuministro = dr.GetOrdinal(helper.Tnsdetptosuministro);
                    if (!dr.IsDBNull(iTnsdetptosuministro)) entity.Tnsdetptosuministro = dr.GetString(iTnsdetptosuministro);

                    int iTnsdetcntadquirida = dr.GetOrdinal(helper.Tnsdetcntadquirida);
                    if (!dr.IsDBNull(iTnsdetcntadquirida)) entity.Tnsdetcntadquirida = Convert.ToDecimal(dr.GetValue(iTnsdetcntadquirida));

                    int iTnsdetprctransferencia = dr.GetOrdinal(helper.Tnsdetprctransferencia);
                    if (!dr.IsDBNull(iTnsdetprctransferencia)) entity.Tnsdetprctransferencia = Convert.ToDecimal(dr.GetValue(iTnsdetprctransferencia));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndTransporteDetalleDTO> ListTransDetalleJoinCapacidadTrans(int emprcodi, int ipericodi)
        {
            IndTransporteDetalleDTO entity = new IndTransporteDetalleDTO();
            List<IndTransporteDetalleDTO> entitys = new List<IndTransporteDetalleDTO>();
            string query = string.Format(helper.SqlListTransDetalleJoinCapacidadTrans, emprcodi, ipericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndTransporteDetalleDTO();

                    int iTnsdetdescripcion = dr.GetOrdinal(helper.Tnsdetdescripcion);
                    if (!dr.IsDBNull(iTnsdetdescripcion)) entity.Tnsdetdescripcion = dr.GetString(iTnsdetdescripcion);

                    int iTnsdetfecha = dr.GetOrdinal(helper.Tnsdetfecha);
                    if (!dr.IsDBNull(iTnsdetfecha)) entity.Tnsdetfecha = dr.GetDateTime(iTnsdetfecha);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTnsdetptosuministro = dr.GetOrdinal(helper.Tnsdetptosuministro);
                    if (!dr.IsDBNull(iTnsdetptosuministro)) entity.Tnsdetptosuministro = dr.GetString(iTnsdetptosuministro);

                    int iTnsdetcntadquirida = dr.GetOrdinal(helper.Tnsdetcntadquirida);
                    if (!dr.IsDBNull(iTnsdetcntadquirida)) entity.Tnsdetcntadquirida = Convert.ToDecimal(dr.GetValue(iTnsdetcntadquirida));

                    int iTnsdetprctransferencia = dr.GetOrdinal(helper.Tnsdetprctransferencia);
                    if (!dr.IsDBNull(iTnsdetprctransferencia)) entity.Tnsdetprctransferencia = Convert.ToDecimal(dr.GetValue(iTnsdetprctransferencia));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndTransporteDetalleDTO> ReporteIncumplimientoByPeriodo(int ipericodi)
        {
            IndTransporteDetalleDTO entity = new IndTransporteDetalleDTO();
            List<IndTransporteDetalleDTO> entitys = new List<IndTransporteDetalleDTO>();
            string query = string.Format(helper.SqlReporteIncumplimientoByPeriodo, ipericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndTransporteDetalleDTO();

                    int iFechacumplimiento = dr.GetOrdinal(helper.Fechacumplimiento);
                    if (!dr.IsDBNull(iFechacumplimiento)) entity.FechaCumplimiento = dr.GetString(iFechacumplimiento);

                    int iEmpresacompra = dr.GetOrdinal(helper.Empresacompra);
                    if (!dr.IsDBNull(iEmpresacompra)) entity.EmpresaCompra = dr.GetString(iEmpresacompra);

                    int iCantidadcompra = dr.GetOrdinal(helper.Cantidadcompra);
                    if (!dr.IsDBNull(iCantidadcompra)) entity.CantidadCompra = Convert.ToDecimal(dr.GetValue(iCantidadcompra));

                    int iEmpresaventa = dr.GetOrdinal(helper.Empresaventa);
                    if (!dr.IsDBNull(iEmpresaventa)) entity.EmpresaVenta = dr.GetString(iEmpresaventa);

                    int iCantidadventa = dr.GetOrdinal(helper.Cantidadventa);
                    if (!dr.IsDBNull(iCantidadventa)) entity.CantidadVenta = Convert.ToDecimal(dr.GetValue(iCantidadventa));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
