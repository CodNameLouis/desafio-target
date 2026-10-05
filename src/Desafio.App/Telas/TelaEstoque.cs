using Desafio.Core.Estoque;

namespace Desafio.App.Telas;

internal sealed class TelaEstoque(ControleEstoque controle)
{
    public void Exibir()
    {
        while (true)
        {
            Entrada.LimparTela();
            Console.WriteLine("=== 2. Movimentação de estoque ===");
            Console.WriteLine();
            ListarProdutos();
            Console.WriteLine();
            Console.WriteLine("[1] Lançar entrada");
            Console.WriteLine("[2] Lançar saída");
            Console.WriteLine("[3] Histórico de movimentações");
            Console.WriteLine("[0] Voltar");
            Console.WriteLine();

            switch (Entrada.Texto("Opção: "))
            {
                case "1": Lancar(TipoMovimentacao.Entrada); break;
                case "2": Lancar(TipoMovimentacao.Saida); break;
                case "3": ListarHistorico(); break;
                case "0": return;
                default: Entrada.Erro("Opção inválida."); Entrada.Pausar(); break;
            }
        }
    }

    private void ListarProdutos()
    {
        Console.WriteLine($"{"Código",-8} {"Produto",-28} {"Estoque",8}");
        Console.WriteLine(new string('-', 46));
        foreach (var p in controle.Produtos.OrderBy(p => p.Codigo))
            Console.WriteLine($"{p.Codigo,-8} {p.Descricao,-28} {p.Quantidade,8}");
    }

    private void Lancar(TipoMovimentacao tipo)
    {
        Console.WriteLine();
        Console.WriteLine($"--- Nova {(tipo == TipoMovimentacao.Entrada ? "ENTRADA" : "SAÍDA")} ---");

        Produto produto;
        while (true)
        {
            var codigo = Entrada.Inteiro("Código do produto: ");
            try
            {
                produto = controle.ObterProduto(codigo);
                break;
            }
            catch (KeyNotFoundException ex)
            {
                Entrada.Erro(ex.Message);
            }
        }

        Console.WriteLine($"Produto: {produto.Descricao} (estoque atual: {produto.Quantidade})");
        var quantidade = Entrada.Inteiro("Quantidade: ", minimo: 1);
        var descricao = Entrada.Texto(tipo == TipoMovimentacao.Entrada
            ? "Descrição (ex.: Compra de fornecedor, Devolução de cliente): "
            : "Descrição (ex.: Venda, Perda/avaria, Uso interno): ");

        try
        {
            var mov = controle.Movimentar(produto.Codigo, tipo, quantidade, descricao);
            Console.WriteLine();
            Entrada.Sucesso($"Movimentação #{mov.Id} registrada com sucesso.");
            Console.WriteLine($"  {produto.Descricao}: {mov.EstoqueAnterior} {(tipo == TipoMovimentacao.Entrada ? "+" : "-")} {mov.Quantidade}");
            Entrada.Sucesso($"  Estoque final: {mov.EstoqueFinal}");
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
        {
            Entrada.Erro(ex.Message);
        }

        Entrada.Pausar();
    }

    private void ListarHistorico()
    {
        Console.WriteLine();
        if (controle.Movimentacoes.Count == 0)
        {
            Console.WriteLine("Nenhuma movimentação lançada nesta sessão.");
            Entrada.Pausar();
            return;
        }

        Console.WriteLine($"{"Id",-4} {"Data/hora",-19} {"Produto",-8} {"Tipo",-8} {"Qtde",6} {"Final",7}  Descrição");
        Console.WriteLine(new string('-', 80));
        foreach (var m in controle.Movimentacoes)
        {
            Console.WriteLine(
                $"{m.Id,-4} {m.DataHora,-19:dd/MM/yyyy HH:mm:ss} {m.CodigoProduto,-8} {(m.Tipo == TipoMovimentacao.Entrada ? "Entrada" : "Saída"),-8} " +
                $"{m.Quantidade,6} {m.EstoqueFinal,7}  {m.Descricao}");
        }

        Entrada.Pausar();
    }
}
