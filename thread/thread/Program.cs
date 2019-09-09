using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace thread
{
    class Program
    {
        private static List<Saldo> _saldo;
        private static List<Transferencia> _transferencias;
        private static List<Movimento> _movimentos;

        static void Main(string[] args)
        {
            _movimentos = new List<Movimento>();
            CriaSaldo(200000, 100);
            CarregaTransferencia(10000, 100);

            var tempoInicio = DateTime.Now;
            Console.WriteLine("Processando: {0} saldos e {1} transferencias {2}", _saldo.Count, _transferencias.Count, DateTime.Now);

            //var depositaria = new Depositaria(_saldo, _movimentos);
            //depositaria.TransferirLote(_transferencias);

            Console.WriteLine("Dividindo lotes de ativos");
            var queryTransferencia =
                from transferencia in _transferencias
                group transferencia by transferencia.Ativo into transfPorAtivo
                select new {
                    Ativo = transfPorAtivo.Key,
                    Saldo = _saldo.Where(s => s.Ativo == transfPorAtivo.Key).ToList(),
                    Movimentos = new List<Movimento>(),
                    Transferencias = transfPorAtivo.ToList()
                };
            var listaLote = queryTransferencia.ToList();
            Console.WriteLine("Inicio da transferência");
            //foreach (var transfPorAtivo in queryTransferencia)
            //{
            //    Console.WriteLine("Transferindo ativo {0} que tem {1} transferencias - {2};", transfPorAtivo.Key, transfPorAtivo.Count(), DateTime.Now);
            //    TransferirLote(transfPorAtivo.ToList());
            //}
            Parallel.ForEach(listaLote, transf =>
            {
                Console.WriteLine("Inicio da thread do ativo {0} com {1} transferencias - {2}", transf.Ativo, transf.Transferencias.Count, DateTime.Now);
                var depositaria = new Depositaria(transf.Saldo, transf.Movimentos);
                depositaria.TransferirLote(transf.Transferencias);
            });
            Console.WriteLine("fim do processamento de threads, atualizando saldo {0}", DateTime.Now);

            foreach (var item in listaLote)
            {
                _saldo.RemoveAll(s => s.Ativo == item.Ativo);
                _saldo.AddRange(item.Saldo);
                _movimentos.AddRange(item.Movimentos);
            }

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Processamento finalizado {0} - {1}", tempoInicio , DateTime.Now);
            Console.WriteLine("Transferencias processadas: {0}", _transferencias.Count(t => t.Processada));
            Console.WriteLine("Transferencias com sucesso: {0}", _transferencias.Count(t => t.Quantidade == t.QuantidadeMovimentada));
            Console.WriteLine("Transferencias parcial: {0}", _transferencias.Count(t => t.Quantidade != t.QuantidadeMovimentada));
            Console.WriteLine("Transferencias Sem Saldo: {0}", _transferencias.Count(t => t.QuantidadeMovimentada == 0));
            Console.WriteLine("Movimentos: {0}", _movimentos.Count);
            Console.WriteLine("-----------------------------------------");
            Console.ReadLine();

        }
        

        private static void CarregaTransferencia(int quantidade, int ativos)
        {
            _transferencias = new List<Transferencia>();
            Random randNum = new Random();
            for (int inv = 0; inv < quantidade; inv++)
            {
                _transferencias.Add(new Transferencia(inv, inv + quantidade, randNum.Next(ativos-10), randNum.Next(1000)));
                _transferencias.Add(new Transferencia(inv, inv + quantidade, randNum.Next(ativos-10), randNum.Next(1000)));
                _transferencias.Add(new Transferencia(inv, inv + quantidade, randNum.Next(ativos/2), randNum.Next(1000)));
                _transferencias.Add(new Transferencia(inv, inv + quantidade, randNum.Next(ativos/2), randNum.Next(1000)));
                _transferencias.Add(new Transferencia(inv, inv + quantidade, randNum.Next(ativos/2), randNum.Next(1000)));
            }
        }

        private static void CriaSaldo(int investidores , int ativos)
        {
            Decimal quantidade = 500;
            _saldo = new List<Saldo>();
            for (int inv = 1; inv < investidores + 1; inv++)
            {
                var randNum = new Random();
                var quantidadeAleatoriaAtivosPorInvestidor = randNum.Next(ativos);
                quantidadeAleatoriaAtivosPorInvestidor = randNum.Next(quantidadeAleatoriaAtivosPorInvestidor); //Roda duas vezes para baixar a distribuiçaod e ativos
                for (int ativ = 1; ativ < quantidadeAleatoriaAtivosPorInvestidor + 1; ativ++)
                {
                    _saldo.Add(new Saldo(inv, ativ, quantidade) );
                }
            }
        }

        private static void ListaSaldo(int investidor)
        {
            var saldo = _saldo.FindAll(s=>s.Investidor==investidor);
            foreach (var item in saldo)
            {
                Console.WriteLine("Investidor {0} Ativo {1} Quantidade{2}", item.Investidor, item.Ativo, item.Quantidade);
            }
        }
    }
}
