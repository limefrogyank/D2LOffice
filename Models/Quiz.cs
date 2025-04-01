// /* 
//  Licensed under the Apache License, Version 2.0

//  http://www.apache.org/licenses/LICENSE-2.0
//  */
// using System;
// using System.Xml.Serialization;
// using System.Collections.Generic;

// namespace D2LOffice.Models
// {
//     [XmlRoot(ElementName = "mattext")]
//     public class Mattext
//     {
//         [XmlAttribute(AttributeName = "isdisplayed", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Isdisplayed { get; set; }
//         [XmlAttribute(AttributeName = "texttype")]
//         public required string Texttype { get; set; }
//         [XmlText]
//         public required string Text { get; set; }
//     }

//     [XmlRoot(ElementName = "material")]
//     public class Material
//     {
//         [XmlElement(ElementName = "mattext")]
//         public required Mattext Mattext { get; set; }
//         [XmlAttribute(AttributeName = "label")]
//         public required string Label { get; set; }
//         [XmlElement(ElementName = "matimage")]
//         public required Matimage Matimage { get; set; }
//     }

//     [XmlRoot(ElementName = "flow_mat")]
//     public class Flow_mat
//     {
//         [XmlElement(ElementName = "material")]
//         public required List<Material> Material { get; set; }
//     }

//     [XmlRoot(ElementName = "rubric")]
//     public class Rubric
//     {
//         [XmlElement(ElementName = "flow_mat")]
//         public required Flow_mat Flow_mat { get; set; }
//     }

//     [XmlRoot(ElementName = "assessmentcontrol")]
//     public class Assessmentcontrol
//     {
//         [XmlAttribute(AttributeName = "hintswitch")]
//         public required string Hintswitch { get; set; }
//         [XmlAttribute(AttributeName = "solutionswitch")]
//         public required string Solutionswitch { get; set; }
//         [XmlAttribute(AttributeName = "feedbackswitch")]
//         public required string Feedbackswitch { get; set; }
//     }

//     [XmlRoot(ElementName = "presentation_material")]
//     public class Presentation_material
//     {
//         [XmlElement(ElementName = "flow_mat")]
//         public required Flow_mat Flow_mat { get; set; }
//     }

//     [XmlRoot(ElementName = "intro_message", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//     public class Intro_message
//     {
//         [XmlAttribute(AttributeName = "isdisplayed", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Isdisplayed { get; set; }
//         [XmlAttribute(AttributeName = "texttype")]
//         public required string Texttype { get; set; }
//         [XmlText]
//         public required string Text { get; set; }
//     }

//     [XmlRoot(ElementName = "grade_item")]
//     public class Grade_item
//     {
//         [XmlAttribute(AttributeName = "is_autoexport", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Is_autoexport { get; set; }
//         [XmlAttribute(AttributeName = "resource_code")]
//         public required string Resource_code { get; set; }
//         [XmlText]
//         public required string Text { get; set; }
//     }

//     [XmlRoot(ElementName = "timestamp", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//     public class Timestamp
//     {
//         [XmlElement(ElementName = "month", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Month { get; set; }
//         [XmlElement(ElementName = "day", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Day { get; set; }
//         [XmlElement(ElementName = "year", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Year { get; set; }
//         [XmlElement(ElementName = "hour", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Hour { get; set; }
//         [XmlElement(ElementName = "minutes", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Minutes { get; set; }
//         [XmlElement(ElementName = "seconds", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Seconds { get; set; }
//     }

//     [XmlRoot(ElementName = "date_start", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//     public class Date_start
//     {
//         [XmlElement(ElementName = "timestamp", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required Timestamp Timestamp { get; set; }
//     }

//     [XmlRoot(ElementName = "date_end", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//     public class Date_end
//     {
//         [XmlElement(ElementName = "timestamp", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required Timestamp Timestamp { get; set; }
//     }

//     [XmlRoot(ElementName = "date_due", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//     public class Date_due
//     {
//         [XmlElement(ElementName = "timestamp", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required Timestamp Timestamp { get; set; }
//     }

