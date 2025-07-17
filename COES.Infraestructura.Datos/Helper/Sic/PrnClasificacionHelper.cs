using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnClasificacionHelper : HelperBase
    {
        public PrnClasificacionHelper()
            : base(Consultas.PrnClasificacionSql)
        {
        }

        public PrnClasificacionDTO Create(IDataReader dr)
        {
            PrnClasificacionDTO entity = new PrnClasificacionDTO();

            int iPrnclscodi = dr.GetOrdinal(this.Prnclscodi);
            if (!dr.IsDBNull(iPrnclscodi)) entity.Prnclscodi = Convert.ToInt32(dr.GetValue(iPrnclscodi));

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iPrnclsfecha = dr.GetOrdinal(this.Prnclsfecha);
            if (!dr.IsDBNull(iPrnclsfecha)) entity.Prnclsfecha = dr.GetDateTime(iPrnclsfecha);

            int iPrnclsclasificacion = dr.GetOrdinal(this.Prnclsclasificacion);
            if (!dr.IsDBNull(iPrnclsclasificacion)) entity.Prnclsclasificacion = Convert.ToInt32(dr.GetValue(iPrnclsclasificacion));

            int iPrnclsporcerrormin = dr.GetOrdinal(this.Prnclsporcerrormin);
            if (!dr.IsDBNull(iPrnclsporcerrormin)) entity.Prnclsporcerrormin = Convert.ToDecimal(dr.GetValue(iPrnclsporcerrormin));

            int iPrnclsporcerrormax = dr.GetOrdinal(this.Prnclsporcerrormax);
            if (!dr.IsDBNull(iPrnclsporcerrormax)) entity.Prnclsporcerrormax = Convert.ToDecimal(dr.GetValue(iPrnclsporcerrormax));

            int iPrnclsmagcargamin = dr.GetOrdinal(this.Prnclsmagcargamin);
            if (!dr.IsDBNull(iPrnclsmagcargamin)) entity.Prnclsmagcargamin = Convert.ToDecimal(dr.GetValue(iPrnclsmagcargamin));

            int iPrnclsmagcargamax = dr.GetOrdinal(this.Prnclsmagcargamax);
            if (!dr.IsDBNull(iPrnclsmagcargamax)) entity.Prnclsmagcargamax = Convert.ToDecimal(dr.GetValue(iPrnclsmagcargamax));

            int iPrnclsestado = dr.GetOrdinal(this.Prnclsestado);
            if (!dr.IsDBNull(iPrnclsestado)) entity.Prnclsestado = dr.GetString(iPrnclsestado);

            int iPrnclsperfil = dr.GetOrdinal(this.Prnclsperfil);
            if (!dr.IsDBNull(iPrnclsperfil)) entity.Prnclsperfil = Convert.ToInt32(dr.GetValue(iPrnclsperfil));

            int iPrnclsvariacion = dr.GetOrdinal(this.Prnclsvariacion);
            if (!dr.IsDBNull(iPrnclsvariacion)) entity.Prnclsvariacion = Convert.ToDecimal(dr.GetValue(iPrnclsvariacion));

            int iPrnclsusucreacion = dr.GetOrdinal(this.Prnclsusucreacion);
            if (!dr.IsDBNull(iPrnclsusucreacion)) entity.Prnclsusucreacion = dr.GetString(iPrnclsusucreacion);

            int iPrnclsfeccreacion = dr.GetOrdinal(this.Prnclsfeccreacion);
            if (!dr.IsDBNull(iPrnclsfeccreacion)) entity.Prnclsfeccreacion = dr.GetDateTime(iPrnclsfeccreacion);

            int iPrnclsusumodificacion = dr.GetOrdinal(this.Prnclsusumodificacion);
            if (!dr.IsDBNull(iPrnclsusumodificacion)) entity.Prnclsusumodificacion = dr.GetString(iPrnclsusumodificacion);

            int iPrnclsfecmodificacion = dr.GetOrdinal(this.Prnclsfecmodificacion);
            if (!dr.IsDBNull(iPrnclsfecmodificacion)) entity.Prnclsfecmodificacion = dr.GetDateTime(iPrnclsfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Prnclscodi = "PRNCLSCODI";
        public string Lectcodi = "LECTCODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Prnclsfecha = "PRNCLSFECHA";
        public string Prnclsclasificacion = "PRNCLSCLASIFICACION";
        public string Prnclsporcerrormin = "PRNCLSPORCERRORMIN";
        public string Prnclsporcerrormax = "PRNCLSPORCERRORMAX";
        public string Prnclsmagcargamin = "PRNCLSMAGCARGAMIN";
        public string Prnclsmagcargamax = "PRNCLSMAGCARGAMAX";
        public string Prnclsestado = "PRNCLSESTADO";
        public string Prnclsperfil = "PRNCLSPERFIL";
        public string Prnclsvariacion = "PRNCLSVARIACION";

        public string Prnclsusucreacion = "PRNCLSUSUCREACION";
        public string Prnclsfeccreacion = "PRNCLSFECCREACION";
        public string Prnclsusumodificacion = "PRNCLSUSUMODIFICACION";
        public string Prnclsfecmodificacion = "PRNCLSFECMODIFICACION";


        public string Medifecha = "MEDIFECHA";
        public string parpToFecha = "PARPTOFECHA";
        public string Prnm48tipo = "PRNM48TIPO";
        public string Prnm96tipo = "PRNM96TIPO";

        public string Ptomedidesc = "PTOMEDIDESC";
        public string Ptomedielenomb = "PTOMEDIELENOMB";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string Equiabrev = "EQUIABREV";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Emprabrev = "EMPRABREV";
        public string Areacodi = "AREACODI";
        public string Areanomb = "AREANOMB";
        public string Areaabrev = "AREAABREV";
        public string Meditotal = "MEDITOTAL";
        public string Prnmestado = "PRNMESTADO";
        public string Anivelcodi = "ANIVELCODI";

        public string Tareaabrev = "TAREAABREV";
        public string Famcodi = "FAMCODI";
        public string Famnomb = "FAMNOMB";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Tipoemprdesc = "TIPOEMPRDESC";

        public string Areaoperativa = "AREAOPERATIVA";

        public string Subcausadesc = "SUBCAUSADESC";
        public string Subcausacodi = "SUBCAUSACODI";
        #endregion

        #region Consultas a la BD

        public string SqlListClasificacion48
        {
            get { return base.GetSqlXml("ListClasificacion48"); }//Eliminar?
        }

        public string SqlListClasificacion96
        {
            get { return base.GetSqlXml("ListClasificacion96"); }//Eliminar?
        }

        public string SqlListPuntosClasificados48
        {
            get { return base.GetSqlXml("ListPuntosClasificados48"); }
        }

        public string SqlListPuntosClasificados96
        {
            get { return base.GetSqlXml("ListPuntosClasificados96"); }
        }

        public string SqlCountMedicionesByRangoFechas
        {
            get { return base.GetSqlXml("CountMedicionesByRangoFechas"); }
        }

        public string SqlListProdemPuntos
        {
            get { return base.GetSqlXml("ListProdemPuntos"); }
        }

        //Inicio PRODEM 2 - Nuevas implementaciones 20190512
        public string SqlListDemandaClasificada
        {
            get { return base.GetSqlXml("ListDemandaClasificada"); }
        }

        public string SqlListDemandaClasificadaBarrasCP
        {
            get { return base.GetSqlXml("ListDemandaClasificadaBarrasCP"); }
        }

        public string SqlListDemandaNoClasificada
        {
            get { return base.GetSqlXml("ListDemandaNoClasificada"); }
        }
        //Fin PRODEM 2 - Nuevas implementaciones 20190512

        #endregion
    }
}
