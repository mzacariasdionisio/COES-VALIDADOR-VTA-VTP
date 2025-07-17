using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;
using COES.Dominio.Interfaces.Sp7;
using COES.Infraestructura.Datos.Helper.Sp7;
using COES.Dominio.DTO.Sp7;

namespace COES.Infraestructura.Datos.Respositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_REPORTEVERSION_SP7
    /// </summary>
    public class TrReporteversionSp7Repository : RepositoryBase, ITrReporteversionSp7Repository
    {
        public TrReporteversionSp7Repository(string strConn) : base(strConn)
        {
        }

        TrReporteversionSp7Helper helper = new TrReporteversionSp7Helper();

        public int Save(TrReporteversionSp7DTO entity)
        {
            DbCommand command;

            if (entity.Vercodi > 0)
            {
                command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            }
            else
            {
                command = dbProvider.GetSqlStringCommand(helper.SqlGetMinId);
            }

            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            try
            {
                dbProvider.AddInParameter(command, helper.Revcodi, DbType.Int32, id);
                dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, entity.Vercodi);
                dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
                dbProvider.AddInParameter(command, helper.Revfecha, DbType.DateTime, entity.Revfecha);
                dbProvider.AddInParameter(command, helper.Revsumaciccpsmed, DbType.Decimal, entity.Revsumaciccpsmed);
                dbProvider.AddInParameter(command, helper.Revsumaciccpsest, DbType.Decimal, entity.Revsumaciccpsest);
                dbProvider.AddInParameter(command, helper.Revsumaciccpsestnoalm, DbType.Decimal, entity.Revsumaciccpsestnoalm);
                dbProvider.AddInParameter(command, helper.Revsumaciccpsalm, DbType.Decimal, entity.Revsumaciccpsalm);
                dbProvider.AddInParameter(command, helper.Revsumaciccpsmedc, DbType.Decimal, entity.Revsumaciccpsmedc);
                dbProvider.AddInParameter(command, helper.Revsumaciccpsestc, DbType.Decimal, entity.Revsumaciccpsestc);
                dbProvider.AddInParameter(command, helper.revsumaciccpsestnoalmc, DbType.Decimal, entity.Revsumaciccpsestnoalmc);
                dbProvider.AddInParameter(command, helper.Revsumaciccpsalmc, DbType.Decimal, entity.Revsumaciccpsalmc);
                dbProvider.AddInParameter(command, helper.Revnummed, DbType.Int32, entity.Revnummed);
                dbProvider.AddInParameter(command, helper.Revnumest, DbType.Int32, entity.Revnumest);
                dbProvider.AddInParameter(command, helper.Revnumestnoalm, DbType.Int32, entity.Revnumestnoalm);
                dbProvider.AddInParameter(command, helper.Revnumalm, DbType.Int32, entity.Revnumalm);
                dbProvider.AddInParameter(command, helper.Revnummedc, DbType.Int32, entity.Revnummedc);
                dbProvider.AddInParameter(command, helper.Revnumestc, DbType.Int32, entity.Revnumestc);
                dbProvider.AddInParameter(command, helper.Revnumestnoalmc, DbType.Int32, entity.Revnumestnoalmc);
                dbProvider.AddInParameter(command, helper.Revnumalmc, DbType.Int32, entity.Revnumalmc);
                dbProvider.AddInParameter(command, helper.Revnummedcsindef, DbType.Int32, entity.Revnummedcsindef);
                dbProvider.AddInParameter(command, helper.Revnumestcsindef, DbType.Int32, entity.Revnumestcsindef);
                dbProvider.AddInParameter(command, helper.Revnumalmcsindef, DbType.Int32, entity.Revnumalmcsindef);
                dbProvider.AddInParameter(command, helper.Revtfse, DbType.Int32, entity.Revtfse);
                dbProvider.AddInParameter(command, helper.Revtfss, DbType.Int32, entity.Revtfss);
                dbProvider.AddInParameter(command, helper.Revttotal, DbType.Int32, entity.Revttotal);
                dbProvider.AddInParameter(command, helper.Revfactdisp, DbType.Decimal, entity.Revfactdisp);
                dbProvider.AddInParameter(command, helper.Revciccpe, DbType.Decimal, entity.Revciccpe);
                dbProvider.AddInParameter(command, helper.Revciccpemedest, DbType.Decimal, entity.Revciccpemedest);
                dbProvider.AddInParameter(command, helper.Revciccpecrit, DbType.Decimal, entity.Revciccpecrit);
                dbProvider.AddInParameter(command, helper.Revttng, DbType.Int32, entity.Revttng);
                dbProvider.AddInParameter(command, helper.Revusucreacion, DbType.String, entity.Revusucreacion);
                dbProvider.AddInParameter(command, helper.Revfeccreacion, DbType.DateTime, entity.Revfeccreacion);
                dbProvider.AddInParameter(command, helper.Revusumodificacion, DbType.String, entity.Revusumodificacion);
                dbProvider.AddInParameter(command, helper.Revfecmodificacion, DbType.DateTime, entity.Revfecmodificacion);

                dbProvider.ExecuteNonQuery(command);
            }
            catch (Exception e)
            {
                int jj = 1;
            }
            return id;
        }

        public void Update(TrReporteversionSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, entity.Vercodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Revfecha, DbType.DateTime, entity.Revfecha);
            dbProvider.AddInParameter(command, helper.Revsumaciccpsmed, DbType.Decimal, entity.Revsumaciccpsmed);
            dbProvider.AddInParameter(command, helper.Revsumaciccpsest, DbType.Decimal, entity.Revsumaciccpsest);
            dbProvider.AddInParameter(command, helper.Revsumaciccpsestnoalm, DbType.Decimal, entity.Revsumaciccpsestnoalm);
            dbProvider.AddInParameter(command, helper.Revsumaciccpsalm, DbType.Decimal, entity.Revsumaciccpsalm);
            dbProvider.AddInParameter(command, helper.Revsumaciccpsmedc, DbType.Decimal, entity.Revsumaciccpsmedc);
            dbProvider.AddInParameter(command, helper.Revsumaciccpsestc, DbType.Decimal, entity.Revsumaciccpsestc);
            dbProvider.AddInParameter(command, helper.revsumaciccpsestnoalmc, DbType.Decimal, entity.Revsumaciccpsestnoalmc);
            dbProvider.AddInParameter(command, helper.Revsumaciccpsalmc, DbType.Decimal, entity.Revsumaciccpsalmc);
            dbProvider.AddInParameter(command, helper.Revnummed, DbType.Int32, entity.Revnummed);
            dbProvider.AddInParameter(command, helper.Revnumest, DbType.Int32, entity.Revnumest);
            dbProvider.AddInParameter(command, helper.Revnumestnoalm, DbType.Int32, entity.Revnumestnoalm);
            dbProvider.AddInParameter(command, helper.Revnumalm, DbType.Int32, entity.Revnumalm);
            dbProvider.AddInParameter(command, helper.Revnummedc, DbType.Int32, entity.Revnummedc);
            dbProvider.AddInParameter(command, helper.Revnumestc, DbType.Int32, entity.Revnumestc);
            dbProvider.AddInParameter(command, helper.Revnumestnoalmc, DbType.Int32, entity.Revnumestnoalmc);
            dbProvider.AddInParameter(command, helper.Revnumalmc, DbType.Int32, entity.Revnumalmc);
            dbProvider.AddInParameter(command, helper.Revnummedcsindef, DbType.Int32, entity.Revnummedcsindef);
            dbProvider.AddInParameter(command, helper.Revnumestcsindef, DbType.Int32, entity.Revnumestcsindef);
            dbProvider.AddInParameter(command, helper.Revnumalmcsindef, DbType.Int32, entity.Revnumalmcsindef);
            dbProvider.AddInParameter(command, helper.Revtfse, DbType.Int32, entity.Revtfse);
            dbProvider.AddInParameter(command, helper.Revtfss, DbType.Int32, entity.Revtfss);
            dbProvider.AddInParameter(command, helper.Revttotal, DbType.Int32, entity.Revttotal);
            dbProvider.AddInParameter(command, helper.Revfactdisp, DbType.Decimal, entity.Revfactdisp);
            dbProvider.AddInParameter(command, helper.Revciccpe, DbType.Decimal, entity.Revciccpe);
            dbProvider.AddInParameter(command, helper.Revciccpemedest, DbType.Decimal, entity.Revciccpemedest);
            dbProvider.AddInParameter(command, helper.Revciccpecrit, DbType.Decimal, entity.Revciccpecrit);
            dbProvider.AddInParameter(command, helper.Revttng, DbType.Int32, entity.Revttng);
            dbProvider.AddInParameter(command, helper.Revusucreacion, DbType.String, entity.Revusucreacion);
            dbProvider.AddInParameter(command, helper.Revfeccreacion, DbType.DateTime, entity.Revfeccreacion);
            dbProvider.AddInParameter(command, helper.Revusumodificacion, DbType.String, entity.Revusumodificacion);
            dbProvider.AddInParameter(command, helper.Revfecmodificacion, DbType.DateTime, entity.Revfecmodificacion);
            dbProvider.AddInParameter(command, helper.Revcodi, DbType.Int32, entity.Revcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int revcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Revcodi, DbType.Int32, revcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteVersion(int vercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteVersion);

            dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, vercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrReporteversionSp7DTO GetById(int revcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Revcodi, DbType.Int32, revcodi);
            TrReporteversionSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrReporteversionSp7DTO> List()
        {
            List<TrReporteversionSp7DTO> entitys = new List<TrReporteversionSp7DTO>();
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

        public List<TrReporteversionSp7DTO> List(int verCodi)
        {
            List<TrReporteversionSp7DTO> entitys = new List<TrReporteversionSp7DTO>();
            String sql = String.Format(this.helper.SqlListVersion, verCodi);
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

        public List<TrReporteversionSp7DTO> List(int verCodi, DateTime fechaIni, DateTime fechaFin)
        {
            List<TrReporteversionSp7DTO> entitys = new List<TrReporteversionSp7DTO>();
            String sql = String.Format(this.helper.SqlListAgrupada, verCodi, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrReporteversionSp7DTO entity = new TrReporteversionSp7DTO();


                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));


                    int iRevsumaciccpsmed = dr.GetOrdinal(this.helper.Revsumaciccpsmed);
                    if (!dr.IsDBNull(iRevsumaciccpsmed)) entity.Revsumaciccpsmed = dr.GetDecimal(iRevsumaciccpsmed);

                    int iRevsumaciccpsest = dr.GetOrdinal(this.helper.Revsumaciccpsest);
                    if (!dr.IsDBNull(iRevsumaciccpsest)) entity.Revsumaciccpsest = dr.GetDecimal(iRevsumaciccpsest);

                    int iRevsumaciccpsestnoalm = dr.GetOrdinal(this.helper.Revsumaciccpsestnoalm);
                    if (!dr.IsDBNull(iRevsumaciccpsestnoalm)) entity.Revsumaciccpsestnoalm = dr.GetDecimal(iRevsumaciccpsestnoalm);

                    int iRevsumaciccpsalm = dr.GetOrdinal(this.helper.Revsumaciccpsalm);
                    if (!dr.IsDBNull(iRevsumaciccpsalm)) entity.Revsumaciccpsalm = dr.GetDecimal(iRevsumaciccpsalm);

                    int iRevsumaciccpsmedc = dr.GetOrdinal(this.helper.Revsumaciccpsmedc);
                    if (!dr.IsDBNull(iRevsumaciccpsmedc)) entity.Revsumaciccpsmedc = dr.GetDecimal(iRevsumaciccpsmedc);

                    int iRevsumaciccpsestc = dr.GetOrdinal(this.helper.Revsumaciccpsestc);
                    if (!dr.IsDBNull(iRevsumaciccpsestc)) entity.Revsumaciccpsestc = dr.GetDecimal(iRevsumaciccpsestc);

                    int iRevsumaciccpsestnoalmc = dr.GetOrdinal(this.helper.revsumaciccpsestnoalmc);
                    if (!dr.IsDBNull(iRevsumaciccpsestnoalmc)) entity.Revsumaciccpsestnoalmc = dr.GetDecimal(iRevsumaciccpsestnoalmc);

                    int iRevsumaciccpsalmc = dr.GetOrdinal(this.helper.Revsumaciccpsalmc);
                    if (!dr.IsDBNull(iRevsumaciccpsalmc)) entity.Revsumaciccpsalmc = dr.GetDecimal(iRevsumaciccpsalmc);

                    int iRevnummed = dr.GetOrdinal(this.helper.Revnummed);
                    if (!dr.IsDBNull(iRevnummed)) entity.Revnummed = Convert.ToInt32(dr.GetValue(iRevnummed));

                    int iRevnumest = dr.GetOrdinal(this.helper.Revnumest);
                    if (!dr.IsDBNull(iRevnumest)) entity.Revnumest = Convert.ToInt32(dr.GetValue(iRevnumest));

                    int iRevnumestnoalm = dr.GetOrdinal(this.helper.Revnumestnoalm);
                    if (!dr.IsDBNull(iRevnumestnoalm)) entity.Revnumestnoalm = Convert.ToInt32(dr.GetValue(iRevnumestnoalm));

                    int iRevnumalm = dr.GetOrdinal(this.helper.Revnumalm);
                    if (!dr.IsDBNull(iRevnumalm)) entity.Revnumalm = Convert.ToInt32(dr.GetValue(iRevnumalm));

                    int iRevnummedc = dr.GetOrdinal(this.helper.Revnummedc);
                    if (!dr.IsDBNull(iRevnummedc)) entity.Revnummedc = Convert.ToInt32(dr.GetValue(iRevnummedc));

                    int iRevnumestc = dr.GetOrdinal(this.helper.Revnumestc);
                    if (!dr.IsDBNull(iRevnumestc)) entity.Revnumestc = Convert.ToInt32(dr.GetValue(iRevnumestc));

                    int iRevnumestnoalmc = dr.GetOrdinal(this.helper.Revnumestnoalmc);
                    if (!dr.IsDBNull(iRevnumestnoalmc)) entity.Revnumestnoalmc = Convert.ToInt32(dr.GetValue(iRevnumestnoalmc));

                    int iRevnumalmc = dr.GetOrdinal(this.helper.Revnumalmc);
                    if (!dr.IsDBNull(iRevnumalmc)) entity.Revnumalmc = Convert.ToInt32(dr.GetValue(iRevnumalmc));

                    int iRevnummedcsindef = dr.GetOrdinal(this.helper.Revnummedcsindef);
                    if (!dr.IsDBNull(iRevnummedcsindef)) entity.Revnummedcsindef = Convert.ToInt32(dr.GetValue(iRevnummedcsindef));

                    int iRevnumestcsindef = dr.GetOrdinal(this.helper.Revnumestcsindef);
                    if (!dr.IsDBNull(iRevnumestcsindef)) entity.Revnumestcsindef = Convert.ToInt32(dr.GetValue(iRevnumestcsindef));

                    int iRevnumalmcsindef = dr.GetOrdinal(this.helper.Revnumalmcsindef);
                    if (!dr.IsDBNull(iRevnumalmcsindef)) entity.Revnumalmcsindef = Convert.ToInt32(dr.GetValue(iRevnumalmcsindef));

                    int iRevtfse = dr.GetOrdinal(this.helper.Revtfse);
                    if (!dr.IsDBNull(iRevtfse)) entity.Revtfse = Convert.ToInt32(dr.GetValue(iRevtfse));

                    int iRevtfss = dr.GetOrdinal(this.helper.Revtfss);
                    if (!dr.IsDBNull(iRevtfss)) entity.Revtfss = Convert.ToInt32(dr.GetValue(iRevtfss));

                    int iRevttotal = dr.GetOrdinal(this.helper.Revttotal);
                    if (!dr.IsDBNull(iRevttotal)) entity.Revttotal = Convert.ToInt32(dr.GetValue(iRevttotal));

                    int iRevfactdisp = dr.GetOrdinal(this.helper.Revfactdisp);
                    if (!dr.IsDBNull(iRevfactdisp)) entity.Revfactdisp = dr.GetDecimal(iRevfactdisp);

                    int iRevciccpe = dr.GetOrdinal(this.helper.Revciccpe);
                    if (!dr.IsDBNull(iRevciccpe)) entity.Revciccpe = dr.GetDecimal(iRevciccpe);

                    int iRevciccpemedest = dr.GetOrdinal(this.helper.Revciccpemedest);
                    if (!dr.IsDBNull(iRevciccpemedest)) entity.Revciccpemedest = dr.GetDecimal(iRevciccpemedest);

                    int iRevciccpecrit = dr.GetOrdinal(this.helper.Revciccpecrit);
                    if (!dr.IsDBNull(iRevciccpecrit)) entity.Revciccpecrit = dr.GetDecimal(iRevciccpecrit);

                    int iRevttng = dr.GetOrdinal(this.helper.Revttng);
                    if (!dr.IsDBNull(iRevttng)) entity.Revttng = Convert.ToInt32(dr.GetValue(iRevttng));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrReporteversionSp7DTO> GetByCriteria()
        {
            List<TrReporteversionSp7DTO> entitys = new List<TrReporteversionSp7DTO>();
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

        /// <summary>
        /// Graba los datos de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public int SaveTrReporteversionSp7Id(TrReporteversionSp7DTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Revcodi == 0)
                    id = Save(entity);
                else
                {
                    Update(entity);
                    id = entity.Revcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<TrReporteversionSp7DTO> BuscarOperaciones(int verCodi, DateTime fechaIni, DateTime fechaFin, int nroPage, int pageSize)
        {
            List<TrReporteversionSp7DTO> entitys = new List<TrReporteversionSp7DTO>();
            String sql = String.Format(this.helper.ObtenerListado, verCodi, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrReporteversionSp7DTO entity = new TrReporteversionSp7DTO();

                    int iRevcodi = dr.GetOrdinal(this.helper.Revcodi);
                    if (!dr.IsDBNull(iRevcodi)) entity.Revcodi = Convert.ToInt32(dr.GetValue(iRevcodi));

                    int iVercodi = dr.GetOrdinal(this.helper.Vercodi);
                    if (!dr.IsDBNull(iVercodi)) entity.Vercodi = Convert.ToInt32(dr.GetValue(iVercodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iRevfecha = dr.GetOrdinal(this.helper.Revfecha);
                    if (!dr.IsDBNull(iRevfecha)) entity.Revfecha = dr.GetDateTime(iRevfecha);

                    int iRevsumaciccpsmed = dr.GetOrdinal(this.helper.Revsumaciccpsmed);
                    if (!dr.IsDBNull(iRevsumaciccpsmed)) entity.Revsumaciccpsmed = dr.GetDecimal(iRevsumaciccpsmed);

                    int iRevsumaciccpsest = dr.GetOrdinal(this.helper.Revsumaciccpsest);
                    if (!dr.IsDBNull(iRevsumaciccpsest)) entity.Revsumaciccpsest = dr.GetDecimal(iRevsumaciccpsest);

                    int iRevsumaciccpsestnoalm = dr.GetOrdinal(this.helper.Revsumaciccpsestnoalm);
                    if (!dr.IsDBNull(iRevsumaciccpsestnoalm)) entity.Revsumaciccpsestnoalm = dr.GetDecimal(iRevsumaciccpsestnoalm);

                    int iRevsumaciccpsalm = dr.GetOrdinal(this.helper.Revsumaciccpsalm);
                    if (!dr.IsDBNull(iRevsumaciccpsalm)) entity.Revsumaciccpsalm = dr.GetDecimal(iRevsumaciccpsalm);

                    int iRevsumaciccpsmedc = dr.GetOrdinal(this.helper.Revsumaciccpsmedc);
                    if (!dr.IsDBNull(iRevsumaciccpsmedc)) entity.Revsumaciccpsmedc = dr.GetDecimal(iRevsumaciccpsmedc);

                    int iRevsumaciccpsestc = dr.GetOrdinal(this.helper.Revsumaciccpsestc);
                    if (!dr.IsDBNull(iRevsumaciccpsestc)) entity.Revsumaciccpsestc = dr.GetDecimal(iRevsumaciccpsestc);

                    int iRevsumaciccpsestnoalmc = dr.GetOrdinal(this.helper.revsumaciccpsestnoalmc);
                    if (!dr.IsDBNull(iRevsumaciccpsestnoalmc)) entity.Revsumaciccpsestnoalmc = dr.GetDecimal(iRevsumaciccpsestnoalmc);

                    int iRevsumaciccpsalmc = dr.GetOrdinal(this.helper.Revsumaciccpsalmc);
                    if (!dr.IsDBNull(iRevsumaciccpsalmc)) entity.Revsumaciccpsalmc = dr.GetDecimal(iRevsumaciccpsalmc);

                    int iRevnummed = dr.GetOrdinal(this.helper.Revnummed);
                    if (!dr.IsDBNull(iRevnummed)) entity.Revnummed = Convert.ToInt32(dr.GetValue(iRevnummed));

                    int iRevnumest = dr.GetOrdinal(this.helper.Revnumest);
                    if (!dr.IsDBNull(iRevnumest)) entity.Revnumest = Convert.ToInt32(dr.GetValue(iRevnumest));

                    int iRevnumestnoalm = dr.GetOrdinal(this.helper.Revnumestnoalm);
                    if (!dr.IsDBNull(iRevnumestnoalm)) entity.Revnumestnoalm = Convert.ToInt32(dr.GetValue(iRevnumestnoalm));

                    int iRevnumalm = dr.GetOrdinal(this.helper.Revnumalm);
                    if (!dr.IsDBNull(iRevnumalm)) entity.Revnumalm = Convert.ToInt32(dr.GetValue(iRevnumalm));

                    int iRevnummedc = dr.GetOrdinal(this.helper.Revnummedc);
                    if (!dr.IsDBNull(iRevnummedc)) entity.Revnummedc = Convert.ToInt32(dr.GetValue(iRevnummedc));

                    int iRevnumestc = dr.GetOrdinal(this.helper.Revnumestc);
                    if (!dr.IsDBNull(iRevnumestc)) entity.Revnumestc = Convert.ToInt32(dr.GetValue(iRevnumestc));

                    int iRevnumestnoalmc = dr.GetOrdinal(this.helper.Revnumestnoalmc);
                    if (!dr.IsDBNull(iRevnumestnoalmc)) entity.Revnumestnoalmc = Convert.ToInt32(dr.GetValue(iRevnumestnoalmc));

                    int iRevnumalmc = dr.GetOrdinal(this.helper.Revnumalmc);
                    if (!dr.IsDBNull(iRevnumalmc)) entity.Revnumalmc = Convert.ToInt32(dr.GetValue(iRevnumalmc));

                    int iRevnummedcsindef = dr.GetOrdinal(this.helper.Revnummedcsindef);
                    if (!dr.IsDBNull(iRevnummedcsindef)) entity.Revnummedcsindef = Convert.ToInt32(dr.GetValue(iRevnummedcsindef));

                    int iRevnumestcsindef = dr.GetOrdinal(this.helper.Revnumestcsindef);
                    if (!dr.IsDBNull(iRevnumestcsindef)) entity.Revnumestcsindef = Convert.ToInt32(dr.GetValue(iRevnumestcsindef));

                    int iRevnumalmcsindef = dr.GetOrdinal(this.helper.Revnumalmcsindef);
                    if (!dr.IsDBNull(iRevnumalmcsindef)) entity.Revnumalmcsindef = Convert.ToInt32(dr.GetValue(iRevnumalmcsindef));

                    int iRevtfse = dr.GetOrdinal(this.helper.Revtfse);
                    if (!dr.IsDBNull(iRevtfse)) entity.Revtfse = Convert.ToInt32(dr.GetValue(iRevtfse));

                    int iRevtfss = dr.GetOrdinal(this.helper.Revtfss);
                    if (!dr.IsDBNull(iRevtfss)) entity.Revtfss = Convert.ToInt32(dr.GetValue(iRevtfss));

                    int iRevttotal = dr.GetOrdinal(this.helper.Revttotal);
                    if (!dr.IsDBNull(iRevttotal)) entity.Revttotal = Convert.ToInt32(dr.GetValue(iRevttotal));

                    int iRevfactdisp = dr.GetOrdinal(this.helper.Revfactdisp);
                    if (!dr.IsDBNull(iRevfactdisp)) entity.Revfactdisp = dr.GetDecimal(iRevfactdisp);

                    int iRevciccpe = dr.GetOrdinal(this.helper.Revciccpe);
                    if (!dr.IsDBNull(iRevciccpe)) entity.Revciccpe = dr.GetDecimal(iRevciccpe);

                    int iRevciccpemedest = dr.GetOrdinal(this.helper.Revciccpemedest);
                    if (!dr.IsDBNull(iRevciccpemedest)) entity.Revciccpemedest = dr.GetDecimal(iRevciccpemedest);

                    int iRevciccpecrit = dr.GetOrdinal(this.helper.Revciccpecrit);
                    if (!dr.IsDBNull(iRevciccpecrit)) entity.Revciccpecrit = dr.GetDecimal(iRevciccpecrit);

                    int iRevttng = dr.GetOrdinal(this.helper.Revttng);
                    if (!dr.IsDBNull(iRevttng)) entity.Revttng = Convert.ToInt32(dr.GetValue(iRevttng));

                    int iRevusucreacion = dr.GetOrdinal(this.helper.Revusucreacion);
                    if (!dr.IsDBNull(iRevusucreacion)) entity.Revusucreacion = dr.GetString(iRevusucreacion);

                    int iRevfeccreacion = dr.GetOrdinal(this.helper.Revfeccreacion);
                    if (!dr.IsDBNull(iRevfeccreacion)) entity.Revfeccreacion = dr.GetDateTime(iRevfeccreacion);

                    int iRevusumodificacion = dr.GetOrdinal(this.helper.Revusumodificacion);
                    if (!dr.IsDBNull(iRevusumodificacion)) entity.Revusumodificacion = dr.GetString(iRevusumodificacion);

                    int iRevfecmodificacion = dr.GetOrdinal(this.helper.Revfecmodificacion);
                    if (!dr.IsDBNull(iRevfecmodificacion)) entity.Revfecmodificacion = dr.GetDateTime(iRevfecmodificacion);

                    int iVerfechaini = dr.GetOrdinal(this.helper.Verfechaini);
                    if (!dr.IsDBNull(iVerfechaini)) entity.Verfechaini = dr.GetDateTime(iVerfechaini);

                    //-----------------------------
                    int iEmprenomb = dr.GetOrdinal(this.helper.Emprenomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprenomb = dr.GetString(iEmprenomb);

                    int iCanGeneral = dr.GetOrdinal(this.helper.CanGeneral);
                    if (!dr.IsDBNull(iCanGeneral)) entity.CanGeneral = Convert.ToInt32(dr.GetValue(iCanGeneral));

                    int iCanMedEst = dr.GetOrdinal(this.helper.CanMedEst);
                    if (!dr.IsDBNull(iCanMedEst)) entity.CanMedEst = Convert.ToInt32(dr.GetValue(iCanMedEst));

                    int iCanCritico = dr.GetOrdinal(this.helper.CanCritico);
                    if (!dr.IsDBNull(iCanCritico)) entity.CanCritico = Convert.ToInt32(dr.GetValue(iCanCritico));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrReporteversionSp7DTO> BuscarOperacionesResumen(int verCodi, DateTime fechaIni, DateTime fechaFin, int nroPage, int pageSize)
        {
            List<TrReporteversionSp7DTO> entitys = new List<TrReporteversionSp7DTO>();
            String sql = String.Format(this.helper.ObtenerListadoResumen, verCodi, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrReporteversionSp7DTO entity = new TrReporteversionSp7DTO();

                    int iRevcodi = dr.GetOrdinal(this.helper.Revcodi);
                    if (!dr.IsDBNull(iRevcodi)) entity.Revcodi = Convert.ToInt32(dr.GetValue(iRevcodi));

                    int iVercodi = dr.GetOrdinal(this.helper.Vercodi);
                    if (!dr.IsDBNull(iVercodi)) entity.Vercodi = Convert.ToInt32(dr.GetValue(iVercodi));

                    int iVerfechaini = dr.GetOrdinal(this.helper.Verfechaini);
                    if (!dr.IsDBNull(iVerfechaini)) entity.Verfechaini = dr.GetDateTime(iVerfechaini);

                    int iVerfechafin = dr.GetOrdinal(this.helper.Verfechafin);
                    if (!dr.IsDBNull(iVerfechafin)) entity.Verfechafin = dr.GetDateTime(iVerfechafin);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprenomb = dr.GetOrdinal(this.helper.Emprenomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprenomb = dr.GetString(iEmprenomb);

                    int iRevfactdisp = dr.GetOrdinal(this.helper.Revfactdisp);
                    if (!dr.IsDBNull(iRevfactdisp)) entity.Revfactdisp = dr.GetDecimal(iRevfactdisp);

                    int iRevciccpe = dr.GetOrdinal(this.helper.Revciccpe);
                    if (!dr.IsDBNull(iRevciccpe)) entity.Revciccpe = dr.GetDecimal(iRevciccpe);

                    int iRevciccpemedest = dr.GetOrdinal(this.helper.Revciccpemedest);
                    if (!dr.IsDBNull(iRevciccpemedest)) entity.Revciccpemedest = dr.GetDecimal(iRevciccpemedest);

                    int iRevciccpecrit = dr.GetOrdinal(this.helper.Revciccpecrit);
                    if (!dr.IsDBNull(iRevciccpecrit)) entity.Revciccpecrit = dr.GetDecimal(iRevciccpecrit);

                    int iCanGeneral = dr.GetOrdinal(this.helper.CanGeneral);
                    if (!dr.IsDBNull(iCanGeneral)) entity.CanGeneral = Convert.ToInt32(dr.GetValue(iCanGeneral));

                    int iCanMedEst = dr.GetOrdinal(this.helper.CanMedEst);
                    if (!dr.IsDBNull(iCanMedEst)) entity.CanMedEst = Convert.ToInt32(dr.GetValue(iCanMedEst));

                    int iCanCritico = dr.GetOrdinal(this.helper.CanCritico);
                    if (!dr.IsDBNull(iCanCritico)) entity.CanCritico = Convert.ToInt32(dr.GetValue(iCanCritico));


                    int iRevusucreacion = dr.GetOrdinal(this.helper.Revusucreacion);
                    if (!dr.IsDBNull(iRevusucreacion)) entity.Revusucreacion = dr.GetString(iRevusucreacion);

                    int iRevfeccreacion = dr.GetOrdinal(this.helper.Revfeccreacion);
                    if (!dr.IsDBNull(iRevfeccreacion)) entity.Revfeccreacion = dr.GetDateTime(iRevfeccreacion);

                    int iRevusumodificacion = dr.GetOrdinal(this.helper.Revusumodificacion);
                    if (!dr.IsDBNull(iRevusumodificacion)) entity.Revusumodificacion = dr.GetString(iRevusumodificacion);

                    int iRevfecmodificacion = dr.GetOrdinal(this.helper.Revfecmodificacion);
                    if (!dr.IsDBNull(iRevfecmodificacion)) entity.Revfecmodificacion = dr.GetDateTime(iRevfecmodificacion);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int verCodi, DateTime revFeccreacion, DateTime revFecmodificacion)
        {
            String sql = String.Format(this.helper.TotalRegistros, verCodi, revFeccreacion.ToString(ConstantesBase.FormatoFecha), revFecmodificacion.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }


        public List<TrReporteversionSp7DTO> GetListaDispMensualVersion(int emprcodi, DateTime fecha)
        {

            string query = string.Format(helper.SqlGetDispMensualVersion, emprcodi, fecha.ToString(ConstantesBase.FormatoFechaSoloMes), fecha.ToString(ConstantesBase.FormatoFechaAn));
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            List<TrReporteversionSp7DTO> entitys = new List<TrReporteversionSp7DTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrReporteversionSp7DTO entity = new TrReporteversionSp7DTO();

                    int iRevfecha = dr.GetOrdinal(this.helper.Revfecha);
                    if (!dr.IsDBNull(iRevfecha)) entity.Revfecha = dr.GetDateTime(iRevfecha);

                    //int iVernumero = dr.GetOrdinal(this.helper.VerNumero);
                    //if (!dr.IsDBNull(iVernumero)) entity.Vernumero = Convert.ToInt32(dr.GetValue(iVernumero));

                    int iRevciccpe = dr.GetOrdinal(this.helper.Revciccpe);
                    if (!dr.IsDBNull(iRevciccpe)) entity.Revciccpe = dr.GetDecimal(iRevciccpe);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }


        #region FIT - Señales no Disponibles


        public TrReporteversionSp7DTO GetEmpresasDiasVersion(int emprcodi, DateTime fecha)
        {

            string query = string.Format(helper.SqlGetEmpresasDiasVersion, emprcodi, fecha.ToString(ConstantesBase.FormatoFechaSoloDia), fecha.ToString(ConstantesBase.FormatoFechaSoloMes), fecha.ToString(ConstantesBase.FormatoFechaAn));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            TrReporteversionSp7DTO entity = new TrReporteversionSp7DTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));


                    int iRevciccpe = dr.GetOrdinal(this.helper.Revciccpe);
                    if (!dr.IsDBNull(iRevciccpe)) entity.Revciccpe = dr.GetDecimal(iRevciccpe);
                }
            }

            return entity;
        }

        public bool GetCongelamientoSeñales(int emprcodi, DateTime fecha)
        {
            //bool indicadorCongelamiento = false;
            //string query = string.Format(helper.SqlObtenerCongelamientoSenales, emprcodi, fecha.Day, fecha.Month, fecha.Year);
            //DbCommand command = dbProvider.GetSqlStringCommand(query);

            //int contador = -1;
            //Object result = dbProvider.ExecuteScalar(command);

            //if(result!= null)
            //{
            //    contador = Convert.ToInt32(result);
            //}

            //if (contador == 0)
            //{
            //    query = string.Format(helper.SqlObtenerIndicadorCaidaEnlace, fecha.ToString(ConstantesBase.FormatoFecha), emprcodi, 0);
            //    command = dbProvider.GetSqlStringCommand(query);

            //    int suma = -1;

            //    object resultMatriz = dbProvider.ExecuteScalar(command);

            //    if (resultMatriz != null)
            //    {                    
            //        int.TryParse(resultMatriz.ToString(), out suma);

            //        if (suma == 0)
            //        {
            //            indicadorCongelamiento = true;      
            //        }
            //    }
            //}

            //return indicadorCongelamiento;


            //- Obtenemos el ultimo mensaje de la empresa
            string query = string.Format(helper.SqlObtenerCaidaEnlace, emprcodi, fecha.Day, fecha.Month, fecha.Year);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            bool indicadorCongelamiento = false;
            int hora = -1;
            int minuto = -1;
            int conectado = -1;
            string tiempo = string.Empty;

            //- Leemos el último mensaje del sp7 para la empresa y fecha
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iHora = dr.GetOrdinal("hora");
                    if (!dr.IsDBNull(iHora)) hora = Convert.ToInt32(dr.GetValue(iHora));

                    int iMinuto = dr.GetOrdinal("minuto");
                    if (!dr.IsDBNull(iMinuto)) minuto = Convert.ToInt32(dr.GetValue(iMinuto));

                    int iMsgconectado = dr.GetOrdinal("msgconectado");
                    if (!dr.IsDBNull(iMsgconectado)) conectado = Convert.ToInt32(dr.GetValue(iMsgconectado));

                    int iMsgstamp = dr.GetOrdinal("msgstamp");
                    if (!dr.IsDBNull(iMsgstamp)) tiempo = (dr.GetDateTime(iMsgstamp)).ToString("dd/MM/yyyy HH:mm");
                }
            }

            //- En caso no se encuentren registros de mensajes o que el enlace aparezca como conectado
            if (hora == -1 || conectado == 1)
            {
                if (hora == -1)
                {
                    hora = 0;
                    minuto = 0;
                }

                int sumaMinuto = -1;

                if (minuto < 59)
                {
                    //- Obteneemos los valores para los minutos que quedan
                    string queryHora = string.Empty;
                    for (int j = minuto + 1; j < 60; j++)
                    {
                        queryHora = queryHora + "H" + j + " + ";
                    }
                    queryHora = queryHora + "0";
                    query = string.Format(helper.SqlObtenerIndicadorCaidaEnlaceHora, fecha.ToString(ConstantesBase.FormatoFecha), emprcodi, hora, queryHora);
                    command = dbProvider.GetSqlStringCommand(query);

                    object resultMinuto = dbProvider.ExecuteScalar(command);

                    if (resultMinuto != null)
                    {
                        if (resultMinuto.ToString() != string.Empty)
                        {
                            int.TryParse(resultMinuto.ToString(), out sumaMinuto);
                        }
                    }
                }
                else
                {
                    sumaMinuto = -2;
                }

                //- Obtenemos los valores para las horas siguientes
                query = string.Format(helper.SqlObtenerIndicadorCaidaEnlace, fecha.ToString(ConstantesBase.FormatoFecha), emprcodi, hora + 1);
                command = dbProvider.GetSqlStringCommand(query);

                int suma = -1;

                //- Aca debemos corregir los casos porque no está considerando correctamente

                object result = dbProvider.ExecuteScalar(command);

                if (result != null)
                {
                    if (result.ToString() != string.Empty)
                    {
                        int.TryParse(result.ToString(), out suma);
                    }
                }

                //- Verificamos si existe una desconexion en el enlace
                if ((suma == 0 && sumaMinuto == -2) || (suma == 0 && sumaMinuto == 0) || (suma == -1 && sumaMinuto == 0))
                {
                    indicadorCongelamiento = true;
                }
            }

            return indicadorCongelamiento;
        }

        public bool ObtenerCaidaEnlace(int emprcodi, DateTime fecha, out string tiempoDesconexion)
        {
            //- Obtenemos el ultimo mensaje de la empresa
            string query = string.Format(helper.SqlObtenerCaidaEnlace, emprcodi, fecha.Day, fecha.Month, fecha.Year);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            bool indicadorDesconexion = false;
            int hora = -1;
            int minuto = -1;
            int conectado = -1;
            string tiempo = string.Empty;

            //- Leemos el último mensaje del sp7 para la empresa y fecha
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iHora = dr.GetOrdinal("hora");
                    if (!dr.IsDBNull(iHora)) hora = Convert.ToInt32(dr.GetValue(iHora));

                    int iMinuto = dr.GetOrdinal("minuto");
                    if (!dr.IsDBNull(iMinuto)) minuto = Convert.ToInt32(dr.GetValue(iMinuto));

                    int iMsgconectado = dr.GetOrdinal("msgconectado");
                    if (!dr.IsDBNull(iMsgconectado)) conectado = Convert.ToInt32(dr.GetValue(iMsgconectado));

                    int iMsgstamp = dr.GetOrdinal("msgstamp");
                    if (!dr.IsDBNull(iMsgstamp)) tiempo = (dr.GetDateTime(iMsgstamp)).ToString("dd/MM/yyyy HH:mm");
                }
            }

            //- En caso se encuentre registro de mensajes
            if (hora != -1)
            {
                //- Si el mensaje corresponde a una caida de enlace
                if (conectado == 0)
                {
                    int sumaMinuto = -1;

                    if (minuto < 59)
                    {
                        //- Obteneemos los valores para los minutos que quedan
                        string queryHora = string.Empty;
                        for (int j = minuto + 1; j < 60; j++)
                        {
                            queryHora = queryHora + "H" + j + " + ";
                        }
                        queryHora = queryHora + "0";
                        query = string.Format(helper.SqlObtenerIndicadorCaidaEnlaceHora, fecha.ToString(ConstantesBase.FormatoFecha), emprcodi, hora, queryHora);
                        command = dbProvider.GetSqlStringCommand(query);

                        object resultMinuto = dbProvider.ExecuteScalar(command);

                        if (resultMinuto != null)
                        {
                            if (resultMinuto.ToString() != string.Empty)
                            {
                                int.TryParse(resultMinuto.ToString(), out sumaMinuto);
                            }
                        }
                    }
                    else
                    {
                        sumaMinuto = -2;
                    }

                    //- Obtenemos los valores para las horas siguientes
                    query = string.Format(helper.SqlObtenerIndicadorCaidaEnlace, fecha.ToString(ConstantesBase.FormatoFecha), emprcodi, hora + 1);
                    command = dbProvider.GetSqlStringCommand(query);

                    int suma = -1;

                    //- Aca debemos corregir los casos porque no está considerando correctamente

                    object result = dbProvider.ExecuteScalar(command);

                    if (result != null)
                    {
                        if (result.ToString() != string.Empty)
                        {
                            int.TryParse(result.ToString(), out suma);
                        }
                    }

                    //- Verificamos si existe una desconexion en el enlace
                    if ((suma == 0 && sumaMinuto == -2) || (suma == 0 && sumaMinuto == 0) || (suma == -1 && sumaMinuto == 0))
                    {
                        indicadorDesconexion = true;
                    }
                }
            }

            tiempoDesconexion = tiempo;

            return indicadorDesconexion;
        }
        #endregion
    }
}
