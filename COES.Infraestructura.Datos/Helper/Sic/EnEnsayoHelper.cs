using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EN_ENSAYO
    /// </summary>
    public class EnEnsayoHelper : HelperBase
    {
        public EnEnsayoHelper()
            : base(Consultas.EnEnsayoSql)
        {
        }

        public EnEnsayoDTO Create(IDataReader dr)
        {
            EnEnsayoDTO entity = new EnEnsayoDTO();

            int iEnsayocodi = dr.GetOrdinal(this.Ensayocodi);
            if (!dr.IsDBNull(iEnsayocodi)) entity.Ensayocodi = Convert.ToInt32(dr.GetValue(iEnsayocodi));

            int iEnsayofecha = dr.GetOrdinal(this.Ensayofecha);
            if (!dr.IsDBNull(iEnsayofecha)) entity.Ensayofecha = dr.GetDateTime(iEnsayofecha);

            int iUsercodi = dr.GetOrdinal(this.Usercodi);
            if (!dr.IsDBNull(iUsercodi)) entity.Usercodi = dr.GetString(iUsercodi);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iestadocodi = dr.GetOrdinal(this.Estadocodi);
            if (!dr.IsDBNull(iestadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iestadocodi));

            int iEnsayofechaevento = dr.GetOrdinal(this.Ensayofechaevento);
            if (!dr.IsDBNull(iEnsayofechaevento)) entity.Ensayofechaevento = dr.GetDateTime(iEnsayofechaevento);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);


            return entity;
        }


        #region Mapeo de Campos

        public string Ensayocodi = "ENSAYOCODI";
        public string Ensayofecha = "ENSAYOFECHA";
        public string Usercodi = "ENSAYOUSERCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Estadocodi = "ESTADOCODI";
        public string acepenvcodi = "ACEPENVCODI";
        public string Ensayofechaevento = "ENSAYOFECHAEVENTO";
        public string Lastdate = "ENSAYOLASTDATE";
        public string Lastuser = "ENSAYOLASTUSER";

        #endregion

        public string SqlListaDetalleFiltro
        {
            get { return GetSqlXml("ListaDetalleFiltro"); }
        }
        public string SqlListaDetalleFiltroXls
        {
            get { return GetSqlXml("ListaDetalleFiltroXls"); }
        }
        public string SqlUpdateEstadoEnsayo
        {
            get { return GetSqlXml("UpdateEstadoEnsayo"); }
        }

        public string SqlTotalListaEnsayo
        {
            get { return GetSqlXml("TotalListaEnsayo"); }
        }
        public string SqlSaveEnsayoMaster
        {
            get { return GetSqlXml("SaveEnsayoMaster"); }
        }

        public string SqlGetMaxIdEnMaster
        {
            get { return GetSqlXml("GetMaxIdEnMaster"); }
        }

    }
}
