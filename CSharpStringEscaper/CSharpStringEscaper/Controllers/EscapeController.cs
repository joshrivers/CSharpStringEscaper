using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSharpStringEscaper.Controllers
{
    public class EscapeController : Controller
    {
        public ActionResult Index()
        {
            this.ViewBag.multilineChecked = true;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormCollection form)
        {
            this.ViewBag.input = form["unescapedCodeInput"];
            this.ViewBag.result = ToLiteral(form["unescapedCodeInput"]);
            return View();
        }

        private static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, new CodeGeneratorOptions { IndentString = "\t" });
                    var literal = writer.ToString();
                    return literal;
                }
            }
        }
    }
}
