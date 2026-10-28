namespace Conde.Divisao;

/// <summary>
/// As medidas do README. Os testes conferem o que estes métodos devolvem; o
/// medidor só imprime.
/// </summary>
public static class Medidas
{
    public readonly record struct LinhaExiste(
        int Pessoas, int Fatias, int MaiorValor, long Bolos, long ComSemInveja, long ComProporcional, double Fracao);

    public readonly record struct LinhaProtocolo(
        string Protocolo, long Bolos, long Proporcionais, long SemInveja, long Otimas, double Fracao);

    public readonly record struct LinhaBolo(
        string Bolo, long Divisoes, long Proporcionais, long SemInveja, long Eficientes, int MenorInveja);

    public readonly record struct LinhaRelacao(
        int Pessoas, long Divisoes, long SoProporcional, long SoSemInveja, long Ambas, bool UmaImplicaAOutra);

    /// <summary>
    /// Em quantos bolos existe divisão sem inveja. Com fatias indivisíveis a
    /// resposta deixa de ser "sempre", e esta tabela mede quando.
    /// </summary>
    public static List<LinhaExiste> Existe()
    {
        var linhas = new List<LinhaExiste>();
        foreach (var (pessoas, fatias, maior) in new[]
                 {
                     (2, 1, 1), (2, 2, 1), (2, 3, 1), (2, 4, 1), (2, 2, 2), (2, 3, 2), (3, 3, 1), (3, 4, 1),
                 })
        {
            long bolos = 0, comSemInveja = 0, comProporcional = 0;
            foreach (var bolo in Varredura.TodosOsBolos(pessoas, fatias, maior))
            {
                bolos++;
                var achouSemInveja = false;
                var achouProporcional = false;
                foreach (var dono in Varredura.Todas(bolo))
                {
                    if (!achouSemInveja && Juiz.SemInveja(bolo, dono)) achouSemInveja = true;
                    if (!achouProporcional && Juiz.Proporcional(bolo, dono)) achouProporcional = true;
                    if (achouSemInveja && achouProporcional) break;
                }
                if (achouSemInveja) comSemInveja++;
                if (achouProporcional) comProporcional++;
            }

            linhas.Add(new LinhaExiste(pessoas, fatias, maior, bolos, comSemInveja, comProporcional, (double)comSemInveja / bolos));
        }
        return linhas;
    }

    /// <summary>
    /// Os protocolos contra a varredura, em todos os bolos de 2 pessoas e 4
    /// fatias com valores de 0 a 2. A coluna das ótimas diz em quantos o
    /// protocolo achou uma divisão com a menor inveja possível.
    /// </summary>
    public static List<LinhaProtocolo> Protocolos()
    {
        var resultados = new Dictionary<string, (long Proporcional, long SemInveja, long Otimas)>
        {
            ["corta e escolhe"] = (0, 0, 0),
            ["guloso"] = (0, 0, 0),
            ["rodizio"] = (0, 0, 0),
        };

        long bolos = 0;
        foreach (var bolo in Varredura.TodosOsBolos(2, 4, 2))
        {
            bolos++;
            var menorInveja = Juiz.Inveja(bolo, Conde.Divisao.Protocolos.MenorInveja(bolo));

            foreach (var (nome, divisao) in new[]
                     {
                         ("corta e escolhe", Conde.Divisao.Protocolos.CortaEEscolhe(bolo)),
                         ("guloso", Conde.Divisao.Protocolos.Guloso(bolo)),
                         ("rodizio", Conde.Divisao.Protocolos.Rodizio(bolo)),
                     })
            {
                var atual = resultados[nome];
                if (Juiz.Proporcional(bolo, divisao)) atual.Proporcional++;
                if (Juiz.SemInveja(bolo, divisao)) atual.SemInveja++;
                if (Juiz.Inveja(bolo, divisao) == menorInveja) atual.Otimas++;
                resultados[nome] = atual;
            }
        }

        return resultados
            .Select(r => new LinhaProtocolo(r.Key, bolos, r.Value.Proporcional, r.Value.SemInveja, r.Value.Otimas, (double)r.Value.SemInveja / bolos))
            .OrderByDescending(l => l.SemInveja)
            .ToList();
    }

    /// <summary>Três bolos escolhidos a dedo, varridos inteiros.</summary>
    public static List<LinhaBolo> BolosDeExemplo()
    {
        var bolos = new (string Nome, int[,] Valores)[]
        {
            ("gostos iguais", new[,] { { 3, 2, 1 }, { 3, 2, 1 } }),
            ("gostos opostos", new[,] { { 3, 2, 1 }, { 1, 2, 3 } }),
            ("uma fatia so interessa", new[,] { { 5, 0, 0 }, { 5, 0, 0 } }),
            ("tres pessoas, quatro fatias", new[,] { { 4, 3, 2, 1 }, { 1, 4, 3, 2 }, { 2, 1, 4, 3 } }),
        };

        var linhas = new List<LinhaBolo>();
        foreach (var (nome, valores) in bolos)
        {
            var bolo = new Bolo(valores);
            var resumo = Varredura.Resume(bolo);
            linhas.Add(new LinhaBolo(
                nome, resumo.Divisoes, resumo.Proporcionais, resumo.SemInveja, resumo.Eficientes, resumo.MenorInveja));
        }
        return linhas;
    }

    /// <summary>
    /// A relação entre as duas garantias, contada divisão por divisão em todos os
    /// bolos de um tamanho. Com duas pessoas elas são a mesma coisa; com três,
    /// não.
    /// </summary>
    public static List<LinhaRelacao> Relacao()
    {
        var linhas = new List<LinhaRelacao>();
        foreach (var (pessoas, fatias, maior) in new[] { (2, 3, 2), (3, 3, 1), (3, 4, 1) })
        {
            long divisoes = 0, soProporcional = 0, soSemInveja = 0, ambas = 0;

            foreach (var bolo in Varredura.TodosOsBolos(pessoas, fatias, maior))
                foreach (var dono in Varredura.Todas(bolo))
                {
                    divisoes++;
                    var proporcional = Juiz.Proporcional(bolo, dono);
                    var semInveja = Juiz.SemInveja(bolo, dono);
                    if (proporcional && semInveja) ambas++;
                    else if (proporcional) soProporcional++;
                    else if (semInveja) soSemInveja++;
                }

            linhas.Add(new LinhaRelacao(pessoas, divisoes, soProporcional, soSemInveja, ambas, soSemInveja == 0));
        }
        return linhas;
    }
}
