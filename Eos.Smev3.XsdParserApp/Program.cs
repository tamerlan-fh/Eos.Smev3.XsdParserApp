using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;

namespace Eos.Smev3.XsdParserApp
{
    partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainAsync(args).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        static async Task MainAsync(string[] args)
        {
            //var paths = new[]
            //{
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Commons.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\TDocument.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\CommonSimpleType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DDocument.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DRequestDocument.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\TSubject.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\TAddress.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DHouse.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DCountry.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DBenefitCategory.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DDeclarantKind.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DRegionsRF.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DDeclarantKindReg.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DContractor.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\TStatementCommons.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DActionCode.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DStatementType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\TObject.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DUsageType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DHousingPurpose.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DObjectPurpose.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DRoomPurpose.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DObjectType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DUnitType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DInterdepObjectType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DRecieveResultType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DReceivingMethod.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DAgreements.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Dictionary\DKindInfo.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Interdep_v026\Interdep_v026.xsd"
            //};
            //var paths = new[]
            //{
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\TObject.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Commons.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\TAddress.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DHouse.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\CommonSimpleType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DUsageType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DHousingPurpose.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DObjectPurpose.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DRoomPurpose.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DObjectType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DUnitType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DInterdepObjectType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\TSubject.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\TDocument.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DDocument.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DRequestDocument.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DCountry.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DBenefitCategory.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DDeclarantKind.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DRegionsRF.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DDeclarantKindReg.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DContractor.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DCadastralAction.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DDeal.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DEncumbrance.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DRegistryAction.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DRequestEGRNAccessAction.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DResult.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DRight.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DRightAction.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\TStatementCommons.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DActionCode.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DStatementType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DRecieveResultType.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DReceivingMethod.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DAgreements.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Dictionary\DKindInfo.xsd",
            //    @"C:\Users\FakhrutdinovTN\Desktop\ЕГРН\1.2.2\Statement_v026\Statement_v026.xsd",
            //};

            var paths = new[]
            {
                @"C:\Users\FakhrutdinovTN\Desktop\Новая папка (10)\2.6.0\commons\Common.xsd",
@"C:\Users\FakhrutdinovTN\Desktop\Новая папка (10)\2.6.0\commons\Package.xsd",
@"C:\Users\FakhrutdinovTN\Desktop\Новая папка (10)\2.6.0\commons\Refund.xsd",
@"C:\Users\FakhrutdinovTN\Desktop\Новая папка (10)\2.6.0\commons\Organization.xsd",
@"C:\Users\FakhrutdinovTN\Desktop\Новая папка (10)\2.6.0\commons\Charge.xsd",
@"C:\Users\FakhrutdinovTN\Desktop\Новая папка (10)\2.6.0\commons\Payment.xsd",
@"C:\Users\FakhrutdinovTN\Desktop\Новая папка (10)\2.6.0\commons\Income.xsd",
@"C:\Users\FakhrutdinovTN\Desktop\Новая папка (10)\2.6.0\commons\Clarification.xsd",
@"C:\Users\FakhrutdinovTN\Desktop\Новая папка (10)\2.6.0\commons\Ruling.xsd",
@"C:\Users\FakhrutdinovTN\Desktop\Новая папка (10)\2.6.0\ImportCharges.xsd"
            };

            var schemas = await GetSchemasAsync(paths);

            var element = schemas
                .SelectMany(schm => schm.Items.OfType<XmlSchemaElement>())
                .FirstOrDefault(elm => elm.QualifiedName.Name == "ImportChargesRequest");

            var _element = ParseElement(element).First();

            var result = string.Join("\r\n", GetItems(_element));
        }

        private static async Task<XmlSchema[]> GetSchemasAsync(string[] paths)
        {
            var schemas = new List<XmlSchema>();
            foreach (var path in paths)
            {
                using (var reader = new XmlTextReader(path, new NameTable()))
                {
                    var schema = XmlSchema.Read(reader, null);

                    if (schema is not null)
                        schemas.Add(schema);
                }
            }

            var schemaSet = new XmlSchemaSet();
            foreach (var schema in schemas)
            {
                schemaSet.Add(schema);
            }

            if (!schemaSet.IsCompiled)
                schemaSet.Compile();

            return await Task.FromResult(schemaSet.Schemas().OfType<XmlSchema>().ToArray());
        }

        static List<XmlElementInfo> _elements = new List<XmlElementInfo>();

