using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_CORREO
    /// </summary>
    public class SiCorreoHelper : HelperBase
    {
        public SiCorreoHelper(): base(Consultas.SiCorreoSql)
        {
        }

        public SiCorreoDTO Create(IDataReader dr)
        {
            SiCorreoDTO entity = new SiCorreoDTO();

            int iCorrcodi = dr.GetOrdinal(this.Corrcodi);
            if (!dr.IsDBNull(iCorrcodi)) entity.Corrcodi = Convert.ToInt32(dr.GetValue(iCorrcodi));

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iPlantcodi = dr.GetOrdinal(this.Plantcodi);
            if (!dr.IsDBNull(iPlantcodi)) entity.Plantcodi = Convert.ToInt32(dr.GetValue(iPlantcodi));

            int iCorrto = dr.GetOrdinal(this.Corrto);
            if (!dr.IsDBNull(iCorrto)) entity.Corrto = dr.GetString(iCorrto);

            int iCorrfrom = dr.GetOrdinal(this.Corrfrom);
            if (!dr.IsDBNull(iCorrfrom)) entity.Corrfrom = dr.GetString(iCorrfrom);

            int iCorrcc = dr.GetOrdinal(this.Corrcc);
            if (!dr.IsDBNull(iCorrcc)) entity.Corrcc = dr.GetString(iCorrcc);

            int iCorrfechaenvio = dr.GetOrdinal(this.Corrfechaenvio);
            if (!dr.IsDBNull(iCorrfechaenvio)) entity.Corrfechaenvio = dr.GetDateTime(iCorrfechaenvio);

            int iCorrbcc = dr.GetOrdinal(this.Corrbcc);
            if (!dr.IsDBNull(iCorrbcc)) entity.Corrbcc = dr.GetString(iCorrbcc);

            int iCorrasunto = dr.GetOrdinal(this.Corrasunto);
            if (!dr.IsDBNull(iCorrasunto)) entity.Corrasunto = dr.GetString(iCorrasunto);

            int iCorrcontenido = dr.GetOrdinal(this.Corrcontenido);
            if (!dr.IsDBNull(iCorrcontenido)) entity.Corrcontenido = dr.GetString(iCorrcontenido);

            int iCorrfechaperiodo = dr.GetOrdinal(this.Corrfechaperiodo);
            if (!dr.IsDBNull(iCorrfechaperiodo)) entity.Corrfechaperiodo = dr.GetDateTime(iCorrfechaperiodo);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCorrusuenvio = dr.GetOrdinal(this.Corrusuenvio);
            if (!dr.IsDBNull(iCorrusuenvio)) entity.Corrusuenvio = dr.GetString(iCorrusuenvio);

            return entity;
        }

        #region Mapeo de Campos

        public string Corrcodi = "CORRCODI";
        public string Enviocodi = "ENVIOCODI";
        public string Plantcodi = "PLANTCODI";
        public string Corrto = "CORRTO";
        public string Corrfrom = "CORRFROM";
        public string Corrcc = "CORRCC";
        public string Corrfechaenvio = "CORRFECHAENVIO";
        public string Corrbcc = "CORRBCC";
        public string Corrasunto = "CORRASUNTO";
        public string Corrcontenido = "CORRCONTENIDO";
        public string Corrfechaperiodo = "CORRFECHAPERIODO";
        public string Corrusuenvio = "CORRUSUENVIO";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";

        public string SqlListarLogCorreo
        {
            get { return base.GetSqlXml("ListarLogCorreo"); }
        }

        #endregion
    }
}
