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
            bool multilineChecked = !string.IsNullOrEmpty(form["useMultiline"]);
            this.ViewBag.input = form["unescapedCodeInput"];
            this.ViewBag.result = ToLiteral(form["unescapedCodeInput"], multilineChecked);
            this.ViewBag.multilineChecked = multilineChecked;
            return View();
        }

        private static string ToLiteral(string input, bool multiline)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, new CodeGeneratorOptions { IndentString = "\t" });
                    var literal = writer.ToString();
                    if (!multiline)
                    {
                        literal = literal.Replace(string.Format("\" +{0}\t\"", Environment.NewLine), "");
                    }
                    return literal;
                }
            }
        }
    }
}
