using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class DpoCasoHelper : HelperBase
    {
        public DpoCasoHelper() : base(Consultas.DpoCasoSql)
        {

        }

        #region Metodos
        public DpoCasoDTO CreateDpoCaso(IDataReader dr)
        {
            DpoCasoDTO entity = new DpoCasoDTO();


            int iDpocsocodi = dr.GetOrdinal(this.Dpocsocodi);
            if (!dr.IsDBNull(iDpocsocodi)) entity.Dpocsocodi = Convert.ToInt32(dr.GetValue(iDpocsocodi));


            int iDpocsocnombre = dr.GetOrdinal(this.Dpocsocnombre);
            if (!dr.IsDBNull(iDpocsocnombre)) entity.Dpocsocnombre = dr.GetString(iDpocsocnombre);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iDpocsousucreacion = dr.GetOrdinal(this.Dpocsousucreacion);
            if (!dr.IsDBNull(iDpocsousucreacion)) entity.Dpocsousucreacion = dr.GetString(iDpocsousucreacion);

            int iDpocsofeccreacion = dr.GetOrdinal(this.Dpocsofeccreacion);
            if (!dr.IsDBNull(iDpocsofeccreacion)) entity.Dpocsofeccreacion = dr.GetDateTime(iDpocsofeccreacion);

            int iDpocsousumodificacion = dr.GetOrdinal(this.Dpocsousumodificacion);
            if (!dr.IsDBNull(iDpocsousumodificacion)) entity.Dpocsousumodificacion = dr.GetString(iDpocsousumodificacion);

            int iDposcofecmodificacion = dr.GetOrdinal(this.Dposcofecmodificacion);
            if (!dr.IsDBNull(iDposcofecmodificacion)) entity.Dposcofecmodificacion = dr.GetDateTime(iDposcofecmodificacion);


            entity.StrDpocsofeccreacion = entity.Dpocsofeccreacion.ToString("dd/MM/yyyy");

            entity.StrDpocsousumodificacion = entity.Dposcofecmodificacion.ToString("dd/MM/yyyy");


            return entity;
        }

        #endregion

        #region Mapeo de Campos

        #region Tabla DPO_CASO
        public string Dpocsocodi = "DPOCSOCODI";

        public string Dpocsocnombre = "DPOCSOCNOMBRE";
        public string Areaabrev = "AREAABREV"; 
        public string Dpocsousucreacion = "DPOCSOUSUCREACION";
        public string Dpocsofeccreacion = "DPOCSOFECCREACION";
        public string Dpocsousumodificacion = "DPOCSOUSUMODIFICACION";
        public string Dposcofecmodificacion = "DPOSCOFECMODIFICACION";

        public string Nombre = "NOMBRE";
        public string Usuario = "USUARIO";
        #endregion

        #endregion

        #region Querys

        #region Querys Tabla DPO_CASO
        public string SqlFilter
        {
            get { return base.GetSqlXml("Filter"); }
        }

        public string SqlListNombreCasos
        {
            get { return base.GetSqlXml("ListNombreCasos"); }
        }

        public string SqlListUsuarios
        {
            get { return base.GetSqlXml("ListUsuarios"); }
        }
        #endregion

        #endregion
    }
}
