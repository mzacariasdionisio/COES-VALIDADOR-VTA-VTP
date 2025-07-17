using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_EVENTO_PRODUCTO
    /// </summary>
    public class ReEventoProductoHelper : HelperBase
    {
        public ReEventoProductoHelper(): base(Consultas.ReEventoProductoSql)
        {
        }

        public ReEventoProductoDTO Create(IDataReader dr)
        {
            ReEventoProductoDTO entity = new ReEventoProductoDTO();

            int iReevprcodi = dr.GetOrdinal(this.Reevprcodi);
            if (!dr.IsDBNull(iReevprcodi)) entity.Reevprcodi = Convert.ToInt32(dr.GetValue(iReevprcodi));

            int iReevpranio = dr.GetOrdinal(this.Reevpranio);
            if (!dr.IsDBNull(iReevpranio)) entity.Reevpranio = Convert.ToInt32(dr.GetValue(iReevpranio));

            int iReevprmes = dr.GetOrdinal(this.Reevprmes);
            if (!dr.IsDBNull(iReevprmes)) entity.Reevprmes = Convert.ToInt32(dr.GetValue(iReevprmes));

            int iReevprfecinicio = dr.GetOrdinal(this.Reevprfecinicio);
            if (!dr.IsDBNull(iReevprfecinicio)) entity.Reevprfecinicio = dr.GetDateTime(iReevprfecinicio);

            int iReevprfecfin = dr.GetOrdinal(this.Reevprfecfin);
            if (!dr.IsDBNull(iReevprfecfin)) entity.Reevprfecfin = dr.GetDateTime(iReevprfecfin);

            int iReevprptoentrega = dr.GetOrdinal(this.Reevprptoentrega);
            if (!dr.IsDBNull(iReevprptoentrega)) entity.Reevprptoentrega = dr.GetString(iReevprptoentrega);

            int iReevprtension = dr.GetOrdinal(this.Reevprtension);
            if (!dr.IsDBNull(iReevprtension)) entity.Reevprtension = dr.GetDecimal(iReevprtension);

            int iReevprempr1 = dr.GetOrdinal(this.Reevprempr1);
            if (!dr.IsDBNull(iReevprempr1)) entity.Reevprempr1 = Convert.ToInt32(dr.GetValue(iReevprempr1));

            int iReevprempr2 = dr.GetOrdinal(this.Reevprempr2);
            if (!dr.IsDBNull(iReevprempr2)) entity.Reevprempr2 = Convert.ToInt32(dr.GetValue(iReevprempr2));

            int iReevprempr3 = dr.GetOrdinal(this.Reevprempr3);
            if (!dr.IsDBNull(iReevprempr3)) entity.Reevprempr3 = Convert.ToInt32(dr.GetValue(iReevprempr3));

            int iReevprporc1 = dr.GetOrdinal(this.Reevprporc1);
            if (!dr.IsDBNull(iReevprporc1)) entity.Reevprporc1 = dr.GetDecimal(iReevprporc1);

            int iReevprporc2 = dr.GetOrdinal(this.Reevprporc2);
            if (!dr.IsDBNull(iReevprporc2)) entity.Reevprporc2 = dr.GetDecimal(iReevprporc2);

            int iReevprporc3 = dr.GetOrdinal(this.Reevprporc3);
            if (!dr.IsDBNull(iReevprporc3)) entity.Reevprporc3 = dr.GetDecimal(iReevprporc3);

            int iReevprcomentario = dr.GetOrdinal(this.Reevprcomentario);
            if (!dr.IsDBNull(iReevprcomentario)) entity.Reevprcomentario = dr.GetString(iReevprcomentario);

            int iReevpracceso = dr.GetOrdinal(this.Reevpracceso);
            if (!dr.IsDBNull(iReevpracceso)) entity.Reevpracceso = dr.GetString(iReevpracceso);

            int iReevprestado = dr.GetOrdinal(this.Reevprestado);
            if (!dr.IsDBNull(iReevprestado)) entity.Reevprestado = dr.GetString(iReevprestado);

            int iReevprusucreacion = dr.GetOrdinal(this.Reevprusucreacion);
            if (!dr.IsDBNull(iReevprusucreacion)) entity.Reevprusucreacion = dr.GetString(iReevprusucreacion);

            int iReevprfeccreacion = dr.GetOrdinal(this.Reevprfeccreacion);
            if (!dr.IsDBNull(iReevprfeccreacion)) entity.Reevprfeccreacion = dr.GetDateTime(iReevprfeccreacion);

            int iReevprusumodificacion = dr.GetOrdinal(this.Reevprusumodificacion);
            if (!dr.IsDBNull(iReevprusumodificacion)) entity.Reevprusumodificacion = dr.GetString(iReevprusumodificacion);

            int iReevprfecmodificacion = dr.GetOrdinal(this.Reevprfecmodificacion);
            if (!dr.IsDBNull(iReevprfecmodificacion)) entity.Reevprfecmodificacion = dr.GetDateTime(iReevprfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reevprcodi = "REEVPRCODI";
        public string Reevpranio = "REEVPRANIO";
        public string Reevprmes = "REEVPRMES";
        public string Reevprfecinicio = "REEVPRFECINICIO";
        public string Reevprfecfin = "REEVPRFECFIN";
        public string Reevprptoentrega = "REEVPRPTOENTREGA";
        public string Reevprtension = "REEVPRTENSION";
        public string Reevprempr1 = "REEVPREMPR1";
        public string Reevprempr2 = "REEVPREMPR2";
        public string Reevprempr3 = "REEVPREMPR3";
        public string Reevprporc1 = "REEVPRPORC1";
        public string Reevprporc2 = "REEVPRPORC2";
        public string Reevprporc3 = "REEVPRPORC3";
        public string Reevprcomentario = "REEVPRCOMENTARIO";
        public string Reevpracceso = "REEVPRACCESO";
        public string Reevprestado = "REEVPRESTADO";
        public string Reevprusucreacion = "REEVPRUSUCREACION";
        public string Reevprfeccreacion = "REEVPRFECCREACION";
        public string Reevprusumodificacion = "REEVPRUSUMODIFICACION";
        public string Reevprfecmodificacion = "REEVPRFECMODIFICACION";

        public string Responsablenomb1 = "RESPONSABLENOMB1";
        public string Responsablenomb2 = "RESPONSABLENOMB2";
        public string Responsablenomb3 = "RESPONSABLENOMB3";
        public string Suministrador = "SUMINISTRADOR";
        public string Estadocarga = "ESTADOCARGA";

        #endregion

        public string SqlObtenerEventosPorSuministrador
        {
            get { return base.GetSqlXml("ObtenerEventosPorSuministrador"); }
        }
    }
}
