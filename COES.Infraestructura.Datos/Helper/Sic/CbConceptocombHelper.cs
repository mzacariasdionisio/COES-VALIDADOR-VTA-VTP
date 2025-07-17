using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_CONCEPTOCOMB
    /// </summary>
    public class CbConceptocombHelper : HelperBase
    {
        public CbConceptocombHelper() : base(Consultas.CbConceptocombSql)
        {
        }

        public CbConceptocombDTO Create(IDataReader dr)
        {
            CbConceptocombDTO entity = new CbConceptocombDTO();

            int iCcombcodi = dr.GetOrdinal(this.Ccombcodi);
            if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));

            int iCcombnombre = dr.GetOrdinal(this.Ccombnombre);
            if (!dr.IsDBNull(iCcombnombre)) entity.Ccombnombre = dr.GetString(iCcombnombre);

            int iCcombnombreficha = dr.GetOrdinal(this.Ccombnombreficha);
            if (!dr.IsDBNull(iCcombnombreficha)) entity.Ccombnombreficha = dr.GetString(iCcombnombreficha);

            int iCcombunidad = dr.GetOrdinal(this.Ccombunidad);
            if (!dr.IsDBNull(iCcombunidad)) entity.Ccombunidad = dr.GetString(iCcombunidad);

            int iCcomborden = dr.GetOrdinal(this.Ccomborden);
            if (!dr.IsDBNull(iCcomborden)) entity.Ccomborden = Convert.ToInt32(dr.GetValue(iCcomborden));

            int iCcombabrev = dr.GetOrdinal(this.Ccombabrev);
            if (!dr.IsDBNull(iCcombabrev)) entity.Ccombabrev = dr.GetString(iCcombabrev);

            int iCcombtipo = dr.GetOrdinal(this.Ccombtipo);
            if (!dr.IsDBNull(iCcombtipo)) entity.Ccombtipo = dr.GetString(iCcombtipo);

            int iCcombreadonly = dr.GetOrdinal(this.Ccombreadonly);
            if (!dr.IsDBNull(iCcombreadonly)) entity.Ccombreadonly = dr.GetString(iCcombreadonly);

            int iCcombnumeral = dr.GetOrdinal(this.Ccombnumeral);
            if (!dr.IsDBNull(iCcombnumeral)) entity.Ccombnumeral = Convert.ToInt32(dr.GetValue(iCcombnumeral));

            int iCcombtunidad = dr.GetOrdinal(this.Ccombtunidad);
            if (!dr.IsDBNull(iCcombtunidad)) entity.Ccombtunidad = Convert.ToInt32(dr.GetValue(iCcombtunidad));

            int iCcombseparador = dr.GetOrdinal(this.Ccombseparador);
            if (!dr.IsDBNull(iCcombseparador)) entity.Ccombseparador = Convert.ToInt32(dr.GetValue(iCcombseparador));

            int iCcombnumdecimal = dr.GetOrdinal(this.Ccombnumdecimal);
            if (!dr.IsDBNull(iCcombnumdecimal)) entity.Ccombnumdecimal = Convert.ToInt32(dr.GetValue(iCcombnumdecimal));

            int iCcombtipo2 = dr.GetOrdinal(this.Ccombtipo2);
            if (!dr.IsDBNull(iCcombtipo2)) entity.Ccombtipo2 = Convert.ToInt32(dr.GetValue(iCcombtipo2));

            int iCcombunidad2 = dr.GetOrdinal(this.Ccombunidad2);
            if (!dr.IsDBNull(iCcombunidad2)) entity.Ccombunidad2 = Convert.ToInt32(dr.GetValue(iCcombunidad2));

            int iCcombestado = dr.GetOrdinal(this.Ccombestado);
            if (!dr.IsDBNull(iCcombestado)) entity.Ccombestado = Convert.ToInt32(dr.GetValue(iCcombestado));

            int iEstcomcodi = dr.GetOrdinal(this.Estcomcodi);
            if (!dr.IsDBNull(iEstcomcodi)) entity.Estcomcodi = Convert.ToInt32(dr.GetValue(iEstcomcodi));

            int iCcombobligatorio = dr.GetOrdinal(this.Ccombobligatorio);
            if (!dr.IsDBNull(iCcombobligatorio)) entity.Ccombobligatorio = dr.GetString(iCcombobligatorio);

            return entity;
        }


        #region Mapeo de Campos

        public string Ccombcodi = "CCOMBCODI";
        public string Ccombnombre = "CCOMBNOMBRE";
        public string Ccombnombreficha = "CCOMBNOMBREFICHA";
        public string Ccombunidad = "CCOMBUNIDAD";
        public string Ccomborden = "CCOMBORDEN";
        public string Ccombabrev = "CCOMBABREV";
        public string Ccombtipo = "CCOMBTIPO";
        public string Ccombreadonly = "CCOMBREADONLY";
        public string Ccombnumeral = "CCOMBNUMERAL";
        public string Ccombtunidad = "CCOMBTUNIDAD";
        public string Ccombseparador = "CCOMBSEPARADOR";
        public string Ccombnumdecimal = "CCOMBNUMDECIMAL";
        public string Ccombtipo2 = "CCOMBTIPO2";
        public string Ccombunidad2 = "CCOMBUNIDAD2";
        public string Ccombestado = "CCOMBESTADO";
        public string Estcomcodi = "ESTCOMCODI";
        public string Ccombobligatorio = "CCOMBOBLIGATORIO";

        #endregion
    }
}
