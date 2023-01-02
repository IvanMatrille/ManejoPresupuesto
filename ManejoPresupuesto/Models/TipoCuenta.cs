using ManejoPresupuesto.Models.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo nombre es requerido")]
        [PrimeraLetraMayuscula]
        //[Remote(action: "VerificaExisteTipoCuenta", controller: "TiposCuentas")]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
         public int Orden { get; set; }

        /*Prueba de otras validaciones por defecto*/
        

    }
}
