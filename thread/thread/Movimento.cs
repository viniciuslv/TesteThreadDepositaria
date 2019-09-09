using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace thread
{
    class Movimento
    {
        public int InvestidorOriem { get; set; }
        public int InvestidorDestino { get; set; }
        public int Ativo { get; set; }
        public Decimal Quantidade { get; set; }
        public DateTime Data { get; set; }

        public Movimento(int investidorOriem, int investidorDestino, int ativo, decimal quantidade)
        {
            InvestidorOriem = investidorOriem;
            InvestidorDestino = investidorDestino;
            Ativo = ativo;
            Quantidade = quantidade;
        }
    }
}