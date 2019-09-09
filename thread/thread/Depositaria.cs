using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace thread
{
    class Depositaria
    {
        private List<Saldo> _saldo;
        private List<Movimento> _movimentos;

        public Depositaria(List<Saldo> saldo, List<Movimento> movimento)
        {
            _saldo = saldo;
            _movimentos = movimento;
        }
        public void Transferir(Transferencia transferencia)
        {
            var saldoOrigem = GetSaldo(transferencia.InvestidorOriem, transferencia.Ativo);
            Decimal quantidadeMovimentada = 0;
            if (saldoOrigem!=null)
            {
                if (saldoOrigem.Quantidade>=transferencia.Quantidade)
                {
                    quantidadeMovimentada = transferencia.Quantidade;
                }
                else
                {
                    quantidadeMovimentada = saldoOrigem.Quantidade;
                }
            }
            if (quantidadeMovimentada > 0)
            {
                saldoOrigem.Debitar(quantidadeMovimentada);
                var saldoDestino = GetSaldo(transferencia.InvestidorDestino, transferencia.Ativo);
                if (saldoDestino == null)
                {
                    saldoDestino = new Saldo(transferencia.InvestidorOriem, transferencia.Ativo, quantidadeMovimentada);
                    _saldo.Add(saldoDestino);
                }
                else
                {
                    saldoDestino.Creditar(quantidadeMovimentada);
                }
                _movimentos.Add(new Movimento(transferencia.InvestidorOriem, transferencia.InvestidorDestino, transferencia.Ativo, quantidadeMovimentada));

            }
            transferencia.SetQuantidadeMovimentada(quantidadeMovimentada);
            
        }
        public Saldo GetSaldo(int investidor, int ativo)
        {
            return _saldo.FirstOrDefault(s => s.Investidor==investidor && s.Ativo == ativo);
        }


        public void TransferirLote(List<Transferencia> transferencias)
        {
            int contador = 0;
            //Console.WriteLine("Processando {0} transferencias {1}", transferencias.Count, DateTime.Now);

            foreach (var transferencia in transferencias)
            {
                contador++;
                if (contador % 1000 == 0)
                {
                    Console.WriteLine("Transferindo {0} de {1} - {2}", contador, transferencias.Count, DateTime.Now);
                }
                Transferir(transferencia);
            }

        }
    }
}