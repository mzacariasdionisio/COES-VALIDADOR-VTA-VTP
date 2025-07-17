using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_COMPENSACION
    /// </summary>
    public class StCompensacionHelper : HelperBase
    {
        public StCompensacionHelper()
            : base(Consultas.StCompensacionSql)
        {
        }

        public StCompensacionDTO Create(IDataReader dr)
        {
            StCompensacionDTO entity = new StCompensacionDTO();

            int iStcompcodi = dr.GetOrdinal(this.Stcompcodi);
            if (!dr.IsDBNull(iStcompcodi)) entity.Stcompcodi = Convert.ToInt32(dr.GetValue(iStcompcodi));

            int iSistrncodi = dr.GetOrdinal(this.Sistrncodi);
            if (!dr.IsDBNull(iSistrncodi)) entity.Sistrncodi = Convert.ToInt32(dr.GetValue(iSistrncodi));

            int iStcompcodelemento = dr.GetOrdinal(this.Stcompcodelemento);
            if (!dr.IsDBNull(iStcompcodelemento)) entity.Stcompcodelemento = dr.GetString(iStcompcodelemento);

            int iStcompnomelemento = dr.GetOrdinal(this.Stcompnomelemento);
            if (!dr.IsDBNull(iStcompnomelemento)) entity.Stcompnomelemento = dr.GetString(iStcompnomelemento);

            int iStcompimpcompensacion = dr.GetOrdinal(this.Stcompimpcompensacion);
            if (!dr.IsDBNull(iStcompimpcompensacion)) entity.Stcompimpcompensacion = dr.GetDecimal(iStcompimpcompensacion);

            int iBarrcodi1 = dr.GetOrdinal(this.Barrcodi1);
            if (!dr.IsDBNull(iBarrcodi1)) entity.Barrcodi1 = Convert.ToInt32(dr.GetValue(iBarrcodi1));

            int iBarrcodi2 = dr.GetOrdinal(this.Barrcodi2);
            if (!dr.IsDBNull(iBarrcodi2)) entity.Barrcodi2 = Convert.ToInt32(dr.GetValue(iBarrcodi2));

            int iSstcmpusucreacion = dr.GetOrdinal(this.Sstcmpusucreacion);
            if (!dr.IsDBNull(iSstcmpusucreacion)) entity.Sstcmpusucreacion = dr.GetString(iSstcmpusucreacion);

            int iSstcmpfeccreacion = dr.GetOrdinal(this.Sstcmpfeccreacion);
            if (!dr.IsDBNull(iSstcmpfeccreacion)) entity.Sstcmpfeccreacion = dr.GetDateTime(iSstcmpfeccreacion);

            int iSstcmpusumodificacion = dr.GetOrdinal(this.Sstcmpusumodificacion);
            if (!dr.IsDBNull(iSstcmpusumodificacion)) entity.Sstcmpusumodificacion = dr.GetString(iSstcmpusumodificacion);

            int iSstcmpfecmodificacion = dr.GetOrdinal(this.Sstcmpfecmodificacion);
            if (!dr.IsDBNull(iSstcmpfecmodificacion)) entity.Sstcmpfecmodificacion = dr.GetDateTime(iSstcmpfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Stcompcodi = "STCOMPCODI";
        public string Sistrncodi = "SISTRNCODI";
        public string Stcompcodelemento = "STCOMPCODELEMENTO";
        public string Stcompnomelemento = "STCOMPNOMELEMENTO";
        public string Stcompimpcompensacion = "STCOMPIMPCOMPENSACION";
        public string Barrcodi1 = "BARRCODI1";
        public string Barrcodi2 = "BARRCODI2";
        public string Sstcmpusucreacion = "STCOMPUSUCREACION";
        public string Sstcmpfeccreacion = "STCOMPFECCREACION";
        public string Sstcmpusumodificacion = "STCOMPUSUMODIFICACION";
        public string Sstcmpfecmodificacion = "STCOMPFECMODIFICACION";
        //Atributos de consultas
        public string Strecacodi = "STRECACODI";
        public string Barrnombre = "BARRNOMBRE";
        public string Barrnombre1 = "BARRNOMBRE1";
        public string Barrnombre2 = "BARRNOMBRE2";
        #endregion

        public string GetBySisTrans
        {
            get { return base.GetSqlXml("GetBySisTrans"); }
        }


        public string SqlListStCompensacionsPorID
        {
            get { return base.GetSqlXml("ListStCompensacionsPorID"); }
        }

        public string SqlDeleteVersion
        {
            get { return base.GetSqlXml("DeleteVersion"); }
        }



        
    }
}
