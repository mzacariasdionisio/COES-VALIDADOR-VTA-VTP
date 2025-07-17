using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EN_ENSAYOFORMATO
    /// </summary>
    public class EnEnsayoformatoHelper : HelperBase
    {
        public EnEnsayoformatoHelper()
            : base(Consultas.EnEnsayoformatoSql)
        {
        }

        public EnEnsayoformatoDTO Create(IDataReader dr)
        {
            EnEnsayoformatoDTO entity = new EnEnsayoformatoDTO();

            int iFormatocodi = dr.GetOrdinal(this.Formatocodi);
            if (!dr.IsDBNull(iFormatocodi)) entity.Formatocodi = Convert.ToInt32(dr.GetValue(iFormatocodi));

            int iEnunidadcodi = dr.GetOrdinal(this.Enunidadcodi);
            if (!dr.IsDBNull(iEnunidadcodi)) entity.Enunidadcodi = Convert.ToInt32(dr.GetValue(iEnunidadcodi));

            int iEnsformatnomblogico = dr.GetOrdinal(this.Ensformatnomblogico);
            if (!dr.IsDBNull(iEnsformatnomblogico)) entity.Ensformatnomblogico = dr.GetString(iEnsformatnomblogico);

            int iEnsformtestado = dr.GetOrdinal(this.Ensformtestado);
            if (!dr.IsDBNull(iEnsformtestado)) entity.Ensformtestado = Convert.ToInt32(dr.GetValue(iEnsformtestado));

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iEnsformatnombfisico = dr.GetOrdinal(this.Ensformatnombfisico);
            if (!dr.IsDBNull(iEnsformatnombfisico)) entity.Ensformatnombfisico = dr.GetString(iEnsformatnombfisico);

            return entity;
        }


        #region Mapeo de Campos

        public string Formatocodi = "ENFMTCODI";
        public string Enunidadcodi = "ENUNIDCODI";
        public string Ensformatnombfisico = "ENSFMTNOMBFISICO";
        public string Ensformatnomblogico = "ENSFMTNOMBLOGICO";
        public string Lastdate = "ENSFMTLASTDATE";
        public string Lastuser = "ENSFMTLASTUSER";
        public string Ensformtestado = "ENSFMTESTADO";

        public string Equicodi = "EQUICODI";
        public string Ensayocodi = "ENSAYOCODI";


        #endregion

        public string SqlListaFormatoXEnsayo
        {
            get { return GetSqlXml("ListFormatoXEnsayo"); }
        }
        public string SqlListaFormatoXEnsayo2
        {
            get { return GetSqlXml("ListFormatoEnsayo2"); }
        }
        public string SqlUpdateEstado
        {
            get { return GetSqlXml("UpdateEstado"); }
        }
        public string SqlListaFormatosEmpresaCentral
        {
            get { return GetSqlXml("ListaFormatosEmpresaCentral"); }
        }
        public string SqlListarUnidadesConFormatos
        {
            get { return base.GetSqlXml("UnidadesconSusFormatos"); }
        }

    }
}
