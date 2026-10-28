namespace Conde.Divisao;

/// <summary>
/// As quatro perguntas que se faz a uma divisão, cada uma pela definição e sem
/// saber como a divisão foi feita.
/// </summary>
public static class Juiz
{
    /// <summary>
    /// Proporcional: cada um recebe pelo menos a enésima parte do bolo, medida
    /// pelos próprios olhos. Com fatias indivisíveis isso é contado com arredondamento
    /// para cima do que cabe, ou seja, n vezes o lote tem de dar pelo menos o total.
    /// </summary>
    public static bool Proporcional(Bolo bolo, IReadOnlyList<int> donoDaFatia)
    {
        ArgumentNullException.ThrowIfNull(bolo);
        for (var p = 0; p < bolo.Pessoas; p++)
            if (bolo.Pessoas * bolo.ValorDoLote(p, donoDaFatia) < bolo.Total(p))
                return false;
        return true;
    }

    /// <summary>
    /// Sem inveja: ninguém prefere o lote de outro ao próprio. É mais forte que
    /// proporcional, e com três pessoas ou mais as duas coisas deixam de andar
    /// juntas.
    /// </summary>
    public static bool SemInveja(Bolo bolo, IReadOnlyList<int> donoDaFatia)
    {
        ArgumentNullException.ThrowIfNull(bolo);
        for (var p = 0; p < bolo.Pessoas; p++)
        {
            var meu = bolo.ValorDoLote(p, donoDaFatia);
            for (var outro = 0; outro < bolo.Pessoas; outro++)
                if (outro != p && bolo.ValorDoLoteAlheio(p, outro, donoDaFatia) > meu)
                    return false;
        }
        return true;
    }

    /// <summary>
    /// Quanta inveja existe: a maior diferença entre o que alguém vê no prato do
    /// outro e o que vê no próprio. Zero é "sem inveja".
    /// </summary>
    public static int Inveja(Bolo bolo, IReadOnlyList<int> donoDaFatia)
    {
        ArgumentNullException.ThrowIfNull(bolo);
        var pior = 0;
        for (var p = 0; p < bolo.Pessoas; p++)
        {
            var meu = bolo.ValorDoLote(p, donoDaFatia);
            for (var outro = 0; outro < bolo.Pessoas; outro++)
            {
                if (outro == p) continue;
                pior = Math.Max(pior, bolo.ValorDoLoteAlheio(p, outro, donoDaFatia) - meu);
            }
        }
        return pior;
    }

    /// <summary>
    /// Eficiente no sentido de Pareto: não existe outra divisão em que ninguém
    /// perca e alguém ganhe. É a única das quatro perguntas que precisa olhar
    /// TODAS as outras divisões para ser respondida.
    /// </summary>
    public static bool Eficiente(Bolo bolo, IReadOnlyList<int> donoDaFatia)
    {
        ArgumentNullException.ThrowIfNull(bolo);
        var meu = new int[bolo.Pessoas];
        for (var p = 0; p < bolo.Pessoas; p++) meu[p] = bolo.ValorDoLote(p, donoDaFatia);

        foreach (var outra in Varredura.Todas(bolo))
        {
            var algumGanhou = false;
            var algumPerdeu = false;
            for (var p = 0; p < bolo.Pessoas; p++)
            {
                var valor = bolo.ValorDoLote(p, outra);
                if (valor > meu[p]) algumGanhou = true;
                if (valor < meu[p]) algumPerdeu = true;
            }
            if (algumGanhou && !algumPerdeu) return false;
        }

        return true;
    }

    /// <summary>
    /// A divisão é uma atribuição válida: cada fatia tem exatamente um dono, e o
    /// dono é uma das pessoas.
    /// </summary>
    public static bool EhDivisao(Bolo bolo, IReadOnlyList<int> donoDaFatia)
    {
        ArgumentNullException.ThrowIfNull(bolo);
        ArgumentNullException.ThrowIfNull(donoDaFatia);
        if (donoDaFatia.Count != bolo.Fatias) return false;
        foreach (var dono in donoDaFatia)
            if (dono < 0 || dono >= bolo.Pessoas)
                return false;
        return true;
    }
}
