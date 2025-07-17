using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnAgrupacionHelper : HelperBase
    {
        public PrnAgrupacionHelper()
            : base(Consultas.PrnAgrupacionSql)
        {
        }

        public PrnAgrupacionDTO Create(IDataReader dr)
        {
            PrnAgrupacionDTO entity = new PrnAgrupacionDTO();

            int iAgrupcodi = dr.GetOrdinal(this.Agrupcodi);
            if (!dr.IsDBNull(iAgrupcodi)) entity.Agrupcodi = Convert.ToInt32(dr.GetValue(iAgrupcodi));

            int iPtogrpcodi = dr.GetOrdinal(this.Ptogrpcodi);
            if (!dr.IsDBNull(iPtogrpcodi)) entity.Ptogrpcodi = Convert.ToInt32(dr.GetValue(iPtogrpcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iAgrupfactor = dr.GetOrdinal(this.Agrupfactor);
            if (!dr.IsDBNull(iAgrupfactor)) entity.Agrupfactor = Convert.ToInt32(dr.GetValue(iAgrupfactor));

            int iAgrupfechaini = dr.GetOrdinal(this.Agrupfechaini);
            if (!dr.IsDBNull(iAgrupfechaini)) entity.Agrupfechaini = dr.GetDateTime(iAgrupfechaini);

            int iAgrupfechafin = dr.GetOrdinal(this.Agrupfechafin);
            if (!dr.IsDBNull(iAgrupfechafin)) entity.Agrupfechafin = dr.GetDateTime(iAgrupfechafin);

            return entity;
        }

        #region Mapeo de los campos

        public string Agrupcodi = "AGRUPCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Agrupfactor = "AGRUPFACTOR";
        public string Agrupfechaini = "AGRUPFECHAINI";
        public string Agrupfechafin = "AGRUPFECHAFIN";

        public string Ptogrpcodi = "PTOGRPCODI";
        public string Ptogrppronostico = "PTOGRPPRONOSTICO";
        public string Ptogrpfechaini = "PTOGRPFECHAINI";
        public string Ptogrpfechafin = "PTOGRPFECHAFIN";
        public string Ptogrpusumodificacion = "PTOGRPUSUMODIFICACION";

        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Emprcodi = "EMPRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Osicodi = "OSICODI";
        public string Equicodi = "EQUICODI";
        public string Codref = "CODREF";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Orden = "ORDEN";
        public string Ptomedielenomb = "PTOMEDIELENOMB";
        public string Ptomedibarranomb = "PTOMEDIBARRANOMB";
        public string Origlectcodi = "ORIGLECTCODI";
        public string Tptomedicodi = "TPTOMEDICODI";
        public string Ptomediestado = "PTOMEDIESTADO";
        public string Ptomedicalculado = "PTOMEDICALCULADO";


        public string Ptomedifechaini = "PTOMEDIFECHAINI";
        public string Ptomedifechafin = "PTOMEDIFECHAFIN";

        public string Valido = "VALIDO";

        public string Areacodi = "AREACODI";
        public string Areanomb = "AREANOMB";
        public string Emprnomb = "EMPRNOMB";
        public string Ptogrphijocodi = "PTOGRPHIJOCODI";
        public string Ptogrphijodesc = "PTOGRPHIJODESC";
        public string Meditotal = "MEDITOTAL";
        public string Prnm48tipo = "PRNM48TIPO";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Prnmestado = "PRNMESTADO";
        #endregion

        #region Consultas

        public string SqlListById
        {
            get { return base.GetSqlXml("ListById"); }
        }

        public string SqlListMeAgrupacion
        {
            get { return base.GetSqlXml("ListMeAgrupacion"); }
        }
        
        public string SqlSavePuntoAgrupacion
        {
            get { return base.GetSqlXml("SavePuntoAgrupacion"); }
        }

        public string SqlGetMaxIdPuntoAgrupacion
        {
            get { return base.GetSqlXml("GetMaxIdPuntoAgrupacion"); }
        }

        public string SqlListByIdPuntoAgrupacion
        {
            get { return base.GetSqlXml("ListByIdPuntoAgrupacion"); }
        }

        public string SqlListPtosAgrupadosParaProdem
        {
            get { return base.GetSqlXml("ListPtosAgrupadosParaProdem"); }
        }

        public string SqlCerrarPuntoAgrupacion
        {
            get { return base.GetSqlXml("CerrarPuntoAgrupacion"); }
        }

        public string SqlValidarNombreAgrupacion
        {
            get { return base.GetSqlXml("ValidarNombreAgrupacion"); }
        }

        public string SqlListDemandaAgrupada
        {
            get { return base.GetSqlXml("ListDemandaAgrupada"); }
        }

        public string SqlListAgrupacionesActivas
        {
            get { return base.GetSqlXml("ListAgrupacionesActivas"); }
        }

        public string SqlListPuntosPR03
        {
            get { return base.GetSqlXml("ListPuntosPR03"); }
        }
        public string SqlListUbicacionesPR03
        {
            get { return base.GetSqlXml("ListUbicacionesPR03"); }
        }
        public string SqlListEmpresasPR03
        {
            get { return base.GetSqlXml("ListEmpresasPR03"); }
        }
        public string SqlListPuntosSeleccionados
        {
            get { return base.GetSqlXml("ListPuntosSeleccionados"); }
        }
        public string SqlGetDetalleAgrupacion
        {
            get { return base.GetSqlXml("GetDetalleAgrupacion"); }
        }
        public string SqlGetAgrupacion
        {
            get { return base.GetSqlXml("GetAgrupacion"); }
        }

        #endregion

    }
}
