using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class VceCompRegularDetHelper : HelperBase
    {
        public VceCompRegularDetHelper() : base(Consultas.VceCompRegularDetSql)
        {
        }
        public VceCompRegularDetDTO Create(IDataReader dr)
        {
            VceCompRegularDetDTO entity = new VceCompRegularDetDTO();

            int iCrdettipocalc = dr.GetOrdinal(this.Crdettipocalc);
            if (!dr.IsDBNull(iCrdettipocalc)) entity.Crdettipocalc = dr.GetString(iCrdettipocalc);

            int iCrdetcvtbajaefic = dr.GetOrdinal(this.Crdetcvtbajaefic);
            if (!dr.IsDBNull(iCrdetcvtbajaefic)) entity.Crdetcvtbajaefic = dr.GetDecimal(iCrdetcvtbajaefic);

            int iCrdetcompensacion = dr.GetOrdinal(this.Crdetcompensacion);
            if (!dr.IsDBNull(iCrdetcompensacion)) entity.Crdetcompensacion = dr.GetDecimal(iCrdetcompensacion);

            int iCrdetcmg = dr.GetOrdinal(this.Crdetcmg);
            if (!dr.IsDBNull(iCrdetcmg)) entity.Crdetcmg = dr.GetDecimal(iCrdetcmg);

            int iCrdetcvt = dr.GetOrdinal(this.Crdetcvt);
            if (!dr.IsDBNull(iCrdetcvt)) entity.Crdetcvt = dr.GetDecimal(iCrdetcvt);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iCrdetvalor = dr.GetOrdinal(this.Crdetvalor);
            if (!dr.IsDBNull(iCrdetvalor)) entity.Crdetvalor = dr.GetDecimal(iCrdetvalor);

            int iCrdethora = dr.GetOrdinal(this.Crdethora);
            if (!dr.IsDBNull(iCrdethora)) entity.Crdethora = dr.GetDateTime(iCrdethora);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = Convert.ToInt32(dr.GetValue(iPecacodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Crdettipocalc = "CRDETTIPOCALC";
        public string Crdetcvtbajaefic = "CRDETCVTBAJAEFIC";
        public string Crdetcompensacion = "CRDETCOMPENSACION";
        public string Crdetcmg = "CRDETCMG";
        public string Crdetcvt = "CRDETCVT";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Crdetvalor = "CRDETVALOR";
        public string Crdethora = "CRDETHORA";
        public string Grupocodi = "GRUPOCODI";
        public string Pecacodi = "PECACODI";

        //Adicionales
        public string Emprnomb = "EMPRNOMB";
        public string Gruponomb = "GRUPONOMB";

        #endregion

        public string SqlListCompensacionesEspeciales
        {
            get { return base.GetSqlXml("ListCompensacionesEspeciales"); }
        }

        public string SqlListFechaMod
        {
            get { return base.GetSqlXml("ListFechaMod"); }
        }

        public string SqlDeleteCompensacionManual
        {
            get { return base.GetSqlXml("DeleteCompensacionManual"); }
        }

        public string SqlDeleteByVersion
        {
            get { return base.GetSqlXml("DeleteByVersion"); }
        }

        public string SqlDeleteByVersionTipoCalculoAutomatico
        {
            get { return base.GetSqlXml("DeleteByVersionTipoCalculoAutomatico"); }
        }

        public string SqlDeleteByGroup
        {
            get { return base.GetSqlXml("DeleteByGroup"); }
        }

        public string SqlSaveManual
        {
            get { return base.GetSqlXml("SaveManual"); }
        }

        public string SqlSaveFromOtherVersion
        {
            get { return base.GetSqlXml("SaveFromOtherVersion"); }
        }

        public string SqlListCursorHoraOperacionComun
        {
            get { return base.GetSqlXml("ListCursorHoraOperacionComun"); }
        }

        public string SqlListCursorGrupoComun
        {
            get { return base.GetSqlXml("ListCursorGrupoComun"); }
        }

        public string SqlListCursorFechasComun
        {
            get { return base.GetSqlXml("ListCursorFechasComun"); }
        }

        public string SqlListCursorParticipacionComun
        {
            get { return base.GetSqlXml("ListCursorParticipacionComun"); }
        }

        public string SqlActualizarPotencia
        {
            get { return base.GetSqlXml("ActualizarPotencia"); }
        }

        public string SqlActualizarPotenciaMinima
        {
            get { return base.GetSqlXml("ActualizarPotenciaMinima"); }
        }
        public string SqlActualizarPotenciaMaxima
        {
            get { return base.GetSqlXml("ActualizarPotenciaMaxima"); }
        }
        public string SqlActualizarConsumoCombustible
        {
            get { return base.GetSqlXml("ActualizarConsumoCombustible"); }
        }
        public string SqlActualizarPotenciaEfectiva
        {
            get { return base.GetSqlXml("ActualizarPotenciaEfectiva"); }
        }
        public string SqlActualizarParametrosGenerador
        {
            get { return base.GetSqlXml("ActualizarParametrosGenerador"); }
        }
        public string SqlActualizarCVC
        {
            get { return base.GetSqlXml("ActualizarCVC"); }
        }
        public string SqlActualizarCVT
        {
            get { return base.GetSqlXml("ActualizarCVT"); }
        }
        public string SqlInicializarEnergiaSinCalificacion
        {
            get { return base.GetSqlXml("InicializarEnergiaSinCalificacion"); }
        }

        public string SqlListGrupoCodi
        {
            get { return base.GetSqlXml("ListGrupoCodi"); }
        }
    }
}
