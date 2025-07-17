using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;
using COES.Dominio.DTO.Sp7;

namespace COES.Infraestructura.Datos.Helper.Sp7
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_VERSION_SP7
    /// </summary>
    public class TrVersionSp7Helper : HelperBase
    {
        public TrVersionSp7Helper(): base(Consultas.TrVersionSp7Sql)
        {
        }

        public TrVersionSp7DTO Create(IDataReader dr)
        {
            TrVersionSp7DTO entity = new TrVersionSp7DTO();

            int iVercodi = dr.GetOrdinal(this.Vercodi);
            if (!dr.IsDBNull(iVercodi)) entity.Vercodi = Convert.ToInt32(dr.GetValue(iVercodi));

            int iEmprcodieje = dr.GetOrdinal(this.Emprcodieje);
            if (!dr.IsDBNull(iEmprcodieje)) entity.Emprcodieje = Convert.ToInt32(dr.GetValue(iEmprcodieje));

            int iVerfechaini = dr.GetOrdinal(this.Verfechaini);
            if (!dr.IsDBNull(iVerfechaini)) entity.Verfechaini = dr.GetDateTime(iVerfechaini);

            int iVerfechafin = dr.GetOrdinal(this.Verfechafin);
            if (!dr.IsDBNull(iVerfechafin)) entity.Verfechafin = dr.GetDateTime(iVerfechafin);

            int iVernota = dr.GetOrdinal(this.Vernota);
            if (!dr.IsDBNull(iVernota)) entity.Vernota = dr.GetString(iVernota);

            int iVernotaeje = dr.GetOrdinal(this.Vernotaeje);
            if (!dr.IsDBNull(iVernotaeje)) entity.Vernotaeje = dr.GetString(iVernotaeje);

            int iVernumero = dr.GetOrdinal(this.Vernumero);
            if (!dr.IsDBNull(iVernumero)) entity.Vernumero = Convert.ToInt32(dr.GetValue(iVernumero));

            int iVerrepoficial = dr.GetOrdinal(this.Verrepoficial);
            if (!dr.IsDBNull(iVerrepoficial)) entity.Verrepoficial = dr.GetString(iVerrepoficial);

            int iVerestado = dr.GetOrdinal(this.Verestado);
            if (!dr.IsDBNull(iVerestado)) entity.Verestado = dr.GetString(iVerestado);

            int iVerreprocestadcanal = dr.GetOrdinal(this.Verreprocestadcanal);
            if (!dr.IsDBNull(iVerreprocestadcanal)) entity.Verreprocestadcanal = dr.GetString(iVerreprocestadcanal);

            int iVerconsidexclus = dr.GetOrdinal(this.Verconsidexclus);
            if (!dr.IsDBNull(iVerconsidexclus)) entity.Verconsidexclus = dr.GetString(iVerconsidexclus);

            int iVerestadisticacontabmae = dr.GetOrdinal(this.Verestadisticacontabmae);
            if (!dr.IsDBNull(iVerestadisticacontabmae)) entity.Verestadisticacontabmae = dr.GetString(iVerestadisticacontabmae);

            int iVerestadisticamaefecha = dr.GetOrdinal(this.Verestadisticamaefecha);
            if (!dr.IsDBNull(iVerestadisticamaefecha)) entity.Verestadisticamaefecha = dr.GetDateTime(iVerestadisticamaefecha);

            int iVerauditoria = dr.GetOrdinal(this.Verauditoria);
            if (!dr.IsDBNull(iVerauditoria)) entity.Verauditoria = dr.GetString(iVerauditoria);

            int iVerultfechaejec = dr.GetOrdinal(this.Verultfechaejec);
            if (!dr.IsDBNull(iVerultfechaejec)) entity.Verultfechaejec = dr.GetDateTime(iVerultfechaejec);

            int iVersionaplic = dr.GetOrdinal(this.Versionaplic);
            if (!dr.IsDBNull(iVersionaplic)) entity.Versionaplic = dr.GetString(iVersionaplic);

            int iVerusucreacion = dr.GetOrdinal(this.Verusucreacion);
            if (!dr.IsDBNull(iVerusucreacion)) entity.Verusucreacion = dr.GetString(iVerusucreacion);

            int iVerfeccreacion = dr.GetOrdinal(this.Verfeccreacion);
            if (!dr.IsDBNull(iVerfeccreacion)) entity.Verfeccreacion = dr.GetDateTime(iVerfeccreacion);

            int iVerusumodificacion = dr.GetOrdinal(this.Verusumodificacion);
            if (!dr.IsDBNull(iVerusumodificacion)) entity.Verusumodificacion = dr.GetString(iVerusumodificacion);

            int iVerfecmodificacion = dr.GetOrdinal(this.Verfecmodificacion);
            if (!dr.IsDBNull(iVerfecmodificacion)) entity.Verfecmodificacion = dr.GetDateTime(iVerfecmodificacion);
            
            return entity;
        }


        #region Mapeo de Campos

        public string Vercodi = "VERCODI";
        public string Emprcodieje = "EMPRCODIEJE";
        public string Emprenomb = "EMPRENOMB";
        public string Verfechaini = "VERFECHAINI";
        public string Verfechafin = "VERFECHAFIN";
        public string Vernota = "VERNOTA";
        public string Vernotaeje = "VERNOTAEJE";
        public string Vernumero = "VERNUMERO";
        public string Verrepoficial = "VERREPOFICIAL";
        public string Verestado = "VERESTADO";
        public string Verreprocestadcanal = "VERREPROCESTADCANAL";
        public string Verconsidexclus = "VERCONSIDEXCLUS";
        public string Verestadisticacontabmae = "VERESTADISTICACONTABMAE";
        public string Verestadisticamaefecha = "VERESTADISTICAMAEFECHA";
        public string Verauditoria = "VERAUDITORIA";
        public string Verultfechaejec = "VERULTFECHAEJEC";
        public string Versionaplic = "VERSIONAPLIC";
        public string Verusucreacion = "VERUSUCREACION";
        public string Verfeccreacion = "VERFECCREACION";
        public string Verusumodificacion = "VERUSUMODIFICACION";
        public string Verfecmodificacion = "VERFECMODIFICACION";
        public string Verestadodescrip = "VERESTADODESCRIP";
        
        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlListFecha
        {
            get { return base.GetSqlXml("ListFecha"); }
        }

        public string SqlListPendiente
        {
            get { return base.GetSqlXml("ListPendiente"); }
        }

        public string SqlGetMinId
        {
            get { return base.GetSqlXml("GetMinId"); }
        }


        public string SqlGetVersion
        {
            get { return base.GetSqlXml("GetVersion"); }
        }

        #endregion
    }
}
