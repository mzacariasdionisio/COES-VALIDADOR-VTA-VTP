using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class DpoFuncionHelper : HelperBase
    {
        public DpoFuncionHelper() : base(Consultas.DpoFuncionSql)
        {

        }

        #region Metodos
        public DpoFuncionDTO Create(IDataReader dr)
        {
            DpoFuncionDTO entity = new DpoFuncionDTO();

            int iDpofnccodi = dr.GetOrdinal(this.Dpofnccodi);
            if (!dr.IsDBNull(iDpofnccodi)) entity.Dpofnccodi = Convert.ToInt32(dr.GetValue(iDpofnccodi));

            int iDpofncnombre = dr.GetOrdinal(this.Dpofncnombre);
            if (!dr.IsDBNull(iDpofncnombre)) entity.Dpofncnombre = dr.GetString(iDpofncnombre);

            int iDpofnctipo = dr.GetOrdinal(this.Dpofnctipo);
            if (!dr.IsDBNull(iDpofnctipo)) entity.Dpofnctipo = dr.GetString(iDpofnctipo);

            int iDpofncdescripcion = dr.GetOrdinal(this.Dpofncdescripcion);
            if (!dr.IsDBNull(iDpofncdescripcion)) entity.Dpofncdescripcion = dr.GetString(iDpofncdescripcion);

            int iDpofncusucreacion = dr.GetOrdinal(this.Dpofncusucreacion);
            if (!dr.IsDBNull(iDpofncusucreacion)) entity.Dpofncusucreacion = dr.GetString(iDpofncusucreacion);

            int iDpofncfeccreacion = dr.GetOrdinal(this.Dpofncfeccreacion);
            if (!dr.IsDBNull(iDpofncfeccreacion)) entity.Dpofncfeccreacion = dr.GetDateTime(iDpofncfeccreacion);

            int iDpofncusumodificacion = dr.GetOrdinal(this.Dpofncusumodificacion);
            if (!dr.IsDBNull(iDpofncusumodificacion)) entity.Dpofncusumodificacion = dr.GetString(iDpofncusumodificacion);

            int iDpofncfecmodificacion = dr.GetOrdinal(this.Dpofncfecmodificacion);
            if (!dr.IsDBNull(iDpofncfecmodificacion)) entity.Dpofncfecmodificacion = dr.GetDateTime(iDpofncfecmodificacion);


            return entity;
        }
        #endregion

        #region Mapeo de Campos
        public string Dpofnccodi = "DPOFNCCODI";
        public string Dpofncnombre = "DPOFNCNOMBRE";
        public string Dpofnctipo = "DPOFNCTIPO";
        public string Dpofncdescripcion = "DPOFNCDESCRIPCION";
        public string Dpofncusucreacion = "DPOFNCUSUCREACION";
        public string Dpofncfeccreacion = "DPOFNCFECCREACION";
        public string Dpofncusumodificacion = "DPOFNCUSUMODIFICACION";
        public string Dpofncfecmodificacion = "DPOFNCFECMODIFICACION";
        #endregion

        #region Querys

        #endregion
    }
}
