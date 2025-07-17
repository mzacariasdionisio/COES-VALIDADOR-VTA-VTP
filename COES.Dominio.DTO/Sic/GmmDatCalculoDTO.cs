using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public partial class GmmDatCalculoDTO : EntityBase
    {
        /// <summary>
        /// Clase que mapea la tabla GMM_DATCALCULO
        /// </summary>
        public int DCALCODI { get; set; }
        public string TINSCODI { get; set; }
        public int EMPGCODI { get; set; }
        public int PERICODI { get; set; }


        public decimal DCALVALOR { get; set; }
        public int EMPRCODI { get; set; }
        public int BARRCODI { get; set; }
        public int PTOMEDICODI { get; set; }
        public int TPTOMEDICODI { get; set; }
        public int TIPOINFOCODI { get; set; }
        public DateTime? MEDIFECHA { get; set; }
        public int LECTCODI { get; set; }

        public int DCALANIO { get; set; }
        public int DCALMES { get; set; }

        public string DCALUSUCREACION { get; set; }
        public DateTime? DCALFECCREACION { get; set; }

        #region Campos para resultados
        public string EMPRESA { get; set; }
        public string RPMES { get; set; }
        public DateTime? RFECCREACION { get; set; }
        public string RUSUCREACION { get; set; }
        public decimal RSCOMPLE { get; set; }
        public decimal RMES3 { get; set; }
        public decimal RMES2 { get; set; }
        public decimal RMES1 { get; set; }
        public decimal RCODI { get; set; }
        public decimal RPEAJE { get; set; }
        public decimal RCAPACIDAD { get; set; }
        public decimal RMPM3 { get; set; }
        public decimal RMPM2 { get; set; }
        public decimal RPLVTP { get; set; }
        public decimal RMCM3 { get; set; }
        public decimal RMCM2 { get; set; }
        public decimal RCLVTP { get; set; }
        public decimal RDMES3 { get; set; }
        public decimal RDMES2 { get; set; }
        public decimal RDMES1 { get; set; }
        public decimal RPFIRME1MR { get; set; }

        public decimal RPREPOT { get; set; }
        public decimal RPEAJEU { get; set; }
        public decimal RMARGENR { get; set; }

        public decimal ENRGN1 { get; set; }
        public decimal ENRGN { get; set; }


        public decimal RDEMM1 { get; set; }
        public decimal RDEMM2 { get; set; }
        public decimal RDEMM3 { get; set; }
        public decimal RINFLEX { get; set; }

        public decimal RPFIRME { get; set; }
        public decimal RINFLEXOP { get; set; }
        public decimal RCOL2 { get; set; }
        public decimal RCOL1 { get; set; }
        public decimal RLSCIO { get; set; }
        public decimal REPR11 { get; set; }
        public decimal RER10 { get; set; }
        public decimal RPARM3 { get; set; }
        public decimal REPRM3 { get; set; }
        public decimal RPARM2 { get; set; }
        public decimal REPRM2 { get; set; }
        public decimal RPARM1 { get; set; }
        public decimal REPRM1 { get; set; }
        public decimal REREACTIVA { get; set; }
        public decimal RVMERM1 { get; set; }
        public decimal RENERGIA { get; set; }
        public decimal RVPROY { get; set; }
        public decimal RVD11 { get; set; }
        public decimal RVD10 { get; set; }
        public decimal RLVTEA { get; set; }
        public decimal RENTREGA { get; set; }
        public decimal RRETIRO { get; set; }
        public decimal REGICODI { get; set; }

        public decimal TOTALGARANTIA { get; set; }
        public decimal GARANTIADEP { get; set; }
        public decimal FACTOR { get; set; }
        public string COMENTARIO { get; set; }


        public decimal anio { get; set; }
        public string mes { get; set; }
        public decimal periodo { get; set; }
        public string empresa { get; set; }
        public string insumo { get; set; }
        public decimal valor { get; set; }
        public string usuario { get; set; }


        public string barra { get; set; }
        public DateTime? fecha { get; set; }

        public decimal energia_t1 { get; set; }
        public decimal energia_t2 { get; set; }
        public decimal energia_t3 { get; set; }
        public decimal energia_t4 { get; set; }
        public decimal energia_t5 { get; set; }
        public decimal energia_t6 { get; set; }
        public decimal energia_t7 { get; set; }
        public decimal energia_t8 { get; set; }
        public decimal energia_t9 { get; set; }
        public decimal energia_t10 { get; set; }
        public decimal energia_t11 { get; set; }
        public decimal energia_t12 { get; set; }
        public decimal energia_t13 { get; set; }
        public decimal energia_t14 { get; set; }
        public decimal energia_t15 { get; set; }
        public decimal energia_t16 { get; set; }
        public decimal energia_t17 { get; set; }
        public decimal energia_t18 { get; set; }
        public decimal energia_t19 { get; set; }
        public decimal energia_t20 { get; set; }
        public decimal energia_t21 { get; set; }
        public decimal energia_t22 { get; set; }
        public decimal energia_t23 { get; set; }
        public decimal energia_t24 { get; set; }
        public decimal energia_t25 { get; set; }
        public decimal energia_t26 { get; set; }
        public decimal energia_t27 { get; set; }
        public decimal energia_t28 { get; set; }
        public decimal energia_t29 { get; set; }
        public decimal energia_t30 { get; set; }
        public decimal energia_t31 { get; set; }
        public decimal energia_t32 { get; set; }
        public decimal energia_t33 { get; set; }
        public decimal energia_t34 { get; set; }
        public decimal energia_t35 { get; set; }
        public decimal energia_t36 { get; set; }
        public decimal energia_t37 { get; set; }
        public decimal energia_t38 { get; set; }
        public decimal energia_t39 { get; set; }
        public decimal energia_t40 { get; set; }
        public decimal energia_t41 { get; set; }
        public decimal energia_t42 { get; set; }
        public decimal energia_t43 { get; set; }
        public decimal energia_t44 { get; set; }
        public decimal energia_t45 { get; set; }
        public decimal energia_t46 { get; set; }
        public decimal energia_t47 { get; set; }
        public decimal energia_t48 { get; set; }
        public decimal energia_t49 { get; set; }
        public decimal energia_t50 { get; set; }
        public decimal energia_t51 { get; set; }
        public decimal energia_t52 { get; set; }
        public decimal energia_t53 { get; set; }
        public decimal energia_t54 { get; set; }
        public decimal energia_t55 { get; set; }
        public decimal energia_t56 { get; set; }
        public decimal energia_t57 { get; set; }
        public decimal energia_t58 { get; set; }
        public decimal energia_t59 { get; set; }
        public decimal energia_t60 { get; set; }
        public decimal energia_t61 { get; set; }
        public decimal energia_t62 { get; set; }
        public decimal energia_t63 { get; set; }
        public decimal energia_t64 { get; set; }
        public decimal energia_t65 { get; set; }
        public decimal energia_t66 { get; set; }
        public decimal energia_t67 { get; set; }
        public decimal energia_t68 { get; set; }
        public decimal energia_t69 { get; set; }
        public decimal energia_t70 { get; set; }
        public decimal energia_t71 { get; set; }
        public decimal energia_t72 { get; set; }
        public decimal energia_t73 { get; set; }
        public decimal energia_t74 { get; set; }
        public decimal energia_t75 { get; set; }
        public decimal energia_t76 { get; set; }
        public decimal energia_t77 { get; set; }
        public decimal energia_t78 { get; set; }
        public decimal energia_t79 { get; set; }
        public decimal energia_t80 { get; set; }
        public decimal energia_t81 { get; set; }
        public decimal energia_t82 { get; set; }
        public decimal energia_t83 { get; set; }
        public decimal energia_t84 { get; set; }
        public decimal energia_t85 { get; set; }
        public decimal energia_t86 { get; set; }
        public decimal energia_t87 { get; set; }
        public decimal energia_t88 { get; set; }
        public decimal energia_t89 { get; set; }
        public decimal energia_t90 { get; set; }
        public decimal energia_t91 { get; set; }
        public decimal energia_t92 { get; set; }
        public decimal energia_t93 { get; set; }
        public decimal energia_t94 { get; set; }
        public decimal energia_t95 { get; set; }
        public decimal energia_t96 { get; set; }
        public decimal cm_t1 { get; set; }
        public decimal cm_t2 { get; set; }
        public decimal cm_t3 { get; set; }
        public decimal cm_t4 { get; set; }
        public decimal cm_t5 { get; set; }
        public decimal cm_t6 { get; set; }
        public decimal cm_t7 { get; set; }
        public decimal cm_t8 { get; set; }
        public decimal cm_t9 { get; set; }
        public decimal cm_t10 { get; set; }
        public decimal cm_t11 { get; set; }
        public decimal cm_t12 { get; set; }
        public decimal cm_t13 { get; set; }
        public decimal cm_t14 { get; set; }
        public decimal cm_t15 { get; set; }
        public decimal cm_t16 { get; set; }
        public decimal cm_t17 { get; set; }
        public decimal cm_t18 { get; set; }
        public decimal cm_t19 { get; set; }
        public decimal cm_t20 { get; set; }
        public decimal cm_t21 { get; set; }
        public decimal cm_t22 { get; set; }
        public decimal cm_t23 { get; set; }
        public decimal cm_t24 { get; set; }
        public decimal cm_t25 { get; set; }
        public decimal cm_t26 { get; set; }
        public decimal cm_t27 { get; set; }
        public decimal cm_t28 { get; set; }
        public decimal cm_t29 { get; set; }
        public decimal cm_t30 { get; set; }
        public decimal cm_t31 { get; set; }
        public decimal cm_t32 { get; set; }
        public decimal cm_t33 { get; set; }
        public decimal cm_t34 { get; set; }
        public decimal cm_t35 { get; set; }
        public decimal cm_t36 { get; set; }
        public decimal cm_t37 { get; set; }
        public decimal cm_t38 { get; set; }
        public decimal cm_t39 { get; set; }
        public decimal cm_t40 { get; set; }
        public decimal cm_t41 { get; set; }
        public decimal cm_t42 { get; set; }
        public decimal cm_t43 { get; set; }
        public decimal cm_t44 { get; set; }
        public decimal cm_t45 { get; set; }
        public decimal cm_t46 { get; set; }
        public decimal cm_t47 { get; set; }
        public decimal cm_t48 { get; set; }
        public decimal cm_t49 { get; set; }
        public decimal cm_t50 { get; set; }
        public decimal cm_t51 { get; set; }
        public decimal cm_t52 { get; set; }
        public decimal cm_t53 { get; set; }
        public decimal cm_t54 { get; set; }
        public decimal cm_t55 { get; set; }
        public decimal cm_t56 { get; set; }
        public decimal cm_t57 { get; set; }
        public decimal cm_t58 { get; set; }
        public decimal cm_t59 { get; set; }
        public decimal cm_t60 { get; set; }
        public decimal cm_t61 { get; set; }
        public decimal cm_t62 { get; set; }
        public decimal cm_t63 { get; set; }
        public decimal cm_t64 { get; set; }
        public decimal cm_t65 { get; set; }
        public decimal cm_t66 { get; set; }
        public decimal cm_t67 { get; set; }
        public decimal cm_t68 { get; set; }
        public decimal cm_t69 { get; set; }
        public decimal cm_t70 { get; set; }
        public decimal cm_t71 { get; set; }
        public decimal cm_t72 { get; set; }
        public decimal cm_t73 { get; set; }
        public decimal cm_t74 { get; set; }
        public decimal cm_t75 { get; set; }
        public decimal cm_t76 { get; set; }
        public decimal cm_t77 { get; set; }
        public decimal cm_t78 { get; set; }
        public decimal cm_t79 { get; set; }
        public decimal cm_t80 { get; set; }
        public decimal cm_t81 { get; set; }
        public decimal cm_t82 { get; set; }
        public decimal cm_t83 { get; set; }
        public decimal cm_t84 { get; set; }
        public decimal cm_t85 { get; set; }
        public decimal cm_t86 { get; set; }
        public decimal cm_t87 { get; set; }
        public decimal cm_t88 { get; set; }
        public decimal cm_t89 { get; set; }
        public decimal cm_t90 { get; set; }
        public decimal cm_t91 { get; set; }
        public decimal cm_t92 { get; set; }
        public decimal cm_t93 { get; set; }
        public decimal cm_t94 { get; set; }
        public decimal cm_t95 { get; set; }
        public decimal cm_t96 { get; set; }
        #endregion
    }

    public partial class GmmDatCalculoDTO : EntityBase
    {
        // Campo genérico para grabar el usuario de creación o modificación
        public string DINSUSUARIO { get; set; }
        public int formatoDemandaTrimestral { get; set; }
        public int formatoDemandaMensual { get; set; }
        public bool EmpgPrimerMes { get; set; }
        
    }
}
