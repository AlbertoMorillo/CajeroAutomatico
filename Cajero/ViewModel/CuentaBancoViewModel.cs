using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cajero.ViewModel
{
    public class CuentaBancoViewModel
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public string NoCuenta { get; set; }
        public string PIN { get; set; }
        public string IdUser { get; set; }
        public int Deposito { get; set; }
        public int Retiro { get; set; }
    }
}