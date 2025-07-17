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
    public class PmoPeriodoHelper : HelperBase
    {
        public PmoPeriodoHelper()
            : base(Consultas.PmoPeriodo)
        {
        }

        public PmoPeriodoDTO Create(IDataReader dr)
        {
            PmoPeriodoDTO entity = new PmoPeriodoDTO();

            int iPmPeriCodi = dr.GetOrdinal(this.PmPeriCodi);
            if (!dr.IsDBNull(iPmPeriCodi)) entity.PmPeriCodi = Convert.ToInt32(dr.GetValue(iPmPeriCodi));

            int iPmPeriNombre = dr.GetOrdinal(this.PmPeriNombre);
            if (!dr.IsDBNull(iPmPeriNombre)) entity.PmPeriNombre = dr.GetString(iPmPeriNombre);

            int iPmPeriAniOMes = dr.GetOrdinal(this.PmPeriAniOMes);
            if (!dr.IsDBNull(iPmPeriAniOMes)) entity.PmPeriAniOMes = dr.GetInt32(iPmPeriAniOMes);

            int iPmPeriTipo = dr.GetOrdinal(this.PmPeriTipo);
            if (!dr.IsDBNull(iPmPeriTipo)) entity.PmPeriTipo = dr.GetString(iPmPeriTipo);

            int iPmPeriEstado = dr.GetOrdinal(this.PmPeriEstado);
            if (!dr.IsDBNull(iPmPeriEstado)) entity.PmPeriEstado = dr.GetString(iPmPeriEstado);

            int iPmPeriUsuCreacion = dr.GetOrdinal(this.PmPeriUsuCreacion);
            if (!dr.IsDBNull(iPmPeriUsuCreacion)) entity.PmPeriUsuCreacion = dr.GetString(iPmPeriUsuCreacion);

            int iPmPeriFecCreacion = dr.GetOrdinal(this.PmPeriFecCreacion);
            if (!dr.IsDBNull(iPmPeriFecCreacion)) entity.PmPeriFecCreacion = dr.GetDateTime(iPmPeriFecCreacion);

            int iPmPeriUsuModificacion = dr.GetOrdinal(this.PmPeriUsuModificacion);
            if (!dr.IsDBNull(iPmPeriUsuModificacion)) entity.PmPeriUsuModificacion = dr.GetString(iPmPeriUsuModificacion);

            int iPmPeriFecModificacion = dr.GetOrdinal(this.PmPeriFecModificacion);
            if (!dr.IsDBNull(iPmPeriFecModificacion)) entity.PmPeriFecModificacion = dr.GetDateTime(iPmPeriFecModificacion);

            int iPmPeriFecIniMantAnual = dr.GetOrdinal(this.PmPeriFecIniMantAnual);
            if (!dr.IsDBNull(iPmPeriFecIniMantAnual)) entity.PmPeriFecIniMantAnual = dr.GetDateTime(iPmPeriFecIniMantAnual);

            int iPmPeriFecFinMantAnual = dr.GetOrdinal(this.PmPeriFecFinMantAnual);
            if (!dr.IsDBNull(iPmPeriFecFinMantAnual)) entity.PmPeriFecFinMantAnual = dr.GetDateTime(iPmPeriFecFinMantAnual);

            int iPmPeriFecIniMantMensual = dr.GetOrdinal(this.PmPeriFecIniMantMensual);
            if (!dr.IsDBNull(iPmPeriFecIniMantMensual)) entity.PmPeriFecIniMantMensual = dr.GetDateTime(iPmPeriFecIniMantMensual);

            int iPmPeriFecFinMantMensual = dr.GetOrdinal(this.PmPeriFecFinMantMensual);
            if (!dr.IsDBNull(iPmPeriFecFinMantMensual)) entity.PmPeriFecFinMantMensual = dr.GetDateTime(iPmPeriFecFinMantMensual);

            int iPmPeriFechaPeriodo = dr.GetOrdinal(this.PmPeriFechaPeriodo);
            if (!dr.IsDBNull(iPmPeriFechaPeriodo)) entity.PmPeriFechaPeriodo = dr.GetDateTime(iPmPeriFechaPeriodo);

            int iPmperifecini = dr.GetOrdinal(this.Pmperifecini);
            if (!dr.IsDBNull(iPmperifecini)) entity.Pmperifecini = dr.GetDateTime(iPmperifecini);

            int iPmperifecinimes = dr.GetOrdinal(this.Pmperifecinimes);
            if (!dr.IsDBNull(iPmperifecinimes)) entity.Pmperifecinimes = dr.GetDateTime(iPmperifecinimes);

            int iPmperinumsem = dr.GetOrdinal(this.Pmperinumsem);
            if (!dr.IsDBNull(iPmperinumsem)) entity.Pmperinumsem = Convert.ToInt32(dr.GetValue(iPmperinumsem));

            return entity;
        }

        #region Mapeo de Campos

        public string PmPeriCodi = "PMPERICODI";
        public string PmPeriNombre = "PMPERINOMBRE";
        public string PmPeriAniOMes = "PMPERIANIOMES";
        public string PmPeriTipo = "PMPERITIPO";
        public string PmPeriEstado = "PMPERIESTADO";
        public string PmPeriUsuCreacion = "PMPERIUSUCREACION";
        public string PmPeriFecCreacion = "PMPERIFECCREACION";
        public string PmPeriUsuModificacion = "PMPERIUSUMODIFICACION";
        public string PmPeriFecModificacion = "PMPERIFECMODIFICACION";
        public string PmPeriFechaPeriodo = "PMPERIFECHAPERIODO";

        public string PmPeriFecIniMantAnual = "PMPERIFECINIMANTANUAL";
        public string PmPeriFecFinMantAnual = "PMPERIFECFINMANTANUAL";
        public string PmPeriFecIniMantMensual = "PMPERIFECINIMANTMENSUAL";
        public string PmPeriFecFinMantMensual = "PMPERIFECFINMANTMENSUAL";

        public string Pmperifecini = "PMPERIFECINI";
        public string Pmperifecinimes = "PMPERIFECINIMES";
        public string Pmperinumsem = "PMPERINUMSEM";

        #endregion


        public string SqlUpdateFechasMantenimiento
        {
            get { return base.GetSqlXml("updateFechasMantenimiento"); }
        }

        public string SqlUpdateEstadoBaja
        {
            get { return GetSqlXml("UpdateEstadoBaja"); }
        }
    }
}
