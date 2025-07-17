using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class VcePtomedModopeHelper : HelperBase
    {
        public VcePtomedModopeHelper() : base(Consultas.VcePtomedModopeSql)
        {
        }

        public VcePtomedModopeDTO Create(IDataReader dr)
        {
            VcePtomedModopeDTO entity = new VcePtomedModopeDTO();

            int iPmemopfecmodificacion = dr.GetOrdinal(this.Pmemopfecmodificacion);
            if (!dr.IsDBNull(iPmemopfecmodificacion)) entity.Pmemopfecmodificacion = dr.GetDateTime(iPmemopfecmodificacion);

            int iPmemopusumodificacion = dr.GetOrdinal(this.Pmemopusumodificacion);
            if (!dr.IsDBNull(iPmemopusumodificacion)) entity.Pmemopusumodificacion = dr.GetString(iPmemopusumodificacion);

            int iPmemopfeccreacion = dr.GetOrdinal(this.Pmemopfeccreacion);
            if (!dr.IsDBNull(iPmemopfeccreacion)) entity.Pmemopfeccreacion = dr.GetDateTime(iPmemopfeccreacion);

            int iPmemopusucreacion = dr.GetOrdinal(this.Pmemopusucreacion);
            if (!dr.IsDBNull(iPmemopusucreacion)) entity.Pmemopusucreacion = dr.GetString(iPmemopusucreacion);

            int iPmemopestregistro = dr.GetOrdinal(this.Pmemopestregistro);
            if (!dr.IsDBNull(iPmemopestregistro)) entity.Pmemopestregistro = dr.GetString(iPmemopestregistro);

            int iPmemoporden = dr.GetOrdinal(this.Pmemoporden);
            if (!dr.IsDBNull(iPmemoporden)) entity.Pmemoporden = Convert.ToInt32(dr.GetValue(iPmemoporden));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.Pecacodi = Convert.ToInt32(dr.GetValue(iPecacodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Pmemopfecmodificacion = "PMEMOPFECMODIFICACION";
        public string Pmemopusumodificacion = "PMEMOPUSUMODIFICACION";
        public string Pmemopfeccreacion = "PMEMOPFECCREACION";
        public string Pmemopusucreacion = "PMEMOPUSUCREACION";
        public string Pmemopestregistro = "PMEMOPESTREGISTRO";
        public string Pmemoporden = "PMEMOPORDEN";
        public string Grupocodi = "GRUPOCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Gruponomb = "GRUPONOMB";
        public string Pecacodi = "PECACODI";

        #endregion

        public string SqlGetMaxOrden
        {
            get { return base.GetSqlXml("GetMaxOrden"); }
        }

        public string SqlValidar
        {
            get { return base.GetSqlXml("Validar"); }
        }

        public string SqlListById
        {
            get { return base.GetSqlXml("ListById"); }
        }

        public string SqlSaveByEntity
        {
            get { return base.GetSqlXml("SaveByEntity"); }
        }

        public string SqlUpdateByEntity
        {
            get { return base.GetSqlXml("UpdateByEntity"); }
        }

        public string SqlDeleteByEntity
        {
            get { return base.GetSqlXml("DeleteByEntity"); }
        }

        public string SqlDeleteByVersion
        {
            get { return base.GetSqlXml("DeleteByVersion"); }
        }
        public string SqlSaveFromOtherVersion
        {
            get { return base.GetSqlXml("SaveFromOtherVersion"); }
        }

    }
}
