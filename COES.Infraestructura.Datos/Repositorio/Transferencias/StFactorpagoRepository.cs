using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ST_FACTORPAGO
    /// </summary>
    public class StFactorpagoRepository : RepositoryBase, IStFactorpagoRepository
    {
        public StFactorpagoRepository(string strConn)
            : base(strConn)
        {
        }

        StFactorpagoHelper helper = new StFactorpagoHelper();

        public int Save(StFactorpagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Facpagcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, entity.Stcntgcodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);
            dbProvider.AddInParameter(command, helper.Facpagfggl, DbType.Decimal, entity.Facpagfggl);
            dbProvider.AddInParameter(command, helper.Facpagreajuste, DbType.Decimal, entity.Facpagreajuste);
            dbProvider.AddInParameter(command, helper.Facpagfgglajuste, DbType.Decimal, entity.Facpagfgglajuste);
            dbProvider.AddInParameter(command, helper.Facpagusucreacion, DbType.String, entity.Facpagusucreacion);
            dbProvider.AddInParameter(command, helper.Facpagfeccreacion, DbType.DateTime, entity.Facpagfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StFactorpagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);//

            dbProvider.AddInParameter(command, helper.Facpagfggl, DbType.Decimal, entity.Facpagfggl);
            dbProvider.AddInParameter(command, helper.Facpagfgglajuste, DbType.Decimal, entity.Facpagfgglajuste);
            dbProvider.AddInParameter(command, helper.Facpagreajuste, DbType.Int32, entity.Facpagreajuste);
            dbProvider.AddInParameter(command, helper.Facpagcodi, DbType.Int32, entity.Facpagcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StFactorpagoDTO GetById(int facpagcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Facpagcodi, DbType.Int32, facpagcodi);
            StFactorpagoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public StFactorpagoDTO GetByIdFK(int strecacodi, int stcntgcodi, int stcompcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdFK);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, stcntgcodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);
            StFactorpagoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StFactorpagoDTO> List(int strecacodi, int stcompcodi)
        {
            List<StFactorpagoDTO> entitys = new List<StFactorpagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<StFactorpagoDTO> GetByCriteria(int strecacodi, int stcompcodi)
        {
            List<StFactorpagoDTO> entitys = new List<StFactorpagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<StFactorpagoDTO> GetByCriteriaInicialReporte(int strecacodi)
        {
            List<StFactorpagoDTO> entitys = new List<StFactorpagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaInicialReporte);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StFactorpagoDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int ielecmpmonto = dr.GetOrdinal(this.helper.Elecmpmonto);
                    if (!dr.IsDBNull(ielecmpmonto)) entity.Elecmpmonto = Convert.ToDecimal(dr.GetValue(ielecmpmonto));

                    int iStcompcodelemento = dr.GetOrdinal(this.helper.Stcompcodelemento);
                    if (!dr.IsDBNull(iStcompcodelemento)) entity.Stcompcodelemento = dr.GetString(iStcompcodelemento); //

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StFactorpagoDTO> GetByCriteriaReporte(int strecacodi)
        {
            List<StFactorpagoDTO> entitys = new List<StFactorpagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaReporte);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StFactorpagoDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int ielecmpmonto = dr.GetOrdinal(this.helper.Elecmpmonto);
                    if (!dr.IsDBNull(ielecmpmonto)) entity.Elecmpmonto = Convert.ToDecimal(dr.GetValue(ielecmpmonto));

                    int iStcompcodelemento = dr.GetOrdinal(this.helper.Stcompcodelemento);
                    if (!dr.IsDBNull(iStcompcodelemento)) entity.Stcompcodelemento = dr.GetString(iStcompcodelemento); //

                    int ipagasgcmggl = dr.GetOrdinal(this.helper.Pagasgcmggl);
                    if (!dr.IsDBNull(ipagasgcmggl)) entity.Pagasgcmggl = Convert.ToDecimal(dr.GetValue(ipagasgcmggl));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StFactorpagoDTO> GetByCriteriaReporteFactorPago(int strecacodi)
        {
            List<StFactorpagoDTO> entitys = new List<StFactorpagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaReporteFactorPago);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StFactorpagoDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iStcompcodelemento = dr.GetOrdinal(this.helper.Stcompcodelemento);
                    if (!dr.IsDBNull(iStcompcodelemento)) entity.Stcompcodelemento = dr.GetString(iStcompcodelemento); //

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region SIOSEIN2

        public List<StFactorpagoDTO> ObtenerFactorPagoParticipacion(int strecacodi, int stcompcodi)
        {
            List<StFactorpagoDTO> entitys = new List<StFactorpagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerFactorPagoParticipacion);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StFactorpagoDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iStcompcodelemento = dr.GetOrdinal(this.helper.Stcompcodelemento);
                    if (!dr.IsDBNull(iStcompcodelemento)) entity.Stcompcodelemento = dr.GetString(iStcompcodelemento);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<StFactorpagoDTO> ObtenerCompensacionMensual(int strecacodi, int stcompcodi)
        {
            List<StFactorpagoDTO> entitys = new List<StFactorpagoDTO>();
            var query = string.Format(helper.SqlObtenerCompensacionMensual, strecacodi, stcompcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StFactorpagoDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int ielecmpmonto = dr.GetOrdinal(this.helper.Elecmpmonto);
                    if (!dr.IsDBNull(ielecmpmonto)) entity.Elecmpmonto = Convert.ToDecimal(dr.GetValue(ielecmpmonto));

                    int iStcompcodelemento = dr.GetOrdinal(this.helper.Stcompcodelemento);
                    if (!dr.IsDBNull(iStcompcodelemento)) entity.Stcompcodelemento = dr.GetString(iStcompcodelemento);

                    int ipagasgcmggl = dr.GetOrdinal(this.helper.Pagasgcmggl);
                    if (!dr.IsDBNull(ipagasgcmggl)) entity.Pagasgcmggl = Convert.ToDecimal(dr.GetValue(ipagasgcmggl));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}
