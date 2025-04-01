using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace D2LOffice.Models.D2L
{
    public class RichTextInput
    {

        public required string Content { get; set; } = "";

        /// <summary>
        /// Values are "Text" or "Html"
        /// </summary>
        public required string Type { get; set; } = "Text";
    }
}
