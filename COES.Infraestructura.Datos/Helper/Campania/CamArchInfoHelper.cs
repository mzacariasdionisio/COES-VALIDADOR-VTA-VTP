using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamArchInfoHelper : HelperBase
    {
        public CamArchInfoHelper() : base(Consultas.CamArchInfoSql) { }


        public string SqlGetArchivoInfoProyCodi
        {
            get { return base.GetSqlXml("GetArchivoInfoProyCodi"); }
        }

        public string SqlGetArchivoInfoByProyCodi
        {
            get { return base.GetSqlXml("GetArchivoInfoByProyCodi"); }
        }

        public string SqlSaveArchivoInfo { 
            get { return base.GetSqlXml("SaveArchivoInfo"); }
        }

        public string SqlUpdateArchivoInfo
        {
            get { return base.GetSqlXml("UpdateArchivoInfo"); }
        }
        public string SqlUpdateUbicacionByproy
        {
            get { return base.GetSqlXml("UpdateUbicacionByproyCodi"); }
        }

        public string SqlGetLastArchivoInfoId
        {
            get { return base.GetSqlXml("GetLastArchivoInfoId"); }
        }

        public string SqlDeleteArchivoInfoById
        {
            get { return base.GetSqlXml("DeleteArchivoInfoById"); }
        }

        public string SqlGetArchivoInfoById
        {
            get { return base.GetSqlXml("GetArchivoInfoById"); }
        }

        public string SqlGetArchivoInfoNombreGenerado
        {
            get { return base.GetSqlXml("GetArchivoInfoNombreGenerado"); }
        }

        public string SqlGetArchivoInfoProyCodiNom
        {
            get { return base.GetSqlXml("GetArchivoInfoProyCodiNom"); }
        }

    }
}
