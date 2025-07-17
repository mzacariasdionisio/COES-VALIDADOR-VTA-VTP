using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_ROL_TURNO
    /// </summary>
    public class SiRolTurnoHelper : HelperBase
    {
        public SiRolTurnoHelper(): base(Consultas.SiRolTurnoSql)
        {
        }

        public SiRolTurnoDTO Create(IDataReader dr)
        {
            SiRolTurnoDTO entity = new SiRolTurnoDTO();

            int iRoltfecha = dr.GetOrdinal(this.Roltfecha);
            if (!dr.IsDBNull(iRoltfecha)) entity.Roltfecha = dr.GetDateTime(iRoltfecha);

            int iActcodi = dr.GetOrdinal(this.Actcodi);
            if (!dr.IsDBNull(iActcodi)) entity.Actcodi = Convert.ToInt32(dr.GetValue(iActcodi));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iPercodi = dr.GetOrdinal(this.Percodi);
            if (!dr.IsDBNull(iPercodi)) entity.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

            int iRolestado = dr.GetOrdinal(this.Roltestado);
            if (!dr.IsDBNull(iRolestado)) entity.Roltestado = dr.GetString(iRolestado);

            int iRoltfechaactualizacion = dr.GetOrdinal(this.Roltfechaactualizacion);
            if (!dr.IsDBNull(iRoltfechaactualizacion)) entity.Roltfechaactualizacion = dr.GetDateTime(iRoltfechaactualizacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Roltfecha = "ROLTFECHA";
        public string Actcodi = "ACTCODI";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Percodi = "PERCODI";
        public string Actabrev = "Actabrev";
        public string TableName = "SI_ROL_TURNO";
        public string Pernomb = "Pernomb";
        public string Actnomb = "Actnomb";
        public string Roltestado = "ROLTESTADO";
        public string Roltfechaactualizacion = "Roltfechaactualizacion";

        #endregion

        public string SqlListaRols
        {
            get { return base.GetSqlXml("ListaRols"); }
        }

        public string SqlDeleteSiRolTurnoMasivo
        {
            get { return base.GetSqlXml("DeleteSiRolTurnoMasivo"); }
        }

        public string SqlListaMovimientos
        {
            get { return base.GetSqlXml("ListaMovimientos"); }
        }
    }
}
