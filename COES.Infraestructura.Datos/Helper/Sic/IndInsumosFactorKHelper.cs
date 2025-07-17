using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_STOCK_COMBUSTIBLE
    /// </summary>
    public class IndInsumosFactorKHelper : HelperBase
    {

        #region Mapeo de Campos
        //table
        public string Insfckcodi = "INSFCKCODI";
        public string Ipericodi = "IPERICODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodicentral = "EQUICODICENTRAL";
        public string Equicodiunidad = "EQUICODIUNIDAD";
        public string Grupocodi = "GRUPOCODI";
        public string Famcodi = "FAMCODI";
        public string Insfckfrc = "INSFCKFRC";
        public string Insfckusucreacion = "INSFCKUSUCREACION";
        public string Insfckfeccreacion = "INSFCKFECCREACION";
        public string Insfckusumodificacion = "INSFCKUSUMODIFICACION";
        public string Insfckfecmodificacion = "INSFCKFECMODIFICACION";
        public string Insfckusuultimp = "INSFCKUSUULTIMP";
        public string Insfckfecultimp = "INSFCKFECULTIMP";
        public string Insfckranfecultimp = "INSFCKRANFECULTIMP";

        //aditional
        public string Iperinombre = "IPERINOMBRE";
        public string Emprnomb = "EMPRNOMB";
        public string Equinombcentral = "EQUINOMBCENTRAL";
        public string Equinombunidad = "EQUINOMBUNIDAD";
        public string Gruponomb = "GRUPONOMB";
        public string Famnomb = "FAMNOMB";
        #endregion

        #region Constructor
        public IndInsumosFactorKHelper() : base(Consultas.IndInsumosFactorKSql)
        {
        }
        #endregion

        #region Crear datos
        public IndInsumosFactorKDTO Create(IDataReader dr)
        {
            IndInsumosFactorKDTO entity = new IndInsumosFactorKDTO();
            SetCreate(dr, entity);
            return entity;
        }

        public IndInsumosFactorKDTO CreateByCriteria(IDataReader dr)
        {
            IndInsumosFactorKDTO entity = new IndInsumosFactorKDTO();
            SetCreate(dr, entity);

            int iPerinombre = dr.GetOrdinal(this.Iperinombre);
            if (!dr.IsDBNull(iPerinombre)) entity.Iperinombre = dr.GetString(iPerinombre);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iEquinombcentral = dr.GetOrdinal(this.Equinombcentral);
            if (!dr.IsDBNull(iEquinombcentral)) entity.Equinombcentral = dr.GetString(iEquinombcentral);

            int iEquinombunidad = dr.GetOrdinal(this.Equinombunidad);
            if (!dr.IsDBNull(iEquinombunidad)) entity.Equinombunidad = dr.GetString(iEquinombunidad);

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iFamnomb = dr.GetOrdinal(this.Famnomb);
            if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

            return entity;
        }

        private void SetCreate(IDataReader dr, IndInsumosFactorKDTO entity) 
        {
            int iInsfckcodi = dr.GetOrdinal(this.Insfckcodi);
            if (!dr.IsDBNull(iInsfckcodi)) entity.Insfckcodi = Convert.ToInt32(dr.GetValue(iInsfckcodi));

            int iIpericodi = dr.GetOrdinal(this.Ipericodi);
            if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodicentral = dr.GetOrdinal(this.Equicodicentral);
            if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

            int iEquicodiunidad = dr.GetOrdinal(this.Equicodiunidad);
            if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iInsfckfrc = dr.GetOrdinal(this.Insfckfrc);
            if (!dr.IsDBNull(iInsfckfrc)) entity.Insfckfrc = Convert.ToDecimal(dr.GetValue(iInsfckfrc));

            int iInsfckusucreacion = dr.GetOrdinal(this.Insfckusucreacion);
            if (!dr.IsDBNull(iInsfckusucreacion)) entity.Insfckusucreacion = dr.GetString(iInsfckusucreacion);

            int iInsfckfeccreacion = dr.GetOrdinal(this.Insfckfeccreacion);
            if (!dr.IsDBNull(iInsfckfeccreacion)) entity.Insfckfeccreacion = dr.GetDateTime(iInsfckfeccreacion);

            int iInsfckusumodificacion = dr.GetOrdinal(this.Insfckusumodificacion);
            if (!dr.IsDBNull(iInsfckusumodificacion)) entity.Insfckusumodificacion = dr.GetString(iInsfckusumodificacion);

            int iInsfckfecmodificacion = dr.GetOrdinal(this.Insfckfecmodificacion);
            if (!dr.IsDBNull(iInsfckfecmodificacion)) entity.Insfckfecmodificacion = dr.GetDateTime(iInsfckfecmodificacion);

            int iInsfckusuultimp = dr.GetOrdinal(this.Insfckusuultimp);
            if (!dr.IsDBNull(iInsfckusuultimp)) entity.Insfckusuultimp = dr.GetString(iInsfckusuultimp);

            int iInsfckfecultimp = dr.GetOrdinal(this.Insfckfecultimp);
            if (!dr.IsDBNull(iInsfckfecultimp)) entity.Insfckfecultimp = dr.GetDateTime(iInsfckfecultimp);

            int iInsfckranfecultimp = dr.GetOrdinal(this.Insfckranfecultimp);
            if (!dr.IsDBNull(iInsfckranfecultimp)) entity.Insfckranfecultimp = dr.GetString(iInsfckranfecultimp);
        }
        #endregion

        #region Consultas
        public string SqlUpdateFRC
        {
            get { return base.GetSqlXml("UpdateFRC"); }
        }

        public string SqlUpdateFRCByImport
        {
            get { return base.GetSqlXml("UpdateFRCByImport"); }
        }

        public string SqlGetByPeriodo
        {
            get { return base.GetSqlXml("GetByPeriodo"); }
        }
        #endregion
    }
}
