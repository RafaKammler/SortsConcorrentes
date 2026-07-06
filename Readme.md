# Ordenação Concorrente - Visualizador WPF 

Um projeto desenvolvido em **C#** utilizando **WPF (Windows Presentation Foundation)** para visualizar e comparar a execução de algoritmos de ordenação em tempo real. O aplicativo executa diferentes métodos de ordenação de forma concorrente (usando *multithreading*), exibindo a diferença de desempenho e complexidade visualmente.

## Funcionalidades

* **Execução Concorrente:** O *Bubble Sort* e o *Quick Sort* rodam simultaneamente em *threads* separadas.
* **Visualização em Tempo Real:** Representação gráfica dos arrays usando gráficos de barras verticais.
* **Métricas ao Vivo:** Contagem do tempo de execução (cronômetro) e número de operações realizadas por cada algoritmo.
* **Controle de Estado:** Funcionalidade para iniciar a ordenação e um botão de "Resetar" que utiliza `CancellationTokenSource` para interromper as *threads* de forma limpa e segura.
* **Interface Fluida:** Utilização de `Dispatcher.BeginInvoke` para atualizações assíncronas da interface, prevenindo o congelamento da tela (UI freeze) durante cálculos pesados.

## Tecnologias Utilizadas

* **Linguagem:** C# (.NET)
* **Interface Gráfica:** WPF e XAML
* **Conceitos Aplicados:** 
  * Orientação a Objetos
  * Concorrência e Paralelismo (*Multithreading* / *Tasks*)
  * *Data Binding* e Clonagem de Estados
  * Controle de execução assíncrona

## Como Executar

1. Clone este repositório para a sua máquina local:
   ```bash
   git clone [https://github.com/seu-usuario/nome-do-repositorio.git](https://github.com/seu-usuario/nome-do-repositorio.git)