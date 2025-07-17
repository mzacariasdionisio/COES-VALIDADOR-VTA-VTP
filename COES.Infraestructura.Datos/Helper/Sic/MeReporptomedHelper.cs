using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_REPORPTOMED
    /// </summary>
    public class MeReporptomedHelper : HelperBase
    {
        public MeReporptomedHelper()
            : base(Consultas.MeReporptomedSql)
        {
        }

        public MeReporptomedDTO Create(IDataReader dr)
        {
            MeReporptomedDTO entity = new MeReporptomedDTO();

            int iRepptocodi = dr.GetOrdinal(this.Repptocodi);
            if (!dr.IsDBNull(iRepptocodi)) entity.Repptocodi = Convert.ToInt32(dr.GetValue(iRepptocodi));

            int iReporcodi = dr.GetOrdinal(this.Reporcodi);
            if (!dr.IsDBNull(iReporcodi)) entity.Reporcodi = Convert.ToInt32(dr.GetValue(iReporcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iRepptoorden = dr.GetOrdinal(this.Repptoorden);
            if (!dr.IsDBNull(iRepptoorden)) entity.Repptoorden = Convert.ToInt32(dr.GetValue(iRepptoorden));

            int iRepptoestado = dr.GetOrdinal(this.Repptoestado);
            if (!dr.IsDBNull(iRepptoestado)) entity.Repptoestado = Convert.ToInt32(dr.GetValue(iRepptoestado));

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iTipoptomedicodi = dr.GetOrdinal(this.Tipoptomedicodi);
            if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));

            int iRepptotabmed = dr.GetOrdinal(this.Repptotabmed);
            if (!dr.IsDBNull(iRepptotabmed)) entity.Repptotabmed = Convert.ToInt32(dr.GetValue(iRepptotabmed));

            int iRepptonomb = dr.GetOrdinal(this.Repptonomb);
            if (!dr.IsDBNull(iRepptonomb)) entity.Repptonomb = dr.GetString(iRepptonomb);

            int iFunptocodi = dr.GetOrdinal(this.Funptocodi);
            if (!dr.IsDBNull(iFunptocodi)) entity.Funptocodi = dr.GetInt32(iFunptocodi);

            int iRepptocolorcelda = dr.GetOrdinal(this.Repptocolorcelda);
            if (!dr.IsDBNull(iRepptocolorcelda)) entity.Repptocolorcelda = dr.GetString(iRepptocolorcelda);                       

            int iRepptoequivpto = dr.GetOrdinal(this.Repptoequivpto);
            if (!dr.IsDBNull(iRepptoequivpto)) entity.Repptoequivpto = dr.GetInt32(iRepptoequivpto);

            int iRepptoindcopiado = dr.GetOrdinal(this.Repptoindcopiado);
            if (!dr.IsDBNull(iRepptoindcopiado)) entity.Repptoindcopiado = dr.GetString(iRepptoindcopiado);

            return entity;
        }

        public string GetQueryMeReporptomed(string sql)
        {
            string query = sql.ToUpper();
            query = query.Replace("ME_HOJAPTOMED", "ME_REPORPTOMED ");
            int posWhere = query.LastIndexOf("WHERE");
            if (posWhere >= 0)
            {
                query = query.Substring(0, posWhere - 1);
            }
            query += " WHERE hp.reporcodi= {0}";

            return query;
        }

        #region Mapeo de Campos

        public string Repptocodi = "REPPTOCODI";
        public string Reporcodi = "REPORCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Repptoorden = "REPPTOORDEN";
        public string Repptoestado = "REPPTOESTADO";
        public string Tipoptomedicodi = "TPTOMEDICODI";
        public string Repptotabmed = "REPPTOTABMED";
        public string Repptonomb = "REPPTONOMB";
        public string Funptocodi = "FUNPTOCODI";
        public string Repptocolorcelda = "REPPTOCOLORCELDA";
        public string Repptoequivpto = "REPPTOEQUIVPTO";
        public string Repptoindcopiado = "REPPTOINDCOPIADO";

        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Emprabrev = "EMPRABREV";
        public string Equinomb = "Equinomb";
        public string Cuenca = "Cuenca";
        public string Tipoptomedinomb = "Tptomedinomb";
        public string Tipoinfoabrev = "Tipoinfoabrev";
        public string Ptomedibarranomb = "Ptomedibarranomb";
        public string Ptomedielenomb = "Ptomedielenomb";
        public string Ptomedidesc = "Ptomedidesc";
        public string Famcodi = "Famcodi";
        public string Famnomb = "Famnomb";
        public string Famabrev = "Famabrev";

        public string AreaOperativa = "AreaOperativa";
        public string Areanomb = "Areanomb";
        public string Valor1 = "Valor1";
        public string Suministrador = "Suministrador";
        public string CodigoOsinergmin = "CodigoOsinergmin";

        public string Unidad = "Unidad";
        public string PtoMediEleNomb = "PtoMediEleNomb";
        public string Equipopadre = "Equipopadre";
        public string Equipadre = "Equipadre";
        public string Equicodi = "Equicodi";
        public string Equiabrev = "Equiabrev";
        public string Repornombre = "repornombre";
        public string PtomediCalculado = "PTOMEDICALCULADO";
        public string PtomedicodiCalculado = "PTOMEDICODI_CALCULADO";
        public string PtomedicodidescCalculado = "PTOMEDICODIDESC_CALCULADO";
        public string EmprcodiOrigen = "EMPRCODI_ORIGEN";
        public string EmprabrevOrigen = "EMPRABREV_ORIGEN";
        public string TipoRelacioncodi = "TRPTOCODI";
        public string TipoRelacionnombre = "TRPTONOMBRE";
        public string PtomedicodiOrigen = "PTOMEDICODI_ORIGEN";
        public string PtomedicodidescOrigen = "PTOMEDICODIDESC_ORIGEN";
        public string FactorOrigen = "FACTOR_ORIGEN";
        public string EquicodiOrigen = "EQUICODI_ORIGEN";
        public string EquinombOrigen = "EQUINOMB_ORIGEN";
        public string PtomedibarranombOrigen = "Ptomedibarranomb_ORIGEN";
        public string PtomedielenombOrigen = "Ptomedielenomb_ORIGEN";
        public string Relptocodi = "RELPTOCODI";
        public string Areacodi = "AREACODI";
        public string Subestacioncodi = "SUBESTACIONCODI";
        public string Valor4 = "Valor4";
        public string Valor5 = "Valor5";
        public string Lectcodi = "Lectcodi";
        public string Lectabrev = "Lectabrev";
        public string Lectnomb = "Lectnomb";
        public string Tgenercodi = "Tgenercodi";
        public string EmprnombOrigen = "EMPRNOMB_ORIGEN";
        public string Central = "CENTRAL";
        public string Tptomedinomb = "Tptomedinomb";
        public string Origlectcodi = "ORIGLECTCODI";
        public string Origlectnombre = "Origlectnombre";

        #region SIOSEIN
        public string Funptofuncion = "FUNPTOFUNCION";
        public string Osicodi = "OSICODI";
        public string Codref = "CODREF";
        #endregion

        #endregion

        public string SqlListEncabezado
        {
            get { return base.GetSqlXml("ListEncabezado"); }
        }

        public string SqlGetById2
        {
            get { return base.GetSqlXml("GetById2"); }
        }

        public string SqlGetById3
        {
            get { return base.GetSqlXml("GetById3"); }
        }

        public string SqlGetMaxOrder
        {
            get { return base.GetSqlXml("GetMaxOrder"); }
        }

        public string SqlListarPuntoReporte
        {
            get { return base.GetSqlXml("ListarPuntoReporte"); }
        }

        public string SqlPaginacionReporte
        {
            get { return base.GetSqlXml("PaginacionReporte"); }
        }

        public string SqlListEncabezadoPowel
        {
            get { return base.GetSqlXml("ListEncabezadoPowel"); }
        }
    }
}