//     [XmlRoot(ElementName = "assess_procextension")]
//     public class Assess_procextension
//     {
//         [XmlElement(ElementName = "intro_message", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required Intro_message Intro_message { get; set; }
//         [XmlElement(ElementName = "grade_item")]
//         public required Grade_item Grade_item { get; set; }
//         [XmlElement(ElementName = "disable_right_click", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Disable_right_click { get; set; }
//         [XmlElement(ElementName = "disable_pager_access", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Disable_pager_access { get; set; }
//         [XmlElement(ElementName = "is_active")]
//         public required string Is_active { get; set; }
//         [XmlElement(ElementName = "date_start", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required Date_start Date_start { get; set; }
//         [XmlElement(ElementName = "date_end", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required Date_end Date_end { get; set; }
//         [XmlElement(ElementName = "date_due", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required Date_due Date_due { get; set; }
//         [XmlElement(ElementName = "has_schedule_event", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Has_schedule_event { get; set; }
//         [XmlElement(ElementName = "is_attempt_Rldb", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Is_attempt_Rldb { get; set; }
//         [XmlElement(ElementName = "is_subview_Rldb", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Is_subview_Rldb { get; set; }
//         [XmlElement(ElementName = "time_limit", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Time_limit { get; set; }
//         [XmlElement(ElementName = "show_clock", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Show_clock { get; set; }
//         [XmlElement(ElementName = "enforce_time_limit", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Enforce_time_limit { get; set; }
//         [XmlElement(ElementName = "grace_period", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Grace_period { get; set; }
//         [XmlElement(ElementName = "late_limit", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Late_limit { get; set; }
//         [XmlElement(ElementName = "late_limit_data", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Late_limit_data { get; set; }
//         [XmlElement(ElementName = "attempts_allowed", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Attempts_allowed { get; set; }
//         [XmlElement(ElementName = "attempt_restrictions", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Attempt_restrictions { get; set; }
//         [XmlElement(ElementName = "mark_calculation_type", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Mark_calculation_type { get; set; }
//         [XmlElement(ElementName = "is_forward_only", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Is_forward_only { get; set; }
//     }

//     [XmlRoot(ElementName = "assessfeedback")]
//     public class Assessfeedback
//     {
//         [XmlElement(ElementName = "rubric")]
//         public required Rubric Rubric { get; set; }
//         [XmlElement(ElementName = "duration", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Duration { get; set; }
//         [XmlElement(ElementName = "response_display_type_id", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Response_display_type_id { get; set; }
//         [XmlElement(ElementName = "show_correct_answers", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Show_correct_answers { get; set; }
//         [XmlElement(ElementName = "submission_restrictip", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Submission_restrictip { get; set; }
//         [XmlElement(ElementName = "show_class_average", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Show_class_average { get; set; }
//         [XmlElement(ElementName = "show_score_distribution", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Show_score_distribution { get; set; }
//         [XmlAttribute(AttributeName = "title")]
//         public required string Title { get; set; }
//         [XmlElement(ElementName = "release_date")]
//         public required Release_date Release_date { get; set; }
//     }

//     [XmlRoot(ElementName = "order")]
//     public class Order
//     {
//         [XmlAttribute(AttributeName = "order_type")]
//         public required string Order_type { get; set; }
//     }

//     [XmlRoot(ElementName = "release_date")]
//     public class Release_date
//     {
//         [XmlElement(ElementName = "timestamp", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required Timestamp Timestamp { get; set; }
//     }

//     [XmlRoot(ElementName = "qti_metadatafield")]
//     public class Qti_metadatafield
//     {
//         [XmlElement(ElementName = "fieldlabel")]
//         public required string Fieldlabel { get; set; }
//         [XmlElement(ElementName = "fieldentry")]
//         public required string Fieldentry { get; set; }
//     }

//     [XmlRoot(ElementName = "qtimetadata")]
//     public class Qtimetadata
//     {
//         [XmlElement(ElementName = "qti_metadatafield")]
//         public required List<Qti_metadatafield> Qti_metadatafield { get; set; }
//     }

//     [XmlRoot(ElementName = "itemmetadata")]
//     public class Itemmetadata
//     {
//         [XmlElement(ElementName = "qtimetadata")]
//         public required Qtimetadata Qtimetadata { get; set; }
//     }

//     [XmlRoot(ElementName = "file", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//     public class File
//     {
//         [XmlAttribute(AttributeName = "href")]
//         public required string Href { get; set; }
//     }

//     [XmlRoot(ElementName = "matimage")]
//     public class Matimage
//     {
//         [XmlAttribute(AttributeName = "is_hidden", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Is_hidden { get; set; }
//         [XmlAttribute(AttributeName = "uri")]
//         public required string Uri { get; set; }
//         [XmlText]
//         public required string Text { get; set; }
//     }


