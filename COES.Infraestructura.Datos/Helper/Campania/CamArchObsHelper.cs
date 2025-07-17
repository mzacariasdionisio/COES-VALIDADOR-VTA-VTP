using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamArchObsHelper : HelperBase
    {
        public CamArchObsHelper() : base(Consultas.CamArchObsSql) { }

        public string SqlGetArchivoObsByObsId
        {
            get { return base.GetSqlXml("GetArchivoObsByObsId"); }
        }

        public string SqlSaveArchivoObs { 
            get { return base.GetSqlXml("SaveArchivoObs"); }
        }

        public string SqlUpdateArchivoObs
        {
            get { return base.GetSqlXml("UpdateArchivoObs"); }
        }

        public string SqlGetLastArchivoObsId
        {
            get { return base.GetSqlXml("GetLastArchivoObsId"); }
        }

        public string SqlDeleteArchivoObsById
        {
            get { return base.GetSqlXml("DeleteArchivoObsById"); }
        }

        public string SqlGetArchivoObsById
        {
            get { return base.GetSqlXml("GetArchivoObsById"); }
        }

        public string SqlGetArchivoObsNombreGenerado
        {
            get { return base.GetSqlXml("GetArchivoObsNombreGenerado"); }
        }

        public string SqlGetArchivoObsProyCodiNom
        {
            get { return base.GetSqlXml("GetArchivoObsProyCodiNom"); }
        }

    }
}
