// /* 
//  Licensed under the Apache License, Version 2.0

//  http://www.apache.org/licenses/LICENSE-2.0
//  */
// using System;
// using System.Xml.Serialization;
// using System.Collections.Generic;

// namespace D2LOffice.Models
// {
//     [XmlRoot(ElementName = "description")]
//     public class Description
//     {
//         [XmlAttribute(AttributeName = "text_type")]
//         public required string Text_type { get; set; }
//         [XmlText]
//         public required string Text { get; set; }
//     }

//     [XmlRoot(ElementName = "name")]
//     public class Name
//     {
//         [XmlAttribute(AttributeName = "text_type")]
//         public required string Text_type { get; set; }
//         [XmlText]
//         public required string Text { get; set; }
//     }

//     [XmlRoot(ElementName = "item")]
//     public class ChecklistItem
//     {
//         [XmlElement(ElementName = "name")]
//         public required Name Name { get; set; }
//         [XmlElement(ElementName = "description")]
//         public required Description Description { get; set; }
//         [XmlElement(ElementName = "date_end")]
//         public required string Date_end { get; set; }
//         [XmlAttribute(AttributeName = "sort_order")]
//         public required string Sort_order { get; set; }
//         [XmlAttribute(AttributeName = "in_schedule")]
//         public required string In_schedule { get; set; }
//     }

//     [XmlRoot(ElementName = "category")]
//     public class ChecklistCategory
//     {
//         [XmlElement(ElementName = "name")]
//         public required Name Name { get; set; }
//         [XmlElement(ElementName = "description")]
//         public required Description Description { get; set; }
//         [XmlElement(ElementName = "item")]
//         public required List<ChecklistItem> Item { get; set; }
//         [XmlAttribute(AttributeName = "sort_order")]
//         public required string Sort_order { get; set; }
//     }

//     [XmlRoot(ElementName = "checklist")]
//     public class Checklist
//     {
//         [XmlElement(ElementName = "name")]
//         public required string Name { get; set; }
//         [XmlElement(ElementName = "description")]
//         public required Description Description { get; set; }
//         [XmlElement(ElementName = "category")]
//         public required List<ChecklistCategory> Category { get; set; }
//         [XmlAttribute(AttributeName = "id")]
//         public required string Id { get; set; }
//         [XmlAttribute(AttributeName = "resource_code")]
//         public required string Resource_code { get; set; }
//         [XmlAttribute(AttributeName = "display_in_new_window")]
//         public required string Display_in_new_window { get; set; }
//     }

//     [XmlRoot(ElementName = "checklists")]
//     public class ChecklistRoot
//     {
//         [XmlElement(ElementName = "checklist")]
//         public required List<Checklist> Checklists { get; set; }
//     }

// }