        private static XmlElementInfo[] ParseElement(XmlSchemaParticle obj, XmlElementInfo parent = null)
        {
            if (obj is XmlSchemaElement element)
            {
                var type = GetTypeInfo(element.ElementSchemaType);
                var _element = _elements.FirstOrDefault(e => e.ElementName == element.QualifiedName.Name && e.Namespace == element.QualifiedName.Namespace && e.Type.TypeName == type.TypeName && e.Parent?.ElementName == parent?.ElementName);

                if (_element == null)
                {
                    _element = new XmlElementInfo
                    {
                        Parent = parent,
                        Type = type,
                        ElementName = element.QualifiedName.Name,
                        Namespace = element.QualifiedName.Namespace,
                        Description = GetDescription(element) ?? GetDescription(element.ElementSchemaType),
                        Min = element.MinOccurs,
                        Max = element.MaxOccurs
                    };
                    _elements.Add(_element);

                    _element.Children = GetElementsFromType(element.ElementSchemaType, _element);
                }

                return new[] { _element };
            }

            if (obj is XmlSchemaChoice choice)
            {
                return choice.Items
                    .OfType<XmlSchemaParticle>()
                    .SelectMany(element => ParseElement(element, parent))
                    .ToArray();
            }

            if (obj is XmlSchemaSequence sequence)
            {
                return sequence.Items
                    .OfType<XmlSchemaParticle>()
                    .SelectMany(element => ParseElement(element, parent))
                    .ToArray();
            }

            throw new NotImplementedException();
        }

        private static XmlTypeInfo GetTypeInfo(XmlSchemaType type)
        {
            var _type = new XmlTypeInfo
            {
                Namespace = type.QualifiedName.Namespace,
                TypeName = type.QualifiedName.Name,
                Description = GetDescription(type),
                IsSimple = type is XmlSchemaSimpleType
            };

            if (!string.IsNullOrEmpty(_type.TypeName))
                return _type;

            return GetTypeInfo(type.BaseXmlSchemaType);
        }

        private static XmlElementInfo[] GetElementsFromType(XmlSchemaType type, XmlElementInfo parent)
        {
            if (type is XmlSchemaComplexType complexType)
            {
                if (complexType.ContentTypeParticle is XmlSchemaGroupBase group)
                {
                    try
                    {
                        return group.Items
                             .OfType<XmlSchemaParticle>()
                             .SelectMany(element => ParseElement(element, parent))
                             .ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

            return null;
        }

        private static string? GetDescription(XmlSchemaAnnotated? element)
        {
            if (element == null)
                return string.Empty;

            return element.Annotation?.Items.OfType<XmlSchemaDocumentation>().FirstOrDefault()?.Markup?.OfType<XmlText>().FirstOrDefault()?.InnerText;
        }

        private static List<XmlTypeInfo> _types = new List<XmlTypeInfo>();
        private static IEnumerable<string> GetItems(XmlElementInfo element)
        {
            var replay = !element.Type.IsSimple && _types.Any(t => t.TypeName == element.Type.TypeName && t.Namespace == element.Type.Namespace);

            var value = $"{element.ElementName}" +
                $"\t{element.Type.Namespace}" +
                $"\t{element.Type.TypeName}" +
                $"\t{element.Description?.Replace("\r\n", " ").Replace("\n", " ")}" +
                $"\t{element.Min}" +
                $"\t{(element.Max > 100 ? "unbounded" : element.Max.ToString())}" +
                $"\t{(replay ? "да" : string.Empty)}";

            yield return value;

            if (!element.Type.IsSimple && replay)
                yield break;
            else
                _types.Add(element.Type);

            if (element.Children is not null)
            {
                foreach (var child in element.Children)
                {
                    foreach (var item in GetItems(child))
                        yield return $"{element.ElementName}\\{item}";
                }
            }
        }

        [DebuggerDisplay($"{{{nameof(ElementName)}}}")]
        internal class XmlElementInfo
        {
            public XmlTypeInfo Type { get; set; }

            public string Namespace { get; set; }

            public string ElementName { get; set; }

            public string? Description { get; set; }

            public XmlElementInfo Parent { get; set; }

            public XmlElementInfo[] Children { get; set; }

            public decimal Min { get; set; }

            public decimal Max { get; set; }
        }

        [DebuggerDisplay($"{{{nameof(TypeName)}}}")]
        internal class XmlTypeInfo
        {
            public string Namespace { get; set; }

            public string TypeName { get; set; }

            public string? Description { get; set; }

            public bool IsSimple { get; set; }
        }
    }
}