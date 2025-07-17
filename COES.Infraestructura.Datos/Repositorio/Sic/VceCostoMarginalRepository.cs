using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class VceCostoMarginalRepository : RepositoryBase, IVceCostoMarginalRepository
    {
        public VceCostoMarginalRepository(string strConn) : base(strConn)
        {
        }

        VceCostoMarginalHelper helper = new VceCostoMarginalHelper();

        public List<int> LstGrupos()
        {
            //Prueba Costos Variables
            string queryGrupo = "SELECT GRUPOCODI FROM PR_GRUPO WHERE CATECODI = 2 AND GRUPOACTIVO ='S'";
            DbCommand command = dbProvider.GetSqlStringCommand(queryGrupo);

            List<int> lstGrupo = new List<int>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    lstGrupo.Add(Int32.Parse(dr["GRUPOCODI"].ToString()));
                }
            }

            return lstGrupo;
        }

        public List<string> LstBodyCostoMarginal(int pericodi, int cosmarversion)
        {   


            //Costo Marginal
            string queryString = string.Format(helper.SqlListarBarras);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            string query = "SELECT VCECMRSEC";
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    query = query + ",MAX(CASE WHEN BARRCODI = " + dr["BARRCODI"].ToString() + " THEN VCECMRVALOR END) AS  \"" + dr["BARRBARRATRANSFERENCIA"].ToString() + "\"";
                }
            }
            query = query + " FROM (";
            for (int i = 1; i <= 96; i++)
            {
                if (i > 1)
                {
                    query = query + " UNION ALL ";
                }
                query = query + " SELECT pericodi,BARRCODI,(COSMARDIA-1)*96+" + i + " AS VCECMRSEC,ROUND(COSMAR" + i + ",10) AS VCECMRVALOR FROM TRN_COSTO_MARGINAL WHERE pericodi = "+pericodi + " AND COSMARVERSION = " +cosmarversion;
            }
            query = query + " ) CM";
            query = query + " WHERE pericodi = " + pericodi;
            query = query + " GROUP BY VCECMRSEC";
            query = query + " ORDER BY 1";

            command = dbProvider.GetSqlStringCommand(query);

            string strHtml = "";

            List<string> lstBody = new List<string>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                var columns = new List<string>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    columns.Add(dr.GetName(i));
                }
                while (dr.Read())
                {
                    foreach (var item in columns)
                    {
                        if (strHtml.Equals(""))
                        {
                            strHtml = strHtml + dr[item.Trim()].ToString();
                        }
                        else
                        {
                            strHtml = strHtml + "|" + dr[item.Trim()].ToString();
                        }
                    }

                    lstBody.Add(strHtml);
                    strHtml = "";
                }
            }
            return lstBody;
        }

        public List<string> LstHeadCostoMarginal(int pericodi)
        {
            string queryString = string.Format(helper.SqlListarBarras);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            string query = "SELECT VCECMRSEC";
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    query = query + ",MAX(CASE WHEN BARRCODI = " + dr["BARRCODI"].ToString() + " THEN VCECMRVALOR END) AS  \"" + dr["BARRBARRATRANSFERENCIA"].ToString() + "\"";
                }
            }
            query = query + " FROM (";
            for (int i = 1; i <= 96; i++)
            {
                if (i > 1)
                {
                    query = query + " UNION ALL ";
                }
                query = query + " SELECT pericodi,BARRCODI,(COSMARDIA-1)*96+" + i + " AS VCECMRSEC,ROUND(COSMAR" + i + ",10) AS VCECMRVALOR FROM TRN_COSTO_MARGINAL WHERE pericodi = " + pericodi;
            }
            query = query + " ) CM";
            query = query + " WHERE pericodi = " + pericodi;
            query = query + " GROUP BY VCECMRSEC";
            query = query + " ORDER BY 1";

            command = dbProvider.GetSqlStringCommand(query);

            List<string> lstHead = new List<string>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    lstHead.Add(dr.GetName(i));
                }
            }
            return lstHead;
        }  

    }

}
