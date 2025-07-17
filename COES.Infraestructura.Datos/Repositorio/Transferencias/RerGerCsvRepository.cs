using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RER_GERCSV
    /// </summary>
    public class RerGerCsvRepository : RepositoryBase, IRerGerCsvRepository
    {
        public RerGerCsvRepository(string strConn) : base(strConn)
        {
        }

        readonly RerGerCsvHelper helper = new RerGerCsvHelper();

        public int Save(RerGerCsvDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Regercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Resddpcodi, DbType.Int32, entity.Resddpcodi);
            dbProvider.AddInParameter(command, helper.Regergndarchivo, DbType.String, entity.Regergndarchivo);
            dbProvider.AddInParameter(command, helper.Regerhidarchivo, DbType.String, entity.Regerhidarchivo);
            dbProvider.AddInParameter(command, helper.Regerusucreacion, DbType.String, entity.Regerusucreacion);
            dbProvider.AddInParameter(command, helper.Regerfeccreacion, DbType.DateTime, entity.Regerfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerGerCsvDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Resddpcodi, DbType.Int32, entity.Resddpcodi);
            dbProvider.AddInParameter(command, helper.Regergndarchivo, DbType.String, entity.Regergndarchivo);
            dbProvider.AddInParameter(command, helper.Regerhidarchivo, DbType.String, entity.Regerhidarchivo);
            dbProvider.AddInParameter(command, helper.Regerusucreacion, DbType.String, entity.Regerusucreacion);
            dbProvider.AddInParameter(command, helper.Regerfeccreacion, DbType.DateTime, entity.Regerfeccreacion);
            dbProvider.AddInParameter(command, helper.Regercodi, DbType.Int32, entity.Regercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerGerCsvId)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Regercodi, DbType.Int32, rerGerCsvId);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerGerCsvDTO GetById(int rerGerCsvId)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Regercodi, DbType.Int32, rerGerCsvId);
            RerGerCsvDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerGerCsvDTO> List()
        {
            List<RerGerCsvDTO> entities = new List<RerGerCsvDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }



        public void BulkInsertTablaTemporal(List<RerLecCsvTemp> entitys, string nombreTabla)
        {
            dbProvider.AddColumnMapping(helper.Rerfecinicio, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Reretapa, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rerserie, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rerbloque, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rercentrsddp, DbType.String);
            dbProvider.AddColumnMapping(helper.Rervalor, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rertipcsv, DbType.String);

            dbProvider.BulkInsert<RerLecCsvTemp>(entitys, nombreTabla);
        }

        public void TruncateTablaTemporal(string nombreTabla)
        {
            string query = string.Format(helper.SqlTruncateTablaTemporal, nombreTabla);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void InsertTablaTemporal(RerLecCsvTemp entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlInsertTablaTemporal);

            dbProvider.AddInParameter(command, helper.Rerfecinicio, DbType.DateTime, entity.Rerfecinicio);
            dbProvider.AddInParameter(command, helper.Reretapa, DbType.Int32, entity.Reretapa);
            dbProvider.AddInParameter(command, helper.Rerserie, DbType.Int32, entity.Rerserie);
            dbProvider.AddInParameter(command, helper.Rerbloque, DbType.Int32, entity.Rerbloque);
            dbProvider.AddInParameter(command, helper.Rercentrsddp, DbType.String, entity.Rercentrsddp);
            dbProvider.AddInParameter(command, helper.Rervalor, DbType.Decimal, entity.Rervalor);
            dbProvider.AddInParameter(command, helper.Rertipcsv, DbType.String, entity.Rertipcsv);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<RerLecCsvTemp> ListTablaTemporal(string nomCentralSddp)
        {
            List<RerLecCsvTemp> entitys = new List<RerLecCsvTemp>();

            string query = string.Format(helper.SqlListTablaTemporal, nomCentralSddp);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerLecCsvTemp entity = new RerLecCsvTemp();

                    int iRerfecinicio = dr.GetOrdinal(helper.Rerfecinicio);
                    if (!dr.IsDBNull(iRerfecinicio)) entity.Rerfecinicio = dr.GetDateTime(iRerfecinicio);

                    int iReretapa = dr.GetOrdinal(helper.Reretapa);
                    if (!dr.IsDBNull(iReretapa)) entity.Reretapa = Convert.ToInt32(dr.GetValue(iReretapa));

                    int iRerbloque = dr.GetOrdinal(helper.Rerbloque);
                    if (!dr.IsDBNull(iRerbloque)) entity.Rerbloque = Convert.ToInt32(dr.GetValue(iRerbloque));

                    int iRertipcsv = dr.GetOrdinal(helper.Rertipcsv);
                    if (!dr.IsDBNull(iRertipcsv)) entity.Rertipcsv = dr.GetString(iRertipcsv);

                    int iRervalor = dr.GetOrdinal(helper.Rervalor);
                    if (!dr.IsDBNull(iRervalor)) entity.Rervalor = dr.GetDecimal(iRervalor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RerCentralDTO> ListEquiposEmpresasCentralesRer()
        {
            List<RerCentralDTO> entitys = new List<RerCentralDTO>();

            string query = helper.SqlListEquiposEmpresasCentralesRer;
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralDTO entity = new RerCentralDTO();

                    int iRercencodi = dr.GetOrdinal(helper.Rercencodi);
                    if (!dr.IsDBNull(iRercencodi)) entity.Rercencodi = Convert.ToInt32(dr.GetValue(iRercencodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RerCentralPmpoDTO> ListPtosMedicionCentralesPmpo(int rercencodi)
        {
            List<RerCentralPmpoDTO> entitys = new List<RerCentralPmpoDTO>();

            string query = string.Format(helper.SqlListPtosMedicionCentralesPmpo, rercencodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralPmpoDTO entity = new RerCentralPmpoDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PmoSddpCodigoDTO GetByCentralesSddp(int ptoMediCodi)
        {
            PmoSddpCodigoDTO entity = null;

            string query = string.Format(helper.SqlGetByCentralesSddp, ptoMediCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new PmoSddpCodigoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iSddpnomb = dr.GetOrdinal(helper.Sddpnomb);
                    if (!dr.IsDBNull(iSddpnomb)) entity.Sddpnomb = dr.GetString(iSddpnomb);
                }
            }

            return entity;
        }
        
        public List<RerCentralDTO> ListPtoMedicionCentralesRer()
        {
            List<RerCentralDTO> entitys = new List<RerCentralDTO>();

            string query = helper.SqlListPtoMedicionCentralesRer;
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralDTO entity = new RerCentralDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RerInsumoTemporalDTO> ListTablaCMTemporal(int ptoMediCodi)
        {
            List<RerInsumoTemporalDTO> entitys = new List<RerInsumoTemporalDTO>();

            string query = string.Format(helper.SqlListTablaCMTemporal, ptoMediCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerInsumoTemporalDTO entity = new RerInsumoTemporalDTO();

                    int iRerfecinicio = dr.GetOrdinal(helper.Rerfecinicio);
                    if (!dr.IsDBNull(iRerfecinicio)) entity.Rerfecinicio = dr.GetDateTime(iRerfecinicio);

                    int iReretapa = dr.GetOrdinal(helper.Reretapa);
                    if (!dr.IsDBNull(iReretapa)) entity.Reretapa = Convert.ToInt32(dr.GetValue(iReretapa));

                    int iRerbloque = dr.GetOrdinal(helper.Rerbloque);
                    if (!dr.IsDBNull(iRerbloque)) entity.Rerbloque = Convert.ToInt32(dr.GetValue(iRerbloque));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iRervalor = dr.GetOrdinal(helper.Rervalor);
                    if (!dr.IsDBNull(iRervalor)) entity.Rervalor = dr.GetDecimal(iRervalor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RerInsumoDiaTemporalDTO> ListTablaCMTemporalDia(int ptoMediCodi, DateTime fechaInicio, DateTime fechaFin, decimal dTipoCambio)
        {
            List<RerInsumoDiaTemporalDTO> entitys = new List<RerInsumoDiaTemporalDTO>();
            string sFechaInicio = fechaInicio.ToString("dd/MM/yyyy");
            string sFechaFin = fechaFin.ToString("dd/MM/yyyy");
            string query = string.Format(helper.SqlListTablaCMTemporalDia, ptoMediCodi, sFechaInicio, sFechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerInsumoDiaTemporalDTO entity = new RerInsumoDiaTemporalDTO();

                    int iRerinddiafecdia = dr.GetOrdinal(helper.Rerinddiafecdia);
                    if (!dr.IsDBNull(iRerinddiafecdia)) entity.Rerinddiafecdia = dr.GetDateTime(iRerinddiafecdia);

                    int iRerinddiah1 = dr.GetOrdinal(helper.Rerinddiah1);
                    if (!dr.IsDBNull(iRerinddiah1)) entity.Rerinddiah1 = dr.GetDecimal(iRerinddiah1) * dTipoCambio;

                    int iRerinddiah2 = dr.GetOrdinal(helper.Rerinddiah2);
                    if (!dr.IsDBNull(iRerinddiah2)) entity.Rerinddiah2 = dr.GetDecimal(iRerinddiah2) * dTipoCambio;

                    int iRerinddiah3 = dr.GetOrdinal(helper.Rerinddiah3);
                    if (!dr.IsDBNull(iRerinddiah3)) entity.Rerinddiah3 = dr.GetDecimal(iRerinddiah3) * dTipoCambio;

                    int iRerinddiah4 = dr.GetOrdinal(helper.Rerinddiah4);
                    if (!dr.IsDBNull(iRerinddiah4)) entity.Rerinddiah4 = dr.GetDecimal(iRerinddiah4) * dTipoCambio;

                    int iRerinddiah5 = dr.GetOrdinal(helper.Rerinddiah5);
                    if (!dr.IsDBNull(iRerinddiah5)) entity.Rerinddiah5 = dr.GetDecimal(iRerinddiah5) * dTipoCambio;

                    int iRerinddiah6 = dr.GetOrdinal(helper.Rerinddiah6);
                    if (!dr.IsDBNull(iRerinddiah6)) entity.Rerinddiah6 = dr.GetDecimal(iRerinddiah6) * dTipoCambio;

                    int iRerinddiah7 = dr.GetOrdinal(helper.Rerinddiah7);
                    if (!dr.IsDBNull(iRerinddiah7)) entity.Rerinddiah7 = dr.GetDecimal(iRerinddiah7) * dTipoCambio;

                    int iRerinddiah8 = dr.GetOrdinal(helper.Rerinddiah8);
                    if (!dr.IsDBNull(iRerinddiah8)) entity.Rerinddiah8 = dr.GetDecimal(iRerinddiah8) * dTipoCambio;

                    int iRerinddiah9 = dr.GetOrdinal(helper.Rerinddiah9);
                    if (!dr.IsDBNull(iRerinddiah9)) entity.Rerinddiah9 = dr.GetDecimal(iRerinddiah9) * dTipoCambio;

                    int iRerinddiah10 = dr.GetOrdinal(helper.Rerinddiah10);
                    if (!dr.IsDBNull(iRerinddiah10)) entity.Rerinddiah10 = dr.GetDecimal(iRerinddiah10) * dTipoCambio;

                    int iRerinddiah11 = dr.GetOrdinal(helper.Rerinddiah11);
                    if (!dr.IsDBNull(iRerinddiah11)) entity.Rerinddiah11 = dr.GetDecimal(iRerinddiah11) * dTipoCambio;

                    int iRerinddiah12 = dr.GetOrdinal(helper.Rerinddiah12);
                    if (!dr.IsDBNull(iRerinddiah12)) entity.Rerinddiah12 = dr.GetDecimal(iRerinddiah12) * dTipoCambio;

                    int iRerinddiah13 = dr.GetOrdinal(helper.Rerinddiah13);
                    if (!dr.IsDBNull(iRerinddiah13)) entity.Rerinddiah13 = dr.GetDecimal(iRerinddiah13) * dTipoCambio;

                    int iRerinddiah14 = dr.GetOrdinal(helper.Rerinddiah14);
                    if (!dr.IsDBNull(iRerinddiah14)) entity.Rerinddiah14 = dr.GetDecimal(iRerinddiah14) * dTipoCambio;

                    int iRerinddiah15 = dr.GetOrdinal(helper.Rerinddiah15);
                    if (!dr.IsDBNull(iRerinddiah15)) entity.Rerinddiah15 = dr.GetDecimal(iRerinddiah15) * dTipoCambio;

                    int iRerinddiah16 = dr.GetOrdinal(helper.Rerinddiah16);
                    if (!dr.IsDBNull(iRerinddiah16)) entity.Rerinddiah16 = dr.GetDecimal(iRerinddiah16) * dTipoCambio;

                    int iRerinddiah17 = dr.GetOrdinal(helper.Rerinddiah17);
                    if (!dr.IsDBNull(iRerinddiah17)) entity.Rerinddiah17 = dr.GetDecimal(iRerinddiah17) * dTipoCambio;

                    int iRerinddiah18 = dr.GetOrdinal(helper.Rerinddiah18);
                    if (!dr.IsDBNull(iRerinddiah18)) entity.Rerinddiah18 = dr.GetDecimal(iRerinddiah18) * dTipoCambio;

                    int iRerinddiah19 = dr.GetOrdinal(helper.Rerinddiah19);
                    if (!dr.IsDBNull(iRerinddiah19)) entity.Rerinddiah19 = dr.GetDecimal(iRerinddiah19) * dTipoCambio;

                    int iRerinddiah20 = dr.GetOrdinal(helper.Rerinddiah20);
                    if (!dr.IsDBNull(iRerinddiah20)) entity.Rerinddiah20 = dr.GetDecimal(iRerinddiah20) * dTipoCambio;

                    int iRerinddiah21 = dr.GetOrdinal(helper.Rerinddiah21);
                    if (!dr.IsDBNull(iRerinddiah21)) entity.Rerinddiah21 = dr.GetDecimal(iRerinddiah21) * dTipoCambio;

                    int iRerinddiah22 = dr.GetOrdinal(helper.Rerinddiah22);
                    if (!dr.IsDBNull(iRerinddiah22)) entity.Rerinddiah22 = dr.GetDecimal(iRerinddiah22) * dTipoCambio;

                    int iRerinddiah23 = dr.GetOrdinal(helper.Rerinddiah23);
                    if (!dr.IsDBNull(iRerinddiah23)) entity.Rerinddiah23 = dr.GetDecimal(iRerinddiah23) * dTipoCambio;

                    int iRerinddiah24 = dr.GetOrdinal(helper.Rerinddiah24);
                    if (!dr.IsDBNull(iRerinddiah24)) entity.Rerinddiah24 = dr.GetDecimal(iRerinddiah24) * dTipoCambio;

                    int iRerinddiah25 = dr.GetOrdinal(helper.Rerinddiah25);
                    if (!dr.IsDBNull(iRerinddiah25)) entity.Rerinddiah25 = dr.GetDecimal(iRerinddiah25) * dTipoCambio;

                    int iRerinddiah26 = dr.GetOrdinal(helper.Rerinddiah26);
                    if (!dr.IsDBNull(iRerinddiah26)) entity.Rerinddiah26 = dr.GetDecimal(iRerinddiah26) * dTipoCambio;

                    int iRerinddiah27 = dr.GetOrdinal(helper.Rerinddiah27);
                    if (!dr.IsDBNull(iRerinddiah27)) entity.Rerinddiah27 = dr.GetDecimal(iRerinddiah27) * dTipoCambio;

                    int iRerinddiah28 = dr.GetOrdinal(helper.Rerinddiah28);
                    if (!dr.IsDBNull(iRerinddiah28)) entity.Rerinddiah28 = dr.GetDecimal(iRerinddiah28) * dTipoCambio;

                    int iRerinddiah29 = dr.GetOrdinal(helper.Rerinddiah29);
                    if (!dr.IsDBNull(iRerinddiah29)) entity.Rerinddiah29 = dr.GetDecimal(iRerinddiah29) * dTipoCambio;

                    int iRerinddiah30 = dr.GetOrdinal(helper.Rerinddiah30);
                    if (!dr.IsDBNull(iRerinddiah30)) entity.Rerinddiah30 = dr.GetDecimal(iRerinddiah30) * dTipoCambio;

                    int iRerinddiah31 = dr.GetOrdinal(helper.Rerinddiah31);
                    if (!dr.IsDBNull(iRerinddiah31)) entity.Rerinddiah31 = dr.GetDecimal(iRerinddiah31) * dTipoCambio;

                    int iRerinddiah32 = dr.GetOrdinal(helper.Rerinddiah32);
                    if (!dr.IsDBNull(iRerinddiah32)) entity.Rerinddiah32 = dr.GetDecimal(iRerinddiah32) * dTipoCambio;

                    int iRerinddiah33 = dr.GetOrdinal(helper.Rerinddiah33);
                    if (!dr.IsDBNull(iRerinddiah33)) entity.Rerinddiah33 = dr.GetDecimal(iRerinddiah33) * dTipoCambio;

                    int iRerinddiah34 = dr.GetOrdinal(helper.Rerinddiah34);
                    if (!dr.IsDBNull(iRerinddiah34)) entity.Rerinddiah34 = dr.GetDecimal(iRerinddiah34) * dTipoCambio;

                    int iRerinddiah35 = dr.GetOrdinal(helper.Rerinddiah35);
                    if (!dr.IsDBNull(iRerinddiah35)) entity.Rerinddiah35 = dr.GetDecimal(iRerinddiah35) * dTipoCambio;

                    int iRerinddiah36 = dr.GetOrdinal(helper.Rerinddiah36);
                    if (!dr.IsDBNull(iRerinddiah36)) entity.Rerinddiah36 = dr.GetDecimal(iRerinddiah36) * dTipoCambio;

                    int iRerinddiah37 = dr.GetOrdinal(helper.Rerinddiah37);
                    if (!dr.IsDBNull(iRerinddiah37)) entity.Rerinddiah37 = dr.GetDecimal(iRerinddiah37) * dTipoCambio;

                    int iRerinddiah38 = dr.GetOrdinal(helper.Rerinddiah38);
                    if (!dr.IsDBNull(iRerinddiah38)) entity.Rerinddiah38 = dr.GetDecimal(iRerinddiah38) * dTipoCambio;

                    int iRerinddiah39 = dr.GetOrdinal(helper.Rerinddiah39);
                    if (!dr.IsDBNull(iRerinddiah39)) entity.Rerinddiah39 = dr.GetDecimal(iRerinddiah39) * dTipoCambio;

                    int iRerinddiah40 = dr.GetOrdinal(helper.Rerinddiah40);
                    if (!dr.IsDBNull(iRerinddiah40)) entity.Rerinddiah40 = dr.GetDecimal(iRerinddiah40) * dTipoCambio;

                    int iRerinddiah41 = dr.GetOrdinal(helper.Rerinddiah41);
                    if (!dr.IsDBNull(iRerinddiah41)) entity.Rerinddiah41 = dr.GetDecimal(iRerinddiah41) * dTipoCambio;

                    int iRerinddiah42 = dr.GetOrdinal(helper.Rerinddiah42);
                    if (!dr.IsDBNull(iRerinddiah42)) entity.Rerinddiah42 = dr.GetDecimal(iRerinddiah42) * dTipoCambio;

                    int iRerinddiah43 = dr.GetOrdinal(helper.Rerinddiah43);
                    if (!dr.IsDBNull(iRerinddiah43)) entity.Rerinddiah43 = dr.GetDecimal(iRerinddiah43) * dTipoCambio;

                    int iRerinddiah44 = dr.GetOrdinal(helper.Rerinddiah44);
                    if (!dr.IsDBNull(iRerinddiah44)) entity.Rerinddiah44 = dr.GetDecimal(iRerinddiah44) * dTipoCambio;

                    int iRerinddiah45 = dr.GetOrdinal(helper.Rerinddiah45);
                    if (!dr.IsDBNull(iRerinddiah45)) entity.Rerinddiah45 = dr.GetDecimal(iRerinddiah45) * dTipoCambio;

                    int iRerinddiah46 = dr.GetOrdinal(helper.Rerinddiah46);
                    if (!dr.IsDBNull(iRerinddiah46)) entity.Rerinddiah46 = dr.GetDecimal(iRerinddiah46) * dTipoCambio;

                    int iRerinddiah47 = dr.GetOrdinal(helper.Rerinddiah47);
                    if (!dr.IsDBNull(iRerinddiah47)) entity.Rerinddiah47 = dr.GetDecimal(iRerinddiah47) * dTipoCambio;

                    int iRerinddiah48 = dr.GetOrdinal(helper.Rerinddiah48);
                    if (!dr.IsDBNull(iRerinddiah48)) entity.Rerinddiah48 = dr.GetDecimal(iRerinddiah48) * dTipoCambio;

                    int iRerinddiah49 = dr.GetOrdinal(helper.Rerinddiah49);
                    if (!dr.IsDBNull(iRerinddiah49)) entity.Rerinddiah49 = dr.GetDecimal(iRerinddiah49) * dTipoCambio;

                    int iRerinddiah50 = dr.GetOrdinal(helper.Rerinddiah50);
                    if (!dr.IsDBNull(iRerinddiah50)) entity.Rerinddiah50 = dr.GetDecimal(iRerinddiah50) * dTipoCambio;

                    int iRerinddiah51 = dr.GetOrdinal(helper.Rerinddiah51);
                    if (!dr.IsDBNull(iRerinddiah51)) entity.Rerinddiah51 = dr.GetDecimal(iRerinddiah51) * dTipoCambio;

                    int iRerinddiah52 = dr.GetOrdinal(helper.Rerinddiah52);
                    if (!dr.IsDBNull(iRerinddiah52)) entity.Rerinddiah52 = dr.GetDecimal(iRerinddiah52) * dTipoCambio;

                    int iRerinddiah53 = dr.GetOrdinal(helper.Rerinddiah53);
                    if (!dr.IsDBNull(iRerinddiah53)) entity.Rerinddiah53 = dr.GetDecimal(iRerinddiah53) * dTipoCambio;

                    int iRerinddiah54 = dr.GetOrdinal(helper.Rerinddiah54);
                    if (!dr.IsDBNull(iRerinddiah54)) entity.Rerinddiah54 = dr.GetDecimal(iRerinddiah54) * dTipoCambio;

                    int iRerinddiah55 = dr.GetOrdinal(helper.Rerinddiah55);
                    if (!dr.IsDBNull(iRerinddiah55)) entity.Rerinddiah55 = dr.GetDecimal(iRerinddiah55) * dTipoCambio;

                    int iRerinddiah56 = dr.GetOrdinal(helper.Rerinddiah56);
                    if (!dr.IsDBNull(iRerinddiah56)) entity.Rerinddiah56 = dr.GetDecimal(iRerinddiah56) * dTipoCambio;

                    int iRerinddiah57 = dr.GetOrdinal(helper.Rerinddiah57);
                    if (!dr.IsDBNull(iRerinddiah57)) entity.Rerinddiah57 = dr.GetDecimal(iRerinddiah57) * dTipoCambio;

                    int iRerinddiah58 = dr.GetOrdinal(helper.Rerinddiah58);
                    if (!dr.IsDBNull(iRerinddiah58)) entity.Rerinddiah58 = dr.GetDecimal(iRerinddiah58) * dTipoCambio;

                    int iRerinddiah59 = dr.GetOrdinal(helper.Rerinddiah59);
                    if (!dr.IsDBNull(iRerinddiah59)) entity.Rerinddiah59 = dr.GetDecimal(iRerinddiah59) * dTipoCambio;

                    int iRerinddiah60 = dr.GetOrdinal(helper.Rerinddiah60);
                    if (!dr.IsDBNull(iRerinddiah60)) entity.Rerinddiah60 = dr.GetDecimal(iRerinddiah60) * dTipoCambio;

                    int iRerinddiah61 = dr.GetOrdinal(helper.Rerinddiah61);
                    if (!dr.IsDBNull(iRerinddiah61)) entity.Rerinddiah61 = dr.GetDecimal(iRerinddiah61) * dTipoCambio;

                    int iRerinddiah62 = dr.GetOrdinal(helper.Rerinddiah62);
                    if (!dr.IsDBNull(iRerinddiah62)) entity.Rerinddiah62 = dr.GetDecimal(iRerinddiah62) * dTipoCambio;

                    int iRerinddiah63 = dr.GetOrdinal(helper.Rerinddiah63);
                    if (!dr.IsDBNull(iRerinddiah63)) entity.Rerinddiah63 = dr.GetDecimal(iRerinddiah63) * dTipoCambio;

                    int iRerinddiah64 = dr.GetOrdinal(helper.Rerinddiah64);
                    if (!dr.IsDBNull(iRerinddiah64)) entity.Rerinddiah64 = dr.GetDecimal(iRerinddiah64) * dTipoCambio;

                    int iRerinddiah65 = dr.GetOrdinal(helper.Rerinddiah65);
                    if (!dr.IsDBNull(iRerinddiah65)) entity.Rerinddiah65 = dr.GetDecimal(iRerinddiah65) * dTipoCambio;

                    int iRerinddiah66 = dr.GetOrdinal(helper.Rerinddiah66);
                    if (!dr.IsDBNull(iRerinddiah66)) entity.Rerinddiah66 = dr.GetDecimal(iRerinddiah66) * dTipoCambio;

                    int iRerinddiah67 = dr.GetOrdinal(helper.Rerinddiah67);
                    if (!dr.IsDBNull(iRerinddiah67)) entity.Rerinddiah67 = dr.GetDecimal(iRerinddiah67) * dTipoCambio;

                    int iRerinddiah68 = dr.GetOrdinal(helper.Rerinddiah68);
                    if (!dr.IsDBNull(iRerinddiah68)) entity.Rerinddiah68 = dr.GetDecimal(iRerinddiah68) * dTipoCambio;

                    int iRerinddiah69 = dr.GetOrdinal(helper.Rerinddiah69);
                    if (!dr.IsDBNull(iRerinddiah69)) entity.Rerinddiah69 = dr.GetDecimal(iRerinddiah69) * dTipoCambio;

                    int iRerinddiah70 = dr.GetOrdinal(helper.Rerinddiah70);
                    if (!dr.IsDBNull(iRerinddiah70)) entity.Rerinddiah70 = dr.GetDecimal(iRerinddiah70) * dTipoCambio;

                    int iRerinddiah71 = dr.GetOrdinal(helper.Rerinddiah71);
                    if (!dr.IsDBNull(iRerinddiah71)) entity.Rerinddiah71 = dr.GetDecimal(iRerinddiah71) * dTipoCambio;

                    int iRerinddiah72 = dr.GetOrdinal(helper.Rerinddiah72);
                    if (!dr.IsDBNull(iRerinddiah72)) entity.Rerinddiah72 = dr.GetDecimal(iRerinddiah72) * dTipoCambio;

                    int iRerinddiah73 = dr.GetOrdinal(helper.Rerinddiah73);
                    if (!dr.IsDBNull(iRerinddiah73)) entity.Rerinddiah73 = dr.GetDecimal(iRerinddiah73) * dTipoCambio;

                    int iRerinddiah74 = dr.GetOrdinal(helper.Rerinddiah74);
                    if (!dr.IsDBNull(iRerinddiah74)) entity.Rerinddiah74 = dr.GetDecimal(iRerinddiah74) * dTipoCambio;

                    int iRerinddiah75 = dr.GetOrdinal(helper.Rerinddiah75);
                    if (!dr.IsDBNull(iRerinddiah75)) entity.Rerinddiah75 = dr.GetDecimal(iRerinddiah75) * dTipoCambio;

                    int iRerinddiah76 = dr.GetOrdinal(helper.Rerinddiah76);
                    if (!dr.IsDBNull(iRerinddiah76)) entity.Rerinddiah76 = dr.GetDecimal(iRerinddiah76) * dTipoCambio;

                    int iRerinddiah77 = dr.GetOrdinal(helper.Rerinddiah77);
                    if (!dr.IsDBNull(iRerinddiah77)) entity.Rerinddiah77 = dr.GetDecimal(iRerinddiah77) * dTipoCambio;

                    int iRerinddiah78 = dr.GetOrdinal(helper.Rerinddiah78);
                    if (!dr.IsDBNull(iRerinddiah78)) entity.Rerinddiah78 = dr.GetDecimal(iRerinddiah78) * dTipoCambio;

                    int iRerinddiah79 = dr.GetOrdinal(helper.Rerinddiah79);
                    if (!dr.IsDBNull(iRerinddiah79)) entity.Rerinddiah79 = dr.GetDecimal(iRerinddiah79) * dTipoCambio;

                    int iRerinddiah80 = dr.GetOrdinal(helper.Rerinddiah80);
                    if (!dr.IsDBNull(iRerinddiah80)) entity.Rerinddiah80 = dr.GetDecimal(iRerinddiah80) * dTipoCambio;

                    int iRerinddiah81 = dr.GetOrdinal(helper.Rerinddiah81);
                    if (!dr.IsDBNull(iRerinddiah81)) entity.Rerinddiah81 = dr.GetDecimal(iRerinddiah81) * dTipoCambio;

                    int iRerinddiah82 = dr.GetOrdinal(helper.Rerinddiah82);
                    if (!dr.IsDBNull(iRerinddiah82)) entity.Rerinddiah82 = dr.GetDecimal(iRerinddiah82) * dTipoCambio;

                    int iRerinddiah83 = dr.GetOrdinal(helper.Rerinddiah83);
                    if (!dr.IsDBNull(iRerinddiah83)) entity.Rerinddiah83 = dr.GetDecimal(iRerinddiah83) * dTipoCambio;

                    int iRerinddiah84 = dr.GetOrdinal(helper.Rerinddiah84);
                    if (!dr.IsDBNull(iRerinddiah84)) entity.Rerinddiah84 = dr.GetDecimal(iRerinddiah84) * dTipoCambio;

                    int iRerinddiah85 = dr.GetOrdinal(helper.Rerinddiah85);
                    if (!dr.IsDBNull(iRerinddiah85)) entity.Rerinddiah85 = dr.GetDecimal(iRerinddiah85) * dTipoCambio;

                    int iRerinddiah86 = dr.GetOrdinal(helper.Rerinddiah86);
                    if (!dr.IsDBNull(iRerinddiah86)) entity.Rerinddiah86 = dr.GetDecimal(iRerinddiah86) * dTipoCambio;

                    int iRerinddiah87 = dr.GetOrdinal(helper.Rerinddiah87);
                    if (!dr.IsDBNull(iRerinddiah87)) entity.Rerinddiah87 = dr.GetDecimal(iRerinddiah87) * dTipoCambio;

                    int iRerinddiah88 = dr.GetOrdinal(helper.Rerinddiah88);
                    if (!dr.IsDBNull(iRerinddiah88)) entity.Rerinddiah88 = dr.GetDecimal(iRerinddiah88) * dTipoCambio;

                    int iRerinddiah89 = dr.GetOrdinal(helper.Rerinddiah89);
                    if (!dr.IsDBNull(iRerinddiah89)) entity.Rerinddiah89 = dr.GetDecimal(iRerinddiah89) * dTipoCambio;

                    int iRerinddiah90 = dr.GetOrdinal(helper.Rerinddiah90);
                    if (!dr.IsDBNull(iRerinddiah90)) entity.Rerinddiah90 = dr.GetDecimal(iRerinddiah90) * dTipoCambio;

                    int iRerinddiah91 = dr.GetOrdinal(helper.Rerinddiah91);
                    if (!dr.IsDBNull(iRerinddiah91)) entity.Rerinddiah91 = dr.GetDecimal(iRerinddiah91) * dTipoCambio;

                    int iRerinddiah92 = dr.GetOrdinal(helper.Rerinddiah92);
                    if (!dr.IsDBNull(iRerinddiah92)) entity.Rerinddiah92 = dr.GetDecimal(iRerinddiah92) * dTipoCambio;

                    int iRerinddiah93 = dr.GetOrdinal(helper.Rerinddiah93);
                    if (!dr.IsDBNull(iRerinddiah93)) entity.Rerinddiah93 = dr.GetDecimal(iRerinddiah93) * dTipoCambio;

                    int iRerinddiah94 = dr.GetOrdinal(helper.Rerinddiah94);
                    if (!dr.IsDBNull(iRerinddiah94)) entity.Rerinddiah94 = dr.GetDecimal(iRerinddiah94) * dTipoCambio;

                    int iRerinddiah95 = dr.GetOrdinal(helper.Rerinddiah95);
                    if (!dr.IsDBNull(iRerinddiah95)) entity.Rerinddiah95 = dr.GetDecimal(iRerinddiah95) * dTipoCambio;

                    int iRerinddiah96 = dr.GetOrdinal(helper.Rerinddiah96);
                    if (!dr.IsDBNull(iRerinddiah96)) entity.Rerinddiah96 = dr.GetDecimal(iRerinddiah96) * dTipoCambio;

                    int iRerinddiatotal = dr.GetOrdinal(helper.Rerinddiatotal);
                    if (!dr.IsDBNull(iRerinddiatotal)) entity.Rerinddiatotal = dr.GetDecimal(iRerinddiatotal) * dTipoCambio;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

