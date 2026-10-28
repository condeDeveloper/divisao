using Conde.Divisao;
using Xunit;

namespace Conde.Divisao.Testes;

public class BoloEJuizTestes
{
    private static Bolo GostosOpostos() => new(new[,] { { 3, 2, 1 }, { 1, 2, 3 } });

    [Fact]
    public void OTotalEhASomaDoQueAPessoaVeNoBoloInteiro()
    {
        var bolo = GostosOpostos();
        Assert.Equal(6, bolo.Total(0));
        Assert.Equal(6, bolo.Total(1));
    }

    [Fact]
    public void OLoteValeOQueODonoAchaQueVale()
    {
        var bolo = GostosOpostos();
        var dono = new[] { 0, 0, 1 };
        Assert.Equal(5, bolo.ValorDoLote(0, dono));
        Assert.Equal(3, bolo.ValorDoLote(1, dono));
    }

    [Fact]
    public void OLoteAlheioEhOlhadoComOsProprioOlhos()
    {
        var bolo = GostosOpostos();
        var dono = new[] { 0, 0, 1 };
        // A pessoa 1 olha o lote da 0 e vê 1 + 2 = 3.
        Assert.Equal(3, bolo.ValorDoLoteAlheio(1, 0, dono));
    }

    [Fact]
    public void ValorNegativoNaoEhFatia()
    {
        Assert.Throws<ArgumentException>(() => new Bolo(new[,] { { 1, -1 } }));
    }

    [Fact]
    public void ADivisaoPrecisaDarDonoAToDaFatia()
    {
        var bolo = GostosOpostos();
        Assert.True(Juiz.EhDivisao(bolo, new[] { 0, 1, 0 }));
        Assert.False(Juiz.EhDivisao(bolo, new[] { 0, 1 }));
        Assert.False(Juiz.EhDivisao(bolo, new[] { 0, 1, 2 }));
    }

    [Fact]
    public void SemInvejaSempreEhProporcionalComDuasPessoas()
    {
        var bolo = GostosOpostos();
        foreach (var dono in Varredura.Todas(bolo))
            if (Juiz.SemInveja(bolo, dono))
                Assert.True(Juiz.Proporcional(bolo, dono));
    }

    [Fact]
    public void AInvejaEhZeroExatamenteQuandoNaoHaInveja()
    {
        var bolo = GostosOpostos();
        foreach (var dono in Varredura.Todas(bolo))
            Assert.Equal(Juiz.SemInveja(bolo, dono), Juiz.Inveja(bolo, dono) == 0);
    }

    [Fact]
    public void QuandoUmaFatiaSoInteressaNaoHaDivisaoJusta()
    {
        // Os dois querem a mesma fatia e o resto não vale nada.
        var bolo = new Bolo(new[,] { { 5, 0, 0 }, { 5, 0, 0 } });
        foreach (var dono in Varredura.Todas(bolo))
        {
            Assert.False(Juiz.SemInveja(bolo, dono));
            Assert.False(Juiz.Proporcional(bolo, dono));
        }
    }

    [Fact]
    public void DarTudoAUmaPessoaEhEficienteENadaJusto()
    {
        var bolo = GostosOpostos();
        var dono = new[] { 0, 0, 0 };
        Assert.True(Juiz.Eficiente(bolo, dono));
        Assert.False(Juiz.SemInveja(bolo, dono));
    }

    [Fact]
    public void UmaDivisaoQueDeixaFatiaValiosaComQuemNaoQuerNaoEhEficiente()
    {
        var bolo = GostosOpostos();
        // A fatia 0 vale 3 para a pessoa 0 e 1 para a 1; a fatia 2 é o contrário.
        Assert.False(Juiz.Eficiente(bolo, new[] { 1, 0, 0 }));
    }
}
