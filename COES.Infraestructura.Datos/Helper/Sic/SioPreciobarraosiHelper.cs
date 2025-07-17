using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{

    /// <summary>
    /// Clase que contiene el mapeo de la tabla SIO_PRECIOBARRAOSI
    /// </summary>
    public class SioPreciobarraosiHelper : HelperBase
    {
        public SioPreciobarraosiHelper()
            : base(Consultas.SioPreciobarraosiSql)
        {
        }

        public SioPreciobarraosiDTO Create(IDataReader dr)
        {
            SioPreciobarraosiDTO entity = new SioPreciobarraosiDTO();

            int iPbosicodi = dr.GetOrdinal(this.Pbosicodi);
            if (!dr.IsDBNull(iPbosicodi)) entity.Pbosicodi = Convert.ToInt32(dr.GetValue(iPbosicodi));

            int iPbosiperiodo = dr.GetOrdinal(this.Pbosiperiodo);
            if (!dr.IsDBNull(iPbosiperiodo)) entity.Pbosiperiodo = dr.GetDateTime(iPbosiperiodo);

            int iPbosivalor1fp = dr.GetOrdinal(this.Pbosivalor1fp);
            if (!dr.IsDBNull(iPbosivalor1fp)) entity.Pbosivalor1fp = dr.GetDecimal(iPbosivalor1fp);

            int iPbosivalor2fp = dr.GetOrdinal(this.Pbosivalor2fp);
            if (!dr.IsDBNull(iPbosivalor2fp)) entity.Pbosivalor2fp = dr.GetDecimal(iPbosivalor2fp);

            int iPbosivalor1p = dr.GetOrdinal(this.Pbosivalor1p);
            if (!dr.IsDBNull(iPbosivalor1p)) entity.Pbosivalor1p = dr.GetDecimal(iPbosivalor1p);

            int iPbosivalor2p = dr.GetOrdinal(this.Pbosivalor2p);
            if (!dr.IsDBNull(iPbosivalor2p)) entity.Pbosivalor2p = dr.GetDecimal(iPbosivalor2p);

            int iPbosiarchivo1 = dr.GetOrdinal(this.Pbosiarchivo1);
            if (!dr.IsDBNull(iPbosiarchivo1)) entity.Pbosiarchivo1 = dr.GetString(iPbosiarchivo1);

            int iPbosiarchivo2 = dr.GetOrdinal(this.Pbosiarchivo2);
            if (!dr.IsDBNull(iPbosiarchivo2)) entity.Pbosiarchivo2 = dr.GetString(iPbosiarchivo2);

            int iPbosifecha = dr.GetOrdinal(this.Pbosifecha);
            if (!dr.IsDBNull(iPbosifecha)) entity.Pbosifecha = dr.GetDateTime(iPbosifecha);

            int iPbosiusuario = dr.GetOrdinal(this.Pbosiusuario);
            if (!dr.IsDBNull(iPbosiusuario)) entity.Pbosiusuario = dr.GetString(iPbosiusuario);

            return entity;
        }


        #region Mapeo de Campos

        public string Pbosicodi = "PBOSICODI";
        public string Pbosiperiodo = "PBOSIPERIODO";
        public string Pbosivalor1fp = "PBOSIVALOR1FP";
        public string Pbosivalor2fp = "PBOSIVALOR2FP";
        public string Pbosivalor1p = "PBOSIVALOR1P";
        public string Pbosivalor2p = "PBOSIVALOR2P";
        public string Pbosiarchivo1 = "PBOSIARCHIVO1";
        public string Pbosiarchivo2 = "PBOSIARCHIVO2";
        public string Pbosifecha = "PBOSIFECCREACION";
        public string Pbosiusuario = "PBOSIUSUCREACION";

        #endregion
    }
}
