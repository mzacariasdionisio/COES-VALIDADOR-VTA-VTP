using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_EVENTO_MEDICION
    /// </summary>
    public class ReEventoMedicionHelper : HelperBase
    {
        public ReEventoMedicionHelper(): base(Consultas.ReEventoMedicionSql)
        {
        }

        public ReEventoMedicionDTO Create(IDataReader dr)
        {
            ReEventoMedicionDTO entity = new ReEventoMedicionDTO();

            int iReemedcodi = dr.GetOrdinal(this.Reemedcodi);
            if (!dr.IsDBNull(iReemedcodi)) entity.Reemedcodi = Convert.ToInt32(dr.GetValue(iReemedcodi));

            int iReevprcodi = dr.GetOrdinal(this.Reevprcodi);
            if (!dr.IsDBNull(iReevprcodi)) entity.Reevprcodi = Convert.ToInt32(dr.GetValue(iReevprcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iReemedfechahora = dr.GetOrdinal(this.Reemedfechahora);
            if (!dr.IsDBNull(iReemedfechahora)) entity.Reemedfechahora = dr.GetDateTime(iReemedfechahora);

            int iReemedtensionrs = dr.GetOrdinal(this.Reemedtensionrs);
            if (!dr.IsDBNull(iReemedtensionrs)) entity.Reemedtensionrs = dr.GetDecimal(iReemedtensionrs);

            int iReemedtensionst = dr.GetOrdinal(this.Reemedtensionst);
            if (!dr.IsDBNull(iReemedtensionst)) entity.Reemedtensionst = dr.GetDecimal(iReemedtensionst);

            int iReemedtensiontr = dr.GetOrdinal(this.Reemedtensiontr);
            if (!dr.IsDBNull(iReemedtensiontr)) entity.Reemedtensiontr = dr.GetDecimal(iReemedtensiontr);

            int iReemedvarp = dr.GetOrdinal(this.Reemedvarp);
            if (!dr.IsDBNull(iReemedvarp)) entity.Reemedvarp = dr.GetDecimal(iReemedvarp);

            int iReemedvala = dr.GetOrdinal(this.Reemedvala);
            if (!dr.IsDBNull(iReemedvala)) entity.Reemedvala = dr.GetDecimal(iReemedvala);

            int iReemedvalap = dr.GetOrdinal(this.Reemedvalap);
            if (!dr.IsDBNull(iReemedvalap)) entity.Reemedvalap = dr.GetDecimal(iReemedvalap);

            int iReemedvalep = dr.GetOrdinal(this.Reemedvalep);
            if (!dr.IsDBNull(iReemedvalep)) entity.Reemedvalep = dr.GetDecimal(iReemedvalep);

            int iReemedvalaapep = dr.GetOrdinal(this.Reemedvalaapep);
            if (!dr.IsDBNull(iReemedvalaapep)) entity.Reemedvalaapep = dr.GetDecimal(iReemedvalaapep);

            int iReemedusucreacion = dr.GetOrdinal(this.Reemedusucreacion);
            if (!dr.IsDBNull(iReemedusucreacion)) entity.Reemedusucreacion = dr.GetString(iReemedusucreacion);

            int iReemedfeccreacion = dr.GetOrdinal(this.Reemedfeccreacion);
            if (!dr.IsDBNull(iReemedfeccreacion)) entity.Reemedfeccreacion = dr.GetDateTime(iReemedfeccreacion);

            int iReemedusumodificacion = dr.GetOrdinal(this.Reemedusumodificacion);
            if (!dr.IsDBNull(iReemedusumodificacion)) entity.Reemedusumodificacion = dr.GetString(iReemedusumodificacion);

            int iReemedfecmodificacion = dr.GetOrdinal(this.Reemedfecmodificacion);
            if (!dr.IsDBNull(iReemedfecmodificacion)) entity.Reemedfecmodificacion = dr.GetDateTime(iReemedfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reemedcodi = "REEMEDCODI";
        public string Reevprcodi = "REEVPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Reemedfechahora = "REEMEDFECHAHORA";
        public string Reemedtensionrs = "REEMEDTENSIONRS";
        public string Reemedtensionst = "REEMEDTENSIONST";
        public string Reemedtensiontr = "REEMEDTENSIONTR";
        public string Reemedvarp = "REEMEDVARP";
        public string Reemedvala = "REEMEDVALA";
        public string Reemedvalap = "REEMEDVALAP";
        public string Reemedvalep = "REEMEDVALEP";
        public string Reemedvalaapep = "REEMEDVALAAPEP";
        public string Reemedusucreacion = "REEMEDUSUCREACION";
        public string Reemedfeccreacion = "REEMEDFECCREACION";
        public string Reemedusumodificacion = "REEMEDUSUMODIFICACION";
        public string Reemedfecmodificacion = "REEMEDFECMODIFICACION";

        #endregion

        public string SqlObtenerMedicion
        {
            get { return base.GetSqlXml("ObtenerMedicion"); }
        }
    }
}
