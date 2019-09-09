using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace thread
{
    class Transferencia
    {
        public int InvestidorOriem { get; set; }
        public int InvestidorDestino { get; set; }
        public int Ativo { get; set; }
        public Decimal Quantidade { get; set; }
        public Decimal QuantidadeMovimentada { get; set; }
        public Boolean Processada { get; set; }
        public Transferencia(int investidorOriem, int investidorDestino, int ativo, decimal quantidade)
        {
            InvestidorOriem = investidorOriem;
            InvestidorDestino = investidorDestino;
            Ativo = ativo;
            Quantidade = quantidade;
            Processada = false;
        }
        public void SetQuantidadeMovimentada(Decimal quantidade)
        {
            QuantidadeMovimentada = quantidade;
            Processada = true;
        }
    }
}