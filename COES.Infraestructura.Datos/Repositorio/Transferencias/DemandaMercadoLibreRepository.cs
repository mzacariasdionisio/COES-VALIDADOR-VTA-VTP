using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class DemandaMercadoLibreRepository : RepositoryBase, IDemandaMercadoLibreRepository
    {
        public DemandaMercadoLibreRepository(string strConn)
            : base(strConn)
        {
        }

        DemandaMercadoLibreHelper helper = new DemandaMercadoLibreHelper();

        public List<DemandaMercadoLibreDTO> ListDemandaMercadoLibre(DateTime[] periodos, DateTime periodoMes, int tipoEmpresa, string empresas,
            int codigoLectura, int codigoOrigenLectura)
        {
            List<DemandaMercadoLibreDTO> entitys = new List<DemandaMercadoLibreDTO>();
            var consultaSql = helper.SqlListaDemandaMercadoLibre;

            var condicionSelect = string.Empty;
            var condicionWhere = string.Empty;
            const string comilla = "\"";  

            for (int i = 0; i < 12; i++)
            {
                condicionSelect = condicionSelect + string.Format(",MAX(CASE WHEN MES='{0}' THEN MAX_DEM END) AS " + comilla + "{1}" + comilla
                    ,periodos[i].ToString("yyyyMM"),"DemandaMes"+(i+1).ToString());
            }

            if (tipoEmpresa > 0)
            {
                if (tipoEmpresa.Equals(2))
                {
                    condicionWhere = condicionWhere + " AND EMP.TIPOEMPRCODI = " + tipoEmpresa;
                }
                else
                {
                    condicionWhere = condicionWhere + string.Format(" AND EMP.TIPOEMPRCODI = {0} AND EMP.INDDEMANDA='S' ", tipoEmpresa);
                }
                
            }
            else
            {
                condicionWhere = condicionWhere + " AND (EMP.TIPOEMPRCODI = 2 OR (EMP.TIPOEMPRCODI = 4 AND EMP.INDDEMANDA='S') ) ";
            }

            if (!string.IsNullOrEmpty(empresas))
            {
                condicionWhere = condicionWhere + string.Format("AND NVL(EMP.EMPRRAZSOCIAL,EMP.EMPRNOMB) LIKE '%{0}%'", empresas.ToUpper());
            }

            if (codigoLectura > 0)
            {
                condicionWhere = condicionWhere + "  AND ME.LECTCODI = " + codigoLectura;
            }

            if (codigoOrigenLectura > 0)
            {
                condicionWhere = condicionWhere + "  AND PTO.ORIGLECTCODI = " + codigoOrigenLectura;
            }

            consultaSql = string.Format(consultaSql, condicionSelect, periodoMes.ToString("yyyyMM"), condicionWhere);

            DbCommand command = dbProvider.GetSqlStringCommand(consultaSql);
            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DemandaMercadoLibreDTO entity = new DemandaMercadoLibreDTO();

                    int iEmprCodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);

                    int iEmprRazSocial = dr.GetOrdinal(helper.EmprRazSocial);
                    if (!dr.IsDBNull(iEmprRazSocial)) entity.EmprRazSocial = dr.GetString(iEmprRazSocial);

                    int iDemandaMes1 = dr.GetOrdinal(helper.DemandaMes1);
                    if (!dr.IsDBNull(iDemandaMes1)) entity.DemandaMes1 = dr.GetDecimal(iDemandaMes1);

                    int iDemandaMes2 = dr.GetOrdinal(helper.DemandaMes2);
                    if (!dr.IsDBNull(iDemandaMes2)) entity.DemandaMes2 = dr.GetDecimal(iDemandaMes2);

                    int iDemandaMes3 = dr.GetOrdinal(helper.DemandaMes3);
                    if (!dr.IsDBNull(iDemandaMes3)) entity.DemandaMes3 = dr.GetDecimal(iDemandaMes3);

                    int iDemandaMes4 = dr.GetOrdinal(helper.DemandaMes4);
                    if (!dr.IsDBNull(iDemandaMes4)) entity.DemandaMes4 = dr.GetDecimal(iDemandaMes4);

                    int iDemandaMes5 = dr.GetOrdinal(helper.DemandaMes5);
                    if (!dr.IsDBNull(iDemandaMes5)) entity.DemandaMes5 = dr.GetDecimal(iDemandaMes5);

                    int iDemandaMes6 = dr.GetOrdinal(helper.DemandaMes6);
                    if (!dr.IsDBNull(iDemandaMes6)) entity.DemandaMes6 = dr.GetDecimal(iDemandaMes6);

                    int iDemandaMes7 = dr.GetOrdinal(helper.DemandaMes7);
                    if (!dr.IsDBNull(iDemandaMes7)) entity.DemandaMes7 = dr.GetDecimal(iDemandaMes7);
                    
                    int iDemandaMes8 = dr.GetOrdinal(helper.DemandaMes8);
                    if (!dr.IsDBNull(iDemandaMes8)) entity.DemandaMes8 = dr.GetDecimal(iDemandaMes8);

                    int iDemandaMes9 = dr.GetOrdinal(helper.DemandaMes9);
                    if (!dr.IsDBNull(iDemandaMes9)) entity.DemandaMes9 = dr.GetDecimal(iDemandaMes9);

                    int iDemandaMes10 = dr.GetOrdinal(helper.DemandaMes10);
                    if (!dr.IsDBNull(iDemandaMes10)) entity.DemandaMes10 = dr.GetDecimal(iDemandaMes10);

                    int iDemandaMes11 = dr.GetOrdinal(helper.DemandaMes11);
                    if (!dr.IsDBNull(iDemandaMes11)) entity.DemandaMes11 = dr.GetDecimal(iDemandaMes11);

                    int iDemandaMes12 = dr.GetOrdinal(helper.DemandaMes12);
                    if (!dr.IsDBNull(iDemandaMes12)) entity.DemandaMes12 = dr.GetDecimal(iDemandaMes12);


                    //int iCompCodi = dr.GetOrdinal(this.CompCodi);
                    //if (!dr.IsDBNull(iCompCodi)) entity.CompCodi = dr.GetInt32(iCompCodi);

                    //int iIngrCompVers = dr.GetOrdinal(this.IngrCompVers);
                    //if (!dr.IsDBNull(iIngrCompVers)) entity.IngrCompVersion = dr.GetInt32(iIngrCompVers);

                    int iDemandaMaxima = dr.GetOrdinal(helper.DemandaMaxima);
                    if (!dr.IsDBNull(iDemandaMaxima)) entity.DemandaMaxima = dr.GetDecimal(iDemandaMaxima);

                    //int iIngrCompusername = dr.GetOrdinal(this.IngrCompusername);
                    //if (!dr.IsDBNull(iIngrCompusername)) entity.IngrCompUserName = dr.GetString(iIngrCompusername);

                    //int iIngrCompfecins = dr.GetOrdinal(this.IngrCompfecins);
                    //if (!dr.IsDBNull(iIngrCompfecins)) entity.IngrCompFecIns = dr.GetDateTime(iIngrCompfecins); 7

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        
    }
}
