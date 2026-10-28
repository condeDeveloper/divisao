# Divisão justa

"Eu corto, você escolhe" é a receita mais antiga de divisão justa que existe, e
ela funciona: com duas pessoas e um bolo que dá para cortar em qualquer lugar,
ninguém sai invejando ninguém.

Este repositório tira o "qualquer lugar". O bolo já vem cortado em fatias
**indivisíveis** — as fatias da festa, os móveis do divórcio, os brinquedos entre
dois irmãos — e aí quase tudo que o teorema promete deixa de valer. Em C# e .NET
8, com todas as divisões e **todos os bolos** varridos.

```bash
dotnet test testes/Divisao.Testes/Divisao.Testes.csproj -c Release    # 49 testes
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release
```

## As quatro perguntas

**Proporcional**: cada um recebe pelo menos a enésima parte do bolo, medida pelos
próprios olhos. **Sem inveja**: ninguém prefere o lote de outro ao próprio.
**Eficiente**: não existe outra divisão em que ninguém perca e alguém ganhe. E a
quarta é "quanta inveja", para quando o zero não é alcançável.

```
  bolo                          divisoes  proporcionais  sem inveja  eficientes  menor inveja
  gostos iguais                        8              2           2           8             0
  gostos opostos                       8              2           2           4             0
  uma fatia so interessa               8              0           0           8             5
  tres pessoas, quatro fatias         81              3           1          27             0
```

A terceira linha é o caso que estraga tudo. Duas pessoas, três fatias, e só a
primeira fatia vale alguma coisa — para as duas. **Nenhuma das oito divisões é
justa**, de nenhum dos dois jeitos: quem não ficar com a fatia boa fica com nada.
E as oito são eficientes, porque não há o que melhorar sem piorar alguém. Um bolo
em que toda divisão é eficiente e nenhuma é justa.

## Quando a divisão sem inveja existe

```
   pessoas  fatias  valores ate     bolos  tem sem inveja  tem proporcional   fracao
         2       1            1         4               3                 3   75.0%
         2       2            1        16              14                14   87.5%
         2       3            1        64              60                60   93.8%
         2       4            1       256             248               248   96.9%
         2       2            2        81              63                63   77.8%
         2       3            2       729             656               656   90.0%
         3       3            1       512             407               407   79.5%
         3       4            1     4,096           3,767             3,791   92.0%
```

Aqui não é um bolo: são **todos os bolos** daquele tamanho, com cada pessoa dando
a cada fatia um valor de 0 ao teto. Para cada um, a pergunta é se existe pelo
menos uma divisão sem inveja.

Com bolo contínuo a resposta seria "sempre". Com fatias indivisíveis, **nunca é
sempre**: 75% com uma fatia, 92% com quatro. Mais fatias ajudam, porque mais
fatias é mais parecido com um bolo que dá para cortar onde se quer.

E olhe a última linha. Com três pessoas, 3.791 bolos têm divisão proporcional e só
3.767 têm divisão sem inveja: **24 bolos em que dá para todo mundo receber a
terça parte e ainda assim alguém olha o prato do vizinho**. Com duas pessoas essa
diferença é zero em todas as linhas.

## As duas garantias, divisão por divisão

```
   pessoas     divisoes  so proporcional  so sem inveja   as duas  sem inveja implica
         2        5,832                0              0     1,434                 sim
         3       13,824              126              0     1,278                 sim
         3      331,776            8,964              0    26,958                 sim
```

A coluna `só sem inveja` é zero em todas as linhas, e isso não é acaso: se ninguém
inveja ninguém, cada um acha que tem pelo menos a média, e a média é a enésima
parte. **Sem inveja implica proporcional, sempre.**

O contrário falha, e falha muito: 8.964 divisões proporcionais e invejosas na
última linha. Com duas pessoas as duas ideias são a mesma; com três, "recebi a
minha parte" e "não quero a do outro" se separam.

## Os protocolos

```
  protocolo              bolos  proporcionais  sem inveja  com a menor inveja   fracao
  rodizio                6,561          5,272       5,272               5,484   80.4%
  guloso                 6,561          3,909       3,909               4,033   59.6%
  corta e escolhe        6,561          3,759       3,759               3,971   57.3%
```

Todos os 6.561 bolos de duas pessoas e quatro fatias, com os três protocolos
rodando em cada um.

**"Eu corto, você escolhe" é o pior dos três.** O protocolo mais famoso da divisão
justa, o que vem com teorema e tudo, entrega divisão sem inveja em 57,3% dos
bolos, contra 80,4% do rodízio — simplesmente alternar quem escolhe a próxima
fatia.

O motivo é que o teorema do corta-e-escolhe depende de o cortador **conseguir**
fazer dois lotes que ele ache iguais. Com fatias indivisíveis ele muitas vezes não
consegue: faz o corte mais parecido que dá, o outro escolhe o lado melhor, e sobra
inveja do lado de quem cortou. O escolhedor nunca inveja ninguém — isso continua
valendo, e está testado. Quem inveja é sempre o cortador.

## O que as medidas me corrigiram

**Achei que o corta-e-escolhe fosse pelo menos empatar.** É o protocolo com nome,
com teorema, com prova. Ele perde para o rodízio por 23 pontos. O teorema não está
errado: a hipótese dele é que o bolo dá para cortar em qualquer lugar, e essa
hipótese é o protocolo inteiro. **Tirar a hipótese não deixa o resultado mais
fraco, deixa outro resultado.**

**Escrevi "sem inveja é mais forte que proporcional" sem saber se era sempre.**
É, e agora está contado: zero divisões "só sem inveja" em 351 mil divisões de três
tamanhos diferentes. A implicação contrária falha em 8.964 delas. Era uma frase de
livro que virou uma coluna com zero.

**O caso que quebra tudo é pequeno demais para eu ter imaginado.** Duas pessoas,
uma fatia que vale 5 para as duas, duas fatias que não valem nada. Nenhuma das
oito divisões é proporcional. Eu procurava o contraexemplo em bolos grandes e
complicados, e ele cabe numa linha.

**A eficiência de Pareto é a única pergunta que não é sobre a divisão.** As outras
três se respondem olhando a divisão. Essa precisa olhar **todas as outras**, o que
faz o custo da varredura saltar de linear para quadrático no número de divisões.
Foi preciso um botão para desligá-la nas tabelas grandes, e isso virou um
parâmetro com nome honesto em vez de um `if` escondido.

## Os arquivos

| arquivo | o que faz |
|---|---|
| `src/Divisao/Bolo.cs` | as fatias, e quanto cada pessoa acha que cada uma vale |
| `src/Divisao/Juiz.cs` | proporcional, sem inveja, quanta inveja e eficiente, pela definição |
| `src/Divisao/Varredura.cs` | todas as divisões de um bolo, e todos os bolos de um tamanho |
| `src/Divisao/Protocolos.cs` | corta e escolhe, guloso, rodízio, e a menor inveja achada varrendo |
| `src/Divisao/Medidas.cs` | as quatro tabelas deste README |
| `ferramentas/Medidor` | imprime as tabelas, uma por comando |

## Rodando

```bash
dotnet test testes/Divisao.Testes/Divisao.Testes.csproj -c Release

dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- existe
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- bolos
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- protocolos
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- relacao
```

C# 12 e .NET 8. Zero dependências fora do xUnit, e só nos testes.

49 testes. Os números deste README estão presos em `MedidasTestes`.
