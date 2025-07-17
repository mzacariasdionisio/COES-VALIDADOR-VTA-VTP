using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using COES.Infraestructura.Datos.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamTipoproyectoRepository : RepositoryBase, ICamTipoproyectoRepository
    {
        public CamTipoproyectoRepository(string strConn) : base(strConn) { }

        CamTipoproyectoHelper Helper = new CamTipoproyectoHelper();
        CamHojaFichaHelper HelperFicha = new CamHojaFichaHelper();
        CamTipoFichaProyectoHelper HelperTipoFicha = new CamTipoFichaProyectoHelper();

        public List<TipoProyectoDTO> GetTipoProyecto()
        {
            List<TipoProyectoDTO> entitys = new List<TipoProyectoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetTipoProyecto);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, 0);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TipoProyectoDTO ob = new TipoProyectoDTO();
                    ob.TipoCodigo = Int32.Parse(dr["TIPOCODI"].ToString());
                    ob.TipoNombre = dr["TIPONOMBRE"].ToString();
                    ob.IndDel = dr["IND_DEL"].ToString();
                    //ob.UsuarioDel = dr["USU_MODIFICACION"].ToString();
                    ob.Orden = Int32.Parse(dr["ORDEN"].ToString());

                    DbCommand commandTipoFichaProyecto = dbProvider.GetSqlStringCommand(HelperTipoFicha.SqlGetTipoFichaProyecto);
                    dbProvider.AddInParameter(commandTipoFichaProyecto, "IND_DEL", DbType.String, 0);
                    dbProvider.AddInParameter(commandTipoFichaProyecto, "TIPOCODI", DbType.Int32, ob.TipoCodigo);

                    List<TipoFichaProyectoDTO> tipoFichaProyectoDTOs = new List<TipoFichaProyectoDTO>();
                    using (IDataReader drSub = dbProvider.ExecuteReader(commandTipoFichaProyecto))
                    {
                        while (drSub.Read())
                        {
                            TipoFichaProyectoDTO tipoFichaProyectoDTO = new TipoFichaProyectoDTO();
                            tipoFichaProyectoDTO.TipoFiCodigo = Int32.Parse(drSub["TIPOFICODI"].ToString());
                            tipoFichaProyectoDTO.TipoFiNombre = drSub["TIPOFINOMBRE"].ToString();
                            tipoFichaProyectoDTO.TipoCodigo = Int32.Parse(drSub["TIPOCODI"].ToString());
                            tipoFichaProyectoDTO.SubTipoCodigo = Int32.Parse(drSub["SUBTIPOCODI"].ToString());
                            tipoFichaProyectoDTO.ContrHtml = drSub["CONTRHTML"].ToString();
                            tipoFichaProyectoDTO.IndDel = drSub["IND_DEL"].ToString();
                            tipoFichaProyectoDTO.Orden = Int32.Parse(drSub["ORDEN"].ToString());

                            DbCommand commandFicha = dbProvider.GetSqlStringCommand(HelperFicha.SqlGetFicha);
                            dbProvider.AddInParameter(commandFicha, "IND_DEL", DbType.String, 0);
                            dbProvider.AddInParameter(commandFicha, "TIPOFICODI", DbType.Int32, tipoFichaProyectoDTO.TipoFiCodigo);
                            List<HojaFichaDTO> fichasDTO = new List<HojaFichaDTO>();
                            using (IDataReader drFicha = dbProvider.ExecuteReader(commandFicha))
                            {
                                while (drFicha.Read())
                                {
                                    HojaFichaDTO fichaDTO = new HojaFichaDTO();
                                    fichaDTO.HojaCodigo = Int32.Parse(drFicha["HOJACODI"].ToString());
                                    fichaDTO.TipoFichaCodigo = Int32.Parse(drFicha["TIPOFICODI"].ToString());
                                    fichaDTO.HojaNombre = drFicha["HOJANOMBRE"].ToString();
                                    fichaDTO.IndDel = drFicha["IND_DEL"].ToString();
                                    //fichaDTO.UsuarioDel = drFicha["USU_MODIFICACION"].ToString();
                                    fichaDTO.Orden = Int32.Parse(drFicha["ORDEN"].ToString());
                                    fichasDTO.Add(fichaDTO);
                                }
                            }
                            tipoFichaProyectoDTO.ListaHojaFichas = fichasDTO;
                            tipoFichaProyectoDTOs.Add(tipoFichaProyectoDTO);
                        }
                        ob.ListaTipoFichaProyecto = tipoFichaProyectoDTOs;
                    }
                    entitys.Add(ob);
                }
            }
            return entitys;
        }



    }
}