//     [XmlRoot(ElementName = "sectionproc_extension")]
//     public class Sectionproc_extension
//     {
//         [XmlElement(ElementName = "display_section_name", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Display_section_name { get; set; }
//         [XmlElement(ElementName = "display_section_line", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Display_section_line { get; set; }
//         [XmlElement(ElementName = "type_display_section", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Type_display_section { get; set; }
//     }

//     [XmlRoot(ElementName = "itemproc_extension")]
//     public class Itemproc_extension
//     {
//         [XmlElement(ElementName = "difficulty", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Difficulty { get; set; }
//         [XmlElement(ElementName = "isbonus", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Isbonus { get; set; }
//         [XmlElement(ElementName = "ismandatory", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Ismandatory { get; set; }
//     }

//     [XmlRoot(ElementName = "response_extension")]
//     public class Response_extension
//     {
//         [XmlElement(ElementName = "display_style", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Display_style { get; set; }
//         [XmlElement(ElementName = "enumeration", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Enumeration { get; set; }
//         [XmlElement(ElementName = "grading_type", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Grading_type { get; set; }
//     }

//     [XmlRoot(ElementName = "response_label")]
//     public class Response_label
//     {
//         [XmlElement(ElementName = "flow_mat")]
//         public required Flow_mat Flow_mat { get; set; }
//         [XmlAttribute(AttributeName = "ident")]
//         public required string Ident { get; set; }
//     }

//     [XmlRoot(ElementName = "flow_label")]
//     public class Flow_label
//     {
//         [XmlElement(ElementName = "response_label")]
//         public required Response_label Response_label { get; set; }
//         [XmlAttribute(AttributeName = "class")]
//         public required string Class { get; set; }
//     }

//     [XmlRoot(ElementName = "render_choice")]
//     public class Render_choice
//     {
//         [XmlElement(ElementName = "flow_label")]
//         public required List<Flow_label> Flow_label { get; set; }
//         [XmlAttribute(AttributeName = "shuffle")]
//         public required string Shuffle { get; set; }
//     }

//     [XmlRoot(ElementName = "response_lid")]
//     public class Response_lid
//     {
//         [XmlElement(ElementName = "render_choice")]
//         public required Render_choice Render_choice { get; set; }
//         [XmlAttribute(AttributeName = "ident")]
//         public required string Ident { get; set; }
//         [XmlAttribute(AttributeName = "rcardinality")]
//         public required string Rcardinality { get; set; }
//     }

//     [XmlRoot(ElementName = "flow")]
//     public class Flow
//     {
//         [XmlElement(ElementName = "material")]
//         public required List<Material> Material { get; set; }
//         [XmlElement(ElementName = "response_extension")]
//         public required Response_extension Response_extension { get; set; }
//         [XmlElement(ElementName = "response_lid")]
//         public required Response_lid Response_lid { get; set; }
//         [XmlElement(ElementName = "response_str")]
//         public required List<Response_str> Response_str { get; set; }
//     }

//     [XmlRoot(ElementName = "presentation")]
//     public class Presentation
//     {
//         [XmlElement(ElementName = "flow")]
//         public required Flow Flow { get; set; }
//     }

//     [XmlRoot(ElementName = "varequal")]
//     public class Varequal
//     {
//         [XmlAttribute(AttributeName = "respident")]
//         public required string Respident { get; set; }
//         [XmlText]
//         public required string Text { get; set; }
//         [XmlAttribute(AttributeName = "case")]
//         public required string Case { get; set; }
//     }

//     [XmlRoot(ElementName = "conditionvar")]
//     public class Conditionvar
//     {
//         [XmlElement(ElementName = "varequal")]
//         public required List<Varequal> Varequal { get; set; }
//         [XmlElement(ElementName = "not")]
//         public required Not Not { get; set; }
//         [XmlElement(ElementName = "and")]
//         public required And And { get; set; }
//     }

//     [XmlRoot(ElementName = "setvar")]
//     public class Setvar
//     {
//         [XmlAttribute(AttributeName = "action")]
//         public required string Action { get; set; }
//         [XmlText]
//         public required string Text { get; set; }
//     }

//     [XmlRoot(ElementName = "displayfeedback")]
//     public class Displayfeedback
//     {
//         [XmlAttribute(AttributeName = "feedbacktype")]
//         public required string Feedbacktype { get; set; }
//         [XmlAttribute(AttributeName = "linkrefid")]
//         public required string Linkrefid { get; set; }
//     }

