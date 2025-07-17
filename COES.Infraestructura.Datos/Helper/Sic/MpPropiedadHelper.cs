using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MP_PROPIEDAD
    /// </summary>
    public class MpPropiedadHelper : HelperBase
    {
        public MpPropiedadHelper(): base(Consultas.MpPropiedadSql)
        {
        }

        public MpPropiedadDTO Create(IDataReader dr)
        {
            MpPropiedadDTO entity = new MpPropiedadDTO();

            int iMpropcodi = dr.GetOrdinal(this.Mpropcodi);
            if (!dr.IsDBNull(iMpropcodi)) entity.Mpropcodi = Convert.ToInt32(dr.GetValue(iMpropcodi));

            int iMcatcodi = dr.GetOrdinal(this.Mcatcodi);
            if (!dr.IsDBNull(iMcatcodi)) entity.Mcatcodi = Convert.ToInt32(dr.GetValue(iMcatcodi));

            int iMpropnombre = dr.GetOrdinal(this.Mpropnombre);
            if (!dr.IsDBNull(iMpropnombre)) entity.Mpropnombre = dr.GetString(iMpropnombre);

            int iMpropabrev = dr.GetOrdinal(this.Mpropabrev);
            if (!dr.IsDBNull(iMpropabrev)) entity.Mpropabrev = dr.GetString(iMpropabrev);

            int iMpropunidad = dr.GetOrdinal(this.Mpropunidad);
            if (!dr.IsDBNull(iMpropunidad)) entity.Mpropunidad = dr.GetString(iMpropunidad);

            int iMproporden = dr.GetOrdinal(this.Mproporden);
            if (!dr.IsDBNull(iMproporden)) entity.Mproporden = Convert.ToInt32(dr.GetValue(iMproporden));

            int iMproptipo = dr.GetOrdinal(this.Mproptipo);
            if (!dr.IsDBNull(iMproptipo)) entity.Mproptipo = dr.GetString(iMproptipo);

            int iMpropcodisicoes = dr.GetOrdinal(this.Mpropcodisicoes);
            if (!dr.IsDBNull(iMpropcodisicoes)) entity.Mpropcodisicoes = Convert.ToInt32(dr.GetValue(iMpropcodisicoes));

            int iMpropcodisicoes2 = dr.GetOrdinal(this.Mpropcodisicoes2);
            if (!dr.IsDBNull(iMpropcodisicoes2)) entity.Mpropcodisicoes2 = Convert.ToInt32(dr.GetValue(iMpropcodisicoes2));

            int iMpropusumodificacion = dr.GetOrdinal(this.Mpropusumodificacion);
            if (!dr.IsDBNull(iMpropusumodificacion)) entity.Mpropusumodificacion = dr.GetString(iMpropusumodificacion);

            int iMpropfecmodificacion = dr.GetOrdinal(this.Mpropfecmodificacion);
            if (!dr.IsDBNull(iMpropfecmodificacion)) entity.Mpropfecmodificacion = dr.GetDateTime(iMpropfecmodificacion);

            int iMpropancho = dr.GetOrdinal(this.Mpropancho);
            if (!dr.IsDBNull(iMpropancho)) entity.Mpropancho = Convert.ToInt32(dr.GetValue(iMpropancho));

            int iMpropvalordefault = dr.GetOrdinal(this.Mpropvalordefault);
            if (!dr.IsDBNull(iMpropvalordefault)) entity.Mpropvalordefault = dr.GetString(iMpropvalordefault);

            int iMpropvalordefault2 = dr.GetOrdinal(this.Mpropvalordefault2);
            if (!dr.IsDBNull(iMpropvalordefault2)) entity.Mpropvalordefault2 = dr.GetString(iMpropvalordefault2);

            int iMpropprioridad = dr.GetOrdinal(this.Mpropprioridad);
            if (!dr.IsDBNull(iMpropprioridad)) entity.Mpropprioridad = Convert.ToInt32(dr.GetValue(iMpropprioridad));

            return entity;
        }


        #region Mapeo de Campos

        public string Mpropcodi = "MPROPCODI";
        public string Mcatcodi = "MCATCODI";
        public string Mpropnombre = "MPROPNOMBRE";
        public string Mpropabrev = "MPROPABREV";
        public string Mpropunidad = "MPROPUNIDAD";
        public string Mproporden = "MPROPORDEN";
        public string Mproptipo = "MPROPTIPO";
        public string Mpropcodisicoes = "MPROPCODISICOES";
        public string Mpropcodisicoes2 = "MPROPCODISICOES2";
        public string Mpropusumodificacion = "MPROPUSUMODIFICACION";
        public string Mpropfecmodificacion = "MPROPFECMODIFICACION";
        public string Mpropancho = "MPROPANCHO";
        public string Mpropvalordefault = "MPROPVALORDEFAULT";
        public string Mpropvalordefault2 = "MPROPVALORDEFAULT2";
        public string Mpropprioridad = "MPROPPRIORIDAD";

        #endregion
    }
}
