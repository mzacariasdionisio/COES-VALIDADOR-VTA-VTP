using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;
using COES.Dominio.DTO.Sic;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CAI_AJUSTEEMPRESA
    /// </summary>
    public class CaiAjusteempresaRepository: RepositoryBase, ICaiAjusteempresaRepository
    {
        public CaiAjusteempresaRepository(string strConn): base(strConn)
        {
        }

        CaiAjusteempresaHelper helper = new CaiAjusteempresaHelper();

        public int Save(CaiAjusteempresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Caiajecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Caiajetipoinfo, DbType.String, entity.Caiajetipoinfo);
            dbProvider.AddInParameter(command, helper.Caiajereteneejeini, DbType.DateTime, entity.Caiajereteneejeini);
            dbProvider.AddInParameter(command, helper.Caiajereteneejefin, DbType.DateTime, entity.Caiajereteneejefin);
            dbProvider.AddInParameter(command, helper.Caiajeretenepryaini, DbType.DateTime, entity.Caiajeretenepryaini);
            dbProvider.AddInParameter(command, helper.Caiajeretenepryafin, DbType.DateTime, entity.Caiajeretenepryafin);
            dbProvider.AddInParameter(command, helper.Caiajereteneprybini, DbType.DateTime, entity.Caiajereteneprybini);
            dbProvider.AddInParameter(command, helper.Caiajereteneprybfin, DbType.DateTime, entity.Caiajereteneprybfin);
            dbProvider.AddInParameter(command, helper.Caiajeusucreacion, DbType.String, entity.Caiajeusucreacion);
            dbProvider.AddInParameter(command, helper.Caiajefeccreacion, DbType.DateTime, entity.Caiajefeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiAjusteempresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Caiajetipoinfo, DbType.String, entity.Caiajetipoinfo);
            dbProvider.AddInParameter(command, helper.Caiajereteneejeini, DbType.DateTime, entity.Caiajereteneejeini);
            dbProvider.AddInParameter(command, helper.Caiajereteneejefin, DbType.DateTime, entity.Caiajereteneejefin);
            dbProvider.AddInParameter(command, helper.Caiajeretenepryaini, DbType.DateTime, entity.Caiajeretenepryaini);
            dbProvider.AddInParameter(command, helper.Caiajeretenepryafin, DbType.DateTime, entity.Caiajeretenepryafin);
            dbProvider.AddInParameter(command, helper.Caiajereteneprybini, DbType.DateTime, entity.Caiajereteneprybini);
            dbProvider.AddInParameter(command, helper.Caiajereteneprybfin, DbType.DateTime, entity.Caiajereteneprybfin);
            dbProvider.AddInParameter(command, helper.Caiajeusucreacion, DbType.String, entity.Caiajeusucreacion);
            dbProvider.AddInParameter(command, helper.Caiajefeccreacion, DbType.DateTime, entity.Caiajefeccreacion);
            dbProvider.AddInParameter(command, helper.Caiajecodi, DbType.Int32, entity.Caiajecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int caiajecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Caiajecodi, DbType.Int32, caiajecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiAjusteempresaDTO GetById(int caiajecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Caiajecodi, DbType.Int32, caiajecodi);
            CaiAjusteempresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iPtoMediCodi = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedielenomb = dr.GetString(iPtoMediCodi);
                }
            }

            return entity;
        }

        public List<CaiAjusteempresaDTO> List()
        {
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
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

        public List<CaiAjusteempresaDTO> GetByCriteria()
        {
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
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

        public List<CaiAjusteempresaDTO> ListAjuste(int caiajcodi, string caiajetipoinfo)
        {
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAjuste);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            dbProvider.AddInParameter(command, helper.Caiajetipoinfo, DbType.String, caiajetipoinfo);

            CaiAjusteempresaDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iTipoemprdesc = dr.GetOrdinal(this.helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CaiAjusteempresaDTO> ListAjusteEmpresa(int caiajcodi, int emprcodi)
        {
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAjusteEmpresa);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            CaiAjusteempresaDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iPtoMediCodi = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedielenomb = dr.GetString(iPtoMediCodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CaiAjusteempresaDTO> ListEmpresaByAjusteTipoEmpresa(int caiajcodi, int tipoemprcodi)
        {
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaByAjusteTipoEmpresa);

            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiAjusteempresaDTO entity = new CaiAjusteempresaDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CaiAjusteempresaDTO> ListEmpresasByAjuste(int caiajcodi)
        {
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresasByAjuste);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiAjusteempresaDTO entity = new CaiAjusteempresaDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CaiAjusteempresaDTO> ListCaiAjusteempresasTipoEmpresa(int caiajcodi)
        {
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCaiAjusteempresasTipoEmpresa);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);

            CaiAjusteempresaDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iTipoEmprDesc = dr.GetOrdinal(this.helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoEmprDesc)) entity.Tipoemprdesc = dr.GetString(iTipoEmprDesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CaiAjusteempresaDTO> ListEmpresasXPtoGeneracion(string sFechaInicio, string sFechaFin, string tiposGeneracion, string empresas, int IdFamiliaSSAA, int IdTipogrupoNoIntegrante,
            int lectcodi, int IdTipoInfoPotenciaActiva, int TptoMedicodiTodos)
        {
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
            string sql = String.Format(this.helper.SqlListEmpresasXPtoGeneracion, tiposGeneracion, IdFamiliaSSAA, IdTipogrupoNoIntegrante
              , sFechaInicio, sFechaFin, empresas, lectcodi, IdTipoInfoPotenciaActiva, TptoMedicodiTodos);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiAjusteempresaDTO entity = new CaiAjusteempresaDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CaiAjusteempresaDTO> ListEmpresasXPtoUL(string sFechaInicio, string sFechaFin, int iFormatcodi, int iTipoEmprcodi, string lectCodiPR16, string lectCodiAlpha)
        {
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
            string sql = String.Format(this.helper.SqlListEmpresasXPtoUL, sFechaInicio, sFechaFin, iFormatcodi, iTipoEmprcodi, lectCodiPR16, lectCodiAlpha);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiAjusteempresaDTO entity = new CaiAjusteempresaDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CaiAjusteempresaDTO> ListEmpresasXPtoDist()
        {
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
            string sql = String.Format(this.helper.SqlListEmpresasXPtoDist);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiAjusteempresaDTO entity = new CaiAjusteempresaDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CaiAjusteempresaDTO> ListEmpresasXPtoTrans()
        {
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
            string sql = String.Format(this.helper.SqlListEmpresasXPtoTrans);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiAjusteempresaDTO entity = new CaiAjusteempresaDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CaiAjusteempresaDTO> ObtenerListaPeriodoEjecutado(string caiajetipoinfo, int caiajcodi, int emprcodi)
        {
            string sqlQuery = string.Format(this.helper.SqlObtenerListaPeriodoEjecutado, emprcodi, caiajetipoinfo, caiajcodi);
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiAjusteempresaDTO entity = new CaiAjusteempresaDTO();

                    int iFechaPeriodo = dr.GetOrdinal(helper.FechaPeriodo);
                    if (!dr.IsDBNull(iFechaPeriodo)) entity.FechaPeriodo = dr.GetDateTime(iFechaPeriodo);

                    int iPeriodo = dr.GetOrdinal(helper.Periodo);
                    if (!dr.IsDBNull(iPeriodo)) entity.Periodo = dr.GetString(iPeriodo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CaiAjusteempresaDTO> ObtenerListaPeriodoProyectado(string caiajetipoinfo, int caiajcodi, int emprcodi)
        {
            string sqlQuery = string.Format(this.helper.SqlObtenerListaPeriodoProyectado, emprcodi, caiajetipoinfo, caiajcodi);
            List<CaiAjusteempresaDTO> entitys = new List<CaiAjusteempresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiAjusteempresaDTO entity = new CaiAjusteempresaDTO();

                    int iFechaPeriodo = dr.GetOrdinal(helper.FechaPeriodo);
                    if (!dr.IsDBNull(iFechaPeriodo)) entity.FechaPeriodo = dr.GetDateTime(iFechaPeriodo);

                    int iPeriodo = dr.GetOrdinal(helper.Periodo);
                    if (!dr.IsDBNull(iPeriodo)) entity.Periodo = dr.GetString(iPeriodo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
        public MePtomedicionDTO GetMePtomedicionByNombre(int Emprcodi, string Ptomedidesc)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMePtomedicionByNombre);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedidesc, DbType.Int32, Ptomedidesc);
            MePtomedicionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                }
            }

            return entity;
        }

        public List<MeHojaptomedDTO> GetByCriteriaMeHojaptomeds(int emprcodi, int formatcodi)
        {
            List<MeHojaptomedDTO> entitys = new List<MeHojaptomedDTO>();
            MeHojaptomedHelper helperHojaPto = new MeHojaptomedHelper();
            string queryString = string.Format(helper.SqlGetByCriteriaMeHojaptomeds, emprcodi, formatcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            MeHojaptomedDTO entity = new MeHojaptomedDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helperHojaPto.Create(dr);
                    entity.Tipoinfoabrev = dr.GetString(dr.GetOrdinal(helperHojaPto.Tipoinfoabrev));
                    if (!dr.IsDBNull(dr.GetOrdinal(helperHojaPto.Equinomb))) entity.Equinomb = dr.GetString(dr.GetOrdinal(helperHojaPto.Equinomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helperHojaPto.Emprabrev))) entity.Emprabrev = dr.GetString(dr.GetOrdinal(helperHojaPto.Emprabrev));
                    if (!dr.IsDBNull(dr.GetOrdinal(helperHojaPto.Tptomedicodi))) entity.Tptomedicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helperHojaPto.Tptomedicodi)));
                    if (!dr.IsDBNull(dr.GetOrdinal(helperHojaPto.Famcodi))) entity.Famcodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helperHojaPto.Famcodi)));
                    if (!dr.IsDBNull(dr.GetOrdinal(helperHojaPto.Tipoptomedinomb))) entity.Tipoptomedinomb = dr.GetString(dr.GetOrdinal(helperHojaPto.Tipoptomedinomb));

                    //- JDEL Fin

                    if (!dr.IsDBNull(dr.GetOrdinal(helperHojaPto.Ptomedibarranomb))) entity.Ptomedibarranomb = dr.GetString(dr.GetOrdinal(helperHojaPto.Ptomedibarranomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(helperHojaPto.Ptomedidesc))) entity.Ptomedidesc = dr.GetString(dr.GetOrdinal(helperHojaPto.Ptomedidesc));
                    if (!dr.IsDBNull(dr.GetOrdinal(helperHojaPto.Equicodi))) entity.Equicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helperHojaPto.Equicodi)));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListPtomed(int origlectcodi)
        {
            MePtomedicionHelper helperPtoMed = new MePtomedicionHelper();
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.ListPtomed, origlectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helperPtoMed.Create(dr));
                }
            }

            return entitys;
        }
    }
}
