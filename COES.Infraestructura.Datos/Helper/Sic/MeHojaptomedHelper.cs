using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_HOJAPTOMED
    /// </summary>
    public class MeHojaptomedHelper : HelperBase
    {
        public MeHojaptomedHelper(): base(Consultas.MeHojaptomedSql)
        {
        }


        public MeHojaptomedDTO Create(IDataReader dr, bool incluirObservacion = true)
        {
            MeHojaptomedDTO entity = new MeHojaptomedDTO();

            int iHojaptocodi = dr.GetOrdinal(this.Hojaptocodi);
            if (!dr.IsDBNull(iHojaptocodi)) entity.Hojaptomedcodi = Convert.ToInt32(dr.GetValue(iHojaptocodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iHojaptolimsup = dr.GetOrdinal(this.Hojaptolimsup);
            if (!dr.IsDBNull(iHojaptolimsup)) entity.Hojaptolimsup = dr.GetDecimal(iHojaptolimsup);

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iHojaptoliminf = dr.GetOrdinal(this.Hojaptoliminf);
            if (!dr.IsDBNull(iHojaptoliminf)) entity.Hojaptoliminf = dr.GetDecimal(iHojaptoliminf);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iHojaptoactivo = dr.GetOrdinal(this.Hojaptoactivo);
            if (!dr.IsDBNull(iHojaptoactivo)) entity.Hojaptoactivo = Convert.ToInt32(dr.GetValue(iHojaptoactivo));

            int iHojaptoorden = dr.GetOrdinal(this.Hojaptoorden);
            if (!dr.IsDBNull(iHojaptoorden)) entity.Hojaptoorden = Convert.ToInt32(dr.GetValue(iHojaptoorden));

            int iHojaptosigno = dr.GetOrdinal(this.Hojaptosigno);
            if (!dr.IsDBNull(iHojaptosigno)) entity.Hojaptosigno = Convert.ToInt32(dr.GetValue(iHojaptosigno));

            int iTipoptomedicodi = dr.GetOrdinal(this.Tptomedicodi);
            if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));
            entity.Tptomedicodi = entity.Tptomedicodi > 0 ? entity.Tptomedicodi : -1;

            int iHojacodi = dr.GetOrdinal(this.Hojacodi);
            if (!dr.IsDBNull(iHojacodi)) entity.Hojacodi = Convert.ToInt32(dr.GetValue(iHojacodi));

            if (incluirObservacion)
            {
                try
                {
                    int iHptoindcheck = dr.GetOrdinal(this.Hptoindcheck);
                    if (!dr.IsDBNull(iHptoindcheck)) entity.Hptoindcheck = dr.GetString(iHptoindcheck);

                    int iHptoobservacion = dr.GetOrdinal(this.Hptoobservacion);
                    if (!dr.IsDBNull(iHptoobservacion)) entity.Hptoobservacion = dr.GetString(iHptoobservacion);
                }
                catch { }
            }

            return entity;
        }


        #region Mapeo de Campos

        public string Ptomedicodi = "PTOMEDICODI";
        public string Hojaptolimsup = "HPTOLIMSUP";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Formatcodi = "FORMATCODI";
        public string Hojaptoliminf = "HPTOLIMINF";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Hojaptoactivo = "HPTOACTIVO";
        public string Hojaptoorden = "HPTOORDEN";
        public string Hojaptosigno = "HPTOSIGNO";
        public string Hojaptocodi = "HPTOCODI";
        public string Hptoobservacion = "HPTOOBSERVACION";
        public string Hptoindcheck = "HPTOINDCHECK";

        public string Tipoinfoabrev = "Tipoinfoabrev";
        public string Equinomb = "Equinomb";
        public string Emprabrev = "Emprabrev";
        public string Tipoptomedinomb = "Tptomedinomb";
        public string Tptomedicodi = "Tptomedicodi";
        public string Famcodi = "Famcodi";
        public string Ptomedibarranomb = "Ptomedibarranomb";
        public string Ptomedidesc = "Ptomedidesc";
        public string AreaOperativa = "AreaOperativa";
        public string Areanomb = "Areanomb";
        public string Valor1 = "Valor1";
        public string Suministrador = "Suministrador";
        public string CodigoOsinergmin = "CodigoOsinergmin";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Unidad = "Unidad";
        public string PtoMediEleNomb = "PtoMediEleNomb";
        public string Ptomediestado = "Ptomediestado";
        public string Equipopadre = "Equipopadre";
        public string Equipadre = "Equipadre";
        public string Equicodi = "Equicodi";
        public string Equiabrev = "Equiabrev";
        public string Famabrev = "Famabrev";
        public string Areacodi = "Areacodi";
        public string Grupocodi = "Grupocodi";
        public string Gruponomb = "Gruponomb";
        public string ObraFechaPlanificada = "ObraFechaPlanificada";
        public string Tptomedinomb = "Tptomedinomb";

        public string Hojacodi = "HOJACODI";
        public string Valor4 = "Valor4";
        public string Medidornomb = "MEDNOMBRE";
        public string Medidorserie = "MEDSERIE";
        public string Medidorclaseprecision = "MEDCLASEPRECISION";

        public string Tgenercodi = "TGENERCODI";
        public string Tgenernomb = "tgenernomb";
        #region Modificación PR5-DemandaDiaria 29-11-2017
        public string Valor5 = "Valor5";
        #endregion

        public string Hptominfila = "HPTOMINFILA";
        public string Hptodiafinplazo = "HPTODIAFINPLAZO";
        public string Hptominfinplazo = "HPTOMINFINPLAZO";

        public string Origlectnombre = "ORIGLECTNOMBRE";
        public string Origlectcodi = "ORIGLECTCODI";

        public string Formatnombre = "FORMATNOMBRE";

        #endregion

        #region FIT - VALORIZACION
        public string Clientenomb = "clientenomb";
        public string PuntoConexion = "PTOCONEXION";
        public string Barranomb = "barranomb";
        public string ClienteNomb = "CLIENTENOMB";
        public string BarrNomb = "BARRANOMB";
        #endregion

        #region Mejoras IEOD
        public string Equitension = "EQUITENSION";
        #endregion

        #region Mejoras RDO
        public string Cuenca = "CUENCA";
        #endregion

        #region Assetec - DemandaPO
        public string Areapadre = "AREAPADRE";
        #endregion

        public string SqlGetMaxOrder
        {
            get { return base.GetSqlXml("GetMaxOrder"); }
        }

        public string SqlObtenerEmpresaFormato
        {
            get { return base.GetSqlXml("ObtenerEmpresaFormato"); }
        }

        public string SqlObtenerPtosXFormato
        {
            get { return base.GetSqlXml("ObtenerPtosXFormato"); }
        }

        public string SqlGetPuntosFormato
        {
            get { return base.GetSqlXml("GetPuntosFormato"); }
        }

        public string SqlListPtosWithTipoGeneracion
        {
            get { return base.GetSqlXml("ListPtosWithTipoGeneracion"); }
        }

        public string SqlListarHojaPtoByFormatoAndEmpresa
        {
            get { return base.GetSqlXml("ListarHojaPtoByFormatoAndEmpresa"); }
        }

        public string SqlListarHojaPtoByFormatoAndEmpresaHoja
        {
            get { return base.GetSqlXml("ListarHojaPtoByFormatoAndEmpresaHoja"); }
        }
        public string SqlListarHojaPtoByFormatoAndEmpresaHojaPr16
        {
            get { return base.GetSqlXml("ListarHojaPtoByFormatoAndEmpresaHojaPR16"); }
        }
        
        public string SqlGetByIdHoja
        {
            get { return base.GetSqlXml("GetByIdHoja"); }
        }

        public string SqlDeleteHoja
        {
            get { return base.GetSqlXml("DeleteHoja"); }
        }

        public string SqlListByFormatcodi
        {
            get { return base.GetSqlXml("ListByFormatcodi"); }
        }

        public string SqlDeleteById
        {
            get { return base.GetSqlXml("DeleteById"); }
        }

    }
}
