using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_LOGSORTEO
    /// </summary>
    public class PrLogsorteoHelper : HelperBase
    {
        public PrLogsorteoHelper(): base(Consultas.PrLogsorteoSql)
        {
        }

        public PrLogsorteoDTO Create(IDataReader dr)
        {
            PrLogsorteoDTO entity = new PrLogsorteoDTO();

            int iLogusuario = dr.GetOrdinal(this.Logusuario);
            if (!dr.IsDBNull(iLogusuario)) entity.Logusuario = dr.GetString(iLogusuario);

            int iLogfecha = dr.GetOrdinal(this.Logfecha);
            if (!dr.IsDBNull(iLogfecha)) entity.Logfecha = dr.GetDateTime(iLogfecha);

            int iLogdescrip = dr.GetOrdinal(this.Logdescrip);
            if (!dr.IsDBNull(iLogdescrip)) entity.Logdescrip = dr.GetString(iLogdescrip);

            int iLogtipo = dr.GetOrdinal(this.Logtipo);
            if (!dr.IsDBNull(iLogtipo)) entity.Logtipo = dr.GetString(iLogtipo);

            int iLogcoordinador = dr.GetOrdinal(this.Logcoordinador);
            if (!dr.IsDBNull(iLogcoordinador)) entity.Logcoordinador = dr.GetString(iLogcoordinador);

            int iLogdocoes = dr.GetOrdinal(this.Logdocoes);
            if (!dr.IsDBNull(iLogdocoes)) entity.Logdocoes = dr.GetString(iLogdocoes);

            return entity;
        }


        #region Mapeo de Campos

        public string Logusuario = "LOGUSUARIO";
        public string Logfecha = "LOGFECHA";
        public string Logdescrip = "LOGDESCRIP";
        public string Logtipo = "LOGTIPO";
        public string Logcoordinador = "LOGCOORDINADOR";
        public string Logdocoes = "LOGDOCOES";
        public string Emprnomb = "EMPRNOMB";
        public string Areanomb = "AREANOMB";
        public string Equiabrev = "EQUIABREV";
        public string Equicodi = "EQUICODI";
        public string Fechaini = "HOPHORINI";
        public string Fechafin = "HOPHORFIN";
        public string Subcausadesc = "SUBCAUSADESC";
        public string Evenclase = "EVENCLASE";
        public string Evendescrip = "EVENDESCRIP";
        public string Emprcodi = "EMPRCODI";

        public string SqlObtenerSituacionUnidades
        {
            get { return base.GetSqlXml("ObtenerSituacionUnidades"); }
        }

        public string SqlObtenerMantenimientos
        {
            get { return base.GetSqlXml("ObtenerMantenimientos"); }
        }

        public string TotalConteoTipo
        {
            get { return base.GetSqlXml("TotalConteoTipo"); }
        }

        public string EquipoPrueba
        {
            get { return base.GetSqlXml("EquipoPrueba"); }
        }

        public string TotalBalotaNegra
        {
            get { return base.GetSqlXml("TotalBalotaNegra"); }
        }

        public string EquicodiPrueba
        {
            get { return base.GetSqlXml("EquicodiPrueba"); }
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlListaLogSorteo
        {
            get { return base.GetSqlXml("ListaLogSorteo"); }
        }
        #endregion


        #region FIT SGOCOES func A 
        public PrLogsorteoDTO CreateEQ(IDataReader dr)
        {
            PrLogsorteoDTO entity = new PrLogsorteoDTO();

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iAreanomb = dr.GetOrdinal(this.Areanomb);
            if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            return entity;
        }

        public PrLogsorteoDTO CreateEve(IDataReader dr)
        {
            PrLogsorteoDTO entity = new PrLogsorteoDTO();

            int iAreanomb = dr.GetOrdinal(this.Areanomb);
            if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEvenini = dr.GetOrdinal(this.Evenini);
            if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

            int iEvenfin = dr.GetOrdinal(this.Evenfin);
            if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

            return entity;
        }

        public PrLogsorteoDTO CreateInd(IDataReader dr)
        {
            PrLogsorteoDTO entity = new PrLogsorteoDTO();

            int iAreanomb = dr.GetOrdinal(this.Areanomb);
            if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iIchorfin = dr.GetOrdinal(this.Ichorfin);
            if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

            int iIchorini = dr.GetOrdinal(this.Ichorini);
            if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

            return entity;
        }

        public PrLogsorteoDTO CreateOpe(IDataReader dr)
        {
            PrLogsorteoDTO entity = new PrLogsorteoDTO();

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iAreanomb = dr.GetOrdinal(this.Areanomb);
            if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            int iHopfalla = dr.GetOrdinal(this.Hopfalla);
            if (!dr.IsDBNull(iHopfalla)) entity.Hopfalla = dr.GetString(iHopfalla);

            int iHophorini = dr.GetOrdinal(this.Hophorini);
            if (!dr.IsDBNull(iHophorini)) entity.Hophorini = dr.GetDateTime(iHophorini);

            int iIchorini = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iIchorini)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iIchorini));

            return entity;
        }


        public string Equipadre = "EQUIPADRE";
        public string Evenini = "EVENINI";
        public string Evenfin = "EVENFIN";
        public string Ichorini = "ICHORINI";
        public string Ichorfin = "ICHORFIN";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Hopfalla = "HOPFALLA";
        public string Hophorini = "HOPHORINI";
        public string fecha = "FECHA";
        public string prueba = "PRUEBA";
        public string GRUPOCODI = "GRUPOCODI";
        public string PRUNDFECHA = "PRUNDFECHA";
        public string equicodi1 = "EQUICOD1";
        public string equicodi2 = "EQUICODI2";
        public string i_codigo = "I_CODIGO";        

        public PrLogsorteoDTO CreateUnd(IDataReader dr)
        {
            PrLogsorteoDTO entity = new PrLogsorteoDTO();

            int iPRUNDFECHA = dr.GetOrdinal(this.PRUNDFECHA);
            if (!dr.IsDBNull(iPRUNDFECHA)) entity.PRUNDFECHA = dr.GetDateTime(iPRUNDFECHA);

            int iGRUPOCODI = dr.GetOrdinal(this.GRUPOCODI);
            if (!dr.IsDBNull(iGRUPOCODI)) entity.GRUPOCODI = Convert.ToInt32(dr.GetValue(iGRUPOCODI));

            return entity;
        }

        public Prequipos_validosDTO CreateEqv(IDataReader dr)
        {
            Prequipos_validosDTO entity = new Prequipos_validosDTO();

            int iequicodi1 = dr.GetOrdinal(this.equicodi1);
            if (!dr.IsDBNull(iequicodi1)) entity.equicodi1 = dr.GetString(iequicodi1);

            int iequicodi2 = dr.GetOrdinal(this.equicodi2);
            if (!dr.IsDBNull(iequicodi2)) entity.equicodi2 = dr.GetString(iequicodi2);

            int ii_codigo = dr.GetOrdinal(this.i_codigo);
            if (!dr.IsDBNull(ii_codigo)) entity.i_codigo = Convert.ToInt32(dr.GetValue(ii_codigo));

            return entity;
        }

        public string Sqleq_equipo
        {
            get { return base.GetSqlXml("eq_equipo"); }
        }

        public string Sqleq_central
        {
            get { return base.GetSqlXml("eq_central"); }
        }

        public string Sqleve_mantto
        {
            get { return base.GetSqlXml("eve_mantto"); }
        }

        public string Sqleve_indisponibilidad
        {
            get { return base.GetSqlXml("eve_indisponibilidad"); }
        }

        public string Sqleve_horaoperacion
        {
            get { return base.GetSqlXml("eve_horaoperacion"); }
        }
        public string Sqleve_pruebaunidad
        {
            get { return base.GetSqlXml("eve_pruebaunidad"); }
        }
        public string Sqlequipos_validos
        {
            get { return base.GetSqlXml("equipos_validos"); }
        }

        public string Sqleve_mantto_calderos
        {
            get { return base.GetSqlXml("eve_mantto_calderos"); }
        }

        public string SqlInsertPrSorteo
        {
            get { return base.GetSqlXml("InsertPrSorteo"); }
        }

        public string SqlDeleteEquipo
        {
            get { return base.GetSqlXml("DeleteEquipo"); }
        }
        public string SqlTotalConteoTipoXEQ
        {
            get { return base.GetSqlXml("TotalConteoTipoXEQ"); }
        }

        public string SqlDiasFaltantes
        {
            get { return base.GetSqlXml("DiasFaltantes"); }
        }

        public string SqlGetByCriteriaHistorico
        {
            get { return base.GetSqlXml("GetByCriteriaHistorico"); }
        }
        
        #endregion

    }
}
