using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace thread
{
    class Saldo
    {
        public int Investidor { get; set; }
        public int Ativo { get; set; }
        public Decimal Quantidade { get; set; }

        public Saldo(int investidor, int ativo, decimal quantidade)
        {
            Investidor = investidor;
            Ativo = ativo;
            Quantidade = quantidade;
        }

        public void Debitar(Decimal quantidade)
        {
            Quantidade -= quantidade;
        }
        public void Creditar(Decimal quantidade)
        {
            Quantidade -= quantidade;
        }
    }
}