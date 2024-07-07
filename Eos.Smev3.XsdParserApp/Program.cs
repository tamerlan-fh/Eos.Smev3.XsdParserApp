using OfficeOpenXml;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;

namespace Eos.Smev3.XsdParserApp
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new string('*', 90));
            Console.WriteLine("** Copyright (c) ООО «ЭОС Платформа», 2024-07-03");
            Console.WriteLine("** Программа отображает линейную структуру xml-элемента");
            Console.WriteLine("**");
            Console.WriteLine("** Требуется указать директорию с xsd-схемамм, описывающими xml, и имя корневого элемента");
            Console.WriteLine(new string('*', 90));
            Console.WriteLine();

            try
            {
                MainAsync(args).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Write("\r\nPress any key to exit...");
            Console.ReadKey();
        }

        private static List<XmlElementInfo> _elements = new List<XmlElementInfo>();

        static async Task MainAsync(string[] args)
        {
            Console.Write(" Директория с xsd-схемами: ");
            string directory = Console.ReadLine()!;

            if (!Directory.Exists(directory))
            {
                Console.WriteLine($" Указанная директория не найдена");
                return;
            }

            string[] paths = Directory.GetFiles(directory, "*.xsd", SearchOption.AllDirectories);
            if (!paths.Any())
            {
                Console.WriteLine(" Указанная директория не содержит файлы схем с расширением .xsd");
                return;
            }

            XmlSchema[] schemas = await GetSchemasAsync(paths);

            Console.Write(" Имя корневого элемента xml: ");
            string elementName = Console.ReadLine()!;

            XmlSchemaElement? element = schemas
                .SelectMany(schm => schm.Items.OfType<XmlSchemaElement>())
                .FirstOrDefault(elm => elm.QualifiedName.Name == elementName);

            if (element is null)
            {
                Console.WriteLine($" Элемент '{elementName}' не описан в указанных xsd-схемах");
                return;
            }

            XmlElementInfo _element = ParseElement(element).First();

            string filename = $"_{elementName}_{DateTime.Now:yyyy-MM-dd_HH_mm}.xlsx";
            Save(filename, _element);

            Console.WriteLine($" Результат сохранен в файл: {filename}");
        }

        private static async Task<XmlSchema[]> GetSchemasAsync(string[] paths)
        {
            List<XmlSchema> schemas = new();
            foreach (string path in paths)
            {
                using (XmlTextReader reader = new(path, new NameTable()))
                {
                    XmlSchema schema = XmlSchema.Read(reader, null)!;

                    if (schema is not null)
                        schemas.Add(schema);
                }
            }

            XmlSchemaSet schemaSet = new();
            foreach (XmlSchema schema in schemas)
            {
                schemaSet.Add(schema);
            }

            if (!schemaSet.IsCompiled)
                schemaSet.Compile();

            return await Task.FromResult(schemaSet.Schemas().OfType<XmlSchema>().ToArray());
        }

        private static XmlElementInfo[] ParseElement(XmlSchemaAnnotated obj, XmlElementInfo? parent = null)
        {
            if (obj is XmlSchemaElement element)
            {
                XmlTypeInfo type = GetTypeInfo(element);
                XmlElementInfo? _element = _elements.FirstOrDefault(e => e.ElementName == element.QualifiedName.Name 
                    && e.Namespace == element.QualifiedName.Namespace 
                    && e.Type.TypeName == type.TypeName);

                if (_element is null)
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
                else
                {
                    _element = new XmlElementInfo
                    {
                        Parent = parent,
                        Type = type,
                        ElementName = _element.ElementName,
                        Namespace = _element.Namespace,
                        Description = _element.Description,
                        Min = _element.Min,
                        Max = _element.Max, 
                        Children = _element.Children
                    };
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

            if (obj is XmlSchemaAttribute attribute)
            {
                XmlTypeInfo type = GetTypeInfo(attribute.AttributeSchemaType!);
                XmlElementInfo? _element = _elements.FirstOrDefault(e => e.ElementName == attribute.QualifiedName.Name 
                    && e.Namespace == attribute.QualifiedName.Namespace 
                    && e.Type.TypeName == type.TypeName 
                    && e.Parent?.ElementName == parent?.ElementName);

                if (_element is null)
                {
                    _element = new XmlElementInfo
                    {
                        Parent = parent,
                        Type = type,
                        ElementName = attribute.QualifiedName.Name,
                        Namespace = attribute.QualifiedName.Namespace,
                        Description = GetDescription(attribute),
                        Min = attribute.Use == XmlSchemaUse.Required ? 1 : 0,
                        Max = 1
                    };
                    _elements.Add(_element);
                }

                return new[] { _element };
            }

            throw new NotImplementedException();
        }

        private static XmlTypeInfo GetTypeInfo(XmlSchemaElement element)
        {
            XmlTypeInfo type = GetTypeInfo(element.ElementSchemaType!);

            if (!string.IsNullOrEmpty(type.TypeName))
                return type;

            return new XmlTypeInfo
            {
                Namespace = element.QualifiedName.Namespace,
                TypeName = element.QualifiedName.Name,
                Description = GetDescription(element.ElementSchemaType),
                IsSimple = element.ElementSchemaType is XmlSchemaSimpleType
            };
        }

        private static XmlTypeInfo GetTypeInfo(XmlSchemaType type)
        {
            XmlTypeInfo _type = new()
            {
                Namespace = type.QualifiedName.Namespace,
                TypeName = type.QualifiedName.Name,
                Description = GetDescription(type),
                IsSimple = type is XmlSchemaSimpleType
            };

            return _type.IsSimple && string.IsNullOrEmpty(_type.TypeName)
                ? GetTypeInfo(type.BaseXmlSchemaType!)
                : _type;
        }

        private static XmlElementInfo[]? GetElementsFromType(XmlSchemaType? type, XmlElementInfo parent)
        {
            if (type is XmlSchemaComplexType complexType)
            {
                IEnumerable<XmlElementInfo> elements = Array.Empty<XmlElementInfo>();

                if (complexType.AttributeUses.Count > 0)
                {
                    XmlElementInfo[] _elements = complexType.AttributeUses.Values
                        .OfType<XmlSchemaAttribute>()
                        .SelectMany(element => ParseElement(element, parent))
                        .ToArray();

                    elements = elements.Concat(_elements);
                }

                if (complexType.ContentTypeParticle is XmlSchemaGroupBase group)
                {
                    XmlElementInfo[] _elements = group.Items
                        .OfType<XmlSchemaParticle>()
                        .SelectMany(element => ParseElement(element, parent))
                        .ToArray();

                    elements = elements.Concat(_elements);
                }

                return elements.Any() ? elements.ToArray() : null;
            }

            if (type is XmlSchemaSimpleType)
                return null;

            throw new NotImplementedException();
        }

        private static string? GetDescription(XmlSchemaAnnotated? element)
        {
            if (element == null)
                return string.Empty;

            return element.Annotation?.Items.OfType<XmlSchemaDocumentation>().FirstOrDefault()?.Markup?.OfType<XmlText>().FirstOrDefault()?.InnerText;
        }

        private static void Save(string filename, XmlElementInfo rootElement)
        {
            using (ExcelPackage excel = new())
            {
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(rootElement.ElementName);
                worksheet.View.FreezePanes(2, 1);

                worksheet.Cells["A1"].Value = "Min";
                worksheet.Cells["B1"].Value = "Max";
                worksheet.Cells["C1"].Value = "XPath";
                worksheet.Cells["D1"].Value = "Type";
                worksheet.Cells["E1"].Value = "Uri";
                worksheet.Cells["F1"].Value = "Description";

                worksheet.Cells["A1:F1"].Style.Font.Bold = true;

                List<XmlTypeInfo> _types = new();

                IEnumerable<XmlElementInfo> GetElements(XmlElementInfo baseElement)
                {                 
                    yield return baseElement;

                    bool replay = !baseElement.Type.IsSimple && _types.Any(t => t.TypeName == baseElement.Type.TypeName && t.Namespace == baseElement.Type.Namespace);
                    if (!baseElement.Type.IsSimple && replay)
                        yield break;
                    else
                        _types.Add(baseElement.Type);

                    if (baseElement.Children is not null)
                    {
                        foreach (XmlElementInfo child in baseElement.Children)
                        {
                            foreach (XmlElementInfo childElement in GetElements(child))
                                yield return childElement;
                        }
                    }
                }

                int index = 1;
                foreach (XmlElementInfo element in GetElements(rootElement))
                {
                    index++;

                    worksheet.Cells[$"A{index}"].Value = element.Min.ToString();
                    worksheet.Cells[$"B{index}"].Value = element.Max == decimal.MaxValue ? "unbounded" : element.Max.ToString();
                    worksheet.Cells[$"C{index}"].Value = element.XPath;
                    worksheet.Cells[$"D{index}"].Value = element.Type.TypeName;
                    worksheet.Cells[$"E{index}"].Value = element.Type.Namespace;
                    worksheet.Cells[$"F{index}"].Value = element.Description?.Replace("\r\n", " ").Replace("\n", " ").Replace("\t", " ");
                }

                worksheet.Cells.AutoFitColumns();
                excel.SaveAs(new FileInfo(filename));
            }
        }

        [DebuggerDisplay($"{{{nameof(ElementName)}}}")]
        internal class XmlElementInfo
        {
            public XmlTypeInfo Type { get; set; }

            public string Namespace { get; set; }

            public string ElementName { get; set; }

            public string? Description { get; set; }

            public XmlElementInfo? Parent { get; set; }

            public XmlElementInfo[]? Children { get; set; }

            public decimal Min { get; set; }

            public decimal Max { get; set; }

            public string XPath
            {
                get { return $"{Parent?.XPath}\\{ElementName}".TrimStart('\\'); }
            }
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