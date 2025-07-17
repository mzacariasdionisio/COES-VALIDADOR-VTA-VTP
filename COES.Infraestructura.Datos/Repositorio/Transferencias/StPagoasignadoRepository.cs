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
    /// Clase de acceso a datos de la tabla ST_PAGOASIGNADO
    /// </summary>
    public class StPagoasignadoRepository : RepositoryBase, IStPagoasignadoRepository
    {
        public StPagoasignadoRepository(string strConn)
            : base(strConn)
        {
        }

        StPagoasignadoHelper helper = new StPagoasignadoHelper();

        public int Save(StPagoasignadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pagasgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, entity.Stcntgcodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);
            dbProvider.AddInParameter(command, helper.Pagasgcmggl, DbType.Decimal, entity.Pagasgcmggl);
            dbProvider.AddInParameter(command, helper.Pagasgcmgglp, DbType.Decimal, entity.Pagasgcmgglp);
            dbProvider.AddInParameter(command, helper.Pagasgcmgglfinal, DbType.Decimal, entity.Pagasgcmgglfinal);
            dbProvider.AddInParameter(command, helper.Pagasgusucreacion, DbType.String, entity.Pagasgusucreacion);
            dbProvider.AddInParameter(command, helper.Pagasgfeccreacion, DbType.DateTime, entity.Pagasgfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StPagoasignadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pagasgcodi, DbType.Int32, entity.Pagasgcodi);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, entity.Stcntgcodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);
            dbProvider.AddInParameter(command, helper.Pagasgcmggl, DbType.Decimal, entity.Pagasgcmggl);
            dbProvider.AddInParameter(command, helper.Pagasgcmgglp, DbType.Decimal, entity.Pagasgcmgglp);
            dbProvider.AddInParameter(command, helper.Pagasgcmgglfinal, DbType.Decimal, entity.Pagasgcmgglfinal);
            dbProvider.AddInParameter(command, helper.Pagasgusucreacion, DbType.String, entity.Pagasgusucreacion);
            dbProvider.AddInParameter(command, helper.Pagasgfeccreacion, DbType.DateTime, entity.Pagasgfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCompensacion(int stcompcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCompensacion);

            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StPagoasignadoDTO GetById(int facpagcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pagasgcodi, DbType.Int32, facpagcodi);
            StPagoasignadoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StPagoasignadoDTO> List(int strecacodi)
        {
            List<StPagoasignadoDTO> entitys = new List<StPagoasignadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public StPagoasignadoDTO GetByCriteria(int stcntgcodi, int stcompcodi)
        {
            List<StPagoasignadoDTO> entitys = new List<StPagoasignadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, stcntgcodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);

            StPagoasignadoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StPagoasignadoDTO> GetByCriteriaReporte(int strecacodi)
        {
            List<StPagoasignadoDTO> entitys = new List<StPagoasignadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaReporte);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StPagoasignadoDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iStcompcodelemento = dr.GetOrdinal(this.helper.Stcompcodelemento);
                    if (!dr.IsDBNull(iStcompcodelemento)) entity.Stcompcodelemento = dr.GetString(iStcompcodelemento);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StPagoasignadoDTO> ListEmpresaGeneradores(int strecacodi)
        {
            List<StPagoasignadoDTO> entitys = new List<StPagoasignadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaGeneradores);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StPagoasignadoDTO entity = new StPagoasignadoDTO();

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprruc = dr.GetOrdinal(this.helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StPagoasignadoDTO> ListEmpresaSistemas(int strecacodi)
        {
            List<StPagoasignadoDTO> entitys = new List<StPagoasignadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaSistemas);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StPagoasignadoDTO entity = new StPagoasignadoDTO();

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iSistrnnombre = dr.GetOrdinal(this.helper.Sistrnnombre);
                    if (!dr.IsDBNull(iSistrnnombre)) entity.Sistrnnombre = dr.GetString(iSistrnnombre);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iSistrncodi = dr.GetOrdinal(this.helper.Sistrncodi);
                    if (!dr.IsDBNull(iSistrncodi)) entity.Sistrncodi = Convert.ToInt32(dr.GetValue(iSistrncodi));

                    int iEmprruc = dr.GetOrdinal(this.helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal GetPagoGeneradorXSistema(int strecacodi, int genemprcodi, int sisemprecodi, int sistrncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPagoGeneradorXSistema);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, genemprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, sisemprecodi);
            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, sistrncodi);
            object result = dbProvider.ExecuteScalar(command);
            decimal dPago = 0;
            if (result != null) dPago = Convert.ToDecimal(result);
            return dPago;
        }
    }
}
