using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_AJUSTEEMPRESA
    /// </summary>
    public class CaiAjusteempresaHelper : HelperBase
    {
        public CaiAjusteempresaHelper(): base(Consultas.CaiAjusteempresaSql)
        {
        }

        public CaiAjusteempresaDTO Create(IDataReader dr)
        {
            CaiAjusteempresaDTO entity = new CaiAjusteempresaDTO();

            int iCaiajecodi = dr.GetOrdinal(this.Caiajecodi);
            if (!dr.IsDBNull(iCaiajecodi)) entity.Caiajecodi = Convert.ToInt32(dr.GetValue(iCaiajecodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iCaiajetipoinfo = dr.GetOrdinal(this.Caiajetipoinfo);
            if (!dr.IsDBNull(iCaiajetipoinfo)) entity.Caiajetipoinfo = dr.GetString(iCaiajetipoinfo);

            int iCaiajereteneejeini = dr.GetOrdinal(this.Caiajereteneejeini);
            if (!dr.IsDBNull(iCaiajereteneejeini)) entity.Caiajereteneejeini = dr.GetDateTime(iCaiajereteneejeini);

            int iCaiajereteneejefin = dr.GetOrdinal(this.Caiajereteneejefin);
            if (!dr.IsDBNull(iCaiajereteneejefin)) entity.Caiajereteneejefin = dr.GetDateTime(iCaiajereteneejefin);

            int iCaiajeretenepryaini = dr.GetOrdinal(this.Caiajeretenepryaini);
            if (!dr.IsDBNull(iCaiajeretenepryaini)) entity.Caiajeretenepryaini = dr.GetDateTime(iCaiajeretenepryaini);

            int iCaiajeretenepryafin = dr.GetOrdinal(this.Caiajeretenepryafin);
            if (!dr.IsDBNull(iCaiajeretenepryafin)) entity.Caiajeretenepryafin = dr.GetDateTime(iCaiajeretenepryafin);

            int iCaiajereteneprybini = dr.GetOrdinal(this.Caiajereteneprybini);
            if (!dr.IsDBNull(iCaiajereteneprybini)) entity.Caiajereteneprybini = dr.GetDateTime(iCaiajereteneprybini);

            int iCaiajereteneprybfin = dr.GetOrdinal(this.Caiajereteneprybfin);
            if (!dr.IsDBNull(iCaiajereteneprybfin)) entity.Caiajereteneprybfin = dr.GetDateTime(iCaiajereteneprybfin);

            int iCaiajeusucreacion = dr.GetOrdinal(this.Caiajeusucreacion);
            if (!dr.IsDBNull(iCaiajeusucreacion)) entity.Caiajeusucreacion = dr.GetString(iCaiajeusucreacion);

            int iCaiajefeccreacion = dr.GetOrdinal(this.Caiajefeccreacion);
            if (!dr.IsDBNull(iCaiajefeccreacion)) entity.Caiajefeccreacion = dr.GetDateTime(iCaiajefeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Caiajecodi = "CAIAJECODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Emprcodi = "EMPRCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Caiajetipoinfo = "CAIAJETIPOINFO";
        public string Caiajereteneejeini = "CAIAJERETENEEJEINI";
        public string Caiajereteneejefin = "CAIAJERETENEEJEFIN";
        public string Caiajeretenepryaini = "CAIAJERETENEPRYAINI";
        public string Caiajeretenepryafin = "CAIAJERETENEPRYAFIN";
        public string Caiajereteneprybini = "CAIAJERETENEPRYBINI";
        public string Caiajereteneprybfin = "CAIAJERETENEPRYBFIN";
        public string Caiajeusucreacion = "CAIAJEUSUCREACION";
        public string Caiajefeccreacion = "CAIAJEFECCREACION";

        //Atributos de consulta
        public string Emprnomb = "EMPRNOMB";
        public string Ptomedielenomb = "PTOMEDIELENOMB";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string FechaPeriodo = "FECHAPERIODO";
        public string Periodo = "PERIODO";

        #endregion

        public string SqlListAjuste
        {
            get { return GetSqlXml("ListAjuste"); }
        }

        public string SqlListAjusteEmpresa
        {
            get { return GetSqlXml("ListAjusteEmpresa"); }
        }

        public string SqlListEmpresasByAjuste
        {
            get { return GetSqlXml("ListEmpresasByAjuste"); }
        }

        public string SqlListCaiAjusteempresasTipoEmpresa
        {
            get { return GetSqlXml("ListCaiAjusteempresasTipoEmpresa"); }
        }

        public string SqlListEmpresasXPtoGeneracion
        {
            get { return GetSqlXml("ListEmpresasXPtoGeneracion"); }
        }

        public string SqlListEmpresasXPtoUL
        {
            get { return GetSqlXml("ListEmpresasXPtoUL"); }
        }

        public string SqlListEmpresaByAjusteTipoEmpresa
        {
            get { return GetSqlXml("ListEmpresaByAjusteTipoEmpresa"); }
        }

        public string SqlObtenerListaPeriodoEjecutado
        {
            get { return GetSqlXml("ObtenerListaPeriodoEjecutado"); }
        }

        public string SqlObtenerListaPeriodoProyectado
        {
            get { return GetSqlXml("ObtenerListaPeriodoProyectado"); }
        }

        public string SqlGetMePtomedicionByNombre
        {
            get { return base.GetSqlXml("GetMePtomedicionByNombre"); }
        }

        public string SqlListEmpresasXPtoTrans
        {
            get { return GetSqlXml("ListEmpresasXPtoTrans"); }
        }

        public string SqlListEmpresasXPtoDist
        {
            get { return GetSqlXml("ListEmpresasXPtoDist"); }
        }

        public string SqlGetByCriteriaMeHojaptomeds
        {
            get { return GetSqlXml("GetByCriteriaMeHojaptomeds"); }
        }

        public string ListPtomed
        {
            get { return GetSqlXml("ListPtomed"); }
        }
    }
}
