using DataED.Utils;

namespace DataEDNUnitTests {
    [TestFixture]
    public class ToInt32Parser_Tests {
        // Parse()
        [Test]
        public void Parse_ValidString_ReturnsCorrectValue() {
            Assert.That(ToInt32Parser.Parse("21"), Is.EqualTo(21));
        }

        [Test]
        public void Parse_StringWithLeadingWhitespace_ReturnsCorrectValue() {
            Assert.That(ToInt32Parser.Parse("   1"), Is.EqualTo(1));
        }

        [Test]
        public void Parse_StringWithTrailingWhitespace_ReturnsCorrectValue() {
            Assert.That(ToInt32Parser.Parse("1045   "), Is.EqualTo(1045));
        }

        [Test]
        public void Parse_StringWithBothLeadingAndTrailingWhitespace_ReturnsCorrectValue() {
            Assert.That(ToInt32Parser.Parse("   5844   "), Is.EqualTo(5844));
        }

        [Test]
        public void Parse_StringWithPlusSign_ReturnsCorrectValue() {
            Assert.That(ToInt32Parser.Parse("+1555"), Is.EqualTo(1555));
        }

        [Test]
        public void Parse_StringWithMinusSign_ReturnsCorrectValue() {
            Assert.That(ToInt32Parser.Parse("-79321"), Is.EqualTo(-79321));
        }

        [Test]
        public void Parse_StringWithPlusSignAndSpaces_ReturnsCorrectValue() {
            Assert.That(ToInt32Parser.Parse("   +5   "), Is.EqualTo(5));
        }

        [Test]
        public void Parse_StringWithSpacesAndSign_ReturnsCorrectValue() {
            Assert.That(ToInt32Parser.Parse("   -146   "), Is.EqualTo(-146));
        }

        [Test]
        public void Parse_NullString_ThrowsArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => ToInt32Parser.Parse(null));
        }

        [Test]
        public void Parse_InvalidFormat_ThrowsFormatException() {
            Assert.Throws<FormatException>(() => ToInt32Parser.Parse("abc"));
        }

        [Test]
        public void Parse_PositiveOverflow_ThrowsOverflowException() {
            Assert.Throws<OverflowException>(() => ToInt32Parser.Parse("3147483647"));
        }

        [Test]
        public void Parse_NegativeOverflow_ThrowsOverflowException() {
            Assert.Throws<OverflowException>(() => ToInt32Parser.Parse("-3147483649"));
        }
        // ParseInRange()
        [Test]
        public void ParseInRange_ValidInputWithinRange_ReturnsCorrectValue() {
            Assert.That(ToInt32Parser.ParseInRange("32", 16, 64), Is.EqualTo(32));
        }

        [Test]
        public void ParseInRange_InvalidRange_ThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => ToInt32Parser.ParseInRange("75", 178, -7));
        }

        [Test]
        public void ParseInRange_InputBelowRange_ThrowsArgumentOutOfRangeException() {
            Assert.Throws<ArgumentOutOfRangeException>(() => ToInt32Parser.ParseInRange("-13", 5, 99));
        }

        [Test]
        public void ParseInRange_InputAboveRange_ThrowsArgumentOutOfRangeException() {
            Assert.Throws<ArgumentOutOfRangeException>(() => ToInt32Parser.ParseInRange("1919", 1, 6));
        }
    }
}