using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VTP_INGRESO_POTEFR_DETALLE
    /// </summary>
    public class VtpIngresoPotefrDetalleRepository : RepositoryBase, IVtpIngresoPotefrDetalleRepository
    {
        public VtpIngresoPotefrDetalleRepository(string strConn)
            : base(strConn)
        {
        }

        VtpIngresoPotefrDetalleHelper helper = new VtpIngresoPotefrDetalleHelper();

        public int Save(VtpIngresoPotefrDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ipefrdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, entity.Ipefrcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cenequicodi, DbType.Int32, entity.Cenequicodi);
            dbProvider.AddInParameter(command, helper.Uniequicodi, DbType.Int32, entity.Uniequicodi);
            dbProvider.AddInParameter(command, helper.Ipefrdpoteefectiva, DbType.Decimal, entity.Ipefrdpoteefectiva);
            dbProvider.AddInParameter(command, helper.Ipefrdpotefirme, DbType.Decimal, entity.Ipefrdpotefirme);
            dbProvider.AddInParameter(command, helper.Ipefrdpotefirmeremunerable, DbType.Decimal, entity.Ipefrdpotefirmeremunerable);
            dbProvider.AddInParameter(command, helper.Ipefrdusucreacion, DbType.String, entity.Ipefrdusucreacion);
            dbProvider.AddInParameter(command, helper.Ipefrdfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Ipefrdusumodificacion, DbType.String, entity.Ipefrdusumodificacion);
            dbProvider.AddInParameter(command, helper.Ipefrdfecmodificacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Unigrupocodi, DbType.Int32, entity.Unigrupocodi);
            dbProvider.AddInParameter(command, helper.Ipefrdunidadnomb, DbType.String, entity.Ipefrdunidadnomb);
            dbProvider.AddInParameter(command, helper.Ipefrdficticio, DbType.Int32, entity.Ipefrdficticio);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpIngresoPotefrDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ipefrdcodi, DbType.Int32, entity.Ipefrdcodi);
            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, entity.Ipefrcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cenequicodi, DbType.Int32, entity.Cenequicodi);
            dbProvider.AddInParameter(command, helper.Uniequicodi, DbType.Int32, entity.Uniequicodi);
            dbProvider.AddInParameter(command, helper.Ipefrdpoteefectiva, DbType.Decimal, entity.Ipefrdpoteefectiva);
            dbProvider.AddInParameter(command, helper.Ipefrdpotefirme, DbType.Decimal, entity.Ipefrdpotefirme);
            dbProvider.AddInParameter(command, helper.Ipefrdpotefirmeremunerable, DbType.Decimal, entity.Ipefrdpotefirmeremunerable);
            dbProvider.AddInParameter(command, helper.Ipefrdusucreacion, DbType.String, entity.Ipefrdusucreacion);
            dbProvider.AddInParameter(command, helper.Ipefrdfeccreacion, DbType.DateTime, entity.Ipefrdfeccreacion);
            dbProvider.AddInParameter(command, helper.Ipefrdusumodificacion, DbType.String, entity.Ipefrdusumodificacion);
            dbProvider.AddInParameter(command, helper.Ipefrdfecmodificacion, DbType.DateTime, entity.Ipefrdfecmodificacion);
            dbProvider.AddInParameter(command, helper.Unigrupocodi, DbType.Int32, entity.Unigrupocodi);
            dbProvider.AddInParameter(command, helper.Ipefrdunidadnomb, DbType.String, entity.Ipefrdunidadnomb);
            dbProvider.AddInParameter(command, helper.Ipefrdficticio, DbType.Int32, entity.Ipefrdficticio);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ipefrdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ipefrdcodi, DbType.Int32, ipefrdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int ipefrcodi, int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeletebyCriteria);

            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, ipefrcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);

        }

        public void DeleteByCriteriaVersion(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeletebyCriteriaVersion);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);

        }

        public VtpIngresoPotefrDetalleDTO GetById(int ipefrdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ipefrdcodi, DbType.Int32, ipefrdcodi);
            VtpIngresoPotefrDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpIngresoPotefrDetalleDTO> List()
        {
            List<VtpIngresoPotefrDetalleDTO> entitys = new List<VtpIngresoPotefrDetalleDTO>();
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

        public List<VtpIngresoPotefrDetalleDTO> GetByCriteria(int ipefrcodi, int pericodi, int recpotcodi)
        {
            List<VtpIngresoPotefrDetalleDTO> entitys = new List<VtpIngresoPotefrDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, ipefrcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotefrDetalleDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iCenequinomb = dr.GetOrdinal(helper.Cenequinomb);
                    if (!dr.IsDBNull(iCenequinomb)) entity.Cenequinomb = Convert.ToString(dr.GetValue(iCenequinomb));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }

                return entitys;
            }
        }

        public List<VtpIngresoPotefrDetalleDTO> GetByCriteriaSumCentral(int ipefrcodi, int pericodi, int recpotcodi)
        {
            List<VtpIngresoPotefrDetalleDTO> entitys = new List<VtpIngresoPotefrDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaSumCentral);

            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, ipefrcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotefrDetalleDTO entity = new VtpIngresoPotefrDetalleDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iCenequicodi = dr.GetOrdinal(helper.Cenequicodi);
                    if (!dr.IsDBNull(iCenequicodi)) entity.Cenequicodi = Convert.ToInt32(dr.GetValue(iCenequicodi));

                    int iCenequinomb = dr.GetOrdinal(helper.Cenequinomb);
                    if (!dr.IsDBNull(iCenequinomb)) entity.Cenequinomb = Convert.ToString(dr.GetValue(iCenequinomb));

                    int iUnigrupocodi = dr.GetOrdinal(helper.Unigrupocodi);
                    if (!dr.IsDBNull(iUnigrupocodi)) entity.Unigrupocodi = Convert.ToInt32(dr.GetValue(iUnigrupocodi));
                    int iUniequicodi = dr.GetOrdinal(helper.Uniequicodi);
                    if (!dr.IsDBNull(iUniequicodi)) entity.Uniequicodi = Convert.ToInt32(dr.GetValue(iUniequicodi));

                    int iIpefrdunidadnomb = dr.GetOrdinal(helper.Ipefrdunidadnomb);
                    if (!dr.IsDBNull(iIpefrdunidadnomb)) entity.Ipefrdunidadnomb = Convert.ToString(dr.GetValue(iIpefrdunidadnomb));

                    int iIpefrdficticio = dr.GetOrdinal(helper.Ipefrdficticio);
                    if (!dr.IsDBNull(iIpefrdficticio)) entity.Ipefrdficticio = Convert.ToInt32(dr.GetValue(iIpefrdficticio));

                    int iIpefrdpotefirmeremunerable = dr.GetOrdinal(helper.Ipefrdpotefirmeremunerable);
                    if (!dr.IsDBNull(iIpefrdpotefirmeremunerable)) entity.Ipefrdpotefirmeremunerable = dr.GetDecimal(iIpefrdpotefirmeremunerable);

                    entitys.Add(entity);
                }

                return entitys;
            }
        }

        #region SIOSEIN2

        public List<VtpIngresoPotefrDetalleDTO> ObtenerPotenciaEFRSumPorEmpresa(string ipefrcodis, int periCodi, int recpotcodi)
        {
            List<VtpIngresoPotefrDetalleDTO> entitys = new List<VtpIngresoPotefrDetalleDTO>();
            var query = string.Format(helper.SqlObtenerPotenciaEFRSumPorEmpresa, ipefrcodis, periCodi, recpotcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotefrDetalleDTO entity = new VtpIngresoPotefrDetalleDTO();

                    int iIpefrcodi = dr.GetOrdinal(helper.Ipefrcodi);
                    if (!dr.IsDBNull(iIpefrcodi)) entity.Ipefrcodi = Convert.ToInt32(dr.GetValue(iIpefrcodi));

                    int iPericodi = dr.GetOrdinal(helper.Pericodi);
                    if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

                    int iRecpotcodi = dr.GetOrdinal(helper.Recpotcodi);
                    if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iIpefrdpoteefectiva = dr.GetOrdinal(helper.Ipefrdpoteefectiva);
                    if (!dr.IsDBNull(iIpefrdpoteefectiva)) entity.Ipefrdpoteefectiva = dr.GetDecimal(iIpefrdpoteefectiva);

                    int iIpefrdpotefirme = dr.GetOrdinal(helper.Ipefrdpotefirme);
                    if (!dr.IsDBNull(iIpefrdpotefirme)) entity.Ipefrdpotefirme = dr.GetDecimal(iIpefrdpotefirme);

                    int iIpefrdpotefirmeremunerable = dr.GetOrdinal(helper.Ipefrdpotefirmeremunerable);
                    if (!dr.IsDBNull(iIpefrdpotefirmeremunerable)) entity.Ipefrdpotefirmeremunerable = dr.GetDecimal(iIpefrdpotefirmeremunerable);

                    entitys.Add(entity);
                }

                return entitys;
            }
        }
        #endregion

        #region SIOSEIN-PRIE-2021
        public List<VtpIngresoPotefrDetalleDTO> GetByCriteriaSinPRIE(DateTime dFecha, int ipefrcodi, int pericodi, int recpotcodi)
        {
            List<VtpIngresoPotefrDetalleDTO> entitys = new List<VtpIngresoPotefrDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaSinPRIE);

            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, ipefrcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Ipefrdfeccreacion, DbType.DateTime, dFecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotefrDetalleDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iCenequinomb = dr.GetOrdinal(helper.Cenequinomb);
                    if (!dr.IsDBNull(iCenequinomb)) entity.Cenequinomb = Convert.ToString(dr.GetValue(iCenequinomb));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = Convert.ToString(dr.GetValue(iOsinergcodi));

                    int iEquicodivtp = dr.GetOrdinal(helper.Equicodivtp);
                    if (!dr.IsDBNull(iEquicodivtp)) entity.Cenequicodi = Convert.ToInt32(dr.GetValue(iEquicodivtp));

                    entitys.Add(entity);
                }

                return entitys;
            }
        }
        #endregion

        #region PrimasRER.2023
        public List<VtpIngresoPotefrDetalleDTO> GetCentralUnidadByEmpresa(int emprcodi)
        {
            List<VtpIngresoPotefrDetalleDTO> entitys = new List<VtpIngresoPotefrDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetCentralUnidadByEmpresa);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotefrDetalleDTO entity = new VtpIngresoPotefrDetalleDTO();

                    int iCenequicodi = dr.GetOrdinal(helper.Cenequicodi);
                    if (!dr.IsDBNull(iCenequicodi)) entity.Cenequicodi = Convert.ToInt32(dr.GetValue(iCenequicodi));

                    int iCenequinomb = dr.GetOrdinal(helper.Cenequinomb);
                    if (!dr.IsDBNull(iCenequinomb)) entity.Cenequinomb = Convert.ToString(dr.GetValue(iCenequinomb));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }

}