//     [XmlRoot(ElementName = "respcondition")]
//     public class Respcondition
//     {
//         [XmlElement(ElementName = "conditionvar")]
//         public required Conditionvar Conditionvar { get; set; }
//         [XmlElement(ElementName = "setvar")]
//         public required Setvar Setvar { get; set; }
//         [XmlElement(ElementName = "displayfeedback")]
//         public required Displayfeedback Displayfeedback { get; set; }
//         [XmlAttribute(AttributeName = "title")]
//         public required string Title { get; set; }
//         [XmlAttribute(AttributeName = "continue")]
//         public required string Continue { get; set; }
//     }

//     [XmlRoot(ElementName = "resprocessing")]
//     public class Resprocessing
//     {
//         [XmlElement(ElementName = "respcondition")]
//         public required List<Respcondition> Respcondition { get; set; }
//         [XmlElement(ElementName = "outcomes")]
//         public required Outcomes Outcomes { get; set; }
//     }

//     [XmlRoot(ElementName = "itemfeedback")]
//     public class Itemfeedback
//     {
//         [XmlElement(ElementName = "material")]
//         public required Material Material { get; set; }
//         [XmlAttribute(AttributeName = "ident")]
//         public required string Ident { get; set; }
//     }

//     [XmlRoot(ElementName = "itemref")]
//     public class Itemref
//     {
//         [XmlElement(ElementName = "file", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required File File { get; set; }
//         [XmlElement(ElementName = "points", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Points { get; set; }
//         [XmlElement(ElementName = "difficulty", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Difficulty { get; set; }
//         [XmlElement(ElementName = "isbonus", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Isbonus { get; set; }
//         [XmlElement(ElementName = "ismandatory", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Ismandatory { get; set; }
//         [XmlAttribute(AttributeName = "linkrefid")]
//         public required string Linkrefid { get; set; }
//         [XmlAttribute(AttributeName = "page", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Page { get; set; }
//     }

//     [XmlRoot(ElementName = "item")]
//     public class QuizItem
//     {
//         [XmlElement(ElementName = "itemmetadata")]
//         public required Itemmetadata Itemmetadata { get; set; }
//         [XmlElement(ElementName = "itemproc_extension")]
//         public required Itemproc_extension Itemproc_extension { get; set; }
//         [XmlElement(ElementName = "presentation")]
//         public required Presentation Presentation { get; set; }
//         [XmlElement(ElementName = "resprocessing")]
//         public required Resprocessing Resprocessing { get; set; }
//         [XmlElement(ElementName = "itemfeedback")]
//         public required List<Itemfeedback> Itemfeedback { get; set; }
//         [XmlAttribute(AttributeName = "ident")]
//         public required string Ident { get; set; }
//         [XmlAttribute(AttributeName = "label")]
//         public required string Label { get; set; }
//         [XmlAttribute(AttributeName = "page", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Page { get; set; }
//         [XmlAttribute(AttributeName = "title")]
//         public required string Title { get; set; }
//         [XmlElement(ElementName = "hint")]
//         public required Hint Hint { get; set; }
//     }

//     [XmlRoot(ElementName = "decvar")]
//     public class Decvar
//     {
//         [XmlAttribute(AttributeName = "vartype")]
//         public required string Vartype { get; set; }
//         [XmlAttribute(AttributeName = "defaultval")]
//         public required string Defaultval { get; set; }
//         [XmlAttribute(AttributeName = "minvalue")]
//         public required string Minvalue { get; set; }
//         [XmlAttribute(AttributeName = "maxvalue")]
//         public required string Maxvalue { get; set; }
//         [XmlAttribute(AttributeName = "varname")]
//         public required string Varname { get; set; }
//     }

//     [XmlRoot(ElementName = "outcomes")]
//     public class Outcomes
//     {
//         [XmlElement(ElementName = "decvar")]
//         public required List<Decvar> Decvar { get; set; }
//     }


//     [XmlRoot(ElementName = "not")]
//     public class Not
//     {
//         [XmlElement(ElementName = "and")]
//         public required And AndConditional { get; set; }

//         [XmlElement(ElementName = "not")]
//         public required Not NotConditional { get; set; }

