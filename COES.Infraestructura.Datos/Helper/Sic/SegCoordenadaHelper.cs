using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SEG_COORDENADA
    /// </summary>
    public class SegCoordenadaHelper : HelperBase
    {
        public SegCoordenadaHelper(): base(Consultas.SegCoordenadaSql)
        {
        }

        public SegCoordenadaDTO Create(IDataReader dr)
        {
            SegCoordenadaDTO entity = new SegCoordenadaDTO();

            int iSegcocodi = dr.GetOrdinal(this.Segcocodi);
            if (!dr.IsDBNull(iSegcocodi)) entity.Segcocodi = Convert.ToInt32(dr.GetValue(iSegcocodi));

            int iSegconro = dr.GetOrdinal(this.Segconro);
            if (!dr.IsDBNull(iSegconro)) entity.Segconro = Convert.ToInt32(dr.GetValue(iSegconro));

            int iSegcoflujo1 = dr.GetOrdinal(this.Segcoflujo1);
            if (!dr.IsDBNull(iSegcoflujo1)) entity.Segcoflujo1 = dr.GetDecimal(iSegcoflujo1);

            int iSegcoflujo2 = dr.GetOrdinal(this.Segcoflujo2);
            if (!dr.IsDBNull(iSegcoflujo2)) entity.Segcoflujo2 = dr.GetDecimal(iSegcoflujo2);

            int iSegcogener1 = dr.GetOrdinal(this.Segcogener1);
            if (!dr.IsDBNull(iSegcogener1)) entity.Segcogener1 = dr.GetDecimal(iSegcogener1);

            int iSegcogener2 = dr.GetOrdinal(this.Segcogener2);
            if (!dr.IsDBNull(iSegcogener2)) entity.Segcogener2 = dr.GetDecimal(iSegcogener2);

            int iZoncodi = dr.GetOrdinal(this.Zoncodi);
            if (!dr.IsDBNull(iZoncodi)) entity.Zoncodi = Convert.ToInt32(dr.GetValue(iZoncodi));

            int iRegcodi = dr.GetOrdinal(this.Regcodi);
            if (!dr.IsDBNull(iRegcodi)) entity.Regcodi = Convert.ToInt32(dr.GetValue(iRegcodi));

            int iSegcotipo = dr.GetOrdinal(this.Segcotipo);
            if (!dr.IsDBNull(iSegcotipo)) entity.Segcotipo = Convert.ToInt32(dr.GetValue(iSegcotipo));

            int iRegsegcodi = dr.GetOrdinal(this.Regsegcodi);
            if (!dr.IsDBNull(iRegsegcodi)) entity.Regsegcodi = Convert.ToInt32(dr.GetValue(iRegsegcodi));

            int iSegcousucreacion = dr.GetOrdinal(this.Segcousucreacion);
            if (!dr.IsDBNull(iSegcousucreacion)) entity.Segcousucreacion = dr.GetString(iSegcousucreacion);

            int iSegcofeccreacion = dr.GetOrdinal(this.Segcofeccreacion);
            if (!dr.IsDBNull(iSegcofeccreacion)) entity.Segcofeccreacion = dr.GetDateTime(iSegcofeccreacion);

            int iSegcousumodificacion = dr.GetOrdinal(this.Segcousumodificacion);
            if (!dr.IsDBNull(iSegcousumodificacion)) entity.Segcousumodificacion = dr.GetString(iSegcousumodificacion);

            int iSegcofecmodificacion = dr.GetOrdinal(this.Segcofecmodificacion);
            if (!dr.IsDBNull(iSegcofecmodificacion)) entity.Segcofecmodificacion = dr.GetDateTime(iSegcofecmodificacion);

            int iSegcoestado = dr.GetOrdinal(this.Segcoestado);
            if (!dr.IsDBNull(iSegcoestado)) entity.Segcoestado = dr.GetString(iSegcoestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Segcocodi = "SEGCOCODI";
        public string Segconro = "SEGCONRO";
        public string Segcoflujo1 = "SEGCOFLUJO1";
        public string Segcoflujo2 = "SEGCOFLUJO2";
        public string Segcogener1 = "SEGCOGENER1";
        public string Segcogener2 = "SEGCOGENER2";
        public string Zoncodi = "ZONCODI";
        public string Regcodi = "REGCODI";
        public string Segcotipo = "SEGCOTIPO";
        public string Regsegcodi = "REGSEGCODI";
        public string Segcousucreacion = "SEGCOUSUCREACION";
        public string Segcofeccreacion = "SEGCOFECCREACION";
        public string Segcousumodificacion = "SEGCOUSUMODIFICACION";
        public string Segcofecmodificacion = "SEGCOFECMODIFICACION";
        public string Segcoestado = "SEGCOESTADO";
        public string Totalrestriccion = "Totalrestriccion";
        

        public string Zonnombre = "ZONNOMBRE";
        #endregion

        public string SqlTotalrestriccion
        {
            get { return GetSqlXml("Totalrestriccion"); }
        }
        
    }
}
