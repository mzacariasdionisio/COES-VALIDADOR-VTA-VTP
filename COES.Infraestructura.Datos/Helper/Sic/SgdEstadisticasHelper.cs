using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SGD_ESTADISTICAS
    /// </summary>
    public class SgdEstadisticasHelper : HelperBase
    {
        public SgdEstadisticasHelper()
            : base(Consultas.SgdEstadisticasSql)
        {
        }

        public SgdEstadisticasDTO Create(IDataReader dr)
        {
            SgdEstadisticasDTO entity = new SgdEstadisticasDTO();


            int iSgdecodi = dr.GetOrdinal(this.Sgdecodi);
            if (!dr.IsDBNull(iSgdecodi)) entity.Sgdecodi = Convert.ToInt32(dr.GetValue(iSgdecodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iFljcodi = dr.GetOrdinal(this.Fljcodi);
            if (!dr.IsDBNull(iFljcodi)) entity.Fljcodi = Convert.ToInt32(dr.GetValue(iFljcodi));

            int iFljdetcodi = dr.GetOrdinal(this.Fljdetcodi);
            if (!dr.IsDBNull(iFljdetcodi)) entity.Fljdetcodi = Convert.ToInt32(dr.GetValue(iFljdetcodi));

            int iFljdetcodiref = dr.GetOrdinal(this.Fljdetcodiref);
            if (!dr.IsDBNull(iFljdetcodiref)) entity.Fljdetcodiref = Convert.ToInt32(dr.GetValue(iFljdetcodiref));

            int iFljnumext = dr.GetOrdinal(this.Fljnumext);
            if (!dr.IsDBNull(iFljnumext)) entity.Fljnumext = dr.GetString(iFljnumext);

            int iFljestado = dr.GetOrdinal(this.Fljestado);
            if (!dr.IsDBNull(iFljestado)) entity.Fljestado = dr.GetString(iFljestado);

            int iFljnombre = dr.GetOrdinal(this.Fljnombre);
            if (!dr.IsDBNull(iFljnombre)) entity.Fljnombre = dr.GetString(iFljnombre);

            int iFljfecharecep = dr.GetOrdinal(this.Fljfecharecep);
            if (!dr.IsDBNull(iFljfecharecep)) entity.Fljfecharecep = dr.GetDateTime(iFljfecharecep);

            int iFljfechaorig = dr.GetOrdinal(this.Fljfechaorig);
            if (!dr.IsDBNull(iFljfechaorig)) entity.Fljfechaorig = dr.GetDateTime(iFljfechaorig);

            int iFljfechaproce = dr.GetOrdinal(this.Fljfechaproce);
            if (!dr.IsDBNull(iFljfechaproce)) entity.Fljfechaproce = dr.GetDateTime(iFljfechaproce);

            int iFljfechaterm = dr.GetOrdinal(this.Fljfechaterm);
            if (!dr.IsDBNull(iFljfechaterm)) entity.Fljfechaterm = dr.GetDateTime(iFljfechaterm);

            int iFljdiasmaxaten = dr.GetOrdinal(this.Fljdiasmaxaten);
            if (!dr.IsDBNull(iFljdiasmaxaten)) entity.Fljdiasmaxaten = Convert.ToInt32(dr.GetValue(iFljdiasmaxaten));

            int iSgdendiasdearea = dr.GetOrdinal(this.Sgdediasdearea);
            if (!dr.IsDBNull(iSgdendiasdearea)) entity.Sgdediasdearea = Convert.ToInt32(dr.GetValue(iSgdendiasdearea));

            int iSgdendiasdedir = dr.GetOrdinal(this.Sgdediasdedir);
            if (!dr.IsDBNull(iSgdendiasdedir)) entity.Sgdediasdedir = Convert.ToInt32(dr.GetValue(iSgdendiasdedir));

            int iSgdediadoc = dr.GetOrdinal(this.Sgdediadoc);
            if (!dr.IsDBNull(iSgdediadoc)) entity.Sgdediadoc = dr.GetString(iSgdediadoc);

            int iDescTipDoc = dr.GetOrdinal(this.DescTipDoc);
            if (!dr.IsDBNull(iDescTipDoc)) entity.DescTipoDoc = dr.GetString(iDescTipDoc);

            int iSgdedirrespcodi = dr.GetOrdinal(this.Sgdedirrespcodi);
            if (!dr.IsDBNull(iSgdedirrespcodi)) entity.Sgdedirrespcodi = Convert.ToInt32(dr.GetValue(iSgdedirrespcodi));

            int iSgdearearespcodi = dr.GetOrdinal(this.Sgdearearespcodi);
            if (!dr.IsDBNull(iSgdearearespcodi)) entity.Sgdearearespcodi = Convert.ToInt32(dr.GetValue(iSgdearearespcodi));

            int iAreacodedest = dr.GetOrdinal(this.Areacodedest);
            if (!dr.IsDBNull(iAreacodedest)) entity.Areacodedest = Convert.ToInt32(dr.GetValue(iAreacodedest));

            int iTatcodi = dr.GetOrdinal(this.Tatcodi);
            if (!dr.IsDBNull(iTatcodi)) entity.Tatcodi = Convert.ToInt32(dr.GetValue(iTatcodi));

            int iSgdeusucreacion = dr.GetOrdinal(this.Sgdeusucreacion);
            if (!dr.IsDBNull(iSgdeusucreacion)) entity.Sgdeusucreacion = dr.GetString(iSgdeusucreacion);

            int iSgdefeccreacion = dr.GetOrdinal(this.Sgdefeccreacion);
            if (!dr.IsDBNull(iSgdefeccreacion)) entity.Sgdefeccreacion = dr.GetDateTime(iSgdefeccreacion);

            int iSgdeusumodificacion = dr.GetOrdinal(this.Sgdeusumodificacion);
            if (!dr.IsDBNull(iSgdeusumodificacion)) entity.Sgdeusumodificacion = dr.GetString(iSgdeusumodificacion);

            int iSgdefecmodificacion = dr.GetOrdinal(this.Sgdefecmodificacion);
            if (!dr.IsDBNull(iSgdefecmodificacion)) entity.Sgdefecmodificacion = dr.GetDateTime(iSgdefecmodificacion);


            int iSgdefecderdirresp = dr.GetOrdinal(this.Sgdefecderdirresp);
            if (!dr.IsDBNull(iSgdefecderdirresp)) entity.Sgdefecderdirresp = dr.GetDateTime(iSgdefecderdirresp);

            int iSgdefecderarearesp = dr.GetOrdinal(this.Sgdefecderarearesp);
            if (!dr.IsDBNull(iSgdefecderarearesp)) entity.Sgdefecderarearesp = dr.GetDateTime(iSgdefecderarearesp);

            int iSgdediasatencion = dr.GetOrdinal(this.Sgdediasatencion);
            if (!dr.IsDBNull(iSgdediasatencion)) entity.Sgdediasatencion = Convert.ToInt32(dr.GetValue(iSgdediasatencion));

            int iSgdedirrespnomb = dr.GetOrdinal(this.Sgdedirrespnomb);
            if (!dr.IsDBNull(iSgdedirrespnomb)) entity.Sgdedirrespnomb = dr.GetString(iSgdedirrespnomb);

            int iSgdearearespnomb = dr.GetOrdinal(this.Sgdearearespnomb);
            if (!dr.IsDBNull(iSgdearearespnomb)) entity.Sgdearearespnomb = dr.GetString(iSgdearearespnomb);

            int iCorrnumproc = dr.GetOrdinal(this.Corrnumproc);
            if (!dr.IsDBNull(iCorrnumproc)) entity.Corrnumproc = Convert.ToInt32(dr.GetValue(iCorrnumproc));

            return entity;
        }

        public SgdEstadisticasDTO CreateCodiref(IDataReader dr)
        {
            SgdEstadisticasDTO entity = new SgdEstadisticasDTO();

            int iFljdetcodiref = dr.GetOrdinal(this.Fljdetcodiref);
            if (!dr.IsDBNull(iFljdetcodiref)) entity.Fljdetcodiref = Convert.ToInt32(dr.GetValue(iFljdetcodiref));

            int iFljcodi = dr.GetOrdinal(this.Fljcodi);
            if (!dr.IsDBNull(iFljcodi)) entity.Fljcodi = Convert.ToInt32(dr.GetValue(iFljcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Sgdecodi = "SGDECODI";
        public string Tipcodi = "TIPCODI";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Fljcodi = "FLJCODI";
        public string Fljdetcodi = "FLJDETCODI";
        public string Fljdetcodiref = "FLJDETCODIREF";
        public string Fljestado = "FLJESTADO";
        public string Fljnumext = "FLJNUMEXT";
        public string Fljnombre = "FLJNOMBRE";
        public string Fljdiasmaxaten = "FLJDIASMAXATEN";
        public string Fljfecharecep = "FLJFECHARECEP";
        public string Fljfechaorig = "FLJFECHAORIG";
        public string Fljfechaproce = "FLJFECHAPROCE";
        public string Fljfechaterm = "FLJFECHATERM";
        public string Sgdediasdearea = "SGDEDIASDEAREA";
        public string Sgdediasdedir = "SGDEDIASDEDIR";
        public string Sgdediadoc = "SGDEDIADOC";
        public string Sgdedirrespcodi = "SGDEDIRRESPCODI";
        public string Sgdearearespcodi = "SGDEAREARESPCODI";
        public string Areacodedest = "AREACODEDEST";
        public string Sgdeusucreacion = "SGDEUSUCREACION";
        public string Sgdefeccreacion = "SGDEFECCREACION";
        public string Sgdeusumodificacion = "SGDEUSUMODIFICACION";
        public string Sgdefecmodificacion = "SGDEFECMODIFICACION";
        public string Tatcodi = "TATCODI";
        public string DescTipDoc = "DESCTIPDOC";
        public string Sgdefecderdirresp = "SGDEFECDERDIRRESP";
        public string Sgdefecderarearesp = "SGDEFECDERAREARESP";
        public string Sgdediasatencion = "SGDEDIASATENCION";
        public string Sgdedirrespnomb = "SGDEDIRRESPNOMB";
        public string Sgdearearespnomb = "SGDEAREARESPNOMB";
        public string Corrnumproc = "CORRNUMPROC";

        #endregion
        public string SqlListAllCompanies
        {
            get { return base.GetSqlXml("ListAllCompanies"); }
        }

        public string SqlListCodiRef
        {
            get { return base.GetSqlXml("ListCodiRef"); }
        }

        public string SqlUpdateCodiRef
        {
            get { return base.GetSqlXml("UpdateCodiRef"); }
        }

        public string SqlUpdateNumext
        {
            get { return base.GetSqlXml("UpdateNumext"); }
        }
    }
}
