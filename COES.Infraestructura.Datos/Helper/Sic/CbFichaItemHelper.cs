using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_FICHA_ITEM
    /// </summary>
    public class CbFichaItemHelper : HelperBase
    {
        public CbFichaItemHelper() : base(Consultas.CbFichaItemSql)
        {
        }

        public CbFichaItemDTO Create(IDataReader dr)
        {
            CbFichaItemDTO entity = new CbFichaItemDTO();

            int iCbftcodi = dr.GetOrdinal(this.Cbftcodi);
            if (!dr.IsDBNull(iCbftcodi)) entity.Cbftcodi = Convert.ToInt32(dr.GetValue(iCbftcodi));

            int iCbftitcodi = dr.GetOrdinal(this.Cbftitcodi);
            if (!dr.IsDBNull(iCbftitcodi)) entity.Cbftitcodi = Convert.ToInt32(dr.GetValue(iCbftitcodi));

            int iCcombcodi = dr.GetOrdinal(this.Ccombcodi);
            if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));

            int iCbftitesseccion = dr.GetOrdinal(this.Cbftitesseccion);
            if (!dr.IsDBNull(iCbftitesseccion)) entity.Cbftitesseccion = dr.GetString(iCbftitesseccion);

            int iCbftitnombre = dr.GetOrdinal(this.Cbftitnombre);
            if (!dr.IsDBNull(iCbftitnombre)) entity.Cbftitnombre = dr.GetString(iCbftitnombre);

            int iCbftitnumeral = dr.GetOrdinal(this.Cbftitnumeral);
            if (!dr.IsDBNull(iCbftitnumeral)) entity.Cbftitnumeral = dr.GetString(iCbftitnumeral);

            int iCbftitformula = dr.GetOrdinal(this.Cbftitformula);
            if (!dr.IsDBNull(iCbftitformula)) entity.Cbftitformula = dr.GetString(iCbftitformula);

            int iCbftitinstructivo = dr.GetOrdinal(this.Cbftitinstructivo);
            if (!dr.IsDBNull(iCbftitinstructivo)) entity.Cbftitinstructivo = dr.GetString(iCbftitinstructivo);

            int iCbftittipodato = dr.GetOrdinal(this.Cbftittipodato);
            if (!dr.IsDBNull(iCbftittipodato)) entity.Cbftittipodato = dr.GetString(iCbftittipodato);

            int iCbftitabrev = dr.GetOrdinal(this.Cbftitabrev);
            if (!dr.IsDBNull(iCbftitabrev)) entity.Cbftitabrev = dr.GetString(iCbftitabrev);

            int iCbftitconfidencial = dr.GetOrdinal(this.Cbftitconfidencial);
            if (!dr.IsDBNull(iCbftitconfidencial)) entity.Cbftitconfidencial = dr.GetString(iCbftitconfidencial);

            int iCbftittipocelda = dr.GetOrdinal(this.Cbftittipocelda);
            if (!dr.IsDBNull(iCbftittipocelda)) entity.Cbftittipocelda = dr.GetString(iCbftittipocelda);

            int iCbftitceldatipo1 = dr.GetOrdinal(this.Cbftitceldatipo1);
            if (!dr.IsDBNull(iCbftitceldatipo1)) entity.Cbftitceldatipo1 = dr.GetString(iCbftitceldatipo1);

            int iCbftitceldatipo2 = dr.GetOrdinal(this.Cbftitceldatipo2);
            if (!dr.IsDBNull(iCbftitceldatipo2)) entity.Cbftitceldatipo2 = dr.GetString(iCbftitceldatipo2);

            int iCbftitceldatipo3 = dr.GetOrdinal(this.Cbftitceldatipo3);
            if (!dr.IsDBNull(iCbftitceldatipo3)) entity.Cbftitceldatipo3 = dr.GetString(iCbftitceldatipo3);

            int iCbftitceldatipo4 = dr.GetOrdinal(this.Cbftitceldatipo4);
            if (!dr.IsDBNull(iCbftitceldatipo4)) entity.Cbftitceldatipo4 = dr.GetString(iCbftitceldatipo4);

            int iCbftitoperacion = dr.GetOrdinal(this.Cbftitoperacion);
            if (!dr.IsDBNull(iCbftitoperacion)) entity.Cbftitoperacion = dr.GetString(iCbftitoperacion);

            int iCbftitactivo = dr.GetOrdinal(this.Cbftitactivo);
            if (!dr.IsDBNull(iCbftitactivo)) entity.Cbftitactivo = Convert.ToInt32(dr.GetValue(iCbftitactivo));

            int iCbftitusucreacion = dr.GetOrdinal(this.Cbftitusucreacion);
            if (!dr.IsDBNull(iCbftitusucreacion)) entity.Cbftitusucreacion = dr.GetString(iCbftitusucreacion);

            int iCbftitfeccreacion = dr.GetOrdinal(this.Cbftitfeccreacion);
            if (!dr.IsDBNull(iCbftitfeccreacion)) entity.Cbftitfeccreacion = dr.GetDateTime(iCbftitfeccreacion);

            int iCbftitusumodificacion = dr.GetOrdinal(this.Cbftitusumodificacion);
            if (!dr.IsDBNull(iCbftitusumodificacion)) entity.Cbftitusumodificacion = dr.GetString(iCbftitusumodificacion);

            int iCbftitfecmodificacion = dr.GetOrdinal(this.Cbftitfecmodificacion);
            if (!dr.IsDBNull(iCbftitfecmodificacion)) entity.Cbftitfecmodificacion = dr.GetDateTime(iCbftitfecmodificacion);

            int iCbftitcnp0 = dr.GetOrdinal(this.Cbftitcnp0);
            if (!dr.IsDBNull(iCbftitcnp0)) entity.Cbftitcnp0 = Convert.ToInt32(dr.GetValue(iCbftitcnp0));

            int iCbftitcnp1 = dr.GetOrdinal(this.Cbftitcnp1);
            if (!dr.IsDBNull(iCbftitcnp1)) entity.Cbftitcnp1 = Convert.ToInt32(dr.GetValue(iCbftitcnp1));

            int iCbftitcnp2 = dr.GetOrdinal(this.Cbftitcnp2);
            if (!dr.IsDBNull(iCbftitcnp2)) entity.Cbftitcnp2 = Convert.ToInt32(dr.GetValue(iCbftitcnp2));

            int iCbftitcnp3 = dr.GetOrdinal(this.Cbftitcnp3);
            if (!dr.IsDBNull(iCbftitcnp3)) entity.Cbftitcnp3 = Convert.ToInt32(dr.GetValue(iCbftitcnp3));

            int iCbftitcnp4 = dr.GetOrdinal(this.Cbftitcnp4);
            if (!dr.IsDBNull(iCbftitcnp4)) entity.Cbftitcnp4 = Convert.ToInt32(dr.GetValue(iCbftitcnp4));

            int iCbftitcnp5 = dr.GetOrdinal(this.Cbftitcnp5);
            if (!dr.IsDBNull(iCbftitcnp5)) entity.Cbftitcnp5 = Convert.ToInt32(dr.GetValue(iCbftitcnp5));

            int iCbftitcnp6 = dr.GetOrdinal(this.Cbftitcnp6);
            if (!dr.IsDBNull(iCbftitcnp6)) entity.Cbftitcnp6 = Convert.ToInt32(dr.GetValue(iCbftitcnp6));

            return entity;
        }

        #region Mapeo de Campos

        public string Cbftcodi = "CBFTCODI";
        public string Cbftitcodi = "CBFTITCODI";
        public string Ccombcodi = "CCOMBCODI";
        public string Cbftitesseccion = "CBFTITESSECCION";
        public string Cbftitnombre = "CBFTITNOMBRE";
        public string Cbftitnumeral = "CBFTITNUMERAL";
        public string Cbftitformula = "CBFTITFORMULA";
        public string Cbftitinstructivo = "CBFTITINSTRUCTIVO";
        public string Cbftittipodato = "CBFTITTIPODATO";
        public string Cbftitabrev = "CBFTITABREV";
        public string Cbftitconfidencial = "CBFTITCONFIDENCIAL";
        public string Cbftittipocelda = "CBFTITTIPOCELDA";
        public string Cbftitceldatipo1 = "CBFTITCELDATIPO1";
        public string Cbftitceldatipo2 = "CBFTITCELDATIPO2";
        public string Cbftitceldatipo3 = "CBFTITCELDATIPO3";
        public string Cbftitceldatipo4 = "CBFTITCELDATIPO4";
        public string Cbftitcnp0 = "CBFTITCNP0";
        public string Cbftitcnp1 = "CBFTITCNP1";
        public string Cbftitcnp2 = "CBFTITCNP2";
        public string Cbftitcnp3 = "CBFTITCNP3";
        public string Cbftitcnp4 = "CBFTITCNP4";
        public string Cbftitcnp5 = "CBFTITCNP5";
        public string Cbftitcnp6 = "CBFTITCNP6";
        public string Cbftitoperacion = "CBFTITOPERACION";
        public string Cbftitactivo = "CBFTITACTIVO";
        public string Cbftitusucreacion = "CBFTITUSUCREACION";
        public string Cbftitfeccreacion = "CBFTITFECCREACION";
        public string Cbftitusumodificacion = "CBFTITUSUMODIFICACION";
        public string Cbftitfecmodificacion = "CBFTITFECMODIFICACION";

        public string Ccombnombre = "CCOMBNOMBRE";
        public string Ccombunidad = "CCOMBUNIDAD";

        #endregion
    }
}
