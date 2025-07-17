using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_INTERRUPCION
    /// </summary>
    public class EveInterrupcionHelper : HelperBase
    {
        public EveInterrupcionHelper(): base(Consultas.EveInterrupcionSql)
        {
        }

        public EveInterrupcionDTO Create(IDataReader dr)
        {
            EveInterrupcionDTO entity = new EveInterrupcionDTO();

            int iInterrmwDe = dr.GetOrdinal(this.InterrmwDe);
            if (!dr.IsDBNull(iInterrmwDe)) entity.InterrmwDe = dr.GetDecimal(iInterrmwDe);

            int iInterrmwA = dr.GetOrdinal(this.InterrmwA);
            if (!dr.IsDBNull(iInterrmwA)) entity.InterrmwA = dr.GetDecimal(iInterrmwA);

            int iInterrminu = dr.GetOrdinal(this.Interrminu);
            if (!dr.IsDBNull(iInterrminu)) entity.Interrminu = dr.GetDecimal(iInterrminu);

            int iInterrmw = dr.GetOrdinal(this.Interrmw);
            if (!dr.IsDBNull(iInterrmw)) entity.Interrmw = dr.GetDecimal(iInterrmw);

            int iInterrdesc = dr.GetOrdinal(this.Interrdesc);
            if (!dr.IsDBNull(iInterrdesc)) entity.Interrdesc = dr.GetString(iInterrdesc);

            int iInterrupcodi = dr.GetOrdinal(this.Interrupcodi);
            if (!dr.IsDBNull(iInterrupcodi)) entity.Interrupcodi = Convert.ToInt32(dr.GetValue(iInterrupcodi));

            int iPtointerrcodi = dr.GetOrdinal(this.Ptointerrcodi);
            if (!dr.IsDBNull(iPtointerrcodi)) entity.Ptointerrcodi = Convert.ToInt32(dr.GetValue(iPtointerrcodi));

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iInterrnivel = dr.GetOrdinal(this.Interrnivel);
            if (!dr.IsDBNull(iInterrnivel)) entity.Interrnivel = dr.GetString(iInterrnivel);

            int iInterrracmf = dr.GetOrdinal(this.Interrracmf);
            if (!dr.IsDBNull(iInterrracmf)) entity.Interrracmf = dr.GetString(iInterrracmf);

            int iInterrmfetapa = dr.GetOrdinal(this.Interrmfetapa);
            if (!dr.IsDBNull(iInterrmfetapa)) entity.Interrmfetapa = Convert.ToInt32(dr.GetValue(iInterrmfetapa));

            int iInterrmanualr = dr.GetOrdinal(this.Interrmanualr);
            if (!dr.IsDBNull(iInterrmanualr)) entity.Interrmanualr = dr.GetString(iInterrmanualr);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string InterrmwDe = "INTERRMW_DE";
        public string InterrmwA = "INTERRMW_A";
        public string Interrminu = "INTERRMINU";
        public string Interrmw = "INTERRMW";
        public string Interrdesc = "INTERRDESC";
        public string Interrupcodi = "INTERRUPCODI";
        public string Ptointerrcodi = "PTOINTERRCODI";
        public string Evencodi = "EVENCODI";
        public string Interrnivel = "INTERRNIVEL";
        public string Interrracmf = "INTERRRACMF";
        public string Interrmfetapa = "INTERRMFETAPA";
        public string Interrmanualr = "INTERRMANUALR";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string PtoInterrupNomb = "PTOINTERRUPNOMB";
        public string PtoEntreNomb = "PTOENTRENOMB";
        public string AreaNomb = "AREANOMB";
        public string EquiAbrev = "EQUIABREV";
        public string EquiTension = "EQUITENSION";
        public string EmprNomb = "EMPRNOMB";

        #region SIOSEIN
        public string Bajomw = "BAJOMW";
        public string Energia = "ENERGIA";
        public string Equipo = "EQUIPO";
        public string Famnomb = "FAMNOMB";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Famcodi = "FAMCODI";
        public string Famnom = "FAMNOMB";
        public string Interrupciontotal = "INTERRUPCIONTOTAL";
        public string Emprcodi = "EMPRCODI";
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string Evenini = "Evenini";
        #endregion

        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlListaCalidadSuministro
        {
            get { return base.GetSqlXml("ListaCalidadSuministro"); }
        }
        #endregion
    }
}

