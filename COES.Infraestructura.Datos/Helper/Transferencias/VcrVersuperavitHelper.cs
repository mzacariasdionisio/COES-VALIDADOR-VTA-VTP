using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_VERSUPERAVIT
    /// </summary>
    public class VcrVersuperavitHelper : HelperBase
    {
        public VcrVersuperavitHelper(): base(Consultas.VcrVersuperavitSql)
        {
        }

        public VcrVersuperavitDTO Create(IDataReader dr)
        {
            VcrVersuperavitDTO entity = new VcrVersuperavitDTO();

            int iVcrvsacodi = dr.GetOrdinal(this.Vcrvsacodi);
            if (!dr.IsDBNull(iVcrvsacodi)) entity.Vcrvsacodi = Convert.ToInt32(dr.GetValue(iVcrvsacodi));

            int iVcrdsrcodi = dr.GetOrdinal(this.Vcrdsrcodi);
            if (!dr.IsDBNull(iVcrdsrcodi)) entity.Vcrdsrcodi = Convert.ToInt32(dr.GetValue(iVcrdsrcodi));

            int iVcrvsafecha = dr.GetOrdinal(this.Vcrvsafecha);
            if (!dr.IsDBNull(iVcrvsafecha)) entity.Vcrvsafecha = dr.GetDateTime(iVcrvsafecha);

            int iVcrvsahorinicio = dr.GetOrdinal(this.Vcrvsahorinicio);
            if (!dr.IsDBNull(iVcrvsahorinicio)) entity.Vcrvsahorinicio = dr.GetDateTime(iVcrvsahorinicio);

            int iVcrvsahorfinal = dr.GetOrdinal(this.Vcrvsahorfinal);
            if (!dr.IsDBNull(iVcrvsahorfinal)) entity.Vcrvsahorfinal = dr.GetDateTime(iVcrvsahorfinal);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iVcrvsasuperavit = dr.GetOrdinal(this.Vcrvsasuperavit);
            if (!dr.IsDBNull(iVcrvsasuperavit)) entity.Vcrvsasuperavit = dr.GetDecimal(iVcrvsasuperavit);

            int iVcrvsausucreacion = dr.GetOrdinal(this.Vcrvsausucreacion);
            if (!dr.IsDBNull(iVcrvsausucreacion)) entity.Vcrvsausucreacion = dr.GetString(iVcrvsausucreacion);

            int iVcrvsafeccreacion = dr.GetOrdinal(this.Vcrvsafeccreacion);
            if (!dr.IsDBNull(iVcrvsafeccreacion)) entity.Vcrvsafeccreacion = dr.GetDateTime(iVcrvsafeccreacion);

            return entity;
        }
        
        #region Mapeo de Campos

        public string Vcrvsacodi = "VCRVSACODI";
        public string Vcrdsrcodi = "VCRDSRCODI";
        public string Vcrvsafecha = "VCRVSAFECHA";
        public string Vcrvsahorinicio = "VCRVSAHORINICIO";
        public string Vcrvsahorfinal = "VCRVSAHORFINAL";
        public string Emprcodi = "EMPRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Vcrvsasuperavit = "VCRVSASUPERAVIT";
        public string Vcrvsausucreacion = "VCRVSAUSUCREACION";
        public string Vcrvsafeccreacion = "VCRVSAFECCREACION";
        //atributos adicionales
        public string EmprNombre = "EMPRNOMBRE";

        #endregion

        //Metodos de la clase
        public string SqlListDia
        {
            get { return base.GetSqlXml("ListDia"); }
        }

        //agregado el 29-04-2019
        public string SqlListDiaURS
        {
            get { return base.GetSqlXml("ListDiaURS"); }
        }
    }
}
