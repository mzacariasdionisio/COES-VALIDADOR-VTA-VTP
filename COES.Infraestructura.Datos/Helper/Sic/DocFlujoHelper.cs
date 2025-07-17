using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla DOC_DIA_ESP
    /// </summary>
    public class DocFlujoHelper : HelperBase
    {
        public DocFlujoHelper()
            : base(Consultas.DocFlujoSql)
        {
        }

        public DocFlujoDTO Create(IDataReader dr)
        {
            DocFlujoDTO entity = new DocFlujoDTO();

            int iFljfecharecep = dr.GetOrdinal(this.Fljfecharecep);
            if (!dr.IsDBNull(iFljfecharecep)) entity.Fljfecharecep = dr.GetDateTime(iFljfecharecep);

            int iFljfechaproce = dr.GetOrdinal(this.Fljfechaproce);
            if (!dr.IsDBNull(iFljfechaproce)) entity.Fljfechaproce = dr.GetDateTime(iFljfechaproce);

            int iFljcodi = dr.GetOrdinal(this.Fljcodi);
            if (!dr.IsDBNull(iFljcodi)) entity.Fljcodi = Convert.ToInt32(dr.GetValue(iFljcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            int iFljremitente = dr.GetOrdinal(this.Fljremitente);
            if (!dr.IsDBNull(iFljremitente)) entity.Fljremitente = dr.GetInt32(iFljremitente);

            int iFljfechaorig = dr.GetOrdinal(this.Fljfechaorig);
            if (!dr.IsDBNull(iFljfechaorig)) entity.Fljfechaorig = dr.GetDateTime(iFljfechaorig);

            int iFljfechainicio = dr.GetOrdinal(this.Fljfechainicio);
            if (!dr.IsDBNull(iFljfechainicio)) entity.Fljfechainicio = dr.GetDateTime(iFljfechainicio);

            int iAreaCodeDest = dr.GetOrdinal(this.Areacodedest);
            if (!dr.IsDBNull(iAreaCodeDest)) entity.Areacodedest = dr.GetInt32(iAreaCodeDest);

            int iTipcodi = dr.GetOrdinal(this.Tipcodi);
            if (!dr.IsDBNull(iTipcodi)) entity.Tipcodi = dr.GetInt32(iTipcodi);

            int iFljnombre = dr.GetOrdinal(this.Fljnombre);
            if (!dr.IsDBNull(iFljnombre)) entity.Fljnombre = dr.GetString(iFljnombre);

            int iFljdiasmaxaten = dr.GetOrdinal(this.Fljdiasmaxaten);
            if (!dr.IsDBNull(iFljdiasmaxaten)) entity.Fljdiasmaxaten = dr.GetInt32(iFljdiasmaxaten);

            int iFljfechaterm = dr.GetOrdinal(this.Fljfechaterm);
            if (!dr.IsDBNull(iFljfechaterm)) entity.Fljfechaterm = dr.GetDateTime(iFljfechaterm);

            int iFljnumext = dr.GetOrdinal(this.Fljnumext);
            if (!dr.IsDBNull(iFljnumext)) entity.Fljnumext = dr.GetString(iFljnumext);

            int iFljestado = dr.GetOrdinal(this.Fljestado);
            if (!dr.IsDBNull(iFljestado)) entity.Fljestado = dr.GetString(iFljestado);

            int iTatcodi = dr.GetOrdinal(this.Tatcodi);
            if (!dr.IsDBNull(iTatcodi)) entity.Tatcodi = Convert.ToInt32(dr.GetValue(iTatcodi));

            int iFljdetcodi = dr.GetOrdinal(this.Fljdetcodi);
            if (!dr.IsDBNull(iFljdetcodi)) entity.Fljdetcodi = Convert.ToInt32(dr.GetValue(iFljdetcodi));

            int iFljdetcodiref = dr.GetOrdinal(this.Fljdetcodiref);
            if (!dr.IsDBNull(iFljdetcodiref)) entity.Fljdetcodiref = Convert.ToInt32(dr.GetValue(iFljdetcodiref));

            return entity;
        }

        public DocFlujoDTO CreateArea(IDataReader dr)
        {
            DocFlujoDTO entity = new DocFlujoDTO();

            int iFljdetnivel = dr.GetOrdinal(this.Fljdetnivel);
            if (!dr.IsDBNull(iFljdetnivel)) entity.Fljdetnivel = dr.GetInt32(iFljdetnivel);

            int iAreacode = dr.GetOrdinal(this.Areacode);
            if (!dr.IsDBNull(iAreacode)) entity.Areacode = dr.GetInt32(iAreacode);

            int iAreapadre = dr.GetOrdinal(this.Areapadre);
            if (!dr.IsDBNull(iAreapadre)) entity.Areapadre = dr.GetInt32(iAreapadre);

            int iTatcodi = dr.GetOrdinal(this.Tatcodi);
            if (!dr.IsDBNull(iTatcodi)) entity.Tatcodi = Convert.ToInt32(dr.GetValue(iTatcodi));  // Modificado por sts

            int iFljcadatencion = dr.GetOrdinal(this.Fljcadatencion); // 07 - Nov- 2017 STS
            if (!dr.IsDBNull(iFljcadatencion)) entity.Fljcadatencion = dr.GetString(iFljcadatencion); // 07 - Nov- 2017 STS


            return entity;
        }

        #region Mapeo de Campos

        public string Fljdetnivel = "FLJDETNIVEL";
        public string Fljfecharecep = "FLJFECHARECEP";
        public string Fljfechaproce = "FLJFECHAPROCE";
        public string Fljcodi = "FLJCODI";
        public string Fljdetcodi = "FLJDETCODI";
        public string Fljdetcodiref = "FLJDETCODIREF";
        public string Emprcodi = "EMPRCODI";
        public string Fljremitente = "FLJREMITENTE";
        public string Fljfechaorig = "FLJFECHAORIG";
        public string Fljfechainicio = "FLJFECHAINICIO";
        public string Areacodedest = "AREACODEDEST";
        public string Areacode = "AREACODE";
        public string Areapadre = "AREAPADRE";
        public string Tipcodi = "TIPCODI";
        public string Fljnombre = "FLJNOMBRE";
        public string Fljdiasmaxaten = "FLJDIASMAXATEN";
        public string Fljfechaterm = "FLJFECHATERM";
        public string Fljnumext = "FLJNUMEXT";
        public string Fljestado = "FLJESTADO";
        public string Tatcodi = "TATCODI";
        public string Fljcadatencion = "FLJCADATENCION"; // 07 - Nov - 2017 

        #endregion

        public string SqlListEstad
        {
            get { return base.GetSqlXml("ListEstad"); }
        }

        public string SqlGetDocRespuesta
        {
            get { return base.GetSqlXml("GetDocRespuesta"); }
        }
        public string SqlListAreaResponsable
        {
            get { return base.GetSqlXml("ListAreaResponsable"); }
        }

        #region MigracionSGOCOES-GrupoB
        public string SqlListDocCV
        {
            get { return base.GetSqlXml("ListDocCV"); }
        }
        #endregion
    }
}
