using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class SubCausaEventoHelper: HelperBase
    {
        public SubCausaEventoHelper()
            : base(Consultas.SubCausaEventoSql)
        {
        }

        public SubCausaEventoDTO Create(IDataReader dr)
        {
            SubCausaEventoDTO entity = new SubCausaEventoDTO();

            int iSUBCAUSACODI = dr.GetOrdinal(this.SUBCAUSACODI);
            int iCAUSAEVENCODI = dr.GetOrdinal(this.CAUSAEVENCODI);
            int iSUBCAUSADESC = dr.GetOrdinal(this.SUBCAUSADESC);
            int iSUBCAUSAABREV = dr.GetOrdinal(this.SUBCAUSAABREV);

            if (!dr.IsDBNull(iSUBCAUSACODI)) entity.SUBCAUSACODI = Convert.ToInt16(dr.GetValue(iSUBCAUSACODI));
            if (!dr.IsDBNull(iCAUSAEVENCODI)) entity.CAUSAEVENCODI = Convert.ToInt16(dr.GetValue(iCAUSAEVENCODI));
            if (!dr.IsDBNull(iSUBCAUSADESC)) entity.SUBCAUSADESC = dr.GetString(iSUBCAUSADESC);
            if (!dr.IsDBNull(iSUBCAUSAABREV)) entity.SUBCAUSAABREV = dr.GetString(iSUBCAUSAABREV);

            return entity;
        }

        public string SUBCAUSACODI = "SUBCAUSACODI";
        public string CAUSAEVENCODI = "CAUSAEVENCODI";
        public string SUBCAUSADESC = "SUBCAUSADESC";
        public string SUBCAUSAABREV = "SUBCAUSAABREV";
        public string TIPOEVENCODI = "TIPOEVENCODI";
    }
}

