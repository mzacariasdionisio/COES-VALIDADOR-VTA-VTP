using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;


namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RNT_EMPRESA_REGPTOENTREGA
    /// </summary>
    public class RntEmpresaRegptoentregaHelper : HelperBase
    {

        public RntEmpresaRegptoentregaHelper()
            : base(Consultas.RntEmpresaRegptoentregaSql)
        {

        }

        public RntEmpresaRegptoentregaDTO Create(IDataReader dr)
        {          
            RntEmpresaRegptoentregaDTO entity = new RntEmpresaRegptoentregaDTO();

            int iEmpgencodi = dr.GetOrdinal(this.Empgencodi);
            if (!dr.IsDBNull(iEmpgencodi)) entity.EmpGenCodi = Convert.ToInt32(dr.GetValue(iEmpgencodi));           

            int iEmprpenombre = dr.GetOrdinal(this.Emprpenombre);
            if (!dr.IsDBNull(iEmprpenombre)) entity.EmpRpeNombre = dr.GetString(iEmprpenombre);

            int iRegporcentaje = dr.GetOrdinal(this.Regporcentaje);
            if (!dr.IsDBNull(iRegporcentaje)) entity.RegPorcentaje = dr.GetDecimal(iRegporcentaje);

            int iPeeusuariocreacion = dr.GetOrdinal(this.Peeusuariocreacion);
            if (!dr.IsDBNull(iPeeusuariocreacion)) entity.PeeUsuarioCreacion = dr.GetString(iPeeusuariocreacion);

            int iPeefechacreacion = dr.GetOrdinal(this.Peefechacreacion);
            if (!dr.IsDBNull(iPeefechacreacion)) entity.PeeFechaCreacion = dr.GetDateTime(iPeefechacreacion);

            int iPeeusuarioupdate = dr.GetOrdinal(this.Peeusuarioupdate);
            if (!dr.IsDBNull(iPeeusuarioupdate)) entity.PeeUsuarioUpdate = dr.GetString(iPeeusuarioupdate);

            int iPeefechaupdate = dr.GetOrdinal(this.Peefechaupdate);
            if (!dr.IsDBNull(iPeefechaupdate)) entity.PeeFechaUpdate = dr.GetDateTime(iPeefechaupdate);

            int iRegpuntoentcodi = dr.GetOrdinal(this.Regpuntoentcodi);
            if (!dr.IsDBNull(iRegpuntoentcodi)) entity.RegPuntoEntCodi = Convert.ToInt32(dr.GetValue(iRegpuntoentcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprcodi));


            return entity;
        }


        #region Mapeo de Campos

        public string Empgencodi = "EMPRPECODI";
        public string Emprpenombre = "EMPRPENOMBRE";
        public string Regporcentaje = "EMPRPEPORCENTAJE";
        public string Peeusuariocreacion = "EMPRPEUSUARIOCREACION";
        public string Peefechacreacion = "EMPRPEFECHACREACION";
        public string Peeusuarioupdate = "EMPRPEUSUARIOUPDATE";
        public string Peefechaupdate = "EMPRPEFECHAUPDATE";
        public string Regpuntoentcodi = "RPECODI";
        public string Emprcodi = "EMPRCODI";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

    }
}
