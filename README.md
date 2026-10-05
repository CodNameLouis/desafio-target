# desafio-target

Solução do desafio técnico para a vaga de **Desenvolvedor(a) de Sistemas – Target Sistemas**, feita em **C# / .NET 10**.

Os três exercícios estão em uma aplicação de console com menu, e as regras de negócio ficam em uma biblioteca separada, coberta por testes automatizados.

## Como executar

Pré-requisito: [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project src/Desafio.App
```

Rodar os testes:

```bash
dotnet test
```

## Estrutura

```
data/
  vendas.json            # dados do exercício 1
  estoque.json           # dados do exercício 2
src/
  Desafio.Core/          # regras de negócio (sem dependência de console)
    Comissoes/           # exercício 1
    Estoque/             # exercício 2
    Juros/               # exercício 3
  Desafio.App/           # aplicação de console (menu e telas)
tests/
  Desafio.Tests/         # testes xUnit das regras de negócio
```

## 1. Comissão dos vendedores

Lê `data/vendas.json` e calcula a comissão de **cada venda** conforme a faixa:

| Valor da venda          | Comissão |
|-------------------------|----------|
| abaixo de R$ 100,00     | 0%       |
| abaixo de R$ 500,00     | 1%       |
| a partir de R$ 500,00   | 5%       |

As comissões são somadas por vendedor. Os valores usam `decimal` (sem erro de ponto flutuante) e o arredondamento para centavos é feito apenas no total de cada vendedor.

Resultado com os dados do desafio:

| Vendedor        | Vendas | Total vendido | Comissão  |
|-----------------|-------:|--------------:|----------:|
| João Silva      | 10     | R$ 10.754,70  | R$ 495,68 |
| Maria Souza     | 9      | R$ 9.874,30   | R$ 465,95 |
| Ana Lima        | 9      | R$ 8.763,95   | R$ 404,98 |
| Carlos Oliveira | 8      | R$ 7.928,35   | R$ 379,37 |
| **Total**       | **36** | **R$ 37.321,30** | **R$ 1.745,98** |

## 2. Movimentação de estoque

Carrega os produtos de `data/estoque.json` e permite lançar **entradas** e **saídas**. Cada movimentação tem:

- **Id** único e sequencial;
- **Descrição** livre identificando a operação (ex.: "Compra de fornecedor", "Venda", "Perda/avaria");
- tipo, quantidade, data/hora, estoque anterior e **estoque final**, que é exibido ao concluir o lançamento.

Validações: produto deve existir, quantidade maior que zero, descrição obrigatória e saída não pode deixar o estoque negativo. Há também um histórico das movimentações da sessão. O estoque é mantido em memória durante a execução; o arquivo JSON original não é alterado.

## 3. Cálculo de juros

Informados um valor e uma data de vencimento, calcula os juros na data de hoje com **2,5% ao dia** (juros simples sobre o valor original):

```
juros = valor × 2,5% × dias de atraso
```

- Dias de atraso = diferença entre hoje e o vencimento; se o título vence hoje ou no futuro, não há juros.
- O resultado é arredondado para centavos.

Exemplo: R$ 1.000,00 vencido há 10 dias → juros de R$ 250,00 → total de R$ 1.250,00.
