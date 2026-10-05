using System.Globalization;
using System.Text;
using Desafio.App;
using Desafio.App.Telas;
using Desafio.Core.Estoque;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");

var pastaDados = Path.Combine(AppContext.BaseDirectory, "data");
var caminhoVendas = Path.Combine(pastaDados, "vendas.json");
var caminhoEstoque = Path.Combine(pastaDados, "estoque.json");

// O estoque é carregado uma vez e mantido em memória durante a execução,
// para que as movimentações se acumulem entre um lançamento e outro.
var telaEstoque = new TelaEstoque(new ControleEstoque(LeitorEstoque.LerArquivo(caminhoEstoque)));

while (true)
{
    Entrada.LimparTela();
    Console.WriteLine("==============================");
    Console.WriteLine("   Desafio Target Sistemas");
    Console.WriteLine("==============================");
    Console.WriteLine("[1] Comissão dos vendedores");
    Console.WriteLine("[2] Movimentação de estoque");
    Console.WriteLine("[3] Cálculo de juros");
    Console.WriteLine("[0] Sair");
    Console.WriteLine();

    var opcao = Entrada.Texto("Opção: ");
    Entrada.LimparTela();

    switch (opcao)
    {
        case "1": TelaComissoes.Exibir(caminhoVendas); Entrada.Pausar(); break;
        case "2": telaEstoque.Exibir(); break;
        case "3": TelaJuros.Exibir(); Entrada.Pausar(); break;
        case "0": return;
        default: Entrada.Erro("Opção inválida."); Entrada.Pausar(); break;
    }
}
