using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// Clase que contiene el mapeo de la tabla vce_periodo_calculo
    public class VcePeriodoCalculoHelper : HelperBase
    {
        public VcePeriodoCalculoHelper() : base(Consultas.VcePeriodoCalculoSql )
        {
        }

        public VcePeriodoCalculoDTO Create(IDataReader dr)
        {
            VcePeriodoCalculoDTO entity = new VcePeriodoCalculoDTO();

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = dr.GetInt32(iPecacodi);

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.PeriCodi = dr.GetInt32(iPericodi);
            
            int iPecaversioncomp = dr.GetOrdinal(this.Pecaversioncomp);
            if (!dr.IsDBNull(iPecaversioncomp)) entity.PecaVersionComp = dr.GetInt32(iPecaversioncomp);

            int iPecanombre = dr.GetOrdinal(this.Pecanombre);
            if (!dr.IsDBNull(iPecanombre)) entity.PecaNombre = dr.GetString(iPecanombre);

            int iPecaversionvtea = dr.GetOrdinal(this.Pecaversionvtea);
            if (!dr.IsDBNull(iPecaversionvtea)) entity.PecaVersionVtea = dr.GetInt32(iPecaversionvtea);

            int iPecatipocambio = dr.GetOrdinal(this.Pecatipocambio);
            if (!dr.IsDBNull(iPecatipocambio)) entity.PecaTipoCambio = dr.GetDecimal(iPecatipocambio);

            int iPecaestregistro = dr.GetOrdinal(this.Pecaestregistro);
            if (!dr.IsDBNull(iPecaestregistro)) entity.PecaEstRegistro = dr.GetString(iPecaestregistro);

            int iPecausucreacion = dr.GetOrdinal(this.Pecausucreacion);
            if (!dr.IsDBNull(iPecausucreacion)) entity.PecaUsuCreacion = dr.GetString(iPecausucreacion);

            int iPecafeccreacion = dr.GetOrdinal(this.Pecafeccreacion);
            if (!dr.IsDBNull(iPecafeccreacion)) entity.PecaFecCreacion = dr.GetDateTime(iPecafeccreacion);

            int iPecausumodificacion = dr.GetOrdinal(this.Pecausumodificacion);
            if (!dr.IsDBNull(iPecausumodificacion)) entity.PecaUsuModificacion = dr.GetString(iPecausumodificacion);

            int iPecafecmodificacion = dr.GetOrdinal(this.Pecafecmodificacion);
            if (!dr.IsDBNull(iPecafecmodificacion)) entity.PecaFecModificacion = dr.GetDateTime(iPecafecmodificacion);

            int iPecamotivo = dr.GetOrdinal(this.Pecamotivo);
            if (!dr.IsDBNull(iPecamotivo)) entity.PecaMotivo = dr.GetString(iPecamotivo);

            return entity;
        }
                
        #region Mapeo de Campos
        public string Pecacodi = "PECACODI";
        public string Pericodi = "PERICODI";
        public string Pecaversioncomp = "PECAVERSIONCOMP";
        public string Pecanombre = "PECANOMBRE";
        public string Pecaversionvtea = "PECAVERSIONVTEA";
        public string Pecatipocambio = "PECATIPOCAMBIO";
        public string Pecaestregistro = "PECAESTREGISTRO";
        public string Pecausucreacion = "PECAUSUCREACION";
        public string Pecafeccreacion = "PECAFECCREACION";
        public string Pecausumodificacion = "PECAUSUMODIFICACION";
        public string Pecafecmodificacion = "PECAFECMODIFICACION";
        public string Pecamotivo = "PECAMOTIVO";
        // campos de la tabla trn_periodo, se usa para los listados
        public string Perinombre = "PERINOMBRE";
        public string Aniocodi = "PERIANIO";
        public string Mescodi = "PERIMES";
        public string Perianiomes = "PERIANIOMES";
        public string Periinforme = "PERIINFORME";

        /*
        public string Aniocodi = "PERIANIO";
        public string Mescodi = "PERIMES";
        public string Recanombre = "RECANOMBRE";
        public string Perifechavalorizacion = "PERIFECHAVALORIZACION";
        public string Perifechalimite = "PERIFECHALIMITE";
        public string Perihoralimite = "PERIHORALIMITE";
        public string Perifechaobservacion = "PERIFECHAOBSERVACION";
        public string Periestado = "PERIESTADO";
        public string Periusername = "PERIUSERNAME";
        public string Perifecins = "PERIFECINS";
        public string Perifecact = "PERIFECACT";
        public string Perianiomes = "PERIANIOMES";
        

        //- conpensaciones.JDEL - Inicio 26/08/2016: Cambio para atender el requerimiento.
        public string PeriDescripcion = "PERIDESCRIPCION";

        //- compensaciones.HDT - 05/04/2017: Cambio para atender el requerimiento. 
        public string Fechaini = "FECHAINI";

        //- compensaciones.HDT - 05/04/2017: Cambio para atender el requerimiento. 
        public string Fechafin = "FECHAFIN";
        */

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }
                
        public string SqlGetByAnioMes
        {
            get { return base.GetSqlXml("GetByAnioMes"); }
        }

        public string SqlGetByIdPeriodo
        {
            get { return base.GetSqlXml("GetByIdPeriodo"); }
        }

        public string SqlGetNumRegistros
        {
            get { return base.GetSqlXml("GetNumRegistros"); }
        }

        public string SqlListarByEstado
        {
            get { return base.GetSqlXml("ListarByEstado"); }
        }

        public string SqlListarByEstadoPublicarCerrado 
        {
            get { return base.GetSqlXml("ListarByEstadoPublicarCerrado"); }
        }

        public string SqlGetPeriodoAnteriorById 
        {
            get { return base.GetSqlXml("BuscarPeriodoAnterior"); }
        }

        public string SqlListarPeriodosFuturos
        {
            get { return base.GetSqlXml("ListarPeriodosFuturos"); }
        }

        public string SqlObtenerPeriodoDTR
        {
            get { return base.GetSqlXml("ObtenerPeriodoDTR"); }
        }

        public string SqlNroCalculosActivosPeriodo
        {
            get { return base.GetSqlXml("NroCalculosActivosPeriodo"); }
        }

        public string SqlGetIdAnteriorCalculo
        {
            get { return base.GetSqlXml("GetIdAnteriorCalculo"); }
        }
        public string SqlGetIdAnteriorConfig
        {
            get { return base.GetSqlXml("GetIdAnteriorConfig"); }
        }

        //Cambio
        public string SqlGetPeriodo
        {
            get { return base.GetSqlXml("GetPeriodo"); }
        }

        public string SqlGetPeriodoMaximo
        {
            get { return base.GetSqlXml("GetPeriodoMaximo"); }
        }
        public string SqlUpdateCompensacionInforme
        {
            get { return GetSqlXml("UpdateCompensacionInforme"); }
        }
        /*
        //- conpensaciones.JDEL - Inicio 10/11/2016: Cambio para atender el requerimiento de agregar la de descripción del periodo.
        public string SqlListarPeriodosTC
        {
            get { return base.GetSqlXml("ListarPeriodosTC"); }
        }

        //- JDEL Fin

        //- compensaciones.HDT - 05/04/2017: Cambio para atender el requerimiento. 
        public string SqlListPeriodoByIdProcesa
        {
            get { return base.GetSqlXml("ListPeriodoByIdProcesa"); }
        }
         */

    }
}
