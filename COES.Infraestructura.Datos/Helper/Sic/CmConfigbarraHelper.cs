using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_CONFIGBARRA
    /// </summary>
    public class CmConfigbarraHelper : HelperBase
    {
        public CmConfigbarraHelper(): base(Consultas.CmConfigbarraSql)
        {
        }

        public CmConfigbarraDTO Create(IDataReader dr)
        {
            CmConfigbarraDTO entity = new CmConfigbarraDTO();

            int iCnfbarcodi = dr.GetOrdinal(this.Cnfbarcodi);
            if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));

            int iCnfbarnodo = dr.GetOrdinal(this.Cnfbarnodo);
            if (!dr.IsDBNull(iCnfbarnodo)) entity.Cnfbarnodo = dr.GetString(iCnfbarnodo);

            int iCnfbarnombre = dr.GetOrdinal(this.Cnfbarnombre);
            if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = dr.GetString(iCnfbarnombre);

            int iCnfbarcoorx = dr.GetOrdinal(this.Cnfbarcoorx);
            if (!dr.IsDBNull(iCnfbarcoorx)) entity.Cnfbarcoorx = dr.GetString(iCnfbarcoorx);

            int iCnfbarcoory = dr.GetOrdinal(this.Cnfbarcoory);
            if (!dr.IsDBNull(iCnfbarcoory)) entity.Cnfbarcoory = dr.GetString(iCnfbarcoory);

            int iCnfbarestado = dr.GetOrdinal(this.Cnfbarestado);
            if (!dr.IsDBNull(iCnfbarestado)) entity.Cnfbarestado = dr.GetString(iCnfbarestado);

            int iCnfbarindpublicacion = dr.GetOrdinal(this.Cnfbarindpublicacion);
            if (!dr.IsDBNull(iCnfbarindpublicacion)) entity.Cnfbarindpublicacion = dr.GetString(iCnfbarindpublicacion);

            int iCnfbarusucreacion = dr.GetOrdinal(this.Cnfbarusucreacion);
            if (!dr.IsDBNull(iCnfbarusucreacion)) entity.Cnfbarusucreacion = dr.GetString(iCnfbarusucreacion);

            int iCnfbarfeccreacion = dr.GetOrdinal(this.Cnfbarfeccreacion);
            if (!dr.IsDBNull(iCnfbarfeccreacion)) entity.Cnfbarfeccreacion = dr.GetDateTime(iCnfbarfeccreacion);

            int iCnfbarusumodificacion = dr.GetOrdinal(this.Cnfbarusumodificacion);
            if (!dr.IsDBNull(iCnfbarusumodificacion)) entity.Cnfbarusumodificacion = dr.GetString(iCnfbarusumodificacion);

            int iCnfbarfecmodificacion = dr.GetOrdinal(this.Cnfbarfecmodificacion);
            if (!dr.IsDBNull(iCnfbarfecmodificacion)) entity.Cnfbarfecmodificacion = dr.GetDateTime(iCnfbarfecmodificacion);

            int iCnfbardefecto = dr.GetOrdinal(this.Cnfbardefecto);
            if (!dr.IsDBNull(iCnfbardefecto)) entity.Cnfbardefecto = dr.GetString(iCnfbardefecto);

            int iCnfbarnombncp = dr.GetOrdinal(this.Cnfbarnombncp);
            if (!dr.IsDBNull(iCnfbarnombncp)) entity.Cnfbarnombncp = dr.GetString(iCnfbarnombncp);

            int iCnfbarnomtna = dr.GetOrdinal(this.Cnfbarnomtna);
            if (!dr.IsDBNull(iCnfbarnomtna)) entity.Cnfbarnomtna = dr.GetString(iCnfbarnomtna);

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Cnfbarcodi = "CNFBARCODI";
        public string Cnfbarnodo = "CNFBARNODO";
        public string Cnfbarnombre = "CNFBARNOMBRE";
        public string Cnfbarcoorx = "CNFBARCOORX";
        public string Cnfbarcoory = "CNFBARCOORY";
        public string Cnfbarestado = "CNFBARESTADO";
        public string Cnfbarindpublicacion = "CNFBARINDPUBLICACION";
        public string Cnfbarusucreacion = "CNFBARUSUCREACION";
        public string Cnfbarfeccreacion = "CNFBARFECCREACION";
        public string Cnfbarusumodificacion = "CNFBARUSUMODIFICACION";
        public string Cnfbarfecmodificacion = "CNFBARFECMODIFICACION";
        public string Cnfbardefecto = "CNFBARDEFECTO";
        public string Cnfbarnombncp = "CNFBARNOMBNCP";
        public string Cnfbarnomtna = "CNFBARNOMTNA";

        #region Mejoras CMgN - Movisoft
        public string Canalcodi = "CANALCODI";
        public string Recurcodi = "RECURCODI";
        public string Topcodi = "TOPCODI";
        public string Recurnombre = "RECURNOMBRE";

        public string SqlValidarRegistro
        {
            get { return base.GetSqlXml("ValidarRegistro"); }
        }

        public string SqlObtenerBarrasYupana
        {
            get { return base.GetSqlXml("ObtenerBarrasYupana"); }
        }

        public string SqlValidarCodigoScada
        {
            get { return base.GetSqlXml("ValidarCodigoScada"); }
        }

        #endregion

        public string SqlUpdateCoordenada
        {
            get { return base.GetSqlXml("UpdateCoordenada"); }
        }

        #endregion
    }
}
