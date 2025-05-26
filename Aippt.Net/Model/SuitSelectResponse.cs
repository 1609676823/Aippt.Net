using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Aippt.Net.Model
{
    /// <summary>
    /// 套装选择响应类，包含返回码、数据和提示信息。
    /// Suit selection response class, containing return code, data, and prompt information.
    /// </summary>
    public class SuitSelectResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常。
        /// Return code, 0 indicates success, non-zero indicates an exception.
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含套装选择相关数据的对象。
        /// Object containing suit selection-related data.
        /// </summary>
        public SuitSelectData? data { get; set; } = new SuitSelectData();

        /// <summary>
        /// 返回提示信息。
        /// Return prompt information.
        /// </summary>
        public string? msg { get; set; }

        [JsonIgnore]
        private string realJsonstring = string.Empty;

        /// <summary>
        /// Gets or sets the real JSON string representation of the response.
        /// 获取或设置响应的真实 JSON 字符串表示。
        /// </summary>
        [JsonIgnore]
        public string? RealJsonstring
        {
            get { return realJsonstring; }
            set
            {
                realJsonstring = value!;
                try
                {
                    DeserializeSuitSelectResponse(value!);
                }
                catch (Exception)
                {
                    // Log or handle the exception as needed
                }
            }
        }

        /// <summary>
        /// 反序列化 JSON 字符串到当前的实例。
        /// 该方法会解析 JSON 中的每个属性，并将值赋给当前实例的对应属性。
        /// 如果在解析过程中出现异常，会捕获异常并继续解析后续属性。
        /// Deserialize a JSON string into the current instance.
        /// This method parses each property in the JSON and assigns the values to the corresponding properties of the current instance.
        /// If an exception occurs during the parsing process, it catches the exception and continues to parse the subsequent properties.
        /// </summary>
        /// <param name="json">需要解析的 JSON 字符串。The JSON string to be parsed.</param>
        public virtual void DeserializeSuitSelectResponse(string json)
        {
            var jsonNode = JsonNode.Parse(json);
            if (jsonNode != null)
            {
                try
                {
                    var codeNode = jsonNode["code"];
                    if (codeNode != null)
                    {
                        this.code = codeNode.GetValue<long>();
                    }
                }
                catch { }

                try
                {
                    var msgNode = jsonNode["msg"];
                    if (msgNode != null)
                    {
                        this.msg = msgNode.GetValue<string>();
                    }
                }
                catch { }

                var dataNode = jsonNode["data"];
                if (dataNode != null)
                {
                    this.data = new SuitSelectData();

                    try
                    {
                        var colourArray = dataNode["colour"]?.AsArray();
                        if (colourArray != null)
                        {
                            foreach (var item in colourArray)
                            {
                                if (item != null)
                                {
                                    this.data.colour.Add(new Colour
                                    {
                                        id = item["id"]?.GetValue<long>() ?? 0,
                                        name = item["name"]?.GetValue<string>(),
                                        // Removed en_name as it's not in the provided parameter list
                                        code = item["code"]?.GetValue<string>(),
                                        // Removed is_hot as it's not in the provided parameter list
                                    });
                                }
                            }
                        }
                    }
                    catch { }

                    try
                    {
                        var suitStyleArray = dataNode["suit_style"]?.AsArray();
                        if (suitStyleArray != null)
                        {
                            foreach (var item in suitStyleArray)
                            {
                                if (item != null)
                                {
                                    this.data.suit_style.Add(new SuitStyle
                                    {
                                        id = item["id"]?.GetValue<long>() ?? 0,
                                        title = item["title"]?.GetValue<string>(),
                                        // Removed is_hot as it's not in the provided parameter list
                                    });
                                }
                            }
                        }
                    }
                    catch { }

                    try
                    {
                        var suitSceneArray = dataNode["suit_scene"]?.AsArray();
                        if (suitSceneArray != null)
                        {
                            foreach (var item in suitSceneArray)
                            {
                                if (item != null)
                                {
                                    this.data.suit_scene.Add(new SuitScene
                                    {
                                        id = item["id"]?.GetValue<long>() ?? 0,
                                        title = item["title"]?.GetValue<string>(),
                                        // Removed is_hot as it's not in the provided parameter list
                                    });
                                }
                            }
                        }
                    }
                    catch { }

                    // Removed career parsing as it's not in the provided parameter list
                }
            }
        }
    }

    /// <summary>
    /// 套装选择数据类。
    /// Suit selection data class.
    /// </summary>
    public class SuitSelectData
    {
        /// <summary>
        /// 颜色列表。
        /// List of colors.
        /// </summary>
        public List<Colour> colour { get; set; } = new List<Colour>();

        /// <summary>
        /// 套装风格列表。
        /// List of suit styles.
        /// </summary>
        public List<SuitStyle> suit_style { get; set; } = new List<SuitStyle>();

        /// <summary>
        /// 套装场景列表。
        /// List of suit scenes.
        /// </summary>
        public List<SuitScene> suit_scene { get; set; } = new List<SuitScene>();

        // Removed career property as it's not in the provided parameter list
    }

    /// <summary>
    /// 颜色信息。
    /// Colour information.
    /// </summary>
    public class Colour
    {
        /// <summary>
        /// 颜色ID。
        /// Colour ID.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 颜色名称。
        /// Colour name.
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 颜色值。
        /// Colour code.
        /// </summary>
        public string? code { get; set; }

        // Removed en_name and is_hot properties as they're not in the provided parameter list
    }

    /// <summary>
    /// 套装风格信息。
    /// Suit style information.
    /// </summary>
    public class SuitStyle
    {
        /// <summary>
        /// 风格ID。
        /// Style ID.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 风格名称。
        /// Style name.
        /// </summary>
        public string? title { get; set; }

        // Removed is_hot property as it's not in the provided parameter list
    }

    /// <summary>
    /// 套装场景信息。
    /// Suit scene information.
    /// </summary>
    public class SuitScene
    {
        /// <summary>
        /// 场景ID。
        /// Scene ID.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 场景名称。
        /// Scene name.
        /// </summary>
        public string? title { get; set; }

        // Removed is_hot property as it's not in the provided parameter list
    }

    // Removed Career class as it's not in the provided parameter list
}
