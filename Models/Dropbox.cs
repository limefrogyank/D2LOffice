// /* 
//  Licensed under the Apache License, Version 2.0

//  http://www.apache.org/licenses/LICENSE-2.0
//  */
// using System;
// using System.Xml.Serialization;
// using System.Collections.Generic;

// namespace D2LOffice.Models
// {
//     [XmlRoot(ElementName = "instructions")]
//     public class Instructions
//     {
//         [XmlElement(ElementName = "text")]
//         required public string Text { get; set; }
//         [XmlAttribute(AttributeName = "text_type")]
//         required public string Text_type { get; set; }
//     }

//     [XmlRoot(ElementName = "folder")]
//     public class Folder
//     {
//         [XmlElement(ElementName = "instructions")]
//         public required Instructions Instructions { get; set; }
//         [XmlAttribute(AttributeName = "name")]
//         public required string Name { get; set; }
//         [XmlAttribute(AttributeName = "id")]
//         public required string Id { get; set; }
//         [XmlAttribute(AttributeName = "submission_type")]
//         public required string Submission_type { get; set; }
//         [XmlAttribute(AttributeName = "completion_type")]
//         public required string Completion_type { get; set; }
//         [XmlAttribute(AttributeName = "folder_type")]
//         public required string Folder_type { get; set; }
//         [XmlAttribute(AttributeName = "sort_order")]
//         public required string Sort_order { get; set; }
//         [XmlAttribute(AttributeName = "folder_is_retricted")]
//         public required string Folder_is_retricted { get; set; }
//         [XmlAttribute(AttributeName = "files_per_submission")]
//         public required string Files_per_submission { get; set; }
//         [XmlAttribute(AttributeName = "submissions")]
//         public required string Submissions { get; set; }
//         [XmlAttribute(AttributeName = "resource_code")]
//         public required string Resource_code { get; set; }
//         [XmlAttribute(AttributeName = "is_hidden")]
//         public required string Is_hidden { get; set; }


//         [XmlElement(ElementName = "date_start")]
//         public required string Date_start { get; set; }

//         [XmlIgnore]
//         public DateTime DateDue { get; set; }

//         [XmlElement(ElementName = "date_due")]
//         public string Date_due
//         {
//             get => DateDue.ToString("yyy-MM-ddTHH:mm:ss");
//             set => DateDue = DateTime.Parse(value);
//         }


//         [XmlAttribute(AttributeName = "out_of")]
//         public required string Out_of { get; set; }
//         [XmlAttribute(AttributeName = "grade_item")]
//         public required string Grade_item { get; set; }
//     }

//     [XmlRoot(ElementName = "category")]
//     public class DropboxCategory
//     {
//         [XmlElement(ElementName = "folder")]
//         public required List<Folder> Folder { get; set; }
//         [XmlAttribute(AttributeName = "name")]
//         public required string Name { get; set; }
//         [XmlAttribute(AttributeName = "sort_order")]
//         public required string Sort_order { get; set; }
//         [XmlAttribute(AttributeName = "id")]
//         public required string Id { get; set; }
//     }

//     [XmlRoot(ElementName = "dropbox")]
//     public class Dropbox
//     {
//         [XmlElement(ElementName = "category")]
//         public required List<DropboxCategory> Categories { get; set; }
//         [XmlAttribute(AttributeName = "d2l_2p0", Namespace = "http://www.w3.org/2000/xmlns/")]
//         public required string D2l_2p0 { get; set; }
//     }

// }
