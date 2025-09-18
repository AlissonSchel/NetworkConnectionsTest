using FluentAssertions;

namespace LogicNetwork.Tests
{
    public class NetworkTests
    {
        [Fact]
        public void Network_Test_Should_Return_Correct_Elements()
        {
            Network network = new Network(8);

            network.Elements.ToArray().Should().BeEquivalentTo(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        }

        [Fact]
        public void Network_Test_Should_Throw_Exception_For_Zero_Elements()
        {
            Action act = () => new Network(0);
            act.Should().Throw<ArgumentException>().WithMessage("Elements amount must be greater than zero.");
        }

        [Fact]
        public void Network_Test_Should_Return_True_For_Direct_Connection()
        {
            //Arrange
            Network network = new Network(8);
            network.Connect((1, 8));
            network.Connect((2, 8));

            //Act
            bool result = network.Query((8, 1));

            //Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(3, 4)]
        [InlineData(7, 8)]
        [InlineData(6, 5)]
        public void Network_Test_Should_Return_False_For_Non_Existing_Connection(uint item1, uint item2)
        {
            //Arrange
            Network network = new Network(8);
            network.Connect((1, 8));
            network.Connect((2, 8));

            //Act
            bool result = network.Query((item1, item2));

            //Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(1, 2)]
        [InlineData(8, 3)]
        public void Network_Test_Should_Retun_True_For_Indirect_Connection(uint item1, uint item2)
        {
            //Arrange
            Network network = new Network(8);
            network.Connect((1, 8));
            network.Connect((2, 8));
            network.Connect((2, 3));

            //Act
            bool result = network.Query((item1, item2));

            //Assert
            result.Should().BeTrue();
        }
    }
}