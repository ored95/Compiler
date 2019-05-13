using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ParseTests
{
    [TestClass]
    public class Teast
    {
        [TestMethod]
        public void Test_primary_expression()
        {
            Assert.IsTrue(_primary_expression.Test());
        }

        [TestMethod]
        public void Test_postfix_expression()
        {
            Assert.IsTrue(_postfix_expression.Test());
        }

        [TestMethod]
        public void Test_unary_expression()
        {
            Assert.IsTrue(_unary_expression.Test());
        }

        [TestMethod]
        public void Test_cast_expression()
        {
            Assert.IsTrue(_cast_expression.Test());
        }

        [TestMethod]
        public void Test_multiplicative_expression()
        {
            Assert.IsTrue(_multiplicative_expression.Test());
        }

        [TestMethod]
        public void Test_additive_expression()
        {
            Assert.IsTrue(_additive_expression.Test());
        }

        [TestMethod]
        public void Test_shift_expression()
        {
            Assert.IsTrue(_shift_expression.Test());
        }

        [TestMethod]
        public void Test_relational_expression()
        {
            Assert.IsTrue(_relational_expression.Test());
        }

        [TestMethod]
        public void Test_declaration_specifiers()
        {
            Assert.IsTrue(_declaration_specifiers.Test());
        }

        [TestMethod]
        public void Test_storage_class_specifier()
        {
            Assert.IsTrue(_storage_class_specifier.Test());
        }

        [TestMethod]
        public void Test_type_specifier()
        {
            Assert.IsTrue(_type_specifier.Test());
        }

        [TestMethod]
        public void Test_type_qualifier()
        {
            Assert.IsTrue(_type_qualifier.Test());
        }

        [TestMethod]
        public void Test_function_definition()
        {
            Assert.IsTrue(_function_definition.Test());
        }

        [TestMethod]
        public void Test_direct_declarator()
        {
            Assert.IsTrue(_direct_declarator.Test());
        }

        [TestMethod]
        public void Test_direct_abstract_declarator()
        {
            Assert.IsTrue(_direct_abstract_declarator.Test());
        }

        [TestMethod]
        public void Test_specifier_qualifier_list()
        {
            Assert.IsTrue(_specifier_qualifier_list.Test());
        }

        [TestMethod]
        public void Test_struct_declarator_list()
        {
            Assert.IsTrue(_struct_declarator_list.Test());
        }

        [TestMethod]
        public void Test_parameter_declaration()
        {
            Assert.IsTrue(_parameter_declaration.Test());
        }

        [TestMethod]
        public void Test_declarator()
        {
            Assert.IsTrue(_declarator.Test());
        }

        [TestMethod]
        public void Test_init_declarator()
        {
            Assert.IsTrue(_init_declarator.Test());
        }
    }
}
