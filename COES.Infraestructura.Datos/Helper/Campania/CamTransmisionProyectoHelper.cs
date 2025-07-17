using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamTransmisionProyectoHelper : HelperBase
    {

        public CamTransmisionProyectoHelper() : base(Consultas.CamTransmProyectoSql ) { }

        public string SqlGetTransmisionProyecto
        {
            get { return base.GetSqlXml("GetTransmisionProyecto"); }
        }


        public string SqlSaveTransmisionProyecto
        {
            get { return base.GetSqlXml("SaveTransmisionProyecto"); }
        }

        public string SqlUpdateTransmisionProyecto
        {
            get { return base.GetSqlXml("UpdateTransmisionProyecto"); }
        }

        public string SqlGetLastTransmisionProyectoId
        {
            get { return base.GetSqlXml("GetLastTransmisionProyectoId"); }
        }

        public string SqlDeleteTransmisionProyectoById
        {
            get { return base.GetSqlXml("DeleteTransmisionProyectoById"); }
        }

        public string SqlGetTransmisionProyectoById
        {
            get { return base.GetSqlXml("GetTransmisionProyectoById"); }
        }

        public string SqlGetTransmisionProyectoByPeriodo
        {
            get { return base.GetSqlXml("GetTransmisionProyectoByPeriodo"); }
        }

        public string SqlUpdateProyEstadoById
        {
            get { return base.GetSqlXml("UpdateProyEstadoById"); }
        }

        public string SqlUpdateProyEstadoByIdProy
        {
            get { return base.GetSqlXml("UpdateProyEstadoByIdProy"); }
        }

        public string SqlUpdateProyFechaEnvioObsById
        {
            get { return base.GetSqlXml("UpdateProyFechaEnvioObsById"); }
        }


    }
}
