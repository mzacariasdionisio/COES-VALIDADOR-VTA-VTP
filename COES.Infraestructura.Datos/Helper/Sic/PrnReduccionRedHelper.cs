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
    public class PrnReduccionRedHelper : HelperBase
    {

        public PrnReduccionRedHelper() : base(Consultas.PrnReduccionRedSql)
        {
        }

        public PrnReduccionRedDTO Create(IDataReader dr)
        {
            PrnReduccionRedDTO entity = new PrnReduccionRedDTO();

            int iPrnredcodi = dr.GetOrdinal(this.Prnredcodi);
            if (!dr.IsDBNull(iPrnredcodi)) entity.Prnredcodi = Convert.ToInt32(dr.GetValue(iPrnredcodi));

            int iPrnvercodi = dr.GetOrdinal(this.Prnvercodi);
            if (!dr.IsDBNull(iPrnvercodi)) entity.Prnvercodi = Convert.ToInt32(dr.GetValue(iPrnvercodi));

            int iPrnredbarracp = dr.GetOrdinal(this.Prnredbarracp);
            if (!dr.IsDBNull(iPrnredbarracp)) entity.Prnredbarracp = Convert.ToInt32(dr.GetValue(iPrnredbarracp));

            int iPrnredbarrapm = dr.GetOrdinal(this.Prnredbarrapm);
            if (!dr.IsDBNull(iPrnredbarrapm)) entity.Prnredbarrapm = Convert.ToInt32(dr.GetValue(iPrnredbarrapm));

            int iPrnredgauss = dr.GetOrdinal(this.Prnredgauss);
            if (!dr.IsDBNull(iPrnredgauss)) entity.Prnredgauss = dr.GetDecimal(iPrnredgauss);

            int iPrnredperdida = dr.GetOrdinal(this.Prnredperdida);
            if (!dr.IsDBNull(iPrnredperdida)) entity.Prnredperdida = dr.GetDecimal(iPrnredperdida);

            int iPrnredfecha = dr.GetOrdinal(this.Prnredfecha);
            if (!dr.IsDBNull(iPrnredfecha)) entity.Prnredfecha = dr.GetDateTime(iPrnredfecha);

            int iPrnredusucreacion = dr.GetOrdinal(this.Prnredusucreacion);
            if (!dr.IsDBNull(iPrnredusucreacion)) entity.Prnredusucreacion = dr.GetString(iPrnredusucreacion);

            int iPrnredfeccreacion = dr.GetOrdinal(this.Prnredfeccreacion);
            if (!dr.IsDBNull(iPrnredfeccreacion)) entity.Prnredfeccreacion = dr.GetDateTime(iPrnredfeccreacion);

            int iPrnredusumodificacion = dr.GetOrdinal(this.Prnredusumodificacion);
            if (!dr.IsDBNull(iPrnredusumodificacion)) entity.Prnredusumodificacion = dr.GetString(iPrnredusumodificacion);

            int iPrnredfecmodificacion = dr.GetOrdinal(this.Prnredfecmodificacion);
            if (!dr.IsDBNull(iPrnredfecmodificacion)) entity.Prnredfecmodificacion = dr.GetDateTime(iPrnredfecmodificacion);

            int iPrnrednombre = dr.GetOrdinal(this.Prnrednombre);
            if (!dr.IsDBNull(iPrnrednombre)) entity.Prnrednombre = dr.GetString(iPrnrednombre);
            
            int iPrnredtipo = dr.GetOrdinal(this.Prnredtipo);
            if (!dr.IsDBNull(iPrnredtipo)) entity.Prnredtipo = dr.GetString(iPrnredtipo);

            return entity;
        }

        #region Mapeo de los campos

        public string Prnredcodi = "PRNREDCODI";
        public string Prnvercodi = "PRNVERCODI";
        public string Prnredbarracp = "PRNREDBARRACP";
        public string Prnredbarrapm = "PRNREDBARRAPM";
        public string Prnredgauss = "PRNREDGAUSS";
        public string Prnredperdida = "PRNREDPERDIDA";
        public string Prnredfecha = "PRNREDFECHA";
        public string Prnredusucreacion = "PRNREDUSUCREACION";
        public string Prnredfeccreacion = "PRNREDFECCREACION";
        public string Prnredusumodificacion = "PRNREDUSUMODIFICACION";
        public string Prnredfecmodificacion = "PRNREDFECMODIFICACION";
        public string Prnrednombre = "PRNREDNOMBRE";
        public string Prnredtipo = "PRNREDTIPO";

        //Adicionales
        public string Nombrecp = "NOMBRECP";
        public string Nombrepm = "NOMBREPM";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Nombre = "NOMBRE";
        public string Ptomedibarranomb = "PTOMEDIBARRANOMB";
        public string Grupocodibarra = "GRUPOCODIBARRA"; 
        #endregion


        public string SqlListByNombre
        {
            get { return base.GetSqlXml("ListByNombre"); }
        }
        public string SqlListByCPNivel
        {
            get { return base.GetSqlXml("ListByCPNivel"); }
        }
        public string SqlDeleteReduccionRed
        {
            get { return base.GetSqlXml("DeleteReduccionRed"); }
        }
        public string SqlGetModeloActivo
        {
            get { return base.GetSqlXml("GetModeloActivo"); }
        }

        public string SqlDeleteReduccionRedBarraVersion
        {
            get { return base.GetSqlXml("DeleteReduccionRedBarraVersion"); }
        }

        public string SqlListSumaBarraGaussPM
        {
            get { return base.GetSqlXml("ListSumaBarraGaussPM"); }
        }

        public string SqlListBarraCPPorArea
        {
            get { return base.GetSqlXml("ListBarraCPPorArea"); }
        }
        
        public string SqlListPuntosAgrupacionesByBarra
        {
            get { return base.GetSqlXml("ListPuntosAgrupacionesByBarra"); }
        }
    }
}
