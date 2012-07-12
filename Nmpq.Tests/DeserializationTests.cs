using System.IO;
using Nmpq.Parsing;
using NUnit.Framework;

namespace Nmpq.Tests {
	[TestFixture]
	public class DeserializationTests {
		// format information and test values taken from http://www.teamliquid.net/forum/viewmessage.php?topic_id=117260&currentpage=3#45
		[TestCase(new byte[] { 0x00 }, 0)]
		[TestCase(new byte[] { 0x01 }, 0)]
		[TestCase(new byte[] { 0x02 }, 1)]
		[TestCase(new byte[] { 0x03 }, -1)]
		[TestCase(new byte[] { 0x7e }, 63)]
		[TestCase(new byte[] { 0x7f }, -63)]
		[TestCase(new byte[] { 0x80 }, 0, ExpectedException = typeof(EndOfStreamException))]
		[TestCase(new byte[] { 0x80, 0x00 }, 0)]
		[TestCase(new byte[] { 0x80, 0x01 }, 64)]
		[TestCase(new byte[] { 0x81, 0x01 }, -64)]
		[TestCase(new byte[] { 0x82, 0x01 }, 65)]
		[TestCase(new byte[] { 0x83, 0x01 }, -65)]
		[TestCase(new byte[] { 0x80, 0x02 }, 128)]
		[TestCase(new byte[] { 0x81, 0x02 }, -128)]
		[TestCase(new byte[] { 0x80, 0x80, 0x80, 0x10 }, 16777216)]
		public void Variable_length_integers_are_deserialized_correctly(byte[] bytes, int expectedValue) {
			using (var stream = new MemoryStream(bytes)) 
			using (var reader = new BinaryReader(stream)) {
				var value = Deserialization.ParseVariableLengthInteger(reader);

				Assert.That(value, Is.EqualTo(expectedValue));
			}
		}
	}
}