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
    public class PmpoObraDetalleHelper : HelperBase
    {
        public PmpoObraDetalleHelper(): base(Consultas.PmpoObraDetalleSql)
        {
        }

        public PmpoObraDetalleDTO Create(IDataReader dr)
        {
            PmpoObraDetalleDTO entity = new PmpoObraDetalleDTO();

            int iObradtcodi = dr.GetOrdinal(this.Obradtcodi);
            if (!dr.IsDBNull(iObradtcodi)) entity.Obradetcodi = Convert.ToInt32(dr.GetValue(iObradtcodi));

            int iObracodi = dr.GetOrdinal(this.Obracodi);
            if (!dr.IsDBNull(iObracodi)) entity.Obracodi = Convert.ToInt32(dr.GetValue(iObracodi));

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iObradtdescripcion = dr.GetOrdinal(this.Obradtdescripcion);
            if (!dr.IsDBNull(iEquicodi)) entity.Obradetdescripcion = dr.GetString(iObradtdescripcion);

            return entity;
        }


        public PmpoObraDetalleDTO CreateBarra(IDataReader dr)
        {
            PmpoObraDetalleDTO entity = new PmpoObraDetalleDTO();

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iBarrnombre = dr.GetOrdinal(this.Barrnombre);
            if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

            int iBarrsituacion = dr.GetOrdinal(this.Barrsituacion);
            if (!dr.IsDBNull(iBarrsituacion)) entity.Barrsituacion = dr.GetString(iBarrsituacion);
            return entity;
        }

        public PmpoObraDetalleDTO CreateEquipo(IDataReader dr)
        {
            PmpoObraDetalleDTO entity = new PmpoObraDetalleDTO();

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            int iEquiestado = dr.GetOrdinal(this.Equiestado);
            if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

            return entity;
        }

        public PmpoObraDetalleDTO CreateGrupo(IDataReader dr)
        {
            PmpoObraDetalleDTO entity = new PmpoObraDetalleDTO();

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponombre);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponombre = dr.GetString(iGruponomb);

            int iGrupoestado = dr.GetOrdinal(this.Grupoestado);
            if (!dr.IsDBNull(iGrupoestado)) entity.Grupoestado = dr.GetString(iGrupoestado);

            return entity;
        }

        #region Mapeo de Campos

        public string Obradtcodi = "OBRADTCODI";
        public string Obracodi = "OBRACODI";
        public string Equicodi = "EQUICODI";

        public string Obrafecprop = "OBRAFECPROP";
        public string Obradtdescripcion = "OBRADTDESCRIPCION";
        public string Obrauser = "OBRAUSER";
        public string Obrafeccreacion = "OBRAFECCREACION";

        public string Formatnombre = "FORMATNOMBRE";
        public string Equinomb = "EQUINOMB";
        public string Equiestado = "EQUIESTADO";
        public string Famnomb = "FAMNOMB";
        public string Emprnomb = "EMPRNOMB";
        public string Equipot = "EQUIPOT";
        public string Equifechiniopcom = "EQUIFECHINIOPCOM";

        public string Barrcodi = "BARRCODI";
        public string Barrnombre = "BARRNOMBRE";
        public string Barrsituacion = "BARRSITUACION";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponombre = "GRUPONOMB";
        public string Grupoestado = "GRUPOESTADO";

        #endregion


        //SqlListBarras
        public string SqlListBarras
        {
            get { return base.GetSqlXml("ListBarras"); }
        }
        public string SqlListEquipos
        {
            get { return base.GetSqlXml("ListEquipos"); }
        }

        public string SqlListGrupos
        {
            get { return base.GetSqlXml("ListGrupos"); }
        }

        public string SqlGetByCriteriaGeneracion
        {
            get { return base.GetSqlXml("GetByCriteriaGeneracion"); }
        }

    }
}
