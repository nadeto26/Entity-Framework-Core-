using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.Utilities
{
    public class XmlHelper
    {
        //generic method

        public T Deserialize<T> (string inputXml, string rootName)
        {
            //първо подаваме типа данните , към какво  ще сериализираме или десериализираме и след това Root

            //-> всичко това го виждаме от suppliers.xml - кой root да вземем и кой тип подаваме 


            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            //Serialize+Deserialize и за двете го използваме 
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), xmlRootAttribute);

           using StringReader reader = new StringReader(inputXml);
            //защото не приема стрингове и за да може да чегем от inputxml

            //така се десериализила или сериализира 
            T supplierDtos =
                (T)xmlSerializer.Deserialize(reader);

            return supplierDtos;
        }

        //May not be used
        public IEnumerable<T> DeserializeCollection<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T[]), xmlRoot);

            using StringReader reader = new StringReader(inputXml);
            T[] desirializedDtos =
                (T[])xmlSerializer.Deserialize(reader);

            return desirializedDtos;
        }

        // Serialize<ExportDto[]>(ExportDto[], rootName)
        // Serialize<ExportDto>(ExportDto, rootName)
        public string Serialize<T>(T obj,string rootName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //за премахването на текста под мета данните 
            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add(string.Empty, string.Empty);

            //serialize

            XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), rootAttribute);
            //първо казваме типа с който ще работи, виждаме го от cars.xml и виждаме,че е масив

            //трябва да напише стринг от обектите , съответстващ на  xml
            // ще иска някъда да пише , съответно искаме да пише в strinbuilder 

            using StringWriter sw = new StringWriter(stringBuilder); //ще пише вместо нас в sb
            xmlSerializer.Serialize(sw, obj, nameSpaces);

            return sw.ToString().TrimEnd();
        }

        // Serialize<ExportDto>(ExportDto[], rootName)
        public string Serialize<T>(T[] obj, string rootName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //за премахването на текста под мета данните 
            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add(string.Empty, string.Empty);

            //serialize

            XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T[]), rootAttribute);
            //първо казваме типа с който ще работи, виждаме го от cars.xml и виждаме,че е масив

            //трябва да напише стринг от обектите , съответстващ на  xml
            // ще иска някъда да пише , съответно искаме да пише в strinbuilder 

            using StringWriter sw = new StringWriter(stringBuilder); //ще пише вместо нас в sb
            xmlSerializer.Serialize(sw, obj, nameSpaces);

            return sw.ToString().TrimEnd();
        }
    }
}
