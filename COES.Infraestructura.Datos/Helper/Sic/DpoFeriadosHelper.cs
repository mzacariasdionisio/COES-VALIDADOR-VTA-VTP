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
    public class DpoFeriadosHelper : HelperBase
    {
        public DpoFeriadosHelper() : base(Consultas.DpoFeriadosSql)
        {
        }

        public DpoFeriadosDTO Create(IDataReader dr)
        {
            DpoFeriadosDTO entity = new DpoFeriadosDTO();

            int iDpofercodi = dr.GetOrdinal(this.Dpofercodi);
            if (!dr.IsDBNull(iDpofercodi)) entity.Dpofercodi = Convert.ToInt32(dr.GetValue(iDpofercodi));

            int iDpoferanio = dr.GetOrdinal(this.Dpoferanio);
            if (!dr.IsDBNull(iDpoferanio)) entity.Dpoferanio = Convert.ToInt32(dr.GetValue(iDpoferanio));

            int iDpoferfecha = dr.GetOrdinal(this.Dpoferfecha);
            if (!dr.IsDBNull(iDpoferfecha)) entity.Dpoferfecha = dr.GetDateTime(iDpoferfecha);

            int iDpoferdescripcion = dr.GetOrdinal(this.Dpoferdescripcion);
            if (!dr.IsDBNull(iDpoferdescripcion)) entity.Dpoferdescripcion = dr.GetString(iDpoferdescripcion);

            int iDpoferspl = dr.GetOrdinal(this.Dpoferspl);
            if (!dr.IsDBNull(iDpoferspl)) entity.Dpoferspl = dr.GetString(iDpoferspl);

            int iDpofersco = dr.GetOrdinal(this.Dpofersco);
            if (!dr.IsDBNull(iDpofersco)) entity.Dpofersco = dr.GetString(iDpofersco);

            int iDpoferusucreacion = dr.GetOrdinal(this.Dpoferusucreacion);
            if (!dr.IsDBNull(iDpoferusucreacion)) entity.Dpoferusucreacion = dr.GetString(iDpoferusucreacion);

            int iDpoferfeccreacion = dr.GetOrdinal(this.Dpoferfeccreacion);
            if (!dr.IsDBNull(iDpoferfeccreacion)) entity.Dpoferfeccreacion = dr.GetDateTime(iDpoferfeccreacion);

            entity.Strfecha = entity.Dpoferfecha.ToString("dd/MM/yyyy");

            return entity;
        }


        #region Mapeo de Campos

        public string Dpofercodi = "DPOFERCODI";
        public string Dpoferanio = "DPOFERANIO";
        public string Dpoferfecha = "DPOFERFECHA";
        public string Dpoferdescripcion = "DPOFERDESCRIPCION";
        public string Dpoferspl = "DPOFERSPL";
        public string Dpofersco = "DPOFERSCO";
        public string Dpoferusucreacion = "DPOFERUSUCREACION";
        public string Dpoferfeccreacion = "DPOFERFECCREACION";

        #endregion

        public string SqlGetByAnhio
        {
            get { return GetSqlXml("GetByAnhio"); }
        }
        public string SqlGetByFecha
        {
            get { return GetSqlXml("GetByFecha"); }
        }
        public string SqlUpdateById
        {
            get { return GetSqlXml("UpdateById"); }
        }
        public string SqlObtenerFeriadosSpl
        {
            get { return GetSqlXml("ObtenerFeriadosSpl"); }
        }
        public string SqlObtenerFeriadosSco
        {
            get { return GetSqlXml("ObtenerFeriadosSco"); }
        }

        public string SqlObtenerFeriadosPorAnio
        {
            get { return GetSqlXml("ObtenerFeriadosPorAnio"); }
        }
        public string SqlGetByAnioRango
        {
            get { return GetSqlXml("GetByAnioRango"); }
        }
    }
}