//         [XmlElement(ElementName = "varequal")]
//         public required List<Varequal> Varequal { get; set; }
//     }

//     [XmlRoot(ElementName = "and")]
//     public class And
//     {
//         [XmlElement(ElementName = "and")]
//         public required And AndConditional { get; set; }

//         [XmlElement(ElementName = "not")]
//         public required Not NotConditional { get; set; }

//         [XmlElement(ElementName = "varequal")]
//         public required List<Varequal> Varequal { get; set; }
//     }

//     [XmlRoot(ElementName = "render_fib")]
//     public class Render_fib
//     {
//         [XmlElement(ElementName = "response_label")]
//         public required Response_label Response_label { get; set; }
//         [XmlAttribute(AttributeName = "rows")]
//         public required string Rows { get; set; }
//         [XmlAttribute(AttributeName = "columns")]
//         public required string Columns { get; set; }
//         [XmlAttribute(AttributeName = "prompt")]
//         public required string Prompt { get; set; }
//         [XmlAttribute(AttributeName = "fibtype")]
//         public required string Fibtype { get; set; }
//     }

//     [XmlRoot(ElementName = "response_str")]
//     public class Response_str
//     {
//         [XmlElement(ElementName = "render_fib")]
//         public required Render_fib Render_fib { get; set; }
//         [XmlAttribute(AttributeName = "ident")]
//         public required string Ident { get; set; }
//         [XmlAttribute(AttributeName = "rcardinality")]
//         public required string Rcardinality { get; set; }
//     }

//     [XmlRoot(ElementName = "hintmaterial")]
//     public class Hintmaterial
//     {
//         [XmlElement(ElementName = "flow_mat")]
//         public required Flow_mat Flow_mat { get; set; }
//     }

//     [XmlRoot(ElementName = "hint")]
//     public class Hint
//     {
//         [XmlElement(ElementName = "hintmaterial")]
//         public required Hintmaterial Hintmaterial { get; set; }
//     }

//     [XmlRoot(ElementName = "section")]
//     public class Section
//     {
//         [XmlElement(ElementName = "qtimetadata")]
//         public required Qtimetadata Qtimetadata { get; set; }
//         [XmlElement(ElementName = "sectionproc_extension")]
//         public required Sectionproc_extension Sectionproc_extension { get; set; }
//         [XmlElement(ElementName = "itemref")]
//         public required List<Itemref> Itemref { get; set; }
//         [XmlAttribute(AttributeName = "title")]
//         public required string Title { get; set; }
//         [XmlAttribute(AttributeName = "page", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Page { get; set; }
//         [XmlAttribute(AttributeName = "ident")]
//         public required string Ident { get; set; }
//         [XmlElement(ElementName = "item")]
//         public required QuizItem Item { get; set; }
//     }

//     [XmlRoot(ElementName = "assessment")]
//     public class Assessment
//     {
//         [XmlElement(ElementName = "rubric")]
//         public required Rubric Rubric { get; set; }
//         [XmlElement(ElementName = "assessmentcontrol")]
//         public required Assessmentcontrol Assessmentcontrol { get; set; }
//         [XmlElement(ElementName = "presentation_material")]
//         public required Presentation_material Presentation_material { get; set; }
//         [XmlElement(ElementName = "assess_procextension")]
//         public required Assess_procextension Assess_procextension { get; set; }
//         [XmlElement(ElementName = "assessfeedback")]
//         public required List<Assessfeedback> Assessfeedback { get; set; }
//         [XmlElement(ElementName = "section")]
//         public required Section Section { get; set; }
//         [XmlAttribute(AttributeName = "id", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Id { get; set; }
//         [XmlAttribute(AttributeName = "title")]
//         public required string Title { get; set; }
//         [XmlAttribute(AttributeName = "ident")]
//         public required string Ident { get; set; }
//         [XmlAttribute(AttributeName = "resource_code", Namespace = "http://desire2learn.com/xsd/d2lcp_v2p0")]
//         public required string Resource_code { get; set; }
//     }

//     [XmlRoot(ElementName = "questestinterop")]
//     public class Questestinterop
//     {
//         [XmlElement(ElementName = "assessment")]
//         public required Assessment Assessment { get; set; }
//         [XmlAttribute(AttributeName = "d2l_2p0", Namespace = "http://www.w3.org/2000/xmlns/")]
//         public required string D2l_2p0 { get; set; }

//         public required string FileName { get; set; }
//     }

// }
