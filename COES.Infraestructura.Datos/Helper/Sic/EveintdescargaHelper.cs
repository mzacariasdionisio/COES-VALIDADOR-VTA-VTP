using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class EveintdescargaHelper : HelperBase
    {
        public EveintdescargaHelper() : base(Consultas.EveIntDescargaSql)
        {
        }

        public EveintdescargaDTO Create(IDataReader dr)
        {
            EveintdescargaDTO entity = new EveintdescargaDTO();

            int iEveintdescodi = dr.GetOrdinal(this.Eveintdescodi);
            if (!dr.IsDBNull(iEveintdescodi)) entity.EVEINTDESCODI = dr.GetInt32(iEveintdescodi);

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.EVENCODI = dr.GetInt32(iEvencodi);

            int iEveintdestipo = dr.GetOrdinal(this.Eveintdestipo);
            if (!dr.IsDBNull(iEveintdestipo)) entity.EVEINTDESTIPO = dr.GetInt32(iEveintdestipo);

            int iEveintdescelda = dr.GetOrdinal(this.Eveintdescelda);
            if (!dr.IsDBNull(iEveintdescelda)) entity.EVEINTDESCELDA = dr.GetString(iEveintdescelda);

            int iEveintdescodigo = dr.GetOrdinal(this.Eveintdescodigo);
            if (!dr.IsDBNull(iEveintdescodigo)) entity.EVEINTDESCODIGO = dr.GetString(iEveintdescodigo);

            int iEveintdessubestacion = dr.GetOrdinal(this.Eveintdessubestacion);
            if (!dr.IsDBNull(iEveintdessubestacion)) entity.EVEINTDESSUBESTACION = dr.GetString(iEveintdessubestacion);

            int iEveintdesr_Antes = dr.GetOrdinal(this.Eveintdesr_Antes);
            if (!dr.IsDBNull(iEveintdesr_Antes)) entity.EVEINTDESR_ANTES = dr.GetInt32(iEveintdesr_Antes);

            int iEveintdess_Antes = dr.GetOrdinal(this.Eveintdess_Antes);
            if (!dr.IsDBNull(iEveintdess_Antes)) entity.EVEINTDESS_ANTES = dr.GetInt32(iEveintdess_Antes);

            int iEveintdest_Antes = dr.GetOrdinal(this.Eveintdest_Antes);
            if (!dr.IsDBNull(iEveintdest_Antes)) entity.EVEINTDEST_ANTES = dr.GetInt32(iEveintdest_Antes);

            int iEveintdesr_Despues = dr.GetOrdinal(this.Eveintdesr_Despues);
            if (!dr.IsDBNull(iEveintdesr_Despues)) entity.EVEINTDESR_DESPUES = dr.GetInt32(iEveintdesr_Despues);

            int iEveintdess_Despues = dr.GetOrdinal(this.Eveintdess_Despues);
            if (!dr.IsDBNull(iEveintdess_Despues)) entity.EVEINTDESS_DESPUES = dr.GetInt32(iEveintdess_Despues);

            int iEveintdest_Despues = dr.GetOrdinal(this.Eveintdest_Despues);
            if (!dr.IsDBNull(iEveintdest_Despues)) entity.EVEINTDEST_DESPUES = dr.GetInt32(iEveintdest_Despues);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.LASTDATE = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.LASTUSER = dr.GetString(iLastuser);

            return entity;
        }

        #region Mapeo de Campos

        public string Eveintdescodi = "EVEINTDESCODI";
        public string Evencodi = "EVENCODI";
        public string Eveintdestipo = "EVEINTDESTIPO";
        public string Eveintdescelda = "EVEINTDESCELDA";
        public string Eveintdescodigo = "EVEINTDESCODIGO";
        public string Eveintdessubestacion = "EVEINTDESSUBESTACION";
        public string Eveintdesr_Antes = "EVEINTDESR_ANTES";
        public string Eveintdess_Antes = "EVEINTDESS_ANTES";
        public string Eveintdest_Antes = "EVEINTDEST_ANTES";
        public string Eveintdesr_Despues = "EVEINTDESR_DESPUES";
        public string Eveintdess_Despues = "EVEINTDESS_DESPUES";
        public string Eveintdest_Despues = "EVEINTDEST_DESPUES";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";

        #endregion
    }
}
