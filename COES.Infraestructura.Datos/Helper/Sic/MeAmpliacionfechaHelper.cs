using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_AMPLIACIONFECHA
    /// </summary>
    public class MeAmpliacionfechaHelper : HelperBase
    {
        public MeAmpliacionfechaHelper(): base(Consultas.MeAmpliacionfechaSql)
        {
        }

        public MeAmpliacionfechaDTO Create(IDataReader dr)
        {
            MeAmpliacionfechaDTO entity = new MeAmpliacionfechaDTO();

            int iAmplicodi = dr.GetOrdinal(this.Amplicodi);
            if (!dr.IsDBNull(iAmplicodi)) entity.Amplicodi = Convert.ToInt32(dr.GetValue(iAmplicodi));

            int iAmplifecha = dr.GetOrdinal(this.Amplifecha);
            if (!dr.IsDBNull(iAmplifecha)) entity.Amplifecha = dr.GetDateTime(iAmplifecha);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iAmplifechaplazo = dr.GetOrdinal(this.Amplifechaplazo);
            if (!dr.IsDBNull(iAmplifechaplazo)) entity.Amplifechaplazo = dr.GetDateTime(iAmplifechaplazo);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Amplifecha = "AMPLIFECHA";
        public string Emprcodi = "EMPRCODI";
        public string Formatcodi = "FORMATCODI";
        public string Amplifechaplazo = "AMPLIFECHAPLAZO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Emprnomb = "Emprnomb";
        public string Formatnombre = "Formatnombre";
        public string Formatdiafinplazo = "FORMATDIAFINPLAZO";
        public string Formatdiaplazo = "FORMATDIAPLAZO";
        public string Amplicodi = "AMPLICODI";

        public string Qregistros = "Q_REGISTROS";

        #endregion

        public string SqlListaAmpliacion
        {
            get { return base.GetSqlXml("ListaAmpliacion"); }
        }

        public string SqlListaAmpliacionMultiple
        {
            get { return base.GetSqlXml("ListaAmpliacionMultiple"); }
        }

        public string SqlUpdateById
        {
            get { return base.GetSqlXml("UpdateById"); }
        }

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlGetAmpliacionNow
        {
            get { return base.GetSqlXml("GetAmpliacionNow"); }
        }

        public string SqlListaEmpresasAmpliacionPlazo
        {
            get { return base.GetSqlXml("ListaEmpresasAmpliacionPlazo"); }
        }

        public string SqListaAmpliacionFiltro
        {
            get { return base.GetSqlXml("ListaAmpliacionFiltro"); }
        }

        public string SqListaAmpliacionCount
        {
            get { return base.GetSqlXml("ListaAmpliacionCount"); }
        }
    }
}
