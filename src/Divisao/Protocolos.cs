namespace Conde.Divisao;

/// <summary>
/// Os jeitos de dividir sem olhar todas as divisões. Cada um é um procedimento
/// que uma pessoa de verdade conseguiria executar numa mesa de aniversário.
/// </summary>
public static class Protocolos
{
    /// <summary>
    /// "Eu corto, você escolhe", com duas pessoas. A primeira parte as fatias nos
    /// dois lotes mais parecidos possíveis **pelos olhos dela**, e a segunda
    /// escolhe o lote que preferir.
    /// </summary>
    public static int[] CortaEEscolhe(Bolo bolo)
    {
        ArgumentNullException.ThrowIfNull(bolo);
        if (bolo.Pessoas != 2) throw new ArgumentException("corta e escolhe é para duas pessoas", nameof(bolo));

        // O cortador procura o corte mais equilibrado para ele.
        int[]? melhorCorte = null;
        var menorDiferenca = int.MaxValue;

        foreach (var dono in Varredura.Todas(bolo))
        {
            var meu = bolo.ValorDoLote(0, dono);
            var outro = bolo.ValorDoLoteAlheio(0, 1, dono);
            var diferenca = Math.Abs(meu - outro);
            if (diferenca >= menorDiferenca) continue;
            menorDiferenca = diferenca;
            melhorCorte = (int[])dono.Clone();
        }

        var corte = melhorCorte!;

        // O escolhedor fica com o lote que vale mais para ele.
        var lote0 = bolo.ValorDoLoteAlheio(1, 0, corte);
        var lote1 = bolo.ValorDoLote(1, corte);
        if (lote0 <= lote1) return corte;

        var trocado = new int[bolo.Fatias];
        for (var f = 0; f < bolo.Fatias; f++) trocado[f] = 1 - corte[f];
        return trocado;
    }

    /// <summary>
    /// O guloso: cada fatia vai para quem mais a valoriza, e empate fica com quem
    /// vier primeiro. É o que todo mundo faria de cabeça, e maximiza a soma.
    /// </summary>
    public static int[] Guloso(Bolo bolo)
    {
        ArgumentNullException.ThrowIfNull(bolo);
        var dono = new int[bolo.Fatias];
        for (var f = 0; f < bolo.Fatias; f++)
        {
            var melhor = 0;
            for (var p = 1; p < bolo.Pessoas; p++)
                if (bolo.Valor(p, f) > bolo.Valor(melhor, f))
                    melhor = p;
            dono[f] = melhor;
        }
        return dono;
    }

    /// <summary>
    /// O rodízio: as pessoas escolhem em ordem, uma fatia por vez, sempre a melhor
    /// que sobrou para quem está escolhendo.
    /// </summary>
    public static int[] Rodizio(Bolo bolo)
    {
        ArgumentNullException.ThrowIfNull(bolo);
        var dono = new int[bolo.Fatias];
        Array.Fill(dono, -1);

        for (var rodada = 0; rodada < bolo.Fatias; rodada++)
        {
            var pessoa = rodada % bolo.Pessoas;
            var melhor = -1;
            for (var f = 0; f < bolo.Fatias; f++)
            {
                if (dono[f] >= 0) continue;
                if (melhor < 0 || bolo.Valor(pessoa, f) > bolo.Valor(pessoa, melhor)) melhor = f;
            }
            if (melhor < 0) break;
            dono[melhor] = pessoa;
        }

        return dono;
    }

    /// <summary>
    /// A melhor divisão que existe para o critério da menor inveja, achada
    /// varrendo tudo. Não é protocolo: é o teto contra o qual os protocolos são
    /// medidos.
    /// </summary>
    public static int[] MenorInveja(Bolo bolo)
    {
        ArgumentNullException.ThrowIfNull(bolo);
        int[]? melhor = null;
        var menor = int.MaxValue;

        foreach (var dono in Varredura.Todas(bolo))
        {
            var inveja = Juiz.Inveja(bolo, dono);
            if (inveja >= menor) continue;
            menor = inveja;
            melhor = (int[])dono.Clone();
        }

        return melhor!;
    }
}
