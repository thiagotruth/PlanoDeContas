using Application.Services;
using Domain.Interfaces;
using Domain.Models;
using Moq;

namespace PlanoDeContas.UnitTests.Services
{
    [TestFixture]
    public class ContaServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IContaRepository> mockContaRepository;
        private ContaService contaService;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            mockContaRepository = this.mockRepository.Create<IContaRepository>();
            contaService = new ContaService(mockContaRepository.Object);
        }

        [Test]
        public void CriarContaAsync_ValidarCodigo_CodigoInvalido_DeveLancarExcecao()
        {
            // Arrange
            var conta = new Conta { Codigo = "abc.def" };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => contaService.CriarContaAsync(conta));
        }

        [Test]
        public void CriarContaAsync_ValidarCodigo_CodigoExcede999_DeveLancarExcecao()
        {
            // Arrange
            var conta = new Conta { Codigo = "1.2.1000" };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => contaService.CriarContaAsync(conta));
        }

        [Test]
        public void CriarContaAsync_ValidarCodigo_CodigoExistente_DeveLancarExcecao()
        {
            // Arrange
            var conta = new Conta { Codigo = "1.2.3" };
            mockContaRepository.Setup(repo => repo.RecuperarPorCodigoAsync(conta.Codigo)).ReturnsAsync(new Conta());

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => contaService.CriarContaAsync(conta));
        }

        [Test]
        public async Task ExcluirContaAsync_DeveChamarMetodoExcluirContaAsyncDoRepositorio()
        {
            // Arrange
            int idConta = 1;
            mockContaRepository.Setup(repo => repo.ExcluirContaAsync(idConta)).Returns(Task.CompletedTask);

            // Act
            await contaService.ExcluirContaAsync(idConta);

            // Assert
            mockContaRepository.Verify(repo => repo.ExcluirContaAsync(idConta), Times.Once);
        }

        [Test]
        public async Task RecuperarPorCodigoAsync_DeveChamarMetodoRecuperarPorCodigoAsyncDoRepositorio()
        {
            // Arrange
            string codigo = "1.2.3";
            mockContaRepository.Setup(repo => repo.RecuperarPorCodigoAsync(codigo)).ReturnsAsync(new Conta());

            // Act
            var result = await contaService.RecuperarPorCodigoAsync(codigo);

            // Assert
            mockContaRepository.Verify(repo => repo.RecuperarPorCodigoAsync(codigo), Times.Once);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task RecuperarPorIdAsync_DeveChamarMetodoRecuperarPorIdAsyncDoRepositorio()
        {
            // Arrange
            int id = 1;
            mockContaRepository.Setup(repo => repo.RecuperarPorIdAsync(id)).ReturnsAsync(new Conta());

            // Act
            var result = await contaService.RecuperarPorIdAsync(id);

            // Assert
            mockContaRepository.Verify(repo => repo.RecuperarPorIdAsync(id), Times.Once);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task SugerirProximoCodigoAsync_ContaFilhaInexistente_DeveRetornarProximoCodigoValido()
        {
            // Arrange
            int contaId = 1;
            var conta = new Conta { Id = contaId, Codigo = "1", ContasFilhas = new List<Conta>() };
            mockContaRepository.Setup(repo => repo.RecuperarPorIdAsync(contaId)).ReturnsAsync(conta);
            mockContaRepository.Setup(repo => repo.CodigoExisteAsync("1.1")).ReturnsAsync(false);

            // Act
            var result = await contaService.SugerirProximoCodigoAsync(contaId);

            // Assert
            Assert.That(result, Is.EqualTo("1.1"));
        }

        [Test]
        public async Task SugerirProximoCodigoAsync_ContaFilhaExistente_DeveRetornarProximoCodigoValido()
        {
            // Arrange
            int contaId = 1;
            var conta = new Conta { Id = contaId, Codigo = "1", ContasFilhas = new List<Conta> { new Conta { Codigo = "1.1" } } };
            mockContaRepository.Setup(repo => repo.RecuperarPorIdAsync(contaId)).ReturnsAsync(conta);
            mockContaRepository.Setup(repo => repo.CodigoExisteAsync("1.2")).ReturnsAsync(false);

            // Act
            var result = await contaService.SugerirProximoCodigoAsync(contaId);

            // Assert
            Assert.That(result, Is.EqualTo("1.2"));
        }

        [Test]
        public async Task CalcularProximoCodigoHierarquia_CodigoExistente_DeveRetornarProximoCodigoValido()
        {
            // Arrange
            int contaId = 1;
            var conta = new Conta { Id = contaId, Codigo = "1.2", ContasFilhas = new List<Conta> { new Conta { Codigo = "1.2.999" } } };
            List<string> codigosExistentes = new() { "1.1", "1.2", "1.3" };

            mockContaRepository.Setup(repo => repo.RecuperarPorIdAsync(contaId)).ReturnsAsync(conta);
            mockContaRepository.Setup(repo => repo.CodigoExisteAsync("1.3")).ReturnsAsync(true);
            mockContaRepository.Setup(repo => repo.RecuperarTodosOsCodigosAsync()).ReturnsAsync(codigosExistentes);

            // Act
            var result = await contaService.SugerirProximoCodigoAsync(contaId);

            // Assert
            Assert.That(result, Is.EqualTo("1.4"));
        }

        [Test]
        public void CalcularProximoCodigoHierarquia_CodigoMaximo_DeveLancarExcecao()
        {
            // Arrange
            int contaId = 1;
            var conta = new Conta { Id = contaId, Codigo = "999.999", ContasFilhas = new List<Conta> { new Conta { Codigo = "999.999.999" } } };
            List<string> codigosExistentes = new() { "999.999", "999.999.999", "2" };
            mockContaRepository.Setup(repo => repo.RecuperarPorIdAsync(contaId)).ReturnsAsync(conta);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await contaService.SugerirProximoCodigoAsync(contaId));
        }

        [Test]
        public async Task RecuperarTodasAsync_DeveRetornarTodasAsContas()
        {
            // Arrange
            var contas = new List<Conta>
        {
            new Conta { Id = 1, Nome = "Conta 1" },
            new Conta { Id = 2, Nome = "Conta 2" },
            new Conta { Id = 3, Nome = "Conta 3" }
        };
            mockContaRepository.Setup(repo => repo.RecuperarTodasAsync()).ReturnsAsync(contas);

            // Act
            var result = await contaService.RecuperarTodasAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(contas.Count));
            Assert.That(contas.SequenceEqual(result), Is.True);
        }

        [Test]
        public async Task FiltrarPorNomeAsync_DeveRetornarContasComNomeCorrespondente()
        {
            // Arrange
            string nome = "Conta 1";
            var contas = new List<Conta>
        {
            new Conta { Id = 1, Nome = "Conta 1" },
            new Conta { Id = 2, Nome = "Conta 2" },
            new Conta { Id = 3, Nome = "Conta 1" }
        };
            mockContaRepository.Setup(repo => repo.FiltrarPorNomeAsync(nome)).ReturnsAsync(contas.Where(c => c.Nome == nome));

            // Act
            var result = await contaService.FiltrarPorNomeAsync(nome);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.All(c => c.Nome == nome), Is.True);
        }
    }
}
