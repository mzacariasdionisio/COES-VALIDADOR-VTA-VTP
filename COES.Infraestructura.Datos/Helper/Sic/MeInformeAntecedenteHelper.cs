using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_INFORME_ANTECEDENTE
    /// </summary>
    public class MeInformeAntecedenteHelper : HelperBase
    {
        public MeInformeAntecedenteHelper(): base(Consultas.MeInformeAntecedenteSql)
        {
        }

        public MeInformeAntecedenteDTO Create(IDataReader dr)
        {
            MeInformeAntecedenteDTO entity = new MeInformeAntecedenteDTO();

            int iInfantcodi = dr.GetOrdinal(this.Infantcodi);
            if (!dr.IsDBNull(iInfantcodi)) entity.Infantcodi = Convert.ToInt32(dr.GetValue(iInfantcodi));

            int iInfantorden = dr.GetOrdinal(this.Infantorden);
            if (!dr.IsDBNull(iInfantorden)) entity.Infantorden = Convert.ToInt32(dr.GetValue(iInfantorden));

            int iIntantcontenido = dr.GetOrdinal(this.Intantcontenido);
            if (!dr.IsDBNull(iIntantcontenido)) entity.Intantcontenido = dr.GetString(iIntantcontenido);

            int iIntantestado = dr.GetOrdinal(this.Intantestado);
            if (!dr.IsDBNull(iIntantestado)) entity.Intantestado = dr.GetString(iIntantestado);

            int iIntantusucreacion = dr.GetOrdinal(this.Intantusucreacion);
            if (!dr.IsDBNull(iIntantusucreacion)) entity.Intantusucreacion = dr.GetString(iIntantusucreacion);

            int iIntantfeccreacion = dr.GetOrdinal(this.Intantfeccreacion);
            if (!dr.IsDBNull(iIntantfeccreacion)) entity.Intantfeccreacion = dr.GetDateTime(iIntantfeccreacion);

            int iIntantusumodificacion = dr.GetOrdinal(this.Intantusumodificacion);
            if (!dr.IsDBNull(iIntantusumodificacion)) entity.Intantusumodificacion = dr.GetString(iIntantusumodificacion);

            int iIntantfecmodificacion = dr.GetOrdinal(this.Intantfecmodificacion);
            if (!dr.IsDBNull(iIntantfecmodificacion)) entity.Intantfecmodificacion = dr.GetDateTime(iIntantfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Infantcodi = "INFANTCODI";
        public string Infantorden = "INFANTORDEN";
        public string Intantcontenido = "INTANTCONTENIDO";
        public string Intantestado = "INTANTESTADO";
        public string Intantusucreacion = "INTANTUSUCREACION";
        public string Intantfeccreacion = "INTANTFECCREACION";
        public string Intantusumodificacion = "INTANTUSUMODIFICACION";
        public string Intantfecmodificacion = "INTANTFECMODIFICACION";

        #endregion
    }
}
