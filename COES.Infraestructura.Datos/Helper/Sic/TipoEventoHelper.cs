using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class TipoEventoHelper: HelperBase
    {
        public TipoEventoHelper()
            : base(Consultas.TipoEventoSql)
        {

        }

        public TipoEventoDTO Create(IDataReader dr)
        {
            TipoEventoDTO entity = new TipoEventoDTO();

            int iTipoEvenCodi = dr.GetOrdinal(this.TIPOEVENCODI);
            if (!dr.IsDBNull(iTipoEvenCodi)) entity.TIPOEVENCODI = Convert.ToInt16(dr.GetValue(iTipoEvenCodi));

            int iTipoEvenDesc = dr.GetOrdinal(this.TIPOEVENDESC);
            if (!dr.IsDBNull(iTipoEvenDesc)) entity.TIPOEVENDESC = dr.GetString(iTipoEvenDesc);

            int iSubCausaCodi = dr.GetOrdinal(this.SUBCAUSACODI);
            if (!dr.IsDBNull(iSubCausaCodi)) entity.SUBCAUSACODI = Convert.ToInt16(dr.GetValue(iSubCausaCodi));

            int iTipoEventoAbrev = dr.GetOrdinal(this.TIPOEVENABREV);
            if (!dr.IsDBNull(iTipoEventoAbrev)) entity.TIPOEVENABREV = dr.GetString(iTipoEventoAbrev);             

            return entity;
        }

        public string  TIPOEVENCODI = "TIPOEVENCODI";
	    public string  TIPOEVENDESC = "TIPOEVENDESC";
     	public string  SUBCAUSACODI = "SUBCAUSACODI";
	    public string  TIPOEVENABREV =  "TIPOEVENABREV";
        public string CATEEVENCODI = "CATEEVENCODI";


    }
}

