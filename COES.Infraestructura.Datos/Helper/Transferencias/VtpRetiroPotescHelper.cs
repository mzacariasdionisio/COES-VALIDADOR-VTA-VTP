using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTP_RETIRO_POTESC
    /// </summary>
    public class VtpRetiroPotescHelper : HelperBase
    {
        public VtpRetiroPotescHelper(): base(Consultas.VtpRetiroPotescSql)
        {
        }

        public VtpRetiroPotescDTO Create(IDataReader dr)
        {
            VtpRetiroPotescDTO entity = new VtpRetiroPotescDTO();

            int iRpsccodi = dr.GetOrdinal(this.Rpsccodi);
            if (!dr.IsDBNull(iRpsccodi)) entity.Rpsccodi = Convert.ToInt32(dr.GetValue(iRpsccodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iRpsctipousuario = dr.GetOrdinal(this.Rpsctipousuario);
            if (!dr.IsDBNull(iRpsctipousuario)) entity.Rpsctipousuario = dr.GetString(iRpsctipousuario);

            int iRpsccalidad = dr.GetOrdinal(this.Rpsccalidad);
            if (!dr.IsDBNull(iRpsccalidad)) entity.Rpsccalidad = dr.GetString(iRpsccalidad);

            int iRpscprecioppb = dr.GetOrdinal(this.Rpscprecioppb);
            if (!dr.IsDBNull(iRpscprecioppb)) entity.Rpscprecioppb = dr.GetDecimal(iRpscprecioppb);

            int iRpscpreciopote = dr.GetOrdinal(this.Rpscpreciopote);
            if (!dr.IsDBNull(iRpscpreciopote)) entity.Rpscpreciopote = dr.GetDecimal(iRpscpreciopote);

            int iRpscpoteegreso = dr.GetOrdinal(this.Rpscpoteegreso);
            if (!dr.IsDBNull(iRpscpoteegreso)) entity.Rpscpoteegreso = dr.GetDecimal(iRpscpoteegreso);

            int iRpscpeajeunitario = dr.GetOrdinal(this.Rpscpeajeunitario);
            if (!dr.IsDBNull(iRpscpeajeunitario)) entity.Rpscpeajeunitario = dr.GetDecimal(iRpscpeajeunitario);

            int iBarrcodifco = dr.GetOrdinal(this.Barrcodifco);
            if (!dr.IsDBNull(iBarrcodifco)) entity.Barrcodifco = Convert.ToInt32(dr.GetValue(iBarrcodifco));

            int iRpscpoteactiva = dr.GetOrdinal(this.Rpscpoteactiva);
            if (!dr.IsDBNull(iRpscpoteactiva)) entity.Rpscpoteactiva = dr.GetDecimal(iRpscpoteactiva);

            int iRpscpotereactiva = dr.GetOrdinal(this.Rpscpotereactiva);
            if (!dr.IsDBNull(iRpscpotereactiva)) entity.Rpscpotereactiva = dr.GetDecimal(iRpscpotereactiva);

            int iRpscusucreacion = dr.GetOrdinal(this.Rpscusucreacion);
            if (!dr.IsDBNull(iRpscusucreacion)) entity.Rpscusucreacion = dr.GetString(iRpscusucreacion);

            int iRpscfeccreacion = dr.GetOrdinal(this.Rpscfeccreacion);
            if (!dr.IsDBNull(iRpscfeccreacion)) entity.Rpscfeccreacion = dr.GetDateTime(iRpscfeccreacion);

            int iRpscusumodificacion = dr.GetOrdinal(this.Rpscusumodificacion);
            if (!dr.IsDBNull(iRpscusumodificacion)) entity.Rpscusumodificacion = dr.GetString(iRpscusumodificacion);

            int iRpscfecmodificacion = dr.GetOrdinal(this.Rpscfecmodificacion);
            if (!dr.IsDBNull(iRpscfecmodificacion)) entity.Rpscfecmodificacion = dr.GetDateTime(iRpscfecmodificacion);

            int iRpcCodiVTP = dr.GetOrdinal(this.RpcCodiVTP);
            if (!dr.IsDBNull(iRpcCodiVTP)) entity.RpsCodiVTP = dr.GetString(iRpcCodiVTP);


            return entity;
        }


        #region Mapeo de Campos

        public string Rpsccodi = "RPSCCODI";

        public string RpcCodiVTP = "RPSCODIVTP";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Barrcodi = "BARRCODI";
        public string Rpsctipousuario = "RPSCTIPOUSUARIO";
        public string Rpsccalidad = "RPSCCALIDAD";
        public string Rpscprecioppb = "RPSCPRECIOPPB";
        public string Rpscpreciopote = "RPSCPRECIOPOTE";
        public string Rpscpoteegreso = "RPSCPOTEEGRESO";
        public string Rpscpeajeunitario = "RPSCPEAJEUNITARIO";
        public string Barrcodifco = "BARRCODIFCO";
        public string Rpscpoteactiva = "RPSCPOTEACTIVA";
        public string Rpscpotereactiva = "RPSCPOTEREACTIVA";
        public string Rpscusucreacion = "RPSCUSUCREACION";
        public string Rpscfeccreacion = "RPSCFECCREACION";
        public string Rpscusumodificacion = "RPSCUSUMODIFICACION";
        public string Rpscfecmodificacion = "RPSCFECMODIFICACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb = "EMPRNOMB";
        public string Barrnombre = "BARRNOMBRE";
        public string Barrnombrefco = "BARRNOMBREFCO";
        #endregion

        public string SqlListView 
        {
            get { return base.GetSqlXml("ListView"); }
        }

        public string SqlListByEmpresa
        {
            get { return base.GetSqlXml("ListByEmpresa"); }
        }
    }
}
