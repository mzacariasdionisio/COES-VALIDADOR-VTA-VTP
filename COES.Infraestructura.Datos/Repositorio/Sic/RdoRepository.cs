using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class RdoRepository : RepositoryBase, IRdoRepository
    {
        string sConn = string.Empty;
        public RdoRepository(string strConn) : base(strConn)
        {
            sConn = strConn;
        }


        RdoHelper helper = new RdoHelper();
        RdoCumplimientoHelper helperCumplimiento = new RdoCumplimientoHelper();

        public List<RdoCumplimiento> GetByCriteria(RdoCumplimiento estudioeo)
        {
            //string sFechaInicio = estudioeo.Rdofechaini != null ? " TO_CHAR(eo.esteofechaini, 'yyyy/MM/dd') >= '" + estudioeo.Rdofechaini.Value.ToString("yyyy/MM/dd") + "'  " : " 1=1 ";
            ////string sFechaFin = estudioeo.Esteofechafin != null ? " TO_CHAR(eo.esteofechafin, 'yyyy/MM/dd') <= '" + estudioeo.Esteofechafin.Value.ToString("yyyy/MM/dd") + "'  " : " 1=1 ";
            //string sFechaFin = estudioeo.Rdofechafin != null ? " TO_CHAR(eo.esteofechaini, 'yyyy/MM/dd') <= '" + estudioeo.Rdofechafin.Value.ToString("yyyy/MM/dd") + "'  " : " 1=1 ";

            string tipoProyecto = "";
            string sFechaInicio = estudioeo.Rdofechaini.ToString("dd/MM/yyyy");
            string codFormato = estudioeo.codFormato.ToString();

            //string sql = string.Format(helper.SqlGetByCriteria, sFechaInicio, sFechaFin
            //                                                  , estudioeo.Estacodi, estudioeo.Emprcoditp, estudioeo.Esteonomb
            //                                                  , estudioeo.Esteopuntoconexion, estudioeo.Esteocodiproy
            //                                                  , estudioeo.nroPagina, estudioeo.nroFilas, estudioeo.Esteoanospuestaservicio, tipoProyecto);

            string sql = string.Format(helper.SqlGetRdoByCriteria, codFormato, sFechaInicio);

            List<RdoCumplimiento> entitys = new List<RdoCumplimiento>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RdoCumplimiento entity = new RdoCumplimiento();



                    int iEstepocodiusu = dr.GetOrdinal(helperCumplimiento.RdoNombreEmpresa);
                    if (!dr.IsDBNull(iEstepocodiusu)) entity.NombreEmpresa = dr.GetString(iEstepocodiusu);

                    //int iEsteponomb = dr.GetOrdinal(helper.H3);
                    //if (!dr.IsDBNull(iEsteponomb)) entity.Hora3 = dr.GetValue(2).ToString();
                    if (dr.GetValue(1).ToString() == "1")
                        entity.Hora3 = "Envio";
                    else
                        entity.Hora3 = "No Envio";

                    if (dr.GetValue(2).ToString() == "1")
                        entity.Hora6 = "Envio";
                    else
                        entity.Hora6 = "No Envio";


                    if (dr.GetValue(3).ToString() == "1")
                        entity.Hora9 = "Envio";
                    else
                        entity.Hora9 = "No Envio";

                    if (dr.GetValue(4).ToString() == "1")
                        entity.Hora12 = "Envio";
                    else
                        entity.Hora12 = "No Envio";


                    if (dr.GetValue(5).ToString() == "1")
                        entity.Hora15 = "Envio";
                    else
                        entity.Hora15 = "No Envio";


                    if (dr.GetValue(6).ToString() == "1")
                        entity.Hora18 = "Envio";
                    else
                        entity.Hora18 = "No Envio";


                    if (dr.GetValue(7).ToString() == "1")
                        entity.Hora21 = "Envio";
                    else
                        entity.Hora21 = "No Envio";

                    if (dr.GetValue(8).ToString() == "1")
                        entity.Hora24 = "Envio";
                    else
                        entity.Hora24 = "No Envio";
                    /*
                    int iEstadescripcion = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iEstadescripcion)) entity.Hora6 = dr.GetString( iEstadescripcion);

                    int iEmprnomb = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Hora9 = dr.GetString(iEmprnomb);

                    int iUsername = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iUsername)) entity.Hora12 = dr.GetString(iUsername);

                    int iTerceroinvolucrado = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iTerceroinvolucrado)) entity.Hora15 = dr.GetString(iTerceroinvolucrado);

                    int iEsteoalcancesolestenl = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iEsteoalcancesolestenl)) entity.Hora18 = dr.GetString(iEsteoalcancesolestenl);

                    int iEsteopotencia = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iEsteopotencia)) entity.Hora21 = dr.GetString(iEsteopotencia);

                    int iEsteopuntoconexion = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iEsteopuntoconexion)) entity.Hora24 = dr.GetString(iEsteopuntoconexion); */

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
