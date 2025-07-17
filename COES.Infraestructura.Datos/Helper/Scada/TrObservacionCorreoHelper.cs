using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_OBSERVACION_CORREO
    /// </summary>
    public class TrObservacionCorreoHelper : HelperBase
    {
        public TrObservacionCorreoHelper(): base(Consultas.TrObservacionCorreoSql)
        {
        }

        public TrObservacionCorreoDTO Create(IDataReader dr)
        {
            TrObservacionCorreoDTO entity = new TrObservacionCorreoDTO();

            int iObscorcodi = dr.GetOrdinal(this.Obscorcodi);
            if (!dr.IsDBNull(iObscorcodi)) entity.Obscorcodi = Convert.ToInt32(dr.GetValue(iObscorcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iObscoremail = dr.GetOrdinal(this.Obscoremail);
            if (!dr.IsDBNull(iObscoremail)) entity.Obscoremail = dr.GetString(iObscoremail);

            int iObscorestado = dr.GetOrdinal(this.Obscorestado);
            if (!dr.IsDBNull(iObscorestado)) entity.Obscorestado = dr.GetString(iObscorestado);

            int iObscornombre = dr.GetOrdinal(this.Obscornombre);
            if (!dr.IsDBNull(iObscornombre)) entity.Obscornombre = dr.GetString(iObscornombre);

            int iObscorusumodificacion = dr.GetOrdinal(this.Obscorusumodificacion);
            if (!dr.IsDBNull(iObscorusumodificacion)) entity.Obscorusumodificacion = dr.GetString(iObscorusumodificacion);

            int iObscorfecmodificacion = dr.GetOrdinal(this.Obscorfecmodificacion);
            if (!dr.IsDBNull(iObscorfecmodificacion)) entity.Obscorfecmodificacion = dr.GetDateTime(iObscorfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Obscorcodi = "OBSCORCODI";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRENOMB";
        public string Obscoremail = "OBSCOREMAIL";
        public string Obscorestado = "OBSCORESTADO";
        public string Obscornombre = "OBSCORNOMBRE";
        public string Obscorusumodificacion = "OBSCORUSUMODIFICACION";
        public string Obscorfecmodificacion = "OBSCORFECMODIFICACION";

        #endregion
    }
}
