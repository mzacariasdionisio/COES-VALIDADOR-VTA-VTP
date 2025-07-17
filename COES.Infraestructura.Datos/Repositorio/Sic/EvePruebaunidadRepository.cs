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
    /// Clase de acceso a datos de la tabla EVE_PRUEBAUNIDAD
    /// </summary>
    public class EvePruebaunidadRepository : RepositoryBase, IEvePruebaunidadRepository
    {
        public EvePruebaunidadRepository(string strConn)
            : base(strConn)
        {
        }

        EvePruebaunidadHelper helper = new EvePruebaunidadHelper();
        EqEquipoHelper helperEquipo = new EqEquipoHelper();

        public int Save(EvePruebaunidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prundcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prundfecha, DbType.DateTime, entity.Prundfecha);
            dbProvider.AddInParameter(command, helper.Prundescenario, DbType.Int32, entity.Prundescenario);
            dbProvider.AddInParameter(command, helper.Prundhoraordenarranque, DbType.DateTime, entity.Prundhoraordenarranque);
            dbProvider.AddInParameter(command, helper.Prundhorasincronizacion, DbType.DateTime, entity.Prundhorasincronizacion);
            dbProvider.AddInParameter(command, helper.Prundhorainiplenacarga, DbType.DateTime, entity.Prundhorainiplenacarga);
            dbProvider.AddInParameter(command, helper.Prundhorafalla, DbType.DateTime, entity.Prundhorafalla);
            dbProvider.AddInParameter(command, helper.Prundhoraordenarranque2, DbType.DateTime, entity.Prundhoraordenarranque2);
            dbProvider.AddInParameter(command, helper.Prundhorasincronizacion2, DbType.DateTime, entity.Prundhorasincronizacion2);
            dbProvider.AddInParameter(command, helper.Prundhorainiplenacarga2, DbType.DateTime, entity.Prundhorainiplenacarga2);
            dbProvider.AddInParameter(command, helper.Prundsegundadesconx, DbType.String, entity.Prundsegundadesconx);
            dbProvider.AddInParameter(command, helper.Prundfallaotranosincronz, DbType.String, entity.Prundfallaotranosincronz);
            dbProvider.AddInParameter(command, helper.Prundfallaotraunidsincronz, DbType.String, entity.Prundfallaotraunidsincronz);
            dbProvider.AddInParameter(command, helper.Prundfallaequiposinreingreso, DbType.String, entity.Prundfallaequiposinreingreso);
            dbProvider.AddInParameter(command, helper.Prundcalchayregmedid, DbType.String, entity.Prundcalchayregmedid);
            dbProvider.AddInParameter(command, helper.Prundcalchorafineval, DbType.DateTime, entity.Prundcalchorafineval);
            dbProvider.AddInParameter(command, helper.Prundcalhayindisp, DbType.String, entity.Prundcalhayindisp);
            dbProvider.AddInParameter(command, helper.Prundcalcpruebaexitosa, DbType.String, entity.Prundcalcpruebaexitosa);
            dbProvider.AddInParameter(command, helper.Prundcalcperiodoprogprueba, DbType.Decimal, entity.Prundcalcperiodoprogprueba);
            dbProvider.AddInParameter(command, helper.Prundcalccondhoratarr, DbType.String, entity.Prundcalccondhoratarr);
            dbProvider.AddInParameter(command, helper.Prundcalccondhoraprogtarr, DbType.String, entity.Prundcalccondhoraprogtarr);
            dbProvider.AddInParameter(command, helper.Prundcalcindispprimtramo, DbType.String, entity.Prundcalcindispprimtramo);
            dbProvider.AddInParameter(command, helper.Prundcalcindispsegtramo, DbType.String, entity.Prundcalcindispsegtramo);
            dbProvider.AddInParameter(command, helper.Prundrpf, DbType.Decimal, entity.Prundrpf);
            dbProvider.AddInParameter(command, helper.Prundtiempoprueba, DbType.Decimal, entity.Prundtiempoprueba);
            dbProvider.AddInParameter(command, helper.Prundusucreacion, DbType.String, entity.Prundusucreacion);
            dbProvider.AddInParameter(command, helper.Prundfeccreacion, DbType.DateTime, entity.Prundfeccreacion);
            dbProvider.AddInParameter(command, helper.Prundusumodificacion, DbType.String, entity.Prundusumodificacion);
            dbProvider.AddInParameter(command, helper.Prundfecmodificacion, DbType.DateTime, entity.Prundfecmodificacion);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Prundeliminado, DbType.String, entity.Prundeliminado);

            dbProvider.AddInParameter(command, helper.Prundpotefectiva, DbType.Decimal, entity.Prundpotefectiva);
            dbProvider.AddInParameter(command, helper.Prundtiempoentarranq, DbType.Decimal, entity.Prundtiempoentarranq);
            dbProvider.AddInParameter(command, helper.Prundtiempoarranqasinc, DbType.Decimal, entity.Prundtiempoarranqasinc);
            dbProvider.AddInParameter(command, helper.Prundtiemposincapotefect, DbType.Decimal, entity.Prundtiemposincapotefect);


            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EvePruebaunidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Prundfecha, DbType.DateTime, entity.Prundfecha);
            dbProvider.AddInParameter(command, helper.Prundescenario, DbType.Int32, entity.Prundescenario);
            dbProvider.AddInParameter(command, helper.Prundhoraordenarranque, DbType.DateTime, entity.Prundhoraordenarranque);
            dbProvider.AddInParameter(command, helper.Prundhorasincronizacion, DbType.DateTime, entity.Prundhorasincronizacion);
            dbProvider.AddInParameter(command, helper.Prundhorainiplenacarga, DbType.DateTime, entity.Prundhorainiplenacarga);
            dbProvider.AddInParameter(command, helper.Prundhorafalla, DbType.DateTime, entity.Prundhorafalla);
            dbProvider.AddInParameter(command, helper.Prundhoraordenarranque2, DbType.DateTime, entity.Prundhoraordenarranque2);
            dbProvider.AddInParameter(command, helper.Prundhorasincronizacion2, DbType.DateTime, entity.Prundhorasincronizacion2);
            dbProvider.AddInParameter(command, helper.Prundhorainiplenacarga2, DbType.DateTime, entity.Prundhorainiplenacarga2);
            dbProvider.AddInParameter(command, helper.Prundsegundadesconx, DbType.String, entity.Prundsegundadesconx);
            dbProvider.AddInParameter(command, helper.Prundfallaotranosincronz, DbType.String, entity.Prundfallaotranosincronz);
            dbProvider.AddInParameter(command, helper.Prundfallaotraunidsincronz, DbType.String, entity.Prundfallaotraunidsincronz);
            dbProvider.AddInParameter(command, helper.Prundfallaequiposinreingreso, DbType.String, entity.Prundfallaequiposinreingreso);
            dbProvider.AddInParameter(command, helper.Prundcalchayregmedid, DbType.String, entity.Prundcalchayregmedid);
            dbProvider.AddInParameter(command, helper.Prundcalchorafineval, DbType.DateTime, entity.Prundcalchorafineval);
            dbProvider.AddInParameter(command, helper.Prundcalhayindisp, DbType.String, entity.Prundcalhayindisp);
            dbProvider.AddInParameter(command, helper.Prundcalcpruebaexitosa, DbType.String, entity.Prundcalcpruebaexitosa);
            dbProvider.AddInParameter(command, helper.Prundcalcperiodoprogprueba, DbType.Decimal, entity.Prundcalcperiodoprogprueba);
            dbProvider.AddInParameter(command, helper.Prundcalccondhoratarr, DbType.String, entity.Prundcalccondhoratarr);
            dbProvider.AddInParameter(command, helper.Prundcalccondhoraprogtarr, DbType.String, entity.Prundcalccondhoraprogtarr);
            dbProvider.AddInParameter(command, helper.Prundcalcindispprimtramo, DbType.String, entity.Prundcalcindispprimtramo);
            dbProvider.AddInParameter(command, helper.Prundcalcindispsegtramo, DbType.String, entity.Prundcalcindispsegtramo);
            dbProvider.AddInParameter(command, helper.Prundrpf, DbType.Decimal, entity.Prundrpf);
            dbProvider.AddInParameter(command, helper.Prundtiempoprueba, DbType.Decimal, entity.Prundtiempoprueba);
            dbProvider.AddInParameter(command, helper.Prundusucreacion, DbType.String, entity.Prundusucreacion);
            dbProvider.AddInParameter(command, helper.Prundfeccreacion, DbType.DateTime, entity.Prundfeccreacion);
            dbProvider.AddInParameter(command, helper.Prundusumodificacion, DbType.String, entity.Prundusumodificacion);
            dbProvider.AddInParameter(command, helper.Prundfecmodificacion, DbType.DateTime, entity.Prundfecmodificacion);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Prundeliminado, DbType.String, entity.Prundeliminado);

            dbProvider.AddInParameter(command, helper.Prundpotefectiva, DbType.Decimal, entity.Prundpotefectiva);
            dbProvider.AddInParameter(command, helper.Prundtiempoentarranq, DbType.Decimal, entity.Prundtiempoentarranq);
            dbProvider.AddInParameter(command, helper.Prundtiempoarranqasinc, DbType.Decimal, entity.Prundtiempoarranqasinc);
            dbProvider.AddInParameter(command, helper.Prundtiemposincapotefect, DbType.Decimal, entity.Prundtiemposincapotefect);


            dbProvider.AddInParameter(command, helper.Prundcodi, DbType.Int32, entity.Prundcodi);



            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int prundcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Prundcodi, DbType.Int32, prundcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EvePruebaunidadDTO GetById(int prundcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Prundcodi, DbType.Int32, prundcodi);
            EvePruebaunidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    //se agrega la unidad
                    int iPrundUnidad = dr.GetOrdinal(this.helper.PrundUnidad);
                    if (!dr.IsDBNull(iPrundUnidad)) entity.PrundUnidad = dr.GetString(iPrundUnidad);

                }
            }

            return entity;
        }

        public List<EvePruebaunidadDTO> List()
        {
            List<EvePruebaunidadDTO> entitys = new List<EvePruebaunidadDTO>();
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

        public List<EvePruebaunidadDTO> GetByCriteria(DateTime prundFechaIni, DateTime prundFechaFin)
        {
            EvePruebaunidadDTO entity = null;
            List<EvePruebaunidadDTO> entitys = new List<EvePruebaunidadDTO>();
            string query = string.Format(helper.SqlGetByCriteria, prundFechaIni.ToString(ConstantesBase.FormatoFecha), prundFechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EvePruebaunidadDTO();
                    entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupoabrev = dr.GetOrdinal(this.helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Graba los datos de la tabla EVE_PRUEBAUNIDAD
        /// </summary>
        public int SaveEvePruebaunidadId(EvePruebaunidadDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Prundcodi == 0)
                    id = Save(entity);
                else
                {
                    Update(entity);
                    id = entity.Prundcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<EvePruebaunidadDTO> BuscarOperaciones(string estado, DateTime prundFechaIni, DateTime prundFechaFin, int nroPage, int pageSize)
        {
            List<EvePruebaunidadDTO> entitys = new List<EvePruebaunidadDTO>();
            String sql = String.Format(this.helper.ObtenerListado, estado, prundFechaIni.ToString(ConstantesBase.FormatoFecha), prundFechaFin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EvePruebaunidadDTO entity = new EvePruebaunidadDTO();

                    int iPrundcodi = dr.GetOrdinal(this.helper.Prundcodi);
                    if (!dr.IsDBNull(iPrundcodi)) entity.Prundcodi = Convert.ToInt32(dr.GetValue(iPrundcodi));

                    int iPrundfecha = dr.GetOrdinal(this.helper.Prundfecha);
                    if (!dr.IsDBNull(iPrundfecha)) entity.Prundfecha = dr.GetDateTime(iPrundfecha);

                    int iPrundescenario = dr.GetOrdinal(this.helper.Prundescenario);
                    if (!dr.IsDBNull(iPrundescenario)) entity.Prundescenario = Convert.ToInt32(dr.GetValue(iPrundescenario));

                    int iPrundhoraordenarranque = dr.GetOrdinal(this.helper.Prundhoraordenarranque);
                    if (!dr.IsDBNull(iPrundhoraordenarranque)) entity.Prundhoraordenarranque = dr.GetDateTime(iPrundhoraordenarranque);

                    int iPrundhorasincronizacion = dr.GetOrdinal(this.helper.Prundhorasincronizacion);
                    if (!dr.IsDBNull(iPrundhorasincronizacion)) entity.Prundhorasincronizacion = dr.GetDateTime(iPrundhorasincronizacion);

                    int iPrundhorainiplenacarga = dr.GetOrdinal(this.helper.Prundhorainiplenacarga);
                    if (!dr.IsDBNull(iPrundhorainiplenacarga)) entity.Prundhorainiplenacarga = dr.GetDateTime(iPrundhorainiplenacarga);

                    int iPrundhorafalla = dr.GetOrdinal(this.helper.Prundhorafalla);
                    if (!dr.IsDBNull(iPrundhorafalla)) entity.Prundhorafalla = dr.GetDateTime(iPrundhorafalla);

                    int iPrundhoraordenarranque2 = dr.GetOrdinal(this.helper.Prundhoraordenarranque2);
                    if (!dr.IsDBNull(iPrundhoraordenarranque2)) entity.Prundhoraordenarranque2 = dr.GetDateTime(iPrundhoraordenarranque2);

                    int iPrundhorasincronizacion2 = dr.GetOrdinal(this.helper.Prundhorasincronizacion2);
                    if (!dr.IsDBNull(iPrundhorasincronizacion2)) entity.Prundhorasincronizacion2 = dr.GetDateTime(iPrundhorasincronizacion2);

                    int iPrundhorainiplenacarga2 = dr.GetOrdinal(this.helper.Prundhorainiplenacarga2);
                    if (!dr.IsDBNull(iPrundhorainiplenacarga2)) entity.Prundhorainiplenacarga2 = dr.GetDateTime(iPrundhorainiplenacarga2);

                    int iPrundsegundadesconx = dr.GetOrdinal(this.helper.Prundsegundadesconx);
                    if (!dr.IsDBNull(iPrundsegundadesconx)) entity.Prundsegundadesconx = dr.GetString(iPrundsegundadesconx);

                    int iPrundfallaotranosincronz = dr.GetOrdinal(this.helper.Prundfallaotranosincronz);
                    if (!dr.IsDBNull(iPrundfallaotranosincronz)) entity.Prundfallaotranosincronz = dr.GetString(iPrundfallaotranosincronz);

                    int iPrundfallaotraunidsincronz = dr.GetOrdinal(this.helper.Prundfallaotraunidsincronz);
                    if (!dr.IsDBNull(iPrundfallaotraunidsincronz)) entity.Prundfallaotraunidsincronz = dr.GetString(iPrundfallaotraunidsincronz);

                    int iPrundfallaequiposinreingreso = dr.GetOrdinal(this.helper.Prundfallaequiposinreingreso);
                    if (!dr.IsDBNull(iPrundfallaequiposinreingreso)) entity.Prundfallaequiposinreingreso = dr.GetString(iPrundfallaequiposinreingreso);

                    int iPrundcalchayregmedid = dr.GetOrdinal(this.helper.Prundcalchayregmedid);
                    if (!dr.IsDBNull(iPrundcalchayregmedid)) entity.Prundcalchayregmedid = dr.GetString(iPrundcalchayregmedid);

                    int iPrundcalchorafineval = dr.GetOrdinal(this.helper.Prundcalchorafineval);
                    if (!dr.IsDBNull(iPrundcalchorafineval)) entity.Prundcalchorafineval = dr.GetDateTime(iPrundcalchorafineval);

                    int iPrundcalhayindisp = dr.GetOrdinal(this.helper.Prundcalhayindisp);
                    if (!dr.IsDBNull(iPrundcalhayindisp)) entity.Prundcalhayindisp = dr.GetString(iPrundcalhayindisp);

                    int iPrundcalcpruebaexitosa = dr.GetOrdinal(this.helper.Prundcalcpruebaexitosa);
                    if (!dr.IsDBNull(iPrundcalcpruebaexitosa)) entity.Prundcalcpruebaexitosa = dr.GetString(iPrundcalcpruebaexitosa);

                    int iPrundcalcperiodoprogprueba = dr.GetOrdinal(this.helper.Prundcalcperiodoprogprueba);
                    if (!dr.IsDBNull(iPrundcalcperiodoprogprueba)) entity.Prundcalcperiodoprogprueba = dr.GetDecimal(iPrundcalcperiodoprogprueba);

                    int iPrundcalccondhoratarr = dr.GetOrdinal(this.helper.Prundcalccondhoratarr);
                    if (!dr.IsDBNull(iPrundcalccondhoratarr)) entity.Prundcalccondhoratarr = dr.GetString(iPrundcalccondhoratarr);

                    int iPrundcalccondhoraprogtarr = dr.GetOrdinal(this.helper.Prundcalccondhoraprogtarr);
                    if (!dr.IsDBNull(iPrundcalccondhoraprogtarr)) entity.Prundcalccondhoraprogtarr = dr.GetString(iPrundcalccondhoraprogtarr);

                    int iPrundcalcindispprimtramo = dr.GetOrdinal(this.helper.Prundcalcindispprimtramo);
                    if (!dr.IsDBNull(iPrundcalcindispprimtramo)) entity.Prundcalcindispprimtramo = dr.GetString(iPrundcalcindispprimtramo);

                    int iPrundcalcindispsegtramo = dr.GetOrdinal(this.helper.Prundcalcindispsegtramo);
                    if (!dr.IsDBNull(iPrundcalcindispsegtramo)) entity.Prundcalcindispsegtramo = dr.GetString(iPrundcalcindispsegtramo);

                    int iPrundrpf = dr.GetOrdinal(this.helper.Prundrpf);
                    if (!dr.IsDBNull(iPrundrpf)) entity.Prundrpf = dr.GetDecimal(iPrundrpf);

                    int iPrundtiempoprueba = dr.GetOrdinal(this.helper.Prundtiempoprueba);
                    if (!dr.IsDBNull(iPrundtiempoprueba)) entity.Prundtiempoprueba = dr.GetDecimal(iPrundtiempoprueba);

                    int iPrundusucreacion = dr.GetOrdinal(this.helper.Prundusucreacion);
                    if (!dr.IsDBNull(iPrundusucreacion)) entity.Prundusucreacion = dr.GetString(iPrundusucreacion);

                    int iPrundfeccreacion = dr.GetOrdinal(this.helper.Prundfeccreacion);
                    if (!dr.IsDBNull(iPrundfeccreacion)) entity.Prundfeccreacion = dr.GetDateTime(iPrundfeccreacion);

                    int iPrundusumodificacion = dr.GetOrdinal(this.helper.Prundusumodificacion);
                    if (!dr.IsDBNull(iPrundusumodificacion)) entity.Prundusumodificacion = dr.GetString(iPrundusumodificacion);

                    int iPrundfecmodificacion = dr.GetOrdinal(this.helper.Prundfecmodificacion);
                    if (!dr.IsDBNull(iPrundfecmodificacion)) entity.Prundfecmodificacion = dr.GetDateTime(iPrundfecmodificacion);

                    int iPrundUnidad = dr.GetOrdinal(this.helper.PrundUnidad);
                    if (!dr.IsDBNull(iPrundUnidad)) entity.PrundUnidad = dr.GetString(iPrundUnidad);

                    int iPrundEliminado = dr.GetOrdinal(this.helper.Prundeliminado);
                    if (!dr.IsDBNull(iPrundEliminado)) entity.Prundeliminado = dr.GetString(iPrundEliminado);

                    int iPrGrupo = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iPrGrupo)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iPrGrupo));

                    int iPrundpotefectiva = dr.GetOrdinal(this.helper.Prundpotefectiva);
                    if (!dr.IsDBNull(iPrundpotefectiva)) entity.Prundpotefectiva = dr.GetDecimal(iPrundpotefectiva);
                                       

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(string estado, DateTime prundFechaIni, DateTime prundFechaFin)
        {
            String sql = String.Format(this.helper.TotalRegistros, estado, prundFechaIni.ToString(ConstantesBase.FormatoFecha), prundFechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }


        public EqEquipoDTO ObtenerUnidadSorteada(DateTime prundFecha)
        {
            String sql = String.Format(this.helper.SqlUnidadSorteada, prundFecha.ToString(ConstantesBase.FormatoFecha));

            EqEquipoDTO equipo = null;

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquiCodi = dr.GetOrdinal(helperEquipo.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(helperEquipo.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    int iEmprCodi = dr.GetOrdinal(helperEquipo.Emprcodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iEmprNomb = dr.GetOrdinal(helperEquipo.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    equipo = entity;
                }

            }

            return equipo;

        }

        public List<EqEquipoDTO> ObtenerUnidadSorteadaHabilitada(DateTime prundFecha)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            String sql = String.Format(this.helper.SqlUnidadSorteadaHabilitada, prundFecha.ToString(ConstantesBase.FormatoFecha));

            EqEquipoDTO equipo = null;

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquiCodi = dr.GetOrdinal(helperEquipo.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(helperEquipo.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    int iEmprCodi = dr.GetOrdinal(helperEquipo.Emprcodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iEmprNomb = dr.GetOrdinal(helperEquipo.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    entitys.Add(entity);
                }

            }

            return entitys;

        }


    }
}
