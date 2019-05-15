using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class LexTest
{
    [TestMethod]
    public void test_lex()
    {
        var LA = new Scanner();
        LA.src = "int main() { return 0; }";
        LA.Lex();
        string output = LA.ToString();
    }
}