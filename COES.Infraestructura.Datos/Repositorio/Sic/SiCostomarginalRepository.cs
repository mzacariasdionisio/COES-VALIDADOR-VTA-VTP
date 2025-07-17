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
    /// Clase de acceso a datos de la tabla SI_COSTOMARGINAL
    /// </summary>
    public class SiCostomarginalRepository : RepositoryBase, ISiCostomarginalRepository
    {
        public SiCostomarginalRepository(string strConn)
            : base(strConn)
        {
        }

        SiCostomarginalHelper helper = new SiCostomarginalHelper();

        public int Save(SiCostomarginalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmgrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cmgrenergia, DbType.Decimal, entity.Cmgrenergia);
            dbProvider.AddInParameter(command, helper.Cmgrcongestion, DbType.Decimal, entity.Cmgrcongestion);
            dbProvider.AddInParameter(command, helper.Cmgrtotal, DbType.Decimal, entity.Cmgrtotal);
            dbProvider.AddInParameter(command, helper.Cmgrcorrelativo, DbType.Int32, entity.Cmgrcorrelativo);
            dbProvider.AddInParameter(command, helper.Cmgrfecha, DbType.DateTime, entity.Cmgrfecha);
            dbProvider.AddInParameter(command, helper.Cmgrusucreacion, DbType.String, entity.Cmgrusucreacion);
            dbProvider.AddInParameter(command, helper.Cmgrfeccreacion, DbType.DateTime, entity.Cmgrfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmgrtcodi, DbType.Int32, entity.Cmgrtcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiCostomarginalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmgrcodi, DbType.Int32, entity.Cmgrcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cmgrenergia, DbType.Decimal, entity.Cmgrenergia);
            dbProvider.AddInParameter(command, helper.Cmgrcongestion, DbType.Decimal, entity.Cmgrcongestion);
            dbProvider.AddInParameter(command, helper.Cmgrtotal, DbType.Decimal, entity.Cmgrtotal);
            dbProvider.AddInParameter(command, helper.Cmgrcorrelativo, DbType.Int32, entity.Cmgrcorrelativo);
            dbProvider.AddInParameter(command, helper.Cmgrfecha, DbType.DateTime, entity.Cmgrfecha);
            dbProvider.AddInParameter(command, helper.Cmgrusucreacion, DbType.String, entity.Cmgrusucreacion);
            dbProvider.AddInParameter(command, helper.Cmgrfeccreacion, DbType.DateTime, entity.Cmgrfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmgrtcodi, DbType.Int32, entity.Cmgrtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int sicmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmgrcodi, DbType.Int32, sicmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiCostomarginalDTO GetById(int sicmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmgrcodi, DbType.Int32, sicmcodi);
            SiCostomarginalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiCostomarginalDTO> List()
        {
            List<SiCostomarginalDTO> entitys = new List<SiCostomarginalDTO>();
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

        public List<SiCostomarginalDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin, string barrcodi)
        {
            List<SiCostomarginalDTO> entitys = new List<SiCostomarginalDTO>();
            string fec1 = fechaIni.ToString(ConstantesBase.FormatoFecha), fec2 = fechaFin.ToString(ConstantesBase.FormatoFecha);
            string query = string.Format(helper.SqlGetByCriteria, fec1, fec2, barrcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iBarrnomb = dr.GetOrdinal(helper.Barrnomb);
                    if (!dr.IsDBNull(iBarrnomb)) entity.Barrnomb = dr.GetString(iBarrnomb);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int MaxSiCostomarginal()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void SaveSiCostomarginalMasivo(List<SiCostomarginalDTO> ListObj)
        {
            dbProvider.AddColumnMapping(helper.Cmgrcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmgrenergia, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cmgrcongestion, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cmgrtotal, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cmgrcorrelativo, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmgrfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Cmgrusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Cmgrfeccreacion, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Barrcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmgrtcodi, DbType.Int32);

            dbProvider.BulkInsert<SiCostomarginalDTO>(ListObj, helper.TableName);
        }

        public void DeleteSiCostomarginalXFecha(DateTime f_1, DateTime f_2)
        {
            string query = string.Format(helper.SqlDeleteSiCostomarginalXFecha, f_1.ToString(ConstantesBase.FormatoFecha), f_2.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<SiCostomarginalDTO> GetByCriteriaSiCostomarginalDet(DateTime fechahora)
        {
            List<SiCostomarginalDTO> entitys = new List<SiCostomarginalDTO>();
            string query = string.Format(helper.SqlGetByCriteriaSiCostomarginalDet, fechahora.ToString(ConstantesBase.FormatoFechaExtendido));
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

        public void ProcesarTempCostoMarginal(int enviocodi,DateTime fechaini,DateTime fechafin,string usuario)
        {

            string query = string.Format(helper.SqlProcesarTempCostoMarginal, enviocodi, fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha),usuario);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);

        }

        public List<SiCostomarginalDTO> ObtenerCmgPromedioDiarioDeBarras(DateTime fechaInicio, DateTime fechaFin, string barrcodi)
        {
            List<SiCostomarginalDTO> entitys = new List<SiCostomarginalDTO>();
            string query = string.Format(helper.SqlObtenerCmgPromedioDiarioDeBarras, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), barrcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new SiCostomarginalDTO();

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

                    int iBarrnomb = dr.GetOrdinal(helper.Barrnomb);
                    if (!dr.IsDBNull(iBarrnomb)) entity.Barrnomb = dr.GetString(iBarrnomb);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iCmgrfecha = dr.GetOrdinal(helper.Cmgrfecha);
                    if (!dr.IsDBNull(iCmgrfecha)) entity.Cmgrfecha = dr.GetDateTime(iCmgrfecha);

                    int iCmgrenergia = dr.GetOrdinal(helper.Cmgrenergia);
                    if (!dr.IsDBNull(iCmgrenergia)) entity.Cmgrenergia = dr.GetDecimal(iCmgrenergia);

                    int iCmgrtotal = dr.GetOrdinal(helper.Cmgrtotal);
                    if (!dr.IsDBNull(iCmgrtotal)) entity.Cmgrtotal = dr.GetDecimal(iCmgrtotal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiCostomarginalDTO> ObtenerReporteValoresNulos(DateTime fechaInicio, DateTime fechaFin)
        {
            List<SiCostomarginalDTO> entitys = new List<SiCostomarginalDTO>();

            string query = string.Format(helper.SqlObtenerReporteValoresNulos, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCostomarginalDTO entity = this.helper.Create(dr);

                    int iBarranombre = dr.GetOrdinal(this.helper.Barrnomb);                   
                    if (!dr.IsDBNull(iBarranombre)) entity.Barrnomb = dr.GetString(iBarranombre);

                    int iFechahoracm = dr.GetOrdinal(this.helper.Fechahoracm);
                    if (!dr.IsDBNull(iFechahoracm)) entity.Fechahoracm = dr.GetString(iFechahoracm);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
