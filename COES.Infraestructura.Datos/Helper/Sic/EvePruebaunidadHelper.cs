using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_PRUEBAUNIDAD
    /// </summary>
    public class EvePruebaunidadHelper : HelperBase
    {
        public EvePruebaunidadHelper()
            : base(Consultas.EvePruebaunidadSql)
        {
        }

        public EvePruebaunidadDTO Create(IDataReader dr)
        {
            EvePruebaunidadDTO entity = new EvePruebaunidadDTO();

            int iPrundcodi = dr.GetOrdinal(this.Prundcodi);
            if (!dr.IsDBNull(iPrundcodi)) entity.Prundcodi = Convert.ToInt32(dr.GetValue(iPrundcodi));

            int iPrundfecha = dr.GetOrdinal(this.Prundfecha);
            if (!dr.IsDBNull(iPrundfecha)) entity.Prundfecha = dr.GetDateTime(iPrundfecha);

            int iPrundescenario = dr.GetOrdinal(this.Prundescenario);
            if (!dr.IsDBNull(iPrundescenario)) entity.Prundescenario = Convert.ToInt32(dr.GetValue(iPrundescenario));

            int iPrundhoraordenarranque = dr.GetOrdinal(this.Prundhoraordenarranque);
            if (!dr.IsDBNull(iPrundhoraordenarranque)) entity.Prundhoraordenarranque = dr.GetDateTime(iPrundhoraordenarranque);

            int iPrundhorasincronizacion = dr.GetOrdinal(this.Prundhorasincronizacion);
            if (!dr.IsDBNull(iPrundhorasincronizacion)) entity.Prundhorasincronizacion = dr.GetDateTime(iPrundhorasincronizacion);

            int iPrundhorainiplenacarga = dr.GetOrdinal(this.Prundhorainiplenacarga);
            if (!dr.IsDBNull(iPrundhorainiplenacarga)) entity.Prundhorainiplenacarga = dr.GetDateTime(iPrundhorainiplenacarga);

            int iPrundhorafalla = dr.GetOrdinal(this.Prundhorafalla);
            if (!dr.IsDBNull(iPrundhorafalla)) entity.Prundhorafalla = dr.GetDateTime(iPrundhorafalla);

            int iPrundhoraordenarranque2 = dr.GetOrdinal(this.Prundhoraordenarranque2);
            if (!dr.IsDBNull(iPrundhoraordenarranque2)) entity.Prundhoraordenarranque2 = dr.GetDateTime(iPrundhoraordenarranque2);

            int iPrundhorasincronizacion2 = dr.GetOrdinal(this.Prundhorasincronizacion2);
            if (!dr.IsDBNull(iPrundhorasincronizacion2)) entity.Prundhorasincronizacion2 = dr.GetDateTime(iPrundhorasincronizacion2);

            int iPrundhorainiplenacarga2 = dr.GetOrdinal(this.Prundhorainiplenacarga2);
            if (!dr.IsDBNull(iPrundhorainiplenacarga2)) entity.Prundhorainiplenacarga2 = dr.GetDateTime(iPrundhorainiplenacarga2);

            int iPrundsegundadesconx = dr.GetOrdinal(this.Prundsegundadesconx);
            if (!dr.IsDBNull(iPrundsegundadesconx)) entity.Prundsegundadesconx = dr.GetString(iPrundsegundadesconx);

            int iPrundfallaotranosincronz = dr.GetOrdinal(this.Prundfallaotranosincronz);
            if (!dr.IsDBNull(iPrundfallaotranosincronz)) entity.Prundfallaotranosincronz = dr.GetString(iPrundfallaotranosincronz);

            int iPrundfallaotraunidsincronz = dr.GetOrdinal(this.Prundfallaotraunidsincronz);
            if (!dr.IsDBNull(iPrundfallaotraunidsincronz)) entity.Prundfallaotraunidsincronz = dr.GetString(iPrundfallaotraunidsincronz);

            int iPrundfallaequiposinreingreso = dr.GetOrdinal(this.Prundfallaequiposinreingreso);
            if (!dr.IsDBNull(iPrundfallaequiposinreingreso)) entity.Prundfallaequiposinreingreso = dr.GetString(iPrundfallaequiposinreingreso);

            int iPrundcalchayregmedid = dr.GetOrdinal(this.Prundcalchayregmedid);
            if (!dr.IsDBNull(iPrundcalchayregmedid)) entity.Prundcalchayregmedid = dr.GetString(iPrundcalchayregmedid);

            int iPrundcalchorafineval = dr.GetOrdinal(this.Prundcalchorafineval);
            if (!dr.IsDBNull(iPrundcalchorafineval)) entity.Prundcalchorafineval = dr.GetDateTime(iPrundcalchorafineval);

            int iPrundcalhayindisp = dr.GetOrdinal(this.Prundcalhayindisp);
            if (!dr.IsDBNull(iPrundcalhayindisp)) entity.Prundcalhayindisp = dr.GetString(iPrundcalhayindisp);

            int iPrundcalcpruebaexitosa = dr.GetOrdinal(this.Prundcalcpruebaexitosa);
            if (!dr.IsDBNull(iPrundcalcpruebaexitosa)) entity.Prundcalcpruebaexitosa = dr.GetString(iPrundcalcpruebaexitosa);

            int iPrundcalcperiodoprogprueba = dr.GetOrdinal(this.Prundcalcperiodoprogprueba);
            if (!dr.IsDBNull(iPrundcalcperiodoprogprueba)) entity.Prundcalcperiodoprogprueba = dr.GetDecimal(iPrundcalcperiodoprogprueba);

            int iPrundcalccondhoratarr = dr.GetOrdinal(this.Prundcalccondhoratarr);
            if (!dr.IsDBNull(iPrundcalccondhoratarr)) entity.Prundcalccondhoratarr = dr.GetString(iPrundcalccondhoratarr);

            int iPrundcalccondhoraprogtarr = dr.GetOrdinal(this.Prundcalccondhoraprogtarr);
            if (!dr.IsDBNull(iPrundcalccondhoraprogtarr)) entity.Prundcalccondhoraprogtarr = dr.GetString(iPrundcalccondhoraprogtarr);

            int iPrundcalcindispprimtramo = dr.GetOrdinal(this.Prundcalcindispprimtramo);
            if (!dr.IsDBNull(iPrundcalcindispprimtramo)) entity.Prundcalcindispprimtramo = dr.GetString(iPrundcalcindispprimtramo);

            int iPrundcalcindispsegtramo = dr.GetOrdinal(this.Prundcalcindispsegtramo);
            if (!dr.IsDBNull(iPrundcalcindispsegtramo)) entity.Prundcalcindispsegtramo = dr.GetString(iPrundcalcindispsegtramo);

            int iPrundrpf = dr.GetOrdinal(this.Prundrpf);
            if (!dr.IsDBNull(iPrundrpf)) entity.Prundrpf = dr.GetDecimal(iPrundrpf);

            int iPrundtiempoprueba = dr.GetOrdinal(this.Prundtiempoprueba);
            if (!dr.IsDBNull(iPrundtiempoprueba)) entity.Prundtiempoprueba = dr.GetDecimal(iPrundtiempoprueba);

            int iPrundusucreacion = dr.GetOrdinal(this.Prundusucreacion);
            if (!dr.IsDBNull(iPrundusucreacion)) entity.Prundusucreacion = dr.GetString(iPrundusucreacion);

            int iPrundfeccreacion = dr.GetOrdinal(this.Prundfeccreacion);
            if (!dr.IsDBNull(iPrundfeccreacion)) entity.Prundfeccreacion = dr.GetDateTime(iPrundfeccreacion);

            int iPrundusumodificacion = dr.GetOrdinal(this.Prundusumodificacion);
            if (!dr.IsDBNull(iPrundusumodificacion)) entity.Prundusumodificacion = dr.GetString(iPrundusumodificacion);

            int iPrundfecmodificacion = dr.GetOrdinal(this.Prundfecmodificacion);
            if (!dr.IsDBNull(iPrundfecmodificacion)) entity.Prundfecmodificacion = dr.GetDateTime(iPrundfecmodificacion);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPrundeliminado = dr.GetOrdinal(this.Prundeliminado);
            if (!dr.IsDBNull(iPrundeliminado)) entity.Prundeliminado = dr.GetString(iPrundeliminado);

            int iPrundpotefectiva = dr.GetOrdinal(this.Prundpotefectiva);
            if (!dr.IsDBNull(iPrundpotefectiva)) entity.Prundpotefectiva = Convert.ToDecimal(dr.GetValue(iPrundpotefectiva));

            int iPrundtiempoentarranq = dr.GetOrdinal(this.Prundtiempoentarranq);
            if (!dr.IsDBNull(iPrundtiempoentarranq)) entity.Prundtiempoentarranq = Convert.ToDecimal(dr.GetValue(iPrundtiempoentarranq));

            int iPrundtiempoarranqasinc = dr.GetOrdinal(this.Prundtiempoarranqasinc);
            if (!dr.IsDBNull(iPrundtiempoarranqasinc)) entity.Prundtiempoarranqasinc = Convert.ToDecimal(dr.GetValue(iPrundtiempoarranqasinc));

            int iPrundtiemposincapotefect = dr.GetOrdinal(this.Prundtiemposincapotefect);
            if (!dr.IsDBNull(iPrundtiemposincapotefect)) entity.Prundtiemposincapotefect = Convert.ToDecimal(dr.GetValue(iPrundtiemposincapotefect));

            return entity;
        }


        #region Mapeo de Campos

        public string Prundcodi = "PRUNDCODI";
        public string Prundfecha = "PRUNDFECHA";
        public string Prundescenario = "PRUNDESCENARIO";
        public string Prundhoraordenarranque = "PRUNDHORAORDENARRANQUE";
        public string Prundhorasincronizacion = "PRUNDHORASINCRONIZACION";
        public string Prundhorainiplenacarga = "PRUNDHORAINIPLENACARGA";
        public string Prundhorafalla = "PRUNDHORAFALLA";
        public string Prundhoraordenarranque2 = "PRUNDHORAORDENARRANQUE2";
        public string Prundhorasincronizacion2 = "PRUNDHORASINCRONIZACION2";
        public string Prundhorainiplenacarga2 = "PRUNDHORAINIPLENACARGA2";
        public string Prundsegundadesconx = "PRUNDSEGUNDADESCONX";
        public string Prundfallaotranosincronz = "PRUNDFALLAOTRANOSINCRONZ";
        public string Prundfallaotraunidsincronz = "PRUNDFALLAOTRAUNIDSINCRONZ";
        public string Prundfallaequiposinreingreso = "PRUNDFALLAEQUIPOSINREINGRESO";
        public string Prundcalchayregmedid = "PRUNDCALCHAYREGMEDID";
        public string Prundcalchorafineval = "PRUNDCALCHORAFINEVAL";
        public string Prundcalhayindisp = "PRUNDCALHAYINDISP";
        public string Prundcalcpruebaexitosa = "PRUNDCALCPRUEBAEXITOSA";
        public string Prundcalcperiodoprogprueba = "PRUNDCALCPERIODOPROGPRUEBA";
        public string Prundcalccondhoratarr = "PRUNDCALCCONDHORATARR";
        public string Prundcalccondhoraprogtarr = "PRUNDCALCCONDHORAPROGTARR";
        public string Prundcalcindispprimtramo = "PRUNDCALCINDISPPRIMTRAMO";
        public string Prundcalcindispsegtramo = "PRUNDCALCINDISPSEGTRAMO";
        public string Prundrpf = "PRUNDRPF";
        public string Prundtiempoprueba = "PRUNDTIEMPOPRUEBA";
        public string Prundusucreacion = "PRUNDUSUCREACION";
        public string Prundfeccreacion = "PRUNDFECCREACION";
        public string Prundusumodificacion = "PRUNDUSUMODIFICACION";
        public string Prundfecmodificacion = "PRUNDFECMODIFICACION";
        public string Grupocodi = "GRUPOCODI";
        public string Prundeliminado = "PRUNDELIMINADO";
        public string PrundUnidad = "UNIDAD";
        public string Prundpotefectiva = "PRUNDPOTEFECTIVA";
        public string Prundtiempoentarranq = "PRUNDTIEMPOENTARRANQ";
        public string Prundtiempoarranqasinc = "PRUNDTIEMPOARRANQASINC";
        public string Prundtiemposincapotefect = "PRUNDTIEMPOSINCAPOTEFECT";

        #region INDISPONIBILIDADES
        public string Emprnomb = "EMPRNOMB";
        public string Gruponomb = "GRUPONOMB";
        public string Grupoabrev = "GRUPOABREV";
        public string Emprcodi = "EMPRCODI";
        #endregion

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlUnidadSorteada
        {
            get { return base.GetSqlXml("GetByFecha"); }
        }

        public string SqlUnidadSorteadaHabilitada
        {
            get { return base.GetSqlXml("GetByFechaUnidadHabilitada"); }
        }


        #endregion
    }
}
